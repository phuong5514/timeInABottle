using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Contracts.Services;
internal interface IStorageService
{
    public T Read<T>(string key);
    public void Write<T>(string key, T value);
}
