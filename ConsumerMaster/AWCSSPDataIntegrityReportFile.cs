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
    //enum CompareMethod
    //{
    //    Expected = 0,
    //    EqualToZero = 1,
    //    NotEqualToOne = 2
    //}

    public class AWCSSPDataIntegrityReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 1;

        public Workbook CreateWorkbook(UploadedFile clientRosterFile, UploadedFile clientAuthorizationListFile, UploadedFile clientStaffListFile)
        {
            DIColumn[] dIColumns = new DIColumn[19]
            {
                new DIColumn("name", null, false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("gender", null, true, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("dob", null, false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("current_location", null, false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("current_phone_day", null, false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("intake_date", null, false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("managing_office", "Washington", false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("program_name", "Agency With Choice", false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("unit", "Room 1", false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("program_modifier", null, false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("worker_name", null, false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("worker_role", "AWC VP/Regional Manager/Support Staff", false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("is_primary_worker", "Yes", true, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("medicaid_number", null, false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("medicaid_payer", "Medicaid", false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("medicaid_plan_name", "PA Medical Assistance Waiver", false, typeof(string), (int)CompareMethod.Expected),
                new DIColumn("ba_count", null, true, typeof(int), (int)CompareMethod.EqualToZero),
                new DIColumn("me_count", null, true, typeof(int), (int)CompareMethod.NotEqualToOne),
                new DIColumn("ssp_count", null, true, typeof(int), (int)CompareMethod.EqualToZero)
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

                Stream clientStaffListFileInput = clientStaffListFile.InputStream;
                DataTable clientStaffListDataTable = util.GetClientStaffListDataTable(clientStaffListFileInput);
                DataTable clientStaffDataTable = util.ClientStaffGroupBy("SClientID", "MemberRole", clientStaffListDataTable);

                var JoinResult = (from c in clientRosterDataTable.AsEnumerable()
                                  join a in clientAuthorizationDataTable.AsEnumerable() on c.Field<string>("id_no") equals a.Field<string>("AClientID")
                                  join s in clientStaffDataTable.AsEnumerable() on c.Field<string>("id_no") equals s.Field<string>("SClientID") into tempJoin
                                  from leftJoin in tempJoin.DefaultIfEmpty()
                                  select new
                                  {
                                      id_no = c.Field<string>("id_no"),
                                      name = c.Field<string>("name"),
                                      gender = c.Field<string>("gender"),
                                      dob = c.Field<string>("dob"),
                                      current_location = c.Field<string>("current_location"),
                                      current_phone_day = c.Field<string>("current_phone_day"),
                                      intake_date = c.Field<string>("intake_date"),
                                      managing_office = c.Field<string>("managing_office"),
                                      program_name = c.Field<string>("program_name"),
                                      unit = c.Field<string>("unit"),
                                      program_modifier = c.Field<string>("program_modifier"),
                                      worker_name = c.Field<string>("worker_name"),
                                      worker_role = c.Field<string>("worker_role"),
                                      is_primary_worker = c.Field<string>("is_primary_worker"),
                                      medicaid_number = c.Field<string>("medicaid_number"),
                                      medicaid_payer = c.Field<string>("medicaid_payer"),
                                      medicaid_plan_name = c.Field<string>("medicaid_plan_name"),
                                      ba_count = leftJoin == null ? null : leftJoin.Field<string>("ba_count"),
                                      me_count = leftJoin == null ? null : leftJoin.Field<string>("me_count"),
                                      ssp_count = leftJoin == null ? null : leftJoin.Field<string>("ssp_count")
                                  }).ToList();

                DataTable joinResult = JoinResult.ToDataTable();

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
                //foreach (DataRow dr in clientRosterDataTable.Rows)
                foreach (DataRow dr in joinResult.Rows)
                {
                    for (int columnNumber = 0; columnNumber < dIColumns.Count(); columnNumber++)
                    {
                        CellSelection cellSelection = FormattedColumn(sheet1Worksheet, currentRow, columnNumber, dr[dIColumns[columnNumber].name].ToString(), dIColumns[columnNumber]);
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

        private bool IsExpected(string value, string expected)
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

        public CellSelection FormattedColumn(Worksheet worksheet, int row, int col, string value, DIColumn diColumn)
        {
            CellSelection cellSelection = null;
            try
            {
                PatternFill redSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 0, 0), Colors.Transparent);
                PatternFill whiteSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 255, 255), Colors.Transparent);

                cellSelection = worksheet.Cells[row, col];

                if (diColumn.type == typeof(string))
                {
                    worksheet.Cells[row, col].SetFill(IsExpected(value, diColumn.expected) ? redSolidFill : whiteSolidFill);
                }
                else if (diColumn.type == typeof(int))
                {
                    if (diColumn.method == (int)CompareMethod.EqualToZero)
                    {
                        worksheet.Cells[row, col].SetFill(Convert.ToInt32(value) == 0 ? redSolidFill : whiteSolidFill);
                    }
                    else if (diColumn.method == (int)CompareMethod.NotEqualToOne)
                    {
                        worksheet.Cells[row, col].SetFill(Convert.ToInt32(value) != 1 ? redSolidFill : whiteSolidFill);
                    }
                }

                worksheet.Cells[row, col].SetValue(value);  
                
                if(diColumn.isCentered)
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