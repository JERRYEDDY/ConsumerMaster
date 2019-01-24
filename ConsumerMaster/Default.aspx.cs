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
using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

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

                string inFileName = @"C:\Billing Software\EI\GREENE CTY DEC 2018 -FINAL.tsv";
                var inEngine = new FileHelperEngine<EIBillingInput>();
                var inResult = inEngine.ReadFile(inFileName);

                var outEngine = new FileHelperEngine<EIServiceExportFormat>();
                outEngine.HeaderText = outEngine.GetFileHeader();
                var billingTrans = new List<EIServiceExportFormat>();

                string queryString = "SELECT * FROM Consumers AS c ";
                Utility util = new Utility();

                DataTable consumersDataTable = util.GetDataTable(queryString);
                DataTable billingDataTable = new DataTable();

                billingDataTable.Columns.Add("Therapists", typeof(string));
                billingDataTable.Columns.Add("BillDate", typeof(string));
                billingDataTable.Columns.Add("LastName", typeof(string));
                billingDataTable.Columns.Add("FirstName", typeof(string));
                billingDataTable.Columns.Add("County", typeof(string));
                billingDataTable.Columns.Add("FundingSource", typeof(string));
                billingDataTable.Columns.Add("VisitType", typeof(string));
                billingDataTable.Columns.Add("Discipline", typeof(string));
                billingDataTable.Columns.Add("TotalUnits", typeof(string));

                foreach (EIBillingInput ebi in inResult)
                {
                    DataRow dr = billingDataTable.NewRow();

                    dr[0] = ebi.Therapists;
                    dr[1] = ebi.BillDate.ToString("MM/dd/yyyy");
                    dr[2] = ebi.LastName;
                    dr[3] = ebi.FirstName;
                    dr[4] = ebi.County;
                    dr[5] = ebi.FundingSource;
                    dr[6] = ebi.VisitType;
                    dr[7] = ebi.Discipline;
                    dr[8] = ebi.TotalUnits;

                    billingDataTable.Rows.Add(dr);
                }

                using (SqlConnection sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnect))
                    {
                        //Set the database table name
                        sqlBulkCopy.DestinationTableName = "#EIBillingTransactions";

                        //[OPTIONAL]: Map the DataTable columns with that of the database table
                        sqlBulkCopy.ColumnMappings.Add("Therapists", "Therapists");
                        sqlBulkCopy.ColumnMappings.Add("BillDate", "BillDate");
                        sqlBulkCopy.ColumnMappings.Add("LastName", "LastName");
                        sqlBulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                        sqlBulkCopy.ColumnMappings.Add("County", "County");
                        sqlBulkCopy.ColumnMappings.Add("FundingSource", "FundingSource");
                        sqlBulkCopy.ColumnMappings.Add("VisitType", "VisitType");
                        sqlBulkCopy.ColumnMappings.Add("Discipline", "Discipline");
                        sqlBulkCopy.ColumnMappings.Add("TotalUnits", "TotalUnits");

                        sqlConnect.Open();

                        var command = new SqlCommand("CREATE TABLE #EIBillingTransactions (Id UNIQUEIDENTIFIER, Status INT)", sqlConnect);
                        command.ExecuteNonQuery();





                        sqlBulkCopy.WriteToServer(billingDataTable);
                        sqlConnect.Close();
                    }
                }

                var result = (from row1 in billingDataTable.AsEnumerable()
                    join row2 in consumersDataTable.AsEnumerable() on
                        new
                        {
                            lastName = row1.Field<string>("LastName"),
                            firstName = row1.Field<string>("FirstName")
                        }
                        equals
                        new
                        {
                            lastName = row2.Field<string>("consumer_last"),
                            firstName = row2.Field<string>("consumer_first")
                        }
                    select new
                    {
                        consumer_internal_number = row2.Field<int>("consumer_internal_number"),
                        lastName1 = row1.Field<string>("LastName"),
                        firstName1 = row1.Field<string>("FirstName"),
                        lastName2 = row2.Field<string>("consumer_last"),
                        firstName2 = row2.Field<string>("consumer_first"),
                        billDate = row2.Field<string>("BillDate"),
                        county = row2.Field<string>("County"),
                        fundingSource = row2.Field<string>("FundingSource"),
                        visitType = row2.Field<string>("VisitType")

                    }).ToList();




                //var JoinResult = (from p in dt.AsEnumerable()
                //    join t in dt2.AsEnumerable()
                //        on p.Field<string>("EmpName") equals t.Field<string>("EmpName") into tempJoin
                //    from leftJoin in tempJoin.DefaultIfEmpty()
                //    select new
                //    {
                //        EmpName = p.Field<string>("EmpName"),
                //        Grade = leftJoin == null ? 0 : leftJoin.Field<int>("Grade")
                //    }).ToList();





                foreach (EIBillingInput ebi in inResult)
                {
                    string tradingPartner = (ebi.Discipline.Equals("SPECIAL INSTRUCTION")) ? "eisi_in_home" : "eidt_in_home";

                    string tradingPartnerProgram = " ";
                    switch (ebi.FundingSource)
                    {
                        case "MA":
                            tradingPartnerProgram = "ma" + "_" + ebi.County.ToLower();
                            break;

                        case "COUNTY":
                            tradingPartnerProgram = "b" + "_" + ebi.County.ToLower();
                            break;

                        case "WAIVER":
                            tradingPartnerProgram = "w" + "_" + ebi.County.ToLower();
                            break;

                    }

                    string[] therapist = ebi.Therapists.Split(',');

                    string billingNote = " ";
                    switch (ebi.County)
                    {
                        case "ALLEGHENY":
                            billingNote = "CC11006 - ALLEGHENY";
                            break;

                        case "FAYETTE":
                            billingNote = "CC11029 - FAYETTE";
                            break;

                        case "GREENE":
                            billingNote = "CC11032 - GREENE";
                            break;

                        case "WASHINGTON":
                            billingNote = "CC11049 - WASHINGTON";
                            break;

                        case "WESTMORELAND":
                            billingNote = "CC11050 - WESTMORELAND";
                            break;
                    }

                    billingTrans.Add(new EIServiceExportFormat()
                    {
                        consumer_first = ebi.FirstName,
                        consumer_last = ebi.LastName,
                        consumer_internal_number = "4444",
                        diagnosis_code_1_code = "F84.0",
                        trading_partner_string = tradingPartner,
                        trading_partner_program_string = tradingPartnerProgram,
                        start_date_string = ebi.BillDate.ToString("MM/dd/yyyy"),
                        end_date_string = ebi.BillDate.ToString("MM/dd/yyyy"),
                        composite_procedure_code_string = " ",
                        units = ebi.TotalUnits.ToString(),
                        manual_billable_rate = " ",
                        prior_authorization_number = " ",
                        referral_number = " ",
                        referring_provider_id = " ",
                        referring_provider_first_name = " ",
                        referring_provider_last_name = " ",
                        rendering_provider_id = " ",
                        rendering_provider_first_name = therapist[1],
                        rendering_provider_last_name = therapist[0],
                        billing_note = billingNote
                    });
                }

                string outFileName = @"C:\Billing Software\EI\GREENE CTY DEC 2018 -FINAL OUTPUT.csv";
                outEngine.WriteFile(outFileName, billingTrans);

                //BindToDataTable();
                //int range = IsInRange(7.45);
                //range = IsInRange(24.99);
                //range = IsInRange(25.00);
                //range = IsInRange(25.01);
            }
        }

        private void BindToDataTable()        
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = @"ITLOGIC1\UCPDB";
            csb.InitialCatalog = "ATFIS";
            csb.IntegratedSecurity = true;
            string connString = csb.ToString();

            using (SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringAttendance"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetConsumersData", sqlConnection1))
                {
                    Int32 rowsAffected = 0;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@StartDateTime",SqlDbType.Text).Value = "2018-07-01 00:00:00";
                    cmd.Parameters.Add("@EndDateTime", SqlDbType.Text).Value = "2018-07-07 23:59:59";

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                }
            }

            //string queryString =
            //"SELECT tim.[Social Security #]AS ID, " +
            //"tim.[FullName], RIGHT(FullName, 6) AS Ratio, SUBSTRING(RIGHT(FullName, 6), 5, 1) AS Type, " +
            //"SUM((ISNULL(DATEDIFF(second, ta.[StartTime], ta.[EndTime]), 0) + ISNULL(DATEDIFF(second, ta.[StartTime1], ta.[EndTime1]), 0)) / 900) AS TotalUnits " +
            //"FROM([ATFIS].[dbo].[T_Attendance] AS ta " +
            //"LEFT OUTER JOIN[ATFIS].[dbo].[T_Individual Master] AS tim ON ta.[CID] = tim.[IndID]) " + 
            //"LEFT OUTER JOIN[ATFIS].[dbo].[T_Site] AS ts  ON tim.[Site]=ts.[S_ID] " +
            //"WHERE(ta.[ADate]>= '2018-11-01 00:00:00' AND ta.[ADate]<'2018-11-30 23:59:59') AND tim.Status = 'Active' " +
            //"GROUP BY tim.[Social Security #],tim.[FullName]";  
            
            //SqlConnection connection = new SqlConnection(connString);
            //SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
            //DataTable dataTable = new DataTable();
            //adapter.Fill(dataTable);

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