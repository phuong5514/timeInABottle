using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Contracts.Services;
public partial interface IDaoService
{
    public void AddTask(ITask task);
    public void UpdateTask(ITask task);
    public void DeleteTask(ITask task);
    public void AddTasks(IEnumerable<ITask> tasks);
    public void UpdateTasks(IEnumerable<ITask> tasks);
    public void DeleteTasks(IEnumerable<ITask> tasks);
}
