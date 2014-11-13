using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace HeritageProperties.iOS
{
    public class HeritagePropertyService
    {
        private static HeritagePropertyService _Default;
        public static HeritagePropertyService Default
        {
            get { return _Default = _Default ?? new HeritagePropertyService(); }
        }
        
        private HeritagePropertyService() { }

        public Task<List<HeritageProperty>> Load()
        {
            // Start a task to load the data
            return Task.Factory.StartNew(() =>
            {
                var filename = "./data/data.kml";
                var ns = XNamespace.Get("http://www.opengis.net/kml/2.2");
                var xdoc = XDocument.Load(filename);
                var ps = xdoc.Element(ns + "kml").
                    Element(ns + "Document").
                    Element(ns + "Folder").
                    Elements(ns + "Placemark");

                var ret = new List<HeritageProperty>();
                var count = 0;
                foreach (var item in ps)
                {
                    var t = HeritageProperty.Parse(item, ns);
                    var e = ret.Find(i => { return t.Name.Equals(i.Name); });
                    if (e == null)
                        ret.Add(t);
                    else
                        count++;
                }

                return ret;
            });
        }
    }
}