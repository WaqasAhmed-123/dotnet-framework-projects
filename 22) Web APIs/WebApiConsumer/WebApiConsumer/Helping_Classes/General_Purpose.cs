using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace WebApiConsumer.Helping_Classes
{
    public class General_Purpose
    {

        public UserDTO ValidateUser()
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal; // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();


            UserDTO loggedInUser = null;
            if (userId != null)
            {
                loggedInUser = new UserDTO()
                {
                    Id = Convert.ToInt32(identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                    Name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault(),
                    Email = identity.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault(),
                    AccessToken = identity.Claims.Where(c => c.Type == "AccessToken").Select(c => c.Value).SingleOrDefault()
                };
            }

            return loggedInUser;
        }
    }
}