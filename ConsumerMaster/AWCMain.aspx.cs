using System;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using Telerik.Windows.Documents.Fixed.Model;

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
            string outFilename = "AWC40HoursReport.txt";
            try
            {
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    string inFilename = RadAsyncUpload1.UploadedFiles[0].FileName;
                    AWC40HoursReportFile payrollReport = new AWC40HoursReportFile();
                    Utility utility = new Utility();
                    
                    MemoryStream output = payrollReport.CreateDocument(RadAsyncUpload1.UploadedFiles[0].InputStream, inFilename);
                    utility.DownloadTXTFile(output, outFilename);

                    //Workbook workbook = payrollReport.CreateWorkbook(RadAsyncUpload1.UploadedFiles[0].InputStream, inFilename);
                    //utility.DownloadCSVFile(workbook, outFilename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }
        }

        protected void RadButton2_Click(object sender, EventArgs e)
        {
            string outFilename = "AWC29HoursReport.txt";
            try
            {
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    string inFilename = RadAsyncUpload1.UploadedFiles[0].FileName;
                    AWC29HoursReportFile payrollReport = new AWC29HoursReportFile();

                    Utility utility = new Utility();
                    MemoryStream output = payrollReport.CreateDocument(RadAsyncUpload1.UploadedFiles[0].InputStream, inFilename);
                    utility.DownloadTXTFile(output, outFilename);

                    //utility.DownloadCSVFile(workbook, filename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }
        }

        protected void RadButton3_Click(object sender, EventArgs e)
        {
            string outFilename = "AWCOverlapShiftsReport.txt";
            try
            {
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    string inFilename = RadAsyncUpload1.UploadedFiles[0].FileName;
                    AWCOverlapReportFile payrollReport = new AWCOverlapReportFile();
                    //Workbook workbook = payrollReport.CreateWorkbook(RadAsyncUpload1.UploadedFiles[0].InputStream, inFilename);
                    Utility utility = new Utility();

                    MemoryStream output = payrollReport.CreateDocument(RadAsyncUpload1.UploadedFiles[0].InputStream, inFilename);
                    utility.DownloadTXTFile(output, outFilename);
                    //utility.DownloadCSVFile(workbook, filename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }
        }

        protected void RadButton4_Click(object sender, EventArgs e)
        {
            string outFilename = "AWCClientStaffAuthorizationReport.txt";
            try
            {
                if (RadAsyncUpload2.UploadedFiles.Count == 1)
                {
                    string inFilename = RadAsyncUpload2.UploadedFiles[0].FileName;
                    AWCClientStaffAuthorizationReportFile payrollReport = new AWCClientStaffAuthorizationReportFile();
                    Utility utility = new Utility();

                    MemoryStream output = payrollReport.CreateDocument(RadAsyncUpload2.UploadedFiles[0].InputStream, inFilename);
                    utility.DownloadTXTFile(output, outFilename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }
        }

    }
}