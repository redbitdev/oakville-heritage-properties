using Android.App;
using Android.Content;
using Android.Locations;
using HeritageProperties.PCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeritageProperties.Droid
{
    public class LocationService : IGpsService
    {
        LocationManager _locationManager;
        LocationListener _listener;

        public Task<HeritageProperties.PCL.Location> GetLocation()
        {
            // check to see if location is found
            bool locationFound = false;
            var ret = new HeritageProperties.PCL.Location();

            // create the location manager passing in the callback parameter
            InitializeLocationManager((loc) =>
                    {
                        if (loc != null)
                        {
                            ret = new HeritageProperties.PCL.Location()
                            {
                                Latitude = loc.Latitude,
                                Longitude = loc.Longitude
                            };
                        }

                        locationFound = true;
                    });

            // run the task
            return Task.Run(async () =>
            {
                // wait till done
                while (!locationFound)
                    await Task.Delay(200);

                // stop udpating the location
                _locationManager.RemoveUpdates(_listener);

                // return the location
                return ret;
            });
        }

        private void InitializeLocationManager(Action<Android.Locations.Location> callback)
        {
            if (_locationManager == null)
                _locationManager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);

            // creatae a listener
            _listener = new LocationListener()
            {
                LocationChangedCallback = callback
            };

            // ask the location manager to start listening
            _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, _listener);
            _locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 0, 0, _listener);
        }

        private class LocationListener : Java.Lang.Object, ILocationListener
        {
            public Action<Android.Locations.Location> LocationChangedCallback { get; set; }

            public void OnLocationChanged(Android.Locations.Location location)
            {
                if (LocationChangedCallback != null)
                    LocationChangedCallback(location);
            }

            public void OnProviderDisabled(string provider)
            {
            }

            public void OnProviderEnabled(string provider)
            {
            }

            public void OnStatusChanged(string provider, Availability status, Android.OS.Bundle extras)
            {
            }
        }
    }
}