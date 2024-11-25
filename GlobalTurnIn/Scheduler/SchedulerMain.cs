using ECommons.DalamudServices;
using GlobalTurnIn.Scheduler.Tasks;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler
{
    internal static unsafe class SchedulerMain
    {
        internal static bool AreWeTicking;
        internal static bool EnableTicking
        {
            get => AreWeTicking;
            private set => AreWeTicking = value;
        }
        internal static bool EnablePlugin()
        {
            EnableTicking = true;
            return true;
        }
        internal static bool DisablePlugin()
        {
            EnableTicking = false;
            return true;
        }

        static bool yes = false;
        internal static void Tick()
        {
            if (AreWeTicking) 
            {
                if (!P.taskManager.IsBusy)
                {
                    TaskMountUp.Enqueue();
                    if (!yes)
                    {
                        TaskChangeArmorySetting.Enqueue();
                        yes = true;
                    }
                }
            }
        }
    }
}
