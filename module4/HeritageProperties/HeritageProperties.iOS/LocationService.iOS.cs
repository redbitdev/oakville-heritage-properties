using HeritageProperties.PCL;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeritageProperties.iOS
{
    public partial class LocationService : IGpsService
    {
        CLLocationManager manager = new CLLocationManager();

        public Task<Location> GetLocation()
        {
            manager.StartUpdatingLocation();
            
            return Task.Run(async () =>
            {
                var ret = new Location();
                
                var locationFound = false;

                // create the locator
                manager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    if (e.Locations.Length > 0)
                    {
                        ret = new Location()
                        {
                            Latitude = e.Locations[0].Coordinate.Latitude,
                            Longitude = e.Locations[0].Coordinate.Longitude
                        };
                    }

                    locationFound = true;
                };
                manager.Failed += (object sender, NSErrorEventArgs e) =>
                {
                    locationFound = true;
                };

                // wait till done
                while (!locationFound)
                    await Task.Delay(200);

                // stop udpating the location
                manager.StopUpdatingLocation();

                // return the location
                return ret;
            });
        }
    }
}