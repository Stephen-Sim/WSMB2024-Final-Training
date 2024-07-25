using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public List<Attempt> Attempts { get; set; } = new List<Attempt>()
        {
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "non-expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "non-expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
            new Attempt(){username = "test test", role = "non-expert", attempt = 1, completion = "in progress (50%)", grade = "-", datetime = DateTime.Now, },
        };

        public FileContentResult downloa(int quizId, int typeId, bool isExpert)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] arr = null;

                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.WriteLine("{0},{1},{2},{3},{4}", "Username", "Attempt", "Completion", "Grade", "DateTime");

                    var attempts = Attempts.ToList();

                    foreach (var attempt in attempts)
                    {
                        sw.WriteLine("{0},{1},{2},{3},{4}", $"{attempt.username} ({attempt.role})",
                            attempt.attempt, attempt.completion, attempt.grade, 
                            attempt.datetime.ToString("dd MMMM yyyy h:mm tt"));
                    }
                }

                arr = ms.ToArray();

                return File(arr, "text/csv", "result.csv");
            }
        }
    }
}
