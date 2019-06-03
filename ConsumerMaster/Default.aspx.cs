using System;
using System.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class _Default : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Logger.Info("ConsumerMaster started");
                //BindToTPDropDownList(TPRadDropDownList);
            }
        }

        protected void ConsumerExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ConsumerExport.xlsx";
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void EIServiceExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"EIServiceExport.xlsx";
            try
            {
                EIServiceExportExcelFileReadWrite serviceExport = new EIServiceExportExcelFileReadWrite();
                Workbook workbook = serviceExport.CreateWorkbook();
                Utility utility = new Utility();
                utility.DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void BindToTPDropDownList(RadDropDownList dropdownlist)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id AS trading_partner_id, name FROM TradingPartners", con))
                    {
                        DataTable tradingPartners = new DataTable();
                        adapter.Fill(tradingPartners);

                        DataRow dr = tradingPartners.NewRow();
                        dr["trading_partner_id"] = "0";
                        dr["name"] = "ALL TRADING PARTNERS";
                        tradingPartners.Rows.InsertAt(dr, 0);

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
    }
}