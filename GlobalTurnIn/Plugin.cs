using Dalamud.Plugin;
using ECommons;
using ECommons.Automation.NeoTaskManager;
using ECommons.Configuration;
using ECommons.DalamudServices;
using ECommons.SimpleGui;
using GlobalTurnIn;
using GlobalTurnIn.IPC;
using GlobalTurnIn.Scheduler;
using GlobalTurnIn.Windows;
using static FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Delegates;



namespace GlobalTurnIn;

public class Plugin : IDalamudPlugin
{
    private const string Command = "/globalt";
    private static string[] Aliases => ["/pgt", "/pglobal"];

    internal static Plugin P = null!;
    private readonly Config config;
    
    public static Config C => P.config;

    internal TaskManager taskManager;
    internal AutoRetainerIPC autoRetainer;
    internal DeliverooIPC deliveroo;
    internal LifestreamIPC lifestream;
    internal NavmeshIPC navmesh;

    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        P = this;
        ECommonsMain.Init(pluginInterface, P, ECommons.Module.DalamudReflector, ECommons.Module.ObjectFunctions);
        config = EzConfig.Init<Config>();

        
        EzConfigGui.Init(new MainWindow().Draw);
        EzConfigGui.WindowSystem.AddWindow(new SettingMenu());
        EzCmd.Add(Command, OnCommand, "Open Interface");
        Aliases.ToList().ForEach(a => EzCmd.Add(a, OnCommand, $"{Command} Alias"));

        taskManager = new();
        autoRetainer = new();
        deliveroo = new();
        lifestream = new();
        navmesh = new();
        Svc.Framework.Update += Tick;
    }
    private void Tick(object _)
    {
        if (SchedulerMain.AreWeTicking && Svc.ClientState.LocalPlayer != null)
        {
            SchedulerMain.Tick();
        }
    }
    public void Dispose()
    {
        Safe(() => Svc.Framework.Update -= Tick);
        ECommonsMain.Dispose();
    }
    private void OnCommand(string command, string args)
    {
        if (args.StartsWith("s"))
            EzConfigGui.WindowSystem.Windows.First(w => w is SettingMenu).IsOpen ^= true;
        else
            EzConfigGui.Window.IsOpen = !EzConfigGui.Window.IsOpen;
    }
}
