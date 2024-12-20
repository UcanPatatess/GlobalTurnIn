using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Dalamud.Game.ClientState.Conditions;
using System.Numerics;
using ECommons.Automation.NeoTaskManager;
using System.Runtime.InteropServices;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ValueType = FFXIVClientStructs.FFXIV.Component.GUI.ValueType;

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
            TaskTarget.Enqueue(GTData.RightForeleg);
            P.taskManager.Enqueue(() => MoveToCombat(GTData.RightForeLegPos), "Moving to Combat");
            P.taskManager.Enqueue(() => !Svc.Condition[ConditionFlag.InCombat], "Waiting for combat to end", DConfig);
            // if wrath installed, enable wrath
            // also if Wrath Rotation, enable the distance module in BM 
            // elseif !wrath rotation, enable BM rotation + Ai

            TaskMoveTo.Enqueue(new Vector3(-0.08f, 10.6f, -6.46f), "Center Chest", 0.5f);
            TaskOpenChest.Enqueue(GTData.A4NChest1);
            TaskOpenChest.Enqueue(GTData.A4NChest2);
            TaskOpenChest.Enqueue(GTData.A4NChest3);
            P.taskManager.Enqueue(() => LeaveDuty());
        }

        private static float Distance(this Vector3 v, Vector3 v2)
        {
            return new Vector2(v.X - v2.X, v.Z - v2.Z).Length();
        }
        private static unsafe bool IsMoving()
        {
            return AgentMap.Instance()->IsPlayerMoving == 1;
        }

        internal unsafe static bool? MoveToCombat(Vector3 targetPosition, float toleranceDistance = 3f)
        {
            if (targetPosition.Distance(Player.GameObject->Position) <= toleranceDistance || Svc.Condition[ConditionFlag.InCombat])
            {
                P.navmesh.Stop();
                return true;
            }
            if (!P.navmesh.IsReady()) { UpdateCurrentTask("Waiting Navmesh"); return false; }
            if (P.navmesh.PathfindInProgress() || P.navmesh.IsRunning() || IsMoving()) return false;

            P.navmesh.PathfindAndMoveTo(targetPosition, false);
            P.navmesh.SetAlignCamera(true);
            return false;
        }

        private static unsafe bool? LeaveDuty()
        {
            var agent = AgentModule.Instance()->GetAgentByInternalId(AgentId.ContentsFinderMenu);
            if (agent == null)
            {
                return false;
            }

            var eventObject = stackalloc AtkValue[1];
            var atkValues = CreateEventParams();
            if (atkValues != null)
            {
                try
                {
                    agent->ReceiveEvent(eventObject, atkValues, 1, 0);
                    return true;
                }
                finally
                {
                    Marshal.FreeHGlobal(new IntPtr(atkValues));
                }
            }

            return false;
        }

        public static unsafe AtkValue* CreateEventParams()
        {
            try
            {
                var atkValues = (AtkValue*)Marshal.AllocHGlobal(sizeof(AtkValue));
                if (atkValues == null) return null;
                atkValues[0].Type = ValueType.Int;
                atkValues[0].Int = 0;
                return atkValues;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static TaskManagerConfiguration DConfig => new(timeLimitMS: 10 * 60 * 1000, abortOnTimeout: false);


    }
}
