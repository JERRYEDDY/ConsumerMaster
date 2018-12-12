using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;

namespace ConsumerMaster
{
    public class ServiceExportExcelFile
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
        private static readonly int IndexColumnHours = 9;
        private static readonly int IndexColumnUnits = 10;
        private static readonly int IndexColumnManualBillableRate = 11;
        private static readonly int IndexColumnPriorAuthorizationNumber = 12;
        private static readonly int IndexColumnReferralNumber = 13;
        private static readonly int IndexColumnReferringProviderId = 14;
        private static readonly int IndexColumnReferringProviderFirstName = 15;
        private static readonly int IndexColumnReferringProviderLastName = 16;
        private static readonly int IndexColumnRenderingProviderId = 17;
        private static readonly int IndexColumnRenderingProviderFirstName = 18;
        private static readonly int IndexColumnRenderingProviderLastName = 19;

        private static readonly int IndexRowItemStart = 0;

        private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);
        private static readonly ThemableColor InvoiceHeaderForeground = ThemableColor.FromArgb(255, 255, 255, 255);

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
            {9, "hours" },
            {10, "units"},
            {11, "manual_billable_rate"},
            {12, "prior_authorization_number"},
            {13, "referral_number"},
            {14, "referring_provider_id"},
            {15, "referring_provider_first_name"},
            {16, "referring_provider_last_name"},
            {17, "rendering_provider_id"},
            {18, "rendering_provider_first_name"},
            {19, "rendering_provider_last_name"}
        };

        public Workbook CreateWorkbook()
        {
            Workbook workbook = new Workbook();
            workbook.Sheets.Add(SheetType.Worksheet);
            Worksheet worksheet = workbook.ActiveWorksheet;

            string seQuery =
                "SELECT c.consumer_first AS consumer_first, c.consumer_last AS consumer_last, c.consumer_internal_number AS consumer_internal_number, " +
                "tp.string AS trading_partner_string, 'waiver' AS trading_partner_program_string, ' ' AS start_date_string, ' ' AS end_date_string," +
                "c.diagnosis AS diagnosis_code_1_code, ' ' AS composite_procedure_code_string, ' ' AS hours, ' ' AS units, ' ' AS manual_billable_rate, ' ' AS prior_authorization_number, " +
                "' ' AS referral_number, ' ' AS referring_provider_id, ' ' AS referring_provider_first_name, ' ' AS referring_provider_last_name, " +
                "' ' AS rendering_provider_id, ' ' AS rendering_provider_first_name, ' ' AS rendering_provider_last_name FROM[TESTConsumerMaster].[dbo].[Consumers] AS c " +
                "INNER JOIN ConsumerTradingPartner AS ctp ON c.consumer_internal_number = ctp.consumer_internal_number " +
                "INNER JOIN TradingPartners AS tp ON  ctp.trading_partner_id = tp.id WHERE ctp.trading_partner_id = 5";

            Utility util = new Utility();
            DataTable seDataTable = util.GetDataTable(seQuery);

            int totalConsumers = seDataTable.Rows.Count;
            PrepareWorksheet(worksheet, totalConsumers);

            int currentRow = IndexRowItemStart + 1;
            foreach (DataRow dr in seDataTable.Rows)
            {
                worksheet.Cells[currentRow, IndexColumnConsumerFirst].SetValue(dr["consumer_first"].ToString());
                worksheet.Cells[currentRow, IndexColumnConsumerLast].SetValue(dr["consumer_last"].ToString());

                worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber].SetValue(dr["consumer_internal_number"].ToString());
                CellSelection cellLeadingZeros1 = worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber];
                CellValueFormat leadingZerosFormat1 = new CellValueFormat("0000");
                cellLeadingZeros1.SetFormat(leadingZerosFormat1);

                worksheet.Cells[currentRow, IndexColumnTradingPartnerString].SetValue(dr["trading_partner_string"].ToString());
                worksheet.Cells[currentRow, IndexColumnTradingPartnerProgramString].SetValue(dr["trading_partner_program_string"].ToString());

                worksheet.Cells[currentRow, IndexColumnStartDateString].SetValue(dr["start_date_string"].ToString());
                worksheet.Cells[currentRow, IndexColumnEndDateString].SetValue(dr["end_date_string"].ToString());
                worksheet.Cells[currentRow, IndexColumnDiagnosisCode1Code].SetValue(dr["diagnosis_code_1_code"].ToString());
                worksheet.Cells[currentRow, IndexColumnCompositeProcedureCodeString].SetValue(dr["composite_procedure_code_string"].ToString());
                worksheet.Cells[currentRow, IndexColumnUnits].SetValue(dr["hours"].ToString());

                string convertToUnits = "=J" + (currentRow +1) + "*4";
                worksheet.Cells[currentRow, IndexColumnUnits].SetValue(convertToUnits);

                worksheet.Cells[currentRow, IndexColumnManualBillableRate].SetValue(dr["manual_billable_rate"].ToString());
                worksheet.Cells[currentRow, IndexColumnPriorAuthorizationNumber].SetValue(dr["prior_authorization_number"].ToString());
                worksheet.Cells[currentRow, IndexColumnReferralNumber].SetValue(dr["referral_number"].ToString());
                worksheet.Cells[currentRow, IndexColumnReferringProviderId].SetValue(dr["referring_provider_id"].ToString());
                worksheet.Cells[currentRow, IndexColumnReferringProviderFirstName].SetValue(dr["referring_provider_first_name"].ToString());
                worksheet.Cells[currentRow, IndexColumnReferringProviderLastName].SetValue(dr["referring_provider_last_name"].ToString());
                worksheet.Cells[currentRow, IndexColumnRenderingProviderId].SetValue(dr["rendering_provider_id"].ToString());
                worksheet.Cells[currentRow, IndexColumnRenderingProviderFirstName].SetValue(dr["rendering_provider_first_name"].ToString());
                worksheet.Cells[currentRow, IndexColumnRenderingProviderLastName].SetValue(dr["rendering_provider_last_name"].ToString());

                currentRow++;
            }

            for (int i = 0; i < worksheet.Columns.Count; i++)
            {
                worksheet.Columns[i].AutoFitWidth();
            }

            return workbook;
        }

        private void PrepareWorksheet(Worksheet worksheet, int itemsCount)
        {
            try
            {
                int lastItemIndexRow = IndexRowItemStart + itemsCount;

                CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                CellIndex firstRowLastCellIndex = new CellIndex(0, 19);
                CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnConsumerFirst);
                CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnRenderingProviderLastName);
                CellBorder border = new CellBorder(CellBorderStyle.Medium, InvoiceBackground);

                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetValue(ceHeader[0]);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetValue(ceHeader[1]);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetValue(ceHeader[2]);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetValue(ceHeader[3]);
                worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerProgramString].SetValue(ceHeader[4]);
                worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerProgramString].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnStartDateString].SetValue(ceHeader[5]);
                worksheet.Cells[IndexRowItemStart, IndexColumnStartDateString].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnEndDateString].SetValue(ceHeader[6]);
                worksheet.Cells[IndexRowItemStart, IndexColumnEndDateString].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnDiagnosisCode1Code].SetValue(ceHeader[7]);
                worksheet.Cells[IndexRowItemStart, IndexColumnDiagnosisCode1Code].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnCompositeProcedureCodeString].SetValue(ceHeader[8]);
                worksheet.Cells[IndexRowItemStart, IndexColumnCompositeProcedureCodeString].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetValue(ceHeader[9]);
                worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnUnits].SetValue(ceHeader[10]);
                worksheet.Cells[IndexRowItemStart, IndexColumnUnits].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnManualBillableRate].SetValue(ceHeader[11]);
                worksheet.Cells[IndexRowItemStart, IndexColumnManualBillableRate].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnPriorAuthorizationNumber].SetValue(ceHeader[12]);
                worksheet.Cells[IndexRowItemStart, IndexColumnPriorAuthorizationNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferralNumber].SetValue(ceHeader[13]);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferralNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderId].SetValue(ceHeader[14]);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderId].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderFirstName].SetValue(ceHeader[15]);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderFirstName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderLastName].SetValue(ceHeader[16]);
                worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderLastName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderId].SetValue(ceHeader[17]);
                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderId].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderFirstName].SetValue(ceHeader[18]);
                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderFirstName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderLastName].SetValue(ceHeader[19]);
                worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderLastName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}

