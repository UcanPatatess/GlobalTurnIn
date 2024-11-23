using ECommons.DalamudServices;
using ECommons.Throttlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin.Tasks
{
    public class DeliveroTask() : IBaseTask
    {
        public unsafe bool? Run() 
        {
            if (!deliveroo.IsTurnInRunning()) 
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
