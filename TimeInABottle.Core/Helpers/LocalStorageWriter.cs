using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimeInABottle.Core.Contracts.Services;

namespace TimeInABottle.Core.Helpers;
public class LocalStorageWriter
{
    public static void Write<T>(string filename, T value)
    {
        if (value == null)
        {
            return;
        }

        var path = Path.Combine(AppContext.BaseDirectory, filename);
        File.WriteAllText(path, JsonConvert.SerializeObject(value));
    }
}
