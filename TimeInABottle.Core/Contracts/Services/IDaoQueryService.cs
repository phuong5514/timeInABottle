using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;
using TimeInABottle.Core.Models.Filters;

namespace TimeInABottle.Core.Contracts.Services;
/// <summary>
/// Interface for DAO query services.
/// Provides methods to perform custom queries on tasks.
/// </summary>
public interface IDaoQueryService
{
    /// <summary>
    /// Executes a custom query based on the provided filter and sorting order.
    /// </summary>
    /// <param name="filter">The filter criteria to apply to the query.</param>
    /// <param name="isSortAscending">Indicates whether the results should be sorted in ascending order.</param>
    /// <returns>A collection of tasks that match the filter criteria.</returns>
    FullObservableCollection<ITask> CustomQuery(IFilter filter, bool isSortAscending = true);
}
