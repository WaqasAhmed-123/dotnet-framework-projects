using DbTransactionPractice.DAL;
using DbTransactionPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbTransactionPractice.BL
{
    public class StudentBL
    {

        public List<Student> GetStudentList()
        {
            return new StudentDAL().GetStudentList();
        }

        public List<Student> GetActiveStudentList()
        {
            return new StudentDAL().GetActiveStudentList();
        }

        public List<Student> GetInactiveStudentList()
        {
            return new StudentDAL().GetInactiveStudentList();
        }

        public Student GetStudentbyId(int _id)
        {
            return new StudentDAL().GetStudentById(_id);
        }

        public bool AddStudent(Student _Student)
        {
            if (_Student.RollNo == -1 || _Student.Name == null || _Student.Age == null ||
                _Student.Class == null || _Student.IsActive == null || _Student.CreatedAt == null)
            {
                return false;
            }

            return new StudentDAL().AddStudent(_Student);
        }

        public bool UpdateStudent(Student _Student)
        {
            if (_Student.RollNo == -1 || _Student.Name == null || _Student.Age == null ||
                _Student.Class == null || _Student.IsActive == null || _Student.CreatedAt == null)
            {
                return false;
            }

            return new StudentDAL().UpdateStudent(_Student);
        }
    }
}