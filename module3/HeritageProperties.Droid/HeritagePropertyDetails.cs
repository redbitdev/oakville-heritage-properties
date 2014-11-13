using Android.App;
using Android.Webkit;
using Android.Widget;

namespace HeritageProperties.Droid
{
    [Activity(Icon = "@drawable/icon")]
    public class HeritagePropertyDetail : Activity
    {
        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            // set the view to the details page
            SetContentView(Resource.Layout.HeritagePropertyDetail);

            // setup the view
            this.FindViewById<TextView>(Resource.Id.lblId).Text = SelectedItem.Id;
            this.FindViewById<TextView>(Resource.Id.lblLat).Text = SelectedItem.Latitude.ToString();
            this.FindViewById<TextView>(Resource.Id.lblLon).Text = SelectedItem.Longitude.ToString();
            this.FindViewById<WebView>(Resource.Id.webView).LoadData(SelectedItem.Description, "text/html", "utf-8");

            // set the title
            this.Title = SelectedItem.Name;

            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// The item selected
        /// </summary>
        public static HeritageProperty SelectedItem { get; set; }
    }
}