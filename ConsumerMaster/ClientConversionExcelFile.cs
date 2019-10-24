
using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;

namespace ConsumerMaster
{
    public class ClientConversionExcelFile
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
                    SELECT 
                        c.consumer_internal_number AS client_id, c.consumer_last AS last_name, c.consumer_first AS first_name, ' ' AS middle_name,

                        ' ' AS gender, c.gender AS gender_code, c.date_of_birth AS date_of_birth, ' ' AS ss_number, ' ' AS driver_license_number,

                        ' ' AS city_of_birth, ' ' AS state_of_birth, ' ' AS state_of_birth_code, ' ' AS country_of_birth, c.address_line_1 AS street_address_1, 

                        ISNULL(c.address_line_2, ' ') AS street_address_2, c.city AS city, states.name AS state, c.state AS state_code, c.zip_code AS zip_code,  


' ' AS address_effective_date,
' ' AS  religion,
 ' ' AS religion_code,
 ' ' AS citizenship,
 ' ' AS citizenship_code,
 ' ' AS marital_status,
 ' ' AS marital_status_code,
 ' ' AS ethnicity,
 ' ' AS ethnicity_code,
 ' ' AS primary_language,
 ' ' AS primary_language_code,
 secondary_language,
 secondary_language_code,
 day_phone,
 evening_phone,
 mobile_phone,
 pager,
 email_address,
 race_1,
 race_1_code,
 race_1_other_description,
 race_2,
 race_2_code,
 race_2_other_description,
 curr_employment_name,
 curr_employment_business,
 curr_employment_position,
 curr_employment_status,
 curr_employment_status_code,
 curr_employment_phone,
 curr_employment_start_date,
 education_degree,
 education_degree_code,
 education_highest_grade,
 education_highest_grade_code,
 urn_no,
 county,
 county_code,
 salutation,
 salutation_code,
 fax_number,
 school_attended,
 school_attended_code,
 TABS_ID,
 education_id,
 residence_type,
 residence_type_code,
 name_suffix,
 agency_id_no,
 agency_other_id_number,
 other_id_no,
 hair_color,
 hair_color_code,
 eye_color,
 eye_color_code,
 aka,
 maiden_name,
 ethnicity_details,
 ethnicity_details_code,
 original_table_name,
 ssn_unknown,
 ssn_no,
 mothers_first_name,
 veteran_status,
 veteran_status_code,
 region,
 planning_area,
 geocode,
 twn_ca_name,
 address_type,
 address_type_code,
 agency_name,






' ' AS day_phone, ' ' AS evening_phone

                        , ' ' AS mobile_phone, ' ' AS email_address, ' ' AS curr_employment_name, ' ' AS curr_employment_business,' ' AS curr_employment_position, ' ' AS curr_employment_status

                        , ' ' AS curr_employment_status_code, ' ' AS urr_employment_phone, ' ' AS curr_employment_start_date, ' ' AS education_degree, ' ' AS education_degree_code

                        , ' ' AS education_highest_grade, tp.symbol AS trading_partner_string, c.identifier AS identifier, c.diagnosis AS diagnosis_code 




                    FROM 
                        Consumers AS c
                    INNER JOIN 
                        TradingPartners AS tp ON c.trading_partner_id1 = tp.id 
                    INNER JOIN
                        States AS states ON c.state = states.Abbreviation
                    ORDER BY consumer_last
                 ";

                Utility util = new Utility();
                ClientConversionFormat ccf = new ClientConversionFormat();
                DataTable ceDataTable = util.GetDataTable(selectQuery);

                int totalConsumers = ceDataTable.Rows.Count;
                PrepareWorksheet(worksheet);

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


                    worksheet.Cells[currentRow, ccf.GetIndex("diagnosis_code")].SetValue(dr["diagnosis_code"].ToString());

                    worksheet.Cells[currentRow, ccf.GetIndex("identifier")].SetFormat(new CellValueFormat("0000000000"));
                    worksheet.Cells[currentRow, ccf.GetIndex("identifier")].SetValue(dr["identifier"].ToString());
                    //worksheet.Cells[currentRow, ccf.GetIndex("identifier")].SetValue("0000000011");

                    //worksheet.Cells[currentRow, ccf.GetIndex("trading_partner_string")].SetValue(dr["trading_partner_string"].ToString());

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

        private void PrepareWorksheet(Worksheet worksheet)
        {
            try
            {
                ClientConversionFormat ccf = new ClientConversionFormat();
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