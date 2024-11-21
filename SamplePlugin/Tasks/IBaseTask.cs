namespace SamplePlugin.Tasks;

/**
 * Interface for tasks that can be enqueued in the TaskManager.
 */
public interface IBaseTask
{
    public bool? Run();
}
