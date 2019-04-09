using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class RenderingProviders : Page
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
            string selectQuery = "SELECT id, first_name, last_name, medicad_number, npi_number, taxonomy_number, name FROM RenderingProviders"; 
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
                string medicadNumber = ((RadMaskedTextBox)insertedItem.FindControl("medicad_number")).Text;
                string npiNumber = ((RadMaskedTextBox)insertedItem.FindControl("npi_number")).Text;
                string taxonomyNumber = ((RadTextBox)insertedItem.FindControl("taxonomy_number")).Text;

                string insertQuery = "INSERT INTO RenderingProviders(first_name, last_name, medicad_number, npi_number, taxonomy_number) " +
                    "VALUES(@first_name, @last_name, @medicad_number, @npi_number, @taxonomy_number)";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = insertQuery;

                        cmd.Parameters.Add("first_name", SqlDbType.VarChar).Value = firstName;
                        cmd.Parameters.Add("last_name", SqlDbType.VarChar).Value = lastName;
                        cmd.Parameters.Add("medicad_number", SqlDbType.VarChar).Value = medicadNumber;
                        cmd.Parameters.Add("npi_number", SqlDbType.VarChar).Value = npiNumber;
                        cmd.Parameters.Add("taxonomy_number", SqlDbType.VarChar).Value = taxonomyNumber;

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            var message = $"RenderingProviders: {firstName} {lastName} is inserted.";
                            DisplayMessage(message);
                            Logger.Info(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"RenderingProviders: {firstName} {lastName} cannot be inserted. Reason: ";
                DisplayMessage(message + ex.Message);
                Logger.Info(message + ex.Message);
                e.Canceled = true;
            }
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditableItem of the RadGrid 
            GridEditableItem editedItem = e.Item as GridEditableItem;
            string renderId = null;
            string firstName = null;
            string lastName = null;

            try
            {
                //Get the primary key value using the DataKeyValue. 
                renderId = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id"].ToString();
                Int32.TryParse(renderId, out int id);


                firstName = ((RadTextBox)editedItem.FindControl("first_name"))?.Text;
                lastName = ((RadTextBox)editedItem.FindControl("last_name")).Text;
                string medicadNumber = ((RadMaskedTextBox)editedItem.FindControl("medicad_number")).Text;
                string npiNumber = ((RadMaskedTextBox)editedItem.FindControl("npi_number")).Text;
                string taxonomyNumber = ((RadTextBox)editedItem.FindControl("taxonomy_number")).Text;

                string updateQuery = "UPDATE RenderingProviders SET first_name = @first_name, last_name = @last_name, medicad_number = @medicad_number, " +
                                     "npi_number = @npi_number, taxonomy_number = @taxonomy_number WHERE id = @id";

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
                        cmd.Parameters.Add("medicad_number", SqlDbType.VarChar).Value = medicadNumber;
                        cmd.Parameters.Add("npi_number", SqlDbType.VarChar).Value = npiNumber;
                        cmd.Parameters.Add("taxonomy_number", SqlDbType.VarChar).Value = taxonomyNumber;

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            var message = $"RenderingProviders: {firstName} {lastName} is updated.";
                            DisplayMessage(message);
                            Logger.Info(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"RenderingProviders: {firstName} {lastName} cannot be updated. Reason: ";
                DisplayMessage(message + ex.Message);
                Logger.Info(message + ex.Message);
                e.Canceled = true;
            }
        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridDataItem of the RadGrid 
            GridDataItem item = (GridDataItem)e.Item;
            string renderId = null;
            string first_name = null;
            string last_name = null;

            try
            {
                //Get the primary key value using the DataKeyValue. 
                renderId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["id"].ToString();
                Int32.TryParse(renderId, out int id);

                first_name = item["first_name"].Text;
                last_name = item["last_name"].Text;

                string deleteQuery = "DELETE FROM RenderingProviders WHERE id = @id";

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
                            var message = $"RenderingProviders: {renderId} {first_name} {last_name} is deleted.";
                            DisplayMessage(message);
                            Logger.Info(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"RenderingProviders: {renderId} {first_name} {last_name} cannot be deleted. Reason: ";
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