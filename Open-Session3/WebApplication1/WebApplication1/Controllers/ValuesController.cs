using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        public List<Attempt> Attempts { get; set; } = new List<Attempt>() 
        {
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
        };

        [HttpGet]
        public object GetData(int quizId, int typeId, bool isExpert)
        {
            return Attempts;
        }
    }
}
