using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using GlobalTurnIn.Scheduler.Handlers;
using Serilog;
using static FFXIVClientStructs.FFXIV.Component.GUI.AtkComponentTreeList.Delegates;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskTurnIn
    {
        internal unsafe static void Enqueue()
        {
            if (PluginInstalled("Automaton"))
            {
                P.taskManager.Enqueue(() => Chat.Instance.SendMessage("/inventory"));
                P.taskManager.EnqueueDelay(100);
                P.taskManager.Enqueue(() => Chat.Instance.SendMessage("/inventory"));
                P.taskManager.EnqueueDelay(100);
            }
            else
                P.taskManager.Enqueue(() => Svc.Chat.PrintError("Open AutoMerge in Automaton"));

        int? lastShopType = null;
        int? LastIconShopType = null;


        int[,] TableName = null!;
            if (Svc.ClientState.TerritoryType == 478)
                TableName = SabinaTable;
            if (Svc.ClientState.TerritoryType == 635)
                TableName = GelfradusTable;

            int SlotINV = GetInventoryFreeSlotCount();
            bool isItemPurchasedFromArmory = false;
            int lastArmoryType = -1;
            int armoryExchaneAmount =0;

            UpdateDict();

            for (int i = 0; i < TableName.GetLength(0); i++)
            {
                int shopType = TableName[i, 0];
                int itemType = TableName[i, 1];
                int itemTypeBuy = TableName[i, 2];
                int gearItem = TableName[i, 3];
                int pcallValue = TableName[i, 4];
                int iconShopType = TableName[i, 5];

                int ItemAmount = VendorSellDict[itemType].CurrentItemCount;
                int GearAmount = GetItemCount(gearItem);
                int CanExchange = (int)Math.Floor((double)ItemAmount / itemTypeBuy);
                
                int ArmoryType = 0;
                if (ItemIdArmoryTable.TryGetValue(gearItem, out int category))
                    ArmoryType = category;

                int SlotArmoryINV = GetFreeSlotsInContainer(ArmoryType);

                if (ArmoryType != lastArmoryType)
                {
                    isItemPurchasedFromArmory = false; // Reset the flag for the new armory
                    lastArmoryType = ArmoryType; // Update the last armory type
                    armoryExchaneAmount += 1;
                }
                if (isItemPurchasedFromArmory || armoryExchaneAmount > 8)
                {
                    SlotArmoryINV = 0; // Don't consider armory slots if we've already purchased
                }
                if (CanExchange > 0 && GearAmount == 0 && (SlotINV > 0 || (SlotArmoryINV > 0 && C.MaxArmory))) // >o< looks like a emoji lol 
                {
                    if (shopType != lastShopType)
                    {
                        P.taskManager.Enqueue(CloseShop);
                        OpenShopMenu(iconShopType, shopType);
                        lastShopType = shopType;
                    }
                    if (SlotArmoryINV != 0 && C.MaxArmory)
                    {
                        Exchange(gearItem, pcallValue, SlotArmoryINV);
                        VendorSellDict[itemType].CurrentItemCount = ItemAmount- SlotArmoryINV;
                        isItemPurchasedFromArmory = true;

                        if (LastIconShopType != null && iconShopType != LastIconShopType)
                        {
                            P.taskManager.Enqueue(CloseShop);
                        }
                        continue;
                    }
                    if (C.MaxItem)
                    {
                        if (CanExchange < SlotINV)
                        {
                            Exchange(gearItem, pcallValue, CanExchange);
                            P.taskManager.Enqueue(() => VendorSellDict[itemType].CurrentItemCount = ItemAmount - CanExchange);
                            SlotINV -= CanExchange;
                        }
                        else
                        {
                            Exchange(gearItem, pcallValue, SlotINV);
                            P.taskManager.Enqueue(() => VendorSellDict[itemType].CurrentItemCount = ItemAmount - SlotINV);
                            SlotINV -= 127;
                        }
                    }
                    else
                    {
                        Exchange(gearItem, pcallValue, 1);
                        P.taskManager.Enqueue(() => VendorSellDict[itemType].CurrentItemCount = ItemAmount - 1);
                        SlotINV -= 1;
                    }
                    if (LastIconShopType != null && iconShopType != LastIconShopType)
                    {
                        P.taskManager.Enqueue(CloseShop);
                    }
                }
            }
            P.taskManager.Enqueue(CloseShop);
            P.taskManager.Enqueue(() => GenericHandlers.FireCallback("SelectString", true, -1));
        }
        internal static void TargetNpc()
        {
            string NpcName = string.Empty;
            if (Svc.ClientState.TerritoryType == 478) //Idyllshire
                NpcName = "Sabina";
            if (Svc.ClientState.TerritoryType == 635)//Rhalgr
                NpcName = "Gelfradus";
            Log.Debug("TargetNpc" + NpcName);

            var target = GetObjectByName(NpcName);
            if (EzThrottler.Throttle("TargetNpc", 20))
                Svc.Targets.Target = target;

        }

        internal unsafe static bool? TargetInteract()
        {
            Log.Debug("TargetInteract");
            var target = Svc.Targets.Target;
            if (target != null)
            {
                if (IsAddonActive("SelectString") || IsAddonActive("SelectIconString") || IsAddonActive("ShopExchangeItem"))
                    return true;

                if (EzThrottler.Throttle("TargetInteract", 100))
                    TargetSystem.Instance()->InteractWithObject(target.Struct(), false);


                return false;
            }
            return false;
        }
        internal static void OpenShopMenu(int SelectIconString, int SelectString)
        {
            Log.Debug("OpenShopMenu" + " " + SelectIconString + " " + SelectString);
            P.taskManager.EnqueueDelay(100);
            P.taskManager.Enqueue(TargetNpc);
            P.taskManager.EnqueueDelay(100);
            P.taskManager.Enqueue(TargetInteract);
            P.taskManager.Enqueue(() => GenericHandlers.FireCallback("SelectIconString", true, SelectIconString));
            P.taskManager.Enqueue(() => GenericHandlers.FireCallback("SelectString", true, SelectString));
            P.taskManager.Enqueue(() => IsAddonActive("ShopExchangeItem"));
        }
        internal static void Exchange(int gearItem, int List, int Amount)
        {
            int ArmoryType = 0;
            if (ItemIdArmoryTable.TryGetValue(gearItem, out int category))
                ArmoryType = category;
            Log.Debug($"Exchange  gearid: {gearItem} List: {List} Amount: {Amount}");
            if (Amount > 127)
                Amount = 127;

            P.taskManager.Enqueue(() => GenericHandlers.FireCallback("ShopExchangeItem", true, 0, List, Amount));
            P.taskManager.Enqueue(() => GenericHandlers.FireCallback("ShopExchangeItemDialog", true, 0));
            
            if (ArmoryType == 3207 || ArmoryType == 3208 || ArmoryType == 3209 || ArmoryType == 3300) { }
            else
                P.taskManager.Enqueue(() => GenericHandlers.FireCallback("SelectYesno", true, 0));
            P.taskManager.Enqueue(() => DidAmountChange(0, GetItemCount(gearItem)));
            P.taskManager.EnqueueDelay(100);
        }
    }
}
