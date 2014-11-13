using HeritageProperties.PCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace HeritageProperties.WinPhone
{
    public partial class LocationService : IGpsService
    {

        public async Task<Location> GetLocation()
        {
            var geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            Geoposition geoposition = await geolocator.GetGeopositionAsync(
                maximumAge: TimeSpan.FromMinutes(5),
                timeout: TimeSpan.FromSeconds(10)
                );

            return new Location() { Latitude = geoposition.Coordinate.Latitude, Longitude = geoposition.Coordinate.Longitude };
        }
    }
}