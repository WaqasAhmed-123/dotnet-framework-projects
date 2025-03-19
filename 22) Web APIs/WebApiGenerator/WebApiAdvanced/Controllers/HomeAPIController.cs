using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiAdvanced.Models;
using WebApiAdvanced.BL;
using WebApiAdvanced.Helping_Classes;

namespace WebApiAdvanced.Controllers
{
    public class HomeAPIController : ApiController
    {
        private readonly General_Purpose gp = new General_Purpose();
        private readonly DatabaseEntities de = new DatabaseEntities();

        [Route("api/HomeAPI/AddTask")]
        [HttpPost]
        public IHttpActionResult AddTask(Task _task)
        {
            Task obj = new Task()
            {
                UserId = _task.UserId,
                Name = _task.Name,
                Date = _task.Date,
                Duration = _task.Duration,
                IsActive = 1,
                CreatedAt = DateTime.Now
            };

            bool chkTask = new TaskBL().AddTask(obj, de);
            if (chkTask)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }


        [Route("api/HomeAPI/GetTask")]
        [HttpGet]
        public IHttpActionResult GetTask(string UserId, string Token)
        {
            Authentication auth = new AuthenticationBL().GetAuthentications().Where(x => x.AccessToken == Token).FirstOrDefault();
            if(auth == null)
            {
                return Ok(0);
            }

            List<Task> tlist = new TaskBL().GetActiveTasks().Where(x => x.UserId == Convert.ToInt32(UserId)).ToList();

            List<Task> tdto = new List<Task>();

            foreach (Task t in tlist)
            {
                Task obj = new Task()
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Name = t.Name,
                    Date = t.Date,
                    Duration = t.Duration,
                    IsActive = t.IsActive,
                    CreatedAt = t.CreatedAt 
                };

                tdto.Add(obj);
            }

            return Ok(tdto);
        }


        [Route("api/HomeAPI/GetTaskById")]
        [HttpGet]
        public IHttpActionResult GetTaskById(string TaskId, string Token)
        {
            Authentication auth = new AuthenticationBL().GetAuthentications().Where(x => x.AccessToken == Token).FirstOrDefault();
            if (auth == null)
            {
                return Ok(0);
            }

            Task t = new TaskBL().GetActiveTasks().Where(x => x.Id == Convert.ToInt32(TaskId)).FirstOrDefault();

            Task obj = new Task()
            {
                Id = t.Id,
                UserId = t.UserId,
                Name = t.Name,
                Date = t.Date,
                Duration = t.Duration,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt
            };


            return Ok(obj);
        }


        [Route("api/HomeAPI/DeleteTask")]
        [HttpGet]
        public IHttpActionResult DeleteTask(string TaskId, string Token)
        {
            Authentication auth = new AuthenticationBL().GetAuthentications().Where(x => x.AccessToken == Token).FirstOrDefault();
            if (auth == null)
            {
                return Ok(0);
            }

            Task t = new TaskBL().GetActiveTasks().Where(x => x.Id == Convert.ToInt32(TaskId)).FirstOrDefault();

            Task obj = new Task()
            {
                Id = t.Id,
                UserId = t.UserId,
                Name = t.Name,
                Date = t.Date,
                Duration = t.Duration,
                IsActive = 0,
                CreatedAt = t.CreatedAt
            };

            bool chk = new TaskBL().UpdateTask(obj, de);

            if(chk)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }

    }
}
