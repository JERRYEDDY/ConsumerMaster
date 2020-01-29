using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.Windows.Media;
using Telerik.Web.UI;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using System.IO;
using System.Linq;
using System.Globalization;

namespace ConsumerMaster
{
    public class AWCPayrollReportExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;
        //private static readonly int IndexColumnName = 0;

        SPColumn[] spc = new SPColumn[20]
{
                new SPColumn("Staff ID", typeof(string)),
                new SPColumn("Secondary Staff ID", typeof(string)),
                new SPColumn("Staff Name", typeof(string) ),
                new SPColumn("Activity ID", typeof(string)),
                new SPColumn("Activity Type", typeof(string)),
                new SPColumn("ID", typeof(string)),
                new SPColumn("Secondary ID", typeof(string)),
                new SPColumn("Name", typeof(string)),
                new SPColumn("Start", typeof(DateTime)),
                new SPColumn("Finish", typeof(DateTime)),
                new SPColumn("Duration", typeof(Int32)),
                new SPColumn("Travel Time", typeof(string)),
                new SPColumn("TSrc", typeof(string)),
                new SPColumn("Distance", typeof(string)),
                new SPColumn("DSrc", typeof(string)),
                new SPColumn("Phone", typeof(string)),
                new SPColumn("Service", typeof(string)),
                new SPColumn("On-call", typeof(string)),
                new SPColumn("Location", typeof(string)),
                new SPColumn("Discipline", typeof(string))
        };
        public Workbook CreateWorkbook()
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                worksheets.Add();
                worksheets.Add();
                //worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];   //40 Hours per week Report
                Worksheet sheet2Worksheet = worksheets["Sheet2"];   //29 Hours per week Report
                Worksheet sheet3Worksheet = worksheets["Sheet3"];   //Overlap Shifts Report
/*                Worksheet sheet4Worksheet = worksheets["Sheet4"]; */  //

                Utility util = new Utility();

                //DataTable tppDataTable = util.GetDataTable("SELECT symbol, billing_note FROM TradingPartnerPrograms");
                //int tppCount = tppDataTable.Rows.Count;
                //CreateSheet2Worksheet(sheet2Worksheet, tppDataTable);

                //Early Intervention Direct Therapy; In Home = 7 or Early Intervention Special Instruction; In Home = 8 
                //DataTable cpcDataTable = util.GetDataTable("SELECT name, taxonomy_code FROM CompositeProcedureCodes WHERE trading_partner_id = " + tradingPartnerId);
                //int cpcCount = cpcDataTable.Rows.Count;
                //CreateSheet3Worksheet(sheet3Worksheet, cpcDataTable);

                //DataTable rnDataTable = util.GetDataTable("SELECT name, npi_number, ma_number, first_name, last_name FROM RenderingProviders WHERE npi_number IS NOT NULL ORDER BY last_name");
                //int rnCount = rnDataTable.Rows.Count;
                //CreateSheet4Worksheet(sheet4Worksheet, rnDataTable);
                int rnCount = 0;


                Payroll40HoursReportFormat sef = new Payroll40HoursReportFormat();

                //string selectQuery =
                //$@"
                //    SELECT 
                //        c.consumer_first AS consumer_first, c.consumer_last AS consumer_last, c.consumer_internal_number AS consumer_internal_number
                //        ,tp.symbol AS trading_partner_string, ' ' AS trading_partner_program_string, ' ' AS start_date_string, ' ' AS end_date_string
                //        ,c.diagnosis AS diagnosis_code_1_code, ' ' AS composite_procedure_code_string, ' ' AS units, ' ' AS manual_billable_rate
                //        ,' ' AS prior_authorization_number,' ' AS referral_number, c.referring_provider_id AS referring_id, rp.npi_number AS referring_provider_id
                //        ,rp.first_name AS referring_provider_first_name, rp.last_name AS referring_provider_last_name,' ' AS rendering_names, ' ' AS rendering_provider_id
                //        ,' ' AS rendering_provider_secondary_id, ' ' AS rendering_provider_first_name,' ' AS rendering_provider_last_name, ' ' AS rendering_provider_taxonomy_code
                //        ,' ' AS billing_note 
                //    FROM 
                //        Consumers AS c 
                //    INNER JOIN 
                //        TradingPartners AS tp ON {tradingPartnerId} = tp.id
                //    LEFT JOIN 
                //        ReferringProviders rp ON rp.id = c.referring_provider_id 
                //    WHERE 
                //        c.trading_partner_id1 = {tradingPartnerId} OR c.trading_partner_id2 = {tradingPartnerId} OR c.trading_partner_id3 = {tradingPartnerId}
                //    ORDER BY consumer_last
                //";

                //DataTable seDataTable = util.GetDataTable(selectQuery);
                //int totalConsumers = seDataTable.Rows.Count;


                string spreadsheetFilename = "TimeDistance_20200105_20200111.xlsx";
                //string spreadsheetFilename = "TimeDistance_20200112_20200118.xlsx";

                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(File.ReadAllBytes(@"C:\NetSmart\Reports Written\" + spreadsheetFilename));

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;
                DataTable inputDataTable = new DataTable();

                for (int i = 0; i < spc.Count(); i++)
                {
                    CellSelection selection = InputWorksheet.Cells[0, i];
                    var columnName = "Column" + (i + 1);
                    inputDataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[spc.Count()];
                    values[0] = GetCellData(InputWorksheet, i, 0); //Staff ID
                    values[1] = GetCellData(InputWorksheet, i, 1); //Secondary Staff ID
                    values[2] = GetCellData(InputWorksheet, i, 2); //Activity ID
                    values[3] = GetCellData(InputWorksheet, i, 3); //Secondary Staff ID
                    values[4] = GetCellData(InputWorksheet, i, 4); //Activity Type
                    values[5] = GetCellData(InputWorksheet, i, 5); //ID
                    values[6] = GetCellData(InputWorksheet, i, 6); //Secondary ID"

                    string name = GetCellData(InputWorksheet, i, 7);
                    values[7] = name.Replace("\"", ""); //Name

                    string combinedStart = GetCellData(InputWorksheet, i, 8) + " " + GetCellData(InputWorksheet, i, 9);
                    DateTime startDate = Convert.ToDateTime(combinedStart);
                    values[8] = startDate; //Start

                    string combinedFinish = GetCellData(InputWorksheet, i, 11) + " " + GetCellData(InputWorksheet, i, 12);
                    DateTime finishDate = Convert.ToDateTime(combinedFinish);
                    values[9] = finishDate; //Finish

                    string durationStr = GetCellData(InputWorksheet, i, 14);
                    values[10] = int.Parse(durationStr, NumberStyles.AllowThousands);  //Duration

                    values[11] = GetCellData(InputWorksheet, i, 15); //Travel Time
                    values[12] = GetCellData(InputWorksheet, i, 16); //TSrc
                    values[13] = GetCellData(InputWorksheet, i, 17); //Distance
                    values[14] = GetCellData(InputWorksheet, i, 18); //DSrc
                    values[15] = GetCellData(InputWorksheet, i, 19); //Phone
                    values[16] = GetCellData(InputWorksheet, i, 20); //Service
                    values[17] = GetCellData(InputWorksheet, i, 21); //On-call
                    values[18] = GetCellData(InputWorksheet, i, 22); //Location
                    values[19] = GetCellData(InputWorksheet, i, 23); //Discipline"

                    inputDataTable.Rows.Add(values);
                }

                PrepareSheet1Worksheet(sheet1Worksheet);


                // 40 hour per week limit – show anyone who worked over 40 hours 
                var query1 = from row in inputDataTable.AsEnumerable()
                             group row by new
                             {
                                 StaffID = row.Field<string>("Staff ID"),
                                 StaffName = row.Field<string>("Staff Name")
                             }
                into TD
                             where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 40.00
                             orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
                             select new
                             {
                                 ID = TD.Key.StaffID,
                                 Name = TD.Key.StaffName,
                                 Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
                             };

                
                //TextWriter tw1 = new StreamWriter(Server.MapPath("40 Hours Report.txt"));
                //tw1.WriteLine("40 hours per week limit – show anyone who worked over 40 hours");
                //tw1.WriteLine("Filename:{0}", spreadsheetFilename);
                //tw1.WriteLine("ID\tName\t\t\t\t\tHours");
                //// print result
                //foreach (var staff in query1)
                //{
                //    string name = staff.Name.Replace("\t", "");
                //    tw1.WriteLine("{0}\t{1}\t\t\t\t{2:0.00}", staff.ID, name.Trim(), staff.Hours);
                //}
                //tw1.Close();


                int currentRow = IndexRowItemStart + 1;
                foreach (var staff in query1)
                {
                    string name = staff.Name.Replace("\t", "");
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("ID")].SetValue(staff.ID);
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("Name")].SetValue(name.Trim());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("Hours")].SetValue(staff.Hours);

                    currentRow++;
                }

                for (int i = 0; i < inputDataTable.Columns.Count; i++)
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

        string GetCellData(Worksheet worksheet, int i, int j)
        {
            CellSelection selection = worksheet.Cells[i, j];

            ICellValue value = selection.GetValue().Value;
            CellValueFormat format = selection.GetFormat().Value;
            CellValueFormatResult formatResult = format.GetFormatResult(value);
            string result = formatResult.InfosText;
            return result;
        }

        private void PrepareSheet1Worksheet(Worksheet worksheet)
        {
            try
            {
                Payroll40HoursReportFormat prf = new Payroll40HoursReportFormat();
                string[] columnsList = prf.ColumnStrings;

                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 0, 0), Colors.Transparent);
                PatternFill goldPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 215, 0), Colors.Transparent);

                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column);
                    string columnName = column;

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                    //if (columnName.Equals("consumer_first") || columnName.Equals("consumer_last") || columnName.Equals("rendering_names"))
                    //{
                    //    worksheet.Cells[IndexRowItemStart, columnKey].SetFill(solidPatternFill);
                    //}

                    //if (columnName.Equals("billing_note") ||
                    //    columnName.Equals("rendering_provider_taxonomy_code") ||
                    //    columnName.Equals("rendering_provider_id") ||
                    //    columnName.Equals("rendering_provider_first_name") ||
                    //    columnName.Equals("rendering_provider_last_name"))
                    //{
                    //    worksheet.Cells[IndexRowItemStart, columnKey].SetFill(goldPatternFill);
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void PrepareSheet2Worksheet(Worksheet worksheet)
        {
            try
            {
                string[] columnsList = { "symbol", "billing_note" };
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

        private void CreateSheet2Worksheet(Worksheet worksheet, DataTable dTable)
        {
            try
            {
                PrepareSheet2Worksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in dTable.Rows)
                {
                    worksheet.Cells[currentRow, 0].SetValue(dr["symbol"].ToString());
                    worksheet.Cells[currentRow, 1].SetValue(dr["billing_note"].ToString());
                    currentRow++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void PrepareSheet3Worksheet(Worksheet worksheet)
        {
            try
            {
                string[] columnsList = { "name", "taxonomy_code" };
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

        private void CreateSheet3Worksheet(Worksheet worksheet, DataTable dTable)
        {
            try
            {
                PrepareSheet3Worksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in dTable.Rows)
                {
                    worksheet.Cells[currentRow, 0].SetValue(dr["name"].ToString());
                    worksheet.Cells[currentRow, 1].SetValue(dr["taxonomy_code"].ToString());
                    currentRow++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        //private void PrepareSheet4Worksheet(Worksheet worksheet)
        //{
        //    try
        //    {
        //        string[] columnsList = { "name", "npi_number", "ma_number", "first_name", "last_name" };
        //        foreach (string column in columnsList)
        //        {
        //            int columnKey = Array.IndexOf(columnsList, column);
        //            string columnName = column;

        //            worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
        //            worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}

        //private void CreateSheet4Worksheet(Worksheet worksheet, DataTable dTable)
        //{
        //    try
        //    {
        //        PrepareSheet4Worksheet(worksheet);

        //        int currentRow = IndexRowItemStart + 1;
        //        foreach (DataRow dr in dTable.Rows)
        //        {
        //            worksheet.Cells[currentRow, 0].SetValue(dr["name"].ToString());
        //            worksheet.Cells[currentRow, 1].SetValue(dr["npi_number"].ToString());
        //            worksheet.Cells[currentRow, 2].SetValue(dr["ma_number"].ToString());
        //            worksheet.Cells[currentRow, 3].SetValue(dr["first_name"].ToString());
        //            worksheet.Cells[currentRow, 4].SetValue(dr["last_name"].ToString());
        //            currentRow++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}
    }
}