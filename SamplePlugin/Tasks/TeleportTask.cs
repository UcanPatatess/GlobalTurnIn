using ECommons.Throttlers;
using SamplePlugin.Tasks;

namespace GlobalTurnIn.Tasks
{
    public class TeleportTask(uint territoryId,string teleportname) : IBaseTask
    {
        public unsafe bool? Run() 
        {
            if (GetZoneID() == territoryId)
                return true; // already in correct zone
            else 
            {
                if (PlayerNotBusy())
                {
                    if (EzThrottler.Throttle("Teleporting", 1000))
                    {
                        Lifestream.ExecuteCommand("tp " + teleportname);
                    }
                }
                return false; ;
            }
        }
    }
}
