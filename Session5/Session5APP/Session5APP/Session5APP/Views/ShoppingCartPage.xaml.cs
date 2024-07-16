using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Session5APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingCartPage : ContentPage
    {
        public ShoppingCartPage()
        {
            InitializeComponent();
            CartLabel.Text = $"Cart ({App.CartItems.Count})";
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new AddonServiceMenuPage());
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            // checkout
            // App.Current.MainPage = new NavigationPage(new ShoppingCartPage());
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            // about
            App.Current.MainPage = new NavigationPage(new AboutPage());
        }
    }
}