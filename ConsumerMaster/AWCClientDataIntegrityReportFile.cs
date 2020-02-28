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

        //string[] columns = {"name", "gender", "dob", "current_location", "current_phone_day", "intake_date", "managing_office",
        //        "program_name", "unit", "program_modifier", "worker_name", "worker_role", "is_primary_worker", "medicaid_number", "medicaid_payer", "medicaid_plan_name"};

        public Workbook CreateWorkbook(UploadedFile clientRosterFile)
        {
            DIColumn[] dIColumns = new DIColumn[16]
            {
                new DIColumn("name", null, null),
                new DIColumn("gender",null, null),
                new DIColumn("dob", null, null),
                new DIColumn("current_location", null, null),
                new DIColumn("current_phone_day", null, null),
                new DIColumn("intake_date", null, null),
                new DIColumn("managing_office", null, "Washington"),
                new DIColumn("program_name", null, "Agency With Choice"),
                new DIColumn("unit", null, "Room 1"),
                new DIColumn("program_modifier", null, null),
                new DIColumn("worker_name", null, null),
                new DIColumn("worker_role", null, "AWC VP/Regional Manager/Support Staff"),
                new DIColumn("is_primary_worker", null, "Yes"),
                new DIColumn("medicaid_number", null, null),
                new DIColumn("medicaid_payer", null, "Medicaid"),
                new DIColumn("medicaid_plan_name", null, "PA Medical Assistance Waiver")
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
                DataTable clientRosterDataTable = util. GetClientRosterDataTable(clientRosterInput);

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
                    int columnNumber = 0;
                    sheet1Worksheet.Cells[currentRow, columnNumber].SetValue(dr[dIColumns[columnNumber].name].ToString());  //Column A name
                    columnNumber++;


                    CellSelection cellSelection = FormattedColumn(sheet1Worksheet, currentRow, columnNumber, dr[dIColumns[columnNumber].name].ToString(), dIColumns[columnNumber].expected);
                    //CellSelection cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    //string columnValue = dr[dIColumns[columnNumber].name].ToString();   
                    //cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    //cellSelection.SetValue(columnValue);     //Column B gender
                    //cellSelection.SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    string columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue);    //Column C - dob   
                    cellSelection.SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue);    //Column D - current_location  
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue); //Column E - current_phone_day
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue);    //Column F - intake_date  
                    cellSelection.SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue);  //Column G - managing_office
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue);  //Column H - program_name 
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue);    //Column I - unit  
                    cellSelection.SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue);    //Column J - program_modifier  
                    cellSelection.SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue); //Column K - worker_name
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue); //Column L - worker_role
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue);    //Column M - is_primary_worker 
                    cellSelection.SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue);   //Column N - medicaid_number
                    cellSelection.SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue); //Column O - medicaid_payer
                    columnNumber++;

                    cellSelection = sheet1Worksheet.Cells[currentRow, columnNumber];
                    columnValue = dr[dIColumns[columnNumber].name].ToString();
                    cellSelection.SetFill(IsInvalid(columnValue, dIColumns[columnNumber].expected) ? redSolidFill : whiteSolidFill);
                    cellSelection.SetValue(columnValue); //Column P - medicaid_plan_name
                    columnNumber++;

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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false
            }
        }


        public CellSelection FormattedColumn(Worksheet worksheet, int row, int col, string value, string expected)
        {
            CellSelection cellSelection = null;
            try
            {
                PatternFill redSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 0, 0), Colors.Transparent);
                PatternFill whiteSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 255, 255), Colors.Transparent);

                cellSelection = worksheet.Cells[row, col];
                worksheet.Cells[row, col].SetFill(IsInvalid(value, expected) ? redSolidFill : whiteSolidFill);
                worksheet.Cells[row, col].SetValue(value);      
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