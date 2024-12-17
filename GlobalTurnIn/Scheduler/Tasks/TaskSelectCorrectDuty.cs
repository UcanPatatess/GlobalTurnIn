using ECommons.Throttlers;
using GlobalTurnIn.Scheduler.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal class TaskSelectCorrectDuty
    {
        private static int PcallValue = C.DutyFinderCallValue;
        internal static void Enqueue()
        {
            PcallValue = 0;
            P.taskManager.Enqueue(SelectLoopIfNotCorrect);
        }
        internal static bool SelectLoopIfNotCorrect()
        {
            if (CorrectDuty()) 
            {
                C.DutyFinderCallValue = PcallValue;
                return true;
            }

            if (EzThrottler.Throttle("Throttling Pcall for Normal raid", 75))
            {
                GenericHandlers.FireCallback("ContentsFinder", true,3, PcallValue);
                PcallValue += 1;
            }
            return false;
        }
    }
}
