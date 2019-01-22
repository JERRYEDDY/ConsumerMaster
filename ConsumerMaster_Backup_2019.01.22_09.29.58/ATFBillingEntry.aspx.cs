using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.ServiceModel.Description;
using System.Web.UI;
using Telerik.Web.Spreadsheet;


namespace ConsumerMaster
{
    public partial class ATFBillingEntry : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        protected global::Telerik.Web.UI.RadSpreadsheet RadSpreadsheet1;

        private static readonly int IndexColumnConsumerName = 0;
        private static readonly int IndexColumnRateCode = 1;
        private static readonly int IndexColumnFacility = 2;
        private static readonly int IndexColumnCommunity = 3;
        private static readonly int IndexColumnTotal = 4;
        private static readonly int IndexColumnPercentage = 5;
        private static readonly int IndexColumnBillingRate = 6;
        private static readonly int IndexColumnComProcCode = 7;

        private static readonly int IndexRowItemStart = 0;
        private static readonly int IndexColumnName = 0;

       //private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);

        static Dictionary<int, string> beHeader = new Dictionary<int, string>
        {
            {0, "ConsumerName"},
            {1, "RateCode"},
            {2, "Facility"},
            {3, "Community"},
            {4, "Total"},
            {5, "Percentage"},
            {6, "BillingRate"},
            {7, "ComProcCode"}
        };



        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable data = GetData();
            //var sheet1 = FillWorksheet(data);
            var sheet1 = FillWorksheet1();
            RadSpreadsheet1.Sheets.Add(sheet1);
        }

        public DataTable GetData()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Price", typeof(int));
            table.Columns.Add("Quantity", typeof(int));

            table.Rows.Add("Product1", 100, 2);
            table.Rows.Add("Product2", 150, 10);
            table.Rows.Add("Product3", 120, 5);
            table.Rows.Add("Product4", 300, 10);

            return table;
        }

        private static Worksheet FillWorksheet(DataTable data)
        {
            var workbook = new Workbook();
            var sheet = workbook.AddSheet();
            sheet.Columns = new List<Column>();

            var row = new Row() { Index = 0 };
            int columnIndex = 0;

            foreach (DataColumn dataColumn in data.Columns)
            {
                sheet.Columns.Add(new Column());
                string cellValue = dataColumn.ColumnName;
                var cell = new Cell() { Index = columnIndex++, Value = cellValue, Bold = true };
                row.AddCell(cell);
            }

            sheet.AddRow(row);

            int rowIndex = 1;
            foreach (DataRow dataRow in data.Rows)
            {
                row = new Row() { Index = rowIndex++ };
                columnIndex = 0;
                foreach (DataColumn dataColumn in data.Columns)
                {
                    string cellValue = dataRow[dataColumn.ColumnName].ToString();
                    var cell = new Cell() { Index = columnIndex++, Value = cellValue };
                    row.AddCell(cell);
                }
                sheet.AddRow(row);
            }

            return sheet;
        }

        private static Worksheet FillWorksheet1()
        {
            var workbook = new Workbook();
            var worksheet = workbook.AddSheet();
            worksheet.Columns = new List<Column>();

            var row = new Row() { Index = 0 };
            foreach(KeyValuePair<int, string> i in beHeader)
            {
                Column col = new Column() {Width = 100};
                worksheet.Columns.Add(col);

                var cell = new Cell() {Index = i.Key, Value = i.Value, Bold = true};
                row.AddCell(cell);
            }

            worksheet.AddRow(row);

            int rowIndex = 1;
            row = new Row() { Index = rowIndex++ };

            int columnIndex = 0;

            var cell0 = new Cell() { Index = columnIndex++ }; //A
            row.AddCell(cell0);

            var cell1 = new Cell() { Index = columnIndex++ }; //B
            row.AddCell(cell1);

            var cell2 = new Cell() { Index = columnIndex++ }; //C
            row.AddCell(cell2);

            var cell3 = new Cell() { Index = columnIndex++ }; //D
            row.AddCell(cell3);

            var cell4 = new Cell() { Index = columnIndex++, Formula = "=C2+D2"}; //E
            row.AddCell(cell4);

            var cell5 = new Cell() { Index = columnIndex++, Formula = "=IFERROR(D2/E2,0)", Format = "00.00%" }; //F
            row.AddCell(cell5);

            worksheet.AddRow(row);

            //        
            //    }
            //    
            //    foreach (DataColumn dataColumn in data.Columns)
            //    {



            //foreach (DataRow dataRow in data.Rows)
            //{
            //    row = new Row() { Index = rowIndex++ };
            //    columnIndex = 0;
            //    foreach (DataColumn dataColumn in data.Columns)
            //    {
            //        string cellValue = dataRow[dataColumn.ColumnName].ToString();
            //        var cell = new Cell() { Index = columnIndex++, Value = cellValue };
            //        row.AddCell(cell);
            //    }
            //    sheet.AddRow(row);
            //}

            return worksheet;
        }

    }
}