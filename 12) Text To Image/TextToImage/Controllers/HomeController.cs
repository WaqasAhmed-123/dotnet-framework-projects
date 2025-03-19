using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TextToImage.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult GenerateBarCode(string barcode)
        {

            using (MemoryStream memoryStream = new MemoryStream())
            {
                int width = -1;
                //setting the width of image
                if(barcode.Length < 5)
                {
                    if (barcode.Length == 1)
                    {
                        width = barcode.Length * 30;
                    }
                    else
                    {
                        width = barcode.Length * 20;
                    }
                }
                else
                {
                    width = barcode.Length * 15;
                }

                using (Bitmap bitMap = new Bitmap(width, 40)) //40 is the height of image
                {
                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        Font oFont = new Font("IDAutomationHC39M", 16);
                        PointF point = new PointF(2f, 2f);
                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                        SolidBrush blackBrush = new SolidBrush(Color.DarkBlue);
                        graphics.DrawString(barcode, oFont, blackBrush, point);
                    }

                    bitMap.Save(memoryStream, ImageFormat.Jpeg);

                    ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            return View();
        }

    }
}