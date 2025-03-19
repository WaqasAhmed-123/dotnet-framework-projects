using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiAdvanced.DAL;
using WebApiAdvanced.Models;

namespace WebApiAdvanced.BL
{
    public class UserBL
    {

        public List<User> GetAllUsers()
        {
            return new UserDAL().GetActiveUsers();
        }

        public List<User> GetActiveUsers()
        {
            return new UserDAL().GetActiveUsers();
        }

        public User GetUserById(int _Id)
        {
            return new UserDAL().GetUserById(_Id);
        }

        public bool AddUser(User _user, DatabaseEntities de = null)
        {
            return new UserDAL().AddUser(_user, de);
        }

        public bool UpdateUser(User _user, DatabaseEntities de = null)
        {
            return new UserDAL().UpdateUser(_user, de);
        }

        public bool DeleteUser(int _Id)
        {
            return new UserDAL().DeleteUser(_Id);
        }
    }
}