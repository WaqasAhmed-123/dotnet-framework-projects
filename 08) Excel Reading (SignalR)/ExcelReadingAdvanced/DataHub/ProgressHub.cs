using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcelReadingAdvanced.DataHub
{
    //First we need to install package -> Microsoft.AspNet.SignalR
    //now need to setup ownin and add the following line in <appsetings> of WebConfig
    //<add key="owin:AppStartup" value="ChatBox.App_Start.Startup" />
    //ChatBox should be replaced with your project name

    //Now add a class in App_start, named "Startup.cs"
    //paste the following code in it
    //public void Configuration(IAppBuilder app)
    //{
    //    app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
    //    app.UseCookieAuthentication(new CookieAuthenticationOptions());
    //    app.UseCookieAuthentication(new CookieAuthenticationOptions
    //    {
    //        AuthenticationType = "ApplicationCookie",
    //        LoginPath = new PathString("/Auth/Login")
    //    });
    //
    //    app.MapSignalR(); //required to referencing SignalR classes
    //}

    //now create a new folder at root called "DataHub" or any custom name
    //right click on DataHub folder and select Add-> New item...
    //Now Create "SignalR Hub Class (V2)"
    //and paste the following code

    public class ProgressHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}