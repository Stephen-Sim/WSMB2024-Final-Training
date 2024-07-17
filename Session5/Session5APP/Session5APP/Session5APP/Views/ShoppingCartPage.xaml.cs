using Session5APP.Models;
using Session5APP.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            APIService = new APIService();
            loadData();
        }

        public List<CartItem> Items { get; set; }

        void loadData()
        {
            isCouponAppliedLabel.IsVisible = false;
            Items = new List<CartItem>(App.CartItems);
            lv.ItemsSource = Items;
            
            PayPrice = Items.Sum(x => x.Price);

            if (CouponApplied != null)
            {
                var discount = PayPrice * (CouponApplied.DiscountPercent / 100);
                isCouponAppliedLabel.IsVisible = true;
                PayPrice = PayPrice - (discount > CouponApplied.MaximimDiscountAmount ? CouponApplied.MaximimDiscountAmount : discount);
                TotalPriceLabel.Text = $"Total Amount payable ({Items.Count} items): ${PayPrice.ToString("0.00")}";
            }

            TotalPriceLabel.Text = $"Total Amount payable ({Items.Count} items): ${PayPrice.ToString("0.00")}";
            CartLabel.Text = $"Cart ({Items.Count})";
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert("", "are you sure to delete the item?", "yes", "no");

            if (res)
            {
                var cartitem = (sender as Button).CommandParameter as CartItem;
                var temp = Items;
                temp.Remove(cartitem);
                App.CartItems = temp;
                loadData();
            }
        }

        public APIService APIService { get; set; }

        public Coupon CouponApplied { get; set; } = null;

        public decimal PayPrice { get; set; }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var coupons = await APIService.GetCoupons();

            PayPrice = Items.Sum(x => x.Price);

            CouponApplied = coupons.FirstOrDefault(x => x.CouponCode == couponEntry.Text);

            if (CouponApplied == null)
            {
                await DisplayAlert("", "invalid coupon", "Ok");
                PayPrice = Items.Sum(x => x.Price);
                TotalPriceLabel.Text = $"Total Amount payable ({Items.Count} items): ${PayPrice.ToString("0.00")}";
                isCouponAppliedLabel.IsVisible = false;
            }
            else
            {
                var discount = PayPrice * (CouponApplied.DiscountPercent / 100);
                PayPrice = PayPrice - (discount > CouponApplied.MaximimDiscountAmount ? CouponApplied.MaximimDiscountAmount : discount);
                TotalPriceLabel.Text = $"Total Amount payable ({Items.Count} items): ${PayPrice.ToString("0.00")}";
                isCouponAppliedLabel.IsVisible = true;
            }
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            // pay
            if (Items.Count == 0)
            {
                await DisplayAlert("", "cart is empty", "Ok");
                return;
            }

            var addonServiceDetails = new List<AddonServiceDetailStoreRequest>();

            foreach (var item in Items)
            {
                addonServiceDetails.Add(new AddonServiceDetailStoreRequest()
                { 
                    FromDate = item.FromDate,
                    NOP = item.NOP,
                    Note = item.Note,
                    Price = item.Price,
                    ServiceID = item.ServiceID,
                });
            }

            var addonRequest = new AddonServiceStoreRequest()
            {
                UserID = App.User.ID,
                CouponID = CouponApplied == null ? null : (long?)CouponApplied.ID,
                Items = addonServiceDetails
            };

            var res = await APIService.StoreAddonService(addonRequest);

            if (res)
            {
                App.CartItems = new List<CartItem>();
                loadData();
                await DisplayAlert("", "payment is succeeded.", "ok");
            }
            else
            {
                await DisplayAlert("", "payment failed.", "ok");
            }
        }
    }
}