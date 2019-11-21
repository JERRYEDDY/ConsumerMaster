
using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Web.UI;

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
                    SELECT [P_ACTIVE]
                          ,[P_EMPNO]
                          ,[P_FNAME]
                          ,[P_LNAME]
                          ,[P_MI]
                          ,[P_BIRTH]
                          ,[P_SSN]
                          ,[P_SEX]
                          ,[P_EMPEMAIL]
                          ,[P_JOBCODE]
                          ,[P_JOBTITLE]
                          ,[P_RACE]
                          ,[P_LASTHIRE]
                          ,[P_HCITY]
                          ,[P_HSTATE]
                          ,[P_HZIP]
                          ,[P_HSTREET1]
                          ,[P_HSTREET2]
                          ,[P_HCOUNTY]
                      FROM [StaffDatabase].[dbo].[Staff]
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
                    worksheet.Cells[currentRow, sif.GetIndex("last_name")].SetValue(dr["P_LNAME"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("first_name")].SetValue(dr["P_FNAME"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("middle_name")].SetValue(dr["P_MI"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("dob")].SetValue(dr["P_BIRTH"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("ssn_number")].SetValue(dr["P_SSN"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("gender_code")].SetValue(dr["P_SEX"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("email_address")].SetValue(dr["P_EMPEMAIL"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("npi_number")].SetValue("");

                    worksheet.Cells[currentRow, sif.GetIndex("job_title_code")].SetValue(dr["P_JOBCODE"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("job_title")].SetValue(dr["P_JOBTITLE"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("race_code")].SetValue(dr["P_RACE"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("start_date")].SetValue(dr["P_LASTHIRE"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("end_date")].SetValue("");

                    worksheet.Cells[currentRow, sif.GetIndex("supervisor_id")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("supervisor_win_username")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("can_supervise")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("security_scheme_code")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("billing_staff_credentials_code")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("credential_effective_date")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("credential_expiration_date")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("program_code")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("managing_office_code")].SetValue("");

                    worksheet.Cells[currentRow, sif.GetIndex("win_domain")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("win_username")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("use_active_directory")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("is_all_incidents")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("group_name")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("role_workgroup_code")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("workgroup_start_date")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("workgroup_end_date")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("address_type")].SetValue("");
                    worksheet.Cells[currentRow, sif.GetIndex("residence_type")].SetValue("");

                    worksheet.Cells[currentRow, sif.GetIndex("city")].SetValue(dr["P_HCITY"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("state")].SetValue(dr["P_HSTATE"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("zip_code")].SetValue(dr["P_HZIP"].ToString());

                    worksheet.Cells[currentRow, sif.GetIndex("street_address_1")].SetValue(dr["P_HSTREET1"].ToString());
                    worksheet.Cells[currentRow, sif.GetIndex("street_address_2")].SetValue(dr["P_HSTREET2"].ToString());

                    worksheet.Cells[currentRow, sif.GetIndex("county")].SetValue(dr["P_HCOUNTY"].ToString());
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
    }
}