using FFXIVClientStructs.FFXIV.Component.GUI;
using ECommons.Automation;
using ECommons.Logging;

namespace GlobalTurnIn.Scheduler.Handlers
{
    internal static unsafe class ShopExchangeItemManager
    {
        internal static long NoSelectYesno = long.MaxValue;
        internal static long NoShopExchangeItemDialog = long.MaxValue;
        internal static void Tick()
        {
            if (SchedulerMain.EnableTicking)
            {
                if (IsAddonActive("ShopExchangeItem"))
                {
                    if (TryGetAddonByName<AtkUnitBase>("ShopExchangeItemDialog",out var addon)&&IsAddonReady(addon))
                    {
                        if (Environment.TickCount64 - NoShopExchangeItemDialog > 10)
                        {
                            if (GenericThrottle)
                            {
                                Callback.Fire(addon, true, 0);
                                NoShopExchangeItemDialog = Environment.TickCount64;
                            }
                        }
                    }
                    else
                    {
                        NoShopExchangeItemDialog = Environment.TickCount64;
                    }
                    if (TryGetAddonByName<AtkUnitBase>("SelectYesno", out var addon2) && IsAddonReady(addon2))
                    {
                        if (Environment.TickCount64 - NoSelectYesno > 10)
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
