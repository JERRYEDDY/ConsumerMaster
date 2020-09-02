using System;
using System.IO;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Windows.Media;
using System.Text;
using System.Collections.Generic;

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

        public Workbook CreateWorkbook(UploadedFile uploadedTDFile, UploadedFile uploadedBAFile, UploadedFile uploadedHBAFile)
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
                Stream inputHBA = uploadedHBAFile.InputStream;

                DataTable tempTable = util.GetUPVTDDataTable(inputTD);
                tempTable.DefaultView.Sort = "Name, Start";
                DataTable dUPVTDTable = tempTable.DefaultView.ToTable();  //Sort by Client Name and Start DateTime

                DataTable dBATable = util.GetBillingAuthorizationDataTable(inputBA);

                DataTable tmpTable = util.GetClientIDsDataTable(inputBA);
                DataTable dClientIDsTable = util.RemoveDuplicateRows(tmpTable, "id_no");
                DataTable dHBATable = util.GetHCSISDataTable(inputHBA, dClientIDsTable);

                DataTable exceptionsTable = FindAllExceptions(dUPVTDTable, dBATable, dHBATable);
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

                    ThemableColor textColor = new ThemableColor(Colors.Red);
                    sheet1Worksheet.Cells[currentRow, column].SetForeColor(textColor);
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Exception"].ToString());

                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["NS Billing Auth"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["HCSIS Billing Auth"].ToString());

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
            SPColumn[] spc = new SPColumn[12]
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
                new SPColumn("Exception", typeof(string)),
                new SPColumn("NS Billing Auth", typeof(string)),
                new SPColumn("HCSIS Billing Auth", typeof(string))
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

        public DataTable FindAllExceptions(DataTable dTDTable, DataTable dBATable, DataTable dHBATable)
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
                    bool servicesMismatched = IsServicesMisMatched(payrollCodeIndex, billingCodeIndex); //Payroll/Billing Code Mismatched;
                    if (servicesMismatched)
                    {
                        exceptionsString.Append("P/B CODE MISMATCHED; ");
                    }


                    int noBillingAuthorizationCount = 0;
                    if(serviceCodeIndex == -1)
                    {
                        String condition = String.Format("id_no = '" + clientID + "' AND service_name = '" + payrollCode + "'");
                        DataRow[] results = dBATable.Select(condition);
                        noBillingAuthorizationCount = results.Count();  //NO Billing Authorization;
                    }
                    else
                    {
                        string pCode = payrollCodeArray[serviceCodeIndex].ToString();
                        String condition = String.Format("id_no = '" + clientID + "' AND service_name = '" + pCode + "'");
                        DataRow[] results = dBATable.Select(condition);
                        noBillingAuthorizationCount = results.Count();  //NO Billing Authorization;
                    }
                    if (noBillingAuthorizationCount == 0)
                        exceptionsString.Append("NO BillAuth;");


                    String BACheck = String.Format("id_no = '" + clientID + "'");
                    DataRow[] BAResults = dBATable.Select(BACheck);
                    DataRow[] HBAResults = dHBATable.Select(BACheck);

                    bool billingAuthorizationsMismatched = IsBillingAuthorizationsMisMatched(BAResults, HBAResults);
                    if (billingAuthorizationsMismatched)
                    {
                        exceptionsString.Append("HCSIS MISMATCHED");
                    }


                    if (noBillingAuthorizationCount == 0 || servicesMismatched || billingAuthorizationsMismatched)
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

                        var ba = new StringBuilder();
                        foreach (DataRow row in BAResults)
                        {
                            ba.Append(row[9].ToString());
                            ba.Append(";");
                        }
                        values[vIndex++] = ba;

                        if(billingAuthorizationsMismatched)
                        {
                            var hba = new StringBuilder();
                            foreach (DataRow row in HBAResults)
                            {
                                hba.Append(row[2].ToString());
                                hba.Append(";");
                            }
                            values[vIndex++] = hba;
                        }

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

        public List<string> GetBAList(DataRow[] dataRows, int col)
        {
            List<string> baList = new List<string>();
            foreach (DataRow row in dataRows)
            {
                baList.Add(row[col].ToString());
            }
            return baList; 
        }

        bool IsBillingAuthorizationsMisMatched(DataRow[] BAResults, DataRow[] HBAResults)
        {
            bool matched;

            List<string> baList = GetBAList(BAResults, 9);
            List<string> sortedBAList = baList.OrderBy(q => q).ToList();

            List<string> hbaList = GetBAList(HBAResults, 2);
            List<string> sortedHBAList = hbaList.OrderBy(q => q).ToList();

            if (sortedBAList.SequenceEqual(sortedHBAList))
            {
                matched = false;
            }
            else
            {
                matched = true;
            }

            return matched;
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
                    isMisMatched = true;
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
                worksheet.Cells[rowCount++, 0].SetValue("AWC Services Exception Report – Payroll/Billing Code Mismatched, NO Billing Authorization and HCSIS Mismatched");
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