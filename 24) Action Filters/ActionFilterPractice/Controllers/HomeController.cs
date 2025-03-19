using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActionFilterPractice.Helping_Classes;

namespace ActionFilterPractice.Controllers
{
    [ValidationFilter]
    public class HomeController : Controller
    {
        [ValidationFilter(Role = 2)]
        public ActionResult Index(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }


        public ActionResult ContactUs(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }


        public ActionResult About(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }
    }
}