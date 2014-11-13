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
	[Register ("HeritagePropertiesiOSViewController")]
	partial class HeritagePropertiesiOSViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem btnMapList { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView tableViewProperties { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnMapList != null) {
				btnMapList.Dispose ();
				btnMapList = null;
			}
			if (tableViewProperties != null) {
				tableViewProperties.Dispose ();
				tableViewProperties = null;
			}
		}
	}
}
