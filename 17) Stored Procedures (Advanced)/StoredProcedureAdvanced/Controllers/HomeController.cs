using StoredProcedureAdvanced.BL;
using StoredProcedureAdvanced.Helping_Classes;
using StoredProcedureAdvanced.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoredProcedureAdvanced.Controllers
{
    public class HomeController : Controller
    {
        DatabaseEntities de = new DatabaseEntities();

        public ActionResult GetUser(string msg="", string color="black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [HttpPost]
        public ActionResult AddUsers(int count)
        {
            try
            {
                int c = 1;
                for (int i = 1; i <= count; i++)
                {
                    User obj = new User()
                    {
                        Name = "name" + i,
                        Email = "email" + i,
                        Password = "123",
                        IsActive = 1,
                        CreatedAt = DateTime.Now
                    };

                    new UserBL().AddUser(obj, de);
                    c++;
                }

                de.SaveChanges();

                return RedirectToAction("GetUser", "Home", new { msg = c + " Users inserted successfully", color = "green" });
            }
            catch
            {
                return RedirectToAction("GetUser", "Home", new { msg = "Somethings' Wrong", color = "red" });
            }
        }

        [HttpPost]
        public ActionResult GetUserDatatable()
        {
            
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            string likeOperator = "%";

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = likeOperator + searchValue + likeOperator;
            }
            else
            {
                searchValue = likeOperator + likeOperator;
            }

            int totalrows = new UserBL().GetUserCount(searchValue);

            List<User> ulist = new UserBL().GetUserList(searchValue, start, length).ToList();

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

            int totalrowsafterfilterinig = totalrows;

            List<UserDTO> udto = new List<UserDTO>();

            //int count = start + 1;

            foreach (User x in ulist)
            {
                UserDTO u = new UserDTO()
                {
                    //SerialNo = count,
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Password = x.Password
                };

                udto.Add(u);

                //count++;
            }

            return Json(new { data = udto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }



    }
}