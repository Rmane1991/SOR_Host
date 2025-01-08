using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using Newtonsoft.Json;
using Npgsql;
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

        public string InsertFileImport(out string fileId)
        {
            fileId = string.Empty;
            string status = string.Empty;
            string statusDesc = string.Empty;

            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Prepare the CALL statement
                        cmd.CommandText = "CALL Proc_InsertFileImportDetails(null, null, null, @p_Username, @p_FileName, @p_FilePath, @p_FileType, @p_FileDateTime, @p_ClientId, @p_FileDescID, @p_FileDesc, @p_Mode, @p_CreateBy, @p_ActionTypeID, @p_ActionType)";
                        cmd.CommandType = CommandType.Text;

                        // OUT parameters
                        var outStatusParam = new NpgsqlParameter("@p_Status", NpgsqlTypes.NpgsqlDbType.Varchar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var outStatusDescParam = new NpgsqlParameter("@p_StatusDesc", NpgsqlTypes.NpgsqlDbType.Varchar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var outFileIdParam = new NpgsqlParameter("@p_FileID", NpgsqlTypes.NpgsqlDbType.Varchar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };

                        // Add parameters corresponding to the procedure
                        cmd.Parameters.Add(outStatusParam);
                        cmd.Parameters.Add(outStatusDescParam);
                        cmd.Parameters.Add(outFileIdParam);
                        cmd.Parameters.AddWithValue("@p_Username", UserName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileName", FileName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FilePath", FilePath ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileType", FileType ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileDateTime", DBNull.Value); // Use DBNull for null value
                        cmd.Parameters.AddWithValue("@p_ClientId", ClientID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileDescID", FileDescID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileDesc", FileDescName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Mode", Mode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_CreateBy", UserName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ActionTypeID", null ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ActionType", null ?? (object)DBNull.Value);

                        // Execute the command
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();

                        // Retrieve OUT parameters
                        status = Convert.ToString(outStatusParam.Value);
                        statusDesc = Convert.ToString(outStatusDescParam.Value);
                        fileId = Convert.ToString(outFileIdParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log your error here
                ErrorLog.AgentManagementTrace($"AgentRegistrationDAL: InsertFileImport: UserName: {UserName} Exception: {ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }

            return status;
        }



        //public string InsertBulkTerminalDetails(DataTable dt, out string Status, out string StatusDesc, string FileId)
        //{
        //    DataSet dataSet = new DataSet();
        //    string _Status = string.Empty;
        //    string _StatusDesc = string.Empty;
        //    try
        //    {
        //        DataTable dataTable = new DataTable();
        //        SqlParameter[] _Params =
        //        {
        //           new SqlParameter("@NegativeAgentTT", dt),
        //           new SqlParameter("@CreatedBy", UserName),
        //           new SqlParameter("@FileID", FileId),
        //           new SqlParameter("@Status", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output },
        //           new SqlParameter("@StatusDesc", SqlDbType.VarChar, 100){ Direction = ParameterDirection.Output }

        //    };
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = new SqlConnection(ConnectionString);
        //        cmd.CommandTimeout = PageRequestTimeoutInMLS;
        //        cmd.CommandText = "SP_NegativeAgent_Insert";
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddRange(_Params);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dataTable);
        //        Status = cmd.Parameters["@Status"] != null && !string.IsNullOrEmpty(cmd.Parameters["@Status"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@Status"].Value) : null;
        //        StatusDesc = cmd.Parameters["@StatusDesc"] != null && !string.IsNullOrEmpty(cmd.Parameters["@StatusDesc"].Value.ToString()) ? Convert.ToString(cmd.Parameters["@StatusDesc"].Value) : null;

        //    }
        //    catch (Exception Ex)
        //    {
        //        // systemLogger.DBErrorLog(this, Ex);
        //        throw;
        //    }
        //    return Status;
        //}
        public string InsertBulkTerminalDetails(DataTable dt, out string status, out string statusDesc, string fileId)
        {
            status = string.Empty;
            statusDesc = string.Empty;
            string jsonData = DataTableToJson(dt);

            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Prepare the CALL statement
                        cmd.CommandText = "CALL SP_NegativeAgent_Insert(@p_NegativeAgentTT, @p_CreatedBy, @p_FileID, null, null)";
                        cmd.CommandType = CommandType.Text;

                        // OUT parameters
                        var outStatusParam = new NpgsqlParameter("@p_Status", NpgsqlTypes.NpgsqlDbType.Varchar, 20)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var outStatusDescParam = new NpgsqlParameter("@p_StatusDesc", NpgsqlTypes.NpgsqlDbType.Varchar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };

                        // Add parameters corresponding to the procedure
                        cmd.Parameters.AddWithValue("@p_NegativeAgentTT", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonData);
                        cmd.Parameters.AddWithValue("@p_CreatedBy", UserName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileID", fileId ?? (object)DBNull.Value);
                        cmd.Parameters.Add(outStatusParam);
                        cmd.Parameters.Add(outStatusDescParam);

                        // Execute the command
                        cmd.ExecuteNonQuery();

                        // Retrieve OUT parameters
                        status = Convert.ToString(outStatusParam.Value);
                        statusDesc = Convert.ToString(outStatusDescParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log your error here
                ErrorLog.CommonTrace($"InsertBulkTerminalDetails: UserName: {UserName} Exception: {ex.Message}");
                throw;
            }

            return status;
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
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Create a command to call the stored procedure
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL Proc_RestrictedName_GetExcel(@p_FromDate, @p_ToDate, @p_Username, @p_Status, @p_FileID, @p_FileDescID, @p_Flag)";
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        // Add parameters with p_ prefix
                        cmd.Parameters.AddWithValue("@p_FromDate", (object)_FromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ToDate", (object)_ToDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Status", (object)FileStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileID", (object)FileID ?? DBNull.Value);  // Handle potential null
                        cmd.Parameters.AddWithValue("@p_FileDescID", (object)FileDescID ?? DBNull.Value); // Handle potential null
                        cmd.Parameters.AddWithValue("@p_Flag", Flag);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Now select from the temp table based on the flag
                        string tempTableName = string.Empty;

                        if (Flag == 1)
                        {
                            tempTableName = "TempBindResults";
                        }
                        else if (Flag == 2)
                        {
                            tempTableName = "TempExportResults";
                        }

                        if (!string.IsNullOrEmpty(tempTableName))
                        {
                            cmd.CommandText = $"SELECT * FROM {tempTableName}"; // Select from the appropriate temp table
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                dataAdapter.Fill(dataSet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                ErrorLog.CommonTrace($"Class: YourClassName \nFunction: ExportRestrictedName() \nException Occurred\n{ex.Message}");
            }
            return dataSet;
        }



        #region ExportRestrictedPIN
        public DataSet ExportRestrictedPIN()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Create a command to call the stored procedure
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL Proc_RestrictedPIN_GetExcel(@p_FromDate, @p_ToDate, @p_Username, @p_Status, @p_FileID, @p_FileDescID, @p_Flag)";
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        // Add parameters with p_ prefix
                        cmd.Parameters.AddWithValue("@p_FromDate", (object)_FromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ToDate", (object)_ToDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Status", (object)FileStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileID", (object)FileID ?? DBNull.Value);  // Handle potential null
                        cmd.Parameters.AddWithValue("@p_FileDescID", (object)FileDescID ?? DBNull.Value); // Handle potential null
                        cmd.Parameters.AddWithValue("@p_Flag", Flag);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Now select from the temp table based on the flag
                        string tempTableName = string.Empty;

                        if (Flag == 1)
                        {
                            tempTableName = "TempBindResults";
                        }
                        else if (Flag == 2)
                        {
                            tempTableName = "TempExportResults";
                        }

                        if (!string.IsNullOrEmpty(tempTableName))
                        {
                            cmd.CommandText = $"SELECT * FROM {tempTableName}"; // Select from the appropriate temp table
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                dataAdapter.Fill(dataSet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                ErrorLog.CommonTrace($"Class: YourClassName \nFunction: ExportRestrictedPIN() \nException Occurred\n{ex.Message}");
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
        public string InsertBulkTerminalDetailsPIN(DataTable dt, out string status, out string statusDesc, string fileId)
        {
            status = string.Empty;
            statusDesc = string.Empty;

            // Convert DataTable to JSON string
            string jsonData = DataTableToJson(dt);

            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Prepare the CALL statement
                        cmd.CommandText = "CALL SP_RestrictedPIN_Insert(@p_RestrictedPINTT, @p_CreatedBy, @p_FileID, null, null)";
                        cmd.CommandType = CommandType.Text;

                        // OUT parameters
                        var outStatusParam = new NpgsqlParameter("@p_Status", NpgsqlTypes.NpgsqlDbType.Varchar, 20)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var outStatusDescParam = new NpgsqlParameter("@p_StatusDesc", NpgsqlTypes.NpgsqlDbType.Varchar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };

                        // Add parameters corresponding to the procedure
                        cmd.Parameters.AddWithValue("@p_RestrictedPINTT", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonData);
                        cmd.Parameters.AddWithValue("@p_CreatedBy", UserName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileID", fileId ?? (object)DBNull.Value);
                        cmd.Parameters.Add(outStatusParam);
                        cmd.Parameters.Add(outStatusDescParam);

                        // Execute the command
                        cmd.ExecuteNonQuery();

                        // Retrieve OUT parameters
                        status = Convert.ToString(outStatusParam.Value);
                        statusDesc = Convert.ToString(outStatusDescParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log your error here
                ErrorLog.CommonTrace($"InsertBulkTerminalDetailsPIN: UserName: {UserName} Exception: {ex.Message}");
                throw;
            }

            return status;
        }

        #endregion

        public DataSet Get_AgentManualKycUpload()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Create a command to call the stored procedure
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL SP_BulkUploadAgentRestriction_Get(@p_FromDate, @p_ToDate, @p_ClientID, @p_UserName, @p_Status, @p_Flag, @p_FileID)";
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        // Add parameters with p_ prefix
                        cmd.Parameters.AddWithValue("@p_FromDate", (object)FromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ToDate", (object)ToDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ClientID", DBNull.Value); // Assuming ClientID is not used here
                        cmd.Parameters.AddWithValue("@p_UserName", UserName);
                        cmd.Parameters.AddWithValue("@p_Status", DBNull.Value); // Assuming Status is not used here
                        cmd.Parameters.AddWithValue("@p_Flag", Flag);
                        cmd.Parameters.AddWithValue("@p_FileID", Fileid);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Now select from the temp table based on the flag
                        string tempTableName = string.Empty;

                        if (Flag == 1)
                        {
                            tempTableName = "temp_results_flag1";
                        }
                        else if (Flag == 2)
                        {
                            tempTableName = "temp_results_flag2";
                        }
                        else if (Flag == 3)
                        {
                            tempTableName = "temp_results_flag3";
                        }

                        if (!string.IsNullOrEmpty(tempTableName))
                        {
                            cmd.CommandText = $"SELECT * FROM {tempTableName}"; // Select from the appropriate temp table
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                dataAdapter.Fill(dataSet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {              
                ErrorLog.AgentManagementTrace("Class: ImportEntity.cs \nFunction: Get_AgentManualKycUpload() \nException Occurred\n" + ex.Message);
            }
            return dataSet;
        }

        public string DataTableToJson(DataTable dt)
        {
            return JsonConvert.SerializeObject(dt);
        }

        public string InsertBulk(out string _StatusMsg)
        {
            string Status = string.Empty;
            string StatusMsg = string.Empty;

            try
            {
                // Convert DataTable to JSON string
                string jsonData = DataTableToJson(dataTable);

                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("CALL public.sp_uploadbulk_insert(null, null, @p_dt, @p_FileID, @p_Username, @p_RRN)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // OUT parameters
                        var outStatusParam = new NpgsqlParameter("@p_OutStatus", NpgsqlTypes.NpgsqlDbType.Varchar, 10)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var outStatusMsgParam = new NpgsqlParameter("@p_OutStatusMsg", NpgsqlTypes.NpgsqlDbType.Varchar, 200)
                        {
                            Direction = ParameterDirection.Output
                        };

                        // Add parameters to the command
                        cmd.Parameters.Add(outStatusParam); // OUT parameter first
                        cmd.Parameters.Add(outStatusMsgParam); // OUT parameter second
                        cmd.Parameters.AddWithValue("@p_dt", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonData); // Input parameter
                        cmd.Parameters.AddWithValue("@p_FileID", FileId ?? (object)DBNull.Value); // Input parameter
                        cmd.Parameters.AddWithValue("@p_Username", UserName ?? (object)DBNull.Value); // Input parameter
                        cmd.Parameters.AddWithValue("@p_RRN", RRN ?? (object)DBNull.Value); // Input parameter

                        // Open the connection and execute the command
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        Status = Convert.ToString(outStatusParam.Value);
                        StatusMsg = Convert.ToString(outStatusMsgParam.Value);
                    }
                }
            }
            catch (Exception Ex)
            {
                // Log your error here
                ErrorLog.AgentManagementTrace("Class : ImportEntity.cs \nFunction : InsertBulk \nException Occurred\n" + Ex.Message);
            }

            _StatusMsg = StatusMsg;
            return Status;
        }


        public string InsertManuaBulkImportDetailsAgent(out string fileId)
        {
            try
            {
                fileId = string.Empty;
                string status = string.Empty;

                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Prepare the CALL statement
                        cmd.CommandText = "CALL SP_UploadBulkAgent_Import(null, null, @p_FileName, @p_FilePath, @p_FileType, @p_FileDateTime, @p_ClientId, @p_FileDescID, @p_FileDesc, @p_Mode, @p_CreateBy)";
                        cmd.CommandType = CommandType.Text;

                        // OUT parameters
                        var outStatusParam = new NpgsqlParameter("@p_Status", NpgsqlTypes.NpgsqlDbType.Varchar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var outStatusOutParam = new NpgsqlParameter("@p_Status_Out", NpgsqlTypes.NpgsqlDbType.Varchar) // Use Integer for the output ID
                        {
                            Direction = ParameterDirection.Output
                        };

                        // Add parameters corresponding to the procedure
                        cmd.Parameters.Add(outStatusParam);
                        cmd.Parameters.Add(outStatusOutParam);
                        cmd.Parameters.AddWithValue("@p_FileName", filename ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FilePath", FilePath ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileType", FileType ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileDateTime", (object)DBNull.Value); // Use DBNull for null value
                        cmd.Parameters.AddWithValue("@p_ClientId", ClientID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_FileDescID", FileDescID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_FileDesc", FileDescName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Mode", Mode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_CreateBy", UserName ?? (object)DBNull.Value);

                        // Execute the command
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();

                        // Retrieve OUT parameters
                        status = Convert.ToString(outStatusParam.Value);
                        fileId = Convert.ToString(outStatusOutParam.Value); // Cast to string for output
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                // Log your error here
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: InsertManualBulkImportDetailsAgent: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }




        public DataSet Get_AgentBulkUpload()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Create a command to call the stored procedure
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL SP_BulkUploadAgReg_Get(@p_FromDate, @p_ToDate, @p_ClientID, @p_UserName, @p_Status, @p_Flag, @p_FileID, @p_BCstatus, @p_Makerstatus, @p_Checkstatus, @p_Authstatus)";
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        cmd.Parameters.AddWithValue("@p_FromDate", (object)FromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ToDate", (object)ToDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ClientID", (object)ClientID ?? DBNull.Value); // Ensure ClientID is defined
                        cmd.Parameters.AddWithValue("@p_UserName", (object)UserName ?? DBNull.Value); // Ensure UserName is defined
                        cmd.Parameters.AddWithValue("@p_Status", DBNull.Value); // Assuming Status is not used
                        cmd.Parameters.AddWithValue("@p_Flag", Flag);
                        cmd.Parameters.AddWithValue("@p_FileID", (object)FileID ?? DBNull.Value); // Ensure FileID is defined
                        cmd.Parameters.AddWithValue("@p_BCstatus", (object)BCstatus ?? DBNull.Value); // Ensure BCstatus is defined
                        cmd.Parameters.AddWithValue("@p_Makerstatus", (object)Mstatus ?? DBNull.Value); // Ensure Mstatus is defined
                        cmd.Parameters.AddWithValue("@p_Checkstatus", (object)ChStatus ?? DBNull.Value); // Ensure ChStatus is defined
                        cmd.Parameters.AddWithValue("@p_Authstatus", (object)AtStatus ?? DBNull.Value); // Ensure AtStatus is defined


                        // Execute the stored procedure
                        cmd.ExecuteNonQuery();

                        // Now select from the appropriate temp table based on the flag
                        string tempTableName = string.Empty;

                        if (Flag == 1)
                        {
                            tempTableName = "temp_results"; // Replace with actual temp table name
                        }
                        
                        else if (Flag == 3)
                        {
                            tempTableName = "temp_results"; // Replace with actual temp table name
                        }

                        // If the temp table name is set, query it
                        if (!string.IsNullOrEmpty(tempTableName))
                        {
                            cmd.CommandText = $"SELECT * FROM {tempTableName}"; // Select from the appropriate temp table
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                dataAdapter.Fill(dataSet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class: ImportEntity.cs \nFunction: Get_AgentBulkUpload() \nException Occurred\n" + ex.Message);
            }
            return dataSet;
        }


    }
}