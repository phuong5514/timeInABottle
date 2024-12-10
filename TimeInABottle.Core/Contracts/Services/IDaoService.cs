using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    /// <summary>
    /// Retrieves tasks scheduled for the current week.
    /// </summary>
    /// <returns>A collection of this week's tasks.</returns>
    FullObservableCollection<ITask> GetThisWeekTasks();

    /// <summary>
    /// Retrieves tasks scheduled for the current month.
    /// </summary>
    /// <returns>A collection of this month's tasks.</returns>
    FullObservableCollection<ITask> GetThisMonthTasks();
}
