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
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            };

            return dataTable;
        }

        public DataTable GetClientRosterDataTable(Stream input)
        {
            SPColumn[] spc = new SPColumn[15]
            {
                new SPColumn("name", typeof(string)),
                new SPColumn("gender", typeof(string)),
                new SPColumn("dob", typeof(string)),
                new SPColumn("program_name", typeof(string)),
                new SPColumn("unit", typeof(string)),
                new SPColumn("worker_name", typeof(string)),
                new SPColumn("worker_role", typeof(string)),
                new SPColumn("is_primary_worker", typeof(string)),
                new SPColumn("current_location", typeof(string)),
                new SPColumn("current_phone_day", typeof(string)),
                new SPColumn("managing_office", typeof(string)),
                new SPColumn("medicaid_number", typeof(string)),
                new SPColumn("medicaid_payer", typeof(string)),
                new SPColumn("medicaid_plan_name", typeof(string)),
                new SPColumn("program_modifier", typeof(string))
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
                        values[0] = GetCellData(InputWorksheet, i, 1); //name
                        values[1] = GetCellData(InputWorksheet, i, 3); //gender
                        values[2] = GetCellData(InputWorksheet, i, 4); //dob
                        values[3] = GetCellData(InputWorksheet, i, 6); //program_name
                        values[4] = GetCellData(InputWorksheet, i, 14); //unit
                        values[5] = GetCellData(InputWorksheet, i, 20); //worker_name
                        values[6] = GetCellData(InputWorksheet, i, 21); //worker_role
                        values[7] = GetCellData(InputWorksheet, i, 24); //is_primary_worker
                        values[8] = GetCellData(InputWorksheet, i, 37); //current_location
                        values[9] = GetCellData(InputWorksheet, i, 38); //current_phone_day
                        values[10] = GetCellData(InputWorksheet, i, 67); //managing_office
                        values[11] = GetCellData(InputWorksheet, i, 76); //medicaid_number
                        values[12] = GetCellData(InputWorksheet, i, 77); //medicaid_payer
                        values[13] = GetCellData(InputWorksheet, i, 78); //medicaid_plan_name
                        values[14] = GetCellData(InputWorksheet, i, 91); //program_modifier

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