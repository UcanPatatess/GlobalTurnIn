using ECommons.Automation.NeoTaskManager;
using ECommons.Configuration;
using ECommons.DalamudServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskTeleportTo
    {
        internal static int WhereToTeleportInt()
        {
            var where = 478;// idyllshire
            if (DeltascapeTurnInCount > 0)
            {
                where = 635;// Rhalgr
                return where;
            }
            return where;
        } 
        internal static string WhereToTeleportString()
        {
            var where = "idyllshire";
            if (DeltascapeTurnInCount > 0)
            {
                where = "Rhalgr";
                return where;
            }
            return where;
        }
        internal static void Enqueue()
        {
            Svc.Log.Info("TaskTeleportTo");
            if (IsTeritory(WhereToTeleportInt())) { return; }
            P.taskManager.Enqueue(()=> Teleport());

        }
        private static bool IsTeritory(int TeritoryId)
        {
            if (CurrentTerritory() == TeritoryId && PlayerNotBusy())
            {
                return true;
            }
            return false;
        }
        private static TaskManagerConfiguration LSConfig => new(timeLimitMS: 2 * 60 * 1000);
        private static void Teleport() => P.taskManager.InsertMulti([new(() => P.lifestream.ExecuteCommand("tp " + WhereToTeleportString())), new(() => !IsTeritory(WhereToTeleportInt())), new(() => IsTeritory(WhereToTeleportInt()), LSConfig)]);
    }
}
