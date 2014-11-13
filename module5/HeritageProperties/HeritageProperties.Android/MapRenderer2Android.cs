using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using CustomRenderer.Android;
using Xamarin.Forms.Maps.Android;
using System.Threading.Tasks;
using HeritageProperties.PCL;
using HeritageProperties;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;

[assembly: ExportRenderer(typeof(MapRenderer2), typeof(MapRenderer2Android))]

namespace CustomRenderer.Android
{
    public class MapRenderer2Android : MapRenderer
    {
        public MapRenderer2Android()
            : base()
        {
            WireUpMap();

            MessagingCenter.Subscribe<IEnumerable<HeritageProperty>>(this, MapRenderer2.MESSAGE_ADD_AND_ZOOM_ON_PINS, async (items) =>
            {
                // wait for map
                await WaitForMap();
                   
                // loop all the properties and add them as markers
                foreach (var item in items)
                {
                    // create the marker
                    var m = new MarkerOptions();
                    m.SetPosition(new LatLng(item.Latitude, item.Longitude));
                    m.SetTitle(item.Name);

                    // add to map
                    this.NativeMap.AddMarker(m);
                }

                // zoom in on the pins
                ZoomAndCenterMap(items);
            });

            MessagingCenter.Subscribe<IEnumerable<HeritageProperty>>(this, MapRenderer2.MESSAGE_ZOOM_ON_PINS, (items) =>
            {
                // zoom in on the pins
                ZoomAndCenterMap(items);
            });
        }

        private void ZoomAndCenterMap(IEnumerable<HeritageProperty> items)
        {
            Task.Run(async () =>
            {
                // wait a bit
                await Task.Delay(50);

                // invoke on main thread
                Handler.InvokeOnMainThread(() =>
                {
                    // create a bounds builder
                    LatLngBounds.Builder builder = new LatLngBounds.Builder();

                    // loop all the properties and add them as markers
                    foreach (var item in items)
                        builder.Include(new LatLng(item.Latitude, item.Longitude));

                    // zoom the map in
                    CameraUpdate cu = CameraUpdateFactory.NewLatLngBounds(builder.Build(), 100);
                    this.NativeMap.MoveCamera(cu);
                    this.NativeMap.AnimateCamera(cu);
                });
            });
        }

        private async void WireUpMap()
        {
            // wait for the map object
            await WaitForMap();
            
            // wire up the map click
            this.NativeMap.InfoWindowClick += (s, e) =>
            {
                MessagingCenter.Send<Location>(
                    new Location() { Latitude = e.P0.Position.Latitude, Longitude = e.P0.Position.Longitude },
                    MapRenderer2.MESSAGE_ON_INFO_WINDOW_CLICKED);
            };
        }

        private Task WaitForMap()
        {
            return Task.Run(async () =>
            {
                // just delay so we are not overloading the CPU
                await Task.Delay(1000);

                if (Handler == null)
                {
                    await WaitForMap();
                }
                else
                {
                    // make sure we run on the UI thread
                    Handler.InvokeOnMainThread(async () =>
                    {
                        // if map is still not populated try again
                        if (this.NativeMap == null)
                        {
                            await WaitForMap();
                        }
                    });
                }
            });
        }

       
        
    }

    public static class Dispatcher
    {
        public static void InvokeOnMainThread(this Handler handler, Action action)
        {
            if (handler == null)
            {
                InvokeOnMainThread(handler, action);
            }
            else
            {
                // make sure we run on the UI thread
                handler.Post(action);
            }
        }
    }
}