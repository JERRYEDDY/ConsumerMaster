﻿using System;
using System.IO;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Windows.Media;
using System.Collections.Generic;

namespace ConsumerMaster
{
    public class AWCEVVVisitsComparisonReportFile
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

        string[] sandataServiceArray = new string[8]
        {
            "Companion (1:1)",
            "IHCS Level 2 (1:1)",
            "IHCS Level 2 (1:1) Enhanced",
            "IHCS Level 3 (2:1)",
            "IHCS Level 3 (2:1) Enhanced",
            "Respite Level 3 (1:1)-Day",
            "Respite Level 3 (1:1)-15 Mins",
            "Respite Level 3 (1:1) Enhanced-15 Mins"
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

        public Workbook CreateWorkbook(UploadedFile uploadedNCSFile, UploadedFile uploadedSEVFile)
        {
            Workbook workbook = new Workbook();

            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                Worksheet sheet1Worksheet = worksheets["Sheet1"];

                Utility util = new Utility();
                Stream inputNCS = uploadedNCSFile.InputStream;
                Stream inputSEV = uploadedSEVFile.InputStream;


                DataTable ncsDataTable = util.GetNetsmartClientServicesDataTableViaCSV(inputNCS);
                DataTable sevDataTable = util.GetSandataExportVisitsDataTableViaCSV(inputSEV);


                //int blank = 0, no_blank = 0;

                //foreach (DataRow row in tadDataTable.Rows)
                //{
                //    string bCode = row["Billing Code"].ToString();
                //    string pCode = row["Payroll Code"].ToString();

                //    bool b = string.IsNullOrEmpty(bCode);
                //    bool p = string.IsNullOrEmpty(pCode);

                //    if(b & p) 
                //    {
                //        blank++;
                //    }
                //    else
                //    {
                //        no_blank++;
                //    }
                //}

                //List<EVVTransaction> evvTransactionList =
                //(from cca in ccaDataTable.AsEnumerable()
                // join tad in tadDataTable.AsEnumerable() on cca["Activity ID"] equals tad["Activity ID"]
                // select new EVVTransaction
                // {
                //     ActivityID = cca.Field<string>("Activity ID"),
                //     ClientID = cca.Field<string>("ID"),
                //     ClientName = cca.Field<string>("Activity Name"),
                //     StaffID = cca.Field<string>("Staff ID"),
                //     StaffName = cca.Field<string>("Executed By"),
                //     ActivityType = tad.Field<string>("Activity Type"),
                //     ActivitySource = cca.Field<string>("Activity Source"),
                //     BillingCode = tad.Field<string>("Billing Code"),
                //     PayrollCode = tad.Field<string>("Payroll Code"),
                //     Start = tad.Field<DateTime>("Start"),
                //     Finish = tad.Field<DateTime>("Finish"),
                //     Duration = tad.Field<Int32>("Duration"),
                //     Alerts = cca.Field<string>("Alerts"),
                //     Status = cca.Field<string>("Status")
                // }).ToList();

                //DataTable evvTransactions = util.ConvertToDataTable(evvTransactionList);

                //int rowCount = rowCount = Sheet1WorksheetHeader(sheet1Worksheet, evvTransactions);

                //int currentRow = IndexRowItemStart + rowCount;
                //foreach (DataRow row in evvTransactions.Rows)
                //{
                //    int column = 0;

                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["ActivityID"].ToString());

                //    CellValueFormat textFormat = new CellValueFormat("@");
                //    sheet1Worksheet.Cells[currentRow, column].SetFormat(textFormat);
                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["ClientID"].ToString());

                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["ClientName"].ToString());
                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["StaffID"].ToString());
                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["StaffName"].ToString());

                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["ActivityType"].ToString());
                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["ActivitySource"].ToString());
                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["BillingCode"].ToString());
                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["PayrollCode"].ToString());

                //    CellValueFormat dateTimeFormat = new CellValueFormat("MM/dd/yyyy hh:mm AM/PM");
                //    sheet1Worksheet.Cells[currentRow, column].SetFormat(dateTimeFormat);
                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Start"].ToString());

                //    sheet1Worksheet.Cells[currentRow, column].SetFormat(dateTimeFormat);
                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Finish"].ToString());

                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Duration"].ToString());
                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Alerts"].ToString());
                //    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Status"].ToString());

                //    currentRow++;
                //}

                //for (int i = 1; i < sevDataTable.Columns.Count; i++)  //Start at 1 instead of 0
                //{
                //    sheet1Worksheet.Columns[i].AutoFitWidth();
                //}
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return workbook;
        }

        private int Sheet1WorksheetHeader(Worksheet worksheet, DataTable dataTable)
        {
            int rowCount = 0;
            try
            {
                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255, 255, 0, 0), Colors.Transparent);
                worksheet.Cells[rowCount, 0].SetIsBold(true);
                worksheet.Cells[rowCount++, 0].SetValue("AWC EVV Visits Comparison Report");
                worksheet.Cells[rowCount++, 0].SetValue(String.Format("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));
                rowCount++;

                //worksheet.Cells[rowCount, 0].SetIsBold(true);
                //worksheet.Cells[rowCount, 0].SetValue("Visit Status");
                //worksheet.Cells[rowCount, 1].SetIsBold(true);
                //worksheet.Cells[rowCount++, 1].SetValue("Visit Description");

                //worksheet.Cells[rowCount, 0].SetValue("In Process");
                //worksheet.Cells[rowCount++, 1].SetValue("A visit has started and not yet completed");
                //worksheet.Cells[rowCount, 0].SetValue("Incomplete");
                //worksheet.Cells[rowCount++, 1].SetValue("A visit has exceeded a 24-hr. period and is still missing a call-in/call-out");
                //worksheet.Cells[rowCount, 0].SetValue("Verified");
                //worksheet.Cells[rowCount++, 1].SetValue("A visit that does not contain any exceptions");
                //worksheet.Cells[rowCount, 0].SetValue("Processed");
                //worksheet.Cells[rowCount++, 1].SetValue("A visit that does not contain any exceptions and has been returned to the claims validation engine at least once");
                //worksheet.Cells[rowCount, 0].SetValue("Omit");
                //worksheet.Cells[rowCount++, 1].SetValue("A visit that is marked \"Do Not Bill\"");
                rowCount++;

                string[] columnNames = dataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
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

        public class EVVTransaction
        {
            public string ActivityID { get; set; } //Closed Activities Activity ID
            public string ClientID { get; set; } //Closed Activities ID
            public string ClientName { get; set; } //Closed Activities Activity Name
            public string StaffID { get; set; } // Closed Activities Staff ID
            public string StaffName { get; set; } // Closed Activities Executed By
            public string ActivityType { get; set; }
            public string ActivitySource { get; set; }
            public string BillingCode { get; set; }
            public string PayrollCode { get; set; }
            public DateTime Start { get; set; }
            public DateTime Finish { get; set; }
            public int Duration { get; set; }
            public string Alerts { get; set; }
            public string Status { get; set; }
        }
    }
}