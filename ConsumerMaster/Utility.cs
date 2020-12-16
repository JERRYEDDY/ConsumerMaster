using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Csv;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;
using System.Text;
using System.CodeDom.Compiler;
using Newtonsoft.Json;
using System.Reflection;

namespace ConsumerMaster
{
    public class Utility
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public DataTable GetDataTable(string queryString)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString))
                {
                    sqlConnect.Open();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnect))
                    {

                        sqlDataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            }
        }

        public DataTable GetDataTable2(string queryString)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb2"].ConnectionString))
                {
                    sqlConnect.Open();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnect))
                    {

                        sqlDataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            }
        }

        public DataTable GetDataTable3(string queryString)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb3"].ConnectionString))
                {
                    sqlConnect.Open();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnect))
                    {

                        sqlDataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            }
        }

        public List<String> GetList(string queryString)
        {
            List<String> cpcData = new List<String>();
            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString))
                {
                    sqlConnect.Open();
                    using (SqlCommand command = new SqlCommand(queryString, sqlConnect))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cpcData.Add(reader.GetString(0));
                            }
                            return cpcData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return cpcData;
            };
        }

        public DataTable GetEmployeePersonnelDataTable(UploadedFile file)
        {
            String[] columns = new string[19] { 
                "P_ACTIVE", 
                "P_EMPNO ", 
                "P_FNAME ", 
                "P_LNAME ", 
                "P_MI ", 
                "P_BIRTH", 
                "P_SSN", 
                "P_SEX ", 
                "P_EMPEMAIL", 
                "P_JOBCODE ", 
                "P_JOBTITLE ", 
                "P_RACE ", 
                "P_LASTHIRE", 
                "P_HCITY", 
                "P_HSTATE", 
                "P_HZIP", 
                "P_HSTREET1", 
                "P_HSTREET2", 
                "P_HCOUNTY" };

            DataTable dataTable = new DataTable();

            try
            {
                using (TextReader reader = new StreamReader(file.InputStream))
                {
                    bool createColumns = true;

                    using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        csv.Configuration.HasHeaderRecord = false;

                        while (csv.Read())
                        {
                            if (createColumns)
                            {
                                if (columns.Length != csv.Context.Record.Length)
                                {
                                    Logger.Error("DataTable headers and data do not align");
                                    return dataTable;
                                }

                                for (int i = 0; i < csv.Context.Record.Length; i++)
                                {
                                    dataTable.Columns.Add(columns[i].ToString());
                                }
                                createColumns = false;
                            }

                            DataRow row = dataTable.NewRow();
                            for (int i = 0; i < csv.Context.Record.Length; i++)
                            {
                                row[i] = csv.Context.Record[i];
                            }
                            dataTable.Rows.Add(row);
                        }
                    }

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            };
        }

        public DataTable FromCSVFile(UploadedFile file)
        {
            DataTable dt;

            using (var reader = new StreamReader(file.InputStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Do any configuration to `CsvReader` before creating CsvDataReader.
                using (var dr = new CsvDataReader(csv))
                {
                    dt = new DataTable();
                    dt.Load(dr);
                }
            }
            return dt;
        }

        public DataTable GetEmployeePersonnelDataTable2(UploadedFile file)
        {
            String[] columns = new string[19] { 
                "P_ACTIVE",
                "P_EMPNO ",
                "P_FNAME ",
                "P_LNAME ",
                "P_MI ",
                "P_BIRTH",
                "P_SSN",
                "P_SEX ",
                "P_EMPEMAIL",
                "P_JOBCODE ",
                "P_JOBTITLE ",
                "P_RACE ",
                "P_LASTHIRE",
                "P_HCITY",
                "P_HSTATE",
                "P_HZIP",
                "P_HSTREET1",
                "P_HSTREET2",
                "P_HCOUNTY" };

            DataTable dataTable = new DataTable();

            try
            {
                using (var reader = new StreamReader(file.InputStream))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
                {
                    // Do any configuration to `CsvReader` before creating CsvDataReader.
                    using (var dr = new CsvDataReader(csv))
                    {
                        dataTable.Load(dr);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            };
        }

        public void WorksheetColumnHeaders(string[] columns, Worksheet worksheet)
        {
            ColumnSelection columnSelection = worksheet.Columns[0, columns.Count()];
            columnSelection.AutoFitWidth();
            int IndexRowItemStart = 0;

            foreach (string column in columns)
            {
                int columnKey = Array.IndexOf(columns, column);
                string columnName = column;

                worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
            }
        }

        public DataTable GetClientAddressDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[9]
            {
                new SPColumn("ClientID", typeof(string)),
                new SPColumn("ClientFirst", typeof(string)),
                new SPColumn("ClientLast", typeof(string)),
                new SPColumn("StreetAddress1", typeof(string)),
                new SPColumn("StreetAddress2", typeof(string)),
                new SPColumn("City", typeof(string)),
                new SPColumn("State", typeof(string)),
                new SPColumn("ZipCode", typeof(string)),
                new SPColumn("EmailAddress", typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;
                for (int i = 0; i < spc.Count(); i++)
                {
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[spc.Count()];
                    if (!string.IsNullOrEmpty(GetCellData(InputWorksheet, i, 0))) //Client ID
                    {
                        values[0] = GetCellData(InputWorksheet, i, 0); //Client ID
                        values[1] = GetCellData(InputWorksheet, i, 1); //Client First
                        values[2] = GetCellData(InputWorksheet, i, 2); //Client Last
                        values[3] = GetCellData(InputWorksheet, i, 3); //Street Address1
                        values[4] = GetCellData(InputWorksheet, i, 4); //Street Address2
                        values[5] = GetCellData(InputWorksheet, i, 5); //City
                        values[6] = GetCellData(InputWorksheet, i, 6); //State
                        values[7] = GetCellData(InputWorksheet, i, 7); //Zip Code
                        values[8] = GetCellData(InputWorksheet, i, 8); //Email Address

                        dataTable.Rows.Add(values);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            };
        }

        public DataTable GetClientMemberDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[4]
            {
                new SPColumn("ClientID", typeof(string)),
                new SPColumn("MemberID", typeof(string)),
                new SPColumn("MemberName", typeof(string)),
                new SPColumn("MemberRole", typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;
                for (int i = 0; i < spc.Count(); i++)
                {
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[spc.Count()];
                    if (!string.IsNullOrEmpty(GetCellData(InputWorksheet, i, 0))) //Client ID
                    {
                        values[0] = GetCellData(InputWorksheet, i, 0);  //ClientID
                        values[1] = GetCellData(InputWorksheet, i, 2);  //MemberID
                        values[2] = GetCellData(InputWorksheet, i, 3);  //MemberName
                        values[3] = GetCellData(InputWorksheet, i, 4);  //MemberRole

                        dataTable.Rows.Add(values);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            };
        }

        public DataTable GetClientAuthorizationsDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[7]
            {
                new SPColumn("ClientID", typeof(string)),
                new SPColumn("From", typeof(string)),
                new SPColumn("To", typeof(string)),
                new SPColumn("Service", typeof(string)),
                new SPColumn("Total", typeof(string)),
                new SPColumn("Used", typeof(string)),
                new SPColumn("Balance", typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;
                for (int i = 0; i < spc.Count(); i++)
                {
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[spc.Count()];

                    if (!string.IsNullOrEmpty(GetCellData(InputWorksheet, i, 0))) //Client ID
                    {
                        values[0] = GetCellData(InputWorksheet, i, 0); //Client ID
                        values[1] = GetCellData(InputWorksheet, i, 2); //From
                        values[2] = GetCellData(InputWorksheet, i, 3); //To
                        values[3] = GetCellData(InputWorksheet, i, 4); //Service
                        values[4] = GetCellData(InputWorksheet, i, 5); //Total
                        values[5] = GetCellData(InputWorksheet, i, 6); //Used
                        values[6] = GetCellData(InputWorksheet, i, 7); //Balance

                        dataTable.Rows.Add(values);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            };
        }

        SPColumn[] GetSPColumns()
        {
            SPColumn[] spc = new SPColumn[22]
            {
                new SPColumn("Staff ID", typeof(string)),
                new SPColumn("Secondary Staff ID", typeof(string)),
                new SPColumn("Staff Name", typeof(string) ),
                new SPColumn("Activity ID", typeof(string)),
                new SPColumn("Activity Type", typeof(string)),
                new SPColumn("ID", typeof(string)),
                new SPColumn("Secondary ID", typeof(string)),
                new SPColumn("Name", typeof(string)),
                new SPColumn("Start", typeof(DateTime)),
                new SPColumn("Finish", typeof(DateTime)),
                new SPColumn("Duration", typeof(Int32)),
                new SPColumn("Travel Time", typeof(string)),
                new SPColumn("TSrc", typeof(string)),
                new SPColumn("Distance", typeof(string)),
                new SPColumn("DSrc", typeof(string)),
                new SPColumn("Phone", typeof(string)),
                new SPColumn("Billing Code", typeof(string)), //Billing Code
                new SPColumn("Payroll Code", typeof(string)), //Payroll Code
                new SPColumn("Service", typeof(string)),
                new SPColumn("On-call", typeof(string)),
                new SPColumn("Location", typeof(string)),
                new SPColumn("Discipline", typeof(string))
            };

            return spc;
        }

        public DataTable GetTimeAndDistanceDataTable(Stream input)
        {
            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;

                SPColumn[] spc = GetSPColumns();

                for (int i = 0; i < spc.Count(); i++)
                {
                    CellSelection selection = InputWorksheet.Cells[0, i];
                    var columnName = "Column" + (i + 1);
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    int vIndex = 0;

                    var values = new object[spc.Count()];
                    values[vIndex++] = GetCellData(InputWorksheet, i, 0); //Staff ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 1); //Secondary Staff ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 2); //Activity ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 3); //Secondary Staff ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 4); //Activity Type
                    values[vIndex++] = GetCellData(InputWorksheet, i, 5); //ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 6); //Secondary ID"

                    string name = GetCellData(InputWorksheet, i, 7);
                    values[vIndex++] = name.Replace("\"", ""); //Name

                    string combinedStart = GetCellData(InputWorksheet, i, 8) + " " + GetCellData(InputWorksheet, i, 9);
                    DateTime startDate = Convert.ToDateTime(combinedStart);
                    values[vIndex++] = startDate; //Start

                    string combinedFinish = GetCellData(InputWorksheet, i, 11) + " " + GetCellData(InputWorksheet, i, 12);
                    DateTime finishDate = Convert.ToDateTime(combinedFinish);
                    values[vIndex++] = finishDate; //Finish

                    string durationStr = GetCellData(InputWorksheet, i, 14);
                    values[vIndex++] = int.Parse(durationStr, NumberStyles.AllowThousands);  //Duration

                    values[vIndex++] = GetCellData(InputWorksheet, i, 15); //Travel Time
                    values[vIndex++] = GetCellData(InputWorksheet, i, 16); //TSrc
                    values[vIndex++] = GetCellData(InputWorksheet, i, 17); //Distance
                    values[vIndex++] = GetCellData(InputWorksheet, i, 18); //DSrc
                    values[vIndex++] = GetCellData(InputWorksheet, i, 19); //Phone

                    values[vIndex++] = GetCellData(InputWorksheet, i, 20); //Billing Code
                    values[vIndex++] = GetCellData(InputWorksheet, i, 21); //Payroll Code
                    values[vIndex++] = GetCellData(InputWorksheet, i, 22); //Service
                    values[vIndex++] = GetCellData(InputWorksheet, i, 23); //On-call
                    values[vIndex++] = GetCellData(InputWorksheet, i, 24); //Location
                    values[vIndex++] = GetCellData(InputWorksheet, i, 25); //Discipline

                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            };

            return dataTable;
        }

        public DataTable GetTimeAndDistanceDataTableViaCSV(Stream input)
        {
            SPColumn[] spc = GetSPColumns();
            DataTable dataTable = new DataTable();

            for (int i = 0; i < spc.Count(); i++)
            {
                dataTable.Columns.Add(spc[i].name, spc[i].type);
            }

            using (var reader = new StreamReader(input))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    int vIndex = 0;
                    var values = new object[spc.Count()];

                    values[vIndex++] = csv.GetField<string>(0); //Staff ID
                    values[vIndex++] = csv.GetField<string>(1); //Secondary Staff ID
                    values[vIndex++] = csv.GetField<string>(2); //Staff Name
                    values[vIndex++] = csv.GetField<string>(3); //Activity ID
                    values[vIndex++] = csv.GetField<string>(4); //Activity Type
                    values[vIndex++] = csv.GetField<string>(5); //ID
                    values[vIndex++] = csv.GetField<string>(6); //Secondary ID

                    string name = csv.GetField<string>(7); ;
                    values[vIndex++] = name.Replace("\"", ""); //Name

                    string start = csv.GetField<string>(8) + " " + csv.GetField<string>(9);
                    DateTime? startDate = TryParseDateTime(start);
                    values[vIndex++] = startDate; //Start

                    string finish = csv.GetField<string>(11) + " " + csv.GetField<string>(12);
                    DateTime? finishDate = TryParseDateTime(finish);
                    values[vIndex++] = finishDate; //Finish

                    if (Int32.TryParse(csv.GetField<string>(14), NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int duration))
                    {
                        values[vIndex++] = duration; //Duration
                    }
                    else
                    {
                        values[vIndex++] = 0;
                    }

                    values[vIndex++] = csv.GetField<string>(15); //Travel Time
                    values[vIndex++] = csv.GetField<string>(16); //TSrc
                    values[vIndex++] = csv.GetField<string>(17); //Distance
                    values[vIndex++] = csv.GetField<string>(18); //DSrc
                    values[vIndex++] = csv.GetField<string>(19); //Phone
                    values[vIndex++] = csv.GetField<string>(20); //Billing Code
                    values[vIndex++] = csv.GetField<string>(21); //Payroll Code
                    values[vIndex++] = csv.GetField<string>(22); //Service
                    values[vIndex++] = csv.GetField<string>(23); //On-call
                    values[vIndex++] = csv.GetField<string>(24); //Location
                    values[vIndex++] = csv.GetField<string>(25); //Discipline

                    dataTable.Rows.Add(values);
                }
            }
            return dataTable;
        }

        public DataTable GetUPVTDDataTableViaCSV(Stream input)
        {
            SPColumn[] spc = GetSPColumns();
            DataTable dataTable = new DataTable();

            for (int i = 0; i < spc.Count(); i++)
            {
                dataTable.Columns.Add(spc[i].name, spc[i].type);
            }

            using (var reader = new StreamReader(input))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    int vIndex = 0;

                    bool isUPV = csv.GetField<string>(4).Contains("UPV");
                    bool noBillingCode = string.IsNullOrEmpty(csv.GetField<string>(20));  //Billing Code
                    bool noPayrollCode = string.IsNullOrEmpty(csv.GetField<string>(21));  //Payroll Code
                    bool noService = string.IsNullOrEmpty(csv.GetField<string>(22));   //Service

                    if (!isUPV)
                        continue;

                    if (noBillingCode && noPayrollCode && noService)
                        continue;

                    var values = new object[spc.Count()];

                    values[vIndex++] = csv.GetField<string>(0); //Staff ID
                    values[vIndex++] = csv.GetField<string>(1); //Secondary Staff ID
                    values[vIndex++] = csv.GetField<string>(2); //Staff Name
                    values[vIndex++] = csv.GetField<string>(3); //Activity ID
                    values[vIndex++] = csv.GetField<string>(4); //Activity Type
                    values[vIndex++] = csv.GetField<string>(5); //ID
                    values[vIndex++] = csv.GetField<string>(6); //Secondary ID

                    string name = csv.GetField<string>(7); ;
                    values[vIndex++] = name.Replace("\"", ""); //Name

                    string start = csv.GetField<string>(8) + " " + csv.GetField<string>(9);
                    DateTime? startDate = TryParseDateTime(start);
                    values[vIndex++] = startDate; //Start

                    string finish = csv.GetField<string>(11) + " " + csv.GetField<string>(12);
                    DateTime? finishDate = TryParseDateTime(finish);
                    values[vIndex++] = finishDate; //Finish

                    if (Int32.TryParse(csv.GetField<string>(14), NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int duration))
                    {
                        values[vIndex++] = duration; //Duration
                    }
                    else
                    {
                        values[vIndex++] = 0;
                    }

                    values[vIndex++] = csv.GetField<string>(15); //Travel Time
                    values[vIndex++] = csv.GetField<string>(16); //TSrc
                    values[vIndex++] = csv.GetField<string>(17); //Distance
                    values[vIndex++] = csv.GetField<string>(18); //DSrc
                    values[vIndex++] = csv.GetField<string>(19); //Phone
                    values[vIndex++] = csv.GetField<string>(20); //Billing Code
                    values[vIndex++] = csv.GetField<string>(21); //Payroll Code
                    values[vIndex++] = csv.GetField<string>(22); //Service
                    values[vIndex++] = csv.GetField<string>(23); //On-call
                    values[vIndex++] = csv.GetField<string>(24); //Location
                    values[vIndex++] = csv.GetField<string>(25); //Discipline

                    dataTable.Rows.Add(values);
                }
            }
            return dataTable;
        }

        public DataTable GetScheduledActualDataTableViaCSV(Stream input)
        {
            SPColumn[] spc = new SPColumn[4]
            {
                new SPColumn("ID", typeof(string)),
                new SPColumn("Name", typeof(string)),
                new SPColumn("StaffName", typeof(string)),
                new SPColumn("ActivitySource", typeof(string)),
            };

            DataTable dataTable = new DataTable();
            for (int i = 0; i < spc.Count(); i++)
            {
                dataTable.Columns.Add(spc[i].name, spc[i].type);
            }

            using (var reader = new StreamReader(input))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    int vIndex = 0;
                    var values = new object[spc.Count()];

                    values[vIndex++] = csv.GetField<string>(6); //ID
                    values[vIndex++] = csv.GetField<string>(8).Replace("\"", ""); //Name
                    values[vIndex++] = csv.GetField<string>(2); //Staff Name
                    values[vIndex++] = csv.GetField<string>(5); //Activity Source

                    if (String.IsNullOrEmpty(csv.GetField<string>(5))) //Ignore blank in ActivitySource column
                        continue;

                    dataTable.Rows.Add(values);
                }
            }

            return dataTable;
        }

        public static DateTime? TryParseDateTime(string text) =>  DateTime.TryParse(text, out var date) ? date : (DateTime?)null;
        public static TimeSpan? TryParseTimeSpan(string text) => TimeSpan.TryParse(text, out var timespan) ? timespan : (TimeSpan?)null;

        public DataTable GetScheduledActualDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[18]
            {
                new SPColumn("Staff ID", typeof(string)),
                new SPColumn("Secondary Staff ID", typeof(string)),
                new SPColumn("Staff Name", typeof(string) ),
                new SPColumn("Activity ID", typeof(string)),
                new SPColumn("Activity Type", typeof(string)),
                new SPColumn("Activity Source", typeof(string)),
                new SPColumn("ID", typeof(string)),
                new SPColumn("Secondary ID", typeof(string)),
                new SPColumn("Name", typeof(string)),
                new SPColumn("Scheduled Start", typeof(DateTime?)),
                new SPColumn("Scheduled Finish", typeof(DateTime?)),
                new SPColumn("SDuration", typeof(TimeSpan)),
                new SPColumn("Actual Start", typeof(DateTime?)),
                new SPColumn("Actual Finish", typeof(DateTime?)), 
                new SPColumn("ADuration", typeof(TimeSpan)),
                new SPColumn("Difference", typeof(string)),
                new SPColumn("Location", typeof(string)),
                new SPColumn("Discipline", typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    int vIndex = 0;

                    var values = new object[spc.Count()];
                    values[vIndex++] = GetCellData(InputWorksheet, i, 0); //Staff ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 1); //Secondary Staff ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 2); //Staff Name
                    values[vIndex++] = GetCellData(InputWorksheet, i, 3); //Activity ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 4); //Activity Type
                    values[vIndex++] = GetCellData(InputWorksheet, i, 5); //Activity Source
                    values[vIndex++] = GetCellData(InputWorksheet, i, 6); //ID"
                    values[vIndex++] = GetCellData(InputWorksheet, i, 7); //Secondary ID"

                    string name = GetCellData(InputWorksheet, i, 8);
                    values[vIndex++] = name.Replace("\"", ""); //Name

                    string scheduledStart = GetCellData(InputWorksheet, i, 9) + " " + GetCellData(InputWorksheet, i, 10);
                    DateTime? scheduledStartDate = string.IsNullOrEmpty(scheduledStart) ? (DateTime?)null : Convert.ToDateTime(scheduledStart);
                    values[vIndex++] = scheduledStartDate; //Scheduled Start

                    string scheduledEnd = GetCellData(InputWorksheet, i, 11) + " " + GetCellData(InputWorksheet, i, 12);
                    DateTime? scheduledEndDate = string.IsNullOrEmpty(scheduledEnd) ? (DateTime?)null : Convert.ToDateTime(scheduledEnd);
                    values[vIndex++] = scheduledEndDate; //Scheduled End

                    string sDuration = GetCellData(InputWorksheet, i, 13);
                    TimeSpan sDurationTime;
                    if (TimeSpan.TryParse(sDuration, out sDurationTime))
                    {
                        values[vIndex++] = sDurationTime;  //SDuration
                    }

                    string actualStart = GetCellData(InputWorksheet, i, 14) + " " + GetCellData(InputWorksheet, i, 15);
                    DateTime? actualStartDate = string.IsNullOrEmpty(actualStart) ? (DateTime?)null : Convert.ToDateTime(actualStart);
                    values[vIndex++] = actualStartDate; //Actual Start

                    string actualEnd = GetCellData(InputWorksheet, i, 17) + " " + GetCellData(InputWorksheet, i, 18);
                    DateTime? actualEndDate = string.IsNullOrEmpty(actualEnd) ? (DateTime?)null : Convert.ToDateTime(actualEnd);
                    values[vIndex++] = actualEndDate; //Actual End

                    string aDuration = GetCellData(InputWorksheet, i, 13);
                    TimeSpan aDurationTime;
                    if (TimeSpan.TryParse(aDuration, out aDurationTime))
                    {
                        values[vIndex++] = aDurationTime;  //ADuration
                    }

                    values[vIndex++] = GetCellData(InputWorksheet, i, 21); //Difference
                    values[vIndex++] = GetCellData(InputWorksheet, i, 22); //Location
                    values[vIndex++] = GetCellData(InputWorksheet, i, 23); //Discipline

                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            };

            return dataTable;
        }

        public DataTable GetUPVTDDataTable(Stream input)
        {
            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;

                SPColumn[] spc = GetSPColumns(); //Billing Code and Payroll Code

                for (int i = 0; i < spc.Count(); i++)
                {
                    CellSelection selection = InputWorksheet.Cells[0, i];
                    var columnName = "Column" + (i + 1);
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    int vIndex = 0;

                    string activityType = GetCellData(InputWorksheet, i, 4);  //Activity Type
                    string billingCode = GetCellData(InputWorksheet, i, 20);  //Billing Code
                    string payrollCode = GetCellData(InputWorksheet, i, 21);  //Payroll Code
                    string service = GetCellData(InputWorksheet, i, 22);      //Service

                    bool isUPV = GetCellData(InputWorksheet, i, 4).Contains("UPV");
                    bool noBillingCode = string.IsNullOrEmpty(GetCellData(InputWorksheet, i, 20));  //Billing Code
                    bool noPayrollCode = string.IsNullOrEmpty(GetCellData(InputWorksheet, i, 21));  //Payroll Code
                    bool noService = string.IsNullOrEmpty(GetCellData(InputWorksheet, i, 22));   //Service

                    if (!isUPV)
                        continue;

                    if (noBillingCode && noPayrollCode && noService)
                        continue;

                    var values = new object[spc.Count()];
                    values[vIndex++] = GetCellData(InputWorksheet, i, 0); //Staff ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 1); //Secondary Staff ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 2); //Staff Name
                    values[vIndex++] = GetCellData(InputWorksheet, i, 3); //Activity ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 4); //Activity Type
                    values[vIndex++] = GetCellData(InputWorksheet, i, 5); //ID
                    values[vIndex++] = GetCellData(InputWorksheet, i, 6); //Secondary ID"

                    string name = GetCellData(InputWorksheet, i, 7);
                    values[vIndex++] = name.Replace("\"", ""); //Name

                    string combinedStart = GetCellData(InputWorksheet, i, 8) + " " + GetCellData(InputWorksheet, i, 9);
                    DateTime startDate = Convert.ToDateTime(combinedStart);
                    values[vIndex++] = startDate; //Start

                    string combinedFinish = GetCellData(InputWorksheet, i, 11) + " " + GetCellData(InputWorksheet, i, 12);
                    DateTime finishDate = Convert.ToDateTime(combinedFinish);
                    values[vIndex++] = finishDate; //Finish

                    string durationStr = GetCellData(InputWorksheet, i, 14);
                    values[vIndex++] = int.Parse(durationStr, System.Globalization.NumberStyles.AllowThousands);  //Duration

                    values[vIndex++] = GetCellData(InputWorksheet, i, 15); //Travel Time
                    values[vIndex++] = GetCellData(InputWorksheet, i, 16); //TSrc
                    values[vIndex++] = GetCellData(InputWorksheet, i, 17); //Distance
                    values[vIndex++] = GetCellData(InputWorksheet, i, 18); //DSrc
                    values[vIndex++] = GetCellData(InputWorksheet, i, 19); //Phone

                    values[vIndex++] = GetCellData(InputWorksheet, i, 20); //Billing Code
                    values[vIndex++] = GetCellData(InputWorksheet, i, 21); //Payroll Code
                    values[vIndex++] = GetCellData(InputWorksheet, i, 22); //Service
                    values[vIndex++] = GetCellData(InputWorksheet, i, 23); //On-call
                    values[vIndex++] = GetCellData(InputWorksheet, i, 24); //Location
                    values[vIndex++] = GetCellData(InputWorksheet, i, 25); //Discipline

                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            };

            return dataTable;
        }

        public DataTable GetBillingAuthorizationDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[57]
            {
                new SPColumn("authorizations_id", typeof(string)),
                new SPColumn("authorization_details_id", typeof(string)),
                new SPColumn("authorization_type", typeof(string)),
                new SPColumn("is_expired", typeof(string)),
                new SPColumn("full_name", typeof(string)),
                new SPColumn("authorization_number", typeof(string)),
                new SPColumn("people_id", typeof(string)),
                new SPColumn("id_no", typeof(string)),
                new SPColumn("medicaid_number", typeof(string)),
                new SPColumn("policy_num", typeof(string)),
                new SPColumn("from_date", typeof(DateTime)),
                new SPColumn("to_date", typeof(DateTime)),
                new SPColumn("date_from_details", typeof(DateTime)),
                new SPColumn("date_to_details", typeof(DateTime)),
                new SPColumn("benefits_assignments_id", typeof(string)),
                new SPColumn("payor_vendor_id", typeof(string)),
                new SPColumn("vendor_name", typeof(string)),
                new SPColumn("units_aut_header", typeof(string)),
                new SPColumn("units_used_header", typeof(string)),
                new SPColumn("header_balance", typeof(string)),
                new SPColumn("total_aut_detail", typeof(string)),
                new SPColumn("units_aut_detail", typeof(string)),
                new SPColumn("units_used_detail", typeof(string)),
                new SPColumn("detail_balance", typeof(string)),
                new SPColumn("units_performed_header", typeof(string)),
                new SPColumn("units_sched_header", typeof(string)),
                new SPColumn("units_performed_detail", typeof(string)),
                new SPColumn("units_sched_detail", typeof(string)),
                new SPColumn("program_name", typeof(string)),
                new SPColumn("group_profile_type", typeof(string)),
                new SPColumn("profile_name", typeof(string)),
                new SPColumn("service_name", typeof(string)),
                new SPColumn("rate_description", typeof(string)),
                new SPColumn("program_modifier_code", typeof(string)),
                new SPColumn("billing_payment_plan_id", typeof(string)),
                new SPColumn("over_procedure_code", typeof(string)),
                new SPColumn("procedure_code_id", typeof(string)),
                new SPColumn("billing_payment_plan_scheme_link_id", typeof(string)),
                new SPColumn("billing_service_bundle_id", typeof(string)),
                new SPColumn("service_bundle_name", typeof(string)),
                new SPColumn("is_billing", typeof(string)),
                new SPColumn("type_of_authorizations", typeof(string)),
                new SPColumn("staff_id", typeof(string)),
                new SPColumn("staff_name", typeof(string)),
                new SPColumn("amount_charged", typeof(string)),
                new SPColumn("over_procedure_code_amt", typeof(string)),
                new SPColumn("pa_location_code", typeof(string)),
                new SPColumn("school_district_id", typeof(string)),
                new SPColumn("school_district", typeof(string)),
                new SPColumn("school_district_code", typeof(string)),
                new SPColumn("dtFromDate", typeof(string)),
                new SPColumn("dtToDate", typeof(string)),
                new SPColumn("authorization_reason", typeof(string)),
                new SPColumn("authorization_message", typeof(string)),
                new SPColumn("client_facility_name", typeof(string)),
                new SPColumn("client_managing_office_name", typeof(string)),
                new SPColumn("is_extended", typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;

                dataTable.Columns.Add(spc[7].name, spc[7].type); //id_no
                dataTable.Columns.Add(spc[4].name, spc[4].type); //full_name
                dataTable.Columns.Add(spc[10].name, spc[10].type); //from_date
                dataTable.Columns.Add(spc[11].name, spc[11].type); //to_date
                dataTable.Columns.Add(spc[12].name, spc[12].type); //date_from_details
                dataTable.Columns.Add(spc[13].name, spc[13].type); //date_to_details
                dataTable.Columns.Add(spc[31].name, spc[31].type); //service_name
                dataTable.Columns.Add(spc[32].name, spc[32].type); //rate_description
                dataTable.Columns.Add(spc[23].name, spc[23].type); //detail_balance
                dataTable.Columns.Add(spc[3].name, spc[3].type); //is_expired
                dataTable.Columns.Add(spc[8].name, spc[8].type); //medicaid_number
                dataTable.Columns.Add("procedure_code", typeof(string)); //procedure_code

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[12];

                    values[0] = GetCellData(InputWorksheet, i, 7); //id_no

                    string full_name = GetCellData(InputWorksheet, i, 4);
                    values[1] = full_name.Replace(",", ""); //full_name

                    string fromDateString = GetCellData(InputWorksheet, i, 10); //from_date
                    if (!string.IsNullOrEmpty(fromDateString))
                    { 
                        DateTime fromDate = Convert.ToDateTime(fromDateString);
                        values[2] = fromDate; //From Date
                    }

                    string toDateString = GetCellData(InputWorksheet, i, 11); //to_date
                    if (!string.IsNullOrEmpty(toDateString))
                    {
                        DateTime toDate = Convert.ToDateTime(toDateString);
                        values[3] = toDate; //To Date
                    }

                    string dateFromString = GetCellData(InputWorksheet, i, 12); //date_from_details
                    if (!string.IsNullOrEmpty(dateFromString))
                    {
                        DateTime dateFrom = Convert.ToDateTime(dateFromString);
                        values[4] = dateFrom; //date_from_details
                    }

                    string dateToString = GetCellData(InputWorksheet, i, 13); //date_to_details
                    if (!string.IsNullOrEmpty(dateToString))
                    {
                        DateTime dateTo = Convert.ToDateTime(dateToString);
                        values[5] = dateTo; //To Date
                    }

                    values[6] = GetCellData(InputWorksheet, i, 31); //service_name

                    string rateDescription = GetCellData(InputWorksheet, i, 32); //rate_description
                    values[7] = rateDescription;

                    values[8] = GetCellData(InputWorksheet, i, 23); //detail_balance

                    values[9] = GetCellData(InputWorksheet, i, 3); //is_expired

                    values[10] = GetCellData(InputWorksheet, i, 8); //medicaid_number

                    Match procedureCode = Regex.Match(rateDescription, @"^.*?(?=-)");
                    values[11] = procedureCode; //procedure_code

                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            };

            return dataTable;
        }

        public DataTable GetClientIDsDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[57]
            {
                new SPColumn("authorizations_id", typeof(string)),
                new SPColumn("authorization_details_id", typeof(string)),
                new SPColumn("authorization_type", typeof(string)),
                new SPColumn("is_expired", typeof(string)),
                new SPColumn("full_name", typeof(string)),
                new SPColumn("authorization_number", typeof(string)),
                new SPColumn("people_id", typeof(string)),
                new SPColumn("id_no", typeof(string)),
                new SPColumn("medicaid_number", typeof(string)),
                new SPColumn("policy_num", typeof(string)),
                new SPColumn("from_date", typeof(string)),
                new SPColumn("to_date", typeof(string)),
                new SPColumn("date_from_details", typeof(DateTime)),
                new SPColumn("date_to_details", typeof(DateTime)),
                new SPColumn("benefits_assignments_id", typeof(string)),
                new SPColumn("payor_vendor_id", typeof(string)),
                new SPColumn("vendor_name", typeof(string)),
                new SPColumn("units_aut_header", typeof(string)),
                new SPColumn("units_used_header", typeof(string)),
                new SPColumn("header_balance", typeof(string)),
                new SPColumn("total_aut_detail", typeof(string)),
                new SPColumn("units_aut_detail", typeof(string)),
                new SPColumn("units_used_detail", typeof(string)),
                new SPColumn("detail_balance", typeof(string)),
                new SPColumn("units_performed_header", typeof(string)),
                new SPColumn("units_sched_header", typeof(string)),
                new SPColumn("units_performed_detail", typeof(string)),
                new SPColumn("units_sched_detail", typeof(string)),
                new SPColumn("program_name", typeof(string)),
                new SPColumn("group_profile_type", typeof(string)),
                new SPColumn("profile_name", typeof(string)),
                new SPColumn("service_name", typeof(string)),
                new SPColumn("rate_description", typeof(string)),
                new SPColumn("program_modifier_code", typeof(string)),
                new SPColumn("billing_payment_plan_id", typeof(string)),
                new SPColumn("over_procedure_code", typeof(string)),
                new SPColumn("procedure_code_id", typeof(string)),
                new SPColumn("billing_payment_plan_scheme_link_id", typeof(string)),
                new SPColumn("billing_service_bundle_id", typeof(string)),
                new SPColumn("service_bundle_name", typeof(string)),
                new SPColumn("is_billing", typeof(string)),
                new SPColumn("type_of_authorizations", typeof(string)),
                new SPColumn("staff_id", typeof(string)),
                new SPColumn("staff_name", typeof(string)),
                new SPColumn("amount_charged", typeof(string)),
                new SPColumn("over_procedure_code_amt", typeof(string)),
                new SPColumn("pa_location_code", typeof(string)),
                new SPColumn("school_district_id", typeof(string)),
                new SPColumn("school_district", typeof(string)),
                new SPColumn("school_district_code", typeof(string)),
                new SPColumn("dtFromDate", typeof(string)),
                new SPColumn("dtToDate", typeof(string)),
                new SPColumn("authorization_reason", typeof(string)),
                new SPColumn("authorization_message", typeof(string)),
                new SPColumn("client_facility_name", typeof(string)),
                new SPColumn("client_managing_office_name", typeof(string)),
                new SPColumn("is_extended", typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;

                dataTable.Columns.Add(spc[7].name, spc[7].type); //id_no

                dataTable.Columns.Add(spc[8].name, spc[8].type); //medicaid_number

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[2];
                    values[0] = GetCellData(InputWorksheet, i, 7); //id_no
                    values[1] = GetCellData(InputWorksheet, i, 8); //medicaid_number

                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            };

            return dataTable;
        }

        public DataTable GetHCSISDataTable(Stream input, DataTable dClientIDsTable)
        {
            SPColumn[] spc = new SPColumn[]
            {
                new SPColumn("Program_Office",typeof(string)),
                new SPColumn("County_Joinder",typeof(string)),
                new SPColumn("Fiscal_Year",typeof(string)),
                new SPColumn("Waiver_Program",typeof(string)),
                new SPColumn("Individual_Name",typeof(string)),
                new SPColumn("Recipient_ID",typeof(string)),
                new SPColumn("SC_Entity_Name",typeof(string)),
                new SPColumn("SC_Name",typeof(string)),
                new SPColumn("Provider_Name",typeof(string)),
                new SPColumn("MPI",typeof(string)),
                new SPColumn("Service_Location",typeof(string)),
                new SPColumn("Service_Name",typeof(string)),
                new SPColumn("ProcedureCode_Modifier",typeof(string)),
                new SPColumn("Service_Status",typeof(string)),
                new SPColumn("Service_Start_Date",typeof(DateTime)),
                new SPColumn("Service_End_Date",typeof(DateTime)),
                new SPColumn("Funding_Stream",typeof(string)),
                new SPColumn("Authorized_Units",typeof(string)),
                new SPColumn("Utilized_Units",typeof(string)),
                new SPColumn("Remaining_Units",typeof(string)),
                new SPColumn("Unit_Cost",typeof(string)),
                new SPColumn("Authorized_Amount",typeof(string)),
                new SPColumn("Utilized_Amount",typeof(string)),
                new SPColumn("Last_Paid_Service_Date",typeof(string)),
                new SPColumn("Last_Authorized_Date",typeof(string)),
                new SPColumn("ICD_9_Diagnosis_Code",typeof(string)),
                new SPColumn("ICD_10_Diagnosis_Code",typeof(string)),
                new SPColumn("TXT_NEEDS_LEVEL",typeof(string)),
                new SPColumn("TXT_NEEDS_GROUP",typeof(string)),
                new SPColumn("NL_NG_EFFBEG_DATE",typeof(string)),
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;

                dataTable.Columns.Add(spc[4].name, spc[4].type); //Individual_Name
                dataTable.Columns.Add(spc[5].name, spc[5].type); //Recipient_ID
                dataTable.Columns.Add(spc[12].name, spc[12].type); //ProcedureCode_Modifier
                dataTable.Columns.Add(spc[14].name, spc[14].type); //Service_Start_Date
                dataTable.Columns.Add(spc[15].name, spc[15].type); //Service_End_Date
                dataTable.Columns.Add(spc[17].name, spc[17].type); //Authorized_Units
                dataTable.Columns.Add(spc[18].name, spc[18].type); //Utilized_Units
                dataTable.Columns.Add(spc[19].name, spc[19].type); //Remaining_Units
                dataTable.Columns.Add("id_no", typeof(string)); //ClientID

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[9];

                    values[0] = GetCellData(InputWorksheet, i, 4); //iIndividual_Names_expired

                    string recipientID = GetCellData(InputWorksheet, i, 5); //Recipient_ID
                    values[1] = recipientID;

                    string service = GetCellData(InputWorksheet, i, 12); //ProcedureCode_Modifier  W7060:00:00:00:00
                    string result = Regex.Replace(service, @"\b:00\b", "");
                    values[2] = result;

                    string startDateString = GetCellData(InputWorksheet, i, 14); //Service_Start_Date
                    DateTime startDate = Convert.ToDateTime(startDateString);
                    values[3] = startDate; //Start Date

                    string endDateString = GetCellData(InputWorksheet, i, 15); //Service_End_Date
                    DateTime endDate = Convert.ToDateTime(endDateString);
                    values[4] = endDate; //End Date

                    values[5] = GetCellData(InputWorksheet, i, 17); //Authorized_Units
                    values[6] = GetCellData(InputWorksheet, i, 18); //Utilized_Units
                    values[7] = GetCellData(InputWorksheet, i, 19); //Remaining_Units

                    String condition = String.Format("medicaid_number = '" + recipientID + "'");
                    DataRow[] results = dClientIDsTable.Select(condition);
                    if (results.Length == 1)
                    {
                        values[8] = results[0].Field<string>("id_no");
                    }

                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            };

            return dataTable;
        }

        public DataTable GetSandataExportVisitsDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[]
            {
                new SPColumn("Client Name",typeof(string)),
                new SPColumn("Employee Name",typeof(string)),
                new SPColumn("Service",typeof(string)),
                new SPColumn("WCode",typeof(string)),
                new SPColumn("Visit Date",typeof(DateTime)),
                new SPColumn("Call In",typeof(DateTime)),
                new SPColumn("Call Out",typeof(DateTime)),
                new SPColumn("Call Hours",typeof(TimeSpan)),
                new SPColumn("Adjusted In",typeof(DateTime)),
                new SPColumn("Adjusted Out",typeof(DateTime)),
                new SPColumn("Adjusted Hours",typeof(TimeSpan)),
                new SPColumn("Bill Hours",typeof(TimeSpan)),
                new SPColumn("Visit Status",typeof(string)),
                new SPColumn("Do Not Bill",typeof(bool)),
                new SPColumn("Exceptions",typeof(string)),
            };

            var services = new Dictionary<string, string>()
            {
                {"Companion (1:1)" ,"W1726"},
                {"IHCS Level 2 (1:1)", "W7060"},
                {"IHCS Level 2 (1:1) Enhanced", "W7061"},
                {"IHCS Level 3 (2:1)", "W7068"},
                {"IHCS Level 3 (2:1) Enhanced", "W7069"},
                {"Respite Level 3 (1:1)-Day", "W9798"},
                {"Respite Level 3 (1:1)-15 Mins", "W9862"},
                {"Respite Level 3 (1:1) Enhanced-15 Mins", "W9863"}
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;

                for (int i = 0; i < spc.Count(); i++)
                {
                    CellSelection selection = InputWorksheet.Cells[0, i];
                    var columnName = "Column" + (i + 1);
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }
 
                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[spc.Count()];

                    values[0] = GetCellData(InputWorksheet, i, 0); //Client Name
                    values[1] = GetCellData(InputWorksheet, i, 1); //Employee Name
                    values[2] = GetCellData(InputWorksheet, i, 2); //Service

                    if(services.ContainsKey(values[2].ToString()))
                    {
                        values[3] = services[values[2].ToString()];
                    }
                    else
                    {
                        values[3] = "NOTFOUND";
                    }

                    string dateStr1 = GetCellData(InputWorksheet, i, 3);
                    DateTime visitDateOnly = Convert.ToDateTime(dateStr1);
                    values[4] = visitDateOnly; //Visit Date

                    SandataDateTimeDuration sdtd1 = SetDateTimeDuration(visitDateOnly,  GetCellData(InputWorksheet, i, 7), GetCellData(InputWorksheet, i, 8),  GetCellData(InputWorksheet, i, 9));
                    values[5] = sdtd1.Start; //Call In
                    values[6] = sdtd1.End; //Call Out
                    values[7] = sdtd1.Duration; //Call Hours

                    SandataDateTimeDuration sdtd2 = SetDateTimeDuration(visitDateOnly, GetCellData(InputWorksheet, i, 10), GetCellData(InputWorksheet, i, 11), GetCellData(InputWorksheet, i, 12));
                    values[8] = sdtd2.Start; //Adjusted In
                    values[9] = sdtd2.End; //Adjusted Out
                    values[10] = sdtd2.Duration; //Adjusted Hours

                    TimeSpan billDuration;
                    if (!TimeSpan.TryParse(GetCellData(InputWorksheet, i, 13), out billDuration))
                    {
                    }
                    values[11] = billDuration; //Bill Hours

                    values[12] = GetCellData(InputWorksheet, i, 14); //Visit Status

                    bool doNotBill = ("Yes".Equals(GetCellData(InputWorksheet, i, 15)) ? true : false);
                    values[13] = doNotBill; //Do Not Bill

                    values[14] = GetCellData(InputWorksheet, i, 16); //Exceptions

                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            };

            return dataTable;
        }

        public DataTable GetSandataExportVisitsDataTableViaCSV(Stream input)
        {
            SPColumn[] spc = new SPColumn[]
            {
                new SPColumn("Client Name",typeof(string)),
                new SPColumn("Employee Name",typeof(string)),
                new SPColumn("Service",typeof(string)),
                new SPColumn("WCode",typeof(string)),
                new SPColumn("Visit Date",typeof(DateTime)),
                new SPColumn("Call In",typeof(DateTime)),
                new SPColumn("Call Out",typeof(DateTime)),
                new SPColumn("Call Hours",typeof(TimeSpan)),
                new SPColumn("Adjusted In",typeof(DateTime)),
                new SPColumn("Adjusted Out",typeof(DateTime)),
                new SPColumn("Adjusted Hours",typeof(TimeSpan)),
                new SPColumn("Bill Hours",typeof(TimeSpan)),
                new SPColumn("Visit Status",typeof(string)),
                new SPColumn("Do Not Bill",typeof(bool)),
                new SPColumn("Exceptions",typeof(string)),
            };

            var services = new Dictionary<string, string>()
            {
                {"Companion (1:1)" ,"W1726"},
                {"IHCS Level 2 (1:1)", "W7060"},
                {"IHCS Level 2 (1:1) Enhanced", "W7061"},
                {"IHCS Level 3 (2:1)", "W7068"},
                {"IHCS Level 3 (2:1) Enhanced", "W7069"},
                {"Respite Level 3 (1:1)-Day", "W9798"},
                {"Respite Level 3 (1:1)-15 Mins", "W9862"},
                {"Respite Level 3 (1:1) Enhanced-15 Mins", "W9863"}
            };

            DataTable dataTable = new DataTable();
            for (int i = 0; i < spc.Count(); i++)
            {
                dataTable.Columns.Add(spc[i].name, spc[i].type);
            }

            using (var reader = new StreamReader(input))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var values = new object[spc.Count()];

                    values[0] = csv.GetField<string>(0); //Client Name
                    values[1] = csv.GetField<string>(1); //Employee Name
                    values[2] = csv.GetField<string>(2); //Service

                    if (services.ContainsKey(values[2].ToString()))
                    {
                        values[3] = services[values[2].ToString()];
                    }
                    else
                    {
                        values[3] = "NOTFOUND";
                    }

                    string dateStr1 = csv.GetField<string>(3);
                    DateTime visitDateOnly = Convert.ToDateTime(dateStr1);
                    values[4] = visitDateOnly; //Visit Date


                    SandataDateTimeDuration sdtd1 = SetDateTimeDuration(visitDateOnly, csv.GetField<string>(7), csv.GetField<string>(8), csv.GetField<string>(9));
                    values[5] = sdtd1.Start; //Call In
                    values[6] = sdtd1.End; //Call Out
                    values[7] = sdtd1.Duration; //Call Hours

                    SandataDateTimeDuration sdtd2 = SetDateTimeDuration(visitDateOnly, csv.GetField<string>(10), csv.GetField<string>(11), csv.GetField<string>(12));
                    values[8] = sdtd2.Start; //Adjusted In
                    values[9] = sdtd2.End; //Adjusted Out
                    values[10] = sdtd2.Duration; //Adjusted Hours

                    TimeSpan billDuration;
                    if (!TimeSpan.TryParse(csv.GetField<string>(13), out billDuration))
                    {
                    }
                    values[11] = billDuration; //Bill Hours

                    values[12] = csv.GetField<string>(14); //Visit Status

                    bool doNotBill = ("Yes".Equals(csv.GetField<string>(15)) ? true : false);
                    values[13] = doNotBill; //Do Not Bill

                    values[14] = csv.GetField<string>(16); //Exceptions

                    dataTable.Rows.Add(values);

                }
            }

            return dataTable;
        }

        SandataDateTimeDuration SetDateTimeDuration(DateTime visitDateOnly, string startTime, string endTime, string duration)
        {
            SandataDateTimeDuration sdtd = new SandataDateTimeDuration();

            DateTime callinTimeOnly;
            DateTime startDateTime;
            if (!DateTime.TryParseExact(startTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out callinTimeOnly))
            { 
            }
            startDateTime = visitDateOnly.Date.Add(callinTimeOnly.TimeOfDay);
            sdtd.Start = startDateTime; //Call In

            TimeSpan coDuration;
            if (!TimeSpan.TryParse(duration, out coDuration))
            {
            }
            sdtd.Duration = coDuration; //Call Hours

            DateTime endDateTime;
            if (!DateTime.TryParseExact(endTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime))
            {
            }

            if(string.IsNullOrEmpty(duration))
            {
                DateTime temp = visitDateOnly.Date.Add(endDateTime.TimeOfDay);
                sdtd.End = temp;
            }
            else
            {
                DateTime temp = startDateTime.Add(coDuration);
                sdtd.End = temp; //Call Out
            }

            return sdtd;
        }

        public DataTable GetNetsmartClientServicesDataTableViaCSV(Stream input)
        {
            SPColumn[] spc = new SPColumn[]
            {
                new SPColumn("people_id",typeof(string)),
                new SPColumn("full_name",typeof(string)),
                new SPColumn("id_no",typeof(string)),
                new SPColumn("other_id_number",typeof(string)),
                new SPColumn("dob",typeof(DateTime)),
                new SPColumn("gender",typeof(string)),
                new SPColumn("gender_code",typeof(string)),
                new SPColumn("ssn_number",typeof(string)),
                new SPColumn("is_staff",typeof(string)),
                new SPColumn("intake_dt",typeof(DateTime)),
                new SPColumn("discharge_dt",typeof(string)),
                new SPColumn("medicaid_number",typeof(string)),
                new SPColumn("ipd",typeof(string)),
                new SPColumn("current_location",typeof(string)),
                new SPColumn("program_info_id",typeof(string)),
                new SPColumn("program_name",typeof(string)),
                new SPColumn("program_type",typeof(string)),
                new SPColumn("site_providing_service",typeof(string)),
                new SPColumn("facility",typeof(string)),
                new SPColumn("license_number",typeof(string)),
                new SPColumn("staff_id",typeof(string)),
                new SPColumn("job_title",typeof(string)),
                new SPColumn("staff_name",typeof(string)),
                new SPColumn("actual_date",typeof(DateTime)),
                new SPColumn("end_date",typeof(DateTime)),
                new SPColumn("duration",typeof(string)),
                new SPColumn("event_log_id",typeof(string)),
                new SPColumn("event_definition_id",typeof(string)),
                new SPColumn("event_name",typeof(string)),
                new SPColumn("parent_event",typeof(string)),
                new SPColumn("service",typeof(string)),
                new SPColumn("activity_type",typeof(string)),
                new SPColumn("encounter_with",typeof(string)),
                new SPColumn("is_client_involved",typeof(string)),
                new SPColumn("is_noshow",typeof(string)),
                new SPColumn("is_locked",typeof(string)),
                new SPColumn("progress_note",typeof(string)),
                new SPColumn("event_category_id",typeof(string)),
                new SPColumn("form_header_id",typeof(string)),
                new SPColumn("is_billed",typeof(string)),
                new SPColumn("is_paid",typeof(string)),
                new SPColumn("invoice_number",typeof(string)),
                new SPColumn("date_entered",typeof(string)),
                new SPColumn("user_entered",typeof(string)),
                new SPColumn("user_entered_name",typeof(string)),
                new SPColumn("approved_date",typeof(string)),
                new SPColumn("approved_by_id",typeof(string)),
                new SPColumn("approved_staff_name",typeof(string)),
                new SPColumn("submitted",typeof(string)),
                new SPColumn("is_approved",typeof(string)),
                new SPColumn("is_notapproved",typeof(string)),
                new SPColumn("is_notapproved_subm",typeof(string)),
                new SPColumn("depended_activity",typeof(string)),
                new SPColumn("program_unit_description",typeof(string)),
                new SPColumn("sc_code",typeof(string)),
                new SPColumn("duration_num",typeof(string)),
                new SPColumn("do_not_bill",typeof(string)),
                new SPColumn("do_not_pay",typeof(string)),
                new SPColumn("general_location_id",typeof(string)),
                new SPColumn("general_location",typeof(string)),
                new SPColumn("program_modifier_id",typeof(string)),
                new SPColumn("program_modifier",typeof(string)),
                new SPColumn("program_modifier_code",typeof(string)),
                new SPColumn("NormalWorkHours",typeof(string)),
                new SPColumn("duration_other_num",typeof(string)),
                new SPColumn("duration_other",typeof(string)),
                new SPColumn("travel_time_num",typeof(string)),
                new SPColumn("travel_time",typeof(string)),
                new SPColumn("planning_time_num",typeof(string)),
                new SPColumn("planning_time",typeof(string)),
                new SPColumn("total_duration_num",typeof(string)),
                new SPColumn("total_duration",typeof(string)),
                new SPColumn("total_duration_num_cc",typeof(string)),
                new SPColumn("total_duration_cc",typeof(string)),
                new SPColumn("actual_location_facility_id",typeof(string)),
                new SPColumn("actual_location_facility",typeof(string)),
                new SPColumn("reason_for_no_show_id",typeof(string)),
                new SPColumn("reason_for_no_show",typeof(string)),
                new SPColumn("form_program",typeof(string)),
                new SPColumn("cue_number",typeof(string)),
                new SPColumn("cue_type",typeof(string)),
                new SPColumn("client_participation_response",typeof(string)),
                new SPColumn("client_part_resp_description",typeof(string)),
                new SPColumn("outcome_id",typeof(string)),
                new SPColumn("outcome_description",typeof(string)),
                new SPColumn("gaf_score_current",typeof(string)),
                new SPColumn("date_order",typeof(string)),
                new SPColumn("is_billable",typeof(string)),
                new SPColumn("snb_reasons",typeof(string)),
                new SPColumn("is_billing",typeof(string)),
                new SPColumn("sr_incident_to",typeof(string)),
                new SPColumn("is_incident_to",typeof(string)),
                new SPColumn("medicare_incident_to_supervisor",typeof(string)),
                new SPColumn("it_supervisor_name",typeof(string)),
                new SPColumn("whole_date_order",typeof(string)),
                new SPColumn("serv_entry_actual_date",typeof(string)),
                new SPColumn("sort_order",typeof(string)),
                new SPColumn("conversion_id",typeof(string))
            };

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo p in typeof(NSClientServices).GetProperties())
            {
                dataTable.Columns.Add(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType);
            }

            for (int i = 0; i < spc.Count(); i++)
            {
                dataTable.Columns.Add(spc[i].name, spc[i].type);
            }

            List<String> lines = new List<String>();
            using (var reader = new StreamReader(input))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    String replaced = line.Replace("\"images/CHECK.gif\"", "images/CHECK.gif");
                    lines.Add(replaced);
                }
            }

            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            foreach (String line in lines)
            {
                streamWriter.WriteLine(line);
            }
            streamWriter.Flush();                                   
            memStream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(memStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    dataTable.Load(dr);
                }
            }

            return dataTable;
        }

        public DataTable GetNetsmartClientServicesDataTablePayrollViaCSV(Stream input, string weekName)
        {
            SPColumn[] spc = new SPColumn[]
            {
                new SPColumn("staff_name",typeof(string)),
                new SPColumn("event_name",typeof(string)),
                new SPColumn("full_name",typeof(string)),
                new SPColumn("duration",typeof(string)),
                new SPColumn("total_duration",typeof(string)),
                new SPColumn("duration_num",typeof(int)),
                new SPColumn("total_duration_num",typeof(int)),
                new SPColumn("is_approved",typeof(string)),
                new SPColumn("week",typeof(string)),
            };

            //var services = new Dictionary<string, string>()
            //{
            //    {"Companion (1:1)" ,"W1726"},
            //    {"IHCS Level 2 (1:1)", "W7060"},
            //    {"IHCS Level 2 (1:1) Enhanced", "W7061"},
            //    {"IHCS Level 3 (2:1)", "W7068"},
            //    {"IHCS Level 3 (2:1) Enhanced", "W7069"},
            //    {"Respite Level 3 (1:1)-Day", "W9798"},
            //    {"Respite Level 3 (1:1)-15 Mins", "W9862"},
            //    {"Respite Level 3 (1:1) Enhanced-15 Mins", "W9863"}
            //};

            List<String> lines = new List<String>();
            using (var reader = new StreamReader(input))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    String replaced = line.Replace("\"images/CHECK.gif\"", "images/CHECK.gif");
                    lines.Add(replaced);
                }
            }

            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            foreach (String line in lines)
            {
                streamWriter.WriteLine(line);
            }
            streamWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);


            DataTable dataTable = new DataTable();
            for (int i = 0; i < spc.Count(); i++)
            {
                dataTable.Columns.Add(spc[i].name, spc[i].type);
            }

            using (var reader = new StreamReader(memStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var values = new object[spc.Count()];

                    values[0] = csv.GetField<string>(22); //staff_name
                    values[1] = csv.GetField<string>(28); //event_name
                    values[2] = csv.GetField<string>(1); //full_name
                    values[3] = csv.GetField<string>(25); //duration
                    values[4] = csv.GetField<string>(71); //total_duration
                    values[5] = Int32.Parse(csv.GetField<string>(55)); //duration_num
                    values[6] = Int32.Parse(csv.GetField<string>(70)); //total_duration_num
                    values[7] = csv.GetField<string>(49); //is_approved
                    values[8] = weekName; //week

                    dataTable.Rows.Add(values);
                }
            }

            return dataTable;

        }

        string ParseClientID(string clientString)
        {
            string idString = clientString.Split('(', ')')[1];

            string[] clientIDString = idString.Split(',');

            return clientIDString[0];
        }

        DateTime[] Parse2StartStopTime(string startString, string stopString)
        {
            DateTime[] startStopTime = new DateTime[2];

            try
            {
                if (!DateTime.TryParse(startString, out startStopTime[0]))
                    Logger.Error("Unable to parse '{0}'.", startString);

                string stopDateTimeString = null;
                if (stopString.Contains(','))
                {
                    stopDateTimeString = stopString;
                }
                else
                {
                    DateTime dateString = startStopTime[0].Date;
                    stopDateTimeString = dateString.ToString("MM/dd/yyyy ") + stopString;
                }

                if (!DateTime.TryParse(stopDateTimeString, out startStopTime[1]))
                    Logger.Error("Unable to parse '{0}'.", startString);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return startStopTime;
        }

        public class Temp
        {
            public int Key { get; set; }
            public string Value { get; set; }
        };


        public DataTable GetClosedActivitiesDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[19]
            {
                new SPColumn("Activity ID", typeof(string)),
                new SPColumn("Activity Type", typeof(string)),
                new SPColumn("Activity Source", typeof(string)),
                new SPColumn("Activity Name", typeof(string)),
                new SPColumn("ID", typeof(string)),
                new SPColumn("Executed By", typeof(string)),
                new SPColumn("Staff ID", typeof(string)),
                new SPColumn("Executed By Type", typeof(string)),
                new SPColumn("Start Time", typeof(DateTime)),
                new SPColumn("Stop Time", typeof(DateTime)),
                new SPColumn("Duration", typeof(string)),
                new SPColumn("Status", typeof(string)),
                new SPColumn("Travel To Activity Exported Time", typeof(string)),
                new SPColumn("Travel To Activity Exported Distance", typeof(string)),
                new SPColumn("Travel During Activity Exported Time", typeof(string)),
                new SPColumn("Travel During Activity Exported Distance", typeof(string)),
                new SPColumn("Travel Info", typeof(string)),
                new SPColumn("Alerts", typeof(string)),
                new SPColumn("Location", typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;

                for (int i = 0; i < spc.Count(); i++)
                {
                    CellSelection selection = InputWorksheet.Cells[0, i];
                    var columnName = "Column" + (i + 1);
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    if (GetCellData(InputWorksheet, i, 11) != "Finished") //Status
                        continue;

                    var values = new object[spc.Count()];

                    values[0] = GetCellData(InputWorksheet, i, 0); //Activity ID
                    values[1] = GetCellData(InputWorksheet, i, 1); //Activity Type
                    values[2] = GetCellData(InputWorksheet, i, 2); //Activity Source
                    values[3] = GetCellData(InputWorksheet, i, 3); //Activity Name
                    values[4] = GetCellData(InputWorksheet, i, 4); //ID
                    values[5] = GetCellData(InputWorksheet, i, 5); //Executed By
                    values[6] = GetCellData(InputWorksheet, i, 6); //Staff ID
                    values[7] = GetCellData(InputWorksheet, i, 7); //Executed By Type

                    string dateStr1 = GetCellData(InputWorksheet, i, 8);
                    DateTime tDate1 = Convert.ToDateTime(dateStr1);
                    values[8] = tDate1; //Start Time

                    string dateStr2 = GetCellData(InputWorksheet, i, 9);
                    DateTime tDate2 = Convert.ToDateTime(dateStr2);
                    values[9] = tDate2; //Stop Time

                    values[10] = GetCellData(InputWorksheet, i, 10); //Duration
                    values[11] = GetCellData(InputWorksheet, i, 11); //Status
                    values[12] = GetCellData(InputWorksheet, i, 12); //Travel To Activity Exported Time
                    values[13] = GetCellData(InputWorksheet, i, 13); //Travel To Activity Exported Distance
                    values[14] = GetCellData(InputWorksheet, i, 14); //Travel During Activity Exported Time
                    values[15] = GetCellData(InputWorksheet, i, 15); //Travel During Activity Exported Distance
                    values[16] = GetCellData(InputWorksheet, i, 16); //Travel Info
                    values[17] = GetCellData(InputWorksheet, i, 17); //Alerts
                    values[18] = GetCellData(InputWorksheet, i, 18); //Location

                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            };

            return dataTable;
        }

        public DataTable GetClosedActivitiesDataTableViaCSV(Stream input)
        {
            SPColumn[] spc = new SPColumn[19]
            {
                new SPColumn("Activity ID", typeof(string)),
                new SPColumn("Activity Type", typeof(string)),
                new SPColumn("Activity Source", typeof(string)),
                new SPColumn("Activity Name", typeof(string)),
                new SPColumn("ID", typeof(string)),
                new SPColumn("Executed By", typeof(string)),
                new SPColumn("Staff ID", typeof(string)),
                new SPColumn("Executed By Type", typeof(string)),
                new SPColumn("Start Time", typeof(DateTime)),
                new SPColumn("Stop Time", typeof(DateTime)),
                new SPColumn("Duration", typeof(string)),
                new SPColumn("Status", typeof(string)),
                new SPColumn("Travel To Activity Exported Time", typeof(string)),
                new SPColumn("Travel To Activity Exported Distance", typeof(string)),
                new SPColumn("Travel During Activity Exported Time", typeof(string)),
                new SPColumn("Travel During Activity Exported Distance", typeof(string)),
                new SPColumn("Travel Info", typeof(string)),
                new SPColumn("Alerts", typeof(string)),
                new SPColumn("Location", typeof(string))
            };

            DataTable dataTable = new DataTable();

            for (int i = 0; i < spc.Count(); i++)
            {
                dataTable.Columns.Add(spc[i].name, spc[i].type);
            }

            using (var reader = new StreamReader(input))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    int vIndex = 0;
                    var values = new object[spc.Count()];

                    values[vIndex++] = csv.GetField<string>(0); //Activity ID
                    values[vIndex++] = csv.GetField<string>(1); //Activity Type
                    values[vIndex++] = csv.GetField<string>(2); //Activity Source
                    values[vIndex++] = csv.GetField<string>(3); //Activity Name
                    values[vIndex++] = csv.GetField<string>(4); //ID
                    values[vIndex++] = csv.GetField<string>(5); //Executed By
                    values[vIndex++] = csv.GetField<string>(6); //Staff ID
                    values[vIndex++] = csv.GetField<string>(7); //Executed By Type
                    
                    
                    string iString = "8/15/2020 7:13 PM";
                    DateTime startDatTime = DateTime.ParseExact(iString, "M/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);

                    string start = csv.GetField<string>(8);
                    DateTime startDate = DateTime.ParseExact(start, "M/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
                    values[vIndex++] = startDate; //Start Date Time

                    string stop = csv.GetField<string>(9);
                    DateTime stopDate = DateTime.ParseExact(stop, "M/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
                    values[vIndex++] = stopDate; //Stop Date Time

                    values[vIndex++] = csv.GetField<string>(10); //Duration

                    values[vIndex++] = csv.GetField<string>(11); //Status
                    values[vIndex++] = csv.GetField<string>(12); //Travel To Activity Exported Time
                    values[vIndex++] = csv.GetField<string>(13); //Travel To Activity Exported Distance
                    values[vIndex++] = csv.GetField<string>(14); //Travel During Activity Exported Time
                    values[vIndex++] = csv.GetField<string>(15); //Travel During Activity Exported Distance
                    values[vIndex++] = csv.GetField<string>(16); //Travel Info
                    values[vIndex++] = csv.GetField<string>(17); //Alerts
                    values[vIndex++] = csv.GetField<string>(18); //Location

                    dataTable.Rows.Add(values);
                }
            }
            return dataTable;
        }
        public DataTable GetClientRosterDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[17]
            {
                new SPColumn("id_no", typeof(string)),
                new SPColumn("name", typeof(string)),
                new SPColumn("gender", typeof(string)),
                new SPColumn("dob", typeof(string)),
                new SPColumn("current_location", typeof(string)),
                new SPColumn("current_phone_day", typeof(string)),
                new SPColumn("intake_date",typeof(string)),
                new SPColumn("managing_office", typeof(string)),
                new SPColumn("program_name", typeof(string)),
                new SPColumn("unit", typeof(string)),
                new SPColumn("program_modifier", typeof(string)),
                new SPColumn("worker_name", typeof(string)),
                new SPColumn("worker_role", typeof(string)),
                new SPColumn("is_primary_worker", typeof(string)),
                new SPColumn("medicaid_number", typeof(string)),
                new SPColumn("medicaid_payer", typeof(string)),
                new SPColumn("medicaid_plan_name", typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;
                for (int i = 0; i < spc.Count(); i++)
                {
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[spc.Count()];
                    if (!string.IsNullOrEmpty(GetCellData(InputWorksheet, i, 2))) //id_no
                    {
                        values[0] = GetCellData(InputWorksheet, i, 2); //id_no
                        values[1] = GetCellData(InputWorksheet, i, 1); //name
                        values[2] = GetCellData(InputWorksheet, i, 3); //gender
                        values[3] = GetCellData(InputWorksheet, i, 4); //dob
                        values[4] = GetCellData(InputWorksheet, i, 37); //current_location
                        values[5] = GetCellData(InputWorksheet, i, 38); //current_phone_day
                        values[6] = GetCellData(InputWorksheet, i, 27); //intake_date
                        values[7] = GetCellData(InputWorksheet, i, 67); //managing_office
                        values[8] = GetCellData(InputWorksheet, i, 6); //program_name
                        values[9] = GetCellData(InputWorksheet, i, 14); //unit
                        values[10] = GetCellData(InputWorksheet, i, 91); //program_modifier
                        values[11] = GetCellData(InputWorksheet, i, 20); //worker_name
                        values[12] = GetCellData(InputWorksheet, i, 21); //worker_role
                        values[13] = GetCellData(InputWorksheet, i, 24); //is_primary_worker
                        values[14] = GetCellData(InputWorksheet, i, 76); //medicaid_number
                        values[15] = GetCellData(InputWorksheet, i, 77); //medicaid_payer
                        values[16] = GetCellData(InputWorksheet, i, 78); //medicaid_plan_name

                        dataTable.Rows.Add(values);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            };
        }

        public DataTable GetClientAuthorizationListDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[9]
            {
                new SPColumn("AClientID", typeof(string)),
                new SPColumn("AClientName", typeof(string)),
                new SPColumn("From", typeof(string)),
                new SPColumn("To", typeof(string)),
                new SPColumn("Service", typeof(string)),
                new SPColumn("Total", typeof(string)),
                new SPColumn("Used",typeof(string)),
                new SPColumn("Balance", typeof(string)),
                new SPColumn("Program", typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;
                for (int i = 0; i < spc.Count(); i++)
                {
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[spc.Count()];

                    if (!string.IsNullOrEmpty(GetCellData(InputWorksheet, i, 0))) //AClientID
                    {
                        values[0] = GetCellData(InputWorksheet, i, 0).TrimStart('0'); //AClientID - remove leading zeros
                        values[1] = GetCellData(InputWorksheet, i, 1); //AClientName
                        values[2] = GetCellData(InputWorksheet, i, 2); //From
                        values[3] = GetCellData(InputWorksheet, i, 3); //To
                        values[4] = GetCellData(InputWorksheet, i, 4); //Service
                        values[5] = GetCellData(InputWorksheet, i, 5); //Total
                        values[6] = GetCellData(InputWorksheet, i, 6); //Used
                        values[7] = GetCellData(InputWorksheet, i, 7); //Balance
                        values[8] = GetCellData(InputWorksheet, i, 8); //Program

                        dataTable.Rows.Add(values);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            };
        }

        public DataTable GetClientStaffListDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[7]
            {
                new SPColumn("SClientID", typeof(string)),
                new SPColumn("SClientName", typeof(string)),
                new SPColumn("MemberID", typeof(string)),
                new SPColumn("MemberName", typeof(string)),
                new SPColumn("MemberRole", typeof(string)),
                new SPColumn("IsSupervisor", typeof(string)),
                new SPColumn("MemberStart",typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                XlsxFormatProvider formatProvider = new XlsxFormatProvider();
                Workbook InputWorkbook = formatProvider.Import(input);

                var InputWorksheet = InputWorkbook.Sheets[0] as Worksheet;
                for (int i = 0; i < spc.Count(); i++)
                {
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[spc.Count()];

                    if (!string.IsNullOrEmpty(GetCellData(InputWorksheet, i, 0))) //SClientID
                    {
                        values[0] = GetCellData(InputWorksheet, i, 0).TrimStart('0'); //SClientID  - Remove leading zeros
                        values[1] = GetCellData(InputWorksheet, i, 1); //SClientName
                        values[2] = GetCellData(InputWorksheet, i, 2); //MemberID
                        values[3] = GetCellData(InputWorksheet, i, 3); //MemberName
                        values[4] = GetCellData(InputWorksheet, i, 4); //MemberRole
                        values[5] = GetCellData(InputWorksheet, i, 5); //IsSupervisor
                        values[6] = GetCellData(InputWorksheet, i, 6); //MemberStart

                        dataTable.Rows.Add(values);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            };
        }

        public DataTable ClientAuthorizationGroupBy(string sGroupByColumn, string sAggregateColumn, DataTable dSourceTable)
        {
            DataView dv = new DataView(dSourceTable);

            //getting distinct values for group column
            DataTable dtGroup = dv.ToTable(true, new string[] { sGroupByColumn });

            //adding column for the row count
            dtGroup.Columns.Add("ba_count", typeof(string));

            //looping thru distinct values for the group, counting
            foreach (DataRow dr in dtGroup.Rows)
            {
                string expression = String.Format("Count({0})", sAggregateColumn);
                string filter = String.Format("{0} = '{1}'", sGroupByColumn, dr[sGroupByColumn]);

                dr["ba_count"] = dSourceTable.Compute(expression, filter);
            }

            //returning grouped/counted result
            return dtGroup;
        }

        public DataTable ClientStaffGroupBy(string sGroupByColumn, string sAggregateColumn, DataTable dSourceTable)
        {
            DataView dv = new DataView(dSourceTable);

            //getting distinct values for group column
            DataTable dtGroup = dv.ToTable(true, new string[] { sGroupByColumn });

            //adding column for the row count
            dtGroup.Columns.Add("me_count", typeof(string));
            dtGroup.Columns.Add("ssp_count", typeof(string));

            //looping thru distinct values for the group, counting
            foreach (DataRow dr in dtGroup.Rows)
            {
                string expression = String.Format("Count({0})",sAggregateColumn);
                string filter = String.Format("{0} = '{1}' AND {2} = ", sGroupByColumn, dr[sGroupByColumn], sAggregateColumn);
                string meFilter = filter + "'Managing Employer'";
                string sspFilter = filter + "'AWC Support Service Professional'";

                dr["me_count"] = dSourceTable.Compute(expression, meFilter);
                dr["ssp_count"] = dSourceTable.Compute(expression, sspFilter);
             }

            //returning grouped/counted result
            return dtGroup;
        }

        public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);

            //Datatable which contains unique records will be return as output.
            return dTable;
        }

        string GetCellData(Worksheet worksheet, int i, int j)
        {
            string result = null;
            try
            {
                CellSelection selection = worksheet.Cells[i, j];

                ICellValue value = selection.GetValue().Value;
                CellValueFormat format = selection.GetFormat().Value;
                CellValueFormatResult formatResult = format.GetFormatResult(value);
                result = formatResult.InfosText;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

        public void DownloadExcelFile(Workbook workbook, string fileName)
        {
            try
            {
                IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
                byte[] renderedBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    formatProvider.Export(workbook, ms);
                    renderedBytes = ms.ToArray();
                }

                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.BinaryWrite(renderedBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void DownloadDocxFile(MemoryStream ms, string fileName)
        {
            try
            {
                DocxFormatProvider formatProvider = new DocxFormatProvider();
                byte[] renderedBytes;

                renderedBytes = ms.ToArray();

                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                HttpContext.Current.Response.AddHeader("Pragma", "public");
                HttpContext.Current.Response.BinaryWrite(renderedBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.Response.Write(renderedBytes);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public void DownloadCSVFile(Workbook workbook, string fileName)
        {
            try
            {
                IWorkbookFormatProvider formatProvider = new CsvFormatProvider();
                byte[] renderedBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    formatProvider.Export(workbook, ms);
                    renderedBytes = ms.ToArray();
                }

                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.ContentType = "text/csv";
                HttpContext.Current.Response.AddHeader("Pragma", "public");
                HttpContext.Current.Response.BinaryWrite(renderedBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.Response.Write(renderedBytes);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void DownloadPDFFile(RadFixedDocument document, string fileName)
        {
            try
            {
                PdfFormatProvider formatProvider = new PdfFormatProvider();

                byte[] renderedBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                     formatProvider.Export(document, ms);
                    renderedBytes = ms.ToArray();
                }

                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.ContentType = "text/csv";
                HttpContext.Current.Response.AddHeader("Pragma", "public");
                HttpContext.Current.Response.BinaryWrite(renderedBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.Response.Write(renderedBytes);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void DownloadTXTFile(MemoryStream ms, string fileName)
        {
            try
            {
                byte[] renderedBytes = ms.ToArray();

                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.ContentType = "text/csv";
                HttpContext.Current.Response.AddHeader("Pragma", "public");
                HttpContext.Current.Response.BinaryWrite(renderedBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.Response.Write(renderedBytes);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}