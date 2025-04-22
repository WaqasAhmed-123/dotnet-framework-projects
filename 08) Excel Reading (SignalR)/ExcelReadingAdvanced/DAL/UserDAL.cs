using ExcelReadingAdvanced.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcelReadingAdvanced.DAL
{
    public class UserDAL
    {

        public List<User> GetAllUsersList(DatabaseEntities de)
        {
            return de.Users.ToList();
        }

        public List<User> GetActiveUsersList(DatabaseEntities de)
        {
            return de.Users.Where(x => x.IsActive == 1).ToList();
        }

        public User GetUserById(int id, DatabaseEntities de)
        {
            return de.Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public User GetActiveUserById(int id, DatabaseEntities de)
        {
            return de.Users.Where(x => x.Id == id).FirstOrDefault(x => x.IsActive == 1);
        }

        public bool AddUser(User user, DatabaseEntities de)
        {
            try
            {
                de.Users.Add(user);
                de.SaveChanges();

                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        public int AddUser2(User user, DatabaseEntities de)
        {
            try
            {
                de.Users.Add(user);
                de.SaveChanges();

                return user.Id;
            }
            catch
            {
                return -1;
            }
        }

        public bool UpdateUser(User user, DatabaseEntities de)
        {
            try
            {
                de.Entry(user).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUser(User obj, DatabaseEntities de)
        {
            try
            {
                obj.IsActive = 0;
                de.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteAllUser(DatabaseEntities de)
        {
            try
            {
                List<User> userList = de.Users.ToList();
                de.Users.RemoveRange(userList);
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}