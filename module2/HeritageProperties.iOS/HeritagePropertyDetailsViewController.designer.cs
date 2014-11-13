// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace HeritageProperties.iOS
{
	[Register ("HeritagePropertyDetailsViewController")]
	partial class HeritagePropertyDetailsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblId { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblLat { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblLon { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIWebView webview { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (lblId != null) {
				lblId.Dispose ();
				lblId = null;
			}
			if (lblLat != null) {
				lblLat.Dispose ();
				lblLat = null;
			}
			if (lblLon != null) {
				lblLon.Dispose ();
				lblLon = null;
			}
			if (webview != null) {
				webview.Dispose ();
				webview = null;
			}
		}
	}
}
