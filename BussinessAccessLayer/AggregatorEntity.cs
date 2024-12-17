using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BussinessAccessLayer
{
    public class AggregatorEntity
    {
        #region Object Declaration
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        static int CommandTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeOut"]);
        #endregion

        #region Properties
        public string AgentCode { get; set; }
        public string ClientId { get; set; }
        public string CreatedBy { get; set; }
        public string AgentName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string PassportNo { get; set; }
        public string PersonalEmailID { get; set; }
        public string FirstName { get; set; }
        public string AgentDOB { get; set; }
        public string LandlineNo { get; set; }
        public string AlternateNo { get; set; }
        public string AadharNo { get; set; }
        public string PanNo { get; set; }
        public string DeviceCode { get; set; }
        public string GSTNo { get; set; }
        public string AgentDistrict { get; set; }
        public string AgentAddress { get; set; }
        public string AgentCountry { get; set; }
        public string AgentState { get; set; }
        public string PopulationGroup { get; set; }
        public string AgentCity { get; set; }
        public string AgentPincode { get; set; }
        public string BCCode { get; set; }
        public string AgentCategory { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string Bank { get; set; }
        public string ShopAddress { get; set; }
        public string shopemail { get; set; }
        public string ShopCountry { get; set; }
        public string ShopState { get; set; }
        public string ShopCity { get; set; }
        public string ShopDistrict { get; set; }
        public string ShopPinCode { get; set; }
        public int IsPANVerified { get; set; }
        public int IsNSVerified { get; set; }
        public int IsIBAVerified { get; set; }
        public int KYCTypeId { get; set; }
        public string KYCType { get; set; }
        public string IdentityProofType { get; set; }
        public string IdentityProofDocument { get; set; }
        public string AddressProofType { get; set; }
        public string SignatureProofType { get; set; }
        public string AddressProofDocument { get; set; }
        public string SignatureProofDocument { get; set; }
        public string BusinessEmailID { get; set; }
        public int agReqId { get; set; }
        public string Documents { get; set; }
        public string DocumentType { get; set; }
        public int AEPS { get; set; }
        public int MATM { get; set; }
        public int StageId { get; set; }
        public int Stage { get; set; }
        public int Flag { get; set; }
        public int RequestId { get; set; }
        public int Status { get; set; }
        public string StatusMsg = null;
        public string UserName { get; set; }
        public string RequestTypeId { get; set; }
        public string BucketId { get; set; }
        public string RequestStatusId { get; set; }
        public string IsRemoved { get; set; }
        public string IsActive { get; set; }
        public int VerificationStatus { get; set; }
        public int SFlag { get; set; }
        public string IsdocUploaded { get; set; }
        public string AgentID { get; set; }
        public string TerminalId { get; set; }
        public string BCstatus { get; set; }
        public string Mstatus { get; set; }
        public int ApiReqId { get; set; }

        public string Prestatus { get; set; }
        public string ChStatus { get; set; }
        public string AtStatus { get; set; }
        public string AgentReqId { get; set; }
        public string Activity { get; set; }
        public string ATRemark { get; set; }
        public string CheckerRemark { get; set; }
        public string MakerRemark { get; set; }
        public string ActionType { get; set; }
        public int ActivityType { get; set; }
        public string BusinessEmail { get; set; }
        public string PersonalContact { get; set; }
        public string _fileLineNo { get; set; }
        public string Mode { get; set; }
        public string _RandomStringForSalt { get; set; }
        public string Password { get; set; }
        public int DocumentID { get; set; }
        public string BcRemarks { get; set; }
        public string VerificationLevel { get; set; }
        public int RoleID { get; set; }
        public string OperationType { get; set; }
        public string FileName { get; set; }
        public int mode { get; set; }
        public string FileID { get; set; }

        public string Fromdate { get; set; }
        public string Todate { get; set; }

        public string ParentRoleID { get; set; }

        public string Lattitude { get; set; }

        public string Longitude { get; set; }
        #endregion
        

        public DataTable GetBC(string UserName, int? VerificationStatus, int? IsActive, int? IsRemoved, string ClientID, int? IsdocUploaded)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                        {
                            cmd.Connection = sqlConn;
                            cmd.CommandText = "SELECT * FROM SP_BindBC_Reports(@p_UserName, @p_IsVerified, @p_IsActive, @p_IsRemoved, @p_ClientID, @p_IsDocUploaded)";

                            // Set parameters with p_ prefix
                            cmd.Parameters.Add(new NpgsqlParameter("p_UserName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)UserName ?? DBNull.Value });
                            cmd.Parameters.Add(new NpgsqlParameter("p_IsVerified", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)VerificationStatus ?? DBNull.Value });
                            cmd.Parameters.Add(new NpgsqlParameter("p_IsActive", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)IsActive ?? DBNull.Value });
                            cmd.Parameters.Add(new NpgsqlParameter("p_IsRemoved", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)IsRemoved ?? DBNull.Value });
                            cmd.Parameters.Add(new NpgsqlParameter("p_ClientID", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)ClientID ?? DBNull.Value });
                            cmd.Parameters.Add(new NpgsqlParameter("p_IsDocUploaded", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)IsdocUploaded ?? DBNull.Value });

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