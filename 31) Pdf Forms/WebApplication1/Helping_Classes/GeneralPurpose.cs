using System;
using System.Linq;
using WebApplication1.Models;
using WebApplication1.BL;

namespace WebApplication1.Helping_Classes
{
    public class GeneralPurpose
    {
        private readonly DatabaseEntities de = new DatabaseEntities();
        
        public bool ValidateEmail(string email = "", int id = -1)
        {
            int emailCount = 0;

            if (id != -1)
            {
                emailCount = new UserBL().GetAllUsersList(de).Count(x => x.IsActive != 0 && x.Id != id && x.Email.ToLower() == email.ToLower());
            }
            else
            {
                emailCount = new UserBL().GetAllUsersList(de).Count(x => x.IsActive != 0 && x.Email.ToLower() == email.ToLower());
            }

            if (emailCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static DateTime DateTimeNow()
        {
            return DateTime.Now;
        }

    }
}