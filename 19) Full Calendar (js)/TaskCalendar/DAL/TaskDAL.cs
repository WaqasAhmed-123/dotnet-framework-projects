using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskCalendar.Models;

namespace TaskCalendar.DAL
{
    public class TaskDAL
    {
        DatabaseEntities db;


        public List<Task> GetTaskList()
        {
            List<Task> Tasks;
            db = new DatabaseEntities();

            Tasks = db.Tasks.ToList();

            return Tasks;
        }

        public List<Task> GetActiveTaskList()
        {
            List<Task> Tasks;
            db = new DatabaseEntities();

            Tasks = db.Tasks.Where(x => x.IsActive == 1).ToList();

            return Tasks;
        }

        public List<Task> GetInactiveTaskList()
        {
            List<Task> Tasks;
            db = new DatabaseEntities();

            Tasks = db.Tasks.Where(x => x.IsActive == 0).ToList();

            return Tasks;
        }

        public Task GetTaskById(int _Id)
        {
            db = new DatabaseEntities();

            Task Task = db.Tasks.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);

            return Task;
        }

        public bool AddTask(Task _Task)
        {
            using (db = new DatabaseEntities())
            {
                db.Tasks.Add(_Task);
                db.SaveChanges();
            }

            return true;
        }

        public bool UpdateTask(Task _Task)
        {
            using (db = new DatabaseEntities())
            {
                db.Entry(_Task).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

    }
}