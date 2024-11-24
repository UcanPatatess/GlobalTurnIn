using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System.Numerics;


namespace GlobalTurnIn.Windows
{
    internal class SettingMenu : Window ,IDisposable
    {
        public static new readonly string WindowName = "GlobalTurnIn Settings";
        public SettingMenu() : base(WindowName)
        {
            Flags = ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse;
            ImGui.SetNextWindowSize(new Vector2(450, 0), ImGuiCond.Always);
        }
        
        public void Dispose() { }
        private bool UseTicket = Configuration.UseTicket;
        private bool MaxItem = Configuration.MaximizeInv;
        private bool MaxArmory = Configuration.FillArmory;
        private int MaxArmoryFreeSlot = Configuration.FreeArmorySlots;
        private bool VendorTurnIn = Configuration.VendorTurnIn;
        private bool TeleportToFC = Configuration.TeleportToFC;

        public override void Draw()
        {
            ImGui.PushItemWidth(100);
            ImGui.PopItemWidth();
            ImGui.Text("Teleport Settings:");
            ImGui.Separator();

            // UseTicket
            if (ImGui.Checkbox("Use Tickets to Teleport##useticket", ref UseTicket))
            {
                Configuration.UseTicket = UseTicket;
                Configuration.Save();
            }
            if (ImGui.Checkbox("Teleport to FC##teleporttofc", ref TeleportToFC))
            {
                Configuration.TeleportToFC = TeleportToFC;
                Configuration.Save();
            }
            ImGui.NewLine();

            ImGui.Text("Inventory Management:");
            ImGui.Separator();

            // MaxItem
            if (ImGui.Checkbox("Maximize Inventory##maxitem", ref MaxItem))
            {
                Configuration.MaximizeInv = MaxItem;
                Configuration.Save();
            }
            ImGui.TextWrapped("Maximize inventory by buying one of a single item.");


            using (ImRaii.Disabled(!MaxItem))
            {
                if (!MaxItem) 
                    MaxArmory = false;
                if (ImGui.Checkbox("Fill Armory##maxarmory", ref MaxArmory))
                {
                    Configuration.FillArmory = MaxArmory;
                }
                if (MaxArmory)
                {
                    ImGui.Indent();
                    ImGui.Text("Free Armory Slots:");
                    ImGui.SameLine();
                    ImGui.PushItemWidth(100);
                    if (ImGui.InputInt("##maxarmoryfreeslot", ref MaxArmoryFreeSlot))
                    {
                        if (MaxArmoryFreeSlot < 0) MaxArmoryFreeSlot = 0;
                        Configuration.FreeArmorySlots = MaxArmoryFreeSlot;
                        Configuration.Save();
                    }
                    ImGui.PopItemWidth();
                    ImGui.Unindent();
                }
            }
            ImGui.NewLine();

            ImGui.Text("Turn-in Settings:");
            ImGui.Separator();

            // VendorTurnIn
            if (ImGui.Checkbox("Vendor Turn-in##vendorturnin", ref VendorTurnIn))
            {
                Configuration.VendorTurnIn = VendorTurnIn;
                Configuration.Save();
            }
            ImGui.TextWrapped("Stay off the marketboard and sell to your retainer.");
        }
    }
}
