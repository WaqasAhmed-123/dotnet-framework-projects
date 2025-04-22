using DbTransactionPractice.DAL;
using DbTransactionPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbTransactionPractice.BL
{
    public class CourseBL
    {

        public List<Course> GetCourseList()
        {
            return new CourseDAL().GetCourseList();
        }

        public List<Course> GetActiveCourseList()
        {
            return new CourseDAL().GetActiveCourseList();
        }

        public List<Course> GetInactiveCourseList()
        {
            return new CourseDAL().GetInactiveCourseList();
        }

        public Course GetCoursebyId(int _id)
        {
            return new CourseDAL().GetCourseById(_id);
        }

        public bool AddCourse(Course _Course)
        {
            if (_Course.StudentRollNo == -1 || _Course.CourseName == null ||
                _Course.IsActive == null || _Course.CreatedAt == null)
            {
                return false;
            }

            return new CourseDAL().AddCourse(_Course);
        }

        public bool UpdateCourse(Course _Course)
        {
            if (_Course.StudentRollNo == -1 || _Course.CourseName == null ||
                _Course.IsActive == null || _Course.CreatedAt == null)
            {
                return false;
            }

            return new CourseDAL().UpdateCourse(_Course);
        }
    }
}