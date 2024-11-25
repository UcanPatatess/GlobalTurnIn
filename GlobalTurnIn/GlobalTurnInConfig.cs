using Dalamud.Configuration;
using ECommons.DalamudServices;

namespace SamplePlugin;

[Serializable]
public class GlobalTurnInConfig : IPluginConfiguration
{
    public int Version { get; set; } = 0;
    public bool UseTicket { get; set; } = false;
    public bool TeleportToFC { get; set; } = false;
    public bool MaxItem { get; set; } = false;
    public bool MaxArmory { get; set; } = false;
    public int MaxArmoryFreeSlot { get; set; } = 2;
    public bool VendorTurnIn {  get; set; } = false;
    internal void Save() => Svc.PluginInterface.SavePluginConfig(this);
}
