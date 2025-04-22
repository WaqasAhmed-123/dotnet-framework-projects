using AdminPanel.DAL;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.BL
{
    public class StudentBL
    {
        #region Student
        public List<Student> GetActiveStudentsList()
        {
            return new StudentDAL().GetActiveStudentsList();
        }

        public List<Student> GetInactiveStudentsList()
        {
            return new StudentDAL().GetInactiveStudentsList();
        }

        public Student GetStudentbyId(int _id)
        {
            return new StudentDAL().getStudentById(_id);
        }

        public bool AddStudent(Student _Student)
        {
            if (_Student.Fname == null || _Student.Lname == null || _Student.Class == null ||
                _Student.CreatedAt == null)
            {
                return false;
            }

            return new StudentDAL().AddStudent(_Student);
        }

        public bool UpdateStudent(Student _Student)
        {
            if (_Student.Fname == null || _Student.Lname == null || _Student.Class == null ||
                _Student.CreatedAt == null)
            {
                return false;
            }

            return new StudentDAL().UpdateStudent(_Student);
        }

        #endregion
    }
}