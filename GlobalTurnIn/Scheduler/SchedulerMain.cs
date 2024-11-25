using ECommons.DalamudServices;
using GlobalTurnIn.Scheduler.Tasks;
using GlobalTurnIn.Windows;
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

        internal static void Tick()
        {
            if (AreWeTicking) 
            {
                if (!P.taskManager.IsBusy)
                {
                    TaskMountUp.Enqueue();
                    if (!C.ChangeArmory)
                    {
                        TaskChangeArmorySetting.Enqueue();
                        C.ChangeArmory = true;
                    }
                }
            }
        }
    }
}
