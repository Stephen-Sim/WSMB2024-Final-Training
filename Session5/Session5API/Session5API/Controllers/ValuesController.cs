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
                Name = user.FullName
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
        public object IsAvailable(long id)
        {
            var service = ent.Services.First(x => x.ID == id);

            var date = new DateTime(2024, 7, 20);

            var nums = new List<int>();

            if (!string.IsNullOrWhiteSpace(service.DayOfMonth))
            {
                // day of month
                // * 
                if (service.DayOfMonth == "*")
                {
                    for (int i = 1; i <= 31; i++) { 
                        nums.Add(i);
                    }
                }
                else
                {
                    // split ,
                    // 1,2,3,4-6
                    var strs = service.DayOfMonth.Split(',');
                    // 1
                    // 2
                    // 3
                    // 44-66

                    foreach (var str in strs)
                    {
                        if (str.Contains('-'))
                        {
                            var days = str.Split('-');
                            for (int j = int.Parse(days[0]); j <= int.Parse(days[1]); j++)
                            {
                                nums.Add(j);
                            }
                        }
                        else
                        {
                            nums.Add(int.Parse(str));
                        }
                    }
                }

                return Ok(nums.Any(x => x == date.Day));
            }
            else
            {
                // * 
                if (service.DayOfMonth == "*")
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        nums.Add(i);
                    }
                }
                else
                {
                    // split ,
                    // 1,2,3,4-6
                    var strs = service.DayOfWeek.Split(',');
                    // 1
                    // 2
                    // 3
                    // 4-6

                    foreach (var str in strs)
                    {
                        if (str.Contains('-'))
                        {
                            var days = str.Split('-');
                            for (int j = int.Parse(days[0]); j <= int.Parse(days[1]); j++)
                            {
                                nums.Add(j);
                            }
                        }
                        else
                        {
                            nums.Add(int.Parse(str));
                        }
                    }
                }

                // 7 in nums then, remove and 0

                if (nums.Any(x => x == 7))
                {
                    nums.Remove(7);
                    nums.Add(0);
                }

                return Ok(nums.Any(x => x == (int)date.DayOfWeek));
            }

            return Ok(nums);
        }
    }
}
