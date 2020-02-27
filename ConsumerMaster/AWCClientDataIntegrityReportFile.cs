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

        string[] columns = {"name", "gender", "dob", "current_location", "current_phone_day", "intake_date", "managing_office",
                "program_name", "unit", "program_modifier", "worker_name", "worker_role", "is_primary_worker", "medicaid_number", "medicaid_payer", "medicaid_plan_name"};

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
                DataTable clientRosterDataTable = util. GetClientRosterDataTable(clientRosterInput);

                //int totalConsumers = crrDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet, columns);

                PatternFill greenSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromArgb(255, 46, 204, 113), Colors.Transparent);
                PatternFill redSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 0, 0), Colors.Transparent);
                PatternFill whiteSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 255, 255), Colors.Transparent);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in clientRosterDataTable.Rows)
                {
                    int columnNumber = 0;

                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetValue(dr[columns[1]].ToString());  //Column A

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["gender"].ToString(), null) ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["gender"].ToString());    //Column C - gender  
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["dob"].ToString(), null) ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["dob"].ToString());  //Column D - dob  
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["current_location"].ToString(), null) ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["current_location"].ToString());           //Column E - current_location (address)
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["current_phone_day"].ToString(), null) ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["current_phone_day"].ToString());     //Column F - current_phone_day
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["intake_date"].ToString(), null) ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["intake_date"].ToString());           //Column G - intake_date  
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["managing_office"].ToString(), "Washington") ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["managing_office"].ToString());           //Column H - managing_office
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["program_name"].ToString(), "Agency With Choice") ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["program_name"].ToString());           //Column I - program_name  
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["unit"].ToString(), "Room 1") ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["unit"].ToString());           //Column J - unit
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["program_modifier"].ToString(), null) ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["program_modifier"].ToString());          //Column K - program_modifier
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["worker_name"].ToString(), null) ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["worker_name"].ToString());           //Column L - worker_name
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["worker_role"].ToString(), "AWC VP/Regional Manager/Support Staff") ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["worker_role"].ToString());           //Column M - worker_role
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["is_primary_worker"].ToString(), "Yes") ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["is_primary_worker"].ToString());             //Column N - is_primary_worker
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["medicaid_number"].ToString(), null) ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["medicaid_number"].ToString());        //Column O - medicaid_number
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["medicaid_payer"].ToString(), "Medicaid") ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["medicaid_payer"].ToString());        //Column P - medicaid_payer
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, columnNumber].SetFill(IsInvalid(dr["medicaid_plan_name"].ToString(), "PA Medical Assistance Waiver") ? redSolidFill : whiteSolidFill);
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr["medicaid_plan_name"].ToString());       //Column Q - medicaid_plan_name
                    sheet1Worksheet.Cells[currentRow, columnNumber++].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    currentRow++;
                }

                for (int i = 0; i < clientRosterDataTable.Columns.Count; i++)
                {
                    sheet1Worksheet.Columns[i].AutoFitWidth();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }

        private bool IsInvalid(string value, string expected)
        {
            if(string.IsNullOrEmpty(value) || value == "null" || value == "N/A")
            {
                return true;
            }
            else if(expected != null && value != expected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void PrepareSheet1Worksheet(Worksheet worksheet, string[] columns)
        {
            try
            {
                //string[] columnsList = {"name", "gender", "dob", "current_location", "current_phone_day", "intake_date", "managing_office",
                //"program_name", "unit", "program_modifier", "worker_name", "worker_role", "is_primary_worker", "medicaid_number", "medicaid_payer", "medicaid_plan_name"};

                foreach (string column in columns)
                {
                    int columnKey = Array.IndexOf(columns, column);
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