﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class Consumers3 : Page
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




/// <summary>
/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>
/// <param name="source"></param>
/// <param name="e"></param>

        protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            //Populate the Radgrid1
            DataTable dtTable = new DataTable();
            string selectQuery = "SELECT consumer_internal_number, consumer_first, consumer_last, date_of_birth, address_line_1, address_line_2, " +
                                "city, state, zip_code, identifier, gender, diagnosis, nickname_first, nickname_last FROM Consumers ORDER BY consumer_last";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = selectQuery;

                        SqlDataAdapter adapter = new SqlDataAdapter
                        {
                            SelectCommand = cmd
                        };
                        adapter.Fill(dtTable);
                        RadGrid1.DataSource = dtTable;
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("Cannot select Consumers. Reason: " + ex.Message);
                        Logger.Info("Cannot select Consumers. Reason: " + ex.Message);
                    }
                }
            }
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditFormInsertItem of the RadGrid 
            GridEditFormInsertItem insertedItem = (GridEditFormInsertItem)e.Item;

            //Access the textbox from the insert form template and store the values in string variables. 
            string consumerFirst = ((TextBox)insertedItem.FindControl("consumer_first")).Text;
            string consumerLast = ((TextBox)insertedItem.FindControl("consumer_last"))?.Text;
            RadDatePicker dateOfBirth = insertedItem.FindControl("date_of_birth") as RadDatePicker;
            string addressLine1 = ((TextBox)insertedItem.FindControl("address_line_1")).Text;
            string addressLine2 = ((TextBox)insertedItem.FindControl("address_line_2")).Text;
            string city = ((TextBox)insertedItem.FindControl("city")).Text;
            string state = ((TextBox)insertedItem.FindControl("state")).Text;
            string zipCode = ((TextBox)insertedItem.FindControl("zip_code")).Text;
            string identifier = ((TextBox)insertedItem.FindControl("identifier")).Text;
            string gender = ((TextBox)insertedItem.FindControl("gender")).Text;
            string diagnosis = ((TextBox)insertedItem.FindControl("diagnosis")).Text;
            string nicknameFirst = ((TextBox)insertedItem.FindControl("nickname_first")).Text;
            string nicknameLast = ((TextBox)insertedItem.FindControl("nickname_last")).Text;

            string insertQuery =
                "INSERT INTO Consumers (consumer_first, consumer_last, date_of_birth, address_line_1, address_line_2, city, state, zip_code, identifier, gender, diagnosis, " +
                "nickname_first, nickname_last)" +
                " VALUES (@consumer_first, @consumer_last, @date_of_birth, @address_line_1, @address_line_2, @city, @state, @zip_code, @identifier, @gender, @diagnosis, " +
                "@nickname_first, @nickname_last)";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
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
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var message = $"Consumer: {consumerFirst}  {consumerLast} cannot be inserted. Reason: ";
                        DisplayMessage(message + ex.Message);
                        Logger.Info(message + ex.Message);
                        e.Canceled = true;
                    }
                }
            }
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditableItem of the RadGrid 
            GridEditableItem editedItem = e.Item as GridEditableItem;

            //Get the primary key value using the DataKeyValue. 
            string consumerId = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["consumer_internal_number"].ToString();
            Int32.TryParse(consumerId, out int consumer_internal_number);

            //Access the controls from the edit form template and store the values. 
            string consumerFirst = ((TextBox)editedItem.FindControl("consumer_first")).Text;
            string consumerLast = ((TextBox)editedItem.FindControl("consumer_last"))?.Text;
            RadDatePicker dateOfBirth = (RadDatePicker)editedItem.FindControl("date_of_birth");
            string addressLine1 = ((TextBox)editedItem.FindControl("address_line_1")).Text;
            string addressLine2 = ((TextBox)editedItem.FindControl("address_line_2")).Text;
            string city = ((TextBox)editedItem.FindControl("city")).Text;
            string state = ((TextBox)editedItem.FindControl("state")).Text;
            string zipCode = ((TextBox)editedItem.FindControl("zip_code")).Text;
            string identifier = ((TextBox)editedItem.FindControl("identifier")).Text;
            string gender = ((TextBox)editedItem.FindControl("gender")).Text;
            string diagnosis = ((TextBox)editedItem.FindControl("diagnosis")).Text;
            string nicknameFirst = ((TextBox)editedItem.FindControl("nickname_first")).Text;
            string nicknameLast = ((TextBox)editedItem.FindControl("nickname_last")).Text;

            string updateQuery = "UPDATE ConsumersEI SET consumer_first=@consumer_first, consumer_last=@consumer_last, date_of_birth=@date_of_birth, address_line_1=@address_line_1, address_line_2=@address_line_2, " +
                   "city=@city, state=@state, zip_code=@zip_code, identifier=@identifier, gender=@gender, diagnosis=@diagnosis, nickname_first=@nickname_first, nickname_last=@nickname_last " +
                   " WHERE consumer_internal_number=@consumer_internal_number";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
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
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var message = $"Consumer: {consumerId} cannot be updated. Reason: ";
                        DisplayMessage(message + ex.Message);
                        Logger.Info(message + ex.Message);
                        e.Canceled = true;
                    }

                }
            }
        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridDataItem of the RadGrid 
            GridDataItem item = (GridDataItem)e.Item;

            //Get the primary key value using the DataKeyValue. 
            string consumerId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["consumer_internal_number"].ToString();
            Int32.TryParse(consumerId, out int consumerInternalNumber);

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

                        cmd.Parameters.Add("consumerInternalNumber", SqlDbType.Int).Value = consumerInternalNumber;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var message = $"Consumer: {consumerId} cannot be deleted. Reason: ";
                        DisplayMessage(message + ex.Message);
                        Logger.Info(message + ex.Message);
                        e.Canceled = true;
                    }
                }
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "YourCommandName")
            {
                // For Normal mode
                GridDataItem item = e.Item as GridDataItem;
                TextBox textBox1 = item.FindControl("TextBox1") as TextBox;
                // Access your TextBox Here


                // For Edit mode
                GridEditableItem eitem = e.Item as GridEditableItem;
                TextBox textBox2 = eitem.FindControl("TextBox2") as TextBox; // From Item Template
                TextBox textBox3 = eitem["ColumnUniqueName"].Controls[0] as TextBox; // From Bound Column
                // Access your TextBox Here
            }
        }

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl($"<span style='color:red'>{text}</span>"));
        }
        
        protected void RadGrid2_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            //Populate the Radgrid2 
            DataTable dtTable = new DataTable();
            string selectQuery = "SELECT ctp.consumer_internal_number, ctp.trading_partner_id, tp.name AS trading_partner_name FROM ConsumerTradingPartner ctp " +
                                 "INNER JOIN TradingPartners tp ON ctp.trading_partner_id = tp.id WHERE consumer_internal_number = @consumer_internal_number";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = selectQuery;

                        SqlDataAdapter adapter = new SqlDataAdapter
                        {
                            SelectCommand = cmd
                        };
                        adapter.Fill(dtTable);
                        RadGrid1.DataSource = dtTable;
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("Cannot select Consumer-Trading Partner. Reason: " + ex.Message);
                        Logger.Info("Cannot select Consumer-Trading Partner. Reason: " + ex.Message);
                    }
                }
            }
        }

        protected void RadGrid2_InsertCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditFormInsertItem of the RadGrid 
            GridEditFormInsertItem insertedItem = (GridEditFormInsertItem)e.Item;

            //Get the primary key value using the DataKeyValue. 
            string consumerId = insertedItem.OwnerTableView.DataKeyValues[insertedItem.ItemIndex]["consumer_internal_number"].ToString();
            Int32.TryParse(consumerId, out int consumerInternalNumber);

            string tradingPartnerId = ((DropDownList)insertedItem.FindControl("trading_partner_id")).SelectedValue;

            string insertQuery = "INSERT INTO ConsumerTradingPartner (consumer_internal_number, trading_partner_id) VALUES (@consumer_internal_number, @trading_partner_id)";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = insertQuery;

                        cmd.Parameters.Add("consumer_internal_number", SqlDbType.Int).Value = consumerInternalNumber;
                        cmd.Parameters.Add("trading_partner_id", SqlDbType.Int).Value = tradingPartnerId;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var message = $"Consumer-Trading Partner: {consumerId} {tradingPartnerId} cannot be inserted. Reason: ";
                        DisplayMessage(message + ex.Message);
                        Logger.Info(message + ex.Message);
                        e.Canceled = true;
                    }
                }
            }
        }

        protected void RadGrid2_UpdateCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridEditableItem of the RadGrid 
            GridEditableItem editedItem = e.Item as GridEditableItem;

            //Get the primary key value using the DataKeyValue. 
            string consumerId = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["consumer_internal_number"].ToString();
            Int32.TryParse(consumerId, out int consumerInternalNumber);

            //Access the controls from the edit form template and store the values. 
            string tradingPartnerId = ((DropDownList)editedItem.FindControl("trading_partner_id")).SelectedValue;

            string updateQuery = "UPDATE ConsumerTradingPartner SET trading_partner_id = @trading_partner_id WHERE consumer_internal_number = @consumer_internal_number";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = updateQuery;
                        cmd.Parameters.Add("consumer_internal_number", SqlDbType.Int).Value = consumerInternalNumber;
                        cmd.Parameters.Add("trading_partner_id1", SqlDbType.Int).Value = tradingPartnerId;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var message = $"Consumer-Trading Partner: {consumerId} {tradingPartnerId} cannot be updated. Reason: ";
                        DisplayMessage(message + ex.Message);
                        Logger.Info(message + ex.Message);
                        e.Canceled = true;
                    }

                }
            }
        }

        protected void RadGrid2_DeleteCommand(object source, GridCommandEventArgs e)
        {
            //Get the GridDataItem of the RadGrid 
            GridDataItem item = (GridDataItem)e.Item;

            //Get the primary key value using the DataKeyValue. 
            string consumerId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["consumer_internal_number"].ToString();
            Int32.TryParse(consumerId, out int consumerInternalNumber);

            string deleteQuery = "DELETE FROM ConsumerTradingPartner WHERE consumer_internal_number = @consumer_internal_number";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = deleteQuery;

                        cmd.Parameters.Add("consumerInternalNumber", SqlDbType.Int).Value = consumerInternalNumber;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var message = $"Consumer-Trading Partner: {consumerId} cannot be deleted. Reason: ";
                        DisplayMessage(message + ex.Message);
                        Logger.Info(message + ex.Message);
                        e.Canceled = true;
                    }
                }
            }
        }

        protected void RadGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "YourCommandName")
            {
                // For Normal mode
                GridDataItem item = e.Item as GridDataItem;
                TextBox TextBox1 = item.FindControl("TextBox1") as TextBox;
                // Access your TextBox Here


                // For Edit mode
                GridEditableItem eitem = e.Item as GridEditableItem;
                TextBox TextBox2 = eitem.FindControl("TextBox2") as TextBox; // From Item Template
                TextBox TextBox3 = eitem["ColumnUniqueName"].Controls[0] as TextBox; // From Bound Column
                // Access your TextBox Here
            }
        }
    }
}