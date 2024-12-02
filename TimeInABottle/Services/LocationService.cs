using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeInABottle.Contracts.Services;
using Windows.Devices.Geolocation;

namespace TimeInABottle.Services;
public class LocationService : ILocationService
{
    //public (double Latitude, double Longitude) GetCoordinates() => GetCoordinatesAsync().GetAwaiter().GetResult();

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
