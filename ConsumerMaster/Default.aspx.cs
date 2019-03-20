using System;
using System.Web.UI;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ConsumerMaster
{
    public partial class _Default : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Logger.Info("ConsumerMaster started");

                //ServiceExportFormat sef = new ServiceExportFormat();
                //string[] list = sef.ColumnStrings;

                //ServiceExportFormat sef1 = new ServiceExportFormat(true);
                //string[] list1 = sef1.ColumnStrings;

                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                //{
                //    DataSource = "consumermaster.database.windows.net",
                //    UserID = "CSAdmin",
                //    Password = "MyCSDB2918",
                //    InitialCatalog = "consumermaster"
                //};

                //string connection = builder.ConnectionString;

                RadComboBoxItem item1 = new RadComboBoxItem();
                item1.Text = "Item1";
                item1.Value = "1";
                RadComboBox1.Items.Add(item1);
                RadComboBoxItem item2 = new RadComboBoxItem();
                item2.Text = "Item2";
                item2.Value = "2";
                item2.Checked = true;
                RadComboBox1.Items.Add(item2);
                RadComboBoxItem item3 = new RadComboBoxItem();
                item3.Text = "Item3";
                item3.Value = "3";
                item3.Checked = true;
                RadComboBox1.Items.Add(item3);

                BindToTPDropDownList(TPRadDropDownList);
                BindToATF_TPDropDownList(ATFConsumerList);
                BindToATF_TPDropDownList(ATFServiceList);
            }
        }

        private void BindToDataTable()        
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder
            {
                DataSource = @"ITLOGIC1\UCPDB", InitialCatalog = "ATFIS", IntegratedSecurity = true
            };
            string connString = csb.ToString();
        }

        public void DownloadExcelFile(Workbook workbook, string filename)
        {
            try
            {
                IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
                byte[] renderedBytes;

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
                AWCConsumerExportExcelFile consumerExport = new AWCConsumerExportExcelFile();
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
                AWCServiceExportExcelFile serviceExport = new AWCServiceExportExcelFile();
                Workbook workbook = serviceExport.CreateWorkbook();
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void ConsumerExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ConsumerExport.xlsx";
            try
            {
                string selectedValue = TPRadDropDownList.SelectedValue;

                ConsumerExportExcelFile consumerExport = new ConsumerExportExcelFile();
                Workbook workbook = consumerExport.CreateWorkbook(selectedValue);
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void EIServiceExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"EIServiceExport.xlsx";
            try
            {
                EIServiceExportExcelFile serviceExport = new EIServiceExportExcelFile();
                Workbook workbook = serviceExport.CreateWorkbook();
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void ATFConsumerExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ATFConsumerExport.xlsx";
            try
            {
                string selectedValue = ATFConsumerList.SelectedValue;
                ConsumerExportExcelFile consumerExport = new ConsumerExportExcelFile();
                Workbook workbook = consumerExport.CreateWorkbook(selectedValue);
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void ATFServiceExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"ATFServiceExport.xlsx";
            try
            {
                string selectedValue = ATFServiceList.SelectedValue;
                ATFServiceExportExcelFile serviceExport = new ATFServiceExportExcelFile();
                Workbook workbook = serviceExport.CreateWorkbook(selectedValue);
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void BindToTPDropDownList(RadDropDownList dropdownlist)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id AS trading_partner_id, name FROM TradingPartners", con))
                    {
                        DataTable tradingPartners = new DataTable();
                        adapter.Fill(tradingPartners);

                        DataRow dr = tradingPartners.NewRow();
                        dr["trading_partner_id"] = "0";
                        dr["name"] = "ALL TRADING PARTNERS";
                        tradingPartners.Rows.InsertAt(dr, 0);

                        dropdownlist.DataTextField = "name";
                        dropdownlist.DataValueField = "trading_partner_id";
                        dropdownlist.DataSource = tradingPartners;
                        dropdownlist.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void BindToATF_TPDropDownList(RadDropDownList dropdownlist)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
                {
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id AS trading_partner_id, name FROM TradingPartners WHERE id IN (1, 2, 3, 4)", con))
                    {
                        DataTable tradingPartners = new DataTable();
                        adapter.Fill(tradingPartners);

                        dropdownlist.DataTextField = "name";
                        dropdownlist.DataValueField = "trading_partner_id";
                        dropdownlist.DataSource = tradingPartners;
                        dropdownlist.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static void ShowCheckedItems(RadComboBox comboBox, Literal literal)
        {
            var sb = new StringBuilder();
            var collection = comboBox.CheckedItems;

            if (collection.Count != 0)
            {
                sb.Append("<h3>Checked Items:</h3><ul class=\"results\">");

                foreach (var item in collection)
                    sb.Append("<li>" + item.Text + "</li>");

                sb.Append("</ul>");

                literal.Text = sb.ToString();
            }
            else
            {
                literal.Text = "<p>No items selected</p>";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ShowCheckedItems(RadComboBox1, itemsClientSide);
        }
    }
}