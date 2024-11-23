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
using GlobalTurnIn.Tasks;
using ImGuiNET;
using SamplePlugin.Tasks;

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
            ImGui.Text($"Player X Y Z :");
            ImGui.SameLine();
            ImGui.Text(PlayerXYZ());
            if (ImGui.Button("Pathfind"))
            {
                Vector3 Targetxyz = new Vector3(10.2f, 0.1f, 2.2f);
                Enqueue(new PathfindTask(Targetxyz), 10000);
            }
            ImGui.SameLine();
            if (ImGui.Button("Interact"))
            {
                Enqueue(new InteractTask(), 100);
            }
            ImGui.SameLine();
            if (ImGui.Button("Mount Up"))
            {
                Enqueue(new MountTask(), 100);
            }
            ImGui.SameLine();
            if(ImGui.Button("Callback shop 0"))
            {
                Enqueue(new FireCallback("Shop", true, 0,11,1),100);
            }
            if (ImGui.Button("TeleportLimsa"))
            {
                Enqueue(new TeleportTask(129, "limsa"));
            }
            ImGui.Text("LifestreamBusy: " + Lifestream.IsBusy());
            if (ImGui.Button("li aftercastle"))
            {
                Enqueue(new AethernetTask(128, "aftcastle"));
            }
        }
        public void Dispose() { }
    }
}
