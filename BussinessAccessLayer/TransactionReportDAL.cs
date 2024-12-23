using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;
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
        public string PinCode { get; set; }
        public string Name { get; set; }

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
        public string NamePIN { get; set; }
        public string GroupId { get; set; }
        public string RuleId { get; set; }
        public string Clientcode { get; set; }

        #endregion
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        //static string TxnReport = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["TxnReport"].ToString()) : ConfigurationManager.ConnectionStrings["TxnReport"].ToString();
        
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
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        NpgsqlParameter[] _Params =
                        {
                            new NpgsqlParameter("p_FromDate", Fromdate),
                            new NpgsqlParameter("p_ToDate", Todate),
                            new NpgsqlParameter("p_ChannelType", CType),
                            new NpgsqlParameter("p_AggregatorCode", (object)AggregatorCode ?? DBNull.Value),
                            new NpgsqlParameter("p_AgentCode", (object)AgentCode ?? DBNull.Value),
                            new NpgsqlParameter("p_TranType", (object)TransType ?? DBNull.Value),
                            new NpgsqlParameter("p_TransStatus", (object)TransStatus ?? DBNull.Value),
                            new NpgsqlParameter("p_RRN", (object)RRN ?? DBNull.Value),
                            new NpgsqlParameter("p_UserName", UserName),
                            new NpgsqlParameter("p_Flag", flag),
                            new NpgsqlParameter("p_PageIndex", PageIndex)
                        };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL TxnReport_AEPS(@p_FromDate, @p_ToDate, @p_ChannelType, @p_AggregatorCode, @p_AgentCode, @p_TranType, @p_TransStatus, @p_RRN, @p_UserName, @p_Flag, @p_PageIndex)";
                        cmd.CommandType = CommandType.Text; // Using text command for CALL
                        cmd.Parameters.AddRange(_Params);
                        cmd.CommandTimeout = PageRequestTimeoutInMLS;

                        // Execute the command to call the procedure
                        sqlConn.Open();
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_table", sqlConn))
                        {
                            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectCmd);
                            DataTable tempTable = new DataTable("TempTable");
                            dataAdapter.Fill(tempTable);
                            dataSet.Tables.Add(tempTable);
                        }

                        // Retrieve data from the counts_table
                        try
                        {
                            using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_count", sqlConn))
                            {
                                NpgsqlDataAdapter countAdapter = new NpgsqlDataAdapter(selectCmd);
                                DataTable countsTable = new DataTable("CountsTable");
                                countAdapter.Fill(countsTable);
                                dataSet.Tables.Add(countsTable);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception for counts_table retrieval
                            ErrorLog.TransactionReportTrace("Error retrieving counts_table: " + ex.Message);
                        }

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
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        NpgsqlParameter[] _Params = {
                    new NpgsqlParameter("p_fromdate", Fromdate),
                    new NpgsqlParameter("p_todate", Todate),
                    new NpgsqlParameter("p_channeltype", CType),
                    new NpgsqlParameter("p_aggregatorcode", (object)AggregatorCode ?? DBNull.Value),
                    new NpgsqlParameter("p_agentcode", (object)AgentCode ?? DBNull.Value),
                    new NpgsqlParameter("p_trantype", (object)TransType ?? DBNull.Value),
                    new NpgsqlParameter("p_transstatus", (object)TransStatus ?? DBNull.Value),
                    new NpgsqlParameter("p_rrn", (object)RRN ?? DBNull.Value),
                    new NpgsqlParameter("p_username", UserName),
                    new NpgsqlParameter("p_flag", flag),
                    new NpgsqlParameter("p_pageindex", PageIndex)
                };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL TxnReport_All(@p_fromdate, @p_todate, @p_channeltype, @p_aggregatorcode, @p_agentcode, @p_trantype, @p_transstatus, @p_rrn, @p_username, @p_flag, @p_pageindex)";
                        cmd.CommandType = CommandType.Text; // Using text command for CALL
                        cmd.Parameters.AddRange(_Params);
                        cmd.CommandTimeout = PageRequestTimeoutInMLS;

                        sqlConn.Open();
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        // Retrieve data from the temp_table
                        using (var selectCmd = new NpgsqlCommand("SELECT * FROM TempTxnReport", sqlConn))
                        {
                            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectCmd);
                            DataTable tempTable = new DataTable("TempTable");
                            dataAdapter.Fill(tempTable);
                            dataSet.Tables.Add(tempTable);
                        }

                        // Retrieve data from the counts table
                        using (var selectCmd = new NpgsqlCommand("SELECT * FROM TempTotalCounts", sqlConn))
                        {
                            NpgsqlDataAdapter countAdapter = new NpgsqlDataAdapter(selectCmd);
                            DataTable countsTable = new DataTable("TempTotalCounts");
                            countAdapter.Fill(countsTable);
                            dataSet.Tables.Add(countsTable);
                        }
                        using (var selectCmd = new NpgsqlCommand("SELECT * FROM TempChannel1Counts", sqlConn))
                        {
                            NpgsqlDataAdapter countAdapter = new NpgsqlDataAdapter(selectCmd);
                            DataTable countsTable = new DataTable("TempChannel1Counts");
                            countAdapter.Fill(countsTable);
                            dataSet.Tables.Add(countsTable);
                        }
                        using (var selectCmd = new NpgsqlCommand("SELECT * FROM TempChannel2Counts", sqlConn))
                        {
                            NpgsqlDataAdapter countAdapter = new NpgsqlDataAdapter(selectCmd);
                            DataTable countsTable = new DataTable("TempChannel2Counts");
                            countAdapter.Fill(countsTable);
                            dataSet.Tables.Add(countsTable);
                        }

                        cmd.Dispose();
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.TransactionReportTrace("TransactionReportDAL: GetTransactions_All: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
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

        public DataTable GetRule()
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("public.fn_bindrule", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_groupid", (object)GroupId ?? DBNull.Value);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: GetRule() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
            return dataTable;
        }
        #region RGS Traansaction
        public DataSet GetRGSTransactionReport()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        NpgsqlParameter[] _Params =
                        {
                            new NpgsqlParameter("p_FromDate", Fromdate),
                            new NpgsqlParameter("p_ToDate", Todate),
                            new NpgsqlParameter("p_ChannelType", CType),
                            new NpgsqlParameter("p_groupid", (object)GroupId ?? DBNull.Value),
                            new NpgsqlParameter("p_ruleid", (object)RuleId ?? DBNull.Value),
                            new NpgsqlParameter("p_UserName", UserName),
                            new NpgsqlParameter("p_Flag", flag),
                            new NpgsqlParameter("p_PageIndex", PageIndex)
                        };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL txnreport_rgs(@p_FromDate, @p_ToDate, @p_ChannelType, @p_groupid, @p_ruleid, @p_UserName, @p_Flag, @p_PageIndex)";
                        cmd.CommandType = CommandType.Text; // Using text command for CALL
                        cmd.Parameters.AddRange(_Params);
                        cmd.CommandTimeout = PageRequestTimeoutInMLS;

                        // Execute the command to call the procedure
                        sqlConn.Open();
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_table", sqlConn))
                        {
                            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectCmd);
                            DataTable tempTable = new DataTable("TempTable");
                            dataAdapter.Fill(tempTable);
                            dataSet.Tables.Add(tempTable);
                        }

                        // Retrieve data from the counts_table
                        try
                        {
                            using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_count", sqlConn))
                            {
                                NpgsqlDataAdapter countAdapter = new NpgsqlDataAdapter(selectCmd);
                                DataTable countsTable = new DataTable("CountsTable");
                                countAdapter.Fill(countsTable);
                                dataSet.Tables.Add(countsTable);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception for counts_table retrieval
                            ErrorLog.TransactionReportTrace("Error retrieving counts_table: " + ex.Message);
                        }

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
        #region RGS Traansaction
        public DataSet GetBCTransactionReport()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        NpgsqlParameter[] _Params =
                        {
                            new NpgsqlParameter("p_FromDate", Fromdate),
                            new NpgsqlParameter("p_ToDate", Todate),
                            new NpgsqlParameter("p_bcid", (object)BCID ?? DBNull.Value),
                            new NpgsqlParameter("p_UserName", UserName),
                            new NpgsqlParameter("p_Flag", flag),
                            new NpgsqlParameter("p_PageIndex", PageIndex)
                        };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL txnreport_bc(@p_FromDate, @p_ToDate, @p_bcid, @p_UserName, @p_Flag, @p_PageIndex)";
                        cmd.CommandType = CommandType.Text; // Using text command for CALL
                        cmd.Parameters.AddRange(_Params);
                        cmd.CommandTimeout = PageRequestTimeoutInMLS;

                        // Execute the command to call the procedure
                        sqlConn.Open();
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_table", sqlConn))
                        {
                            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectCmd);
                            DataTable tempTable = new DataTable("TempTable");
                            dataAdapter.Fill(tempTable);
                            dataSet.Tables.Add(tempTable);
                        }

                        // Retrieve data from the counts_table
                        try
                        {
                            using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_count", sqlConn))
                            {
                                NpgsqlDataAdapter countAdapter = new NpgsqlDataAdapter(selectCmd);
                                DataTable countsTable = new DataTable("CountsTable");
                                countAdapter.Fill(countsTable);
                                dataSet.Tables.Add(countsTable);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception for counts_table retrieval
                            ErrorLog.TransactionReportTrace("Error retrieving counts_table: " + ex.Message);
                        }

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
        #region RestrictedDetails
        public DataSet GetRestrictedDetails()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        NpgsqlParameter[] _Params =
                        {
                            new NpgsqlParameter("p_FromDate", Fromdate),
                            new NpgsqlParameter("p_ToDate", Todate),
                            new NpgsqlParameter("p_namepin", (object)NamePIN?? DBNull.Value),
                            new NpgsqlParameter("p_UserName", UserName),
                            new NpgsqlParameter("p_Flag", flag),
                            new NpgsqlParameter("p_pincode", (object)PinCode?? DBNull.Value),
                            new NpgsqlParameter("p_name", (object)Name?? DBNull.Value),
                            new NpgsqlParameter("p_PageIndex", PageIndex)
                        };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL SP_NegativeAgent_Report(@p_FromDate, @p_ToDate, @p_namepin, @p_UserName, @p_Flag, @p_pincode, @p_name, @p_PageIndex)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);
                        cmd.CommandTimeout = PageRequestTimeoutInMLS;

                        sqlConn.Open();
                        cmd.ExecuteNonQuery();

                        using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_table", sqlConn))
                        {
                            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectCmd);
                            DataTable tempTable = new DataTable("TempTable");
                            dataAdapter.Fill(tempTable);
                            dataSet.Tables.Add(tempTable);
                        }
                        try
                        {
                            using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_count", sqlConn))
                            {
                                NpgsqlDataAdapter countAdapter = new NpgsqlDataAdapter(selectCmd);
                                DataTable countsTable = new DataTable("CountsTable");
                                countAdapter.Fill(countsTable);
                                dataSet.Tables.Add(countsTable);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.TransactionReportTrace("Error retrieving counts_table: " + ex.Message);
                        }
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
        public DataTable GetAction()
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SELECT * FROM get_action()";


                        DataTable dataTable = new DataTable();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataTable);
                        }

                        cmd.Dispose();
                        return dataTable;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetBC: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
    }
}
