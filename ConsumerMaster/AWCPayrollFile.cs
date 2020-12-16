using System;
using System.IO;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Windows.Media;
using System.Collections.Generic;

namespace ConsumerMaster
{
    public class AWCPayrollFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateWorkbook(UploadedFile uploadedWeek1File, UploadedFile uploadedWeek2File)
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];

                Utility util = new Utility();
                Stream inputWeek1 = uploadedWeek1File.InputStream;
                Stream inputWeek2 = uploadedWeek2File.InputStream;

                DataTable week1DataTable = util.GetNetsmartClientServicesDataTablePayrollViaCSV(inputWeek1, "week 1");
                var week1Query = from row in week1DataTable.AsEnumerable()
                            group row by new
                            {
                                StaffName = row.Field<string>("staff_name"),
                                EventName = row.Field<string>("event_name"),
                                ClientName = row.Field<string>("full_name")
                            }
                into TD
                            select new
                            {
                                StaffName = TD.Key.StaffName,
                                EventName = TD.Key.EventName,
                                ClientName = TD.Key.ClientName,
                                TotalHours = TD.Sum(v => v.Field<int>("duration_num") / 60.00),
                                Week = TD.Max(v => v.Field<string>("week"))
                            };

                DataTable week2DataTable = util.GetNetsmartClientServicesDataTablePayrollViaCSV(inputWeek2, "week 2");
                var week2Query = from row in week2DataTable.AsEnumerable()
                                 group row by new
                                 {
                                     StaffName = row.Field<string>("staff_name"),
                                     EventName = row.Field<string>("event_name"),
                                     ClientName = row.Field<string>("full_name")
                                 }
                into TD
                                 select new
                                 {
                                     StaffName = TD.Key.StaffName,
                                     EventName = TD.Key.EventName,
                                     ClientName = TD.Key.ClientName,
                                     TotalHours = TD.Sum(v => v.Field<int>("duration_num") / 60.00),
                                     Week = TD.Max(v => v.Field<string>("week"))
                                 };


                var results = week1Query.ToList().Union(week2Query.ToList())
                    .OrderBy(r => r.StaffName)
                    .ThenBy(r => r.EventName)
                    .ThenBy(r => r.ClientName)
                    .ThenBy(r => r.Week);


                int rowCount = rowCount = Sheet1WorksheetHeader(sheet1Worksheet, uploadedWeek1File.FileName, uploadedWeek2File.FileName);
                int currentRow = IndexRowItemStart + rowCount;

                double grandTotal = 0.00;
                foreach(var row in results)
                { 
                    sheet1Worksheet.Cells[currentRow, 0].SetValue(row.StaffName);
                    sheet1Worksheet.Cells[currentRow, 1].SetValue(row.EventName);
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row.ClientName);

                    CellValueFormat decimalFormat = new CellValueFormat("###0.#####");
                    sheet1Worksheet.Cells[currentRow, 3].SetFormat(decimalFormat);
                    sheet1Worksheet.Cells[currentRow, 3].SetValue(row.TotalHours);

                    sheet1Worksheet.Cells[currentRow, 4].SetValue(row.Week);

                    grandTotal += row.TotalHours;

                    currentRow++;
                }

                sheet1Worksheet.Cells[currentRow, 3].SetValue(grandTotal.ToString());

                for (int i = 1; i < 5; i++)  //Start at 1 instead of 0
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

        private int Sheet1WorksheetHeader(Worksheet worksheet, string week1Filename, string week2Filename)
        {
            int rowCount = 0;
            try
            {
                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 0, 0), Colors.Transparent);
                worksheet.Cells[rowCount, 0].SetIsBold(true);
                worksheet.Cells[rowCount++, 0].SetValue("AWC Payroll File");
                worksheet.Cells[rowCount++, 0].SetValue(String.Format("Week 1 Filename:{0}", week1Filename));
                worksheet.Cells[rowCount++, 0].SetValue(String.Format("Week 2 Filename:{0}", week2Filename));
                rowCount++;

                worksheet.Cells[rowCount, 0].SetValue("Staff Name");
                worksheet.Cells[rowCount, 0].SetIsBold(true);
                worksheet.Cells[rowCount, 1].SetValue("Event Name");
                worksheet.Cells[rowCount, 1].SetIsBold(true);
                worksheet.Cells[rowCount, 2].SetValue("Client Name");
                worksheet.Cells[rowCount, 2].SetIsBold(true);
                worksheet.Cells[rowCount, 3].SetValue("Total Hours");
                worksheet.Cells[rowCount, 3].SetIsBold(true);
                worksheet.Cells[rowCount, 4].SetValue("Week");
                worksheet.Cells[rowCount, 4].SetIsBold(true);
                rowCount++;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return rowCount;
        }
    }
}