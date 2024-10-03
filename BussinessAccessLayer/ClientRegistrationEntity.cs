using System;
using AppLogger;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;

namespace BussinessAccessLayer
{
    public class ClientRegistrationEntity
    {
        #region Objects Declaration
        public string UserName { get; set; }
        DataSet dataSet = null;
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        #endregion

        #region Property Declration
        public string AgentID { get; set; }
        public string ClientID { get; set; }
        public string IsRemoved { get; set; }
        public string IsActive { get; set; }
        public string VerificationStatus { get; set; }
        public int SFlag { get; set; }
        public string BCID { get; set; }
        public string IsdocUploaded { get; set; }
        #endregion

        #region BindClient
        public DataSet BindClient()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("SELECT * FROM fn_bindclient(@_ClientID, @_BCID, @_AgentID, @_IsDocUploaded, @_IsVerified, @_IsActive, @_BankStatus, @_IsRemoved, @_UserName)", sqlConn))
                    {
                        cmd.Parameters.Add("@_ClientID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ClientID) ? (object)DBNull.Value : ClientID;
                        cmd.Parameters.Add("@_BCID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(BCID) ? (object)DBNull.Value : BCID;
                        cmd.Parameters.Add("@_AgentID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(AgentID) ? (object)DBNull.Value : AgentID;
                        cmd.Parameters.Add("@_IsDocUploaded", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(IsdocUploaded) ? (object)DBNull.Value : IsdocUploaded.ToString();
                        cmd.Parameters.Add("@_IsVerified", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(VerificationStatus) ? (object)DBNull.Value : VerificationStatus.ToString();
                        cmd.Parameters.Add("@_IsActive", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(IsActive) ? (object)DBNull.Value : IsActive.ToString();
                        cmd.Parameters.Add("@_BankStatus", NpgsqlTypes.NpgsqlDbType.Varchar).Value = DBNull.Value; // or null if needed
                        cmd.Parameters.Add("@_IsRemoved", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(IsRemoved) ? (object)DBNull.Value : IsRemoved.ToString();
                        cmd.Parameters.Add("@_UserName", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(UserName) ? (object)DBNull.Value : UserName;

                        // Open the connection and execute the command
                        sqlConn.Open();
                        DataSet dataSet = new DataSet();
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.writeLogEmailError($"Class: ClientRegistrationEntity.cs \nFunction: BindClient() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;  // Rethrow to allow further handling if necessary
            }
        }
        #endregion
    }
}
