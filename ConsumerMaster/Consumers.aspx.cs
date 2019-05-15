using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
            if (RadGrid1.SelectedIndexes.Count != 0) return;
            RadGrid1.SelectedIndexes.Add(0);
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
            string selectQuery =
                "SELECT c.consumer_internal_number,c.consumer_first,c.consumer_last,c.date_of_birth,c.address_line_1,c.address_line_2,c.city, " +
                "c.state,c.zip_code,c.identifier,c.gender,c.diagnosis, tp1.id AS tpId1, tp1.short_name AS tpName1, tp2.id AS tpId2, tp2.short_name AS tpName2, " + 
                " tp3.id AS tpId3,tp3.short_name AS tpName3, referring_provider_id " + 
                "FROM[ConsumerMaster].[dbo].[Consumers] AS c " +
                "LEFT JOIN TradingPartners AS tp1 ON c.trading_partner_id1 = tp1.id " + 
                "LEFT JOIN TradingPartners AS tp2 ON c.trading_partner_id2 = tp2.id " +
                "LEFT JOIN TradingPartners AS tp3 ON c.trading_partner_id3 = tp3.id " +
                " ORDER BY consumer_last";

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

                string tradingPartner1 = ((RadComboBox)insertedItem.FindControl("cbTradingPartner1")).SelectedValue;
                Int32.TryParse(tradingPartner1, out int tradingPartnerId1);
                string tradingPartner2 = ((RadComboBox)insertedItem.FindControl("cbTradingPartner2")).SelectedValue;
                Int32.TryParse(tradingPartner2, out int tradingPartnerId2);
                string tradingPartner3 = ((RadComboBox)insertedItem.FindControl("cbTradingPartner3")).SelectedValue;
                Int32.TryParse(tradingPartner3, out int tradingPartnerId3);

                string referringProvider = ((RadComboBox)insertedItem.FindControl("cbReferringProvider")).SelectedValue;
                Int32.TryParse(referringProvider, out int referring_provider_id);

                string insertQuery =
                    "INSERT INTO Consumers (consumer_first, consumer_last, date_of_birth, address_line_1, address_line_2, city, state, zip_code, identifier, gender, diagnosis, trading_partner_id1, trading_partner_id2, trading_partner_id3, referring_provider_id)" +
                    " VALUES (@consumer_first, @consumer_last, @date_of_birth, @address_line_1, @address_line_2, @city, @state, @zip_code, @identifier, @gender, @diagnosis, @trading_partner_id1, @trading_partner_id2, @trading_partner_id3, @referring_provider_id)";

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
                        cmd.Parameters.Add("trading_partner_id1", SqlDbType.Int).Value = tradingPartnerId1;
                        cmd.Parameters.Add("trading_partner_id2", SqlDbType.Int).Value = tradingPartnerId2;
                        cmd.Parameters.Add("trading_partner_id3", SqlDbType.Int).Value = tradingPartnerId3;
                        cmd.Parameters.Add("referring_provider_id", SqlDbType.Int).Value = referring_provider_id;

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

            string updateQuery = "UPDATE Consumers SET consumer_first=@consumer_first, consumer_last=@consumer_last, date_of_birth=@date_of_birth, address_line_1=@address_line_1," +
                                 " address_line_2=@address_line_2, city=@city, state=@state, zip_code=@zip_code, identifier=@identifier, gender=@gender, diagnosis=@diagnosis," +
                                 " trading_partner_id1=@trading_partner_id1, trading_partner_id2=@trading_partner_id2, trading_partner_id3=@trading_partner_id3, referring_provider_id=@referring_provider_id" + 
                                 " WHERE consumer_internal_number=@consumer_internal_number";
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

                string tradingPartner1 = ((RadComboBox) editedItem.FindControl("cbTradingPartner1")).SelectedValue;
                Int32.TryParse(tradingPartner1, out int tradingPartnerId1);
                string tradingPartner2 = ((RadComboBox)editedItem.FindControl("cbTradingPartner2")).SelectedValue;
                Int32.TryParse(tradingPartner2, out int tradingPartnerId2);
                string tradingPartner3 = ((RadComboBox)editedItem.FindControl("cbTradingPartner3")).SelectedValue;
                Int32.TryParse(tradingPartner3, out int tradingPartnerId3);

                string referringProvider = ((RadComboBox)editedItem.FindControl("cbReferringProvider")).SelectedValue;
                Int32.TryParse(referringProvider, out int referring_provider_id);


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
                            cmd.Parameters.Add("trading_partner_id1", SqlDbType.Int).Value = tradingPartnerId1;
                            cmd.Parameters.Add("trading_partner_id2", SqlDbType.Int).Value = tradingPartnerId2;
                            cmd.Parameters.Add("trading_partner_id3", SqlDbType.Int).Value = tradingPartnerId3;

                            cmd.Parameters.Add("referring_provider_id", SqlDbType.Int).Value = referring_provider_id;

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