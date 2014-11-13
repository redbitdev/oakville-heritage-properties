using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using System.Threading.Tasks;
using HeritageProperties.PCL;
using HeritageProperties;
using Xamarin.Forms.Maps;
using CustomRenderer.iOS;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Maps.iOS;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using HeritageProperties.iOS;

[assembly: ExportRenderer(typeof(MapRenderer2), typeof(MapRenderer2iOS))]

namespace CustomRenderer.iOS
{
    public class MapRenderer2iOS : MapRenderer
    {
        public MapRenderer2iOS()
            : base()
        {
            MessagingCenter.Subscribe<IEnumerable<HeritageProperty>>(this, MapRenderer2.MESSAGE_ADD_AND_ZOOM_ON_PINS, (items) =>
            {
                // wire up the map 
                WireUpMap();

                // value to determin where to zoom in
                var topLeft = new CLLocationCoordinate2D(-90, 180);
                var bottomRight = new CLLocationCoordinate2D(90, -180);

                // loop through all the properties and add them to the list
                foreach (var item in items)
                {
                    // create the pin and add the annoation
                    var pin = new HeritagePropertyAnnotation(item);
                    this.NativeMap.AddAnnotation(pin);

                    // determin the topleft and right
                    topLeft.Longitude = Math.Min(topLeft.Longitude, pin.Coordinate.Longitude);
                    topLeft.Latitude = Math.Max(topLeft.Latitude, pin.Coordinate.Latitude);
                    bottomRight.Longitude = Math.Max(bottomRight.Longitude, pin.Coordinate.Longitude);
                    bottomRight.Latitude = Math.Min(bottomRight.Latitude, pin.Coordinate.Latitude);
                }

                // zoom in on the annotations
                var region = new MKCoordinateRegion();
                region.Center = new CLLocationCoordinate2D(
                    topLeft.Latitude - (topLeft.Latitude - bottomRight.Latitude) * 0.5,
                    topLeft.Longitude + (bottomRight.Longitude - topLeft.Longitude) * 0.5);
                region.Span.LatitudeDelta = Math.Abs(topLeft.Latitude - bottomRight.Latitude) * 1.1;
                region.Span.LongitudeDelta = Math.Abs(bottomRight.Longitude - topLeft.Longitude) * 1.1;

                // set the region
                region = this.NativeMap.RegionThatFits(region);
                this.NativeMap.SetRegion(region, true);

            });

            MessagingCenter.Subscribe<IEnumerable<HeritageProperty>>(this, MapRenderer2.MESSAGE_ZOOM_ON_PINS, (items) =>
            {
                // value to determin where to zoom in
                var topLeft = new CLLocationCoordinate2D(-90, 180);
                var bottomRight = new CLLocationCoordinate2D(90, -180);

                // loop through all the properties and add them to the list
                foreach (var item in items)
                {
                    // determin the topleft and right
                    topLeft.Longitude = Math.Min(topLeft.Longitude, item.Longitude);
                    topLeft.Latitude = Math.Max(topLeft.Latitude, item.Latitude);
                    bottomRight.Longitude = Math.Max(bottomRight.Longitude, item.Longitude);
                    bottomRight.Latitude = Math.Min(bottomRight.Latitude, item.Latitude);
                }

                // zoom in on the annotations
                var region = new MKCoordinateRegion();
                region.Center = new CLLocationCoordinate2D(
                    topLeft.Latitude - (topLeft.Latitude - bottomRight.Latitude) * 0.5,
                    topLeft.Longitude + (bottomRight.Longitude - topLeft.Longitude) * 0.5);
                region.Span.LatitudeDelta = Math.Abs(topLeft.Latitude - bottomRight.Latitude) * 1.1;
                region.Span.LongitudeDelta = Math.Abs(bottomRight.Longitude - topLeft.Longitude) * 1.1;

                // set the region
                region = this.NativeMap.RegionThatFits(region);
                this.NativeMap.SetRegion(region, true);
            });
        }

        private MKMapView NativeMap { get { return (this.NativeView as MapRenderer).Control as MKMapView; } }

        private bool _wiredUp = false;
        private void WireUpMap()
        {
            if (!_wiredUp)
            {
                _wiredUp = true;
                this.NativeMap.Delegate = new HeritagePropertyMapViewDelegate((item) =>
                {
                    MessagingCenter.Send<Location>(
                    new Location() { Latitude = item.Latitude, Longitude = item.Longitude },
                    MapRenderer2.MESSAGE_ON_INFO_WINDOW_CLICKED);
                });
            }
        }
    }
}