using StoredProcedureAdvanced.DAL;
using StoredProcedureAdvanced.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoredProcedureAdvanced.BL
{
    public class UserBL
    {
        public List<User> GetAllUser()
        {
            return new UserDAL().GetAllUser();
        }

        public List<User> GetUserList(string searchValue = "", int start = -1, int length = -1)
        {
            return new UserDAL().GetUserList(searchValue, start, length);
        }

        public int GetUserCount(string searchValue = "")
        {
            return new UserDAL().GetUserCount(searchValue);
        }

        public bool AddUser(User u, DatabaseEntities de)
        {
            return new UserDAL().AddUser(u, de);
        }
    }
}