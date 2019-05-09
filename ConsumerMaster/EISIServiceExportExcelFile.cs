using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
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
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];   //Data Entry Sheet
                Worksheet sheet2Worksheet = worksheets["Sheet2"];   //Trading Partner Programs
                Worksheet sheet3Worksheet = worksheets["Sheet3"];   //Composite Procedure Codes
                Worksheet sheet4Worksheet = worksheets["Sheet4"];   //Rendering Providers
                Worksheet sheet5Worksheet = worksheets["Sheet5"];   //Billing Codes

                Utility util = new Utility();

                List<string> tppList = util.GetList("SELECT symbol FROM TradingPartnerPrograms");
                CreateDropDownListWorksheet(sheet2Worksheet, tppList, "trading_partner_program");

                //Early Intervention Direct Therapy; In Home = 7 or Early Intervention Special Instruction; In Home = 8 
                List<string> cpcList = util.GetList("SELECT name FROM CompositeProcedureCodes WHERE trading_partner_id = " + tradingPartnerId);
                CreateDropDownListWorksheet(sheet3Worksheet, cpcList, "composite_procedure_code");


                List<string> bnList = new List<string>() { "CC11006", "CC11029", "CC11032", "CC11049", "CC11050" };     //Billing Note List
                CreateDropDownListWorksheet(sheet5Worksheet, bnList, "billing_note");


                EISIServiceExportFormat sef = new EISIServiceExportFormat();
                string seQuery =
                    "SELECT c.consumer_first AS consumer_first, c.consumer_last AS consumer_last, c.consumer_internal_number AS consumer_internal_number," +
                    " tp.symbol AS trading_partner_string, 'waiver' AS trading_partner_program_string, ' ' AS start_date_string, ' ' AS end_date_string, " +
                    "c.diagnosis AS diagnosis_code_1_code, ' ' AS composite_procedure_code_string, ' ' AS units, ' ' AS manual_billable_rate, ' ' AS prior_authorization_number, " +
                    " ' ' AS referral_number, ' ' AS referring_provider_id, ' ' AS referring_provider_first_name, ' ' AS referring_provider_last_name, ' ' AS rendering_provider_id, " +
                    "' ' AS rendering_provider_first_name, ' ' AS rendering_provider_last_name FROM Consumers AS c " +
                    "INNER JOIN TradingPartners AS tp ON " + tradingPartnerId + " = tp.id" + 
                    " WHERE c.trading_partner_id1 = " + tradingPartnerId + " OR c.trading_partner_id2 = " + tradingPartnerId + " OR c.trading_partner_id3 = " + tradingPartnerId + 
                    " ORDER BY consumer_last";

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
                    CellIndex dataValidationRuleCellIndex1 = new CellIndex(currentRow, sef.GetIndex("composite_procedure_code_string"));
                    ListDataValidationRuleContext context1 = new ListDataValidationRuleContext(sheet1Worksheet, dataValidationRuleCellIndex1)
                    {
                        InputMessageTitle = "Restricted input",
                        InputMessageContent = "The input is restricted to the composite procedure codes.",
                        ErrorStyle = ErrorStyle.Stop,
                        ErrorAlertTitle = "Wrong value",
                        ErrorAlertContent = "The entered value is not valid. Allowed values are the composite procedure codes!",
                        InCellDropdown = true
                    };
                    string listRange1 = "=Sheet2!$A$2:$A$" + tppList.Count + 1;  //= Sheet2!$A$2:$A$73
                    context1.Argument1 = listRange1; //   
                    ListDataValidationRule rule1 = new ListDataValidationRule(context1);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex1].SetDataValidationRule(rule1);



                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("start_date_string")].SetValue(dr["start_date_string"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("end_date_string")].SetValue(dr["end_date_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("diagnosis_code_1_code")].SetValue(dr["diagnosis_code_1_code"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("composite_procedure_code_string")].SetValue(dr["composite_procedure_code_string"].ToString());
                    CellIndex dataValidationRuleCellIndex2 = new CellIndex(currentRow, sef.GetIndex("composite_procedure_code_string"));
                    ListDataValidationRuleContext context2 = new ListDataValidationRuleContext(sheet1Worksheet, dataValidationRuleCellIndex2)
                    {
                        InputMessageTitle = "Restricted input",
                        InputMessageContent = "The input is restricted to the composite procedure codes.",
                        ErrorStyle = ErrorStyle.Stop,
                        ErrorAlertTitle = "Wrong value",
                        ErrorAlertContent = "The entered value is not valid. Allowed values are the composite procedure codes!",
                        InCellDropdown = true
                    };
                    string listRange2 = "=Sheet3!$A$2:$A$" + cpcList.Count + 1;  //= Sheet2!$A$2:$A$73
                    context2.Argument1 = listRange2; //   
                    ListDataValidationRule rule = new ListDataValidationRule(context2);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex2].SetDataValidationRule(rule);

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("units")].SetValue(dr["units"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("manual_billable_rate")].SetValue(dr["manual_billable_rate"].ToString());                            //"manual_billable_rate"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("prior_authorization_number")].SetValue(dr["prior_authorization_number"].ToString());                //"prior_authorization_number"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referral_number")].SetValue(dr["referral_number"].ToString());                                      //"referral_number"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_id")].SetValue(dr["referring_provider_id"].ToString());                          //"referring_provider_id"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_first_name")].SetValue(dr["referring_provider_first_name"].ToString());          //"referring_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_last_name")].SetValue(dr["referring_provider_last_name"].ToString());            //"referring_provider_last_name"

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_names")].SetValue(" ");                                                                   //"rendering_names"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_id")].SetValue(dr["rendering_provider_id"].ToString());                          //"rendering_provider_id"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_secondary_id")].SetValue(" ");                                                   //"rendering_provider_secondary_id"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_first_name")].SetValue(dr["rendering_provider_first_name"].ToString());          //"rendering_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_last_name")].SetValue(dr["rendering_provider_last_name"].ToString());            //"rendering_provider_last_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_taxonomy_code")].SetValue(" ");                                                  //"rendering_provider_taxonomy_code"

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("billing_note")].SetValue(" ");                                                                      //"billing_note"


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
    }
}


