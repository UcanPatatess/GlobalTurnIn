using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ECommons.DalamudServices;
using ECommons.SimpleGui;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using ImGuiNET;

namespace SamplePlugin.Windows
{
    public class DebugWindow : Window, IDisposable
    {
        public static new readonly string WindowName = "Debug";
        public DebugWindow() : base(WindowName) 
        {
            Flags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoCollapse;
            SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(100, 100),
                MaximumSize = new Vector2(800, 600)
            };
        }
        public override void Draw() 
        {
            ImGui.Text($"PlayerZone :" + Svc.ClientState.TerritoryType);
            ImGui.Text($"Target :" + Svc.Targets.Target?.Name.TextValue ?? "");
            ImGui.Text($"EnqueBussy :" + TaskManager.IsBusy.ToString());
            int ItemTest = Service.Configuration.ItemTest;
            ImGui.Text($"Input ItemId :");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100);
            if (ImGui.InputInt("", ref ItemTest))
            {
                Service.Configuration.ItemTest = ItemTest;
                Service.Configuration.Save();
            }
            ImGui.Text($"ItemCountTest :" + GetItemCount(ItemTest));
            ImGui.Text($"IsPlayerMoving :" + IsMoving());

        }
        public void Dispose() { }
    }
}
