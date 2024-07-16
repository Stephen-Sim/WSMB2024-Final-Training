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
    }
}
