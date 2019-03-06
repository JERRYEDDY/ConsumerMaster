using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Media;

namespace ConsumerMaster
{
    public class ATFConsumerRatioReport
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 3;
        private static readonly int IndexColumnName = 0;

        public Workbook CreateWorkbook(DateTime startDate, DateTime endDate, int Site)
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];

                ConsumerRatioReportFormat crrf = new ConsumerRatioReportFormat();
                DataTable crrDataTable = GetAttendanceData(startDate, endDate, Site);

                int totalConsumers = crrDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet);

                CellIndex A1Cell = new CellIndex(0, 0);
                CellIndex D1Cell = new CellIndex(0, 3);
                sheet1Worksheet.Cells[A1Cell, D1Cell].Merge();
                sheet1Worksheet.Cells[0, 0].SetValue("Consumers Ratio Report");
                sheet1Worksheet.Cells[0, 0].SetIsBold(true);
                sheet1Worksheet.Cells[0, 0].SetFontSize(20);

                CellIndex A2Cell = new CellIndex(1, 0);
                CellIndex D2Cell = new CellIndex(1, 3);
                sheet1Worksheet.Cells[A2Cell, D2Cell].Merge();
                string rangeText = "Range: " + startDate.ToString("d") + "-" + endDate.ToString("d");
                sheet1Worksheet.Cells[1, 0].SetValue(rangeText);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in crrDataTable.Rows)
                {
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Site")].SetValue(dr["Site"].ToString());
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetValue(dr["FullName"].ToString());
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Ratio1")].SetValue(dr["Ratio1"].ToString());
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Ratio2")].SetValue(dr["Ratio2"].ToString());
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units1")].SetValue(dr["Units1"].ToString());
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units2")].SetValue(dr["Units2"].ToString());
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetValue(dr["Total"].ToString());
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Pct1")].SetValue(dr["Pct1"].ToString());
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Pct2")].SetValue(dr["Pct2"].ToString());

                    currentRow++;
                }

                ThemableColor black = new ThemableColor(Colors.Black);
                CellBorders blackBorders = new CellBorders(
                    new CellBorder(CellBorderStyle.None, black),   // Left border 
                    new CellBorder(CellBorderStyle.Medium, black),   // Top border 
                    new CellBorder(CellBorderStyle.None, black),   // Right border 
                    new CellBorder(CellBorderStyle.None, black),   // Bottom border 
                    new CellBorder(CellBorderStyle.None, black),       // Inside horizontal border 
                    new CellBorder(CellBorderStyle.None, black),       // Inside vertical border 
                    new CellBorder(CellBorderStyle.None, black),     // Diagonal up border 
                    new CellBorder(CellBorderStyle.None, black));    // Diagonal down border 

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units1")].SetBorders(blackBorders);
                string sumUnits1 = "=SUM(E2:E" + currentRow + ")";
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units1")].SetValue(sumUnits1);

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units2")].SetBorders(blackBorders);
                string sumUnits2 = "=SUM(F2:F" + currentRow + ")";
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units2")].SetValue(sumUnits2);

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetBorders(blackBorders);

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetHorizontalAlignment(RadHorizontalAlignment.Right);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetIsBold(true);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetValue("Total:");
                string sumTotal = "=SUM(G2:G" + currentRow + ")";
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetValue(sumTotal);

                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("FullName")].SetHorizontalAlignment(RadHorizontalAlignment.Right);
                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("FullName")].SetIsBold(true);
                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("FullName")].SetValue("Average:");
                CellValueFormat decimalFormat = new CellValueFormat("0.00");
                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("Total")].SetFormat(decimalFormat);
                string avgTotal = "=AVERAGE(G2:G" + currentRow + ")";
                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("Total")].SetValue(avgTotal);

                for (int i = 0; i < crrDataTable.Columns.Count; i++)
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

        private void PrepareSheet1Worksheet(Worksheet worksheet)
        {
            try
            {
                //int lastItemIndexRow = IndexRowItemStart + itemsCount;

                ConsumerRatioReportFormat crrf = new ConsumerRatioReportFormat();
                string[] columnsList = crrf.ColumnStrings;
                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column);
                    string columnName = column;

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetIsBold(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public DataTable GetAttendanceData(DateTime startDateTime, DateTime endDateTime, int Site)
        {
            DataTable consumersTable = new DataTable("Consumers");
            consumersTable.Columns.Add("Site", typeof(int));
            consumersTable.Columns.Add("FullName", typeof(string));
            consumersTable.Columns.Add("Ratio1", typeof(string));
            consumersTable.Columns.Add("Ratio2", typeof(string));
            consumersTable.Columns.Add("Units1", typeof(int));
            consumersTable.Columns.Add("Units2", typeof(int));
            consumersTable.Columns.Add("Total", typeof(int));
            consumersTable.Columns.Add("Pct1", typeof(string));
            consumersTable.Columns.Add("Pct2", typeof(string));

            using (SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringAttendance"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetConsumersData", sqlConnection1))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@StartDateTime", SqlDbType.Text).Value = startDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters.Add("@EndDateTime", SqlDbType.Text).Value = endDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters.Add("@Site", SqlDbType.Int).Value = Site;

                    sqlConnection1.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var row = consumersTable.NewRow();
                            row["Site"] = dr.IsDBNull(0) ? 0 : dr.GetInt32(0);
                            //string maNumber = dr.IsDBNull(1) ? String.Empty : dr.GetString(1);
                            row["FullName"] = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                            row["Ratio1"] = dr.IsDBNull(3) ? string.Empty : dr.GetString(3);
                            row["Ratio2"] = dr.IsDBNull(4) ? string.Empty : dr.GetString(4);
                            row["Units1"] = dr.IsDBNull(5) ? 0 : dr.GetInt32(5);
                            row["Units2"] = dr.IsDBNull(6) ? 0 : dr.GetInt32(6);

                            int units1 = dr.IsDBNull(5) ? 0 : dr.GetInt32(5);
                            int units2 = dr.IsDBNull(6) ? 0 : dr.GetInt32(6);
                            int total = units1 + units2;
                            row["Total"] = total;

                            double pct1 = (total == 0) ? 0 : units1 / (double)total;
                            double pct2 = (total == 0) ? 0 : units2 / (double)total;
                            row["Pct1"] = $"{pct1:P2}";
                            row["Pct2"] = $"{pct2:P2}";

                            consumersTable.Rows.Add(row);
                        }
                        dr.Close();
                    }
                }
            }
            return consumersTable;
        }
    }
}


