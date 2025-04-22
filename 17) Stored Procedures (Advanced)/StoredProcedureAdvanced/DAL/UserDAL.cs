using StoredProcedureAdvanced.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoredProcedureAdvanced.DAL
{
    public class UserDAL
    {

        DatabaseEntities db;

        public List<User> GetAllUser()
        {
            List<User> Users;
            db = new DatabaseEntities();

            Users = db.sp_GetAllUser().ToList();

            return Users;
        }

        public List<User> GetUserList(string searchValue = "", int start = -1, int length = -1)
        {
            List<User> Users;
            db = new DatabaseEntities();

            Users = db.sp_GetUserList(searchValue, start, length).ToList();

            return Users;
        }

        public int GetUserCount(string searchValue = "")
        {
            db = new DatabaseEntities();

            User users = db.sp_GetUserCount(searchValue).FirstOrDefault();

            return Convert.ToInt32(users.Id);
        }

        public bool AddUser(User user, DatabaseEntities de)
        {
            try
            {
                de.Users.Add(user);
                //de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}