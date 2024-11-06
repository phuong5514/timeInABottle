using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public class DailyTaskFilter : ITypeFilter
{
    public bool MatchesCriteria(ITask task) => task is DailyTask;
    public string Name() => "Daily";
    public override string ToString() => $"Type: Daily";
}
