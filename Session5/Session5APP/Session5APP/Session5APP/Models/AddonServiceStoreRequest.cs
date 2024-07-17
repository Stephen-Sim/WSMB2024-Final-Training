using System;
using System.Collections.Generic;
using System.Text;

namespace Session5APP.Models
{
    public class AddonServiceStoreRequest
    {
        public long UserID { get; set; }
        public long? CouponID { get; set; }

        public List<AddonServiceDetailStoreRequest> Items { get; set; } = new List<AddonServiceDetailStoreRequest>();
    }
}
