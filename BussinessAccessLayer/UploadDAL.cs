using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BussinessAccessLayer
{
    public class UploadDAL
    {
        #region Property Declaration
        public string UserName { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string BC { get; set; }
        public string CreatedBy { get; set; }

        public string IsRemoved { get; set; }

        public string IsActive { get; set; }

        public string IsdocUploaded { get; set; }

        public string VerificationStatus { get; set; }

        public string Clientcode { get; set; }
        public int Flag { get; set; }

        #endregion

        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public static int PageRequestTimeoutInMLS = Convert.ToInt32(ConfigurationManager.AppSettings["PageRequestTimeoutInMLS"]);

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

        #region NegativeAgentDetails
        public DataSet NegativeAgentDetails()
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
                                                 new SqlParameter("@FromDate", FromDate),
                                                 new SqlParameter("@ToDate", ToDate),
                                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_NegativeAgent_Report";
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
    }
}
