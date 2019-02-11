using System;
using System.Web.UI;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class Consumers2 : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private const string ConsumersTable = "Consumers";
        private const string TradingPartnersTable = "TradingPartners";
        private const string CompositeProcedureCodesTable = "CompositeProcedureCodes";

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (RadGrid1.SelectedIndexes.Count == 0)
                RadGrid1.SelectedIndexes.Add(0);
            if (RadGrid2.SelectedIndexes.Count == 0)
            {
                RadGrid2.Rebind();
                RadGrid2.SelectedIndexes.Add(0);
            }
        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {

            }
        }

        protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage("Consumer " + e.Item["consumer_internal_number"].Text + " cannot be inserted. Reason: " + e.Exception.Message);
                Logger.Info("Consumer " + e.Item["consumer_internal_number"].Text + " cannot be inserted. Reason: " + e.Exception.Message);
                Logger.Error(e);
            }
            else
            {
                DisplayMessage("Consumer " + e.Item["consumer_internal_number"].Text + " inserted!");
                Logger.Info("Consumer " + e.Item["consumer_internal_number"].Text + " inserted!");
            }
        }

        //protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        //{
        //    RadGrid1.SelectedIndexes.Clear();
        //}

        protected void RadGrid1_ItemUpdated(object source, GridUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage("Consumer " + e.Item["consumer_internal_number"].Text + " cannot be updated. Reason: " + e.Exception.Message);
                Logger.Error("Consumer " + e.Item["consumer_internal_number"].Text + " cannot be updated. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage("Consumer " + e.Item["consumer_internal_number"].Text + " updated");
                Logger.Info("Consumer " + e.Item["consumer_internal_number"].Text + " updated");
            }
        }

        protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage("Consumer " + e.Item["consumer_internal_number"].Text + " cannot be deleted. Reason: " + e.Exception.Message);
                Logger.Error("Consumer " + e.Item["consumer_internal_number"].Text + " cannot be deleted. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage("Consumer " + e.Item["consumer_internal_number"].Text + " deleted");
                Logger.Info("Consumer " + e.Item["consumer_internal_number"].Text + " deleted");
            }
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            //switch (e.Item.OwnerTableView.Name)
            //{
            //    case ConsumersTable:
            //        {
            //            //GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
            //            //SqlDataSource1.InsertParameters["consumer_internal_number"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["consumer_internal_number"].ToString();
            //        }
            //        break;
            //    case TradingPartnersTable:
            //        {
            //            GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
            //            SqlDataSource2.InsertParameters["consumer_internal_number"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["consumer_internal_number"].ToString();
            //        }
            //        break;
            //}
        }

        protected void RadGrid2_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {

            }
        }

        protected void RadGrid2_ItemInserted(object source, GridInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " cannot be inserted. Reason: " + e.Exception.Message);
                Logger.Error(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " cannot be inserted. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " cannot be inserted. Reason: " + e.Exception.Message);
                Logger.Info(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " cannot be inserted. Reason: " + e.Exception.Message);
            }
        }

        protected void RadGrid2_ItemUpdated(object source, GridUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " cannot be updated. Reason: " + e.Exception.Message);
                Logger.Error(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " cannot be updated. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " updated");
                Logger.Info(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " updated");
            }
        }

        protected void RadGrid2_ItemDeleted(object source, GridDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " cannot be deleted. Reason: " + e.Exception.Message);
                Logger.Error(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " cannot be deleted. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " deleted");
                Logger.Info(e.Item["consumer_internal_number"].Text + "-" + "Trading Partners " + e.Item["Trading Partners"].Text + " deleted");
            }
        }

        protected void RadGrid2_InsertCommand(object source, GridCommandEventArgs e)
        {
            //switch (e.Item.OwnerTableView.Name)
            //{
            //    case ConsumersTable:
            //        {
            //            //GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
            //            //SqlDataSource1.InsertParameters["consumer_internal_number"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["consumer_internal_number"].ToString();
            //        }
            //        break;
            //    case TradingPartnersTable:
            //        {
            //            GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
            //            SqlDataSource2.InsertParameters["consumer_internal_number"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["consumer_internal_number"].ToString();
            //        }
            //        break;
            //}
        }

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl(string.Format("<span style='color:red'>{0}</span>", text)));
        }
    }
}