using ECommons.Automation;
using ECommons.Throttlers;
using GlobalTurnIn.Scheduler.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskChangeArmorySetting
    {
        internal static bool? Enqueue()
        {
            if (EzThrottler.Throttle("", 20))
                P.taskManager.Enqueue(() => GenericHandlers.OpenCharaSettings());

            if (EzThrottler.Throttle("", 20))
                P.taskManager.Enqueue(() => GenericHandlers.FireCallback("ConfigCharacter", true, 10, 0, 20));
            if (EzThrottler.Throttle("", 20))
                P.taskManager.Enqueue(() => GenericHandlers.FireCallback("ConfigCharacter", true, 18, 300, C.MaxArmory ? 1 : 0));
            if (EzThrottler.Throttle("", 20))
                P.taskManager.Enqueue(() => GenericHandlers.FireCallback("ConfigCharacter", true, 0));
            if (EzThrottler.Throttle("", 20))
                P.taskManager.Enqueue(() => GenericHandlers.FireCallback("ConfigCharacter", true, -1));

            return true;
        }
    }
}
