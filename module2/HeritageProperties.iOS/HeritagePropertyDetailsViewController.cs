using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace HeritageProperties.iOS
{
	partial class HeritagePropertyDetailsViewController : UIViewController
	{
		public HeritagePropertyDetailsViewController (IntPtr handle) : base (handle)
		{
		}



        /// <summary>
        /// The Selected Heritage Property item
        /// </summary>
        public HeritageProperty SelectedHeritageProperty { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // set the data
            this.Title = SelectedHeritageProperty.Name;
            this.lblId.Text = SelectedHeritageProperty.Id;
            this.lblLat.Text = SelectedHeritageProperty.Latitude.ToString();
            this.lblLon.Text = SelectedHeritageProperty.Longitude.ToString();
            this.webview.LoadHtmlString(SelectedHeritageProperty.Description, null);
        }




	}
}
