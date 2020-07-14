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

        protected void RadButtonMismatchServices_Click(object sender, EventArgs e)
        {
            string outFilename = "AWCMismatchServicesReport.txt";
            try
            {
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    Utility utility = new Utility();
                    AWCMismatchedServicesReportFile MismatchedServicesReport = new AWCMismatchedServicesReportFile();

                    UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports
                    MemoryStream output = MismatchedServicesReport.CreateDocument(uploadedFile);
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
            string outFilename = "AWCServiceNoteAuditReport.xlsx";
            try
            {
                if (RadAsyncUploadClosedActivities.UploadedFiles.Count == 1)
                {
                    UploadedFile closedActivitiesFile = RadAsyncUploadClosedActivities.UploadedFiles[0]; //Closed Activities Report
                    UploadedFile auditLogFile = RadAsyncUploadAuditLog.UploadedFiles[0]; //Client Authorization List
                    //UploadedFile clientStaffListFile = RadAsyncUploadClosedActivities.UploadedFiles[0]; //Client Staff List

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
            string outFilename = "AWCTravelTimeReport.txt";
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

            string outFilename = "AWCTravelTimeReport.xlsx";
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

//protected void RVButton_Click(object sender, EventArgs e)
//{
//    try
//    {
//        //UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports

//        Utility util = new Utility();
//        //Stream input = uploadedFile.InputStream;
//        //DataTable dTable = util.GetTimeAndDistanceDataTable(input);

//        //var query = from row in dTable.AsEnumerable()
//        //            group row by new
//        //            {
//        //                StaffID = row.Field<string>("Staff ID"),
//        //                StaffName = row.Field<string>("Staff Name")
//        //            }
//        //into TD
//        //            where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 40.00
//        //            orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
//        //            select new
//        //            {
//        //                ID = TD.Key.StaffID,
//        //                Name = TD.Key.StaffName,
//        //                Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
//        //            };

//        //DataTable rptDataTable = new DataTable();
//        //rptDataTable.Columns.Add("ID", typeof(string));
//        //rptDataTable.Columns.Add("Name", typeof(string));
//        //rptDataTable.Columns.Add("Hours", typeof(double));

//        //foreach (var element in query)
//        //{
//        //    rptDataTable.Rows.Add(element.ID, element.Name, element.Hours);
//        //}

//        string selectQuery =
//        $@"
//            SELECT cr.id_no,cr.name,cr.gender,cr.dob,cr.current_location,cr.current_phone_day,
//                cr.intake_date,cr.managing_office,cr.program_name,cr.unit,cr.program_modifier,cr.worker_name,
//                cr.worker_role,cr.is_primary_worker,cr.medicaid_number,cr.medicaid_payer,cr.medicaid_plan_name,
//                ca.ba_count,cs.me_count,cs.ssp_count
//            FROM ClientRoster AS cr
//            LEFT JOIN ClientAuthorizations AS ca ON cr.id_no = ca.AClientID
//            LEFT JOIN ClientStaff AS cs ON cr.id_no = cs.SClientID
//            ORDER BY cr.gender
//         ";

//        DataTable sqlDataTable = util.GetDataTable3(selectQuery);


//        //this.ReportViewer1.Reset();
//        //this.ReportViewer1.ProcessingMode = ProcessingMode.Remote;
//        this.ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://itlt21t:80/ReportServer");
//        this.ReportViewer1.ServerReport.ReportPath = "/AWC/ClientDataIntegrity";
//        ReportDataSource rds = new ReportDataSource("dsNewDataSet_Table", sqlDataTable);
//        this.ReportViewer1.LocalReport.DataSources.Clear();
//        this.ReportViewer1.LocalReport.DataSources.Add(rds);
//        this.ReportViewer1.DataBind();

//        this.ReportViewer1.ServerReport.Refresh();

//    }
//    catch (Exception ex)
//    {
//        Logger.Error(ex);
//    }
//}

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