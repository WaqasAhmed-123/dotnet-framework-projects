using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.DAL
{
    public class StudentDAL
    {
        SchoolEntities db;

        #region Student
        public List<Student> GetActiveStudentsList()
        {
            List<Student> Students;
            db = new SchoolEntities();

            Students = db.Students.Where(x => x.IsActive == 1).ToList();

            return Students;
        }

        public List<Student> GetInactiveStudentsList()
        {
            List<Student> Students;
            db = new SchoolEntities();

            Students = db.Students.Where(x => x.IsActive == 0).ToList();

            return Students;
        }

        public Student getStudentById(int _Id)
        {
            db = new SchoolEntities();

            Student Student = db.Students.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);

            return Student;
        }

        public bool AddStudent(Student _Student)
        {
            using (db = new SchoolEntities())
            {
                db.Students.Add(_Student);
                db.SaveChanges();
            }

            return true;
        }

        public bool UpdateStudent(Student _Student)
        {
            using (db = new SchoolEntities())
            {
                db.Entry(_Student).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

        #endregion
    }
}