using Braintree.Helping_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Braintree.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Payment(string Message = "")
        {
            BrainTreePayment payment = new BrainTreePayment();

            ViewBag.Message = Message;
            
            return View();
        }

        //goto following site and copy a dummy credit card and add expire date to check transactions
        //https://developers.braintreepayments.com/guides/credit-cards/testing-go-live/php#test-value-378282246310005
        
        [HttpPost]
        public ActionResult PostPayment(FormCollection collection)
        {
            BrainTreePayment payments = new BrainTreePayment();
            string nonceFromTheClient = collection["payment_method_nonce"];

            string Subscription_Id = payments.proceedPayment(nonceFromTheClient);
            if (Subscription_Id == "-1")
            {
                return RedirectToAction("Payment", "Account", new { Message = "Requirements of customer for Braintree is not complete." });
            }
            else if (Subscription_Id == "-2")
            {
                return RedirectToAction("Payment", "Account", new { Message = "Problem in your card." });
            }
            else if (Subscription_Id == "-3")
            {
                return RedirectToAction("Payment", "Account", new { Message = "Problem in your funds." });
            }
            else
            {
                
                return RedirectToAction("Payment", "Account", new { Message = "Congratulations..!! you got the subscription successfully. Now you can add campaigns." });
            }
        }


        [HttpPost]
        public ActionResult PostDiscountPayment(FormCollection collection)
        {
            BrainTreePayment payments = new BrainTreePayment();
            string nonceFromTheClient = collection["payment_method_nonce"];

            string Subscription_Id = payments.proceedDiscountPayment(nonceFromTheClient);
            if (Subscription_Id == "-1")
            {
                return RedirectToAction("Payment", "Account", new { Message = "Requirements of customer for Braintree is not complete." });
            }
            else if (Subscription_Id == "-2")
            {
                return RedirectToAction("Payment", "Account", new { Message = "Problem in your card." });
            }
            else if (Subscription_Id == "-3")
            {
                return RedirectToAction("Payment", "Account", new { Message = "Problem in your funds." });
            }
            else
            {

                return RedirectToAction("Payment", "Account", new { Message = "Congratulations..!! you got the subscription successfully. Now you can add campaigns." });
            }


        }



        public ActionResult SinglePayment(string Message = "")
        {
            BrainTreePayment payment = new BrainTreePayment();

            ViewBag.Message = Message;

            return View();
        }


        [HttpPost]
        public ActionResult PostSinglePayment(FormCollection collection)
        {
            BrainTreePayment payments = new BrainTreePayment();
            string nonceFromTheClient = collection["payment_method_nonce"];

            string Transaction_Id = payments.proceedSinglePayment(nonceFromTheClient);
            if (Transaction_Id == "-1")
            {
                return RedirectToAction("SinglePayment", "Account", new { Message = "Requirements of customer for Braintree is not complete." });
            }
            else if (Transaction_Id == "-2")
            {
                return RedirectToAction("SinglePayment", "Account", new { Message = "Problem in your card." });
            }
            else if (Transaction_Id == "-3")
            {
                return RedirectToAction("SinglePayment", "Account", new { Message = "Problem in your funds." });
            }
            else
            {
                return RedirectToAction("SinglePayment", "Account", new { Message = "Payment Successful." });
            }


        }
    }
}