using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using System;
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

    }

}