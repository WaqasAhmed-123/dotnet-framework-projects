using AdminPanel.BL;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class TeacherController : Controller
    {
        [HttpGet]
        public ActionResult AddTeacher()
        {
            ViewBag.CssVal = "none";
            ViewBag.CssVal1 = "none";
            return View();
        }

        [HttpPost]
        [ActionName("AddTeacher")]
        public ActionResult AddTeacher(string fname, string lname, string email, int age, string address)
        {
            if(fname.Length < 0 || lname.Length < 0 || email.Length < 0 
                || age <= 0 || address.Length < 0)
            {
                TempData["fn"] = fname;
                TempData["ln"] = lname;
                TempData["em"] = email;
                TempData["ad"] = address;

                ViewBag.CssVal = "inline";
                ViewBag.CssVal1 = "none";
            }
            else 
            {
                Teacher teacher = new Teacher() { Fname = fname, Lname = lname,
                                    Email = email, Age = age, Address = address, 
                                    IsActive = 1, CreatedAt = DateTime.Now } ;


                bool check = new TeacherBL().AddTeacher(teacher);

                if (check == true)
                {
                    ViewBag.CssVal = "none";
                    ViewBag.CssVal1 = "inline";
                }
            }

            return View();
        }

        public ActionResult GetTeacher()
        {
            List<Teacher> teacherlist = new TeacherBL().GetActiveTeachersList();

            return View(teacherlist);
        }


        [HttpGet]
        public ActionResult EditTeacher(int id)
        {
            

            Teacher getteacher = new TeacherBL().GetTeacherbyId(id);

            return View(getteacher);
        }


        [HttpPost]
        [ActionName("EditTeacher")]
        public ActionResult EditTeacher(int id, string fname, string lname, string email, int age, string address)
        {

            if (fname.Length > 0 && lname.Length > 0 && email.Length > 0
                && age != 0 && address.Length > 0)
            {
                Teacher teacher = new Teacher()
                {
                    Id = id ,
                    Fname = fname,
                    Lname = lname,
                    Email = email,
                    Age = age,
                    Address = address,
                    IsActive = 1,
                    CreatedAt = DateTime.Now
                };


                bool check = new TeacherBL().UpdateTeacher(teacher);

                

                return RedirectToAction("GetTeacher");
  
            }
            else
            {
                ViewData["CssVal"] = "All Fields are Required!!";

                EditTeacher(id);
            }

            return View();
        }





        public ActionResult DeleteTeacher(int id)
        {
            Teacher GetTeacher = new TeacherBL().GetTeacherbyId(id);

            Teacher teacher = new Teacher()
            {
                Id = GetTeacher.Id,
                Fname = GetTeacher.Fname,
                Lname = GetTeacher.Lname,
                Email = GetTeacher.Email,
                Age = GetTeacher.Age,
                Address = GetTeacher.Address,
                IsActive = 0,
                CreatedAt = GetTeacher.CreatedAt
            };

            bool check = new TeacherBL().UpdateTeacher(teacher);


            return RedirectToAction("GetTeacher");
        }
    }
}