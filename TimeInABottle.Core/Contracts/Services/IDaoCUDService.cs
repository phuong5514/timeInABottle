using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Core.Models.Tasks;

namespace TimeInABottle.Core.Contracts.Services;
public partial interface IDaoService
{
    public void AddTask(ITask task);
    public void UpdateTask(ITask task);
    public void DeleteTask(ITask task);
}
