using HeritageProperties.Pages;
using HeritageProperties.PCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HeritageProperties
{
    public class App
    {
        public static Page GetMainPage()
        {
            return new MainPage();
        }

        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator
        {
            get
            {
                return _locator ?? (_locator = new ViewModelLocator());
            }
        }

    }
}