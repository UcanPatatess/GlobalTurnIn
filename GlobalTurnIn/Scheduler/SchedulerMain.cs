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
                        //TaskGcTurnIn.Enqueue();
                    }
                    P.taskManager.Enqueue(TaskMountUp.Enqueue);
                    P.taskManager.Enqueue(() => TaskMoveTo.Enqueue(new Vector3(57, 207, -11)));
                    /*
                    if (IsThereTradeItem())
                    {
                        if (!C.ChangeArmory)
                        {
                            TaskChangeArmorySetting.Enqueue();
                            C.ChangeArmory = true;
                        }
                        if ((GordianTurnInCount > 0 || AlexandrianTurnInCount > 0) && GetInventoryFreeSlotCount() != 0)
                        {

                        }
                        else
                        {
                            Svc.Chat.Print("No TurnIn material Stopping");
                        }
                    }*/
                }
            }
        }
    }
}
