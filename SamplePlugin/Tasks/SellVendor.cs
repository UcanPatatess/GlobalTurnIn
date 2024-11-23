using ECommons.DalamudServices;
using ECommons.Throttlers;

namespace SamplePlugin.Tasks
{
    internal class SellVendor() : IBaseTask
    {
        public unsafe bool? Run() 
        {
            if (!autoRetainer.IsBusy())
            {
                if (EzThrottler.Throttle("Delivero", 1000))
                {
                    Svc.Commands.ProcessCommand("/deliveroo enable");
                    return true;
                }
            }
            return false;
        }
    }
}
