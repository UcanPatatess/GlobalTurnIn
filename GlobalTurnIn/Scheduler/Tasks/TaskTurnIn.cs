using ECommons.Automation;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using ECommons.GameFunctions;
using ECommons.Throttlers;
using GlobalTurnIn.Scheduler.Handlers;
using ECommons.Automation.NeoTaskManager;
using Serilog;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskTurnIn
    {
        
        internal static void Enqueue()
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

                if (CanExchange > 0 && GearAmount == 0 && SlotINV > 0) // >o< looks like a emoji lol 
                {
                    if (shopType != lastShopType)
                    {
                        P.taskManager.Enqueue(CloseShop);
                        OpenShopMenu(iconShopType, shopType);
                        lastShopType = shopType;
                    }
                    if (SlotArmoryINV != 0 && C.MaxArmory)
                    {
                        Exchange(GearAmount, gearItem, pcallValue, SlotArmoryINV);
                        if (LastIconShopType != null && iconShopType != LastIconShopType)
                        {
                            P.taskManager.Enqueue(CloseShop);
                        }
                        continue;
                    }
                    if (C.MaxItem)
                    {
                        if (CanExchange < SlotINV)
                            Exchange(GearAmount, gearItem, pcallValue, CanExchange);
                        else
                            Exchange(GearAmount, gearItem, pcallValue, SlotINV);
                    }
                    else
                        Exchange(GearAmount, gearItem, pcallValue, 1);

                    if (LastIconShopType != null && iconShopType != LastIconShopType)
                    {
                        P.taskManager.Enqueue(CloseShop);
                    }
                }
            }
            P.taskManager.Enqueue(CloseShop);

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
        internal unsafe static bool? AddonCallSelectIconString(int SelectIconString)
        {
            Log.Debug($"AddonCallSelectIconString: {SelectIconString}");
            if (EzThrottler.Throttle("AddonCallSelectIconString", 20))
                GenericHandlers.FireCallback("SelectIconString", true, SelectIconString);

            if (!IsAddonActive("SelectIconString"))
                return true;
            
            return false;
        }
        internal unsafe static bool? AddonCallSelectString(int SelectString)
        {
            Log.Debug($"AddonCallSelectString: {SelectString}");
            if (EzThrottler.Throttle("AddonCallSelectString", 20))
                GenericHandlers.FireCallback("SelectString", true, SelectString);

            if (!IsAddonActive("SelectString"))
                return true;

            return false;
        }
        internal static bool? OpenShopMenu(int SelectIconString, int SelectString)
        {
            Log.Debug("OpenShopMenu"+" "+SelectIconString+" "+ SelectString);
            P.taskManager.EnqueueDelay(100);
            P.taskManager.Enqueue(TargetNpc);
            P.taskManager.Enqueue(TargetInteract);
            P.taskManager.Enqueue(() => AddonCallSelectIconString(SelectIconString));
            P.taskManager.Enqueue(() => AddonCallSelectString(SelectString));
            
            if (IsAddonActive("ShopExchangeItem"))
                return true;

            return false;
        }
        internal static bool? Exchange(int currentgearamount, int gearid, int List, int Amount)
        {
            Log.Debug($"Exchange currentgearamount: {currentgearamount} gearid: {gearid} List: {List} Amount: {Amount}");
            if (!IsAddonActive("ShopExchangeItem"))
            {
                return false;
            }
            if (Amount > 127)
                Amount = 127;


            if (EzThrottler.Throttle("AddonCallSelectString", 20))
                P.taskManager.Enqueue(() => GenericHandlers.FireCallback("ShopExchangeItem", true, 0, List, Amount));
            if (EzThrottler.Throttle("AddonCallSelectString", 20))
                P.taskManager.Enqueue(() => IsAddonActive("ShopExchangeItemDialog"));
            if (EzThrottler.Throttle("AddonCallSelectString", 20))
                P.taskManager.Enqueue(() => GenericHandlers.FireCallback("ShopExchangeItemDialog", true, 0));

            if (IsAddonActive("SelectYesno"))
            {
                if (EzThrottler.Throttle("AddonCallSelectString", 20))
                    P.taskManager.Enqueue(() => GenericHandlers.FireCallback("SelectYesno", true, 0));
                P.taskManager.Enqueue(() => !IsAddonActive("SelectYesno"));
            }
            if (DidAmountChange(currentgearamount, GetItemCount(gearid)))
            {
                return true;
            }

            return false;
        }
    }
}
