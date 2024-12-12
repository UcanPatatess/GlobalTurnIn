using ECommons.Logging;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ECommons.Automation;

namespace GlobalTurnIn.Scheduler.Handlers
{
    internal static unsafe class GetOutManager
    {
        internal static long NoSelectString = long.MaxValue;
        internal static void Tick()
        {
            if (SchedulerMain.EnableTicking)
            {
                //DuoLog.Warning($"TickCount64 = {NoSelectString}");
                if (TryGetAddonByName<AtkUnitBase>("SelectString", out var addon) && IsAddonReady(addon))
                {
                    if (Environment.TickCount64 - NoSelectString > 400)
                    {
                        if (GenericThrottle)
                        {
                            Callback.Fire(addon, true, -1);
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
