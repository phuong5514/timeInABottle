using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;

public class WeeklyTaskFilter : ITypeFilter
{
    public bool MatchesCriteria(ITask task) => task is WeeklyTask;
    public string Name() => "Weekly";
    public override string ToString() => $"Type: Weekly";
}
