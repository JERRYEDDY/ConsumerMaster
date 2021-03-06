﻿using System;
using System.Data;
using Telerik.Web.UI;
using System.Linq;
using System.IO;

namespace ConsumerMaster
{
    public class AWC29HoursReportFile
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public MemoryStream CreateDocument(UploadedFile uploadedFile)
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
                         where TD.Select(v => v.Field<string>("ID")).Distinct().Count() > 1
                         where TD.Sum(v => v.Field<int>("Duration") / 60.00) > 29.00
                         orderby TD.Sum(v => v.Field<int>("Duration") / 60.00)
                         select new
                         {
                             ID = TD.Key.StaffID,
                             Name = TD.Key.StaffName,
                             Count = TD.Select(v => v.Field<string>("ID")).Distinct().Count(),
                             Hours = TD.Sum(v => v.Field<int>("Duration") / 60.00)
                         };


            using (var ms = new MemoryStream())
            using (var streamWriter = new StreamWriter(ms))
            {
                streamWriter.WriteLine("29 hours per week limit – if staff supports more than one client");
                streamWriter.WriteLine("Date/time:{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                streamWriter.WriteLine("Filename:{0}", uploadedFile.FileName);
                streamWriter.WriteLine(" ");
                streamWriter.WriteLine("{0,-10} {1,-30} {2,-5}  {3}", "StaffID","StaffName","Count","Hours");
                // print result
                foreach (var staff in query)
                {
                    string name = staff.Name.Replace("\t", "");
                    streamWriter.WriteLine("{0,-10} {1,-30} {2,-5}  {3:0.00}", staff.ID, name.Trim(), staff.Count, staff.Hours);
                }

                streamWriter.Flush();
                return ms;
            }
        }
    }
}