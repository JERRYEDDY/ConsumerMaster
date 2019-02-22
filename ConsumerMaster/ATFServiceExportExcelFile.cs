using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Configuration;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Font;
using iText.Layout.Element;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;

namespace ConsumerMaster
{
    public class ATFServiceExportExcelFile
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

        public Workbook ATFCreateWorkbook()
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
                    "INNER JOIN TradingPartners AS tp ON  ctp.trading_partner_id = tp.id WHERE ctp.trading_partner_id = 3 ORDER BY consumer_last";

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

                    //string tradingPartnerProgramString = "waiver";
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("trading_partner_program_string")].SetValue(dr["trading_partner_program_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("start_date_string")].SetValue(dr["start_date_string"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("end_date_string")].SetValue(dr["end_date_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("diagnosis_code_1_code")].SetValue(dr["diagnosis_code_1_code"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("composite_procedure_code_string")].SetValue(dr["composite_procedure_code_string"].ToString());
                    CellIndex dataValidationRuleCellIndex = new CellIndex(currentRow, sef.GetKey("composite_procedure_code_string"));
                    ListDataValidationRuleContext context = new ListDataValidationRuleContext(sheet1Worksheet, dataValidationRuleCellIndex);
                    context.InputMessageTitle = "Restricted input";
                    context.InputMessageContent = "The input is restricted to the composite procedure codes.";
                    context.ErrorStyle = ErrorStyle.Stop;
                    context.ErrorAlertTitle = "Wrong value";
                    context.ErrorAlertContent = "The entered value is not valid. Allowed values are the composite procedure codes!";
                    context.InCellDropdown = true;

                    string cpcRange = "=Sheet2!$A$2:$A$" + cpcList.Count + 1;  //= Sheet2!$A$2:$A$73
                    context.Argument1 = cpcRange; //   
                    ListDataValidationRule rule = new ListDataValidationRule(context);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex].SetDataValidationRule(rule);

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("units")].SetValue(dr["units"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetKey("manual_billable_rate")].SetValue(dr["manual_billable_rate"].ToString());                      //"manual_billable_rate"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("prior_authorization_number")].SetValue(dr["prior_authorization_number"].ToString());          //"prior_authorization_number"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referral_number")].SetValue(dr["referral_number"].ToString());                                //"referral_number"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referring_provider_id")].SetValue(dr["referring_provider_id"].ToString());                    //"referring_provider_id"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referring_provider_first_name")].SetValue(dr["referring_provider_first_name"].ToString());    //"referring_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("referring_provider_last_name")].SetValue(dr["referring_provider_last_name"].ToString());      //"referring_provider_last_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_id")].SetValue(dr["rendering_provider_id"].ToString());                    //"rendering_provider_id"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_first_name")].SetValue(dr["rendering_provider_first_name"].ToString());    //"rendering_provider_first_name"
                    sheet1Worksheet.Cells[currentRow, sef.GetKey("rendering_provider_last_name")].SetValue(dr["rendering_provider_last_name"].ToString());      //"rendering_provider_last_name"

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

                ServiceExportFormat sef = new ServiceExportFormat();
                string[] columnsList = sef.GetColumns();

                CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                CellIndex firstRowLastCellIndex = new CellIndex(0, columnsList.Length);
                CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, sef.GetKey("consumer_first"));
                CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1, sef.GetKey("rendering_provider_last_name"));
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

        public void TableToPDF(DataTable dTable, string destinationPath)
        {
            var writer = new PdfWriter(destinationPath);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4.Rotate());
            document.SetMargins(20, 20, 20, 20);
            var font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);
            var bold = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            var table = new Table(new float[] { 4, 1, 3, 3, 3, 3, 3, 3 });
            table.SetWidth(UnitValue.CreatePercentValue(100));

            foreach (DataColumn column in dTable.Columns)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(column.ColumnName).SetFont(font)));
            }

            foreach (DataRow dr in dTable.Rows)
            {
                table.AddCell(new Cell().Add(new Paragraph(dr["FullName"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Ratio1"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Ratio2"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Units1"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Units2"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Total"].ToString()).SetFont(font)));

                table.AddCell(new Cell().Add(new Paragraph(dr["Pct1"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Pct2"].ToString()).SetFont(font)));
            }

            document.Add(table);
            document.Close();
        }

        public DataTable GetAttendanceData(DateTime startDateTime, DateTime endDateTime)
        {
            DataTable consumersTable = new DataTable("Consumers");
            DataColumn consumerCol = consumersTable.Columns.Add("FullName", typeof(String));
            consumersTable.Columns.Add("Ratio1", typeof(String));
            consumersTable.Columns.Add("Ratio2", typeof(String));
            consumersTable.Columns.Add("Units1", typeof(int));
            consumersTable.Columns.Add("Units2", typeof(int));
            consumersTable.Columns.Add("Total", typeof(int));
            consumersTable.Columns.Add("Pct1", typeof(String));
            consumersTable.Columns.Add("Pct2", typeof(String));

            using (SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringAttendance"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetConsumersData", sqlConnection1))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@StartDateTime", SqlDbType.Text).Value = startDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters.Add("@EndDateTime", SqlDbType.Text).Value = endDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                    sqlConnection1.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var row = consumersTable.NewRow();
                            //int site = dr.IsDBNull(0) ? 0 : dr.GetInt32(0);
                            //string maNumber = dr.IsDBNull(1) ? String.Empty : dr.GetString(1);

                            row["FullName"] = dr.IsDBNull(2) ? String.Empty : dr.GetString(2);
                            row["Ratio1"] = dr.IsDBNull(3) ? String.Empty : dr.GetString(3);
                            row["Ratio2"] = dr.IsDBNull(4) ? String.Empty : dr.GetString(4);
                            row["Units1"] = dr.IsDBNull(5) ? 0 : dr.GetInt32(5);
                            row["Units2"] = dr.IsDBNull(6) ? 0 : dr.GetInt32(6);

                            int units1 = dr.IsDBNull(5) ? 0 : dr.GetInt32(5);
                            int units2 = dr.IsDBNull(6) ? 0 : dr.GetInt32(6);
                            int total = units1 + units2;
                            row["Total"] = total;

                            double pct1 = (total == 0) ? 0 : ((double)units1 / (double)total);
                            double pct2 = (total == 0) ? 0 : ((double)units2 / (double)total);
                            row["Pct1"] = string.Format("{0:P2}", pct1);
                            row["Pct2"] = string.Format("{0:P2}", pct2);

                            consumersTable.Rows.Add(row);
                        }
                        dr.Close();
                    }
                }
            }

            string outFileName = @"C:\Billing Software\ATF\ATFConsumerRatio.pdf";
            return consumersTable;
        }
    }
}


