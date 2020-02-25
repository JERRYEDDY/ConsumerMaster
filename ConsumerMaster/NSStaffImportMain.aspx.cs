using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace ConsumerMaster
{
    public partial class NSStaffImportMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        const int MaxTotalBytes = 1048576; // 1 MB
        //long totalBytes;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void NSStaffImportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"NSStaffImportExport.xlsx";
            try
            {
                NSStaffImportExcelFile conversionExport = new NSStaffImportExcelFile();
                Workbook workbook = conversionExport.CreateWorkbook();
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