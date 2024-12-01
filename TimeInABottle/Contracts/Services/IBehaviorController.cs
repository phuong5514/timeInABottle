using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Contracts.Services;
public interface IBehaviorController
{
    public bool CanRun();
    public bool CanStop();
    public void Update();
    public void Run();
}
