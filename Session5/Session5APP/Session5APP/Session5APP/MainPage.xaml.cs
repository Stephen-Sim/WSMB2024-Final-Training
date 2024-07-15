using Session5APP.Models;
using Session5APP.Services;
using Session5APP.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Session5APP
{
    public partial class MainPage : ContentPage
    {
        public APIService APIService { get; set; }

        public MainPage()
        {
            InitializeComponent();
            APIService = new APIService();  
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(uEntry.Text) || string.IsNullOrEmpty(pEntry.Text))
            {
                await DisplayAlert("", "All the fields are required", "Ok");
                return;
            }

            var res = await APIService.Login(uEntry.Text, pEntry.Text);

            // var res = await SampleService<User>.Get($"Login?u={uEntry.Text}&p={pEntry.Text}");

            if (res)
            {
                // App.User = res;

                App.Current.MainPage = new NavigationPage(new AddonServiceMenuPage());
            }
            else
            {
                await DisplayAlert("", "Invalid Login", "Ok");
            }
        }
    }
}
