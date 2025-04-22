using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ChatBox.BL;
using ChatBox.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;

namespace ChatBox.DataHub
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


    public class ChatHub : Hub
    {

        public override Task OnConnected()
        {
            
            string name = Context.User.Identity.Name;
            

            User u = new UserBL().GetActiveUser().Where(x=>x.Name.ToLower() == name.ToLower()).FirstOrDefault();

            u.ConnectionId = Context.ConnectionId;

            bool chk = new UserBL().UpdateUser(u);
            if (chk == true)
            {
                Clients.All.broadcastCheckLogin("1", u.Id);
            }
            else
            {
                Clients.All.broadcastCheckLogin("0", 0);
            }

            return Task.FromResult(0);
        }


        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            User u = new UserBL().GetActiveUser().Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();

            u.ConnectionId = Context.ConnectionId;

            bool chk = new UserBL().UpdateUser(u);
            if (chk == true)
            {
                Clients.All.broadcastCheckLogin("1", u.Id);
            }
            else
            {
                Clients.All.broadcastCheckLogin("0", 0);
            }

            return base.OnReconnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            User u = new UserBL().GetActiveUser().Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
            if (u != null)
            {
                u.ConnectionId = null;

                bool chk = new UserBL().UpdateUser(u);
                if (chk == true)
                {
                    Clients.All.broadcastCheckLogin("2", u.Id);
                }
                else
                {
                    Clients.All.broadcastCheckLogin("1", u.Id);
                }
            }
            

            return Task.FromResult(0);
        }


        public void SendMessage(int SenderId, int ReceiverId, string Message)
        {
            Chat obj = new Chat()
            {
                SenderId = SenderId,
                ReceiverId = ReceiverId,
                Message = Message,
                isRead = 0,
                IsActive = 1,
                CreatedAt = DateTime.Now
            };

            int chk = new ChatBL().AddChat(obj);

            if(chk != -1)
            {
                User u = new UserBL().GetUserById(SenderId);

                Clients.All.broadcastMessage("1", u.Id, u.Name, chk, Message);

                Clients.All.broadcastChatCount("1", SenderId);
            }
            else
            {
                Clients.All.broadcastMessage("0", "null", "null", "null", "null");

                Clients.All.broadcastChatCount("0", 0);
            }
        }


        public void UpdateMessage(int Id, string Message)
        {
            Chat c = new ChatBL().GetChatById(Id);

            c.Message = Message;

            bool chk = new ChatBL().UpdatChart(c);

            if (chk == true)
            {
                Clients.All.broadcastUpdateMessage("1");
            }
            else
            {
                Clients.All.broadcastUpdateMessage("0");
            }
        }


        public void DeleteMessage(int ChatId, int SenderId)
        {
            bool chk = new ChatBL().DeleteChat(ChatId);
            
            if (chk == true)
            {
                Clients.All.broadcastdeleteMessage("1");

                Clients.All.broadcastChatCount("1", SenderId); //updating message count in index.cshtml while deleting messages(read/ unread messages)
            }
            else
            {
                Clients.All.broadcastdeleteMessage("0");

                Clients.All.broadcastChatCount("0", 0);
            }
        }

       
    }
}