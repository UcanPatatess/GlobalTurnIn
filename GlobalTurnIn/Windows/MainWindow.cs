using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using ECommons.SimpleGui;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using GlobalTurnIn.Scheduler;
using GlobalTurnIn.Scheduler.Handlers;
using GlobalTurnIn.Scheduler.Tasks;
using ImGuiNET;
using System;
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
        public override void Draw()
        {
            ImGui.Text($"Current task is: {CurrentTask()}");
            ImGui.Text($"Number of task: {P.taskManager.NumQueuedTasks}");
            ImGui.Text($"Exchange Item Count: " + TotalExchangeItem);
            ImGui.SameLine();
            ImGui.Text($"GordianTurnIn Count: " + GordianTurnInCount);
            ImGui.Text($"AlexandrianTurnIn Count: " + AlexandrianTurnInCount);
            ImGui.SameLine();
            ImGui.Text($"DeltascapeTurnIn Count: " + DeltascapeTurnInCount);
            bool isRunning = SchedulerMain.AreWeTicking;
            if (ImGui.Button(isRunning ? "Stop" : "Start"))
            {
                if (isRunning)
                {
                    SchedulerMain.DisablePlugin(); // Call DisablePlugin if running
                }
                else
                {
                    SchedulerMain.EnablePlugin(); // Call EnablePlugin if not running
                }
            }
            ImGui.SameLine();
            if (ImGuiEx.IconButton(FontAwesomeIcon.Wrench, "Settings"))
                EzConfigGui.WindowSystem.Windows.FirstOrDefault(w => w.WindowName == SettingMenu.WindowName)!.IsOpen ^= true;
        }
    }
}
