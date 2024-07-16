using Session5APP.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Session5APP
{
    public partial class App : Application
    {
        public static User User { get; set; } = new User();
        public static List<CartItem> CartItems { get; set; }

        public App()
        {
            InitializeComponent();
            CartItems = new List<CartItem>();
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
