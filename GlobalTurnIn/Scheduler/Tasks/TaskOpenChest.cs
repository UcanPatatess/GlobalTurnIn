using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskOpenChest
    {
        public static void Enqueue()
        {
            if (IsInZone(GTData.A4N))
            {
                TaskMoveTo.Enqueue(GTData.A4NChest1Pos, "Chest 1", 1);
                TaskTarget.Enqueue(GTData.A4NChest1);
            }
        }
    }
}
