using System;
using System.IO;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Windows.Media;
using System.Text;

namespace ConsumerMaster
{
    public class AWCServicesExceptionReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;

        string[] billingCodeArray = new string[13]
        {
            "ODP / W1726 / Companion W/B",
            "ODP / W1726:U4 / Companion W/O",
            "ODP/ W7061 / H&C 1:1 Degreed Staff",
            "ODP / W7060 / H&C 1:1 W/B",
            "ODP / W7060:U4 / H&C 1:1 W/O",
            "ODP / W7069 / H&C 2:1 Enhanced W/B",
            "ODP / W7068 / H&C 2:1 W/B",
            "ODP / W9863 / Respite 1:1 Enhanced 15 min W/B",
            "ODP / W9862 / Respite 15 min W/B",
            "ODP / W9862:U4 / Respite 15 min W/O",
            "ODP / W9798 / Respite 24HR W/B",
            "ODP / W9798:U4/ Respite 24 HR W/O",
            "Supported Employment (All)"
        };

        string[] payrollCodeArray = new string[16]
        {
            "Companion W/B (W1726)",
            "Companion W/O (W1726:U4)",
            "H&C 1:1 Degreed Staff (W7061)",
            "H&C 1:1 W/B (W7060)",
            "H&C 1:1 W/O (W7060:U4)",
            "H&C 2:1 Enhanced W/B (W7069)",
            "H&C 2:1 W/B (W7068)",
            "Respite 1:1 Enhanced 15 min W/B (W9863)",
            "Respite 15 min W/B (W9862)",
            "Respite 15 min W/O (W9862:U4)",
            "Respite 24HR W/B (W9798)",
            "Respite 24 HR W/O (W9798:U4)",
            "SE Career Assessment W/B (W7235)",
            "SE Job Coach W/B (W9794)",
            "SE Job Coach W/O (W9794:U4)",
            "SE Job Find W/B (H2023)"
        };

        string[] serviceCodeArray = new string[16]
        {
            "(W1726): Companion W/B",
            "(W1726:U4): Companion W/O",
            "(W7061): H&C 1:1 Degreed Staff",
            "(W7060): H&C 1:1 W/B",
            "(W7060:U4): H&C 1:1 W/O",
            "(W7069): H&C 2:1 Enhanced W/B",
            "(W7068): H&C 2:1 W/B",
            "(W9863):Respite 1:1 Enhanced 15 min W/B",
            "(W9862):Respite 15 min W/B",
            "(W9862:U4):Respite 15 min W/O",
            "(W9798):Respite 24HR W/B",
            "(W9798:U4):Respite 24 HR W/O",
            "(W7235):SE Career Assessment W/B",
            "(W9794): SE Job Coach W/B",
            "(W9794:U4):SE Job Coach W/O",
            "(H2023):SE Job Find W/B"
        };

        public Workbook CreateWorkbook(UploadedFile uploadedTDFile, UploadedFile uploadedBAFile, UploadedFile uploadedALFile)
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];

                Utility util = new Utility();
                Stream inputTD = uploadedTDFile.InputStream;
                Stream inputBA = uploadedBAFile.InputStream;
                Stream inputAL = uploadedALFile.InputStream;

                DataTable tempTable = util.GetUPVTDDataTable(inputTD);
                tempTable.DefaultView.Sort = "Name, Start";
                DataTable dUPVTDTable = tempTable.DefaultView.ToTable();  //Sort by Client Name and Start DateTime

                DataTable dBATable = util.GetBillingAuthorizationDataTable(inputBA);

                DataTable exceptionsTable = FindAllExceptions(dUPVTDTable, dBATable);

                DataTable tmpALTable = util.GetAuditLogDataTable(inputAL);
                var result = tmpALTable.AsEnumerable().GroupBy(x => new { ActivityID = x["Activity ID"], ID = x["ID"] })
                    .Select(item => new
                    {
                        ActivityID = (string)item.Key.ActivityID,
                        ID = (string)item.Key.ID,
                        Action = string.Join(" ; ", item.Select(c => c["Action"]))
                    }).ToList();
                DataTable mergedAuditLogDataTable = result.ToDataTable();

                var JoinResult = (from e in mergedAuditLogDataTable.AsEnumerable()
                                  join m in mergedAuditLogDataTable.AsEnumerable() 
                                  on e.Field<string>("Activity ID") equals m.Field<string>("Activity ID") into tempJoin
                                  from leftJoin in tempJoin.DefaultIfEmpty()
                                  select new
                                  {
                                      //id_no = c.Field<string>("id_no"),
                                      //name = c.Field<string>("name"),
                                      //gender = c.Field<string>("gender"),
                                      //dob = c.Field<string>("dob"),
                                      //current_location = c.Field<string>("current_location"),
                                      //current_phone_day = c.Field<string>("current_phone_day"),
                                      //intake_date = c.Field<string>("intake_date"),
                                      //managing_office = c.Field<string>("managing_office"),
                                      //program_name = c.Field<string>("program_name"),
                                      //unit = c.Field<string>("unit"),
                                      //program_modifier = c.Field<string>("program_modifier"),
                                      //worker_name = c.Field<string>("worker_name"),
                                      //worker_role = c.Field<string>("worker_role"),
                                      //is_primary_worker = c.Field<string>("is_primary_worker"),
                                      //medicaid_number = c.Field<string>("medicaid_number"),
                                      //medicaid_payer = c.Field<string>("medicaid_payer"),
                                      //medicaid_plan_name = c.Field<string>("medicaid_plan_name"),
                                      //ba_count = leftJoin == null ? null : leftJoin.Field<string>("ba_count"),
                                      //me_count = leftJoin == null ? null : leftJoin.Field<string>("me_count"),
                                      //ssp_count = leftJoin == null ? null : leftJoin.Field<string>("ssp_count")
                                  }).ToList();



                string[] exceptionColumnNames = exceptionsTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

                int rowCount = Sheet1WorksheetHeader(sheet1Worksheet, exceptionColumnNames, uploadedTDFile.FileName, uploadedBAFile.FileName);
                int currentRow = IndexRowItemStart + rowCount;
                foreach (DataRow row in exceptionsTable.Rows)
                {
                    int column = 0;

                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Name"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Activity ID"].ToString());

                    CellValueFormat dateCellValueFormat = new CellValueFormat("MM/dd/yyyy hh:mm AM/PM");
                    sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Start"].ToString());

                    sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Finish"].ToString());

                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Duration"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Billing Code"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Payroll Code"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Service"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Exception"].ToString());

                    currentRow++;
                }

                for (int i = 1; i < exceptionsTable.Columns.Count; i++)  //Start at 1 instead of 0
                {
                    sheet1Worksheet.Columns[i].AutoFitWidth();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }

        public DataTable BuildExceptionsDataTable()
        {
            SPColumn[] spc = new SPColumn[10]
            {
                new SPColumn("ID", typeof(string)),
                new SPColumn("Name", typeof(string)),
                new SPColumn("Activity ID", typeof(string)),
                new SPColumn("Start", typeof(DateTime)),
                new SPColumn("Finish", typeof(DateTime)),
                new SPColumn("Duration", typeof(Int32)),
                new SPColumn("Billing Code", typeof(string)), //Billing Code
                new SPColumn("Payroll Code", typeof(string)), //Payroll Code
                new SPColumn("Service", typeof(string)), //Service
                new SPColumn("Exception", typeof(string))
            };

            DataTable dataTable = new DataTable();
            try
            {
                for (int i = 0; i < spc.Count(); i++)
                {
                    dataTable.Columns.Add(spc[i].name, spc[i].type);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return dataTable;
        }

        public DataTable FindAllExceptions(DataTable dTDTable, DataTable dBATable)
        {
            DataTable exceptionsTable = BuildExceptionsDataTable();

            try
            {
                foreach (DataRow tdRow in dTDTable.Rows)
                {
                    string clientID = tdRow["ID"].ToString();
                    string payrollCode = tdRow["Payroll Code"].ToString();
                    string billingCode = tdRow["Billing Code"].ToString();
                    string serviceCode = tdRow["Service"].ToString();

                    int billingCodeIndex = Array.FindIndex(billingCodeArray, m => m == billingCode);
                    int payrollCodeIndex = Array.FindIndex(payrollCodeArray, m => m == payrollCode);
                    int serviceCodeIndex = Array.FindIndex(serviceCodeArray, m => m == serviceCode);

                    StringBuilder exceptionsString = new StringBuilder();

                    //if(serviceCodeIndex == -1)
                    //{
                    //    string pCode = payrollCodeArray[serviceCodeIndex].ToString();
                    //    String condition = String.Format("id_no = '" + clientID + "' AND service_name = '" + pCode + "'");
                    //    DataRow[] results = dBATable.Select(condition);
                    //    int noBillingAuthorizationCount = results.Count();  //NO Billing Authorization;
                    //    if (noBillingAuthorizationCount == 0)
                    //        exceptionsString.Append("NO BILLING Authorization;");
                    //}
                    //else
                    //{



                    //}

                    bool servicesMismatched = IsServicesMisMatched(payrollCodeIndex, billingCodeIndex); //Payroll/Billing Code Mismatched;
                    if (servicesMismatched)
                        exceptionsString.Append("Payroll/Billing Code Mismatched; ");

                    //String condition = String.Format("id_no = '" + clientID + "' AND service_name = '" + payrollCode + "'");
                    //DataRow[] results = dBATable.Select(condition);

                    //int noBillingAuthorizationCount = results.Count();  //NO Billing Authorization;
                    //if (noBillingAuthorizationCount == 0)
                    //    exceptionsString.Append("NO Billing Authorization;");
                    //if (noBillingAuthorizationCount == 0 || servicesMismatched)

                    if (servicesMismatched)
                    {
                        int vIndex = 0;
                        var values = new object[exceptionsTable.Columns.Count];

                        values[vIndex++] = tdRow["ID"].ToString();
                        values[vIndex++] = tdRow["Name"].ToString();
                        values[vIndex++] = tdRow["Activity ID"].ToString();
                        values[vIndex++] = tdRow["Start"];
                        values[vIndex++] = tdRow["Finish"];
                        values[vIndex++] = tdRow["Duration"];
                        values[vIndex++] = billingCodeIndex != -1 ? string.Format("[{0}]{1}", billingCodeIndex.ToString(), tdRow["Billing Code"].ToString()) : "";
                        values[vIndex++] = payrollCodeIndex != -1 ? string.Format("[{0}]{1}", payrollCodeIndex.ToString(), tdRow["Payroll Code"].ToString()) : "";
                        values[vIndex++] = serviceCodeIndex != -1 ? string.Format("[{0}]{1}", serviceCodeIndex.ToString(), tdRow["Service"].ToString()) : "";
                        values[vIndex++] = exceptionsString;  //Exception

                        exceptionsTable.Rows.Add(values);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return exceptionsTable;
        }

        bool IsServicesMisMatched(int payrollCodeIndex, int billingCodeIndex) 
        {
            bool isMisMatched;

            if (billingCodeIndex < 12)  //Billing Code from 0 to 11
                isMisMatched = payrollCodeIndex != billingCodeIndex ? true : false;
            else
            {
                if (billingCodeIndex == 12 && Enumerable.Range(12, 15).Contains(payrollCodeIndex))
                    isMisMatched = false;
                else
                    isMisMatched = false;
            }
            return isMisMatched;
        }

        private int Sheet1WorksheetHeader(Worksheet worksheet, string[] columnNames, string uploadedTDFileName, string uploadedBAFilename)
        {
            int rowCount = 0;
            try
            {
                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 0, 0), Colors.Transparent);

                worksheet.Cells[rowCount, 0].SetIsBold(true);
                worksheet.Cells[rowCount++, 0].SetValue("AWC Services Exception Report – Payroll/Billing Code Mismatched and NO Billing Authorization");
                worksheet.Cells[rowCount++, 0].SetValue(String.Format("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));
                worksheet.Cells[rowCount++, 0].SetValue(String.Format("Filename:{0}; {1}", uploadedTDFileName, uploadedBAFilename));
                rowCount++;

                foreach (string column in columnNames)
                {
                    int columnKey = Array.IndexOf(columnNames, column);
                    string columnName = column;

                    CellIndex cellIndex = new CellIndex(rowCount, columnKey);
                    CellSelection cellSelection = worksheet.Cells[cellIndex];
                    cellSelection.SetIsBold(true);
                    cellSelection.SetUnderline(UnderlineType.Single);
                    cellSelection.SetHorizontalAlignment(RadHorizontalAlignment.Center);

                    worksheet.Cells[rowCount, columnKey].SetValue(columnName);
                }

                rowCount++;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return rowCount;
        }
    }
}