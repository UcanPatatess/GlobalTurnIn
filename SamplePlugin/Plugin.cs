using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SamplePlugin.Windows;
using ECommons;
using ECommons.SimpleGui;
using System.Linq;
using ECommons.DalamudServices;
using SamplePlugin.Managers;
using ECommons.Automation.LegacyTaskManager;
using SamplePlugin.IPC;

namespace SamplePlugin;

public sealed class Plugin : IDalamudPlugin
{
    public static string Prefix => "SMP";
    private const string Command = "/pmysample";
    private readonly LoopingService exampleService;
    private static string[] Aliases => ["/sample", "/simp"];
    public Plugin(IDalamudPluginInterface pluginInterface) 
    {
        pluginInterface.Create<Service>();
        Service.Plugin = this;
        Configuration = pluginInterface.GetPluginConfig() as Config ?? new Config();
        //Service.Configuration = Config.Load(pluginInterface.ConfigDirectory);
        ECommonsMain.Init(pluginInterface, this);

        exampleService = new LoopingService();
        Service.Example = exampleService;
        NavmeshIPC navmesh = new();
        Service.Navmesh = navmesh;


        EzConfigGui.Init(new MainWindow().Draw);
        MainWindow.SetWindowProperties();
        EzConfigGui.WindowSystem.AddWindow(new SettingsWindow());
        EzConfigGui.WindowSystem.AddWindow(new DebugWindow());

        EzCmd.Add(Command, OnCommand, "Open Settings.");
        Aliases.ToList().ForEach(a => EzCmd.Add(a, OnCommand, $"{Command} Alias"));
    }

    public void Dispose()
    {
        ECommonsMain.Dispose();
        exampleService.Dispose();
        
    }
    private void OnCommand(string command, string args)
    {
        if (args == "debug") 
        {
            EzConfigGui.WindowSystem.Windows.FirstOrDefault(w => w.WindowName == DebugWindow.WindowName)!.IsOpen ^= true;
        }
        else 
        { 
            EzConfigGui.Window.IsOpen = !EzConfigGui.Window.IsOpen; return;
        }
        

    }
}
