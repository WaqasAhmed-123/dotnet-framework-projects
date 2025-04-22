using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AerotechDesign.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound(string msg = "")
        {
            ViewBag.Message = msg;

            return View();
        }
    }
}