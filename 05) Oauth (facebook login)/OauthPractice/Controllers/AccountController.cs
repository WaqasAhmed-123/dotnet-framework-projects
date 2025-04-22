using Facebook;
using ForgotPassword.BL;
using OauthPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OauthPractice.Controllers
{
    public class AccountController : Controller
    { 
        //first we need to install Facebook NuGet Package from tools
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }


        [AllowAnonymous]
        public ActionResult login()
        {
            return View();
        }

        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return View("Login");
        }


        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "your client id",
                client_secret = "client secret",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });

            return Redirect(loginUrl.AbsoluteUri);
        }


        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "your client id",
                client_secret = "client secret",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;

            // Store the access token in the session for farther use
            Session["AccessToken"] = accessToken;

            // update the facebook client with the access token so
            // we can make requests on behalf of the user
            fb.AccessToken = accessToken;

            // Get the user's information, like email, first name, middle name etc
            dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
            string email = me.email;
            string firstname = me.first_name;
            string middlename = me.middle_name;
            string lastname = me.last_name;
            
            // Set the auth cookie
            FormsAuthentication.SetAuthCookie(email, false);

            string name = firstname + " " + lastname;

            User u = new User() { Name=name, Email=email, Password="null", IsActive=1, CreatedAt= DateTime.Now };

            bool c = new UserBL().AddUser(u);
            return RedirectToAction("Index", "Home");
        }

    }
}