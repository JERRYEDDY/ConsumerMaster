
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
                        c.consumer_internal_number AS client_id, c.consumer_last AS last_name, c.consumer_first AS first_name, ' ' AS middle_name
                        , c.date_of_birth AS date_of_birth, c.address_line_1 AS street_address_1, ISNULL(c.address_line_2, ' ') AS street_address_2
                        ,c.city AS city, states.name AS state, c.state AS state_code, c.zip_code AS zip_code, c.gender AS gender_code, ' ' AS day_phone, ' ' AS evening_phone
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
                    worksheet.Cells[currentRow, ccf.GetIndex("")].SetValue(dr[""].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("")].SetValue(dr[""].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("")].SetValue(dr[""].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("")].SetValue(dr[""].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("")].SetValue(dr[""].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("")].SetValue(dr[""].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("")].SetValue(dr[""].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("")].SetValue(dr[""].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("")].SetValue(dr[""].ToString());
                    worksheet.Cells[currentRow, ccf.GetIndex("")].SetValue(dr[""].ToString());



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