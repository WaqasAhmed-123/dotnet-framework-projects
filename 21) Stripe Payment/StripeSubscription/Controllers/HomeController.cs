using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StripeSubscription.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //Use following site to get help
        //https://blog.elmah.io/paid-subscriptions-with-asp-net-core-and-stripe-billing/
        [HttpPost]
        public ActionResult Subscribe(string email, string plan, string stripeToken)
        {
            //to get API key
            //Login to Stripe  https://dashboard.stripe.com/
            //select "Developers -> API key" from sidebar
            //now Toggle "View Test data" button and copy publish key and secret
            StripeConfiguration.ApiKey = "sk_test_apikey";

            var customerOptions = new CustomerCreateOptions //setting Customers data
            {
                Email = email,
                Source = stripeToken,
            };

            var customerService = new CustomerService();
            var customer = customerService.Create(customerOptions); //creating a new customer

            //to get Plan id goto Products and add new product
            //after creating product copy "API ID" starting from "Price_...."
            //and paste here
            var planId = "planId"; 
            if (plan == "Yearly")
            {
                planId = "planId"; // create new product with yearly billing period
                
            }

            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = customer.Id,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Plan = planId
                    },
                },
            };
            subscriptionOptions.AddExpand("latest_invoice.payment_intent");

            var subscriptionService = new SubscriptionService();
            var subscription = subscriptionService.Create(subscriptionOptions);

            ViewBag.stripeKey = "stripeKey";
            ViewBag.subscription = subscription.ToJson();

            return View("SubscribeResult");
        }


       

        public ActionResult SubscribeResult()
        {
            return View();
        }


        public ActionResult StripeWebhook()
        {
            try
            {
                var json = new StreamReader(HttpContext.Request.InputStream).ReadToEnd();

                // validate webhook called by stripe only
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], "---REPLACE STRIPE WEBHOOK SECRET---");

                switch (stripeEvent.Type)
                {
                    case "customer.created":
                        var customer = stripeEvent.Data.Object as Customer;
                        // do work

                        break;

                    case "customer.subscription.created":
                    case "customer.subscription.updated":
                    case "customer.subscription.deleted":
                    case "customer.subscription.trial_will_end":
                        var subscription = stripeEvent.Data.Object as Subscription;
                        // do work

                        break;

                    case "invoice.created":
                        var newinvoice = stripeEvent.Data.Object as Invoice;
                        // do work

                        break;

                    case "invoice.upcoming":
                    case "invoice.payment_succeeded":
                    case "invoice.payment_failed":
                        var invoice = stripeEvent.Data.Object as Invoice;
                        // do work

                        break;

                    case "coupon.created":
                    case "coupon.updated":
                    case "coupon.deleted":
                        var coupon = stripeEvent.Data.Object as Coupon;
                        // do work

                        break;

                        // DO SAME FOR OTHER EVENTS
                }

                return null;
            }
            catch (StripeException ex)
            {
                //_logger.LogError(ex, $"StripWebhook: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"StripWebhook: {ex.Message}");
                return null;
            }
        }


    }
}