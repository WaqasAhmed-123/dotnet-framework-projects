using AjaxPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AjaxPractice.DAL
{
    public class StudentDAL
    {
        #region Student

        StudentDatabaseEntities db;

        public List<Student> GetStudentsList()
        {
            List<Student> Students;
            db = new StudentDatabaseEntities();

            Students = db.Students.ToList();

            return Students;
        }

        public List<Student> GetActiveStudentsList()
        {
            List<Student> Students;
            db = new StudentDatabaseEntities();

            Students = db.Students.ToList();

            return Students;
        }

        public List<Student> GetInactiveStudentsList()
        {
            List<Student> Students;
            db = new StudentDatabaseEntities();

            Students = db.Students.ToList();

            return Students;
        }

        public Student getStudentById(int _Id)
        {
            db = new StudentDatabaseEntities();

            Student Student = db.Students.FirstOrDefault(x => x.StudentId == _Id);

            return Student;
        }

        public bool AddStudent(Student _Student)
        {
            using (db = new StudentDatabaseEntities())
            {
                db.Students.Add(_Student);
                db.SaveChanges();
            }

            return true;
        }

        public bool UpdateStudent(Student _Student)
        {
            using (db = new StudentDatabaseEntities())
            {
                db.Entry(_Student).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

        #endregion
    }
}