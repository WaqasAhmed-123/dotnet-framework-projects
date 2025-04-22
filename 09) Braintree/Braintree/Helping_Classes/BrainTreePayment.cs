using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Braintree.Helping_Classes
{
    public class BrainTreePayment
    {

        public string token;
        //First PM> Install-Package Braintree -Version 2.64.0
        //Signup to braintree SandBox
        //Signin sandbox and copy, paste following values
        //MerchantId
        //PublicKey
        //PrivateKey
        /// <summary>
        /// above values can retrieve from setting (icon) API
        /// </summary>
        BraintreeGateway gateway = new BraintreeGateway
        {
            //Production Environment of client
            //Environment = Braintree.Environment.PRODUCTION,
            //MerchantId = "",
            //PublicKey = "",
            //PrivateKey = ""

            //Sandbox environment of Developers account -> rembo733@gmail.com
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "your MerchantId",
            PublicKey = "your PublicKey",
            PrivateKey = "your PrivateKey"

        };

        public BrainTreePayment()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var clientToken = gateway.ClientToken.generate();
            token = clientToken;
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session["token"] = clientToken;
        }
        // Unsubscription
        public bool UnSubscription(string Subscription_Id)
        {
            Result<Subscription> result = gateway.Subscription.Cancel(Subscription_Id);
            if (result.IsSuccess())
                return true;
            return false;
        }


        public string proceedPayment(string nonce)
        {
            string nonceFromTheClient = nonce;

            //add some customer details to send with transaction
            var request = new CustomerRequest
            {
                FirstName = "Waqas Ahmed",
                Company = "Nodlays",
                Email = "waqaxahmed786@gmail.com",
                Phone = "0000000",
                Website = "www.abc.com",
            };
            Result<Customer> result = gateway.Customer.Create(request);

            if (result.IsSuccess())
            {

                string customerId = result.Target.Id;
                var request1 = new PaymentMethodRequest
                {
                    CustomerId = customerId,
                    PaymentMethodNonce = nonce,
                    Options = new PaymentMethodOptionsRequest
                    {
                        VerifyCard = true
                    }
                };

                Result<PaymentMethod> result1 = gateway.PaymentMethod.Create(request1);
                Result<Subscription> result2;
                if (result1.IsSuccess())
                {
                    //goto subscription tab in sandbox
                    //select plans
                    //add new plan and paste its' id below
                    var request2 = new SubscriptionRequest
                    {
                        PaymentMethodToken = result1.Target.Token,
                        PlanId = "waqas123",
                        FirstBillingDate = DateTime.Now

                    };
                    result2 = gateway.Subscription.Create(request2);


                    if (result2.IsSuccess())
                    {
                        string Subscription_Id = result2.Target.Id;
                        return Subscription_Id;
                    }
                    else
                    {
                        //Please check your funds.
                        return "-3";
                    }
                }
                else
                {
                    //Your card is valid or not.
                    return "-2";
                }
            }
            else
            {
                // Requirements of customer for Braintree is not complete
                return "-1";
            }

        }



        public string proceedDiscountPayment(string nonce)
        {
            string nonceFromTheClient = nonce;

            var request = new CustomerRequest
            {
                FirstName = "Waqas Ahmed2",
                Company = "Not Given",
                Email = "waqaxahmed786@gmail.com",
                Phone = "00000000",
                Website = "www.abc2.com",
            };
            Result<Customer> result = gateway.Customer.Create(request);

            if (result.IsSuccess())
            {

                string customerId = result.Target.Id;
                var request1 = new PaymentMethodRequest
                {
                    CustomerId = customerId,
                    PaymentMethodNonce = nonce,
                    Options = new PaymentMethodOptionsRequest
                    {
                        VerifyCard = true
                    }
                };

                Result<PaymentMethod> result1 = gateway.PaymentMethod.Create(request1);
                Result<Subscription> result2;
                if (result1.IsSuccess())
                {
                    var request2 = new SubscriptionRequest
                    {
                        //create a new plan with discounted price and add PlanId below
                        PaymentMethodToken = result1.Target.Token,
                        PlanId = "cmd2",
                        FirstBillingDate = DateTime.Now

                    };
                    result2 = gateway.Subscription.Create(request2);


                    if (result2.IsSuccess())
                    {
                        string Subscription_Id = result2.Target.Id;
                        return Subscription_Id;
                    }
                    else
                    {
                        //Please check your funds.
                        return "-3";
                    }
                }
                else
                {
                    //Your card is valid or not.
                    return "-2";
                }
            }
            else
            {
                // Requirements of customer for Braintree is not complete
                return "-1";
            }

        }


        public string proceedSinglePayment(string nonce)
        {
            string nonceFromTheClient = nonce;

            var CustomerRequest = new CustomerRequest  //creating customer credentials
            {
                FirstName = "Waqas Ahmed",
                Company = "Single payment",
                Email = "waqaxahmed786@gmail.com",
                Phone = "03244060943",
                Website = "www.abc2.com",
            };

            Result<Customer> CustomerResult = gateway.Customer.Create(CustomerRequest);

            if (CustomerResult.IsSuccess())
            {

                string customerId = CustomerResult.Target.Id;

                var MethodRequest = new PaymentMethodRequest
                {
                    CustomerId = customerId,
                    PaymentMethodNonce = nonceFromTheClient,
                    Options = new PaymentMethodOptionsRequest
                    {
                        VerifyCard = true
                    }
                };

                Result<PaymentMethod> MethodResult = gateway.PaymentMethod.Create(MethodRequest);
                
                if (MethodResult.IsSuccess())
                {
                    var TransactionRequest = new TransactionRequest //creating transaction request
                    {
                        CustomerId = customerId,
                        Amount = 10,
                        Options = new TransactionOptionsRequest
                        {
                            SubmitForSettlement = true
                        }
                    };

                    Result<Transaction> TransactionResult = gateway.Transaction.Sale(TransactionRequest);

                    if (TransactionResult.IsSuccess())
                    {
                        Transaction transaction = TransactionResult.Target;
                        return transaction.Id;
                        //return RedirectToAction("Show", new { id = transaction.Id });
                    }
                    else if (TransactionResult.Transaction != null)
                    {
                        return TransactionResult.Transaction.Id;
                        //return RedirectToAction("Show", new { id = result2.Transaction.Id });
                    }
                    else
                    {
                        string errorMessages = "";
                        foreach (ValidationError error in TransactionResult.Errors.DeepAll())
                        {
                            errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                        }



                        //Please check your funds.
                        return "-3";
                    }
                }
                else
                {
                    //Your card is not valid.
                    return "-2";
                }

            }
            else
            {
                // Requirements of customer for Braintree is not complete
                return "-1";
            }

        }


        //public ActionResult Show(String id)
        //{
        //    var gateway = config.GetGateway();
        //    Transaction transaction = gateway.Transaction.Find(id);

        //    if (transactionSuccessStatuses.Contains(transaction.Status))
        //    {
        //        TempData["header"] = "Sweet Success!";
        //        TempData["icon"] = "success";
        //        TempData["message"] = "Your test transaction has been successfully processed. See the Braintree API response and try again.";
        //    }
        //    else
        //    {
        //        TempData["header"] = "Transaction Failed";
        //        TempData["icon"] = "fail";
        //        TempData["message"] = "Your test transaction has a status of " + transaction.Status + ". See the Braintree API response and try again.";
        //    };

        //    ViewBag.Transaction = transaction;
        //    return View();
        //}
    }
}