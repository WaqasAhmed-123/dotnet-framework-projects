using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using WebApiAdvanced.BL;
using WebApiAdvanced.Models;

namespace WebApiAdvanced.Helping_Classes
{
    public class General_Purpose
    {

        public UserDTO ValidateUser()
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal; // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();

            
            UserDTO loggedInUser = null;
            if(userId != null)
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

        public string GenerateToken(int UserId)
        {
            DatabaseEntities db = new DatabaseEntities();

            //generating Unique Access Token with time span
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            //generating Access Token with time span

            Authentication obj = new Authentication()
            {
                UserId = UserId,
                AccessToken = token,
                IsActive = 1,
                CreatedAt = DateTime.Now
            };

            bool chk = new AuthenticationBL().AddAuthentication(obj, db);

            return chk==true? token: "";
        }
    }
}