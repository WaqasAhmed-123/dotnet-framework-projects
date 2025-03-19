using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApiConsumer.Helping_Classes;


namespace WebApiConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly General_Purpose gp = new General_Purpose();
        // GET: Home
        public ActionResult Index(string msg="", string color="black")
        {
            if (gp.ValidateUser() == null)
            {
                return RedirectToAction("Login", "Auth", new { msg="Session Expired, Please Login Again!"});
            }

            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        public ActionResult AddTask(string msg = "", string color = "black")
        {
            if (gp.ValidateUser() == null)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired, Please Login Again!" });
            }

            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        public ActionResult PostAddTask(TaskDTO _task)
        {
            _task.UserId = gp.ValidateUser().Id;
            _task.IsActive = 1;
            _task.CreatedAt = DateTime.Now;

            using (var client = new HttpClient())
            {
                string BaseApiUrl = "https://localhost:44350/api/HomeAPI";

                string inputJson = (new JavaScriptSerializer()).Serialize(_task);
                HttpContent PassingContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(BaseApiUrl + "/AddTask", PassingContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AddTask", "Home", new { msg = "Task Inserted Successfully", color = "green" });
                }
                else
                {
                    return RedirectToAction("AddTask", "Home", new { msg = "Somethings' Wrong", color = "red" });
                }
            }
        }

        public ActionResult ViweTask(string msg = "", string color = "black")
        {
            if (gp.ValidateUser() == null)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Session Expired, Please Login Again!" });
            }

            List<TaskDTO> tlist = new List<TaskDTO>(); 
            using (var client = new HttpClient())
            {
                string BaseApiUrl = "https://localhost:44350/api/HomeAPI";

                HttpResponseMessage response = client.GetAsync(BaseApiUrl + "/GetTask?UserId=" + gp.ValidateUser().Id.ToString() + "&Token=" + gp.ValidateUser().AccessToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    var readTask = response.Content.ReadAsStringAsync();
                    readTask.Wait();

                    string a = readTask.Result;
                    if(a != "0")
                    {
                        tlist = (new JavaScriptSerializer()).Deserialize<List<TaskDTO>>(response.Content.ReadAsStringAsync().Result);
                        
                        ViewBag.Tlist = tlist;

                        ViewBag.Message = msg;
                        ViewBag.Color = color;
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { msg = "Token Expired, Try to Login Again", color = "red" });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { msg = "Somethings' Wrong", color = "red" });
                }
            }
        }


        public ActionResult GetTaskById(int id)
        {
            TaskDTO task = new TaskDTO();
            using (var client = new HttpClient())
            {
                string BaseApiUrl = "https://localhost:44350/api/HomeAPI";

                HttpResponseMessage response = client.GetAsync(BaseApiUrl + "/GetTaskById?TaskId=" + id.ToString() + "&Token=" + gp.ValidateUser().AccessToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    var readTask = response.Content.ReadAsStringAsync();
                    readTask.Wait();

                    string a = readTask.Result;
                    if (a != "0")
                    {
                        task = (new JavaScriptSerializer()).Deserialize<TaskDTO>(response.Content.ReadAsStringAsync().Result);

                        return Json(task, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}