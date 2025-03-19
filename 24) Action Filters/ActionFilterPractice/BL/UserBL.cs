using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActionFilterPractice.Models;
using ActionFilterPractice.DAL;

namespace ActionFilterPractice.BL
{
    public class UserBL
    {
        public List<User> GetAllUserList(DatabaseEntities de = null)
        {
            return new UserDAL().GetAllUserList(de);
        }

        public List<User> GetActiveUserList(DatabaseEntities de = null)
        {
            return new UserDAL().GetActiveUserList(de);
        }

        public User GetUserById(int _Id, DatabaseEntities de = null)
        {
            return new UserDAL().GetUserById(_Id, de);
        }

        public bool AddUser(User _u, DatabaseEntities de = null)
        {
            if (_u.Name == null || _u.Contact == null || _u.Email == null || _u.Password == null || _u.Name == "" || _u.Contact == "" || _u.Email == "" || _u.Password == "")
            {
                return false;
            }
            else
            {
                return new UserDAL().AddUser(_u, de);
            }
        }

        public bool UpdateUser(User _u, DatabaseEntities de = null)
        {
            if (_u.Name == null || _u.Contact == null || _u.Email == null || _u.Password == null || _u.Name == "" || _u.Contact == "" || _u.Email == "" || _u.Password == "")
            {
                return false;
            }
            else
            {
                return new UserDAL().UpdateUser(_u, de);
            }
        }

        public bool DeleteUser(int _Id)
        {
            return new UserDAL().DeleteUser(_Id);
        }
    }
}