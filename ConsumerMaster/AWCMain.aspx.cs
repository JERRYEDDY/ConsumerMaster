using System;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;

namespace ConsumerMaster
{
    public partial class AWCMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        const int MaxTotalBytes = 1048576; // 1 MB
        long totalBytes;

        protected void Page_Load(object sender, EventArgs e)
        {
            RadAsyncUpload1.FileUploaded += new Telerik.Web.UI.FileUploadedEventHandler(RadAsyncUpload1_FileUploaded);
            RadButton1.Click += new EventHandler(RadButton1_Click);

            //if (!IsPostBack)
            //{
            //    RadButton1.Enabled = false;
            //}
            //else
            //{
            //    RadButton1.Enabled = true;
            //}
        }

        public void RadAsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {

            byte[] buffer = new byte[e.File.ContentLength];
            using (Stream str = e.File.InputStream)
            {
                str.Read(buffer, 0, buffer.Length);
 
                // more code
            }
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            string filename = "AWC40HoursReport.csv";
            try
            {
                AWC40HoursReportExportExcelFile payrollExport = new AWC40HoursReportExportExcelFile();
                Workbook workbook = payrollExport.CreateWorkbook();
                Utility utility = new Utility();
                utility.DownloadCSVFile(workbook, filename);

                int l = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }
        }
        protected void RadButton2_Click(object sender, EventArgs e)
        {
            string filename = "AWC29HoursReport.csv";
            try
            {
                AWC40HoursReportExportExcelFile payrollExport = new AWC40HoursReportExportExcelFile();
                Workbook workbook = payrollExport.CreateWorkbook();
                Utility utility = new Utility();
                utility.DownloadCSVFile(workbook, filename);

                int l = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }
        }
        protected void RadButton3_Click(object sender, EventArgs e)
        {
            string filename = "AWCOverlapShiftsReport.csv";
            try
            {
                AWC40HoursReportExportExcelFile payrollExport = new AWC40HoursReportExportExcelFile();
                Workbook workbook = payrollExport.CreateWorkbook();
                Utility utility = new Utility();
                utility.DownloadCSVFile(workbook, filename);

                int l = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }
        }
    }
}