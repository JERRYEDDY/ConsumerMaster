using System;
using System.Web.UI;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class Therapists : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RadGrid1_ItemUpdated(object source, GridUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
                DisplayMessage("Therapist " + e.Item["id"].Text + " cannot be updated. Reason: " + e.Exception.Message);
                Logger.Error("Therapist " + e.Item["id"].Text + " cannot be updated. Reason: " + e.Exception.Message);
                Logger.Error(e);
            }
            else
            {
                DisplayMessage("Therapist " + e.Item["id"].Text + " updated");
                Logger.Info("Therapist " + e.Item["id"].Text + " updated");
            }
        }

        protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
                DisplayMessage("Therapist " + e.Item["id"].Text + "cannot be inserted. Reason: " + e.Exception.Message);
                Logger.Error("Therapist " + e.Item["id"].Text + "cannot be inserted. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage("Therapist " + e.Item["id"].Text + " inserted");
                Logger.Info("Therapist " + e.Item["id"].Text + " inserted");
                Logger.Error(e);
            }
        }

        protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage("Therapist " + e.Item["id"].Text + " cannot be deleted. Reason: " + e.Exception.Message);
                Logger.Info("Therapist " + e.Item["id"].Text + " cannot be deleted. Reason: " + e.Exception.Message);
                Logger.Error(e);
            }
            else
            {
                DisplayMessage("Therapist " + e.Item["id"].Text + " deleted");
                Logger.Info("Therapist " + e.Item["id"].Text + " deleted");
            }
        }

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl($"<span style='color:red'>{text}</span>"));
        }
    }
}