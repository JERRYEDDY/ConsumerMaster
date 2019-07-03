using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace ConsumerMaster
{
    public partial class VendorMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void VendorConsumerExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"VendorConsumerExport.csv";
            try
            {
                string tradingPartnerId = "19"; //Vendor; In Home
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

        protected void VendorServiceExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"VendorServiceExport.xlsx";
            try
            {
                VendorServiceExportExcelFile serviceExport = new VendorServiceExportExcelFile();
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