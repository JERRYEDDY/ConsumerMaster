using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Configuration;

namespace ConsumerMaster
{
    public partial class Consumers2 : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        //Declare a global DataTable dtTable 
        public static DataTable dtTable;
        //Declare a global SqlConnection SqlConnection 
        public SqlConnection SqlConnection = new SqlConnection("Data Source=local;Initial Catalog=Northwind;User ID=**");
        //Declare a global SqlDataAdapter SqlDataAdapter 
        public SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
        //Declare a global SqlCommand SqlCommand 
        public SqlCommand SqlCommand = new SqlCommand();

        protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            //Populate the Radgrid 
            dtTable = new DataTable();
            //Open the SqlConnection 
            SqlConnection.Open();
            try
            {
                //Select Query to populate the RadGrid with data from table Customers. 
                string selectQuery = "SELECT CustomerID,CompanyName,ContactName,Address FROM Customers";
                SqlDataAdapter.SelectCommand = new SqlCommand(selectQuery, SqlConnection);
                SqlDataAdapter.Fill(dtTable);
                RadGrid1.DataSource = dtTable;
            }
            finally
            {
                //Close the SqlConnection 
                SqlConnection.Close();
            }
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditableItem of the RadGrid 
            GridEditableItem editedItem = e.Item as GridEditableItem;
            //Get the primary key value using the DataKeyValue. 
            string CustomerID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["CustomerID"].ToString();
            //Access the textbox from the edit form template and store the values in string variables. 
            string CompanyName = (editedItem.FindControl("txtCompanyName") as TextBox).Text;
            string ContactName = (editedItem.FindControl("txtContactName") as TextBox).Text;
            string Address = (editedItem.FindControl("txtAddress") as TextBox).Text;
            try
            {
                //Open the SqlConnection 
                SqlConnection.Open();
                //Update Query to update the Datatable  
                string updateQuery = "UPDATE Customers set CompanyName='" + CompanyName + "',ContactName='" + ContactName + "',Address='" + Address + "' where CustomerID='" + CustomerID + "'";
                SqlCommand.CommandText = updateQuery;
                SqlCommand.Connection = SqlConnection;
                SqlCommand.ExecuteNonQuery();
                //Close the SqlConnection 
                SqlConnection.Close();
            }
            catch (Exception ex)
            {
                RadGrid1.Controls.Add(new LiteralControl("Unable to update Customers. Reason: " + ex.Message));
                e.Canceled = true;
            }
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditFormInsertItem of the RadGrid 
            GridEditFormInsertItem insertedItem = (GridEditFormInsertItem)e.Item;

            //Access the textbox from the insert form template and store the values in string variables. 
            string consumerFirst = ((TextBox) insertedItem.FindControl("consumer_first") as TextBox).Text;
            string consumerLast = ((TextBox) insertedItem.FindControl("consumer_last") as TextBox)?.Text;
            RadDatePicker birthDate = insertedItem.FindControl("birth_date") as RadDatePicker;
            string addressLine1 = ((TextBox) insertedItem.FindControl("address_line_1")).Text;
            string addressLine2 = ((TextBox) insertedItem.FindControl("address_line_2") as TextBox).Text;
            string city = ((TextBox) insertedItem.FindControl("city") as TextBox).Text;
            string state = ((TextBox) insertedItem.FindControl("state") as TextBox).Text;
            string zipCode = ((TextBox) insertedItem.FindControl("zip_code") as TextBox).Text;
            string identifier = ((TextBox) insertedItem.FindControl("identifier") as TextBox).Text;
            string gender = ((TextBox) insertedItem.FindControl("gender") as TextBox).Text;
            string diagnosis = ((TextBox) insertedItem.FindControl("diagnosis") as TextBox).Text;
            string nicknameFirst = ((TextBox) insertedItem.FindControl("nickname_first") as TextBox).Text;
            string nicknameLast = ((TextBox) insertedItem.FindControl("nickname_last") as TextBox).Text;

            string insertQuery =
                "INSERT INTO Consumers (consumer_first, consumer_last, date_of_birth, address_line_1, address_line_2, city, state, zip_code, identifier, gender, diagnosis, nickname_first, nickname_last)" +
                " VALUES (@consumer_first, @consumer_last, @date_of_birth, @address_line_1, @address_line_2, @city, @state, @zip_code, @identifier, @gender, @diagnosis, @nickname_first, @nickname_last)";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = insertQuery;

                        SqlCommand.Parameters.Add("consumer_first", SqlDbType.VarChar).Value = consumerFirst;
                        SqlCommand.Parameters.Add("consumer_last", SqlDbType.VarChar).Value = consumerLast;

                        if (birthDate != null)
                            SqlCommand.Parameters.Add("date_of_birth", SqlDbType.DateTime).Value = birthDate.SelectedDate;

                        SqlCommand.Parameters.Add("address_line_1", SqlDbType.VarChar).Value = addressLine1;
                        SqlCommand.Parameters.Add("address_line_2", SqlDbType.VarChar).Value = addressLine2;
                        SqlCommand.Parameters.Add("city", SqlDbType.VarChar).Value = city;
                        SqlCommand.Parameters.Add("state", SqlDbType.VarChar).Value = state;
                        SqlCommand.Parameters.Add("zip_code", SqlDbType.VarChar).Value = zipCode;
                        SqlCommand.Parameters.Add("identifier", SqlDbType.VarChar).Value = identifier;
                        SqlCommand.Parameters.Add("gender", SqlDbType.VarChar).Value = gender;
                        SqlCommand.Parameters.Add("diagnosis", SqlDbType.VarChar).Value = diagnosis;
                        SqlCommand.Parameters.Add("nickname_first", SqlDbType.VarChar).Value = nicknameFirst;
                        SqlCommand.Parameters.Add("nickname_last", SqlDbType.VarChar).Value = nicknameLast;


                        con.Open();
                        SqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        RadGrid1.Controls.Add(new LiteralControl("Unable to insert Customers. Reason: " + ex.Message));
                        e.Canceled = true;
                    }

                }
            }
        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridDataItem of the RadGrid 
            GridDataItem item = (GridDataItem) e.Item;

            //Get the primary key value using the DataKeyValue. 
            string consumerInternalNumber = item.OwnerTableView.DataKeyValues[item.ItemIndex]["consumer_internal_number"].ToString();

            string deleteQuery = "DELETE from Consumers where consumer_internal_number = @consumerInternalNumber";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {

                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = deleteQuery;


                        SqlCommand.CommandText = deleteQuery;
                        SqlCommand.Connection = SqlConnection;
                        SqlCommand.ExecuteNonQuery();

                        //Close the SqlConnection 
                        SqlConnection.Close();
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("Unable to delete Consumers. Reason: " + ex.Message);
                        Logger.Info("Unable to delete Consumers. Reason: " + ex.Message);
                        e.Canceled = true;
                    }
                }
            }
        }

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl($"<span style='color:red'>{text}</span>"));
        }

        //protected void Page_PreRender(object sender, EventArgs e)
        //{
        //    if (RadGrid1.SelectedIndexes.Count != 0 || RadGrid2.SelectedIndexes.Count != 0) return;
        //    RadGrid1.SelectedIndexes.Add(0);
        //    RadGrid2.SelectedIndexes.Add(0);
        //}
    }
}