using System;
using System.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.Model;
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
            BindToATF_TPDropDownList2(ATFPartnerList);
        }

        private void BindToATF_TPDropDownList2(RadDropDownList dropdownlist)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id AS trading_partner_id, name FROM TradingPartners WHERE id IN (1, 2)", con))
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
                Logger.Error(ex);
            }
        }

        protected void ATFConsumerRatioReportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ATFConsumerRatioReport.xlsx";
            try
            {
                DateTime startDate = new DateTime(StartDatePicker.SelectedDate.Value.Year,
                    StartDatePicker.SelectedDate.Value.Month, StartDatePicker.SelectedDate.Value.Day, 0, 0, 0);
                DateTime endDate = new DateTime(EndDatePicker.SelectedDate.Value.Year,
                    EndDatePicker.SelectedDate.Value.Month, EndDatePicker.SelectedDate.Value.Day, 23, 59, 59);

                int siteId = Convert.ToInt32(ATFPartnerList.SelectedValue);
                string siteName = ATFPartnerList.SelectedText;

                ATFConsumerRatioReport ratioReport = new ATFConsumerRatioReport();
                Workbook workbook = ratioReport.CreateWorkbook(startDate, endDate, siteId, siteName);
                Utility utility = new Utility();
                utility.DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}