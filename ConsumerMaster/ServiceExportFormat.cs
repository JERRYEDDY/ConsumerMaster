using FileHelpers;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ServiceExportFormat
    {
        //public string consumer_first { get; set; }
        //public string consumer_last { get; set; }
        //public string consumer_internal_number { get; set; }
        //public string diagnosis_code_1_code { get; set; }
        //public string trading_partner_string { get; set; }
        //public string trading_partner_program_string { get; set; }
        //public string start_date_string { get; set; }
        //public string end_date_string { get; set; }
        //public string composite_procedure_code_string { get; set; }
        //public string units { get; set; }
        //public string manual_billable_rate { get; set; }
        //public string prior_authorization_number { get; set; }
        //public string referral_number { get; set; }
        //public string referring_provider_id { get; set; }
        //public string referring_provider_first_name { get; set; }
        //public string referring_provider_last_name { get; set; }
        //public string rendering_provider_id { get; set; }
        //public string rendering_provider_first_name { get; set; }
        //public string rendering_provider_last_name { get; set; }
        //public string billing_note { get; set; }
        //public string rendering_provider_secondary_id { get; set; }

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
            {13, new ServiceExportColumn {Include=true,Name="referring_provider_id"} },
            {14, new ServiceExportColumn {Include=true,Name="referring_provider_first_name"} },
            {15, new ServiceExportColumn {Include=true,Name="referring_provider_last_name"} },
            {16, new ServiceExportColumn {Include=true,Name="rendering_provider_id"} },
            {17, new ServiceExportColumn {Include=true,Name="rendering_provider_first_name"} },
            {18, new ServiceExportColumn {Include=true,Name="rendering_provider_last_name"} },
            {19, new ServiceExportColumn {Include=true,Name="billing_note"} },
            {20, new ServiceExportColumn {Include=true,Name="rendering_provider_secondary_id"} },
        };

        readonly StringCollection _cols = new StringCollection();
        private readonly bool _includeHours = false;

        public ServiceExportFormat()
        {

        }

        public ServiceExportFormat(bool include)
        {
            this._includeHours = include;
        }

        public string[] GetColumns()
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
            return _cols.Cast<string>().ToArray();
        }

        public int GetKey(string value)
        {
            string[] columnList = this.GetColumns();
            return Array.IndexOf(columnList, value);
        }

        //public string[] GetColumns()
        //{
        //    int index = 0;
        //    string[] columnsList = new string[this.GetType().GetProperties().Length]    ;
        //    foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
        //    {
        //        columnsList[index] = propertyInfo.Name;
        //        index++;
        //    }
        //    return columnsList;
        //}

        //public int GetKey(string value)
        //{
        //    string[] columnList = this.GetColumns();
        //    return Array.IndexOf(columnList, value);
        //}
    }
}