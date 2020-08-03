using System;
using Telerik.Web.UI;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.Data.SqlClient;

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
            string outFilename = $"AWC40HoursReport.txt";
            try
            {
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    Utility utility = new Utility();
                    AWC40HoursReportFile payrollReport = new AWC40HoursReportFile();

                    UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports
                    AWC40HoursReportFile report = new AWC40HoursReportFile();
                    Workbook workbook = report.CreateWorkbook(uploadedFile);
                    utility.DownloadExcelFile(workbook, outFilename);

                    //MemoryStream output = payrollReport.CreateDocument(uploadedFile);

                    //string fileDate = uploadedFile.FileName.Between("_", ".");

                    //utility.DownloadTXTFile(output, outFilename);
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

        protected void RadButtonServicesException_Click(object sender, EventArgs e)
        {
            string outFilename = "AWCServicesExceptionReport.txt";
            try
            {
                if (RadAsyncUpload1.UploadedFiles.Count == 1 && RadAsyncUpload2.UploadedFiles.Count == 1)
                {
                    Utility utility = new Utility();
                    AWCServicesExceptionReportFile ServiceExceptionReport = new AWCServicesExceptionReportFile();

                    UploadedFile uploadedTDFile = RadAsyncUpload1.UploadedFiles[0]; //Time & Distance File
                    UploadedFile uploadedBAFile = RadAsyncUpload2.UploadedFiles[0]; //Billing Authorization File

                    Workbook workbook = ServiceExceptionReport.CreateWorkbook(uploadedTDFile, uploadedBAFile);
                    utility.DownloadExcelFile(workbook, outFilename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void RadButton5_Click(object sender, EventArgs e)
        {
            string outFilename = "AWCServiceNoteAuditReport.xlsx";
            try
            {
                if (RadAsyncUploadClosedActivities.UploadedFiles.Count == 1)
                {
                    UploadedFile closedActivitiesFile = RadAsyncUploadClosedActivities.UploadedFiles[0]; //Closed Activities Report
                    UploadedFile auditLogFile = RadAsyncUploadAuditLog.UploadedFiles[0]; //Client Authorization List
 
                    Utility utility = new Utility();
                    AWCServiceNoteAuditReportFile report = new AWCServiceNoteAuditReportFile();
                    MemoryStream output = report.CreateDocument(closedActivitiesFile, auditLogFile);
                    utility.DownloadTXTFile(output, outFilename);
                 }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void RadButtonTravel_Click(object sender, EventArgs e)
        {
            //string outFilename = "AWCTravelTimeReport.txt";
            //try
            //{
            //    if (RadAsyncUpload1.UploadedFiles.Count == 1)
            //    {

            //        bool shiftFilter = ShiftCheckBox.Checked;

            //        Utility utility = new Utility();
            //        AWCTravelTimeReportFile payrollReport = new AWCTravelTimeReportFile();

            //        UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports
            //        MemoryStream output = payrollReport.CreateDocument(uploadedFile, shiftFilter);
            //        utility.DownloadTXTFile(output, outFilename);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ex);
            //}
        }

        protected void RadButtonTravel_Click2(object sender, EventArgs e)
        {

            //string outFilename = "AWCTravelTimeReport.xlsx";
            //try
            //{
            //    if (RadAsyncUpload1.UploadedFiles.Count == 1)
            //    {
            //        bool shiftFilter = ShiftCheckBox.Checked;

            //        Utility utility = new Utility();
            //        AWCTravelTimeReportExcelFile reportExport = new AWCTravelTimeReportExcelFile();
            //        UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports
            //        Workbook workbook = reportExport.CreateWorkbook(uploadedFile, shiftFilter);

            //        utility.DownloadExcelFile(workbook, outFilename);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ex);
            //}

        }

        public DataTable GetDataTable(string queryString)
        {
            DataTable dataTable = new DataTable();
            string connectionString = "Data Source=ITSQLX1\\ITDBVRTX1;Initial Catalog=NetsmartReports;Integrated Security=true;";

            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                {
                    sqlConnect.Open();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnect))
                    {

                        sqlDataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            }
        }
    }
}