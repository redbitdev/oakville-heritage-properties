using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace HeritageProperties
{
    public class HeritagePropertyService
    {
        private static HeritagePropertyService _Default;
        public static HeritagePropertyService Default
        {
            get { return _Default = _Default ?? new HeritagePropertyService(); }
        }
        
        private HeritagePropertyService() { }

#if __IOS__
        public Task<List<HeritageProperty>> Load()
#else
        public Task<List<HeritageProperty>> Load(Stream stream)
#endif
        {
            // Start a task to load the data
            return Task.Factory.StartNew(() =>
            {
                var ns = XNamespace.Get("http://www.opengis.net/kml/2.2");
#if __IOS__
                var filename = "./data/data.kml";
                var xdoc = XDocument.Load(filename);
#else
                var xdoc = XDocument.Load(stream);
#endif
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