using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;
using TimeInABottle.Core.Models.Filters;

namespace TimeInABottle.Core.Contracts.Services;
public interface IDaoQueryService
{
    FullObservableCollection<ITask> CustomQuery(IFilter filter, bool isSortAscending = true);
}
