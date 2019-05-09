using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class EISIServiceExportFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class ServiceExportColumn
        {
            public bool Include { get; set; }
            public string Name { get; set; }
        }

        private readonly Dictionary<int, ServiceExportColumn> _columnNameList = new Dictionary<int, ServiceExportColumn>
        {
            {0, new ServiceExportColumn {Include=true,Name="consumer_first"} },
            {1, new ServiceExportColumn {Include=true,Name="consumer_last"} },
            {2, new ServiceExportColumn {Include=true,Name="consumer_internal_number"} },
            {3, new ServiceExportColumn {Include=true,Name="diagnosis_code_1_code"} },
            {4, new ServiceExportColumn {Include=true,Name="trading_partner_string"} },
            {5, new ServiceExportColumn {Include=true,Name="trading_partner_program_string"} },
            {6, new ServiceExportColumn {Include=true,Name="start_date_string"} },
            {7, new ServiceExportColumn {Include=true,Name="end_date_string"} },
            {8, new ServiceExportColumn {Include=true,Name="composite_procedure_code_string"} },
            {9, new ServiceExportColumn {Include=false,Name="hours"} },
            {10, new ServiceExportColumn {Include=true,Name="units"} },
            {11, new ServiceExportColumn {Include=true,Name="manual_billable_rate"} },
            {12, new ServiceExportColumn {Include=true,Name="prior_authorization_number"} },
            {13, new ServiceExportColumn {Include=true,Name="referral_number"} },
            {14, new ServiceExportColumn {Include=true,Name="referring_provider_id"} },
            {15, new ServiceExportColumn {Include=true,Name="referring_provider_first_name"} },
            {16, new ServiceExportColumn {Include=true,Name="referring_provider_last_name"} },
            {17, new ServiceExportColumn {Include=true,Name="renderingnames"} },
            {18, new ServiceExportColumn {Include=true,Name="rendering_provider_id"} },
            {19, new ServiceExportColumn {Include=true,Name="rendering_provider_secondary_id"} },
            {20, new ServiceExportColumn {Include=true,Name="rendering_provider_first_name"} },
            {21, new ServiceExportColumn {Include=true,Name="rendering_provider_last_name"} },
            {22, new ServiceExportColumn {Include=true,Name="rendering_provider_taxonomy_code"} },
            {23, new ServiceExportColumn {Include=true,Name="billing_note"} },

        };

        public string[] ColumnStrings;
        private readonly bool _includeHours = false;

        public EISIServiceExportFormat()
        {
            ColumnStrings = GetColumns();
        }

        public EISIServiceExportFormat(bool include)
        {
            this._includeHours = include;
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