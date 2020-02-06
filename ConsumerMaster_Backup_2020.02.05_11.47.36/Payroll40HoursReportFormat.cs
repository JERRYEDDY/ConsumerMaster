using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;


namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class Payroll40HoursReportFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class Payroll40HoursReportExportColumn
        {
            public bool Include { get; set; }
            public string Name { get; set; }
        }

        private readonly Dictionary<int, Payroll40HoursReportExportColumn> _columnNameList = new Dictionary<int, Payroll40HoursReportExportColumn>
        {
            {0, new Payroll40HoursReportExportColumn {Include=true,Name="ID"} },
            {1, new Payroll40HoursReportExportColumn {Include=true,Name="Name"} },
            {2, new Payroll40HoursReportExportColumn {Include=true,Name="Hours"} },
        };

        public string[] ColumnStrings;
        private readonly bool _includeHours = false;

        public Payroll40HoursReportFormat()
        {
            ColumnStrings = GetColumns();
        }

        public Payroll40HoursReportFormat(bool include)
        {
            _includeHours = include;
            ColumnStrings = GetColumns();
        }

        private string[] GetColumns()
        {
            StringCollection cols = new StringCollection();
            try
            {
                foreach (var column in _columnNameList)
                {
                    if (_includeHours)
                    {
                        cols.Add(column.Value.Name);
                    }
                    else
                    {
                        if (column.Value.Include)
                            cols.Add(column.Value.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return cols.Cast<string>().ToArray();
        }

        public int GetIndex(string value)
        {
            return Array.IndexOf(ColumnStrings, value);
        }
    }
}