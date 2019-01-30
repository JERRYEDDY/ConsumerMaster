using FileHelpers;
using System.Reflection;
using System;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ServiceExportFormat
    {
        public string consumer_first { get; set; }
        public string consumer_last { get; set; }
        public string consumer_internal_number { get; set; }
        public string diagnosis_code_1_code { get; set; }
        public string trading_partner_string { get; set; }
        public string trading_partner_program_string { get; set; }
        public string start_date_string { get; set; }
        public string end_date_string { get; set; }
        public string composite_procedure_code_string { get; set; }
        public string units { get; set; }
        public string manual_billable_rate { get; set; }
        public string prior_authorization_number { get; set; }
        public string referral_number { get; set; }
        public string referring_provider_id { get; set; }
        public string referring_provider_first_name { get; set; }
        public string referring_provider_last_name { get; set; }
        public string rendering_provider_id { get; set; }
        public string rendering_provider_first_name { get; set; }
        public string rendering_provider_last_name { get; set; }
        public string billing_note { get; set; }

        public string[] GetColumns()
        {
            int index = 0;
            string[] columnsList = new string[this.GetType().GetProperties().Length]    ;
            foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
            {
                columnsList[index] = propertyInfo.Name;
                index++;
            }
            return columnsList;
        }

        public int GetKey(string value)
        {
            string[] columnList = this.GetColumns();
            return Array.IndexOf(columnList, value);
        }
    }
}