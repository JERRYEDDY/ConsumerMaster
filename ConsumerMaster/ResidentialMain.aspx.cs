using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace ConsumerMaster
{
    public partial class ResidentialMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ResidentialConsumerExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ResidentialConsumerExport.csv";
            try
            {
                string tradingPartnerId = "5"; //Agency With Choice;In Home
                ConsumerExportExcelFile consumerExport = new ConsumerExportExcelFile();
                Workbook workbook = consumerExport.CreateWorkbook(tradingPartnerId);
                Utility utility = new Utility();
                utility.DownloadCSVFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void ResidentialServiceExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ResidentialServiceExport.xlsx";
            try
            {
                ResidentialServiceExportExcelFile serviceExport = new ResidentialServiceExportExcelFile();
                Workbook workbook = serviceExport.CreateWorkbook();
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