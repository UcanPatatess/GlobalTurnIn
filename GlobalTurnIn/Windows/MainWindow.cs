using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using ECommons.SimpleGui;
using ECommons.Throttlers;
using GlobalTurnIn.Scheduler;
using GlobalTurnIn.Scheduler.Handlers;
using GlobalTurnIn.TaskAuto;
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
        private string addonName = "kpala";
        
        public override void Draw()
        {
            ImGui.Text($"TerritoryID: "+ Svc.ClientState.TerritoryType);
            ImGui.SameLine();
            ImGui.Text($"Target: " + Svc.Targets.Target);
            ImGui.InputText("##Addon Visible", ref addonName, 100);
            ImGui.SameLine();
            ImGui.Text($"Addon Visible: " + IsAddonActive(addonName));
            ImGui.Text($"PlayerPos: " + PlayerPosition());
            ImGui.Text($"Navmesh BuildProgress :" + P.navmesh.BuildProgress());//working ipc
            ImGui.Text($"IsThereTradeItem " + IsThereTradeItem());
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
            if (ImGui.Button("Test"))
            {

            }
        }
    }
}
