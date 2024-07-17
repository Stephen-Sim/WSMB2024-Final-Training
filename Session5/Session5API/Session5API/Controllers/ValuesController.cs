using Session5API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Session5API.Controllers
{
    public class ValuesController : ApiController
    {
        public WSC2022SE_Session5Entities ent { get; set; }

        public ValuesController()
        {
            ent = new WSC2022SE_Session5Entities();
        }

        [HttpGet]
        public object Login(string u, string p)
        {
            var user = ent.Users.ToList()
                .FirstOrDefault(x => 
                    x.Username == u &&
                    x.Password == p &&
                    x.UserTypeID == 2);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(new
            {
                ID = user.ID,
                Name = user.FullName,
                user.FamilyCount
            });
        }

        [HttpGet]
        public object GetServiceTypes()
        {
            var sts = ent.ServiceTypes.ToList().Select(x => new 
            {
                x.ID,
                x.Name,
                IconName = x.IconName,
                x.Description
            });

            return Ok(sts);
        }

        [HttpGet]
        public object GetServices(long ID)
        {
            var services = ent.Services.ToList().Where(x => x.ServiceTypeID == ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Description,
                x.Price,
                x.ServiceType.IconName,
                x.DailyCap,
                x.BookingCap,
                x.DayOfMonth,
                x.DayOfWeek,
                x.Duration,

                Display = $"{x.Name} ({x.Duration} days) ${x.Price}"
            });

            return Ok(services); 
        }

        [HttpGet]
        public object GetRemainingSpot(long ID, DateTime dateTime)
        {
            var count = ent.Services.First(x => x.ID == x.ID).DailyCap -
                ent.AddonServiceDetails.ToList().Where(x => x.ServiceID == ID && x.FromDate == dateTime.Date).Select(x => new
                {
                    Count = new Func<long>(() =>
                    {
                        return x.NumberOfPeople / x.Service.BookingCap +
                         (x.NumberOfPeople % x.Service.BookingCap != 0 ? 1 : 0);
                    })()
                }).Sum(x => x.Count);

            return Ok(count);
        }

        [HttpGet]
        public object GetCoupons()
        {
            var c = ent.Coupons.ToList().Select(x => new {
                x.ID,
                x.CouponCode,
                x.DiscountPercent,
                x.MaximimDiscountAmount
            });

            return Ok(c);
        }

        [HttpPost]
        public object StoreAddonService(AddonServiceStoreRequest addonRequest)
        {
            var addonservice = new AddonService()
            {
                CouponID = addonRequest.CouponID,
                GUID = Guid.NewGuid(),
                UserID = addonRequest.UserID,
            };

            ent.AddonServices.Add(addonservice);

            foreach (var item in addonRequest.Items)
            {
                var detail = new AddonServiceDetail()
                {
                    AddonServiceID = addonservice.ID,
                    GUID = Guid.NewGuid(),
                    FromDate = item.FromDate,
                    Notes = item.Note,
                    Price = item.Price,
                    NumberOfPeople = item.NOP,
                    ServiceID = item.ServiceID,
                    isRefund = false
                };

                ent.AddonServiceDetails.Add(detail);
            }

            ent.SaveChanges();

            return Ok();
        }

        public class AddonServiceStoreRequest
        {
            public long UserID { get; set; }
            public long? CouponID { get; set; }

            public List<AddonServiceDetailStoreRequest> Items { get; set; } = new List<AddonServiceDetailStoreRequest>();
        }

        public class AddonServiceDetailStoreRequest
        {
            public long ServiceID { get; set; }
            public DateTime FromDate { get; set; }
            public decimal Price { get; set; }
            public string Note { get; set; }
            public int NOP { get; set; }
        }
    }
}
