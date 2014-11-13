using Android.App;
using Android.Runtime;
using System;

namespace HeritageProperties.Droid
{
    [Application]
    public class App : Application
    {
        public App(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            System.Diagnostics.Debug.Write("------------> App.OnCreate()");
            base.OnCreate();
        }
    }
}