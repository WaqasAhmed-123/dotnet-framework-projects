using OwinPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwinPractice.DAL
{
    public class UserDAL
    {
        UserDatabaseEntities db;

        #region User
        public List<User> GetActiveUsersList()
        {
            List<User> Users;
            db = new UserDatabaseEntities();

            Users = db.Users.Where(x => x.IsActive == 1).ToList();

            return Users;
        }

        public List<User> GetInactiveUsersList()
        {
            List<User> Users;
            db = new UserDatabaseEntities();

            Users = db.Users.Where(x => x.IsActive == 0).ToList();

            return Users;
        }

        public User getUserById(int _Id)
        {
            db = new UserDatabaseEntities();

            User User = db.Users.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);

            return User;
        }

        public bool AddUser(User _User)
        {
            using (db = new UserDatabaseEntities())
            {
                db.Users.Add(_User);
                db.SaveChanges();
            }

            return true;
        }

        public bool UpdateUser(User _User)
        {
            using (db = new UserDatabaseEntities())
            {
                db.Entry(_User).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

        #endregion
    }
}