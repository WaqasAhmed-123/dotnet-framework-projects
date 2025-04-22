using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SendMail.Controllers
{
    public class HomeController : Controller
    {

        //first install "RestSharp" library from manager
        //then login/signup to following link
        //https://login.mailgun.com/login/
        
        


        public ActionResult Index()
        {
            string result = SendSimpleMessage();

            ViewBag.Response = result;

            return View();
        }

        public string SendSimpleMessage()
        {
            string filePath1 = Server.MapPath("~/Content/Files/sample1.xlsx");
            string filePath2 = Server.MapPath("~/Content/Files/sample2.png");


            string MailBody = "<html><head></head><body><nav class='navbar navbar-default'><div class='container-fluid'>" +
                    "</div> </nav><center><div><h1 class='text-center'>Password Reset!</h1>" +
                    "<p class='text-center'> Simply click the button showing below to reset your password (Link will expire after date change): </p><br>" +
                    "<button style='background-color: rgb(0,174,239);'>" +
                        "<a href='https://www.google.com.pk/' style='text-decoration:none;font-size:15px;color:white;'>Reset Password</a>" +
                    "</button>" +
                    "<p style='color:red;'>Link will not work in spam. Please move this mail into your inbox.</p></div></center>" +

                     "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";

            
            RestClient client = new RestClient(); //intializing Rest client object 
            client.BaseUrl = new Uri("https://api.mailgun.net/v3"); // this is base url (remains same)
            client.Authenticator = new HttpBasicAuthenticator("api", "key-api");  //copy Private Api Key from Api security (https://app.mailgun.com/app/account/security/api_keys)
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "nodlays.co.uk", ParameterType.UrlSegment);  //Create a new domain from side bar "Sending -> Domains -> Add New Domain"
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "fromemail@gmail.com"); //Can be used any email here, without any password requirements
            request.AddParameter("to", "toemail@gmail.com"); //email where you want to send mail
            request.AddParameter("subject", "Testing"); //subject of mail
            //request.AddParameter("text", "Testing some Mailgun awesomness!"); //sending simple text

            request.AddParameter("html", MailBody); //send html code generated above

            request.AddFile("attachment", Path.Combine("files", filePath1)); // send files through mail

            //request.AddFile("attachment", filePath1); // working code
            //request.AddFile("attachment", filePath2); // working code

            request.Method = Method.POST;

            //success responce is
            //"{\n  \"id\": \"<20210703124800.1.71449D708AD6F035@nodlays.co.uk>\",\n  \"message\": \"Queued. Thank you.\"\n}"
            string response = client.Execute(request).Content.ToString();
            
            return response;
        }

        
    }
}