using Dalamud.Game.ClientState.Objects.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskInteract
    {
        public static void Enqueue(uint dataID)
        {
            IGameObject? gameObject = null;
            P.taskManager.Enqueue(() => Util.TryGetObjectByDataId(dataID, out gameObject));
            P.taskManager.Enqueue(() => Util.TargetByID(gameObject));
        }
    }
}
