using System.Data;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace ConsumerMaster
{
    public class ExcelReportFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public string Header1 { get; set; }
        public string Header2 { get; set; }
        public string Header3 { get; set; }
        public string Header4 { get; set; }

        public Workbook LoadWorkbook(DataTable dTable)
        {
            Workbook workbook = new Workbook();
            workbook.Sheets.Add(SheetType.Worksheet);
            Worksheet worksheet = workbook.ActiveWorksheet;

            int RowItemStart = 0;
            int currentRow = RowItemStart;

            worksheet.Cells[currentRow++, 0].SetValue(Header1);
            worksheet.Cells[currentRow++, 0].SetValue(Header2);
            worksheet.Cells[currentRow++, 0].SetValue(Header3);
            worksheet.Cells[currentRow++, 0].SetValue(Header4);

            foreach (DataColumn columnHeader in dTable.Columns) //Column Headers
            {
                int columnKey = columnHeader.Ordinal;
                worksheet.Cells[currentRow, columnKey].SetValue(columnHeader.ColumnName);
                worksheet.Cells[currentRow, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
            }
            currentRow++;

            foreach (DataRow dtRow in dTable.Rows)  //Report Data
            {
                foreach (DataColumn dtColumn in dTable.Columns)
                {
                    int columnKey = dtColumn.Ordinal;
                    worksheet.Cells[currentRow, columnKey].SetValue(dtRow[columnKey].ToString());
                }
                currentRow++;
            }

            //for (int i = 0; i < dTable.Columns.Count; i++)
            //{
            //    worksheet.Columns[i].AutoFitWidth();
            //}

            return workbook;
        }
    }
}