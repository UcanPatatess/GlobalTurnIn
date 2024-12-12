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
            P.navmesh.Stop();
            return true;
        }

        internal static void Tick()
        {
            if (AreWeTicking)
            {
                if (!P.taskManager.IsBusy)
                {
                    if (TotalExchangeItem != 0)
                    {
                        if (C.VendorTurnIn || (C.SellOilCloth && GetItemCount(10120) > 999))
                        {
                            TaskTeleportTo.Enqueue();
                            TaskMountUp.Enqueue();

                            TaskMoveTo.Enqueue(new Vector3(34, 208, -51), "Summoning Bell");
                            TaskSellVendor.Enqueue();
                        }
                        else
                            TaskGcTurnIn.Enqueue();
                    }
                    else if (IsThereTradeItem())
                    {
                        if (!C.ChangeArmory)
                        {
                            TaskChangeArmorySetting.Enqueue();
                            C.ChangeArmory = true;
                        }
                        if ((GordianTurnInCount > 0 || AlexandrianTurnInCount > 0))
                        {
                            TaskTeleportTo.Enqueue();

                            TaskMountUp.Enqueue();

                            TaskMoveTo.Enqueue(new Vector3(-19, 211, -36), "Shop NPC");
                            TaskMergeWithAutomaton.Enqueue();
                            TaskTurnIn.Enqueue();
                        }
                        else
                        {
                            // needs the rhalgar reach logic
                            Svc.Chat.Print("No TurnIn material Stopping");
                        }
                    }
                    else { DisablePlugin(); }
                }
            }
        }
    }
}
