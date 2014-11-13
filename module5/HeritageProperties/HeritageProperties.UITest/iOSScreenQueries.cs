using System;
using Xamarin.UITest.Queries;

namespace HeritageProperties.UITest
{
	public class iOSScreenQueries : IScreenQueries
	{
		#region IScreenQueries implementation
        public Func<AppQuery, AppQuery> MapView
        {
			get {
				return c => c.Class ("MKMapView");
			}
		}
        public Func<AppQuery, AppQuery> ListView
        {
			get {
				return c => c.Class ("UITableView");
			}
		}
        public Func<AppQuery, AppQuery> NavigationButton
        {
			get {
				return c => c.Class ("UINavigationButton");
			}
		}

        public Func<AppQuery, AppQuery> WebView
        {
            get
            {
                return c => c.Class("UIWebView");
            }
        }
		#endregion
	}
}

