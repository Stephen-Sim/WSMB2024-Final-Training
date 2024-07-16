using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Session5APP.Models
{
    public class CartItem
    {
        // display
        public ImageSource Icon { get; set; }
        public string DisplayName { get; set; }
        public string DisplayFromDate { get; set; }
        public string DisplayNOP { get; set; }

        // store request
        public long ServiceID { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public int NOP { get; set; }
    }
}
