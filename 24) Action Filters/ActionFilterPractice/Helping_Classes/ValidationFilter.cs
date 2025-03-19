using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace ActionFilterPractice.Helping_Classes
{
    //filter class should have "Filter" keyword at the end of its name
    //inherit from Action and exception filter 
    public class ValidationFilter: FilterAttribute, IActionFilter, IExceptionFilter
    {
        public List<int> sss { get; set; }
        public int Role;
        public bool CheckLogin;
        private readonly GeneralPurpose gp = new GeneralPurpose();

        //constructor
        public ValidationFilter()
        {
            CheckLogin = true;
        }

        
        //exception handling
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            //string action = filterContext.RouteData.Values["action"].ToString();
            //Exception e = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
                    { "controller", "Error" },{ "action", "NotFound" }, });
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CheckLogin == true) //only works when check is true
            {
                var controllerName = filterContext.RouteData.Values["controller"].ToString();
                var actionName = filterContext.RouteData.Values["action"].ToString();
                List<int> roles = sss;
                
                int role = Role;

                if (gp.ValidateLoggedinUser() != null)
                {
                    if (role != 0 && role != gp.ValidateLoggedinUser().Role)
                    {
                        if (gp.ValidateLoggedinUser().Role == 1)
                        {
                            var values = new RouteValueDictionary(new
                            {
                                action = "Index",
                                controller = "Admin",
                                msg = "Access Denied!",
                                color = "red"
                            });

                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(values));
                        }
                        else
                        {
                            var values = new RouteValueDictionary(new
                            {
                                action = "Index",
                                controller = "Home",
                                msg = "Access Denied!",
                                color = "red"
                            });

                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(values));
                        }
                    }
                }
                else
                {
                    var values = new RouteValueDictionary(new
                    {
                        action = "Login",
                        controller = "Auth",
                        msg = "Session Expired, Please Login",
                        color = "red"
                    });

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(values));
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

    }
}