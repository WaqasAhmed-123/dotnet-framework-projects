using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBox.Helping_Classes
{
    public class ContactDTO
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public int Count { set; get; }

        public int IsLoggedin { set; get; }
    }
}