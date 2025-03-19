using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeCreator.Models;
using CodeCreator.BL;
using CodeCreator.Helping_Classes;

namespace CodeCreator.Controllers
{
    public class AjaxController : Controller
    {
        public GeneralPurpose gp = new GeneralPurpose();
        public DatabaseEntities de = new DatabaseEntities();

        #region Code Controller

        [HttpPost]
        public ActionResult GetCodeDataTableList()
        {
            List<Code> clist = new CodeBL().GetActiveCodesList(de).OrderByDescending(x => x.CreatedAt).ToList();

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
                        clist = clist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        clist = clist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = clist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                clist = clist.Where(x => x.TableName != null && x.TableName.ToLower().Contains(searchValue.ToLower()) 
                                    ).ToList();
            }

            int totalrowsafterfilterinig = clist.Count();


            // pagination
            clist = clist.Skip(start).Take(length).ToList();

            List<CodeDTO> cdto = new List<CodeDTO>();

            foreach (Code c in clist)
            {
                CodeDTO obj = new CodeDTO()
                {
                    Id = c.Id,
                    EncId = StringCipher.EncryptId(c.Id),
                    TableName = c.TableName,
                    DbArray = c.DbArray
                };

                cdto.Add(obj);
            }

            return Json(new { data = cdto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetCodeById(int id)
        {
            Code c = new CodeBL().GetActiveCodeById(id, de);
            
            if (c == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            Code obj = new Code()
            {
                Id = c.Id,
                TableName = c.TableName,
                DbArray = c.DbArray
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult GetDbArrayByClass(string classCode)
        {
            string finalArray = "";

            string[] Lines = classCode.Split('}');

            for (int i = 0; i <= Lines.Length; i++)
            {
                if (i == 0)
                {
                    try
                    {
                        string temp = Lines[i].Substring(0, Lines[i].Length - 13);
                        string[] memb = temp.Split(' ');

                        finalArray += memb[2] + ",";
                    }
                    catch
                    {
                        continue;
                    }
                }
                else
                {
                    try
                    {
                        string temp2 = Lines[i].Substring(0, Lines[i].Length - 13);
                        string[] memb2 = temp2.Split(' ');

                        finalArray += memb2[2] + ",";
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            if(finalArray.Contains(","))
            {
                finalArray = finalArray.Substring(0, finalArray.Length - 1);
            }

            return Json(finalArray, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public ActionResult ValidateEmail(string email, int id = -1)
        {
            return Json(gp.ValidateEmail(email, id), JsonRequestBehavior.AllowGet);
        }
    }
}