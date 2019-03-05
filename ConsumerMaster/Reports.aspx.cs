using System;
using System.Web.UI;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Telerik.Web.UI;

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
            const string filename = @"ATFConsumerRatioReport.xlsx";
            try
            {
                DateTime startDate = new DateTime(2019, 2, 18, 0, 0, 0);
                DateTime endDate = new DateTime(2019, 2, 22, 23, 59, 59);

                //string selectedValue = ATFConsumerList.SelectedValue;

                ATFConsumerRatioReport ratioReport = new ATFConsumerRatioReport();
                Workbook workbook = ratioReport.CreateWorkbook(startDate, endDate);
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void DownloadExcelFile(Workbook workbook, string filename)
        {
            try
            {
                IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
                byte[] renderedBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    formatProvider.Export(workbook, ms);
                    renderedBytes = ms.ToArray();
                }

                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.BinaryWrite(renderedBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}