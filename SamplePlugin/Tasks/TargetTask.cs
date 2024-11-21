using ECommons.GameFunctions;
using ECommons.Throttlers;
using SamplePlugin.Tasks.Base;
using SamplePlugin.Util;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using System.Linq;
using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons;
using ECommons.DalamudServices;
using static FFXIVClientStructs.FFXIV.Client.UI.AddonRelicNoteBook;
using Dalamud.Game.ClientState.Objects.Types;
using System.Numerics;

namespace SamplePlugin.Tasks;

public class TargetTask(string targetName) : IBaseTask
{
    public unsafe bool? Run()
    {
        var target = Svc.Objects.FirstOrDefault(o => o.IsTargetable && o.Name.TextValue.Equals(targetName, System.StringComparison.InvariantCultureIgnoreCase));
        if (target != default)
        {
            TargetSystem.Instance()->Target = (GameObject*)target.Address; //not working gonna look into it tomorrow
            return true;
        }
        return false;
    }
}
