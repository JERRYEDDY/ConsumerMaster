using System;
using System.IO;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.Collections.Generic;

namespace ConsumerMaster
{
    public class AWCMismatchedServicesReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public MemoryStream CreateDocument(UploadedFile uploadedFile)
        {
            string[] aggregatorArray = new string[12] 
            {
                "ODP / W1726 / Companion W/B",
                "ODP / W1726:U4 / Companion W/O",
                "ODP / W7061 / H&C 1:1 Degreed Staff",
                "ODP / W7060 / H&C 1:1 W/B",
                "ODP / W7060:U4 / H&C 1:1 W/O",
                "ODP / W7069 / H&C 2:1 Enhanced W/B",
                "ODP / W7068 / H&C 2:1 W/B",
                "ODP / W9863 / Respite 1:1 Enhanced 15 min W/B",
                "ODP / W9862 / Respite 15 min W/B",
                "ODP / W9862:U4 / Respite 15 min W/O",
                "ODP / W9798 / Respite 24HR W/B",
                "ODP / W9798:U4/ Respite 24 HR W/O"
            };


            AWCServices services = new AWCServices();

            services.BuildAggregatorArray();

            Utility util = new Utility();
            Stream input = uploadedFile.InputStream;
            DataTable dTable = util.GetTimeAndDistanceDataTable(input);

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("Mismatched Services – show unscheduled client visits where Billing Code and Payroll Code do NOT match");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("{0,-22} {1,-22} {2,-22} {3,-52} {4,-52}", "ClientName", "StaffName", "Start", "Billing Code", "Payroll Code");

                foreach (DataRow row in dTable.Rows)
                {
                    string[] billingSubStrings = row["Billing Code"].ToString().Split('/');
                    string[] payrollSubStrings = row["Payroll Code"].ToString().Split('(');

                    if(row["Activity Type"].ToString().Contains("UPV"))
                    {
                        streamWriter.WriteLine("{0,-22} {1,-22} {2,-22} {3,-52} {4,-52}", row["Name"].ToString(), row["Staff Name"].ToString(), row["Start"].ToString(), row["Billing Code"].ToString(), row["Payroll Code"].ToString());
                    }
                }

                streamWriter.Flush();
                return ms;
            }
        }
    }
}