using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActionFilterPractice.Models;

namespace ActionFilterPractice.DAL
{
    public class UserDAL
    {
        public List<User> GetAllUserList(DatabaseEntities de = null)
        {
            if(de != null)
            {
                return de.Users.ToList();
            }
            else
            {
                DatabaseEntities db = new DatabaseEntities();
                return db.Users.ToList();
            }
        }

        public List<User> GetActiveUserList(DatabaseEntities de = null)
        {
            if (de != null)
            {
                return de.Users.Where(x => x.IsActive == 1).ToList();
            }
            else
            {
                DatabaseEntities db = new DatabaseEntities();
                return db.Users.Where(x => x.IsActive == 1).ToList();

            }
        }

        public User GetUserById(int _Id, DatabaseEntities de = null)
        {
            if (de != null)
            {
                return de.Users.Where(x => x.Id == _Id).FirstOrDefault(x=>x.IsActive == 1);
            }
            else
            {
                DatabaseEntities db = new DatabaseEntities();
                return db.Users.Where(x => x.Id == _Id).FirstOrDefault();

            }
        }

        public bool AddUser(User _u, DatabaseEntities de = null)
        {
            if (de != null)
            {
                de.Users.Add(_u);
                de.SaveChanges();
            }
            else
            {
                DatabaseEntities db = new DatabaseEntities();
                db.Users.Add(_u);
                db.SaveChanges();
            }
            

            return true;
        }

        public bool UpdateUser(User _u, DatabaseEntities de = null)
        {
            if (de != null)
            {
                de.Entry(_u).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            else
            {
                DatabaseEntities db = new DatabaseEntities();
                db.Entry(_u).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            

            return true;
        }

        public bool DeleteUser(int _Id)
        {
            DatabaseEntities db = new DatabaseEntities();

            db.Users.Remove(db.Users.Where(x => x.Id == _Id).FirstOrDefault());
            db.SaveChanges();

            return true;
        }
    }
}