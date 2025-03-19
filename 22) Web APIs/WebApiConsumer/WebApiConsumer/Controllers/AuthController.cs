using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApiConsumer.Helping_Classes;

namespace WebApiConsumer.Controllers
{
    public class AuthController : Controller
    {
        private readonly General_Purpose gp = new General_Purpose();

        public ActionResult Login(string msg = "", string color = "black")
        {

            if (gp.ValidateUser() != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }


        public ActionResult PostLogin(UserDTO _user)
        {
            UserDTO udto = new UserDTO();

            //get help from the following link
            //https://www.aspsnippets.com/Articles/Call-Consume-Web-API-using-HttpClient-in-ASPNet-C.aspx

            using (var client = new HttpClient())
            {
                string BaseApiUrl = "https://localhost:44350/api/LoginAPI";
                
                string inputJson = (new JavaScriptSerializer()).Serialize(_user);
                HttpContent PassingContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(BaseApiUrl + "/PostValidateLogin", PassingContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    udto = (new JavaScriptSerializer()).Deserialize<UserDTO>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //string a = Convert.ToString(response.StatusCode);
                    return RedirectToAction("Login", "Auth", new { msg = "Invalid Email/Password", color = "red" });
                }
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid,udto.Id.ToString()),
                new Claim(ClaimTypes.Name,udto.Name),
                new Claim(ClaimTypes.Email,udto.Email),
                new Claim(type: "AccessToken", value: udto.AccessToken)

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
            
            using (var client = new HttpClient())
            {
                string BaseApiUrl = "https://localhost:44350/api/LoginAPI";
                
                //string inputJson = (new JavaScriptSerializer()).Serialize(gp.ValidateUser());
                //HttpContent PassingContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.GetAsync(BaseApiUrl + "/PostLogout?Id=" + gp.ValidateUser().Id.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    //var readTask = response.Content.ReadAsStringAsync();
                    //readTask.Wait();

                    //string a = readTask.Result;
                    //return RedirectToAction("Index", "Home", new { msg = a, color = "red" });

                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;
                    authManager.SignOut("ApplicationCookie");
                    return RedirectToAction("Login");
                }
                else
                {
                    //string a = Convert.ToString(response.StatusCode);
                    return RedirectToAction("Index", "Home", new { msg = "Api Failed", color = "red" });
                }
            }

        }
    }
}