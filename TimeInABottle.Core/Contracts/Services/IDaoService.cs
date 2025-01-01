using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Contracts.Services;
/// <summary>
/// Interface for Data Access Object (DAO) service to manage tasks.
/// </summary>
public partial interface IDaoService
{
    /// <summary>
    /// Retrieves all tasks.
    /// </summary>
    /// <returns>A collection of all tasks.</returns>
    FullObservableCollection<ITask> GetAllTasks();


    /// <summary>
    /// Retrieves tasks scheduled for today.
    /// </summary>
    /// <returns>A collection of today's tasks.</returns>
    FullObservableCollection<ITask> GetTodayTasks();
}
