using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ClientBenefitsFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class ClientBenefitsColumn
        {
            public ClientBenefitsColumn(bool inc, string name)
            {
                Include = inc;
                Name = name;
            }

            public bool Include { get; set; }
            public string Name { get; set; }
        }

        Dictionary<int, ClientBenefitsColumn> _columnNameList = new Dictionary<int, ClientBenefitsColumn>();

        public string[] ColumnStrings;

        public ClientBenefitsFormat()
        {
            var columnNameList = new List<string>() 
            {
                "client_id",
                "payor_name",
                "contract_name",
                "co_pay_amount",
                "date_start",
                "date_end",
                "is_self_pay",
                "billing_sequence",
                "priority",
                "policy_number",
                "group_number",
                "client_link_id",
                "relationship",
                "relationship_code",
                "is_medicaid_additional",
                "is_deduction",
                "is_deduction_percentage",
                "deduction_percent",
                "deduction_action_code",
                "original_table_name"
             };

            int index = 0;
            foreach (string cname in columnNameList)
            {
                _columnNameList.Add(index++, new ClientBenefitsColumn(true, cname));
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