using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiAdvanced.Models;

namespace WebApiAdvanced.DAL
{
    public class UserDAL
    {
        public List<User> GetAllUsers()
        {
            DatabaseEntities db = new DatabaseEntities();

            return db.Users.ToList(); 
        }

        public List<User> GetActiveUsers()
        {
            DatabaseEntities db = new DatabaseEntities();

            return db.Users.Where(x => x.IsActive == 1).ToList();
        }

        public User GetUserById(int _Id)
        {
            DatabaseEntities db = new DatabaseEntities();

            return db.Users.Where(x => x.Id == _Id).FirstOrDefault();
        }

        public bool AddUser(User _user, DatabaseEntities de = null)
        {
            if(de != null)
            {
                de.Users.Add(_user);
                de.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateUser(User _user, DatabaseEntities de = null)
        {
            try
            {
                if (de != null)
                {
                    de.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                    de.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUser(int _Id)
        {
            try
            {
                DatabaseEntities db = new DatabaseEntities();

                db.Users.Remove(db.Users.FirstOrDefault(x => x.Id == _Id));
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