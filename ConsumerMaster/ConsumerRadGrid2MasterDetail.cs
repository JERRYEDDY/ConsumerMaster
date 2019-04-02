OnNeedDataSource="RadGrid2_NeedDataSource" OnInsertCommand="RadGrid2_InsertCommand" OnUpdateCommand="RadGrid2_UpdateCommand" 
OnDeleteCommand="RadGrid2_DeleteCommand" OnItemCommand="RadGrid2_ItemCommand" 


protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
{
    if (e.CommandName == "RowClick")
    {
        RadGrid2.Rebind();
    }
}

protected void RadGrid2_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
{
    string consumerId = null;

    if (RadGrid1.SelectedValue != null)
    {
        consumerId = RadGrid1.SelectedValue.ToString();
    }
    else
    {
        consumerId = RadGrid1.MasterTableView.Items[0].GetDataKeyValue("consumer_internal_number").ToString();
    }

    Int32.TryParse(consumerId, out int consumerInternalNumber);

    string selectQuery = "SELECT ctp.consumer_internal_number, ctp.trading_partner_id, tp.name AS trading_partner_name FROM ConsumerTradingPartner ctp " +
                         "INNER JOIN TradingPartners tp ON ctp.trading_partner_id = tp.id WHERE consumer_internal_number = @consumer_internal_number";

    SqlParameter sqlParam = new SqlParameter();
    sqlParam.ParameterName = "@consumer_internal_number";
    sqlParam.Value = consumerInternalNumber;
    sqlParam.SqlDbType = SqlDbType.Int;

    DataTable dataTable = GetDataTable(selectQuery, sqlParam);
    RadGrid2.DataSource = dataTable;
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
        RadTextBox TextBox1 = (RadTextBox)item.FindControl("TextBox1");
        // Access your RadTextBox Here


        // For Edit mode
        GridEditableItem eitem = e.Item as GridEditableItem;
        RadTextBox TextBox2 = (RadTextBox)eitem.FindControl("TextBox2"); 
        RadTextBox TextBox3 = (RadTextBox)eitem["ColumnUniqueName"].Controls[0]; 
    }
}