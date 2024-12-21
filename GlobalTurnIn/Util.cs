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
using Lumina.Excel.Sheets;
using ECommons;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using ECommons.DalamudServices.Legacy;
using System.Runtime.InteropServices;


namespace GlobalTurnIn;

public static unsafe class Util
{
    public static string icurrentTask = "";
    public static void UpdateCurrentTask(string task)
    {
        icurrentTask = task;
    }

    internal static bool TryGetObjectByDataId(ulong dataId, out IGameObject? gameObject) => (gameObject = Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(x => x.DataId == dataId)) != null;
    internal static unsafe void InteractWithObject(IGameObject? gameObject)
    {
        try
        {
            if (gameObject == null || !gameObject.IsTargetable)
                return;
            var gameObjectPointer = (FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)gameObject.Address;
            TargetSystem.Instance()->InteractWithObject(gameObjectPointer, false);
        }
        catch (Exception ex)
        {
            Svc.Log.Info($"InteractWithObject: Exception: {ex}");
        }
    }

    internal static bool? TargetByID(IGameObject? gameObject)
    {
        if (!IsOccupied())
        {
            var x = gameObject;
            if (x != null)
            {
                Svc.Targets.SetTarget(x);
                ECommons.Logging.PluginLog.Information($"Setting the target to {x.DataId}");
                return true;
            }
        }
        return false;
    }

    public static unsafe bool CorrectDuty() // first actual function I made that returns a true/false statement in C#... man this was a pain to learn about xD(ice)
    {
        if (TryGetAddonByName<AtkUnitBase>("ContentsFinder", out var addon) && IsAddonReady(addon))
        {
            //var mainAddon = ((AddonContentsFinder*)addon)->SelectedDutyTextNodeSpan[0].Value->NodeText.ToString();
            var mainAddon = ((AddonContentsFinder*)addon) ->SelectedDutyTextNode[0].Value->NodeText.ToString();
            var AlexText = "Alexander - The Burden of the Father";
            return mainAddon == AlexText;
        }
        return false;
    }
    public static unsafe uint GetGil() => InventoryManager.Instance()->GetGil();
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
    internal static bool GenericThrottle => FrameThrottler.Throttle("GlobalTurnInGenericThrottle", 20);
    public static uint CurrentTerritory() => Svc.ClientState.TerritoryType;
    public static bool IsInZone(int zoneID) => Svc.ClientState.TerritoryType == zoneID;
    public static bool IsAddonActive(string AddonName) // bunu kullan
    {
        var addon = RaptureAtkUnitManager.Instance()->GetAddonByName(AddonName);
        return addon != null && addon->IsVisible && addon->IsReady;
    }
    public static void UpdateDict()
    {
        for (var i = 0; i < RaidItemIDs.Length; i++)
        {
            var itemID = RaidItemIDs[i];
            VendorSellDict[itemID].CurrentItemCount = GetItemCount(itemID);
        }
    }
}
