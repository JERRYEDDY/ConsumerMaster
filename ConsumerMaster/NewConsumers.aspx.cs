﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class NewConsumers : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (RadGrid1.SelectedIndexes.Count != 0 || RadGrid2.SelectedIndexes.Count != 0) return;
            //RadGrid1.SelectedIndexes.Add(0);
            //RadGrid2.SelectedIndexes.Add(0);
        }

 /***********RADGRID1******************************************************************************************************************************/
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
            string selectQuery = "SELECT consumer_internal_number, consumer_first, consumer_last, date_of_birth, address_line_1, address_line_2, " +
                                "city, state, zip_code, identifier, gender, diagnosis, nickname_first, nickname_last FROM Consumers ORDER BY consumer_last";
            RadGrid1.DataSource = GetDataTable(selectQuery, null);
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditFormInsertItem of the RadGrid 
            GridEditFormInsertItem insertedItem = (GridEditFormInsertItem)e.Item;
            string consumerFirst = null;
            string consumerLast = null;

            try
            {
                //Access the textbox from the insert form template and store the values in string variables. 
                consumerFirst = ((RadTextBox)insertedItem.FindControl("consumer_first")).Text;
                consumerLast = ((RadTextBox)insertedItem.FindControl("consumer_last"))?.Text;
                RadDatePicker dateOfBirth = (RadDatePicker)insertedItem.FindControl("date_of_birth");
                string addressLine1 = ((RadTextBox)insertedItem.FindControl("address_line_1")).Text;
                string addressLine2 = ((RadTextBox)insertedItem.FindControl("address_line_2")).Text;
                string city = ((RadTextBox)insertedItem.FindControl("city")).Text;
                string state = ((RadComboBox) insertedItem.FindControl("state")).SelectedValue;
                string zipCode = ((RadMaskedTextBox)insertedItem.FindControl("zip_code")).Text;
                string identifier = ((RadMaskedTextBox) insertedItem.FindControl("identifier")).Text;
                string gender = ((RadRadioButtonList) insertedItem.FindControl("gender")).SelectedValue;
                string diagnosis = ((RadTextBox)insertedItem.FindControl("diagnosis_code")).Text;
                string nicknameFirst = ((RadTextBox)insertedItem.FindControl("nickname_first")).Text;
                string nicknameLast = ((RadTextBox)insertedItem.FindControl("nickname_last")).Text;

                string insertQuery =
                    "INSERT INTO Consumers (consumer_first, consumer_last, date_of_birth, address_line_1, address_line_2, city, state, zip_code, identifier, gender, diagnosis, " +
                    "nickname_first, nickname_last)" +
                    " VALUES (@consumer_first, @consumer_last, @date_of_birth, @address_line_1, @address_line_2, @city, @state, @zip_code, @identifier, @gender, @diagnosis, " +
                    "@nickname_first, @nickname_last)";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = insertQuery;

                        cmd.Parameters.Add("consumer_first", SqlDbType.VarChar).Value = consumerFirst;
                        cmd.Parameters.Add("consumer_last", SqlDbType.VarChar).Value = consumerLast;

                        if (dateOfBirth != null)
                            cmd.Parameters.Add("date_of_birth", SqlDbType.DateTime).Value = dateOfBirth.SelectedDate;

                        cmd.Parameters.Add("address_line_1", SqlDbType.VarChar).Value = addressLine1;
                        cmd.Parameters.Add("address_line_2", SqlDbType.VarChar).Value = addressLine2;
                        cmd.Parameters.Add("city", SqlDbType.VarChar).Value = city;
                        cmd.Parameters.Add("state", SqlDbType.VarChar).Value = state;
                        cmd.Parameters.Add("zip_code", SqlDbType.VarChar).Value = zipCode;
                        cmd.Parameters.Add("identifier", SqlDbType.VarChar).Value = identifier;
                        cmd.Parameters.Add("gender", SqlDbType.VarChar).Value = gender;
                        cmd.Parameters.Add("diagnosis", SqlDbType.VarChar).Value = diagnosis;
                        cmd.Parameters.Add("nickname_first", SqlDbType.VarChar).Value = nicknameFirst;
                        cmd.Parameters.Add("nickname_last", SqlDbType.VarChar).Value = nicknameLast;

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            var message = $"Consumer: {consumerFirst} {consumerLast} is inserted.";
                            DisplayMessage(message);
                            Logger.Info(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Consumer: {consumerFirst} {consumerLast} cannot be inserted. Reason: ";
                DisplayMessage(message + ex.Message);
                Logger.Info(message + ex.Message);
                e.Canceled = true;
            }
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditableItem of the RadGrid 
            GridEditableItem editedItem = e.Item as GridEditableItem;
            string consumerId = null;
            string consumerFirst = null;
            string consumerLast = null;

            try
            {
                //Get the primary key value using the DataKeyValue. 
                consumerId = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["consumer_internal_number"].ToString();
                Int32.TryParse(consumerId, out int consumer_internal_number);

                //Access the controls from the edit form template and store the values. 
                consumerFirst = ((RadTextBox)editedItem.FindControl("consumer_first")).Text;
                consumerLast = ((RadTextBox)editedItem.FindControl("consumer_last"))?.Text;
                RadDatePicker dateOfBirth = (RadDatePicker)editedItem.FindControl("date_of_birth");
                string addressLine1 = ((RadTextBox)editedItem.FindControl("address_line_1")).Text;
                string addressLine2 = ((RadTextBox)editedItem.FindControl("address_line_2")).Text;
                string city = ((RadTextBox)editedItem.FindControl("city")).Text;
                string state = ((RadComboBox) editedItem.FindControl("state")).SelectedValue;
                string zipCode = ((RadMaskedTextBox)editedItem.FindControl("zip_code")).Text;
                string identifier = ((RadMaskedTextBox)editedItem.FindControl("identifier")).Text;
                string gender = ((RadRadioButtonList) editedItem.FindControl("gender")).SelectedValue;
                string diagnosis = ((RadTextBox)editedItem.FindControl("diagnosis_code")).Text;
                string nicknameFirst = ((RadTextBox)editedItem.FindControl("nickname_first")).Text;
                string nicknameLast = ((RadTextBox)editedItem.FindControl("nickname_last")).Text;

                string updateQuery = "UPDATE Consumers SET consumer_first=@consumer_first, consumer_last=@consumer_last, date_of_birth=@date_of_birth, address_line_1=@address_line_1, address_line_2=@address_line_2, " +
                       "city=@city, state=@state, zip_code=@zip_code, identifier=@identifier, gender=@gender, diagnosis=@diagnosis, nickname_first=@nickname_first, nickname_last=@nickname_last " +
                       " WHERE consumer_internal_number=@consumer_internal_number";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = updateQuery;
                            cmd.Parameters.Add("consumer_internal_number", SqlDbType.Int).Value = consumer_internal_number;
                            cmd.Parameters.Add("consumer_first", SqlDbType.VarChar).Value = consumerFirst;
                            cmd.Parameters.Add("consumer_last", SqlDbType.VarChar).Value = consumerLast;

                            if (dateOfBirth != null)
                                cmd.Parameters.Add("date_of_birth", SqlDbType.DateTime).Value = dateOfBirth.SelectedDate;

                            cmd.Parameters.Add("address_line_1", SqlDbType.VarChar).Value = addressLine1;
                            cmd.Parameters.Add("address_line_2", SqlDbType.VarChar).Value = addressLine2;
                            cmd.Parameters.Add("city", SqlDbType.VarChar).Value = city;
                            cmd.Parameters.Add("state", SqlDbType.VarChar).Value = state;
                            cmd.Parameters.Add("zip_code", SqlDbType.VarChar).Value = zipCode;
                            cmd.Parameters.Add("identifier", SqlDbType.VarChar).Value = identifier;
                            cmd.Parameters.Add("gender", SqlDbType.VarChar).Value = gender;
                            cmd.Parameters.Add("diagnosis", SqlDbType.VarChar).Value = diagnosis;
                            cmd.Parameters.Add("nickname_first", SqlDbType.VarChar).Value = nicknameFirst;
                            cmd.Parameters.Add("nickname_last", SqlDbType.VarChar).Value = nicknameLast;

                            con.Open();
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                var message = $"Consumer: {consumerFirst} {consumerLast} is updated.";
                                DisplayMessage(message);
                                Logger.Info(message);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Consumer: {consumerId} {consumerFirst} {consumerLast} cannot be updated. Reason: ";
                DisplayMessage(message + ex.Message);
                Logger.Info(message + ex.Message);
                e.Canceled = true;
            }
        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridDataItem of the RadGrid 
            GridDataItem item = (GridDataItem)e.Item;
            string consumerId = null;
            string consumerFirst = null;
            string consumerLast = null;

            try
            {
                //Get the primary key value using the DataKeyValue. 
                consumerId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["consumer_internal_number"].ToString();
                Int32.TryParse(consumerId, out int consumerInternalNumber);

                consumerFirst = item["consumer_first"].Text;
                consumerLast = item["consumer_last"].Text;

                string deleteQuery = "DELETE from Consumers where consumer_internal_number = @consumerInternalNumber";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = deleteQuery;

                        cmd.Parameters.Add("consumerInternalNumber", SqlDbType.Int).Value = consumerInternalNumber;

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            var message = $"Consumer: {consumerId} {consumerFirst} {consumerLast} is deleted.";
                            DisplayMessage(message);
                            Logger.Info(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Consumer: {consumerId} {consumerFirst}  {consumerLast} cannot be deleted. Reason: ";
                DisplayMessage(message + ex.Message);
                Logger.Info(message + ex.Message);
                e.Canceled = true;
            }
        }

 /***********RADGRID2******************************************************************************************************************************/
        protected void RadGrid2_ItemInserted(object source, GridInsertedEventArgs e)
        {
            GridEditFormInsertItem insertedItem = (GridEditFormInsertItem)e.Item;

            try
            {
                string consumerId = insertedItem.OwnerTableView.DataKeyValues[0]["consumer_internal_number"].ToString();
                string tradingPartnerName = ((RadDropDownList)insertedItem.FindControl("trading_partners")).SelectedText;

                if (e.Exception != null)
                {
                    e.ExceptionHandled = true;
                    var message = $"Consumer: {consumerId} - Trading Partners: {tradingPartnerName} cannot be inserted. Reason: {e.Exception.Message}";
                    DisplayMessage(message);
                    Logger.Error(message);
                }
                else
                {
                    var message = $"Consumer: {consumerId} - Trading Partners: {tradingPartnerName}  is inserted";
                    DisplayMessage(message);
                    Logger.Info(message);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
                Logger.Info(ex.Message);
                throw;
            }
        }

        protected void RadGrid2_ItemDeleted(object source, GridDeletedEventArgs e)
        {
            GridDataItem deletedItem = (GridDataItem)e.Item;

            try
            {
                string consumerId = deletedItem.OwnerTableView.DataKeyValues[0]["consumer_internal_number"].ToString();
                string cId = deletedItem["consumer_internal_number"].Text;
                string tradingPartnerName = deletedItem["trading_partner_name"].Text;

                if (e.Exception != null)
                {
                    e.ExceptionHandled = true;
                    var message = $"Consumer: {consumerId} - Trading Partners: {tradingPartnerName} cannot be deleted. Reason: {e.Exception.Message}";
                    Logger.Error(message);
                }
                else
                {
                    var message = $"Consumer: {consumerId} - Trading Partners: {tradingPartnerName}  is deleted";
                    DisplayMessage(message);
                    Logger.Info(message);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
                Logger.Info(ex.Message);
                throw;
            }
        }

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl($"<span style='color:red'>{text}</span>"));
        }

        protected void RadButton_Click(object sender, EventArgs e)
        {
            Response.Write("A server click has been executed!");
        }
    }
}