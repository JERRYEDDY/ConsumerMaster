using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;
using System.Collections.Generic;

namespace ConsumerMaster
{
    public partial class _Default : Page
    {
        private static readonly int IndexColumnConsumerInternalNumber = 0;
        private static readonly int IndexColumnTradingPartnerString = 1;
        private static readonly int IndexColumnConsumerFirst = 2;
        private static readonly int IndexColumnConsumerLast = 3;
        private static readonly int IndexColumnDateOfBirth = 4;
        private static readonly int IndexColumnAddressLine1 = 5;
        private static readonly int IndexColumnAddressLine2 = 6;
        private static readonly int IndexColumnCity = 7;
        private static readonly int IndexColumnState = 8;
        private static readonly int IndexColumnZipCode = 9;
        private static readonly int IndexColumnIdentifier = 10;
        private static readonly int IndexColumnGender = 11;

        private static readonly int IndexRowItemStart = 1;

        private static readonly ThemableColor InvoiceBackground = ThemableColor.FromArgb(255, 44, 62, 80);
        private static readonly ThemableColor InvoiceHeaderForeground = ThemableColor.FromArgb(255, 255, 255, 255);

        Dictionary<int, string> ceHeader = new Dictionary<int, string>
        {
            {0, "consumer_internal_number"},
            {1, "trading_partner_string"},
            {2, "consumer_first"},
            {3, "consumer_last"},
            {4, "date_of_birth"},
            {5, "address_line_1"},
            {6, "address_line2"},
            {7, "city"},
            {8, "state"},
            {9, "zip_code"},
            {10, "identifier"},
            {11, "gender"}
        };
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //this.BindGrid();

            }
        }

        protected void Download_Click(object sender, EventArgs e)
        {
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();

            Workbook workbook = this.CreateWorkbook();
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

        private Workbook CreateWorkbook()
        {
            Workbook workbook = new Workbook();
            workbook.Sheets.Add(SheetType.Worksheet);

            Worksheet worksheet = workbook.ActiveWorksheet;

            string ceQuery = "SELECT consumer_internal_number, consumer_first, consumer_last, date_of_birth, address_line_1, address_line_2, city, state, zip_code, identifier, gender " +
                             "FROM Consumers";
            DataTable ceDataTable = GetDataTable(ceQuery);

            int totalConsumers = ceDataTable.Rows.Count;

            PrepareConsumerExportDocument(worksheet, totalConsumers);

            int currentRow = IndexRowItemStart + 1;

            foreach (DataRow dr in ceDataTable.Rows)
            {
                worksheet.Cells[currentRow, IndexColumnConsumerInternalNumber].SetValue(dr["consumer_internal_number"].ToString());
                worksheet.Cells[currentRow, IndexColumnTradingPartnerString].SetValue("trading_partner_string");
                worksheet.Cells[currentRow, IndexColumnConsumerFirst].SetValue(dr["consumer_first"].ToString());
                worksheet.Cells[currentRow, IndexColumnConsumerLast].SetValue(dr["consumer_last"].ToString());
                worksheet.Cells[currentRow, IndexColumnDateOfBirth].SetValue(dr["date_of_birth"].ToString());
                worksheet.Cells[currentRow, IndexColumnAddressLine1].SetValue(dr["address_line_1"].ToString());
                worksheet.Cells[currentRow, IndexColumnAddressLine2].SetValue(dr["address_line_2"].ToString());
                worksheet.Cells[currentRow, IndexColumnCity].SetValue(dr["city"].ToString());
                worksheet.Cells[currentRow, IndexColumnState].SetValue(dr["state"].ToString());
                worksheet.Cells[currentRow, IndexColumnZipCode].SetValue(dr["zip_code"].ToString());
                worksheet.Cells[currentRow, IndexColumnIdentifier].SetValue(dr["identifier"].ToString());
                worksheet.Cells[currentRow, IndexColumnGender].SetValue(dr["gender"].ToString());

                currentRow++;
            }

            for (int i = 0; i < worksheet.Columns.Count; i++)
            {
                worksheet.Columns[i].AutoFitWidth();
            }

            return workbook;
        }

        private void PrepareConsumerExportDocument(Worksheet worksheet, int itemsCount)
        {
            int lastItemIndexRow = IndexRowItemStart + itemsCount;

            CellIndex firstRowFirstCellIndex = new CellIndex(0, 0);
            CellIndex firstRowLastCellIndex = new CellIndex(0, 4);
            CellIndex lastRowFirstCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnConsumerInternalNumber);
            CellIndex lastRowLastCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnGender);
            worksheet.Cells[firstRowFirstCellIndex, firstRowLastCellIndex].MergeAcross();
            CellBorder border = new CellBorder(CellBorderStyle.Medium, InvoiceBackground);
            worksheet.Cells[firstRowFirstCellIndex, lastRowLastCellIndex].SetBorders(new CellBorders(border, border, border, border, null, null, null, null));
            worksheet.Cells[lastRowFirstCellIndex, lastRowLastCellIndex].SetBorders(new CellBorders(border, border, border, border, null, null, null, null));
            //worksheet.Cells[firstRowFirstCellIndex].SetValue("INVOICE");
           // worksheet.Cells[firstRowFirstCellIndex].SetFontSize(20);

            worksheet.Cells[IndexRowItemStart, IndexColumnConsumerInternalNumber].SetValue(ceHeader[0]);
            worksheet.Cells[IndexRowItemStart, IndexColumnTradingPartnerString].SetValue(ceHeader[1]);
            worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetValue(ceHeader[2]);
            worksheet.Cells[IndexRowItemStart, IndexColumnConsumerFirst].SetHorizontalAlignment(RadHorizontalAlignment.Right);
            worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetValue(ceHeader[3]);
            worksheet.Cells[IndexRowItemStart, IndexColumnConsumerLast].SetHorizontalAlignment(RadHorizontalAlignment.Right);
            worksheet.Cells[IndexRowItemStart, IndexColumnDateOfBirth].SetValue(ceHeader[4]);
            worksheet.Cells[IndexRowItemStart, IndexColumnDateOfBirth].SetHorizontalAlignment(RadHorizontalAlignment.Right);
            worksheet.Cells[IndexRowItemStart, IndexColumnAddressLine1].SetValue(ceHeader[5]);
            worksheet.Cells[IndexRowItemStart, IndexColumnAddressLine2].SetValue(ceHeader[6]);
            worksheet.Cells[IndexRowItemStart, IndexColumnCity].SetValue(ceHeader[7]);
            worksheet.Cells[IndexRowItemStart, IndexColumnState].SetValue(ceHeader[8]);
            worksheet.Cells[IndexRowItemStart, IndexColumnZipCode].SetValue(ceHeader[9]);
            worksheet.Cells[IndexRowItemStart, IndexColumnIdentifier].SetValue(ceHeader[10]);
            worksheet.Cells[IndexRowItemStart, IndexColumnGender].SetValue(ceHeader[11]);

            //worksheet.Cells[IndexRowItemStart, IndexColumnProductID, IndexRowItemStart, IndexColumnSubTotal].SetFill(new GradientFill(GradientType.Horizontal, InvoiceBackground, InvoiceBackground));
            //worksheet.Cells[IndexRowItemStart, IndexColumnProductID, IndexRowItemStart, IndexColumnSubTotal].SetForeColor(InvoiceHeaderForeground);
            //worksheet.Cells[IndexRowItemStart, IndexColumnUnitPrice, lastItemIndexRow, IndexColumnUnitPrice].SetFormat(new CellValueFormat(EnUSCultureAccountFormatString));
            //worksheet.Cells[IndexRowItemStart, IndexColumnSubTotal, lastItemIndexRow, IndexColumnSubTotal].SetFormat(new CellValueFormat(EnUSCultureAccountFormatString));

            //worksheet.Cells[lastItemIndexRow + 1, IndexColumnUnitPrice].SetValue("TOTAL: ");
            //worksheet.Cells[lastItemIndexRow + 1, IndexColumnSubTotal].SetFormat(new CellValueFormat(EnUSCultureAccountFormatString));

            //string subTotalColumnCellRange = NameConverter.ConvertCellRangeToName(new CellIndex(IndexRowItemStart + 1, IndexColumnSubTotal),new CellIndex(lastItemIndexRow, IndexColumnSubTotal));
            //worksheet.Cells[lastItemIndexRow + 1, IndexColumnSubTotal].SetValue(string.Format("=SUM({0})", subTotalColumnCellRange));
            //worksheet.Cells[lastItemIndexRow + 1, IndexColumnUnitPrice, lastItemIndexRow + 1, IndexColumnSubTotal].SetFontSize(20);
        }

        private static DataTable GetDataTable(string queryString)
        {
            using (SqlConnection sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString))
            {
                sqlConnect.Open();
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnect))
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
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