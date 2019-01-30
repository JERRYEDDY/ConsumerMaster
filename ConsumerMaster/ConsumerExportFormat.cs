using FileHelpers;
using System.Reflection;
using System;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ConsumerExportFormat
    {
        public string consumer_internal_number { get; set; }
        public string trading_partner_string { get; set; }
        public string consumer_first { get; set; }
        public string consumer_last { get; set; }
        public string date_of_birth { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string identifier { get; set; }
        public string gender { get; set; }

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