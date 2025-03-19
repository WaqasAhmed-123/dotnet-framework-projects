using ScormReading.DAL;
using ScormReading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScormReading.BL
{
    public class UserCourseBL
    {
        public List<UserCourse> GetAllUserCourseList(DatabaseEntities de)
        {
            return new UserCourseDAL().GetAllUserCourseList(de);
        }

        public List<UserCourse> GetActiveUserCourseList(DatabaseEntities de)
        {
            return new UserCourseDAL().GetActiveUserCourseList(de);
        }

        public UserCourse GetUserCourseById(int id, DatabaseEntities de)
        {
            return new UserCourseDAL().GetUserCourseById(id, de);
        }

        public UserCourse GetActiveUserCourseById(int id, DatabaseEntities de)
        {
            return new UserCourseDAL().GetActiveUserCourseById(id, de);
        }

        public bool AddUserCourse(UserCourse UserCourse, DatabaseEntities de)
        {
            //UserCourse uc = new UserCourseBL().GetActiveUserCourseList(de).Where(x => x.UserId == UserCourse.UserId && x.CourseTitle == UserCourse.CourseTitle).FirstOrDefault();
            //if (uc == null)
            //{
                return new UserCourseDAL().AddUserCourse(UserCourse, de);
            //}
            //else
            //{
            //    return true;
            //}
        }

        public int AddUserCourse2(UserCourse UserCourse, DatabaseEntities de)
        {
            return new UserCourseDAL().AddUserCourse2(UserCourse, de);
        }


        public bool UpdateUserCourse(UserCourse UserCourse, DatabaseEntities de)
        {
            return new UserCourseDAL().UpdateUserCourse(UserCourse, de);
        }

        public bool DeleteUserCourse(int id, DatabaseEntities de)
        {
            return new UserCourseDAL().DeleteUserCourse(id, de);
        }
    }
}