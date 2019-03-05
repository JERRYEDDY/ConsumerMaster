using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using System.Windows.Media;

namespace ConsumerMaster
{
    public class AWCServiceExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        //private static readonly int IndexColumnConsumerFirst = 0;
        //private static readonly int IndexColumnConsumerLast = 1;
        //private static readonly int IndexColumnConsumerInternalNumber = 2;
        //private static readonly int IndexColumnDiagnosisCode1Code = 3;
        //private static readonly int IndexColumnTradingPartnerString = 4;
        //private static readonly int IndexColumnTradingPartnerProgramString = 5;
        //private static readonly int IndexColumnStartDateString = 6;
        //private static readonly int IndexColumnEndDateString = 7;
        //private static readonly int IndexColumnCompositeProcedureCodeString = 8;
        //private static readonly int IndexColumnHours = 9;
        //private static readonly int IndexColumnUnits = 10;
        //private static readonly int IndexColumnManualBillableRate = 11;
        //private static readonly int IndexColumnPriorAuthorizationNumber = 12;
        //private static readonly int IndexColumnReferralNumber = 13;
        //private static readonly int IndexColumnReferringProviderId = 14;
        //private static readonly int IndexColumnReferringProviderFirstName = 15;
        //private static readonly int IndexColumnReferringProviderLastName = 16;
        //private static readonly int IndexColumnRenderingProviderId = 17;
        //private static readonly int IndexColumnRenderingProviderFirstName = 18;
        //private static readonly int IndexColumnRenderingProviderLastName = 19;
        //private static readonly int IndexColumnBillingNote = 20;

        private static readonly int IndexRowItemStart = 0;
        private static readonly int IndexColumnName = 0;
        //private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);

        //readonly Dictionary<int, string> _ceHeader = new Dictionary<int, string>
        //{
        //    {0, "consumer_first"},
        //    {1, "consumer_last"},
        //    {2, "consumer_internal_number"},
        //    {3, "diagnosis_code_1_code"},
        //    {4, "trading_partner_string"},
        //    {5, "trading_partner_program_string"},
        //    {6, "start_date_string"},
        //    {7, "end_date_string"},
        //    {8, "composite_procedure_code_string"},
        //    {9, "hours"},
        //    {10, "units"},
        //    {11, "manual_billable_rate"},
        //    {12, "prior_authorization_number"},
        //    {13, "referral_number"},
        //    {14, "referring_provider_id"},
        //    {15, "referring_provider_first_name"},
        //    {16, "referring_provider_last_name"},
        //    {17, "rendering_provider_id"},
        //    {18, "rendering_provider_first_name"},
        //    {19, "rendering_provider_last_name"},
        //    {20, "billing_note" }
        //};

        public Workbook CreateWorkbook()
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

                string seQuery =
                    "SELECT c.consumer_first AS consumer_first, c.consumer_last AS consumer_last, c.consumer_internal_number AS consumer_internal_number, " +
                    "tp.symbol AS trading_partner_string, 'waiver' AS trading_partner_program_string, ' ' AS start_date_string, ' ' AS end_date_string," +
                    "c.diagnosis AS diagnosis_code_1_code, ' ' AS composite_procedure_code_string, ' ' AS hours, ' ' AS units, ' ' AS manual_billable_rate, ' ' AS prior_authorization_number, " +
                    "' ' AS referral_number, ' ' AS referring_provider_id, ' ' AS referring_provider_first_name, ' ' AS referring_provider_last_name, " +
                    "' ' AS rendering_provider_id, ' ' AS rendering_provider_first_name, ' ' AS rendering_provider_last_name FROM Consumers AS c " +
                    "INNER JOIN ConsumerTradingPartner AS ctp ON c.consumer_internal_number = ctp.consumer_internal_number " +
                    "INNER JOIN TradingPartners AS tp ON  ctp.trading_partner_id = tp.id WHERE ctp.trading_partner_id = 5 ORDER BY consumer_last";

                DataTable seDataTable = util.GetDataTable(seQuery);
                int totalConsumers = seDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in seDataTable.Rows)
                {
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("consumer_first")].SetValue(dr["consumer_first"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("consumer_last")].SetValue(dr["consumer_last"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("consumer_internal_number")].SetValue(dr["consumer_internal_number"].ToString());
                    //CellSelection cellLeadingZeros1 = sheet1Worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber];

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("diagnosis_code_1_code")].SetValue(dr["diagnosis_code_1_code"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("trading_partner_string")].SetValue(dr["trading_partner_string"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("trading_partner_program_string")].SetValue(dr["trading_partner_program_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("start_date_string")].SetValue(dr["start_date_string"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("end_date_string")].SetValue(dr["end_date_string"].ToString());

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
                    context.Argument1 = cpcRange;    
                    ListDataValidationRule rule = new ListDataValidationRule(context);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex].SetDataValidationRule(rule);

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("hours")].SetValue(0); //Set to zero

                    string convertToUnits = "=ROUNDDOWN(J" + (currentRow + 1) + "*4, 0)";
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("units")].SetValue(convertToUnits);

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("manual_billable_rate")].SetValue(dr["manual_billable_rate"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("prior_authorization_number")].SetValue(dr["prior_authorization_number"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referral_number")].SetValue(dr["referral_number"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referring_provider_id")].SetValue(dr["referring_provider_id"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referring_provider_first_name")].SetValue(dr["referring_provider_first_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referring_provider_last_name")].SetValue(dr["referring_provider_last_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_id")].SetValue(dr["rendering_provider_id"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_first_name")].SetValue(dr["rendering_provider_first_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_last_name")].SetValue(dr["rendering_provider_last_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("billing_note")].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_secondary_id")].SetValue(" ");

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
                //int lastItemIndexRow = IndexRowItemStart + itemsCount;

                //CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                //CellIndex firstRowLastCellIndex = new CellIndex(0, 22);
                //CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnConsumerFirst);
                //CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnRenderingProviderLastName);
                //CellBorder border = new CellBorder(CellBorderStyle.Medium, InvoiceBackground);
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

                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetValue(_ceHeader[IndexColumnConsumerFirst]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetFill(solidPatternFill);

                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetValue(_ceHeader[IndexColumnConsumerLast]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetFill(solidPatternFill);

                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetValue(_ceHeader[IndexColumnConsumerInternalNumber]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnDiagnosisCode1Code].SetValue(_ceHeader[IndexColumnDiagnosisCode1Code]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnDiagnosisCode1Code].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetValue(_ceHeader[IndexColumnTradingPartnerString]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerProgramString].SetValue(_ceHeader[IndexColumnTradingPartnerProgramString]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerProgramString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnStartDateString].SetValue(_ceHeader[IndexColumnStartDateString]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnStartDateString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnEndDateString].SetValue(_ceHeader[IndexColumnEndDateString]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnEndDateString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnCompositeProcedureCodeString].SetValue(_ceHeader[IndexColumnCompositeProcedureCodeString]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnCompositeProcedureCodeString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetValue(_ceHeader[IndexColumnHours]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                //worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetFill(solidPatternFill);

                //worksheet.Cells[IndexRowItemStart, IndexColumnUnits].SetValue(_ceHeader[IndexColumnUnits]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnUnits].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnManualBillableRate].SetValue(_ceHeader[IndexColumnManualBillableRate]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnManualBillableRate].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnPriorAuthorizationNumber].SetValue(_ceHeader[IndexColumnPriorAuthorizationNumber]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnPriorAuthorizationNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnReferralNumber].SetValue(_ceHeader[IndexColumnReferralNumber]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnReferralNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderId].SetValue(_ceHeader[IndexColumnReferringProviderId]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderId].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderFirstName].SetValue(_ceHeader[IndexColumnReferringProviderFirstName]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderFirstName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderLastName].SetValue(_ceHeader[IndexColumnReferringProviderLastName]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderLastName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderId].SetValue(_ceHeader[IndexColumnRenderingProviderId]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderId].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderFirstName].SetValue(_ceHeader[IndexColumnRenderingProviderFirstName]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderFirstName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderLastName].SetValue(_ceHeader[IndexColumnRenderingProviderLastName]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderLastName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnBillingNote].SetValue(_ceHeader[IndexColumnBillingNote]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnBillingNote].SetHorizontalAlignment(RadHorizontalAlignment.Left);
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

