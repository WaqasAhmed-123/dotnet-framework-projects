using AjaxPractice.DAL;
using AjaxPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AjaxPractice.BL
{
    public class StudentBL
    {
        #region Student

        public List<Student> GetStudentsList()
        {
            return new StudentDAL().GetStudentsList();
        }

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
            if (_Student.StudentName == null || _Student.StudentAddress == null)
            {
                return false;
            }

            return new StudentDAL().AddStudent(_Student);
        }

        public bool UpdateStudent(Student _Student)
        {
            if (_Student.StudentName == null || _Student.StudentAddress == null)
            {
                return false;
            }

            return new StudentDAL().UpdateStudent(_Student);
        }

        #endregion
    }
}