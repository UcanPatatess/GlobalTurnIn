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

namespace SamplePlugin.Managers
{
    internal class LoopingService : IDisposable
    {
        private readonly Timer updateTimer;
        private bool enabled = Service.Configuration.Looping;
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
            Service.Configuration.Looping = true;
        }
        private void Disable()
        {
            PluginLog.Information("Ending watcher.");
            updateTimer.Enabled = false;
            Service.Configuration.Looping = false;
        }
        private unsafe void OnTimerUpdate(object? sender, ElapsedEventArgs e)
        {
            PluginLog.Information("Watcher tick.");

            if (!TaskManager.IsBusy) 
            {
                if (GetTargetName() == "") // makes sure you have nothing targeted
                {
                    string name = "Maisenta";
                    Enqueue(new TargetTask(name));
                }
                else
                {
                    PluginLog.Information(GetTargetName());
                }
            }
        }
        public void Dispose()
        {
            updateTimer.Dispose();
        }
    }
}
