using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using Telerik.Web.UI;
using System.Collections.Generic;

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

        protected void RadGrid1_PreRender(object sender, System.EventArgs e)
        {
            if (!this.IsPostBack && this.RadGrid1.MasterTableView.Items.Count > 1)
            {
                //RadGrid1.MasterTableView.GetColumn("address_line_1").HeaderStyle.Width = Unit.Pixel(350);
                //RadGrid1.MasterTableView.GetColumn("address_line_1").FilterControlWidth = Unit.Pixel(350);
                //RadGrid1.MasterTableView.GetColumn("address_line_1").ItemStyle.Width = Unit.Pixel(350);
            }
        }

        private static DataTable GetDataTable(string queryString)
        {
            String ConnString = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
            SqlConnection MySqlConnection = new SqlConnection(ConnString);
            SqlDataAdapter MySqlDataAdapter = new SqlDataAdapter();
            MySqlDataAdapter.SelectCommand = new SqlCommand(queryString, MySqlConnection);

            DataTable myDataTable = new DataTable();
            MySqlConnection.Open();
            try
            {
                MySqlDataAdapter.Fill(myDataTable);
            }
            finally
            {
                MySqlConnection.Close();
            }

            return myDataTable;
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
                DataTable myDataTable = new DataTable();
                myDataTable = GetDataTable("SELECT * FROM Consumers");
                this.Session["Consumers"] = myDataTable;
                return myDataTable;
            }
        }

        protected void rblGender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = this.Consumers;
            this.Consumers.PrimaryKey = new DataColumn[] { this.Consumers.Columns["consumer_internal_number"] };
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
          
            //Prepare new row to add it in the DataSource
            DataRow[] changedRows = this.Consumers.Select("ConsumerID = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["consumer_internal_number"]);

            if (changedRows.Length != 1)
            {
                RadGrid1.Controls.Add(new LiteralControl("Unable to locate the Consumer for updating."));
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
                RadGrid1.Controls.Add(lblError);

                e.Canceled = true;
            }
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
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
                RadGrid1.Controls.Add(lblError);

                e.Canceled = true;
            }
        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            string ID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["consumer_internal_number"].ToString();
            DataTable employeeTable = this.Consumers;
            if (employeeTable.Rows.Find(ID) != null)
            {
                employeeTable.Rows.Find(ID).Delete();
                employeeTable.AcceptChanges();
            }
        }

    }
}