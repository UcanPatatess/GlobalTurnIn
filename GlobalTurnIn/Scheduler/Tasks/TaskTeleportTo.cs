using ECommons.Automation.NeoTaskManager;
using ECommons.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskTeleportTo
    {
        internal static void Enqueue(string TerritoryName,int TeritoryId)
        {
            P.taskManager.Enqueue(()=> Teleport(TerritoryName));
            //if (PlayerNotBusy && CurrentTerritory(TeritoryId))
            P.taskManager.EnqueueDelay(1000);

        }
        private static void Teleport(string arg) => P.taskManager.InsertMulti([new(() => P.lifestream.ExecuteCommand("tp " + arg)), new(() => P.lifestream.IsBusy()), new(() => !P.lifestream.IsBusy(), LSConfig)]);
        private static TaskManagerConfiguration LSConfig => new(timeLimitMS: 2 * 60 * 1000);
    }
}
