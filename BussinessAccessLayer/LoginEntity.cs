using System;
using AppLogger;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using NpgsqlTypes;

namespace BussinessAccessLayer
{
    public class LoginEntity
    {
        #region Property Declaration
        //***********Login
        public string UserName { get; set; }
        public string Password { get; set; }
        public string BCAgentID { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }

        //***********Master page 
        public string Mode { get; set; }
        public int ID { get; set; }
        public int RoleID { get; set; }
        public string CurrentPage { get; set; }

        //**********Change Password
        public string OldPassword { get; set; }
        public string MobileNo { get; set; }
        public string Name { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        //
        public string OTP { get; set; }
        public string IsLoginOn { get; set; }
        public string SessionID { get; set; }
        public string IsValidLogin { get; set; }
        //Added while forget password
        public string _Status = string.Empty;
        public string DeviceID { get; set; }
        public string RRN { get; set; }
        public string OTPType { get; set; }
        public string MailID { get; set; }
        public string GeoDetails { get; set; }


        #region "ChangePassword"
        public int UsersID { get; set; }
        public string oldPassword { get; set; }
        public string CreatedBy { get; set; }
        public string Flag { get; set; }
        public int ParentRoleID { get; set; }
        #endregion;

        #endregion;

        #region Objects Declaration
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        DataSet dataSet = null;
        string OutStatus = null; string OutStatusMsg = null;
        #endregion

        #region TrackUsers
        public void TrackUsers(out string _OutStatus, out string _OutStatusMsg)
        {
            _OutStatus = string.Empty;
            _OutStatusMsg = string.Empty;

            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    using (var command = new NpgsqlCommand("sp_trackusers_insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Define parameters
                        command.Parameters.AddWithValue("p_userid", NpgsqlTypes.NpgsqlDbType.Varchar, UserName);
                        command.Parameters.AddWithValue("p_geodetails", NpgsqlTypes.NpgsqlDbType.Text, GeoDetails);

                        // Open connection
                        connection.Open();

                        // Execute command and read result
                        using (var reader = command.ExecuteReader())
                        {
                            //if (reader.Read())
                            //{
                            //    // Retrieve values from the result set
                            //    _OutStatus = reader["outstatus"] != DBNull.Value ? reader["outstatus"].ToString() : string.Empty;
                            //    _OutStatusMsg = reader["outstatusmsg"] != DBNull.Value ? reader["outstatusmsg"].ToString() : string.Empty;
                            //}
                            if (reader.Read())
                            {
                                // Retrieve values from the result set
                                _OutStatus = reader["outstatus"] != DBNull.Value ? reader["outstatus"].ToString() : string.Empty;
                                _OutStatusMsg = reader["outstatusmsg"] != DBNull.Value ? reader["outstatusmsg"].ToString() : string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ErrorLog.AgentManagementTrace("LoginEntity: TrackUsers: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        #endregion

        #region GetUsernameByMobileNo
        public void GetUsernameByMobileNo(out string _OutStatus, out string _OutStatusMsg)
        {
            OutStatus = string.Empty;
            OutStatusMsg = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@MobileNo", MobileNo),
                            new SqlParameter("@OutStatus", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output },
                            new SqlParameter("@OutStatusMsg", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "PROC_GetUsernameBymobile_Login";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        OutStatus = Convert.ToString(cmd.Parameters["@OutStatus"].Value);
                        OutStatusMsg = Convert.ToString(cmd.Parameters["@OutStatusMsg"].Value);
                        sqlConn.Close();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("LoginEntity: GetUsernameByMobileNo: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
            _OutStatus = this.OutStatus;
            _OutStatusMsg = this.OutStatusMsg;
        }
        #endregion

        #region Login
        //public DataSet Login()
        //{
        //    try
        //    {
        //        using (var connection = new NpgsqlConnection(ConnectionString))
        //        {
        //            using (var command = new NpgsqlCommand("proc_userlogin", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("p_username", UserName ?? (object)DBNull.Value);
        //                command.Parameters.AddWithValue("p_password", Password ?? (object)DBNull.Value);
        //                var dataSet = new DataSet();
        //                using (var adapter = new NpgsqlDataAdapter(command))
        //                {
        //                    adapter.Fill(dataSet);
        //                }
        //                return dataSet;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.CommonTrace($"Class : LoginEntity.cs \nFunction : Login() \nException Occurred\n{ex.Message}");
        //        ErrorLog.DBError(ex);
        //        throw;
        //    }
        //}


        public DataSet Login()
        {
            DataSet dataSet = new DataSet();

            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Call the procedure
                    using (var command = new NpgsqlCommand("CALL proc_userlogin(@p_username, @p_password)", connection))
                    {
                        command.Parameters.AddWithValue("p_username", UserName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("p_password", Password ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }

                    // Fetch data from the temporary table
                    using (var fetchCommand = new NpgsqlCommand("SELECT * FROM temp_userlogin_results", connection))
                    {
                        using (var adapter = new NpgsqlDataAdapter(fetchCommand))
                        {
                            adapter.Fill(dataSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Log the error or rethrow it as needed
                throw;
            }

            return dataSet;
        }





        #endregion

        #region SetChangePassword
        public DataSet SetChangePassword()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@BCAgentId", BCAgentID),
                            new SqlParameter("@ConfirmPassword", ConfirmPassword),
                            new SqlParameter("@OldPassword", oldPassword),
                            new SqlParameter("@UserID", UsersID),
                            new SqlParameter("@CreatedBy", CreatedBy)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "PROC_UpdateChangePassword";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : SetChangePassword() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetLoginAttemptCount
        //public DataSet GetLoginAttemptCount()
        //{
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
        //            {
        //                SqlParameter[] _Params =
        //                {
        //                     new SqlParameter("@UserName",UserName)
        //                };
        //                cmd.Connection = sqlConn;
        //                cmd.CommandText = "PROC_GetLoginAttemptCount";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddRange(_Params);
        //                DataSet dataSet = new DataSet();
        //                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //                dataAdapter.Fill(dataSet);
        //                cmd.Dispose();
        //                return dataSet;
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : GetLoginAttemptCount() \nException Occured\n" + Ex.Message);
        //        ErrorLog.DBError(Ex);
        //        throw;
        //    }
        //}
        //public DataSet GetLoginAttemptCount()
        //{
        //    try
        //    {
        //        DataSet dataSet = new DataSet();
        //        using (var connection = new NpgsqlConnection(ConnectionString))
        //        {
        //            using (var command = new NpgsqlCommand("PROC_GetLoginAttemptCount", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("p_username", NpgsqlTypes.NpgsqlDbType.Varchar, UserName);
        //                connection.Open();
        //                using (var adapter = new NpgsqlDataAdapter(command))
        //                {
        //                    adapter.Fill(dataSet);
        //                }
        //            }
        //        }
        //        return dataSet;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : GetLoginAttemptCount() \nException Occurred\n" + ex.Message);
        //        ErrorLog.DBError(ex);
        //        throw;
        //    }
        //}
        public DataSet GetLoginAttemptCount()
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    using (var command = new NpgsqlCommand("proc_get_login_attempt_count", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_username", NpgsqlTypes.NpgsqlDbType.Text, UserName);
                        //command.Parameters.AddWithValue("p_password", NpgsqlTypes.NpgsqlDbType.Text, Password);
                        connection.Open();
                        using (var adapter = new NpgsqlDataAdapter(command))
                        {
                            adapter.Fill(dataSet);
                        }
                    }
                }
                return dataSet;
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : GetLoginAttemptCount() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }
        #endregion

        #region SetLoginAttemptCount
        //public DataSet SetLoginAttemptCount()
        //{
        //    try
        //    {
        //    using (NpgsqlCommand cmd = new NpgsqlCommand())
        //    {
        //        using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
        //        {
        //            NpgsqlParameter[] _Params =
        //            {
        //            new NpgsqlParameter("p_username", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = UserName },
        //            new NpgsqlParameter("p_flag", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = Flag }
        //        };

        //            cmd.Connection = sqlConn;
        //            cmd.CommandText = "PROC_SetLoginAttemptCount"; // Name of the PostgreSQL procedure
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddRange(_Params);

        //            DataSet dataSet = new DataSet();
        //            using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
        //            {
        //                dataAdapter.Fill(dataSet);
        //            }
        //            return dataSet;
        //        }
        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : SetLoginAttemptCount() \nException Occurred\n" + ex.Message);
        //        ErrorLog.DBError(ex);
        //        throw;
        //    }
        //}
        public void SetLoginAttemptCount()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("SELECT public.proc_set_login_attempt_count(@p_username, @p_flag)", conn))
                    {
                        cmd.Parameters.AddWithValue("@p_username", UserName);
                        cmd.Parameters.AddWithValue("@p_flag", Flag);

                        conn.Open();
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery for functions that return void
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Exception Occurred: {ex.Message}");
                throw;
            }
        }


        #endregion

        #region UserIsExistForForgotPassword
        public string UserIsExistForForgotPassword()
        {
            string _Status = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                             new SqlParameter("@USERNAME",UserName),
                             new SqlParameter("@Flag",Flag),
                             new SqlParameter("@STATUS", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_UserIsExistOrNot_ForgotPassword";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        _Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        cmd.Dispose();
                        return _Status;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : UserIsExistForForgotPassword() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Insert OTP into Table
        public string InsertOTPIntoTable()
        {
            string _Status = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                             new SqlParameter("@USERID",UserName),
                             new SqlParameter("@MobileNumber", MobileNo),
                             new SqlParameter("@DEVICEID",DeviceID ),
                             new SqlParameter("@REFRENCENUMBER", RRN),
                             new SqlParameter("@OTP", OTP),
                             new SqlParameter("@RoleId", RoleID),
                             new SqlParameter("@OTPTYPE", OTPType),
                             new SqlParameter("@MailID", MailID),
                             new SqlParameter("@Flag",Flag),
                             new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_InsertUpdateOTPForgetPassword";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        _Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        cmd.Dispose();
                        return _Status;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : InsertOTPIntoTable() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetUserDetails
        public DataSet GetUserDetails()
        {
            string _Status = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                           new SqlParameter("@USERNAME",UserName),
                           new SqlParameter("@Flag",Flag),
                           new SqlParameter("@STATUS", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_UserIsExistOrNot_ForgotPassword";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        _Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        cmd.Dispose();
                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : GetUserDetails() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Change Forgot Password Window
        public DataSet SetChangePasswordForForget()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@ConfirmPassword", ConfirmPassword),
                            new SqlParameter("@OldPassword", oldPassword),
                            new SqlParameter("@UserID", UsersID),
                            new SqlParameter("@CreatedBy", CreatedBy)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "PROC_UpdateChangePasswordForForget";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : SetChangePasswordForForget() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region getMenuList
        //public DataSet getMenuListData()
        //{
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
        //            {
        //                SqlParameter[] _Params =
        //                {
        //                    new SqlParameter("@intRoleID", RoleID),
        //                    new SqlParameter("@UserName",UserName),
        //                    new SqlParameter("@PARENTROLEID", ParentRoleID)
        //                };
        //                cmd.Connection = sqlConn;
        //                cmd.CommandText = "Proc_Select_MENU";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddRange(_Params);
        //                DataSet dataSet = new DataSet();
        //                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //                dataAdapter.Fill(dataSet);
        //                cmd.Dispose();
        //                return dataSet;
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : getMenuListData() \nException Occured\n" + Ex.Message);
        //        ErrorLog.DBError(Ex);
        //        throw;
        //    }
        //}
        public DataSet GetMenuListData()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("public.proc_select_menu", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("p_int_role_id", RoleID);
                        cmd.Parameters.AddWithValue("p_parent_role_id", ParentRoleID);
                        cmd.Parameters.AddWithValue("p_user_name", UserName); 

                        // Create a data adapter
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataSet = new DataSet();

                        // Fill the DataSet
                        dataAdapter.Fill(dataSet);

                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : GetMenuListData() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        #endregion
        ////////////////////////

        #region Store Login Activities
        public void StoreLoginActivities(string[] _Params)
        {
            try
            {
                using (var cmd = new NpgsqlCommand())
                {
                    using (var sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        var _sqlParameter = new[]
                        {
                            new NpgsqlParameter("p_username", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = _Params[0] },
                            new NpgsqlParameter("p_action", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = _Params[1] },
                            new NpgsqlParameter("p_description", NpgsqlTypes.NpgsqlDbType.Text) { Value = _Params[2] }
                        };

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "InsertAuditLoginActivities";
                        cmd.Connection = sqlConn;

                        // Ensure the connection is closed before opening it
                        if (cmd.Connection.State == System.Data.ConnectionState.Open)
                            cmd.Connection.Close();

                        cmd.Connection.Open();
                        cmd.Parameters.AddRange(_sqlParameter);
                        cmd.ExecuteNonQuery();
                        sqlConn.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : StoreLoginActivities() \nException Occurred\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        #endregion

        #region Store Login Attempted Logs
        public void updateAttemptFailed(string[] _Params)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _sqlParameter =
                        {
                                                 new SqlParameter("@Username",_Params[0]),
                                                 new SqlParameter("@AttemptAction",_Params[1]),
                                               };
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Proc_updateAttemptFailed";
                        cmd.Connection = sqlConn;
                        if (cmd.Connection.State == ConnectionState.Open)
                            cmd.Connection.Close();
                        cmd.Connection.Open();
                        cmd.Parameters.AddRange(_sqlParameter);
                        cmd.ExecuteNonQuery();
                        sqlConn.Close();
                    }
                }
            }

            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : LoginEntity.cs \nFunction : updateAttemptFailed() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

    }
}
