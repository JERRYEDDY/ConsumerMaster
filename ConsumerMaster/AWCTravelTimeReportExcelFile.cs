
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
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateWorkbook(UploadedFile uploadedFile, bool shiftFilter)
        {
            Workbook workbook = new Workbook();

            try
            {
                workbook.Sheets.Add(SheetType.Worksheet);
                Worksheet worksheet = workbook.ActiveWorksheet;

                Utility util = new Utility();
                AWCTravelTimeReportFormat reportFormat = new AWCTravelTimeReportFormat();
                Stream input = uploadedFile.InputStream;
                DataTable dTable = util.GetTimeAndDistanceDataTable(input);

                int totalRecords = dTable.Rows.Count;
                PrepareWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;


                var staffGroup = from staffRow in dTable.AsEnumerable()  //Group by StaffID,StaffNamd
                                 group staffRow by new
                                 {
                                     StaffID = staffRow.Field<string>("Staff ID"),
                                     StaffName = staffRow.Field<string>("Staff Name")
                                 };

                DataTable reportResultSet = BuildReportRsultSet();

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
                            foreach (DataRow row in shiftGroup.Rows)
                            {
                                //streamWriter.WriteLine("{0,-10} {1,-22} {2,-10} {3,-22} {4,-22} {5,-22} {6,-10}", row["StaffID"].ToString(), row["StaffName"].ToString(), row["ClientID"].ToString(), row["ClientName"].ToString(), row["Start"].ToString(), row["Finish"].ToString(), row["Duration"].ToString());

                                worksheet.Cells[currentRow, reportFormat.GetIndex("StaffID")].SetValue(row["StaffID"].ToString());
                                worksheet.Cells[currentRow, reportFormat.GetIndex("StaffName")].SetValue(row["StaffName"].ToString());
                                worksheet.Cells[currentRow, reportFormat.GetIndex("ClientID")].SetValue(row["ClientID"].ToString());
                                worksheet.Cells[currentRow, reportFormat.GetIndex("ClientName")].SetValue(row["ClientName"].ToString());
                                worksheet.Cells[currentRow, reportFormat.GetIndex("Start")].SetValue(row["Start"].ToString());
                                worksheet.Cells[currentRow, reportFormat.GetIndex("Finish")].SetValue(row["Finish"].ToString());
                                worksheet.Cells[currentRow, reportFormat.GetIndex("Duration")].SetValue(row["Duration"].ToString());

                                currentRow++;


                                reportResultSet.Rows.Add(row["StaffID"], row["StaffName"], row["ClientID"], row["ClientName"], row["Start"], row["Finish"], row["Duration"]);

                            }

                            //streamWriter.WriteLine(" ");
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
                                    //                                    streamWriter.WriteLine("{0,-10} {1,-22} {2,-10} {3,-22} {4,-22} {5,-22} {6,-10}", finishList[i].StaffID.ToString(), finishList[i].StaffName.ToString(), "TRAVEL ", "TIME ", finishList[i].DateTimeInfo.ToString(), startList[i].DateTimeInfo.ToString(), span.TotalMinutes.ToString());
                                    //streamWriter.WriteLine("{0,-10}, {1,-22}, {2,-10}, {3,-22}, {4,-22}, {5,-22}, {6,-10}", finishList[i].StaffID.ToString(), finishList[i].StaffName.ToString(), "TRAVEL ", "TIME ", finishList[i].DateTimeInfo.ToString(), startList[i].DateTimeInfo.ToString(), span.TotalMinutes.ToString());

                                    worksheet.Cells[currentRow, reportFormat.GetIndex("StaffID")].SetValue(finishList[i].StaffID.ToString());
                                    worksheet.Cells[currentRow, reportFormat.GetIndex("StaffName")].SetValue(finishList[i].StaffName.ToString());
                                    worksheet.Cells[currentRow, reportFormat.GetIndex("ClientID")].SetValue("TRAVEL");
                                    worksheet.Cells[currentRow, reportFormat.GetIndex("ClientName")].SetValue("TIME");
                                    worksheet.Cells[currentRow, reportFormat.GetIndex("Start")].SetValue(finishList[i].DateTimeInfo.ToString());
                                    worksheet.Cells[currentRow, reportFormat.GetIndex("Finish")].SetValue(startList[i].DateTimeInfo.ToString());
                                    worksheet.Cells[currentRow, reportFormat.GetIndex("Duration")].SetValue(span.TotalMinutes.ToString());

                                    currentRow++;

                                    reportResultSet.Rows.Add(finishList[i].StaffID, finishList[i].StaffName, "TRAVEL ", "TIME ", finishList[i].DateTimeInfo, startList[i].DateTimeInfo, span.TotalMinutes);

                                }
                            }

                            //streamWriter.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                        }
                    }
                }


                //int currentRow = IndexRowItemStart + 1;
                //foreach (DataRow dr in dTable.Rows)
                //{
                //    worksheet.Cells[currentRow, reportFormat.GetIndex("StaffID")].SetValue(dr["consumer_internal_number"].ToString());
                //    worksheet.Cells[currentRow, reportFormat.GetIndex("StaffName")].SetValue(dr["trading_partner_string"].ToString());
                //    worksheet.Cells[currentRow, reportFormat.GetIndex("ClientID")].SetValue(dr["consumer_first"].ToString());
                //    worksheet.Cells[currentRow, reportFormat.GetIndex("ClientName")].SetValue(dr["consumer_last"].ToString());
                //    worksheet.Cells[currentRow, reportFormat.GetIndex("Start")].SetValue(dr["date_of_birth"].ToString());
                //    worksheet.Cells[currentRow, reportFormat.GetIndex("Finish")].SetValue(dr["address_line_1"].ToString());
                //    worksheet.Cells[currentRow, reportFormat.GetIndex("Duration")].SetValue(dr["gender"].ToString());

                //    currentRow++;
                //}

                //for (int i = 0; i < dTable.Columns.Count; i++)
                //{
                //    worksheet.Columns[i].AutoFitWidth();
                //}
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }

        public MemoryStream CreateDocument(UploadedFile uploadedFile, bool shiftFilter)
        {
            Utility util = new Utility();
            Stream input = uploadedFile.InputStream;
            DataTable dTable = util.GetTimeAndDistanceDataTable(input);

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("Travel Time – identify any staff that have worked for more than one individual in a day and the time between ending a shift and starting the next shift. ");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("{0,-10} {1,-22} {2,-10} {3,-22} {4,-22} {5,-22} {6,-10}", "StaffID", "StaffName", "ClientID", "ClientName", "Start", "Finish", "Duration");

                var staffGroup = from staffRow in dTable.AsEnumerable()  //Group by StaffID,StaffNamd
                                 group staffRow by new
                                 {
                                     StaffID = staffRow.Field<string>("Staff ID"),
                                     StaffName = staffRow.Field<string>("Staff Name")
                                 };

                DataTable reportResultSet = BuildReportRsultSet();

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
                            foreach (DataRow row in shiftGroup.Rows)
                            {
                                //streamWriter.WriteLine("{0,-10} {1,-22} {2,-10} {3,-22} {4,-22} {5,-22} {6,-10}", row["StaffID"].ToString(), row["StaffName"].ToString(), row["ClientID"].ToString(), row["ClientName"].ToString(), row["Start"].ToString(), row["Finish"].ToString(), row["Duration"].ToString());
                                streamWriter.WriteLine("{0,-10}, {1,-22}, {2,-10}, {3,-22}, {4,-22}, {5,-22}, {6,-10}", row["StaffID"].ToString(), row["StaffName"].ToString(), row["ClientID"].ToString(), row["ClientName"].ToString(), row["Start"].ToString(), row["Finish"].ToString(), row["Duration"].ToString());

                                reportResultSet.Rows.Add(row["StaffID"], row["StaffName"], row["ClientID"], row["ClientName"], row["Start"], row["Finish"], row["Duration"]);

                            }

                            streamWriter.WriteLine(" ");
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
                                    //streamWriter.WriteLine("{0,-10} {1,-22} {2,-10} {3,-22} {4,-22} {5,-22} {6,-10}", finishList[i].StaffID.ToString(), finishList[i].StaffName.ToString(), "TRAVEL ", "TIME ", finishList[i].DateTimeInfo.ToString(), startList[i].DateTimeInfo.ToString(), span.TotalMinutes.ToString());
                                    streamWriter.WriteLine("{0,-10}, {1,-22}, {2,-10}, {3,-22}, {4,-22}, {5,-22}, {6,-10}", finishList[i].StaffID.ToString(), finishList[i].StaffName.ToString(), "TRAVEL ", "TIME ", finishList[i].DateTimeInfo.ToString(), startList[i].DateTimeInfo.ToString(), span.TotalMinutes.ToString());

                                    reportResultSet.Rows.Add(finishList[i].StaffID, finishList[i].StaffName, "TRAVEL ", "TIME ", finishList[i].DateTimeInfo, startList[i].DateTimeInfo, span.TotalMinutes);

                                }
                            }

                            //streamWriter.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                        }
                    }
                }

                streamWriter.Flush();
                return ms;
            }
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

        public DataTable BuildReportRsultSet()
        {
            DataTable reportResultSet = new DataTable();
            reportResultSet.Columns.Add("StaffID", typeof(string));
            reportResultSet.Columns.Add("StaffName", typeof(string));
            reportResultSet.Columns.Add("ClientID", typeof(string));
            reportResultSet.Columns.Add("ClientName", typeof(string));
            reportResultSet.Columns.Add("Start", typeof(DateTime));
            reportResultSet.Columns.Add("Finish", typeof(DateTime));
            reportResultSet.Columns.Add("Duration", typeof(int));

            return reportResultSet;
        }

        public class ShiftItem
        {
            public string StaffID { get; set; }
            public string StaffName { get; set; }
            public DateTime DateTimeInfo { get; set; }
        };



        private void PrepareWorksheet(Worksheet worksheet)
        {
            try
            {
                ConsumerExportFormat cef = new ConsumerExportFormat();
                string[] columnsList = cef.ColumnStrings;

                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column);
                    string columnName = column;

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}