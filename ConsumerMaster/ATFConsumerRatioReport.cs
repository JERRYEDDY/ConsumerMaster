using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Font;
using iText.Layout.Element;
using iText.Kernel.Geom;
using iText.Layout.Properties;

namespace ConsumerMaster
{
    public class ATFConsumerRatioReport
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void TableToPdf(DataTable dTable, string destinationPath)
        {
            var writer = new PdfWriter(destinationPath);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4.Rotate());
            document.SetMargins(20, 20, 20, 20);
            var font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);
            var bold = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            var table = new Table(new float[] { 4, 1, 3, 3, 3, 3, 3, 3 });
            table.SetWidth(UnitValue.CreatePercentValue(100));

            foreach (DataColumn column in dTable.Columns)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(column.ColumnName).SetFont(font)));
            }

            foreach (DataRow dr in dTable.Rows)
            {
                table.AddCell(new Cell().Add(new Paragraph(dr["FullName"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Ratio1"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Ratio2"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Units1"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Units2"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Total"].ToString()).SetFont(font)));

                table.AddCell(new Cell().Add(new Paragraph(dr["Pct1"].ToString()).SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph(dr["Pct2"].ToString()).SetFont(font)));
            }

            document.Add(table);
            document.Close();
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

            string outFileName = @"C:\Billing Software\ATF\ATFConsumerRatio.pdf";
            return consumersTable;
        }
    }
}