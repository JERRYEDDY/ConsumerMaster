using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
                SetMessage("Consumers cannot be inserted. Reason: " + e.Exception.Message);
                Logger.Info("Consumers cannot be inserted.Reason: " + e.Exception.Message);
                Logger.Error(e);
            }
            else
            {
                DisplayMessage("New consumer is inserted!");
                Logger.Info("New consumer is inserted!");
            }
        }


        //protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
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













        //protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        //{
        //    RadGrid1.SelectedIndexes.Clear();
        //}

        //protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
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

        //protected void RadGrid1_ItemUpdated(object source, GridUpdatedEventArgs e)
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

        //protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
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

        //protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
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

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl(string.Format("<span style='color:red'>{0}</span>", text)));
        }

        private void SetMessage(string message)
        {
            gridMessage = message;
        }

        private string gridMessage = null;

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gridMessage))
            {
                DisplayMessage(gridMessage);
            }
        }
    }
}