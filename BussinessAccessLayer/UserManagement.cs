using System;
using System.Data;
using System.Data.SqlClient;
using AppLogger;
using System.Configuration;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using System.Linq;
using Newtonsoft.Json;

namespace BussinessAccessLayer
{
    public class UserManagement
    {
        #region Object Declaration
        DataSet dataSet = null;
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public DataTable dataTable = new DataTable();
        #endregion

        #region Property Declaration
        public bool? _bitIsRemove = false;
        public Nullable<bool> _Access = null;
        public EnumCollection.EnumMenuType _EnumMenuType;

        public string ClientID { get; set; }
        public string BCID { get; set; }
        public string AgentID { get; set; }
        public string MenuIcon { get; set; }
        public int _ActionTypeID { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public int Flag { get; set; }
        public string Status { get; set; }
        public string _Role { get; set; }
        public string _ClientID { get; set; }
        public string _UserID { get; set; }
        public string _UserId { get; set; }
        public string _Email { get; set; }
        public string _ID { get; set; }
        public string _RoleId { get; set; }
        public string _MenuID { get; set; }
        public string _UserName { get; set; }
        public string _MenuName { get; set; }
        public string _MenuLink { get; set; }
        public string _SubMenuID { get; set; }
        public string _SubMenuLink { get; set; }
        public string _SubMenuName { get; set; }
        public string _Mobile { get; set; }
        public string _Password { get; set; }
        public string Email { get; set; }
        public string UserRoleGroup { get; set; }
        public string _RandomStringForSalt { get; set; }
        public int _ActionType { get; set; }
        public string _ParentID { get; set; }
        public string Reason { get; set; }
        public string __RandomStringForSalt { get; set; }
        #endregion

        #region Fill GridViews
        public DataSet FillGridUserPageManagement()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Create a command to call the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL SP_GetPagePermissions(@p_ID, @p_RoleId, @p_MenuID, @p_MenuName, @p_MenuLink, @p_SubMenuID, @p_SubMenName, @p_SubMenuLink, @p_bitIsRemove, @p_UserName, @p_Flag, @p_ClientId)", sqlConn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("p_ID", (object)_ID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_RoleId", (object)_RoleId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_MenuID", (object)_MenuID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_MenuName", (object)_MenuName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_MenuLink", (object)_MenuLink ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_SubMenuID", (object)_SubMenuID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_SubMenName", (object)_SubMenuName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_SubMenuLink", (object)_SubMenuLink ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bitIsRemove", (object)_Access ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_UserName", (object)_UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", (int)EnumCollection.EnumBindingType.BindGrid);
                        cmd.Parameters.AddWithValue("p_ClientId", (object)_ClientID ?? DBNull.Value);

                        // Execute the command
                        cmd.ExecuteNonQuery();
                    }

                    // Now, select from the temporary table
                    DataSet ds = new DataSet();

                    // Select results from the temporary table
                    using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_page_permissions", sqlConn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            dataAdapter.Fill(ds);
                        }
                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserPageManagement(): UserName: " + _UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }


        public DataSet FillGridAddRole()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                             new SqlParameter("@RoleName", _RoleId),
                             new SqlParameter("@Flag",Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_RoleDetailsCreate";
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
                ErrorLog.AgentManagementTrace("UserManagement: FillGridAddRole() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet FillGridUserAccounts()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        NpgsqlParameter[] _Params =
                        {
                            new NpgsqlParameter("p_RoleID", _RoleId ?? (object)DBNull.Value),
                            new NpgsqlParameter("p_UserID", _UserID ?? (object)DBNull.Value),
                            new NpgsqlParameter("p_UserName", UserName ?? (object)DBNull.Value),
                            new NpgsqlParameter("p_Status", Status ?? (object)DBNull.Value),
                            new NpgsqlParameter("p_Flag", Flag),
                            new NpgsqlParameter("p_ClientId", _ClientID ?? (object)DBNull.Value)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL SP_GetManageUsers(:p_RoleID, :p_UserID, :p_UserName, :p_Status, :p_Flag, :p_ClientId)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT * FROM temp_manage_users";
                        cmd.CommandType = CommandType.Text;
                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccounts() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }



        public DataSet FillGridUserAccessManagement()
        {
            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            //        {
            //            SqlParameter[] _Params =
            //            {
            //              new SqlParameter("@ID", _ID),
            //              new SqlParameter("@RoleName", _RoleId),
            //              new SqlParameter("@User",_UserName),
            //              new SqlParameter("@Flag", Flag),
            //            };
            //            cmd.Connection = sqlConn;
            //            cmd.CommandText = "Proc_RoleDetails";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.AddRange(_Params);
            //            DataSet dataSet = new DataSet();
            //            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            //            dataAdapter.Fill(dataSet);
            //            cmd.Dispose();
            //            return dataSet;
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccessManagement() : UserName: " + UserName + " Exception: " + Ex.Message);
            //    ErrorLog.DBError(Ex);
            //    throw;
            //}
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL Proc_RoleDetails(@p_Flag, @p_ID, @p_RoleName, @p_User, @p_Reason)";
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.Parameters.AddWithValue("p_ID", (object)_ID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_RoleName", (object)_RoleId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_User", (object)_UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Reason", (object)Reason ?? DBNull.Value);
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT * FROM temp_role_details"; // Query the temp table
                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: UserAccessManagementDeleteRole() : UserName: " + _UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataSet FillGridCreateRoles()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;
                        NpgsqlParameter[] _Params =
                        {
                            new NpgsqlParameter("p_ID", (object)_ID ?? DBNull.Value),
                            new NpgsqlParameter("p_RoleName", (object)_RoleId ?? DBNull.Value),
                            new NpgsqlParameter("p_User", (object)_UserName ?? DBNull.Value),
                            new NpgsqlParameter("p_Flag", (object)Flag ?? DBNull.Value)
                        };
                        cmd.CommandText = "CALL Proc_RoleDetailsCreate(:p_ID, :p_RoleName, :p_User, :p_Flag)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT * FROM temp_role_details";
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }
                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: FillGridCreateRoles() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        public DataSet FillGridUserAccessManagementForAddRoleValidateRole()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Prepare parameters for the procedure
                        cmd.CommandText = "CALL Proc_ValidateRoleId(:p_Role)";
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("p_Role", (object)_Role ?? DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Call the procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_role_validation"; // Query the temp table
                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet; // Return the dataset with results
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccessManagementForAddRoleValidateRole() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        public DataSet FillGridUserAccessManagementForAddRoleInsertRole()
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
                             new SqlParameter("@Role", _Role),
                             new SqlParameter("@DataTable", dataTable),
                             new SqlParameter("@RoleId", _RoleId)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_CreateRole";
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
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccessManagementForAddRoleInsertRole() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }



        public DataSet UserAccessManagementDeleteRole()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Use CALL statement to execute the stored procedure
                        cmd.CommandText = "CALL Proc_RoleDetails(@p_Flag, @p_ID, @p_RoleName, @p_User, @p_Reason)";

                        // Define the parameters
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.Parameters.AddWithValue("p_ID", _ID);
                        cmd.Parameters.AddWithValue("p_RoleName", _RoleId);
                        cmd.Parameters.AddWithValue("p_User", _UserName);
                        cmd.Parameters.AddWithValue("p_Reason", Reason);

                        // Open connection
                        sqlConn.Open();

                        // Execute the stored procedure
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT * FROM temp_messages"; // Query the temp table
                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }
                        // Now select from the temporary table
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: UserAccessManagementDeleteRole() : UserName: " + _UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }


        public DataSet FillGridUserAccessManagementModalPopupMenu()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (var cmd = new NpgsqlCommand())
                    {
                        // Create the parameters array
                        NpgsqlParameter[] _Params =
                        {
                            new NpgsqlParameter("p_RoleID", (object)_RoleId ?? DBNull.Value),
                            new NpgsqlParameter("p_UserName", (object)_UserName ?? DBNull.Value),
                            new NpgsqlParameter("p_Flag", (object)Flag ?? DBNull.Value),
                            new NpgsqlParameter("p_ClientID", (object)_ClientID ?? DBNull.Value)
                        };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL SP_SelectEditMenu(:p_RoleID, :p_UserName, :p_Flag, :p_ClientID)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since we're just calling a procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_MenuResults"; // Replace with your actual temp table name
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccessManagementModalPopupMenu() : UserName: " + _UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }

        }

        public DataSet FillMenuSubmenuGrid()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@RoleID", _RoleId),
                                            new SqlParameter("@UserName",_UserName),
                                            new SqlParameter("@ParentRoleId",_ParentID),
                                            new SqlParameter("@ClientID",_ClientID),
                                            new SqlParameter("@Flag", Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_SelectMenu";
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
                ErrorLog.AgentManagementTrace("UserManagement: FillMenuSubmenuGrid() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet BindUserBasedRole()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        // Create the parameters array
                        NpgsqlParameter[] _Params =
                        {
                    new NpgsqlParameter("p_RoleID", (object)_RoleId ?? DBNull.Value),
                    new NpgsqlParameter("p_UserName", (object)_UserName ?? DBNull.Value),
                    new NpgsqlParameter("p_Flag", (object)Flag ?? DBNull.Value),
                    new NpgsqlParameter("p_ParentRoleId", (object)_ParentID ?? DBNull.Value),
                    new NpgsqlParameter("p_ClientID", (object)_ClientID ?? DBNull.Value)
                };

                        cmd.Connection = sqlConn;

                        // Call the procedure to create the temporary table
                        cmd.CommandText = "CALL SP_BindRoleGroups_CreateUser(:p_RoleID, :p_UserName, :p_Flag, :p_ParentRoleId, :p_ClientID)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since we're just calling a procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_RoleGroups"; // Select from the temporary table
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: BindUserBasedRole() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        public DataSet FillGridUserAccessManagementModalPopupSubMenu()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (var cmd = new NpgsqlCommand())
                    {
                        // Prepare the call statement with all parameters
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL SP_SelectEditSubMenu(:p_RoleID, :p_MenuId, :p_ParentRoleID, :p_ClientID)";
                        cmd.CommandType = CommandType.Text;

                        // Add parameters
                        cmd.Parameters.AddWithValue("p_RoleID", _RoleId);
                        cmd.Parameters.AddWithValue("p_MenuId", _MenuID);
                        cmd.Parameters.AddWithValue("p_ParentRoleID", _ParentID);
                        cmd.Parameters.AddWithValue("p_ClientID", (object)_ClientID ?? DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // This executes the procedure

                        // Now, we need to select from the temporary table
                        using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_SubMenuResults", sqlConn))
                        {
                            DataSet dataSet = new DataSet();
                            using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                            {
                                dataAdapter.Fill(dataSet); // Fill the dataset with results from the temp table
                            }
                            return dataSet; // Return the populated dataset
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccessManagementModalPopupSubMenu() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        // Edit Role - Sub Menu Fill Grid By Menu ID.
        public DataSet FillGridUserAccessManagementMenu()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        NpgsqlParameter[] _Params =
                        {
                            new NpgsqlParameter("p_RoleID", (object)_RoleId ?? DBNull.Value),
                            new NpgsqlParameter("p_MenuId", (object)_MenuID ?? DBNull.Value),
                            new NpgsqlParameter("p_ClientID", (object)_ClientID ?? DBNull.Value)
                        };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL SP_SelectSubMenu(:p_RoleID, :p_MenuId, :p_ClientID)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT * FROM temp_SubMenu";
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccessManagementMenu() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        //public DataSet UserAccessManagementUpdateTblRoleDetails()
        //{
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
        //            {
        //                SqlParameter[] _Params =
        //                {
        //                                    new SqlParameter("@DataTable", dataTable),
        //                                    new SqlParameter("@RoleId", _RoleId),
        //                                    new SqlParameter("@ID", _ID),
        //                                    new SqlParameter("@RoleName", _Role),
        //                                    new SqlParameter("@User", UserName),
        //                                    new SqlParameter("@Flag", Flag),
        //                                    new SqlParameter("@UserRoleId",_UserID),
        //                                    new SqlParameter("@ClientId",_ClientID)
        //                };
        //                cmd.Connection = sqlConn;
        //                cmd.CommandText = "SP_UpdateTblRoleDetails";
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
        //        ErrorLog.AgentManagementTrace("UserManagement: UserAccessManagementUpdateTblRoleDetails() : UserName: " + UserName + " Exception: " + Ex.Message);
        //        ErrorLog.DBError(Ex);
        //        throw;
        //    }
        //}
        public DataSet UserAccessManagementUpdateTblRoleDetails()
        {
            try
            {
                // Convert DataTable to JSON
                string jsonData = DataTableToJson(dataTable);

                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        // Setting up the command for the stored procedure
                        cmd.Connection = sqlConn;

                        // Prepare the CALL statement
                        cmd.CommandText = "CALL public.sp_updatetblroledetails(:p_datatable, :p_roleid, :p_id, :p_rolename, :p_user, :p_flag, :p_userroleid, :p_clientid)";
                        cmd.CommandType = CommandType.Text;

                        // Adding parameters
                        cmd.Parameters.AddWithValue("p_datatable", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonData);
                        cmd.Parameters.AddWithValue("p_roleid", (object)_RoleId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_id", (object)_ID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_rolename", (object)_Role ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_user", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_userroleid", (object)_UserID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_clientid", (object)_ClientID ?? DBNull.Value);

                        // Execute the CALL statement
                        cmd.ExecuteNonQuery();

                        // Now select from the temporary table
                        cmd.CommandText = "SELECT * FROM TempResults";
                        cmd.CommandType = CommandType.Text;

                        // Fill the DataSet
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
                // Log the error
                ErrorLog.AgentManagementTrace($"UserManagement: UserAccessManagementUpdateTblRoleDetails() : UserName: {UserName} Exception: {ex.Message}");
                ErrorLog.DBError(ex);
                throw; // Rethrow the exception
            }
        }



        private string DataTableToJson(DataTable table)
        {
            // Use Newtonsoft.Json or any preferred method to convert DataTable to JSON
            return JsonConvert.SerializeObject(table);
        }


        public DataSet InsertUpdateRoleDetails()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@DataTable", dataTable),
                                            new SqlParameter("@RoleId", _RoleId),
                                            new SqlParameter("@ID", _ID),
                                            new SqlParameter("@RoleName", _Role),
                                            new SqlParameter("@User", UserName),
                                            new SqlParameter("@Flag", Flag),
                                            new SqlParameter("@UserRoleId",_ParentID),
                                            new SqlParameter("@ClientId",_ClientID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_InsertUpdateSBMTblRoleDetails";
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
                ErrorLog.AgentManagementTrace("UserManagement: InsertUpdateRoleDetails() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet InsertNewRole()
        {
            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            //        {
            //            SqlParameter[] _Params =
            //            {
            //                                new SqlParameter("@RoleName", _Role),
            //                                new SqlParameter("@RoleId", _RoleId),
            //                                new SqlParameter("@User", UserName),
            //                                new SqlParameter("@Flag",Flag)
            //            };
            //            cmd.Connection = sqlConn;
            //            cmd.CommandText = "SP_UpdateTblRoleDetails";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.AddRange(_Params);
            //            DataSet dataSet = new DataSet();
            //            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            //            dataAdapter.Fill(dataSet);
            //            cmd.Dispose();
            //            return dataSet;
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    ErrorLog.AgentManagementTrace("UserManagement: InsertUpdateRoleDetails() : UserName: " + UserName + " Exception: " + Ex.Message);
            //    ErrorLog.DBError(Ex);
            //    throw;
            //}
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Create a command to call the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL Proc_UpdateTblRoleDetails(@p_DataTable, @p_RoleId, @p_ID, @p_RoleName, @p_User, @p_Flag, @p_UserRoleId, @p_ClientId)", sqlConn))
                    {
                        // Convert the list to an array of NpgsqlTypes.Composite
                        cmd.Parameters.AddWithValue("p_DataTable", DBNull.Value);
                        cmd.Parameters.AddWithValue("p_RoleId", _RoleId);
                        cmd.Parameters.AddWithValue("p_ID", 0); // Use appropriate value for p_ID
                        cmd.Parameters.AddWithValue("p_RoleName", _Role);
                        cmd.Parameters.AddWithValue("p_User", UserName);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.Parameters.AddWithValue("p_UserRoleId", DBNull.Value); // Use appropriate value if needed
                        cmd.Parameters.AddWithValue("p_ClientId", DBNull.Value); // Use appropriate value if needed

                        // Execute the command
                        cmd.ExecuteNonQuery();
                    }

                    // Now, select from the temporary table
                    using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_messages", sqlConn))
                    {
                        var dataSet = new DataSet();
                        using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: InsertNewRole() : UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        #endregion

        #region Fill Dropdowns
        public DataSet BindDropdowns()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Create a command to call the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL SP_BindMenuSubMenu(@p_UserName, @p_Flag)", sqlConn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("p_UserName", (object)_UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", (object)Flag ?? DBNull.Value);

                        // Execute the command
                        cmd.ExecuteNonQuery();
                    }

                    // Now, select from the temporary tables
                    DataSet dataSet = new DataSet();

                    // Select menu results
                    using (var menuCmd = new NpgsqlCommand("SELECT * FROM temp_menu_results", sqlConn))
                    {
                        using (var menuAdapter = new NpgsqlDataAdapter(menuCmd))
                        {
                            menuAdapter.Fill(dataSet, "MenuResults");
                        }
                    }

                    // Select submenu results
                    using (var submenuCmd = new NpgsqlCommand("SELECT * FROM temp_submenu_results", sqlConn))
                    {
                        using (var submenuAdapter = new NpgsqlDataAdapter(submenuCmd))
                        {
                            submenuAdapter.Fill(dataSet, "SubmenuResults");
                        }
                    }

                    // Select role results
                    using (var roleCmd = new NpgsqlCommand("SELECT * FROM temp_role_results", sqlConn))
                    {
                        using (var roleAdapter = new NpgsqlDataAdapter(roleCmd))
                        {
                            roleAdapter.Fill(dataSet, "RoleResults");
                        }
                    }

                    // Select specific role results
                    using (var specificRoleCmd = new NpgsqlCommand("SELECT * FROM temp_role_specific_results", sqlConn))
                    {
                        using (var specificRoleAdapter = new NpgsqlDataAdapter(specificRoleCmd))
                        {
                            specificRoleAdapter.Fill(dataSet, "SpecificRoleResults");
                        }
                    }

                    return dataSet;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: BindDropdowns() : UserName: " + _UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataSet BindSubMenu()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Create a command to call the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL sp_bindsubmenu(@p_UserName, @p_menuid, @p_Flag)", sqlConn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("p_UserName", (object)_UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_menuid", (object)_MenuID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", (object)Flag ?? DBNull.Value);

                        // Execute the command
                        cmd.ExecuteNonQuery();
                    }

                    DataSet dataSet = new DataSet();

                    using (var submenuCmd = new NpgsqlCommand("SELECT * FROM temp_submenu_results", sqlConn))
                    {
                        using (var submenuAdapter = new NpgsqlDataAdapter(submenuCmd))
                        {
                            submenuAdapter.Fill(dataSet, "SubmenuResults");
                        }
                    }

                    return dataSet;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: BindDropdowns() : UserName: " + _UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataSet BindDropdownUsers()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL SP_BindUsers(:p_RoleID, :p_UserID, :p_UserName, :p_Status, :p_Flag)";
                        cmd.Parameters.AddWithValue("p_RoleID", _RoleId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_UserID", _UserID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_UserName", UserName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Status", Status ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);

                        sqlConn.Open();

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Now, select from the temporary table
                        using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_users", sqlConn))
                        {
                            var dataSet = new DataSet();

                            using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                            {
                                dataAdapter.Fill(dataSet);
                            }

                            return dataSet;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: BindDropdownUsers() : UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }


        public DataSet BindRolesAndRoleGroup()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    using (var cmd = new NpgsqlCommand("CALL SP_BindDropdownRoles(:p_UserName, :p_RoleID, :p_Flag)", sqlConn))
                    {
                        cmd.Parameters.AddWithValue("p_UserName", UserName);
                        cmd.Parameters.AddWithValue("p_RoleID", _RoleId);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.ExecuteNonQuery();
                    }
                    using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_role_list; SELECT * FROM temp_role_group;", sqlConn))
                    {
                        var dataSet = new DataSet();
                        using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: BindRolesAndRoleGroup() : UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }



        public DataSet BindRoles_CreateUser()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Call the procedure to create the temporary table
                        cmd.CommandText = "CALL SP_BindRoles_CreateUser(:p_RoleID, :p_UserName, :p_Flag, :p_ParentRoleId, :p_ClientID)";
                        cmd.CommandType = CommandType.Text;

                        // Parameters with 'p_' prefix
                        cmd.Parameters.AddWithValue("p_RoleID", (object)_RoleId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_UserName", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", (object)Flag ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ParentRoleId", (object)_ParentID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ClientID", (object)_ClientID ?? DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since we're just calling a procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_roles"; // Select from the temporary table
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: BindRoles_CreateUser() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }



        #endregion

        #region Insert Update Page Permissions
        public DataSet InsertUpdatePagePermissions()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Call the stored procedure with parameters in the correct order
                        cmd.CommandText = "CALL SP_InsertUpdateMenuPermissions(:p_ID, :p_RoleId, :p_MenuID, :p_MenuName, :p_MenuLink, :p_SubMenuID, :p_SubMenName, :p_SubMenuLink, :p_bitIsRemove, :p_UserName, :p_Flag, :p_ClientID, :p_MenuIcon)";
                        cmd.CommandType = CommandType.Text;

                        // Parameters with 'p_' prefix in the order of the function definition
                        cmd.Parameters.AddWithValue("p_ID", (object)_ID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_RoleId", (object)_RoleId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_MenuID", (object)_MenuID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_MenuName", (object)_MenuName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_MenuLink", (object)_MenuLink ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_SubMenuID", (object)_SubMenuID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_SubMenName", (object)_SubMenuName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_SubMenuLink", (object)_SubMenuLink ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bitIsRemove", (object)_bitIsRemove ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_UserName", (object)_UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", (int)_EnumMenuType);
                        cmd.Parameters.AddWithValue("p_ClientID", (object)_ClientID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_MenuIcon", (object)MenuIcon ?? DBNull.Value); // MenuIcon added at the end

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since we're just calling a procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_row_count"; // Adjust as needed for your temp table
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet; // Return the dataset with results
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: InsertUpdatePagePermissions() : UserName: " + _UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }



        public DataSet CreateUser()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Call the procedure to create a user
                        cmd.CommandText = "CALL Proc_CreateUser(:p_username, :p_password, :p_email, :p_rollid, :p_UserRoleGroup, :p_createdBy, :p_UserId, :p_Salt, :p_Salt1, :p_Salt2, :p_Salt3, :p_Salt4, :p_ClientId, :p_Mobile)";
                        cmd.CommandType = CommandType.Text;

                        // Parameters with 'p_' prefix
                        cmd.Parameters.AddWithValue("p_username", (object)_UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_password", (object)_Password ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_email", (object)Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_rollid", (object)_RoleId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_UserRoleGroup", (object)UserRoleGroup ?? DBNull.Value); // Added UserRoleGroup
                        cmd.Parameters.AddWithValue("p_createdBy", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_UserId", (object)_UserId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Salt", (object)_RandomStringForSalt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Salt1", (object)_RandomStringForSalt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Salt2", DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Salt3", DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Salt4", DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ClientId", (object)_ClientID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Mobile", (object)_Mobile ?? DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since we're just calling a procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_notices"; // Select from the temporary table
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet; // Return the dataset with messages
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: CreateUser() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet ManageUserAccounts()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                           new SqlParameter("@UserID",_UserId ),
                                           new SqlParameter("@ActionType",_ActionType),
                                           new SqlParameter("@Reason", "Reset Password"),
                                           new SqlParameter("@Password", _Password),
                                           new SqlParameter("@Salt", _RandomStringForSalt),
                                           new SqlParameter("@UserName",UserName ),
                                           new SqlParameter("@Flag", 1),
                                           new SqlParameter("@Status", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_ManageUserAccounts";
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
                ErrorLog.AgentManagementTrace("UserManagement: ManageUserAccounts() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region CheckIs - User, Email, Mobile Exist
        public DataSet CheckIsUserExist()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        // Create the parameters array
                        NpgsqlParameter[] _Params =
                        {
                    new NpgsqlParameter("p_UserName", (object)_UserName ?? DBNull.Value)
                };

                        cmd.Connection = sqlConn;

                        // Call the procedure to create the temporary table
                        cmd.CommandText = "CALL SP_CheckIsUserExist(:p_UserName)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since we're just calling a procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_UserExist"; // Select from the temporary table
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: CheckIsUserExist() : UserName: " + _UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        public DataSet CheckIsEmailExist()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        // Create the parameters array
                        NpgsqlParameter[] _Params =
                        {
                    new NpgsqlParameter("p_UserEmailID", (object)_Email ?? DBNull.Value)
                };

                        cmd.Connection = sqlConn;

                        // Call the procedure to create the temporary table
                        cmd.CommandText = "CALL SP_CheckIsEmailExist(:p_UserEmailID)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since we're just calling a procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_EmailExist"; // Select from the temporary table
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: CheckIsEmailExist() : UserEmail: " + _Email + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        public DataSet CheckIsMobileExist()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        // Create the parameters array
                        NpgsqlParameter[] _Params =
                        {
                    new NpgsqlParameter("p_mobileNo", (object)_Mobile ?? DBNull.Value)
                };

                        cmd.Connection = sqlConn;

                        // Call the procedure to create the temporary table
                        cmd.CommandText = "CALL SP_CheckIsMobileExist(:p_mobileNo)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since we're just calling a procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_MobileExist"; // Select from the temporary table
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: CheckIsMobileExist() : MobileNo: " + _Mobile + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        #endregion

        #region Get Pages By ID
        public DataSet GetPagesByID()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@ID", _ID),
                                            new SqlParameter("@UserName",_UserName),
                                            new SqlParameter("@Flag", Flag),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_GetPagesByID";
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
                ErrorLog.AgentManagementTrace("UserManagement: GetPagesByID() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Get Pages By Role ID
        public DataSet GetPagesByRoleID()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@ID", _ID),
                                            new SqlParameter("@UserName",_UserName),
                                            new SqlParameter("@Flag", (int)_EnumMenuType),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_GetPagesByRoleID";
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
                ErrorLog.AgentManagementTrace("UserManagement: GetPagesByRoleID() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Manage User Accounts Action
        public void ManageUserAccountsAction(out string _Status)
        {
            _Status = string.Empty;
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL SP_ManageUserAccounts(null, :p_UserID, :p_ActionType, :p_Reason, :p_Password, :p_Salt, :p_UserName, :p_Flag, :p_UserType)";
                        cmd.CommandType = CommandType.Text;

                        // Output parameter
                        var statusParam = new NpgsqlParameter("p_Status", NpgsqlTypes.NpgsqlDbType.Varchar)
                        {
                            Direction = ParameterDirection.Output,
                            Size = 100
                        };
                        cmd.Parameters.Add(statusParam);
                        // Define parameters in the correct order
                        cmd.Parameters.AddWithValue("p_UserID", (object)_UserID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ActionType", (object)_ActionTypeID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Reason", (object)Reason ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Password", (object)_Password ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Salt", DBNull.Value); // If you need to pass null
                        cmd.Parameters.AddWithValue("p_UserName", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.Parameters.AddWithValue("p_UserType", (object)UserType ?? DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Retrieve the output value
                        _Status = statusParam.Value?.ToString() ?? string.Empty;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace($"UserManagement: ManageUserAccountsAction() : UserName: {UserName} Exception: {Ex.Message}");
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        #endregion

        //Switch Audit Trails pgManageUserAccount
        #region insertTblAuditTrail
        public void insertTblAuditTrail(string[] _Params)
        {
            try
            {
                using (var con = new NpgsqlConnection(ConnectionString))
                {
                    con.Open(); // Ensure the connection is opened

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = con;

                        // Use CALL statement to execute the procedure
                        cmd.CommandText = "CALL Proc_InsertSwitchAuditTrail(:p_Username, :p_AuditAction, :p_AuditDesc, :p_Branch)";
                        cmd.CommandType = CommandType.Text;

                        // Add parameters
                        cmd.Parameters.AddWithValue("p_Username", (object)_Params[0] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_AuditAction", (object)_Params[1] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_AuditDesc", (object)_Params[2] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Branch", (object)_Params[3] ?? DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DBError(Ex);
            }
        }


        #endregion

        // Store Login Activities  pgCreateUser
        #region Store Login Activities
        public void StoreLoginActivities(string[] _Params)
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Call the PostgreSQL procedure
                        cmd.CommandText = "CALL InsertAuditLoginActivities(:p_UserName, :p_Action, :p_Description)";
                        cmd.CommandType = CommandType.Text;

                        // Adding parameters
                        cmd.Parameters.AddWithValue("p_UserName", (object)_Params[0] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Action", (object)_Params[1] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Description", (object)_Params[2] ?? DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Execute the command
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: StoreLoginActivities() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        #endregion

        #region Bind Clients Dropdown
        public DataSet BindClientReport()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        // Create the parameters array
                        NpgsqlParameter[] _Params =
                        {
                    new NpgsqlParameter("p_ClientID", (object)ClientID ?? DBNull.Value),
                    new NpgsqlParameter("p_BCID", (object)BCID ?? DBNull.Value),
                    new NpgsqlParameter("p_AgentID", (object)AgentID ?? DBNull.Value),
                    new NpgsqlParameter("p_UserName", (object)UserName ?? DBNull.Value),
                    new NpgsqlParameter("p_Flag", (object)Flag ?? DBNull.Value)
                };

                        cmd.Connection = sqlConn;

                        // Call the procedure to create the temporary table
                        cmd.CommandText = "CALL SP_BindClient_Reports(:p_ClientID, :p_BCID, :p_AgentID, :p_UserName, :p_Flag)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since we're just calling a procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_ClientReports"; // Select from the temporary table
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: BindClientReport() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        #endregion

        #region Bind Franchises Dropdown
        public DataSet BindFranchiseReport()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is opened

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        // Create the parameters array
                        NpgsqlParameter[] _Params =
                        {
                    new NpgsqlParameter("p_ClientID", (object)ClientID ?? DBNull.Value),
                    new NpgsqlParameter("p_BCcode", (object)BCID ?? DBNull.Value),
                    new NpgsqlParameter("p_AgentID", (object)AgentID ?? DBNull.Value),
                    new NpgsqlParameter("p_UserName", (object)UserName ?? DBNull.Value),
                    new NpgsqlParameter("p_Flag", (object)Flag ?? DBNull.Value)
                };

                        cmd.Connection = sqlConn;

                        // Call the procedure to create the temporary table
                        cmd.CommandText = "CALL SP_BindBC_Report(:p_ClientID, :p_BCcode, :p_AgentID, :p_UserName, :p_Flag)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(_Params);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since we're just calling a procedure

                        // Now select from the temporary table to get the results
                        cmd.CommandText = "SELECT * FROM temp_BC_Report"; // Select from the temporary table
                        cmd.CommandType = CommandType.Text;

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: BindFranchiseReport() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        #endregion
    }
    public class MenuSubMenuAccess
    {
        public string MenuId { get; set; }
        public string SubMenuId { get; set; }
        public string Accessibility { get; set; }
        public string IsMenuAccessed { get; set; }
    }

}
