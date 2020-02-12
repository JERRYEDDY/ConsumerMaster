using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;
using System.Collections.Generic;

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
                new SPColumn("ClientID", typeof(string)),
                new SPColumn("ClientName", typeof(string)),
                new SPColumn("RecordType", typeof(int)),
                new SPColumn("RecordOrder", typeof(int)),
                new SPColumn("RecordData", typeof(string))
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

            //DataTable sortedTable = outTable.AsEnumerable().OrderBy(r => r.Field<string>("ClientID")).ThenBy(r => r.Field<int>("RecordType")).ThenBy(r => r.Field<int>("RecordOrder")).CopyToDataTable();
            
            var groupedByClientId = outTable.AsEnumerable().GroupBy(r => r.Field<string>("ClientID"));


            //DataTable dt_grouped_by = outTable.AsEnumerable()
            //              .GroupBy(r => new
            //              {
            //                  ClientID = r.Field<string>("ClientID")
            //              })
            //              .Select(g => new
            //              {
            //                  Code = g.      .First().Field<string>("CODE"),
            //                  SumQr = g.Sum(x => x.Field<int>("quantity_received"))
            //                   SumDr = g.Sum(x => x.Field<int>("damage_received"))
            //              })
            //              .OrderBy(x => x.Code)
            //              .CopyToDataTable();

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                foreach (var clientGroup in groupedByClientId)
                {
                    foreach (DataRow row in clientGroup)
                    {
                        streamWriter.WriteLine("{0,-10} {1,-35} {2,-3} {3,-3} {4,-80}", row.Field<string>("ClientId"), row.Field<string>("ClientName"), row.Field<int>("RecordType"), row.Field<int>("RecordOrder"),
                            row.Field<string>("RecordData"));
                    }
                    streamWriter.WriteLine(" ");
                }
                streamWriter.Flush();
                return ms;
            }

            //using (var ms = new MemoryStream())
            //using (var streamWriter = new StreamWriter(ms))
            //{
            //    //streamWriter.WriteLine("Client Staff Report");
            //    //streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            //    //streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
            //    //streamWriter.WriteLine(" ");

            //    foreach (DataRow row in combinedData.Rows)
            //    {
            //        streamWriter.WriteLine("{0,-10} {1,-35} {2,-3} {3,-3} {4,-80}", row.Field<string>("Client Id"), row.Field<string>("Client Name"), row.Field<int>("Record Type"), row.Field<int>("Record Order"),
            //            row.Field<string>("Record Data"));
            //    }

            //}
        }
    }
}