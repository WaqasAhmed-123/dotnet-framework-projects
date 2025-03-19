using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ActionFilterPractice.Helping_Classes
{
    public class MailSender
    {
        public static bool SendForgotPasswordEmail(string email, string BaseUrl="")
        {
            try
            {
                MailMessage msg = new MailMessage();
                string text = "<link href='https://fonts.googleapis.com/css?family=Bree+Serif' rel='stylesheet'><style>  * {";
                text += "  font-family: 'Bree Serif', serif; }";
                text += " .list-group-item {       border: none;  }    .hor {      border-bottom: 5px solid black;   }";
                text += " .line {       margin-bottom: 20px; }";
                msg.From = new MailAddress("no.replay.nodlays@gmail.com");
                msg.To.Add(email);
                msg.Subject = "Test | Password Reset";
                msg.IsBodyHtml = true;
                string temp = "<html><head></head><body><nav class='navbar navbar-default'><div class='container-fluid'>" +
                    "</div> </nav><center><div><h1 class='text-center'>Password Reset!</h1>" +
                    "<p class='text-center'> Simply Click the button showing below to reset your password (Link will expire after date change): </p><br>" +
                    "<button style='background-color: rgb(0,174,239);'>" +
                        "<a href='" + BaseUrl + "Auth/ResetPassword?email=" + StringCipher.Base64Encode(email) + "&time=" + StringCipher.Base64Encode(DateTime.Now.ToString("MM/dd/yyyy")) + "' style='text-decoration:none;font-size:15px;color:white;'>Reset Password</a>" +
                    "</button>" +
                    "<p style='color:red;'>Link will not work in spam. Please move this mail into your inbox.</p></div></center>";

                temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
                //string link = "https://localhost:44363/Auth/ResetPassword?email=" + StringCipher.Encrypt(email, "test") + "&time=" + StringCipher.Encrypt(DateTime.Now.ToString(), "test");
                //link = link.Replace("+", "%20");
                //temp = temp.Replace("LINKFORFORGOTPASSWORD", link);
                msg.Body = temp;

                //Following method is used when other servers then gmail
                //using (SmtpClient client = new SmtpClient())
                //{
                //    client.EnableSsl = false;
                //    client.UseDefaultCredentials = false;
                //    client.Credentials = new NetworkCredential("info@nodlays.com", "delta@O27");
                //    client.Host = "webmail.nodlays.com";
                //    client.Port = 25;
                //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //    client.Send(msg);
                //}

                //following method is used on gmail server 
                using (SmtpClient smt = new SmtpClient())
                {
                    smt.Host = "smtp.gmail.com";
                    System.Net.NetworkCredential ntwd = new NetworkCredential();
                    ntwd.UserName = "no.replay.nodlays@gmail.com"; //Your Email ID
                    ntwd.Password = "Nodlays@123"; // Your Password
                    smt.UseDefaultCredentials = false;
                    smt.Credentials = ntwd;
                    smt.Port = 587;
                    smt.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smt.EnableSsl = true;
                    smt.Send(msg);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}