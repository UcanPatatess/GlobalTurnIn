using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SamplePlugin.Windows;
using ECommons;
using SomethingNeedDoing.Managers;
using ECommons.SimpleGui;
using System.Linq;

namespace SamplePlugin;

public sealed class Plugin : IDalamudPlugin
{
    public static string Prefix => "SMP";
    private const string Command = "/pmysample";
    private static string[] Aliases => ["/sample", "/simp"];
    public Plugin(IDalamudPluginInterface pluginInterface) 
    {
        pluginInterface.Create<Service>();
        Service.Plugin = this;
        Service.Configuration = Configuration.Load(pluginInterface.ConfigDirectory);

        Service.ChatManager = new ChatManager();
        Service.GameEventManager = new GameEventManager();

        EzConfigGui.WindowSystem.AddWindow(new MainWindow());
        EzCmd.Add(Command, OnCommand, "Open Settings.");
        Aliases.ToList().ForEach(a => EzCmd.Add(a, OnCommand, $"{Command} Alias"));

        ECommonsMain.Init(pluginInterface, this);
    }

    public void Dispose()
    {
        Service.GameEventManager?.Dispose();
        Service.ChatManager?.Dispose();
        ECommonsMain.Dispose();
    }

    private void OnCommand(string command, string args)
    {
        args = args.Trim();

        if (args == string.Empty)
        {
            EzConfigGui.Window.IsOpen ^= true;
            return;
        }
    }
}
