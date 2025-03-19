using DbTransactionPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbTransactionPractice.DAL
{
    public class CourseDAL
    {
        DatabaseEntities db;


        public List<Course> GetCourseList()
        {
            List<Course> Courses;
            db = new DatabaseEntities();

            Courses = db.Courses.ToList();

            return Courses;
        }

        public List<Course> GetActiveCourseList()
        {
            List<Course> Courses;
            db = new DatabaseEntities();

            Courses = db.Courses.Where(x => x.IsActive == 1).ToList();

            return Courses;
        }

        public List<Course> GetInactiveCourseList()
        {
            List<Course> Courses;
            db = new DatabaseEntities();

            Courses = db.Courses.Where(x => x.IsActive == 0).ToList();

            return Courses;
        }

        public Course GetCourseById(int _Id)
        {
            db = new DatabaseEntities();

            Course Course = db.Courses.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);

            return Course;
        }

        public bool AddCourse(Course _Course)
        {
            using (db = new DatabaseEntities())
            {
                db.Courses.Add(_Course);
                db.SaveChanges();
            }

            return true;
        }

        public bool UpdateCourse(Course _Course)
        {
            using (db = new DatabaseEntities())
            {
                db.Entry(_Course).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

    }
}