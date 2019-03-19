using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

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
            string CustomerID = (insertedItem.FindControl("txtCustomerID") as TextBox).Text;
            string CompanyName = (insertedItem.FindControl("txtCompanyName") as TextBox).Text;
            string ContactName = (insertedItem.FindControl("txtContactName") as TextBox).Text;
            string Address = (insertedItem.FindControl("txtAddress") as TextBox).Text;
            try
            {
                //Open the SqlConnection 
                SqlConnection.Open();
                //Update Query to insert into  the database  
                string insertQuery = "INSERT into  Customers(CustomerID,CompanyName,ContactName,Address) values('" + CustomerID + "','" + CompanyName + "','" + ContactName + "','" + Address + "')";
                SqlCommand.CommandText = insertQuery;
                SqlCommand.Connection = SqlConnection;
                SqlCommand.ExecuteNonQuery();
                //Close the SqlConnection 
                SqlConnection.Close();
            }
            catch (Exception ex)
            {
                RadGrid1.Controls.Add(new LiteralControl("Unable to insert Customers. Reason: " + ex.Message));
                e.Canceled = true;
            }

        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridDataItem of the RadGrid 
            GridDataItem item = (GridDataItem)e.Item;
            //Get the primary key value using the DataKeyValue. 
            string CustomerID = item.OwnerTableView.DataKeyValues[item.ItemIndex]["CustomerID"].ToString();
            try
            {
                //Open the SqlConnection 
                SqlConnection.Open();
                string deleteQuery = "DELETE from Consumers where consumer_internal_number='" + CustomerID + "'";
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