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
using Lumina.Excel.Sheets;
using SamplePlugin.Util;
using Dalamud.Utility;
using SamplePlugin.Managers;

namespace SamplePlugin.Windows;

public class SettingsWindow : Window, IDisposable
{
    public static new readonly string WindowName = "Settings Menu";

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public SettingsWindow() : base(WindowName)
    {
        Flags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoCollapse;
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(700, 360),
            MaximumSize = new Vector2(800, 600)
        };
    }

    public void Dispose() { }
    //public static void Main()
    //{
    //    while (Service.Configuration.Looping) 
    //    {
    //        if (Utils.GetTargetName() == "Mender") { }
    //    }
    //}

    public override void Draw()
    {
        // UseTicket Setting
        bool useTicket = false;
        if (ImGui.Checkbox("Use Ticket", ref useTicket))
        {
            // Handle the change
        }
        ImGui.Text("Do you want to use tickets to teleport?");

        // MaxItem Setting
        bool maxItem = true;
        if (ImGui.Checkbox("Maximize Item Buy", ref maxItem))
        {
            // Handle the change
        }
        ImGui.Text("Do you want to bypass the system and maximize your inventory by purchasing a single item multiple times?");

        // MaxArmory Setting
        bool maxArmory = false;
        if (ImGui.Checkbox("Maximize Armory", ref maxArmory))
        {
            // Handle the change
        }
        ImGui.Text("Do you want to fill your armory? MaxItem should be true to use this option.");

        // MaxArmoryFreeSlot Setting
        int maxArmoryFreeSlot = 2;
        ImGui.InputInt("Max Armory Free Slot", ref maxArmoryFreeSlot);
        ImGui.Text("How many empty slots do you want in your inventory?");

        // VendorTurnIn Setting
        bool vendorTurnIn = false;
        if (ImGui.Checkbox("Vendor Turn-In", ref vendorTurnIn))
        {
            // Handle the change
        }
        ImGui.Text("Use this setting to sell items to your retainer.");
        ImGui.Text("You'll lose some gil profit and won't gain FC points, but you'll also stay more off the radar.");

        // TeleportToFC Setting
        bool teleportToFC = false;
        if (ImGui.Checkbox("Teleport To FC", ref teleportToFC))
        {
            // Handle the change
        }
        ImGui.Text("If you want to sell your items in the FC house.");
    }
}
