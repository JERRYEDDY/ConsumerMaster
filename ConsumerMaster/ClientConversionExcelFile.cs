
using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;

namespace ConsumerMaster
{
    public class ClientConversionExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateInformationWorkbook()
        {
            Workbook workbook = new Workbook();

            try
            {
                workbook.Sheets.Add(SheetType.Worksheet);
                Worksheet worksheet = workbook.ActiveWorksheet;
                worksheet.Name = "Client_information";

                string selectQuery = 
                $@"
                    SELECT 
                        c.consumer_internal_number AS client_id, c.consumer_last AS last_name, c.consumer_first AS first_name, ' ' AS middle_name,
                        ' ' AS gender, c.gender AS gender_code, c.date_of_birth AS date_of_birth, c.address_line_1 AS street_address_1, 
                        ISNULL(c.address_line_2, ' ') AS street_address_2, c.city AS city, states.name AS state, c.state AS state_code, c.zip_code AS zip_code,  
                        tp.symbol AS trading_partner_string 
                    FROM 
                        Consumers AS c
                    INNER JOIN 
                        TradingPartners AS tp ON c.trading_partner_id1 = tp.id 
                    INNER JOIN
                        States AS states ON c.state = states.Abbreviation
                    ORDER BY consumer_last
                 ";

                Utility util = new Utility();

                //DataTable dTable = util.GetEmployeePersonnelDataTable("C:/NetSmart/Import/EMPLOYEEPERSONNEL.TXT");

                ClientInformationFormat ccf = new ClientInformationFormat();
                DataTable ceDataTable = util.GetDataTable(selectQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareInformationWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    worksheet.Cells[currentRow, ccf.GetIndex("client_id")].SetValue(dr["client_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("last_name")].SetValue(dr["last_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("first_name")].SetValue(dr["first_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("middle_name")].SetValue(dr["middle_name"].ToString());

                    string genderName = string.Equals(dr["gender_code"], "M") ? "Male" : "Female";
                    worksheet.Cells[currentRow, ccf.GetIndex("gender")].SetValue(genderName);
                    worksheet.Cells[currentRow, ccf.GetIndex("gender_code")].SetValue(dr["gender_code"].ToString());

                    CellValueFormat dateBirthCellValueFormat = new CellValueFormat("mm/dd/yyyy");
                    worksheet.Cells[currentRow, ccf.GetIndex("date_of_birth")].SetFormat(dateBirthCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("date_of_birth")].SetValue(dr["date_of_birth"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("ss_number")].SetValue(dr["ss_number"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("driver_license_number")].SetValue(dr["driver_license_number"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("city_of_birth")].SetValue(dr["city_of_birth"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("state_of_birth")].SetValue(dr["state_of_birth"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("state_of_birth_code")].SetValue(dr["state_of_birth_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("country_of_birth")].SetValue(dr["country_of_birth"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("street_address_1")].SetValue(dr["street_address_1"].ToString());

                    string addressLine2 = dr["street_address_2"] == null ? string.Empty : dr["street_address_2"].ToString(); 
                    worksheet.Cells[currentRow, ccf.GetIndex("street_address_2")].SetValue(addressLine2);

                    worksheet.Cells[currentRow, ccf.GetIndex("city")].SetValue(dr["city"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("state")].SetValue(dr["state"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("state_code")].SetValue(dr["state_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("zip_code")].SetValue(dr["zip_code"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("address_effective_date")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("religion")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("religion_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("citizenship")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("citizenship_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("marital_status")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("marital_status_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("ethnicity")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("ethnicity_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("primary_language")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("primary_language_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("secondary_language")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("secondary_language_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("day_phone")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("evening_phone")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("mobile_phone")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("pager")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("email_address")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("race_1")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("race_1_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("race_1_other_description")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("race_2")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("race_2_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("race_2_other_description")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_name")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_business")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_position")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_status")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_status_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_phone")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_start_date")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("education_degree")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("education_degree_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("education_highest_grade")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("education_highest_grade_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("urn_no")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("county")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("county_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("salutation")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("salutation_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("fax_number")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("school_attended")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("school_attended_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("TABS_ID")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("education_id")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("residence_type")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("residence_type_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("name_suffix")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("agency_id_no")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("agency_other_id_number")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("other_id_no")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("hair_color")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("hair_color_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("eye_color")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("eye_color_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("aka")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("maiden_name")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("ethnicity_details")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("ethnicity_details_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("original_table_name")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("ssn_unknown")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("ssn_no")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("mothers_first_name")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("veteran_status")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("veteran_status_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("region")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("planning_area")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("geocode")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("twn_ca_name")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("address_type")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("address_type_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("agency_name")].SetValue(" ");

                    currentRow++;
                }

                for (int i = 0; i < ceDataTable.Columns.Count; i++)
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

        public Workbook CreateDiagnosisWorkbook()
        {
            Workbook workbook = new Workbook();

            try
            {
                workbook.Sheets.Add(SheetType.Worksheet);
                Worksheet worksheet = workbook.ActiveWorksheet;
                worksheet.Name = "client_diagnoses";

                string selectQuery =
                $@"
                    SELECT 
                        DISTINCT c.consumer_internal_number AS client_id,  d.ICD9Code AS icd9_code,c.diagnosis AS diagnosis_code, d.description AS diagnosis 
                    FROM 
                        Consumers AS c
                    LEFT JOIN 
                        DiagnosisCodes AS d ON replace(c.diagnosis,'.','') = d.ICD10Code 
                    ORDER BY c.consumer_internal_number
                 ";

                Utility util = new Utility();
                ClientDiagnosisFormat ccf = new ClientDiagnosisFormat();
                DataTable ceDataTable = util.GetDataTable(selectQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareDiagnosisWorksheet(worksheet);

                int diagnosisID = 0;
                diagnosisID++;

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    worksheet.Cells[currentRow, ccf.GetIndex("diagnosis_id")].SetValue(currentRow.ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("client_id")].SetValue(dr["client_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("diagnosis")].SetValue(dr["diagnosis"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("diagnosis_code")].SetValue(dr["diagnosis_code"].ToString());

                    CellValueFormat dateDiagnosisCellValueFormat = new CellValueFormat("mm/dd/yyyy");
                    worksheet.Cells[currentRow, ccf.GetIndex("date_diagnosed")].SetFormat(dateDiagnosisCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("date_diagnosed")].SetValue("07/01/2019");

                    worksheet.Cells[currentRow, ccf.GetIndex("is_primary")].SetValue("1");
                    worksheet.Cells[currentRow, ccf.GetIndex("dsm_IV_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("dsm_V_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("icd9_code")].SetValue(dr["icd9_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("icd10_code")].SetValue(dr["diagnosis_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("rule_out")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("gaf_score")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("priority")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("staff_id")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("original_table_name")].SetValue(" ");

                    currentRow++;
                }

                for (int i = 0; i < ceDataTable.Columns.Count; i++)
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

        public Workbook CreateBenefitsWorkbook()
        {
            Workbook workbook = new Workbook();

            try
            {
                workbook.Sheets.Add(SheetType.Worksheet);
                Worksheet worksheet = workbook.ActiveWorksheet;
                worksheet.Name = "Client_Benefits";

                string selectQuery =
                $@"
                    SELECT 
                        c.consumer_internal_number AS client_id, 'Medicaid' AS payor_name,  'PA Medical Assistance Waiver' AS contract_name, ' ' AS date_start, c.identifier AS policy_number
                    FROM 
                        Consumers AS c
                    ORDER BY c.consumer_internal_number
                 ";

                Utility util = new Utility();
                ClientBenefitsFormat ccf = new ClientBenefitsFormat();
                DataTable ceDataTable = util.GetDataTable(selectQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareBenefitsWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    worksheet.Cells[currentRow, ccf.GetIndex("client_id")].SetValue(dr["client_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("payor_name")].SetValue(dr["payor_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("contract_name")].SetValue(dr["contract_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("co_pay_amount")].SetValue(" ");

                    CellValueFormat dateStartCellValueFormat = new CellValueFormat("mm/dd/yyyy");
                    worksheet.Cells[currentRow, ccf.GetIndex("date_start")].SetFormat(dateStartCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("date_start")].SetValue("07/01/2019");

                    worksheet.Cells[currentRow, ccf.GetIndex("date_end")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("is_self_pay")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("billing_sequence")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("priority")].SetValue("1");

                    CellValueFormat identifierCellValueFormat = new CellValueFormat("0000000000");
                    worksheet.Cells[currentRow, ccf.GetIndex("policy_number")].SetFormat(identifierCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("policy_number")].SetValue(dr["policy_number"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("group_number")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("client_link_id")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("relationship")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("relationship_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("is_medicaid_additional")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("is_deduction")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("is_deduction_percentage")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("deduction_percent")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("deduction_action_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("original_table_name")].SetValue(" ");

                    currentRow++;
                }

                for (int i = 0; i < ceDataTable.Columns.Count; i++)
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
                ClientInformationFormat ccf = new ClientInformationFormat();
                string[] columnsList = ccf.ColumnStrings;

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

        private void PrepareDiagnosisWorksheet(Worksheet worksheet)
        {
            try
            {
                ClientDiagnosisFormat ccf = new ClientDiagnosisFormat();
                string[] columnsList = ccf.ColumnStrings;

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

        private void PrepareBenefitsWorksheet(Worksheet worksheet)
        {
            try
            {
                ClientBenefitsFormat ccf = new ClientBenefitsFormat();
                string[] columnsList = ccf.ColumnStrings;

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