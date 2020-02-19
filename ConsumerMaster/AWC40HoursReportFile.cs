using System;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;

namespace ConsumerMaster
{
    public class AWC40HoursReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        //private static readonly int IndexRowItemStart = 0;

        //private static readonly double defaultLeftIndent = 50;
        //private static readonly double defaultLineHeight = 18;

        public MemoryStream CreateDocument(UploadedFile uploadedFile)
        {
            Utility util = new Utility();
            Stream input = uploadedFile.InputStream;
            DataTable dTable = util.GetTimeAndDistanceDataTable(input);

            var query = from row in dTable.AsEnumerable()
                        group row by new
                        {
                            StaffID = row.Field<string>("Staff ID"),
                            StaffName = row.Field<string>("Staff Name")
                        }
            into TD
                        where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 40.00
                        orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
                        select new
                        {
                            ID = TD.Key.StaffID,
                            Name = TD.Key.StaffName,
                            Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
                        };


            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("40 hours per week limit – show anyone who worked over 40 hours");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("{0,-10} {1,-30} {2}", "StaffID", "StaffName", "Hours");
                // print result
                foreach (var staff in query)
                {
                    string name = staff.Name.Replace("\t", "");
                    streamWriter.WriteLine("{0,-10} {1,-30} {2:0.00}", staff.ID, name.Trim(), staff.Hours);
                }

                streamWriter.Flush();
                return ms;
            }
        }

        //public Workbook CreateWorkbook(Stream input, string inFilename)
        //{
        //    Workbook workbook = new Workbook();
        //    WorksheetCollection worksheets = workbook.Worksheets;
        //    string[] columnsList = { "ID", "Name", "Hours" };

        //    try
        //    {
        //        worksheets.Add();
        //        Worksheet sheet1Worksheet = worksheets["Sheet1"];   //40 Hours per week Report

        //        Utility util = new Utility();
        //        DataTable dTable = util.GetTimeAndDistanceDataTable(input);
        //        util.WorksheetColumnHeaders(columnsList, sheet1Worksheet);

        //        var query = from row in dTable.AsEnumerable()
        //                    group row by new
        //                    {
        //                        StaffID = row.Field<string>("Staff ID"),
        //                        StaffName = row.Field<string>("Staff Name")
        //                    }
        //        into TD
        //                    where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 40.00
        //                    orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
        //                    select new
        //                    {
        //                        ID = TD.Key.StaffID,
        //                        Name = TD.Key.StaffName,
        //                        Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
        //                    };


        //        int currentRow = IndexRowItemStart + 1;
        //        foreach (var row in query)
        //        {
        //            sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "ID")].SetValue(row.ID);
        //            sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Name")].SetValue(row.Name);

        //            CellValueFormat decimalFormat = new CellValueFormat("0.00");
        //            sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetFormat(decimalFormat);
        //            sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetValue(row.Hours);

        //            currentRow++;
        //        }

        //        string formattedString1 = String.Format("Date/time: {0} ", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
        //        sheet1Worksheet.Cells[currentRow + 1, 0].SetValue(formattedString1);
        //        string formattedString2 = String.Format("Filename: {0}", inFilename);
        //        sheet1Worksheet.Cells[currentRow + 2, 0].SetValue(formattedString2);
        //        //string formattedString = String.Format("Date/time: {0} Filename: {1}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), inFilename);
        //        //sheet1Worksheet.Cells[currentRow + 1, 0].SetValue(formattedString);

        //        for (int i = 0; i < columnsList.Count(); i++)
        //        {
        //            sheet1Worksheet.Columns[i].AutoFitWidth();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //    return workbook;
        //}
    }
}