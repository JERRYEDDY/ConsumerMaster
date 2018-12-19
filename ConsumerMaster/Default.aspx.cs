using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Text.RegularExpressions;

namespace ConsumerMaster
{
    public partial class _Default : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private const string ConsumersTable = "Consumers";
        private const string TradingPartnersTable = "TradingPartners";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //this.BindGrid();

            }
        }

        public bool IsZipCode(string zipCode)
        {
            string pattern = @"^\d{5}(?:[-\s]\d{4})?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(zipCode);
        }

        protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
        {
            string item = getItemName(e.Item.OwnerTableView.Name);
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(item + " cannot be inserted. Reason: " + e.Exception.Message);
                Logger.Info(item + " cannot be inserted. Reason: " + e.Exception.Message);
                Logger.Error(e);
            }
            else
            {
                DisplayMessage(item + " inserted");
                Logger.Info(item + " inserted");
            }
        }

        protected void RadGrid1_ItemUpdated(object source, GridUpdatedEventArgs e)
        {
            string item = getItemName(e.Item.OwnerTableView.Name);
            string field = getFieldName(e.Item.OwnerTableView.Name);
            if (e.Exception != null)
            {
                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
                DisplayMessage(item + " " + e.Item[field].Text + " cannot be updated. Reason: " + e.Exception.Message);
                Logger.Info(item + " " + e.Item[field].Text + " cannot be updated. Reason: " + e.Exception.Message);
                Logger.Error(e);
            }
            else
            {
                DisplayMessage(item + " " + e.Item[field].Text + " updated");
                Logger.Info(item + " " + e.Item[field].Text + " updated");
            }
        }

        protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
        {
            string item = getItemName(e.Item.OwnerTableView.Name);
            string field = getFieldName(e.Item.OwnerTableView.Name);
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(item + " " + e.Item[field].Text + " cannot be deleted. Reason: " + e.Exception.Message);
                Logger.Info(item + " " + e.Item[field].Text + " cannot be deleted. Reason: " + e.Exception.Message);
                Logger.Error(e);
            }
            else
            {
                DisplayMessage(item + " " + e.Item[field].Text + " deleted");
                Logger.Info(item + " " + e.Item[field].Text + " deleted");
            }
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case ConsumersTable:
                    {
                        //GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                        //SqlDataSource1.InsertParameters["consumer_internal_number"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["consumer_internal_number"].ToString();
                    }
                    break;
                case TradingPartnersTable:
                {
                    GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                    SqlDataSource2.InsertParameters["consumer_internal_number"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["consumer_internal_number"].ToString();
                }
                    break;
            }
        }

        //protected void RadGrid2_ItemInserted(object source, GridInsertedEventArgs e)
        //{
        //    string item = getItemName(e.Item.OwnerTableView.Name);
        //    if (e.Exception != null)
        //    {
        //        e.ExceptionHandled = true;
        //        DisplayMessage(item + " cannot be inserted. Reason: " + e.Exception.Message);
        //        Logger.Info(item + " cannot be inserted. Reason: " + e.Exception.Message);
        //        Logger.Error(e);
        //    }
        //    else
        //    {
        //        DisplayMessage(item + " inserted");
        //        Logger.Info(item + " inserted");
        //    }
        //}

        //protected void RadGrid2_ItemUpdated(object source, GridUpdatedEventArgs e)
        //{
        //    string item = getItemName(e.Item.OwnerTableView.Name);
        //    string field = getFieldName(e.Item.OwnerTableView.Name);
        //    if (e.Exception != null)
        //    {
        //        e.KeepInEditMode = true;
        //        e.ExceptionHandled = true;
        //        DisplayMessage(item + " " + e.Item[field].Text + " cannot be updated. Reason: " + e.Exception.Message);
        //        Logger.Info(item + " " + e.Item[field].Text + " cannot be updated. Reason: " + e.Exception.Message);
        //        Logger.Error(e);
        //    }
        //    else
        //    {
        //        DisplayMessage(item + " " + e.Item[field].Text + " updated");
        //        Logger.Info(item + " " + e.Item[field].Text + " updated");
        //    }
        //}

        //protected void RadGrid2_ItemDeleted(object source, GridDeletedEventArgs e)
        //{
        //    string item = getItemName(e.Item.OwnerTableView.Name);
        //    string field = getFieldName(e.Item.OwnerTableView.Name);
        //    if (e.Exception != null)
        //    {
        //        e.ExceptionHandled = true;
        //        DisplayMessage(item + " " + e.Item[field].Text + " cannot be deleted. Reason: " + e.Exception.Message);
        //        Logger.Info(item + " " + e.Item[field].Text + " cannot be deleted. Reason: " + e.Exception.Message);
        //        Logger.Error(e);
        //    }
        //    else
        //    {
        //        DisplayMessage(item + " " + e.Item[field].Text + " deleted");
        //        Logger.Info(item + " " + e.Item[field].Text + " deleted");
        //    }
        //}

        //protected void RadGrid2_InsertCommand(object source, GridCommandEventArgs e)
        //{
        //    switch (e.Item.OwnerTableView.Name)
        //    {
        //        case ConsumersTable:
        //            {
        //                //GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
        //                //SqlDataSource1.InsertParameters["consumer_internal_number"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["consumer_internal_number"].ToString();
        //            }
        //            break;
        //        case TradingPartnersTable:
        //            {
        //                GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
        //                SqlDataSource2.InsertParameters["consumer_internal_number"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["consumer_internal_number"].ToString();
        //            }
        //            break;
        //    }
        //}

        private String getItemName(string tableName)
        {
            switch (tableName)
            {
                case (ConsumersTable):
                    {
                        return "Consumer";
                    }
                case (TradingPartnersTable):
                    {
                        return "TradeingPartner";
                    }
                default: return "";
            }
        }

        private String getFieldName(string tableName)
        {
            switch (tableName)
            {
                case (ConsumersTable):
                    {
                        return "consumer_internal_number";
                    }
                case (TradingPartnersTable):
                    {
                        return "consumer_internal_number";
                    }
                default: return "";
            }
        }

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl(string.Format("<span style='color:red'>{0}</span>", text)));
        }

        protected void ConsumerExportDownload_Click(object sender, EventArgs e)
        {
            const string Filename = @"AWCConsumerExport.xlsx";
            try
            {
                IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
                ConsumerExportExcelFile consumerExport = new ConsumerExportExcelFile();
                Workbook workbook = consumerExport.CreateWorkbook();
                byte[] renderedBytes = null;

                using (MemoryStream ms = new MemoryStream())
                {
                    formatProvider.Export(workbook, ms);
                    renderedBytes = ms.ToArray();
                }

                Response.ClearHeaders();
                Response.ClearContent();
                Response.AppendHeader("content-disposition", "attachment; filename=" + Filename);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(renderedBytes);
                Response.End();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void ServiceExportDownload_Click(object sender, EventArgs e)
        {
            const string Filename = @"AWCServiceExport.xlsx";
            try
            {
                IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
                ServiceExportExcelFile serviceExport = new ServiceExportExcelFile();
                Workbook workbook = serviceExport.CreateWorkbook();
                byte[] renderedBytes = null;

                using (MemoryStream ms = new MemoryStream())
                {
                    formatProvider.Export(workbook, ms);
                    renderedBytes = ms.ToArray();
                }

                Response.ClearHeaders();
                Response.ClearContent();
                Response.AppendHeader("content-disposition", "attachment; filename=" + Filename);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(renderedBytes);
                Response.End();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void DestinationDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            Logger.Info("DestinationDataSource - Deleted record");
        }

        protected void SourceDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            Logger.Info("SourceDataSource - Deleted record");
        }

        protected void DestinationDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            Logger.Info("DestinationDataSource - Inserted record");
        }

        protected void DestinationDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            Logger.Info("DestinationDataSource - Updated record");
        }

        protected void SourceDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            Logger.Info("SourceDataSource - Inserted record");
        }
    }
}