﻿using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using System.Windows.Media;

namespace ConsumerMaster
{
    public class VendorServiceExportExcelFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly int IndexRowItemStart = 0;
        private static readonly int IndexColumnName = 0;

        public Workbook CreateWorkbook()
        {
            Workbook workbook = new Workbook();
            try
            {
                WorksheetCollection worksheets = workbook.Worksheets;
                worksheets.Add();
                worksheets.Add();
                worksheets.Add();

                Worksheet sheet1Worksheet = worksheets["Sheet1"]; //Data Entry Sheet
                sheet1Worksheet.Name = "Vendor";
                Worksheet sheet2Worksheet = worksheets["Sheet2"]; //Trading Partner Programs
                Worksheet sheet3Worksheet = worksheets["Sheet3"]; //Composite Procedure Codes

                Utility util = new Utility();

                List<string> tppList = util.GetList("SELECT symbol FROM VendorTradingPartnerPrograms");
                CreateTradingPartnerProgramsWorksheet(sheet2Worksheet, tppList);

                List<string> cpcList = util.GetList("SELECT name FROM CompositeProcedureCodes WHERE trading_partner_id = 19");  //Vendor;In Home = 19
                CreateCompositeProcedureCodesWorksheet(sheet3Worksheet, cpcList);

                ServiceExportFormat sef = new ServiceExportFormat(false);
                string tradingPartnerId = "19";//Vendor;In Home = 19
                string selectQuery = 
                $@"
                    SELECT 
                        c.consumer_first AS consumer_first, c.consumer_last AS consumer_last, c.consumer_internal_number AS consumer_internal_number
                        ,tp.symbol AS trading_partner_string, ' ' AS trading_partner_program_string, ' ' AS start_date_string, ' ' AS end_date_string
                        ,c.diagnosis AS diagnosis_code_1_code, ' ' AS composite_procedure_code_string, ' ' AS units, ' ' AS manual_billable_rate
                        ,' ' AS prior_authorization_number,' ' AS referral_number, ' ' AS referring_provider_id, ' ' AS referring_provider_first_name
                        ,' ' AS referring_provider_last_name,' ' AS rendering_provider_id, ' ' AS rendering_provider_first_name, ' ' AS rendering_provider_last_name 
                    FROM 
                        Consumers AS c 
                    INNER JOIN 
                        TradingPartners AS tp ON {tradingPartnerId} = tp.id 
                    WHERE 
                        c.trading_partner_id1 = {tradingPartnerId} OR c.trading_partner_id2 = {tradingPartnerId} OR c.trading_partner_id3 = {tradingPartnerId} 
                    ORDER BY consumer_last
                ";

                DataTable seDataTable = util.GetDataTable(selectQuery);
                int totalConsumers = seDataTable.Rows.Count;
                PrepareSheet1Worksheet(sheet1Worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (DataRow dr in seDataTable.Rows)
                {
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("consumer_first")].SetValue(dr["consumer_first"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("consumer_last")].SetValue(dr["consumer_last"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("consumer_internal_number")].SetValue(dr["consumer_internal_number"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("diagnosis_code_1_code")].SetValue(dr["diagnosis_code_1_code"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("trading_partner_string")].SetValue(dr["trading_partner_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("trading_partner_program_string")].SetValue(dr["trading_partner_program_string"].ToString());
                    CellIndex dataValidationRuleCellIndex1 = new CellIndex(currentRow, sef.GetIndex("trading_partner_program_string"));
                    ListDataValidationRuleContext context1 = new ListDataValidationRuleContext(sheet1Worksheet, dataValidationRuleCellIndex1)
                    {
                        InputMessageTitle = "Restricted input",
                        InputMessageContent = "The input is restricted to the trading partner programs.",
                        ErrorStyle = ErrorStyle.Stop,
                        ErrorAlertTitle = "Wrong value",
                        ErrorAlertContent = "The entered value is not valid. Allowed values are the trading partner programs!",
                        InCellDropdown = true
                    };

                    string tppRange = "=Sheet2!$A$2:$A$" + tppList.Count + 1;  //= Sheet2!$A$2:$A$73
                    context1.Argument1 = tppRange;
                    ListDataValidationRule rule1 = new ListDataValidationRule(context1);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex1].SetDataValidationRule(rule1);

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("start_date_string")].SetValue(dr["start_date_string"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("end_date_string")].SetValue(dr["end_date_string"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("composite_procedure_code_string")].SetValue(dr["composite_procedure_code_string"].ToString());
                    CellIndex dataValidationRuleCellIndex2 = new CellIndex(currentRow, sef.GetIndex("composite_procedure_code_string"));
                    ListDataValidationRuleContext context2 = new ListDataValidationRuleContext(sheet1Worksheet, dataValidationRuleCellIndex2)
                    {
                        InputMessageTitle = "Restricted input",
                        InputMessageContent = "The input is restricted to the composite procedure codes.",
                        ErrorStyle = ErrorStyle.Stop,
                        ErrorAlertTitle = "Wrong value",
                        ErrorAlertContent = "The entered value is not valid. Allowed values are the composite procedure codes!",
                        InCellDropdown = true
                    };

                    string cpcRange = "=Sheet3!$A$2:$A$" + cpcList.Count + 1;  //= Sheet3!$A$2:$A$73
                    context2.Argument1 = cpcRange;    
                    ListDataValidationRule rule2 = new ListDataValidationRule(context2);
                    sheet1Worksheet.Cells[dataValidationRuleCellIndex2].SetDataValidationRule(rule2);

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("units")].SetValue(dr["units"].ToString());

                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("manual_billable_rate")].SetValue(dr["manual_billable_rate"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("prior_authorization_number")].SetValue(dr["prior_authorization_number"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referral_number")].SetValue(dr["referral_number"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_id")].SetValue(dr["referring_provider_id"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_first_name")].SetValue(dr["referring_provider_first_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("referring_provider_last_name")].SetValue(dr["referring_provider_last_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_id")].SetValue(dr["rendering_provider_id"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_first_name")].SetValue(dr["rendering_provider_first_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_last_name")].SetValue(dr["rendering_provider_last_name"].ToString());
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("billing_note")].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_secondary_id")].SetValue(" ");
                    sheet1Worksheet.Cells[currentRow, sef.GetIndex("rendering_provider_taxonomy_code")].SetValue(" ");

                    currentRow++;
                }

                for (int i = 0; i < seDataTable.Columns.Count; i++)
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

        private void CreateTradingPartnerProgramsWorksheet(Worksheet worksheet, List<string> tppList)
        {
            try
            {
                PrepareSheet2Worksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (String tppCode in tppList)
                {
                    worksheet.Cells[currentRow, IndexColumnName].SetValue(tppCode);
                    currentRow++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void CreateCompositeProcedureCodesWorksheet(Worksheet worksheet, List<string> cpcList)
        {
            try
            {
                PrepareSheet3Worksheet(worksheet);

                int currentRow = IndexRowItemStart + 1;
                foreach (String cpCode in cpcList)
                {
                    worksheet.Cells[currentRow, IndexColumnName].SetValue(cpCode);
                    currentRow++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void PrepareSheet1Worksheet(Worksheet worksheet)
        {
            try
            {
                ServiceExportFormat sef = new ServiceExportFormat(false);
                string[] columnsList = sef.ColumnStrings;

                PatternFill solidPatternFill = new PatternFill(PatternType.Solid, Color.FromArgb(255,255,0,0), Colors.Transparent);

                foreach (string column in columnsList)
                {
                    int columnKey = Array.IndexOf(columnsList, column);
                    string columnName = column;

                    worksheet.Cells[IndexRowItemStart, columnKey].SetValue(columnName);
                    worksheet.Cells[IndexRowItemStart, columnKey].SetHorizontalAlignment(RadHorizontalAlignment.Left);
                    if (columnName.Equals("consumer_first") || columnName.Equals("consumer_last"))
                    {
                        worksheet.Cells[IndexRowItemStart, columnKey].SetFill(solidPatternFill);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void PrepareSheet2Worksheet(Worksheet worksheet)
        {
            try
            {
                worksheet.Cells[IndexRowItemStart, IndexColumnName].SetValue("trading_partner_program");
                worksheet.Cells[IndexRowItemStart, IndexColumnName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void PrepareSheet3Worksheet(Worksheet worksheet)
        {
            try
            {
                worksheet.Cells[IndexRowItemStart, IndexColumnName].SetValue("composite_procedure_code");
                worksheet.Cells[IndexRowItemStart, IndexColumnName].SetHorizontalAlignment(RadHorizontalAlignment.Left);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}

