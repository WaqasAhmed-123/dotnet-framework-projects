using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeCreator.Models
{
    public class CodeDTO
    {
        public int Id { get; set; }
        public string EncId { get; set; }
        public string TableName { get; set; }
        public string DbArray { get; set; }
    }
}