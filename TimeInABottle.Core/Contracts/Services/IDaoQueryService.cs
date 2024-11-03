using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Helpers;
using TimeInABottle.Core.Models;

namespace TimeInABottle.Core.Contracts.Services;
internal interface IDaoQueryService
{
    FullObservableCollection<ITask> CustomQuery(Func<ITask, bool> filter, bool isSortAscending = true);
}
