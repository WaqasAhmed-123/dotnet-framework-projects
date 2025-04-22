using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoogleMapPractice.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Location = "gulberg lahore";
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(string Location="")
        {
            ViewBag.Location = Location;
            return View();
        }
    }
}