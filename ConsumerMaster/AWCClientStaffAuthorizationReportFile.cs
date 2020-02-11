﻿using System;
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
        
        public MemoryStream CreateDocument(UploadedFile uploadedFile)
        {
            Utility util = new Utility();
            Stream input = uploadedFile.InputStream;


            DataTable staffTable = util.GetClientStaffDataTable(input);

            DataTable authorizationTable = util.GetClientAuthorizationsDataTable(input);

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("Client Staff Authorization Report");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                streamWriter.WriteLine(" ");

                var groupedByClientId = staffTable.AsEnumerable().GroupBy(row => row.Field<string>("Client ID"));
                foreach (var clientGroup in groupedByClientId)
                {
                    int first = 1;
                    foreach (DataRow row in clientGroup)
                    {
                        if (first == 1)
                        {
                            streamWriter.WriteLine("-------------------------------------------------------------------------------------------------------------");
                            streamWriter.WriteLine("Client ID:{0,-10}", row.Field<string>("Client ID"));
                            streamWriter.WriteLine("Client Name:{0,-30}", row.Field<string>("Client Name"));
                            streamWriter.WriteLine(" ");
                            streamWriter.WriteLine("Billing Authorizations");
                            streamWriter.WriteLine("{0,-15} {1,-15} {2,-50} {3,-12} {4,-12} {5,-12}", "From", "To", "Service", "Total", "Used ", "Balance");
                        }

                        streamWriter.WriteLine("{0,-15} {1,-15} {2,-50} {3,-12} {4,-12} {5,-12}", row.Field<string>("From Date"), 
                            row.Field<string>("To Date"), row.Field<string>("Service"), row.Field<string>("Total Units"), row.Field<string>("Units Used"), row.Field<string>("Balance"));

                        first++;
                    }
                    streamWriter.WriteLine(" ");
                }

                streamWriter.Flush();
                return ms;
            }
        }

        public MemoryStream CreateStaffAuthorizationDocument(UploadedFile staffFile, UploadedFile authorizationFile)
        {
            SPColumn[] spc = new SPColumn[5]
            {
                new SPColumn("Client ID", typeof(string)),
                new SPColumn("Client Name", typeof(string)),
                new SPColumn("Record Type", typeof(int)),
                new SPColumn("Record Order", typeof(int)),
                new SPColumn("Record Data", typeof(string))
            };

            Utility util = new Utility();

            DataTable combinedData = new DataTable();
            for (int i = 0; i < spc.Count(); i++)
            {
                combinedData.Columns.Add(spc[i].name, spc[i].type);
            }

            Stream staffStream = staffFile.InputStream;
            DataTable staffTable = util.GetClientStaffDataTable(staffStream);

            Stream authorizationStream = authorizationFile.InputStream;
            DataTable authorizationTable = util.GetClientAuthorizationsDataTable(authorizationStream);

            DataTable outTable = new DataTable();
            outTable = staffTable.Copy();
            outTable.Merge(authorizationTable);

            var groupedByClientId = outTable.AsEnumerable().GroupBy(row => row.Field<string>("Client ID"));
            foreach (var clientGroup in groupedByClientId)
            {
                int recType = 1; //Staff
                int rowNum = 0;
                foreach (DataRow row in clientGroup)
                {
                    String recordData = String.Format("{0,-10} {1,-35} {2,-35}", row.Field<string>("Staff Id"), row.Field<string>("Staff Name"), row.Field<string>("Staff Role"));
                    combinedData.Rows.Add(row.Field<string>("Client Id"), row.Field<string>("Client Name"), recType, rowNum, recordData);
                    rowNum++;
                }
            }

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                //streamWriter.WriteLine("Client Staff Report");
                //streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                //streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                //streamWriter.WriteLine(" ");

                foreach (DataRow row in combinedData.Rows)
                {
                    streamWriter.WriteLine("{0,-10} {1,-35} {2,-3} {3,-3} {4,-80}", row.Field<string>("Client Id"), row.Field<string>("Client Name"), row.Field<int>("Record Type"), row.Field<int>("Record Order"),
                        row.Field<string>("Record Data"));
                }
                streamWriter.Flush();
                return ms;
            }
        }
    }
}