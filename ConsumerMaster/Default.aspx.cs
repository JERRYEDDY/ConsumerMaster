using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //this.BindGrid();
            }
        }

        protected void ConsumersGrid_ItemUpdated(object source, GridUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
                DisplayMessage(true, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " cannot be updated. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(false, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " updated");
            }
        }

        protected void ConsumersGrid_ItemInserted(object source, GridInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
                DisplayMessage(true, "Consumer cannot be inserted. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(false, "Consumer inserted");
            }
        }

        protected void ConsumersGrid_ItemDeleted(object source, GridDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(true, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " cannot be deleted. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(false, "Consumer " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"] + " deleted");
            }
        }

        protected void ConsumersGrid_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.InitInsertCommandName) //"Add new" button clicked
            {
                GridEditCommandColumn editColumn = (GridEditCommandColumn)ConsumersGrid.MasterTableView.GetColumn("EditCommandColumn");
                editColumn.Visible = false;
            }
            else if (e.CommandName == RadGrid.RebindGridCommandName && e.Item.OwnerTableView.IsItemInserted)
            {
                e.Canceled = true;
            }
            else
            {
                GridEditCommandColumn editColumn = (GridEditCommandColumn)ConsumersGrid.MasterTableView.GetColumn("EditCommandColumn");
                if (!editColumn.Visible)
                    editColumn.Visible = true;
            }
        }

        protected void ConsumersGrid_PreRender(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ConsumersGrid.EditIndexes.Add(0);
                ConsumersGrid.Rebind();
            }
        }

        private void DisplayMessage(bool isError, string text)
        {
            //Label label = (isError) ? this.Label1 : this.Label2;
            //label.Text = text;
        }






























        private static DataTable GetDataTable(string queryString)
        {
            using (SqlConnection sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString))
            {
                sqlConnect.Open();
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnect))
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        private DataTable Consumers
        {
            get
            {
                object obj = this.Session["Consumers"];
                if ((!(obj == null)))
                {
                    return ((DataTable)(obj));
                }
                DataTable dataTable = new DataTable();
                dataTable = GetDataTable("SELECT * FROM Consumers");
                this.Session["Consumers"] = dataTable;
                return dataTable;
            }
        }

        protected void ConsumerGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            this.ConsumersGrid.DataSource = this.Consumers;
            this.Consumers.PrimaryKey = new DataColumn[] { this.Consumers.Columns["consumer_internal_number"] };
        }

        protected void ConsumerGrid_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
          
            //Prepare new row to add it in the DataSource
            DataRow[] changedRows = this.Consumers.Select("ConsumerID = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["consumer_internal_number"]);

            if (changedRows.Length != 1)
            {
                ConsumersGrid.Controls.Add(new LiteralControl("Unable to locate the Consumer for updating."));
                e.Canceled = true;
                return;
            }

            //Update new values
            Hashtable newValues = new Hashtable();

            newValues["consumer_first"] = (userControl.FindControl("txtConsumerFirst") as TextBox).Text;
            newValues["consumer_last"] = (userControl.FindControl("txtConsumerLast") as TextBox).Text;
            newValues["date_of_birth"] = (userControl.FindControl("dpBirthDate") as RadDatePicker).SelectedDate.ToString();
            newValues["address_line_1"] = (userControl.FindControl("txtAddress1") as TextBox).Text;
            newValues["address_line_2"] = (userControl.FindControl("txtAddress2") as RadMaskedTextBox).Text;

            newValues["city"] = (userControl.FindControl("txtCity") as TextBox).Text;
            newValues["state"] = (userControl.FindControl("txtState") as TextBox).Text;
            newValues["zip_code"] = (userControl.FindControl("txtZipCode") as TextBox).Text;
            newValues["identifier"] = (userControl.FindControl("txtIdentifier") as TextBox).Text;
            newValues["gender"] = (userControl.FindControl("ddlGender") as DropDownList).SelectedItem.Value;
            //newValues["gender"] = (userControl.FindControl("txtGender") as RadMaskedTextBox).Text;

            newValues["diagnosis"] = (userControl.FindControl("txtDiagnosis") as TextBox).Text;
            newValues["nickname_first"] = (userControl.FindControl("txtNicknameFirst") as TextBox).Text;
            newValues["nickname_last"] = (userControl.FindControl("txtNicknameLast") as TextBox).Text;

            changedRows[0].BeginEdit();
            try
            {
                foreach (DictionaryEntry entry in newValues)
                {
                    changedRows[0][(string)entry.Key] = entry.Value;
                }
                changedRows[0].EndEdit();
                this.Consumers.AcceptChanges();
            }
            catch (Exception ex)
            {
                changedRows[0].CancelEdit();

                Label lblError = new Label();
                lblError.Text = "Unable to update Consumers. Reason: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
                ConsumersGrid.Controls.Add(lblError);

                e.Canceled = true;
            }
        }

        protected void ConsumerGrid_InsertCommand(object source, GridCommandEventArgs e)
        {
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            //Create new row in the DataSource
            DataRow newRow = this.Consumers.NewRow();

            //Insert new values
            Hashtable newValues = new Hashtable();

            newValues["consumer_first"] = (userControl.FindControl("txtConsumerFirst") as TextBox).Text;
            newValues["consumer_last"] = (userControl.FindControl("txtConsumerLast") as TextBox).Text;
            newValues["date_of_birth"] = (userControl.FindControl("dpBirthDate") as RadDatePicker).SelectedDate.ToString();
            newValues["address_line_1"] = (userControl.FindControl("txtAddress1") as TextBox).Text;
            newValues["address_line_2"] = (userControl.FindControl("txtAddress2") as RadMaskedTextBox).Text;

            newValues["city"] = (userControl.FindControl("txtCity") as TextBox).Text;
            newValues["state"] = (userControl.FindControl("txtState") as TextBox).Text;
            newValues["zip_code"] = (userControl.FindControl("txtZipCode") as TextBox).Text;
            newValues["identifier"] = (userControl.FindControl("txtIdentifier") as TextBox).Text;
            newValues["gender"] = (userControl.FindControl("ddlGender") as DropDownList).SelectedItem.Value;
            //newValues["gender"] = (userControl.FindControl("txtGender") as RadMaskedTextBox).Text;

            newValues["diagnosis"] = (userControl.FindControl("txtDiagnosis") as TextBox).Text;
            newValues["nickname_first"] = (userControl.FindControl("txtNicknameFirst") as TextBox).Text;
            newValues["nickname_last"] = (userControl.FindControl("txtNicknameLast") as TextBox).Text;

            //make sure that unique primary key value is generated for the inserted row 
            newValues["consumer_internal_number"] = (int)this.Consumers.Rows[this.Consumers.Rows.Count - 1]["consumer_internal_number"] + 1;
            try
            {
                foreach (DictionaryEntry entry in newValues)
                {
                    newRow[(string)entry.Key] = entry.Value;
                }
                this.Consumers.Rows.Add(newRow);
                this.Consumers.AcceptChanges();
            }
            catch (Exception ex)
            {
                Label lblError = new Label();
                lblError.Text = "Unable to insert Consumers. Reason: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
                ConsumersGrid.Controls.Add(lblError);

                e.Canceled = true;
            }
        }

        protected void ConsumerGrid_DeleteCommand(object source, GridCommandEventArgs e)
        {
            string ID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"].ToString();
            DataTable employeeTable = this.Consumers;
            if (employeeTable.Rows.Find(ID) != null)
            {
                employeeTable.Rows.Find(ID).Delete();
                employeeTable.AcceptChanges();
            }
        }

        private DataTable ConsumersTradingPartners
        {
            get
            {
                object obj = this.Session["ConsumersTradingPartners"];
                if ((!(obj == null)))
                {
                    return ((DataTable)(obj));
                }
                DataTable dataTable = new DataTable();
                dataTable = GetDataTable("SELECT tp.id AS id, tp.name AS name, tp.string AS string FROM ConsumerTradingPartner AS ctp INNER JOIN TradingPartners AS tp ON ctp.trading_partner_id = tp.id");
                this.Session["ConsumersTradingPartners"] = dataTable;
                return dataTable;
            }
        }


    }
}