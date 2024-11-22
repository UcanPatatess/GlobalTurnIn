using ECommons.Throttlers;
using SamplePlugin.Tasks.Base;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using ECommons.DalamudServices;
using ECommons.GameFunctions;

namespace SamplePlugin.Tasks;

public class InteractTask() : IBaseTask
{
    public unsafe bool? Run()
    {
        var target = Svc.Targets.Target;

        if (target != default)
        {
            unsafe { TargetSystem.Instance()->InteractWithObject(target.Struct(), false); }
            return true;
        }
        return false;
    }
}
