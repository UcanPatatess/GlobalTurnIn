using System;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ECommons.DalamudServices;
using ImGuiNET;
using ECommons.ImGuiMethods;
using ECommons.SimpleGui;

namespace SamplePlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private string GoatImagePath;
    private Plugin Plugin;
    public static new readonly string WindowName = "Sampling The World";
    private const string LogoManifestResource = "Sample.Data.goat.png";

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow() : base(WindowName) 
    {
        Flags |= ImGuiWindowFlags.NoScrollbar;

        Size = new Vector2(400, 600);
        SizeCondition = ImGuiCond.FirstUseEver;
    }
    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text($"The random config bool is {Service.Configuration.SomePropertyToBeSavedAndWithADefault}");

        

        if (ImGui.Button("Show Settings"))
        {
            //Plugin.ToggleConfigUI();
        }

        ImGui.Spacing();

        ImGui.Text("Have a goat:");
        if (Svc.Texture.GetFromManifestResource(Assembly.GetExecutingAssembly(), LogoManifestResource).TryGetWrap(out var logo, out var _))
        {
            ImGui.Image(logo.ImGuiHandle, new Vector2(210), new Vector2(203));
        }
        else
        {
            ImGui.Text("Image not found.");
        }
    }
}
