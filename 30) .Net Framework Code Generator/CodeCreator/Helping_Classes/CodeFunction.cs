using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeCreator.Helping_Classes
{
    public class CodeFunction
    {
        public static char q = '"';

        public static string CreateDatabaseTable(string tableName, string[] columns)
        {
            try
            {
                string DbTable = @"CREATE TABLE [dbo].[" + tableName + "] (<br> &nbsp;&nbsp;&nbsp;&nbsp;[Id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,<br>";

                foreach (string i in columns)
                {
                    DbTable += "&nbsp;&nbsp;&nbsp;&nbsp;[" + i + "] NVARCHAR(255) NULL,<br>";
                }

                DbTable += "&nbsp;&nbsp;&nbsp;&nbsp;[IsActive] INT NULL, <br> &nbsp;&nbsp;&nbsp;&nbsp;[CreatedAt] DATETIME NULL,<br>);";

                return DbTable;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateDataAccessLayer(string tableName)
        {
            try
            {
                string dal = @"public List&lt;" + tableName + "&gt; GetAll" + tableName + "List(DatabaseEntities de)<br>" +
        "{<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;return de." + tableName + "s.ToList();<br>" +
        "}<br>" +

        "<br>public List&lt;" + tableName + "&gt; GetActive" + tableName + "List(DatabaseEntities de)<br>" +
        "{<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;return de." + tableName + "s.Where(x=> x.IsActive == 1).ToList();<br>" +
        "}<br>" +

        "<br>public " + tableName + " Get" + tableName + "ById(int id, DatabaseEntities de)<br>" +
        "{<br>" +
         "&nbsp;&nbsp;&nbsp;&nbsp;return de." + tableName + "s.Where(x=> x.Id == id).FirstOrDefault();<br>" +
        "}<br>" +

        "<br>public " + tableName + " GetActive" + tableName + "ById(int id, DatabaseEntities de)<br>" +
        "{<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;return de." + tableName + "s.Where(x => x.Id == id).FirstOrDefault(x => x.IsActive == 1);<br>" +
        "}<br>" +

        "<br>public bool Add" + tableName + "(" + tableName + " _" + tableName + ", DatabaseEntities de)<br>" +
        "{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;try<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;de." + tableName + "s.Add(_" + tableName + ");<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;de.SaveChanges();<br>" +

                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return true;<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;catch<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
             "&nbsp;&nbsp;&nbsp;&nbsp;return false;<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
        "}<br>" +

        "<br>public int Add" + tableName + "2(" + tableName + " _" + tableName + ", DatabaseEntities de)<br>" +
        "{<br>" +
         "&nbsp;&nbsp;&nbsp;&nbsp;try<br>" +
          "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
           "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;de." + tableName + "s.Add(_" + tableName + ");<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;de.SaveChanges();<br>" +


        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return _" + tableName + ".Id;<br>" +
         "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
          "&nbsp;&nbsp;&nbsp;&nbsp;catch<br>" +
           "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return -1;<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
        "}<br>" +

        "<br>public bool Update" + tableName + "(" + tableName + " _" + tableName + ", DatabaseEntities de)<br>" +
        "{<br>" +
         "&nbsp;&nbsp;&nbsp;&nbsp;try<br>" +
          "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
           "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;de.Entry(_" + tableName + ").State = System.Data.Entity.EntityState.Modified;<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;de.SaveChanges();<br>" +

             "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return true;<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;catch<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
             "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return false;<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
        "}<br>" +

        "<br>public bool Delete" + tableName + "(int id, DatabaseEntities de)<br>" +
        "{<br>" +
         "&nbsp;&nbsp;&nbsp;&nbsp;try<br>" +
          "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
           "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;de." + tableName + "s.Remove(de."+tableName+"s.Where(x => x.Id == id).FirstOrDefault());<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;de.SaveChanges();<br>" +

             "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return true;<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;catch<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return false;<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
        "}<br>";

                return dal;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateBusinessLayer(string tableName)
        {
            try
            {
                string bl = "public List&lt;" + tableName + "&gt; GetAll" + tableName + "List(DatabaseEntities de)<br>" +
        "{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;return new " + tableName + "DAL().GetAll" + tableName + "List(de);<br>" +
        "}<br>" +

        "<br>public List&lt;" + tableName + "&gt; GetActive" + tableName + "List(DatabaseEntities de)<br>" +
        "{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;return new " + tableName + "DAL().GetActive" + tableName + "List(de);<br>" +
        "}<br>" +

        "<br>public " + tableName + " Get" + tableName + "ById(int id, DatabaseEntities de)<br>" +
        "{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;return new " + tableName + "DAL().Get" + tableName + "ById(id, de);<br>" +
        "}<br>" +

        "<br>public " + tableName + " GetActive" + tableName + "ById(int id, DatabaseEntities de)<br>" +
        "{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;return new " + tableName + "DAL().GetActive" + tableName + "ById(id, de);<br>" +
        "}<br>" +

        "<br>public bool Add" + tableName + "(" + tableName + " _" + tableName + ", DatabaseEntities de)<br>" +
        "{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;return new " + tableName + "DAL().Add" + tableName + "(_" + tableName + ", de);<br>" +
        "}<br>" +

        "<br>public int Add" + tableName + "2(" + tableName + " _" + tableName + ", DatabaseEntities de)<br>" +
        "{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;return new " + tableName + "DAL().Add" + tableName + "2(_" + tableName + ", de);<br>" +
        "}<br>" +


        "<br>public bool Update" + tableName + "(" + tableName + " _" + tableName + ", DatabaseEntities de)<br>" +
        "{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;return new " + tableName + "DAL().Update" + tableName + "(_" + tableName + ", de);<br>" +
        "}<br>" +

        "<br>public bool Delete" + tableName + "(int id, DatabaseEntities de)<br>" +
        "{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;return new " + tableName + "DAL().Delete" + tableName + "(id, de);<br>" +
        "}<br>";

                return bl;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateDto(string tableName, string[] columns)
        {
            try
            {
                string tableClassDto = "public class " + tableName + "DTO<br>{<br>";
                tableClassDto += "&nbsp;&nbsp;&nbsp;&nbsp;public int Id { get; set; }<br>";
                tableClassDto += "&nbsp;&nbsp;&nbsp;&nbsp;public string EncId { get; set; }<br>";

                foreach (string i in columns)
                {
                    tableClassDto += "&nbsp;&nbsp;&nbsp;&nbsp;public string " + i + " { get; set; }<br>";
                }
                tableClassDto += "}";

                return tableClassDto;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateAddObject(string tableName, string[] columns)
        {
            try
            {
                string AddObj = tableName + " obj = new " + tableName + "()<br>{<br>";
                foreach (string i in columns)
                {
                    AddObj += "&nbsp;&nbsp;&nbsp;&nbsp;" + i + " = _" + tableName.ToLower() + "." + i + ",<br>";
                }
                AddObj += "&nbsp;&nbsp;&nbsp;&nbsp;IsActive = 1,<br>";
                AddObj += "&nbsp;&nbsp;&nbsp;&nbsp;CreatedAt = GeneralPurpose.DateTimeNow()<br>";
                AddObj += "}<br>";

                return AddObj;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateUpdateObject(string tableName, string[] columns)
        {
            try
            {
                string UpdateObj = tableName + " " + tableName.ToLower() + " = new " + tableName +"BL().Get" + tableName + "ById(id, de);<br>";
                foreach (string i in columns)
                {
                    UpdateObj += tableName.ToLower() + "." + i + " = _" + tableName.ToLower() + "." + i + ";<br>";
                }

                return UpdateObj;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreatePostAddFunction(string tableName, string[] columns)
        {
            try
            {
                string AddObjTemp = "&nbsp;&nbsp;&nbsp;&nbsp;" + tableName + " obj = new " + tableName + "()<br>&nbsp;&nbsp;&nbsp;&nbsp;{<br>";
                foreach (string i in columns)
                {
                    AddObjTemp += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + i + " = _" + tableName.ToLower() + "." + i + ",<br>";
                }
                AddObjTemp += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;IsActive = 1,<br>";
                AddObjTemp += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;CreatedAt = GeneralPurpose.DateTimeNow()<br>";
                AddObjTemp += "&nbsp;&nbsp;&nbsp;&nbsp;};<br>";

                string postAddFunction = "[HttpPost]<br>" +
                "public ActionResult PostAdd" + tableName + "(" + tableName + " _" + tableName.ToLower() + ", string way = " + q + q + ")<br>" +
                "{<br>" +

                AddObjTemp +

                "&nbsp;&nbsp;&nbsp;&nbsp;bool chk" + tableName + " = new " + tableName + "BL().Add" + tableName + "(obj, de);<br>" +

                "&nbsp;&nbsp;&nbsp;&nbsp;if (chk" + tableName + ")<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return RedirectToAction(" + q + "Add" + tableName + "" + q + ", " + q + "Admin" + q + ", new { msg = " + q + "" + tableName + " inserted successfully" + q + ", color = " + q + "green" + q + ", way = way });<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;return RedirectToAction(" + q + "AddEmployee" + q + ", " + q + "Admin" + q + ", new { msg = " + q + "Somethings' wrong" + q + ", color = " + q + "red" + q + ", way = way });<br>" +
            "}";

                return postAddFunction;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreatePostUpdateFunction(string tableName, string[] columns)
        {
            try
            {
                string UpdateObjTemp = "";
                foreach (string i in columns)
                {
                    UpdateObjTemp += "&nbsp;&nbsp;&nbsp;&nbsp;" + tableName.ToLower() + "." + i + " = _" + tableName.ToLower() + "." + i + ";<br>";
                }
                string postUpdateFunction = "[HttpPost]<br>" +
                    "public ActionResult PostUpdate" + tableName + "(" + tableName + " _" + tableName.ToLower() + ", string way = " + q + q + ")<br>" +
                    "{<br>" +

                        "&nbsp;&nbsp;&nbsp;&nbsp;" + tableName + " " + tableName.ToLower() + " = new " + tableName + "BL().GetActive" + tableName + "ById(_" + tableName.ToLower() + ".Id, de);<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;if (" + tableName.ToLower() + " == null)<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return RedirectToAction(" + q + "View" + tableName + "" + q + ", " + q + "Admin" + q + ", new { msg = " + q + "Record not found" + q + ", color = " + q + "red" + q + ", way = way });<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +

                        UpdateObjTemp +


                "<br>&nbsp;&nbsp;&nbsp;&nbsp;bool chk" + tableName + " = new " + tableName + "BL().Update" + tableName + "(" + tableName.ToLower() + ", de);<br>" +

                        "&nbsp;&nbsp;&nbsp;&nbsp;if (chk" + tableName + ")<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return RedirectToAction(" + q + "View" + tableName + "" + q + ", " + q + "Admin" + q + ", new { msg = " + q + "" + tableName + " updated successfully" + q + ", color = " + q + "green" + q + ", way = way });<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;return RedirectToAction(" + q + "View" + tableName + "" + q + ", " + q + "Admin" + q + ", new { msg = " + q + "Somethings' wrong" + q + ", color = " + q + "red" + q + ", way = way });<br>" +
                    "}<br>";

                return postUpdateFunction;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateDeleteFunction(string tableName)
        {
            try
            {
                string deleteFunction = "public ActionResult Delete" + tableName + "(int id, string way = " + q + q + ")<br>" +
        "{<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;" + tableName + " " + tableName.ToLower() + " = new " + tableName + "BL().GetActive" + tableName + "ById(id, de);<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;if (" + tableName.ToLower() + " == null)<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return RedirectToAction(" + q + "View" + tableName + "" + q + ", " + q + "Admin" + q + ", new { msg = " + q + "Record not found" + q + ", color = " + q + "red" + q + ", way = way });<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;" + tableName.ToLower() + ".IsActive = 0;<br>" +

        "&nbsp;&nbsp;&nbsp;&nbsp;bool chk" + tableName + " = new " + tableName + "BL().Update" + tableName + "(" + tableName.ToLower() + ", de);<br>" +

        "&nbsp;&nbsp;&nbsp;&nbsp;if (chk" + tableName + ")<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return RedirectToAction(" + q + "View" + tableName + "" + q + ", " + q + "Admin" + q + ", new { msg = " + q + "Record deleted successfully" + q + ", color = " + q + "green" + q + ", way = way });<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
        "&nbsp;&nbsp;&nbsp;&nbsp;return RedirectToAction(" + q + "View" + tableName + "" + q + ", " + q + "Admin" + q + ", new { msg = " + q + "Somethings' wrong" + q + ", color = " + q + "red" + q + ", way = way });<br>" +
    "}";

                return deleteFunction;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateTableHeader(string tableName, string[] columns)
        {
            try
            {
                string tableHeaderTemp = "";
                foreach (string i in columns)
                {
                    tableHeaderTemp += "&nbsp;&nbsp;&nbsp;&nbsp;&lt;th class=" + q + "text-bold text-center" + q + "&gt;<br>" +
                                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + i + "<br>" +
                                "&nbsp;&nbsp;&nbsp;&nbsp;&lt;/th&gt;<br>";
                }
                string tableHeader = "&lt;tr&gt;<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&lt;th hidden&gt;...&lt;/th&gt;<br>" +
                        tableHeaderTemp +
                    "&nbsp;&nbsp;&nbsp;&nbsp;&lt;th class=" + q + "text -bold text-center" + q + "&gt;<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Action<br>" +
                    "&nbsp;&nbsp;&nbsp;&nbsp;&lt;/th&gt;<br>" +
                    "&lt;/tr&gt;<br>";

                return tableHeader;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateDatatableColumns(string tableName, string[] columns)
        {
            try
            {
                string dataTableCoulmnTemp = "";
                string dataTableCoulmnTemp2 = "";
                int counter = 1;
                foreach (string i in columns)
                {
                    dataTableCoulmnTemp += "&nbsp;&nbsp;&nbsp;&nbsp;{ " + q + "data" + q + ": " + q + i + q + ", " + q + "name" + q + ": " + q + i + q + " },<br>";
                    dataTableCoulmnTemp2 += "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + q + "targets" + q + ": " + counter + ",<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + q + "className" + q + ": ' ',<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + q + "render" + q + ": function (data, type, full, meta) {<br>" +
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return &lt;span&gt;full." + i + "&lt;/span&gt;;<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;},<br>";

                    counter++;
                }
                string dataTableCoulmn = "'columns':<br>" +
                "[<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;{ " + q + "data" + q + ": " + q + "hidden" + q + ", " + q + "name" + q + ": " + q + "0" + q + " },<br>" +
                dataTableCoulmnTemp +
                "&nbsp;&nbsp;&nbsp;&nbsp;{ " + q + "data" + q + ": " + q + "Action" + q + ", " + q + "name" + q + ": " + q + "0" + q + " },<br>" +
                        "],<br>" +


                "'columnDefs':<br>" +
                "[<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + q + "targets" + q + ": 0,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + q + "visible" + q + ": false,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + q + "searchable" + q + ": false,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + q + "render" + q + ": function (data, type, full, meta) {<br>" +
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return null;<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;},<br>" +

                            dataTableCoulmnTemp2 +

                "],<br>";

                return dataTableCoulmn;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateDatatableFunction(string tableName, string[] columns)
        {
            try
            {
                string listName = tableName.ToLower() + "List";
                string dataTableAjaxTemp = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + tableName + "DTO obj = new " + tableName + "DTO()<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br>";
                foreach (string i in columns)
                {
                    dataTableAjaxTemp += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + i + " = " + tableName.ToLower()[0] + "." + i + ",<br>";
                }
                dataTableAjaxTemp += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;};<br>";

                string dataTableAjax = "[HttpPost]<br>" +
                    "public ActionResult Get" + tableName + "DataTableList()<br>" +
                    "{<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;List&lt;" + tableName + "&gt; " + listName + " = new " + tableName + "BL().GetActive" + tableName + "List(de).ToList();<br>" +

                        "&nbsp;&nbsp;&nbsp;&nbsp;int start = Convert.ToInt32(Request[" + q + "start" + q + "]);<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;int length = Convert.ToInt32(Request[" + q + "length" + q + "]);<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;string searchValue = Request[" + q + "search[value]" + q + "];<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;string sortColumnName = Request[" + q + "columns[" + q + " + Request[" + q + "order[0][column]" + q + "] + " + q + "][name]" + q + "];<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;string sortDirection = Request[" + q + "order[0][dir]" + q + "];<br>" +

                        "<br>&nbsp;&nbsp;&nbsp;&nbsp;if (sortColumnName != " + q + "" + q + " && sortColumnName != null)<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;if (sortColumnName != " + q + "0" + q + ")<br>" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;if (sortDirection == " + q + "asc" + q + ")<br>" +
                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + listName + " = " + listName + ".OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();<br>" +
                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;else<br>" +
                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + listName + " = " + listName + ".OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();<br>" +
                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +

                        "&nbsp;&nbsp;&nbsp;&nbsp;int totalrows = " + listName + ".Count();<br>" +


                        "&nbsp;&nbsp;&nbsp;&nbsp;if (!string.IsNullOrEmpty(searchValue))<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + listName + " = " + listName + ".Where(x => x." + columns[0] + " != null && x." + columns[0] + ".ToLower().Contains(searchValue.ToLower()) ||<br>" +
                                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;x." + columns[1] + " != null && x." + columns[1] + ".ToLower().Contains(searchValue.ToLower())<br>" +
                                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;).ToList();<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +

                "&nbsp;&nbsp;&nbsp;&nbsp;int totalrowsafterfilterinig = " + listName + ".Count();<br>" +

                "&nbsp;&nbsp;&nbsp;&nbsp;" + listName + " = " + listName + ".Skip(start).Take(length).ToList();<br>" +

                "&nbsp;&nbsp;&nbsp;&nbsp;List&lt;" + tableName + "DTO&gt; dto = new List&lt;" + tableName + "DTO&gt;();<br>" +

                "&nbsp;&nbsp;&nbsp;&nbsp;foreach (" + tableName + " " + tableName.ToLower()[0] + " in " + listName + ")<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +

                dataTableAjaxTemp +

                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;dto.Add(obj);<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +

                "&nbsp;&nbsp;&nbsp;&nbsp;return Json(new { data = dto, draw = Request[" + q + "draw" + q + "], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);<br>" +
                "}";

                return dataTableAjax;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateEditAjaxFunction(string tableName, string[] columns)
        {
            try
            {
                string editAjaxFunctionTemp = "&nbsp;&nbsp;&nbsp;&nbsp;" + tableName + "DTO obj = new " + tableName + "DTO()<br>&nbsp;&nbsp;&nbsp;&nbsp;{<br>";
                foreach (string i in columns)
                {
                    editAjaxFunctionTemp += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + i + " = " + tableName.ToLower()[0] + "." + i + ",<br>";
                }
                editAjaxFunctionTemp += "&nbsp;&nbsp;&nbsp;&nbsp;};<br>";

                string editAjaxFunction = "[HttpPost]<br>" +
                    "public ActionResult Get" + tableName + "ById(int id)<br>" +
                    "{<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;" + tableName + " " + tableName.ToLower()[0] + " = new " + tableName + "BL().GetActive" + tableName + "ById(id, de);<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;if (" + tableName.ToLower()[0] + " == null)<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;{<br>" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return Json(0, JsonRequestBehavior.AllowGet);<br>" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;}<br>" +

                        editAjaxFunctionTemp +

                        "&nbsp;&nbsp;&nbsp;&nbsp;return Json(obj, JsonRequestBehavior.AllowGet);<br>" +
                    "}";

                return editAjaxFunction;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateInputFields(string tableName, string[] columns)
        {
            try
            {
                string addInputFields = "";
                foreach (string i in columns)
                {
                    addInputFields += "&lt;div class=" + q + "form-group" + q + "&gt;<br>" +
                                    "&nbsp;&nbsp;&nbsp;&nbsp;&lt;label class=" + q + "text-bold" + q + "&gt;" + i + " &lt;span class=" + q + "text-danger" + q + " &gt; *&lt;/span&gt;&lt;/label&gt;<br>" +
                                    "&nbsp;&nbsp;&nbsp;&nbsp;&lt;input type=" + q + "text" + q + " name=" + q + "" + i + "" + q + " class=" + q + "form-control" + q + " placeholder =" + q + "Enter " + i + " Here" + q + " required&gt;<br>" +
                                "&lt;/div&gt;<br><br>";
                }

                return addInputFields;

            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateGetListProcedure(string tableName, string[] columns)
        {
            try
            {
                string getStoreProcedure = "CREATE PROCEDURE [dbo].[sp_Get" + tableName + "List]<br>" +
            "AS<br>" +
            "SELECT * from [dbo].[" + tableName + "]";

                return getStoreProcedure;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateGetByIdProcedure(string tableName, string[] columns)
        {
            try
            {
                string getStoreProcedure = "CREATE PROCEDURE [dbo].[sp_Get" + tableName + "ById]<br>" +
    "@Id INT<br>" +
    "AS<br>" +
    "SELECT * from[dbo].[" + tableName + "] where Id = @Id";

                return getStoreProcedure;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }


        public static string CreateAddUpdateProcedure(string tableName, string[] columns)
        {
            try
            {
                string sp3temp = "";
                string sp3tempa = "";
                string sp3tempb = "";
                string sp3tempc = "";
                string sp3tempd = "";

                foreach (string i in columns)
                {
                    sp3temp += "@" + i + "     NVARCHAR (255)=null,<br>";
                    sp3tempa += i + ",<br>";
                    sp3tempb += "@" + i + ",<br>";
                    sp3tempc += i + " = @" + i + ",<br>";
                    sp3tempd += " _" + tableName + "." + i + ",";
                }
                string getStoreProcedure = "de.sp_AddUpdate" + tableName + "(0,"+ " _" + tableName + ".Id" + sp3tempd + " _" + tableName + ".IsActive" + " _" + tableName + ".CreatedAt); <br><br><br>";
                
                getStoreProcedure += "CREATE PROCEDURE sp_AddUpdate" + tableName + " (<br>" +


                   "@StatementType    INT=0,<br>" +
                   "@Id                 INT=0,<br>" +
                   sp3temp +
                   "@IsActive INT=0,<br>" +
                   "@CreatedAt          DATETIME=null<br>" +
                   ")<br>" +

                    "AS  <br>" +

                      "BEGIN<br>" +

                          "IF @StatementType = 0  <br>" +

                            "BEGIN <br>" +

                                "INSERT INTO [dbo].[" + tableName + "]<br>" +

                                            "(<br>" +
                                            sp3tempa +
                       "IsActive,<br>" +
                       "CreatedAt<br>" +


                                    ") <br>" +

                           "VALUES    (<br>" +
                   sp3tempb +
                   "@IsActive,<br>" +
                   "@CreatedAt<br>" +
                   ")   <br>" +
                    "Select * from [dbo].[" + tableName + "] where Id = SCOPE_IDENTITY()<br>" +
                    "END <br>" +
                      "IF @StatementType = 1 <br>" +

                        "BEGIN<br>" +

                            "UPDATE [dbo].[" + tableName + "]<br>" +

                            "SET  <br>" +


                   sp3tempc +
                   "IsActive = @IsActive,<br>" +
                   "CreatedAt = @CreatedAt<br>" +


                            "WHERE  Id = @Id<br>" +

                            "Select * from [dbo].[" + tableName + "] where Id = @Id<br>" +

                        "END<br>" +

                  "END";

                return getStoreProcedure;
            }
            catch
            {
                return "<b style='color:red'>Something's Wrong</b>";
            }
        }
    }
}