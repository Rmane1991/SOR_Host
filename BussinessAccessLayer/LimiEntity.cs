using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLogger;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using MaxiSwitch.EncryptionDecryption;

namespace BussinessAccessLayer
{
    public class LimiEntity
    {
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        #region Objects Declaration

        DataSet dataSet = null;

        #endregion

        #region Property Declaration
        public string UserName { get; set; }
        public int Flag { get; set; }
        public string CreatedBy { get; set; }
        public string IsRemoved { get; set; }
        public string IsActive { get; set; }
        public string IsdocUploaded { get; set; }
        public string VerificationStatus { get; set; }

        public string Clientcode { get; set; }
        public string ChannelID { get; set; }
        public string KYC_Type { get; set; }
        public string BCCode { get; set; }
        public string Amount { get; set; }
        public string Remark { get; set; }
        public string BCFranchiseID { get; set; }
        public string AgentId { get; set; }
        public string BCID { get; set; }
        public string ValidateAmount { get; set; }

        #endregion

        #region BindBC
        public DataSet BindBC()
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
                ErrorLog.LimitTrace("LimiEntity: BindBC: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }

        }
        #endregion

        #region BindChannel
        public DataSet BindChannel()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _paramsAggregatorDetails = {
                                                 new SqlParameter("@UserName",UserName),
                                                 new SqlParameter("@Flag", Flag)
                                             };
                        ////dataSet = _dbAccess.SelectRecordsWithParams("SP_BindLimitDropDN", _paramsAggregatorDetails);
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindLimitDropDN";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
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
                ErrorLog.LimitTrace("LimiEntity: BindChannel: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region BindKYCType
        public DataSet BindKYCType()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _paramsAggregatorDetails = {
                                                 new SqlParameter("@UserName",UserName),
                                                 new SqlParameter("@Flag", Flag)
                                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindLimitDropDN";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
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
                ErrorLog.LimitTrace("LimiEntity: BindKYCType: UserName: " + UserName + " Exception: " + Ex.Message);
            }
            return dataSet;
        }
        #endregion

        #region InsertLimitGrid
        public DataSet InsertLimitGrid(out string Status)
        {
            Status = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _paramsAggregatorDetails = {
                                                 new SqlParameter("@Remark", Remark),
                                                 new SqlParameter("@BCCode",BCCode),
                                                 new SqlParameter("@ChannelId",ChannelID),
                                                 //new SqlParameter("@DurationId",Duration),
                                                 new SqlParameter("@KycTypeId",KYC_Type),
                                                 new SqlParameter("@Amount",Amount),
                                                 new SqlParameter("@UserName",UserName),
                                                 new SqlParameter("@Flag", Flag),
                                                 new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output }
                                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_InsertLimitHandler";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(ds);
                        Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        cmd.Dispose();
                        //return ds;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.LimitTrace("LimiEntity: InsertLimitGrid: UserName: " + UserName + " Exception: " + Ex.Message);
            }
            return ds;
        }
        #endregion

        #region ValidateLimitAmount
        public DataSet ValidateLimitAmount()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _paramsAggregatorDetails = {
                                                 new SqlParameter("@ChannelId", ChannelID),
                                                 new SqlParameter("@UserName",UserName),
                                                 new SqlParameter("@Flag", Flag)
                                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_ValidateLimitAmount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
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
                ErrorLog.LimitTrace("LimiEntity: ValidateLimitAmount: UserName: " + UserName + " Exception: " + Ex.Message);
            }
            return dataSet;
        }
        #endregion

        #region BindLimitGrid
        public DataSet BindLimitGridPending()
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
                                                 new SqlParameter("@BCCode",BCID),
                                                 new SqlParameter("@IsVerified", VerificationStatus),
                                                 new SqlParameter("@IsActive",IsActive),
                                                 new SqlParameter("@Remark", Remark),
                                                 new SqlParameter("@UserName",UserName),
                                                 new SqlParameter("@Flag", Flag)
                                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindLimitHandler_Pending";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
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
                ErrorLog.LimitTrace("LimiEntity: BindLimitGrid: UserName: " + UserName + " Exception: " + Ex.Message);
            }
            return dataSet;
        }
        #endregion

        #region ChangeLimitStatus
        public DataSet ChangeLimitStatus()
        {
            dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@Remarks",Remark),
                                            new SqlParameter("@UserName",UserName),
                                            new SqlParameter("@Flag",Flag),
                                            new SqlParameter("@BCCode",BCCode),

                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UpdateLimitOnboardProcess";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
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
                ErrorLog.LimitTrace("LimiEntity: ChangeLimitStatus: UserName: " + UserName + " Exception: " + Ex.Message);
            }
            return dataSet;
        }
        #endregion

        #region BindLimitStatusApprove
        public DataSet BindLimitStatusApprove()
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
                                                 new SqlParameter("@BCCode",BCID),
                                                 new SqlParameter("@IsVerified", VerificationStatus),
                                                 new SqlParameter("@IsActive",IsActive),
                                                 new SqlParameter("@Remark", Remark),
                                                 new SqlParameter("@UserName",UserName),
                                                 new SqlParameter("@Flag", Flag)
                                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindLimitStatus_Approve";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
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
                ErrorLog.LimitTrace("LimiEntity: BindLimitStatus: UserName: " + UserName + " Exception: " + Ex.Message);
            }
            return dataSet;
        }
        #endregion

        #region BindLimitGridApprove
        public DataSet BindLimitGridApprove()
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
                                                 new SqlParameter("@BCCode",BCID),
                                                 new SqlParameter("@IsVerified", VerificationStatus),
                                                 new SqlParameter("@IsActive",IsActive),
                                                 new SqlParameter("@Remark", Remark),
                                                 new SqlParameter("@UserName",UserName),
                                                 new SqlParameter("@Flag", Flag)
                                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindLimitHandler_Approve";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
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
                ErrorLog.LimitTrace("LimiEntity: BindLimitGrid: UserName: " + UserName + " Exception: " + Ex.Message);
            }
            return dataSet;
        }
        #endregion
        #region BindLimitStatusPending
        public DataSet BindLimitStatusPending()
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
                                                 new SqlParameter("@BCCode",BCID),
                                                 new SqlParameter("@IsVerified", VerificationStatus),
                                                 new SqlParameter("@IsActive",IsActive),
                                                 new SqlParameter("@Remark", Remark),
                                                 new SqlParameter("@UserName",UserName),
                                                 new SqlParameter("@Flag", Flag)
                                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindLimitStatus_Pending";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_paramsAggregatorDetails);
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
                ErrorLog.LimitTrace("LimiEntity: BindLimitStatus: UserName: " + UserName + " Exception: " + Ex.Message);
            }
            return dataSet;
        }
        #endregion
    }
}
