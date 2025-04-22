using DbTransactionPractice.BL;
using DbTransactionPractice.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DbTransactionPractice.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult AddStudent(string msg="")
        {
            ViewBag.Message = msg;
            return View();
        }

        [HttpPost]
        public ActionResult PostAddStudent(Student _student, string Course = "")
        {
            using (var context = new DatabaseEntities())
            {
                context.Database.Log = Console.Write;

                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Student s = new Student()
                        {
                            RollNo = _student.RollNo,
                            Name = _student.Name,
                            Age = _student.Age,
                            Class = _student.Class,
                            IsActive = 1,
                            CreatedAt = DateTime.Now
                        };


                        Course c = new Course()
                        {
                            //StudentRollNo = -1,  //testing transaction error
                            StudentRollNo = _student.RollNo,
                            CourseName = Course,
                            IsActive = 1,
                            CreatedAt = DateTime.Now
                        };

                        if (!new StudentBL().AddStudent(s) || !new CourseBL().AddCourse(c))
                        {
                            throw new Exception();
                        }
                        else
                        {
                            transaction.Commit();
                            return RedirectToAction("AddStudent", "Home", new { msg = "Student Add successfull!" });
                        }

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return RedirectToAction("AddStudent", "Home", new { msg = "Transaction Error" });
                    }

                } 
            }
            
        }

    }
}