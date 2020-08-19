
using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.IO;
using Telerik.Web.UI;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace ConsumerMaster
{
    public class AWCTravelTimeReportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateWorkbook(UploadedFile uploadedFile, bool shiftFilter)
        {
            Workbook workbook = new Workbook();

            try
            {
                Utility util = new Utility();
                Stream input = uploadedFile.InputStream;
                DataTable dTable = util.GetTimeAndDistanceDataTable(input);

                int totalRecords = dTable.Rows.Count;

                var staffGroup = from staffRow in dTable.AsEnumerable()  //Group by StaffID,StaffName
                                 group staffRow by new
                                 {
                                     StaffID = staffRow.Field<string>("Staff ID"),
                                     StaffName = staffRow.Field<string>("Staff Name")
                                 };

                DataTable reportResultSet = BuildReportResultSet();
                foreach (var staff in staffGroup)
                {
                    DataTable shiftsDataTable = BuildTravelTimeDataTable();
                    foreach (DataRow shiftRow in staff)
                    {
                        shiftsDataTable.Rows.Add(shiftRow.Field<string>("Staff ID"), shiftRow.Field<string>("Staff Name"), shiftRow.Field<string>("ID"), shiftRow.Field<string>("Name"), shiftRow.Field<DateTime>("Start"), shiftRow.Field<DateTime>("Finish"), shiftRow.Field<int>("Duration"));
                    }

                    var shiftDateGroup = from shiftDateRow in shiftsDataTable.AsEnumerable() //Group by Start Date
                                         orderby shiftDateRow.Field<DateTime>("Start")
                                         group shiftDateRow by new
                                         {
                                             StartDate = shiftDateRow.Field<DateTime>("Start").Date
                                         };
                    foreach (var shiftDate in shiftDateGroup)
                    {
                        DataTable shiftGroup = BuildTravelTimeDataTable();
                        foreach (DataRow sRow in shiftDate)
                        {
                            shiftGroup.Rows.Add(sRow.Field<string>("StaffID"), sRow.Field<string>("StaffName"), sRow.Field<string>("ClientID"), sRow.Field<string>("ClientName"), sRow.Field<DateTime>("Start"), sRow.Field<DateTime>("Finish"), sRow.Field<int>("Duration"));
                        }

                        var idCounts = shiftGroup.AsEnumerable()        //Group by Client for Client Count
                                    .GroupBy(row => row.Field<string>("ClientID"))
                                    .Select(g => new
                                    {
                                        EventID = g.Key,
                                        Count = g.Count()
                                    })
                                    .ToList();

                        List<ShiftItem> finishList = new List<ShiftItem>();
                        List<ShiftItem> startList = new List<ShiftItem>();
                        int rowCount = shiftGroup.Rows.Count;

                        for (int i = 0; i < (rowCount - 1); i++)
                        {
                            DataRow fRow = shiftGroup.Rows[i];
                            finishList.Add(new ShiftItem()
                            {
                                StaffID = (string)fRow["StaffID"],
                                StaffName = (string)fRow["StaffName"],
                                DateTimeInfo = (DateTime)fRow["Finish"]
                            });
                        }

                        for (int i = 1; i < rowCount; i++)
                        {
                            DataRow sRow = shiftGroup.Rows[i];
                            startList.Add(new ShiftItem() { DateTimeInfo = (DateTime)sRow["Start"] });
                        }

                        if (finishList.Count == startList.Count)
                        {
                            for (int i = 0; i < finishList.Count; i++)
                            {
                                TimeSpan span = startList[i].DateTimeInfo - finishList[i].DateTimeInfo;
                                int rounded_minutes = Round((int)span.TotalMinutes);
                                double rounded_hours = rounded_minutes / 60.00;
                                reportResultSet.Rows.Add(finishList[i].StaffID, finishList[i].StaffName, "TRAVEL ", "TIME ", finishList[i].DateTimeInfo, startList[i].DateTimeInfo, span.TotalMinutes, rounded_hours);
                            }
                        }
                    }
                }

                var sspGroup = from sspDateRow in reportResultSet.AsEnumerable() //Group by Start Date
                               orderby sspDateRow.Field<string>("StaffName")
                               group sspDateRow by new
                               {
                                   StaffID = sspDateRow.Field<string>("StaffID")/*,*/
                               };


                string[] columnNames =
                {
                    "StaffID",
                    "StaffName",
                    "Start",
                    "Finish",
                    "Duration",
                    "Rounded"
                };

                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];

                int rowCnt = Sheet1WorksheetHeader(sheet1Worksheet, columnNames);
                int currentRow = IndexRowItemStart + rowCnt;

                foreach (var bySSP in sspGroup)
                {
                    double sspHours = 0;
                    foreach (DataRow row in bySSP)
                    {
                        int duration = Convert.ToInt32(row["Duration"].ToString());
                        if (!Between(duration, 7, 61, false)) 
                            continue;

                        int  column = 0;

                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["StaffID"].ToString());
                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["StaffName"].ToString());

                        CellValueFormat dateCellValueFormat = new CellValueFormat("MM/dd/yyyy hh:mm AM/PM");
                        sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Start"].ToString());

                        sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Finish"].ToString());

                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Duration"].ToString());

                        sspHours += Convert.ToDouble(row["Rounded"].ToString());

                        CellValueFormat decimalFormat = new CellValueFormat("0.00");
                        sheet1Worksheet.Cells[currentRow, column].SetFormat(decimalFormat);
                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Rounded"].ToString());

                        currentRow++;
                    }

                    if (sspHours == 0) //No Subtotal Hours
                        continue;

                    int col = 0;

                    sheet1Worksheet.Cells[currentRow, col++].SetValue(" "); //StaffID
                    sheet1Worksheet.Cells[currentRow, col++].SetValue(" "); //StaffName
                    sheet1Worksheet.Cells[currentRow, col++].SetValue(" "); //Start
                    sheet1Worksheet.Cells[currentRow, col++].SetValue(" "); //Finish
                    sheet1Worksheet.Cells[currentRow, col++].SetValue(" "); //Duration

                    CellValueFormat decFormat = new CellValueFormat("0.00");
                    CellBorders borders = new CellBorders();
                    borders.Top = new CellBorder(CellBorderStyle.Thin, new ThemableColor(Colors.Black));
                    sheet1Worksheet.Cells[currentRow, col].SetBorders(borders);
                    sheet1Worksheet.Cells[currentRow, col].SetFormat(decFormat);
                    sheet1Worksheet.Cells[currentRow, col++].SetValue(sspHours); //Rounded

                    currentRow++;
                }

                for (int i = 1; i < columnNames.Count(); i++)  //Start at 1 instead of 0
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

        int Round(int total_minutes)
        {
            int remainder = total_minutes % 15;
            
            int rounded_minutes = remainder > 7 ? total_minutes + (15 - remainder) : total_minutes - remainder;

            return rounded_minutes;
        }

        public bool Between(int num, int lower, int upper, bool inclusive = false)
        {
            return inclusive ? lower <= num && num <= upper : lower < num && num < upper;
        }

        public bool FilterByShiftCount(int shiftCount, int clientCount, bool shiftFilter)
        {
            if (shiftFilter)
            {
                if (shiftCount > 1)
                    return true;
                else
                    return false;
            }
            else
            {
                if (shiftCount > 1 && clientCount > 1)
                    return true;
                else
                    return false;
            }
        }

        public DataTable BuildTravelTimeDataTable()
        {
            DataTable travelTimeDT = new DataTable();
            travelTimeDT.Columns.Add("StaffID", typeof(string));
            travelTimeDT.Columns.Add("StaffName", typeof(string));
            travelTimeDT.Columns.Add("ClientID", typeof(string));
            travelTimeDT.Columns.Add("ClientName", typeof(string));
            travelTimeDT.Columns.Add("Start", typeof(DateTime));
            travelTimeDT.Columns.Add("Finish", typeof(DateTime));
            travelTimeDT.Columns.Add("Duration", typeof(int));

            return travelTimeDT;
        }

        public DataTable BuildReportResultSet()
        {
            DataTable reportResultSet = new DataTable();
            reportResultSet.Columns.Add("StaffID", typeof(string));
            reportResultSet.Columns.Add("StaffName", typeof(string));
            reportResultSet.Columns.Add("ClientID", typeof(string));
            reportResultSet.Columns.Add("ClientName", typeof(string));
            reportResultSet.Columns.Add("Start", typeof(DateTime));
            reportResultSet.Columns.Add("Finish", typeof(DateTime));
            reportResultSet.Columns.Add("Duration", typeof(int)); 
            reportResultSet.Columns.Add("Rounded", typeof(double));

            return reportResultSet;
        }

        public class ShiftItem
        {
            public string StaffID { get; set; }
            public string StaffName { get; set; }
            public DateTime DateTimeInfo { get; set; }
        };

        private int Sheet1WorksheetHeader(Worksheet worksheet, string[] columnNames)
        {
            int rowCount = 0;
            try
            {
                //PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 0, 0), Colors.Transparent);

                //worksheet.Cells[rowCount, 0].SetIsBold(true);
                //worksheet.Cells[rowCount++, 0].SetValue("AWC Services Exception Report – Payroll/Billing Code Mismatched and NO Billing Authorization");
                //worksheet.Cells[rowCount++, 0].SetValue(String.Format("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));
                //worksheet.Cells[rowCount++, 0].SetValue(String.Format("Filename:{0}; {1}", uploadedTDFileName, uploadedBAFilename));
                //rowCount++;

                foreach (string column in columnNames)
                {
                    int columnKey = Array.IndexOf(columnNames, column);
                    string columnName = column;

                    CellIndex cellIndex = new CellIndex(rowCount, columnKey);
                    CellSelection cellSelection = worksheet.Cells[cellIndex];
                    cellSelection.SetIsBold(true);
                    cellSelection.SetUnderline(UnderlineType.Single);
                    cellSelection.SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    worksheet.Cells[rowCount, columnKey].SetValue(columnName);
                }

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