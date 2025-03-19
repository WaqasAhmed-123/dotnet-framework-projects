using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiAdvanced.Models;

namespace WebApiAdvanced.DAL
{
    public class TaskDAL
    {
        public List<Task> GetTasks()
        {
            DatabaseEntities db = new DatabaseEntities();

            return db.Tasks.ToList();
        }

        public List<Task> GetActiveTasks()
        {
            DatabaseEntities db = new DatabaseEntities();

            return db.Tasks.Where(x=>x.IsActive == 1).ToList();
        }

        public List<Task> GetTasksByUserId(int _Id)
        {
            DatabaseEntities db = new DatabaseEntities();

            return db.Tasks.Where(x => x.UserId== _Id && x.IsActive == 1).ToList();
        }

        public bool AddTask(Task _task, DatabaseEntities de=null)
        {
            try
            {
                if(de != null)
                {
                    de.Tasks.Add(_task);
                    de.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTask(Task _task, DatabaseEntities de = null)
        {
            try
            {
                if(de!=null)
                {
                    de.Entry(_task).State = System.Data.Entity.EntityState.Modified;
                    de.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteTask(int _Id)
        {
            DatabaseEntities db = new DatabaseEntities();

            db.Tasks.Remove(db.Tasks.Where(x => x.Id == _Id).FirstOrDefault());
            db.SaveChanges();

            return true;
        }
    }
}