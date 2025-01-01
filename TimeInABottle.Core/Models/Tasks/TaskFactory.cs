namespace TimeInABottle.Core.Models.Tasks;
public class TaskFactory
{

    private static readonly Dictionary<string, Type> _tasks = [];

    public static void RegisterTask(string taskName, Type taskType)
    {
        if (_tasks.ContainsKey(taskName) == false)
        {
            _tasks.Add(taskName, taskType);
        }
    }

    public static ITask CreateTask(string taskName)
    {
        return (ITask)Activator.CreateInstance(_tasks[taskName]);
    }
}
