using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Xamarin.Forms;
using GalaSoft.MvvmLight.Ioc;
using HeritageProperties.PCL;


namespace HeritageProperties.WinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            // register the gps location service
            SimpleIoc.Default.Register<IGpsService, LocationService>();
            Xamarin.FormsMaps.Init();
            Forms.Init();
            Content = HeritageProperties.App.GetMainPage().ConvertPageToUIElement(this);
        }
    }
}
