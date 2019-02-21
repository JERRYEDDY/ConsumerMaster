using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Font;
using iText.Layout.Element;
using iText.IO.Font;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using iText.IO.Util;


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

                string seQuery =
                    "SELECT c.consumer_first AS consumer_first, c.consumer_last AS consumer_last, c.consumer_internal_number AS consumer_internal_number, " +
                    "tp.symbol AS trading_partner_string, 'waiver' AS trading_partner_program_string, ' ' AS start_date_string, ' ' AS end_date_string," +
                    "c.diagnosis AS diagnosis_code_1_code, ' ' AS composite_procedure_code_string, ' ' AS units, ' ' AS manual_billable_rate, ' ' AS prior_authorization_number, " +
                    "' ' AS referral_number, ' ' AS referring_provider_id, ' ' AS referring_provider_first_name, ' ' AS referring_provider_last_name, " +
                    "' ' AS rendering_provider_id, ' ' AS rendering_provider_first_name, ' ' AS rendering_provider_last_name FROM Consumers AS c " +
                    "INNER JOIN ConsumerTradingPartner AS ctp ON c.consumer_internal_number = ctp.consumer_internal_number " +
                    "INNER JOIN TradingPartners AS tp ON  ctp.trading_partner_id = tp.id WHERE ctp.trading_partner_id = 3 ORDER BY consumer_last";

                DataTable seDataTable = util.GetDataTable(seQuery);

                //CultureInfo culture = new CultureInfo("en-US");
                //DateTime startDateTime = Convert.ToDateTime("01/13/2019 12:00:00 AM", culture);
                //DateTime endDateTime = Convert.ToDateTime("01/19/2019 12:59:59 PM", culture);
                //DataTable consumersTable = GetAttendanceData(startDateTime, endDateTime);

                int totalConsumers = seDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet, totalConsumers);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in seDataTable.Rows)
                {
                    //string[] names = dr["FullName"].ToString().Split(',');
                    sheet1Worksheet.Cells[currentRow, IndexColumnConsumerFirst].SetValue("consumer_first");
                    sheet1Worksheet.Cells[currentRow, IndexColumnConsumerLast].SetValue("consumer_last");

                    sheet1Worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber].SetValue(dr["consumer_internal_number"].ToString());
                    //CellSelection cellLeadingZeros1 = sheet1Worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber];

                    //Int32.TryParse(dr["Site"].ToString(),out int site);
                    //string tradingPartnerString = (site == 1) ? "atf_bill_george" : "atf_jefferson";
                    sheet1Worksheet.Cells[currentRow, IndexColumnTradingPartnerString].SetValue("trading_partner_string");

                    //string tradingPartnerProgramString = "waiver";
                    sheet1Worksheet.Cells[currentRow, IndexColumnTradingPartnerProgramString].SetValue("trading_partner_program_string");

                    sheet1Worksheet.Cells[currentRow, IndexColumnStartDateString].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, IndexColumnEndDateString].SetValue(" ");

                    sheet1Worksheet.Cells[currentRow, IndexColumnDiagnosisCode1Code].SetValue(dr["diagnosis_code_1_code"].ToString());

                    sheet1Worksheet.Cells[currentRow, IndexColumnCompositeProcedureCodeString].SetValue(dr["composite_procedure_code_string"].ToString());
                    CellIndex dataValidationRuleCellIndex = new CellIndex(currentRow, IndexColumnCompositeProcedureCodeString);

                    sheet1Worksheet.Cells[currentRow, IndexColumnUnits].SetValue("units");

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

        //private void PrepareSheet1Worksheet(Worksheet worksheet, int itemsCount)
        //{
        //    try
        //    {
        //        int lastItemIndexRow = IndexRowItemStart + itemsCount;

        //        CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
        //        CellIndex firstRowLastCellIndex = new CellIndex(0, 22);
        //        CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnConsumerFirst);
        //        CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnRenderingProviderLastName);
        //        CellBorder border = new CellBorder(CellBorderStyle.Medium, InvoiceBackground);
        //        PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255,255,0,0), Colors.Transparent);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetValue(ceHeader[IndexColumnConsumerFirst]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetHorizontalAlignment(RadHorizontalAlignment.Left);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetFill(solidPatternFill);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetValue(ceHeader[IndexColumnConsumerLast]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetHorizontalAlignment(RadHorizontalAlignment.Left);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetFill(solidPatternFill);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetValue(ceHeader[IndexColumnConsumerInternalNumber]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetValue(ceHeader[IndexColumnTradingPartnerString]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerProgramString].SetValue(ceHeader[IndexColumnTradingPartnerProgramString]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerProgramString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnStartDateString].SetValue(ceHeader[IndexColumnStartDateString]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnStartDateString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnEndDateString].SetValue(ceHeader[IndexColumnEndDateString]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnEndDateString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnDiagnosisCode1Code].SetValue(ceHeader[IndexColumnDiagnosisCode1Code]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnDiagnosisCode1Code].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnCompositeProcedureCodeString].SetValue(ceHeader[IndexColumnCompositeProcedureCodeString]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnCompositeProcedureCodeString].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        //worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetValue(ceHeader[IndexColumnHours]);
        //        //worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetHorizontalAlignment(RadHorizontalAlignment.Left);
        //        //worksheet.Cells[IndexRowItemStart, IndexColumnHours].SetFill(solidPatternFill);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnUnits].SetValue(ceHeader[IndexColumnUnits]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnUnits].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnManualBillableRate].SetValue(ceHeader[IndexColumnManualBillableRate]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnManualBillableRate].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnPriorAuthorizationNumber].SetValue(ceHeader[IndexColumnPriorAuthorizationNumber]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnPriorAuthorizationNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnReferralNumber].SetValue(ceHeader[IndexColumnReferralNumber]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnReferralNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderId].SetValue(ceHeader[IndexColumnReferringProviderId]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderId].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderFirstName].SetValue(ceHeader[IndexColumnReferringProviderFirstName]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderFirstName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderLastName].SetValue(ceHeader[IndexColumnReferringProviderLastName]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnReferringProviderLastName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderId].SetValue(ceHeader[IndexColumnRenderingProviderId]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderId].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderFirstName].SetValue(ceHeader[IndexColumnRenderingProviderFirstName]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderFirstName].SetHorizontalAlignment(RadHorizontalAlignment.Left);

        //        worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderLastName].SetValue(ceHeader[IndexColumnRenderingProviderLastName]);
        //        worksheet.Cells[IndexRowItemStart, IndexColumnRenderingProviderLastName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}

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

        public void ProcessDataTable(Table table, String line, PdfFont font, bool isHeader)
        {
            var tokenizer = new StringTokenizer(line, ";");
            while (tokenizer.HasMoreTokens())
            {
                if (isHeader)
                {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(tokenizer.NextToken()).SetFont(font)));
                }
                else
                {
                    table.AddCell(new Cell().Add(new Paragraph(tokenizer.NextToken()).SetFont(font)));
                }
            }
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


