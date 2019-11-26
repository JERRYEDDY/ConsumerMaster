using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ClientStaffAssignmentsFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class ClientStaffAssignmentsColumn
        {
            public ClientStaffAssignmentsColumn(bool inc, string name)
            {
                Include = inc;
                Name = name;
            }

            public bool Include { get; set; }
            public string Name { get; set; }
        }

        Dictionary<int, ClientStaffAssignmentsColumn> _columnNameList = new Dictionary<int, ClientStaffAssignmentsColumn>();

        public string[] ColumnStrings;

        public ClientStaffAssignmentsFormat()
        {
            var columnNameList = new List<string>() 
            {
                "client_id",
                "staff_id",
                "program_code",
                "start_date",
                "role",
                "role_code",
                "end_date",
                "is_primary",
                "original_table_name"
             };

            int index = 0;
            foreach (string cname in columnNameList)
            {
                _columnNameList.Add(index++, new ClientStaffAssignmentsColumn(true, cname));
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