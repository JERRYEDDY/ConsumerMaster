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
            const string filename = @"ClientAllExport.xlsx";
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

        //protected void ClientInformationDownload_Click(object sender, EventArgs e)
        //{
        //    const string filename = @"ClientInformationExport.xlsx";
        //    try
        //    {
        //        ClientConversionExcelFile conversionExport = new ClientConversionExcelFile();
        //        Workbook workbook = new Workbook();
        //        conversionExport.CreateInformationWorkbook(workbook);
        //        Utility utility = new Utility();
        //        utility.DownloadExcelFile(workbook, filename);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}

        //protected void ClientDiagnosisDownload_Click(object sender, EventArgs e)
        //{
        //    const string filename = @"ClientDiagnosisExport.xlsx";
        //    try
        //    {
        //        ClientConversionExcelFile conversionExport = new ClientConversionExcelFile();
        //        Workbook workbook = new Workbook();
        //        conversionExport.CreateDiagnosisWorkbook(workbook);
        //        Utility utility = new Utility();
        //        utility.DownloadExcelFile(workbook, filename);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}

        //protected void ClientBenefitsDownload_Click(object sender, EventArgs e)
        //{
        //    const string filename = @"ClientBenefitsExport.xlsx";
        //    try
        //    {
        //        ClientConversionExcelFile conversionExport = new ClientConversionExcelFile();
        //        Workbook workbook = conversionExport.CreateBenefitsWorkbook();
        //        Utility utility = new Utility();
        //        utility.DownloadExcelFile(workbook, filename);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}
    }
}