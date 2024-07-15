using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Session5APP.Models
{
    public class ServiceType
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconName { get; set; }
        public ImageSource IconImage { get; set; } = null;
    }
}
