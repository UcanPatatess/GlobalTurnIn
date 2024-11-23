using System;
using System.Diagnostics.CodeAnalysis;
using ECommons.EzIpcManager;

#nullable disable

namespace SamplePlugin.IPC;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnassignedReadonlyField")]
public class AutoRetainerIPC
{
    public AutoRetainerIPC() => EzIPC.Init(this, "AutoRetainer.PluginState");
    
    [EzIPC] public readonly Func<bool> IsBusy;
    [EzIPC] public readonly Func<int> GetInventoryFreeSlotCount;
}
