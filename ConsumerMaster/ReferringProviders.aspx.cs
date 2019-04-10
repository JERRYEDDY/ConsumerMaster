using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class ReferringProviders : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {

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
            string selectQuery = "SELECT id, first_name, last_name, npi_number, name FROM ReferringProviders"; 
            RadGrid1.DataSource = GetDataTable(selectQuery, null);
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditFormInsertItem of the RadGrid 
            GridEditFormInsertItem insertedItem = (GridEditFormInsertItem)e.Item;
            string firstName = null;
            string lastName = null;

            try
            {
                //Access the textbox from the insert form template and store the values in string variables. 
                firstName = ((RadTextBox)insertedItem.FindControl("first_name"))?.Text;
                lastName = ((RadTextBox)insertedItem.FindControl("last_name")).Text;
                string npiNumber = ((RadMaskedTextBox)insertedItem.FindControl("npi_number")).Text;

                string insertQuery = "INSERT INTO ReferringProviders(first_name, last_name, npi_number) VALUES(@first_name, @last_name, @npi_number)";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = insertQuery;

                        cmd.Parameters.Add("first_name", SqlDbType.VarChar).Value = firstName;
                        cmd.Parameters.Add("last_name", SqlDbType.VarChar).Value = lastName;
                        cmd.Parameters.Add("npi_number", SqlDbType.VarChar).Value = npiNumber;

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            var message = $"ReferringProviders: {firstName} {lastName} is inserted.";
                            DisplayMessage(message);
                            Logger.Info(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"ReferringProviders: {firstName} {lastName} cannot be inserted. Reason: ";
                DisplayMessage(message + ex.Message);
                Logger.Info(message + ex.Message);
                e.Canceled = true;
            }
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditableItem of the RadGrid 
            GridEditableItem editedItem = e.Item as GridEditableItem;
            string referId = null;
            string firstName = null;
            string lastName = null;

            try
            {
                //Get the primary key value using the DataKeyValue. 
                referId = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id"].ToString();
                Int32.TryParse(referId, out int id);

                firstName = ((RadTextBox)editedItem.FindControl("first_name"))?.Text;
                lastName = ((RadTextBox)editedItem.FindControl("last_name")).Text;
                string npiNumber = ((RadMaskedTextBox)editedItem.FindControl("npi_number")).Text;

                string updateQuery = "UPDATE ReferringProviders SET first_name = @first_name, last_name = @last_name, npi_number = @npi_number WHERE id = @id";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = updateQuery;

                        cmd.Parameters.Add("id", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("first_name", SqlDbType.VarChar).Value = firstName;
                        cmd.Parameters.Add("last_name", SqlDbType.VarChar).Value = lastName;
                        cmd.Parameters.Add("npi_number", SqlDbType.VarChar).Value = npiNumber;

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            var message = $"ReferringProviders: {referId} {firstName} {lastName} is updated.";
                            DisplayMessage(message);
                            Logger.Info(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"ReferringProviders: {referId} {firstName} {lastName} cannot be updated. Reason: ";
                DisplayMessage(message + ex.Message);
                Logger.Info(message + ex.Message);
                e.Canceled = true;
            }
        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridDataItem of the RadGrid 
            GridDataItem item = (GridDataItem)e.Item;
            string referId = null;
            string firstName = null;
            string lastName = null;

            try
            {
                //Get the primary key value using the DataKeyValue. 
                referId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["id"].ToString();
                Int32.TryParse(referId, out int id);

                firstName = ((RadTextBox)item.FindControl("first_name"))?.Text;
                lastName = ((RadTextBox)item.FindControl("last_name")).Text;

                string deleteQuery = "DELETE FROM ReferringProviders WHERE id = @id";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = deleteQuery;

                        cmd.Parameters.Add("id", SqlDbType.Int).Value = id;

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            var message = $"ReferringProviders: {referId} {firstName} {lastName} is deleted.";
                            DisplayMessage(message);
                            Logger.Info(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"ReferringProviders: {referId} {firstName} {lastName} cannot be deleted. Reason: ";
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