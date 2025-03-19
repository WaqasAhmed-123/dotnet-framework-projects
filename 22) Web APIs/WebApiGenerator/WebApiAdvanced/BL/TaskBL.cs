using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiAdvanced.Models;
using WebApiAdvanced.DAL;

namespace WebApiAdvanced.BL
{
    public class TaskBL
    {

        public List<Task> GetTasks()
        {
            return new TaskDAL().GetTasks();
        }

        public List<Task> GetActiveTasks()
        {
            return new TaskDAL().GetActiveTasks();
        }

        public List<Task> GetTasksByUserId(int _Id)
        {
            return new TaskDAL().GetTasksByUserId(_Id);
        }

        public bool AddTask(Task _task, DatabaseEntities de = null)
        {
            return new TaskDAL().AddTask(_task, de);
        }

        public bool UpdateTask(Task _task, DatabaseEntities de = null)
        {
            return new TaskDAL().UpdateTask(_task, de);
        }

        public bool DeleteTask(int _Id)
        {
            return new TaskDAL().DeleteTask(_Id);
        }
    }
}