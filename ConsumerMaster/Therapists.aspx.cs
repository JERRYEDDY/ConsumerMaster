using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
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
            string selectQuery = "SELECT rendering_provider_id, rendering_provider_name, rendering_provider_first_name, rendering_provider_last_name, rendering_provider_npi FROM Therapists"; 
            RadGrid1.DataSource = GetDataTable(selectQuery, null);
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditFormInsertItem of the RadGrid 
            GridEditFormInsertItem insertedItem = (GridEditFormInsertItem)e.Item;
            string renderingProviderId = null;
            string renderingProviderFirstName = null;
            string renderingProviderLastName = null;

            try
            {
                //Access the textbox from the insert form template and store the values in string variables. 
                renderingProviderId = ((TextBox)insertedItem.FindControl("rendering_provider_id")).Text;
                renderingProviderFirstName = ((TextBox)insertedItem.FindControl("rendering_provider_first_name"))?.Text;
                renderingProviderLastName = ((TextBox)insertedItem.FindControl("rendering_provider_last_name")).Text;
                string renderingProviderNPI = ((TextBox)insertedItem.FindControl("rendering_provider_npi")).Text;

                string insertQuery = "INSERT INTO Therapists(rendering_provider_id, rendering_provider_name, rendering_provider_first_name, rendering_provider_last_name, rendering_provider_npi) " +
                    "VALUES(@rendering_provider_id, @rendering_provider_first_name, @rendering_provider_last_name, @rendering_provider_npi)";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = insertQuery;

                        cmd.Parameters.Add("rendering_provider_id", SqlDbType.VarChar).Value = renderingProviderId;
                        cmd.Parameters.Add("renderingProviderFirstName", SqlDbType.VarChar).Value = renderingProviderFirstName;
                        cmd.Parameters.Add("renderingProviderLastName", SqlDbType.VarChar).Value = renderingProviderLastName;
                        cmd.Parameters.Add("renderingProviderNPI", SqlDbType.VarChar).Value = renderingProviderNPI;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Therapist: {renderingProviderFirstName} {renderingProviderLastName} cannot be inserted. Reason: ";
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
            string renderingProviderId = null;
            string renderingProviderFirstName = null;
            string renderingProviderLastName = null;

            try
            {
                //Get the primary key value using the DataKeyValue. 
                strId = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id"].ToString();
                Int32.TryParse(strId, out int id);

                //Access the textbox from the insert form template and store the values in string variables. 
                renderingProviderId = ((TextBox)editedItem.FindControl("rendering_provider_id")).Text;
                renderingProviderFirstName = ((TextBox)editedItem.FindControl("rendering_provider_first_name"))?.Text;
                renderingProviderLastName = ((TextBox)editedItem.FindControl("rendering_provider_last_name")).Text;
                string renderingProviderNPI = ((TextBox)editedItem.FindControl("rendering_provider_npi")).Text;

                string updateQuery = "UPDATE Therapists SET rendering_provider_id = @rendering_provider_id, rendering_provider_first_name = @rendering_provider_first_name, " +
                    "rendering_provider_last_name = @rendering_provider_last_name, rendering_provider_npi = @rendering_provider_npi WHERE id = @id";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = updateQuery;

                        cmd.Parameters.Add("rendering_provider_id", SqlDbType.VarChar).Value = renderingProviderId;
                        cmd.Parameters.Add("renderingProviderFirstName", SqlDbType.VarChar).Value = renderingProviderFirstName;
                        cmd.Parameters.Add("renderingProviderLastName", SqlDbType.VarChar).Value = renderingProviderLastName;
                        cmd.Parameters.Add("renderingProviderNPI", SqlDbType.VarChar).Value = renderingProviderNPI;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Therapist: {renderingProviderFirstName} {renderingProviderLastName} cannot be updated. Reason: ";
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
            string renderingProviderFirstName = null;
            string renderingProviderLastName = null;

            try
            {
                //Get the primary key value using the DataKeyValue. 
                strId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["id"].ToString();
                Int32.TryParse(strId, out int id);

                renderingProviderFirstName = ((TextBox)item.FindControl("rendering_provider_first_name"))?.Text;
                renderingProviderLastName = ((TextBox)item.FindControl("rendering_provider_last_name")).Text;

                string deleteQuery = "DELETE FROM Therapists WHERE id = @id";

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
                var message = $"Therapist: {renderingProviderFirstName} {renderingProviderLastName} cannot be deleted. Reason: ";
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