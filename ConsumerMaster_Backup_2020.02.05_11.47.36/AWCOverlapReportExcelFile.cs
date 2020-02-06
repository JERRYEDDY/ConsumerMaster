using System;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.Collections.Generic;

namespace ConsumerMaster
{
    public class AWCOverlapReportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateWorkbook(Stream input, string inFilename)
        {
            Workbook workbook = new Workbook();
            WorksheetCollection worksheets = workbook.Worksheets;
            string[] columnsList = { "ID", "Name", "StaffID", "StaffName", "Start", "Finish", "Duration" };

            try
            {
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];   //Overlap Shifts Report
 
                Utility util = new Utility();
                DataTable dTable = util.GetTimeAndDistanceDataTable(input);
                util.WorksheetColumnHeaders(columnsList, sheet1Worksheet);

                var groupedByClientId = dTable.AsEnumerable().GroupBy(row => row.Field<string>("ID"));
                foreach (var clientGroup in groupedByClientId)
                {
                    var shifts = new List<ClientShifts>();
                    foreach (DataRow row in clientGroup)
                    {
                        shifts.Add(new ClientShifts(row.Field<string>("ID"), row.Field<string>("Name"), row.Field<string>("Staff ID"), row.Field<string>("Staff Name"),
                            row.Field<DateTime>("Start"), row.Field<DateTime>("Finish"), row.Field<int>("duration")));
                    }

                    var overlaps = (from t1 in shifts
                                    from t2 in shifts
                                    where !Equals(t1, t2) // Don’t match the same object.
                                    where t1.Start < t2.Finish && t2.Start < t1.Finish   //check intersections use equal for matching times
                                    orderby t1.Start
                                    select t2).Distinct();

                    int currentRow = IndexRowItemStart + 1;
                    foreach (var row in overlaps)
                    {
                        sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "ID")].SetValue(row.ID);
                        sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Name")].SetValue(row.Name);
                        sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "StaffID")].SetValue(row.StaffID);
                        sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "StaffName")].SetValue(row.StaffName);

                        CellValueFormat dateTimeFormat = new CellValueFormat("MM-dd-yyyy hh:mm:ss tt");
                        sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Start")].SetFormat(dateTimeFormat);
                        sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Start")].SetValue(row.Start);
                        sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Finish")].SetFormat(dateTimeFormat);
                        sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Finish")].SetValue(row.Finish);

                        CellValueFormat decimalFormat = new CellValueFormat("0.00");
                        sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Duration")].SetFormat(decimalFormat);
                        sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Duration")].SetValue(row.Duration);

                        currentRow++;
                    }

                    //if(sheet1Worksheet.Rows.CanInsert(currentRow, 0))
                    //{
                    //    RowSelection selection = sheet1Worksheet.Rows[currentRow, 0];
                    //    selection.Insert();
                    //}

                }
                //sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "ID")].SetValue(" ");
                //sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Name")].SetValue(" ");
                //sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "StaffID")].SetValue(" ");
                //sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "StaffName")].SetValue(" ");
                //sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Start")].SetValue(" ");
                //sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Finish")].SetValue(" ");
                //sheet1Worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Duration")].SetValue(" ");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }
    }
}