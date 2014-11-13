using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using HeritageProperties.PCL;

namespace HeritageProperties.iOS
{
    /// <summary>
    /// Custom point annotation so we can store the HeritageProperty value
    /// </summary>
    public class HeritagePropertyAnnotation : MKPointAnnotation
    {
        public HeritagePropertyAnnotation(HeritageProperty property)
        {
            this.Property = property;
            Title = property.Name;
            Coordinate = new CLLocationCoordinate2D(property.Latitude, property.Longitude);
        }

        /// <summary>
        /// Gets the value of heritage property
        /// </summary>
        public HeritageProperty Property { get; private set; }
    }
}