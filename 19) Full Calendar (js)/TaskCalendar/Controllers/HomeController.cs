using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using TaskCalendar.BL;
using TaskCalendar.Helping_Classes;
using TaskCalendar.Models;

namespace TaskCalendar.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string msg="")
        {
            ViewBag.Message = msg;
            return View();
        }


        public ActionResult Calender2()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetTaskData(string startdate = "", string enddate="")
        {

            List<Task> tasklist = new TaskBL().GetActiveTaskList();

            List<TaskDTO> tlist = new List<TaskDTO>();

            string color = "";
            foreach(Task item in tasklist)
            {
                if(item.Priority == 1)
                {
                    color = "red";
                }
                else if(item.Priority == 2)
                {
                    color = "blue";
                }
                else
                {
                    color = "green";
                }

                TaskDTO obj = new TaskDTO()
                {
                    id = item.Id,
                    title = item.Title,
                    start = item.StartDate.Value.ToString("yyyy-MM-dd"),
                    //need to add 1 day in end date, because it is exclusive and will always show 1 less day
                    end = (Convert.ToDateTime(item.EndDate).AddDays(1)).ToString("yyyy-MM-dd"),
                    PopoverString = @"<b>Details: </b>" + item.TaskDetail +  "<br> <b>Start Date: </b>" + item.StartDate.Value.ToString("MM/dd/yyyy") + "<br> <b>End Date: </b>" + item.EndDate.Value.ToString("MM/dd/yyyy"),
                    color = color,
                    url = "../Home/TaskDetail?_Id=" + item.Id,
                    NewEndDate = item.EndDate.Value.ToString("yyyy-MM-dd")
                };

                tlist.Add(obj);
            }

            return new JsonResult { Data = tlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult AddTask(Task _task)
        {

            Task t = new Task() 
            {
                Title = _task.Title,
                TaskDetail = _task.TaskDetail,
                StartDate = _task.StartDate,
                EndDate = Convert.ToDateTime(_task.EndDate).AddDays(-1),
                Priority = _task.Priority,
                IsActive = 1,
                CreatedAt = DateTime.Now
            };

            bool chk = new TaskBL().AddTask(t);

            if(chk == true)
            {
                return RedirectToAction("Index", "Home", new { msg = "Task Added successfully" });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { msg = "Error" });
            }
        }


        public ActionResult TaskDetail(int _Id = -1)
        {
            Task task = new TaskBL().GetTaskbyId(_Id);

            ViewBag.Task = task;

            return View();
        }

        [HttpPost]
        public JsonResult TaskUpdateOnDrag(int Id = -1, string Startdate="")
        {
            Task task = new TaskBL().GetTaskbyId(Id);

            DateTime d1 = Convert.ToDateTime(task.StartDate);
            DateTime d2 = Convert.ToDateTime(task.EndDate);
            TimeSpan tdiffer = (d2 - d1);

            int day = Convert.ToInt32(tdiffer.TotalDays);

            int msg = -1;

            Task t = new Task() 
            {
                Id = task.Id,
                Title = task.Title,
                TaskDetail = task.TaskDetail,
                StartDate = Convert.ToDateTime(Startdate),
                EndDate = Convert.ToDateTime(Startdate).AddDays(day),
                Priority = task.Priority,
                IsActive = task.IsActive,
                CreatedAt = task.CreatedAt
            };

            bool chk = new TaskBL().UpdateTask(t);

            if(chk == true)
            {
                msg = 1;
            }

            return new JsonResult { Data = msg, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        [HttpPost]
        public JsonResult TaskUpdateOnResize(int Id = -1, string Enddate = "")
        {
            Task task = new TaskBL().GetTaskbyId(Id);

            int msg = -1;

            Task t = new Task()
            {
                Id = task.Id,
                Title = task.Title,
                TaskDetail = task.TaskDetail,
                StartDate = task.StartDate,//Convert.ToDateTime(Startdate),
                EndDate = Convert.ToDateTime(Enddate).AddDays(-1), //need to reove 1 day because end date is always 1 more than selected
                Priority = task.Priority,
                IsActive = task.IsActive,
                CreatedAt = task.CreatedAt
            };

            bool chk = new TaskBL().UpdateTask(t);

            if (chk == true)
            {
                msg = 1;
            }

            return new JsonResult { Data = msg, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult DeleteTask(int Id=-1)
        {
            Task task = new TaskBL().GetTaskbyId(Id);

            Task t = new Task()
            {
                Id = task.Id,
                Title = task.Title,
                TaskDetail = task.TaskDetail,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                Priority = task.Priority,
                IsActive = 0,
                CreatedAt = task.CreatedAt
            };

            bool chk = new TaskBL().UpdateTask(t);

            if (chk == true)
            {
                return RedirectToAction("Index", "Home", new { msg = "Task Deleted successfully" });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { msg = "Error" });
            }
        }
    }
}