using ChatBox.BL;
using ChatBox.DataHub;
using ChatBox.Helping_Classes;
using ChatBox.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ChatBox.Controllers
{
    public class HomeController : Controller
    {
        
        public User validateUser()
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal; // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();
            User loggedInUser = new UserBL().GetUserById(Convert.ToInt32(userId));

            return loggedInUser;
        }

        public ActionResult Index()
        {
            if(validateUser() != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Auth");
            }
        }


        public ActionResult Chat(int ReceiverId = -1)
        {
            if (validateUser() != null)
            {
                User u = new UserBL().GetUserById(ReceiverId);

                ViewBag.ReceiverName = u.Name;
                ViewBag.ReceiverId = ReceiverId;
                ViewBag.SenderId = validateUser().Id;

                return View();
            }
            else
            {
                return RedirectToAction("login", "Auth");
            }

        }


        #region Ajax

        [HttpPost]
        public ActionResult GetUserList() //used in Index.cshtml page
        {
            List<User> ulist = new UserBL().GetActiveUser().Where(x => x.Id != validateUser().Id).OrderBy(x => x.Name).ToList();

            List<ContactDTO> clist = new List<ContactDTO>();

            int count = 0;
            int IsLoggedin = 0;

            foreach (User i in ulist)
            {
                count = new ChatBL().GetAllChats().Where(x => x.SenderId == i.Id && x.isRead == 0).Count();

                if(i.ConnectionId != null)
                {
                    IsLoggedin = 1;
                }
                ContactDTO obj = new ContactDTO()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Count = count,
                    IsLoggedin = IsLoggedin
                };

                clist.Add(obj);
                
                IsLoggedin = 0;
            }


            return Json(clist);
        }


        [HttpPost]
        public ActionResult GetUnreadChatCount(int SenderId) //used in Index.cshtml page
        {
            int count = new ChatBL().GetAllChats().Where(x => x.SenderId == SenderId && x.isRead == 0).Count();

            ContactDTO obj = new ContactDTO()
            {
                Id = SenderId,
                Count = count
            };

            return Json(obj);
        }


        [HttpPost]
        public ActionResult GetClearUnreadChat(int SenderId) //thid function will be called when we open Chat.cshtml page
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            bool chk = new ChatBL().ClearUnreadChart(SenderId);

            if (chk == true)
            {
                context.Clients.All.broadcastChatCount("1", SenderId); //this function is used in Index.cshtml, to get updated chat count
                return Json("1");
            }
            else
            {
                context.Clients.All.broadcastChatCount("0", 0);

                return Json("0");
            }
            
        }




        [HttpPost]
        public ActionResult GetChat(int SenderId, int ReceiverId) //used to get chat messages in Chat.cshtml page
        {
            List<Chat> clict = new ChatBL().GetAllChats().Where(x=> (x.SenderId == SenderId && x.ReceiverId == ReceiverId) || (x.ReceiverId == SenderId && x.SenderId == ReceiverId)).ToList();

            List<Chat> uclist = clict.Where(x => x.isRead == 0).ToList();
            
            List<MessageDTO> mlist = new List<MessageDTO>();

            foreach (Chat i in clict)
            {
                User u = new UserBL().GetUserById(Convert.ToInt32(i.SenderId));

                MessageDTO obj = new MessageDTO()
                {
                    Id = i.Id,
                    SenderId = u.Id,
                    SenderName = u.Name,
                    Message = i.Message,
                };

                mlist.Add(obj);
            }

            return Json(mlist);
        }

        [HttpPost]
        public ActionResult GetChatCount(int SenderId, int ReceiverId) //Used in chat.scshtml page at top to get the count of total messages
        {
            int ccount = 0;
            
            ccount = new ChatBL().GetAllChats().Where(x => (x.SenderId == SenderId && x.ReceiverId == ReceiverId) || (x.ReceiverId == SenderId && x.SenderId == ReceiverId)).Count();

            return Json(ccount);
        }

        [HttpPost]
        public ActionResult GetUpdateChat(int Id)
        {
            Chat c = new ChatBL().GetChatById(Id);

            return Json(c);
        }


        #endregion

        public void AddChat(int SenderId, int ReceiverId, string Message) //handling Chathub functions through back end
        {

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            Chat obj = new Chat()
            {
                SenderId = SenderId,
                ReceiverId = ReceiverId,
                Message = Message,
                IsActive = 1,
                CreatedAt = DateTime.Now
            };

            int chk = new ChatBL().AddChat(obj);

            if (chk != -1)
            {
                User u = new UserBL().GetUserById(1);

                context.Clients.All.broadcastMessage("1", u.Id, u.Name, chk, Message);
            }
            else
            {
                context.Clients.All.broadcastMessage("0", "", "", "", "");
            }

        }


        public ActionResult Test(int ReceiverId = -1)
        {
            if (validateUser() != null)
            {
                ViewBag.ReceiverId = ReceiverId;
                ViewBag.SenderId = validateUser().Id;

                return View();
            }
            else
            {
                return RedirectToAction("login", "Auth");
            }

        }
    }
}