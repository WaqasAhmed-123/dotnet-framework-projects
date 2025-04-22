using MigraDoc.DocumentObjectModel.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;

namespace BarCodePractice.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //Goto tools-> Nuget Packet Manager -> Search "zxing.net" and install it
        public ActionResult RenderBarcode(string barcode)
        {
            System.Drawing.Image img = null;

            using (var ms = new MemoryStream())
            {
                var writer = new ZXing.BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
                writer.Options.Height = 80;
                writer.Options.Width = 280;
                writer.Options.PureBarcode = true;
                img = writer.Write(barcode);
                img.Save(ms, ImageFormat.Jpeg);


                //ViewBag.BarCode = File(ms.ToArray(), "image/jpeg");
                ViewBag.BarCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

            }

                return View();
        }



        public ActionResult BarCodeRead()
        {
            return View();
        }


        //[HttpPost]
        //public ActionResult BarCodeRead(HttpPostedFileBase barCodeUpload)
        //{
        //    String localSavePath = "~/UploadFiles/";
        //    string str = string.Empty;
        //    string strImage = string.Empty;
        //    string strBarCode = string.Empty;

        //    if (barCodeUpload != null)
        //    {
        //        String fileName = barCodeUpload.FileName;
        //        localSavePath += fileName;
        //        barCodeUpload.SaveAs(Server.MapPath(localSavePath));

        //        Bitmap bitmap = null;
        //        try
        //        {
        //            bitmap = new Bitmap(barCodeUpload.InputStream);
        //        }
        //        catch (Exception ex)
        //        {
        //            ex.ToString();
        //        }

        //        if (bitmap == null)
        //        {

        //            str = "Your file is not an image";

        //        }
        //        else
        //        {
        //            strImage = "http://localhost:" + Request.Url.Port + "/UploadFiles/" + fileName;

        //            strBarCode = ReadBarcodeFromFile(Server.MapPath(localSavePath));

        //        }
        //    }
        //    else
        //    {
        //        str = "Please upload the bar code Image.";
        //    }
        //    ViewBag.ErrorMessage = str;
        //    ViewBag.BarCode = strBarCode;
        //    ViewBag.BarImage = strImage;
        //    return View();
        //}
        //private String ReadBarcodeFromFile(string _Filepath)
        //{

        //    String[] barcodes = BarcodeScanner.Scan(_Filepath, BarcodeType.Code39);
        //    return barcodes[0];
        //}

    }
}