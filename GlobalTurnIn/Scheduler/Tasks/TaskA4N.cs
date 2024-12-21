using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Dalamud.Game.ClientState.Conditions;
using System.Numerics;
using ECommons.Automation.NeoTaskManager;
using System.Runtime.InteropServices;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ValueType = FFXIVClientStructs.FFXIV.Component.GUI.ValueType;
using System.Data;

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

            P.taskManager.Enqueue(() => IsInZone(A4NMapID));
            P.taskManager.Enqueue(PlayerNotBusy);
            TaskTarget.Enqueue(RightForeleg);
            P.taskManager.Enqueue(() => MoveToCombat(RightForeLegPos), "Moving to Combat");
            P.taskManager.Enqueue(() => !Svc.Condition[ConditionFlag.InCombat], "Waiting for combat to end", DConfig);
            // if wrath installed, enable wrath
            // also if Wrath Rotation, enable the distance module in BM 
            // elseif !wrath rotation, enable BM rotation + Ai

            TaskMoveTo.Enqueue(new Vector3(-0.08f, 10.6f, -6.46f), "Center Chest", 0.5f);
            TaskOpenChest.Enqueue(A4NChest1);
            TaskOpenChest.Enqueue(A4NChest2);
            TaskOpenChest.Enqueue(A4NChest3);
            P.taskManager.Enqueue(LeaveDuty);
            P.taskManager.Enqueue(UpdateStats);
            P.taskManager.Enqueue(() => !IsInZone(A4NMapID));
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
        private static readonly AbandonDuty ExitDuty = Marshal.GetDelegateForFunctionPointer<AbandonDuty>(Svc.SigScanner.ScanText("E8 ?? ?? ?? ?? 48 8B 43 28 41 B2 01"));

        private delegate void AbandonDuty(bool a1);

        public static void LeaveDuty() => ExitDuty(false);

        private static TaskManagerConfiguration DConfig => new(timeLimitMS: 10 * 60 * 1000, abortOnTimeout: false);

        private static void UpdateStats()
        {
            C.SessionStats.TotalA4nRuns = C.SessionStats.TotalA4nRuns + 1;
            C.Stats.TotalA4nRuns = C.Stats.TotalA4nRuns + 1;
        }
    }
}
