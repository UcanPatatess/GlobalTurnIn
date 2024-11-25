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
        private bool useTicket = Configuration.UseTicket;
        private bool maxItem = Configuration.MaxItem;
        private bool maxArmory = Configuration.MaxArmory;
        private int maxArmoryFreeSlot = Configuration.MaxArmoryFreeSlot;
        private bool vendorTurnIn = Configuration.VendorTurnIn;
        private bool teleportToFC = Configuration.TeleportToFC;

        public override void Draw()
        {
            ImGui.PushItemWidth(100);
            ImGui.PopItemWidth();
            ImGui.Text("Teleport Settings:");
            ImGui.Separator();

            // UseTicket
            if (ImGui.Checkbox("Use Tickets to Teleport##useticket", ref useTicket))
            {
                Configuration.UseTicket = useTicket;
                Configuration.Save();
            }
            if (ImGui.Checkbox("Teleport to FC##teleporttofc", ref teleportToFC))
            {
                Configuration.TeleportToFC = teleportToFC;
                Configuration.Save();
            }
            ImGui.NewLine();

            ImGui.Text("Inventory Management:");
            ImGui.Separator();

            // MaxItem
            if (ImGui.Checkbox("Maximize Inventory##maxitem", ref maxItem))
            {
                Configuration.MaxItem = maxItem;
                Configuration.Save();
            }
            ImGui.TextWrapped("Maximize inventory by buying one of a single item.");


            using (ImRaii.Disabled(!maxItem))
            {
                if (!maxItem) 
                    maxArmory = false;
                if (ImGui.Checkbox("Fill Armory##maxarmory", ref maxArmory))
                {
                    Configuration.MaxArmory = maxArmory;
                }
                if (maxArmory)
                {
                    ImGui.Text("Free Armory Slots:");
                    ImGui.SameLine();
                    ImGui.PushItemWidth(100);
                    if (ImGui.InputInt("##maxarmoryfreeslot", ref maxArmoryFreeSlot))
                    {
                        if (maxArmoryFreeSlot < 0) maxArmoryFreeSlot = 0;
                        Configuration.MaxArmoryFreeSlot = maxArmoryFreeSlot;
                        Configuration.Save();
                    }
                    ImGui.PopItemWidth();
                }
            }
            ImGui.NewLine();

            ImGui.Text("Turn-in Settings:");
            ImGui.Separator();

            // VendorTurnIn
            if (ImGui.Checkbox("Vendor Turn-in##vendorturnin", ref vendorTurnIn))
            {
                Configuration.VendorTurnIn = vendorTurnIn;
                Configuration.Save();
            }
            ImGui.TextWrapped("Stay off the marketboard and sell to your retainer.");
        }
    }
}
