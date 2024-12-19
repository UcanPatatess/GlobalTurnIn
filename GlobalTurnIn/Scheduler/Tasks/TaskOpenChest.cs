using Dalamud.Game.ClientState.Objects.Types;
using GlobalTurnIn.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskOpenChest
    {
        public static void Enqueue(ulong GameObjectID)
        {
            IGameObject? gameObject = null;
            P.taskManager.Enqueue(() => TargetUtil.TryGetObjectByDataId(GameObjectID, out gameObject), "Getting Object by ObjectID");
            P.taskManager.Enqueue(() => PlayerNotBusy());
            P.taskManager.Enqueue(() => TargetUtil.InteractWithObject(gameObject), "Interacting w/ Object");
        }
    }
}
