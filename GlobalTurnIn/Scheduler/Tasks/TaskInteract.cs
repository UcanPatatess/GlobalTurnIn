using Dalamud.Game.ClientState.Objects.Types;
using GlobalTurnIn.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskInteract
    {
        public static void Enqueue(ulong dataID)
        {
            IGameObject? gameObject = null;
            P.taskManager.Enqueue(() => TargetUtil.TryGetObjectByDataId(dataID, out gameObject));
            P.taskManager.Enqueue(() => TargetUtil.InteractWithObject(gameObject));
        }
    }
}
