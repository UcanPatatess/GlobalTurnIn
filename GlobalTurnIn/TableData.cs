
namespace GlobalTurnIn;

public static class Data
{
    public static int TotalExchangeItem = 0;
    public static int GordianTurnInCount = 0;
    public static int AlexandrianTurnInCount = 0;
    public static int DeltascapeTurnInCount = 0;




    // Deltascape Item IDs
    private static int DeltascapeLensID = 19111;
    private static int DeltascapeShaftID = 19112;
    private static int DeltascapeCrankID = 19113;
    private static int DeltascapeSpringID = 19114;
    private static int DeltascapePedalID = 19115;
    private static int DeltascapeBoltID = 19117;

    // Tarnished Gordian Item IDs
    private static int GordianLensID = 12674;
    private static int GordianShaftID = 12675;
    private static int GordianCrankID = 12676;
    private static int GordianSpringID = 12677;
    private static int GordianPedalID = 12678;
    private static int GordianBoltID = 12680;

    // Alexandrian Item IDs
    private static int AlexandrianLensID = 16546;
    private static int AlexandrianShaftID = 16547;
    private static int AlexandrianCrankID = 16548;
    private static int AlexandrianSpringID = 16549;
    private static int AlexandrianPedalID = 16550;
    private static int AlexandrianBoltID = 16552;

    private static int BoltBuyAmount = 1;
    private static int PedalBuyAmount = 2;
    private static int SpringBuyAmount = 4;
    private static int CrankBuyAmount = 2;
    private static int ShaftBuyAmount = 4;
    private static int LensBuyAmount = 2;

    public static bool IsThereTradeItem()
    {
        TotalExchangeItem = 0;
        for (int i = 0; i < SabinaTable.GetLength(0); i++)
        {
            int itemID = SabinaTable[i, 3];
            int count = GetItemCount(itemID);
            TotalExchangeItem += count;
        }

        // Add up counts from GelfradusTable
        for (int i = 0; i < GelfradusTable.GetLength(0); i++)
        {
            int itemID = GelfradusTable[i, 3];
            int count = GetItemCount(itemID);
            TotalExchangeItem += count;
        }
        // GORDIAN
        int GordianLensCount = GetItemCount(GordianLensID);
        int GordianShaftCount = GetItemCount(GordianShaftID);
        int GordianCrankCount = GetItemCount(GordianCrankID);
        int GordianSpringCount = GetItemCount(GordianSpringID);
        int GordianPedalCount = GetItemCount(GordianPedalID);
        int GordianBoltCount = GetItemCount(GordianBoltID);

        GordianTurnInCount =
            (int)Math.Floor((double)GordianLensCount / LensBuyAmount) +
            (int)Math.Floor((double)GordianShaftCount / ShaftBuyAmount) +
            (int)Math.Floor((double)GordianCrankCount / CrankBuyAmount) +
            (int)Math.Floor((double)GordianSpringCount / SpringBuyAmount) +
            (int)Math.Floor((double)GordianPedalCount / PedalBuyAmount) +
            (int)Math.Floor((double)GordianBoltCount / BoltBuyAmount);

        // ALEXANDRIAN
        int AlexandrianLensCount = GetItemCount(AlexandrianLensID);
        int AlexandrianShaftCount = GetItemCount(AlexandrianShaftID);
        int AlexandrianCrankCount = GetItemCount(AlexandrianCrankID);
        int AlexandrianSpringCount = GetItemCount(AlexandrianSpringID);
        int AlexandrianPedalCount = GetItemCount(AlexandrianPedalID);
        int AlexandrianBoltCount = GetItemCount(AlexandrianBoltID);

        AlexandrianTurnInCount =
            (int)Math.Floor((double)AlexandrianLensCount / LensBuyAmount) +
            (int)Math.Floor((double)AlexandrianShaftCount / ShaftBuyAmount) +
            (int)Math.Floor((double)AlexandrianCrankCount / CrankBuyAmount) +
            (int)Math.Floor((double)AlexandrianSpringCount / SpringBuyAmount) +
            (int)Math.Floor((double)AlexandrianPedalCount / PedalBuyAmount) +
            (int)Math.Floor((double)AlexandrianBoltCount / BoltBuyAmount);

        // DELTASCAPE
        int DeltascapeLensCount = GetItemCount(DeltascapeLensID);
        int DeltascapeShaftCount = GetItemCount(DeltascapeShaftID);
        int DeltascapeCrankCount = GetItemCount(DeltascapeCrankID);
        int DeltascapeSpringCount = GetItemCount(DeltascapeSpringID);
        int DeltascapePedalCount = GetItemCount(DeltascapePedalID);
        int DeltascapeBoltCount = GetItemCount(DeltascapeBoltID);

        DeltascapeTurnInCount =
            (int)Math.Floor((double)DeltascapeLensCount / LensBuyAmount) +
            (int)Math.Floor((double)DeltascapeShaftCount / ShaftBuyAmount) +
            (int)Math.Floor((double)DeltascapeCrankCount / CrankBuyAmount) +
            (int)Math.Floor((double)DeltascapeSpringCount / SpringBuyAmount) +
            (int)Math.Floor((double)DeltascapePedalCount / PedalBuyAmount) +
            (int)Math.Floor((double)DeltascapeBoltCount / BoltBuyAmount);

        // Final Decision
        return !(GordianTurnInCount < 1 && DeltascapeTurnInCount < 1 && AlexandrianTurnInCount < 1);
    }
    public static Dictionary<int, int> ItemIdArmoryTable { get; } = new Dictionary<int, int>
    {
        // ArmoryHead = 3201
        // Deltascape
        { 19437, 3201 }, { 19443, 3201 }, { 19449, 3201 }, { 19461, 3201 }, { 19455, 3201 },
        { 19467, 3201 }, { 19473, 3201 },
        // Gordian
        { 11450, 3201 }, { 11449, 3201 }, { 11448, 3201 }, { 11451, 3201 }, { 11452, 3201 },
        { 11453, 3201 }, { 11454, 3201 },
        // MIDAN Alexandrian
        { 16439, 3201 }, { 16433, 3201 }, { 16415, 3201 }, { 16409, 3201 }, { 16403, 3201 },
        { 16421, 3201 }, { 16427, 3201 },

        // ArmoryBody = 3202
        // Deltascape
        { 19474, 3202 }, { 19468, 3202 }, { 19462, 3202 }, { 19456, 3202 }, { 19438, 3202 },
        { 19444, 3202 }, { 19450, 3202 },
        // Gordian
        { 11461, 3202 }, { 11460, 3202 }, { 11459, 3202 }, { 11458, 3202 }, { 11455, 3202 },
        { 11456, 3202 }, { 11457, 3202 },
        // MIDAN Alexandrian
        { 16440, 3202 }, { 16434, 3202 }, { 16428, 3202 }, { 16422, 3202 }, { 16404, 3202 },
        { 16410, 3202 }, { 16416, 3202 },

        // ArmoryHands = 3203
        // Deltascape
        { 19475, 3203 }, { 19469, 3203 }, { 19463, 3203 }, { 19457, 3203 }, { 19439, 3203 },
        { 19445, 3203 }, { 19451, 3203 },
        // Gordian
        { 11468, 3203 }, { 11467, 3203 }, { 11466, 3203 }, { 11465, 3203 }, { 11462, 3203 },
        { 11463, 3203 }, { 11464, 3203 },
        // MIDAN Alexandrian
        { 16441, 3203 }, { 16435, 3203 }, { 16429, 3203 }, { 16423, 3203 }, { 16405, 3203 },
        { 16411, 3203 }, { 16417, 3203 },

        // ArmoryLegs = 3205
        // Deltascape
        { 19476, 3205 }, { 19470, 3205 }, { 19464, 3205 }, { 19458, 3205 }, { 19440, 3205 },
        { 19446, 3205 }, { 19452, 3205 },
        // Gordian
        { 11482, 3205 }, { 11481, 3205 }, { 11480, 3205 }, { 11479, 3205 }, { 11476, 3205 },
        { 11477, 3205 }, { 11478, 3205 },
        // MIDAN Alexandrian
        { 16442, 3205 }, { 16436, 3205 }, { 16430, 3205 }, { 16424, 3205 }, { 16406, 3205 },
        { 16412, 3205 }, { 16418, 3205 },

        // ArmoryFeets = 3206
        // Deltascape
        { 19477, 3206 }, { 19471, 3206 }, { 19465, 3206 }, { 19459, 3206 }, { 19441, 3206 },
        { 19447, 3206 }, { 19453, 3206 },
        // Gordian
        { 11489, 3206 }, { 11488, 3206 }, { 11487, 3206 }, { 11486, 3206 }, { 11483, 3206 },
        { 11484, 3206 }, { 11485, 3206 },
        // MIDAN Alexandrian
        { 16443, 3206 }, { 16437, 3206 }, { 16431, 3206 }, { 16425, 3206 }, { 16407, 3206 },
        { 16413, 3206 }, { 16419, 3206 },

        // ArmoryEar = 3207
        // Deltascape
        { 19479, 3207 }, { 19480, 3207 }, { 19481, 3207 }, { 19483, 3207 }, { 19482, 3207 },
        // Gordian
        { 11490, 3207 }, { 11491, 3207 }, { 11492, 3207 }, { 11494, 3207 }, { 11493, 3207 },
        // MIDAN Alexandrian
        { 16449, 3207 }, { 16448, 3207 }, { 16447, 3207 }, { 16445, 3207 }, { 16446, 3207 },

        // ArmoryNeck = 3208
        // Deltascape
        { 19484, 3208 }, { 19485, 3208 }, { 19486, 3208 }, { 19488, 3208 }, { 19487, 3208 },
        // Gordian
        { 11495, 3208 }, { 11496, 3208 }, { 11497, 3208 }, { 11499, 3208 }, { 11498, 3208 },
        // MIDAN Alexandrian
        { 16450, 3208 }, { 16451, 3208 }, { 16452, 3208 }, { 16454, 3208 }, { 16453, 3208 },

        // ArmoryWrist = 3209
        // Deltascape
        { 19489, 3209 }, { 19490, 3209 }, { 19491, 3209 }, { 19493, 3209 }, { 19492, 3209 },
        // Gordian
        { 11500, 3209 }, { 11501, 3209 }, { 11502, 3209 }, { 11504, 3209 }, { 11503, 3209 },
        // MIDAN Alexandrian
        { 16459, 3209 }, { 16458, 3209 }, { 16457, 3209 }, { 16455, 3209 }, { 16456, 3209 },

        // ArmoryRings = 3300
        // Deltascape
        { 19494, 3300 }, { 19495, 3300 }, { 19496, 3300 }, { 19498, 3300 }, { 19497, 3300 },
        // Gordian
        { 11509, 3300 }, { 11508, 3300 }, { 11507, 3300 }, { 11505, 3300 }, { 11506, 3300 },
        // MIDAN Alexandrian
        { 16464, 3300 }, { 16463, 3300 }, { 16462, 3300 }, { 16460, 3300 }, { 16461, 3300 }
    };
    /*
    Example usage
    int itemId = 19437;
    if (ItemIdArmoryTable.TryGetValue(itemId, out int category))
    {
        Console.WriteLine($"Item ID {itemId} belongs to category {category}.");
    }
    else
    {
        Console.WriteLine("Item ID not found.");
    }
    */
    public static int[,] GelfradusTable = new int[,]
    {
        {0, DeltascapeBoltID, BoltBuyAmount, 19495, 22, 0},
        {0, DeltascapeBoltID, BoltBuyAmount, 19494, 21, 0},
        {0, DeltascapeBoltID, BoltBuyAmount, 19490, 20, 0},
        {0, DeltascapeBoltID, BoltBuyAmount, 19489, 19, 0},
        {0, DeltascapeBoltID, BoltBuyAmount, 19485, 18, 0},
        {0, DeltascapeBoltID, BoltBuyAmount, 19484, 17, 0},
        {0, DeltascapeBoltID, BoltBuyAmount, 19480, 16, 0},
        {0, DeltascapeBoltID, BoltBuyAmount, 19479, 15, 0},
        {0, DeltascapePedalID, PedalBuyAmount, 19453, 14, 0},
        {0, DeltascapePedalID, PedalBuyAmount, 19447, 13, 0},
        {0, DeltascapePedalID, PedalBuyAmount, 19441, 12, 0},
        {0, DeltascapeSpringID, SpringBuyAmount, 19452, 11, 0},
        {0, DeltascapeSpringID, SpringBuyAmount, 19446, 10, 0},
        {0, DeltascapeSpringID, SpringBuyAmount, 19440, 9, 0},
        {0, DeltascapeCrankID, CrankBuyAmount, 19451, 8, 0},
        {0, DeltascapeCrankID, CrankBuyAmount, 19445, 7, 0},
        {0, DeltascapeCrankID, CrankBuyAmount, 19439, 6, 0},
        {0, DeltascapeShaftID, ShaftBuyAmount, 19450, 5, 0},
        {0, DeltascapeShaftID, ShaftBuyAmount, 19444, 4, 0},
        {0, DeltascapeShaftID, ShaftBuyAmount, 19438, 3, 0},
        {0, DeltascapeLensID, LensBuyAmount, 19449, 2, 0},
        {0, DeltascapeLensID, LensBuyAmount, 19443, 1, 0},
        {0, DeltascapeLensID, LensBuyAmount, 19437, 0, 0},

        {1, DeltascapeBoltID, BoltBuyAmount, 19496, 13, 1},
        {1, DeltascapeBoltID, BoltBuyAmount, 19491, 12, 1},
        {1, DeltascapeBoltID, BoltBuyAmount, 19486, 11, 1},
        {1, DeltascapeBoltID, BoltBuyAmount, 19481, 10, 1},
        {1, DeltascapePedalID, PedalBuyAmount, 19459, 9, 1},
        {1, DeltascapePedalID, PedalBuyAmount, 19465, 8, 1},
        {1, DeltascapeSpringID, SpringBuyAmount, 19458, 7, 1},
        {1, DeltascapeSpringID, SpringBuyAmount, 19464, 6, 1},
        {1, DeltascapeCrankID, CrankBuyAmount, 19457, 5, 1},
        {1, DeltascapeCrankID, CrankBuyAmount, 19463, 4, 1},
        {1, DeltascapeShaftID, ShaftBuyAmount, 19456, 3, 1},
        {1, DeltascapeShaftID, ShaftBuyAmount, 19462, 2, 1},
        {1, DeltascapeLensID, LensBuyAmount, 19455, 1, 1},
        {1, DeltascapeLensID, LensBuyAmount, 19461, 0, 1},

        {2, DeltascapeBoltID, BoltBuyAmount, 19497, 17, 2},
        {2, DeltascapeBoltID, BoltBuyAmount, 19498, 16, 2},
        {2, DeltascapeBoltID, BoltBuyAmount, 19492, 15, 2},
        {2, DeltascapeBoltID, BoltBuyAmount, 19493, 14, 2},
        {2, DeltascapeBoltID, BoltBuyAmount, 19487, 13, 2},
        {2, DeltascapeBoltID, BoltBuyAmount, 19488, 12, 2},
        {2, DeltascapeBoltID, BoltBuyAmount, 19482, 11, 2},
        {2, DeltascapeBoltID, BoltBuyAmount, 19483, 10, 2},
        {2, DeltascapePedalID, PedalBuyAmount, 19471, 9, 2},
        {2, DeltascapePedalID, PedalBuyAmount, 19477, 8, 2},
        {2, DeltascapeSpringID, SpringBuyAmount, 19470, 7, 2},
        {2, DeltascapeSpringID, SpringBuyAmount, 19476, 6, 2},
        {2, DeltascapeCrankID, CrankBuyAmount, 19469, 5, 2},
        {2, DeltascapeCrankID, CrankBuyAmount, 19475, 4, 2},
        {2, DeltascapeShaftID, ShaftBuyAmount, 19468, 3, 2},
        {2, DeltascapeShaftID, ShaftBuyAmount, 19474, 2, 2},
        {2, DeltascapeLensID, LensBuyAmount, 19467, 1, 2},
        {2, DeltascapeLensID, LensBuyAmount, 19473, 0, 2}
    };
    public static int[,] SabinaTable = new int[,]
    {
        {0, GordianBoltID, BoltBuyAmount, 11506, 22, 0},
        {0, GordianBoltID, BoltBuyAmount, 11505, 21, 0},
        {0, GordianBoltID, BoltBuyAmount, 11501, 20, 0},
        {0, GordianBoltID, BoltBuyAmount, 11500, 19, 0},
        {0, GordianBoltID, BoltBuyAmount, 11496, 18, 0},
        {0, GordianBoltID, BoltBuyAmount, 11495, 17, 0},
        {0, GordianBoltID, BoltBuyAmount, 11491, 16, 0},
        {0, GordianBoltID, BoltBuyAmount, 11490, 15, 0},
        {0, GordianPedalID, PedalBuyAmount, 11485, 14, 0},
        {0, GordianPedalID, PedalBuyAmount, 11484, 13, 0},
        {0, GordianPedalID, PedalBuyAmount, 11483, 12, 0},
        {0, GordianSpringID, SpringBuyAmount, 11478, 11, 0},
        {0, GordianSpringID, SpringBuyAmount, 11477, 10, 0},
        {0, GordianSpringID, SpringBuyAmount, 11476, 9, 0},
        {0, GordianCrankID, CrankBuyAmount, 11464, 8, 0},
        {0, GordianCrankID, CrankBuyAmount, 11463, 7, 0},
        {0, GordianCrankID, CrankBuyAmount, 11462, 6, 0},
        {0, GordianShaftID, ShaftBuyAmount, 11457, 5, 0},
        {0, GordianShaftID, ShaftBuyAmount, 11456, 4, 0},
        {0, GordianShaftID, ShaftBuyAmount, 11455, 3, 0},
        {0, GordianLensID, LensBuyAmount, 11450, 2, 0},
        {0, GordianLensID, LensBuyAmount, 11449, 1, 0},
        {0, GordianLensID, LensBuyAmount, 11448, 0, 0},

        {1, GordianBoltID, BoltBuyAmount, 11507, 13, 0},
        {1, GordianBoltID, BoltBuyAmount, 11502, 12, 0},
        {1, GordianBoltID, BoltBuyAmount, 11497, 11, 0},
        {1, GordianBoltID, BoltBuyAmount, 11492, 10, 0},
        {1, GordianPedalID, PedalBuyAmount, 11486, 9, 0},
        {1, GordianPedalID, PedalBuyAmount, 11487, 8, 0},
        {1, GordianSpringID, SpringBuyAmount, 11479, 7, 0},
        {1, GordianSpringID, SpringBuyAmount, 11480, 6, 0},
        {1, GordianCrankID, CrankBuyAmount, 11465, 5, 0},
        {1, GordianCrankID, CrankBuyAmount, 11466, 4, 0},
        {1, GordianShaftID, ShaftBuyAmount, 11458, 3, 0},
        {1, GordianShaftID, ShaftBuyAmount, 11459, 2, 0},
        {1, GordianLensID, LensBuyAmount, 11451, 1, 0},
        {1, GordianLensID, LensBuyAmount, 11452, 0, 0},

        {2, GordianBoltID, BoltBuyAmount, 11508, 17, 0},
        {2, GordianBoltID, BoltBuyAmount, 11509, 16, 0},
        {2, GordianBoltID, BoltBuyAmount, 11503, 15, 0},
        {2, GordianBoltID, BoltBuyAmount, 11504, 14, 0},
        {2, GordianBoltID, BoltBuyAmount, 11498, 13, 0},
        {2, GordianBoltID, BoltBuyAmount, 11499, 12, 0},
        {2, GordianBoltID, BoltBuyAmount, 11493, 11, 0},
        {2, GordianBoltID, BoltBuyAmount, 11494, 10, 0},
        {2, GordianPedalID, PedalBuyAmount, 11488, 9, 0},
        {2, GordianPedalID, PedalBuyAmount, 11489, 8, 0},
        {2, GordianSpringID, SpringBuyAmount, 11481, 7, 0},
        {2, GordianSpringID, SpringBuyAmount, 11482, 6, 0},
        {2, GordianCrankID, CrankBuyAmount, 11467, 5, 0},
        {2, GordianCrankID, CrankBuyAmount, 11468, 4, 0},
        {2, GordianShaftID, ShaftBuyAmount, 11460, 3, 0},
        {2, GordianShaftID, ShaftBuyAmount, 11461, 2, 0},
        {2, GordianLensID, LensBuyAmount, 11453, 1, 0},
        {2, GordianLensID, LensBuyAmount, 11454, 0, 0},

        {0, AlexandrianBoltID, BoltBuyAmount, 16461, 22, 2},
        {0, AlexandrianBoltID, BoltBuyAmount, 16460, 21, 2},
        {0, AlexandrianBoltID, BoltBuyAmount, 16456, 20, 2},
        {0, AlexandrianBoltID, BoltBuyAmount, 16455, 19, 2},
        {0, AlexandrianBoltID, BoltBuyAmount, 16451, 18, 2},
        {0, AlexandrianBoltID, BoltBuyAmount, 16450, 17, 2},
        {0, AlexandrianBoltID, BoltBuyAmount, 16446, 16, 2},
        {0, AlexandrianBoltID, BoltBuyAmount, 16445, 15, 2},
        {0, AlexandrianPedalID, PedalBuyAmount, 16419, 14, 2},
        {0, AlexandrianPedalID, PedalBuyAmount, 16413, 13, 2},
        {0, AlexandrianPedalID, PedalBuyAmount, 16407, 12, 2},
        {0, AlexandrianSpringID, SpringBuyAmount, 16418, 11, 2},
        {0, AlexandrianSpringID, SpringBuyAmount, 16412, 10, 2},
        {0, AlexandrianSpringID, SpringBuyAmount, 16406, 9, 2},
        {0, AlexandrianCrankID, CrankBuyAmount, 16417, 8, 2},
        {0, AlexandrianCrankID, CrankBuyAmount, 16411, 7, 2},
        {0, AlexandrianCrankID, CrankBuyAmount, 16405, 6, 2},
        {0, AlexandrianShaftID, ShaftBuyAmount, 16416, 5, 2},
        {0, AlexandrianShaftID, ShaftBuyAmount, 16410, 4, 2},
        {0, AlexandrianShaftID, ShaftBuyAmount, 16404, 3, 2},
        {0, AlexandrianLensID, LensBuyAmount, 16415, 2, 2},
        {0, AlexandrianLensID, LensBuyAmount, 16409, 1, 2},
        {0, AlexandrianLensID, LensBuyAmount, 16403, 0, 2},

        {1, AlexandrianBoltID, BoltBuyAmount, 16462, 13, 2},
        {1, AlexandrianBoltID, BoltBuyAmount, 16457, 12, 2},
        {1, AlexandrianBoltID, BoltBuyAmount, 16452, 11, 2},
        {1, AlexandrianBoltID, BoltBuyAmount, 16447, 10, 2},
        {1, AlexandrianPedalID, PedalBuyAmount, 16425, 9, 2},
        {1, AlexandrianPedalID, PedalBuyAmount, 16431, 8, 2},
        {1, AlexandrianSpringID, SpringBuyAmount, 16424, 7, 2},
        {1, AlexandrianSpringID, SpringBuyAmount, 16430, 6, 2},
        {1, AlexandrianCrankID, CrankBuyAmount, 16423, 5, 2},
        {1, AlexandrianCrankID, CrankBuyAmount, 16429, 4, 2},
        {1, AlexandrianShaftID, ShaftBuyAmount, 16422, 3, 2},
        {1, AlexandrianShaftID, ShaftBuyAmount, 16428, 2, 2},
        {1, AlexandrianLensID, LensBuyAmount, 16421, 1, 2},
        {1, AlexandrianLensID, LensBuyAmount, 16427, 0, 2},

        {2, AlexandrianBoltID, BoltBuyAmount, 16463, 17, 2},
        {2, AlexandrianBoltID, BoltBuyAmount, 16464, 16, 2},
        {2, AlexandrianBoltID, BoltBuyAmount, 16458, 15, 2},
        {2, AlexandrianBoltID, BoltBuyAmount, 16459, 14, 2},
        {2, AlexandrianBoltID, BoltBuyAmount, 16453, 13, 2},
        {2, AlexandrianBoltID, BoltBuyAmount, 16454, 12, 2},
        {2, AlexandrianBoltID, BoltBuyAmount, 16448, 11, 2},
        {2, AlexandrianBoltID, BoltBuyAmount, 16449, 10, 2},
        {2, AlexandrianPedalID, PedalBuyAmount, 16437, 9, 2},
        {2, AlexandrianPedalID, PedalBuyAmount, 16443, 8, 2},
        {2, AlexandrianSpringID, SpringBuyAmount, 16436, 7, 2},
        {2, AlexandrianSpringID, SpringBuyAmount, 16442, 6, 2},
        {2, AlexandrianCrankID, CrankBuyAmount, 16435, 5, 2},
        {2, AlexandrianCrankID, CrankBuyAmount, 16441, 4, 2},
        {2, AlexandrianShaftID, ShaftBuyAmount, 16434, 3, 2},
        {2, AlexandrianShaftID, ShaftBuyAmount, 16440, 2, 2},
        {2, AlexandrianLensID, LensBuyAmount, 16433, 1, 2},
        {2, AlexandrianLensID, LensBuyAmount, 16439, 0, 2},
    };
}