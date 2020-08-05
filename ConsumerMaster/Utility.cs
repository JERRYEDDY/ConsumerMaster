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
            String[] columns = new string[19] { "P_ACTIVE", "P_EMPNO ", "P_FNAME ", "P_LNAME ", "P_MI ", "P_BIRTH", "P_SSN", "P_SEX ", "P_EMPEMAIL", "P_JOBCODE ", "P_JOBTITLE ", "P_RACE ", "P_LASTHIRE", "P_HCITY", "P_HSTATE", "P_HZIP", "P_HSTREET1", "P_HSTREET2", "P_HCOUNTY" };
            DataTable dataTable = new DataTable();

            try
            {
                using (TextReader reader = new StreamReader(file.InputStream))
                {
                    //var dataTable = new DataTable();
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

        public DataTable GetEmployeePersonnelDataTable2(UploadedFile file)
        {
            String[] columns = new string[19] { "P_ACTIVE", 
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
                                                                        //ClientName     
                        values[1] = GetCellData(InputWorksheet, i, 2);  //MemberID
                        values[2] = GetCellData(InputWorksheet, i, 3);  //MemberName
                        values[3] = GetCellData(InputWorksheet, i, 4);  //MemberRole
                                                                        //IsSupervisor
                                                                        //MemberStart

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

        //SPColumn[] GetSPColumns20()
        //{
        //    SPColumn[] spc = new SPColumn[20]
        //    {
        //        new SPColumn("Staff ID", typeof(string)),
        //        new SPColumn("Secondary Staff ID", typeof(string)),
        //        new SPColumn("Staff Name", typeof(string) ),
        //        new SPColumn("Activity ID", typeof(string)),
        //        new SPColumn("Activity Type", typeof(string)),
        //        new SPColumn("ID", typeof(string)),
        //        new SPColumn("Secondary ID", typeof(string)),
        //        new SPColumn("Name", typeof(string)),
        //        new SPColumn("Start", typeof(DateTime)),
        //        new SPColumn("Finish", typeof(DateTime)),
        //        new SPColumn("Duration", typeof(Int32)),
        //        new SPColumn("Travel Time", typeof(string)),
        //        new SPColumn("TSrc", typeof(string)),
        //        new SPColumn("Distance", typeof(string)),
        //        new SPColumn("DSrc", typeof(string)),
        //        new SPColumn("Phone", typeof(string)),
        //        new SPColumn("Service", typeof(string)),
        //        new SPColumn("On-call", typeof(string)),
        //        new SPColumn("Location", typeof(string)),
        //        new SPColumn("Discipline", typeof(string))
        //    };

        //    return spc;
        //}

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

        public DataTable GetAuditLogDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[10]
            {
                new SPColumn("Activity ID", typeof(string)),
                new SPColumn("ID", typeof(string)),
                new SPColumn("Date", typeof(DateTime)),
                new SPColumn("Who", typeof(string)),
                new SPColumn("Start Time", typeof(DateTime)),
                new SPColumn("Stop Time", typeof(DateTime)),
                new SPColumn("Action", typeof(string)),
                new SPColumn("To", typeof(string)),
                new SPColumn("From", typeof(string)),
                new SPColumn("Comment", typeof(string)),
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
                    string action = GetCellData(InputWorksheet, i, 4);
                    if (!action.StartsWith("Updated  Billing") && !action.StartsWith("Updated  Payroll")) //Action
                        continue;

                    var values = new object[spc.Count()];
                    string subjectString = GetCellData(InputWorksheet, i, 3); //Subject
                    string[] subjectSub = subjectString.Split('-');

                    Regex rg = new Regex(@"\ (.*)\ ");
                    string activityID = rg.Match(subjectSub[0]).Groups[1].Value;
                    values[0] = activityID;   //Activity ID

                    Regex rx = new Regex(@"\((.*)\,");
                    string clientID = rx.Match(subjectSub[2]).Groups[1].Value;
                    values[1] = clientID;    //ID

                    string dateStr = GetCellData(InputWorksheet, i, 0); 
                    DateTime tDate = Convert.ToDateTime(dateStr);
                    values[2] = tDate; //Date

                    values[3] = GetCellData(InputWorksheet, i, 1); //Who

                    DateTime[] startStopTime = Parse2StartStopTime(subjectSub[3], subjectSub[4]);
                    values[4] = startStopTime[0];  //Start Time
                    values[5] = startStopTime[1];  //Stop Time

                    string payrollAction = "Updated  Payroll Code from  H&C 1:1 Degreed Staff (W7061) to  H&C 1:1 W/B (W7060)";
                    string billingAction = "Updated  Billing Code from  ODP/ W7061 / H&C 1:1 Degreed Staff to  ODP / W7060 / H&C 1:1 W/B";


                    Regex from = new Regex("from(.*)to");
                    var result1 = from.Match(payrollAction);
                    var output1 = result1.Groups[1].ToString();

                    Regex to = new Regex("to(.*)$");
                    var result2 = to.Match(payrollAction);
                    var output2 = result2.Groups[1].ToString();


                    values[6] = GetCellData(InputWorksheet, i, 4); //Action
                    values[7] = GetCellData(InputWorksheet, i, 5); //Comment

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

                //for (int i = 0; i < spc.Count(); i++)
                //{
                //    CellSelection selection = InputWorksheet.Cells[0, i];
                //    var columnName = "Column" + (i + 1);
                //    dataTable.Columns.Add(spc[i].name, spc[i].type);
                //}

                dataTable.Columns.Add(spc[7].name, spc[7].type); //id_no
                dataTable.Columns.Add(spc[4].name, spc[4].type); //full_name
                dataTable.Columns.Add(spc[12].name, spc[12].type); //date_from_details
                dataTable.Columns.Add(spc[13].name, spc[13].type); //date_to_details
                dataTable.Columns.Add(spc[31].name, spc[31].type); //service_name
                dataTable.Columns.Add(spc[32].name, spc[32].type); //rate_description
                dataTable.Columns.Add(spc[23].name, spc[23].type); //detail_balance
                dataTable.Columns.Add(spc[3].name, spc[3].type); //is_expired

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    //if (GetCellData(InputWorksheet, i, 4) != "Added a Note to the Activity") //Action
                    //    continue;

                    var values = new object[8];
                    //values[0] = GetCellData(InputWorksheet, i, 0); //authorizations_id
                    //values[1] = GetCellData(InputWorksheet, i, 1); //authorization_details_id
                    //values[2] = GetCellData(InputWorksheet, i, 2); //authorization_type
                    values[7] = GetCellData(InputWorksheet, i, 3); //is_expired
                    values[1] = GetCellData(InputWorksheet, i, 4); //full_name
                    //values[5] = GetCellData(InputWorksheet, i, 5); //authorization_number
                    //values[6] = GetCellData(InputWorksheet, i, 6); //people_id
                    values[0] = GetCellData(InputWorksheet, i, 7); //id_no
                    //values[8] = GetCellData(InputWorksheet, i, 8); //medicaid_number
                    //values[9] = GetCellData(InputWorksheet, i, 9); //policy_num
                    //values[10] = GetCellData(InputWorksheet, i, 10); //from_date
                    //values[11] = GetCellData(InputWorksheet, i, 11); //to_date

                    string fromDateString = GetCellData(InputWorksheet, i, 12); //date_from_details
                    DateTime fromDate = Convert.ToDateTime(fromDateString);
                    values[2] = fromDate; //From Date

                    string toDateString = GetCellData(InputWorksheet, i, 13); //date_to_details
                    DateTime toDate = Convert.ToDateTime(toDateString);
                    values[3] = toDate; //To Date

                    //values[14] = GetCellData(InputWorksheet, i, 14); //benefits_assignments_id
                    //values[15] = GetCellData(InputWorksheet, i, 15); //payor_vendor_id
                    //values[16] = GetCellData(InputWorksheet, i, 16); //vendor_name
                    //values[17] = GetCellData(InputWorksheet, i, 17); //units_aut_header
                    //values[18] = GetCellData(InputWorksheet, i, 18); //units_used_header
                    //values[19] = GetCellData(InputWorksheet, i, 19); //header_balance
                    //values[20] = GetCellData(InputWorksheet, i, 20); //total_aut_detail
                    //values[21] = GetCellData(InputWorksheet, i, 21); //units_aut_detail
                    //values[22] = GetCellData(InputWorksheet, i, 22); //units_used_detail
                    values[6] = GetCellData(InputWorksheet, i, 23); //detail_balance
                    //values[24] = GetCellData(InputWorksheet, i, 24); //units_performed_header
                    //values[25] = GetCellData(InputWorksheet, i, 25); //units_sched_header
                    //values[26] = GetCellData(InputWorksheet, i, 26); //units_performed_detail
                    //values[27] = GetCellData(InputWorksheet, i, 27); //units_sched_detail
                    //values[28] = GetCellData(InputWorksheet, i, 28); //program_name
                    //values[29] = GetCellData(InputWorksheet, i, 29); //group_profile_type
                    //values[30] = GetCellData(InputWorksheet, i, 30); //profile_name
                    values[4] = GetCellData(InputWorksheet, i, 31); //service_name
                    values[5] = GetCellData(InputWorksheet, i, 32); //rate_description
                    //values[33] = GetCellData(InputWorksheet, i, 33); ////program_modifier_code
                    //values[34] = GetCellData(InputWorksheet, i, 34); //billing_payment_plan_id
                    //values[35] = GetCellData(InputWorksheet, i, 35); //over_procedure_code
                    //values[36] = GetCellData(InputWorksheet, i, 36); //procedure_code_id
                    //values[37] = GetCellData(InputWorksheet, i, 37); //billing_payment_plan_scheme_link_id
                    //values[38] = GetCellData(InputWorksheet, i, 38); //billing_service_bundle_id
                    //values[39] = GetCellData(InputWorksheet, i, 39); //service_bundle_name
                    //values[40] = GetCellData(InputWorksheet, i, 40); //is_billing
                    //values[41] = GetCellData(InputWorksheet, i, 41); //type_of_authorizations
                    //values[42] = GetCellData(InputWorksheet, i, 42); //staff_id
                    //values[43] = GetCellData(InputWorksheet, i, 43); //staff_name
                    //values[44] = GetCellData(InputWorksheet, i, 44); //amount_charged
                    //values[45] = GetCellData(InputWorksheet, i, 45); //over_procedure_code_amt
                    //values[46] = GetCellData(InputWorksheet, i, 46); //pa_location_code
                    //values[47] = GetCellData(InputWorksheet, i, 47); //school_district_id
                    //values[48] = GetCellData(InputWorksheet, i, 48); //school_district
                    //values[49] = GetCellData(InputWorksheet, i, 49); //school_district_code
                    //values[50] = GetCellData(InputWorksheet, i, 50); //dtFromDate
                    //values[51] = GetCellData(InputWorksheet, i, 51); //dtToDate
                    //values[52] = GetCellData(InputWorksheet, i, 52); //authorization_reason
                    //values[53] = GetCellData(InputWorksheet, i, 53); //authorization_message
                    //values[54] = GetCellData(InputWorksheet, i, 54); //client_facility_name
                    //values[55] = GetCellData(InputWorksheet, i, 55); //client_managing_office_name
                    //values[56] = GetCellData(InputWorksheet, i, 56); //is_extended

                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            };

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
                //formatProvider.ExportSettings.ImageQuality = ImageQuality.High;

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