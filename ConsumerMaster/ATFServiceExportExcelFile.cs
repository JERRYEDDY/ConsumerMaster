using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.Windows.Media;

using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;

namespace ConsumerMaster
{
    public class ATFServiceExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly int IndexRowItemStart = 0;
        private static readonly int IndexColumnName = 0;
        private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);

        public Workbook ATFCreateWorkbook(string tradingPartnerID)
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

                List<string> cpcList = util.GetList("SELECT name FROM CompositeProcedureCodes WHERE trading_partner_id = 3 or trading_partner_id = 4");
                                                                                    //Adult Training Facility; Bill George = 3 or Adult Training Facility; Jefferson = 4
                CreateCompositeProcedureCodesWorksheet(sheet2Worksheet, cpcList);
                ServiceExportFormat sef = new ServiceExportFormat();

                string seQuery =
                    "SELECT c.consumer_first AS consumer_first, c.consumer_last AS consumer_last, c.consumer_internal_number AS consumer_internal_number, " +
                    "tp.symbol AS trading_partner_string, 'waiver' AS trading_partner_program_string, ' ' AS start_date_string, ' ' AS end_date_string," +
                    "c.diagnosis AS diagnosis_code_1_code, ' ' AS composite_procedure_code_string, ' ' AS units, ' ' AS manual_billable_rate, ' ' AS prior_authorization_number, " +
                    "' ' AS referral_number, ' ' AS referring_provider_id, ' ' AS referring_provider_first_name, ' ' AS referring_provider_last_name, " +
                    "' ' AS rendering_provider_id, ' ' AS rendering_provider_first_name, ' ' AS rendering_provider_last_name FROM Consumers AS c " +
                    "INNER JOIN ConsumerTradingPartner AS ctp ON c.consumer_internal_number = ctp.consumer_internal_number " +
                    "INNER JOIN TradingPartners AS tp ON  ctp.trading_partner_id = tp.id WHERE ctp.trading_partner_id = " + tradingPartnerID + " ORDER BY consumer_last";

                DataTable seDataTable = util.GetDataTable(seQuery);
                int totalConsumers = seDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet, totalConsumers);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in seDataTable.Rows)
                {
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("consumer_first")].SetValue(dr["consumer_first"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("consumer_last")].SetValue(dr["consumer_last"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("consumer_internal_number")].SetValue(dr["consumer_internal_number"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("trading_partner_string")].SetValue(dr["trading_partner_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("trading_partner_program_string")].SetValue(dr["trading_partner_program_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("start_date_string")].SetValue(dr["start_date_string"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("end_date_string")].SetValue(dr["end_date_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("diagnosis_code_1_code")].SetValue(dr["diagnosis_code_1_code"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("composite_procedure_code_string")].SetValue(dr["composite_procedure_code_string"].ToString());
                    CellIndex dataValidationRuleCellIndex = new CellIndex(currentRow, sef.GetKey("composite_procedure_code_string"));
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
                    context.Argument1 = cpcRange; //   
                    ListDataValidationRule rule = new ListDataValidationRule(context);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex].SetDataValidationRule(rule);

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("units")].SetValue(dr["units"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("manual_billable_rate")].SetValue(dr["manual_billable_rate"].ToString());                          //"manual_billable_rate"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("prior_authorization_number")].SetValue(dr["prior_authorization_number"].ToString());              //"prior_authorization_number"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referral_number")].SetValue(dr["referral_number"].ToString());                                    //"referral_number"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referring_provider_id")].SetValue(dr["referring_provider_id"].ToString());                        //"referring_provider_id"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referring_provider_first_name")].SetValue(dr["referring_provider_first_name"].ToString());        //"referring_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referring_provider_last_name")].SetValue(dr["referring_provider_last_name"].ToString());          //"referring_provider_last_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_id")].SetValue(dr["rendering_provider_id"].ToString());                        //"rendering_provider_id"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_first_name")].SetValue(dr["rendering_provider_first_name"].ToString());        //"rendering_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_last_name")].SetValue(dr["rendering_provider_last_name"].ToString());          //"rendering_provider_last_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("billing_note")].SetValue(" ");                                                                    //"billing_note"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_secondary_id")].SetValue(" ");                                                 //"rendering_provider_secondary_id"

                    currentRow++;
                }

                for (int i = 0; i < sheet1Worksheet.Columns.Count; i++)
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
                PrepareSheet2Worksheet(worksheet, cpcList.Count);

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

        private void PrepareSheet1Worksheet(Worksheet worksheet, int itemsCount)
        {
            try
            {
                //int lastItemIndexRow = IndexRowItemStart + itemsCount;

                ServiceExportFormat sef = new ServiceExportFormat();
                string[] columnsList = sef.GetColumns();

                //CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                //CellIndex firstRowLastCellIndex = new CellIndex(0, columnsList.Length);
                //CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, sef.GetKey("consumer_first"));
                //CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1, sef.GetKey("rendering_provider_last_name"));
                //CellBorder border = new CellBorder(CellBorderStyle.Medium, InvoiceBackground);

                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 0, 0), Colors.Transparent);

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

        private void PrepareSheet2Worksheet(Worksheet worksheet, int itemsCount)
        {
            try
            {
                //int lastItemIndexRow = IndexRowItemStart + itemsCount;

                //CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                //CellIndex firstRowLastCellIndex = new CellIndex(0, itemsCount);
                //CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnName);

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


