using SamplePlugin.IPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin.Util
{
    public static class VnaVmeshUtils
    {
        public static NavmeshIPC Navmesh = new();
        public static void NavStop() => Navmesh.Stop();
        public static void NavPathfindAndMoveTo(float x, float y, float z, bool fly = false) => Navmesh.PathfindAndMoveTo(new Vector3(x,y,z), fly);

    }
}
