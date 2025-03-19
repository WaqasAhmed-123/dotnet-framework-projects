using ChatBox.BL;
using ChatBox.DataHub;
using ChatBox.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ChatBox.Controllers
{
    public class AuthController : Controller
    {
        public User validateUser()
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal; // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault(); 
            User loggedInUser = new UserBL().GetUserById(Convert.ToInt32(userId)); 
            
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
            User user = new UserBL().GetActiveUser().Where(x => x.Email == User.Email && x.Password == User.Password).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Login", new { loginErrMsg = "Incorrect email/password!" });
            }

            var identity = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                                
            }, "ApplicationCookie"); 
            
            var claimsPrincipal = new ClaimsPrincipal(identity); // Set current principal
            Thread.CurrentPrincipal = claimsPrincipal;
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);

            ////setting loggedin status
            //user.IsLoggedin = 1;
            //new UserBL().UpdateUser(user);
            ////setting loggedin status



            //var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //context.Clients.All.broadcastCheckLogin("1", user.Id);

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Registration(string msg = "")
        {
            ViewBag.Message = msg;
            return View();
        }

        public ActionResult PostRegistration(User _user)
        {
            User chk = new UserBL().GetActiveUser().Where(x => x.Email.ToLower() == _user.Email.ToLower()).FirstOrDefault();
            
            if(chk == null)
            {
                User u = new User()
                {
                    Name = _user.Name,
                    Email = _user.Email,
                    Password = _user.Password,
                    IsActive = 1,
                    CreatedAt = DateTime.Now
                };

                bool chk2 = new UserBL().AddUser(u);

                if (chk2 == false)
                {
                    return RedirectToAction("Registration", "Auth", new { msg = "All Fields Required!!" });
                }
                else
                {
                    return RedirectToAction("Login", "Auth", new { loginErrMsg = "Signup Successful, try Signin" });
                }
            }
            else
            {
                return RedirectToAction("Registration", "Auth", new { msg = "Email already exist, try sign in" });
            }

        }



        public ActionResult Logout()
        {
            User u = new UserBL().GetUserById(validateUser().Id);
            u.ConnectionId = null;
            new UserBL().UpdateUser(u);
            
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.broadcastCheckLogin("2", u.Id);

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");
            
            return RedirectToAction("Login");
        }

    }
}