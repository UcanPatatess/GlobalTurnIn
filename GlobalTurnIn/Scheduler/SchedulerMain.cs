using ECommons.Automation.NeoTaskManager.Tasks;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GlobalTurnIn.Scheduler.Tasks;
using System.Numerics;

namespace GlobalTurnIn.Scheduler
{
    internal static unsafe class SchedulerMain
    {
        internal static bool AreWeTicking;
        internal static bool DoWeTick
        {
            get => AreWeTicking;
            private set => AreWeTicking = value;
        }
        internal static bool EnablePlugin()
        {
            DoWeTick = true;
            return true;
        }
        internal static bool DisablePlugin()
        {
            DoWeTick = false;
            P.taskManager.Abort();
            P.navmesh.Stop();
            RunTurnin = false;
            RunA4N = false;
            CurrentState = DutyState.None;
            return true;
        }

        public static bool RunTurnin = false;
        public static bool RunA4N = false;
        private enum DutyState
        {
            None,
            OpenContentsFinder,
            SelectDuty,
            LaunchDuty,
            ConfirmContentWindow,
            EnterCombat,
            CombatMode,
            OpenChest
        }
        private static DutyState CurrentState = DutyState.None;
        internal static void Tick()
        {
            if (DoWeTick)
            {
                if (!P.taskManager.IsBusy)
                {
                    if (RunA4N)
                    {
                        switch(CurrentState)
                        {
                            case DutyState.None:
                                if (IsAddonActive("ContentsFinder"))
                                {
                                    CurrentState = DutyState.OpenContentsFinder;
                                }
                                else
                                {
                                    TaskDutyFinder.Enqueue();
                                    CurrentState = DutyState.OpenContentsFinder;
                                }
                                break;

                            case DutyState.OpenContentsFinder:
                                if (IsAddonActive("ContentsFinder"))
                                {
                                    TaskSelectCorrectDuty.Enqueue();
                                    CurrentState = DutyState.SelectDuty;
                                }
                                break;

                            case DutyState.SelectDuty:
                                if (IsAddonActive("ContentsFinder"))
                                {
                                    TaskLaunchDuty.Enqueue();
                                    CurrentState = DutyState.LaunchDuty;
                                }
                                break;

                            case DutyState.LaunchDuty:
                                if (IsAddonActive("ContentsFinderConfirm"))
                                {
                                    TaskContentWidnowConfirm.Enqueue();
                                    CurrentState = DutyState.ConfirmContentWindow;
                                }
                                break;

                            case DutyState.ConfirmContentWindow:
                                if (!IsAddonActive("ContentsFinderConfirm"))
                                {
                                    CurrentState = DutyState.None;
                                }
                                break;

                            //case DutyState.EnterCombat:
                                //if !()
                        }
                    }
                    else if (RunTurnin)
                    {
                        if (TotalExchangeItem != 0 && !C.VendorTurnIn)
                        {
                            TaskGcTurnIn.Enqueue();
                        }
                        else if ((TotalExchangeItem != 0 && C.VendorTurnIn) || (C.SellOilCloth && GetItemCount(10120) > 0))
                        {
                            if (C.TeleportToFC)
                            {
                                TaskFcSell.Enqueue();
                                TaskSellVendor.Enqueue();
                            }
                            else
                            {
                                TaskTeleportTo.Enqueue();
                                TaskMountUp.Enqueue();
                                TaskMoveTo.Enqueue(new Vector3(34, 208, -51), "Summoning Bell");
                                TaskSellVendor.Enqueue();
                            }
                        }
                        else if (IsThereTradeItem())
                        {
                            if (!C.ChangeArmory)
                            {
                                TaskChangeArmorySetting.Enqueue();
                                C.ChangeArmory = true;
                            }
                            if (GordianTurnInCount > 0 || AlexandrianTurnInCount > 0)
                            {
                                TaskTeleportTo.Enqueue();
                                TaskMountUp.Enqueue();
                                TaskMoveTo.Enqueue(new Vector3(-19, 211, -36), "Shop NPC");
                                TaskMergeInv.Enqueue();
                                TaskTurnIn.Enqueue();
                            }
                            else
                            {
                                //logic is added but it needs to be tested
                                TaskTeleportTo.Enqueue();
                                TaskMountUp.Enqueue();
                                TaskMoveTo.Enqueue(new Vector3(-55.6f, 0.0f, 51.4f), "Shop NPC");
                                TaskMergeInv.Enqueue();
                                TaskTurnIn.Enqueue();
                            }
                        }
                        else { DisablePlugin(); }
                    }
                }
            }
        }
    }
}
