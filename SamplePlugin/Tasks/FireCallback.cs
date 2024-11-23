using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.Logging;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Component.GUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
