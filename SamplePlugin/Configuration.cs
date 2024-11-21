using Dalamud.Configuration;
using Dalamud.Game.Text;
using Dalamud.Plugin;
using ECommons.DalamudServices;
using Newtonsoft.Json;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace SamplePlugin;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;
    public bool IsConfigWindowMovable { get; set; } = true;
    public bool ToggleConfigUI { get; set; } = false;
    public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

    /// <summary>
    /// Gets or sets the chat channel to use.
    /// </summary>
    public XivChatType ChatType { get; set; } = XivChatType.Debug;

    /// <summary>
    /// Gets or sets the error chat channel to use.
    /// </summary>
    public XivChatType ErrorChatType { get; set; } = XivChatType.Urgent;
    internal static Configuration Load(DirectoryInfo configDirectory)
    {
        var pluginConfigPath = new FileInfo(Path.Combine(configDirectory.Parent!.FullName, $"Sample.json"));

        if (!pluginConfigPath.Exists)
            return new Configuration();

        var data = File.ReadAllText(pluginConfigPath.FullName);
        var conf = JsonConvert.DeserializeObject<Configuration>(data);
        return conf ?? new Configuration();
    }


    // the below exist just to make saving less cumbersome
    internal void Save() => Svc.PluginInterface.SavePluginConfig(this);
}
