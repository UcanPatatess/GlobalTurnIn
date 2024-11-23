using Dalamud.IoC;
using Dalamud.Plugin.Services;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using SamplePlugin;

namespace GlobalTurnIn
{
    internal class Service
    {
        [PluginService] public static IFramework Framework { get; private set; } = null!;
        [PluginService] public static IPluginLog Log { get; private set; } = null!;
        [PluginService] public static ICondition Conditions { get; private set; } = null!;
        internal static Plugin Plugin { get; set; } = null!;
        internal static GlobalTurnInConfig Configuration { get; set; } = null!;
        public static Lumina.GameData LuminaGameData => Svc.Data.GameData;
        public static Lumina.Excel.ExcelSheet<T>? LuminaSheet<T>() where T : struct, Lumina.Excel.IExcelRow<T> => LuminaGameData?.GetExcelSheet<T>(Lumina.Data.Language.English);
        public static Lumina.Excel.SubrowExcelSheet<T>? LuminaSheetSubrow<T>() where T : struct, Lumina.Excel.IExcelSubrow<T> => LuminaGameData?.GetSubrowExcelSheet<T>(Lumina.Data.Language.English);
        public static T? LuminaRow<T>(uint row) where T : struct, Lumina.Excel.IExcelRow<T> => LuminaSheet<T>()?.GetRowOrDefault(row);
        public static Lumina.Excel.SubrowCollection<T>? LuminaSubrows<T>(uint row) where T : struct, Lumina.Excel.IExcelSubrow<T> => LuminaSheetSubrow<T>()?.GetRowOrDefault(row);
        public static T? LuminaRow<T>(uint row, ushort subRow) where T : struct, Lumina.Excel.IExcelSubrow<T> => LuminaSheetSubrow<T>()?.GetSubrowOrDefault(row, subRow);
    }
}
