using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Contracts.Services;
using Windows.Devices.Geolocation;

namespace TimeInABottle.Services;
/// <summary>
/// Service for obtaining the device's geographic location.
/// </summary>
public class LocationService : ILocationService
{
    /// <summary>
    /// Asynchronously gets the geographic coordinates (latitude and longitude) of the device.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the latitude and longitude.</returns>
    public async Task<(double Latitude, double Longitude)> GetCoordinatesAsync()
    {
        var geolocator = new Geolocator
        {
            DesiredAccuracy = PositionAccuracy.High
        };

        var position = await geolocator.GetGeopositionAsync();
        var latitude = position.Coordinate.Point.Position.Latitude;
        var longitude = position.Coordinate.Point.Position.Longitude;

        return (latitude, longitude);
    }
}
