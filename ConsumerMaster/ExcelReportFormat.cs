using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ExcelReportFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        //string[] travelTimeColumns = new[] { "StaffID", "StaffName", "ClientID", "ClientName", "Start", "Finish", "Duration" };

        Worksheet worksheet;
        public string[] columnNames;

        public ExcelReportFormat(Worksheet wsheet, string[] columns)
        {
            worksheet = wsheet;
            columnNames = columns;
        
        }

        public void CreateHeader()
        {
            try
            {
                foreach (string columnName in columnNames)
                {
                    int columnKey = Array.IndexOf(columnNames, columnName);

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void CreateDataColumn(int currentRow, string columnName, string columnValue)
        {
            try
            { 
                worksheet.Cells[currentRow, Array.IndexOf(columnNames, columnName)].SetValue(columnValue);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public Workbook LoadWorkbook(DataTable dTable)
        {
            Workbook workbook = new Workbook();
            workbook.Sheets.Add(SheetType.Worksheet);
            Worksheet worksheet = workbook.ActiveWorksheet;

            //List<string> columnList = new List<string>();

            int RowItemStart = 0;
            foreach (DataColumn column in dTable.Columns)
            {
                int columnKey = column.Ordinal;

                worksheet.Cells[RowItemStart, columnKey].SetValue(column.ColumnName);
                worksheet.Cells[RowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
            }

            int currentRow = RowItemStart + 1;
            foreach (DataRow dtRow in dTable.Rows)
            {
                foreach (DataColumn dtColumn in dTable.Columns)
                {
                    int columnKey = dtColumn.Ordinal;
                    worksheet.Cells[currentRow, columnKey].SetValue(dtRow[columnKey].ToString());
                }
                currentRow++;
            }

            return workbook;
        }

        //private string[] GetColumns()
        //{
        //    StringCollection _cols = new StringCollection();
        //    try
        //    {
        //        foreach (var column in _columnNameList)
        //        {
        //            _cols.Add(column.Value.Name);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }

        //    return _cols.Cast<string>().ToArray();
        //}

        //public int GetIndex(string value)
        //{
        //    return Array.IndexOf(ColumnStrings, value);
        //}
    }
}