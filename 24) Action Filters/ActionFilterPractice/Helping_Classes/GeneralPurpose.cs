using ActionFilterPractice.BL;
using ActionFilterPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace ActionFilterPractice.Helping_Classes
{
    public class GeneralPurpose
    {

        public User ValidateLoggedinUser()
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal; // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();
            User loggedInUser = new UserBL().GetUserById(Convert.ToInt32(userId));

            return loggedInUser;
        }


        public bool ValidateEmail(string email = "", int id = -1)
        {
            int emailCount = 0;

            if (id != -1)
            {
                emailCount = new UserBL().GetActiveUserList().Where(x => x.Email.ToLower() == email.ToLower() && x.Id != id).Count();
            }
            else
            {
                emailCount = new UserBL().GetActiveUserList().Where(x => x.Email.ToLower() == email.ToLower()).Count();
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
    }
}