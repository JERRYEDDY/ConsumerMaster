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

        private readonly SortedList<string, bool> headerColumns = new SortedList<string, bool>()
        {
            {"Client_ID", false },
            {"Client_Name", false },
            {"Staff Name", false },
            {"Num_of_IVR_Tranactions", false },
            {"Num_of Mobile_Tranactions", false },
            {"Num_of Portal_Tranactions", false },
            {"Progress Notes Week 1?", true },
            {"Progress Notes Week 2?", true },
            {"Notes_PN", false },
            {"Progress Notes Initials", true },
            {"Notes_Evolv Submission and Payroll", false },
            {"Submit initials", true },
            {"Evolv Submit", true },
            {"Evolv Approval", true },
            {"Sent for Payroll", true },
            {"Evolv Submit (original)", true },
            {"Evolv Approval (orginal)", true },
            {"Sent for Payroll (original)", true },
        };

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
                    int column = 0;

                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Name"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["StaffName"].ToString());

                    //CellValueFormat dateCellValueFormat = new CellValueFormat("MM/dd/yyyy hh:mm AM/PM");
                    //sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                    //sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Start"].ToString());

                    //sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                    //sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Finish"].ToString());

                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["ICount"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["MCount"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["PCount"].ToString());

                    CellIndex week1RuleCellIndex = new CellIndex(currentRow, column);
                    ListDataValidationRuleContext week1Context = new ListDataValidationRuleContext(sheet1Worksheet, week1RuleCellIndex);
                    week1Context.InCellDropdown = true;
                    week1Context.Argument1 = "Yes";
                    ListDataValidationRule week1Rule = new ListDataValidationRule(week1Context);
                    sheet1Worksheet.Cells[week1RuleCellIndex].SetDataValidationRule(week1Rule);

                    int currentColumn = headerColumns.IndexOfKey("Progress Notes Week 1?");
                    


                    sheet1Worksheet.Cells[currentRow, column++].SetValue(""); //Progress Notes Week1


                    sheet1Worksheet.Cells[currentRow, column++].SetValue(""); //Progress Notes Week2

                    //ThemableColor textColor = new ThemableColor(Colors.Red);
                    //sheet1Worksheet.Cells[currentRow, column].SetForeColor(textColor);
                    //sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Exception"].ToString());

                    //sheet1Worksheet.Cells[currentRow, column++].SetValue(row["NS Billing Auth"].ToString());

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
                foreach (KeyValuePair<string, bool> header in headerColumns)
                {
                    string columnName = header.Key;
                    bool colValue = header.Value;
                    worksheet.Cells[IndexRowItemStart, columnKey].SetIsWrapped(colValue);
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