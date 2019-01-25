using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Media;
using System.Linq;
using System.Web;
using Telerik.Windows.Documents.Spreadsheet.Model;
using FileHelpers;

namespace ConsumerMaster
{
    public class EIServiceExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        //private static readonly int IndexColumnConsumerFirst = 0;
        //private static readonly int IndexColumnConsumerLast = 1;
        //private static readonly int IndexColumnConsumerInternalNumber = 2;
        //private static readonly int IndexColumnTradingPartnerString = 3;
        //private static readonly int IndexColumnTradingPartnerProgramString = 4;
        //private static readonly int IndexColumnStartDateString = 5;
        //private static readonly int IndexColumnEndDateString = 6;
        //private static readonly int IndexColumnDiagnosisCode1Code = 7;
        //private static readonly int IndexColumnCompositeProcedureCodeString = 8;
        //private static readonly int IndexColumnUnits = 9;
        //private static readonly int IndexColumnManualBillableRate = 10;
        //private static readonly int IndexColumnPriorAuthorizationNumber = 11;
        //private static readonly int IndexColumnReferralNumber = 12;
        //private static readonly int IndexColumnReferringProviderId = 13;
        //private static readonly int IndexColumnReferringProviderFirstName = 14;
        //private static readonly int IndexColumnReferringProviderLastName = 15;
        //private static readonly int IndexColumnRenderingProviderId = 16;
        //private static readonly int IndexColumnRenderingProviderFirstName = 17;
        //private static readonly int IndexColumnRenderingProviderLastName = 18;

        private static readonly int IndexRowItemStart = 0;

        private static readonly int IndexColumnName = 0;

        private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);


        //Dictionary<int, string> ceHeader = new Dictionary<int, string>
        //{
        //    {0, "consumer_first"},
        //    {1, "consumer_last"},
        //    {2, "consumer_internal_number"},
        //    {3, "trading_partner_string"},
        //    {4, "trading_partner_program_string"},
        //    {5, "start_date_string"},
        //    {6, "end_date_string"},
        //    {7, "diagnosis_code_1_code"},
        //    {8, "composite_procedure_code_string"},
        //    {9, "units"},
        //    {10, "manual_billable_rate"},
        //    {11, "prior_authorization_number"},
        //    {12, "referral_number"},
        //    {13, "referring_provider_id"},
        //    {14, "referring_provider_first_name"},
        //    {15, "referring_provider_last_name"},
        //    {16, "rendering_provider_id"},
        //    {17, "rendering_provider_first_name"},
        //    {18, "rendering_provider_last_name"}
        //};

        public Workbook EICreateWorkbook()
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];
                //Worksheet sheet2Worksheet = worksheets["Sheet2"];

                EIServiceExportFormat esef = new EIServiceExportFormat();
                //Dictionary<int, string> dictionary = new Dictionary<int, string>();

                Utility util = new Utility();





                string inFileName = @"C:\Billing Software\EI\GREENE CTY DEC 2018 -FINAL.tsv";

                var inEngine = new FileHelperEngine<EIBillingTransactions>();
                var inResult = inEngine.ReadFile(inFileName);




                DataTable billingDataTable = new DataTable();

                billingDataTable.Columns.Add("Therapists", typeof(string));
                billingDataTable.Columns.Add("BillDate", typeof(string));
                billingDataTable.Columns.Add("LastName", typeof(string));
                billingDataTable.Columns.Add("FirstName", typeof(string));
                billingDataTable.Columns.Add("County", typeof(string));
                billingDataTable.Columns.Add("FundingSource", typeof(string));
                billingDataTable.Columns.Add("VisitType", typeof(string));
                billingDataTable.Columns.Add("Discipline", typeof(string));
                billingDataTable.Columns.Add("TotalUnits", typeof(string));

                foreach (EIBillingTransactions ebi in inResult)
                {
                    DataRow dr = billingDataTable.NewRow();

                    dr[0] = ebi.Therapists;
                    dr[1] = ebi.BillDate.ToString("MM/dd/yyyy");
                    dr[2] = ebi.LastName;
                    dr[3] = ebi.FirstName;
                    dr[4] = ebi.County;
                    dr[5] = ebi.FundingSource;
                    dr[6] = ebi.VisitType;
                    dr[7] = ebi.Discipline;
                    dr[8] = ebi.TotalUnits;

                    billingDataTable.Rows.Add(dr);
                }


                InsertDataIntoSQLServerUsingSQLBulkCopy(billingDataTable);

                string seQuery =
                    SELECT e.FirstName AS consumer_first, e.LastName AS consumer_last, c.consumer_internal_number, c.diagnosis, 
                e.BillDate,  e.County, e.FundingSource, e.Discipline, e.TotalUnits AS units,
                    t.rendering_provider_id, t.rendering_provider_first_name, t.rendering_provider_last_name
                FROM[EIBillingTransactions] e
                    LEFT OUTER JOIN[Consumers] c ON e.LastName = c.consumer_last AND e.FirstName = c.consumer_first
                LEFT OUTER JOIN[Therapists] t ON e.Therapists = t.rendering_provider_name

                DataTable seDataTable = util.GetDataTable(seQuery);

                int totalConsumers = seDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet, totalConsumers);



                foreach (EIBillingTransactions ebi in inResult)
                {
                    string tradingPartner = (ebi.Discipline.Equals("SPECIAL INSTRUCTION")) ? "eisi_in_home" : "eidt_in_home";

                    string tradingPartnerProgram = " ";
                    switch (ebi.FundingSource)
                    {
                        case "MA":
                            tradingPartnerProgram = "ma" + "_" + ebi.County.ToLower();
                            break;

                        case "COUNTY":
                            tradingPartnerProgram = "b" + "_" + ebi.County.ToLower();
                            break;

                        case "WAIVER":
                            tradingPartnerProgram = "w" + "_" + ebi.County.ToLower();
                            break;

                    }

                    string[] therapist = ebi.Therapists.Split(',');

                    string billingNote = " ";
                    switch (ebi.County)
                    {
                        case "ALLEGHENY":
                            billingNote = "CC11006 - ALLEGHENY";
                            break;

                        case "FAYETTE":
                            billingNote = "CC11029 - FAYETTE";
                            break;

                        case "GREENE":
                            billingNote = "CC11032 - GREENE";
                            break;

                        case "WASHINGTON":
                            billingNote = "CC11049 - WASHINGTON";
                            break;

                        case "WESTMORELAND":
                            billingNote = "CC11050 - WESTMORELAND";
                            break;
                    }









                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in seDataTable.Rows)
                {
                    string[] names = dr["FullName"].ToString().Split(',');

                    // sheet1Worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber].SetValue(dr["consumer_internal_number"].ToString());
                    //CellSelection cellLeadingZeros1 = sheet1Worksheet.Cells[currentRow, esef.DKey("consumer_internal_number")];

                    Int32.TryParse(dr["Site"].ToString(), out int site);
                    string tradingPartnerString = (site == 1) ? "atf_bill_george" : "atf_jefferson";
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("trading_partner_string")].SetValue(tradingPartnerString);

                    string tradingPartnerProgramString = "waiver";
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("trading_partner_program_string")].SetValue(tradingPartnerProgramString);

                    //sheet1Worksheet.Cells[currentRow, esef.DKey("start_date_string")].SetValue(startDateTime.ToString("MM/dd/yyyy"));
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("end_date_string")].SetValue(endDateTime.ToString("MM/dd/yyyy"));

                    //sheet1Worksheet.Cells[currentRow, esef.DKey("diagnosis_code_1_code")].SetValue(dr["diagnosis_code_1_code"].ToString());

                    //sheet1Worksheet.Cells[currentRow, esef.DKey("composite_procedure_code_string")].SetValue(dr["composite_procedure_code_string"].ToString());
                    //CellIndex dataValidationRuleCellIndex = new CellIndex(currentRow, esef.DKey("diagnosis_code_1_code"));

                    Int32.TryParse(dr["Units1"].ToString(), out int units1);
                    Int32.TryParse(dr["Units2"].ToString(), out int units2);
                    int totalUnits = units1 + units2;
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("units")].SetValue(totalUnits.ToString());

                    //sheet1Worksheet.Cells[currentRow, esef.DKey("manual_billable_rate")].SetValue(" ");         //"manual_billable_rate"
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("prior_authorization_number")].SetValue(" ");   //"prior_authorization_number"
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("referral_number")].SetValue(" ");              //"referral_number"
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("referring_provider_id")].SetValue(" ");         //"referring_provider_id"
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("referring_provider_first_name")].SetValue(" ");  //"referring_provider_first_name"
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("referring_provider_last_name")].SetValue(" ");   //"referring_provider_last_name"
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("rendering_provider_id")].SetValue(" ");         //"rendering_provider_id"
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("rendering_provider_first_name")].SetValue(" ");  //"rendering_provider_first_name"
                    //sheet1Worksheet.Cells[currentRow, esef.DKey("rendering_provider_last_name")].SetValue(" ");   //"rendering_provider_last_name"

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

        static void InsertDataIntoSQLServerUsingSQLBulkCopy(DataTable csvFileData)
        {
            using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString))
            {
                dbConnection.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "EIBillingTransactions";
                    foreach (var column in csvFileData.Columns)
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    s.WriteToServer(csvFileData);
                }
            }
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

                EIServiceExportFormat esef = new EIServiceExportFormat();
                Dictionary<int, string> dictionary = new Dictionary<int, string>();

                CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                CellIndex firstRowLastCellIndex = new CellIndex(0, 22);
                //CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, esef.DKey("consumer_first"));
                //CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1, esef.DKey("rendering_provider_last_name"));
                CellBorder border = new CellBorder(CellBorderStyle.Medium, InvoiceBackground);
                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 0, 0), Colors.Transparent);

                foreach (KeyValuePair<int,string> keyValue in dictionary)
                {
                    int columnKey = keyValue.Key;
                    string columnName = keyValue.Value;

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                    if (columnName.Equals("consumer_first") || columnName.Equals("consumer_last"))
                    {
                        worksheet.Cells[IndexRowItemStart, columnKey].SetFill(solidPatternFill);
                    }

                }

                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetValue(ceHeader[IndexColumnConsumerFirst]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetFill(solidPatternFill);

                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetValue(ceHeader[IndexColumnConsumerLast]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetFill(solidPatternFill);

                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetValue(ceHeader[IndexColumnConsumerInternalNumber]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetValue(ceHeader[IndexColumnTradingPartnerString]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerProgramString].SetValue(ceHeader[IndexColumnTradingPartnerProgramString]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerProgramString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnStartDateString].SetValue(ceHeader[IndexColumnStartDateString]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnStartDateString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnEndDateString].SetValue(ceHeader[IndexColumnEndDateString]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnEndDateString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnDiagnosisCode1Code].SetValue(ceHeader[IndexColumnDiagnosisCode1Code]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnDiagnosisCode1Code].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnCompositeProcedureCodeString].SetValue(ceHeader[IndexColumnCompositeProcedureCodeString]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnCompositeProcedureCodeString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnUnits].SetValue(ceHeader[IndexColumnUnits]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnUnits].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnManualBillableRate].SetValue(ceHeader[IndexColumnManualBillableRate]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnManualBillableRate].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnPriorAuthorizationNumber].SetValue(ceHeader[IndexColumnPriorAuthorizationNumber]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnPriorAuthorizationNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnReferralNumber].SetValue(ceHeader[IndexColumnReferralNumber]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnReferralNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderId].SetValue(ceHeader[IndexColumnReferringProviderId]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderId].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderFirstName].SetValue(ceHeader[IndexColumnReferringProviderFirstName]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderFirstName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderLastName].SetValue(ceHeader[IndexColumnReferringProviderLastName]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderLastName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderId].SetValue(ceHeader[IndexColumnRenderingProviderId]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderId].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderFirstName].SetValue(ceHeader[IndexColumnRenderingProviderFirstName]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderFirstName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderLastName].SetValue(ceHeader[IndexColumnRenderingProviderLastName]);
                //worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderLastName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
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