using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ClientConversionFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class ClientConversionColumn
        {
            public ClientConversionColumn(bool inc, string name)
            {
                Include = inc;
                Name = name;
            }

            public bool Include { get; set; }
            public string Name { get; set; }
        }

        Dictionary<int, ClientConversionColumn> _columnNameList = new Dictionary<int, ClientConversionColumn>();

        public string[] ColumnStrings;

        public ClientConversionFormat()
        {
            var columnNameList = new List<string>() {"client_id",
                                            "last_name",
                                            "first_name",
                                            "middle_name",
                                            "gender",
                                            "gender_code",
                                            "date_of_birth",
                                            "ss_number",
                                            "driver_license_number",
                                            "city_of_birth",
                                            "state_of_birth",
                                            "state_of_birth_code",
                                            "country_of_birth",
                                            "street_address_1",
                                            "street_address_2",
                                            "city",
                                            "state",
                                            "state_code",
                                            "zip_code",
                                            "address_effective_date",
                                            "religion",
                                            "religion_code",
                                            "citizenship",
                                            "citizenship_code",
                                            "marital_status",
                                            "marital_status_code",
                                            "ethnicity",
                                            "ethnicity_code",
                                            "primary_language",
                                            "primary_language_code",
                                            "secondary_language",
                                            "secondary_language_code",
                                            "day_phone",
                                            "evening_phone",
                                            "mobile_phone",
                                            "pager",
                                            "email_address",
                                            "race_1",
                                            "race_1_code",
                                            "race_1_other_description",
                                            "race_2",
                                            "race_2_code",
                                            "race_2_other_description",
                                            "curr_employment_name",
                                            "curr_employment_business",
                                            "curr_employment_position",
                                            "curr_employment_status",
                                            "curr_employment_status_code",
                                            "curr_employment_phone",
                                            "curr_employment_start_date",
                                            "education_degree",
                                            "education_degree_code",
                                            "education_highest_grade",
                                            "education_highest_grade_code",
                                            "urn_no",
                                            "county",
                                            "county_code",
                                            "salutation",
                                            "salutation_code",
                                            "fax_number",
                                            "school_attended",
                                            "school_attended_code",
                                            "TABS_ID",
                                            "education_id",
                                            "residence_type",
                                            "residence_type_code",
                                            "name_suffix",
                                            "agency_id_no",
                                            "agency_other_id_number",
                                            "other_id_no",
                                            "hair_color",
                                            "hair_color_code",
                                            "eye_color",
                                            "eye_color_code",
                                            "aka",
                                            "maiden_name",
                                            "ethnicity_details",
                                            "ethnicity_details_code",
                                            "original_table_name",
                                            "ssn_unknown",
                                            "ssn_no",
                                            "mothers_first_name",
                                            "veteran_status",
                                            "veteran_status_code",
                                            "region",
                                            "planning_area",
                                            "geocode",
                                            "twn_ca_name",
                                            "address_type",
                                            "address_type_code",
                                            "agency_name"
             };


            int index = 0;
            foreach (string cname in columnNameList)
            {
                _columnNameList.Add(index++, new ClientConversionColumn(true, cname));
            }

            ColumnStrings = GetColumns();

        }

        private string[] GetColumns()
        {
            StringCollection _cols = new StringCollection();
            try
            {
                foreach (var column in _columnNameList)
                {
                    _cols.Add(column.Value.Name);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return _cols.Cast<string>().ToArray();
        }

        public int GetIndex(string value)
        {
            return Array.IndexOf(ColumnStrings, value);
        }
    }
}