using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ClientServiceAuthorizationsFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class ClientServiceAuthorizationsColumn
        {
            public ClientServiceAuthorizationsColumn(bool inc, string name)
            {
                Include = inc;
                Name = name;
            }

            public bool Include { get; set; }
            public string Name { get; set; }
        }

        Dictionary<int, ClientServiceAuthorizationsColumn> _columnNameList = new Dictionary<int, ClientServiceAuthorizationsColumn>();

        public string[] ColumnStrings;

        public ClientServiceAuthorizationsFormat()
        {
            var columnNameList = new List<string>() 
            {
                "auth_details_original_id",
                "auth_original_id",
                "program_code",
                "service_facility_code",
                "profile_type",
                "rate_code",
                "units",
                "date_from",
                "date_to",
                "staff_id",
                "auth_number_override",
                "is_override",
                "override_rate_code",
                "billing_code",
                "amount_charged",
                "copay_amount"
             };

            int index = 0;
            foreach (string cname in columnNameList)
            {
                _columnNameList.Add(index++, new ClientServiceAuthorizationsColumn(true, cname));
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