using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiAdvanced.Helping_Classes
{
    public class UserDTO
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string AccessToken { set; get; }
    }
}