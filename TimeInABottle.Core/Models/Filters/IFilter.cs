using System.ComponentModel;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public interface IFilter : INotifyPropertyChanged
{
    // Checks if the filter matches the given task
    public bool MatchesCriteria(ITask task);

    // Returns a string representation of the filter
    public string ToString() => "Filter";

    public string Name();
}
