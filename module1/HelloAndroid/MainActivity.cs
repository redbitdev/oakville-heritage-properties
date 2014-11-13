using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace HelloAndroid
{
    [Activity(Label = "HelloAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

            var btn = FindViewById<Button>(Resource.Id.SampleButton);
            button.Click += delegate
            {
                new AlertDialog.Builder(this)
                .SetPositiveButton("OK", delegate
                {
                    Console.WriteLine("OK Clicked");
                })
                .SetNegativeButton("Cancel", delegate
                {
                    Console.WriteLine("Cancel clicked");
                })
                .SetMessage("Lots of code for a messagebox :)")
                .SetTitle("Hello Android!")
                .Show();
            };

        }
    }
}

