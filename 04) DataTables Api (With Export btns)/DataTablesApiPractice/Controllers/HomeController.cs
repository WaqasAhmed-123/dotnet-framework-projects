using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DataTablesApiPractice.BL;
using DataTablesApiPractice.Controllers;
using DataTablesApiPractice.Helping_Classes;
using DataTablesApiPractice.Models;

namespace DataTablesApiPractice.Controllers
{
    public class HomeController : Controller
    {
        

        // GET: Home
        public User validateUser()
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid)
                  .Select(c => c.Value).SingleOrDefault();
            User loggedInUser = new UserBL().GetUserbyId(Convert.ToInt32(userId));

            return loggedInUser;
        }

        
        public ActionResult Index(string msg="")
        {
            if (validateUser() != null)
            {
                ViewBag.Message = msg;
                return View();
            }
            
            return RedirectToAction("Login", "Auth");
        }



        [HttpGet]
        public ActionResult GetDisplayGroupUser()
        {
            List<User> gt = new UserBL().GetActiveUsersList().OrderBy(s => s.Role).ToList();
            
            
            /*int role = logedinuser.Role;
            if (role == (int)Enums.Role.Cordinator || role == (int)Enums.Role.Manager)
            {
                int managerId = logedinuser.Id;
                User manager = new UserBL().getUsersById(managerId);
                gt = gt.Where(x => x.User.DivisionId == manager.DivisionId).OrderBy(s => s.Name).ToList();
            }*/


            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            
            //List<Department> departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
            // List<User> managerlist = new UserBL().getActiveandInvitedUser().Where(x => x.CompanyID == (int)Session["Company"] && (x.Role == 2 || x.Role == 4) && (x.IsActive == 1 || x.IsActive == 2)).OrderBy(s => s.FirstName).ToList();
            
            int totalrows = gt.Count();
            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                gt = gt.Where(x => x.Name.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfilterinig = gt.Count();
            
            //sorting
            //users = users.OrderBy(sortColumnName + " " + sortDirection).ToList();
            
            // pagination
            gt = gt.Skip(start).Take(length).ToList();
            
            List<SessionDTO> mnglist = new List<SessionDTO>();
            int c = 0;
            //string input = "";
            foreach (var x in gt)
            {

                //input = "<input type = 'checkbox' class='fancytree-checkbox deleteCheck' id='check " + x.Id + "'value='" + x.Id + "' name='" + x.Name + "' />";

                SessionDTO obj = new SessionDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Contact = x.Contact,
                    Email = x.Email,
                    Role = Convert.ToInt32(x.Role),
                };
                mnglist.Add(obj);
                c++;
            }
            return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateUser(int UserId=-1, string msg="")
        {
            if (validateUser() != null)
            {
                User getuser = new UserBL().GetUserbyId(UserId);

                ViewBag.ulist = getuser;
                ViewBag.message = msg;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }

        }

        public ActionResult PostUpdateUser(User _user)
        {

            
                User u = new User()
                {
                    Id = _user.Id,
                    Name = _user.Name,
                    Contact = _user.Contact,
                    Email = _user.Email,
                    Role = _user.Role,
                    Password = _user.Password,
                    IsActive = 1,
                    CreatedAt = _user.CreatedAt
                };

                bool check = new UserBL().UpdateUser(u);

                if (check == false)
                {
                    return RedirectToAction("UpdateUser", "Home", new { UserId = _user.Id, msg = "All Fields Required!!" });
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { msg = "Record Updated!!" });
                }
            
        }
        


        public ActionResult DeleteUser(int UserId = -1)
        {
            if (validateUser() != null)
            {
                User getuser = new UserBL().GetUserbyId(UserId);

                User u = new User()
                {
                    Id = getuser.Id,
                    Name = getuser.Name,
                    Contact = getuser.Contact,
                    Email = getuser.Email,
                    Password = getuser.Password,
                    Role = getuser.Role,
                    IsActive = 0,
                    CreatedAt = getuser.CreatedAt
                };

                bool check = new UserBL().UpdateUser(u);

                if (check == true)
                {
                    return RedirectToAction("Index", "Home", new { msg = "Record deleted!!" });
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { msg = "Something Wrong!!" });
                }
            }

            return RedirectToAction("Login", "Auth");

        }

        public ActionResult ContactUs()
        {

            if (validateUser() != null)
            {
                return View();
            }

            return RedirectToAction("Login", "Auth");
        }
    }
}