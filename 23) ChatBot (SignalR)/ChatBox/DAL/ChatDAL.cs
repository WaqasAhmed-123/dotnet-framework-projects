using ChatBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBox.DAL
{
    public class ChatDAL
    {
        DatabaseEntities db = new DatabaseEntities();

        public List<Chat> GetAllChats()
        {
            List<Chat> clist = db.Chats.Where(x => x.IsActive == 1).ToList();

            return clist;
        }

        public Chat GetChatById(int _id)
        {
            Chat chat = new Chat();

            chat = db.Chats.Where(x => x.Id == _id).FirstOrDefault();

            return chat;
        }

        public int AddChat(Chat _chat)
        {
            try
            {
                db.Chats.Add(_chat);
                db.SaveChanges();

                return _chat.Id;
            }
            catch
            {
                return -1;
            }
        }


        public bool UpdatChart(Chat _chat)
        {
            try
            {
                db.Entry(_chat).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ClearUnreadChart(int _id)
        {
            try
            {
                using (db)
                {
                    db.Chats.Where(x => x.SenderId == _id && x.isRead == 0).ToList().ForEach(x => x.isRead = 1);
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteChat(int _id)
        {
            try
            {
                db.Chats.RemoveRange(db.Chats.Where(X => X.Id == _id));
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}