using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public class KeywordFilter : IValueFilter
{
    public string Criteria
    {
        get;
        set;
    }

    public bool MatchesCriteria(ITask task) {
        if (task == null || string.IsNullOrWhiteSpace(Criteria))
        {
            return false;
        }

        // Check if Criteria is in the task's name, description, or formatted time
        return task.Name.Contains(Criteria, StringComparison.OrdinalIgnoreCase) ||
               task.Description.Contains(Criteria, StringComparison.OrdinalIgnoreCase) ||
               task.FormattedTime.Contains(Criteria, StringComparison.OrdinalIgnoreCase);
    }

    public string Name() => "Keyword";

    public override string ToString() => $"Keyword: {Criteria}";
}
