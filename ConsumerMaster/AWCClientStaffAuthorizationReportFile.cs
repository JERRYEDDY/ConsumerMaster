using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;

namespace ConsumerMaster
{
    public class AWCClientStaffAuthorizationReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        private static readonly double defaultLeftIndent = 50;
        private static readonly double defaultLineHeight = 18;
        
        public MemoryStream CreateDocument(Stream input, string inFilename)
        {
            Utility util = new Utility();
            DataTable dTable = util.GetClientAuthorizationDataTable(input);

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("Client Staff Authorization Report");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", inFilename);
                streamWriter.WriteLine(" ");

                var groupedByClientId = dTable.AsEnumerable().GroupBy(row => row.Field<string>("id_no"));
                foreach (var clientGroup in groupedByClientId)
                {
                    int first = 1;
                    foreach (DataRow row in clientGroup)
                    {
                        if (first == 1)
                        {
                            streamWriter.WriteLine("-------------------------------------------------------------------------------------------------------------");
                            streamWriter.WriteLine("Client ID:{0,-10}", row.Field<string>("id_no"));
                            streamWriter.WriteLine("Client Name:{0,-30}", row.Field<string>("full_name"));
                            streamWriter.WriteLine(" ");
                            streamWriter.WriteLine("Billing Authorizations");
                            streamWriter.WriteLine("{0,-15} {1,-15} {2,-35} {3,-12} {4,-12} {5,-12}", "From", "To", "Service", "Total", "Used ", "Balance");
                        }

                        streamWriter.WriteLine("{0,-15} {1,-15} {2,-35} {3,-12} {4,-12} {5,-12}", row.Field<string>("date_from_details"), 
                            row.Field<string>("date_to_details"), row.Field<string>("service_name"), row.Field<string>("units_aut_detail"), row.Field<string>("units_used_detail"), row.Field<string>("detail_balance"));

                        first++;
                    }
                    streamWriter.WriteLine(" ");



                    //var overlaps = (from t1 in shifts
                    //                from t2 in shifts
                    //                where !Equals(t1, t2) // Don’t match the same object.
                    //                where t1.Start < t2.Finish && t2.Start < t1.Finish   //check intersections use equal for matching times
                    //                orderby t1.Start
                    //                select t2).Distinct();

                    //foreach (var shift in overlaps)
                    //{
                    //    streamWriter.WriteLine("{0,-10} {1,-30} {2,-10} {3,-30} {4,-22} {5,-22} {6,-10}", shift.ID, shift.Name, shift.StaffID, shift.StaffName, shift.Start, shift.Finish, shift.Duration);
                    //}
                    //if (overlaps.Count() > 1)
                    //{
                    //    streamWriter.WriteLine(" ");
                    //}

                }


                streamWriter.Flush();
                return ms;
            }
        }
    }
}