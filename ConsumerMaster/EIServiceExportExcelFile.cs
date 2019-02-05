using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using FileHelpers;

namespace ConsumerMaster
{
    public class EIServiceExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly int IndexRowItemStart = 0;
        private static readonly int IndexColumnName = 0;
        private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);

        public Workbook EICreateWorkbook()
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];
                Utility util = new Utility();

                EIServiceExportFormat esef = new EIServiceExportFormat();
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

                DataTable seDataTable = new DataTable();
                using (SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetEIBillingTransactions", sqlConnection1))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@EIBillingTableType", billingDataTable);
                        tvpParam.SqlDbType = SqlDbType.Structured;

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(seDataTable);
                    }
                }

                int totalConsumers = seDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet, totalConsumers);

                string[] columnsList = esef.GetColumns();

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in seDataTable.Rows)
                {
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("consumer_first")].SetValue(dr["consumer_first"].ToString());
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("consumer_last")].SetValue(dr["consumer_last"].ToString());

                    sheet1Worksheet.Cells[currentRow, esef.GetKey("consumer_internal_number")].SetValue(dr["consumer_internal_number"].ToString());
                    CellSelection cellLeadingZeros1 = sheet1Worksheet.Cells[currentRow, esef.GetKey("consumer_internal_number")];
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("diagnosis_code_1_code")].SetValue(dr["diagnosis_code_1_code"].ToString());
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("trading_partner_string")].SetValue(dr["trading_partner_string"].ToString());

                    string tradingPartnerProgram = " ";
                    switch (dr["FundingSource"])
                    {
                        case "MA":
                            tradingPartnerProgram = "ma" + "_" + dr["County"].ToString().ToLower();
                            break;

                        case "COUNTY":
                            tradingPartnerProgram = "b" + "_" + dr["County"].ToString().ToLower();
                            break;

                        case "WAIVER":
                            tradingPartnerProgram = "w" + "_" + dr["County"].ToString().ToLower();
                            break;

                    }
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("trading_partner_program_string")].SetValue(tradingPartnerProgram);

                    sheet1Worksheet.Cells[currentRow, esef.GetKey("start_date_string")].SetValue(dr["bill_date"].ToString());

                    sheet1Worksheet.Cells[currentRow, esef.GetKey("end_date_string")].SetValue(dr["bill_date"].ToString());

                    sheet1Worksheet.Cells[currentRow, esef.GetKey("composite_procedure_code_string")].SetValue(dr["composite_procedure_code_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, esef.GetKey("units")].SetValue(dr["units"].ToString());

                    sheet1Worksheet.Cells[currentRow, esef.GetKey("manual_billable_rate")].SetValue(" ");         //"manual_billable_rate"
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("prior_authorization_number")].SetValue(" ");   //"prior_authorization_number"
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("referral_number")].SetValue(" ");              //"referral_number"
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("referring_provider_id")].SetValue(" ");         //"referring_provider_id"
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("referring_provider_first_name")].SetValue(" ");  //"referring_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("referring_provider_last_name")].SetValue(" ");   //"referring_provider_last_name"
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("rendering_provider_id")].SetValue(dr["rendering_provider_id"].ToString());         //"rendering_provider_id"
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("rendering_provider_first_name")].SetValue(dr["rendering_provider_first_name"].ToString());  //"rendering_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("rendering_provider_last_name")].SetValue(dr["rendering_provider_last_name"].ToString());   //"rendering_provider_last_name"

                    string billingNote = " ";
                    switch (dr["County"])
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
                    sheet1Worksheet.Cells[currentRow, esef.GetKey("billing_note")].SetValue(billingNote);   //"billing_note"

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

                EIServiceExportFormat esef = new EIServiceExportFormat();
                string[] columnsList = esef.GetColumns();

                CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                CellIndex firstRowLastCellIndex = new CellIndex(0, columnsList.Length);
                CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, esef.GetKey("consumer_first"));
                CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1, esef.GetKey("rendering_provider_last_name"));
                CellBorder border = new CellBorder(CellBorderStyle.Medium, InvoiceBackground);
                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 0, 0), Colors.Transparent);

                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column.ToString());
                    string columnName = column.ToString();

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