using System;
using Telerik.Web.UI;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI.Upload;
using System.Globalization;

namespace ConsumerMaster
{
    public partial class AWCMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{ 
            //    string a = "8/15/2020 11:03 PM";
            //    string b = "8/15/2020 8:17 PM";
            //    string c = "8/07/2020 9:00 AM";

            //    DateTime aDatTime = DateTime.ParseExact(a, "M/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            //    DateTime bDatTime = DateTime.ParseExact(b, "M/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            //    DateTime cDatTime = DateTime.ParseExact(c, "M/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);



            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ex);

            //};

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

        protected void RadButtonMismatchedServices_Click(object sender, EventArgs e)
        {
            string outFilename = "AWCMismatchedServicesReport.xlsx";
            try
            {
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    Utility utility = new Utility();

                    AWCMismatchedServicesReportFile MismatchedServicesReport = new AWCMismatchedServicesReportFile();
                     UploadedFile uploadedTDFile = RadAsyncUpload1.UploadedFiles[0]; //Time & Distance File

                    Workbook workbook = MismatchedServicesReport.CreateWorkbook(uploadedTDFile);
                    utility.DownloadExcelFile(workbook, outFilename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void RadButtonPayrollProcessingReport_Click(object sender, EventArgs e)
        {
            string outFilename = "AWCPayrollProcessing.xlsx";
            try
            {
                if (RadAsyncUpload2.UploadedFiles.Count == 1)
                {
                    Utility utility = new Utility();
                    AWCPayrollProcessingReport payrollProcessingReport = new AWCPayrollProcessingReport();

                    UploadedFile uploadedFile = RadAsyncUpload2.UploadedFiles[0]; //Scheduled Actual File

                    Workbook workbook = payrollProcessingReport.CreateWorkbook(uploadedFile);
                    utility.DownloadExcelFile(workbook, outFilename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void RadButtonServicesExceptionHCSIS_Click(object sender, EventArgs e)
        {
            //string outFilename = "AWCServicesExceptionReport.xlsx";
            try
            {
                //if (RadAsyncUpload1.UploadedFiles.Count == 1 && RadAsyncUpload2.UploadedFiles.Count == 1 && RadAsyncUpload3.UploadedFiles.Count == 1)
                //{
                //    Utility utility = new Utility();
                //    AWCServicesExceptionReportFile ServiceExceptionReport = new AWCServicesExceptionReportFile();

                //    UploadedFile uploadedTDFile = RadAsyncUpload1.UploadedFiles[0]; //Time & Distance File
                //    UploadedFile uploadedBAFile = RadAsyncUpload2.UploadedFiles[0]; //NS Billing Authorization File
                //    UploadedFile uploadedHBAFile = RadAsyncUpload3.UploadedFiles[0]; //HCSIS Billing Authorization File

                //    Workbook workbook = ServiceExceptionReport.CreateWorkbook(uploadedTDFile, uploadedBAFile, uploadedHBAFile);
                //    utility.DownloadExcelFile(workbook, outFilename);
                //}
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void RadButtonEVV_VisitsComparison_Click(object sender, EventArgs e)
        {
            string outFilename = "EVVVisitsComparisonReport.xlsx";
            try
            {
                if (RadAsyncUpload3.UploadedFiles.Count == 1 && RadAsyncUpload4.UploadedFiles.Count == 1)
                {
                    Utility utility = new Utility();
                    AWCEVVVisitsComparisonReportFile reportExport = new AWCEVVVisitsComparisonReportFile(); 
                    UploadedFile uploadedCCAFile = RadAsyncUpload3.UploadedFiles[0]; //CellTrak Closed Activities File
                    UploadedFile uploadedSEVFile = RadAsyncUpload4.UploadedFiles[0]; //Sandata Export Visits File

                    Workbook workbook = reportExport.CreateWorkbook(uploadedCCAFile, uploadedSEVFile);
                    utility.DownloadExcelFile(workbook, outFilename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void RadButtonTravel_Click(object sender, EventArgs e)
        {
            string outFilename = "AWCTravelTimeReport.xlsx";
            try
            {
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    bool shiftFilter = true;

                    Utility utility = new Utility();
                    AWCTravelTimeReportExcelFile reportExport = new AWCTravelTimeReportExcelFile();
                    UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports
                    Workbook workbook = reportExport.CreateWorkbook(uploadedFile, shiftFilter);

                    utility.DownloadExcelFile(workbook, outFilename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void RadAsyncUpload_ValidatingFile(object sender, ValidateFileEventArgs e)
        {
            // check only the zip files  
            if (e.UploadedFile.GetExtension().ToLower() == ".xlsx")
            {
                e.SkipInternalValidation = true;
            }
            else
            {
                e.IsValid = false;
            }
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