using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelReadingAdvanced.Models;
using ExcelReadingAdvanced.BL;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using ExcelReadingAdvanced.DataHub;
using ExcelReadingAdvanced.Helping_Classes;

namespace ExcelReadingAdvanced.Controllers
{
    public class HomeController : Controller
    {
        DatabaseEntities de = new DatabaseEntities();

        public ActionResult Index(string msg="", string color="black")
        {
            List<User> uList = new UserBL().GetActiveUsersList(de);

            ViewBag.UserList = uList;

            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        public string DownloadDummyUserFile(int start, int end)
        {
            string dummyPath = Server.MapPath("~/Content/ErrorLogs/DummyFile.xlsx");
            GeneralPurpose.GenerateDummyFile(dummyPath, start, end);

            //downloading file from server directory
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "Dummy_User_Data" + DateTime.Now.Ticks + ".xlsx");
            Response.WriteFile(dummyPath);
            Response.End();

            return "1";
        }

        public ActionResult DeleteAllUser()
        {
            bool chkUsers = new UserBL().DeleteAllUser(de);

            if (chkUsers)
            {
                return RedirectToAction("Index", "Home", new { msg = "Record Cleared successfully", color = "green" });
            }

            return RedirectToAction("Index", "Home", new { msg = "Somethings' wrong", color = "red" });
        }


        public ActionResult UploadFile(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> PostUploadFileAsync()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();

            DateTime Date = Convert.ToDateTime(Request.Form["Date"]);
            
            List<User> oldUserList = new UserBL().GetActiveUsersList(de).ToList();
            List<string> ResponseMsg = new List<string>();

            HttpFileCollectionBase files = Request.Files;
            int TotalFiles = Request.Files.Count;

            for (int i = 0; i < TotalFiles; i++)
            {
                HttpPostedFileBase PostedFile = files[i];
                string fileExt = Path.GetExtension(PostedFile.FileName);

                if(fileExt.ToLower() == ".csv")
                {
                    await Task.Run(() =>
                    {
                        ResponseMsg.AddRange(ReadCsvFile(PostedFile, oldUserList, i+1, TotalFiles));
                    });
                }
                else if(fileExt.ToLower() == ".xlsx")
                {
                    await Task.Run(() =>
                    {
                        ResponseMsg.AddRange(ReadExcelFile(PostedFile, oldUserList, i+1, TotalFiles));
                    });
                }
                else
                {
                    context.Clients.All.broadcastProcessCompleted("0", "Only .csv/.xlsx Files allowed.");

                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }

            if(ResponseMsg.Count() == 0)
            {
                context.Clients.All.broadcastProcessCompleted("1", "");

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if(ResponseMsg[0] == "exception")
                {
                    context.Clients.All.broadcastProcessCompleted("0", ResponseMsg[1]);

                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                string logPath = Server.MapPath("~/Content/ErrorLogs/Upload_Error_Log.xlsx");
                GeneralPurpose.GenerateErrorLog(ResponseMsg, logPath);

                context.Clients.All.broadcastProcessCompleted("2", "");

                return Json(2, JsonRequestBehavior.AllowGet);
            }
        }


        public List<string> ReadCsvFile(HttpPostedFileBase PostedFile, List<User> oldUserList, int currentFile, int totalFiles)
        {
            List<string> errorMsg = new List<string>();
            try
            {
                string fileName = Path.GetFileName(PostedFile.FileName);

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    HeaderValidated = null,
                    BadDataFound = null
                };

                List<UserDTO> udtoList = new List<UserDTO>();

                using (var streamReader = new StreamReader(PostedFile.InputStream))
                {
                    using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {
                        udtoList = csvReader.GetRecords<UserDTO>().ToList();
                    }
                }
                var context = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();

                int i = 1;
                int totalRow = udtoList.Count();
                int flag = 0;

                foreach (UserDTO udto in udtoList)
                {
                    User u = new User()
                    {
                        Name = udto.Name,
                        Contact = udto.Contact,
                        IsActive = 1,
                        CreatedAt = DateTime.Now
                    };

                    if (String.IsNullOrEmpty(u.Name) == true || String.IsNullOrEmpty(u.Contact) == true)
                    {
                        errorMsg.Add("(" + fileName + ") Empty field at row # " + (i + 1));
                    }
                    else
                    {
                        if (oldUserList.Where(x => x.Contact == u.Contact).Count() == 0)
                        {
                            int UserId = new UserBL().AddUser2(u, de);
                            if (UserId != -1)
                            {
                                u.Id = UserId;
                                oldUserList.Add(u);
                            }
                            else
                            {
                                errorMsg.Add("~(" + fileName + ") Data insertion failed at row # " + (i + 1));
                            }
                        }
                        else
                        {
                            errorMsg.Add("(" + fileName + ") Dulpicate entry at row # " + (i + 1));
                        }
                    }

                    context.Clients.All.broadcastUploadProgress(i, totalRow, currentFile, totalFiles);

                    //int percentage = (i * 100) / totalRow;


                    //if(flag != percentage)
                    //{
                    //    context.Clients.All.broadcastUploadProgress(percentage, currentFile, totalFiles);

                    //    flag = percentage;
                    //}

                    i++;
                }

                return errorMsg;
            }
            catch (Exception ex)
            {
                errorMsg.Clear();

                errorMsg.Add("exception");
                errorMsg.Add(ex.Message);

                return errorMsg;
            }
        }


        public List<string> ReadExcelFile(HttpPostedFileBase PostedFile, List<User> oldUserList, int currentFile, int totalFiles)
        {
            List<string> errorMsg = new List<string>();
            try
            {
                string fileName = Path.GetFileName(PostedFile.FileName);

                DataTable dt = new DataTable();
                using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(PostedFile.InputStream, false))
                {
                    WorkbookPart workbookPart = sDoc.WorkbookPart;
                    IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                    string relationshipId = sheets.First().Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();

                    var context = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();


                    foreach (Cell cell in rows.ElementAt(0)) //reading columns Count
                    {
                        dt.Columns.Add(GetCellValue(sDoc, cell));
                    }

                    int i = 0;
                    int totalRow = rows.Count();

                    foreach (Row row in rows) 
                    {
                        User u = new User();
                        
                        if (i == 0)
                        {
                            i++;
                            continue;
                        }

                        DataRow tempRow = dt.NewRow();

                        for (int j = 0; j < row.Descendants<Cell>().Count(); j++)
                        {
                            tempRow[j] = GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(j));

                            if (j == 0)
                            {
                                u.Name = tempRow.ItemArray[j].ToString();
                            }
                            else
                            {
                                u.Contact = tempRow.ItemArray[j].ToString();
                            }
                        }

                        if (String.IsNullOrEmpty(u.Name) == true || String.IsNullOrEmpty(u.Contact) == true)
                        {
                            errorMsg.Add("(" + fileName + ") Empty field at row # " + (i + 1));
                        }
                        else
                        {
                            if (oldUserList.Where(x => x.Contact == u.Contact).Count() == 0)
                            {
                                u.IsActive = 1;
                                u.CreatedAt = DateTime.Now;

                                int UserId = new UserBL().AddUser2(u, de);
                                if (UserId != -1)
                                {
                                    u.Id = UserId;
                                    oldUserList.Add(u);
                                }
                                else
                                {
                                    errorMsg.Add("~(" + fileName + ") Data insertion failed at row # " + (i + 1));
                                }
                            }
                            else
                            {
                                errorMsg.Add("(" + fileName + ") Dulpicate entry at row # " + (i + 1));
                            }
                        }

                        context.Clients.All.broadcastUploadProgress(i, totalRow, currentFile, totalFiles);
                        
                        i++;
                    }
                }

                return errorMsg;
            }
            catch (Exception ex)
            {
                errorMsg.Clear();

                errorMsg.Add("exception");
                errorMsg.Add(ex.Message);

                return errorMsg;
            }
        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }



        #region Excel reading using Open Xml

        //public List<string> ReadExcelFile(HttpPostedFileBase PostedFile, List<User> oldUserList, int currentFile, int totalFiles)
        //{
        //    List<string> errorMsg = new List<string>();
        //    try
        //    {
        //        string fileName = Path.GetFileName(PostedFile.FileName);
        //        string fileExt = Path.GetExtension(PostedFile.FileName);

        //        string Filename = "UploadedFile"+ fileExt;

        //        string serverPath = Path.Combine(Server.MapPath("../Content/ErrorLogs/"), Filename);

        //        PostedFile.SaveAs(serverPath);
        //        using (FileStream fs = new FileStream(serverPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
        //            {
        //                WorkbookPart workbookPart = doc.WorkbookPart;
        //                SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
        //                SharedStringTable sst = sstpart.SharedStringTable;
        //                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
        //                Worksheet sheet = worksheetPart.Worksheet;
        //                var cells = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Cell>();
        //                var rows = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>();
        //                Console.WriteLine("Row count = {0}", rows.LongCount());
        //                Console.WriteLine("Cell count = {0}", cells.LongCount());
        //                int rcount = rows.Count();
        //                int ccount = cells.Count();

        //                int i = 0;

        //                char cellref = 'A';
        //                int cellcount = 2;

        //                var context = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();

        //                //for (int rc = 0; rc < rcount; rc++)
        //                foreach (Row row in rows)
        //                {
        //                    User u = new User();

        //                    if (i > 0)
        //                    {
        //                        for (int j = 0; j <= 1; j++)
        //                        {
        //                            string str;

        //                            Cell cell = GetCell(sheet, cellref.ToString(), (uint)cellcount);
        //                            if (cell != null)
        //                            {
        //                                if (cell.InnerText == "")
        //                                {
        //                                    str = null;
        //                                }
        //                                else
        //                                {
        //                                    float f;

        //                                    if (cell.DataType != null)
        //                                    {
        //                                        if (cell.DataType == CellValues.SharedString)
        //                                        {
        //                                            int ssid = Convert.ToInt32(cell.CellValue.Text);
        //                                            str = sst.ChildElements[ssid].InnerText;
        //                                        }
        //                                        else
        //                                        {
        //                                            str = cell.CellValue.Text;
        //                                        }
        //                                    }
        //                                    else if (float.TryParse(cell.InnerText, out f))
        //                                    {
        //                                        str = cell.InnerText;
        //                                    }
        //                                    else
        //                                    {
        //                                        str = null;
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                str = null;
        //                            }

        //                            switch (j)
        //                            {
        //                                case 0:
        //                                    u.Name = str;
        //                                    break;
        //                                case 1:
        //                                    u.Contact = str;
        //                                    break;
        //                            }

        //                            cellref++;
        //                        }

        //                        if (u.Name != null && u.Contact != null)
        //                        {
        //                            int count = oldUserList.Where(x => x.Contact == u.Contact).Count();

        //                            if (count == 0)
        //                            {
        //                                u.IsActive = 1;
        //                                u.CreatedAt = DateTime.Now;

        //                                bool chkUser = new UserBL().AddUser(u, de);
        //                                if (chkUser)
        //                                {
        //                                    oldUserList.Add(u);


        //                                }
        //                                else
        //                                {
        //                                    errorMsg.Add("~("+fileName+") Data insertion failed at row # " + (i + 1));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                errorMsg.Add("(" + fileName + ") Dulpicate entry at row # " + (i + 1));

        //                            }
        //                        }
        //                        else
        //                        {
        //                            errorMsg.Add("(" + fileName + ") Empty field at row # " + (i + 1));
        //                        }

        //                        cellref = 'A';
        //                        cellcount++;
        //                    }

        //                    context.Clients.All.broadcastUploadProgress(i, rcount, currentFile, totalFiles);


        //                    //int percentage = (i * 100) / rcount;

        //                    //int flag = 0;

        //                    //if (flag != percentage)
        //                    //{
        //                    //    context.Clients.All.broadcastUploadProgress(percentage, currentFile, totalFiles);

        //                    //    flag = percentage;
        //                    //}

        //                    i++;
        //                }
        //            }
        //        }

        //        return errorMsg;
        //        //if (errorMsg.Count == 0)
        //        //{
        //        //    return "Codes uploaded successfully";
        //        //}
        //        //else
        //        //{
        //        //    string logPath = Server.MapPath("~/Content/ErrorLogs/Commodity_Error_Log.xlsx");
        //        //    return GeneralPurpose.GenerateErrorLog(errorMsg, logPath);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg.Clear();

        //        errorMsg.Add("exception");
        //        errorMsg.Add(ex.Message);

        //        return errorMsg;
        //    }
        //}

        //private static Row GetRow(Worksheet worksheet, uint rowIndex)
        //{
        //    return worksheet.GetFirstChild<SheetData>().
        //      Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        //}

        //private static Cell GetCell(Worksheet worksheet, string columnName, uint rowIndex)
        //{
        //    try
        //    {
        //        Row row = GetRow(worksheet, rowIndex);

        //        if (row == null)
        //            return null;

        //        return row.Elements<Cell>().Where(c => string.Compare
        //               (c.CellReference.Value, columnName +
        //               rowIndex, true) == 0).First();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        #endregion

    }
}