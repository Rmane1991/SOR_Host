using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessAccessLayer
{
    public class AdvanceDashbordDAL
    {
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public static int PageRequestTimeoutInMLS = Convert.ToInt32(ConfigurationManager.AppSettings["PageRequestTimeoutInMLS"]);

        public DataSet GetRuleWiseData(string dateFilter = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM Get_Top5_Rule_Data(@dateFilter)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@dateFilter", (object)dateFilter ?? DBNull.Value);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "RuleData");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("Class : AdvanceDashboardDAL.cs \nFunction : GetRuleWiseData() \nException Occured\n" + ex.Message);
                ErrorLog.DBError(ex);
            }

            return ds;
        }

        public DataSet GetChannelWiseData(string dateFilter = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM get_channelwisechartdata(@dateFilter)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@dateFilter", (object)dateFilter ?? DBNull.Value);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "ChannelData");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("Class : AdvanceDashboardDAL.cs \nFunction : GetAgentOnboardingData() \nException Occured\n" + ex.Message);
                ErrorLog.DBError(ex);
            }

            return ds;
        }

        public DataSet GetAgentOnboardingData(string dateFilter = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM get_AgentOnboardedDataCount(@filtertype)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@filtertype", (object)dateFilter ?? DBNull.Value);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "AgentOnbordingData");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("Class : AdvanceDashboardDAL.cs \nFunction : GetAgentOnboardingData() \nException Occured\n" + ex.Message);
                ErrorLog.DBError(ex);
            }

            return ds;
        }

        public DataSet GetAgentTransactionData(string dateFilter = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM Fnget_AgentTransactionSummary()", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "AgentTransactionData");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("Class : AdvanceDashboardDAL.cs \nFunction : GetAgentTransactionData() \nException Occured\n" + ex.Message);
                ErrorLog.DBError(ex);
            }

            return ds;
        }

        public DataSet GetCustomerTransactionData(string dateFilter = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM FnGet_UniqCustomerData()", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "UnqCustomerTrend");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("Class : AdvanceDashboardDAL.cs \nFunction : GetCustomerTransactionData() \nException Occured\n" + ex.Message);
                ErrorLog.DBError(ex);
            }

            return ds;
        }

        public DataSet FilterChartDate(string fromDate, string toDate, string Type, string filter)
        {
            DataSet ds = new DataSet();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    try
                    {
                        if (Type == "ChannelWiseTxn" || Type == "GlobalSearch")
                        {
                            using (var cmd = new NpgsqlCommand("SELECT * FROM get_channelwisechartdata(@fromdate, @todate, @filtertype)", conn))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@fromdate", ParseDate(fromDate));
                                cmd.Parameters.AddWithValue("@todate", ParseDate(toDate));
                                cmd.Parameters.AddWithValue("@filtertype", (object)filter ?? DBNull.Value);

                                using (var adapter = new NpgsqlDataAdapter(cmd))
                                {
                                    adapter.Fill(ds, "ChannelWiseTxn");
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.DashboardTrace("Class : AdvanceDashboardDAL.cs \nFunction : FilterChartDate()\nType : " + Type + " \nException Occured\n" + ex.Message);
                        ErrorLog.DBError(ex);
                    }
                    try
                    {
                        if (Type == "AgentOnbordingData" || Type == "GlobalSearch")
                        {
                            using (var cmd = new NpgsqlCommand("SELECT * FROM get_agentonboardeddatacount(@fromdate, @todate, @filtertype)", conn))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@fromdate", ParseDate(fromDate));
                                cmd.Parameters.AddWithValue("@todate", ParseDate(toDate));
                                cmd.Parameters.AddWithValue("@filtertype", (object)filter ?? DBNull.Value);
                                using (var adapter = new NpgsqlDataAdapter(cmd))
                                {
                                    adapter.Fill(ds, "AgentOnbordingData");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.DashboardTrace("Class : AdvanceDashboardDAL.cs \nFunction : FilterChartDate()\nType : " + Type + " \nException Occured\n" + ex.Message);
                        ErrorLog.DBError(ex);
                    }

                    try
                    {
                        if (Type == "AgentTxnData" || Type == "GlobalSearch")
                        {
                            using (var cmd = new NpgsqlCommand("SELECT * FROM AgentTransactionSummaryDetails(@filtertype, @startdate, @enddate)", conn))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@filtertype", (object)filter ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@startdate", ParseDate(fromDate));
                                cmd.Parameters.AddWithValue("@enddate", ParseDate(toDate));
                                using (var adapter = new NpgsqlDataAdapter(cmd))
                                {
                                    adapter.Fill(ds, "AgentTxnData");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.DashboardTrace("Class : AdvanceDashboardDAL.cs \nFunction : FilterChartDate()\nType : " + Type + " \nException Occured\n" + ex.Message);
                        ErrorLog.DBError(ex);
                    }

                    try
                    {
                        if (Type == "UniqueCustomer" || Type == "GlobalSearch")
                        {
                            using (var cmd = new NpgsqlCommand("SELECT * FROM fnget_uniqcustomerdata(@filtertype, @fromdate, @todate)", conn))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@filtertype", (object)filter ?? DBNull.Value); 
                                cmd.Parameters.AddWithValue("@fromdate", ParseDate(fromDate) ?? DBNull.Value); 
                                cmd.Parameters.AddWithValue("@todate", ParseDate(toDate) ?? DBNull.Value);
                                using (var adapter = new NpgsqlDataAdapter(cmd))
                                {
                                    adapter.Fill(ds, "UniqueCustomer");
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.DashboardTrace("Class : AdvanceDashboardDAL.cs \nFunction : FilterChartDate()\nType : " + Type + " \nException Occured\n" + ex.Message);
                        ErrorLog.DBError(ex);
                    }


                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {

                }
            }

            return ds;
        }



        private object ParseDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return DBNull.Value;
            if (DateTime.TryParseExact(dateString, "dd-MM-yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None,
                                        out DateTime parsedDate))
            {

                return DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
            }

            throw new FormatException($"Invalid date format: {dateString}");
        }
    }
}
