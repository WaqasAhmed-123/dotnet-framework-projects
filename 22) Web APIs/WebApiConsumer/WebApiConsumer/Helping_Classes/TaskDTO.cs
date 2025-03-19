using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiConsumer.Helping_Classes
{
    public class TaskDTO
    {
        public int Id { set; get; }
        public int UserId { set; get; }
        public string Name { set; get; }
        public DateTime Date { set; get; }
        public double Duration { set; get; }
        public int IsActive { set; get; }
        public DateTime CreatedAt { set; get; }
    }
}