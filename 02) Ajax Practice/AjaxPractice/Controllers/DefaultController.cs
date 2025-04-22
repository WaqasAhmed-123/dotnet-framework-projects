using AjaxPractice.BL;
using AjaxPractice.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AjaxPractice.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostAddStudent(Student _s, string msg = "")
        {
            
            Student s = new Student()
            {
                StudentName = _s.StudentName,
                StudentAddress = _s.StudentAddress
            };

            bool check = new StudentBL().AddStudent(s);
            if(check == true)
            {
                msg = "done";
            }
            return View();
        }

        
        public ActionResult GetStudent()
        {
            return View();
        }

        public JsonResult GetStudentData()
        {

            List<Student> studentlist = new StudentBL().GetStudentsList();


            return Json(studentlist, JsonRequestBehavior.AllowGet);
        }


        public List<Student> GetStudent2()
        {

            var empList = new List<Student>()
    {
         new Student { StudentId=1, StudentName="Manas"},
         new Student { StudentId=2, StudentName="sg"},
         new Student { StudentId=3, StudentName="dsfh"}
    };

            return empList;
        }




    }
}