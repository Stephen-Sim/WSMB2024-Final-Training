using System;
using System.Collections.Generic;
using System.Text;

namespace Session5APP.Models
{
    public class Coupon
    {
        public long ID { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal MaximimDiscountAmount { get; set; }
    }
}
