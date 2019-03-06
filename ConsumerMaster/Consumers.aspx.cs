using System;
using System.Web.UI;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class Consumers : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (RadGrid1.SelectedIndexes.Count != 0 || RadGrid2.SelectedIndexes.Count != 0) return;
            RadGrid1.SelectedIndexes.Add(0);
            RadGrid2.SelectedIndexes.Add(0);
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

/***********RADGRID2******************************************************************************************************************************/

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

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl($"<span style='color:red'>{text}</span>"));
        }
    }
}