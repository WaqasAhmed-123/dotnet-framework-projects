using DbTransactionPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbTransactionPractice.DAL
{
    public class StudentDAL
    {
        DatabaseEntities db;


        public List<Student> GetStudentList()
        {
            List<Student> Students;
            db = new DatabaseEntities();

            Students = db.Students.ToList();

            return Students;
        }

        public List<Student> GetActiveStudentList()
        {
            List<Student> Students;
            db = new DatabaseEntities();

            Students = db.Students.Where(x => x.IsActive == 1).ToList();

            return Students;
        }

        public List<Student> GetInactiveStudentList()
        {
            List<Student> Students;
            db = new DatabaseEntities();

            Students = db.Students.Where(x => x.IsActive == 0).ToList();

            return Students;
        }

        public Student GetStudentById(int _Id)
        {
            db = new DatabaseEntities();

            Student Student = db.Students.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);

            return Student;
        }

        public bool AddStudent(Student _Student)
        {
            using (db = new DatabaseEntities())
            {
                db.Students.Add(_Student);
                db.SaveChanges();
            }

            return true;
        }

        public bool UpdateStudent(Student _Student)
        {
            using (db = new DatabaseEntities())
            {
                db.Entry(_Student).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

    }
}