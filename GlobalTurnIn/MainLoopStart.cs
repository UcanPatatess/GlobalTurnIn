using ECommons.Automation;
using ECommons.DalamudServices;
using GlobalTurnIn.TaskAuto;
using Lumina.Excel.Sheets;
using System.Data;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalTurnIn;

public sealed class MainLoopStart : AutoCommon
{
    protected override async Task Execute()
    {
        Status = "Changing To Correct Settings";
        await ChangeArmorySetting(Configuration.MaxArmory);
        if (!IsThereTradeItem())
            return;
        while (IsThereTradeItem())
        {
            if ((GordianTurnInCount > 0 || AlexandrianTurnInCount > 0) && GetInventoryFreeSlotCount() != 0)
            {
                Status = "Teleporting To idlyshire";
                await TeleportTo(478, new Vector3());// idlyshire
                if (GetDistanceToPoint(-19.0f, 211.0f, -35.9f) > 3)
                {
                    Status = "Mounting Up";
                    await MountUp();
                }

                Status = "Moving to Exchange NPC";
                await MoveTo(new Vector3(-19.0f, 211.0f, -35.9f), 2);
                Status = "TrunIn";
                await TurnIn();
                if (TotalExchangeItem > 0)
                {
                    if (Configuration.VendorTurnIn)
                    {
                        if (GetDistanceToPoint(34.0f, 208.0f, -51.9f) > 3)
                        {
                            Status = "Mounting Up";
                            await MountUp();
                        }
                        Status = "Moving to Sell Vendor";
                        await MoveTo(new Vector3(34.0f, 208.0f, -51.9f), 2);
                        Status = "Selling";
                        await SellVendor();
                    }
                }
            }
            else if (DeltascapeTurnInCount > 0)
            {
                Status = "Teleporting To Rhalgr";
                await TeleportTo(635, new Vector3());// Rhalgr
                if (GetDistanceToPoint(125.0f, 0.7f, 40.8f) > 3)
                {
                    Status = "Mounting Up";
                    await MountUp();
                }
                Status = "Moving to Exchange NPC";
                await MoveTo(new Vector3(125.0f, 0.7f, 40.8f), 2);
                Status = "TrunIn";
                await TurnIn();
            }
        }
    }
    private async Task TurnIn() 
    {
        if (CanIBuy())
        {
            if (PluginInstalled("Automaton"))
            {
                Chat.Instance.SendMessage("/inventory");
                await NextFrame(10);
                Chat.Instance.SendMessage("/inventory");
                await NextFrame(10);
                Chat.Instance.SendMessage("/inventory");
            }
            await NextFrame(10);
            await TargetName();
            await TargetInteract();

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
                {
                    ArmoryType = category;
                }
                int SlotArmoryINV = GetFreeSlotsInContainer(ArmoryType);

                if (CanExchange > 0 && GearAmount == 0 && SlotINV > 0) // >o< looks like a emoji lol
                {
                    if (shopType != lastShopType)
                    {
                        CloseShop();
                        await OpenShopMenu(iconShopType, shopType);
                        lastShopType = shopType;
                    }
                    if (SlotArmoryINV != 0 && Configuration.MaxArmory)
                    {
                        await Exchange(GearAmount, gearItem, pcallValue, SlotArmoryINV);
                        if (LastIconShopType != null && iconShopType != LastIconShopType)
                        {
                            CloseShop();
                            await WaitWhile(() => !IsAddonActive("ShopExchangeItem"), "In Exchange Shop Change");
                        }
                        continue;
                    }
                    if (Configuration.MaxItem)
                    {
                        if (CanExchange < SlotINV)
                            await Exchange(GearAmount, gearItem, pcallValue, CanExchange);
                        else
                            await Exchange(GearAmount, gearItem, pcallValue, SlotINV);
                    }
                    else
                        await Exchange(GearAmount, gearItem, pcallValue, 1);

                    if (LastIconShopType != null && iconShopType != LastIconShopType)
                    {
                        CloseShop();
                        await WaitWhile(() => !IsAddonActive("ShopExchangeItem"), "In Exchange Shop Change");
                    }
                }
            }
            CloseShop();
            await CloseSelectString();
        }
    }
}
