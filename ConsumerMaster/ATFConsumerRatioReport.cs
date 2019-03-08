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
        private static readonly int IndexRowItemStart = 4;
        private static readonly int IndexColumnName = 0;

        public Workbook CreateWorkbook(DateTime startDate, DateTime endDate, int siteId, string siteName)
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];

                ConsumerRatioReportFormat crrf = new ConsumerRatioReportFormat();
                DataTable crrDataTable = GetAttendanceData(startDate, endDate, siteId);

                int totalConsumers = crrDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet);

                CellIndex A1Cell = new CellIndex(0, 0);
                CellIndex D1Cell = new CellIndex(0, 3);
                sheet1Worksheet.Cells[A1Cell, D1Cell].Merge();
                sheet1Worksheet.Cells[0, 0].SetValue("ATF Consumers Ratio Report");
                sheet1Worksheet.Cells[0, 0].SetIsBold(true);
                sheet1Worksheet.Cells[0, 0].SetFontSize(20);

                sheet1Worksheet.Cells[1, 0].SetValue("Site: ");
                CellIndex B2Cell = new CellIndex(1, 1);
                CellIndex D2Cell = new CellIndex(1, 3);
                sheet1Worksheet.Cells[B2Cell, D2Cell].Merge();
                sheet1Worksheet.Cells[1, 1].SetValue(siteName);

                CellIndex A3Cell = new CellIndex(2, 0);
                CellIndex D3Cell = new CellIndex(2, 3);
                sheet1Worksheet.Cells[A3Cell, D3Cell].Merge();
                string rangeText = "Range: " + startDate.ToString("d") + "-" + endDate.ToString("d");
                sheet1Worksheet.Cells[2, 0].SetValue(rangeText);

                sheet1Worksheet.Columns[crrf.GetIndex("Site")].SetWidth(new ColumnWidth(40, false));

                sheet1Worksheet.Columns[crrf.GetIndex("FullName")].SetWidth(new ColumnWidth(200, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Ratio1")].SetWidth(new ColumnWidth(80, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Ratio2")].SetWidth(new ColumnWidth(80, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Units1")].SetWidth(new ColumnWidth(75, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Units2")].SetWidth(new ColumnWidth(75, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Total")].SetWidth(new ColumnWidth(80, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Pct1")].SetWidth(new ColumnWidth(80, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Pct2")].SetWidth(new ColumnWidth(80, false));

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in crrDataTable.Rows)
                {
                    string site = dr["Site"].ToString();
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Site")].SetValue(site);                                //Column A
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Site")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetValue(dr["FullName"].ToString());       //Column B

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Ratio1")].SetValue(dr["Ratio1"].ToString());           //Column C    
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Ratio1")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Ratio2")].SetValue(dr["Ratio2"].ToString());           //Column D   
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Ratio2")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units1")].SetValue(dr["Units1"].ToString());           //Column E
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units1")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units2")].SetValue(dr["Units2"].ToString());           //Column F
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units2")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetValue(dr["Total"].ToString());             //Column G
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Pct1")].SetValue(dr["Pct1"].ToString());              //Column H
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Pct1")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Pct2")].SetValue(dr["Pct2"].ToString());              //Column I
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Pct2")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

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
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units1")].SetValue("=SUM(E2:E" + currentRow + ")");

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units2")].SetBorders(blackBorders);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units2")].SetValue("=SUM(F2:F" + currentRow + ")");

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetHorizontalAlignment(RadHorizontalAlignment.Right);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetIsBold(true);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetValue("Total:");

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetValue("=SUM(G2:G" + currentRow + ")");

                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("FullName")].SetHorizontalAlignment(RadHorizontalAlignment.Right);
                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("FullName")].SetIsBold(true);
                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("FullName")].SetValue("Average:");

                CellValueFormat decimalFormat = new CellValueFormat("0.00");
                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("Total")].SetFormat(decimalFormat);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetBorders(blackBorders);
                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("Total")].SetValue("=AVERAGE(G2:G" + currentRow + ")");

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
                ConsumerRatioReportFormat crrf = new ConsumerRatioReportFormat();
                string[] columnsList = crrf.HeaderStrings;
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

        public DataTable GetAttendanceData(DateTime startDateTime, DateTime endDateTime, int siteId)
        {
            DataTable consumersTable = new DataTable("Consumers");

            try
            {
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
                        cmd.Parameters.Add("@SiteId", SqlDbType.Int).Value = siteId;

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
                                row["Pct1"] = $"{pct1:P0}";
                                row["Pct2"] = $"{pct2:P0}";

                                consumersTable.Rows.Add(row);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return consumersTable;
        }
    }
}


