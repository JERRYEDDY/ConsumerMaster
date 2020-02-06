using FileHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ConsumerMaster
{
    [DelimitedRecord(",")]
    public class ClientDiagnosisFormat
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        class ClientDiagnosisColumn
        {
            public ClientDiagnosisColumn(bool inc, string name)
            {
                Include = inc;
                Name = name;
            }

            public bool Include { get; set; }
            public string Name { get; set; }
        }

        Dictionary<int, ClientDiagnosisColumn> _columnNameList = new Dictionary<int, ClientDiagnosisColumn>();

        public string[] ColumnStrings;

        public ClientDiagnosisFormat()
        {
            var columnNameList = new List<string>() 
            {
                "diagnosis_id",
                "client_id",
                "diagnosis",
                "diagnosis_code",
                "date_diagnosed",
                "is_primary",
                "dsm_IV_code",
                "dsm_V_code",
                "icd9_code",
                "icd10_code",
                "rule_out",
                "gaf_score",
                "priority",
                "staff_id",
                "original_table_name"
             };

            int index = 0;
            foreach (string cname in columnNameList)
            {
                _columnNameList.Add(index++, new ClientDiagnosisColumn(true, cname));
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