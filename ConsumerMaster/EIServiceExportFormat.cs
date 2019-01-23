using FileHelpers;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class EIServiceExportFormat
    {
        public string consumer_first;
        public string consumer_last;
        public string consumer_internal_number;
        public string diagnosis_code_1_code;
        public string trading_partner_string;
        public string trading_partner_program_string;
        public string start_date_string;
        public string end_date_string;
        public string composite_procedure_code_string;
        public string units;
        public string manual_billable_rate;
        public string prior_authorization_number;
        public string referral_number;
        public string referring_provider_id;
        public string referring_provider_first_name;
        public string referring_provider_last_name;
        public string rendering_provider_id;
        public string rendering_provider_first_name;
        public string rendering_provider_last_name;
        public string billing_note;
    }
}