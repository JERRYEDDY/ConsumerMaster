using System;
using System.IO;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.Collections.Generic;

namespace ConsumerMaster
{
    public class AWCOverlapReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        //private static readonly int IndexRowItemStart = 0;

        public MemoryStream CreateDocument(UploadedFile uploadedFile)
        {
            Utility util = new Utility();
            Stream input = uploadedFile.InputStream;
            DataTable dTable = util.GetTimeAndDistanceDataTable(input);

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("Overlapped shifts – show shifts that overlap for the same client");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                streamWriter.WriteLine(" ");
                //streamWriter.WriteLine("{0,-10} {1,-30} {2,-10} {3,-30} {4,-22} {5,-22} {6,-10}", "ClientID", "ClientName", "StaffID", "StaffName","Start","Finish","Duration");
                streamWriter.WriteLine("{0,-22} {1,-22} {2,-22} {3,-22} {4,-10}", "ClientName", "StaffName", "Start", "Finish", "Duration");

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

                    foreach (var shift in overlaps)
                    {
                        //streamWriter.WriteLine("{0,-10} {1,-30} {2,-10} {3,-30} {4,-22} {5,-22} {6,-10}", shift.ID, shift.Name, shift.StaffID, shift.StaffName, shift.Start, shift.Finish, shift.Duration);
                        streamWriter.WriteLine("{0,-22} {1,-22} {2,-22} {3,-22} {4,-10}", shift.Name, shift.StaffName, shift.Start, shift.Finish, shift.Duration);

                    }
                    if (overlaps.Count() > 1)
                    {
                        streamWriter.WriteLine(" ");
                    }
                }

                streamWriter.Flush();
                return ms;
            }
        }
    }
}