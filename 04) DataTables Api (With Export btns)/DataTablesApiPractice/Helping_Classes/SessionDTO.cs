using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTablesApiPractice.Helping_Classes
{
    public class SessionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }


        public int getId()
        {
            SessionDTO sdto = (SessionDTO)HttpContext.Current.Session["Session"];
            if (sdto == null)
                return -1;

            return sdto.Id;
        }

        public string getName()
        {
            SessionDTO sdto = (SessionDTO)HttpContext.Current.Session["Session"];
            if (sdto == null)
                return null;

            return sdto.Name;
        }

        public string getContact()
        {
            SessionDTO sdto = (SessionDTO)HttpContext.Current.Session["Session"];
            if (sdto == null)
                return null;

            return sdto.Contact;
        }


        public string getEmail()
        {
            SessionDTO sdto = (SessionDTO)HttpContext.Current.Session["Session"];
            if (sdto == null)
                return null;

            return sdto.Email;
        }

        public int getRole()
        {
            SessionDTO sdto = (SessionDTO)HttpContext.Current.Session["Session"];
            if (sdto == null)
                return -1;

            return sdto.Role;
        }


    }
}