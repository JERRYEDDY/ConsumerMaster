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

                    using (var csv = new CsvReader(reader))
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
            String[] columns = new string[19] { "P_ACTIVE", "P_EMPNO ", "P_FNAME ", "P_LNAME ", "P_MI ", "P_BIRTH", "P_SSN", "P_SEX ", "P_EMPEMAIL", "P_JOBCODE ", "P_JOBTITLE ", "P_RACE ", "P_LASTHIRE", "P_HCITY", "P_HSTATE", "P_HZIP", "P_HSTREET1", "P_HSTREET2", "P_HCOUNTY" };
            DataTable dataTable = new DataTable();

            try
            {
                using (var reader = new StreamReader(file.InputStream))
                using (var csv = new CsvReader(reader))
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

        public DataTable GetClientAuthorizationDataTable(Stream input)
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
                new SPColumn("date_from_details", typeof(string)),
                new SPColumn("date_to_details", typeof(string)),
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
                for (int i = 0; i < spc.Count(); i++)
                {
                    CellSelection selection = InputWorksheet.Cells[0, i];
                    var columnName = "Column" + (i + 1);
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }

                for (int i = 1; i < InputWorksheet.UsedCellRange.RowCount; i++)
                {
                    var values = new object[spc.Count()];
                    values[0] = GetCellData(InputWorksheet, i, 0);
                    values[1] = GetCellData(InputWorksheet, i, 1);
                    values[2] = GetCellData(InputWorksheet, i, 2);
                    values[3] = GetCellData(InputWorksheet, i, 3);
                    values[4] = GetCellData(InputWorksheet, i, 4);
                    values[5] = GetCellData(InputWorksheet, i, 5);
                    values[6] = GetCellData(InputWorksheet, i, 6);
                    values[7] = GetCellData(InputWorksheet, i, 7);
                    values[8] = GetCellData(InputWorksheet, i, 8);
                    values[9] = GetCellData(InputWorksheet, i, 9);

                    values[10] = GetCellData(InputWorksheet, i, 10); //from_date
                    values[11] = GetCellData(InputWorksheet, i, 11); //to_date
                    values[12] = GetCellData(InputWorksheet, i, 12); //date_from_details
                    values[13] = GetCellData(InputWorksheet, i, 13); //date_to_details

                    values[14] = GetCellData(InputWorksheet, i, 14);
                    values[15] = GetCellData(InputWorksheet, i, 15);
                    values[16] = GetCellData(InputWorksheet, i, 16);
                    values[17] = GetCellData(InputWorksheet, i, 17);
                    values[18] = GetCellData(InputWorksheet, i, 18);
                    values[19] = GetCellData(InputWorksheet, i, 19);
                    values[20] = GetCellData(InputWorksheet, i, 20);
                    values[21] = GetCellData(InputWorksheet, i, 21);
                    values[22] = GetCellData(InputWorksheet, i, 22);
                    values[23] = GetCellData(InputWorksheet, i, 23);
                    values[24] = GetCellData(InputWorksheet, i, 24);
                    values[25] = GetCellData(InputWorksheet, i, 25);
                    values[26] = GetCellData(InputWorksheet, i, 26);
                    values[27] = GetCellData(InputWorksheet, i, 27);
                    values[28] = GetCellData(InputWorksheet, i, 28);
                    values[29] = GetCellData(InputWorksheet, i, 29);
                    values[30] = GetCellData(InputWorksheet, i, 30);
                    values[31] = GetCellData(InputWorksheet, i, 31);
                    values[32] = GetCellData(InputWorksheet, i, 32);
                    values[33] = GetCellData(InputWorksheet, i, 33);
                    values[34] = GetCellData(InputWorksheet, i, 34);
                    values[35] = GetCellData(InputWorksheet, i, 35);
                    values[36] = GetCellData(InputWorksheet, i, 36);
                    values[37] = GetCellData(InputWorksheet, i, 37);
                    values[38] = GetCellData(InputWorksheet, i, 38);
                    values[39] = GetCellData(InputWorksheet, i, 39);
                    values[40] = GetCellData(InputWorksheet, i, 40);
                    values[41] = GetCellData(InputWorksheet, i, 41);
                    values[42] = GetCellData(InputWorksheet, i, 42);
                    values[43] = GetCellData(InputWorksheet, i, 43);
                    values[44] = GetCellData(InputWorksheet, i, 44);
                    values[45] = GetCellData(InputWorksheet, i, 45);
                    values[46] = GetCellData(InputWorksheet, i, 46);
                    values[47] = GetCellData(InputWorksheet, i, 47);
                    values[48] = GetCellData(InputWorksheet, i, 48);
                    values[49] = GetCellData(InputWorksheet, i, 49);
                    values[50] = GetCellData(InputWorksheet, i, 50);
                    values[51] = GetCellData(InputWorksheet, i, 51);
                    values[52] = GetCellData(InputWorksheet, i, 52);
                    values[53] = GetCellData(InputWorksheet, i, 53);
                    values[54] = GetCellData(InputWorksheet, i, 54);
                    values[55] = GetCellData(InputWorksheet, i, 55);
                    values[56] = GetCellData(InputWorksheet, i, 56);

                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            };
        }

        public DataTable GetTimeAndDistanceDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[20]
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
                new SPColumn("Service", typeof(string)),
                new SPColumn("On-call", typeof(string)),
                new SPColumn("Location", typeof(string)),
                new SPColumn("Discipline", typeof(string))
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
                    values[0] = GetCellData(InputWorksheet, i, 0); //Staff ID
                    values[1] = GetCellData(InputWorksheet, i, 1); //Secondary Staff ID
                    values[2] = GetCellData(InputWorksheet, i, 2); //Activity ID
                    values[3] = GetCellData(InputWorksheet, i, 3); //Secondary Staff ID
                    values[4] = GetCellData(InputWorksheet, i, 4); //Activity Type
                    values[5] = GetCellData(InputWorksheet, i, 5); //ID
                    values[6] = GetCellData(InputWorksheet, i, 6); //Secondary ID"

                    string name = GetCellData(InputWorksheet, i, 7);
                    values[7] = name.Replace("\"", ""); //Name

                    string combinedStart = GetCellData(InputWorksheet, i, 8) + " " + GetCellData(InputWorksheet, i, 9);
                    DateTime startDate = Convert.ToDateTime(combinedStart);
                    values[8] = startDate; //Start

                    string combinedFinish = GetCellData(InputWorksheet, i, 11) + " " + GetCellData(InputWorksheet, i, 12);
                    DateTime finishDate = Convert.ToDateTime(combinedFinish);
                    values[9] = finishDate; //Finish

                    string durationStr = GetCellData(InputWorksheet, i, 14);
                    values[10] = int.Parse(durationStr, System.Globalization.NumberStyles.AllowThousands);  //Duration

                    values[11] = GetCellData(InputWorksheet, i, 15); //Travel Time
                    values[12] = GetCellData(InputWorksheet, i, 16); //TSrc
                    values[13] = GetCellData(InputWorksheet, i, 17); //Distance
                    values[14] = GetCellData(InputWorksheet, i, 18); //DSrc
                    values[15] = GetCellData(InputWorksheet, i, 19); //Phone
                    values[16] = GetCellData(InputWorksheet, i, 20); //Service
                    values[17] = GetCellData(InputWorksheet, i, 21); //On-call
                    values[18] = GetCellData(InputWorksheet, i, 22); //Location
                    values[19] = GetCellData(InputWorksheet, i, 23); //Discipline"

                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return dataTable;
            };
        }
  
        string GetCellData(Worksheet worksheet, int i, int j)
        {
            CellSelection selection = worksheet.Cells[i, j];

            ICellValue value = selection.GetValue().Value;
            CellValueFormat format = selection.GetFormat().Value;
            CellValueFormatResult formatResult = format.GetFormatResult(value);
            string result = formatResult.InfosText;
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