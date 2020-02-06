using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;

namespace ConsumerMaster
{
    public class AWC40HoursReportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public int IndexRowItemStart { get; private set; }

        public Workbook CreateWorkbook(Stream input, string inFilename)
        {
            Workbook workbook = new Workbook();
            WorksheetCollection worksheets = workbook.Worksheets;
            string[] columnsList = { "ID", "Name", "Hours" };

            IndexRowItemStart = 4;
            
            try
            {
   
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];   //40 Hours per week Report

                sheet1Worksheet.Cells[4, 11].SetValue("ID");
                sheet1Worksheet.Cells[4, 12].SetValue("Name");
                sheet1Worksheet.Cells[4, 23].SetValue("Hours");



                Utility util = new Utility();
                DataTable dTable = util.GetTimeAndDistanceDataTable(input);
                //util.WorksheetColumnHeaders(columnsList, sheet1Worksheet);

                //ColumnSelection columnSelection = sheet1Worksheet.Columns[0, columnsList.Count()];
                //columnSelection.AutoFitWidth();

                //foreach (string column in columnsList)
                //{
                //    int columnKey = Array.IndexOf(columnsList, column);
                //    string columnName = column;

                //    sheet1Worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                //    sheet1Worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                //}




                //var query = from row in dTable.AsEnumerable()
                //            group row by new
                //            {
                //                StaffID = row.Field<string>("Staff ID"),
                //                StaffName = row.Field<string>("Staff Name")
                //            }
                //into TD
                //            where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 40.00
                //            orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
                //            select new
                //            {
                //                ID = TD.Key.StaffID,
                //                Name = TD.Key.StaffName,
                //                Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
                //            };


                //int currentRow = IndexRowItemStart + 1;
                //foreach (var row in query)
                //{
                //    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "ID")].SetValue(row.ID);
                //    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Name")].SetValue(row.Name);

                //    CellValueFormat decimalFormat = new CellValueFormat("0.00");
                //    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetFormat(decimalFormat);
                //    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetValue(row.Hours);

                //    currentRow++;
                //}

                //for (int i = 0; i < columnsList.Count(); i++)
                //{
                //    sheet1Worksheet.Columns[i].AutoFitWidth();
                //}
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }
    }
}