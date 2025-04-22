using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoredProcedureAdvanced.Helping_Classes
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }




        public int getId()
        {
            UserDTO udto = (UserDTO)HttpContext.Current.Session["Session"];
            if (udto == null)
                return -1;

            return udto.Id;
        }

        public string getName()
        {
            UserDTO udto = (UserDTO)HttpContext.Current.Session["Session"];
            if (udto == null)
                return null;

            return udto.Name;
        }

        public string getEmail()
        {
            UserDTO udto = (UserDTO)HttpContext.Current.Session["Session"];
            if (udto == null)
                return null;

            return udto.Email;
        }

        public string getPassword()
        {
            UserDTO udto = (UserDTO)HttpContext.Current.Session["Session"];
            if (udto == null)
                return null;

            return udto.Password;
        }

        public int getRole()
        {
            UserDTO udto = (UserDTO)HttpContext.Current.Session["Session"];
            if (udto == null)
                return -1;

            return udto.Role;
        }


    }
}