using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class ConsumersNew : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private const string ConsumersTable = "Consumers";
        private const string TradingPartnersTable = "TradingPartners";
        private const string CompositeProcedureCodesTable = "CompositeProcedureCodes";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //this.BindGrid();
                Logger.Info("ConsumerMaster started");
            }
        }

        public void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            if (!e.IsFromDetailTable)
            {
                RadGrid1.DataSource = GetDataTable("SELECT * FROM Consumers");
            }
        }

        public void RadGrid1_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "Partners":
                {
                    string ConsumerID = dataItem.GetDataKeyValue("consumer_internal_number").ToString();
                    e.DetailTableView.DataSource = GetDataTable("SELECT * FROM Partners p INNER JOIN TradingPartners tp ON p.trading_partner_id = tp.id WHERE consumer_internal_number = @consumer_internal_number", "consumer_internal_number", ConsumerID);
                    break;
                }

                case "PartnerDetails":
                {
                    string PartnerID = dataItem.GetDataKeyValue("partner_id").ToString();
                    e.DetailTableView.DataSource = GetDataTable("SELECT * FROM [PartnerDetails] WHERE partner_id = @partner_id", "partner_id", PartnerID);
                    break;
                }
            }
        }

        public DataTable GetDataTable(string query, string selectParameter = "", string parameterValue = "")
        {
            String ConnString = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
            SqlConnection conn = new SqlConnection(ConnString);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(query, conn);

            if (!string.IsNullOrEmpty(selectParameter))
            {
                command.Parameters.Add(new SqlParameter(selectParameter, parameterValue));
            }

            adapter.SelectCommand = command;

            DataTable myDataTable = new DataTable();

            conn.Open();
            try
            {
                adapter.Fill(myDataTable);
            }
            finally
            {
                conn.Close();
            }

            return myDataTable;
        }

        public void RadGrid1_PreRender(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RadGrid1.MasterTableView.Items[0].Expanded = true;
                RadGrid1.MasterTableView.Items[0].ChildItem.NestedTableViews[0].Items[0].Expanded = true;
            }
        }

        protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
        {
            string item = getItemName(e.Item.OwnerTableView.Name);
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(item + " cannot be inserted. Reason: " + e.Exception.Message);
                Logger.Info(item + " cannot be inserted. Reason: " + e.Exception.Message);
                Logger.Error(e);
            }
            else
            {
                DisplayMessage(item + " inserted");
                Logger.Info(item + " inserted");
            }
        }

        protected void RadGrid1_ItemUpdated(object source, GridUpdatedEventArgs e)
        {
            string item = getItemName(e.Item.OwnerTableView.Name);
            string field = getFieldName(e.Item.OwnerTableView.Name);
            if (e.Exception != null)
            {
                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
                DisplayMessage(item + " " + e.Item[field].Text + " cannot be updated. Reason: " + e.Exception.Message);
                Logger.Info(item + " " + e.Item[field].Text + " cannot be updated. Reason: " + e.Exception.Message);
                Logger.Error(e);
            }
            else
            {
                DisplayMessage(item + " " + e.Item[field].Text + " updated");
                Logger.Info(item + " " + e.Item[field].Text + " updated");
            }
        }

        protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
        {
            string item = getItemName(e.Item.OwnerTableView.Name);
            string field = getFieldName(e.Item.OwnerTableView.Name);
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(item + " " + e.Item[field].Text + " cannot be deleted. Reason: " + e.Exception.Message);
                Logger.Info(item + " " + e.Item[field].Text + " cannot be deleted. Reason: " + e.Exception.Message);
                Logger.Error(e);
            }
            else
            {
                DisplayMessage(item + " " + e.Item[field].Text + " deleted");
                Logger.Info(item + " " + e.Item[field].Text + " deleted");
            }
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case ConsumersTable:
                    {
                        //GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                        //SqlDataSource1.InsertParameters["consumer_internal_number"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["consumer_internal_number"].ToString();
                    }
                    break;
                case TradingPartnersTable:
                    {
                        GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                        SqlDataSource2.InsertParameters["consumer_internal_number"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["consumer_internal_number"].ToString();
                    }
                    break;
            }
        }

        private String getItemName(string tableName)
        {
            switch (tableName)
            {
                case (ConsumersTable):
                    {
                        return "Consumer";
                    }
                case (TradingPartnersTable):
                    {
                        return "TradingPartner";
                    }
                case (CompositeProcedureCodesTable):
                    {
                        return "CompositeProcedureCodes";
                    }

                default: return "";
            }
        }

        private String getFieldName(string tableName)
        {
            switch (tableName)
            {
                case (ConsumersTable):
                    {
                        return "consumer_internal_number";
                    }
                case (TradingPartnersTable):
                    {
                        return "consumer_internal_number";
                    }
                default: return "";
            }
        }

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl(string.Format("<span style='color:red'>{0}</span>", text)));
        }
    }
}