using Dalamud.Game.ClientState.Conditions;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Network;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using FFXIVClientStructs.Interop;
using System.Numerics;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.GameHelpers;
using ECommons.Reflection;
using Dalamud.Game.ClientState.Objects.SubKinds;
using ImGuiNET;


namespace GlobalTurnIn;

public static unsafe class Game
{
    public static bool ExecuteTeleport(uint aetheryteId) => UIState.Instance()->Telepo.Teleport(aetheryteId, 0);
    internal static unsafe float GetDistanceToPlayer(Vector3 v3) => Vector3.Distance(v3, Player.GameObject->Position);
    internal static unsafe float GetDistanceToPlayer(IGameObject gameObject) => GetDistanceToPlayer(gameObject.Position);
    internal static IGameObject? GetObjectByName(string name) => Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(o => o.Name.TextValue.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    public static unsafe int GetInventoryFreeSlotCount()
    {
        InventoryType[] types = [InventoryType.Inventory1, InventoryType.Inventory2, InventoryType.Inventory3, InventoryType.Inventory4];
        var slots = 0;
        foreach (var x in types)
        {
            var cont = InventoryManager.Instance()->GetInventoryContainer(x);
            for (var i = 0; i < cont->Size; i++)
                if (cont->Items[i].ItemId == 0)
                    slots++;
        }
        return slots;
    }
    public static unsafe int GetItemCount(int itemID, bool includeHQ = true)
       => includeHQ ? InventoryManager.Instance()->GetInventoryItemCount((uint)itemID, true) + InventoryManager.Instance()->GetInventoryItemCount((uint)itemID) + InventoryManager.Instance()->GetInventoryItemCount((uint)itemID + 500_000)
       : InventoryManager.Instance()->GetInventoryItemCount((uint)itemID) + InventoryManager.Instance()->GetInventoryItemCount((uint)itemID + 500_000);
    /*
    public static bool InteractWith(ulong instanceId)
    {
        var obj = GameObjectManager.Instance()->Objects.GetObjectByGameObjectId(instanceId);
        if (obj == null)
            return false;
        TargetSystem.Instance()->InteractWithObject(obj);
        return true;
    }
    
    public static void TeleportToAethernet(uint currentAetheryte, Vector3 destinationAetheryte)
    {
        
        Span<uint> payload = [4, destinationAetheryte];
        PacketDispatcher.SendEventCompletePacket(0x50000 | currentAetheryte, 0, 0, payload.GetPointer(0), (byte)payload.Length, null);
    } 
    */
    public static bool PluginInstalled(string name)
    {
        return DalamudReflector.TryGetDalamudPlugin(name, out _, false, true);
    }
    public static GameObject* LPlayer() => GameObjectManager.Instance()->Objects.IndexSorted[0].Value;
   
    public static Vector3 PlayerPosition()
    {
        var player = LPlayer();
        return player != null ? player->Position : default;
    }

    public static bool PlayerInRange(Vector3 dest, float dist)
    {
        var d = dest - PlayerPosition();
        return d.X * d.X + d.Z * d.Z <= dist * dist;
    }

    public static bool PlayerIsBusy() => Service.Conditions[ConditionFlag.BetweenAreas] || Service.Conditions[ConditionFlag.Casting] || ActionManager.Instance()->AnimationLock > 0;

    public static uint CurrentTerritory() => GameMain.Instance()->CurrentTerritoryTypeId;

    public static (ulong id, Vector3 pos) FindAetheryte(uint id)
    {
        foreach (var obj in GameObjectManager.Instance()->Objects.IndexSorted)
            if (obj.Value != null && obj.Value->ObjectKind == ObjectKind.Aetheryte && obj.Value->BaseId == id)
                return (obj.Value->GetGameObjectId(), *obj.Value->GetPosition());
        return (0, default);
    }

    public static bool IsAddonActive(string AddonName) // bunu kullan
    {
        var addon = RaptureAtkUnitManager.Instance()->GetAddonByName(AddonName);
        return addon != null && addon->IsVisible && addon->IsReady;
    }

    public static bool CloseShop() //dükkanı kapattı biraz daha bakılması lazım
    {
        var agent = AgentShop.Instance();
        if (agent == null || agent->EventReceiver == null)
            return false;
        AtkValue res = default, arg = default;
        var proxy = (ShopEventHandler.AgentProxy*)agent->EventReceiver;
        proxy->Handler->CancelInteraction();
        arg.SetInt(-1);
        agent->ReceiveEvent(&res, &arg, 1, 0);
        return true;
    }
    public static bool IsTalkInProgress()
    {
        var addon = RaptureAtkUnitManager.Instance()->GetAddonByName("Talk");
        return addon != null && addon->IsVisible && addon->IsReady;
    }

    public static void ProgressTalk()
    {
        var addon = RaptureAtkUnitManager.Instance()->GetAddonByName("Talk");
        if (addon != null && addon->IsReady)
        {
            var evt = new AtkEvent() { Listener = &addon->AtkEventListener, Target = &AtkStage.Instance()->AtkEventTarget };
            var data = new AtkEventData();
            addon->ReceiveEvent(AtkEventType.MouseClick, 0, &evt, &data);
        }
    }

    // TODO: this really needs revision...
    public static void SelectTurnIn()
    {
        var addon = RaptureAtkUnitManager.Instance()->GetAddonByName("SelectString");
        if (addon != null && addon->IsReady)
        {
            AtkValue val = default;
            val.SetInt(0);
            addon->FireCallback(1, &val, true);
        }
    }

    public static void TurnInSupply(int slot)
    {
        var agent = AgentSatisfactionSupply.Instance();
        var res = new AtkValue();
        Span<AtkValue> values = stackalloc AtkValue[2];
        values[0].SetInt(1);
        values[1].SetInt(slot);
        agent->ReceiveEvent(&res, values.GetPointer(0), 2, 0);
    }
}
