using ECommons.Configuration;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace GlobalTurnIn;


public class Config : IEzConfig
{
    [JsonIgnore]
    public const int CURRENT_CONFIG_VERSION = 3;
    public int Version = CURRENT_CONFIG_VERSION;

    public bool TeleportToFC { get; set; } = false;
    public bool MaxItem { get; set; } = true;
    public bool MaxArmory { get; set; } = false;
    public int MaxArmoryFreeSlot { get; set; } = 2;
    public bool VendorTurnIn { get; set; } = false;
    public bool SellOilCloth { get; set; } = false;
    public bool ChangeArmory { get; set; } = false;
    public int DutyFinderCallValue { get; set; } = 0;
    public Stats Stats { get; set; } = new Stats();
    public Stats SessionStats { get; set; } = new Stats();
    public void UpdateStats(Action<Stats> updateAction)
    {
        updateAction(Stats);
        updateAction(SessionStats);
    }
    public void Save()
    {
        EzConfig.Save();
    }
}

