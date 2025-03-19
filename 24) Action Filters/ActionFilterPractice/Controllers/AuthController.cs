using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ActionFilterPractice.Models;
using ActionFilterPractice.BL;
using ActionFilterPractice.Helping_Classes;
using System.Net.Mail;
using System.Net;

namespace ActionFilterPractice.Controllers
{
    [ValidationFilter(CheckLogin = false)] //filter class 
    public class AuthController : Controller
    {
        private readonly GeneralPurpose gp = new GeneralPurpose();
        

        public ActionResult Login(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        public ActionResult PostLogin(string Email = "", string Password = "")
        {
            User user = new UserBL().GetActiveUserList().Where(x => x.Email.Trim().ToLower() == Email.Trim().ToLower() && StringCipher.Decrypt(x.Password, "test").Equals(Password)).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Login", new { msg = "Incorrect Email/Password!", color = "red" });
            }

            var identity = new ClaimsIdentity(new[]
            {
                //new Claim(ClaimTypes.Name,user.FName),
                //new Claim(ClaimTypes.Surname,user.LName),

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

            if(user.Role == 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }


        #region Signup
        public ActionResult Register(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        public ActionResult PostRegister(User _user, string ConfirmPassword = "")
        {
            
            if (_user.Password != ConfirmPassword)
            {
                return RedirectToAction("Register", "Auth", new { msg = "Password and Confirm Password Didn't Match", color="red" });
            }

            bool checkEmail = gp.ValidateEmail(_user.Email);
            if (checkEmail == false)
            {
                return RedirectToAction("Register", "Auth", new { msg = "Email already Exist. Try Sign in!", color = "red" });
            }
            


            User u = new User()
            {
                Name = _user.Name,
                Contact = _user.Contact,
                Email = _user.Email,
                Password = StringCipher.Encrypt(_user.Password, "test"),
                Role = 2,
                IsActive = 1,
                CreatedAt = DateTime.Now
            };

            bool check = new UserBL().AddUser(u);

            if (check == true)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Signup successful, Please login", color = "green" });
            }
            else
            {
                return RedirectToAction("Register", "Auth", new { msg = "Somethings' Wrong!", color = "red" });
            }
        }
        #endregion


        #region Forgot Password
        public ActionResult ForgotPassword(string msg = "", string color="black")
        {
            ViewBag.Color = color;
            ViewBag.Message = msg;

            return View();
        }

        

        public ActionResult PostForgotPassword(string Email = "")
        {
            bool checkEmail = gp.ValidateEmail(Email);

            if (checkEmail == false)
            {
                string BaseUrl = string.Format("{0}://{1}{2}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority, "/");

                bool checkMAil = MailSender.SendForgotPasswordEmail(Email, BaseUrl);

                if (checkMAil == true)
                {
                    return RedirectToAction("Login", "Auth", new { msg = "Please check your inbox/spam", color = "green" });
                }
                else
                {
                    return RedirectToAction("ForgotPassword", "Auth", new { msg = "Mail sending fail!", color = "red" });
                }
            }
            else
            {
                return RedirectToAction("ForgotPassword", "Auth", new { msg = "Email does not belong to any record!!", color="red" });
            }

        }


        public ActionResult ResetPassword(string email = "", string time = "", string msg = "", string color ="black")
        {
            DateTime PassDate = Convert.ToDateTime(StringCipher.Base64Decode(time)).Date;
            DateTime CurrentDate = DateTime.Now.Date;

            if(CurrentDate != PassDate)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Link expired, Please try again!", color = "red" });
            }

            
            ViewBag.Time = time;
            ViewBag.Email = email;
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [HttpPost]
        public ActionResult PostResetPassword(string Email = "", string Time = "", string NewPassword = "", string ConfirmPassword = "")
        {
            if (NewPassword != ConfirmPassword)
            {
                return RedirectToAction("ResetPassword", "Auth", new { email = Email, time = Time, msg = "Password and confirm password did not match", color="red" });
            }

            string DecryptEmail = StringCipher.Base64Decode(Email);

            DatabaseEntities de = new DatabaseEntities();
            User user = new UserBL().GetActiveUserList(de).Where(x => x.Email.Trim().ToLower() == DecryptEmail.Trim().ToLower()).FirstOrDefault();

            user.Password = StringCipher.Encrypt(NewPassword, "test");

            bool check = new UserBL().UpdateUser(user, de);

            if (check == true)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Password Reset Successful, Try Login", color = "green" });
            }
            else
            {
                return RedirectToAction("ResetPassword", "Auth", new { email = Email, time = Time, msg = "Somethings Wrong!", color = "red" });
            }


        }

        #endregion


        #region Manage User Profile
        [ValidationFilter(CheckLogin = true)]
        public ActionResult UpdateUserPassword(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }
        
        [ValidationFilter(CheckLogin = true)]
        public ActionResult PostUpdateUserPassword(string oldPassword = "", string newPassword = "", string confirmPassword = "")
        {
            if (newPassword != confirmPassword)
            {
                return RedirectToAction("UpdateUserPassword", "Auth", new { msg = "New password and confirm password did not match!", color = "red" });
            }

            DatabaseEntities de = new DatabaseEntities();
            User u = new UserBL().GetUserById(gp.ValidateLoggedinUser().Id, de);

            if (StringCipher.Decrypt(u.Password, "test") != oldPassword)
            {
                return RedirectToAction("UpdateUserPassword", "Auth", new { msg = "Old password did not match!", color = "red" });
            }

            u.Password = StringCipher.Encrypt(newPassword, "test");

            bool chk = new UserBL().UpdateUser(u, de);

            if (chk == true)
            {
                return RedirectToAction("UpdateUserPassword", "Auth", new { msg = "Password updated successfully!", color = "green" });
            }
            else
            {
                return RedirectToAction("UpdateUserPassword", "Auth", new { msg = "Somthings' Wrong!", color = "red" });
            }
        }


        [ValidationFilter(CheckLogin = true)]
        public ActionResult UpdateProfile(string msg = "", string color = "black")
        {
            User u = new UserBL().GetUserById(gp.ValidateLoggedinUser().Id);

            ViewBag.User = u;
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [ValidationFilter(CheckLogin = true)]
        public ActionResult PostUpdateProfile(User _user)
        {
            bool checkEmail = gp.ValidateEmail(_user.Email, _user.Id);

            if (checkEmail == false)
            {
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Email used by someone else, Please try another", color = "red" });
            }

            DatabaseEntities de = new DatabaseEntities();
            User u = new UserBL().GetUserById(_user.Id, de);

            u.Name = _user.Name.Trim();
            u.Contact = _user.Contact.Trim();
            u.Email = _user.Email.Trim();

            bool chk = new UserBL().UpdateUser(u, de);

            if (chk == true)
            {
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Profile updated successfully!", color = "green" });
            }
            else
            {
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Somthings' Wrong!", color = "red" });
            }
            
        }
        #endregion


        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login");
        }
    }
}