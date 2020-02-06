using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;

namespace ConsumerMaster
{
    public class ATFConsumerRatioReport
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 4;
        //private static readonly int IndexColumnName = 0;

        public Workbook CreateWorkbook(DateTime startDate, DateTime endDate, int siteId, string siteName)
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"];

                HeaderFooterSettings settings = sheet1Worksheet.WorksheetPageSetup.HeaderFooterSettings;
                settings.Footer.CenterSection.Text = "Page &P of &N";

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

                sheet1Worksheet.Columns[crrf.GetIndex("Site")].SetWidth(new ColumnWidth(35, false));
                sheet1Worksheet.Columns[crrf.GetIndex("FullName")].SetWidth(new ColumnWidth(140, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Ratio1")].SetWidth(new ColumnWidth(60, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Ratio2")].SetWidth(new ColumnWidth(60, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Ratio3")].SetWidth(new ColumnWidth(60, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Units1")].SetWidth(new ColumnWidth(60, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Units2")].SetWidth(new ColumnWidth(60, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Units3")].SetWidth(new ColumnWidth(60, false));
                sheet1Worksheet.Columns[crrf.GetIndex("Total")].SetWidth(new ColumnWidth(50, false));
                sheet1Worksheet.Columns[crrf.GetIndex("FacUnits")].SetWidth(new ColumnWidth(70, false));
                sheet1Worksheet.Columns[crrf.GetIndex("ComUnits")].SetWidth(new ColumnWidth(70, false));
                sheet1Worksheet.Columns[crrf.GetIndex("OtherUnits")].SetWidth(new ColumnWidth(60, false));
                sheet1Worksheet.Columns[crrf.GetIndex("ComPct")].SetWidth(new ColumnWidth(60, false));

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

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Ratio3")].SetValue(dr["Ratio3"].ToString());           //Column E   
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Ratio3")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units1")].SetValue(dr["Units1"].ToString());           //Column F
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units1")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units2")].SetValue(dr["Units2"].ToString());           //Column G
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units2")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units3")].SetValue(dr["Units3"].ToString());           //Column H
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Units3")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetValue(dr["Total"].ToString());             //Column I
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FacUnits")].SetValue(dr["FacUnits"].ToString());           //Column J
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FacUnits")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("ComUnits")].SetValue(dr["ComUnits"].ToString());           //Column K
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("ComUnits")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("OtherUnits")].SetValue(dr["OtherUnits"].ToString());           //Column L
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("OtherUnits")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("ComPct")].SetValue(dr["ComPct"].ToString());              //Column M
                    sheet1Worksheet.Cells[currentRow, crrf.GetIndex("ComPct")].SetHorizontalAlignment(RadHorizontalAlignment.Center);

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

                int totalRow = currentRow;
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetValue("=SUM(I2:I" + currentRow + ")");

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FacUnits")].SetBorders(blackBorders);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FacUnits")].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FacUnits")].SetValue("=SUM(J2:J" + currentRow + ")");

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("ComUnits")].SetBorders(blackBorders);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("ComUnits")].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("ComUnits")].SetValue("=SUM(K2:K" + currentRow + ")");

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("OtherUnits")].SetBorders(blackBorders);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("OtherUnits")].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("OtherUnits")].SetValue("=SUM(L2:L" + currentRow + ")");

                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetHorizontalAlignment(RadHorizontalAlignment.Right);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetIsBold(true);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FullName")].SetValue("Total:");

                int averageRow = currentRow + 1;
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("FullName")].SetHorizontalAlignment(RadHorizontalAlignment.Right);
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("FullName")].SetIsBold(true);
                sheet1Worksheet.Cells[currentRow + 1, crrf.GetIndex("FullName")].SetValue("Average:");

                CellValueFormat decimalFormat = new CellValueFormat("0.00");
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("Total")].SetFormat(decimalFormat);
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("Total")].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("Total")].SetBorders(blackBorders);
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("Total")].SetValue("=AVERAGE(I2:I" + currentRow + ")");

                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("FacUnits")].SetFormat(decimalFormat);
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("FacUnits")].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("FacUnits")].SetBorders(blackBorders);
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("FacUnits")].SetValue("=AVERAGE(J2:J" + currentRow + ")");

                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("ComUnits")].SetFormat(decimalFormat);
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("ComUnits")].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("ComUnits")].SetBorders(blackBorders);
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("ComUnits")].SetValue("=AVERAGE(K2:K" + currentRow + ")");

                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("OtherUnits")].SetFormat(decimalFormat);
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("OtherUnits")].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("OtherUnits")].SetBorders(blackBorders);
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("OtherUnits")].SetValue("=AVERAGE(L2:L" + currentRow + ")");

                CellValueFormat percentageFormat = new CellValueFormat("0%");

                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("ComPct")].SetFormat(percentageFormat);
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("ComPct")].SetHorizontalAlignment(RadHorizontalAlignment.Center);
                sheet1Worksheet.Cells[currentRow, crrf.GetIndex("ComPct")].SetBorders(blackBorders);
                string pct2Formula = "=(K" + averageRow + "/I" + averageRow + ")";
                sheet1Worksheet.Cells[averageRow, crrf.GetIndex("ComPct")].SetValue(pct2Formula);
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
                consumersTable.Columns.Add("Ratio3", typeof(string));
                consumersTable.Columns.Add("Units1", typeof(int));
                consumersTable.Columns.Add("Units2", typeof(int));
                consumersTable.Columns.Add("Units3", typeof(int));
                consumersTable.Columns.Add("Total", typeof(int));
                consumersTable.Columns.Add("FacUnits", typeof(int));
                consumersTable.Columns.Add("ComUnits", typeof(int));
                consumersTable.Columns.Add("OtherUnits", typeof(int));
                consumersTable.Columns.Add("ComPct", typeof(string));

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
                            //DataTable dataTable = new DataTable();
                            //DataTable dt = dataTable;
                            //dt.Load(dr);

                            while (dr.Read())
                            {
                                var row = consumersTable.NewRow();
                                row["Site"] = dr.IsDBNull(0) ? 0 : dr.GetInt32(0);
                                //string maNumber = dr.IsDBNull(1) ? String.Empty : dr.GetString(1);

                                row["FullName"] = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                                row["Ratio1"] = dr.IsDBNull(3) ? string.Empty : dr.GetString(3);
                                row["Ratio2"] = dr.IsDBNull(4) ? string.Empty : dr.GetString(4);
                                row["Ratio3"] = dr.IsDBNull(5) ? string.Empty : dr.GetString(5);
                                row["Units1"] = dr.IsDBNull(6) ? 0 : dr.GetInt32(6);
                                row["Units2"] = dr.IsDBNull(7) ? 0 : dr.GetInt32(7);
                                row["Units3"] = dr.IsDBNull(8) ? 0 : dr.GetInt32(8);

                                string[] ratioArray = new string[3];
                                ratioArray[0] = dr.IsDBNull(3) ? string.Empty : dr.GetString(3);
                                ratioArray[1] = dr.IsDBNull(4) ? string.Empty : dr.GetString(4);
                                ratioArray[2] = dr.IsDBNull(5) ? string.Empty : dr.GetString(5);

                                int[] unitsArray = new int[3];
                                unitsArray[0] = dr.IsDBNull(6) ? 0 : dr.GetInt32(6);
                                unitsArray[1] = dr.IsDBNull(7) ? 0 : dr.GetInt32(7);
                                unitsArray[2] = dr.IsDBNull(8) ? 0 : dr.GetInt32(8);

                                int total = 0;
                                total = unitsArray[0] + unitsArray[1] + unitsArray[2];
                                row["Total"] = total;

                                int[] combinedUnits = new int[3];
                                combinedUnits = CombineUnits(ratioArray, unitsArray);

                                row["FacUnits"] = combinedUnits[0]; //Facility
                                row["ComUnits"] = combinedUnits[1]; //Community
                                row["OtherUnits"] = combinedUnits[2]; //Other

                                double pct = 0;
                                pct = (total == 0) ? 0 : combinedUnits[1] / (double)total;
                                row["ComPct"] = $"{pct:P0}";

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

        public int[] CombineUnits(string[] ratio, int[] units)
        {
            int[] combinedUnits = new int[3] { 0, 0, 0 };
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    if (ratio[i].Contains("F"))
                    {
                        combinedUnits[0] += units[i]; //Facility
                    }
                    else if (ratio[i].Contains("c"))
                    {
                        combinedUnits[1] += units[i]; //Community
                    }
                    else
                    {
                        combinedUnits[2] += units[i]; //Other
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return combinedUnits;
        }
    }
}


