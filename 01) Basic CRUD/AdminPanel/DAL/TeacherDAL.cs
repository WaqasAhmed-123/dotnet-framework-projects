using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.DAL
{
    public class TeacherDAL
    {
        SchoolEntities db;

        #region Teacher
        public List<Teacher> GetActiveTeachersList()
        {
            List<Teacher> Teachers;
            db = new SchoolEntities();

            Teachers = db.Teachers.Where(x => x.IsActive == 1).ToList();

            return Teachers;
        }

        public List<Teacher> GetInactiveTeachersList()
        {
            List<Teacher> Teachers;
            db = new SchoolEntities();

            Teachers = db.Teachers.Where(x => x.IsActive == 0).ToList();

            return Teachers;
        }

        public Teacher getTeacherById(int _Id)
        {
            db = new SchoolEntities();

            Teacher Teacher = db.Teachers.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);

            return Teacher;
        }

        public bool AddTeacher(Teacher _Teacher)
        {
            using (db = new SchoolEntities())
            {
                db.Teachers.Add(_Teacher);
                db.SaveChanges();
            }

            return true;
        }

        public bool UpdateTeacher(Teacher _Teacher)
        {
            using (db = new SchoolEntities())
            {
                db.Entry(_Teacher).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

        #endregion
    }
}