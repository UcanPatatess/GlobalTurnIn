using ECommons.Throttlers;

namespace SamplePlugin.Tasks
{
    public class AethernetTask(int territoryId,string aethername) : IBaseTask
    {
        public unsafe bool? Run()
        {
            if (GetZoneID() == territoryId)
                return true; // already in correct zone
            else
            {
                if (!Lifestream.IsBusy())
                {
                    if (EzThrottler.Throttle("Teleporting", 4000))
                    {
                        Lifestream.ExecuteCommand(aethername);
                    }
                }
                return false; ;
            }
        }
    }
}
