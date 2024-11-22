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
public class Config : IPluginConfiguration
{
    public int Version { get; set; } = 0;
    public int ItemTest { get; set; } = 0;
    public bool UseTicket { get; set; } = false;
    public bool MaxItem { get; set; } = false;
    public bool MaxArmory { get; set; } = false;
    public int MaxArmoryFreeSlot { get; set; } = 2;
    public bool vendorTurnIn {  get; set; } = false;
    public bool teleportToFC { get; set; } = false;

    // the below exist just to make saving less cumbersome
    internal void Save() => Svc.PluginInterface.SavePluginConfig(this);
}
