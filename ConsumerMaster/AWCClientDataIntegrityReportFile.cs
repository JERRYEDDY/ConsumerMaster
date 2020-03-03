using System;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;
using GemBox.Document;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using System.ComponentModel;
using System.Collections.Generic;

namespace ConsumerMaster
{
    public class AWCClientDataIntegrityReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 1;

        public Workbook CreateWorkbook(UploadedFile clientRosterFile, UploadedFile clientAuthorizationListFile, UploadedFile clientStaffListFile)
        {
            DIColumn[] dIColumns = new DIColumn[19]
            {
                new DIColumn("name", null, false),
                new DIColumn("gender", null, true),
                new DIColumn("dob", null, false),
                new DIColumn("current_location", null, false),
                new DIColumn("current_phone_day", null, false),
                new DIColumn("intake_date", null, false),
                new DIColumn("managing_office", "Washington", false),
                new DIColumn("program_name", "Agency With Choice", false),
                new DIColumn("unit", "Room 1", false),
                new DIColumn("program_modifier", null, false),
                new DIColumn("worker_name", null, false),
                new DIColumn("worker_role", "AWC VP/Regional Manager/Support Staff", false),
                new DIColumn("is_primary_worker", "Yes", true),
                new DIColumn("medicaid_number", null, false),
                new DIColumn("medicaid_payer", "Medicaid", false),
                new DIColumn("medicaid_plan_name", "PA Medical Assistance Waiver", false),
                new DIColumn("ba_count", null, false),
                new DIColumn("me_count", null, false),
                new DIColumn("ssp_count", null, false)
            };

            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];
                HeaderFooterSettings settings = sheet1Worksheet.WorksheetPageSetup.HeaderFooterSettings;
 
                Utility util = new Utility();
                Stream clientRosterInput = clientRosterFile.InputStream;
                DataTable clientRosterDataTable = util.GetClientRosterDataTable(clientRosterInput);

                Stream clientAuthorizationListInput = clientAuthorizationListFile.InputStream;
                DataTable clientAuthorizationListDataTable = util.GetClientAuthorizationListDataTable(clientAuthorizationListInput);
                DataTable clientAuthorizationDataTable = util.ClientAuthorizationGroupBy("AClientID", "Service", clientAuthorizationListDataTable);
                //DataTable clientAuthorizationDataTable = util.RemoveDuplicateRows(clientAuthorizationListDataTable, "AClientID");

                Stream clientStaffListFileInput = clientStaffListFile.InputStream;
                DataTable clientStaffListDataTable = util.GetClientStaffListDataTable(clientStaffListFileInput);
                DataTable clientStaffDataTable = util.ClientStaffGroupBy("SClientID", "MemberRole", clientStaffListDataTable);


                //int clientRows = clientAuthorizationListDataTable.Rows.Count;
                //int noDupRows = noDuplicates.Rows.Count;

                var JoinResult = (from c in clientRosterDataTable.AsEnumerable()
                                  join a in clientAuthorizationDataTable.AsEnumerable() on c.Field<string>("id_no") equals a.Field<string>("AClientID")
                                  join s in clientStaffDataTable.AsEnumerable() on c.Field<string>("id_no") equals s.Field<string>("SClientID") into tempJoin
                                  from leftJoin in tempJoin.DefaultIfEmpty()
                                  select new
                                  {
                                      IDNo = c.Field<string>("id_no"),
                                      Name = c.Field<string>("name"),
                                      Gender = c.Field<string>("gender"),
                                      DOB = c.Field<string>("dob"),
                                      CurrentLocation = c.Field<string>("current_location"),
                                      CurrentPhoneDay = c.Field<string>("current_phone_day"),
                                      IntakeDate = c.Field<string>("intake_date"),
                                      ManagingOffice = c.Field<string>("managing_office"),
                                      ProgramName = c.Field<string>("program_name"),
                                      Unit = c.Field<string>("unit"),
                                      ProgramModifier = c.Field<string>("program_modifier"),
                                      WorkerName = c.Field<string>("worker_name"),
                                      WorkerRole = c.Field<string>("worker_role"),
                                      IsPrimaryWorker = c.Field<string>("is_primary_worker"),
                                      MedicaidNumber = c.Field<string>("medicaid_number"),
                                      MedicaidPayer = c.Field<string>("medicaid_payer"),
                                      MedicaidPlanName = c.Field<string>("medicaid_plan_name"),
                                      //AuthClientID = leftJoin == null ? "0" : leftJoin.Field<string>("AClientID"),
                                      //ClientName = leftJoin.Field<string>("ClientName"),
                                      //From = leftJoin.Field<string>("From"),
                                      //To = leftJoin.Field<string>("To"),
                                      BACount = a.Field<int>("BACount"),
                                      //,
                                      //Total = leftJoin.Field<string>("Total"),
                                      //Used = leftJoin.Field<string>("Used"),
                                      //Balance = leftJoin.Field<string>("Balance"),
                                      //Program = leftJoin.Field<string>("Program")
                                      //StaffClientID = leftJoin == null ? "0" : leftJoin.Field<string>("SClientID"),
                                      MECount = leftJoin.Field<int>("MECount"),
                                      SSPCount = leftJoin.Field<int>("SSPCount")
                                  }).ToList();

                DataTable joinResult = JoinResult.ToDataTable();

                //int totalConsumers = crrDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet, dIColumns);

                CellIndex A1Cell = new CellIndex(0, 0);
                CellIndex D1Cell = new CellIndex(0, 3);
                sheet1Worksheet.Cells[A1Cell, D1Cell].Merge();
                sheet1Worksheet.Cells[0, 0].SetValue("AWC Client Data Integrity Report");
                sheet1Worksheet.Cells[0, 0].SetIsBold(true);
                sheet1Worksheet.Cells[0, 0].SetFontSize(20);

                sheet1Worksheet.Cells[1, 0].SetValue("Date:" + DateTime.Now.ToString("MM/dd/yyyy"));

                PatternFill greenSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromArgb(255, 46, 204, 113), Colors.Transparent);
                PatternFill redSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 0, 0), Colors.Transparent);
                PatternFill whiteSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 255, 255), Colors.Transparent);

                int currentRow = IndexRowItemStart + 3;
                foreach (DataRow dr in clientRosterDataTable.Rows)
                {
                    for (int columnNumber = 0; columnNumber < dIColumns.Count(); columnNumber++)
                    {
                        CellSelection cellSelection = FormattedColumn(sheet1Worksheet, currentRow, columnNumber, dr[dIColumns[columnNumber].name].ToString(), dIColumns[columnNumber].expected, dIColumns[columnNumber].isCentered);
                    }

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
            bool returnValue = false;
            try
            {
                if(string.IsNullOrEmpty(value) || value == "null" || value == "N/A")
                {
                    returnValue =  true;
                }
                else if(expected != null && value != expected)
                {
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return returnValue;
        }
        private bool IsInvalidCount(string value, string expected)
        {
            bool returnValue = false;
            try
            {
                if (string.IsNullOrEmpty(value) || value == "null" || value == "N/A")
                {
                    returnValue = true;
                }
                else if (expected != null && value != expected)
                {
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return returnValue;
        }

        public CellSelection FormattedColumn(Worksheet worksheet, int row, int col, string value, string expected, bool isCentered)
        {
            CellSelection cellSelection = null;
            try
            {
                PatternFill redSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 0, 0), Colors.Transparent);
                PatternFill whiteSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 255, 255), Colors.Transparent);

                cellSelection = worksheet.Cells[row, col];

                if (Enumerable.Range(16, 18).Contains(col))
                {
                    if(col == 16)
                        worksheet.Cells[row, col].SetFill(value == 0 ? redSolidFill : whiteSolidFill);
                    else if (col == 17)
                        worksheet.Cells[row, col].SetFill(IsInvalid(value, expected) ? redSolidFill : whiteSolidFill);
                    else
                        worksheet.Cells[row, col].SetFill(IsInvalid(value, expected) ? redSolidFill : whiteSolidFill);
                }
                else
                {
                    worksheet.Cells[row, col].SetFill(IsInvalid(value, expected) ? redSolidFill : whiteSolidFill);
                }

                worksheet.Cells[row, col].SetValue(value);  
                
                if(isCentered)
                    worksheet.Cells[row, col].SetHorizontalAlignment(RadHorizontalAlignment.Center);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return cellSelection;
        }

        private void PrepareSheet1Worksheet(Worksheet worksheet, DIColumn[] columns)
        {
            try
            {
                PatternFill yellowSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 216, 0), Colors.Transparent);

                foreach (DIColumn column in columns)
                {
                    int columnKey = Array.IndexOf(columns, column);

                    int row = IndexRowItemStart + 1;
                    worksheet.Cells[row, columnKey].SetValue(column.expected);
                    worksheet.Cells[row, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    worksheet.Cells[row, columnKey].SetIsBold(true);

                    row++;
                    worksheet.Cells[row, columnKey].SetFill(yellowSolidFill);
                    worksheet.Cells[row, columnKey].SetValue(column.name.ToUpper());
                    worksheet.Cells[row, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    worksheet.Cells[row, columnKey].SetIsBold(true);
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