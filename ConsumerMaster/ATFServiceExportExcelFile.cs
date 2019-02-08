using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Runtime.Remoting.Messaging;

namespace ConsumerMaster
{
    public class ATFServiceExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly int IndexColumnConsumerFirst = 0;
        private static readonly int IndexColumnConsumerLast = 1;
        private static readonly int IndexColumnConsumerInternalNumber = 2;
        private static readonly int IndexColumnTradingPartnerString = 3;
        private static readonly int IndexColumnTradingPartnerProgramString = 4;
        private static readonly int IndexColumnStartDateString = 5;
        private static readonly int IndexColumnEndDateString = 6;
        private static readonly int IndexColumnDiagnosisCode1Code = 7;
        private static readonly int IndexColumnCompositeProcedureCodeString = 8;
        private static readonly int IndexColumnUnits = 9;
        private static readonly int IndexColumnManualBillableRate = 10;
        private static readonly int IndexColumnPriorAuthorizationNumber = 11;
        private static readonly int IndexColumnReferralNumber = 12;
        private static readonly int IndexColumnReferringProviderId = 13;
        private static readonly int IndexColumnReferringProviderFirstName = 14;
        private static readonly int IndexColumnReferringProviderLastName = 15;
        private static readonly int IndexColumnRenderingProviderId = 16;
        private static readonly int IndexColumnRenderingProviderFirstName = 17;
        private static readonly int IndexColumnRenderingProviderLastName = 18;

        private static readonly int IndexRowItemStart = 0;

        private static readonly int IndexColumnName = 0;

        private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);

        Dictionary<int, string> ceHeader = new Dictionary<int, string>
        {
            {0, "consumer_first"},
            {1, "consumer_last"},
            {2, "consumer_internal_number"},
            {3, "trading_partner_string"},
            {4, "trading_partner_program_string"},
            {5, "start_date_string"},
            {6, "end_date_string"},
            {7, "diagnosis_code_1_code"},
            {8, "composite_procedure_code_string"},
            {9, "units"},
            {10, "manual_billable_rate"},
            {11, "prior_authorization_number"},
            {12, "referral_number"},
            {13, "referring_provider_id"},
            {14, "referring_provider_first_name"},
            {15, "referring_provider_last_name"},
            {16, "rendering_provider_id"},
            {17, "rendering_provider_first_name"},
            {18, "rendering_provider_last_name"}
        };

        public Workbook ATFCreateWorkbook()
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];
                //Worksheet sheet2Worksheet = worksheets["Sheet2"];

                Utility util = new Utility();

                CultureInfo culture = new CultureInfo("en-US");
                DateTime startDateTime = Convert.ToDateTime("01/13/2019 12:00:00 AM", culture);
                DateTime endDateTime = Convert.ToDateTime("01/19/2019 12:59:59 PM", culture);

                DataTable seDataTable = new DataTable();
                using (SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringAttendance"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetConsumersData", sqlConnection1))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@StartDateTime", SqlDbType.Text).Value = startDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        cmd.Parameters.Add("@EndDateTime", SqlDbType.Text).Value = endDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.Fill(seDataTable);
                    }
                }

                int totalConsumers = seDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet, totalConsumers);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in seDataTable.Rows)
                {
                    string[] names = dr["FullName"].ToString().Split(',');
                    sheet1Worksheet.Cells[currentRow, IndexColumnConsumerFirst].SetValue(names[0]);
                    sheet1Worksheet.Cells[currentRow, IndexColumnConsumerLast].SetValue(names[1]);

                   // sheet1Worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber].SetValue(dr["consumer_internal_number"].ToString());
                    CellSelection cellLeadingZeros1 = sheet1Worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber];

                    Int32.TryParse(dr["Site"].ToString(),out int site);
                    string tradingPartnerString = (site == 1) ? "atf_bill_george" : "atf_jefferson";
                    sheet1Worksheet.Cells[currentRow, IndexColumnTradingPartnerString].SetValue(tradingPartnerString);

                    string tradingPartnerProgramString = "waiver";
                    sheet1Worksheet.Cells[currentRow, IndexColumnTradingPartnerProgramString].SetValue(tradingPartnerProgramString);

                    sheet1Worksheet.Cells[currentRow, IndexColumnStartDateString].SetValue(startDateTime.ToString("MM/dd/yyyy"));
                    sheet1Worksheet.Cells[currentRow, IndexColumnEndDateString].SetValue(endDateTime.ToString("MM/dd/yyyy"));


                    sheet1Worksheet.Cells[currentRow, IndexColumnDiagnosisCode1Code].SetValue(dr["diagnosis_code_1_code"].ToString());

                    sheet1Worksheet.Cells[currentRow, IndexColumnCompositeProcedureCodeString].SetValue(dr["composite_procedure_code_string"].ToString());
                    CellIndex dataValidationRuleCellIndex = new CellIndex(currentRow, IndexColumnCompositeProcedureCodeString);


                    Int32.TryParse(dr["Units1"].ToString(), out int units1);
                    Int32.TryParse(dr["Units2"].ToString(), out int units2);
                    int totalUnits = units1 + units2;
                    sheet1Worksheet.Cells[currentRow, IndexColumnUnits].SetValue(totalUnits.ToString());

                    sheet1Worksheet.Cells[currentRow, IndexColumnManualBillableRate].SetValue(" ");                         //"manual_billable_rate"
                    sheet1Worksheet.Cells[currentRow, IndexColumnPriorAuthorizationNumber].SetValue(dr[" "].ToString());    //"prior_authorization_number"
                    sheet1Worksheet.Cells[currentRow, IndexColumnReferralNumber].SetValue(dr[" "].ToString());              //"referral_number"
                    sheet1Worksheet.Cells[currentRow, IndexColumnReferringProviderId].SetValue(dr[" "].ToString());         //"referring_provider_id"
                    sheet1Worksheet.Cells[currentRow, IndexColumnReferringProviderFirstName].SetValue(dr[" "].ToString());  //"referring_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, IndexColumnReferringProviderLastName].SetValue(dr[" "].ToString());   //"referring_provider_last_name"
                    sheet1Worksheet.Cells[currentRow, IndexColumnRenderingProviderId].SetValue(dr[" "].ToString());         //"rendering_provider_id"
                    sheet1Worksheet.Cells[currentRow, IndexColumnRenderingProviderFirstName].SetValue(dr[" "].ToString());  //"rendering_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, IndexColumnRenderingProviderLastName].SetValue(dr[" "].ToString());   //"rendering_provider_last_name"

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
                int lastItemIndexRow = IndexRowItemStart + itemsCount;

                CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                CellIndex firstRowLastCellIndex = new CellIndex(0, 22);
                CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnConsumerFirst);
                CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnRenderingProviderLastName);
                CellBorder border = new CellBorder(CellBorderStyle.Medium, InvoiceBackground);
                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255,255,0,0), Colors.Transparent);

                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetValue(ceHeader[IndexColumnConsumerFirst]);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetFill(solidPatternFill);

                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetValue(ceHeader[IndexColumnConsumerLast]);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetFill(solidPatternFill);

                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetValue(ceHeader[IndexColumnConsumerInternalNumber]);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetValue(ceHeader[IndexColumnTradingPartnerString]);
                worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerProgramString].SetValue(ceHeader[IndexColumnTradingPartnerProgramString]);
                worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerProgramString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnStartDateString].SetValue(ceHeader[IndexColumnStartDateString]);
                worksheet.Cells[IndexRowItemStart, IndexColumnStartDateString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnEndDateString].SetValue(ceHeader[IndexColumnEndDateString]);
                worksheet.Cells[IndexRowItemStart, IndexColumnEndDateString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnDiagnosisCode1Code].SetValue(ceHeader[IndexColumnDiagnosisCode1Code]);
                worksheet.Cells[IndexRowItemStart, IndexColumnDiagnosisCode1Code].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnCompositeProcedureCodeString].SetValue(ceHeader[IndexColumnCompositeProcedureCodeString]);
                worksheet.Cells[IndexRowItemStart, IndexColumnCompositeProcedureCodeString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetValue(ceHeader[IndexColumnHours]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                //worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetFill(solidPatternFill);

                worksheet.Cells[IndexRowItemStart, IndexColumnUnits].SetValue(ceHeader[IndexColumnUnits]);
                worksheet.Cells[IndexRowItemStart, IndexColumnUnits].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnManualBillableRate].SetValue(ceHeader[IndexColumnManualBillableRate]);
                worksheet.Cells[IndexRowItemStart, IndexColumnManualBillableRate].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnPriorAuthorizationNumber].SetValue(ceHeader[IndexColumnPriorAuthorizationNumber]);
                worksheet.Cells[IndexRowItemStart, IndexColumnPriorAuthorizationNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnReferralNumber].SetValue(ceHeader[IndexColumnReferralNumber]);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferralNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderId].SetValue(ceHeader[IndexColumnReferringProviderId]);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderId].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderFirstName].SetValue(ceHeader[IndexColumnReferringProviderFirstName]);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderFirstName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderLastName].SetValue(ceHeader[IndexColumnReferringProviderLastName]);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderLastName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderId].SetValue(ceHeader[IndexColumnRenderingProviderId]);
                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderId].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderFirstName].SetValue(ceHeader[IndexColumnRenderingProviderFirstName]);
                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderFirstName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderLastName].SetValue(ceHeader[IndexColumnRenderingProviderLastName]);
                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderLastName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
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
                int lastItemIndexRow = IndexRowItemStart + itemsCount;

                CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                CellIndex firstRowLastCellIndex = new CellIndex(0, itemsCount);
                CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnName);

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

