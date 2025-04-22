using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActionFilterPractice.Helping_Classes;
using ActionFilterPractice.Models;
using ActionFilterPractice.BL;

namespace ActionFilterPractice.Controllers
{
    [ValidationFilter(Role = 1)]
    public class AdminController : Controller
    {
        private readonly GeneralPurpose gp = new GeneralPurpose();

        [ValidationFilter(sss = new List<int> { 1,2,4})]
        public ActionResult Index(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }



        public ActionResult AddUser(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }

        public ActionResult PostAddUser(User _user)
        {
            bool checkEmail = gp.ValidateEmail(_user.Email);
            if (checkEmail == false)
            {
                return RedirectToAction("AddUser", "Admin", new { msg = "Email already exist. Try another!", color = "red" });
            }

            User obj = new User()
            {
                Name = _user.Name.Trim(),
                Contact = _user.Contact.Trim(),
                Email = _user.Email.Trim(),
                Password = StringCipher.Encrypt(_user.Password.Trim(), "test"),
                Role = 2,
                IsActive = 1,
                CreatedAt = DateTime.Now
            };

            bool check = new UserBL().AddUser(obj);

            if (check == true)
            {
                return RedirectToAction("AddUser", "Admin", new { msg = "User inserted successfully", color = "green" });
            }
            else
            {
                return RedirectToAction("AddUser", "Admin", new { msg = "Somethings' Wrong!", color = "red" });
            }
        }



        public ActionResult ShowUser(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }



        #region AJAX

        [HttpPost]
        public ActionResult UserList()
        {
            List<User> ulist = new UserBL().GetActiveUserList().Where(x=>x.Role == 2).ToList();


            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];


            int totalrows = ulist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()) ||
                                x.Email.ToLower().Contains(searchValue.ToLower()) ||
                                x.Contact.ToLower().Contains(searchValue.ToLower()) ).ToList();
            }
             
            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            List<User> udto = new List<User>();

            
            foreach (User u in ulist)
            {
                
                User obj = new User()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Contact = u.Contact,
                    Email = u.Email
                };

                udto.Add(obj);

            }

            return Json(new { data = udto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUserById(int _Id = -1)
        {
            User u = new UserBL().GetUserById(_Id);

            User obj = new User()
            {
                Id = u.Id,
                Name = u.Name,
                Contact = u.Contact,
                Email = u.Email
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [ValidationFilter]
        [HttpPost]
        public ActionResult ValidateEmail(string Email, int Id = -1)
        {
            bool checkEmail;
            if (Id != -1)
            {
                checkEmail = gp.ValidateEmail(Email, Id);
            }
            else
            {
                checkEmail = gp.ValidateEmail(Email);
            }

            return Json(checkEmail, JsonRequestBehavior.AllowGet);
        }
        #endregion



        public ActionResult PostUpdateUser(User _user)
        {
            bool checkEmail = gp.ValidateEmail(_user.Email, _user.Id);
            if (checkEmail == false)
            {
                return RedirectToAction("ShowUser", "Admin", new { msg = "Email already exist. Try another!", color = "red" });
            }

            DatabaseEntities de = new DatabaseEntities();
            User u = new UserBL().GetUserById(_user.Id, de);

            u.Name = _user.Name.Trim();
            u.Contact = _user.Contact.Trim();
            u.Email = _user.Email.Trim();

            bool check = new UserBL().UpdateUser(u, de);

            if (check == true)
            {
                return RedirectToAction("ShowUser", "Admin", new { msg = "User updated successfully", color = "green" });
            }
            else
            {
                return RedirectToAction("ShowUser", "Admin", new { msg = "Somethings' Wrong!", color = "red" });
            }
        }

        public ActionResult DeleteUser(int _Id)
        {
            DatabaseEntities de = new DatabaseEntities();
            User u = new UserBL().GetUserById(_Id, de);

            u.IsActive = 0;

            bool check = new UserBL().UpdateUser(u, de);

            if (check == true)
            {
                return RedirectToAction("ShowUser", "Admin", new { msg = "Record deleted successfully", color = "green" });
            }
            else
            {
                return RedirectToAction("ShowUser", "Admin", new { msg = "Somethings' Wrong!", color = "red" });
            }
        }
    }
}