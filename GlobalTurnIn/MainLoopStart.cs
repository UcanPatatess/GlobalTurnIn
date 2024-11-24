using ECommons.DalamudServices;
using GlobalTurnIn.TaskAuto;
using System.Data;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalTurnIn;

public sealed class MainLoopStart : AutoCommon
{
    protected override async Task Execute()
    {
        Status = "testing";
        //await TargetInteract();
        //await TargetName("Maisenta");
        //await TeleportTo(129, new Vector3(-75, 18, 1)); //works limsa
        //await MountUp();
        await OpenShopMenu(0,0,"Shop");
        //await AethernetSwitch("aftcastle",128); //works
        //await MoveTo(new Vector3(45f,20f,2f),1); //works
        //await TargetAndInteract("Bango Zango"); //not async but works
        //await FireCallback("Shop",true,0,0,1); //not async but works
        /*
        if (!IsThereTradeItem())
            return;
        if (TotalExchangeItem > 0)
        {

        }
        else
        {
            if ((GordianTurnInCount > 0 || AlexandrianTurnInCount > 0) && GetInventoryFreeSlotCount() != 0)
            {
                await TeleportTo(478, new Vector3());// idlyshire
                await MountUp();
                await MoveTo(new Vector3(-19.0f, 211.0f, -35.9f),2);

                await TargetAndInteract("");
            }
            else if (DeltascapeTurnInCount > 0) 
            {
                await TeleportTo(635, new Vector3());// Rhalgr
            }
        }*/
    }
    private async Task TurnIn(int MaxArmoryValue) 
    {
        //ItemIdArmoryTable.TryGetValue(itemId, out int category);
        if (PluginInstalled("Automaton")) 
        {
            Svc.Commands.ProcessCommand("/inventory");
            await NextFrame(10);
            Svc.Commands.ProcessCommand("/inventory");
            await NextFrame(10);
            Svc.Commands.ProcessCommand("/inventory");
        }
        string NpcName = string.Empty;
        int? lastShopType = null;
        int? LastIconShopType = null;

        if (Svc.ClientState.TerritoryType == 478)
        {
            NpcName = "Sabina";
        }
        if (Svc.ClientState.TerritoryType == 635)
        {
            NpcName = "Gelfradus";
        }

        while (true) { }
    }
}
