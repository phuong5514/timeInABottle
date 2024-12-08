using Windows.Devices.Geolocation;


using System.Threading.Tasks;
using System;
namespace TimeInABottle.Background.Helpers;
public sealed class GeoLocationFetcher
{
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



