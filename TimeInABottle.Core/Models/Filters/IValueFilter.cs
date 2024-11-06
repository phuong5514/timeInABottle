using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Models.Filters;
public interface IValueFilter : IFilter
{
    string Criteria { get; set; }
}
