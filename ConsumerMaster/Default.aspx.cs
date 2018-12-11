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
        //private static readonly int IndexColumnConsumerInternalNumber = 0;
        //private static readonly int IndexColumnTradingPartnerString = 1;
        //private static readonly int IndexColumnConsumerFirst = 2;
        //private static readonly int IndexColumnConsumerLast = 3;
        //private static readonly int IndexColumnDateOfBirth = 4;
        //private static readonly int IndexColumnAddressLine1 = 5;
        //private static readonly int IndexColumnAddressLine2 = 6;
        //private static readonly int IndexColumnCity = 7;
        //private static readonly int IndexColumnState = 8;
        //private static readonly int IndexColumnZipCode = 9;
        //private static readonly int IndexColumnIdentifier = 10;
        //private static readonly int IndexColumnGender = 11;

        //private static readonly int IndexRowItemStart = 1;

        //private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);
        //private static readonly ThemableColor InvoiceHeaderForeground = ThemableColor.FromArgb(255, 255, 255, 255);

        //Dictionary<int, string> ceHeader = new Dictionary<int, string>
        //{
        //    {0, "consumer_internal_number"},
        //    {1, "trading_partner_string"},
        //    {2, "consumer_first"},
        //    {3, "consumer_last"},
        //    {4, "date_of_birth"},
        //    {5, "address_line_1"},
        //    {6, "address_line2"},
        //    {7, "city"},
        //    {8, "state"},
        //    {9, "zip_code"},
        //    {10, "identifier"},
        //    {11, "gender"}
        //};

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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

        protected void ConsumerExportDownload_Click(object sender, EventArgs e)
        {
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
            ConsumerExportExcelFile consumerExport = new ConsumerExportExcelFile();
            Workbook workbook = consumerExport.CreateWorkbook();
            //Workbook workbook = this.CreateConsumerExportWorkbook();
            byte[] renderedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                formatProvider.Export(workbook, ms);
                renderedBytes = ms.ToArray();
            }

            Response.ClearHeaders();
            Response.ClearContent();
            Response.AppendHeader("content-disposition", "attachment; filename=ExportedFile" + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(renderedBytes);
            Response.End();
        }

        protected void ServiceExportDownload_Click(object sender, EventArgs e)
        {
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
            ServiceExportExcelFile serviceExport = new ServiceExportExcelFile();
            Workbook workbook = serviceExport.CreateWorkbook();
            //Workbook workbook = this.CreateConsumerExportWorkbook();
            byte[] renderedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                formatProvider.Export(workbook, ms);
                renderedBytes = ms.ToArray();
            }

            Response.ClearHeaders();
            Response.ClearContent();
            Response.AppendHeader("content-disposition", "attachment; filename=ExportedFile" + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(renderedBytes);
            Response.End();
        }

        protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
        {
            string item = getItemName(e.Item.OwnerTableView.Name);
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(item + " cannot be inserted. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(item + " inserted");
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
            }
            else
            {
                DisplayMessage(item + " " + e.Item[field].Text + " updated");
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
            }
            else
            {
                DisplayMessage(item + " " + e.Item[field].Text + " deleted");
            }
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Orders":
                    {
                        GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                        SqlDataSource2.InsertParameters["CustomerID"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["CustomerID"].ToString();
                    }
                    break;
            }
        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditFormItem && !(e.Item is IGridInsertItem) && e.Item.IsInEditMode)
            {
                GridEditFormItem item = e.Item as GridEditFormItem;
                switch (item.OwnerTableView.Name)
                {
                    case "Customers": 
                        TextBox customerIDBox = item["CustomerID"].Controls[0] as TextBox;
                        customerIDBox.Enabled = false;
                        break;
                    case "Details":
                        TextBox productIDBox = item["ProductID"].Controls[0] as TextBox;
                        productIDBox.Enabled = false;
                        break;
                }
            }
        }

        private String getItemName(string tableName)
        {
            switch (tableName)
            {
                case ("Consumers"):
                    {
                        return "Consumer";
                    }
                case ("TradingPartners"):
                    {
                        return "TraderPartner";
                    }
                default: return "";
            }
        }

        private String getFieldName(string tableName)
        {
            switch (tableName)
            {
                case ("Consumers"):
                    {
                        return "consumer_internal_number";
                    }
                case ("TradingParters"):
                    {
                        return "id";
                    }
                case ("Details"):
                    {
                        return "OrderID";
                    }
                default: return "";
            }
        }

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl(string.Format("<span style='color:red'>{0}</span>", text)));
        }


        //private class ConsumerExport
        //{
        //    public int consumer_internal_number;
        //    public string trading_partner_string;
        //    public string consumer_first;
        //    public string consumer_last;
        //    public DateTime date_of_birth;
        //    public string address_line_1;
        //    public string address_line_2;
        //    public string city;
        //    public string state;
        //    public string zip_code;
        //    public string identifier;
        //    public string gender;
        //}

        //private class ServiceExport
        //{
        //    public string consumer_first;
        //    public string consumer_last;
        //    public int consumer_internal_number;
        //    public string trading_partner_string;
        //    public string trading_partner_program_string;
        //    public DateTime start_date_string;
        //    public DateTime end_date_string;
        //    public string diagnosis_code_1_code;
        //    public string composite_procedure_code_string;
        //    public decimal hours;
        //    public int units;
        //    public decimal manual_billable_rate;
        //    public string prior_authorization_number;
        //    public string referral_number;
        //    public string referring_provider_id;
        //    public string referring_provider_first_name;
        //    public string referring_provider_last_name;
        //    public string rendering_provider_id;
        //    public string rendering_provider_first_name;
        //    public string rendering_provider_last_name;
        //}

        //protected void RadGrid1_PreRender(object sender, EventArgs e)
        //{
        //    if (!Page.IsPostBack)
        //    {
        //        //ConsumersGrid.EditIndexes.Add(0);
        //        //ConsumersGrid.Rebind();
        //    }
        //}

        //protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
        //{
        //    if (e.Exception != null)
        //    {
        //        e.ExceptionHandled = true;
        //        e.KeepInInsertMode = true;
        //        DisplayMessage(true, "Consumer cannot be inserted. Reason: " + e.Exception.Message);
        //    }
        //    else
        //    {
        //        DisplayMessage(false, "Consumer inserted");
        //    }
        //}

        //protected void RadGrid1_ItemUpdated(object source, GridUpdatedEventArgs e)
        //{
        //    if (e.Exception != null)
        //    {
        //        e.KeepInEditMode = true;
        //        e.ExceptionHandled = true;
        //        DisplayMessage(true, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " cannot be updated. Reason: " + e.Exception.Message);
        //    }
        //    else
        //    {
        //        DisplayMessage(false, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " updated");
        //    }
        //}

        //protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
        //{
        //    if (e.Exception != null)
        //    {
        //        e.ExceptionHandled = true;
        //        DisplayMessage(true, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " cannot be deleted. Reason: " + e.Exception.Message);
        //    }
        //    else
        //    {
        //        DisplayMessage(false, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " deleted");
        //    }
        //}

        //protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        //{
        //    if (e.CommandName == RadGrid.InitInsertCommandName) //"Add new" button clicked
        //    {
        //        GridEditCommandColumn editColumn = (GridEditCommandColumn)RadGrid1.MasterTableView.GetColumn("EditCommandColumn");
        //        editColumn.Visible = false;
        //    }
        //    else if (e.CommandName == RadGrid.RebindGridCommandName && e.Item.OwnerTableView.IsItemInserted)
        //    {
        //        e.Canceled = true;
        //    }
        //    else
        //    {
        //        GridEditCommandColumn editColumn = (GridEditCommandColumn)RadGrid1.MasterTableView.GetColumn("EditCommandColumn");
        //        if (!editColumn.Visible)
        //            editColumn.Visible = true;
        //    }
        //}
    }
}