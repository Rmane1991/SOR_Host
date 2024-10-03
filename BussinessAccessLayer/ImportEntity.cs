using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace BussinessAccessLayer
{
    public class ImportEntity
    {
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public static int PageRequestTimeoutInMLS = Convert.ToInt32(ConfigurationManager.AppSettings["PageRequestTimeoutInMLS"]);//ConnectionString

        public DataTable dataTable = new DataTable();

        #region Properties
        public string UserName { get; set; }
        public int Flag { get; set; }

        public string _FromDate, _ToDate, FileDate;
        private SqlCommand cmd;
        public string filename { get; set; }



        public int FileTypeId { get; set; }
        public string ClientID { get; set; }
        public string RRN { get; set; }
        public string FileId { get; set; }
        public int Fileid { get; set; }
        public string FileStatus { get; set; }
        public string CategoryId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string FileDescID { get; set; }
        public string CategoryType { get; set; }
        public string FileDescName { get; set; }
        public string Mode { get; set; }
        public string AgentName { get; set; }

      
           
        public string ToDate { get; set; }
        public string FromDate { get; set; }

        public string AgentCode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string AadhaarNo { get; set; }
        public string PANNo { get; set; }
        public string MobileNo { get; set; }
        public string DateOfBlackListing { get; set; }
        public string ReasonForBlackListing { get; set; }
        public string PINCODE { get; set; }
        public string CorporateBCName { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }

        public string AccountNo { get; set; }

        public string IsPoliceComplaint { get; set; }
        public string AccountNoIsPoliceComplaint { get; set; }
        public string IfFIRCompliant { get; set; }

        public string DateofComplaint { get; set; }

        public string IsRemoved { get; set; }
        public string IsBCAgentArrested { get; set; }
        public string RecordStatusDescription { get; set; }
        public int IsValidRecord { get; set; }
        public string RecordStatus { get; set; }

        public int RecordID { get; set; }
        public string BC { get; set; }

        public string Remarks { get; set; }
        public string FileID { get; set; }

        public string FileIDEdit { get; set; }

        public DataTable Dtable;

        public string BCstatus { get; set; }
        public string Mstatus { get; set; }
        public string ChStatus { get; set; }
        public string AtStatus { get; set; }

        #endregion

        public string InsertFileImport(out string FileId)
        {
            FileId = string.Empty;
            DataSet dataSet = new DataSet();
            string Status = string.Empty;
            string StatusDesc = string.Empty;

            try
            {
                DataTable dataTable = new DataTable();
                SqlParameter[] _Params =
                {
                   new SqlParameter("@Username", UserName),
                   new SqlParameter("@FileName", FileName),
                   new SqlParameter("@FilePath", FilePath),
                   new SqlParameter("@FileType", FileType),
                   new SqlParameter("@FileDescID", FileDescID),
                   new SqlParameter("@FileDesc", FileDescName),
                   new SqlParameter("@Status", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output },
                   new SqlParameter("@StatusDesc", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output },
                   new SqlParameter("@FileID", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output }
                };
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(ConnectionString);
                cmd.CommandTimeout = PageRequestTimeoutInMLS;
                cmd.CommandText = "Proc_InsertFileImportDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(_Params);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                Status = cmd.Parameters["@Status"] != null && !string.IsNullOrEmpty(cmd.Parameters["@Status"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@Status"].Value) : null;
                StatusDesc = cmd.Parameters["@StatusDesc"] != null && !string.IsNullOrEmpty(cmd.Parameters["@StatusDesc"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@StatusDesc"].Value) : null;
                FileId = cmd.Parameters["@FileID"] != null && !string.IsNullOrEmpty(cmd.Parameters["@FileID"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@FileID"].Value) : null;
                // return dataTable;
            }
            catch (Exception Ex)
            {
                // systemLogger.DBErrorLog(this, Ex);
                throw;
            }
            return Status;
        }

        public string InsertBulkTerminalDetails(DataTable dt, out string Status, out string StatusDesc, string FileId)
        {
            DataSet dataSet = new DataSet();
            string _Status = string.Empty;
            string _StatusDesc = string.Empty;
            try
            {
                DataTable dataTable = new DataTable();
                SqlParameter[] _Params =
                {
                   new SqlParameter("@NegativeAgentTT", dt),
                   new SqlParameter("@CreatedBy", UserName),
                   new SqlParameter("@FileID", FileId),
                   new SqlParameter("@Status", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output },
                   new SqlParameter("@StatusDesc", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output }

            };
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(ConnectionString);
                cmd.CommandTimeout = PageRequestTimeoutInMLS;
                cmd.CommandText = "SP_NegativeAgent_Insert";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(_Params);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                Status = cmd.Parameters["@Status"] != null && !string.IsNullOrEmpty(cmd.Parameters["@Status"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@Status"].Value) : null;
                StatusDesc = cmd.Parameters["@StatusDesc"] != null && !string.IsNullOrEmpty(cmd.Parameters["@StatusDesc"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@StatusDesc"].Value) : null;

            }
            catch (Exception Ex)
            {
                // systemLogger.DBErrorLog(this, Ex);
                throw;
            }
            return Status;
        }
        public string InsertBulkCLLDetails(DataTable dt, out string Status, out string StatusDesc, string FileId)
        {
            DataSet dataSet = new DataSet();
            string _Status = string.Empty;
            string _StatusDesc = string.Empty;
            try
            {
                DataTable dataTable = new DataTable();
                SqlParameter[] _Params =
                {
                   new SqlParameter("@TT_BulkUploadTerminal", dt),
                   new SqlParameter("@UserName", UserName),
                   new SqlParameter("@fileID", FileId),
                   new SqlParameter("@MSP", BC),
                   new SqlParameter("@Status", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output },
                   new SqlParameter("@StatusDesc", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output }

            };
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(ConnectionString);
                cmd.CommandTimeout = PageRequestTimeoutInMLS;
                cmd.CommandText = "SP_UploadTerminals";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(_Params);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                Status = cmd.Parameters["@Status"] != null && !string.IsNullOrEmpty(cmd.Parameters["@Status"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@Status"].Value) : null;
                StatusDesc = cmd.Parameters["@StatusDesc"] != null && !string.IsNullOrEmpty(cmd.Parameters["@StatusDesc"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@StatusDesc"].Value) : null;

            }
            catch (Exception Ex)
            {
                // systemLogger.DBErrorLog(this, Ex);
                throw;
            }
            return Status;
        }

        public DataSet ExportRestrictedName()
        {
            DataSet dataSet = new DataSet();
            string Status = string.Empty;
            try
            {
                DataTable dataTable = new DataTable();
                SqlParameter[] _Params =
                {
                   new SqlParameter("@FromDate", _FromDate),
                   new SqlParameter("@ToDate", _ToDate),
                   new SqlParameter("@Username", UserName),
                   new SqlParameter("@Status", FileStatus),
                   new SqlParameter("@FileID", FileID),
                   new SqlParameter("@FileDescID", FileDescID),
                   new SqlParameter("@Flag", Flag),

            };
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(ConnectionString);
                cmd.CommandTimeout = PageRequestTimeoutInMLS;
                cmd.CommandText = "Proc_RestrictedName_GetExcel";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(_Params);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataSet);
                //Status = cmd.Parameters["@Status"] != null && !string.IsNullOrEmpty(cmd.Parameters["@Status"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@Status"].Value) : null;
                // return dataTable;
            }
            catch (Exception Ex)
            {
                // systemLogger.DBErrorLog(this, Ex);
                throw;
            }

            return dataSet;
        }
        #region ExportRestrictedPIN
        public DataSet ExportRestrictedPIN()
        {
            DataSet dataSet = new DataSet();
            string Status = string.Empty;
            try
            {
                DataTable dataTable = new DataTable();
                SqlParameter[] _Params =
                {
                   new SqlParameter("@FromDate", _FromDate),
                   new SqlParameter("@ToDate", _ToDate),
                   new SqlParameter("@Username", UserName),
                   new SqlParameter("@Status", FileStatus),
                   new SqlParameter("@FileID", FileID),
                   new SqlParameter("@FileDescID", FileDescID),
                   new SqlParameter("@Flag", Flag),

            };
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(ConnectionString);
                cmd.CommandTimeout = PageRequestTimeoutInMLS;
                cmd.CommandText = "Proc_RestrictedPIN_GetExcel";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(_Params);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataSet);
                //Status = cmd.Parameters["@Status"] != null && !string.IsNullOrEmpty(cmd.Parameters["@Status"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@Status"].Value) : null;
                // return dataTable;
            }
            catch (Exception Ex)
            {
                // systemLogger.DBErrorLog(this, Ex);
                throw;
            }

            return dataSet;
        }
        #endregion

        #region InsertFileImportPIN
        public string InsertFileImportPIN(out string FileId)
        {
            FileId = string.Empty;
            DataSet dataSet = new DataSet();
            string Status = string.Empty;
            string StatusDesc = string.Empty;

            try
            {
                DataTable dataTable = new DataTable();
                SqlParameter[] _Params =
                {
                   new SqlParameter("@Username", UserName),
                   new SqlParameter("@FileName", FileName),
                   new SqlParameter("@FilePath", FilePath),
                   new SqlParameter("@FileType", FileType),
                   new SqlParameter("@FileDescID", FileDescID),
                   new SqlParameter("@FileDesc", FileDescName),
                   new SqlParameter("@Status", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output },
                   new SqlParameter("@StatusDesc", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output },
                   new SqlParameter("@FileID", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output }
                };
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(ConnectionString);
                cmd.CommandTimeout = PageRequestTimeoutInMLS;
                cmd.CommandText = "Proc_InsertFileImportDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(_Params);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                Status = cmd.Parameters["@Status"] != null && !string.IsNullOrEmpty(cmd.Parameters["@Status"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@Status"].Value) : null;
                StatusDesc = cmd.Parameters["@StatusDesc"] != null && !string.IsNullOrEmpty(cmd.Parameters["@StatusDesc"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@StatusDesc"].Value) : null;
                FileId = cmd.Parameters["@FileID"] != null && !string.IsNullOrEmpty(cmd.Parameters["@FileID"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@FileID"].Value) : null;
                // return dataTable;
            }
            catch (Exception Ex)
            {
                // systemLogger.DBErrorLog(this, Ex);
                throw;
            }
            return Status;
        }
        #endregion

        #region 
        public string InsertBulkTerminalDetailsPIN(DataTable dt, out string Status, out string StatusDesc, string FileId)
        {
            DataSet dataSet = new DataSet();
            string _Status = string.Empty;
            string _StatusDesc = string.Empty;
            try
            {
                DataTable dataTable = new DataTable();
                SqlParameter[] _Params =
                {
                   new SqlParameter("@RestrictedPINTT", dt),
                   new SqlParameter("@CreatedBy", UserName),
                   new SqlParameter("@FileID", FileId),
                   new SqlParameter("@Status", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output },
                   new SqlParameter("@StatusDesc", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output }

            };
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(ConnectionString);
                cmd.CommandTimeout = PageRequestTimeoutInMLS;
                cmd.CommandText = "SP_RestrictedPIN_Insert";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(_Params);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                Status = cmd.Parameters["@Status"] != null && !string.IsNullOrEmpty(cmd.Parameters["@Status"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@Status"].Value) : null;
                StatusDesc = cmd.Parameters["@StatusDesc"] != null && !string.IsNullOrEmpty(cmd.Parameters["@StatusDesc"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@StatusDesc"].Value) : null;

            }
            catch (Exception Ex)
            {
                // systemLogger.DBErrorLog(this, Ex);
                throw;
            }
            return Status;
        }
        #endregion

        public DataSet Get_AgentManualKycUpload()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {
                                                
                                                //new SqlParameter("@Status", FileStatus),
                                                new SqlParameter("@UserName",UserName),
                                                new SqlParameter("@Flag", Flag) ,
                                                new SqlParameter("@FileID", Fileid) ,
                                                //new SqlParameter("@ClientID",ClientID)
                                                new SqlParameter("@FromDate",FromDate),
                                                new SqlParameter("@ToDate",ToDate)
                                            };
                        cmd.Connection = new SqlConnection(ConnectionString);
                        cmd.CommandTimeout = PageRequestTimeoutInMLS;
                        cmd.CommandText = "SP_BulkUploadAgentRestriction_Get";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dataSet);
                    }
                }
                //dataSet = _dbAccess.SelectRecordsWithParams("SP_BulkUploadAgentRestriction_Get", _Params);
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : ImportEntity.cs \nFunction : GetFileImportDetails_ManualcommissionUpload() \nException Occured\n" + Ex.Message);
            }
            return dataSet;
        }


        public string InsertBulk(out string _StatusMsg)
        {
            string Status = string.Empty;
            string StatusMsg = string.Empty;
            //dataSet = new DataSet();
            try
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                    {
                                           new SqlParameter("@TTBulk", dataTable),
                                           new SqlParameter("@FileID", FileId),
                                           new SqlParameter("@Username", UserName),
                                           new SqlParameter("@RRN",RRN),
                                           //new SqlParameter("@FileTypeID", FileTypeId),
                                           new SqlParameter("@OutStatus", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
                                           new SqlParameter("@OutStatusMsg", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output }
                                        };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UploadBulk_Insert"; 
                       // cmd.CommandText = "SP_UploadBulk_Insert_02032023";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        Status = Convert.ToString(cmd.Parameters["@OutStatus"].Value);
                        StatusMsg = Convert.ToString(cmd.Parameters["@OutStatusMsg"].Value);
                        //StatusMsg = Convert.ToString(cmd.Parameters["@Status_Out"].Value);
                        sqlConn.Close();
                        cmd.Dispose();
                        //return Status;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : ImportEntity.cs \nFunction : InsertAgentBulkRegirstration() \nException Occured\n" + Ex.Message);
            }
            _StatusMsg = StatusMsg;
            return Status;
        }


        public string InsertManuaBulkImportDetailsAgent(out string FileId)
        {
            try
            {
                FileId = string.Empty;
                string Status = string.Empty;
                string StatusMsg = string.Empty;
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                    {
                    new SqlParameter("@FileName", filename),
                                              new SqlParameter("@FilePath", FilePath),
                                              new SqlParameter("@FileType",FileType),
                                              new SqlParameter("@ClientId", ClientID),
                                              new SqlParameter("@FileDescID", FileDescID),
                                              new SqlParameter("@FileDesc", FileDescName),
                                              new SqlParameter("@CreateBy", UserName),
                                              new SqlParameter("@Mode", Mode),
                                              new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
                                              new SqlParameter("@Status_Out", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output }
                };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UploadBulkAgent_Import";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        FileId = Convert.ToString(cmd.Parameters["@Status_Out"].Value);
                        sqlConn.Close();
                        cmd.Dispose();
                        return Status;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: SetInsertUpdateAgentMasterDetails: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
            //return Status;
        }

        public DataSet Get_AgentBulkUpload()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {
                                                //new SqlParameter("@Status", FileStatus),
                                                //new SqlParameter("@UserName",UserName),
                                                new SqlParameter("@Flag", Flag) ,
                                                new SqlParameter("@FileID", FileID) ,
                                                 new SqlParameter("@BCstatus",BCstatus),
                                                 new SqlParameter("@Makerstatus",Mstatus),
                                                 new SqlParameter("@Checkstatus",ChStatus),
                                                 new SqlParameter("@Authstatus",AtStatus),
                                                 };
                        cmd.Connection = new SqlConnection(ConnectionString);
                        //    cmd.CommandTimeout = PageRequestTimeoutInMLS;
                        cmd.CommandText = "SP_BulkUploadAgReg_Get";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dataSet);
                    }
                }
                //dataSet = _dbAccess.SelectRecordsWithParams("SP_BulkUploadAgentRestriction_Get", _Params);
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : ImportEntity.cs \nFunction : GetFileImportDetails_ManualcommissionUpload() \nException Occured\n" + Ex.Message);
            }
            return dataSet;
        }

    }
}