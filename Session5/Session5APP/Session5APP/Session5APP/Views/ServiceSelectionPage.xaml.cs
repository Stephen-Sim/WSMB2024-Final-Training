using Session5APP.Models;
using Session5APP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Session5APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceSelectionPage : ContentPage
    {
        public ServiceType ServiceType { get; set; }

        public ServiceSelectionPage(ServiceType serviceType)
        {
            InitializeComponent();

            CartLabel.Text = $"Cart ({App.CartItems.Count})";

            this.ServiceType = serviceType;

            APIService = new APIService();

            this.Title = $"Seoul Stay - {ServiceType.Name}";

            cartFormStacklayout.IsVisible = false;

            loadData();
        }

        public APIService APIService { get; set; }

        async void loadData()
        {
            stDescLabel.Text = this.ServiceType.Description;

            var res = await APIService.GetServices(this.ServiceType.ID);

            res.ForEach(x => { x.IconImage = ImageSource.FromResource($"Session5APP.Images.{x.IconName}"); });

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

        public Service SelectedService { get; set; }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            cartFormStacklayout.IsVisible = true;

            isDateSelected = false;
            spotLabel.Text = string.Empty;
            additionalNoteEntry.Text = string.Empty;

            SelectedService = (sender as Grid).BindingContext as Service;

            sNameLabel.Text = $"Description of \"{SelectedService.Name}\"";
            sDescLabel.Text = this.SelectedService.Description;

            nopEntry.Text = App.User.FamilyCount.ToString();

            var nop = App.User.FamilyCount;

            var bookingcap = (nop / this.SelectedService.BookingCap) 
                + (nop % this.SelectedService.BookingCap != 0 ? 1 : 0);

            price = this.SelectedService.Price * bookingcap;

            PriceLabel.Text = $"Amount payable: ${price.ToString("0.00")}";
        }

        private void nopEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nopEntry.Text == string.Empty)
            {
                return;
            }

            var nop = int.Parse(nopEntry.Text);

            var bookingcap = (nop / this.SelectedService.BookingCap)
                + (nop % this.SelectedService.BookingCap != 0 ? 1 : 0);

            bookingCapLabel.Text = $"in {bookingcap} bookings.";

            price = this.SelectedService.Price * bookingcap;

            PriceLabel.Text = $"Amount payable: ${price.ToString("0.00")}";
        }

        decimal price = 0.0m;

        bool isDateSelected = false;

        private async void selectedDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (selectedDatePicker.Date <= DateTime.Today)
            {
                await DisplayAlert("", "you could not select the date before.", "Ok");
                isDateSelected = false;
                return;
            }

            if (!isAvailable())
            {
                await DisplayAlert("", "the date is not availale on the selected date.", "Ok");
                isDateSelected = false;
                return;
            }

            var res = await APIService.GetRemainingSpot(this.SelectedService.ID, this.selectedDatePicker.Date);

            if (res <= 0)
            {
                await DisplayAlert("", "the daily cap reaches the limit.", "Ok");
                isDateSelected = false;
                return;
            }

            spotLabel.Text = $"Remaining: {res} spots";
            isDateSelected = true;
        }

        bool isAvailable()
        {
            var nums = new List<int>();

            var service = this.SelectedService;

            var date = selectedDatePicker.Date;

            if (!string.IsNullOrWhiteSpace(service.DayOfMonth))
            {
                // day of month
                // * 
                if (service.DayOfMonth == "*")
                {
                    for (int i = 1; i <= 31; i++)
                    {
                        nums.Add(i);
                    }
                }
                else
                {
                    // split ,
                    // 1,2,3,4-6
                    var strs = service.DayOfMonth.Split(',');
                    // 1
                    // 2
                    // 3
                    // 44-66

                    foreach (var str in strs)
                    {
                        if (str.Contains('-'))
                        {
                            var days = str.Split('-');
                            for (int j = int.Parse(days[0]); j <= int.Parse(days[1]); j++)
                            {
                                nums.Add(j);
                            }
                        }
                        else
                        {
                            nums.Add(int.Parse(str));
                        }
                    }
                }

                if (!nums.Any(x => x == (int)date.DayOfWeek))
                {
                    return false;
                }
            }
            else
            {
                // * 
                if (service.DayOfMonth == "*")
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        nums.Add(i);
                    }
                }
                else
                {
                    // split ,
                    // 1,2,3,4-6
                    var strs = service.DayOfWeek.Split(',');
                    // 1
                    // 2
                    // 3
                    // 4-6

                    foreach (var str in strs)
                    {
                        if (str.Contains('-'))
                        {
                            var days = str.Split('-');
                            for (int j = int.Parse(days[0]); j <= int.Parse(days[1]); j++)
                            {
                                nums.Add(j);
                            }
                        }
                        else
                        {
                            nums.Add(int.Parse(str));
                        }
                    }
                }

                // 7 in nums then, remove and 0

                if (nums.Any(x => x == 7))
                {
                    nums.Remove(7);
                    nums.Add(0);
                }

                if (!nums.Any(x => x == (int)date.DayOfWeek))
                {
                    return false;
                }
            }

            return true;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // add to cart
            if (isDateSelected == false)
            {
                await DisplayAlert("", "The date is not selected yet.", "Ok");
                return;
            }

            if (string.IsNullOrEmpty(nopEntry.Text) || string.IsNullOrEmpty(additionalNoteEntry.Text))
            {
                await DisplayAlert("", "All the fields are required", "Ok");
                return;
            }

            var cartItem = new CartItem()
            {
                // display
                DisplayFromDate = $"From: {selectedDatePicker.Date.ToString("dd/MM/yyyy")} To: {selectedDatePicker.Date.AddDays(SelectedService.Duration).ToString("dd/MM/yyyy")}",
                DisplayName = this.SelectedService.Name,
                DisplayNOP = $"Number of People: {int.Parse(nopEntry.Text)}",
                Icon = SelectedService.IconImage,

                // store
                ServiceID = this.SelectedService.ID,   
                NOP = int.Parse(nopEntry.Text),
                Note = additionalNoteEntry.Text,
                Price = price,
            };

            App.CartItems.Add(cartItem);

            await DisplayAlert("", "service is added.", "Ok");

            App.Current.MainPage = new NavigationPage(new AddonServiceMenuPage());
        }
    }
}