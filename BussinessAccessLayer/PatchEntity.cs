using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessAccessLayer
{
    public class PatchEntity
    {
        #region Objects Declaration
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string OutStatus = null; string OutStatusMsg = null;
        #endregion

        #region Property Declaration
        public string UserName { get; set; }
        public string PatchType { get; set; }
        public string PatchPath { get; set; }
        public string Version { get; set; }
        public string ReleaseNotePath { get; set; }
        public string ReleaseNoteFileName { get; set; }

        public string DocPath { get; set; }
        public string DocName { get; set; }
        public string status { get; set; }
        public string ReleasedOn { get; set; }
        public string scheduledatetime { get; set; }
        public string schedulepatchtype { get; set; }
        public string cugservices { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Flag { get; set; }
        public string Id { get; set; }
        public string ReqId { get; set; }
        public string Remarks { get; set; }

        public string Status { get; set; }
        #endregion

        public string InsertOrUpdatePatchDetails()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT public.insert_patch_details(@p_username, @p_patchtype, @p_patchpath, @p_version, @p_releasenotepath, @p_releasedon, @p_flag, @p_releasenotefilename)", conn))
                    {
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for SELECT

                        // Add input parameters
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_patchtype", (object)PatchType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_patchpath", (object)PatchPath ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_version", (object)Version ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_releasenotepath", (object)ReleaseNotePath ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_releasedon", (object)ReleasedOn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_releasenotefilename", (object)ReleaseNoteFileName ?? DBNull.Value);

                        // Execute the function and retrieve the status code
                        string statusCode = (string)cmd.ExecuteScalar();
                        return statusCode; // Return the status code directly
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdatePatchDetails() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public string UpdatecugprodDocumnetsDetails()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT public.update_cugprod_docs(@p_username, @p_status, @p_docpath, @p_docname, @p_id, @p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for SELECT

                        // Add input parameters
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_status", (object)Status ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_docpath", (object)DocPath ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_docname", (object)DocName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_id", (object)Id ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        

                        // Execute the function and retrieve the status code
                        string statusCode = (string)cmd.ExecuteScalar();
                        return statusCode; // Return the status code directly
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdatePatchDetails() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public string InsertOrUpdateVersioningDetails()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT public.insert_apipatch_details(@p_username, @p_reqid, @p_patchtype, @p_cugservices, @p_scheduledatetime, @p_remarks, @p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for SELECT

                        // Add input parameters
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_reqid", (object)ReqId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_patchtype", (object)schedulepatchtype ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_cugservices", (object)cugservices ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_scheduledatetime", (object)scheduledatetime ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_remarks", (object)Remarks ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);

                        // Execute the function and retrieve the status code
                        string statusCode = (string)cmd.ExecuteScalar();
                        return statusCode; // Return the status code directly
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdatePatchDetails() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        #region ExportRestrictedPIN
        public DataSet GetPatchDetails()
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
                        cmd.CommandText = "CALL sp_getversioning(@p_FromDate, @p_ToDate, @p_Username, @p_Flag)";
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        // Add parameters with p_ prefix
                        cmd.Parameters.AddWithValue("@p_FromDate", (object)FromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ToDate", (object)ToDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Flag", Flag);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Now select from the temp table based on the flag
                        string tempTableName = string.Empty;

                        if (Flag == 1)
                        {
                            tempTableName = "TempBindResults";
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
        #region GetVerificationDetails
        public DataSet GetVerificationDetails()
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
                        cmd.CommandText = "CALL sp_patchverification(@p_FromDate, @p_ToDate, @p_Username, @p_Flag)";
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        // Add parameters with p_ prefix
                        cmd.Parameters.AddWithValue("@p_FromDate", (object)FromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ToDate", (object)ToDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Flag", Flag);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Now select from the temp table based on the flag
                        string tempTableName = string.Empty;

                        if (Flag == 1)
                        {
                            tempTableName = "TempBindResults";
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
        #region ExportRestrictedPIN
        public DataSet GetScheduleDateTime()
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
                        cmd.CommandText = "CALL sp_getscheduledatetime(@p_ReqId, @p_Flag)";
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        // Add parameters with p_ prefix
                        cmd.Parameters.AddWithValue("@p_ReqId", (object)ReqId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Flag", Flag);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Now select from the temp table based on the flag
                        string tempTableName = string.Empty;

                        if (Flag == 1)
                        {
                            tempTableName = "TempBindResults";
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
        public DataTable BindCugServices()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("public.fn_bindcugservice", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Note: Use `DataTable` instead of `DataSet` to hold the result directly.
                        var dataTable = new DataTable();
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        dataAdapter.Fill(dataTable);

                        return dataTable; // Return the DataTable containing the results
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : RuleEntity.cs \nFunction : BindSwitch() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }
        #region ChangeAgentStatus
        public DataSet ChangePatchStatus()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Step 1: Call the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL public.sp_updatepatchstatus(@p_username, @p_id, @p_remarks, @p_flag)", sqlConn))
                    {
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_id", (object)Id ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_remarks", Remarks);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }

                    // Step 2: Select from the temporary table
                    using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_affected_rows", sqlConn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            DataSet ds = new DataSet();
                            dataAdapter.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentStatus: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }
        #endregion
    }
}
