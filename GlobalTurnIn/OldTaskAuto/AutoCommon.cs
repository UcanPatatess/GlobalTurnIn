using ECommons.DalamudServices;
using System.Threading.Tasks;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using ECommons.GameFunctions;

namespace GlobalTurnIn.TaskAuto;

public abstract class AutoCommon() 
{
    /*
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
    */
}
