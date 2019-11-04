using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class NSStaffImportMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        const int MaxTotalBytes = 1048576; // 1 MB
        long totalBytes;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void NSStaffImportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"NSStaffImportExport.xlsx";
            try
            {

                int fileCount = RadAsyncUpload1.UploadedFiles.Count;

                foreach(UploadedFile file in RadAsyncUpload1.UploadedFiles)
                {
                    //byte[] bytes = new byte[file.ContentLength];
                    //file.InputStream.Read(bytes, 0, bytes.Length);

                    NSStaffImportExcelFile conversionExport = new NSStaffImportExcelFile();
                    Workbook workbook = conversionExport.CreateWorkbook(file);
                    Utility utility = new Utility();
                    utility.DownloadExcelFile(workbook, filename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void RadAsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            //BtnSubmit.Visible = false;
            //RefreshButton.Visible = true;
            //RadAsyncUpload1.Visible = false;

            var liItem = new HtmlGenericControl("li");
            liItem.InnerText = e.File.FileName;


            if (totalBytes < MaxTotalBytes)
            {
                // Total bytes limit has not been reached, accept the file
                e.IsValid = true;
                totalBytes += e.File.ContentLength;
            }
            else
            {
                // Limit reached, discard the file
                e.IsValid = false;
            }

            //if (e.IsValid)
            //{

            //    ValidFiles.Visible = true;
            //    ValidFilesList.Controls.AddAt(0, liItem);

            //}
            //else
            //{

            //    InvalidFiles.Visible = true;
            //    InValidFilesList.Controls.AddAt(0, liItem);
            //}
        }
    }
}