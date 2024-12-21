using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTurnIn.Resources
{
    internal static class BMRotations
    {
        public static string rootPassive = @"
        {
            ""Name"": ""ROR Passive"",
            ""Modules"": 
            {
                ""BossMod.Autorotation.MiscAI.StayCloseToTarget"": []
            }
        }";

        public static string rootBoss = @"
        {
          ""Name"": ""RoR Boss"",
          ""Modules"": {
            ""BossMod.Autorotation.xan.HealerAI"": [
              {
                ""Track"": ""Heal"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Esuna"",
                ""Option"": ""Enabled""
              }
            ],
            ""BossMod.Autorotation.xan.MeleeAI"": [
              {
                ""Track"": ""Second Wind"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Bloodbath"",
                ""Option"": ""Enabled""
              }
            ],
            ""BossMod.Autorotation.xan.RangedAI"": [
              {
                ""Track"": ""Second Wind"",
                ""Option"": ""Enabled""
              }
            ],
            ""BossMod.Autorotation.xan.TankAI"": [
              {
                ""Track"": ""Ranged GCD"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Interject"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Low Blow"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Arms' Length"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Personal mits"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Invuln"",
                ""Option"": ""Enabled""
              }
            ],
            ""BossMod.Autorotation.MiscAI.StayCloseToTarget"": [
              {
                ""Track"": ""range"",
                ""Option"": ""1.1""
              }
            ],
            ""BossMod.Autorotation.MiscAI.StayWithinLeylines"": [
              {
                ""Track"": ""Use Retrace"",
                ""Option"": ""Yes""
              },
              {
                ""Track"": ""Use Between The Lines"",
                ""Option"": ""Yes""
              }
            ],
            ""BossMod.Autorotation.akechi.AkechiDRG"": [
              {
                ""Track"": ""Burst"",
                ""Option"": ""Conserve""
              },
              {
                ""Track"": ""Battle Litany"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Lance Charge"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Life Surge"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Dragonfire Dive"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Rise Of The Dragon"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Stardiver"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Starcross"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Geirskogul"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Nastrond"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Wyrmwind Thrust"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Jump"",
                ""Option"": ""Force Jump""
              },
              {
                ""Track"": ""Mirage Dive"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Piercing Talon"",
                ""Option"": ""Allow""
              },
              {
                ""Track"": ""True North"",
                ""Option"": ""Automatic""
              }
            ],
            ""BossMod.Autorotation.akechi.AkechiGNB"": [
              {
                ""Track"": ""Burst"",
                ""Option"": ""Conserve""
              },
              {
                ""Track"": ""No Mercy"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Bloodfest"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Gnashing Fang"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Double Down"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Sonic Break"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Blasting Zone"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Bow Shock"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Burst Strike"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Lightning Shot"",
                ""Option"": ""Opener""
              }
            ],
            ""BossMod.Autorotation.akechi.AkechiPLD"": [
              {
                ""Track"": ""Burst"",
                ""Option"": ""Conserve""
              },
              {
                ""Track"": ""Fight or Flight"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Requiescat"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Atonement"",
                ""Option"": ""Force Atonement""
              },
              {
                ""Track"": ""Blade Combo"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Intervene"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Spirits Within"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Goring Blade Strategy"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Circle of Scorn Strategy"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Blade of Honor Strategy"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Holy Spirit Strategy"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Ranged"",
                ""Option"": ""Opener Ranged Cast""
              }
            ],
            ""BossMod.Autorotation.StandardWAR"": [
              {
                ""Track"": ""Burst"",
                ""Option"": ""Spend""
              },
              {
                ""Track"": ""Infuriate"",
                ""Option"": ""Delay""
              },
              {
                ""Track"": ""IR"",
                ""Option"": ""Delay""
              },
              {
                ""Track"": ""Upheaval"",
                ""Option"": ""Delay""
              },
              {
                ""Track"": ""PR"",
                ""Option"": ""Forbid""
              },
              {
                ""Track"": ""Onslaught"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""Tomahawk"",
                ""Option"": ""OpenerRanged""
              },
              {
                ""Track"": ""Wrath"",
                ""Option"": ""Automatic""
              }
            ],
            ""BossMod.Autorotation.VeynBRD"": [
              {
                ""Track"": ""Songs"",
                ""Option"": ""Extend""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""DOTs"",
                ""Option"": ""ApplyOrExtend""
              },
              {
                ""Track"": ""Apex"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""BL"",
                ""Option"": ""KeepOneCharge""
              },
              {
                ""Track"": ""EA"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""Barrage"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""SW"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""GCDDelay"",
                ""Option"": ""NoPrepull""
              },
              {
                ""Track"": ""Blast"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""Reso"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""Encore"",
                ""Option"": ""Automatic""
              }
            ]
          }
        }";

    }
}
