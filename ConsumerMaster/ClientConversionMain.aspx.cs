﻿using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace ConsumerMaster
{
    public partial class ClientConversionMain : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ClientAllDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ClientConversionExport.xlsx";
            try
            {
                ClientConversionExcelFile conversionExport = new ClientConversionExcelFile();
                Workbook workbook = conversionExport.CreateAllWorkbook();
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