using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public interface IFilter
{
    // Checks if the filter matches the given task
    public bool MatchesCriteria(ITask task);

    // Returns a string representation of the filter
    public string ToString();

    public string Name();
}
