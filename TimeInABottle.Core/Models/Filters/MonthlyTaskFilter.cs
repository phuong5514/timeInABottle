using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;

public class MonthlyTaskFilter : ITypeFilter
{
    public event PropertyChangedEventHandler PropertyChanged;

    public bool MatchesCriteria(ITask task) => task is MonthlyTask;
    public string Name() => "Monthly";
    public override string ToString() => $"Type: Monthly";
}

