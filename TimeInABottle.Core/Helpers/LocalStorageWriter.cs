using Newtonsoft.Json;

namespace TimeInABottle.Core.Helpers;
public class LocalStorageWriter
{
    /// <summary>
    /// Writes the specified value to a file in local storage.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="filename">The name of the file to write to.</param>
    /// <param name="value">The value to write to the file.</param>
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
