using Session5APP.Models;
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
    public partial class ServiceSelectionPage : ContentPage
    {
        public ServiceType ServiceType { get; set; }

        public ServiceSelectionPage(ServiceType serviceType)
        {
            InitializeComponent();

            this.ServiceType = serviceType;

            this.Title = $"Seoul Stay - {ServiceType.Name}";
        }
    }
}