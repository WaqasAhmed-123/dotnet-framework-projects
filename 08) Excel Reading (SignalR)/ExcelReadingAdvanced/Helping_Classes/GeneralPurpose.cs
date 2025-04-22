using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcelReadingAdvanced.Helping_Classes
{
    public class GeneralPurpose
    {

        public static bool GenerateErrorLog(List<string> message, string path)
        {
            try
            {
                SLDocument sl = new SLDocument(); //spreadsheet light library is used to generate this log file

                sl.AddWorksheet("sheet1");

                SLStyle style1 = sl.CreateStyle();
                style1.Font.FontSize = 12;
                style1.Font.Bold = true;

                SLStyle bgColor = sl.CreateStyle();
                bgColor.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Pink, System.Drawing.Color.Pink);


                sl.SetCellValue("A1", "Error List (" + DateTime.Now.ToString("MM/dd/yyyy H:mm") + ")");
                sl.SetCellStyle("A1", style1);
                sl.MergeWorksheetCells(1, 1, 1, 5);
                sl.FreezePanes(1, 5);

                int row = 2;
                foreach (string msg in message)
                {
                    if (msg.Contains("~"))
                    {
                        sl.SetCellValue(row, 1, string.Format(msg.Substring(1)));
                        sl.MergeWorksheetCells(row, 1, row, 5);
                        sl.SetCellStyle(row, 1, row, 5, bgColor);
                    }
                    else
                    {
                        sl.SetCellValue(row, 1, string.Format(msg));
                        sl.MergeWorksheetCells(row, 1, row, 5);
                    }

                    row++;
                }

                sl.SaveAs(path);

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool GenerateDummyFile(string path, int start, int end)
        {
            try
            {
                SLDocument sl = new SLDocument(); //spreadsheet light library is used to generate this log file

                sl.AddWorksheet("sheet1");

                SLStyle style1 = sl.CreateStyle();
                style1.Font.FontSize = 12;
                style1.Font.Bold = true;

                sl.SetCellValue("A1", "Name");
                sl.SetCellStyle("A1", style1);

                sl.SetCellValue("B1", "Contact");
                sl.SetCellStyle("B1", style1);

                int totalRows = end - start;
                for (int row=2; row <= totalRows; row++)
                {
                    sl.SetCellValue(row, 1, "Name " + start);
                    sl.SetCellValue(row, 2, "contact" + start);
                    
                    start++;
                }

                sl.SaveAs(path);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}