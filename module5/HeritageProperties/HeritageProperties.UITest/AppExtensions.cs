using System;
using Xamarin.UITest;

namespace HeritageProperties.UITest
{
	public static class AppExtension
	{
		public static void WaitFor(	this IApp app, TimeSpan time){
			var waitUntil = DateTime.Now.Add (time);
			while (waitUntil.Ticks > DateTime.Now.Ticks);
			// NOTE this will kill the CPU
		}
	}
}

