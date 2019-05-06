﻿
using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;

namespace ConsumerMaster
{
    public class ConsumerExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateWorkbook(string tradingPartnerID)
        {
            Workbook workbook = new Workbook();

            try
            {
                workbook.Sheets.Add(SheetType.Worksheet);
                Worksheet worksheet = workbook.ActiveWorksheet;

                string ceQuery = "SELECT c.consumer_internal_number AS consumer_internal_number, tp.symbol AS trading_partner_string, c.consumer_first AS consumer_first, " +
                                 "c.consumer_last AS consumer_last, c.date_of_birth AS date_of_birth, c.address_line_1 AS address_line_1, c.address_line_2 AS address_line_2, " +
                                 "c.city AS city, c.state AS state, c.zip_code AS zip_code, c.identifier AS identifier, c.gender AS gender FROM Consumers AS c " +
                                 "INNER JOIN TradingPartners AS tp ON " + tradingPartnerID + " = tp.id"
                " WHERE c.trading_partner_id1 = " + tradingPartnerID + " OR c.trading_partner_id2 = " + tradingPartnerID + " OR c.trading_partner_id3 = " + tradingPartnerID +
                    " ORDER BY consumer_last";

                string seQuery =
                    "SELECT c.consumer_first AS consumer_first, c.consumer_last AS consumer_last, c.consumer_internal_number AS consumer_internal_number," +
                    " tp.symbol AS trading_partner_string, 'waiver' AS trading_partner_program_string, ' ' AS start_date_string, ' ' AS end_date_string, " +
                    "c.diagnosis AS diagnosis_code_1_code, ' ' AS composite_procedure_code_string, ' ' AS units, ' ' AS manual_billable_rate, ' ' AS prior_authorization_number, " +
                    " ' ' AS referral_number, ' ' AS referring_provider_id, ' ' AS referring_provider_first_name, ' ' AS referring_provider_last_name, ' ' AS rendering_provider_id, " +
                    "' ' AS rendering_provider_first_name, ' ' AS rendering_provider_last_name FROM Consumers AS c " +
                    "INNER JOIN TradingPartners AS tp ON " + tradingPartnerID + " = tp.id" +






                if (!String.Equals(tradingPartnerId,"0"))
                {
                    ceQuery += " WHERE ctp.trading_partner_id = " + tradingPartnerId;
                }

                ceQuery += " ORDER BY consumer_last";

                Utility util = new Utility();
                ConsumerExportFormat cef = new ConsumerExportFormat();
                DataTable ceDataTable = util.GetDataTable(ceQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    worksheet.Cells[currentRow, cef.GetIndex("consumer_internal_number")].SetValue(dr["consumer_internal_number"].ToString());
                    //CellSelection cellLeadingZeros1 = worksheet.Cells[currentRow, cef.GetIndex("consumer_internal_number")];

                    worksheet.Cells[currentRow, cef.GetIndex("trading_partner_string")].SetValue(dr["trading_partner_string"].ToString());
                    worksheet.Cells[currentRow, cef.GetIndex("consumer_first")].SetValue(dr["consumer_first"].ToString());
                    worksheet.Cells[currentRow, cef.GetIndex("consumer_last")].SetValue(dr["consumer_last"].ToString());
                    worksheet.Cells[currentRow, cef.GetIndex("date_of_birth")].SetValue(dr["date_of_birth"].ToString());
                    worksheet.Cells[currentRow, cef.GetIndex("address_line_1")].SetValue(dr["address_line_1"].ToString());

                    string addressLine2 = dr["address_line_2"] == null ? string.Empty : dr["address_line_2"].ToString(); 
                    worksheet.Cells[currentRow, cef.GetIndex("address_line_2")].SetValue(addressLine2);

                    worksheet.Cells[currentRow, cef.GetIndex("city")].SetValue(dr["city"].ToString());
                    worksheet.Cells[currentRow, cef.GetIndex("state")].SetValue(dr["state"].ToString());
                    worksheet.Cells[currentRow, cef.GetIndex("zip_code")].SetValue(dr["zip_code"].ToString());

                    CellValueFormat identifierCellValueFormat = new CellValueFormat("0000000000");
                    worksheet.Cells[currentRow, cef.GetIndex("identifier")].SetFormat(identifierCellValueFormat);
                    worksheet.Cells[currentRow, cef.GetIndex("identifier")].SetValue(dr["identifier"].ToString());

                    worksheet.Cells[currentRow, cef.GetIndex("gender")].SetValue(dr["gender"].ToString());

                    currentRow++;
                }

                for (int i = 0; i < ceDataTable.Columns.Count; i++)
                {
                    worksheet.Columns[i].AutoFitWidth();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }

        private void PrepareWorksheet(Worksheet worksheet)
        {
            try
            {
                ConsumerExportFormat cef = new ConsumerExportFormat();
                string[] columnsList = cef.ColumnStrings;

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
    }
}