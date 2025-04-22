using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskCalendar.DAL;
using TaskCalendar.Models;

namespace TaskCalendar.BL
{
    public class TaskBL
    {

        public List<Task> GetTaskList()
        {
            return new TaskDAL().GetTaskList();
        }

        public List<Task> GetActiveTaskList()
        {
            return new TaskDAL().GetActiveTaskList();
        }

        public List<Task> GetInactiveTaskList()
        {
            return new TaskDAL().GetInactiveTaskList();
        }

        public Task GetTaskbyId(int _id)
        {
            return new TaskDAL().GetTaskById(_id);
        }

        public bool AddTask(Task _Task)
        {
            if (_Task.Title == null || _Task.TaskDetail == null || _Task.StartDate == null ||
                _Task.EndDate == null || _Task.Priority == null || _Task.IsActive == null || _Task.CreatedAt == null)
            {
                return false;
            }

            return new TaskDAL().AddTask(_Task);
        }

        public bool UpdateTask(Task _Task)
        {
            if (_Task.Id == -1 || _Task.Title == null || _Task.TaskDetail == null || _Task.StartDate == null ||
                _Task.EndDate == null || _Task.Priority == null || _Task.IsActive == null || _Task.CreatedAt == null)
            {
                return false;
            }

            return new TaskDAL().UpdateTask(_Task);
        }
    }
}