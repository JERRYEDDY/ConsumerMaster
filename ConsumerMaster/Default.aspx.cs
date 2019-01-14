using System;
using System.Web.UI;
using System.IO;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace ConsumerMaster
{
    public partial class _Default : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private const string ConsumersTable = "Consumers";
        private const string TradingPartnersTable = "TradingPartners";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //this.BindGrid();
                Logger.Info("ConsumerMaster started");


                BindToDataTable(RadTreeView1);


            }
        }

        private void BindToDataTable(RadTreeView treeView)        
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = @".\SQL2014";
            csb.InitialCatalog = "ConsumerMaster";
            csb.IntegratedSecurity = true;
            string connString = csb.ToString();

            string queryString =
                "SELECT NULL AS ParentID, tp.name AS Name, ctp.trading_partner_id AS ID " +
                "FROM[ConsumerMaster].[dbo].[ConsumerTradingPartner] ctp INNER JOIN TradingPartners tp ON ctp.trading_partner_id = tp.id " +
                "WHERE consumer_internal_number = 237" +
                "UNION ALL " +
                "SELECT ctc.trading_partner_id AS ParentID, cpc.name AS Name, ctc.cpc_id AS ID " +
                "FROM [ConsumerMaster].[dbo].[ConsumerTradingComposite] ctc INNER JOIN CompositeProcedureCodes cpc ON ctc.cpc_id = cpc.id " +
                "WHERE consumer_internal_number = 237";   
            
            SqlConnection connection = new SqlConnection(connString);
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            treeView.DataTextField = "Name";
            //treeView.DataValueField = "Description";
            treeView.DataFieldID = "ID";
            treeView.DataFieldParentID = "ParentID";
            treeView.DataSource = dataTable;
            treeView.DataBind();
        }

        public void BuildTreeView()
        {








        }





        public void DownloadExcelFile(Workbook workbook, string filename)
        {
            try
            {
                IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
                byte[] renderedBytes = null;

                using (MemoryStream ms = new MemoryStream())
                {
                    formatProvider.Export(workbook, ms);
                    renderedBytes = ms.ToArray();
                }

                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.BinaryWrite(renderedBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void AWCConsumerExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"AWCConsumerExport.xlsx";
            try
            {
                ConsumerExportExcelFile consumerExport = new ConsumerExportExcelFile();
                Workbook workbook = consumerExport.CreateWorkbook();
                DownloadExcelFile(workbook,filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void AWCServiceExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"AWCServiceExport.xlsx";
            try
            {
                ServiceExportExcelFile serviceExport = new ServiceExportExcelFile();
                Workbook workbook = serviceExport.AWCCreateWorkbook();
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void AddTradingPartnerNode_Click(object sender, EventArgs e)
        {
            RadButton btn = sender as RadButton;
            //RadDropDownList ddlTradingPartner = btn.Parent.FindControl("RadDropDownList1") as RadDropDownList;
            DropDownList ddlTradingPartner = btn.Parent.FindControl("DropDownList1") as DropDownList;
            try
            {
                if (RadTreeView1.SelectedNode != null) //Selected Node
                {
                    if (RadTreeView1.SelectedNode.ParentNode == null) //Trading Partner has no parent
                    {
                        if(ddlTradingPartner.SelectedItem != null && !String.IsNullOrEmpty(ddlTradingPartner.SelectedItem.Text))
                        { 
                            RadTreeNode addedTradingPartnerNode = new RadTreeNode();
                            addedTradingPartnerNode.Selected = true;
                            addedTradingPartnerNode.Text = ddlTradingPartner.SelectedItem.Text;
                            addedTradingPartnerNode.Value = ddlTradingPartner.SelectedValue;
                            RadTreeView1.Nodes.Add(addedTradingPartnerNode);
                        }
                    }
                        Logger.Info(RadTreeView1.SelectedNode.Text + " Level: " + RadTreeView1.SelectedNode.Level);
                }
                else if (RadTreeView1.Nodes.Count == 0) //Treeview has no nodes
                {
                    if (ddlTradingPartner.SelectedItem != null && !String.IsNullOrEmpty(ddlTradingPartner.SelectedItem.Text))
                    {
                        RadTreeNode addedTradingPartnerNode = new RadTreeNode();
                        addedTradingPartnerNode.Selected = true;
                        addedTradingPartnerNode.Text = ddlTradingPartner.SelectedItem.Text;
                        addedTradingPartnerNode.Value = ddlTradingPartner.SelectedValue;
                        RadTreeView1.Nodes.Add(addedTradingPartnerNode);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void AddCompositeProcedureCodeNode_Click(object sender, EventArgs e)
        {
            RadButton btn = sender as RadButton;
            RadDropDownList ddlTradingPartner = btn.Parent.FindControl("RadDropDownList1") as RadDropDownList;
            RadDropDownList ddlCompositeProcedureCode = btn.Parent.FindControl("RadDropDownList2") as RadDropDownList;
            try
            {
                if (RadTreeView1.SelectedNode != null) //Selected node
                {
                    RadTreeView1.SelectedNode.Expanded = true;
                    if (RadTreeView1.SelectedNode.ParentNode == null) //Trading Partner
                    {
                        if (ddlCompositeProcedureCode.SelectedItem != null && !String.IsNullOrEmpty(ddlCompositeProcedureCode.SelectedItem.Text))
                        {
                            RadTreeNode addedCompositeProcedureCode = new RadTreeNode();
                            addedCompositeProcedureCode.Selected = true;
                            addedCompositeProcedureCode.Text = ddlCompositeProcedureCode.SelectedText;
                            addedCompositeProcedureCode.Value = ddlCompositeProcedureCode.SelectedValue;
                            RadTreeView1.SelectedNode.Nodes.Add(addedCompositeProcedureCode);
                        }

                    }
                    Logger.Info(RadTreeView1.SelectedNode.Text + " Level: " + RadTreeView1.SelectedNode.Level);
                }
                else if (RadTreeView1.Nodes.Count == 0) // Treeview has no nodes
                {
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void DeleteSelectedNode_Click(object sender, EventArgs e)
        {
            RadButton btn = sender as RadButton;
            try
            {
                RadTreeView1.GetXml();

                foreach (RadTreeNode node in RadTreeView1.GetAllNodes())
                {
                    String foo = node.Text;
                }

                if (RadTreeView1.SelectedNode != null) //Selected node
                {
                    RadTreeView1.SelectedNode.Remove();
                    Logger.Info(RadTreeView1.SelectedNode.Text + " Level: " + RadTreeView1.SelectedNode.Level);
                }
                else if (RadTreeView1.Nodes.Count == 0) // Treeview has no nodes
                {
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////


        internal class ConsumerDataItem
        {
            private string text;
            private int id;
            private int parentId;

            public string Text
            {
                get { return text; }
                set { text = value; }
            }


            public int ID
            {
                get { return id; }
                set { id = value; }
            }

            public int ParentID
            {
                get { return parentId; }
                set { parentId = value; }
            }

            public ConsumerDataItem(int id, int parentId, string text)
            {
                this.id = id;
                this.parentId = parentId;
                this.text = text;
            }
        }

        public void old()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = @".\SQL2014";
            csb.InitialCatalog = "ConsumerMaster";
            csb.IntegratedSecurity = true;

            string connString = csb.ToString();

            string queryString = "select * FROM ConsumerTemplate";

            string consumerTemplate;

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                //command.Parameters.Add(new SqlParameter("tabId", tabId));
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int Id = Int32.Parse(reader["Id"].ToString());
                        RadTreeView1.LoadXmlString(reader["Template"].ToString());
                    }
                }
            }
        }
    }
}