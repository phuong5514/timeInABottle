using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Models;
public enum FunctionResultCode
{
    SUCCESS = 0,
    ERROR   = 1,
    ERROR_INVALID_INPUT = 12,
    ERROR_MISSING_INPUT = 13,
    ERROR_UNKNOWN = 14,
    WARNING = 2,
}
