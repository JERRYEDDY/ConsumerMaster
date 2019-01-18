
using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;

namespace ConsumerMaster
{
    public class AWCConsumerExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly int IndexColumnConsumerInternalNumber = 0;
        private static readonly int IndexColumnTradingPartnerString = 1;
        private static readonly int IndexColumnConsumerFirst = 2;
        private static readonly int IndexColumnConsumerLast = 3;
        private static readonly int IndexColumnDateOfBirth = 4;
        private static readonly int IndexColumnAddressLine1 = 5;
        private static readonly int IndexColumnAddressLine2 = 6;
        private static readonly int IndexColumnCity = 7;
        private static readonly int IndexColumnState = 8;
        private static readonly int IndexColumnZipCode = 9;
        private static readonly int IndexColumnIdentifier = 10;
        private static readonly int IndexColumnGender = 11;

        private static readonly int IndexRowItemStart = 0;

        private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);

        Dictionary<int, string> ceHeader = new Dictionary<int, string>
        {
            {0, "consumer_internal_number"},
            {1, "trading_partner_string"},
            {2, "consumer_first"},
            {3, "consumer_last"},
            {4, "date_of_birth"},
            {5, "address_line_1"},
            {6, "address_line_2"},
            {7, "city"},
            {8, "state"},
            {9, "zip_code"},
            {10, "identifier"},
            {11, "gender"}
        };

        public Workbook CreateWorkbook()
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
                                 "INNER JOIN TradingPartners AS tp ON  ctp.trading_partner_id = tp.id WHERE ctp.trading_partner_id = 5 ORDER BY consumer_last"; //Agency With Choice = 5

                Utility util = new Utility();
                DataTable ceDataTable = util.GetDataTable(ceQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareWorksheet(worksheet, totalConsumers);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber].SetValue(dr["consumer_internal_number"].ToString());
                    CellSelection cellLeadingZeros1 = worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber];

                    worksheet.Cells[currentRow, IndexColumnTradingPartnerString].SetValue(dr["trading_partner_string"].ToString());
                    worksheet.Cells[currentRow, IndexColumnConsumerFirst].SetValue(dr["consumer_first"].ToString());
                    worksheet.Cells[currentRow, IndexColumnConsumerLast].SetValue(dr["consumer_last"].ToString());
                    worksheet.Cells[currentRow, IndexColumnDateOfBirth].SetValue(dr["date_of_birth"].ToString());
                    worksheet.Cells[currentRow, IndexColumnAddressLine1].SetValue(dr["address_line_1"].ToString());
                    worksheet.Cells[currentRow, IndexColumnAddressLine2].SetValue(dr["address_line_2"].ToString());
                    worksheet.Cells[currentRow, IndexColumnCity].SetValue(dr["city"].ToString());
                    worksheet.Cells[currentRow, IndexColumnState].SetValue(dr["state"].ToString());
                    worksheet.Cells[currentRow, IndexColumnZipCode].SetValue(dr["zip_code"].ToString());

                    CellValueFormat identifierCellValueFormat = new CellValueFormat("0000000000");
                    worksheet.Cells[currentRow, IndexColumnIdentifier].SetFormat(identifierCellValueFormat);
                    worksheet.Cells[currentRow, IndexColumnIdentifier].SetValue(dr["identifier"].ToString());

                    worksheet.Cells[currentRow, IndexColumnGender].SetValue(dr["gender"].ToString());

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
                int lastItemIndexRow = IndexRowItemStart + itemsCount;

                CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
                CellIndex firstRowLastCellIndex = new CellIndex(0, 11);
                CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnConsumerInternalNumber);
                CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnGender);

                CellBorder border = new CellBorder(CellBorderStyle.Medium, InvoiceBackground);

                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetValue(ceHeader[0]);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetValue(ceHeader[1]);
                worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetValue(ceHeader[2]);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetValue(ceHeader[3]);
                worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnDateOfBirth].SetValue(ceHeader[4]);
                worksheet.Cells[IndexRowItemStart, IndexColumnDateOfBirth].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnAddressLine1].SetValue(ceHeader[5]);
                worksheet.Cells[IndexRowItemStart, IndexColumnAddressLine1].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnAddressLine2].SetValue(ceHeader[6]);
                worksheet.Cells[IndexRowItemStart, IndexColumnAddressLine2].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnCity].SetValue(ceHeader[7]);
                worksheet.Cells[IndexRowItemStart, IndexColumnCity].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnState].SetValue(ceHeader[8]);
                worksheet.Cells[IndexRowItemStart, IndexColumnState].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnZipCode].SetValue(ceHeader[9]);
                worksheet.Cells[IndexRowItemStart, IndexColumnZipCode].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnIdentifier].SetValue(ceHeader[10]);
                worksheet.Cells[IndexRowItemStart, IndexColumnIdentifier].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                worksheet.Cells[IndexRowItemStart, IndexColumnGender].SetValue(ceHeader[11]);
                worksheet.Cells[IndexRowItemStart, IndexColumnGender].SetHorizontalAlignment(RadHorizontalAlignment.Left);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}