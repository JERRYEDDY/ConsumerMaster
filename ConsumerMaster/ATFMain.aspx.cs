using System;
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
                Utility utility = new Utility();
                utility.DownloadExcelFile(workbook, filename);
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
                Utility utility = new Utility();
                utility.DownloadExcelFile(workbook, filename);
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