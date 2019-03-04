using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using Telerik.Web.Spreadsheet;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;

namespace ConsumerMaster
{
    public partial class Reports : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            //BindToATFTPDropDownList2(ATFRatioList);
        }

        private void BindToATFTPDropDownList2(RadDropDownList dropdownlist)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                con.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id AS trading_partner_id, name FROM TradingPartners WHERE id IN (3, 4)", con))
                {
                    DataTable tradingPartners = new DataTable();
                    adapter.Fill(tradingPartners);

                    dropdownlist.DataTextField = "name";
                    dropdownlist.DataValueField = "trading_partner_id";
                    dropdownlist.DataSource = tradingPartners;
                    dropdownlist.DataBind();
                }
            }
        }

        protected void ATFRatioReportDownload_Click(object sender, EventArgs e)
        {
            //const string filename = @"ATFServiceExport.xlsx";
            try
            {
                //ATFRatioReport ratioReport = new ATFRatioReport();
                //ratioReport.CreateReport();
                DownloadPDFFile();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void DownloadPDFFile()
        {
            PdfFormatProvider formatProvider = new PdfFormatProvider();
            formatProvider.ExportSettings.ImageQuality = ImageQuality.High;

            byte[] renderedBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                RadFixedDocument document = this.CreateDocument();
                formatProvider.Export(document, ms);
                renderedBytes = ms.ToArray();
            }

            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=PdfDocument.pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(renderedBytes);
            Response.End();
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

        public RadFixedDocument CreateDocument()
        {
            DateTime startDate = new DateTime(2019, 2, 18, 0, 0, 0);
            DateTime endDate = new DateTime(2019, 2, 22, 23, 59, 59);

            DataTable ratioDataTable = new DataTable();
            ratioDataTable = GetAttendanceData(startDate, endDate);

            RadFixedDocument document = new RadFixedDocument();
            RadFixedDocumentEditor editor = new RadFixedDocumentEditor(document);

            Table table = new Table();
            Border border = new Border();
            table.Borders = new TableBorders(border);
            table.DefaultCellProperties.Borders = new TableCellBorders(border, border, border, border);

            //table.BorderSpacing = 5;
            //table.BorderCollapse = BorderCollapse.Separate;

           var vheaderRow = new TableRow();

                table.Rows.AddTableRow();
            headerRow.Rea
            headerRow.Cells.AddTableCell().Blocks.AddBlock().InsertText("FullName");
            headerRow.Cells.AddTableCell().Blocks.AddBlock().InsertText("Ratio1");
            headerRow.Cells.AddTableCell().Blocks.AddBlock().InsertText("Ratio2");
            headerRow.Cells.AddTableCell().Blocks.AddBlock().InsertText("Units1");
            headerRow.Cells.AddTableCell().Blocks.AddBlock().InsertText("Units2");
            headerRow.Cells.AddTableCell().Blocks.AddBlock().InsertText("Total");
            headerRow.Cells.AddTableCell().Blocks.AddBlock().InsertText("Pct1");
            headerRow.Cells.AddTableCell().Blocks.AddBlock().InsertText("Pct2");
            

            foreach (DataRow dr in ratioDataTable.Rows)
            {
                TableRow row = table.Rows.AddTableRow();
                row.Cells.AddTableCell().Blocks.AddBlock().InsertText(dr["FullName"].ToString());
                row.Cells.AddTableCell().Blocks.AddBlock().InsertText(dr["Ratio1"].ToString());
                row.Cells.AddTableCell().Blocks.AddBlock().InsertText(dr["Ratio2"].ToString());
                row.Cells.AddTableCell().Blocks.AddBlock().InsertText(dr["Units1"].ToString());
                row.Cells.AddTableCell().Blocks.AddBlock().InsertText(dr["Units2"].ToString());
                row.Cells.AddTableCell().Blocks.AddBlock().InsertText(dr["Total"].ToString());
                row.Cells.AddTableCell().Blocks.AddBlock().InsertText(dr["Pct1"].ToString());
                row.Cells.AddTableCell().Blocks.AddBlock().InsertText(dr["Pct2"].ToString());
            }

            table.LayoutType = TableLayoutType.FixedWidth;
            editor.InsertTable(table);

            return document;
        }
    }
}