using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Background;
public static class BackgroundDependencyContainer
{
    private static IServiceProvider? _services;
    private static readonly object _lock = new();

    public static void Initialize(IServiceProvider serviceProvider)
    {
        if (_services != null)
        {
            return; // Already initialized
        }

        lock (_lock)
        {
            if (_services == null) // Double-check locking
            {
                _services = serviceProvider;
            }
        }
    }

    public static T GetService<T>() where T : notnull
    {
        if (_services == null)
        {
            throw new InvalidOperationException("BackgroundDependencyContainer is not initialized.");
        }

        return (T)_services.GetService(typeof(T))!;
    }
}
