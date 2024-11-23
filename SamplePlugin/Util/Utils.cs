using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using ECommons.Reflection;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin.Util;

public static class Utils
{
    public static float SprintCD => Player.Status.FirstOrDefault(s => s.StatusId == 50)?.RemainingTime ?? 0;
    public static bool HasPlugin(string name) => DalamudReflector.TryGetDalamudPlugin(name, out _, false, true);
    internal static unsafe float GetDistanceToPlayer(IGameObject gameObject) => GetDistanceToPlayer(gameObject.Position);
    internal static unsafe float GetDistanceToPlayer(Vector3 v3) => Vector3.Distance(v3, Player.GameObject->Position);
    internal static IGameObject? GetObjectByName(string name) => Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(o => o.Name.TextValue.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    public static int GetZoneID() => Svc.ClientState.TerritoryType;
    public static string GetTargetName() => Svc.Targets.Target?.Name.TextValue ?? "";
    public static unsafe int GetItemCount(int itemID, bool includeHQ = true)
       => includeHQ ? InventoryManager.Instance()->GetInventoryItemCount((uint)itemID, true) + InventoryManager.Instance()->GetInventoryItemCount((uint)itemID) + InventoryManager.Instance()->GetInventoryItemCount((uint)itemID + 500_000)
       : InventoryManager.Instance()->GetInventoryItemCount((uint)itemID) + InventoryManager.Instance()->GetInventoryItemCount((uint)itemID + 500_000);
    public static bool PluginInstalled(string name)
    {
        return DalamudReflector.TryGetDalamudPlugin(name, out _, false, true);
    }
    public static string PlayerXYZ()
        {
        if (Svc.ClientState.LocalPlayer != null)
        {
            var PlayerX = Svc.ClientState.LocalPlayer.Position.X;
            var PlayerY = Svc.ClientState.LocalPlayer.Position.Y;
            var PlayerZ = Svc.ClientState.LocalPlayer.Position.Z;

            var PlayerXYZ = PlayerX.ToString() + " " + PlayerY.ToString() + " " + PlayerZ.ToString();

            return PlayerXYZ.ToString();
        }
        else
        {
            return "Player is null";
        }
    }
    public static bool PlayerNotBusy()
    {
        return Player.Available
               && Player.Object.CastActionId == 0
               && !IsOccupied()
               && !Svc.Condition[ConditionFlag.Jumping]
               && Player.Object.IsTargetable;
    }
    public static unsafe bool IsMoving()
    {
        return AgentMap.Instance()->IsPlayerMoving == 1;
    }

    public static float Distance(this Vector3 v, Vector3 v2)
    {
        return new Vector2(v.X - v2.X, v.Z - v2.Z).Length();
    }
}
