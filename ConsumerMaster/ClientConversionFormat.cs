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
            public bool Include { get; set; }
            public string Name { get; set; }
        }

        private readonly Dictionary<int, ClientConversionColumn> _columnNameList = new Dictionary<int, ClientConversionColumn>
        {
            {0, new ClientConversionColumn {Include=true,Name="client_id"} },
            {1, new ClientConversionColumn {Include=true,Name="last_name"} },
            {2, new ClientConversionColumn {Include=true,Name="first_name"} },
            {3, new ClientConversionColumn {Include=true,Name="middle_name"} },
            {4, new ClientConversionColumn {Include=true,Name="gender"} },
            {5, new ClientConversionColumn {Include=true,Name="gender_code"} },
            {6, new ClientConversionColumn {Include=true,Name="date_of_birth"} },
            {7, new ClientConversionColumn {Include=true,Name="street_address_1"} },
            {8, new ClientConversionColumn {Include=true,Name="street_address_2"} },
            {9, new ClientConversionColumn {Include=true,Name="city"} },
            {10, new ClientConversionColumn {Include=true,Name="state"} },
            {11, new ClientConversionColumn {Include=true,Name="state_code"} },
            {12, new ClientConversionColumn {Include=true,Name="zip_code"} },
            {13, new ClientConversionColumn {Include=true,Name="day_phone"} },
            {14, new ClientConversionColumn {Include=true,Name="evening_phone"} },
            {15, new ClientConversionColumn {Include=true,Name="mobile_phone"} },
            {16, new ClientConversionColumn {Include=true,Name="email_address"} },
            {17, new ClientConversionColumn {Include=true,Name="curr_employment_name"} },
            {18, new ClientConversionColumn {Include=true,Name="curr_employment_business"} },
            {19, new ClientConversionColumn {Include=true,Name="curr_employment_position"} },
            {20, new ClientConversionColumn {Include=true,Name="curr_employment_status"} },
            {21, new ClientConversionColumn {Include=true,Name="curr_employment_status_code"} },
            {22, new ClientConversionColumn {Include=true,Name="curr_employment_phone"} },
            {23, new ClientConversionColumn {Include=true,Name="curr_employment_start_date"} },
            {24, new ClientConversionColumn {Include=true,Name="education_degree"} },
            {25, new ClientConversionColumn {Include=true,Name="education_degree_code"} },
            {26, new ClientConversionColumn {Include=true,Name="education_highest_grade"} },
            {27, new ClientConversionColumn {Include=true,Name="diagnosis_code"} },
            {28, new ClientConversionColumn {Include=true,Name="identifier"} },
        };

        public string[] ColumnStrings;

        public ClientConversionFormat()
        {
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