using ECommons.Automation;
using FFXIVClientStructs.FFXIV.Component.GUI;
using Serilog;

using System.Linq;

namespace SamplePlugin.Tasks
{
    public class FireCallback(string AddonName,bool kapkac, params int[] gibeme) : IBaseTask
    {
        public unsafe bool? Run()
        {
            if (TryGetAddonByName<AtkUnitBase>(AddonName, out var addon) && IsAddonReady(addon))
            {
                Callback.Fire(addon,kapkac, gibeme.Cast<object>().ToArray());
                return true;
            }
            Log.Information("NO ADDON!");
            return false;
        }
    }
}
