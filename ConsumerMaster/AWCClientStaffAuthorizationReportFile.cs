using System;
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

        public MemoryStream CreateStaffAuthorizationDocument(UploadedFile clientFile, UploadedFile staffFile, UploadedFile authorizationFile)
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

            Stream clientStream = clientFile.InputStream;
            DataTable clientTable = util.GetClientAddressDataTable(clientStream);

            Stream staffStream = staffFile.InputStream;
            DataTable staffTable = util.GetClientStaffDataTable(staffStream);

            Stream authorizationStream = authorizationFile.InputStream;
            DataTable authorizationTable = util.GetClientAuthorizationsDataTable(authorizationStream);

            //string clientsRangeName = "Clients";
            string staffRangeName = "Staff";
            string authorizationsRangeName = "Authorizations";

            DataSet clientStaffAuthorizations = new DataSet("ClientStaffAuthorizations");

            clientStaffAuthorizations.Tables.Add(clientTable);
            clientStaffAuthorizations.Tables.Add(staffTable);
            clientStaffAuthorizations.Tables.Add(clientTable);

            clientStaffAuthorizations.Relations.Add(membersRangeName, clientTable.Columns["Id"], members.Columns["ClientId"]);
            data.Relations.Add(authorizationsRangeName, clients.Columns["Id"], authorizations.Columns["ClientId"]);



            DataTable outTable = new DataTable();
            outTable = staffTable.Copy();
            outTable.Merge(authorizationTable);

         
            var groupedByClientId = outTable.AsEnumerable().GroupBy(r => r.Field<string>("ClientID"));

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                foreach (var clientGroup in groupedByClientId)
                {
                    var sortedClient = clientGroup.OrderBy(r => r.Field<int>("RecordType")).ThenBy(r => r.Field<int>("RecordOrder"));

                    foreach (DataRow row in sortedClient)
                    {
                        streamWriter.WriteLine("{0,-10} {1,-35} {2,-3} {3,-3} {4,-80}", row.Field<string>("ClientId"), row.Field<string>("ClientName"), row.Field<int>("RecordType"), row.Field<int>("RecordOrder"),
                            row.Field<string>("RecordData"));
                    }
                    streamWriter.WriteLine(" ");
                }
                streamWriter.Flush();
                return ms;
            }
        }
    }
}