using Dalamud.Configuration;
using ECommons.DalamudServices;

namespace SamplePlugin;

[Serializable]
public class GlobalTurnInConfig : IPluginConfiguration
{
    public int Version { get; set; } = 0;


    internal void Save() => Svc.PluginInterface.SavePluginConfig(this);
}
