using System;
using Telerik.Web.UI;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.Model;

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
                    Utility utility = new Utility();
                    AWC40HoursReportFile payrollReport = new AWC40HoursReportFile();

                    UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports
                    MemoryStream output = payrollReport.CreateDocument(uploadedFile);
                    utility.DownloadTXTFile(output, outFilename);
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
                    Utility utility = new Utility();
                    AWC29HoursReportFile payrollReport = new AWC29HoursReportFile();

                    UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports
                    MemoryStream output = payrollReport.CreateDocument(uploadedFile);
                    utility.DownloadTXTFile(output, outFilename);
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
                    Utility utility = new Utility();
                    AWCOverlapReportFile payrollReport = new AWCOverlapReportFile();

                    UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports
                    MemoryStream output = payrollReport.CreateDocument(uploadedFile);
                    utility.DownloadTXTFile(output, outFilename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void RadButton4_Click(object sender, EventArgs e)
        {
            string outFilename = "ClientMemberAuthMailMerge.docx";
            try
            {
                if (RadAsyncUploadStaff.UploadedFiles.Count == 1)
                {
                    Utility utility = new Utility();
                    AWCClientMemberAuthorizationReportFile otherReport = new AWCClientMemberAuthorizationReportFile();

                    UploadedFile clientFile = RadAsyncClient.UploadedFiles[0]; //Other Reports
                    UploadedFile memberFile = RadAsyncUploadStaff.UploadedFiles[0]; //Other Reports
                    UploadedFile authorizationFile = RadAsyncUploadAuthorization.UploadedFiles[0]; //Other Reports
                    MemoryStream output = otherReport.CreateClientMemberAuthorizationDocument(clientFile, memberFile, authorizationFile);
                    utility.DownloadDocxFile(output, outFilename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void RadButton5_Click(object sender, EventArgs e)
        {
            string outFilename = "AWCClientDataIntegrityReport.xlsx";
            try
            {
                if (RadAsyncUploadClientRoster.UploadedFiles.Count == 1)
                {
                    UploadedFile clientRosterFile = RadAsyncUploadClientRoster.UploadedFiles[0]; //Client Roster Report

                    Utility utility = new Utility();
                    AWCClientDataIntegrityReportFile report = new AWCClientDataIntegrityReportFile();
                    Workbook workbook = report.CreateWorkbook(clientRosterFile);
                    utility.DownloadExcelFile(workbook, outFilename);


                    //UploadedFile memberFile = RadAsyncUploadStaff.UploadedFiles[0]; //Other Reports
                    //UploadedFile authorizationFile = RadAsyncUploadAuthorization.UploadedFiles[0]; //Other Reports


                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}