
using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.IO;
using Telerik.Web.UI;
using System.Linq;
using System.Collections.Generic;
namespace ConsumerMaster
{
    public class AWCTravelTimeReportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Workbook CreateWorkbook(UploadedFile uploadedFile, bool shiftFilter)
        {
            Workbook workbook = new Workbook();

            try
            {
                Utility util = new Utility();
                Stream input = uploadedFile.InputStream;
                DataTable dTable = util.GetTimeAndDistanceDataTable(input);

                int totalRecords = dTable.Rows.Count;

                var staffGroup = from staffRow in dTable.AsEnumerable()  //Group by StaffID,StaffNamd
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

                        if (FilterByShiftCount(shiftGroup.Rows.Count, idCounts.Count, shiftFilter))
                        {
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
                }


                var sspGroup = from sspDateRow in reportResultSet.AsEnumerable() //Group by Start Date
                               orderby sspDateRow.Field<string>("StaffID")
                               group sspDateRow by new
                               {
                                   StaffID = sspDateRow.Field<string>("StaffID")/*,*/
                                   //StaffName = sspDateRow.Field<string>("StaffName"),
                                   //Start = sspDateRow.Field<DateTime>("Start"),
                                   //Finish = sspDateRow.Field<DateTime>("Finish"),
                                   //Duration = sspDateRow.Field<int>("Duration"),
                                   //Rounded = sspDateRow.Field<double>("Rounded")
                               };

                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];
                int currentRow = 0;

                foreach (var bySSP in sspGroup)
                {
                    double sspHours = 0;
                    foreach (DataRow row in bySSP)
                    {
                        int column = 0;

                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["StaffID"].ToString());
                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["StaffName"].ToString());

                        CellValueFormat dateCellValueFormat = new CellValueFormat("MM/dd/yyyy hh:mm AM/PM");
                        sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Start"].ToString());

                        sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Finish"].ToString());

                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Duration"].ToString());



                        sspHours =  sspHours  + Convert.ToDouble(row["Rounded"].ToString());

                        CellValueFormat decimalFormat = new CellValueFormat("0.00");
                        sheet1Worksheet.Cells[currentRow, column].SetFormat(decimalFormat);
                        sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Rounded"].ToString());

                        currentRow++;
                    }

                    int col = 0;

                    sheet1Worksheet.Cells[currentRow, col++].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, col++].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, col++].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, col++].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, col++].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, col++].SetValue(sspHours);

                    currentRow++;

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
    }
}