using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Mvc;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;
using Twilio.Rest.Api.V2010.Account.Message;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace TwilioMessages.Controllers
{
    public class HomeController : TwilioController
    {
        // GET: Home
        public ActionResult SendSms()
        {
            string id="";
            string body="";
            string from="";
            string to="";
            string status="";
            string created="";
            string uri="";
            string med="";
            string content="";
            string direc="";

            string accountSid = "your accountSid";
            string authToken = "your authToken";

            TwilioClient.Init(accountSid, authToken);

            var messages = MessageResource.Read(limit: 9999);

            foreach (var record in messages)
            {
                id = record.Sid;
                body = record.Body;
                from = record.From.ToString();
                to = record.To;
                status = record.Status.ToString();
                created = Convert.ToDateTime(record.DateSent).ToString("MM/dd/yyyy");
                uri = record.Uri;
                med = record.NumMedia;
                direc = record.Direction.ToString();

                if(med == "1")
                {

                    //var media = MediaResource.Fetch(
                    //    pathMessageSid: record.Sid,
                    //    pathSid: record.AccountSid
                    //);
                    //content = "https://api.twilio.com/2010-04-01/Accounts/"+record.AccountSid+"/Messages/"+record.Sid+"/Media/" + media.Sid + ".json";
                    if (body.Contains("\n"))
                    {
                        string[] tokens = body.Split('\n');

                        content = tokens[1];
                    }
                }


            }

            return View();
        }


        

        [HttpPost]
        public ActionResult PostSendSms(string Name = "waqas", string Contact = "+923034116151", string Message = "hello")
        {
            string msg = "";

            if (ModelState.IsValid)
            {
                try
                {
                    // Find your Account Sid and Auth Token at twilio.com/user/account  
                    const string accountSid = "your accountSid";
                    const string authToken = "your authToken";
                    TwilioClient.Init(accountSid, authToken);

                    var mediaUrl = new[]
                    {
                        new Uri("http://nodlayslahore-001-site6.btempurl.com/Content/assets/images/signature.png")
                    }.ToList();


                    var to = new PhoneNumber(Contact);
                    var message = MessageResource.Create(
                        to,
                        from: new PhoneNumber("+1111111111"), //  From number, must be an SMS-enabled Twilio number ( This will send sms from ur "To" numbers ).  
                        body: $" {Message}, Sender # {Name}",
                        mediaUrl: mediaUrl
                        );

                    ModelState.Clear();

                    msg = "Sms Sent Successfully";
                }
                catch (Exception ex)
                {
                    msg = "Sms Sending fail";

                    //Console.WriteLine($" Registration Failure : {ex.Message} ");
                }

            }

            return RedirectToAction("Index", "Home", new { msg = msg });
        }

        [HttpPost]
        public TwiMLResult Index()
        {
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message("The Robots are coming! Head for the hills!");

            return TwiML(messagingResponse);
        }

        public ActionResult ReceiveSms(string msg="")
        {
            ViewBag.Msg = msg;

            return View();
        }

        [HttpPost]
        public ActionResult PostReceiveSms()
        {
            string receiveMsg = "";
            var requestBody = Request.Form["Body"];

            receiveMsg = requestBody;

            return RedirectToAction("ReceiveSms", "Home", new { msg = receiveMsg });
        }
    }
}