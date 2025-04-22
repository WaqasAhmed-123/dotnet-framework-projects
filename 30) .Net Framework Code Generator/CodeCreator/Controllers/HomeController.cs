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
    [ValidationFilter(Role = 1)]
    public class HomeController : Controller
    {
        private GeneralPurpose gp = new GeneralPurpose();
        private DatabaseEntities de = new DatabaseEntities();

        public ActionResult Index(string msg="", string color="black")
        {
            ViewBag.Count = new CodeBL().GetActiveCodesList(de).Count();

            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        public ActionResult AddTable(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [HttpPost]
        public ActionResult PostAddTable(Code code, string way = "")
        {
            code.TableName = code.TableName.Trim();
            code.TableName = code.TableName[0].ToString().ToUpper() + code.TableName.Substring(1);

            code.DbArray = code.DbArray.Trim();

            string[] dbArray = code.DbArray.Split(',');

            string finalData = "";
            foreach (string i in dbArray)
            {
                if (i != "")
                {
                    if (i.ToLower() == "id" || i.ToLower() == "isactive" || i.ToLower() == "createdat")
                    {
                        continue;
                    }
                    else
                    {
                        finalData += i[0].ToString().ToUpper() + i.Substring(1) + ",";
                    }
                }
            }
            finalData = finalData.Substring(0, finalData.Length - 1);
            

            Code obj = new Code()
            {
                TableName = code.TableName,
                DbArray = finalData,
                IsActive = 1,
                CreatedAt = GeneralPurpose.DateTimeNow()
            };

            int codeId = new CodeBL().AddCode2(obj, de);

            if (codeId != -1)
            {
                string BaseUrl = string.Format("{0}://{1}{2}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority, "/");

                return Redirect(BaseUrl + "Home/ViewTableCode?id="+StringCipher.EncryptId(codeId));
            }
            
            return RedirectToAction("AddTable", "Home", new { msg = "Somethings' wrong", color = "red", way = way });
        }

        [ValidationFilter(Role = 0)]
        public ActionResult ViewTable(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [HttpPost]
        public ActionResult PostUpdateTable(Code _code, string way = "")
        {
            Code c = new CodeBL().GetActiveCodeById(_code.Id, de);
            
            if(c == null)
            {
                return RedirectToAction("ViewTable", "Home", new { msg = "Record not found", color = "red", way = way });
            }

            c.TableName = _code.TableName.Trim();
            c.TableName = _code.TableName[0].ToString().ToUpper() + c.TableName.Substring(1);

            string a = _code.DbArray.Trim();

            string[] dbArray = a.Split(',');

            string finalData = "";
            foreach (string i in dbArray)
            {
                if (i != "")
                {
                    if (i.ToLower() == "id" || i.ToLower() == "isactive" || i.ToLower() == "createdat")
                    {
                        continue;
                    }
                    else
                    {
                        finalData += i[0].ToString().ToUpper() + i.Substring(1) + ",";
                    }
                }
            }
            finalData = finalData.Substring(0, finalData.Length - 1);

            c.DbArray = finalData;

            bool chkCode = new CodeBL().UpdateCode(c, de);

            if (chkCode)
            {
                return RedirectToAction("ViewTable", "Home", new { msg = "Table updated successfully", color = "green", way = way });
            }

            return RedirectToAction("ViewTable", "Home", new { msg = "Somethings' wrong", color = "red", way = way });
        }

        [ValidationFilter(Role = 0)]
        [HttpPost]
        public ActionResult DeleteTable(int id, string way = "")
        {
            Code c = new CodeBL().GetActiveCodeById(id, de);

            if (c == null)
            {
                return RedirectToAction("ViewTable", "Home", new { msg = "Record not found", color = "red", way = way });
            }

            c.IsActive = 0;

            bool chkCode = new CodeBL().UpdateCode(c, de);

            if (chkCode)
            {
                return RedirectToAction("ViewTable", "Home", new { msg = "Table deleted successfully", color = "green", way = way });
            }

            return RedirectToAction("ViewTable", "Home", new { msg = "Somethings' wrong", color = "red", way = way });
        }

        [ValidationFilter(Role = 0)]
        public ActionResult ViewTableCode(string id)
        {
            ViewBag.Id = id;

            return View();
        }
    }
}