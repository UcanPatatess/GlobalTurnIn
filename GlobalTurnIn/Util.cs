using Dalamud.Game.ClientState.Conditions;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Numerics;
using ECommons.DalamudServices;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.GameHelpers;
using ECommons.Reflection;
using Serilog;
using Dalamud.Utility;
using ECommons.Throttlers;


namespace GlobalTurnIn;

public static unsafe class Util
{
    internal static unsafe float GetDistanceToPlayer(Vector3 v3) => Vector3.Distance(v3, Player.GameObject->Position);
    internal static unsafe float GetDistanceToPlayer(IGameObject gameObject) => GetDistanceToPlayer(gameObject.Position);
    internal static IGameObject? GetObjectByName(string name) => Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(o => o.Name.TextValue.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    public static float GetDistanceToPoint(float x, float y, float z) => Vector3.Distance(Svc.ClientState.LocalPlayer?.Position ?? Vector3.Zero, new Vector3(x, y, z));
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
    public static bool DidAmountChange(int arg,int argg)
    {
        if (arg == argg)
            return false;
        else
           return true;
    }
    public static int GetFreeSlotsInContainer(int container)
    {
        var inv = InventoryManager.Instance();
        var cont = inv->GetInventoryContainer((InventoryType)container);
        var slots = 0;
        for (var i = 0; i < cont->Size; i++)
            if (cont->Items[i].ItemId == 0)
                slots++;
        return slots;
    }
    public static float GetPlayerRawXPos()
    {
        return Svc.ClientState.LocalPlayer!.Position.X;
    }
    public static float GetPlayerRawYPos()
    {
        return Svc.ClientState.LocalPlayer!.Position.Y;
    }
    public static float GetPlayerRawZPos()
    {
        return Svc.ClientState.LocalPlayer!.Position.Z;
    }
    public static byte GetPlayerGC() => UIState.Instance()->PlayerState.GrandCompany;
    public static bool PlayerNotBusy()
    {
        return Player.Available
               && Player.Object.CastActionId == 0
               && !IsOccupied()
               && !Svc.Condition[ConditionFlag.Jumping]
               && Player.Object.IsTargetable;
    }

    public static uint CurrentTerritory() => Svc.ClientState.TerritoryType;
    public static bool IsInZone(int zoneID) => Svc.ClientState.TerritoryType == zoneID;
    public static bool IsAddonActive(string AddonName) // bunu kullan
    {
        var addon = RaptureAtkUnitManager.Instance()->GetAddonByName(AddonName);
        return addon != null && addon->IsVisible && addon->IsReady;
    }
    internal static bool GenericThrottle => FrameThrottler.Throttle("AutoRetainerGenericThrottle", 10);
    public static bool? CloseShop() //dükkanı kapattı biraz daha bakılması lazım
    {
        Log.Debug("CloseShop");
        var agent = AgentShop.Instance();
        if (agent == null || agent->EventReceiver == null)
            return true;
        AtkValue res = default, arg = default;
        var proxy = (ShopEventHandler.AgentProxy*)agent->EventReceiver;
        proxy->Handler->CancelInteraction();
        arg.SetInt(-1);
        agent->ReceiveEvent(&res, &arg, 1, 0);
        return false;
    }
}
