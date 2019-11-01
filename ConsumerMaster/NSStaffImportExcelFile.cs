
using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;

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

                Utility util = new Utility();

                NSStaffImportFormat ccf = new NSStaffImportFormat();
                DataTable dTable = util.GetEmployeePersonnelDataTable("C:/NetSmart/Import/EMPLOYEEPERSONNEL.TXT");

                int totalConsumers = dTable.Rows.Count;
                PrepareInformationWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in dTable.Rows)
                {
                    worksheet.Cells[currentRow, ccf.GetIndex("client_id")].SetValue(dr["client_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("last_name")].SetValue(dr["last_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("first_name")].SetValue(dr["first_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("middle_name")].SetValue(dr["middle_name"].ToString());

                    string genderName = string.Equals(dr["gender_code"], "M") ? "Male" : "Female";
                    worksheet.Cells[currentRow, ccf.GetIndex("gender")].SetValue(genderName);
                    worksheet.Cells[currentRow, ccf.GetIndex("gender_code")].SetValue(dr["gender_code"].ToString());

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

                    worksheet.Cells[currentRow, ccf.GetIndex("address_effective_date")].SetValue(dr["address_effective_date"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("religion")].SetValue(dr["religion"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("religion_code")].SetValue(dr["religion_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("citizenship")].SetValue(dr["citizenship"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("citizenship_code")].SetValue(dr["citizenship_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("marital_status")].SetValue(dr["marital_status"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("marital_status_code")].SetValue(dr["marital_status_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("ethnicity")].SetValue(dr["ethnicity"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("ethnicity_code")].SetValue(dr["ethnicity_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("primary_language")].SetValue(dr["primary_language"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("primary_language_code")].SetValue(dr["primary_language_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("secondary_language")].SetValue(dr["secondary_language"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("secondary_language_code")].SetValue(dr["secondary_language_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("day_phone")].SetValue(dr["day_phone"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("evening_phone")].SetValue(dr["evening_phone"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("mobile_phone")].SetValue(dr["mobile_phone"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("pager")].SetValue(dr["pager"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("email_address")].SetValue(dr["email_address"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("race_1")].SetValue(dr["race_1"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("race_1_code")].SetValue(dr["race_1_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("race_1_other_description")].SetValue(dr["race_1_other_description"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("race_2")].SetValue(dr["race_2"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("race_2_code")].SetValue(dr["race_2_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("race_2_other_description")].SetValue(dr["race_2_other_description"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_name")].SetValue(dr["curr_employment_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_business")].SetValue(dr["curr_employment_business"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_position")].SetValue(dr["curr_employment_position"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_status")].SetValue(dr["curr_employment_status"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_status_code")].SetValue(dr["curr_employment_status_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_phone")].SetValue(dr["curr_employment_phone"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("curr_employment_start_date")].SetValue(dr["curr_employment_start_date"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("education_degree")].SetValue(dr["education_degree"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("education_degree_code")].SetValue(dr["education_degree_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("education_highest_grade")].SetValue(dr["education_highest_grade"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("education_highest_grade_code")].SetValue(dr["education_highest_grade_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("urn_no")].SetValue(dr["urn_no"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("county")].SetValue(dr["county"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("county_code")].SetValue(dr["county_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("salutation")].SetValue(dr["salutation"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("salutation_code")].SetValue(dr["salutation_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("fax_number")].SetValue(dr["fax_number"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("school_attended")].SetValue(dr["school_attended"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("school_attended_code")].SetValue(dr["school_attended_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("TABS_ID")].SetValue(dr["TABS_ID"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("education_id")].SetValue(dr["education_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("residence_type")].SetValue(dr["residence_type"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("residence_type_code")].SetValue(dr["residence_type_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("name_suffix")].SetValue(dr["name_suffix"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("agency_id_no")].SetValue(dr["agency_id_no"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("agency_other_id_number")].SetValue(dr["agency_other_id_number"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("other_id_no")].SetValue(dr["other_id_no"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("hair_color")].SetValue(dr["hair_color"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("hair_color_code")].SetValue(dr["hair_color_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("eye_color")].SetValue(dr["eye_color"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("eye_color_code")].SetValue(dr["eye_color_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("aka")].SetValue(dr["aka"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("maiden_name")].SetValue(dr["maiden_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("ethnicity_details")].SetValue(dr["ethnicity_details"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("ethnicity_details_code")].SetValue(dr["ethnicity_details_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("original_table_name")].SetValue(dr["original_table_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("ssn_unknown")].SetValue(dr["ssn_unknown"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("ssn_no")].SetValue(dr["ssn_no"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("mothers_first_name")].SetValue(dr["mothers_first_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("veteran_status")].SetValue(dr["veteran_status"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("veteran_status_code")].SetValue(dr["veteran_status_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("region")].SetValue(dr["region"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("planning_area")].SetValue(dr["planning_area"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("geocode")].SetValue(dr["geocode"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("twn_ca_name")].SetValue(dr["twn_ca_name"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("address_type")].SetValue(dr["address_type"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("address_type_code")].SetValue(dr["address_type_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("agency_name")].SetValue(dr["agency_name"].ToString());

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

        public Workbook CreateDiagnosisWorkbook()
        {
            Workbook workbook = new Workbook();

            try
            {
                workbook.Sheets.Add(SheetType.Worksheet);
                Worksheet worksheet = workbook.ActiveWorksheet;

                string selectQuery =
                $@"
                    SELECT 
                        ' ' AS diagnosis_id, c.consumer_internal_number AS client_id, i.description AS diagnosis, c.diagnosis AS diagnosis_code, ' ' AS date_diagnosed, '1' AS is_primary, 
                        ' ' AS dsm_IV_code, ' ' AS dsm_V_code, ' ' AS icd9_code, c.diagnosis AS icd10_code, ' ' AS rule_out, ' ' AS gaf_score, ' ' AS priority, ' ' AS staff_id, 
                        ' ' AS original_table_name 
                    FROM 
                        Consumers AS c
                    INNER JOIN 
                        ICD10Codes AS i ON replace(c.diagnosis,'.','') = i.code 
                    ORDER BY c.consumer_internal_number
                 ";

                Utility util = new Utility();
                ClientDiagnosisFormat ccf = new ClientDiagnosisFormat();
                DataTable ceDataTable = util.GetDataTable(selectQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareDiagnosisWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    int diagnosisID = currentRow;
                    worksheet.Cells[currentRow, ccf.GetIndex("diagnosis_id")].SetValue(diagnosisID.ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("client_id")].SetValue(dr["client_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("diagnosis")].SetValue(dr["diagnosis"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("diagnosis_code")].SetValue(dr["diagnosis_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("date_diagnosed")].SetValue(dr["date_diagnosed"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("is_primary")].SetValue(dr["is_primary"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("dsm_IV_code")].SetValue(dr["dsm_IV_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("dsm_V_code")].SetValue(dr["dsm_V_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("icd9_code")].SetValue(dr["icd9_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("icd10_code")].SetValue(dr["icd10_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("rule_out")].SetValue(dr["rule_out"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("gaf_score")].SetValue(dr["gaf_score"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("priority")].SetValue(dr["priority"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("staff_id")].SetValue(dr["staff_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("original_table_name")].SetValue(dr["original_table_name"].ToString());

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

                string selectQuery =
                $@"
                    SELECT 
                        c.consumer_internal_number AS client_id, ' ' AS payor_name,  ' ' AS contract_name, ' ' AS co_pay_amount, ' ' AS date_start, ' ' AS date_end, 
                        '0' AS is_self_pay, ' ' AS billing_sequence, ' ' AS priority, c.identifier AS policy_number, ' ' AS group_number, ' ' AS client_link_id, 
                        ' ' AS relationship, ' ' AS relationship_code, ' ' AS is_medicaid_additional, ' ' AS is_deduction, ' ' AS is_deduction_percentage, ' ' AS deduction_percent,
                        ' ' AS deduction_action_code, ' ' AS original_table_name
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
                    worksheet.Cells[currentRow, ccf.GetIndex("co_pay_amount")].SetValue(dr["co_pay_amount"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("date_start")].SetValue(dr["date_start"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("date_end")].SetValue(dr["date_end"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("is_self_pay")].SetValue(dr["is_self_pay"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("billing_sequence")].SetValue(dr["billing_sequence"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("priority")].SetValue(dr["priority"].ToString());

                    CellValueFormat identifierCellValueFormat = new CellValueFormat("0000000000");
                    worksheet.Cells[currentRow, ccf.GetIndex("policy_number")].SetFormat(identifierCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("policy_number")].SetValue(dr["policy_number"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("group_number")].SetValue(dr["group_number"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("client_link_id")].SetValue(dr["client_link_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("relationship")].SetValue(dr["relationship"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("relationship_code")].SetValue(dr["relationship_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("is_medicaid_additional")].SetValue(dr["is_medicaid_additional"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("is_deduction")].SetValue(dr["is_deduction"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("is_deduction_percentage")].SetValue(dr["is_deduction_percentage"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("deduction_percent")].SetValue(dr["deduction_percent"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("deduction_action_code")].SetValue(dr["deduction_action_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("original_table_name")].SetValue(dr["original_table_name"].ToString());

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