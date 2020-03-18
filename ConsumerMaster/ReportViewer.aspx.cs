using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ConsumerMaster
{
    public partial class ReportViewer : Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                runReportViewer();
            }
        }

        private DataTable getDataTable()
        {
            string selectQuery = $@"
                                    SELECT 
                                        cr.id_no,cr.name,cr.gender,cr.dob,cr.current_location,cr.current_phone_day,cr.intake_date,cr.managing_office,cr.program_name,cr.unit,cr.program_modifier
                                        ,cr.worker_name,cr.worker_role,cr.is_primary_worker,cr.medicaid_number,cr.medicaid_payer,cr.medicaid_plan_name,ca.ba_count,cs.me_count,cs.ssp_count
                                    FROM 
                                        ClientRoster AS cr
                                    LEFT JOIN ClientAuthorizations AS ca  ON cr.id_no = ca.AClientID
                                    LEFT JOIN ClientStaff AS cs  ON cr.id_no = cs.SClientID
                                ";
            DataTable dataTable = new DataTable();
            try 
            {
                Utility util = new Utility();
                dataTable = util.GetDataTable3(selectQuery);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return dataTable;
        }

        private void runReportViewer()
        {
            try
            {             
                this.ReportViewer1.Reset();
                this.ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://ITLT21T/ReportServer");
                this.ReportViewer1.ServerReport.ReportPath = "/NetSmart/ClientIntegrity";
                //ReportDataSource rds = new ReportDataSource("dsNewDataSet_Table", getDataTable());
                //this.ReportViewer1.LocalReport.DataSources.Clear();
                //this.ReportViewer1.LocalReport.DataSources.Add(rds);
                //this.ReportViewer1.DataBind();
                this.ReportViewer1.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}