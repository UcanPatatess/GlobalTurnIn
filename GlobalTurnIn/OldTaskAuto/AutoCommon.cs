using ECommons.DalamudServices;
using System.Threading.Tasks;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using ECommons.GameFunctions;

namespace GlobalTurnIn.TaskAuto;

public abstract class AutoCommon() 
{
    /*
    
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
