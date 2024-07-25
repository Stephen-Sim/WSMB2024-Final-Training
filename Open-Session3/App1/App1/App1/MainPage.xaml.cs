using App1.Models;
using App1.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            initLoad();

            loadURLLabel();
            loadDataAsync();
        }

        async void loadDataAsync()
        {
            var res = await ApiService<List<Attempt>>.Get($"GetData?quizid={0}" +
                $"&typeid={(typePicker.SelectedItem as Temp).id}" +
                $"&isExpert={isExpertCheckbox.IsChecked}");

            lv.ItemsSource = res;
        }

        void initLoad()
        {
            typePicker.ItemsSource = Types;
            typePicker.ItemDisplayBinding = new Binding("name");
            typePicker.SelectedIndex = 0;
        }

        void loadURLLabel()
        {
            // check type picker or quiz picker is null
            if (typePicker.SelectedItem == null)
            {
                return;
            }

            var text = $"Download excel report at http://10.131.76.121:8090/home/" +
                $"download?quizid={0}" +
                $"&typeid={(typePicker.SelectedItem as Temp).id}" +
                $"&isExpert={isExpertCheckbox.IsChecked}";
            urlLabel.Text = text;
        }

        public List<Temp> Types { get; set; } = new List<Temp>()
        {
            new Temp(){id = 1, name="All attempts"},
            new Temp(){id = 2, name="Attempts completed within the last 30 days"},
            new Temp(){id = 3, name="Attempts completed within the last 60 days"},
            new Temp(){id = 4, name="Attempts completed within the last 90 days"},
            new Temp(){id = 5, name="Attempts in progress"},
            new Temp(){id = 6, name="Not attempted"},
        };

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadURLLabel();
            loadDataAsync();
        }

        private void isExpertCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            loadURLLabel();
            loadDataAsync();
        }
    }
}
