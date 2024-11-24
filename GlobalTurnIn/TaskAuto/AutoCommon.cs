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
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using ECommons.GameFunctions;

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
    protected async Task WaitUntil(Func<bool> condution,string scopeName)
    {
        using var scope = BeginScope(scopeName);
        while (!condution())
        {
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
    protected async Task TargetName(string targetstringName)
    {
        using var scope = BeginScope("Targeting "+ targetstringName);
        var target = GetObjectByName(targetstringName);
        if (target != null)
        {
            Svc.Targets.Target = target;
            await NextFrame();
            return;
        }
    }
    protected async Task TargetInteract(string OpenedShopAddonName)
    {
        var target = Svc.Targets.Target;
        if (target != default)
        {
            if (IsAddonActive("SelectString") || IsAddonActive("SelectIconString") || IsAddonActive(OpenedShopAddonName))
                return;
            unsafe { TargetSystem.Instance()->InteractWithObject(target.Struct(), false); }
            await WaitUntil(() => IsAddonActive("SelectString") || IsAddonActive("SelectIconString"), "TargetInteractWaiting");
        }
    }
    protected unsafe async Task FireCallback(string AddonName, bool kapkac, params int[] gibeme)
    {
        using var scope = BeginScope("CallbackFired!! " + AddonName+" "+ kapkac+ " "+ gibeme);
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
    //OpenedAddonName= "ShopExchangeItem"
    // konuma göre npc ismi ayarlarsın
    protected async Task AddonCallSelectString(int SelectString)
    {
        using var scope = BeginScope("AddonCallSelectString");
        if (IsAddonActive("SelectString"))
        {
            await FireCallback("SelectString", true, SelectString);
            await WaitWhile(() => IsAddonActive("SelectString"), "WaitAddonSelectStringClosing");
        }
    }
    protected async Task AddonCallSelectIconString(int SelectIconString)
    {
        using var scope = BeginScope("AddonCallSelectIconString");
        if (IsAddonActive("SelectIconString"))
        {
            await FireCallback("SelectIconString", true, SelectIconString);
            await WaitWhile(() => IsAddonActive("SelectIconString"), "WaitAddonSelectIconStringClosing");
        }
    }
    protected async Task OpenShopMenu(int SelectIconString,int SelectString,string OpenedShopAddonName)
    {
        using var scope = BeginScope("OpenShopMenu "+ SelectIconString+" " + SelectString+" " + OpenedShopAddonName);
        if (IsAddonActive(OpenedShopAddonName))
            return;
        string NpcName = string.Empty;
        if (Svc.ClientState.TerritoryType == 478)
        {
            NpcName = "Sabina";
        }
        
        if (Svc.ClientState.TerritoryType == 132) //was for testing
        {
            NpcName = "Maisenta";
        }
        
        if (Svc.ClientState.TerritoryType == 635)
        {
            NpcName = "Gelfradus";
        }
        await TargetName(NpcName);
        await TargetInteract(OpenedShopAddonName);
        await AddonCallSelectString(SelectString);
        await AddonCallSelectIconString(SelectIconString);
    }
    protected async Task Exchange(int List,int Amount)
    {
        int ExpectedItemCount = 0;
        int brakepoint = 0;
        if (Configuration.FillArmory)
            ExpectedItemCount = +Amount;
        else 
        {
            if (Configuration.MaximizeInv)
            {
                ExpectedItemCount = +Amount;
            }
        }
    }
    protected static string ItemName(uint itemId) => LuminaRow<Lumina.Excel.Sheets.Item>(itemId)?.Name.ToString() ?? itemId.ToString();
}
