using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using System.Threading.Tasks;
using HeritageProperties.PCL;
using HeritageProperties;
using Xamarin.Forms.Maps.WP8;
using CustomRenderer.WP8;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using HeritageProperties.WinPhone;
using System.Device.Location;

[assembly: ExportRenderer(typeof(MapRenderer2), typeof(MapRenderer2WinPhone))]

namespace CustomRenderer.WP8
{
    public class MapRenderer2WinPhone : MapRenderer
    {
        public MapRenderer2WinPhone()
            : base()
        {
            MessagingCenter.Subscribe<IEnumerable<HeritageProperty>>(this, MapRenderer2.MESSAGE_ADD_AND_ZOOM_ON_PINS, (items) =>
            {
                Task.Run(async () =>
                {
                    // just wait to give the control time to render and set it's position
                    await Task.Delay(300);

                    // invoke on main thread
                    Dispatcher.BeginInvoke(() =>
                    {
                        CustomPushPinWithToolTip prevItem = null;
                        List<GeoCoordinate> coords = new List<GeoCoordinate>();
                        var layer = new MapLayer();
                        // loop through all the properties and add them to the list
                        foreach (var item in items)
                        {
                            // create the overlay
                            var overlay = new MapOverlay()
                            {
                                GeoCoordinate = new GeoCoordinate(item.Latitude, item.Longitude),
                                PositionOrigin = new System.Windows.Point(0, 1)
                            };

                            // create th epin
                            var pin = new CustomPushPinWithToolTip(item, overlay);
                            pin.ToolTipTapped = (i) =>
                            {
                                MessagingCenter.Send<Location>(new Location() { Latitude = i.Latitude, Longitude = i.Longitude }, MapRenderer2.MESSAGE_ON_INFO_WINDOW_CLICKED);
                            };
                            pin.MarkerTapped = (p) =>
                            {
                                if (prevItem != null && prevItem != p)
                                    prevItem.HideCallout();
                                prevItem = p;
                                var o = p.ParentOverlay;
                                layer.Remove(o);
                                layer.Add(o);
                                this.NativeMap.Center = p.ParentOverlay.GeoCoordinate;
                            };

                            // set the content for the overlay
                            overlay.Content = pin;

                            // add overlay to layer
                            layer.Add(overlay);
                            
                            // add to the list
                            coords.Add(overlay.GeoCoordinate);
                        }

                        // add to map
                        this.NativeMap.Layers.Add(layer);

                        // zoom in on pins
                        this.NativeMap.SetView(LocationRectangle.CreateBoundingRectangle(coords));
                    });
                });
            });

            MessagingCenter.Subscribe<IEnumerable<HeritageProperty>>(this, MapRenderer2.MESSAGE_ZOOM_ON_PINS, (items) =>
            {
                List<GeoCoordinate> coords = new List<GeoCoordinate>();
               
                // loop through all the properties and add them to the list
                foreach (var item in items)
                    coords.Add(new GeoCoordinate(item.Latitude,item.Longitude));

                // zoom in on pins
                this.NativeMap.SetView(LocationRectangle.CreateBoundingRectangle(coords));
            });
        }

        private Map NativeMap { get { return (this.Control as Map); } }
    }
}