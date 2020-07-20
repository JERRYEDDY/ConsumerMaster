using System;
using System.IO;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Windows.Media;
using Telerik.Documents.SpreadsheetStreaming;

namespace ConsumerMaster
{
    public class AWCMismatchedServicesReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int IndexRowItemStart = 0;
        //private static readonly int IndexColumnName = 0;

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

                    //int testBilling = 12;
                    //int testPayroll = 12;

                    //for (int i = 0; i < 4; i++)
                    //{
                    //    int testP = testPayroll + i;
                    //    bool testMatched = IsMatched(testBilling, testP);
                    //}

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
            if (billingCodeIndex < 12)  //Billing Code Services from 0 to 11
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
        public Workbook CreateWorkbook(UploadedFile uploadedFile)
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];

                Utility util = new Utility();
                Stream input = uploadedFile.InputStream;
                DataTable dTable = util.GetTimeAndDistanceDataTable(input);

                string[] columnsList = new string[5] {"Name", "Staff Name", "Start", "Billing Code", "Payroll Code"};
                int rowCount = Sheet1WorksheetHeader(sheet1Worksheet, columnsList, uploadedFile);

                int currentRow = IndexRowItemStart + rowCount;
                foreach (DataRow row in dTable.Rows)
                {
                    int billingCodeIndex = Array.FindIndex(billingCodeArray, m => m == row["Billing Code"].ToString());
                    int payrollCodeIndex = Array.FindIndex(payrollCodeArray, m => m == row["Payroll Code"].ToString());

                    bool isMatched = IsMatched(billingCodeIndex, payrollCodeIndex);

                    if (row["Activity Type"].ToString().Contains("UPV") && !isMatched)  //Unscheduled Patient Visit and Mismatched Services
                    {
                        sheet1Worksheet.Cells[currentRow, 0].SetValue(row["Name"].ToString());
                        sheet1Worksheet.Cells[currentRow, 1].SetValue(row["Staff Name"].ToString());

                        CellValueFormat dateCellValueFormat = new CellValueFormat("MM/dd/yyyy hh:mm AM/PM");
                        sheet1Worksheet.Cells[currentRow, 2].SetFormat(dateCellValueFormat);
                        sheet1Worksheet.Cells[currentRow, 2].SetValue(row["Start"].ToString());

                        sheet1Worksheet.Cells[currentRow, 3].SetValue(row["Billing Code"].ToString());
                        sheet1Worksheet.Cells[currentRow, 4].SetValue(row["Payroll Code"].ToString());

                        currentRow++;
                    }

                }

                for (int i = 0; i < dTable.Columns.Count; i++)
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