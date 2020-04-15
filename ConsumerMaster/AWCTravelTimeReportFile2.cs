using System;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace ConsumerMaster
{
    public class AWCTravelTimeReportFile2
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public MemoryStream CreateDocument(UploadedFile uploadedFile)
        {
            Utility util = new Utility();
            Stream input = uploadedFile.InputStream;
            DataTable dTable = util.GetTimeAndDistanceDataTable(input);

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("TRAVEL TIME2 – identify any staff that have worked for more than one individual in a day and the time between ending a shift and starting the next shift. ");
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

                DataTable reportResultSet = GetReportRsultSet();

                foreach (var staff in staffGroup)
                {
                    DataTable shiftsDataTable = GetTravelTimeDataTable(); 
                    foreach (DataRow shiftRow in staff)
                    {
                        shiftsDataTable.Rows.Add(shiftRow.Field<string>("Staff ID"), shiftRow.Field<string>("Staff Name"), shiftRow.Field<string>("ID"), shiftRow.Field<string>("Name"), shiftRow.Field<DateTime>("Start"), shiftRow.Field<DateTime>("Finish"), shiftRow.Field<int>("Duration"));
                    }

                    var shiftDateGroup = from shiftDateRow in shiftsDataTable.AsEnumerable() //Group by Start Date
                                         group shiftDateRow by new
                                         {
                                             StartDate = shiftDateRow.Field<DateTime>("Start").Date
                                         };
                    foreach (var shiftDate in shiftDateGroup)
                    {
                        DataTable shiftGroup = GetTravelTimeDataTable();
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

                        if (shiftGroup.Rows.Count > 1 && idCounts.Count > 1)
                        {
                            foreach (DataRow row in shiftGroup.Rows)
                            {
                                streamWriter.WriteLine("{0,-10} {1,-22} {2,-10} {3,-22} {4,-22} {5,-22} {6,-10}", row["StaffID"].ToString(), row["StaffName"].ToString(), row["ClientID"].ToString(), row["ClientName"].ToString(), row["Start"].ToString(), row["Finish"].ToString(), row["Duration"].ToString());
                                reportResultSet.Rows.Add(row["StaffID"], row["StaffName"], row["ClientID"], row["ClientName"], row["Start"], row["Finish"], row["Duration"]);

                            }

                            streamWriter.WriteLine(" ");
                            List<ShiftItem> finishList = new List<ShiftItem>();
                            List<ShiftItem> startList = new List<ShiftItem>();
                            int rowCount = shiftGroup.Rows.Count;

                            for (int i = 0; i < (rowCount - 1); i++)
                            {
                                DataRow fRow = shiftGroup.Rows[i];
                                finishList.Add(new ShiftItem() {StaffID = (string)fRow["StaffID"],
                                                                StaffName = (string)fRow["StaffName"],
                                                                DateTimeInfo = (DateTime)fRow["Finish"]});
                            }

                            for (int i = 1; i < rowCount; i++)
                            {
                                DataRow sRow = shiftGroup.Rows[i]; 
                                startList.Add(new ShiftItem() { DateTimeInfo = (DateTime)sRow["Start"] });
                            }

                            if(finishList.Count == startList.Count)
                            {
                                for (int i = 0; i < finishList.Count; i++)
                                {
                                    TimeSpan span = startList[i].DateTimeInfo - finishList[i].DateTimeInfo;
                                    streamWriter.WriteLine("{0,-10} {1,-22} {2,-10} {3,-22} {4,-22} {5,-22} {6,-10}", finishList[i].StaffID.ToString(), finishList[i].StaffName.ToString(), "TRAVEL ", "TIME ", finishList[i].DateTimeInfo.ToString(), startList[i].DateTimeInfo.ToString(), span.TotalMinutes.ToString());
                                    reportResultSet.Rows.Add(finishList[i].StaffID, finishList[i].StaffName, "TRAVEL ", "TIME ", finishList[i].DateTimeInfo, startList[i].DateTimeInfo, span.TotalMinutes);

                                }
                            }

                            streamWriter.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                        }
                    }
                }

                streamWriter.Flush();
                return ms;
            }
        }
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public DataTable GetTravelTimeDataTable()
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

        public DataTable GetReportRsultSet()
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

    }
}
