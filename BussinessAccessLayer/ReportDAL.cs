using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BussinessAccessLayer
{

    public class ReportDAL
    {
        /////DbAccess _dbAccess = new DbAccess();
        public int PageRequestTimeoutInMLS = Convert.ToInt32(ConfigurationManager.AppSettings["PageRequestTimeoutInMLS"]);
        public string BcStatus { get; set; }
        public int Flag { get; set; }

        public string flag { get; set; }
        public int CHStatus { get; set; }
        public string ATStatus { get; set; }
        public string CHstatus { get; set; }
        public string Clientcode { get; set; }
        public int Status { get; set; }
        public string UserName { get; set; }
        public int PageIndex { get; set; }
        public string PINCode { get; set; }
        public string VerificationLevel { get; set; }

        public string OperationType { get; set; }

        public string BCAgentID { get; set; }
        public int ActivityType { get; set; }

        public string CreatedBy { get; set; }
        public string IsRemoved { get; set; }
        public string IsActive { get; set; }
        public string IsdocUploaded { get; set; }
        public string VerificationStatus { get; set; }
        public string BCID { get; set; }

        public string AgentReqId { get; set; }
        public string ActionType { get; set; }
        public string AgentCode { get; set; }
        public string Mstatus { get; set; }
        public string BcRemarks { get; set; }
        public string BCstatus { get; set; }
        public string Activity { get; set; }

        public string KYC { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }

        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

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
                ErrorLog.ReportTrace("AgentRegistrationDAL: BindBC: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

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
                        /////dataSet = _dbAccess.SelectRecordsWithParams("SP_BindBC", _paramsAggregatorDetails);
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindBC_Reports";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
                        /////DataTable dataTable = new DataTable();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                        return dataSet;

                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.ReportTrace("AgentRegistrationDAL: GetCountryStateCity: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }

        }
        #endregion


        #region GetActiveInactiveAgentDetails
        public DataSet GetActiveInactiveAgentDetails()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _paramsAggregatorDetails = {
                                             new SqlParameter("@UserName",UserName),
                                             new SqlParameter("@BCCode",BCID),
                                             new SqlParameter("@AgentCode",AgentCode),
                                             new SqlParameter("@ActivityType",Activity),
                                             new SqlParameter("@IsActive",IsActive),
                                             new SqlParameter("@KYCType",KYC),
                                             new SqlParameter("@FromDate", Fromdate),
                                             new SqlParameter("@ToDate", Todate),
                                             new SqlParameter("@Flag",Flag),
                                            };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_ActiveInactiveAgent";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
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
                ErrorLog.ReportTrace("AgentRegistrationDAL: BindBC: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetBCActiveInactiveDetails
        public DataSet GetBCActiveInactiveDetails()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _paramsAggregatorDetails = {
                                            new SqlParameter("@BCID",BCID),
                                            new SqlParameter("@ClientID",Clientcode),
                                            new SqlParameter("@AgentReqId", AgentReqId),
                                            //new SqlParameter("@State", State),
                                            new SqlParameter("@User",UserName),
                                            new SqlParameter("@IsDocUploaded",IsdocUploaded),
                                            new SqlParameter("@BCstatus",BCstatus),
                                            new SqlParameter("@Makerstatus",Mstatus),
                                            new SqlParameter("@Checkstatus",CHstatus),
                                            new SqlParameter("@Authstatus",ATStatus),
                                            new SqlParameter("@IsVerified",VerificationStatus),
                                             new SqlParameter("@ActivityType",Activity),
                                            new SqlParameter("@IsActive",IsActive),
                                            new SqlParameter("@IsRemoved",IsRemoved),
                                            new SqlParameter("@FromDate", Fromdate),
                                             new SqlParameter("@ToDate", Todate),
                                            new SqlParameter("@Flag",Flag),
                                            };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_ActiveInactiveBC";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
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
                ErrorLog.ReportTrace("AgentRegistrationDAL: BindBC: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetAggWiseActiveInactiveAgents
        public DataSet GetAggWiseActiveInactiveAgents()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _paramsAggregatorDetails = {
                                             new SqlParameter("@UserName",UserName),
                                             new SqlParameter("@BCCode",BCID),
                                             new SqlParameter("@AgentCode",AgentCode),
                                             new SqlParameter("@ActivityType",Activity),
                                             new SqlParameter("@IsActive",IsActive),
                                             new SqlParameter("@KYCType",KYC),
                                             new SqlParameter("@FromDate", Fromdate),
                                             new SqlParameter("@ToDate", Todate),
                                             new SqlParameter("@Flag",Flag),
                                            };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_AggWiseActiveInactiveAgent";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
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
                ErrorLog.ReportTrace("AgentRegistrationDAL: BindBC: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region AgentStatusReportGrid
       
        public DataSet AgentStatusReportGrid()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                                       new SqlParameter("@User", UserName),
                                                       new SqlParameter("@FromDate", Fromdate),
                                                       new SqlParameter("@ToDate", Todate),
                                                       new SqlParameter("@Flag", Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_PendingAgentReg";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        cmd.CommandTimeout = PageRequestTimeoutInMLS;
                        DataSet ds = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(ds);
                        cmd.Dispose();
                        return ds;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.ReportTrace("AgentRegistrationDAL: GetAgentRequestList: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region BindClient  
        public DataSet BindClient()
        {
            DataSet dataSet = new DataSet();
            try
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _paramsAggregatorDetails = {
                    new SqlParameter("@ClientID",Clientcode),
                    new SqlParameter("@BCID",BCAgentID),
                    new SqlParameter("@AgentID", BCAgentID),

                    new SqlParameter("@IsDocUploaded",IsdocUploaded),
                    new SqlParameter("@IsVerified", VerificationStatus),
                    new SqlParameter("@IsActive",IsActive),
                    new SqlParameter("@BankStatus",null),
                    new SqlParameter("@IsRemoved", IsRemoved),
                    new SqlParameter("@UserName",UserName),                      
                };
                        cmd.CommandText = "SP_BindClient";

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
                ErrorLog.CommonTrace("Class : ClientRegistrationEntity.cs \nFunction : BindClient() \nException Occured\n" + Ex.Message);
            }
            return dataSet;
        }
        #endregion


        public DataSet BCStatusReportGrid()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                                       new SqlParameter("@User", UserName),
                                                       new SqlParameter("@FromDate", Fromdate),
                                                       new SqlParameter("@ToDate", Todate),
                                                       new SqlParameter("@Flag", Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_PendingBCReg";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        cmd.CommandTimeout = PageRequestTimeoutInMLS;
                        DataSet ds = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(ds);
                        cmd.Dispose();
                        return ds;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.ReportTrace("AgentRegistrationDAL: BCStatusReportGrid: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #region GetAllagentReport
        public DataSet GetAllagentReport()
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
                        new SqlParameter("@UserName", UserName),
                        new SqlParameter("@FromDate", Fromdate),
                        new SqlParameter("@ToDate", Todate),
                        new SqlParameter("@BCCode", BCID),
                        new SqlParameter("@Flag", Flag) }; cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_GetAgentAllRequestedData";
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
        #region Restricted PIN
        public DataSet RestrictedPIN()
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
                                            new SqlParameter("@UserName", UserName),
                                            new SqlParameter("@PINCode", PINCode),
                                            //new SqlParameter("@PageIndex", PageIndex),
                                            new SqlParameter("@Flag", flag)
               };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_GetRestrictedPIN";
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
    }
}
