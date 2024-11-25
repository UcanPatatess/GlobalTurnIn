using GlobalTurnIn.Scheduler.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskMountUp
    {
        internal static void Enqueue()
        {
            P.taskManager.Enqueue(PlayerWorldHandlers.MountUp);
        }
    }
}
