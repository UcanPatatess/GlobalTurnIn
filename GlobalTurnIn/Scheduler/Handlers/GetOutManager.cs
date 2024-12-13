using ECommons.Logging;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ECommons.Automation;
using GlobalTurnIn.Scheduler.Tasks;

namespace GlobalTurnIn.Scheduler.Handlers
{
    internal static unsafe class GetOutManager
    {
        internal static long NoSelectString = long.MaxValue;
        internal static bool InternalBool = false;
        internal static bool InternalTicking // not working i have to somehow make it so it only works when i need to use it
        {
            get => InternalBool;
            set => InternalBool = value;
        }
        internal static void Tick()
        {
            if (SchedulerMain.EnableTicking && InternalTicking)
            {
                //DuoLog.Warning($"TickCount64 = {NoSelectString}");
                if (TryGetAddonByName<AtkUnitBase>("SelectString", out var addon) && IsAddonReady(addon))
                {
                    if (Environment.TickCount64 - NoSelectString > 400)
                    {
                        if (GenericThrottle)
                        {
                            Callback.Fire(addon, true, -1);
                            InternalTicking = false;
                            NoSelectString = Environment.TickCount64;
                        }
                    }
                }
                else
                {
                    NoSelectString = Environment.TickCount64;
                }
            }
        }
    }
}
