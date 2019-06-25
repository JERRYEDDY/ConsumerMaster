using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using System.Windows.Media;

namespace ConsumerMaster
{
    public class ResidentialServiceExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly int IndexRowItemStart = 0;
        private static readonly int IndexColumnName = 0;

        public Workbook CreateWorkbook(string residentialList)
        {
            Workbook workbook = new Workbook();
            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];
                Worksheet sheet2Worksheet = worksheets["Sheet2"];

                Utility util = new Utility();
                List<string> cpcList = util.GetList("SELECT name FROM CompositeProcedureCodes WHERE trading_partner_id = 5");  //Agency With Choice = 5
                CreateCompositeProcedureCodesWorksheet(sheet2Worksheet, cpcList);
                ServiceExportFormat sef = new ServiceExportFormat(true);

                string selectQuery = 
                $@"
                    SELECT 
                        c.consumer_first AS consumer_first, c.consumer_last AS consumer_last, c.consumer_internal_number AS consumer_internal_number
                        ,tp.symbol AS trading_partner_string, 'waiver' AS trading_partner_program_string, ' ' AS start_date_string, ' ' AS end_date_string
                        ,c.diagnosis AS diagnosis_code_1_code, ' ' AS composite_procedure_code_string, ' ' AS hours, ' ' AS units, ' ' AS manual_billable_rate
                        ,' ' AS prior_authorization_number,' ' AS referral_number, ' ' AS referring_provider_id, ' ' AS referring_provider_first_name
                        ,' ' AS referring_provider_last_name,' ' AS rendering_provider_id, ' ' AS rendering_provider_first_name, ' ' AS rendering_provider_last_name 
                    FROM 
                        Consumers AS c 
                    INNER JOIN 
                        TradingPartners AS tp ON c.trading_partner_id1 = tp.id 
                    WHERE 
                        c.trading_partner_id1 = {residentialList} OR 
                        c.trading_partner_id2 = {residentialList} OR 
                        c.trading_partner_id3 = {residentialList} 
                    ORDER BY consumer_last
                ";

                DataTable seDataTable = util.GetDataTable(selectQuery);
                int totalConsumers = seDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in seDataTable.Rows)
                {
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("consumer_first")].SetValue(dr["consumer_first"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("consumer_last")].SetValue(dr["consumer_last"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("consumer_internal_number")].SetValue(dr["consumer_internal_number"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("diagnosis_code_1_code")].SetValue(dr["diagnosis_code_1_code"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("trading_partner_string")].SetValue(dr["trading_partner_string"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("trading_partner_program_string")].SetValue(dr["trading_partner_program_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("start_date_string")].SetValue(dr["start_date_string"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("end_date_string")].SetValue(dr["end_date_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("composite_procedure_code_string")].SetValue(dr["composite_procedure_code_string"].ToString());
                    CellIndex dataValidationRuleCellIndex = new CellIndex(currentRow, sef.GetIndex("composite_procedure_code_string"));
                    ListDataValidationRuleContext context = new ListDataValidationRuleContext(sheet1Worksheet, dataValidationRuleCellIndex)
                    {
                        InputMessageTitle = "Restricted input",
                        InputMessageContent = "The input is restricted to the composite procedure codes.",
                        ErrorStyle = ErrorStyle.Stop,
                        ErrorAlertTitle = "Wrong value",
                        ErrorAlertContent = "The entered value is not valid. Allowed values are the composite procedure codes!",
                        InCellDropdown = true
                    };

                    string cpcRange = "=Sheet2!$A$2:$A$" + cpcList.Count + 1;  //= Sheet2!$A$2:$A$73
                    context.Argument1 = cpcRange;    
                    ListDataValidationRule rule = new ListDataValidationRule(context);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex].SetDataValidationRule(rule);

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("hours")].SetValue(0); //Set to zero

                    string convertToUnits = "=ROUNDDOWN(J" + (currentRow + 1) + "*4, 0)";
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("units")].SetValue(convertToUnits);

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("manual_billable_rate")].SetValue(dr["manual_billable_rate"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("prior_authorization_number")].SetValue(dr["prior_authorization_number"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referral_number")].SetValue(dr["referral_number"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_id")].SetValue(dr["referring_provider_id"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_first_name")].SetValue(dr["referring_provider_first_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_last_name")].SetValue(dr["referring_provider_last_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_id")].SetValue(dr["rendering_provider_id"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_first_name")].SetValue(dr["rendering_provider_first_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_last_name")].SetValue(dr["rendering_provider_last_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("billing_note")].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_secondary_id")].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_taxonomy_code")].SetValue(" ");

                    currentRow++;
                }

                for (int i = 0; i < seDataTable.Columns.Count; i++)
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

        private void CreateCompositeProcedureCodesWorksheet(Worksheet worksheet, List<string> cpcList)
        {
            try
            {
                PrepareSheet2Worksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (String cpCode in cpcList)
                {
                    worksheet.Cells[currentRow, IndexColumnName].SetValue(cpCode);
                    currentRow++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void PrepareSheet1Worksheet(Worksheet worksheet)
        {
            try
            {
                ServiceExportFormat sef = new ServiceExportFormat(true);
                string[] columnsList = sef.ColumnStrings;

                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255,255,0,0), Colors.Transparent);

                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column);
                    string columnName = column;

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                    if (columnName.Equals("consumer_first") || columnName.Equals("consumer_last"))
                    {
                        worksheet.Cells[IndexRowItemStart, columnKey].SetFill(solidPatternFill);
                    }
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
                worksheet.Cells[IndexRowItemStart, IndexColumnName].SetValue("composite_procedure_code");
                worksheet.Cells[IndexRowItemStart, IndexColumnName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}

