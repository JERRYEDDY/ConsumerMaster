using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.Text;
using System.Windows.Media;

using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;

namespace ConsumerMaster
{
    public class EISIServiceExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;
        private static readonly int IndexColumnName = 0;

        public Workbook CreateWorkbook(string tradingPartnerId)
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                worksheets.Add();
                worksheets.Add();
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];   //Data Entry Sheet
                Worksheet sheet2Worksheet = worksheets["Sheet2"];   //Trading Partner Programs/Billing Note
                Worksheet sheet3Worksheet = worksheets["Sheet3"];   //Composite Procedure Codes
                Worksheet sheet4Worksheet = worksheets["Sheet4"];   //Rendering Providers

                Utility util = new Utility();

                DataTable tppDataTable = util.GetDataTable("SELECT symbol, billing_note FROM TradingPartnerPrograms");
                int tppCount = tppDataTable.Rows.Count;
                CreateSheet2Worksheet(sheet2Worksheet, tppDataTable);

                //Early Intervention Direct Therapy; In Home = 7 or Early Intervention Special Instruction; In Home = 8 
                List<string> cpcList = util.GetList("SELECT name FROM CompositeProcedureCodes WHERE trading_partner_id = " + tradingPartnerId);
                CreateDropDownListWorksheet(sheet3Worksheet, cpcList, "composite_procedure_code");

                DataTable rnDataTable = util.GetDataTable("SELECT name, npi_number, ma_number, first_name, last_name FROM RenderingProviders WHERE npi_number IS NULL ORDER BY last_name");
                int rnCount = rnDataTable.Rows.Count;
                CreateSheet4Worksheet(sheet4Worksheet, rnDataTable);

                EISIServiceExportFormat sef = new EISIServiceExportFormat();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT c.consumer_first AS consumer_first, c.consumer_last AS consumer_last, c.consumer_internal_number AS consumer_internal_number, ");
                queryBuilder.Append("tp.symbol AS trading_partner_string, ' ' AS trading_partner_program_string, ' ' AS start_date_string, ' ' AS end_date_string, ");
                queryBuilder.Append("c.diagnosis AS diagnosis_code_1_code, ' ' AS composite_procedure_code_string, ' ' AS units, ' ' AS manual_billable_rate, ' ' AS prior_authorization_number, ");
                queryBuilder.Append("' ' AS referral_number, ' ' AS referring_provider_id, ' ' AS referring_provider_first_name, ' ' AS referring_provider_last_name, ");
                queryBuilder.Append("' ' AS rendering_names, ' ' AS rendering_provider_id, ' ' AS rendering_provider_secondary_id, ' ' AS rendering_provider_first_name, ");
                queryBuilder.Append("' ' AS rendering_provider_last_name, ' ' AS rendering_provider_taxonomy_code, ' ' AS billing_note FROM Consumers AS c ");
                queryBuilder.AppendFormat("INNER JOIN TradingPartners AS tp ON {0} = tp.id ", tradingPartnerId);
                queryBuilder.AppendFormat("WHERE c.trading_partner_id1 = {0} ", tradingPartnerId);
                queryBuilder.AppendFormat("OR c.trading_partner_id2 = {0} ", tradingPartnerId);
                queryBuilder.AppendFormat("OR c.trading_partner_id3 = {0} ", tradingPartnerId);
                queryBuilder.Append("ORDER BY consumer_last");
                string seQuery = queryBuilder.ToString();

                DataTable seDataTable = util.GetDataTable(seQuery);
                int totalConsumers = seDataTable.Rows.Count;

                PrepareSheet1Worksheet(sheet1Worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in seDataTable.Rows)
                {
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("consumer_first")].SetValue(dr["consumer_first"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("consumer_last")].SetValue(dr["consumer_last"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("consumer_internal_number")].SetValue(dr["consumer_internal_number"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("trading_partner_string")].SetValue(dr["trading_partner_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("trading_partner_program_string")].SetValue(dr["trading_partner_program_string"].ToString());
                    CellIndex dataValidationRuleCellIndex2 = new CellIndex(currentRow, sef.GetIndex("trading_partner_program_string"));
                    ListDataValidationRuleContext context2 = new ListDataValidationRuleContext(sheet1Worksheet, dataValidationRuleCellIndex2)
                    {
                        InputMessageTitle = "Restricted input",
                        InputMessageContent = "The input is restricted to the composite procedure codes.",
                        ErrorStyle = ErrorStyle.Stop,
                        ErrorAlertTitle = "Wrong value",
                        ErrorAlertContent = "The entered value is not valid. Allowed values are the composite procedure codes!",
                        InCellDropdown = true
                    };
                    string listRange2 = "=Sheet2!$A$2:$A$" + tppCount + 1;  //= Sheet2!$A$2:$A$
                    context2.Argument1 = listRange2; //   
                    ListDataValidationRule rule2 = new ListDataValidationRule(context2);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex2].SetDataValidationRule(rule2);

                    int rowNumber = currentRow + 1;
                    string billingNoteLookup = "=VLOOKUP(F" + rowNumber  + ",Sheet2!A2:B18,2,FALSE)";
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("billing_note")].SetValue(billingNoteLookup);            //"billing_note"

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("start_date_string")].SetValue(dr["start_date_string"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("end_date_string")].SetValue(dr["end_date_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("diagnosis_code_1_code")].SetValue(dr["diagnosis_code_1_code"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("composite_procedure_code_string")].SetValue(dr["composite_procedure_code_string"].ToString());
                    CellIndex dataValidationRuleCellIndex3 = new CellIndex(currentRow, sef.GetIndex("composite_procedure_code_string"));
                    ListDataValidationRuleContext context3 = new ListDataValidationRuleContext(sheet1Worksheet, dataValidationRuleCellIndex3)
                    {
                        InputMessageTitle = "Restricted input",
                        InputMessageContent = "The input is restricted to the composite procedure codes.",
                        ErrorStyle = ErrorStyle.Stop,
                        ErrorAlertTitle = "Wrong value",
                        ErrorAlertContent = "The entered value is not valid. Allowed values are the composite procedure codes!",
                        InCellDropdown = true
                    };
                    string listRange3 = "=Sheet3!$A$2:$A$" + cpcList.Count + 1;  //= Sheet3!$A$2:$A$
                    context3.Argument1 = listRange3; //   
                    ListDataValidationRule rule3 = new ListDataValidationRule(context3);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex3].SetDataValidationRule(rule3);

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("units")].SetValue(dr["units"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("manual_billable_rate")].SetValue(dr["manual_billable_rate"].ToString());                            //"manual_billable_rate"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("prior_authorization_number")].SetValue(dr["prior_authorization_number"].ToString());                //"prior_authorization_number"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referral_number")].SetValue(dr["referral_number"].ToString());                                      //"referral_number"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_id")].SetValue(dr["referring_provider_id"].ToString());                          //"referring_provider_id"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_first_name")].SetValue(dr["referring_provider_first_name"].ToString());          //"referring_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_last_name")].SetValue(dr["referring_provider_last_name"].ToString());            //"referring_provider_last_name"

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_names")].SetValue(dr["rendering_names"].ToString());                                      //"rendering_names"
                    CellIndex dataValidationRuleCellIndex4 = new CellIndex(currentRow, sef.GetIndex("rendering_names"));
                    ListDataValidationRuleContext context4 = new ListDataValidationRuleContext(sheet1Worksheet, dataValidationRuleCellIndex4)
                    {
                        InputMessageTitle = "Restricted input",
                        InputMessageContent = "The input is restricted to the composite procedure codes.",
                        ErrorStyle = ErrorStyle.Stop,
                        ErrorAlertTitle = "Wrong value",
                        ErrorAlertContent = "The entered value is not valid. Allowed values are the composite procedure codes!",
                        InCellDropdown = true
                    };
                    string listRange4 = "=Sheet4!$A$2:$A$" + rnCount + 1;  //= Sheet4!$A$2:$A$
                    context4.Argument1 = listRange4; //   
                    ListDataValidationRule rule4 = new ListDataValidationRule(context4);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex4].SetDataValidationRule(rule4);

                    Lookup lu = new Lookup(rowNumber, "Sheet4", rnCount);
                    string ex = lu.Append(3);

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_id")].SetValue(dr["rendering_provider_id"].ToString());   //"rendering_provider_id"
                    CellValueFormat maNumberCellValueFormat = new CellValueFormat("0000000000000");

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_secondary_id")].SetFormat(maNumberCellValueFormat);

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_secondary_id")].SetValue(lu.Append(3));   //"rendering_provider_secondary_id"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_first_name")].SetValue(lu.Append(4));     //"rendering_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_last_name")].SetValue(lu.Append(5));      //"rendering_provider_last_name"

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_taxonomy_code")].SetValue(dr["rendering_provider_taxonomy_code"].ToString());    //"rendering_provider_taxonomy_code"

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

        private void PrepareSheet1Worksheet(Worksheet worksheet)
        {
            try
            {
                EISIServiceExportFormat sef = new EISIServiceExportFormat();
                string[] columnsList = sef.ColumnStrings;

                PatternFill redPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(120, 255, 0, 0), Colors.Transparent);
                PatternFill goldPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 215, 0), Colors.Transparent);

                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column);
                    string columnName = column;

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                    if (columnName.Equals("consumer_first") || columnName.Equals("consumer_last") || columnName.Equals("rendering_names"))
                    {
                        worksheet.Cells[IndexRowItemStart, columnKey].SetFill(redPatternFill);
                    }

                    if (columnName.Equals("billing_note") || columnName.Equals("rendering_provider_secondary_id") ||
                        columnName.Equals("rendering_provider_first_name") ||
                        columnName.Equals("rendering_provider_last_name"))
                    {
                        worksheet.Cells[IndexRowItemStart, columnKey].SetFill(goldPatternFill);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void CreateDropDownListWorksheet(Worksheet worksheet, List<string> cpcList, string header)
        {
            try
            {
                PrepareDropDownListWorksheet(worksheet, header);

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

        private void PrepareDropDownListWorksheet(Worksheet worksheet, string header)
        {
            try
            {
                worksheet.Cells[IndexRowItemStart, IndexColumnName].SetValue(header);
                worksheet.Cells[IndexRowItemStart, IndexColumnName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
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

        private void PrepareSheet4Worksheet(Worksheet worksheet)
        {
            try
            {
                string[] columnsList = { "name", "npi", "ma_number", "first_name", "last_name" };
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

        private void CreateSheet4Worksheet(Worksheet worksheet, DataTable dTable)
        {
            try
            {
                PrepareSheet4Worksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in dTable.Rows)
                {
                    worksheet.Cells[currentRow, 0].SetValue(dr["name"].ToString());
                    worksheet.Cells[currentRow, 1].SetValue(dr["npi"].ToString());
                    worksheet.Cells[currentRow, 2].SetValue(dr["ma_number"].ToString());
                    worksheet.Cells[currentRow, 3].SetValue(dr["first_name"].ToString());
                    worksheet.Cells[currentRow, 4].SetValue(dr["last_name"].ToString());
                    currentRow++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        class Lookup
        {
            private readonly string formula = string.Empty; 
            public Lookup(int row, string sheet, int count)
            {
                formula = $"=VLOOKUP(R{row},{sheet}!A2:E{(count + 1)}";
            }

            public string Append(int column)
            {
                string append = $",{column},FALSE)";
                return (formula + append);
            }
        }
    }
}


