using EncriptionPractice.Helping_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EncriptionPractice.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostIndex(string Name = "")
        {
            string encoded = StringCipher.Base64Encode(Name);

            ViewBag.text = Name;
            ViewBag.entext = encoded;

            return View();
        }

        public ActionResult Decoder(string Name = "")
        {
            string decoded = StringCipher.Base64Decode(Name);

            ViewBag.text = decoded;

            return View();
        }


        public ActionResult PostIndex2(string Name = "", string Key = "")
        {
            string encoded = StringCipher.Encrypt(Name, Key);

            ViewBag.text = Name;
            ViewBag.entext = encoded;
            ViewBag.enKey = Key;

            return View();
        }

        public ActionResult Decoder2(string Name = "", string Key ="")
        {
            string decoded = StringCipher.Decrypt(Name, Key);

            ViewBag.text = decoded;

            return View();
        }

    }
}