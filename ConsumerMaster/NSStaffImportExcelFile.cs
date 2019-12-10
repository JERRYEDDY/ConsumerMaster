
using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using System.Globalization;
using System.Threading;

namespace ConsumerMaster
{
    public class NSStaffImportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateWorkbook()
        {
            Workbook workbook = new Workbook();

            try
            {
                workbook.Sheets.Add(SheetType.Worksheet);
                Worksheet worksheet = workbook.ActiveWorksheet;

                string selectQuery =
                $@"
                    SELECT s.P_ACTIVE,awc.P_EMPNO,awc.P_FNAME,awc.P_LNAME,s.P_MI,s.P_BIRTH,s.P_SSN,s.P_SEX,s.P_EMPEMAIL,s.P_JOBCODE,s.P_JOBTITLE,s.P_LASTHIRE,s.P_HCITY,s.P_HSTATE,s.P_HZIP,
                            s.P_HSTREET1,s.P_HSTREET2,s.P_HCOUNTY
                    FROM AWCStaff awc
                    LEFT JOIN
	                    Staff AS s ON awc.P_EMPNO = s.P_EMPNO
                    ORDER BY awc.P_LNAME, awc.P_FNAME
                 ";

                Utility util = new Utility();
                NSStaffImportFormat sif = new NSStaffImportFormat();
                DataTable dTable = util.GetDataTable2(selectQuery);

                int totalConsumers = dTable.Rows.Count;
                PrepareInformationWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in dTable.Rows)
                {
                    worksheet.Cells[currentRow, sif.GetIndex("id_no")].SetValue(dr["P_EMPNO"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("last_name")].SetValue(ToTitleCase(dr["P_LNAME"].ToString()));
                    worksheet.Cells[currentRow, sif.GetIndex("first_name")].SetValue(ToTitleCase(dr["P_FNAME"].ToString()));
                    worksheet.Cells[currentRow, sif.GetIndex("middle_name")].SetValue(dr["P_MI"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("dob")].SetValue(dr["P_BIRTH"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("ssn_number")].SetValue(dr["P_SSN"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("gender_code")].SetValue(dr["P_SEX"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("email_address")].SetValue(dr["P_EMPEMAIL"].ToString().ToLower());
                    worksheet.Cells[currentRow, sif.GetIndex("npi_number")].SetValue("");

                    worksheet.Cells[currentRow, sif.GetIndex("job_title_code")].SetValue("SSP"); //SSP
                    worksheet.Cells[currentRow, sif.GetIndex("job_title")].SetValue("Support Service Professional"); //Support Service Professional
                    worksheet.Cells[currentRow, sif.GetIndex("race_code")].SetValue(" ");
                    worksheet.Cells[currentRow, sif.GetIndex("start_date")].SetValue(dr["P_LASTHIRE"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("end_date")].SetValue("");

                    worksheet.Cells[currentRow, sif.GetIndex("supervisor_id")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("supervisor_win_username")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("can_supervise")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("security_scheme_code")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("billing_staff_credentials_code")].SetValue("SSP");

                    CellValueFormat dateEffectiveCellValueFormat = new CellValueFormat("mm/dd/yyyy");
                    worksheet.Cells[currentRow, sif.GetIndex("credential_effective_date")].SetFormat(dateEffectiveCellValueFormat);
                    worksheet.Cells[currentRow, sif.GetIndex("credential_effective_date")].SetValue("07/01/2019");

                    worksheet.Cells[currentRow, sif.GetIndex("credential_expiration_date")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("program_code")].SetValue("AWC");  //AWC

                    CellValueFormat officeCellValueFormat = new CellValueFormat("00000");
                    worksheet.Cells[currentRow, sif.GetIndex("managing_office_code")].SetFormat(officeCellValueFormat);
                    worksheet.Cells[currentRow, sif.GetIndex("managing_office_code")].SetValue("00014");  //Washington Location

                    worksheet.Cells[currentRow, sif.GetIndex("win_domain")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("win_username")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("use_active_directory")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("is_all_incidents")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("group_name")].SetValue("AWC");  //AWC
                    worksheet.Cells[currentRow, sif.GetIndex("role_workgroup_code")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("workgroup_start_date")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("workgroup_end_date")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("address_type")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("residence_type")].SetValue("");

                    worksheet.Cells[currentRow, sif.GetIndex("city")].SetValue(dr["P_HCITY"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("state")].SetValue(dr["P_HSTATE"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("zip_code")].SetValue(dr["P_HZIP"].ToString());

                    worksheet.Cells[currentRow, sif.GetIndex("street_address_1")].SetValue(ToTitleCase(dr["P_HSTREET1"].ToString()));
                    worksheet.Cells[currentRow, sif.GetIndex("street_address_2")].SetValue(dr["P_HSTREET2"].ToString());

                    worksheet.Cells[currentRow, sif.GetIndex("county")].SetValue(ToTitleCase(dr["P_HCOUNTY"].ToString()));
                    worksheet.Cells[currentRow, sif.GetIndex("address_date")].SetValue("");

                    currentRow++;
                }

                for (int i = 0; i < dTable.Columns.Count; i++)
                {
                    worksheet.Columns[i].AutoFitWidth();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }

        private void PrepareInformationWorksheet(Worksheet worksheet)
        {
            try
            {
                NSStaffImportFormat sif = new NSStaffImportFormat();
                string[] columnsList = sif.ColumnStrings;

                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column);
                    string columnName = column;

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public string ToTitleCase(string str)
        {
            // Convert to proper case.
            CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string title = textInfo.ToTitleCase(str.ToLower());

            return title;
        }

    }
}