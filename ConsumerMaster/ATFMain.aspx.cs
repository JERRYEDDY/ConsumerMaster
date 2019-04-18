using System;
using System.Web;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class ATFMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger IndexLogger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            BindToATF_TPDropDownList(ATFConsumerList);
            BindToATF_TPDropDownList(ATFServiceList);
        }

        protected void ATFConsumerExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ATFConsumerExport.xlsx";
            try
            {
                string selectedValue = ATFConsumerList.SelectedValue;
                ConsumerExportExcelFile consumerExport = new ConsumerExportExcelFile();
                Workbook workbook = consumerExport.CreateWorkbook(selectedValue);
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                IndexLogger.Error(ex);
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
                IndexLogger.Error(ex);
            }
        }

        protected void ATFServiceExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ATFServiceExport.xlsx";
            try
            {
                string selectedValue = ATFServiceList.SelectedValue;
                ATFServiceExportExcelFile serviceExport = new ATFServiceExportExcelFile();
                Workbook workbook = serviceExport.CreateWorkbook(selectedValue);
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                IndexLogger.Error(ex);
            }
        }

        private void BindToATF_TPDropDownList(RadDropDownList dropdownlist)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id AS trading_partner_id, name FROM TradingPartners WHERE id IN (1, 2, 3, 4)", con))
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
            catch (Exception ex)
            {
                IndexLogger.Error(ex);
            }
        }
    }
}