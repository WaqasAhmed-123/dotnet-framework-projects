using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace UploadLargeFile.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ProcessRequest()
        {

            HttpFileCollectionBase files = Request.Files;
            int TotalFiles = Request.Files.Count;
            HttpPostedFileBase PostedFile = files[0];
            
            string fileExt = Path.GetExtension(PostedFile.FileName);

            
            string fileName = "Sample" + DateTime.Now.Ticks.ToString() + fileExt;


            fileName = Path.Combine(Server.MapPath("../Content/LargeFiles/"), fileName);

            PostedFile.SaveAs(fileName);

            return Json(Path.GetFileNameWithoutExtension(PostedFile.FileName), JsonRequestBehavior.AllowGet);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}