using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ConsumerExportFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class ConsumerExportColumn
        {
            public bool Include { get; set; }
            public string Name { get; set; }
        }

        private readonly Dictionary<int, ConsumerExportColumn> _columnNameList = new Dictionary<int, ConsumerExportColumn>
        {
            {0, new ConsumerExportColumn {Include=true,Name="consumer_internal_number"} },
            {1, new ConsumerExportColumn {Include=true,Name="trading_partner_string"} },
            {2, new ConsumerExportColumn {Include=true,Name="consumer_first"} },
            {3, new ConsumerExportColumn {Include=true,Name="consumer_last"} },
            {4, new ConsumerExportColumn {Include=true,Name="date_of_birth"} },
            {5, new ConsumerExportColumn {Include=true,Name="address_line_1"} },
            {6, new ConsumerExportColumn {Include=true,Name="address_line_2"} },
            {7, new ConsumerExportColumn {Include=true,Name="city"} },
            {8, new ConsumerExportColumn {Include=true,Name="state"} },
            {9, new ConsumerExportColumn {Include=true,Name="zip_code"} },
            {10, new ConsumerExportColumn {Include=true,Name="identifier"} },
            {11, new ConsumerExportColumn {Include=true,Name="gender"} },
        };

        readonly StringCollection _cols = new StringCollection();

        public string[] GetColumns()
        {
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

        public int GetKey(string value)
        {
            string[] columnList = GetColumns();
            return Array.IndexOf(columnList, value);
        }
    }
}