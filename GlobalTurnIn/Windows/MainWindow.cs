using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using ECommons.SimpleGui;
using GlobalTurnIn.Scheduler;
using ImGuiNET;
using System.Numerics;

namespace GlobalTurnIn.Windows
{
    internal class MainWindow : ConfigWindow, IDisposable 
    {
        public MainWindow() : base() 
        {
            Flags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoCollapse;
            SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(100, 100),
                MaximumSize = new Vector2(800, 600)
            };
        }
        public void Dispose() 
        {
        }
        private string CurrentTask()
        {
            if (P.taskManager.NumQueuedTasks > 0 && P.taskManager.CurrentTask != null)
            {
                return P.taskManager.CurrentTask.Name?.ToString() ?? "None";
            }
            return "None";
        }
        private void DrawStatsTab()
        {
            if (ImGui.BeginTabBar("Stats"))
            {
                if (ImGui.BeginTabItem("Lifetime"))
                {
                    this.DrawStatsTab(C.Stats, out bool reset);

                    if (reset)
                    {
                        C.Stats = new();
                        C.Save();
                    }
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Session"))
                {
                    this.DrawStatsTab(C.SessionStats, out bool reset);
                    if (reset)
                        C.SessionStats = new();
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
        }
        private void DrawStatsTab(Stats stat, out bool reset)
        {   
            DrawStats(stat);

            bool isCtrlHeld = ImGui.GetIO().KeyCtrl;
            using (var _ = ImRaii.PushStyle(ImGuiStyleVar.Alpha, 0.5f, !ImGui.GetIO().KeyCtrl))
                reset = ImGui.Button("RESET STATS", new Vector2(ImGui.GetContentRegionAvail().X, ImGui.GetContentRegionAvail().Y)) && ImGui.GetIO().KeyCtrl;
            if (ImGui.IsItemHovered()) ImGui.SetTooltip(isCtrlHeld ? "Press to reset your stats." : "Hold Ctrl to enable the button.");
        }
        private void DrawStats(Stats stat)
        {
            ImGui.BeginChild("Stats", new Vector2(0, ImGui.GetContentRegionAvail().Y - 30f), true);
            ImGui.Columns(3, null, false);
            ImGui.NextColumn();
            ImGuiEx.CenterColumnText(ImGuiColors.DalamudRed, "Root Of Riches", true);
            ImGuiHelpers.ScaledDummy(10f);
            ImGui.Columns(2, null, false);
            ImGui.NextColumn();
            ImGui.NextColumn();
            ImGuiEx.CenterColumnText("GillEarned", true);
            ImGui.NextColumn();
            ImGuiEx.CenterColumnText("TotalA4nRuns", true);
            ImGui.NextColumn();
            ImGuiEx.CenterColumnText($"{stat.GillEarned.ToString("N0")}");
            ImGui.NextColumn();
            ImGuiEx.CenterColumnText($"{stat.TotalA4nRuns.ToString("N0")}");

            ImGui.EndChild();
        }
        public override void Draw()
        {
            if (ImGui.BeginTabBar("##Main Window Tabs"))
            {
                if (ImGui.BeginTabItem("Turnin Items"))
                {
                    ImGui.Text($"Current task (Ice) is: {icurrentTask}");
                    ImGui.Text($"Current task is: {CurrentTask()}");
                    ImGui.Text($"Number of task: {P.taskManager.NumQueuedTasks}");
                    ImGui.Text($"Exchange Item Count: " + TotalExchangeItem);
                    ImGui.SameLine();
                    ImGui.Text($"GordianTurnIn Count: " + GordianTurnInCount);
                    ImGui.Text($"AlexandrianTurnIn Count: " + AlexandrianTurnInCount);
                    ImGui.SameLine();
                    ImGui.Text($"DeltascapeTurnIn Count: " + DeltascapeTurnInCount);
                    if (ImGui.Button(SchedulerMain.DoWeTick ? "Stop" : "Start Turnin"))
                    {
                        if (SchedulerMain.DoWeTick)
                        {
                            SchedulerMain.DisablePlugin(); // Call DisablePlugin if running
                        }
                        else
                        {
                            SchedulerMain.EnablePlugin(); // Call EnablePlugin if not running
                            SchedulerMain.RunTurnin = true;
                        }
                    }
                    ImGui.SameLine();
                    if (ImGuiEx.IconButton(FontAwesomeIcon.Wrench, "Settings"))
                        EzConfigGui.WindowSystem.Windows.FirstOrDefault(w => w.WindowName == SettingMenu.WindowName)!.IsOpen ^= true;
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Normal Raid Farm"))
                {
                    ImGui.Text($"Current task is: {SchedulerMain.A4NTask}");
                    ImGui.Text($"Number of task: {P.taskManager.NumQueuedTasks}");
                    ImGui.Text($"Zone ID = {Svc.ClientState.TerritoryType}");
                    using (ImRaii.Disabled(false))// did this to realse the stats
                    {
                        if (ImGui.Button(SchedulerMain.DoWeTick ? "Stop" : "Start A4NMapID"))
                        {
                            if (SchedulerMain.DoWeTick)
                            {
                                SchedulerMain.DisablePlugin(); // Call DisablePlugin if running
                            }
                            else
                            {
                                SchedulerMain.EnablePlugin(); // Call EnablePlugin if not running
                                SchedulerMain.RunA4N = true;
                            }
                        }
                    }
                    ImGui.Text("Coming soon");
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Stats"))
                {
                    DrawStatsTab();
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
        }
    }
}
