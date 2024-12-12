using FFXIVClientStructs.FFXIV.Component.GUI;
using ECommons.Automation;
using ECommons.Logging;

namespace GlobalTurnIn.Scheduler.Handlers
{
    internal static unsafe class SelectYesnoManager
    {
        internal static long NoSelectYesno = long.MaxValue;
        internal static void Tick()
        {
            if (SchedulerMain.EnableTicking)
            {
                if (IsAddonActive("ShopExchangeItem"))
                {
                    if (TryGetAddonByName<AtkUnitBase>("SelectYesno", out var addon2) && IsAddonReady(addon2))
                    {
                        if (Environment.TickCount64 - NoSelectYesno > 100)
                        {
                            if (GenericThrottle)
                            {
                                Callback.Fire(addon2, true, 0);
                                NoSelectYesno = Environment.TickCount64;
                            }
                        }
                    }
                    else
                    {
                        NoSelectYesno = Environment.TickCount64;
                    }
                }
            }
        }
    }
}
