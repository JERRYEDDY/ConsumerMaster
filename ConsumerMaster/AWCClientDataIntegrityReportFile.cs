using System;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;
using GemBox.Document;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
namespace ConsumerMaster
{
    public class AWCClientDataIntegrityReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 1;

        public Workbook CreateWorkbook(UploadedFile clientRosterFile)
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];
                HeaderFooterSettings settings = sheet1Worksheet.WorksheetPageSetup.HeaderFooterSettings;
 
                Utility util = new Utility();
                Stream clientRosterInput = clientRosterFile.InputStream;
                DataTable crrDataTable = util. GetClientRosterDataTable(clientRosterInput);


                //int totalConsumers = crrDataTable.Rows.Count;
                //PrepareSheet1Worksheet(sheet1Worksheet);

                PatternFill greenSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromArgb(255, 46, 204, 113), Colors.Transparent);
                PatternFill redSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 0, 0), Colors.Transparent);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in crrDataTable.Rows)
                {
                    sheet1Worksheet.Cells[currentRow, 0].SetValue(dr["name"].ToString());       //Column A



                    sheet1Worksheet.Cells[currentRow, 1].SetFill(greenSolidFill);
                    sheet1Worksheet.Cells[currentRow, 1].SetValue("=CHAR(252)");       //Column B - name
                    sheet1Worksheet.Cells[currentRow, 1].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 2].SetFill(redSolidFill);
                    sheet1Worksheet.Cells[currentRow, 2].SetValue("X");           //Column C - gender  
                    sheet1Worksheet.Cells[currentRow, 2].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 3].SetValue("=CHAR(252)");  //Column D - dob  
                    sheet1Worksheet.Cells[currentRow, 3].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 4].SetValue("X");           //Column E - program_name  
                    sheet1Worksheet.Cells[currentRow, 4].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 5].SetValue("X");           //Column F - unit
                    sheet1Worksheet.Cells[currentRow, 5].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 6].SetValue("X");           //Column G - worker_name
                    sheet1Worksheet.Cells[currentRow, 6].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 8].SetValue("X");           //Column I - worker_role
                    sheet1Worksheet.Cells[currentRow, 8].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 11].SetValue("X");             //Column L - is_primary_worker
                    sheet1Worksheet.Cells[currentRow, 11].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 14].SetValue("X");           //Column O - current_location (address)
                    sheet1Worksheet.Cells[currentRow, 14].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    string cpdResult = dr["current_phone_day"].ToString() == "null" ? "=CHAR(251)" : "=CHAR(252)";
                    PatternFill cpdFill = dr["current_phone_day"].ToString() == "null" ? redSolidFill : greenSolidFill;
                    sheet1Worksheet.Cells[currentRow, 16].SetFill(cpdFill);
                    sheet1Worksheet.Cells[currentRow, 16].SetValue(cpdResult);           //Column Q - current_phone_day
                    sheet1Worksheet.Cells[currentRow, 16].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 18].SetValue("X");           //Column S - managing_office
                    sheet1Worksheet.Cells[currentRow, 18].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 19].SetValue("X");              //Column T - medicaid_number
                    sheet1Worksheet.Cells[currentRow, 19].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 25].SetValue("X");           //Column Z - medicaid_payer
                    sheet1Worksheet.Cells[currentRow, 25].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 26].SetValue("X");       //Column AA - medicaid_plan_name
                    sheet1Worksheet.Cells[currentRow, 26].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, 27].SetValue("X");              //Column AB - program_modifier
                    sheet1Worksheet.Cells[currentRow, 27].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    currentRow++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }

        private void PrepareSheet1Worksheet(Worksheet worksheet)
        {
            try
            {
                ConsumerRatioReportFormat crrf = new ConsumerRatioReportFormat();
                string[] columnsList = crrf.HeaderStrings;
                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column);
                    string columnName = column;

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetIsBold(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public MemoryStream CreateClientDataIntegrityDocument(UploadedFile clientFile, UploadedFile memberFile, UploadedFile authorizationFile)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
            Utility util = new Utility();

            string clientsRangeName = "Clients";
            string memberRangeName = "Members";
            string authorizationsRangeName = "Authorizations";

            using (var ms = new MemoryStream())
            {
                Stream clientStream = clientFile.InputStream;
                DataTable clients = util.GetClientAddressDataTable(clientStream);
                clients.TableName = clientsRangeName;

                Stream memberStream = memberFile.InputStream;
                DataTable members = util.GetClientMemberDataTable(memberStream);
                members.TableName = memberRangeName;

                Stream authorizationStream = authorizationFile.InputStream;
                DataTable authorizations = util.GetClientAuthorizationsDataTable(authorizationStream);
                authorizations.TableName = authorizationsRangeName;

                DataSet clientStaffAuthorizations = new DataSet("ClientStaffAuthorizations");

                clients.PrimaryKey = new DataColumn[] { clients.Columns["ClientID"] };
                clientStaffAuthorizations.Tables.Add(clients);
                clientStaffAuthorizations.Tables.Add(members);
                clientStaffAuthorizations.Tables.Add(authorizations);

                clientStaffAuthorizations.Relations.Add(memberRangeName, clients.Columns["ClientID"], members.Columns["ClientID"], false);  //Could be no staff members for client
                clientStaffAuthorizations.Relations.Add(authorizationsRangeName, clients.Columns["ClientID"], authorizations.Columns["ClientID"], false); //Could be no billing authorizations for client

                var document = DocumentModel.Load(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/MergeNestedRanges.docx")); //Mail Merge Template Document
                document.MailMerge.RangeStartPrefix = "START:";
                document.MailMerge.RangeEndPrefix = "END:";
                document.MailMerge.Execute(clientStaffAuthorizations, null);    // Execute nested mail merge.
                document.Save(ms, SaveOptions.DocxDefault);

                return ms;
            }
        }

        public MemoryStream CreateDocument(UploadedFile uploadedFile)
        {
            Utility util = new Utility();
            Stream input = uploadedFile.InputStream;


            DataTable staffTable = util.GetClientMemberDataTable(input);

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
    }
}