using System.Xml.Linq;

namespace TimeInABottle.Core.Helpers;
public class ConfigHandler
{
    public static string _filename = "settings.config";
    public static string _path = Path.Combine(AppContext.BaseDirectory, _filename);

    /// <summary>
    /// Gets the value of a specified configuration key.
    /// </summary>
    /// <param name="key">The configuration key.</param>
    /// <returns>The value associated with the specified key.</returns>
    public static string GetConfigValue(string key)
    {
        var config = ReadFile();
        return config.Element(key).Value;
    }

    /// <summary>
    /// Gets the values of specified configuration keys.
    /// </summary>
    /// <param name="keys">The configuration keys.</param>
    /// <returns>A collection of values associated with the specified keys.</returns>
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

    /// <summary>
    /// Sets the value of a specified configuration key.
    /// </summary>
    /// <param name="key">The configuration key.</param>
    /// <param name="value">The value to set.</param>
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

    /// <summary>
    /// Sets the values of specified configuration keys.
    /// </summary>
    /// <param name="keyValues">A dictionary containing the keys and values to set.</param>
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

    /// <summary>
    /// Creates a new configuration file with default values.
    /// </summary>
    private static void CreateConfigFile()
    {
        var newConfig = new XElement("config");

        var defaultValues = new Dictionary<string, string>
        {
            { "TimeSlotIncrement", "15" },
            { "TimeSlotIncrements", "15,30,60" },
            { "IsNotificationEnabled", "false" },
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

    /// <summary>
    /// Reads the configuration file. If the file does not exist, it creates a new one with default values.
    /// </summary>
    /// <returns>An XElement representing the configuration file.</returns>
    private static XElement ReadFile()
    {
        if (!File.Exists(_path))
        {
            CreateConfigFile();
        }
        return XElement.Load(_path);
    }
}
