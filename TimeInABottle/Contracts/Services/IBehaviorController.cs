using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Contracts.Services;
public interface IBehaviorController
{
    public Task<bool> CanRunAsync();
    public bool CanStop();
    public void Update();
    public Task RunAsync();
}
