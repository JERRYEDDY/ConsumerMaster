using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class EIMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            BindToEI_TPDropDownList(EIConsumerList);
            BindToEI_TPDropDownList(EIServiceList);
        }

        protected void EIConsumerExportDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedValue = EIConsumerList.SelectedValue;

                }

                ConsumerExportExcelFile consumerExport = new ConsumerExportExcelFile();
                Workbook workbook = consumerExport.CreateWorkbook(selectedValue);
                Utility utility = new Utility();
                utility.DownloadCSVFile(workbook, filename);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void EIServiceExportDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedValue = EIServiceList.SelectedValue;
                string filename = "";

                if (selectedValue.Equals("7")) // Direct Therapy
                {
                    filename = @"EIDTServiceExport.xlsx";
                    EIDTServiceExportExcelFile serviceExport = new EIDTServiceExportExcelFile();
                    Workbook workbook = serviceExport.CreateWorkbook(selectedValue);
                    Utility utility = new Utility();
                    utility.DownloadExcelFile(workbook, filename);
                }
                else if (selectedValue.Equals("8")) //Special Instruction
                {
                    filename = @"EISIServiceExport.xlsx";
                    EISIServiceExportExcelFile serviceExport = new EISIServiceExportExcelFile();
                    Workbook workbook = serviceExport.CreateWorkbook(selectedValue);
                    Utility utility = new Utility();
                    utility.DownloadExcelFile(workbook, filename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void BindToEI_TPDropDownList(RadDropDownList dropdownlist)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id AS trading_partner_id, name FROM TradingPartners WHERE id IN (7, 8)", con))
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
    }
}