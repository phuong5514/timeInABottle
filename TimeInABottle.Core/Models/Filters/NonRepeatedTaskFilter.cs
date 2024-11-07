using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public class NonRepeatedTaskFilter : ITypeFilter
{
    public event PropertyChangedEventHandler PropertyChanged;

    public bool MatchesCriteria(ITask task) => task is NonRepeatedTask;
    public string Name() => "Non-Repeated";
    public override string ToString() => $"Type: Non-Repeated";
}
