using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BussinessAccessLayer
{
    public class DashBoardDAL
    {
        #region Property Declaration
        public string UserRoleId { get; set; }
        public string UserName { get; set; }

        public string Date { get; set; }

        public string flag { get; set; }

        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string CType { get; set; }

        public int PageIndex { get; set; }
        #endregion
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public static int PageRequestTimeoutInMLS = Convert.ToInt32(ConfigurationManager.AppSettings["PageRequestTimeoutInMLS"]);
        public DataSet BindLineChart()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@UserName", UserName),
                            new SqlParameter("@UserRoleId", UserRoleId),
                            new SqlParameter("@FromDate", Fromdate),
                            new SqlParameter("@ToDate", Todate),
                            //new SqlParameter("@Date", Date)
                        };
                        cmd.Connection = new SqlConnection(ConnectionString);
                        cmd.CommandText = "ChannelWiseCount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : BindLineChart() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
            return ds;
        }

        public DataSet BindLineChartbar()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@UserName", UserName),
                            new SqlParameter("@UserRoleId", UserRoleId),
                            new SqlParameter("@FromDate", Fromdate),
                            new SqlParameter("@ToDate", Todate),
                             //new SqlParameter("@Date", Date)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Dash_BCWiseTxn";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        //DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(ds);
                        cmd.Dispose();

                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : BindLineChartbar() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
            return ds;
        }

        public DataSet BindLineChartAmount()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@UserName", UserName),
                            new SqlParameter("@UserRoleId", UserRoleId),
                            new SqlParameter("@FromDate", Fromdate),
                            new SqlParameter("@ToDate", Todate),
                            //new SqlParameter("@Date", Date)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Dash_BCWiseTxn";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        //DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(ds);
                        cmd.Dispose();

                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : BindLineChartAmount() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
            return ds;
        }
        public DataSet BindPieChart()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(ConnectionString);
                cmd.CommandText = "GetEJStatusDashboard";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : BindPieChart() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
            return ds;
        }

        public DataSet BindLineChartChannel()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@UserName", UserName),
                            new SqlParameter("@UserRoleId", UserRoleId),
                            new SqlParameter("@FromDate", Fromdate),
                            new SqlParameter("@ToDate", Todate),
                            //new SqlParameter("@Date", Date)
                        };

                        cmd.Connection = new SqlConnection(ConnectionString);
                        cmd.CommandText = "ChannelWiseAmount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : BindLineChartChannel() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
            return ds;
        }


        public DataSet BindLineChartPop()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@UserName", UserName),
                            new SqlParameter("@UserRoleId", UserRoleId),
                            new SqlParameter("@FromDate", Fromdate),
                            new SqlParameter("@ToDate", Todate),
                            //new SqlParameter("@Date", Date)
                        };
                        cmd.Connection = new SqlConnection(ConnectionString);
                        cmd.CommandText = "ChannelWisePopDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : BindLineChartPop() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
            return ds;
        }


        #region GetAggregatorStatus
        public DataSet GetAggregatorStatus()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@Username", UserName),
                            new SqlParameter("@FromDate", Fromdate),
                            new SqlParameter("@ToDate", Todate),
                            //new SqlParameter("@Date", Date)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Dash_Top5Agg";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : GetTxnPlazaStatus() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion
        public DataSet GetMaximusHomePageData()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@UserName", UserName),
                            new SqlParameter("@UserRoleId", UserRoleId),
                            new SqlParameter("@Flag", 1),
                            new SqlParameter("@FromDate", Fromdate),
                            new SqlParameter("@ToDate", Todate),
                              //new SqlParameter("@Date", Date)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SBMHomePage";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : GetMaximusHomePageData() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet GetMaximusHomePageCount()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                         new SqlParameter("@UserName", UserName),
                         new SqlParameter("@FromDate", Fromdate),
                         new SqlParameter("@ToDate", Todate)
                         };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SBMHomePageCount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : GetMaximusHomePageCount() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #region Channel Wise Summary
        public DataSet GetChannelSummary()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@FromDate", Fromdate),
                                            new SqlParameter("@ToDate", Todate),
                                            new SqlParameter("@ChannelType", CType),
                                            new SqlParameter("@UserName", UserName),
                                            //new SqlParameter("@PageIndex", PageIndex),
                                            new SqlParameter("@Flag", flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "ChannelWiseSummary";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        cmd.CommandTimeout = PageRequestTimeoutInMLS;
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : GetChannelSummary() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        public DataSet GetMaximusHomePageData1()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {

                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))

                    {

                        SqlParameter[] _Params =

                        {

                            new SqlParameter("@UserName", UserName),

                            new SqlParameter("@UserRoleId", UserRoleId),

                             new SqlParameter("@Flag", 2),
                             new SqlParameter("@FromDate", Fromdate),
                             new SqlParameter("@ToDate", Todate),
                             // new SqlParameter("@Date", Date)
                              
                        };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SBMHomePage";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                        return dataSet;

                    }

                }

            }
            catch (Exception Ex)
            {

                ErrorLog.DashboardTrace("Class : DashBoardDAL.cs \nFunction : GetMaximusHomePageData() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }

        }

        #region DashBoard graphs data

        [Serializable]
        public class OnBoardedData
        {
            public string Name { get; set; }
            public long Count { get; set; }
        }

        [Serializable]
        public class TransactionData
        {
            public int Day { get; set; }
            public int CurrentMonthCount { get; set; }
            public int PreviousMonthCount { get; set; }
        }

        [Serializable]
        public class TransactionSummary
        {
            public int BC { get; set; }
            public int Switch { get; set; }
            public int Rule { get; set; }

        }

        public List<OnBoardedData> GetOnBoardingData()
        {
            var dataList = new List<OnBoardedData>();
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT * FROM get_DashboardOnBoardCardData()", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var data = new OnBoardedData
                            {
                                Name = reader.GetString(0),
                                Count = reader.GetInt64(1)
                            };
                            dataList.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from the database: {ex.Message}");
            }
            return dataList;
        }

        public List<TransactionData> GetMonthlyTxnDataCount()
        {
            var transactions = new List<TransactionData>();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM get_monthly_transaction_counts()", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new TransactionData
                                {
                                    Day = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    CurrentMonthCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                    PreviousMonthCount = reader.IsDBNull(2) ? 0 : reader.GetInt32(2)
                                });

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return transactions;
        }

        public List<TransactionSummary> GetMonthlySummaryCount(string dateFilter = null)
        {
            List<TransactionSummary> summaries = new List<TransactionSummary>();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open(); // Open the connection

                    using (var cmd = new NpgsqlCommand("SELECT * FROM Get_Switch_BC_RuleData(@dateFilter)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@dateFilter", (object)dateFilter ?? DBNull.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                summaries.Add(new TransactionSummary
                                {
                                    BC = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    Switch = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                    Rule = reader.IsDBNull(2) ? 0 : reader.GetInt32(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return summaries; // Return the list of transaction summaries
        }

        public DataSet Get_AllData(string dateFilter = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM get_TransactionSummaryCount()", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "Txnsummarychart");
                        }
                    }

                    using (var cmd = new NpgsqlCommand("SELECT * FROM Get_TopAggregatorData(@dateFilter)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@dateFilter", (object)dateFilter ?? DBNull.Value);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "Aggregators");
                        }
                    }

                    using (var cmd = new NpgsqlCommand("SELECT * FROM get_SwitchChartDtata(@dateFilter)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@dateFilter", (object)dateFilter ?? DBNull.Value);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "SwitchData");
                        }
                    }

                    //using (var cmd = new NpgsqlCommand("SELECT * FROM Get_Top5_Rule_Data(@dateFilter)", conn))
                    //{
                    //    cmd.CommandType = CommandType.Text;
                    //    cmd.Parameters.AddWithValue("@dateFilter", (object)dateFilter ?? DBNull.Value);
                    //    using (var adapter = new NpgsqlDataAdapter(cmd))
                    //    {
                    //        adapter.Fill(ds, "RuleData");
                    //    }
                    //}

                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.get_bctransactionsummarycount()", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "MonthlyBCData");
                        }
                    }

                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.get_channelwisedatacount()", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "ChannelwiseData");
                        }
                    }

                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.getBankRevenueData()", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "BankRevenueData");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return ds;
        }

        public DataSet FilterData(string Type, string dateFilter = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    if (Type == "ddlAggreFilter")
                    {
                        using (var cmd = new NpgsqlCommand("SELECT * FROM Get_TopAggregatorData(@dateFilter)", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@dateFilter", (object)dateFilter ?? DBNull.Value);

                            using (var adapter = new NpgsqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds, "Aggregators");
                            }
                        }
                    }

                    if (Type == "ddlSwitchFilter" || Type == "ddlTopSwitchsFilter")
                    {
                        using (var cmd = new NpgsqlCommand("SELECT * FROM get_SwitchChartDtata(@dateFilter)", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@dateFilter", (object)dateFilter ?? DBNull.Value);

                            using (var adapter = new NpgsqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds, "SwitchData");
                            }
                        }
                    }

                    if (Type == "ddlRuleFilter")
                    {
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

                    if (Type == "ddlChannelFilter")
                    {
                        using (var cmd = new NpgsqlCommand("SELECT * FROM get_channelwisedatacount(@filtertype)", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@filtertype", (object)dateFilter ?? DBNull.Value);
                            using (var adapter = new NpgsqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds, "ChannelData");
                            }
                        }
                    }


                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.get_monthlywiseBcData(@aggregatorCodeFilter, @limitCount)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@aggregatorCodeFilter", (object)dateFilter ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@limitCount", 10);

                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "MonthlyBCData");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
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

                    if (Type == "TransactionSummary" || Type == "GlobalFilter")
                    {
                        using (var cmd = new NpgsqlCommand("SELECT * FROM get_TransactionSummaryCount(@from_date, @to_date, @filter_type)", conn))
                        {
                            cmd.CommandType = CommandType.Text;

                            cmd.Parameters.AddWithValue("@from_date", ParseDate(fromDate));
                            cmd.Parameters.AddWithValue("@to_date", ParseDate(toDate));
                            cmd.Parameters.AddWithValue("@filter_type", (object)filter ?? "Month");

                            using (var adapter = new NpgsqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds, "Txnsummarychart");
                            }
                        }
                    }
                    if (Type == "BCTransactionSummary" || Type == "GlobalFilter")
                    {
                        using (var cmd = new NpgsqlCommand("SELECT * FROM public.get_BCTransactionSummaryCount(@from_date, @to_date, @filter_type, @limit_count)", conn))
                        {
                            cmd.CommandType = CommandType.Text;

                            cmd.Parameters.AddWithValue("@from_date", ParseDate(fromDate));
                            cmd.Parameters.AddWithValue("@to_date", ParseDate(toDate));
                            cmd.Parameters.AddWithValue("@filter_type", (object)filter ?? "Month");
                            cmd.Parameters.AddWithValue("@limit_count", 10);

                            using (var adapter = new NpgsqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds, "bcsummarychart");
                            }
                        }
                    }
                    if (Type == "ddlSwitchFilter" || Type == "GlobalFilter")
                    {
                        DateTime d1 = Convert.ToDateTime(fromDate);
                        DateTime d2 = Convert.ToDateTime(toDate);
                        using (var cmd = new NpgsqlCommand("SELECT * FROM public.get_switchchartdata(@filtertype, @fromdate, @todate)", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@filtertype", (object)filter ?? "last_week");
                            cmd.Parameters.AddWithValue("@fromdate", ParseDate(fromDate));
                            cmd.Parameters.AddWithValue("@todate", ParseDate(toDate));
                            using (var adapter = new NpgsqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds, "switchchart");
                            }
                        }
                    }
                    if (Type == "RevenueSummary" || Type == "GlobalFilter")
                    {
                        using (var cmd = new NpgsqlCommand("SELECT * FROM public.getBankRevenueData(@filtertype, @fromdate, @todate)", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@filtertype", (object)filter ?? "Month");
                            cmd.Parameters.AddWithValue("@fromdate", ParseDate(fromDate));
                            cmd.Parameters.AddWithValue("@todate", ParseDate(toDate));
                            using (var adapter = new NpgsqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds, "Revenuechart");
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
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
        private object ParseDateWithoutTime(string dateString)
        {
            try
            {
                if (string.IsNullOrEmpty(dateString))
                    return DBNull.Value;

                if (DateTime.TryParseExact(dateString, "dd-MM-yyyy",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None,
                                            out DateTime parsedDate))
                {
                    return parsedDate.ToString("yyyy-MM-dd");
                }

                throw new FormatException($"Invalid date format: {dateString}");
            }
            catch (Exception)
            {

                throw;
            }

        }



        #endregion

    }

}