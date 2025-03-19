using System;
using System.Collections.Generic;
using WebApplication1.Models;
using WebApplication1.DAL;

namespace WebApplication1.BL
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
            if (String.IsNullOrEmpty(user.FirstName) || String.IsNullOrEmpty(user.LastName) || String.IsNullOrEmpty(user.Email) )
            {
                return false;
            }
            else
            {
                return new UserDAL().AddUser(user, de);
            }
        }

        public bool UpdateUser(User user, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(user.FirstName) || String.IsNullOrEmpty(user.LastName) || String.IsNullOrEmpty(user.Email) )
            {
                return false;
            }
            else
            {
                return new UserDAL().UpdateUser(user, de);
            }
        }

        public bool ArchiveUser(int id, DatabaseEntities de)
        {
            return new UserDAL().ArchiveUser(id, de);
        }
        
        public bool DeleteUser(int id, DatabaseEntities de)
        {
            return new UserDAL().DeleteUser(id, de);
        }
    }
}