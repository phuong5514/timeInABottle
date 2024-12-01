using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Contracts.Services;
using Windows.Storage;

namespace TimeInABottle.Services;
internal class LocalStorageService : IStorageService
{
    public T? Read<T>(string key)
    {
        var settings = ApplicationData.Current.LocalSettings;
        if (settings.Values.TryGetValue(key, out var value))
        {
            return (T)value;
        }
        return default(T);
    }

    public void Write<T>(string key, T value)
    {
        var settings = ApplicationData.Current.LocalSettings;
        settings.Values[key] = value;
    }
}
