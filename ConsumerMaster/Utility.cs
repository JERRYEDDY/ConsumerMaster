using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Csv;
using Telerik.Windows.Documents.Spreadsheet.Model;
using CsvHelper;
using Telerik.Web.UI;

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
            String[] columns = new string[19] {"P_ACTIVE", "P_EMPNO ", "P_FNAME ", "P_LNAME ", "P_MI ", "P_BIRTH", "P_SSN", "P_SEX ", "P_EMPEMAIL", "P_JOBCODE ", "P_JOBTITLE ", "P_RACE ", "P_LASTHIRE", "P_HCITY", "P_HSTATE", "P_HZIP", "P_HSTREET1", "P_HSTREET2", "P_HCOUNTY" };
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
                                if(columns.Length != csv.Context.Record.Length)
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

                //HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}