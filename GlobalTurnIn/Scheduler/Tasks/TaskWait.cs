using ECommons.Loader;
using GlobalTurnIn.Scheduler.Handlers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskWait
    {
        internal static void Enqueue(int ms)
        {
            Log.Debug("Are we waiting check.");
            P.taskManager.Enqueue(() => GenericHandlers.Throttle(ms));
            P.taskManager.Enqueue(() => GenericHandlers.WaitFor(ms));
        }
    }
}
