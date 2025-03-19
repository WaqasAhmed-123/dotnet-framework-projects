using AdminPanel.BL;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public ActionResult AddStudent()
        {
            List<Teacher> teacherlist = new TeacherBL().GetActiveTeachersList();
            
            ViewBag.CssVal = "none";
            ViewBag.CssVal1 = "none";
            ViewBag.tlist = teacherlist;

            return View();
        }

        [HttpPost]
        [ActionName("AddStudent")]
        public ActionResult AddStudent(string fname="", string lname="", int tid=-1, int clas=-1, string address="")
        {
            if (fname.Length < 1 || lname.Length < 1 || tid <= 0
                || clas <= 0 || address.Length < 1)
            {
                ViewBag.fn = fname;
                ViewBag.ln = lname;
                ViewBag.t = tid;
                ViewBag.cl = clas;
                ViewBag.ad = address;

                ViewBag.CssVal = "inline";
                ViewBag.CssVal1 = "none";
            }
            else
            {
                Student student = new Student()
                {
                    Fname = fname,
                    Lname = lname,
                    TeacherId = tid,
                    Class = clas,
                    Address = address,
                    IsActive = 1,
                    CreatedAt = DateTime.Now
                };


                bool check = new StudentBL().AddStudent(student);

                if (check == true)
                {
                    ViewBag.CssVal = "none";
                    ViewBag.CssVal1 = "inline";
                }
            }
            List<Teacher> teacherlist = new TeacherBL().GetActiveTeachersList();
            ViewBag.tlist = teacherlist;

            return View();
        }




        public ActionResult GetStudent()
        {
            List<Student> studentlist = new StudentBL().GetActiveStudentsList();

            ViewBag.slist = studentlist;

            return View();
        }






        [HttpGet]
        public ActionResult EditStudent(int id)
        {
            Student getstudent = new StudentBL().GetStudentbyId(id);
            List<Teacher> teacherlist = new TeacherBL().GetActiveTeachersList();


            ViewBag.student = getstudent;
            ViewBag.teacher = teacherlist;
            ViewBag.CssVal = "none";


            return View();
        }


        [HttpPost]
        [ActionName("EditStudent")]
        public ActionResult EditStudent(int id=-1, string fname="", string lname="", int tid=-1, int clas=-1, string address="")
        {

            if (fname.Length > 0 && lname.Length > 0 && clas > 0
                && address.Length > 0)
            {
                Student student = new Student()
                {
                    Id = id,
                    Fname = fname,
                    Lname = lname,
                    TeacherId = tid,
                    Class = clas,
                    Address = address,
                    IsActive = 1,
                    CreatedAt = DateTime.Now
                };


                bool check = new StudentBL().UpdateStudent(student);



                return RedirectToAction("GetStudent");

            }
            else
            {
                ViewBag.CssVal = "inline";

                EditStudent(id);
            }

            return View();
        }




        public ActionResult DeleteStudent(int id)
        {
            Student GetStudent = new StudentBL().GetStudentbyId(id);

            Student student = new Student()
            {
                Id = GetStudent.Id,
                Fname = GetStudent.Fname,
                Lname = GetStudent.Lname,
                TeacherId = GetStudent.TeacherId,
                Class = GetStudent.Class,
                Address = GetStudent.Address,
                IsActive = 0,
                CreatedAt = GetStudent.CreatedAt
            };

            bool check = new StudentBL().UpdateStudent(student);


            return RedirectToAction("GetStudent");
        }
    }
}