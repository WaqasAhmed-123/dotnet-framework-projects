using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;

namespace QRCodeReader.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

       

        public ActionResult ReadQr()
        {
            string barcodeText = "";
            string imagePath = "~/Content/testQr.jpg";
            string barcodePath = Server.MapPath(imagePath);
            var barcodeReader = new BarcodeReader();

            var result = barcodeReader.Decode(new Bitmap(barcodePath));
            if (result != null)
            {
                barcodeText = result.Text;
            }

            ViewBag.Path = "../Content/testQr.jpg";
            ViewBag.Text = barcodeText;

            return View();
        }

        public ActionResult ReadBar()
        {
            string barcodeText = "";
            string imagePath = "~/Content/barcode.png";
            string barcodePath = Server.MapPath(imagePath);
            var barcodeReader = new BarcodeReader();

            var result = barcodeReader.Decode(new Bitmap(barcodePath));
            if (result != null)
            {
                barcodeText = result.Text;
            }

            ViewBag.Path = "../Content/barcode.png";
            ViewBag.Text = barcodeText;

            return View();
        }
    }
}