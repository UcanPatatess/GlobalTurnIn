using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ECommons.Automation.LegacyTaskManager;
using SamplePlugin.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin
{
    internal class Service
    {
        internal static Plugin Plugin { get; set; } = null!;
        internal static Configuration Configuration { get; set; } = null!;
        internal static LoopingService Example { get; set; } =null!;
        public static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    }
}
