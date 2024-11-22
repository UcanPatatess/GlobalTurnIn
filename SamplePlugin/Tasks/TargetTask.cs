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
using ECommons.Logging;
using System.Xml.Linq;

namespace SamplePlugin.Tasks;

public class TargetTask(string targetName) : IBaseTask
{
    public unsafe bool? Run()
    {
        var target = GetObjectByName(targetName);
        if (target != null)
        {
            PluginLog.Information("Targeted: " + targetName);
            Svc.Targets.Target = target;
            return true;
        }
        return false;
    }
}
