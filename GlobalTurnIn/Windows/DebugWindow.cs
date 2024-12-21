using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.Windowing;
using ECommons.DalamudServices;
using GlobalTurnIn.Scheduler.Handlers;
using GlobalTurnIn.Scheduler.Tasks;
using ImGuiNET;
using System.IO;
using System.Numerics;

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
        private float targetXPos = 0;
        private float targetYPos = 0;
        private float targetZPos = 0;
        private string pluginName = "none";
        private string commandInput = "";

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
                    if (ImGui.Button("MergeInv"))
                    {
                        TaskMergeInv.Enqueue();
                    }
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Normal Raid Farm"))
                {
                    int zoneID = Svc.ClientState.TerritoryType;
                    ImGui.Text($"Current Zone ID is: {zoneID}");
                    if (ImGui.Button("Copy Current Zone ID"))
                    {
                        ImGui.SetClipboardText($"{zoneID}");
                    }
                    
                    ImGui.InputText("Plugin Name Check", ref pluginName, 100);
                    if (PluginInstalled(pluginName))
                    {
                        ImGui.Text($"Plugin: {pluginName} is installed");
                    }
                    else
                    {
                        ImGui.Text($"Plugin: {pluginName} is not visible");
                    }
                    ImGui.InputText("Command", ref commandInput, 500);
                    if (ImGui.Button("Run Command"))
                    {
                        RunCommand(commandInput);
                    }
                    if (ImGui.Button("Add Passive Preset"))
                    {
                        P.bossMod.RefreshPreset("RoR Passive", Resources.BMRotations.rootPassive);
                    }
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Targeting Debug"))
                {
                    if (Svc.Targets?.Target != null)
                    {
                        targetXPos = (float)Math.Round(Svc.Targets.Target.Position.X, 2);
                        targetYPos = (float)Math.Round(Svc.Targets.Target.Position.Y, 2);
                        targetZPos = (float)Math.Round(Svc.Targets.Target.Position.Z, 2);
                        // Get the GameObjectId and display it in the ImGui.Text box
                        ImGui.Text($"Name: {Svc.Targets.Target.Name}");
                        ImGui.Text($"GameObjectId: {Svc.Targets.Target.GameObjectId}");
                        ImGui.Text($"DataID: {Svc.Targets.Target.DataId}");
                        if (ImGui.Button("Copy DataID to clipboard"))
                        {
                            ImGui.SetClipboardText($"{Svc.Targets.Target.DataId}");
                        }
                        if (ImGui.Button("Copy GameObjectID to clipboard"))
                        {
                            ImGui.SetClipboardText($"{Svc.Targets.Target.GameObjectId}");
                        }
                        ImGui.Text($"Target Pos: X: {targetXPos}, Y: {targetYPos}, Z: {targetZPos}");
                        if (ImGui.Button("Copy Target XYZ"))
                        {
                            ImGui.SetClipboardText($"{targetXPos}f, {targetYPos}f, {targetZPos}f");
                        }
                    }
                    else
                    {
                        // Optionally display a message if no target is selected
                        ImGui.Text("No target selected.");
                    }

                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
        }

        private void A4NGuiUi()
        {
            ImGui.Text($"A4n Duty is selected {CorrectDuty()}");
            ImGui.Text("Open A4NMapID in the duty selection");
            if (ImGui.Button("A4NMapID"))
            {
                TaskDutyFinder.Enqueue();
            }
            if (ImGui.Button("SelectDuty"))
            {
                TaskSelectCorrectDuty.Enqueue();
            }
            if (ImGui.Button("TaskLaunchDuty"))
            {
                TaskLaunchDuty.Enqueue();
            }
            if (ImGui.Button("TaskContentWidnowConfirm"))
            {
                TaskContentWidnowConfirm.Enqueue();
            }
            if (ImGui.Button("GetInA4n"))
            {
                TaskDutyFinder.Enqueue();
                TaskSelectCorrectDuty.Enqueue();
                TaskLaunchDuty.Enqueue();
                TaskContentWidnowConfirm.Enqueue();
            }
            if (ImGui.Button("Chest Task"))
            {
                TaskMoveTo.Enqueue(A4NChestCenter, "Center Chest", 0.5f);
                TaskOpenChest.Enqueue(A4NChest1);
                TaskOpenChest.Enqueue(A4NChest2);
                TaskOpenChest.Enqueue(A4NChest3);
            }
            if (ImGui.Button("Full Inside A4N"))
            {
                TaskA4N.Enqueue();
            }

            if (ImGui.Button("Full A4N Loop"))
            {
                TaskDutyFinder.Enqueue();
                TaskSelectCorrectDuty.Enqueue();
                TaskLaunchDuty.Enqueue();
                TaskContentWidnowConfirm.Enqueue();
                TaskA4N.Enqueue();
            }

            ImGui.Text($"Are we available/not busy? = {PlayerNotBusy()}");
            ImGui.SameLine();
            ImGui.Text($"PluginInstalled : {PluginInstalled("BurningDowntheHouse")}");

            IGameObject? gameObject = null;
            if (TryGetObjectByDataId(LeftForeleg, out gameObject))
            {
                ImGui.Text("Left leg is Targetable");
            }
            else if (TryGetObjectByDataId(A4NChest1, out gameObject))
            {
                ImGui.Text("Chest are Targetable");
            }
            else
            {
                ImGui.Text("Nothing is possible to target!");
            }
        }
    }
}
