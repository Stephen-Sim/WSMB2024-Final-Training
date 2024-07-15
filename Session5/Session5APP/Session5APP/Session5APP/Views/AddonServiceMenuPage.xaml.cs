using Session5APP.Models;
using Session5APP.Services;
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
    public partial class AddonServiceMenuPage : ContentPage
    {
        public User User { get; set; }

        public APIService APIService { get; set; }

        public AddonServiceMenuPage()
        {
            InitializeComponent();

            User = App.User;

            ulabel.Text = $"Welcome {User.Name}";

            APIService = new APIService();

            loadServiceTypesAsync();
        }

        async void loadServiceTypesAsync()
        {
            var res = await APIService.GetServiceTypes();

            // var res = await SampleService<List<ServiceType>>.Get($"Getservicetypes");

            res.ForEach(x =>
            {
                x.IconImage = ImageSource.FromResource($"Session5APP.Images.{x.IconName}");
            });

            lv.ItemsSource = res;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            // checkout
            App.Current.MainPage = new NavigationPage(new ShoppingCartPage());
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            // about
            App.Current.MainPage = new NavigationPage(new AboutPage());
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            var st = (sender as Grid).BindingContext as ServiceType;
            await App.Current.MainPage.Navigation.PushAsync(new ServiceSelectionPage(st));
        }
    }
}