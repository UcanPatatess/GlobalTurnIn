using Dalamud.Game.ClientState.Objects.Types;
using GlobalTurnIn.Utilities;
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
            P.taskManager.Enqueue(() => TargetUtil.TryGetObjectByObjectId(objectID, out gameObject), "Getting Object");
            P.taskManager.Enqueue(() => TargetUtil.TargetByID(gameObject), "Targeting Object");
        }
    }
}
