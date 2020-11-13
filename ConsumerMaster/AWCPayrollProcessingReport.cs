using System;
using System.IO;
using System.Data;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Windows.Media;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

namespace ConsumerMaster
{
    public class AWCPayrollProcessingReport
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

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

                DataTable dTable = util.GetScheduledActualDataTableViaCSV(input);

                DataTable clientsGroup = dTable.DefaultView.ToTable(true, "ID","Name", "StaffName");

                DataTable ivrResults = dTable.Select("[ActivitySource] = 'IVR'").CopyToDataTable();
                var ivrList = ivrResults.AsEnumerable()
                    .GroupBy(row => new { ID = row["ID"], Name = row["Name"], StaffName = row["StaffName"] })
                    .Select(grp => new { ID = grp.Key.ID, Name = grp.Key.Name, StaffName = grp.Key.StaffName, Count = grp.Count() }).ToList();
                DataTable ivrGroup = ConvertToDataTable(ivrList);

                DataTable mobileResults = dTable.Select("[ActivitySource] = 'Mobile'").CopyToDataTable();
                var mobileList = mobileResults.AsEnumerable()
                    .GroupBy(row => new { ID = row["ID"], Name = row["Name"], StaffName = row["StaffName"] })
                    .Select(grp => new { ID = grp.Key.ID, Name = grp.Key.Name, StaffName = grp.Key.StaffName, Count = grp.Count() }).ToList();
                DataTable mobileGroup = ConvertToDataTable(mobileList);

                DataTable portalResults = dTable.Select("[ActivitySource] = 'Portal'").CopyToDataTable();
                var portalList = portalResults.AsEnumerable()
                    .GroupBy(row => new { ID = row["ID"], Name = row["Name"], StaffName = row["StaffName"] })
                    .Select(grp => new { ID = grp.Key.ID, Name = grp.Key.Name, StaffName = grp.Key.StaffName, Count = grp.Count() }).ToList();
                DataTable portalGroup = ConvertToDataTable(portalList);

                List<PayrollProcessClient> collection =
                    (from c in clientsGroup.AsEnumerable()
                     join i in ivrGroup.AsEnumerable() on new { ID = c.Field<string>("ID"), Name = c.Field<string>("Name"), StaffName = c.Field<string>("StaffName") } equals 
                     new { ID = i.Field<string>("ID"), Name = i.Field<string>("Name"), StaffName = i.Field<string>("StaffName") } into ivrData
                     join m in mobileGroup.AsEnumerable() on new { ID = c.Field<string>("ID"), Name = c.Field<string>("Name"), StaffName = c.Field<string>("StaffName") } equals
                     new { ID = m.Field<string>("ID"), Name = m.Field<string>("Name"), StaffName = m.Field<string>("StaffName") } into mobileData
                     from ivrRecord in ivrData.DefaultIfEmpty()
                     from mobileRecord in mobileData.DefaultIfEmpty()
                     select new PayrollProcessClient
                     {
                         ID = c.Field<string>("ID"),
                         Name = c.Field<string>("Name"),
                         StaffName = c.Field<string>("StaffName"),
                         ICount = ivrRecord.Field<int>("Count"),
                         MCount = mobileRecord.Field<int>("Count")
                     }).ToList();






                //var query =
                //    from clients in clientsGroup.AsEnumerable()
                //    join ivr in ivrGroup.AsEnumerable() on new {ID = clients.Field<string>("ID"), Name = clients.Field<string>("Name"), StaffName = clients.Field<string>("StaffName")} equals
                //    new { ID = ivr.Field<string>("ID"), Name = ivr.Field<string>("Name"), StaffName = ivr.Field<string>("StaffName") } into ivrData
                //    join mobile in mobileGroup.AsEnumerable() on new { ID = clients.Field<string>("ID"), Name = clients.Field<string>("Name"), StaffName = clients.Field<string>("StaffName") } equals
                //    new { ID = mobile.Field<string>("ID"), Name = mobile.Field<string>("Name"), StaffName = mobile.Field<string>("StaffName") } into mobileData
                //    from ivrRecord in ivrData.DefaultIfEmpty()
                //    from mobileRecord in mobileData.DefaultIfEmpty()
                //    select new
                //    {
                //        ID = clients.Field<string>("ID"),
                //        Name = ivrRecord.Field<string>("Name"),
                //        StaffName = ivrRecord.Field<string>("StaffName"),
                //        ICount = ivrRecord == null ? 0 : ivrRecord.Field<int>("Count"),
                //        MCount = mobileRecord == null ? 0 : mobileRecord.Field<int>("Count")
                //    };


                //var joins = query.ToList();



                //var query = (from x in clientResults.AsEnumerable()
                //             join y in ivrGroup.AsEnumerable() on x.Field<string>("ID") equals y.Field<string>("ID") into tempJoin
                //             from leftJoin in tempJoin.DefaultIfEmpty()
                //             select new { col1 = x.Field<string>("ID"), col2 = y.Field<string>("Name"), col3 = y.Field<string>("StaffName"), col4 = y.Field<int>("Count") }).ToList();



                PrepareSheet1Worksheet(sheet1Worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow row in dTable.Rows)
                {
                    int column = 0;

                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Name"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Activity ID"].ToString());

                    CellValueFormat dateCellValueFormat = new CellValueFormat("MM/dd/yyyy hh:mm AM/PM");
                    sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Start"].ToString());

                    sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Finish"].ToString());

                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Duration"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["CT Billing Code"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["CT Payroll Code"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Service"].ToString());

                    ThemableColor textColor = new ThemableColor(Colors.Red);
                    sheet1Worksheet.Cells[currentRow, column].SetForeColor(textColor);
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Exception"].ToString());

                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["NS Billing Auth"].ToString());


                    currentRow++;
                }

                for (int i = 1; i < dTable.Columns.Count; i++)  //Start at 1 instead of 0
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
            string[] columnsList = new string[]
            {
                    "Client_ID",
                    "Client_Name",
                    "Staff Name",
                    "IVR_Count",
                    "Mobile_Count",
                    "Portal_Count",
                    "Progress Notes Week 1?",
                    "Progress Notes WeeK 2?",
                    "Notes - PN",
                    "Progress Notes Initials",
                    "Notes - Evolv Submission and Payroll",
                    "Submit initials",
                    "Evolv Submit",
                    "Evolv Approval",
                    "Sent for Payroll",
                    "Evolv Submit (original)",
                    "Evolv Approval (orginal)",
                    "Sent for Payroll (original)",
            };

            try
            {
                PatternFill redPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(120, 255, 0, 0), Colors.Transparent);
                PatternFill goldPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 215, 0), Colors.Transparent);

                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column);
                    string columnName = column;

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
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