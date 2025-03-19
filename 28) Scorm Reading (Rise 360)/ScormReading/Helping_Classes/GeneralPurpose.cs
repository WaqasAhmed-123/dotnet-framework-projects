using ScormReading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using ScormReading.BL;

namespace ScormReading.Helping_Classes
{
    public class GeneralPurpose
    {
        DatabaseEntities de = new DatabaseEntities();
        public User ValidateLoggedinUser()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();
            User loggedInUser = new UserBL().GetActiveUserById(Convert.ToInt32(userId), de);

            return loggedInUser;
        }

        public bool ValidateEmail(string email = "", int id = -1)
        {
            int emailCount = 0;

            if (id != -1)
            {
                emailCount = new UserBL().GetActiveUsersList(de).Where(x => x.Email.ToLower() == email.ToLower() && x.Id != id).Count();
            }
            else
            {
                emailCount = new UserBL().GetActiveUsersList(de).Where(x => x.Email.ToLower() == email.ToLower()).Count();
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