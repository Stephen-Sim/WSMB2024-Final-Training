using System;
using System.Collections.Generic;
using System.Text;

namespace Session5APP.Models
{
    public class AddonServiceDetailStoreRequest
    {
        public long ServiceID { get; set; }
        public DateTime FromDate { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public int NOP { get; set; }
    }
}
