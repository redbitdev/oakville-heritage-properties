using System;
using Xamarin.UITest.Queries;

namespace HeritageProperties.UITest
{

	public interface IScreenQueries
	{
		Func<AppQuery, AppQuery> MapView { get; }

		Func<AppQuery, AppQuery> ListView { get; }

		Func<AppQuery, AppQuery> NavigationButton { get; }

        Func<AppQuery, AppQuery> WebView { get; }
	}
}