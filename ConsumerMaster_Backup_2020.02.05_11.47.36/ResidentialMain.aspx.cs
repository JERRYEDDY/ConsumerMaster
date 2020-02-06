using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace ConsumerMaster
{
    public partial class ResidentialMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        enum Residential
        { 
            Beverly = 9,
            Ewing = 10, 
            Gabby = 11,
            Hewitt = 12,
            Lantz = 13, 
            Linn = 14, 
            Locust = 15,    
            Oakview = 16,   
            Poplar = 17,   
            Sycamore = 18,  
        }
        const string residentialList = "('9', '10', '11', '12', '13', '14', '15', '16', '17', '18')";

        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (string location in Enum.GetNames(typeof(Residential)))
                Console.WriteLine(location);
        }

        protected void ResidentialConsumerExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ResidentialConsumerExport.csv";
            try
            {
                ResidentialConsumerExportExcelFile consumerExport = new ResidentialConsumerExportExcelFile();
                Workbook workbook = consumerExport.CreateWorkbook(residentialList);
                Utility utility = new Utility();
                utility.DownloadCSVFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void ResidentialServiceExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ResidentialServiceExport.xlsx";
            try
            {
                ResidentialServiceExportExcelFile serviceExport = new ResidentialServiceExportExcelFile();
                Workbook workbook = serviceExport.CreateWorkbook(residentialList);
                Utility utility = new Utility();
                utility.DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}