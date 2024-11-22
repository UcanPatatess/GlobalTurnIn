using System.Linq;
using System.Numerics;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.Game;
using SamplePlugin.IPC;
using SamplePlugin.Tasks.Base;

namespace SamplePlugin.Tasks;

public class PathfindTask(Vector3 targetPosition, bool sprint = false, float toleranceDistance = 3f) : IBaseTask
{
    
    public unsafe bool? Run()
    {
        Svc.Log.Info("PathfindTask runned.");
        if (targetPosition.Distance(Player.GameObject->Position) <= toleranceDistance)
        {
            Navmesh.Stop();
            return true;
        }
        
        if (sprint && Player.Status.All(e => e.StatusId != 50) && SprintCD == 0 
                   && ActionManager.Instance()->GetActionStatus(ActionType.GeneralAction, 4) == 0)
        {
            if (EzThrottler.Throttle("Sprint"))
            {
                ActionManager.Instance()->UseAction(ActionType.GeneralAction, 4);
            }
        }
        
        if (Navmesh.PathfindInProgress() || Navmesh.IsRunning() || IsMoving()) return false;

        Navmesh.PathfindAndMoveTo(targetPosition, false);
        Navmesh.SetAlignCamera(true);
        
        return false;
    }
}
