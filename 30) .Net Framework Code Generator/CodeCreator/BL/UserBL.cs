using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCreator.Models;
using CodeCreator.DAL;

namespace CodeCreator.BL
{
    public class UserBL
    {
        public List<User> GetAllUsersList(DatabaseEntities de)
        {
            return new UserDAL().GetAllUsersList(de);
        }

        public List<User> GetActiveUsersList(DatabaseEntities de)
        {
            return new UserDAL().GetActiveUsersList(de);
        }

        public User GetUserById(int id, DatabaseEntities de)
        {
            return new UserDAL().GetUserById(id, de);
        }

        public User GetActiveUserById(int id, DatabaseEntities de)
        {
            return new UserDAL().GetActiveUserById(id, de);
        }

        public bool AddUser(User user, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
            {
                return false;
            }
            else
            {
                return new UserDAL().AddUser(user, de);
            }
        }

        public int AddUser2(User user, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
            {
                return -1;
            }
            else
            {
                return new UserDAL().AddUser2(user, de);
            }
        }


        public bool UpdateUser(User user, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
            {
                return false;
            }
            else
            {
                return new UserDAL().UpdateUser(user, de);
            }
        }

        public bool DeleteUser(int id, DatabaseEntities de)
        {
            return new UserDAL().DeleteUser(id, de);
        }

    }
}