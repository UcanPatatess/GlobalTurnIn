using Dalamud.Interface.Utility.Raii;
using ECommons.DalamudServices;
using ECommons.SimpleGui;
using GlobalTurnIn.TaskAuto;
using ImGuiNET;
using System;
using System.Numerics;


namespace GlobalTurnIn.Windows
{
    internal class MainWindow : ConfigWindow, IDisposable 
    {
        Automation _auto = new();
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
            _auto.Dispose();
        }
        public override async void Draw()
        {
            using (ImRaii.Disabled(!_auto.Running))
                if (ImGui.Button("Stop current task"))
                    _auto.Stop();
            ImGui.SameLine();
            ImGui.TextUnformatted($"Status: {_auto.CurrentTask?.Status ?? "idle"}");
            ImGui.Text($"TerritoryID: "+ Svc.ClientState.TerritoryType);
            ImGui.Text($"PlayerPos: " + PlayerPosition());
            ImGui.Text($"Navmesh BuildProgress :" + Plugin.navmesh.BuildProgress());//working ipc
            if (ImGui.Button($"Test"))
            {
                _auto.Start(new MainLoopStart());
            }
        }
    }
}
