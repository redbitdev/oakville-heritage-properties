using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using GalaSoft.MvvmLight.Ioc;
using HeritageProperties.PCL;

namespace HeritageProperties.Droid
{
    [Activity(Label = "HeritageProperties", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, Icon = "@drawable/icon")]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // register the gps location service
            SimpleIoc.Default.Register<IGpsService, LocationService>();
            
            Xamarin.FormsMaps.Init(this, bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            SetPage(App.GetMainPage());
        }
    }
}

