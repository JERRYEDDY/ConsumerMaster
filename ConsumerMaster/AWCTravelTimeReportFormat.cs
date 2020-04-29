using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class AWCTravelTimeReportFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class AWCTravelTimeReportColumn
        {
            public bool Include { get; set; }
            public string Name { get; set; }
        }

        private readonly Dictionary<int, AWCTravelTimeReportColumn> _columnNameList = new Dictionary<int, AWCTravelTimeReportColumn>
        {
            {0, new AWCTravelTimeReportColumn {Include=true,Name="StaffID"} },
            {1, new AWCTravelTimeReportColumn {Include=true,Name="StaffName"} },
            {2, new AWCTravelTimeReportColumn {Include=true,Name="ClientID"} },
            {3, new AWCTravelTimeReportColumn {Include=true,Name="ClientName"} },
            {4, new AWCTravelTimeReportColumn {Include=true,Name="Start"} },
            {5, new AWCTravelTimeReportColumn {Include=true,Name="Finish"} },
            {6, new AWCTravelTimeReportColumn {Include=true,Name="Duration"} },
        };

        public string[] ColumnStrings;

        public AWCTravelTimeReportFormat()
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