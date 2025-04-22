using AdminPanel.DAL;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.BL
{
    public class TeacherBL
    {
        #region Teacher
        public List<Teacher> GetActiveTeachersList()
        {
            return new TeacherDAL().GetActiveTeachersList();
        }

        public List<Teacher> GetInactiveTeachersList()
        {
            return new TeacherDAL().GetInactiveTeachersList();
        }

        public Teacher GetTeacherbyId(int _id)
        {
            return new TeacherDAL().getTeacherById(_id);
        }

        public bool AddTeacher(Teacher _Teacher)
        {
            if (_Teacher.Fname == null || _Teacher.Lname == null || _Teacher.Email == null ||
                _Teacher.Age == null || _Teacher.Address == null || _Teacher.CreatedAt == null)
            {
                return false;
            }

            return new TeacherDAL().AddTeacher(_Teacher);
        }

        public bool UpdateTeacher(Teacher _Teacher)
        {
            if (_Teacher.Fname == null || _Teacher.Lname == null || _Teacher.Email == null ||
                _Teacher.Age == null || _Teacher.Address == null || _Teacher.CreatedAt == null)
            {
                return false;
            }

            return new TeacherDAL().UpdateTeacher(_Teacher);
        }

        #endregion
    }
}