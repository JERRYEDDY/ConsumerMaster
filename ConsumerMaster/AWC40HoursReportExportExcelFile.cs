using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using System.Linq;
using System.Collections.Generic;

namespace ConsumerMaster
{
    public class AWC40HoursReportExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateWorkbook()
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                worksheets.Add();
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];   //40 Hours per week Report
                Worksheet sheet2Worksheet = worksheets["Sheet2"];   //29 Hours per week Report
                Worksheet sheet3Worksheet = worksheets["Sheet3"];   //Overlap Shifts Report



                string spreadsheetFilename = "TimeDistance_20200105_20200111.xlsx";
                Utility util = new Utility();

                DataTable inputDataTable = util.GetTimeAndDistanceDataTable(spreadsheetFilename);

                //string spreadsheetFilename = "TimeDistance_20200112_20200118.xlsx";

                //XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                //Workbook InputWorkbook = formatProvider.Import(File.ReadAllBytes(@"C:\NetSmart\Reports Written\" + spreadsheetFilename));

                //var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;
                //DataTable inputDataTable = new DataTable();

                //for (int i = 0; i < spc.Count(); i++)
                //{
                //    CellSelection selection = InputWorksheet.Cells[0, i];
                //    var columnName = "Column" + (i + 1);
                //    inputDataTable.Columns.Add(spc[i].name, spc[i].type);
                //}

                //for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                //{
                //    var values = new object[spc.Count()];
                //    values[0] = GetCellData(InputWorksheet, i, 0); //Staff ID
                //    values[1] = GetCellData(InputWorksheet, i, 1); //Secondary Staff ID
                //    values[2] = GetCellData(InputWorksheet, i, 2); //Activity ID
                //    values[3] = GetCellData(InputWorksheet, i, 3); //Secondary Staff ID
                //    values[4] = GetCellData(InputWorksheet, i, 4); //Activity Type
                //    values[5] = GetCellData(InputWorksheet, i, 5); //ID
                //    values[6] = GetCellData(InputWorksheet, i, 6); //Secondary ID"

                //    string name = GetCellData(InputWorksheet, i, 7);
                //    values[7] = name.Replace("\"", ""); //Name

                //    string combinedStart = GetCellData(InputWorksheet, i, 8) + " " + GetCellData(InputWorksheet, i, 9);
                //    DateTime startDate = Convert.ToDateTime(combinedStart);
                //    values[8] = startDate; //Start

                //    string combinedFinish = GetCellData(InputWorksheet, i, 11) + " " + GetCellData(InputWorksheet, i, 12);
                //    DateTime finishDate = Convert.ToDateTime(combinedFinish);
                //    values[9] = finishDate; //Finish

                //    string durationStr = GetCellData(InputWorksheet, i, 14);
                //    values[10] = int.Parse(durationStr, NumberStyles.AllowThousands);  //Duration

                //    values[11] = GetCellData(InputWorksheet, i, 15); //Travel Time
                //    values[12] = GetCellData(InputWorksheet, i, 16); //TSrc
                //    values[13] = GetCellData(InputWorksheet, i, 17); //Distance
                //    values[14] = GetCellData(InputWorksheet, i, 18); //DSrc
                //    values[15] = GetCellData(InputWorksheet, i, 19); //Phone
                //    values[16] = GetCellData(InputWorksheet, i, 20); //Service
                //    values[17] = GetCellData(InputWorksheet, i, 21); //On-call
                //    values[18] = GetCellData(InputWorksheet, i, 22); //Location
                //    values[19] = GetCellData(InputWorksheet, i, 23); //Discipline"

                //    inputDataTable.Rows.Add(values);
                //}

                string[] columnsList1 = { "ID", "Name", "Hours" };
                string[] columnsList2 = { "ID", "Name", "Count", "Hours" };
                string[] columnsList3 = { "ID", "Name", "StaffID", "StaffName", "Start", "Finish", "Duration" };

                string[][] cList = { columnsList1, columnsList2, columnsList3 };

                int q = 0;

                foreach (Worksheet worksheet in workbook.Worksheets)
                {

                    FormatWorksheetColumns(cList[q++], worksheet);

                }





                //40 hour per week limit – show anyone who worked over 40 hours
                //BuildSheet1Worksheet(inputDataTable, sheet1Worksheet);

                ////29 hours per week limit – if staff supports more than one client
                //BuildSheet2Worksheet(inputDataTable, sheet2Worksheet);

                ////29 hours per week limit – if staff supports more than one client
                //BuildSheet3Worksheet(inputDataTable, sheet1Worksheet);

                int l = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }

        string GetCellData(Worksheet worksheet, int i, int j)
        {
            CellSelection selection = worksheet.Cells[i, j];

            ICellValue value = selection.GetValue().Value;
            CellValueFormat format = selection.GetFormat().Value;
            CellValueFormatResult formatResult = format.GetFormatResult(value);
            string result = formatResult.InfosText;
            return result;
        }

        private void BuildSheet1Worksheet(DataTable dTable, Worksheet worksheet)
        {
            try
            {
                string[] columnsList = { "ID", "Name", "Hours" };

                
                FormatWorksheetColumns(columnsList, worksheet);

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
                    worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "ID")].SetValue(row.ID);
                    worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Name")].SetValue(row.Name);

                    CellValueFormat decimalFormat = new CellValueFormat("0.00");
                    worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetFormat(decimalFormat);
                    worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetValue(row.Hours);

                    currentRow++;
                }

                for (int i = 0; i < columnsList.Count(); i++)
                {
                    worksheet.Columns[i].AutoFitWidth();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void BuildSheet2Worksheet(DataTable dTable, Worksheet worksheet)
        {
            try
            {
                string[] columnsList = { "ID", "Name", "Count", "Hours" };
                
                FormatWorksheetColumns(columnsList, worksheet);

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
                    worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "ID")].SetValue(row.ID);
                    worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Name")].SetValue(row.Name);
                    worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Count")].SetValue(row.Count);

                    CellValueFormat decimalFormat = new CellValueFormat("0.00");
                    worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetFormat(decimalFormat);
                    worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Hours")].SetValue(row.Hours);

                    currentRow++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void BuildSheet3Worksheet(DataTable dTable, Worksheet worksheet)
        {
            try
            {
                string[] columnsList = { "ID", "Name", "StaffID", "StaffName", "Start", "Finish", "Duration"  };
                
                FormatWorksheetColumns(columnsList, worksheet);

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
                        worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "ID")].SetValue(row.ID);
                        worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Name")].SetValue(row.Name);
                        worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "StaffID")].SetValue(row.StaffID);
                        worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "StaffName")].SetValue(row.StaffName);

                        CellValueFormat dateTimeFormat = new CellValueFormat("MM-dd-yyyy hh:mm:ss tt");
                        worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Start")].SetFormat(dateTimeFormat);
                        worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Start")].SetValue(row.Start);
                        worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Finish")].SetFormat(dateTimeFormat);
                        worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Finish")].SetValue(row.Finish);

                        CellValueFormat decimalFormat = new CellValueFormat("0.00");
                        worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Duration")].SetFormat(decimalFormat);
                        worksheet.Cells[currentRow, Array.FindIndex(columnsList, x => x == "Duration")].SetValue(row.Duration);

                        currentRow++;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void FormatWorksheetColumns(string[] columns, Worksheet worksheet)
        {
            ColumnSelection columnSelection = worksheet.Columns[0, columns.Count()];
            columnSelection.AutoFitWidth();

            foreach (string column in columns)
            {
                int columnKey = Array.IndexOf(columns, column);
                string columnName = column;

                worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
            }
        }
    }
}