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
            string outFilename = $"AWC40HoursReport.txt";
            try
            {
                if (RadAsyncUpload1.UploadedFiles.Count == 1)
                {
                    Utility utility = new Utility();
                    AWC40HoursReportFile payrollReport = new AWC40HoursReportFile();

                    UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports

                    MemoryStream output = payrollReport.CreateDocument(uploadedFile);

                    //string fileDate = uploadedFile.FileName.Between("_", ".");

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
                    UploadedFile clientAuthorizationListFile = RadAsyncClientAuthorizationList.UploadedFiles[0]; //Client Authorization List
                    UploadedFile clientStaffListFile = RadAsyncClientStaffList.UploadedFiles[0]; //Client Staff List

                    Utility utility = new Utility();
                    AWCClientDataIntegrityReportFile report = new AWCClientDataIntegrityReportFile();
                    Workbook workbook = report.CreateWorkbook(clientRosterFile, clientAuthorizationListFile, clientStaffListFile);
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

        protected void RadButton6_Click(object sender, EventArgs e)
        {
            RadButton7.Enabled = false;
        }

        //protected void RVButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        UploadedFile uploadedFile = RadAsyncUpload1.UploadedFiles[0]; //Payroll Reports

        //        Utility util = new Utility();
        //        Stream input = uploadedFile.InputStream;
        //        DataTable dTable = util.GetTimeAndDistanceDataTable(input);

        //        var query = from row in dTable.AsEnumerable()
        //                    group row by new
        //                    {
        //                        StaffID = row.Field<string>("Staff ID"),
        //                        StaffName = row.Field<string>("Staff Name")
        //                    }
        //        into TD
        //                    where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 40.00
        //                    orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
        //                    select new
        //                    {
        //                        ID = TD.Key.StaffID,
        //                        Name = TD.Key.StaffName,
        //                        Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
        //                    };

        //        DataTable rptDataTable = new DataTable();
        //        rptDataTable.Columns.Add("ID", typeof(string));
        //        rptDataTable.Columns.Add("Name", typeof(string));
        //        rptDataTable.Columns.Add("Hours", typeof(double));

        //        foreach (var element in query)
        //        {
        //            rptDataTable.Rows.Add(element.ID,element.Name, element.Hours);
        //        }

        //        string selectQuery =
        //        $@"
        //            SELECT rpt.ID, rpt.Name, rpt.Hours
        //            FROM 40HoursReport rpt
        //         ";

        //        DataTable sqlDataTable = util.GetDataTable3(selectQuery);


        //        //this.ReportViewer1.Reset();
        //        //this.ReportViewer1.ProcessingMode = ProcessingMode.Remote;
        //        this.ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://itlt21t:80/ReportServer");
        //        this.ReportViewer1.ServerReport.ReportPath = "/AWC/40 Hours Report";
        //        ReportDataSource rds = new ReportDataSource("dsNewDataSet_Table", rptDataTable);
        //        this.ReportViewer1.LocalReport.DataSources.Clear();
        //        this.ReportViewer1.LocalReport.DataSources.Add(rds);
        //        //this.ReportViewer1.DataBind();
        //        this.ReportViewer1.ServerReport.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}
    }
}