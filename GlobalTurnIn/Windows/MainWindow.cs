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
            ImGui.Text($"Target Name: " + Svc.Targets.Target?.Name.TextValue);
            ImGui.InputText("##Addon Visible", ref addonName, 100);
            ImGui.SameLine();
            ImGui.Text($"Addon Visible: " + IsAddonActive(addonName));
            ImGui.Text($"PlayerPos: " + PlayerPosition());
            ImGui.Text($"Navmesh BuildProgress: " + P.navmesh.BuildProgress());//working ipc
            //ImGui.SameLine();
            //ImGui.Text($"IsThereTradeItem: " + IsThereTradeItem());
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
            if (ImGui.Button("Test"))
            {
                P.lifestream.ExecuteCommand("tp Idy");
            }
        }
    }
}
