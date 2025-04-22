using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskCalendar.Helping_Classes
{
    public class TaskDTO
    {

        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string PopoverString { get; set; }
        public string color { get; set; }
        public string url { get; set; }
        public string NewEndDate { get; set; }

    }
}