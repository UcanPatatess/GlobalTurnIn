using ECommons.Automation;
using ECommons.DalamudServices;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal static class TaskTurnin2
    {
        internal unsafe static void Enqueue()
        {
            if (PluginInstalled("Automaton"))
            {
                P.taskManager.Enqueue(() => Chat.Instance.SendMessage("/inventory"));
                P.taskManager.EnqueueDelay(100);
                P.taskManager.Enqueue(() => Chat.Instance.SendMessage("/inventory"));
                P.taskManager.EnqueueDelay(100);
            }
            else
                P.taskManager.Enqueue(() => Svc.Chat.PrintError("Open AutoMerge in Automaton"));


            int? lastShopType = null;
            int? LastIconShopType = null;
            int locationID = 0;

            int[,] TableName = null!;
            if (Svc.ClientState.TerritoryType == 478)
            {
                TableName = SabinaTable;
                locationID = 478;
            }
            if (Svc.ClientState.TerritoryType == 635)
            {
                TableName = GelfradusTable;
                locationID = 635;
            }

            int slotINV = GetInventoryFreeSlotCount(); // gets current inventory free slots
            int lastArmoryType = -1;
            int raidItemPiece = 0;



            for (int i = 0; i < TableName.GetLength(0); i++)
            {
                int shopType = TableName[i, 0]; // initial shop menu
                int itemType = TableName[i, 1]; // ItemID [Raid ID]
                int itemTypeBuy = TableName[i, 2]; // Raid Item Per Buy [
                int gearItem = TableName[i, 3]; // Armory slot
                int pcallValue = TableName[i, 4]; // callback (self explanitory)
                int iconShopType = TableName[i, 5]; // Used in Omega Raids (Sub Sub Menu)


            }
        }
    }
}
