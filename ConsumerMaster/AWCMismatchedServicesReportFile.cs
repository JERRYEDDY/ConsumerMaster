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
            string[] billingCodeArray = new string[12] 
            {
                "ODP / W1726 / Companion W/B",
                "ODP / W1726:U4 / Companion W/O",
                "ODP/ W7061 / H&C 1:1 Degreed Staff",
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

            string[] payrollCodeArray = new string[16]
            {
                "Companion W/B (W1726)",
                "Companion W/O (W1726:U4)",
                "H&C 1:1 Degreed Staff (W7061)",
                "H&C 1:1 W/B (W7060)", 
                "H&C 1:1 W/O (W7060:U4)",
                "H&C 2:1 Enhanced W/B (W7069)",
                "H&C 2:1 W/B (W7068)",
                "Respite 1:1 Enhanced 15 min W/B (W9863)",
                "Respite 15 min W/B (W9862)",
                "Respite 15 min W/O (W9862:U4)",
                "Respite 24HR W/B (W9798)",
                "Respite 24 HR W/O (W9798:U4)",
                "SE Career Assessment W/B (W7235)",
                "SE Job Coach W/B (W9794)",
                "SE Job Coach W/O (W9794:U4)",
                "SE Job Find W/B (H2023)"
            };

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
                streamWriter.WriteLine("{0,-20} {1,-20} {2,-22} {3,-52} {4,-52} {5,-42}", "ClientName", "StaffName", "Start", "Billing Code", "Payroll Code","Match Status");

                foreach (DataRow row in dTable.Rows)
                {
                    int billingCodeIndex = Array.FindIndex(billingCodeArray, m => m == row["Billing Code"].ToString());
                    int payrollCodeIndex = Array.FindIndex(payrollCodeArray, m => m == row["Payroll Code"].ToString());

                    string matchStatus;
                    if (billingCodeIndex != payrollCodeIndex)
                        matchStatus = "MISMATCHED";
                    else
                        matchStatus = "MATCHED";


                    if (row["Activity Type"].ToString().Contains("UPV"))
                    {
                        streamWriter.WriteLine("{0,-20} {1,-20} {2,-22} {3,-6} {4,-42} {5,-6} {6,-42} {7,-22}", row["Name"].ToString(),
                            row["Staff Name"].ToString(), row["Start"].ToString(), billingCodeIndex, row["Billing Code"].ToString(), 
                            payrollCodeIndex, row["Payroll Code"].ToString(), matchStatus);
                    }
                }

                streamWriter.Flush();
                return ms;
            }
        }
    }
}