
using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;

namespace ConsumerMaster
{
    public class ClientConversionExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        public Workbook CreateAllWorkbook()
        {
            Workbook workbook = new Workbook();

            WorksheetCollection worksheets = workbook.Worksheets;
            worksheets.Add(); //Client_information
            worksheets.Add(); //client_diagnosis
            worksheets.Add(); //Client_Benefits
            worksheets.Add(); //Client_Enrollment
            worksheets.Add(); //Staff Assignment
            worksheets.Add(); //Authorizations
            //worksheets.Add();  //conv_authorization_details

            Worksheet sheet1Worksheet = worksheets[0];
            sheet1Worksheet.Name = "Client_information"; 
            CreateInformationWorkbook(sheet1Worksheet);

            Worksheet sheet2Worksheet = worksheets[1];
            sheet2Worksheet.Name = "client_diagnosis";
            CreateDiagnosisWorkbook(sheet2Worksheet);

            Worksheet sheet3Worksheet = worksheets[2];
            sheet3Worksheet.Name = "Client_Benefits";
            CreateBenefitsWorkbook(sheet3Worksheet);

            Worksheet sheet4Worksheet = worksheets[3];
            sheet4Worksheet.Name = "Client_Enrollment";
            CreateEnrollmentWorkbook(sheet4Worksheet);

            Worksheet sheet5Worksheet = worksheets[4];
            sheet5Worksheet.Name = "Staff Assignment";
            CreateStaffAssignmentsWorkbook(sheet5Worksheet);

            Worksheet sheet6Worksheet = worksheets[5];
            sheet6Worksheet.Name = "Authorizations";
            CreateAuthorizationsWorkbook(sheet6Worksheet);

            //Worksheet sheet7Worksheet = worksheets[6];
            //sheet7Worksheet.Name = "conv_authorization_details";
            //CreateServiceAuthorizationsWorkbook(sheet7Worksheet);

            return workbook;
        }

        public void CreateInformationWorkbook(Worksheet worksheet)
        {
            try
            {
                string selectQuery = 
                $@"
                    SELECT 
                        c.consumer_internal_number AS client_id, c.consumer_last AS last_name, c.consumer_first AS first_name, ' ' AS middle_name,
                        ' ' AS gender, c.gender AS gender_code, c.date_of_birth AS date_of_birth, ' ' AS ss_number, c.address_line_1 AS street_address_1, 
                        ISNULL(c.address_line_2, ' ') AS street_address_2, c.city AS city, states.name AS state, c.state AS state_code, c.zip_code AS zip_code,  
                        c.trading_partner_id1,c.trading_partner_id2,c.trading_partner_id3
                    FROM 
                        Consumers AS c
                    INNER JOIN 
                        TradingPartners AS tp ON c.trading_partner_id1 = tp.id 
                    INNER JOIN
                        States AS states ON c.state = states.Abbreviation
                    WHERE c.trading_partner_id1 = 5 OR c.trading_partner_id2 = 5 OR c.trading_partner_id3 = 5
                    ORDER BY c.consumer_internal_number
                 ";

                Utility util = new Utility();
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

                    CellValueFormat identifierCellValueFormat = new CellValueFormat("000000000");
                    worksheet.Cells[currentRow, ccf.GetIndex("ss_number")].SetFormat(identifierCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("ss_number")].SetValue(dr["ss_number"].ToString());


                    worksheet.Cells[currentRow, ccf.GetIndex("driver_license_number")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("city_of_birth")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("state_of_birth")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("state_of_birth_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("country_of_birth")].SetValue(" ");

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
        }

        public void CreateDiagnosisWorkbook(Worksheet worksheet)
        {
            try
            {
                string selectQuery =
                $@"
                    SELECT 
                        DISTINCT c.consumer_internal_number AS client_id,  d.ICD9Code AS icd9_code,c.diagnosis AS diagnosis_code, d.description AS diagnosis 
                    FROM 
                        Consumers AS c
                    LEFT JOIN 
                        DiagnosisCodes AS d ON replace(c.diagnosis,'.','') = d.ICD10Code 
                    WHERE c.trading_partner_id1 = 5 OR c.trading_partner_id2 = 5 OR c.trading_partner_id3 = 5
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
        }

        public void CreateBenefitsWorkbook(Worksheet worksheet)
        {
            try
            {
                string selectQuery =
                $@"
                    SELECT 
                        c.consumer_internal_number AS client_id, '001' AS payor_name,  'PA_MAW' AS contract_name, ' ' AS date_start, c.identifier AS policy_number
                    FROM 
                        Consumers AS c
                    WHERE c.trading_partner_id1 = 5 OR c.trading_partner_id2 = 5 OR c.trading_partner_id3 = 5
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
                    worksheet.Cells[currentRow, ccf.GetIndex("payor_name")].SetValue(dr["payor_name"].ToString()); //001
                    worksheet.Cells[currentRow, ccf.GetIndex("contract_name")].SetValue(dr["contract_name"].ToString()); //PA_MAW

                    CellValueFormat co_pay_amountCellValueFormat = new CellValueFormat("0.00");
                    worksheet.Cells[currentRow, ccf.GetIndex("co_pay_amount")].SetFormat(co_pay_amountCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("co_pay_amount")].SetValue("0");  //decimal form

                    CellValueFormat dateStartCellValueFormat = new CellValueFormat("mm/dd/yyyy");
                    worksheet.Cells[currentRow, ccf.GetIndex("date_start")].SetFormat(dateStartCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("date_start")].SetValue("07/01/2019");

                    worksheet.Cells[currentRow, ccf.GetIndex("date_end")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("is_self_pay")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("billing_sequence")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("priority")].SetValue("1");

                    CellValueFormat policy_numberCellValueFormat = new CellValueFormat("0000000000");
                    worksheet.Cells[currentRow, ccf.GetIndex("policy_number")].SetFormat(policy_numberCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("policy_number")].SetValue(dr["policy_number"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("group_number")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("client_link_id")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("relationship")].SetValue("Self"); //Self
                    worksheet.Cells[currentRow, ccf.GetIndex("relationship_code")].SetValue("45"); //45
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
        }

        public void CreateEnrollmentWorkbook(Worksheet worksheet)
        {
            try
            {
                string selectQuery =
                $@"
                    SELECT 
                        c.consumer_internal_number AS client_id, 'AWC' AS program_code, '00014' AS service_facility_code, '1' AS room_number
                    FROM 
                        Consumers AS c
                    WHERE c.trading_partner_id1 = 5 OR c.trading_partner_id2 = 5 OR c.trading_partner_id3 = 5
                    ORDER BY c.consumer_internal_number
                 ";

                Utility util = new Utility();
                ClientEnrollmentFormat ccf = new ClientEnrollmentFormat();
                DataTable ceDataTable = util.GetDataTable(selectQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareEnrollmentWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    worksheet.Cells[currentRow, ccf.GetIndex("client_id")].SetValue(dr["client_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("program_code")].SetValue(dr["program_code"].ToString()); //AWC

                    CellValueFormat identifierCellValueFormat = new CellValueFormat("00000");
                    worksheet.Cells[currentRow, ccf.GetIndex("service_facility_code")].SetFormat(identifierCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("service_facility_code")].SetValue(dr["service_facility_code"].ToString()); //00014

                    worksheet.Cells[currentRow, ccf.GetIndex("foster_home_id")].SetValue(" ");  
                    worksheet.Cells[currentRow, ccf.GetIndex("room_number")].SetValue("1"); //1
                    worksheet.Cells[currentRow, ccf.GetIndex("enrollment_id")].SetValue(currentRow.ToString());

                    CellValueFormat dateStartCellValueFormat = new CellValueFormat("mm/dd/yyyy");
                    worksheet.Cells[currentRow, ccf.GetIndex("start_date")].SetFormat(dateStartCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("start_date")].SetValue("07/01/2019");

                    worksheet.Cells[currentRow, ccf.GetIndex("end_date")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("is_planned_discharge")].SetValue(" ");

                    worksheet.Cells[currentRow, ccf.GetIndex("outcome")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("outcome_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("closing_reason")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("closing_reason_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("overall_discharge_date")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("org_id")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("referral_date")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("group_id")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("unit_id")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("schedule_id")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("referral_id")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("referral_reason")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("referral_reason_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("discharged_to_type")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("discharged_to_type_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("discharge_to_other")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("inquiry_type")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("inquiry_type_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("marketing_info")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("marketing_info_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("remarks")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("placed_by")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("placed_by_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("transfer_type")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("transfer_type_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("opening_reasons")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("opening_reasons_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("condition_at_discharge")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("condition_at_discharge_code")].SetValue(" ");
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
        }

        public void CreateStaffAssignmentsWorkbook(Worksheet worksheet)
        {
            try
            {

                //string tamaraSchererID = "4681";
                //string monicaKingID = "5717";

                string selectQuery =
                $@"
                    SELECT c.consumer_internal_number AS client_id,s.P_EMPNO AS staff_id, ' ' AS is_primary
                    FROM ConsumerSSP cs
                    LEFT JOIN
	                    Consumers AS c ON cs.consumer like CONCAT(c.consumer_last,', ',c.consumer_first)
                    LEFT JOIN
	                    SSPIDs AS s ON cs.SSP like s.P_SSP
                    WHERE c.consumer_internal_number IS NOT NULL AND s.P_EMPNO IS NOT NULL
                    ORDER BY cs.consumer
                 ";

                string selectTamaraQuery =
                $@"
                    SELECT c.consumer_internal_number AS client_id,'4681' AS staff_id,'1' AS is_primary
                    FROM ConsumerSSP cs
                    LEFT JOIN
	                    Consumers AS c ON cs.consumer like CONCAT(c.consumer_last,', ',c.consumer_first)
                    LEFT JOIN
	                    SSPIDs AS s ON cs.SSP like s.P_SSP
                    LEFT JOIN
	                    ZipCounty AS z ON SUBSTRING(c.zip_code,1,5) = z.ZipCode
                    WHERE c.consumer_internal_number IS NOT NULL AND s.P_EMPNO IS NOT NULL AND z.County IN ('Allegheny','Lawrence','Greene')
                    ORDER BY cs.consumer
                ";

                string selectMonicaQuery =
                $@"
                    SELECT c.consumer_internal_number AS client_id,'5717' AS staff_id,'1' AS is_primary
                    FROM ConsumerSSP cs
                    LEFT JOIN
	                    Consumers AS c ON cs.consumer like CONCAT(c.consumer_last,', ',c.consumer_first)
                    LEFT JOIN
	                    SSPIDs AS s ON cs.SSP like s.P_SSP
                    LEFT JOIN
	                    ZipCounty AS z ON SUBSTRING(c.zip_code,1,5) = z.ZipCode
                    WHERE c.consumer_internal_number IS NOT NULL AND s.P_EMPNO IS NOT NULL AND z.County NOT IN ('Allegheny','Lawrence','Greene')
                    ORDER BY cs.consumer
                ";
                
                Utility util = new Utility();
                ClientStaffAssignmentsFormat ccf = new ClientStaffAssignmentsFormat();
                DataTable ceDataTable = util.GetDataTable(selectQuery);

                DataTable tamaraDataTable = util.GetDataTable(selectTamaraQuery);
                ceDataTable.Merge(tamaraDataTable);  //Append Tamara assignments
                ceDataTable.AcceptChanges();

                DataTable monicaDataTable = util.GetDataTable(selectMonicaQuery);
                ceDataTable.Merge(monicaDataTable); //Append Monica assignments
                ceDataTable.AcceptChanges();

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareStaffAssignmentsWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    worksheet.Cells[currentRow, ccf.GetIndex("client_id")].SetValue(dr["client_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("staff_id")].SetValue(dr["staff_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("program_code")].SetValue("AWC");  //AWC

                    CellValueFormat startDateCellValueFormat = new CellValueFormat("mm/dd/yyyy");
                    worksheet.Cells[currentRow, ccf.GetIndex("start_date")].SetFormat(startDateCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("start_date")].SetValue("07/01/2019");

                    worksheet.Cells[currentRow, ccf.GetIndex("role")].SetValue("AWC Support Service Professional");
                    worksheet.Cells[currentRow, ccf.GetIndex("role_code")].SetValue("AWCSS");
                    worksheet.Cells[currentRow, ccf.GetIndex("end_date")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("is_primary")].SetValue(dr["is_primary"].ToString());
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
        }

        public void CreateAuthorizationsWorkbook(Worksheet worksheet)
        {
            try
            {
                //string selectQuery =
                //$@"
                //    SELECT ROW_NUMBER() OVER(ORDER BY c.consumer_internal_number) auth_original_id,
                //        c.consumer_internal_number AS client_id, '001' AS payor_name, 'PA_MAW' AS contract_name,
                //        ca.ServiceStartDate AS start_date,ca.ServiceEndDate AS end_date,ca.AuthorizedUnits AS units
                //    FROM ConsumerAuthorizations AS ca
                //    INNER JOIN
	               //     Consumers AS c ON ca.Identifier = c.identifier
                //    ORDER BY c.consumer_internal_number
                // ";

                string selectQuery =
                $@"
                    SELECT ROW_NUMBER() OVER(ORDER BY c.consumer_internal_number) auth_original_id,
                        c.consumer_internal_number AS client_id, 
	                    '001' AS payor_name, 
	                    'PA_MAW' AS contract_name,
                        ca.ServiceStartDate AS start_date,
	                    ca.ServiceEndDate AS end_date,
	                    ca.AuthorizedUnits AS units
                    FROM Consumers c
                    INNER JOIN
	                    ConsumerAuthorizations ca  ON c.Identifier = ca.identifier
                 ";

                Utility util = new Utility();
                ClientAuthorizationsFormat ccf = new ClientAuthorizationsFormat();
                DataTable ceDataTable = util.GetDataTable(selectQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareAuthorizationsWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    worksheet.Cells[currentRow, ccf.GetIndex("auth_original_id")].SetValue(dr["auth_original_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("client_id")].SetValue(dr["client_id"].ToString());

                    CellValueFormat payor_numberCellValueFormat = new CellValueFormat("000");
                    worksheet.Cells[currentRow, ccf.GetIndex("payor_name")].SetFormat(payor_numberCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("payor_name")].SetValue(dr["payor_name"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("contract_name")].SetValue(dr["contract_name"].ToString());

                    CellValueFormat date1CellValueFormat = new CellValueFormat("mm/dd/yyyy");
                    worksheet.Cells[currentRow, ccf.GetIndex("start_date")].SetFormat(date1CellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("start_date")].SetValue(dr["start_date"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("end_date")].SetFormat(date1CellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("end_date")].SetValue(dr["end_date"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("units")].SetValue(dr["units"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("authorization_number")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("authorization_type")].SetValue("Internal Use Only ");
                    worksheet.Cells[currentRow, ccf.GetIndex("authorization_type_code")].SetValue("IU");
                    worksheet.Cells[currentRow, ccf.GetIndex("service_bundle_name")].SetValue(" ");
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
        }

        public void CreateServiceAuthorizationsWorkbook(Worksheet worksheet)
        {
            try
            {
                string selectQuery =
                $@"
                    SELECT '1' AS auth_details_original_id,ROW_NUMBER() OVER(ORDER BY c.consumer_internal_number) auth_original_id,
                        'AWC' AS program_code, '00014' AS service_facility_code,'PA_MAW' AS profile_type,ca.ProcedureCode AS rate_code,
                        ca.RemainingUnits AS units,ca.ServiceStartDate AS date_from,ca.ServiceEndDate AS date_to
                    FROM ConsumerAuthorizations AS ca
                    INNER JOIN
	                    Consumers AS c ON ca.Identifier = c.identifier
                    ORDER BY c.consumer_internal_number
                 ";

                Utility util = new Utility();
                ClientServiceAuthorizationsFormat ccf = new ClientServiceAuthorizationsFormat();
                DataTable ceDataTable = util.GetDataTable(selectQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareServiceAuthorizationsWorksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in ceDataTable.Rows)
                {
                    worksheet.Cells[currentRow, ccf.GetIndex("auth_details_original_id")].SetValue(dr["auth_details_original_id"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("auth_original_id")].SetValue(dr["auth_original_id"].ToString());

                    CellValueFormat sfc_numberCellValueFormat = new CellValueFormat("00000");
                    worksheet.Cells[currentRow, ccf.GetIndex("service_facility_code")].SetFormat(sfc_numberCellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("service_facility_code")].SetValue(dr["service_facility_code"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("profile_type")].SetValue(dr["profile_type"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("rate_code")].SetValue(dr["rate_code"].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("units")].SetValue(dr["units"].ToString());

                    CellValueFormat date1CellValueFormat = new CellValueFormat("mm/dd/yyyy");
                    worksheet.Cells[currentRow, ccf.GetIndex("date_from")].SetFormat(date1CellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("date_from")].SetValue(dr["date_from"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("date_to")].SetFormat(date1CellValueFormat);
                    worksheet.Cells[currentRow, ccf.GetIndex("date_to")].SetValue(dr["date_to"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("staff_id")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("auth_number_override")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("is_override")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("override_rate_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("billing_code")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("amount_charged")].SetValue(" ");
                    worksheet.Cells[currentRow, ccf.GetIndex("copay_amount")].SetValue(" ");

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

        private void PrepareEnrollmentWorksheet(Worksheet worksheet)
        {
            try
            {
                ClientEnrollmentFormat ccf = new ClientEnrollmentFormat();
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

        private void PrepareStaffAssignmentsWorksheet(Worksheet worksheet)
        {
            try
            {
                ClientStaffAssignmentsFormat ccf = new ClientStaffAssignmentsFormat();
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

        private void PrepareAuthorizationsWorksheet(Worksheet worksheet)
        {
            try
            {
                ClientAuthorizationsFormat ccf = new ClientAuthorizationsFormat();
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

        private void PrepareServiceAuthorizationsWorksheet(Worksheet worksheet)
        {
            try
            {
                ClientServiceAuthorizationsFormat ccf = new ClientServiceAuthorizationsFormat();
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