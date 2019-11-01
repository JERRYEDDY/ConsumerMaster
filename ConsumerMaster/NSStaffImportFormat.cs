using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class NSStaffImportFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class NSStaffImportColumn
        {
            public NSStaffImportColumn(bool inc, string name)
            {
                Include = inc;
                Name = name;
            }

            public bool Include { get; set; }
            public string Name { get; set; }
        }

        Dictionary<int, NSStaffImportColumn> _columnNameList = new Dictionary<int, NSStaffImportColumn>();

        public string[] ColumnStrings;

        public NSStaffImportFormat()
        {
            var columnNameList = new List<string>() 
            {
                "id_no",
                "last_name",
                "first_name",
                "middle_name",
                "dob",
                "ssn_number",
                "gender_code",
                "email_address",
                "npi_number",
                "job_title_code",
                "job_title",
                "race_code",
                "start_date",
                "end_date",
                "supervisor_id",
                "supervisor_win_username",
                "can_supervise",
                "security_scheme_code",
                "billing_staff_credentials_code",
                "credential_effective_date",
                "credential_expiration_date",
                "program_code",
                "managing_office_code",
                "win_domain",
                "win_username",
                "use_active_directory",
                "is_all_incidents",
                "group_name",
                "role_workgroup_code",
                "workgroup_start_date",
                "workgroup_end_date",
                "address_type",
                "residence_type",
                "city",
                "state",
                "zip_code",
                "street_address_1",
                "street_address_2",
                "county",
                "address_date"
             };

            int index = 0;
            foreach (string cname in columnNameList)
            {
                _columnNameList.Add(index++, new NSStaffImportColumn(true, cname));
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