using System;
using Telerik.Web.UI;
using System.IO;
using System.Collections.Generic;
using System.Data;
using GemBox.Document;

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

        protected void Download_Click(object sender, EventArgs e)
        {
            List<FollowUpData> mailMergeDataSource = new List<FollowUpData>()
            {
                new FollowUpData()
                {
                    FirstName = "Andrew",
                    LastName = "Fuller",
                    CompanyName = "Your AWC",
                    PurchasedItemsCount = 4,
                    ProductName = "Donuts",
                    ProductSupportPhone = "555-1212",
                    ProductSupportPhoneAvailability = "from 8am to 4pm",
                    ProductSupportEmail = "afuller@gmail.com",
                    SalesRepFirstName = "John",
                    SalesRepLastName = "Smith",
                    SalesRepTitle = "Dentist"
                },
                new FollowUpData()
                {
                    FirstName = "Nancy",
                    LastName = "Davolio",
                    CompanyName = "Your AWC",
                    PurchasedItemsCount = 8,
                    ProductName = "Donut Holes",
                    ProductSupportPhone = "555-1212",
                    ProductSupportPhoneAvailability = "from 8am to 4pm",
                    ProductSupportEmail = "ndavolio@gmail.com",
                    SalesRepFirstName = "John",
                    SalesRepLastName = "Smith",
                    SalesRepTitle = "Dentist"
                },
            };


            GenerateDocument genDoc = new GenerateDocument();

            //RadFlowDocument document = genDoc.CreateDocument();

            //RadFlowDocument mailMergedDocument = document.MailMerge(mailMergeDataSource);

            //genDoc.GemBoxNestMailMerge();



            //byte[] renderedBytes = null;
            //IFormatProvider<RadFlowDocument> formatProvider = new DocxFormatProvider();
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    formatProvider.Export(mailMergedDocument, ms);
            //    renderedBytes = ms.ToArray();
            //}

            //Response.ClearHeaders();
            //Response.ClearContent();
            //Response.AppendHeader("content-disposition", "attachment; filename=ExportedFile" + ".docx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            //Response.BinaryWrite(renderedBytes);
            //Response.End();

        }

        public class FollowUpData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CompanyName { get; set; }
            public int PurchasedItemsCount { get; set; }
            public string ProductName { get; set; }
            public string ProductSupportPhone { get; set; }
            public string ProductSupportPhoneAvailability { get; set; }
            public string ProductSupportEmail { get; set; }
            public string SalesRepFirstName { get; set; }
            public string SalesRepLastName { get; set; }
            public string SalesRepTitle { get; set; }
        }
    }
}