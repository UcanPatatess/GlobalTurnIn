using ECommons.EzIpcManager;
using SamplePlugin.Util;
using System;

#nullable disable

namespace SamplePlugin.IPC
{
    public class DeliverooIPC
    {
        public const string Name = "Deliveroo";
        public const string Repo = "https://git.carvel.li/liza/";
        public DeliverooIPC() => EzIPC.Init(this, Name, SafeWrapper.AnyException);
        public static bool Installed => Utils.HasPlugin(Name);

        [EzIPC] public Func<bool> IsTurnInRunning;
    }
}
