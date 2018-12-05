using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //this.BindGrid();
            }
        }

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

        protected void RadGrid1_ItemUpdated(object source, Telerik.Web.UI.GridUpdatedEventArgs e)
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
    }
}