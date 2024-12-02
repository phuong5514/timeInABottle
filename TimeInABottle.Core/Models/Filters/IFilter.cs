using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public interface IFilter : INotifyPropertyChanged
{
    // Checks if the filter matches the given task
    public bool MatchesCriteria(ITask task);

    // Returns a string representation of the filter
    public string ToString() => "Filter";

    public string Name();

    public event PropertyChangedEventHandler PropertyChanged;
}
