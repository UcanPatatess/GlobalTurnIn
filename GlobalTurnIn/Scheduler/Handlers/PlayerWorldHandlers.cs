using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using ECommons.GameFunctions;
using Dalamud.Game.ClientState.Conditions;
using FFXIVClientStructs.FFXIV.Client.Game;
using ECommons.Automation;
using GlobalTurnIn.Scheduler.Tasks;

namespace GlobalTurnIn.Scheduler.Handlers
{
    internal static unsafe class PlayerWorldHandlers
    {
        internal static bool? InteractShopNpc()
        {
            var OpenedShopAddonName = "ShopExchangeItem";
            var target = Svc.Targets.Target;
            if (target != default)
            {
                if (IsAddonActive("SelectString") || IsAddonActive("SelectIconString") || IsAddonActive(OpenedShopAddonName))
                    return true;
                unsafe { TargetSystem.Instance()->InteractWithObject(target.Struct(), false); }
            }
            return false;
        }
        internal static bool? TargetShopNpc()
        {
            string NpcName = string.Empty;
            if (Svc.ClientState.TerritoryType == 478) //Idyllshire
                NpcName = "Sabina";

            if (Svc.ClientState.TerritoryType == 635)//Rhalgr
                NpcName = "Gelfradus";

            var target = GetObjectByName(NpcName);
            if (target != null)
            {
                Svc.Targets.Target = target;
                return true;
            }
            return false;
        }
        internal static bool? MountUp()
        {
            if (Svc.Condition[ConditionFlag.Mounted]) return true;

            if (!Svc.Condition[ConditionFlag.Casting] && !Svc.Condition[ConditionFlag.Unknown57])
            {
                unsafe { ActionManager.Instance()->UseAction(ActionType.GeneralAction, 24); }
            }
            return false;
        }
    }
}
