using ECommons.DalamudServices;
using ECommons.Logging;

namespace SamplePlugin.Tasks;

public class TargetTask(string targetName) : IBaseTask
{
    public unsafe bool? Run()
    {
        var target = GetObjectByName(targetName);
        if (target != null)
        {
            PluginLog.Information("Targeted: " + targetName);
            Svc.Targets.Target = target;
            return true;
        }
        return false;
    }
}
