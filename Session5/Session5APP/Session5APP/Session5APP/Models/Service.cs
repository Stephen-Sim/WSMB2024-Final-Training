using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Session5APP.Models
{
    public class Service
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long Duration { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string IconName { get; set; }
        public long DailyCap { get; set; }
        public long BookingCap { get; set; }
        public string DayOfMonth { get; set; }
        public string DayOfWeek { get; set; }
        public string Display { get; set; }
        public ImageSource IconImage{ get; set; }
    }
}
