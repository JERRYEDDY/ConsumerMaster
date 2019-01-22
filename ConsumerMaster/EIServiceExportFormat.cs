using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ConsumerMaster
{
    public class EIServiceExportFormat
    {
        public string consumer_first { get; set; }
        public string consumer_last { get; set; }
        public string consumer_internal_number {get; set; }
        public string trading_partner_string {get; set; }
        public string trading_partner_program_string {get; set; }
        public string start_date_string {get; set; }
        public string end_date_string {get; set; }
        public string diagnosis_code_1_code {get; set; }
        public string composite_procedure_code_string {get; set; }
        public string units {get; set; }
        public string billing_note { get; set; }
        public string manual_billable_rate {get; set; }
        public string prior_authorization_number {get; set; }
        public string referral_number {get; set; }
        public string referring_provider_id {get; set; }
        public string referring_provider_first_name {get; set; }
        public string referring_provider_last_name {get; set; }
        public string rendering_provider_id {get; set; }
        public string rendering_provider_first_name {get; set; }
        public string rendering_provider_last_name {get; set; }

        public DataTable ObjectToData()
        {
            DataTable dt = new DataTable();

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            this.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(this, null);
                    dt.Columns.Add(f.Name, f.PropertyType);
                    dt.Rows[0][f.Name] = f.GetValue(this, null);
                }
                catch { }
            });
            return dt;
        }

        public Dictionary<int, string> ObjectToDictionary()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            int index = 0;

            this.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(this, null);
                    dictionary.Add(index++, f.Name);
                }
                catch { }
            });
            return dictionary;
        }

        public int DKey(string name)
        {
            Dictionary<int, string> dictionary = this.ObjectToDictionary();

            int index = dictionary.Values.ToList().IndexOf(name);
            return index;
        }
    }
}