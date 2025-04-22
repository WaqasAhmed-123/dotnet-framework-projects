using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScreenRecoder.Helping_Classes;

namespace ScreenRecoder.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        //Update following Code in Web.config file in order to handle larger files
        //<system.web>
        //    <compilation debug="true" targetFramework="4.7.2" />
        //    <httpRuntime targetFramework="4.7.2" maxRequestLength="1073741824" executionTimeout="3600"/>
        //</system.web>

        //<system.webServer>
        //    <security>
        //        <requestFiltering>
	       //         <requestLimits maxAllowedContentLength="1073741824" />
        //        </requestFiltering>
        //    </security>
        //</system.webServer>

        [HttpPost]
        public ActionResult SaveFile()
        {
            VideoData obj = new VideoData()
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
                    string fileName = DateTime.Now.Ticks.ToString() + ".mp4";

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


        public ActionResult Record()
        {
            return View();
        }

    }
}