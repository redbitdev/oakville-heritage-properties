using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace HeritageProperties.PCL
{
    public class HeritagePropertyService
    {
        public HeritagePropertyService() { }

        public Task<List<HeritageProperty>> Load()
        {
            // Start a task to load the data
            return Task.Run(() =>
            {
                // load the stream that is embedded in PCL
                var assembly = typeof(HeritagePropertyService).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("HeritageProperties.data.data.kml");

                // load the stream to read the kml
                var xdoc = XDocument.Load(stream);
                var ns = XNamespace.Get("http://www.opengis.net/kml/2.2");
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