using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScormReading.Models;
using ScormReading.Helping_Classes;
using ScormReading.BL;
using System.Threading.Tasks;

namespace ScormReading.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseEntities de = new DatabaseEntities();
        private GeneralPurpose gp = new GeneralPurpose();
        
        public ActionResult Index(string msg="")
        {
            if(gp.ValidateLoggedinUser() == null)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired" });
            }

            ViewBag.Message = msg;

            return View();
        }

        public ActionResult CourseResult(string msg="")
        {
            if (gp.ValidateLoggedinUser() == null)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired" });
            }

            ViewBag.Message = msg;

            return View();
        }

        public ActionResult DeleteUserCourse(int id)
        {
            if (gp.ValidateLoggedinUser() == null)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired" });
            }

            UserCourse uc = new UserCourseBL().GetActiveUserCourseById(id, de);
            uc.IsActive = 0;

            new UserCourseBL().UpdateUserCourse(uc, de);

            return RedirectToAction("CourseResult", "Home", new { msg="Course deleted successfully"});
        }


        #region Ajax

        [HttpPost]
        public ActionResult ValidateUserCourse(string courseTitle, string courseLink, int haveQuiz)
        {
            UserCourse uc = new UserCourseBL().GetActiveUserCourseList(de).Where(x=> x.UserId == gp.ValidateLoggedinUser().Id && x.CourseTitle == courseTitle).FirstOrDefault();
            if (uc != null)
            {
                UserCourse obj = new UserCourse()
                {
                    UserId = uc.UserId,
                    CourseTitle = uc.CourseTitle,
                    CourseId = uc.CourseId,
                    percentageCompleted = uc.percentageCompleted,
                    currentLesson = uc.currentLesson,
                    IsCompleted = uc.IsCompleted,
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                UserCourse obj = new UserCourse()
                {
                    UserId = gp.ValidateLoggedinUser().Id,
                    CourseTitle = courseTitle,
                    CourseLink = courseLink,
                    CourseId = null,
                    percentageCompleted = 0,
                    currentLesson = null,
                    IsCompleted = 0,
                    IsPassed = null,
                    //Result = haveQuiz == 1 ? null : -1,
                    IsActive = 1,
                    CreatedAt = GeneralPurpose.DateTimeNow()
                };

                if(haveQuiz == 1)
                {
                    obj.Result = null;
                }
                else
                {
                    obj.Result = -1;
                }

                new UserCourseBL().AddUserCourse(obj, de);

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUserCourseLesson(string courseTitle, string currentLesson, string lessonHeading = "", string lessonNumber = "")
        {
            UserCourse uc = new UserCourseBL().GetActiveUserCourseList(de).Where(x => x.UserId == gp.ValidateLoggedinUser().Id && x.CourseTitle == courseTitle).FirstOrDefault();
            if (uc != null)
            {
                if (uc.currentLesson != currentLesson)
                {
                    await Task.Run(() =>
                    {
                        uc.currentLesson = currentLesson;
                        uc.lessonHeading = lessonHeading;
                        uc.lessonNumber = lessonNumber;

                        new UserCourseBL().UpdateUserCourse(uc, de);
                    });
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public async Task<ActionResult> UpdateUserCourseProgress(string courseTitle, string currentLesson, int percentageCompleted = 0)
        {
            UserCourse uc = new UserCourseBL().GetActiveUserCourseList(de).Where(x => x.UserId == gp.ValidateLoggedinUser().Id && x.CourseTitle == courseTitle).FirstOrDefault();
            if (uc != null)
            {
                await Task.Run(() =>
                {
                    int flag = 0;

                    if (percentageCompleted >= uc.percentageCompleted && uc.IsCompleted != 1)
                    {
                        uc.percentageCompleted = percentageCompleted;
                        flag = 1;
                    }
                    if(uc.currentLesson != currentLesson)
                    {
                        uc.currentLesson = currentLesson;
                        flag = 1;
                    }

                    if(flag == 1)
                    {
                        new UserCourseBL().UpdateUserCourse(uc, de);
                    }
                });

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<ActionResult> UpdateQuizResult(string courseTitle, string courseLink, string status, int result, string CourseId)
        {
            UserCourse uc = new UserCourseBL().GetActiveUserCourseList(de).Where(x => x.UserId == gp.ValidateLoggedinUser().Id && x.CourseTitle == courseTitle).FirstOrDefault();
            if (uc != null)
            {
                bool chk = false;
                await Task.Run(() =>
                {
                    uc.CourseId = CourseId;
                    uc.percentageCompleted = 100;
                    uc.currentLesson = null;
                    uc.IsCompleted = 1;
                    uc.IsPassed = status == "true" ? 1 : 0;
                    uc.Result = result;

                    chk = new UserCourseBL().UpdateUserCourse(uc, de);
                });

                return Json(chk, JsonRequestBehavior.AllowGet);
            }
            else
            {
                UserCourse obj = new UserCourse()
                {
                    UserId = gp.ValidateLoggedinUser().Id,
                    CourseTitle = courseTitle,
                    CourseLink = courseLink,
                    CourseId = CourseId,
                    percentageCompleted = 100,
                    currentLesson = null,
                    IsCompleted = 1,
                    IsPassed = status == "true" ? 1 : 0,
                    Result = result,
                    IsActive = 1,
                    CreatedAt = GeneralPurpose.DateTimeNow()
                };

                bool chk = false;
                await Task.Run(() =>
                {
                    chk = new UserCourseBL().AddUserCourse(obj, de);
                });
                
                return Json(chk, JsonRequestBehavior.AllowGet);
            }
        }
        
        #endregion

    }
}