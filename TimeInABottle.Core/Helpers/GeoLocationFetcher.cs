using Windows.Devices.Geolocation;

namespace TimeInABottle.Core.Helpers;
public class GeoLocationFetcher
{
    public static async Task<(double Latitude, double Longitude)> GetCoordinatesAsync()
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



