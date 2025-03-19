using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaptureImage.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostIndex(String ImgUrl = "")
        {

            byte[] imageBytes = Convert.FromBase64String(ImgUrl.Substring(23));
            
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

            image.Save(Server.MapPath("~/Images/Hello.jpg"));

            ViewBag.Img = ImgUrl;

            return View();
        }


        //public System.Drawing.Image Base64ToImage(String ImgUrl = "")
        //{
        //    byte[] imageBytes = Convert.FromBase64String(ImgUrl);
        //    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
        //    ms.Write(imageBytes, 0, imageBytes.Length);
        //    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
        //    return image;
        //}

        //public ActionResult BaseToImage_Click(String ImgUrl = "")
        //{
        //    Base64ToImage(ImgUrl.Substring(23)).Save(Server.MapPath("~/Images/Hello.jpg"));

        //    return RedirectToAction("Index", "Home");
        //}


    }
}