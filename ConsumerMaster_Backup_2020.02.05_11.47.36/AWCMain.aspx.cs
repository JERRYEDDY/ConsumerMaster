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

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            string outFilename = "AWC40HoursReport.csv";
            try
            {
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    string inFilename = RadAsyncUpload1.UploadedFiles[0].FileName;
                    AWC40HoursReportExcelFile payrollReport = new AWC40HoursReportExcelFile();
                    Workbook workbook = payrollReport.CreateWorkbook(RadAsyncUpload1.UploadedFiles[0].InputStream, inFilename);
                    Utility utility = new Utility();
                    utility.DownloadCSVFile(workbook, outFilename);
                }
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
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    string inFilename = RadAsyncUpload1.UploadedFiles[0].FileName;
                    AWC29HoursReportExcelFile payrollReport = new AWC29HoursReportExcelFile();
                    Workbook workbook = payrollReport.CreateWorkbook(RadAsyncUpload1.UploadedFiles[0].InputStream, inFilename);
                    Utility utility = new Utility();
                    utility.DownloadCSVFile(workbook, filename);
                }
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
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    string inFilename = RadAsyncUpload1.UploadedFiles[0].FileName;
                    AWCOverlapReportExcelFile payrollReport = new AWCOverlapReportExcelFile();
                    Workbook workbook = payrollReport.CreateWorkbook(RadAsyncUpload1.UploadedFiles[0].InputStream, inFilename);
                    Utility utility = new Utility();
                    utility.DownloadCSVFile(workbook, filename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }
        }
    }
}