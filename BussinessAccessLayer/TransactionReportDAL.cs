using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace BussinessAccessLayer
{
    public class TransactionReportDAL
    {
        #region Property Declaration
        public string AggregatorCode { get; set; }
        public string AgentCode { get; set; }
        public string UserName { get; set; }

        public int PageIndex { get; set; }
        public int Flag { get; set; }

        public string flag { get; set; }

        public string DateRange { get; set; }

        public string Fromdate { get; set; }
        public string Todate { get; set; }

        public string RRN { get; set; }

        public string PRRN { get; set; }
       
        public string Status { get; set; }

        public int TransStatus { get; set; }

        public string ChannelType { get; set; }

        public int CType { get; set; }

        public string TransType { get; set; }

        public int TransactionType { get; set; }
        public string CreatedBy { get; set; }
        public string IsRemoved { get; set; }
        public string IsActive { get; set; }
        public string IsdocUploaded { get; set; }
        public string VerificationStatus { get; set; }
        public string BCID { get; set; }
        public string Clientcode { get; set; }

        #endregion
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        
        public static int PageRequestTimeoutInMLS = Convert.ToInt32(ConfigurationManager.AppSettings["PageRequestTimeoutInMLS"]);//ConnectionString
        public DataTable GetBC(string UserName, int VerificationStatus, int IsActive, int IsRemoved, string ClientID, int IsdocUploaded)
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
                            new SqlParameter("@IsVerified", VerificationStatus),
                            new SqlParameter("@IsActive", IsActive),
                            new SqlParameter("@IsRemoved", IsRemoved),
                            new SqlParameter("@ClientID",ClientID ),
                            new SqlParameter("@IsDocUploaded", IsdocUploaded),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindBC_Reports";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataTable dataTable = new DataTable();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataTable);
                        cmd.Dispose();
                        return dataTable;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("TransactionReportDAL: BindBC: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        public DataTable GetCountryStateCity(string UserName, string Mode, int CountryID, int StateID)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@Mode", Mode),
                            new SqlParameter("@CountryID", CountryID),
                            new SqlParameter("@StateID", StateID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_MasterDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataTable dataTable = new DataTable();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataTable);
                        cmd.Dispose();
                        return dataTable;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("TransactionReportDAL: GetCountryStateCity: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #region AEPS Traansaction
        public DataSet GetAEPSTransactionReport()
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
                                            new SqlParameter("@AggregatorCode", BCID),
                                            new SqlParameter("@AgentCode", AgentCode),
                                            new SqlParameter("@TranType", TransType),
                                            new SqlParameter("@TransStatus",TransStatus),
                                            new SqlParameter("@RRN",RRN),
                                            new SqlParameter("@UserName", UserName),
                                            new SqlParameter("@PageIndex", PageIndex),
                                            new SqlParameter("@Flag", flag)
               };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "TxnReport_AEPS";
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
                ErrorLog.TransactionReportTrace("TransactionReportDAL: GetAEPSTransactionReport: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region BindBC
        public DataSet BindBCddl()
        {
            DataSet dataSet = new DataSet();
            try
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _paramsAggregatorDetails = {
                                                 new SqlParameter("@UserName", UserName),
                                                 new SqlParameter("@IsVerified", VerificationStatus),
                                                 new SqlParameter("@IsActive", IsActive),
                                                 new SqlParameter("@IsRemoved", IsRemoved),
                                                 new SqlParameter("@ClientID", Clientcode),
                                                 new SqlParameter("@IsDocUploaded", IsdocUploaded),
                                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindBC_Reports";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                        return dataSet;

                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("TransactionReportDAL: BindBCddl: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }

        }
        #endregion


        #region GetTransactions_All
        public DataSet GetTransactions_All()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                                SqlParameter[] _Params = {
                                            new SqlParameter("@FromDate", Fromdate),
                                            new SqlParameter("@ToDate", Todate),
                                            new SqlParameter("@ChannelType", CType),
                                            //new SqlParameter("@ClientID", ClientID),
                                            new SqlParameter("@AggregatorCode", BCID),
                                            new SqlParameter("@AgentCode", AgentCode),
                                            new SqlParameter("@TranType", TransType),
                                            new SqlParameter("@TransStatus",TransStatus),
                                            new SqlParameter("@RRN",RRN),
                                            new SqlParameter("@UserName", UserName),
                                            new SqlParameter("@PageIndex", PageIndex),
                                            new SqlParameter("@Flag", flag)
                                         };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "TxnReport_All";
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
                ErrorLog.TransactionReportTrace("TransactionReportDAL: GetTransactions_All: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        
        }
        #endregion

        #region MATM  Traansaction
        public DataSet GetMATMTransactionReport()
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
                                            new SqlParameter("@AggregatorCode", BCID),
                                            new SqlParameter("@AgentCode", AgentCode),
                                            new SqlParameter("@TranType", TransType),
                                            new SqlParameter("@TransStatus",TransStatus),
                                            new SqlParameter("@RRN",RRN),
                                            new SqlParameter("@UserName", UserName),
                                            new SqlParameter("@PageIndex", PageIndex),
                                            new SqlParameter("@Flag", Flag) };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "TxnReport_MATM";
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
                ErrorLog.TransactionReportTrace("TransactionReportDAL: GetMATMTransactionReport: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }

        }
        #endregion



        #region Get Success Transaction
        public DataSet GetTransactions_Success()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {
                                            new SqlParameter("@FromDate", Fromdate),
                                            new SqlParameter("@ToDate", Todate),
                                            new SqlParameter("@ChannelType", CType),
                                            new SqlParameter("@AggregatorCode", BCID),
                                            new SqlParameter("@AgentCode", AgentCode),
                                            new SqlParameter("@TranType", TransType),
                                            new SqlParameter("@TransStatus",TransStatus),
                                            new SqlParameter("@RRN",RRN),
                                            new SqlParameter("@UserName", UserName),
                                            new SqlParameter("@PageIndex", PageIndex),
                                            new SqlParameter("@Flag", flag)
                                         };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "TxnReport_Success";
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
                ErrorLog.TransactionReportTrace("TransactionReportDAL: GetTransactions_Success: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }

        }
        #endregion

        #region Get Decline Transaction
        public DataSet GetTransactions_Decline()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {
                                            new SqlParameter("@FromDate", Fromdate),
                                            new SqlParameter("@ToDate", Todate),
                                            new SqlParameter("@ChannelType", CType),
                                            new SqlParameter("@AggregatorCode", BCID),
                                            new SqlParameter("@AgentCode", AgentCode),
                                            new SqlParameter("@TranType", TransType),
                                            new SqlParameter("@TransStatus",TransStatus),
                                            new SqlParameter("@RRN",RRN),
                                            new SqlParameter("@UserName", UserName),
                                            new SqlParameter("@PageIndex", PageIndex),
                                            new SqlParameter("@Flag", flag)
                                         };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "TxnReport_Decline";
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
                ErrorLog.TransactionReportTrace("TransactionReportDAL: GetTransactions_Decline: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }

        }
        #endregion
    }
}
