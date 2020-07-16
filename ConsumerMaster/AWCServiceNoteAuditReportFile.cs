using System;
using System.Data;
using Telerik.Web.UI;
using System.IO;
using System.Linq;

namespace ConsumerMaster
{
    public class AWCServiceNoteAuditReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public MemoryStream CreateDocument(UploadedFile closedActivitiesFile, UploadedFile auditLogFile)
        {
            Utility util = new Utility();
            Stream closedActivitiesInput = closedActivitiesFile.InputStream;
            Stream auditLogInput = auditLogFile.InputStream;

            DataTable closedActivitiesTable = util.GetClosedActivitiesDataTable(closedActivitiesInput);
            DataTable auditLogTable = util.GetAuditLogDataTable(auditLogInput);



            var JoinResult = (from ClosedActivities in closedActivitiesTable.AsEnumerable()
                           join AuditLog in auditLogTable.AsEnumerable()
                           on ClosedActivities.Field<string>("Activity ID") equals AuditLog.Field<string>("Activity ID") into tempJoin
                           from leftJoin in tempJoin.DefaultIfEmpty()
                           select new 
                           { 
                               CA_ActivityID = ClosedActivities.Field<string>("Activity ID"),
                               AL_ActivityID = leftJoin.Field<string>("Activity ID"),
                               CA_ActivityName = ClosedActivities.Field<string>("Activity Name"),
                               AL_Subject = leftJoin.Field<string>("Subject"),
                               CA_StartTime = ClosedActivities.Field<DateTime>("Start Time"),
                               CA_StopTime = ClosedActivities.Field<DateTime>("Stop Time"),
                               AL_StartTime = leftJoin.Field<DateTime>("Start Time"),
                               AL_StopTime = leftJoin.Field<DateTime>("Stop Time"),
                               AL_Action = leftJoin.Field<string>("Action"),
                               AL_Comment = leftJoin.Field<string>("Comment")
                           }).ToList();

            DataTable joinResult = JoinResult.ToDataTable();

            // or select new { FirstColumn= DataFileInfos.FirstColumn, ... }.ToList();




            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                //streamWriter.WriteLine("Client Staff Authorization Report");
                //streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                //streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                //streamWriter.WriteLine(" ");

                //var groupedByClientId = staffTable.AsEnumerable().GroupBy(row => row.Field<string>("Client ID"));
                //foreach (var clientGroup in groupedByClientId)
                //{
                //    int first = 1;
                //    foreach (DataRow row in clientGroup)
                //    {
                //        if (first == 1)
                //        {
                //            streamWriter.WriteLine("-------------------------------------------------------------------------------------------------------------");
                //            streamWriter.WriteLine("Client ID:{0,-10}", row.Field<string>("Client ID"));
                //            streamWriter.WriteLine("Client Name:{0,-30}", row.Field<string>("Client Name"));
                //            streamWriter.WriteLine(" ");
                //            streamWriter.WriteLine("Billing Authorizations");
                //            streamWriter.WriteLine("{0,-15} {1,-15} {2,-50} {3,-12} {4,-12} {5,-12}", "From", "To", "Service", "Total", "Used ", "Balance");
                //        }

                //        streamWriter.WriteLine("{0,-15} {1,-15} {2,-50} {3,-12} {4,-12} {5,-12}", row.Field<string>("From Date"), 
                //            row.Field<string>("To Date"), row.Field<string>("Service"), row.Field<string>("Total Units"), row.Field<string>("Units Used"), row.Field<string>("Balance"));

                //        first++;
                //    }
                //    streamWriter.WriteLine(" ");
                //}

                //streamWriter.Flush();
                return ms;
            }
        }

        //    public Workbook CreateWorkbook(UploadedFile closedActivitiesFile, UploadedFile auditLogFile)
        //    {
        //        DIColumn[] dIColumns = new DIColumn[19]
        //        {
        //            new DIColumn("name", null, false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("gender", null, true, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("dob", null, false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("current_location", null, false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("current_phone_day", null, false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("intake_date", null, false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("managing_office", "Washington", false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("program_name", "Agency With Choice", false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("unit", "Room 1", false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("program_modifier", null, false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("worker_name", null, false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("worker_role", "AWC VP/Regional Manager/Support Staff", false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("is_primary_worker", "Yes", true, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("medicaid_number", null, false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("medicaid_payer", "Medicaid", false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("medicaid_plan_name", "PA Medical Assistance Waiver", false, typeof(string), (int)CompareMethod.Expected),
        //            new DIColumn("ba_count", null, true, typeof(int), (int)CompareMethod.EqualToZero),
        //            new DIColumn("me_count", null, true, typeof(int), (int)CompareMethod.NotEqualToOne),
        //            new DIColumn("ssp_count", null, true, typeof(int), (int)CompareMethod.EqualToZero)
        //        };

        //        Workbook workbook = new Workbook();

        //        try
        //        {
        //            WorksheetCollection worksheets = workbook.Worksheets;
        //            worksheets.Add();

        //            Worksheet sheet1Worksheet = worksheets["Sheet1"];
        //            HeaderFooterSettings settings = sheet1Worksheet.WorksheetPageSetup.HeaderFooterSettings;

        //            Utility util = new Utility();
        //            Stream clientRosterInput = clientRosterFile.InputStream;
        //            DataTable clientRosterDataTable = util.GetClientRosterDataTable(clientRosterInput);
        //            int crCount = clientRosterDataTable.Rows.Count;

        //            Stream clientAuthorizationListInput = clientAuthorizationListFile.InputStream;
        //            DataTable clientAuthorizationListDataTable = util.GetClientAuthorizationListDataTable(clientAuthorizationListInput);
        //            DataTable clientAuthorizationDataTable = util.ClientAuthorizationGroupBy("AClientID", "Service", clientAuthorizationListDataTable);
        //            int caCount = clientAuthorizationDataTable.Rows.Count;

        //            Stream clientStaffListFileInput = clientStaffListFile.InputStream;
        //            DataTable clientStaffListDataTable = util.GetClientStaffListDataTable(clientStaffListFileInput);
        //            DataTable clientStaffDataTable = util.ClientStaffGroupBy("SClientID", "MemberRole", clientStaffListDataTable);
        //            int csCount = clientStaffDataTable.Rows.Count;

        //            var JoinResult1 = (from c in clientRosterDataTable.AsEnumerable()
        //                              join a in clientAuthorizationDataTable.AsEnumerable() on c.Field<string>("id_no") equals a.Field<string>("AClientID")
        //                              into tempJoin
        //                              from leftJoin in tempJoin.DefaultIfEmpty()
        //                              select new
        //                              {
        //                                  id_no = c.Field<string>("id_no"),
        //                                  name = c.Field<string>("name"),
        //                                  gender = c.Field<string>("gender"),
        //                                  dob = c.Field<string>("dob"),
        //                                  current_location = c.Field<string>("current_location"),
        //                                  current_phone_day = c.Field<string>("current_phone_day"),
        //                                  intake_date = c.Field<string>("intake_date"),
        //                                  managing_office = c.Field<string>("managing_office"),
        //                                  program_name = c.Field<string>("program_name"),
        //                                  unit = c.Field<string>("unit"),
        //                                  program_modifier = c.Field<string>("program_modifier"),
        //                                  worker_name = c.Field<string>("worker_name"),
        //                                  worker_role = c.Field<string>("worker_role"),
        //                                  is_primary_worker = c.Field<string>("is_primary_worker"),
        //                                  medicaid_number = c.Field<string>("medicaid_number"),
        //                                  medicaid_payer = c.Field<string>("medicaid_payer"),
        //                                  medicaid_plan_name = c.Field<string>("medicaid_plan_name"),
        //                                  ba_count = leftJoin == null ? "0" : leftJoin.Field<string>("ba_count")
        //                              }).ToList();
        //            DataTable joinResult1 = JoinResult1.ToDataTable();

        //            var JoinResult2 = (from c in joinResult1.AsEnumerable()
        //                              join s in clientStaffDataTable.AsEnumerable() on c.Field<string>("id_no") equals s.Field<string>("SClientID")
        //                              into tempJoin
        //                              from leftJoin in tempJoin.DefaultIfEmpty()
        //                              select new
        //                              {
        //                                  id_no = c.Field<string>("id_no"),
        //                                  name = c.Field<string>("name"),
        //                                  gender = c.Field<string>("gender"),
        //                                  dob = c.Field<string>("dob"),
        //                                  current_location = c.Field<string>("current_location"),
        //                                  current_phone_day = c.Field<string>("current_phone_day"),
        //                                  intake_date = c.Field<string>("intake_date"),
        //                                  managing_office = c.Field<string>("managing_office"),
        //                                  program_name = c.Field<string>("program_name"),
        //                                  unit = c.Field<string>("unit"),
        //                                  program_modifier = c.Field<string>("program_modifier"),
        //                                  worker_name = c.Field<string>("worker_name"),
        //                                  worker_role = c.Field<string>("worker_role"),
        //                                  is_primary_worker = c.Field<string>("is_primary_worker"),
        //                                  medicaid_number = c.Field<string>("medicaid_number"),
        //                                  medicaid_payer = c.Field<string>("medicaid_payer"),
        //                                  medicaid_plan_name = c.Field<string>("medicaid_plan_name"),
        //                                  ba_count = c.Field<string>("ba_count"),
        //                                  me_count = leftJoin == null ? "0" : leftJoin.Field<string>("me_count"),
        //                                  ssp_count = leftJoin == null ? "0" : leftJoin.Field<string>("ssp_count")
        //                              }).ToList();
        //            DataTable joinResult2 = JoinResult2.ToDataTable();

        //            PrepareSheet1Worksheet(sheet1Worksheet, dIColumns);
        //            CellIndex A1Cell = new CellIndex(0, 0);
        //            CellIndex D1Cell = new CellIndex(0, 3);
        //            sheet1Worksheet.Cells[A1Cell, D1Cell].Merge();
        //            sheet1Worksheet.Cells[0, 0].SetValue("AWC Client Data Integrity Report");
        //            sheet1Worksheet.Cells[0, 0].SetIsBold(true);
        //            sheet1Worksheet.Cells[0, 0].SetFontSize(20);

        //            sheet1Worksheet.Cells[1, 0].SetValue("Date:" + DateTime.Now.ToString("MM/dd/yyyy"));

        //            PatternFill greenSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromArgb(255, 46, 204, 113), Colors.Transparent);
        //            PatternFill redSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 0, 0), Colors.Transparent);
        //            PatternFill whiteSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 255, 255), Colors.Transparent);

        //            int currentRow = IndexRowItemStart + 3;

        //            foreach (DataRow dr in joinResult2.Rows)
        //            {
        //                for (int columnNumber = 0; columnNumber < dIColumns.Count(); columnNumber++)
        //                {
        //                    CellSelection cellSelection = FormattedColumn(sheet1Worksheet, currentRow, columnNumber, dr[dIColumns[columnNumber].name].ToString(), dIColumns[columnNumber]);
        //                }

        //                currentRow++;
        //            }

        //            for (int i = 0; i < clientRosterDataTable.Columns.Count; i++)
        //            {
        //                sheet1Worksheet.Columns[i].AutoFitWidth();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.Error(ex);
        //        }
        //        return workbook;
        //    }

        //private bool IsExpected(string value, string expected)
        //{
        //    bool returnValue = false;
        //    try
        //    {
        //        if(string.IsNullOrEmpty(value) || value == "null" || value == "N/A")
        //        {
        //            returnValue =  true;
        //        }
        //        else if(expected != null && value != expected)
        //        {
        //            returnValue = true;
        //        }
        //        else
        //        {
        //            returnValue = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }

        //    return returnValue;
        //}

        //private bool IsEqualTo(string value, int match)
        //{
        //    bool returnValue = false;
        //    try
        //    {
        //        if (string.IsNullOrEmpty(value) || value == null || value == "null" || value == "N/A" || Convert.ToInt32(value) == match)
        //        {
        //            returnValue = true;
        //        }
        //        else
        //        {
        //            returnValue = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }

        //    return returnValue;
        //}

        //public CellSelection FormattedColumn(Worksheet worksheet, int row, int col, string value, DIColumn diColumn)
        //{
        //    CellSelection cellSelection = null;
        //    try
        //    {
        //        PatternFill redSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 0, 0), Colors.Transparent);
        //        PatternFill whiteSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 255, 255), Colors.Transparent);

        //        cellSelection = worksheet.Cells[row, col];

        //        if (diColumn.type == typeof(string))
        //        {
        //            worksheet.Cells[row, col].SetFill(IsExpected(value, diColumn.expected) ? redSolidFill : whiteSolidFill);
        //        }
        //        else if (diColumn.type == typeof(int))
        //        {
        //            if (diColumn.method == (int)CompareMethod.EqualToZero)
        //            {
        //                worksheet.Cells[row, col].SetFill(IsEqualTo(value, 0) ? redSolidFill : whiteSolidFill);
        //            }
        //            else if (diColumn.method == (int)CompareMethod.NotEqualToOne)
        //            {
        //                worksheet.Cells[row, col].SetFill(!IsEqualTo(value, 1) ? redSolidFill : whiteSolidFill);
        //            }
        //        }

        //        worksheet.Cells[row, col].SetValue(value);  

        //        if(diColumn.isCentered)
        //            worksheet.Cells[row, col].SetHorizontalAlignment(RadHorizontalAlignment.Center);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //    return cellSelection;
        //}

        //private void PrepareSheet1Worksheet(Worksheet worksheet, DIColumn[] columns)
        //{
        //    try
        //    {
        //        PatternFill yellowSolidFill = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromRgb(255, 216, 0), Colors.Transparent);

        //        foreach (DIColumn column in columns)
        //        {
        //            int columnKey = Array.IndexOf(columns, column);

        //            int row = IndexRowItemStart + 1;
        //            worksheet.Cells[row, columnKey].SetValue(column.expected);
        //            worksheet.Cells[row, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Center);
        //            worksheet.Cells[row, columnKey].SetIsBold(true);

        //            row++;
        //            worksheet.Cells[row, columnKey].SetFill(yellowSolidFill);
        //            worksheet.Cells[row, columnKey].SetValue(column.name.ToUpper());
        //            worksheet.Cells[row, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Center);
        //            worksheet.Cells[row, columnKey].SetIsBold(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}
    }
}