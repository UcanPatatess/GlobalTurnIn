using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomethingNeedDoing.Managers;

namespace SamplePlugin
{
    internal class Service
    {
        internal static Plugin Plugin { get; set; } = null!;
        internal static Configuration Configuration { get; set; } = null!;
        internal static ChatManager ChatManager { get; set; } = null!;
        internal static GameEventManager GameEventManager { get; set; } = null!;
    }
}
