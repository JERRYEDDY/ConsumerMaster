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
using Telerik.Web.UI;

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
                Logger.Info("ConsumerMaster started");

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "consumermaster.database.windows.net";
                builder.UserID = "CSAdmin";
                builder.Password = "MyCSDB2918";
                builder.InitialCatalog = "consumermaster";

                string connection = builder.ConnectionString;

                //ATFServiceExportExcelFile atf = new ATFServiceExportExcelFile();
                //atf.ATFCreateWorkbook();


                BindToTPDropDownList(TPRadDropDownList);
                BindToATFTPDropDownList(ATFTPRadDropDownList);
            }
        }

        private void BindToDataTable()        
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = @"ITLOGIC1\UCPDB";
            csb.InitialCatalog = "ATFIS";
            csb.IntegratedSecurity = true;
            string connString = csb.ToString();
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
                Workbook workbook = serviceExport.AWCCreateWorkbook();
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////
        ///

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

        private void BindToTPDropDownList(RadDropDownList dropdownlist)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                con.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id AS trading_partner_id, name FROM TradingPartners",con))
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

        private void BindToATFTPDropDownList(RadDropDownList dropdownlist)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString()))
            {
                con.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id AS trading_partner_id, name FROM TradingPartners WHERE id IN (1, 2, 3, 4)", con))
                {
                    DataTable tradingPartners = new DataTable();
                    adapter.Fill(tradingPartners);

                    //DataRow dr = tradingPartners.NewRow();
                    //dr["trading_partner_id"] = "0";
                    //dr["name"] = "ALL TRADING PARTNERS";
                    //tradingPartners.Rows.InsertAt(dr, 0);

                    dropdownlist.DataTextField = "name";
                    dropdownlist.DataValueField = "trading_partner_id";
                    dropdownlist.DataSource = tradingPartners;
                    dropdownlist.DataBind();
                }
            }
        }

        protected void EIServiceExportDownload_Click(object sender, EventArgs e)
        {
            const string filename = @"EIServiceExport.xlsx";
            try
            {
                EIServiceExportExcelFile serviceExport = new EIServiceExportExcelFile();
                Workbook workbook = serviceExport.EICreateWorkbook();
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
                string selectedValue = ATFTPRadDropDownList.SelectedValue;

                ATFServiceExportExcelFile serviceExport = new ATFServiceExportExcelFile();
                Workbook workbook = serviceExport.ATFCreateWorkbook(selectedValue);
                DownloadExcelFile(workbook, filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


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

        public int IsInRange(double percentage)
        {
            if (percentage >= 0.00 && percentage <= 25.00)
                return 1;
            else if (percentage >= 25.01 && percentage <= 50.00)
                return 2;
            else if (percentage >= 50.01 && percentage <= 75.00)
                return 3;
            else if (percentage >= 75.01 && percentage <= 100.00)
                return 4;

            return 0;
        }
    }
}