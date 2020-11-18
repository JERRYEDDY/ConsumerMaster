using System;
using System.IO;
using System.Data;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;

namespace ConsumerMaster
{
    public class AWCPayrollProcessingReport
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        private readonly HeaderColumns[] hdrColumns = new HeaderColumns[]
        {
            new HeaderColumns("Client_ID", false),
            new HeaderColumns("Client_Name", false),
            new HeaderColumns("Staff Name", false),
            new HeaderColumns("Num_of_IVR_Tranactions", false),
            new HeaderColumns("Num_of Mobile_Tranactions", false),
            new HeaderColumns("Num_of Portal_Tranactions", false),
            new HeaderColumns("Progress Notes Week 1?", true),
            new HeaderColumns("Progress Notes Week 2?", true),
            new HeaderColumns("Notes_PN             ", false),
            new HeaderColumns("Progress Notes Initials", true),
            new HeaderColumns("Notes_Evolv Submission and Payroll", false),
            new HeaderColumns("Submit initials", true),
            new HeaderColumns("Evolv Submit", true),
            new HeaderColumns("Evolv Approval", true),
            new HeaderColumns("Sent for Payroll", true),
            new HeaderColumns("Evolv Submit (original)", true),
            new HeaderColumns("Evolv Approval (orginal)", true),
            new HeaderColumns("Sent for Payroll (original)", true)
        };

        public static class Header
        {
            public const int Client_ID = 0;
            public const int Client_Name = 1;
            public const int Staff_Name = 2;
            public const int Num_of_IVR_Tranactions = 3;
            public const int Num_of_Mobile_Tranactions = 4;
            public const int Num_of_Portal_Tranactions = 5;
            public const int Progress_Notes_Week_1 = 6;
            public const int Progress_Notes_Week_2 = 7;
            public const int Notes_PN = 8;
            public const int Progress_Notes_Initials = 9;
            public const int Notes_Evolv_Submission_and_Payroll = 10;
            public const int Submit_initials = 11;
            public const int Evolv_Submit = 12;
            public const int Evolv_Approval = 13;
            public const int Sent_for_Payroll = 14;
            public const int Evolv_Submit_original = 15;
            public const int Evolv_Approval_orginal = 16;
            public const int Sent_for_Payroll_original = 17;
        }

        public Workbook CreateWorkbook(UploadedFile uploadedFile)
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];   //Data Entry Sheet
                Worksheet sheet2Worksheet = worksheets["Sheet2"];   //Dropdown data

                Utility util = new Utility();
                Stream input = uploadedFile.InputStream;

                DataTable dataTable = util.GetScheduledActualDataTableViaCSV(input);
                DataTable clients = dataTable.DefaultView.ToTable(true, "ID","Name", "StaffName");

                var groupByList = dataTable.AsEnumerable()
                .GroupBy(row => new
                {
                    ID = row["ID"],
                    Name = row["Name"],
                    StaffName = row["StaffName"],
                    ActivitySource = row["ActivitySource"]
                })
                .Select(grp => new
                {
                    ID = grp.Key.ID,
                    Name = grp.Key.Name,
                    StaffName = grp.Key.StaffName,
                    ActivitySource = grp.Key.ActivitySource,
                    Count = grp.Count()
                }).ToList();

                DataTable groupBy = ConvertToDataTable(groupByList);
                DataTable ivrGroup = groupBy.Select("[ActivitySource] = 'IVR'").CopyToDataTable();
                DataTable mobileGroup = groupBy.Select("[ActivitySource] = 'Mobile'").CopyToDataTable();
                DataTable portalGroup = groupBy.Select("[ActivitySource] = 'Portal'").CopyToDataTable();

                List<PayrollProcessClient> collection =
                    (from c in clients.AsEnumerable()
                     join i in ivrGroup.AsEnumerable() on new { ID = c.Field<string>("ID"), Name = c.Field<string>("Name"), StaffName = c.Field<string>("StaffName") } equals 
                     new { ID = i.Field<string>("ID"), Name = i.Field<string>("Name"), StaffName = i.Field<string>("StaffName") } into ivrData
                     join m in mobileGroup.AsEnumerable() on new { ID = c.Field<string>("ID"), Name = c.Field<string>("Name"), StaffName = c.Field<string>("StaffName") } equals
                     new { ID = m.Field<string>("ID"), Name = m.Field<string>("Name"), StaffName = m.Field<string>("StaffName") } into mobileData
                     join p in portalGroup.AsEnumerable() on new { ID = c.Field<string>("ID"), Name = c.Field<string>("Name"), StaffName = c.Field<string>("StaffName") } equals
                     new { ID = p.Field<string>("ID"), Name = p.Field<string>("Name"), StaffName = p.Field<string>("StaffName") } into portalData
                     from ivrRecord in ivrData.DefaultIfEmpty()
                     from mobileRecord in mobileData.DefaultIfEmpty()
                     from portalRecord in portalData.DefaultIfEmpty()
                     select new PayrollProcessClient
                     {
                         ID = c.Field<string>("ID"),
                         Name = c.Field<string>("Name"),
                         StaffName = c.Field<string>("StaffName"),
                         ICount = ivrRecord == null ? 0 : ivrRecord.Field<int>("Count"),
                         MCount = mobileRecord == null ? 0 : mobileRecord.Field<int>("Count"),
                         PCount = portalRecord == null ? 0 : portalRecord.Field<int>("Count")
                     }).ToList();


                DataTable join = ConvertToDataTable(collection);
                DataView dv = join.DefaultView;
                dv.Sort = "Name, StaffName";
                DataTable dTable= dv.ToTable();

                PrepareSheet1Worksheet(sheet1Worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow row in dTable.Rows)
                {
                    sheet1Worksheet.Cells[currentRow, Header.Client_ID].SetValue(row["ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, Header.Client_Name].SetValue(row["Name"].ToString());
                    sheet1Worksheet.Cells[currentRow, Header.Staff_Name].SetValue(row["StaffName"].ToString());

                    sheet1Worksheet.Cells[currentRow, Header.Num_of_IVR_Tranactions].SetValue(row["ICount"].ToString()); //Num_of_IVR_Tranactions
                    sheet1Worksheet.Cells[currentRow, Header.Num_of_Mobile_Tranactions].SetValue(row["MCount"].ToString());
                    sheet1Worksheet.Cells[currentRow, Header.Num_of_Portal_Tranactions].SetValue(row["PCount"].ToString());

                    CellIndex c1RuleCellIndex = new CellIndex(currentRow, Header.Progress_Notes_Week_1);
                    ListDataValidationRuleContext c1Context = new ListDataValidationRuleContext(sheet1Worksheet, c1RuleCellIndex);
                    c1Context.InCellDropdown = true;
                    c1Context.Argument1 = "Yes";
                    ListDataValidationRule c1Rule = new ListDataValidationRule(c1Context);
                    sheet1Worksheet.Cells[c1RuleCellIndex].SetDataValidationRule(c1Rule);
                    sheet1Worksheet.Cells[currentRow, Header.Progress_Notes_Week_1].SetValue(""); //Progress Notes Week1

                    CellIndex c2RuleCellIndex = new CellIndex(currentRow, Header.Progress_Notes_Week_2);
                    ListDataValidationRuleContext c2Context = new ListDataValidationRuleContext(sheet1Worksheet,c2RuleCellIndex);
                    c2Context.InCellDropdown = true;
                    c2Context.Argument1 = "Yes";
                    ListDataValidationRule c2Rule = new ListDataValidationRule(c2Context);
                    sheet1Worksheet.Cells[c2RuleCellIndex].SetDataValidationRule(c2Rule);
                    sheet1Worksheet.Cells[currentRow, Header.Progress_Notes_Week_2].SetValue(""); //Progress Notes Week2

                    CellIndex c3RuleCellIndex = new CellIndex(currentRow, Header.Evolv_Submit);
                    ListDataValidationRuleContext c3Context = new ListDataValidationRuleContext(sheet1Worksheet, c3RuleCellIndex);
                    c3Context.InCellDropdown = true;
                    c3Context.Argument1 = "Done";
                    ListDataValidationRule c3Rule = new ListDataValidationRule(c3Context);
                    sheet1Worksheet.Cells[c3RuleCellIndex].SetDataValidationRule(c3Rule);
                    sheet1Worksheet.Cells[currentRow, Header.Evolv_Submit].SetValue("");

                    CellIndex c4RuleCellIndex = new CellIndex(currentRow, Header.Evolv_Approval);
                    ListDataValidationRuleContext c4Context = new ListDataValidationRuleContext(sheet1Worksheet, c4RuleCellIndex);
                    c4Context.InCellDropdown = true;
                    c4Context.Argument1 = "Done";
                    ListDataValidationRule c4Rule = new ListDataValidationRule(c4Context);
                    sheet1Worksheet.Cells[c4RuleCellIndex].SetDataValidationRule(c4Rule);
                    sheet1Worksheet.Cells[currentRow, Header.Evolv_Approval].SetValue("");

                    CellIndex c5RuleCellIndex = new CellIndex(currentRow, Header.Sent_for_Payroll);
                    ListDataValidationRuleContext c5Context = new ListDataValidationRuleContext(sheet1Worksheet, c5RuleCellIndex);
                    c5Context.InCellDropdown = true;
                    c5Context.Argument1 = "Done";
                    ListDataValidationRule c5Rule = new ListDataValidationRule(c5Context);
                    sheet1Worksheet.Cells[c5RuleCellIndex].SetDataValidationRule(c5Rule);
                    sheet1Worksheet.Cells[currentRow, Header.Sent_for_Payroll].SetValue("");

                    CellIndex c6RuleCellIndex = new CellIndex(currentRow, Header.Evolv_Submit_original);
                    ListDataValidationRuleContext c6Context = new ListDataValidationRuleContext(sheet1Worksheet, c6RuleCellIndex);
                    c6Context.InCellDropdown = true;
                    c6Context.Argument1 = "Done";
                    ListDataValidationRule c6Rule = new ListDataValidationRule(c6Context);
                    sheet1Worksheet.Cells[c6RuleCellIndex].SetDataValidationRule(c6Rule);
                    sheet1Worksheet.Cells[currentRow, Header.Evolv_Submit_original].SetValue("");

                    CellIndex c7RuleCellIndex = new CellIndex(currentRow, Header.Evolv_Approval_orginal);
                    ListDataValidationRuleContext c7Context = new ListDataValidationRuleContext(sheet1Worksheet, c7RuleCellIndex);
                    c7Context.InCellDropdown = true;
                    c7Context.Argument1 = "Done";
                    ListDataValidationRule c7Rule = new ListDataValidationRule(c7Context);
                    sheet1Worksheet.Cells[c7RuleCellIndex].SetDataValidationRule(c7Rule);
                    sheet1Worksheet.Cells[currentRow, Header.Evolv_Approval_orginal].SetValue("");

                    CellIndex c8RuleCellIndex = new CellIndex(currentRow, Header.Sent_for_Payroll_original);
                    ListDataValidationRuleContext c8Context = new ListDataValidationRuleContext(sheet1Worksheet, c8RuleCellIndex);
                    c8Context.InCellDropdown = true;
                    c8Context.Argument1 = "Done";
                    ListDataValidationRule c8Rule = new ListDataValidationRule(c8Context);
                    sheet1Worksheet.Cells[c8RuleCellIndex].SetDataValidationRule(c8Rule);
                    sheet1Worksheet.Cells[currentRow, Header.Sent_for_Payroll_original].SetValue("");

                    currentRow++;
                }

                for (int i = 1; i < 18; i++)  //Start at 1 instead of 0
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

        private void PrepareSheet1Worksheet(Worksheet worksheet)
        {
            try
            {
                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 192, 192, 192), Colors.Transparent);

                worksheet.ViewState.FreezePanes(0, 3);

                int columnKey = 0;
                foreach (HeaderColumns header in hdrColumns)
                {
                    string columnName = header.Name;
                    bool iswrap = header.IsWrap;
                    worksheet.Cells[IndexRowItemStart, columnKey].SetIsWrapped(iswrap);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);

                    if (columnKey < 6)
                        worksheet.Cells[IndexRowItemStart, columnKey].SetFill(solidPatternFill);

                    worksheet.Cells[IndexRowItemStart, columnKey++].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public class HeaderColumns 
        {
            public string Name { get; set; }
            public bool IsWrap { get; set; }

            public HeaderColumns(string name, bool iswrap )
            {
                Name = name;
                IsWrap = iswrap;
            }

        }

        public class PayrollProcessClient
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string StaffName { get; set; }
            public int ICount { get; set; }
            public int MCount { get; set; }
            public int PCount { get; set; }
        }
    }
}