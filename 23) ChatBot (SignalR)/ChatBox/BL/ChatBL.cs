using ChatBox.DAL;
using ChatBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBox.BL
{
    public class ChatBL
    {

        public List<Chat> GetAllChats()
        {
            return new ChatDAL().GetAllChats();
        }

        public Chat GetChatById(int _id)
        {
            return new ChatDAL().GetChatById(_id);
        }

        public int AddChat(Chat _chat)
        {
            return new ChatDAL().AddChat(_chat);
        }

        public bool ClearUnreadChart(int _id)
        {
            return new ChatDAL().ClearUnreadChart(_id);
        }

        public bool UpdatChart(Chat _chat)
        {
            return new ChatDAL().UpdatChart(_chat);
        }


        public bool DeleteChat(int _id)
        {
            return new ChatDAL().DeleteChat(_id);
        }
    }
}