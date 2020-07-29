using System;
using System.IO;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Windows.Media;

namespace ConsumerMaster
{
    public class AWCBillingAuthorizationExceptionReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;
        
        string[] columnsName = new string[9] { "Activity ID", "Name", "Staff Name", "Start", "Finish", "BIndex", "Billing Code", "PIndex", "Payroll Code" };
        Type[] columnsType = new Type[9] { typeof(String), typeof(String), typeof(String), typeof(DateTime), typeof(DateTime), typeof(int), typeof(String), typeof(int), typeof(String) };

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

        public MemoryStream CreateDocument(UploadedFile uploadedFile)
        {
            Utility util = new Utility();
            Stream input = uploadedFile.InputStream;
            DataTable dTable = util.GetTimeAndDistanceDataTable(input);

            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("Mismatched Services – show unscheduled client visits where Billing Code and Payroll Code do NOT match");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("{0,-20} {1,-20} {2,-22} {3,-52} {4,-52}", "ClientName", "StaffName", "Start", "Billing Code", "Payroll Code");

                foreach (DataRow row in dTable.Rows)
                {
                    int billingCodeIndex = Array.FindIndex(billingCodeArray, m => m == row["Billing Code"].ToString());
                    int payrollCodeIndex = Array.FindIndex(payrollCodeArray, m => m == row["Payroll Code"].ToString());

                    bool isMatched = IsMatched(billingCodeIndex, payrollCodeIndex);

                    if (row["Activity Type"].ToString().Contains("UPV") && !isMatched)  //Unscheduled Patient Visit and Mismatched Services
                    {
                        streamWriter.WriteLine("{0,-20} {1,-20} {2,-22} [{3,-2}]{4,-47} [{5,-2}]{6,-47}", row["Name"].ToString(),
                            row["Staff Name"].ToString(), row["Start"].ToString(), billingCodeIndex.ToString("D2"), row["Billing Code"].ToString(), 
                            payrollCodeIndex.ToString("D2"), row["Payroll Code"].ToString());
                    }
                }

                streamWriter.Flush();
                return ms;
            }
        }

        bool IsMatched(int billingCodeIndex, int payrollCodeIndex)
        {
            bool isMatched;
            if (billingCodeIndex < 12)  //Billing Code from 0 to 11
                isMatched = billingCodeIndex == payrollCodeIndex ? true : false;
            else
            {
                if (billingCodeIndex == 12 && Enumerable.Range(12, 15).Contains(payrollCodeIndex))
                    isMatched = true;
                else 
                    isMatched = false;
            }
            return isMatched;
        }

        public Workbook CreateWorkbook(UploadedFile uploadedTDFile, UploadedFile uploadedBAFile)
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

                //new SPColumn("Staff ID", typeof(string)),
                //new SPColumn("Secondary Staff ID", typeof(string)),
                //new SPColumn("Staff Name", typeof(string)),
                //new SPColumn("Activity ID", typeof(string)),
                //new SPColumn("Activity Type", typeof(string)),
                //new SPColumn("ID", typeof(string)),
                //new SPColumn("Secondary ID", typeof(string)),
                //new SPColumn("Name", typeof(string)),
                //new SPColumn("Start", typeof(DateTime)),
                //new SPColumn("Finish", typeof(DateTime)),
                //new SPColumn("Duration", typeof(Int32)),
                //new SPColumn("Travel Time", typeof(string)),
                //new SPColumn("TSrc", typeof(string)),
                //new SPColumn("Distance", typeof(string)),
                //new SPColumn("DSrc", typeof(string)),
                //new SPColumn("Phone", typeof(string)),
                //new SPColumn("Billing Code", typeof(string)), //Billing Code
                //new SPColumn("Payroll Code", typeof(string)), //Payroll Code
                //new SPColumn("Service", typeof(string)),
                //new SPColumn("On-call", typeof(string)),
                //new SPColumn("Location", typeof(string)),
                //new SPColumn("Discipline", typeof(string))

                DataTable dTDTable = util.GetTDDataTable(inputTD);
                DataTable dBATable = util.GetBillingAuthorizationDataTable(inputBA);

                DataTable noMatchesTable = FindMatches(dTDTable, dBATable);

                int rowCount = Sheet1WorksheetHeader(sheet1Worksheet, columnsName, uploadedTDFile);

                int currentRow = IndexRowItemStart + rowCount;
                foreach (DataRow row in noMatchesTable.Rows)
                {
                    sheet1Worksheet.Cells[currentRow, 0].SetValue(row["Staff ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, 1].SetValue(row["Secondary Staff ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Staff Name"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Activity ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Activity Type"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Secondary ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Name"].ToString());

                    CellValueFormat dateCellValueFormat = new CellValueFormat("MM/dd/yyyy hh:mm AM/PM");
                    sheet1Worksheet.Cells[currentRow, 3].SetFormat(dateCellValueFormat);
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Start"].ToString());

                    sheet1Worksheet.Cells[currentRow, 3].SetFormat(dateCellValueFormat);
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Finish"].ToString());



                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Duration"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Travel Time"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["TSrc"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Distance"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["DSrc"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Phone"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Billing Code"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Payroll Code"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Service"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["On-call"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Location"].ToString());
                    sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Discipline"].ToString());

                    //CellValueFormat dateCellValueFormat = new CellValueFormat("MM/dd/yyyy hh:mm AM/PM");
                    sheet1Worksheet.Cells[currentRow, 3].SetFormat(dateCellValueFormat);
                    sheet1Worksheet.Cells[currentRow, 3].SetValue(row["Start"].ToString());

                    sheet1Worksheet.Cells[currentRow, 4].SetFormat(dateCellValueFormat);
                    sheet1Worksheet.Cells[currentRow, 4].SetValue(row["Finish"].ToString());

                    sheet1Worksheet.Cells[currentRow, 5].SetValue(row["BIndex"].ToString());
                    sheet1Worksheet.Cells[currentRow, 6].SetValue(row["Billing Code"].ToString());

                    sheet1Worksheet.Cells[currentRow, 7].SetValue(row["PIndex"].ToString());
                    sheet1Worksheet.Cells[currentRow, 8].SetValue(row["Payroll Code"].ToString());

                    currentRow++;
                }

                for (int i = 1; i < noMatchesTable.Columns.Count; i++)  //Start at 1 instead of 0
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

        public DataTable FindMatches(DataTable dTDTable, DataTable dBATable)
        {
            DataTable noMatchesTable = dTDTable.Clone();

            try
            {
                foreach (DataRow tdRow in dTDTable.Rows)
                {
                    string clientID = tdRow["ID"].ToString();
                    string payrollCode = tdRow["Payroll Code"].ToString();

                    String condition = String.Format("id_no = '" + clientID + "' AND service_name = '" + payrollCode + "'");
                    DataRow[] results = dBATable.Select(condition);
                    int matchesCount = results.Count();

                    if(matchesCount == 0)
                    {
                        noMatchesTable.ImportRow(tdRow);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return noMatchesTable;
        }

        private int Sheet1WorksheetHeader(Worksheet worksheet, string[] columnsList, UploadedFile uploadedFile)
        {
            int rowCount = 0;
            try
            {
                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 0, 0), Colors.Transparent);

                worksheet.Cells[rowCount, 0].SetIsBold(true);
                worksheet.Cells[rowCount++, 0].SetValue("Mismatched Services – show unscheduled client visits where Billing Code and Payroll Code do NOT match");
                worksheet.Cells[rowCount++, 0].SetValue(String.Format("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));
                worksheet.Cells[rowCount++, 0].SetValue(String.Format("Filename:{0}", uploadedFile.FileName));
                rowCount++;

                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column);
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