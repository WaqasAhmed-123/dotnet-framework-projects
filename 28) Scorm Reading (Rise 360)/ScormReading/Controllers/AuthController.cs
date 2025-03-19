using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScormReading.Models;
using ScormReading.Helping_Classes;
using ScormReading.BL;
using System.Security.Claims;
using System.Threading;

namespace ScormReading.Controllers
{
    public class AuthController : Controller
    {
        private GeneralPurpose gp = new GeneralPurpose();
        private DatabaseEntities de = new DatabaseEntities();

        public ActionResult Login(string msg = "", string color = "black")
        {
            if (gp.ValidateLoggedinUser() != null)
            {
                return RedirectToAction("Index", "Home");
            }

            int userCount = new UserBL().GetActiveUsersList(de).Count();
            if (userCount == 0)
            {
                User obj = new User()
                {
                    Name = "Uzair Aslam",
                    Email = "uzair.aslam02@gmail.com",
                    Password = "123",
                    Role = 1,
                    IsActive = 1,
                    CreatedAt = GeneralPurpose.DateTimeNow()
                };

                new UserBL().AddUser(obj, de);
            }

            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }


        [HttpPost]
        public ActionResult PostLogin(string Email = "", string Password = "")
        {
            User user = new UserBL().GetActiveUsersList(de).Where(x => x.Email.Trim().ToLower() == Email.Trim().ToLower() && x.Password == Password).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Login", new { msg = "Incorrect Email/Password!", color = "red" });
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim("UserName", user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("Role", user.Role.ToString()),
            }, "ApplicationCookie");

            var claimsPrincipal = new ClaimsPrincipal(identity); // Set current principal
            Thread.CurrentPrincipal = claimsPrincipal;
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(identity);

            return RedirectToAction("Index", "Home");
        }



        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Auth");
        }
    }
}