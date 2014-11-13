using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HeritageProperties.iOS
{
    public class HeritageProperty
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StyleUrl { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public static HeritageProperty Parse(XElement element, XNamespace ns = null)
        {
            return new HeritageProperty()
            {
                Id = element.Attribute("id").Value,
                Name = element.Elements(ns + "name").FirstOrDefault().Value,
                StyleUrl = element.Elements(ns + "styleUrl").FirstOrDefault().Value,
                Description = element.Elements(ns + "description").FirstOrDefault().Value,
                Longitude = double.Parse(element.Elements(ns + "Point").Elements(ns + "coordinates").FirstOrDefault().Value.Split(',')[0]),
                Latitude = double.Parse(element.Elements(ns + "Point").Elements(ns + "coordinates").FirstOrDefault().Value.Split(',')[1]),
            };
        }
    }
}
