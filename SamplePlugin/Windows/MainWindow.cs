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

namespace SamplePlugin.Windows;

public class MainWindow : ConfigWindow, IDisposable
{
    public static new readonly string WindowName = "Sampling The World";
    private const string LogoManifestResource = "SamplePlugin.Data.UcanPatates.png";
    private const uint SidebarWidth = 203;
    private Point _logoSize = new(210, 203);
    private const float _logoScale = 1f;

    public MainWindow() {}
    public void Dispose() { }
    public static void SetWindowProperties()
    {
        var width = SidebarWidth;

        EzConfigGui.Window.Size = new Vector2(width, 500);
        EzConfigGui.Window.SizeConstraints = new()
        {
            MinimumSize = new Vector2(width, 320),
            MaximumSize = new Vector2(1920, 1080)
        };

        EzConfigGui.Window.SizeCondition = ImGuiCond.Always;

        EzConfigGui.Window.Flags |= ImGuiWindowFlags.AlwaysAutoResize;
        EzConfigGui.Window.Flags |= ImGuiWindowFlags.NoSavedSettings;

        EzConfigGui.Window.AllowClickthrough = false;
        EzConfigGui.Window.AllowPinning = false;
    }
    public override void Draw()
    {
        if (Svc.Texture.GetFromManifestResource(Assembly.GetExecutingAssembly(), LogoManifestResource).TryGetWrap(out var logo, out var _))
        {
            var maxWidth = 375 * 2 * 0.85f * ImGuiHelpers.GlobalScale;
            var ratio = maxWidth / _logoSize.X;
            var scaledLogoSize = new Vector2(_logoSize.X * _logoScale, _logoSize.Y * _logoScale);
            ImGui.Image(logo.ImGuiHandle, scaledLogoSize);
        }
        else
        {
            ImGui.Text("Image not found.");
        }
        ImGui.Spacing();

        ImGui.TextColored(Service.Configuration.Looping ? new Vector4(0.0f, 1.0f, 0.0f, 1.0f) : new Vector4(1.0f, 0.0f, 0.0f, 1.0f), $"Are we working: {(Service.Configuration.Looping ? "Yes" : "No")}");




        // Track the current state
        bool isRunning = Service.Example.IsEnabled;

        if (ImGui.Button(isRunning ? "Stop" : "Start"))
        {
            // Toggle the state
            isRunning = !isRunning;

            // Apply the state to your service
            Service.Example.IsEnabled = isRunning;
        }



        if (ImGui.Button("Show Settings"))
        {
            EzConfigGui.WindowSystem.Windows.FirstOrDefault(w => w.WindowName == SettingsWindow.WindowName)!.IsOpen ^= true;
        }
        ImGui.Text($"PlayerZone :" + Svc.ClientState.TerritoryType);
        ImGui.Text($"Target :" + Svc.Targets.Target?.Name.TextValue ?? "");
        ImGui.Text($"EnqueBussy :" + TaskManager.IsBusy.ToString());
        ImGui.Text($"IsPlayerMoving: " + IsMoving());
    }
}
