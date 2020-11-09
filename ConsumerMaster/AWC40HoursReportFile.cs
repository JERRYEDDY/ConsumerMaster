using System;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace ConsumerMaster
{
    public class AWC40HoursReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Workbook CreateWorkbook(UploadedFile uploadedFile)
        {
            Workbook workbook = new Workbook();

            try
            {
                Utility util = new Utility();
                Stream input = uploadedFile.InputStream;

                DataTable dTable = util.GetTimeAndDistanceDataTableViaCSV(input);
                //DataTable dTable = util.GetTimeAndDistanceDataTable(input);

                var query = from row in dTable.AsEnumerable()
                            group row by new
                            {
                                StaffID = row.Field<string>("Staff ID"),
                                StaffName = row.Field<string>("Staff Name")
                            }
                into TD
                            where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 40.00
                            orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
                            select new
                            {
                                ID = TD.Key.StaffID,
                                Name = TD.Key.StaffName,
                                Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
                            };

                DataTable reportResultSet = BuildReportResultSet();
                foreach (var staff in query)
                {
                    string name = staff.Name.Replace("\t", "");
                    string hours = staff.Hours.ToString("F"); 
                    reportResultSet.Rows.Add(staff.ID, name.Trim(), hours);
                }

                ExcelReportFormat reportFormat = new ExcelReportFormat();
                reportFormat.Header1 = "40 hours per week limit – show anyone who worked over 40 hours";
                reportFormat.Header2 = String.Format("Date/time: {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                reportFormat.Header3 = String.Format("Filename: {0}", uploadedFile.FileName);
                reportFormat.Header4 = " ";
                workbook = reportFormat.LoadWorkbook(reportResultSet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return workbook;
        }

        public MemoryStream CreateDocument(UploadedFile uploadedFile)
        {
            Utility util = new Utility();
            Stream input = uploadedFile.InputStream;
            DataTable dTable = util.GetTimeAndDistanceDataTable(input);

            var query = from row in dTable.AsEnumerable()
                        group row by new
                        {
                            StaffID = row.Field<string>("Staff ID"),
                            StaffName = row.Field<string>("Staff Name")
                        }
            into TD
                        where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 40.00
                        orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
                        select new
                        {
                            ID = TD.Key.StaffID,
                            Name = TD.Key.StaffName,
                            Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
                        };


            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("40 hours per week limit – show anyone who worked over 40 hours");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("{0,-10} {1,-30} {2}", "StaffID", "StaffName", "Hours");
                // print result
                foreach (var staff in query)
                {
                    string name = staff.Name.Replace("\t", "");
                    streamWriter.WriteLine("{0,-10} {1,-30} {2:0.00}", staff.ID, name.Trim(), staff.Hours);
                }

                streamWriter.Flush();
                return ms;
            }
        }

        public DataTable BuildReportResultSet()
        {
            DataTable reportResultSet = new DataTable();
            reportResultSet.Columns.Add("StaffID", typeof(string));
            reportResultSet.Columns.Add("StaffName", typeof(string));
            reportResultSet.Columns.Add("Duration", typeof(string));

            return reportResultSet;
        }
    }
}