using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ChatBox
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
