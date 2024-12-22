using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public class DerivedTaskFilter : ITypeFilter
{
    public event PropertyChangedEventHandler PropertyChanged;
    public bool MatchesCriteria(ITask task) => task is DerivedTask;
    public string Name() => "Derived";
    public override string ToString() => $"Type: Derived";
}
