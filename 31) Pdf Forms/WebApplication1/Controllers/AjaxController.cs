using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.BL;
using WebApplication1.Helping_Classes;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AjaxController : Controller
    {
        private readonly GeneralPurpose gp = new GeneralPurpose();
        private readonly DatabaseEntities de = new DatabaseEntities();

        [HttpPost]
        public ActionResult GetUserDataTableList()
        {
            List<User> ulist = new UserBL().GetActiveUsersList(de).ToList();

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count;

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.Contact != null && x.Contact.ToLower().Contains(searchValue.ToLower()) ||
                                    x.Address != null && x.Address.ToLower().Contains(searchValue.ToLower()) ||
                                    x.FirstName != null && x.FirstName.ToLower().Contains(searchValue.ToLower()) ||
                                    x.LastName != null && x.LastName.ToLower().Contains(searchValue.ToLower()) ||
                                    x.Email != null && x.Email.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count;


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            List<UserDTO> udto = new List<UserDTO>();

            foreach (User u in ulist)
            {
                UserDTO obj = new UserDTO()
                {
                    Id = u.Id,
                    Name = u.FirstName + " " + u.LastName,
                    Dob = u.Dob.Value.ToString("MM/dd/yyyy"),
                    Contact = u.Contact,
                    Address = u.Address,
                    Email = u.Email,
                    Gender = u.Gender,
                    Role = Enum.GetName(typeof(EnumRole), u.Role)
                };

                udto.Add(obj);
            }

            return Json(new { data = udto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUserById(int id)
        {
            User u = new UserBL().GetActiveUserById(id, de);
            
            if (u == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            UserDTO obj = new UserDTO()
            {
                Id = u.Id,
                Name = u.FirstName,
                LName = u.LastName,
                Dob = u.Dob.Value.ToString("yyyy-MM-dd"),
                Contact = u.Contact,
                Address = u.Address,
                Email = u.Email,
                Gender = u.Gender,
                Role = u.Role.ToString()
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public ActionResult ValidateEmail(string email, int id = -1)
        {
            return Json(gp.ValidateEmail(email, id), JsonRequestBehavior.AllowGet);
        }
    }
}