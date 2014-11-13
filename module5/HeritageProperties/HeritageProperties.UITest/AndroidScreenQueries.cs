using System;
using Xamarin.UITest.Queries;

namespace HeritageProperties.UITest
{
	public class AndroidScreenQueries : IScreenQueries
	{
		#region IScreenQueries implementation
        public Func<AppQuery, AppQuery> MapView
        {
			get {
				return c => c.Class ("MapView");
			}
		}
        public Func<AppQuery, AppQuery> ListView
        {
			get {
				return c => c.Class ("ListView");
			}
		}
        public Func<AppQuery, AppQuery> NavigationButton
        {
			get {
				return c => c.Class ("ActionMenuItemView");
			}
		}
        public Func<AppQuery, AppQuery> WebView
        {
            get
            {
                return c => c.Class("WebView");
            }
        }
		#endregion
	}
}

