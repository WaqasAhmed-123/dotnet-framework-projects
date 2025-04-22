using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeCreator.Models;
using CodeCreator.BL;
using CodeCreator.Helping_Classes;

namespace CodeCreator.Controllers
{
    [ValidationFilter(Role = 1)]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}