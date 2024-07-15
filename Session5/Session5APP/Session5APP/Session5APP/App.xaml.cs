using Session5APP.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Session5APP
{
    public partial class App : Application
    {
        public static User User { get; set; } = new User();

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
