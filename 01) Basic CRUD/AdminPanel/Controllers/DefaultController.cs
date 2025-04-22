using AdminPanel.BL;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class DefaultController : Controller
    {
        
        public ActionResult Index()
        {
            List<Teacher> teacherlist1 = new TeacherBL().GetActiveTeachersList();
            List<Teacher> teacherlist2 = new TeacherBL().GetInactiveTeachersList();

            List<Student> studentlist1 = new StudentBL().GetActiveStudentsList();
            List<Student> studentlist2 = new StudentBL().GetInactiveStudentsList();

            ViewBag.tlist1 = teacherlist1;
            ViewBag.tlist2 = teacherlist2;

            ViewBag.slist1 = studentlist1;
            ViewBag.slist2 = studentlist2;


            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }
    }
}