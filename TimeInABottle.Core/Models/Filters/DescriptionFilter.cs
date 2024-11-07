using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;

public class DescriptionFilter : IValueFilter
{
    public string Criteria
    {
        get;
        set;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public bool MatchesCriteria(ITask task)
    {
        if (task == null || string.IsNullOrWhiteSpace(Criteria))
        {
            return false;
        }

        // Check if Criteria is in the task's description
        return task.Description.Contains(Criteria, StringComparison.OrdinalIgnoreCase);
    }

    public string Name() => "Description";

    public override string ToString() => $"Description: {Criteria}";
}

