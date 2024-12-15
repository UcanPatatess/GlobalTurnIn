using ECommons.Automation;

namespace GlobalTurnIn.Scheduler.Tasks
{
    internal class TaskMergeWithAutomaton
    {
        internal static void Enqueue()
        {
            P.taskManager.Enqueue(() => UpdateCurrentTask("MergeWithAutomaton"));
            P.taskManager.Enqueue(() => Chat.Instance.SendMessage("/inventory"));
            P.taskManager.EnqueueDelay(100);
            P.taskManager.Enqueue(() => Chat.Instance.SendMessage("/inventory"));
            P.taskManager.EnqueueDelay(100);
            P.taskManager.Enqueue(() => Chat.Instance.SendMessage("/inventory"));
            P.taskManager.EnqueueDelay(100);
            P.taskManager.Enqueue(() => UpdateCurrentTask(""));
        }
    }
}
