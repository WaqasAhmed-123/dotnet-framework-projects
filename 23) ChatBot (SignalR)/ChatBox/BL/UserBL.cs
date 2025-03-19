using ChatBox.Models;
using ChatBox.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBox.BL
{
    public class UserBL
    {

        public List<User> GetAllUsers()
        {
            return new UserDAL().GetAllUsers();
        }


        public List<User> GetActiveUser()
        {
            return new UserDAL().GetActiveUser();
        }


        public User GetUserById(int _id)
        {
            return new UserDAL().GetUserById(_id);
        }


        public bool AddUser(User _user)
        {
            return new UserDAL().AddUser(_user);
        }


        public bool UpdateUser(User _user)
        {
            return new UserDAL().UpdateUser(_user);
        }


        public bool DeleteUser(int _id)
        {
            return new UserDAL().DeleteUser(_id);
        }
    }
}