using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;

namespace ConsumerMaster
{
    public class AWC29HoursReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateWorkbook(Stream input, string inFilename)
        {
            Workbook workbook = new Workbook();
            WorksheetCollection worksheets = workbook.Worksheets;
            string[] columnsList = { "ID", "Name", "Count", "Hours" };

            try
            {
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];   //29 Hours per week Report
                
                Utility util = new Utility();
                DataTable dTable = util.GetTimeAndDistanceDataTable(input);
                util.WorksheetColumnHeaders(columnsList, sheet1Worksheet);

                var query = from row in dTable.AsEnumerable()
                            group row by new
                            {
                                StaffID = row.Field<string>("Staff ID"),
                                StaffName = row.Field<string>("Staff Name")
                            }
                into TD
                            where TD.Select(v => v.Field<string>("ID")).Distinct().Count() > 1
                            where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 29.00
                            orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
                            select new
                            {
                                ID = TD.Key.StaffID,
                                Name = TD.Key.StaffName,
                                Count = TD.Select(v => v.Field<string>("ID")).Distinct().Count(),
                                Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
                            };


                int currentRow = IndexRowItemStart + 1;
                foreach (var row in query)
                {
                    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "ID")].SetValue(row.ID);
                    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Name")].SetValue(row.Name);
                    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Count")].SetValue(row.Count);

                    CellValueFormat decimalFormat = new CellValueFormat("0.00");
                    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetFormat(decimalFormat);
                    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetValue(row.Hours);

                    currentRow++;
                }

                int l = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }

        public MemoryStream CreateDocument(Stream input, string inFilename)
        {
            Utility util = new Utility();
            DataTable dTable = util.GetTimeAndDistanceDataTable(input);

            var query = from row in dTable.AsEnumerable()
                         group row by new
                         {
                             StaffID = row.Field<string>("Staff ID"),
                             StaffName = row.Field<string>("Staff Name")
                         }
            into TD
                         where TD.Select(v => v.Field<string>("ID")).Distinct().Count() > 1
                         where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 29.00
                         orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
                         select new
                         {
                             ID = TD.Key.StaffID,
                             Name = TD.Key.StaffName,
                             Count = TD.Select(v => v.Field<string>("ID")).Distinct().Count(),
                             Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
                         };


            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("29 hours per week limit – if staff supports more than one client");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", inFilename);
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("{0,-10} {1,-30} {2,-5}  {3}", "StaffID","StaffName","Count","Hours");
                // print result
                foreach (var staff in query)
                {
                    string name = staff.Name.Replace("\t", "");
                    streamWriter.WriteLine("{0,-10} {1,-30} {2,-5}  {3:0.00}", staff.ID, name.Trim(), staff.Count, staff.Hours);
                }

                streamWriter.Flush();
                return ms;
            }
        }
    }
}