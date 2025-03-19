using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBox.Helping_Classes
{
    public class MessageDTO
    {
        public int Id { set; get; }
        
        public int SenderId { set; get; }
        public string SenderName { set; get; }

        public string Message { set; get; }
    }
}