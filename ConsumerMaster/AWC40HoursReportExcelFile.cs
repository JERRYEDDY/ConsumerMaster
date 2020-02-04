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
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateWorkbook(Stream input)
        {
            Workbook workbook = new Workbook();
            WorksheetCollection worksheets = workbook.Worksheets;
            string[] columnsList = { "ID", "Name", "Hours" };
               
            try
            {
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];   //40 Hours per week Report

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
                            where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 40.00
                            orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
                            select new
                            {
                                ID = TD.Key.StaffID,
                                Name = TD.Key.StaffName,
                                Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
                            };


                int currentRow = IndexRowItemStart + 1;
                foreach (var row in query)
                {
                    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "ID")].SetValue(row.ID);
                    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Name")].SetValue(row.Name);

                    CellValueFormat decimalFormat = new CellValueFormat("0.00");
                    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetFormat(decimalFormat);
                    sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetValue(row.Hours);

                    currentRow++;
                }

                for (int i = 0; i < columnsList.Count(); i++)
                {
                    sheet1Worksheet.Columns[i].AutoFitWidth();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }

        //string GetCellData(Worksheet worksheet, int i, int j)
        //{
        //    CellSelection selection = worksheet.Cells[i, j];

        //    ICellValue value = selection.GetValue().Value;
        //    CellValueFormat format = selection.GetFormat().Value;
        //    CellValueFormatResult formatResult = format.GetFormatResult(value);
        //    string result = formatResult.InfosText;
        //    return result;
        //}

        //private void FormatWorksheetColumns(string[] columns, Worksheet worksheet)
        //{
        //    ColumnSelection columnSelection = worksheet.Columns[0, columns.Count()];
        //    columnSelection.AutoFitWidth();

        //    foreach (string column in columns)
        //    {
        //        int columnKey = Array.IndexOf(columns, column);
        //        string columnName = column;

        //        worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
        //        worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
        //    }
        //}
    }
}