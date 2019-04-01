using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class Physicians : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        //protected void RadGrid1_ItemUpdated(object source, GridUpdatedEventArgs e)
        //{
        //    if (e.Exception != null)
        //    {
        //        e.ExceptionHandled = true;
        //        e.KeepInEditMode = true;
        //        DisplayMessage("Physician " + e.Item["id"].Text + " cannot be updated. Reason: " + e.Exception.Message);
        //        Logger.Error("Physician " + e.Item["id"].Text + " cannot be updated. Reason: " + e.Exception.Message);
        //        Logger.Error(e);
        //    }
        //    else
        //    {
        //        DisplayMessage("Physician " + e.Item["id"].Text + " updated");
        //        Logger.Info("Physician " + e.Item["id"].Text + " updated");
        //    }
        //}

        //protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
        //{
        //    if (e.Exception != null)
        //    {
        //        e.ExceptionHandled = true;
        //        e.KeepInInsertMode = true;
        //        DisplayMessage("Physician " + e.Item["id"].Text + "cannot be inserted. Reason: " + e.Exception.Message);
        //        Logger.Error("Physician " + e.Item["id"].Text + "cannot be inserted. Reason: " + e.Exception.Message);
        //    }
        //    else
        //    {
        //        DisplayMessage("Physician " + e.Item["id"].Text + " inserted");
        //        Logger.Info("Physician " + e.Item["id"].Text + " inserted");
        //        Logger.Error(e);
        //    }
        //}

        //protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
        //{
        //    if (e.Exception != null)
        //    {
        //        e.ExceptionHandled = true;
        //        DisplayMessage("Physician " + e.Item["id"].Text + " cannot be deleted. Reason: " + e.Exception.Message);
        //        Logger.Info("Physician " + e.Item["id"].Text + " cannot be deleted. Reason: " + e.Exception.Message);
        //        Logger.Error(e);
        //    }
        //    else
        //    {
        //        DisplayMessage("Physician " + e.Item["id"].Text + " deleted");
        //        Logger.Info("Physician " + e.Item["id"].Text + " deleted");
        //    }
        //}

        private DataTable GetDataTable(string selectQuery, params SqlParameter[] parameters)
        {
            DataTable dtTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = selectQuery;

                        if (parameters != null)
                        {
                            foreach (SqlParameter parm in parameters)
                            {
                                cmd.Parameters.Add(parm);
                            }
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter
                        {
                            SelectCommand = cmd
                        };
                        adapter.Fill(dtTable);
                        RadGrid1.DataSource = dtTable;
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("Cannot create datatable. Reason: " + ex.Message);
                        Logger.Info("Cannot create datatable. Reason: " + ex.Message);
                    }
                }
            }

            return dtTable;
        }

        protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            //Populate the Radgrid1
            string selectQuery = "SELECT id, referring_provider_id, referring_provider_name, referring_provider_first_name, referring_provider_last_name, referring_provider_npi FROM Physicians"; 
            RadGrid1.DataSource = GetDataTable(selectQuery, null);
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditFormInsertItem of the RadGrid 
            GridEditFormInsertItem insertedItem = (GridEditFormInsertItem)e.Item;

            string referringProviderId = null;
            string referringProviderFirstName = null;
            string referringProviderLastName = null;

            try
            {
                //Access the textbox from the insert form template and store the values in string variables. 
                RadTextBox referring_provider_id = (RadTextBox)insertedItem.FindControl("referring_provider_id");
                referringProviderId = referring_provider_id.Text;


                referringProviderFirstName = ((RadTextBox)insertedItem.FindControl("referring_provider_first_name"))?.Text;
                referringProviderLastName = ((RadTextBox)insertedItem.FindControl("referring_provider_last_name")).Text;
                string referringProviderNPI = ((RadTextBox)insertedItem.FindControl("referring_provider_npi")).Text;

                string insertQuery = "INSERT INTO Physicians(referring_provider_id, referring_provider_name, referring_provider_first_name, referring_provider_last_name, referring_provider_npi) " +
                    "VALUES(@referring_provider_id, @referring_provider_first_name, @referring_provider_last_name, @referring_provider_npi)";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = insertQuery;

                        cmd.Parameters.Add("referring_provider_id", SqlDbType.VarChar).Value = referringProviderId;
                        cmd.Parameters.Add("referring_provider_first_name", SqlDbType.VarChar).Value = referringProviderFirstName;
                        cmd.Parameters.Add("referring_provider_last_name", SqlDbType.VarChar).Value = referringProviderLastName;
                        cmd.Parameters.Add("referring_provider_npi", SqlDbType.VarChar).Value = referringProviderNPI;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Physician: {referringProviderFirstName} {referringProviderLastName} cannot be inserted. Reason: ";
                DisplayMessage(message + ex.Message);
                Logger.Info(message + ex.Message);
                e.Canceled = true;
            }
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditableItem of the RadGrid 
            GridEditableItem editedItem = e.Item as GridEditableItem;
            string strId = null;
            string referringProviderId = null;
            string referringProviderFirstName = null;
            string referringProviderLastName = null;

            try
            {
                //Get the primary key value using the DataKeyValue. 
                strId = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id"].ToString();
                Int32.TryParse(strId, out int id);

                //Access the textbox from the insert form template and store the values in string variables. 
                referringProviderId = ((TextBox)editedItem.FindControl("referring_provider_id")).Text;
                referringProviderFirstName = ((TextBox)editedItem.FindControl("referring_provider_first_name"))?.Text;
                referringProviderLastName = ((TextBox)editedItem.FindControl("referring_provider_last_name")).Text;
                string referringProviderNPI = ((TextBox)editedItem.FindControl("referring_provider_npi")).Text;

                string updateQuery = "UPDATE Physicians SET referring_provider_id = @referring_provider_id, referring_provider_first_name = @referring_provider_first_name, " +
                    "referring_provider_last_name = @referring_provider_last_name, referring_provider_npi = @referring_provider_npi WHERE id = @id";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = updateQuery;

                        cmd.Parameters.Add("referring_provider_id", SqlDbType.VarChar).Value = referringProviderId;
                        cmd.Parameters.Add("referring_provider_first_name", SqlDbType.VarChar).Value = referringProviderFirstName;
                        cmd.Parameters.Add("referring_provider_last_name", SqlDbType.VarChar).Value = referringProviderLastName;
                        cmd.Parameters.Add("referring_provider_npi", SqlDbType.VarChar).Value = referringProviderNPI;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Physician: {referringProviderFirstName} {referringProviderLastName} cannot be updated. Reason: ";
                DisplayMessage(message + ex.Message);
                Logger.Info(message + ex.Message);
                e.Canceled = true;
            }
        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridDataItem of the RadGrid 
            GridDataItem item = (GridDataItem)e.Item;
            string strId = null;
            string referringProviderFirstName = null;
            string referringProviderLastName = null;

            try
            {
                //Get the primary key value using the DataKeyValue. 
                strId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["id"].ToString();
                Int32.TryParse(strId, out int id);

                referringProviderFirstName = ((TextBox)item.FindControl("referring_provider_first_name"))?.Text;
                referringProviderLastName = ((TextBox)item.FindControl("referring_provider_last_name")).Text;

                string deleteQuery = "DELETE FROM Physicians WHERE id = @id";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = deleteQuery;

                        cmd.Parameters.Add("id", SqlDbType.Int).Value = id;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Physician: {referringProviderFirstName} {referringProviderLastName} cannot be deleted. Reason: ";
                DisplayMessage(message + ex.Message);
                Logger.Info(message + ex.Message);
                e.Canceled = true;
            }
        }

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl($"<span style='color:red'>{text}</span>"));
        }
    }
}