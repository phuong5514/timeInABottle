using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TimeInABottle.Core.Helpers;
public class ConfigHandler
{
    public static string _filename = "settings.config";
    public static string _path = Path.Combine(AppContext.BaseDirectory, _filename);

    public static string GetConfigValue(string key)
    {
        var config = ReadFile();
        return config.Element(key).Value;
    }

    public static IEnumerable<string> GetConfigValues(IEnumerable<string> keys)
    {
        var config = ReadFile();
        List<string> values = [];
        foreach (var key in keys)
        {
            values.Add(config.Element(key).Value);
        }
        return values;
    }

    public static void SetConfigValue(string key, string value)
    {
        var config = ReadFile();
        if (config.Element(key) == null)
        {
            config.Add(new XElement(key, value));
        }
        else
        {
            config.Element(key).Value = value;
        }
        config.Save(_path);
    }

    public static void SetConfigValues(Dictionary<string, string> keyValues)
    {
        var config = ReadFile();
        foreach (var keyValue in keyValues)
        {
            if (config.Element(keyValue.Key) == null)
            {
                config.Add(new XElement(keyValue.Key, keyValue.Value));
            }
            else
            {
                config.Element(keyValue.Key).Value = keyValue.Value;
            }
        }
        config.Save(_path);
    }

    private static void CreateConfigFile()
    {
        var newConfig = new XElement("config");

        var defaultValues = new Dictionary<string, string>
        {
            { "TimeSlotIncrement", "15" },
            { "TimeSlotIncrements", "15,30,60" },
            { "IsNotificationEnabled", "true" },
            { "BackgroundTaskRefreshRate", "15" },
            { "NotificationTime", "15" },
            { "NotificationDuration", "5" },
            { "SchedulingStartTime", "08:00:00" },
            { "SchedulingEndTime", "20:00:00" }
        };

        foreach (var keyValue in defaultValues)
        {
            newConfig.Add(new XElement(keyValue.Key, keyValue.Value));
        }

        newConfig.Save(_path);
    }

    private static XElement ReadFile()
    {
        if (!File.Exists(_path))
        {
            CreateConfigFile();
        }
        return XElement.Load(_path);
    }
}
