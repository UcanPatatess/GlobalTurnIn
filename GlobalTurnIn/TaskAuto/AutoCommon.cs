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
    protected async Task TargetName()
    {
        string NpcName = string.Empty;
        if (Svc.ClientState.TerritoryType == 478) //Idyllshire
            NpcName = "Sabina";

        if (Svc.ClientState.TerritoryType == 635)//Rhalgr
            NpcName = "Gelfradus";

        using var scope = BeginScope("Targeting "+ NpcName);
        var target = GetObjectByName(NpcName);
        if (target != null)
        {
            Svc.Targets.Target = target;
            await NextFrame();
            return;
        }
    }
    protected async Task TargetInteract()
    {
        var OpenedShopAddonName = "ShopExchangeItem";
        var target = Svc.Targets.Target;
        if (target != default)
        {
            if (IsAddonActive("SelectString") || IsAddonActive("SelectIconString") || IsAddonActive(OpenedShopAddonName))
                return;
            unsafe { TargetSystem.Instance()->InteractWithObject(target.Struct(), false); }
            await WaitUntil(() => IsAddonActive("SelectString") || IsAddonActive("SelectIconString"), "TargetInteractWaiting");
        }
    }

    protected async Task MountUp()
    {
        await NextFrame(100);
        using var scope = BeginScope("MountUp");
        if (Svc.Condition[ConditionFlag.Mounted]) return;

        if (!Svc.Condition[ConditionFlag.Casting] && !Svc.Condition[ConditionFlag.Unknown57])
        {
            unsafe { ActionManager.Instance()->UseAction(ActionType.GeneralAction, 24); }
        }
        await WaitWhile(() => (!Svc.Condition[ConditionFlag.Mounted]), "MountingFinish");
        await WaitWhile(() => PlayerIsBusy(), "Waiting Player");
    }
    //OpenedAddonName= "ShopExchangeItem"
    // konuma göre npc ismi ayarlarsın
    protected async Task AddonCallSelectString(int SelectString)
    {
        using var scope = BeginScope("AddonCallSelectString");
        await WaitWhile(() => !IsAddonActive("SelectString"), "WaitAddonSelectStringClosing");
        FireCallback("SelectString", true, SelectString);
        await WaitWhile(() => IsAddonActive("SelectString"), "WaitAddonSelectStringClosing");
    }
    protected async Task AddonCallSelectIconString(int SelectIconString)
    {
        using var scope = BeginScope("AddonCallSelectIconString");
        FireCallback("SelectIconString", true, SelectIconString);
        await WaitWhile(() => IsAddonActive("SelectIconString"), "WaitAddonSelectIconStringClosing");
        
    }
    protected async Task ChangeArmorySetting(bool arg)
    {
        using var scope = BeginScope("Changed Armorry Setting to: "+ (arg ? "Enabled" : "Disabled"));
        if (!IsAddonActive("ConfigCharacter"))
            Chat.Instance.SendMessage("/characterconfig");

        await WaitUntil(() => IsAddonActive("ConfigCharacter"),"CharaConfigWait");
        FireCallback("ConfigCharacter", true,10,0,20);
        await NextFrame();
        FireCallback("ConfigCharacter", true,18,300, arg ? 1 : 0);
        await NextFrame();
        FireCallback("ConfigCharacter", true, 0);
        await NextFrame();
        FireCallback("ConfigCharacter", true, -1);
        await WaitWhile(()=>IsAddonActive("ConfigCharacter"), "CharaConfigEndWait");
        await WaitWhile(()=>PlayerIsBusy(),"Waiting Player");

    }
    protected async Task SellVendor()
    {
        Svc.Commands.ProcessCommand("/ays itemsell");
        await WaitWhile(() => Plugin.autoRetainer.IsBusy(), "AutoReatinerBusy");
        await WaitWhile(() => PlayerIsBusy(),"PlayerBusy");
    }
    protected async Task OpenShopMenu(int SelectIconString,int SelectString)
    {
        using var scope = BeginScope("OpenShopMenu "+ SelectIconString+" " + SelectString+" " + "ShopExchangeItem");

        await TargetName();
        await TargetInteract();
        await AddonCallSelectIconString(SelectIconString);
        await AddonCallSelectString(SelectString);
        await WaitUntil(() => IsAddonActive("ShopExchangeItem"), "ShopWait");
    }
    protected async Task CloseSelectString()
    {
        await WaitUntil(()=>IsAddonActive("SelectString"), "CloseSelectstring");
        FireCallback("SelectString", true, -1);
        await WaitWhile(() => PlayerIsBusy(),"PlayerBusy");
    }
    protected async Task Exchange(int currentgearamount,int gearid,int List,int Amount)
    {
        if (!IsAddonActive("ShopExchangeItem"))
            return;

        if (Amount >127)
            Amount =127;

        FireCallback("ShopExchangeItem", true, 0, List, Amount);
        await WaitUntil(() => IsAddonActive("ShopExchangeItemDialog"), "ShopExchangeItemDialogWait");
        FireCallback("ShopExchangeItemDialog", true, 0);
        await WaitUntil(() => IsAddonActive("Request"), "Pandora Should Handle it");
        await WaitUntil(() => DidAmountChange(currentgearamount, GetItemCount(gearid)), "");
        await NextFrame(10);
    }
}
