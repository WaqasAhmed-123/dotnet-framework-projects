using ScormReading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScormReading.DAL
{
    public class UserCourseDAL
    {

        public List<UserCourse> GetAllUserCourseList(DatabaseEntities de)
        {
            return de.UserCourses.ToList();
        }

        public List<UserCourse> GetActiveUserCourseList(DatabaseEntities de)
        {
            return de.UserCourses.Where(x => x.IsActive == 1).ToList();
        }


        public UserCourse GetUserCourseById(int id, DatabaseEntities de)
        {
            return de.UserCourses.Where(x => x.Id == id).FirstOrDefault();
        }

        public UserCourse GetActiveUserCourseById(int id, DatabaseEntities de)
        {
            return de.UserCourses.Where(x => x.Id == id).FirstOrDefault(x => x.IsActive == 1);
        }

        public bool AddUserCourse(UserCourse UserCourse, DatabaseEntities de)
        {
            try
            {
                de.UserCourses.Add(UserCourse);
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int AddUserCourse2(UserCourse UserCourse, DatabaseEntities de)
        {
            try
            {
                de.UserCourses.Add(UserCourse);
                de.SaveChanges();

                return UserCourse.Id;
            }
            catch
            {
                return -1;
            }
        }

        public bool UpdateUserCourse(UserCourse UserCourse, DatabaseEntities de)
        {
            try
            {
                de.Entry(UserCourse).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUserCourse(int id, DatabaseEntities de)
        {
            try
            {
                de.UserCourses.Remove(de.UserCourses.Where(x => x.Id == id).FirstOrDefault());
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