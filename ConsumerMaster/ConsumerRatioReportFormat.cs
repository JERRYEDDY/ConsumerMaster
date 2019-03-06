using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ConsumerRatioReportFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class ConsumerRatioReportColumn
        {
            public bool Include { get; set; }
            public string Name { get; set; }
        }

        private readonly Dictionary<int, ConsumerRatioReportColumn> _columnNameList = new Dictionary<int, ConsumerRatioReportColumn>
        {
            {0, new ConsumerRatioReportColumn {Include=true,Name="Site"} },
            {1, new ConsumerRatioReportColumn {Include=true,Name="FullName"} },
            {2, new ConsumerRatioReportColumn {Include=true,Name="Ratio1"} },
            {3, new ConsumerRatioReportColumn {Include=true,Name="Ratio2"} },
            {4, new ConsumerRatioReportColumn {Include=true,Name="Units1"} },
            {5, new ConsumerRatioReportColumn {Include=true,Name="Units2"} },
            {6, new ConsumerRatioReportColumn {Include=true,Name="Total"} },
            {7, new ConsumerRatioReportColumn {Include=true,Name="Pct1"} },
            {8, new ConsumerRatioReportColumn {Include=true,Name="Pct2"} },
        };

        public string[] ColumnStrings;
        private readonly bool _includeHours = false;

        public ConsumerRatioReportFormat()
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
                    if (_includeHours)
                    {
                        _cols.Add(column.Value.Name);
                    }
                    else
                    {
                        if (column.Value.Include)
                            _cols.Add(column.Value.Name);
                    }
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