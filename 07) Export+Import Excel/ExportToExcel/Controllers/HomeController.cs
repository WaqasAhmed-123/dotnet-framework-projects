using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using SpreadsheetLight.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExportToExcel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string GenerateExcel()
        {
            // SpreadsheetLight works on the idea of a currently selected worksheet.
            // So there's always a worksheet selected, just like you always only
            // have one worksheet active when in Excel.
            SLDocument sl = new SLDocument();

            SLStyle style1 = sl.CreateStyle();
            style1.Font.FontName = "TimesNewRoman";
            style1.Font.FontSize = 14;
            style1.Font.FontColor = System.Drawing.Color.Blue;
            style1.Font.Bold = true;
            style1.Font.Italic = true;
            //style1.Font.Strike = true;
            style1.Font.Underline = UnderlineValues.Single;
            //setting background colors
            //1st color is for foreground, 2nd is for background
            style1.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.LightGray, System.Drawing.Color.White);


            



            for (int k=1; k<= 3; k++)
            {
                
                sl.AddWorksheet("sheet" + k);

                sl.SetCellValue("A1", "Column 1");
                sl.SetCellValue("B1", "Column 2");
                sl.SetCellValue("C1", "Column 3");
                sl.SetCellValue("D1", "Column 4");

                for (int i = 2; i < 6; ++i)
                {
                    for (int j = 1; j < 5; ++j)
                    {
                        sl.SetCellValue(i, j, string.Format("R{0}C{1}", i+" " + k, j + " " + k));
                    }
                }

                sl.SetCellStyle(1,1,1,4, style1);
                sl.FreezePanes(1,0);
                sl.SetRowHeight(1, 150);

                SLPicture pic = new SLPicture(Server.MapPath("~/Content/Excels/img.png"));
                // 1st form top, 2nd from left
                pic.SetPosition(0, 6);
                sl.InsertPicture(pic);
            }

            

            sl.SaveAs(Server.MapPath("~/Content/Excels/DummyReport.xlsx"));

            return "0";
        }


        public string GenerateZymahHeader()
        {
            
            SLDocument sl = new SLDocument();

            #region Styling

            SLStyle bold = sl.CreateStyle();
            bold.Font.FontName = "Arial";
            bold.Font.FontSize = 10;
            bold.Font.Bold = true;

            SLStyle largeBold = sl.CreateStyle();
            largeBold.Font.FontName = "Arial";
            largeBold.Font.FontSize = 11;
            largeBold.Font.Bold = true;

            SLStyle largerBold = sl.CreateStyle();
            largerBold.Font.FontName = "Arial";
            largerBold.Font.FontSize = 12;
            largerBold.Font.Bold = true;

            SLStyle aligncenter = sl.CreateStyle();
            aligncenter.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            aligncenter.SetVerticalAlignment(VerticalAlignmentValues.Center);

            SLStyle wrapText = sl.CreateStyle();
            wrapText.SetWrapText(true);

            SLStyle bgColor = sl.CreateStyle();
            bgColor.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.LightGray, System.Drawing.Color.White);
            
            #endregion

            for (int k = 1; k <= 12; k++)
            {
                string Month = k + "-" + "2021";

                sl.AddWorksheet(Convert.ToDateTime(Month).ToString("MMMM"));

                #region Report Header
                sl.SetCellValue(1,1, "Month:");
                sl.SetRowHeight(1, 20);
                sl.SetCellValue(1,2, Convert.ToDateTime(Month).ToString("MMMM"));
                sl.SetCellStyle(1,2, bold);
                sl.MergeWorksheetCells("B1", "E1");

                sl.SetCellValue(2, 1, "Year:");
                sl.SetRowHeight(2, 20);
                sl.SetCellValue(2, 2, "2021");
                sl.SetCellStyle(2, 2, bold);
                sl.SetCellStyle(2, 2, aligncenter);
                sl.MergeWorksheetCells("B2", "E2");

                sl.SetCellValue(3, 1, "Monthly Food Program Expenses:");
                sl.SetCellStyle(3, 1, wrapText);
                sl.MergeWorksheetCells("A3", "C3");
                sl.SetRowHeight(3, 35);
                sl.SetCellValue(3, 4, " ");
                sl.MergeWorksheetCells("D3", "E3");

                sl.SetCellValue(4, 1, "Amount Needed from Other Funding Source:");
                sl.SetCellStyle(4, 1, wrapText);
                sl.MergeWorksheetCells("A4", "C4");
                sl.SetRowHeight(4, 35);
                sl.SetCellValue(4, 4, " -\n");
                sl.SetCellStyle(4, 4, bgColor);
                sl.SetCellStyle(4, 4, aligncenter);
                sl.MergeWorksheetCells("D4", "E4");

                sl.SetCellValue(5, 1, " ");
                sl.SetRowHeight(5, 25);

                sl.SetCellValue(6, 1, "Journal Ledger - Food Program Expenses");
                sl.SetCellStyle(6, 1, aligncenter);
                sl.SetCellStyle(6, 1, largerBold);
                sl.MergeWorksheetCells("A6", "N6");
                sl.SetRowHeight(6, 32);

                sl.SetCellValue(7, 7, "Labour & Benefits");
                sl.MergeWorksheetCells("G7", "H7");
                sl.SetCellStyle(7, 7, aligncenter);
                sl.SetCellStyle(7, 7, largeBold);


                sl.SetCellValue(7, 9, "Others");
                sl.MergeWorksheetCells("I7", "N7");
                sl.SetCellStyle(7, 9, aligncenter);
                sl.SetCellStyle(7, 9, largeBold);

                sl.SetCellValue(8, 1, "Date");
                sl.SetCellValue(8, 2, "Check No.");
                sl.SetCellValue(8, 3, "Name (Payee on Check)");
                sl.SetCellValue(8, 4, "Total Amount of Check");
                sl.SetCellValue(8, 5, "Food");
                sl.SetCellValue(8, 6, "NonFood/ Kitchen Supplies");
                sl.SetCellValue(8, 7, "Food Service (Kitchen)");
                sl.SetCellValue(8, 8, "Admin.");
                sl.SetCellValue(8, 9, "Contracted Services (Food Vendor)");
                sl.SetCellValue(8, 10, "Kitchen Equipment");
                sl.SetCellValue(8, 11, "Travel (Admin.)");
                sl.SetCellValue(8, 12, "Travel (Oper.)");
                sl.SetCellValue(8, 13, "Misc.");
                sl.SetCellValue(8, 14, "Specify Misc.");
                sl.SetCellStyle(8, 1, 8, 14, bold);
                sl.SetCellStyle(8, 1, 8, 14, aligncenter);
                sl.SetCellStyle(8, 1, 8, 14, wrapText);

                SLPicture pic = new SLPicture(Server.MapPath("~/Content/Excels/img.png"));
                // 1st value form top, 2nd value from left
                pic.SetPosition(2, 9);
                pic.LockWithSheet = true;
                sl.InsertPicture(pic);

                sl.FreezePanes(8, 0);

                #endregion


                sl.SetCellValue(15, 1, "Monthly Expences:");
                sl.MergeWorksheetCells("A15", "C15");
                sl.SetCellStyle(15, 1, 15, 14, bgColor);
                sl.SetCellStyle(15, 1, 15, 14, bold);

            }

            //deleting default generated file which is empty and useless
            sl.DeleteWorksheet("Sheet1");

            //saving file to server directory
            string filePath = Server.MapPath("~/Content/Excels/ZymahReport" + DateTime.Now.Ticks + ".xlsx");
            sl.SaveAs(filePath);

           

            //downloading file from server directory
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "Zymah_Report.xlsx");
            Response.WriteFile(filePath);
            Response.End();


            return "0";
        }

        public string ImportExcel()
        {
            string file = Server.MapPath("~/Content/Excels/ImportSample.xlsx");


            var sheetNames = new SLDocument(file);
            foreach (var name in sheetNames.GetWorksheetNames())
            {
                // do something with each worksheet name
            }

            using (SLDocument sl = new SLDocument())
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                SLDocument sheet = new SLDocument(fs, "Sheet1");

                SLWorksheetStatistics stats = sheet.GetWorksheetStatistics();
                for (int i = 2; i <= stats.NumberOfRows; i++)
                {
                    for (int j = 1; j <= stats.NumberOfColumns; j++)
                    {
                        // Get the first column of the row (SLS is a 1-based index)
                        var value = sheet.GetCellValueAsString(i, j);
                    }
                }
            }

            return "0";
        }
    }
}