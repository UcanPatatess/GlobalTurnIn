using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace SamplePlugin.Tasks;

public class MountTask : IBaseTask // Example to show how a condition would be used
{
    public unsafe bool? Run()
    {
        if (Svc.Condition[ConditionFlag.Mounted] && PlayerNotBusy()) return true;
        if (!Svc.Condition[ConditionFlag.Casting] && !Svc.Condition[ConditionFlag.Unknown57])
        {
            ActionManager.Instance()->UseAction(ActionType.GeneralAction, 24);
        }

        return false;
    }
}
