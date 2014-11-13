using HeritageProperties.PCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HeritageProperties.Pages
{
    public class DetailsPage : ContentPage
    {
        public DetailsPage()
        {
            // set the binding context
            this.BindingContext = ViewModel;

            // bind the title property to the selected item name
            this.SetBinding(ContentPage.TitleProperty, "SelectedItem.Name");

            // create the stack layout
            var stack = new StackLayout()
            {
                Spacing = 10,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            // set the padding
            Padding = new Thickness(15);

            // create the labels
            // if we are on windows phone, add an extra label to show the name since we have no titlebar
            Device.OnPlatform(null, null, () =>
            {
                // on winphone create a clabel for the name
                stack.Children.Add(CreateLabel("Name"));
                stack.Children.Add(CreateLabel("Name", "Name"));
            }, null);

            // create the id label
            stack.Children.Add(CreateLabel("Id"));
            stack.Children.Add(CreateLabel("Id", "Id"));

            // create the lat label
            stack.Children.Add(CreateLabel("Lat"));
            stack.Children.Add(CreateLabel("Lat", "Latitude"));

            // create the lon label
            stack.Children.Add(CreateLabel("Lon"));
            stack.Children.Add(CreateLabel("Lon", "Longitude"));

            // create the webview
            var wv = new WebView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            wv.Source = new HtmlWebViewSource() { Html = ViewModel.SelectedItem.Description };

            // add the webview to the stack layout
            stack.Children.Add(wv);

            // set the content
            this.Content = stack;
        }

        private DetailsViewModel ViewModel { get { return App.Locator.GetViewModel<DetailsViewModel>(); } }

        private Label CreateLabel(string text, string bindingPath = null)
        {
            var label = new Label();

            if (bindingPath != null)
                label.SetBinding(Label.TextProperty, string.Format("SelectedItem.{0}", bindingPath));
            else
                label.Text = text;

            return label;
        }

    }
}
