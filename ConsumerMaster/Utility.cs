using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ConsumerMaster
{
    public class Utility
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public double DurationToHours(double duration)
        {
            var hours = TimeSpan.Zero;

            try
            {
                hours = TimeSpan.FromSeconds(duration);
            }
            catch (OverflowException ex)
            {
                Logger.Error(ex.Message + "\n" + ex.GetType());
            }
            catch (ArgumentException ex)
            {
                Logger.Error(ex.Message + "\n" + ex.GetType());
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            return hours.TotalHours;
        }

        public DataTable GetDataTable(string queryString)
        {
            using (SqlConnection sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString))
            {
                sqlConnect.Open();
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnect))
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }


    }
}