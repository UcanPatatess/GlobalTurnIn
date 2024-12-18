using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn;

internal class GTData
{
    // Targets (ObjectID):
    // Alexander - The Burder of the Father (A4N) 
    public static readonly uint RightForeleg = 1073770019;
    public static readonly uint LeftForeleg = 1073770020;
    public static readonly uint Manipulator = 1073770018;
    public static readonly uint A4NChest1 = 438;
    public static readonly uint A4NChest2 = 480;
    public static readonly uint A4NChest3 = 479;

    // Zones
    public static int A4N = 445;

    // Chest Positions
    public static Vector3 A4NChest1Pos = new Vector3(-0.02f, 10.54f, -8.38f);
    public static Vector3 A4NChest2Pos = new Vector3(2f, 10.54f, -6.36f);
    public static Vector3 A4NChest3Pos = new Vector3(-2.03f, 10.54f, -6.36f);

}
