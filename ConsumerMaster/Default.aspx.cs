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

        protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
                DisplayMessage(true, "Consumer cannot be inserted. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(false, "Consumer inserted");
            }
        }

        protected void RadGrid1_ItemUpdated(object source, GridUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
                DisplayMessage(true, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " cannot be updated. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(false, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " updated");
            }
        }

        protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(true, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " cannot be deleted. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(false, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " deleted");
            }
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.InitInsertCommandName) //"Add new" button clicked
            {
                GridEditCommandColumn editColumn = (GridEditCommandColumn)RadGrid1.MasterTableView.GetColumn("EditCommandColumn");
                editColumn.Visible = false;
            }
            else if (e.CommandName == RadGrid.RebindGridCommandName && e.Item.OwnerTableView.IsItemInserted)
            {
                e.Canceled = true;
            }
            else
            {
                GridEditCommandColumn editColumn = (GridEditCommandColumn)RadGrid1.MasterTableView.GetColumn("EditCommandColumn");
                if (!editColumn.Visible)
                    editColumn.Visible = true;
            }
        }

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //ConsumersGrid.EditIndexes.Add(0);
                //ConsumersGrid.Rebind();
            }
        }

        private void DisplayMessage(bool isError, string text)
        {
            Label label = (isError) ? this.Label1 : this.Label2;
            label.Text = text;
        }
    }
}