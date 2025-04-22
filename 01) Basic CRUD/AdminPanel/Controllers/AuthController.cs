using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.err = "none";

            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult Login(string name="", string pw="")
        {
            if(name=="admin" && pw=="admin")
            {
                return RedirectToAction("Index", "Default");
            }
            else
            {
                ViewBag.err = "inline";
            }
            return View();
        }
    }
}