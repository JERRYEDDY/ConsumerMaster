﻿using System;
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

        string[] columnsName = new string[13] { "Staff ID", "Staff Name", "Activity ID", "Activity Type", "ID", "Name", "Start", "Finish", "Duration", "Billing Code", "Payroll Code", "Service", "Discipline" };

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

                DataTable dTDTable = util.GetTDDataTable(inputTD);
                DataTable dBATable = util.GetBillingAuthorizationDataTable(inputBA);

                DataTable noMatchesTable = FindMatches(dTDTable, dBATable);

                int rowCount = Sheet1WorksheetHeader(sheet1Worksheet, columnsName, uploadedTDFile);

                int currentRow = IndexRowItemStart + rowCount;
                foreach (DataRow row in noMatchesTable.Rows)
                {
                    int column = 0;

                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Staff ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Staff Name"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Activity ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Activity Type"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["ID"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Name"].ToString());

                    CellValueFormat dateCellValueFormat = new CellValueFormat("MM/dd/yyyy hh:mm AM/PM");
                    sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Start"].ToString());

                    sheet1Worksheet.Cells[currentRow, column].SetFormat(dateCellValueFormat);
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Finish"].ToString());

                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Duration"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Billing Code"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Payroll Code"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Service"].ToString());
                    sheet1Worksheet.Cells[currentRow, column++].SetValue(row["Discipline"].ToString());

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
                worksheet.Cells[rowCount++, 0].SetValue("Billing Authorization Exception Report – show transactions that do not have a corresponding billing authorization service available");
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