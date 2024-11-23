using ECommons.DalamudServices;
using ECommons.Logging;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ECommons.Automation;
using static FFXIVClientStructs.FFXIV.Client.UI.AddonRelicNoteBook;
using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel.Sheets;

namespace GlobalTurnIn.TaskAuto;

public abstract class AutoCommon() : AutoTask
{
    protected async Task MoveTo(Vector3 dest, float tolerance, bool fly = false)
    {
        using var scope = BeginScope("MoveTo");
        if (PlayerInRange(dest, tolerance))
            return; // already in range

        // ensure navmesh is ready
        await WaitWhile(() => Plugin.navmesh.BuildProgress() >= 0, "BuildMesh");
        ErrorIf(!Plugin.navmesh.IsReady(), "Failed to build navmesh for the zone");
        ErrorIf(!Plugin.navmesh.PathfindAndMoveTo(dest, fly), "Failed to start pathfinding to destination");
        using var stop = new OnDispose(Plugin.navmesh.Stop);
        await WaitWhile(() => !PlayerInRange(dest, tolerance), "Navigate");
    }
    protected async Task TeleportTo(uint territoryId, Vector3 destination)
    {
        using var scope = BeginScope("Teleport");
        if (CurrentTerritory() == territoryId)
            return; // already in correct zone

        var closestAetheryteId = Map.FindClosestAetheryte(territoryId, destination);
        var teleportAetheryteId = Map.FindPrimaryAetheryte(closestAetheryteId);
        ErrorIf(teleportAetheryteId == 0, $"Failed to find aetheryte in {territoryId}");
        if (CurrentTerritory() != LuminaRow<Lumina.Excel.Sheets.Aetheryte>(teleportAetheryteId)!.Value.Territory.RowId)
        {
            ErrorIf(!ExecuteTeleport(teleportAetheryteId), $"Failed to teleport to {teleportAetheryteId}");
            await WaitWhile(() => !PlayerIsBusy(), "TeleportStart");
            await WaitWhile(PlayerIsBusy, "TeleportFinish");
        }
        /*
        if (teleportAetheryteId != closestAetheryteId)
        {
            var (aetheryteId, aetherytePos) = Game.FindAetheryte(teleportAetheryteId);
            await MoveTo(aetherytePos, 10);
            ErrorIf(!Game.InteractWith(aetheryteId), "Failed to interact with aetheryte");
            await WaitUntilSkipTalk(Game.IsSelectStringAddonActive, "WaitSelectAethernet");
            Game.TeleportToAethernet(teleportAetheryteId, closestAetheryteId);
            await WaitWhile(() => !Game.PlayerIsBusy(), "TeleportAethernetStart");
            await WaitWhile(Game.PlayerIsBusy, "TeleportAethernetFinish");
        }
        */
        ErrorIf(CurrentTerritory() != territoryId, "Failed to teleport to expected zone");
    }
    protected async Task WaitUntilSkipTalk(Func<bool> condition, string scopeName)
    {
        using var scope = BeginScope(scopeName);
        while (!condition())
        {
            if (IsTalkInProgress())
            {
                Log("progressing talk...");
                ProgressTalk();
            }
            Log("waiting...");
            await NextFrame();
        }
    }
    protected async Task AethernetSwitch(string aethername,uint territoryId)
    {
        if (CurrentTerritory() == territoryId)
            return; // already in correct zone
        using var scope = BeginScope("Aether Teleport");
        ErrorIf(!Plugin.lifestream.AethernetTeleport(aethername), $"Failed to teleport to {aethername}");
        await WaitWhile(() => !PlayerIsBusy(), "TeleportStart");
        await WaitWhile(PlayerIsBusy, "TeleportFinish");
    }
    protected async Task TargetAndInteract(string targetstringName)
    {
        using var scope = BeginScope("Targeting and interacting with "+ targetstringName);
        var target = GetObjectByName(targetstringName);
        if (target != null)
        {
            Svc.Targets.Target = target;
            TargetInteract();
            if (IsAddonActive("")||IsAddonActive(""))
            {

            }
            return;
        }
    }
    protected unsafe async Task FireCallback(string AddonName, bool kapkac, params int[] gibeme)
    {
        if (ECommons.GenericHelpers.TryGetAddonByName<AtkUnitBase>(AddonName, out var addon) && ECommons.GenericHelpers.IsAddonReady(addon))
        {
            Callback.Fire(addon, kapkac, gibeme.Cast<object>().ToArray());
            return;
        }
    }
    protected async Task MountUp()
    {
        using var scope = BeginScope("MountUp");
        if (Svc.Data.GetExcelSheet<TerritoryType>()?.GetRow(Player.Territory).Unknown4 != 0)
        {
            if (Svc.Condition[ConditionFlag.Mounted]) return ;

            if (!Svc.Condition[ConditionFlag.Casting] && !Svc.Condition[ConditionFlag.Unknown57])
            {
                unsafe { ActionManager.Instance()->UseAction(ActionType.GeneralAction, 24); }
            }
            await WaitWhile(() => (!Svc.Condition[ConditionFlag.Mounted]), "MountingFinish");
        }
    }
    protected async Task OpenShopMenu(int SelectIconString,int SelectString,string NpcName,string OpenedAddonName= "ShopExchangeItem")
    {

        while (true) 
        {
            var target = GetObjectByName(NpcName);
            if (target != Svc.Targets.Target)
            {
                await TargetAndInteract(NpcName);
            }
            await FireCallback("SelectString", true, SelectString);
            await FireCallback("SelectIconString", true, SelectIconString);
            if (IsAddonActive(OpenedAddonName)) return;
        }
    }
    protected static string ItemName(uint itemId) => LuminaRow<Lumina.Excel.Sheets.Item>(itemId)?.Name.ToString() ?? itemId.ToString();
}
