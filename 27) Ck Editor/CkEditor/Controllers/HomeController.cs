using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkEditor.Models;
using CkEditor.BL;

namespace CkEditor.Controllers
{
    public class HomeController : Controller
    {
        DatabaseEntities de = new DatabaseEntities();


        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateHtml(string msg="")
        {
            Page page = new PageBL().GetPage("page", de);
            if (page == null)
            {
                page = AddUser();
            }

            ViewBag.Message = msg;
            ViewBag.Page = page;

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult PostUpdateHtml(Page _page)
        {
            Page page = new PageBL().GetPageById(_page.Id, de);

            page.Tags = _page.Tags;

            bool chkPage = new PageBL().UpdatePage(page, de);

            if (chkPage)
            {
                return RedirectToAction("UpdateHtml", "Home",  new { msg = "Page Updated Successfully"});
            }
            else
            {
                return RedirectToAction("UpdateHtml", "Home", new { msg = "Somethings' wrong" });
            }
            
        }

        public ActionResult ViewHtml(string msg="")
        {
            Page page = new PageBL().GetPage("page", de);
            if(page == null)
            {
                page = AddUser();
            }

            ViewBag.Message = msg;
            ViewBag.Page = page;

            return View();
        }

        public Page AddUser()
        {
            Page obj = new Page()
            {
                Title = "page",
                Tags = "<b>Hello</b>",
                IsActive = 1,
                CreatedAt = DateTime.Now
            };

            bool chkPage = new PageBL().AddPage(obj, de);

            return obj;
        }
    }
}