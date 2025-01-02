using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

using TimeInABottle.Contracts.Services;
using Windows.Storage;
using TimeInABottle.Core.Models.Weather;

namespace TimeInABottle.Services;
internal class LocalStorageService : IStorageService
{
    public T Read<T>(string key)
    {
        var settings = ApplicationData.Current.LocalSettings;
        if (settings.Values.TryGetValue(key, out var value))
        {
            if (value is string jsonString)
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<T>(jsonString);
                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        throw new Exception();
                    }
                } // yeah this is bad, but I'm not sure what to do here
                catch (Exception)
                {
                    return (T)value;
                }

            }
            return (T)value;
        }
        return default(T)!;
    }

    public void Write<T>(string key, T value)
    {
        var settings = ApplicationData.Current.LocalSettings;
        if (value is not string && value is not int && value is not bool && value is not double)
        {
            var jsonSerializer = JsonSerializer.Create();
            using var stringWriter = new StringWriter();
            jsonSerializer.Serialize(stringWriter, value);
            var jsonString = stringWriter.ToString();
            settings.Values[key] = jsonString;
        }
        else
        {
            settings.Values[key] = value;
        }
    }
}
