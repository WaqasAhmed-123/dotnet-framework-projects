using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AudioRecording.Helping_Classes;

namespace AudioRecording.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveFile()
        {
            AudioData obj = new AudioData()
            {
                IsSaved = 0,
                Link = ""
            };

            try
            {
                if (Request.Files.Count > 0)
                {
                    //creating file directory if not exist
                    string path = Server.MapPath("~/Content/Clips/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //creating file directory if not exist


                    HttpPostedFileBase file = Request.Files[0];
                    string fileName = DateTime.Now.Ticks.ToString() + ".wav";

                    string filepath = Path.Combine(Server.MapPath("~/Content/Clips/"), fileName);

                    file.SaveAs(filepath);

                    obj.IsSaved = 1;
                    obj.Link = "../Content/Clips/" + fileName;

                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(obj, JsonRequestBehavior.AllowGet);
            }

        }



    }
}