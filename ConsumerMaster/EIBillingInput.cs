using System;
using FileHelpers;

namespace ConsumerMaster
{
    [DelimitedRecord("\t")]
    public class EIBillingInput
    {
        public string Therapists;
        [FieldConverter(ConverterKind.Date, "MM/dd/yy")]
        public DateTime BillDate;
        public string LastName;
        public string FirstName;
        public string County;
        public string FundingSource;
        public string VisitType;
        public string Discipline;
        public string StartTime;
        public string EndTime;
        public int TotalMinutes;
        public int TotalUnits;
        public int? TravelTime;
        public string CollOfficeTime;
    }
}