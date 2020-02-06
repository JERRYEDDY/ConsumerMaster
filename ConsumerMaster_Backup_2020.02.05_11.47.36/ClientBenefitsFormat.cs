using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ClientAuthorizationsFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class ClientAuthorizationsColumn
        {
            public ClientAuthorizationsColumn(bool inc, string name)
            {
                Include = inc;
                Name = name;
            }

            public bool Include { get; set; }
            public string Name { get; set; }
        }

        Dictionary<int, ClientAuthorizationsColumn> _columnNameList = new Dictionary<int, ClientAuthorizationsColumn>();

        public string[] ColumnStrings;

        public ClientAuthorizationsFormat()
        {
            var columnNameList = new List<string>() 
            {
                "auth_original_id",
                "client_id",
                "payor_name",
                "contract_name",
                "start_date",
                "end_date",
                "units",
                "authorization_number",
                "authorization_type",
                "authorization_type_code",
                "service_bundle_name",
                "original_table_name"
             };

            int index = 0;
            foreach (string cname in columnNameList)
            {
                _columnNameList.Add(index++, new ClientAuthorizationsColumn(true, cname));
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