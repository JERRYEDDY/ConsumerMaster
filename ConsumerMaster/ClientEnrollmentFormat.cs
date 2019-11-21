using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ClientEnrollmentFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class ClientEnrollmentColumn
        {
            public ClientEnrollmentColumn(bool inc, string name)
            {
                Include = inc;
                Name = name;
            }

            public bool Include { get; set; }
            public string Name { get; set; }
        }

        Dictionary<int, ClientEnrollmentColumn> _columnNameList = new Dictionary<int, ClientEnrollmentColumn>();

        public string[] ColumnStrings;

        public ClientEnrollmentFormat()
        {
            var columnNameList = new List<string>()
            {
                "client_id",
                "program_code",
                "service_facility_code",
                "foster_home_id",
                "room_number",
                "enrollment_id",
                "start_date",
                "end_date",
                "is_planned_discharge",
                "outcome",
                "outcome_code",
                "closing_reason",
                "closing_reason_code",
                "overall_discharge_date",
                "org_id",
                "referral_date",
                "group_id",
                "unit_id",
                "schedule_id",
                "referral_id",
                "referral_reason",
                "referral_reason_code",
                "discharged_to_type",
                "discharged_to_type_code",
                "discharge_to_other",
                "inquiry_type",
                "inquiry_type_code",
                "marketing_info",
                "marketing_info_code",
                "remarks",
                "placed_by",
                "placed_by_code",
                "transfer_type",
                "transfer_type_code",
                "opening_reasons",
                "opening_reasons_code",
                "condition_at_discharge",
                "condition_at_discharge_code",
                "original_table_name"
             };


            int index = 0;
            foreach (string cname in columnNameList)
            {
                _columnNameList.Add(index++, new ClientEnrollmentColumn(true, cname));
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