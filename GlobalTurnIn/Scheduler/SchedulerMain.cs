using ECommons.DalamudServices;
using GlobalTurnIn.Scheduler.Tasks;
using System.Numerics;

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
            P.taskManager.Abort();

            return true;
        }

        internal static void Tick()
        {
            if (AreWeTicking)
            {
                if (!P.taskManager.IsBusy)
                {
                    if (TotalExchangeItem !=0)
                    {
                        if (C.VendorTurnIn)
                        {
                            TaskTeleportTo.Enqueue();
                            if (GetDistanceToPoint(34, 208, -51) > 4)
                            {
                                TaskMountUp.Enqueue();
                            }
                            TaskMoveTo.Enqueue(new Vector3(34, 208, -51));
                            TaskSellVendor.Enqueue();
                        }
                        else
                            TaskGcTurnIn.Enqueue();
                    }
                    if (IsThereTradeItem())
                    {
                        if (!C.ChangeArmory)
                        {
                            TaskChangeArmorySetting.Enqueue();
                            C.ChangeArmory = true;
                        }
                        if ((GordianTurnInCount > 0 || AlexandrianTurnInCount > 0) && GetInventoryFreeSlotCount() != 0)
                        {
                            TaskTeleportTo.Enqueue();
                            if(GetDistanceToPoint(-19, 211, -36) > 4)
                            {
                                TaskMountUp.Enqueue();
                            }
                            TaskMoveTo.Enqueue(new Vector3(-19, 211, -36));
                            TaskTurnIn.Enqueue();
                        }
                        else
                        {
                            Svc.Chat.Print("No TurnIn material Stopping");
                        }
                    }
                    
                }
            }
        }
    }
}
