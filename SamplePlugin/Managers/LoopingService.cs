using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommons;
using System.Timers;
using ECommons.Logging;
using SamplePlugin.Tasks;
using ECommons.DalamudServices;
using System.Numerics;

namespace SamplePlugin.Managers
{
    internal class LoopingService : IDisposable
    {
        private readonly Timer updateTimer;
        public bool enabled = false;
        public LoopingService()
        {
            updateTimer = new Timer();
            updateTimer.Elapsed += OnTimerUpdate;
            updateTimer.Interval = 1000; // You watcher Tickrate in milliseconds
            updateTimer.Enabled = false;
        }
        public bool IsEnabled
        {
            get => enabled;
            set
            {
                enabled = value;
                if (enabled)
                {
                    Enable();
                }
                else
                {
                    Disable();
                }
            }
        }
        private void Enable()
        {
            PluginLog.Information("Watcher started.");
            updateTimer.Enabled = true;
            enabled = true;
        }
        private void Disable()
        {
            PluginLog.Information("Ending watcher.");
            updateTimer.Enabled = false;
            enabled = false;
        }
        private unsafe void OnTimerUpdate(object? sender, ElapsedEventArgs e)
        {
            PluginLog.Information("Watcher tick.");

            if (!TaskManager.IsBusy) 
            {
                if (Configuration.vendorTurnIn)
                {
                    Vector3 Targetxyz = new Vector3(10.2f, 0.1f, 2.2f);
                    //Enqueue(new PathfindTask(Targetxyz, false, 100));
                    //Navmesh.PathfindAndMoveTo(Targetxyz, false);

                }
                /*
                if (GetTargetName() == "") // makes sure you have nothing targeted
                {
                    string name = "Maisenta";
                    Enqueue(new TargetTask(name));
                }
                else
                {
                    PluginLog.Information(GetTargetName());
                }
                */
            }
        }
        public void Dispose()
        {
            updateTimer.Dispose();
        }
    }
}
