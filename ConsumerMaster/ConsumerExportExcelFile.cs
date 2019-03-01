
using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;

namespace ConsumerMaster
{
    public class ConsumerExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly int IndexRowItemStart = 0;
        private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);


        public Workbook CreateWorkbook(string tradingPartnerId)
        {
            Workbook workbook = new Workbook();

            try
            {
                workbook.Sheets.Add(SheetType.Worksheet);
                Worksheet worksheet = workbook.ActiveWorksheet;

                string ceQuery = "SELECT c.consumer_internal_number AS consumer_internal_number, tp.symbol AS trading_partner_string, c.consumer_first AS consumer_first, " +
                                 "c.consumer_last AS consumer_last, c.date_of_birth AS date_of_birth, c.address_line_1 AS address_line_1, c.address_line_2 AS address_line_2, " +
                                 "c.city AS city, c.state AS state, c.zip_code AS zip_code, c.identifier AS identifier, c.gender AS gender FROM Consumers AS c " +
                                 "INNER JOIN ConsumerTradingPartner AS ctp ON c.consumer_internal_number = ctp.consumer_internal_number " +
                                 "INNER JOIN TradingPartners AS tp ON ctp.trading_partner_id = tp.id";

                if(!String.Equals(tradingPartnerId,"0"))
                {
                    ceQuery += " WHERE ctp.trading_partner_id = " + tradingPartnerId;
                }

                ceQuery += " ORDER BY consumer_last";

                Utility util = new Utility();
                ConsumerExportFormat cef = new ConsumerExportFormat();
                DataTable ceDataTable = util.GetDataTable(ceQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareWorksheet(worksheet, totalConsumers);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    worksheet.Cells[currentRow, cef.GetKey("consumer_internal_number")].SetValue(dr["consumer_internal_number"].ToString());
                    //CellSelection cellLeadingZeros1 = worksheet.Cells[currentRow, cef.GetKey("consumer_internal_number")];

                    worksheet.Cells[currentRow, cef.GetKey("trading_partner_string")].SetValue(dr["trading_partner_string"].ToString());
                    worksheet.Cells[currentRow, cef.GetKey("consumer_first")].SetValue(dr["consumer_first"].ToString());
                    worksheet.Cells[currentRow, cef.GetKey("consumer_last")].SetValue(dr["consumer_last"].ToString());
                    worksheet.Cells[currentRow, cef.GetKey("date_of_birth")].SetValue(dr["date_of_birth"].ToString());
                    worksheet.Cells[currentRow, cef.GetKey("address_line_1")].SetValue(dr["address_line_1"].ToString());

                    string addressLine2 = dr["address_line_2"] == null ? string.Empty : dr["address_line_2"].ToString(); 
                    worksheet.Cells[currentRow, cef.GetKey("address_line_2")].SetValue(addressLine2);

                    worksheet.Cells[currentRow, cef.GetKey("city")].SetValue(dr["city"].ToString());
                    worksheet.Cells[currentRow, cef.GetKey("state")].SetValue(dr["state"].ToString());
                    worksheet.Cells[currentRow, cef.GetKey("zip_code")].SetValue(dr["zip_code"].ToString());

                    CellValueFormat identifierCellValueFormat = new CellValueFormat("0000000000");
                    worksheet.Cells[currentRow, cef.GetKey("identifier")].SetFormat(identifierCellValueFormat);
                    worksheet.Cells[currentRow, cef.GetKey("identifier")].SetValue(dr["identifier"].ToString());

                    worksheet.Cells[currentRow, cef.GetKey("gender")].SetValue(dr["gender"].ToString());

                    currentRow++;
                }

                for (int i = 0; i < worksheet.Columns.Count; i++)
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

        private void PrepareWorksheet(Worksheet worksheet, int itemsCount)
        {
            try
            {
                //int lastItemIndexRow = IndexRowItemStart + itemsCount;

                ConsumerExportFormat cef = new ConsumerExportFormat();
                string[] columnsList = cef.GetColumns();

                //CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                //CellIndex firstRowLastCellIndex = new CellIndex(0, columnsList.Length);
                //CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, cef.GetKey("consumer_first"));
                //CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1,cef.GetKey("gender"));
                //CellBorder border = new CellBorder(CellBorderStyle.Medium, InvoiceBackground);

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