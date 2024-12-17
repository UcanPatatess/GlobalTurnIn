using Dalamud.Interface.Windowing;
using ECommons.DalamudServices;
using ECommons.SimpleGui;
using GlobalTurnIn.Scheduler.Tasks;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Windows
{
    internal class DebugWindow : Window, IDisposable
    {
        public new static readonly string WindowName = "Debug";
        public DebugWindow() : base(WindowName)
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
        private string addonName = "default";
        private float xPos = 0;
        private float yPos = 0;
        private float zPos = 0;
        private int tolerance = 0;
        public override void Draw()
        {
            if (ImGui.BeginTabBar("##Debug Tabs"))
            {
                if (ImGui.BeginTabItem("Global Turnin debug"))
                {
                    ImGui.Text($"General Information");
                    ImGui.Text($"TerritoryID: " + Svc.ClientState.TerritoryType);
                    ImGui.Text($"Target: " + Svc.Targets.Target);
                    ImGui.InputText("##Addon Visible", ref addonName, 100);
                    ImGui.SameLine();
                    ImGui.Text($"Addon Visible: " + IsAddonActive(addonName));
                    ImGui.Text($"Navmesh information");
                    ImGui.Text($"PlayerPos: " + PlayerPosition());
                    ImGui.Text($"Navmesh BuildProgress :" + P.navmesh.BuildProgress());//working ipc
                    ImGui.Text("X:");
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(75);
                    if (ImGui.InputFloat("##X Position", ref xPos))
                    {
                        xPos = (float)Math.Round(xPos, 2);
                    }
                    ImGui.SameLine();
                    ImGui.Text("Y:");
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(75);
                    if (ImGui.InputFloat("##Y Position", ref yPos))
                    {
                        yPos = (float)Math.Round(yPos, 2);
                    }
                    ImGui.SameLine();
                    ImGui.Text("Z:");
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(75);
                    if (ImGui.InputFloat("##Z Position", ref zPos))
                    {
                        zPos = (float)Math.Round(zPos, 2);
                    }
                    ImGui.SameLine();
                    ImGui.Text("Tolerance:");
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(100);
                    ImGui.InputInt("##Tolerance", ref tolerance);
                    if (ImGui.Button("Set to Current"))
                    {
                        xPos = (float)Math.Round(GetPlayerRawXPos(), 2);
                        yPos = (float)Math.Round(GetPlayerRawYPos(), 2);
                        zPos = (float)Math.Round(GetPlayerRawZPos(), 2);
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Copy to clipboard"))
                    {
                        string clipboardText = $"{xPos}f, {yPos}f, {zPos}f";
                        ImGui.SetClipboardText(clipboardText);
                    }
                    if (ImGui.Button("Vnav Moveto!"))
                    {
                        P.taskManager.Enqueue(() => TaskMoveTo.Enqueue(new Vector3(xPos, yPos, zPos), "Interact string", tolerance));
                        ECommons.Logging.InternalLog.Information("Firing off Vnav Moveto");
                    }
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Normal Raid Farm"))
                {
                    ImGui.Text($"A4n Duty is selected {CorrectDuty()}");
                    ImGui.Text("Open A4N in the duty selection");
                    if (ImGui.Button("A4N"))
                    {
                        TaskDutyFinder.Enqueue();
                    }
                    if (ImGui.Button("SelectDuty"))
                    {
                        TaskSelectCorrectDuty.Enqueue();
                    }

                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
        }
    }
}
