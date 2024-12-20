using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.Automation.NeoTaskManager.Tasks;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GlobalTurnIn.Scheduler.Tasks;
using ImGuiNET;
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
            NRaidRun = 0;
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
            hasEnqueuedDutyFinder = false;
            return true;
        }

        public static bool RunTurnin = false; // Used for Turnin Toggle
        public static bool RunA4N = false; // Used for N-Raid Toggle
        public static bool hasEnqueuedDutyFinder = false;
        public static string A4NTask = "";
        public static int NRaidRun = 0;

        internal static void Tick()
        {
            if (DoWeTick)
            {
                if (!P.taskManager.IsBusy)
                {
                    if (RunA4N)
                    {
                        if (NRaidRun < RunAmount || RunInfinite)
                        {
                            if (IsInZone(A4NMapID))
                            {
                                if (!Svc.Condition[ConditionFlag.InCombat])
                                {
                                    IGameObject? gameObject = null;
                                    if (TryGetObjectByDataId(LeftForeleg, out gameObject) || TryGetObjectByDataId(RightForeleg, out gameObject))
                                    {
                                        P.taskManager.Enqueue(() => A4NTask = "Targeted Left Foreleg");
                                        // Left Leg is targetable... which means you aren't in combat/you haven't initiated it yet
                                        P.taskManager.Enqueue(PlayerNotBusy);
                                        TaskTarget.Enqueue(RightForeleg);
                                        P.taskManager.Enqueue(() => MoveToCombat(RightForeLegPos), "Moving to Combat");
                                        // If Left Leg is Targetable, enable the following
                                        if (PluginInstalled("WrathCombo"))
                                        {
                                            RunCommand("wrath auto on"); // this is here while the IPC method doesn't exist (yet). Things to impliment cause I don't like commands
                                            RunCommand("vbm ai on"); // Need to dig through VBM IPC to see if this is something that I can control through that. . . 
                                        }
                                        // BM ai (to move to the target while in combat)
                                        // if Wrath installed, enable wrath + BM ai Limited
                                        // if RSR installed, 
                                        P.taskManager.Enqueue(() => !Svc.Condition[ConditionFlag.InCombat], "Waiting for combat to end", DConfig);
                                    }
                                    else if (TryGetObjectByDataId(A4NChest1, out gameObject))
                                    {
                                        if (PluginInstalled("WrathCombo"))
                                        {
                                            RunCommand("wrath auto off");
                                            RunCommand("vbm ai off");
                                        }
                                        P.taskManager.Enqueue(() => A4NTask = "Chest Task");
                                        TaskMoveTo.Enqueue(new Vector3(-0.08f, 10.6f, -6.46f), "Center Chest", 0.5f);
                                        TaskOpenChest.Enqueue(A4NChest1);
                                        TaskOpenChest.Enqueue(A4NChest2);
                                        TaskOpenChest.Enqueue(A4NChest3);
                                        P.taskManager.Enqueue(LeaveDuty);
                                        P.taskManager.Enqueue(UpdateStats);
                                        P.taskManager.Enqueue(() => !IsInZone(A4NMapID), "Waiting for you to leave A4N");
                                        hasEnqueuedDutyFinder = false;
                                        P.taskManager.Enqueue(() => NRaidRun = NRaidRun + 1);
                                    }
                                    else
                                    {
                                        P.taskManager.EnqueueDelay(100);
                                        // just an exit for it to catch/reset in case either of these come false (it shouldn't, but better to have a failsafe)
                                    }
                                }
                            }
                            else if (!IsInZone(A4NMapID))
                            {
                                A4NTask = "Loading into duty";
                                if (!IsAddonActive("ContentsFinder") && !hasEnqueuedDutyFinder)
                                {
                                    TaskDutyFinder.Enqueue();
                                }
                                else if (IsAddonActive("ContentsFinder"))
                                {
                                    TaskSelectCorrectDuty.Enqueue();
                                    TaskLaunchDuty.Enqueue();
                                    hasEnqueuedDutyFinder = true;
                                }
                                else if (IsAddonActive("ContentsFinderConfirm"))
                                {
                                    TaskContentWidnowConfirm.Enqueue();
                                }
                            }
                        }
                        else
                        {
                            DisablePlugin();
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
