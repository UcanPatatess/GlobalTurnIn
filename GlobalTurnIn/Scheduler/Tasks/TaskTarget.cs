using Dalamud.Game.ClientState.Objects.Types;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskTarget
    {
        public static void Enqueue(ulong objectID)
        {
            IGameObject? gameObject = null;
            P.taskManager.Enqueue(() => TryGetObjectByDataId(objectID, out gameObject), "Getting Object");
            P.taskManager.Enqueue(() => TargetByID(gameObject), "Targeting Object");
        }
    }
}
