using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiAdvanced.BL;
using WebApiAdvanced.Helping_Classes;
using WebApiAdvanced.Models;

namespace WebApiAdvanced.Controllers
{
    //need to create webApi MVC project (Not simple MVC project)
    //For Creation of Web Api's We need;
    //WEBAPI 2 Controller - Empty (Right click on Controller -> Click Add -> Click Controller... -> Select WEB API 2 Controller - Empty -> Click Add)
    public class LoginAPIController : ApiController
    {
        private readonly General_Purpose gp = new General_Purpose();

        public IHttpActionResult PostValidateLogin(User _user)
        {
            string AccessToken = "";
            DatabaseEntities db = new DatabaseEntities();

            User u = new UserBL().GetActiveUsers().Where(x => x.Email.ToLower() == _user.Email.ToLower() && x.Password == _user.Password).FirstOrDefault();
            if (u == null)
            {
                return NotFound();
            }
            else
            {
                Authentication auth = new AuthenticationBL().GetAuthenticationByUserId(u.Id);

                if (auth == null)
                {
                    AccessToken = gp.GenerateToken(u.Id);
                }
                else
                {
                    //validation of token
                    //if its older then 24 hours then it will be deleted and new token will be assigned
                    byte[] data = Convert.FromBase64String(auth.AccessToken);
                    DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
                    DateTime now = DateTime.UtcNow.AddHours(-24);
                    if (when < now)
                    {
                        bool chk = new AuthenticationBL().DeleteAuthentication(auth.Id);
                        if(chk == true)
                        {
                            AccessToken = gp.GenerateToken(u.Id);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        AccessToken = auth.AccessToken;
                    }
                }

                UserDTO udto = new UserDTO()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    AccessToken = AccessToken
                };

                return Ok(udto);
            }


        }


        [Route("api/LoginAPI/PostLogout")]
        [HttpGet]
        public IHttpActionResult Logout(string Id)
        {
            Authentication auth = new AuthenticationBL().GetAuthenticationByUserId(Convert.ToInt32(Id));
            if(auth != null)
            {
                bool chkAuth = new AuthenticationBL().DeleteAuthentication(auth.Id);

                if(chkAuth)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }





        //public IHttpActionResult GetAllUsers(int id)
        //{
        //    IList<User> users = null;

        //    users = new UserBL().GetActiveUsers();

        //    if (users.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(users);
        //}

        //public IHttpActionResult PostAccessToken(User _user)
        //{
        //    DatabaseEntities db = new DatabaseEntities();
        //    User u = new UserBL().GetActiveUsers().Where(x => x.Email.ToLower() == _user.Email.ToLower() && x.Password == _user.Password).FirstOrDefault();

        //    if (u == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        Authentication auth = new AuthenticationBL().GetAuthenticationByUserId(u.Id);

        //        if (auth == null)
        //        {
        //            //generating Unique Access Token with time span
        //            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
        //            byte[] key = Guid.NewGuid().ToByteArray();
        //            string token = Convert.ToBase64String(time.Concat(key).ToArray());
        //            //generating Access Token with time span

        //            Authentication obj = new Authentication()
        //            {
        //                UserId = u.Id,
        //                AccessToken = token,
        //                IsActive = 1,
        //                CreatedAt = DateTime.Now
        //            };

        //            bool chk = new AuthenticationBL().AddAuthentication(obj, db);
        //            if (chk == true)
        //            {
        //                return Ok(token);
        //            }
        //            else
        //            {
        //                return NotFound();
        //            }
        //        }
        //        else
        //        {
        //            byte[] data = Convert.FromBase64String(auth.AccessToken);
        //            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
        //            if (when < DateTime.UtcNow.AddHours(-24))
        //            {
        //                bool chk2 = new AuthenticationBL().DeleteAuthentication(auth.Id);

        //                //generating Unique Access Token with time span
        //                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
        //                byte[] key = Guid.NewGuid().ToByteArray();
        //                string token = Convert.ToBase64String(time.Concat(key).ToArray());
        //                //generating Access Token with time span

        //                Authentication obj = new Authentication()
        //                {
        //                    UserId = u.Id,
        //                    AccessToken = token,
        //                    IsActive = 1,
        //                    CreatedAt = DateTime.Now
        //                };

        //                bool chk = new AuthenticationBL().AddAuthentication(obj, db);

        //                return Ok(token);
        //            }
        //            else
        //            {
        //                return Ok(auth.AccessToken);
        //            }
        //        }

        //    }
        //}


        //public IHttpActionResult GetLogin(string Email, string Password, string AccessToken)
        //{
        //    User user = new User();
        //    //Authentication auth = new AuthenticationBL().get
        //    user = new UserBL().GetActiveUsers().Where(x => x.Email.ToLower() == Email.ToLower() && x.Password == Password).FirstOrDefault();

        //    //need to add DTO, elsewise it will return the whole modal because of foreign key constraints
        //    //now it will only return defined data
        //    UserDTO obj = new UserDTO()
        //    {
        //        Id = user.Id,
        //        Name = user.Name,
        //        Email = user.Email,
        //        AccessToken = AccessToken
        //    };

        //    return Ok(obj);
        //}

        //public IHttpActionResult Posttest(int id)
        //{
        //    return Ok(id);
        //}

    }
}
