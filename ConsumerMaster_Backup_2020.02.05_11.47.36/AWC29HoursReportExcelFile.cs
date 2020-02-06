using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;

namespace ConsumerMaster
{
    public class AWC29HoursReportExcelFile
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
    }
}