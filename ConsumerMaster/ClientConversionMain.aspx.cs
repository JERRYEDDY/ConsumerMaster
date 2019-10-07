using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace ConsumerMaster
{
    public partial class ClientConversionMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ClientConversionDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ClientConversionExport.csv";
            try
            {
                ClientConversionExcelFile conversionExport = new ClientConversionExcelFile();
                Workbook workbook = conversionExport.CreateWorkbook();
                Utility utility = new Utility();
                utility.DownloadCSVFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}