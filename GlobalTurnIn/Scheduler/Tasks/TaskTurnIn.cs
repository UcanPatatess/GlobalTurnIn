using ECommons.Automation;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using ECommons.GameFunctions;
using ECommons.Throttlers;
using GlobalTurnIn.Scheduler.Handlers;
using Serilog;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskTurnIn
    {
        internal unsafe static void Enqueue()
        {
            if (PluginInstalled("Automaton"))
            {
                P.taskManager.Enqueue(()=> Chat.Instance.SendMessage("/inventory"));
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

            for (int i = 0; i < TableName.GetLength(0); i++)
            {
                int shopType = TableName[i, 0];
                int itemType = TableName[i, 1];
                int itemTypeBuy = TableName[i, 2];
                int gearItem = TableName[i, 3];
                int pcallValue = TableName[i, 4];
                int iconShopType = TableName[i, 5];

                int ItemAmount = GetItemCount(itemType);
                int GearAmount = GetItemCount(gearItem);
                int CanExchange = (int)Math.Floor((double)ItemAmount / itemTypeBuy);
                int SlotINV = GetInventoryFreeSlotCount();
                int ArmoryType = 0;
                if (ItemIdArmoryTable.TryGetValue(gearItem, out int category))
                    ArmoryType = category;

                int SlotArmoryINV = GetFreeSlotsInContainer(ArmoryType);

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

                        if (LastIconShopType != null && iconShopType != LastIconShopType)
                        {
                            P.taskManager.Enqueue(CloseShop);
                        }
                        continue;
                    }
                    if (C.MaxItem)
                    {
                        if (CanExchange < SlotINV)
                            Exchange(gearItem, pcallValue, CanExchange);
                        else
                            Exchange(gearItem, pcallValue, SlotINV);
                    }
                    else
                        Exchange(gearItem, pcallValue, 1);

                    if (LastIconShopType != null && iconShopType != LastIconShopType)
                    {
                        P.taskManager.Enqueue(CloseShop);
                    }
                }
            }
            P.taskManager.Enqueue(CloseShop);
            P.taskManager.Enqueue(() => GenericHandlers.FireCallback("SelectString",true,-1));
        }
        internal static bool? TargetNpc()
        {
            string NpcName = string.Empty;
            if (Svc.ClientState.TerritoryType == 478) //Idyllshire
                NpcName = "Sabina";
            if (Svc.ClientState.TerritoryType == 635)//Rhalgr
                NpcName = "Gelfradus";
            Log.Debug("TargetNpc" + NpcName);

            var target = GetObjectByName(NpcName);
            if (target != null)
            {
                if (EzThrottler.Throttle("TargetNpc", 20))
                    Svc.Targets.Target = target;
                return true;
            }
            return false;
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
            P.taskManager.Enqueue(TargetInteract);
            P.taskManager.Enqueue(() => GenericHandlers.FireCallback("SelectIconString",true,SelectIconString));
            P.taskManager.Enqueue(() => GenericHandlers.FireCallback("SelectString", true, SelectString));
            P.taskManager.Enqueue(() => IsAddonActive("ShopExchangeItem"));
        }
        internal static void Exchange( int gearid, int List, int Amount)
        {
            Log.Debug($"Exchange  gearid: {gearid} List: {List} Amount: {Amount}");
            if (Amount > 127)
                Amount = 127;


            P.taskManager.Enqueue(() => GenericHandlers.FireCallback("ShopExchangeItem", true, 0, List, Amount));
            P.taskManager.Enqueue(() => GenericHandlers.FireCallback("ShopExchangeItemDialog", true, 0));
            // biraz bekletmek lazım denenicek
            //P.taskManager.Enqueue(() => GenericHandlers.FireCallback("SelectYesno", true, 0));
            P.taskManager.Enqueue(() => DidAmountChange(0, GetItemCount(gearid)));
            //P.taskManager.EnqueueDelay(20);
        }
    }
}
