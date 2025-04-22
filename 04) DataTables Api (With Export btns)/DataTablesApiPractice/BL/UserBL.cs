using DataTablesApiPractice.DAL;
using DataTablesApiPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTablesApiPractice.BL
{
    public class UserBL
    {
        #region User

        public List<User> GetUsersList()
        {
            return new UserDAL().GetUsersList();
        }

        public List<User> GetActiveUsersList()
        {
            return new UserDAL().GetActiveUsersList();
        }

        public List<User> GetInactiveUsersList()
        {
            return new UserDAL().GetInactiveUsersList();
        }

        public User GetUserbyId(int _id)
        {
            return new UserDAL().getUserById(_id);
        }

        public bool AddUser(User _User)
        {
            if (_User.Name == null || _User.Contact == null || _User.Email == null ||
                _User.Password == null || _User.Role == null || _User.IsActive == null ||
                _User.CreatedAt == null)
            {
                return false;
            }

            return new UserDAL().AddUser(_User);
        }

        public bool UpdateUser(User _User)
        {
            if (_User.Name == null || _User.Contact == null || _User.Email == null ||
                _User.Password == null || _User.Role == null || _User.IsActive == null ||
                _User.CreatedAt == null)
            {
                return false;
            }

            return new UserDAL().UpdateUser(_User);
        }

        #endregion
    }
}