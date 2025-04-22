using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UploadLargeFile.Controllers
{
    public class UploadController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult UploadFileChunks()
        {
            var files = Request.Files;

            if (files.Count > 0)
            {
                try
                {
                    string filePath = Path.Combine(GetUploadPath(), files[0].FileName);

                    using (FileStream fs = new FileStream(filePath, FileMode.Append))
                    {
                        var bytes = GetBytes(files[0].InputStream);
                        fs.Write(bytes, 0, bytes.Length);
                    }

                    return Json(new { status = true });
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, message = ex.Message });
                }
            }

            return Json(new { status = false });
        }

        private byte[] GetBytes(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        private string GetUploadPath()
        {
            var rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/UploadedFiles/");

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            return rootPath;
        }
    }
}