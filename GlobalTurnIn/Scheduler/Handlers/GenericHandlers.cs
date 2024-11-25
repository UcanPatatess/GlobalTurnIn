using ECommons.Automation;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GlobalTurnIn.Scheduler.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Handlers
{
    internal class GenericHandlers
    {
        internal static bool? Throttle(int ms)
        {
            return EzThrottler.Throttle("GlobalTurnInWait", ms);
        }
        internal static bool? WaitFor(int ms)
        {
            return EzThrottler.Check("GlobalTurnInWait");
        }

        internal unsafe static bool? OpenCharaSettings()
        {
            if (!IsOccupied())
            {
                var addon = RaptureAtkUnitManager.Instance()->GetAddonByName("ConfigCharacter");
                if (addon != null && addon->IsVisible && addon->IsReady)
                    return true;

                if (EzThrottler.Throttle("ConfigCharacterWait", 100))
                Chat.Instance.SendMessage("/characterconfig");
            } 
            return false;
        }
        public unsafe static bool? FireCallback(string AddonName, bool kapkac, params int[] gibeme)
        {
            if (ECommons.GenericHelpers.TryGetAddonByName<AtkUnitBase>(AddonName, out var addon) && ECommons.GenericHelpers.IsAddonReady(addon))
            {
                    Callback.Fire(addon, kapkac, gibeme.Cast<object>().ToArray());
                return true;
            }
            return false;
        }
    }
}
