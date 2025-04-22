using OwinPractice.BL;
using OwinPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace OwinPractice.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Registration(string msg="")
        {
            ViewBag.Message = msg;
            return View();
        }

        public ActionResult PostRegistration(User _user)
        {
            User u = new User()
            {
                Name = _user.Name,
                Contact = _user.Contact,
                Email = _user.Email,
                Address = _user.Address,
                Password = _user.Password,
                Role = _user.Role,
                IsActive = 1,
                CreatedAt = DateTime.Now
            };

            bool check = new UserBL().AddUser(u);

            if (check == false)
            {
                return RedirectToAction("Registration", "Auth", new { msg = "All Fields Required!!" });
            }
            else
            {
                return RedirectToAction("Login", "Auth", new { loginErrMsg = "Signup Successful, try Signin" });
            }
        }

        public User validateUser()
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
           
            // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid)
                  .Select(c => c.Value).SingleOrDefault();
            User loggedInUser = new UserBL().GetUserbyId(Convert.ToInt32(userId));
           
            return loggedInUser;
        }


        public ActionResult Login(string loginErrMsg = "", string passSet = "", string loginSuccessMsg = "")
        {
            if (validateUser() != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ConfirmPasswordUpdate = passSet;
            ViewBag.ConfirmloginErrMsg = loginErrMsg;
            ViewBag.ConfirmloginSuccessMsg = loginSuccessMsg;
            return View();
        }
        public ActionResult PostLogin(User User)
        {
            User user = new UserBL().GetActiveUsersList().Where(x => x.Email == User.Email && x.Password == User.Password).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Login", new { loginErrMsg = "Incorrect email/password!" });
            }
            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,user.Name),
                     new Claim(ClaimTypes.Email,user.Email),
                     new Claim(ClaimTypes.Sid,user.Id.ToString()),
                     new Claim("CreatedAt", user.CreatedAt.ToString()),
                     new Claim("Role", user.Role.ToString()),
                   }, "ApplicationCookie");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            // Set current principal
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
            return RedirectToAction("Login");
        }

    }
}