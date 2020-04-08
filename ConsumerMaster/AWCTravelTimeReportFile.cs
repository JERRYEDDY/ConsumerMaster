using System;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace ConsumerMaster
{
    public class AWCTravelTimeReportFile
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
                streamWriter.WriteLine("Travel Time – identify any staff that have worked for more than one individual in a day and the time between ending a shift and starting the next shift. ");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("{0,-10} {1,-22} {2,-10} {3,-22} {4,-22} {5,-22} {6,-10}", "StaffID", "StaffName", "ClientID", "ClientName","Start","Finish","Duration");

                var staffGroup = from staffRow in dTable.AsEnumerable()
                        group staffRow by new
                        {
                            StaffID = staffRow.Field<string>("Staff ID"),
                            StaffName = staffRow.Field<string>("Staff Name")
                        };
                foreach (var staff in staffGroup)
                {
                    DataTable shiftsDataTable = new DataTable();
                    shiftsDataTable.Columns.Add("StaffID", typeof(string));
                    shiftsDataTable.Columns.Add("StaffName", typeof(string));
                    shiftsDataTable.Columns.Add("ClientID", typeof(string));
                    shiftsDataTable.Columns.Add("ClientName", typeof(string));
                    shiftsDataTable.Columns.Add("Start", typeof(DateTime));
                    shiftsDataTable.Columns.Add("Finish", typeof(DateTime));
                    shiftsDataTable.Columns.Add("Duration", typeof(int));

                    foreach (DataRow shiftRow in staff)
                    {
                        shiftsDataTable.Rows.Add(shiftRow.Field<string>("Staff ID"), shiftRow.Field<string>("Staff Name"), shiftRow.Field<string>("ID"), shiftRow.Field<string>("Name"), shiftRow.Field<DateTime>("Start"), shiftRow.Field<DateTime>("Finish"), shiftRow.Field<int>("Duration"));
                    }

                    var shiftDateGroup = from shiftDateRow in shiftsDataTable.AsEnumerable()
                                     group shiftDateRow by new
                                     {
                                        StartDate = shiftDateRow.Field<DateTime>("Start").Date
                                     };
                    foreach (var shiftDate in shiftDateGroup)
                    {
                        DataTable shiftGroup = new DataTable();
                        shiftGroup.Columns.Add("StaffID", typeof(string));
                        shiftGroup.Columns.Add("StaffName", typeof(string));
                        shiftGroup.Columns.Add("ClientID", typeof(string));
                        shiftGroup.Columns.Add("ClientName", typeof(string));
                        shiftGroup.Columns.Add("Start", typeof(DateTime));
                        shiftGroup.Columns.Add("Finish", typeof(DateTime));
                        shiftGroup.Columns.Add("Duration", typeof(int));

                        foreach(DataRow sRow in shiftDate)
                        {
                            shiftGroup.Rows.Add(sRow.Field<string>("StaffID"), sRow.Field<string>("StaffName"), sRow.Field<string>("ClientID"), sRow.Field<string>("ClientName"), sRow.Field<DateTime>("Start"), sRow.Field<DateTime>("Finish"), sRow.Field<int>("Duration"));
                            //streamWriter.WriteLine("{0,-10} {1,-22} {2,-10} {3,-22} {4,-22} {5,-22} {6,-10}", sRow.Field<string>("StaffID"), sRow.Field<string>("StaffName"), sRow.Field<string>("ClientID"), sRow.Field<DateTime>("Start"), sRow.Field<DateTime>("Finish"), sRow.Field<int>("Duration"));
                        }

                        var idCounts = shiftGroup.AsEnumerable()
                                    .GroupBy(row => row.Field<string>("ClientID"))
                                    .Select(g => new
                                    {
                                        EventID = g.Key,
                                        Count = g.Count()
                                    })
                                    .ToList();

                        if (shiftGroup.Rows.Count > 1 && idCounts.Count > 1)
                        {


                            streamWriter.WriteLine("++++++++++++++++++++++++++++++++++++++Count: {0,10}+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++", shiftGroup.Rows.Count);
                            streamWriter.WriteLine("DISTINCT COUNT: {0,-10}", idCounts.Count);
                            foreach (DataRow row in shiftGroup.Rows)
                            {
                                //    string name = shift.ClientName.Replace("\t", "");
                                streamWriter.WriteLine("{0,-10} {1,-22} {2,-10} {3,-22} {4,-22} {5,-22} {6,-10}", row["StaffID"].ToString(), row["StaffName"].ToString(), row["ClientID"].ToString(), row["ClientName"].ToString(), row["Start"].ToString(), row["Finish"].ToString(), row["Duration"].ToString());
                            }
                            streamWriter.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                        }
                    }

                    //streamWriter.WriteLine("------------------------------------------------------------------------------------------");
                }

                streamWriter.Flush();
                return ms;
            }
        }
    }
}
