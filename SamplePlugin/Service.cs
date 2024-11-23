using Dalamud.Plugin;
using SamplePlugin.IPC;
using SamplePlugin.Managers;


namespace SamplePlugin
{
    internal class Service
    {
        internal static Plugin Plugin { get; set; } = null!;
        internal static Config Configuration { get; set; } = null!;
        internal static LoopingService Example { get; set; } =null!;
        internal static NavmeshIPC Navmesh { get; set; } = null!;
        internal static LifestreamIPC Lifestream { get; set; } = null!;
        internal static DeliverooIPC deliveroo { get; set; } = null!;
        internal static AutoRetainerIPC autoRetainer { get; set; } = null!; 
    }
}
