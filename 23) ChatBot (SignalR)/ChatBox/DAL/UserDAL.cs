using ChatBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ChatBox.DAL
{
    public class UserDAL
    {
        DatabaseEntities db = new DatabaseEntities();

        public List<User> GetAllUsers()
        {
            List<User> ulist = new List<User>();
            
            ulist = db.Users.ToList();
            
            return ulist;
        }


        public List<User> GetActiveUser()
        {
            List<User> ulist = new List<User>();

            ulist = db.Users.Where(x => x.IsActive == 1).ToList();

            return ulist;
        }


        public User GetUserById(int _id)
        {
            User user = new User();

            user = db.Users.Where(x => x.Id == _id).FirstOrDefault();

            return user;
        }


        public bool AddUser(User _user)
        {
            try
            {
                db.Users.Add(_user);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool UpdateUser(User _user)
        {
            try
            {
                db.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool DeleteUser(int _id)
        {
            try
            {
                db.Users.RemoveRange(db.Users.Where(x => x.Id == _id));
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