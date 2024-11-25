using Dalamud.Plugin;
using ECommons;
using ECommons.SimpleGui;
using GlobalTurnIn;
using GlobalTurnIn.IPC;
using GlobalTurnIn.Windows;



namespace SamplePlugin;

public sealed class Plugin : IDalamudPlugin
{
    private const string Command = "/globalt";
    private static string[] Aliases => ["/pgt", "/pglobal"];
    internal AutoRetainerIPC autoRetainer;
    internal DeliverooIPC deliveroo;
    internal LifestreamIPC lifestream;
    internal NavmeshIPC navmesh;

    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();
        Service.Plugin = this;
        Configuration = pluginInterface.GetPluginConfig() as GlobalTurnInConfig ?? new GlobalTurnInConfig();
        ECommonsMain.Init(pluginInterface, this, ECommons.Module.DalamudReflector, ECommons.Module.ObjectFunctions);
        EzConfigGui.Init(new MainWindow().Draw);
        EzConfigGui.WindowSystem.AddWindow(new SettingMenu());
        EzCmd.Add(Command, OnCommand, "Open Interface");
        Aliases.ToList().ForEach(a => EzCmd.Add(a, OnCommand, $"{Command} Alias"));

        autoRetainer = new();
        deliveroo = new();
        lifestream = new();
        navmesh = new();
    }
    public void Dispose()
    {
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
