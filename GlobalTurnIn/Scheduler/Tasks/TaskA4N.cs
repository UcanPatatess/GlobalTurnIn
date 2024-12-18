using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskA4N
    {
        internal static void Enqueue()
        {
            // some things to do:
                // Could easily have BM manage the targeting for us/moving AI. 
                // This would reduce some of the opt codes for us
                // Other option is for us to just use the good old tried and true of moving ourselves to close the gap
                // Add switch statements like you did for queueing
                // Have an 
                // -> Entrance Mode (Haven't started combat yet)
                // -> In Combat
                // -> Exit Combat -> Open Chest
                // -> Leave Duty

            // need to add a check to make sure you're in the zone here, THEN check PlayerNotBusy()
            P.taskManager.Enqueue(() => PlayerNotBusy());
            // Target Left Leg 
            // if wrath installed, enable wrath
                // also if Wrath Rotation, enable the distance module in BM 
            // elseif !wrath rotation, enable BM rotation + Ai
        }
    }
}
