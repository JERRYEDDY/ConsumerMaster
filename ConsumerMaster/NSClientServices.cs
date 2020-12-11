using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerMaster
{
    public class NSClientServices
    {
        public string people_id { get; set; }
        public string full_name { get; set; }
        public string id_no { get; set; }
        public string other_id_number { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string gender_code { get; set; }
        public string ssn_number { get; set; }
        public string is_staff { get; set; }
        public DateTime? intake_dt { get; set; }
        public string discharge_dt { get; set; }
        public string medicaid_number { get; set; }
        public string ipd { get; set; }
        public string current_location { get; set; }
        public string program_info_id { get; set; }
        public string program_name { get; set; }
        public string program_type { get; set; }
        public string site_providing_service { get; set; }
        public string facility { get; set; }
        public string license_number { get; set; }
        public string staff_id { get; set; }
        public string job_title { get; set; }
        public string staff_name { get; set; }
        public DateTime? actual_date { get; set; }
        public DateTime? end_date { get; set; }
        public string duration { get; set; }
        public string event_log_id { get; set; }
        public string event_definition_id { get; set; }
        public string event_name { get; set; }
        public string parent_event { get; set; }
        public string service { get; set; }
        public string activity_type { get; set; }
        public string encounter_with { get; set; }
        public string is_client_involved { get; set; }
        public string is_noshow { get; set; }
        public string is_locked { get; set; }
        public string progress_note { get; set; }
        public string event_category_id { get; set; }
        public string form_header_id { get; set; }
        public string is_billed { get; set; }
        public string is_paid { get; set; }
        public string invoice_number { get; set; }
        public DateTime? date_entered { get; set; }
        public string user_entered { get; set; }
        public string user_entered_name { get; set; }
        public string approved_date { get; set; }
        public string approved_by_id { get; set; }
        public string approved_staff_name { get; set; }
        public string submitted { get; set; }
        public string is_approved { get; set; }
        public string is_notapproved { get; set; }
        public string is_notapproved_subm { get; set; }
        public string depended_activity { get; set; }
        public string program_unit_description { get; set; }
        public string sc_code { get; set; }
        public string duration_num { get; set; }
        public string do_not_bill { get; set; }
        public string do_not_pay { get; set; }
        public string general_location_id { get; set; }
        public string general_location { get; set; }
        public string program_modifier_id { get; set; }
        public string program_modifier { get; set; }
        public string program_modifier_code { get; set; }
        public string NormalWorkHours { get; set; }
        public string duration_other_num { get; set; }
        public string duration_other { get; set; }
        public string travel_time_num { get; set; }
        public string travel_time { get; set; }
        public string planning_time_num { get; set; }
        public string planning_time { get; set; }
        public string total_duration_num { get; set; }
        public string total_duration { get; set; }
        public string total_duration_num_cc { get; set; }
        public string total_duration_cc { get; set; }
        public string actual_location_facility_id { get; set; }
        public string actual_location_facility { get; set; }
        public string reason_for_no_show_id { get; set; }
        public string reason_for_no_show { get; set; }
        public string form_program { get; set; }
        public string cue_number { get; set; }
        public string cue_type { get; set; }
        public string client_participation_response { get; set; }
        public string client_part_resp_description { get; set; }
        public string outcome_id { get; set; }
        public string outcome_description { get; set; }
        public string gaf_score_current { get; set; }
        public DateTime? date_order { get; set; }
        public string is_billable { get; set; }
        public string snb_reasons { get; set; }
        public string is_billing { get; set; }
        public string sr_incident_to { get; set; }
        public string is_incident_to { get; set; }
        public string medicare_incident_to_supervisor { get; set; }
        public string it_supervisor_name { get; set; }
        public DateTime? whole_date_order { get; set; }
        public DateTime? serv_entry_actual_date { get; set; }
        public string sort_order { get; set; }
        public string conversion_id { get; set; }
    }
}