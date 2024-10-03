using System;
using System.Data;
using System.Data.SqlClient;
using AppLogger;
using System.Configuration;
using MaxiSwitch.EncryptionDecryption;

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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                             new SqlParameter("@ID", _ID),
                             new SqlParameter("@RoleId", _RoleId),
                             new SqlParameter("@MenuID", _MenuID),
                             new SqlParameter("@MenuName", _MenuName),
                             new SqlParameter("@MenuLink", _MenuLink),
                             new SqlParameter("@SubMenuID", _SubMenuID),
                             new SqlParameter("@SubMenName",_SubMenuName),
                             new SqlParameter("@SubMenuLink",_SubMenuLink),
                             new SqlParameter("@bitIsRemove",_Access),
                             new SqlParameter("@UserName",_UserName),
                             new SqlParameter("@Flag", (int)EnumCollection.EnumBindingType.BindGrid),
                             new SqlParameter("@ClientId",_ClientID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_GetPagePermissions";
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
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserPageManagement(): UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                           new SqlParameter("@RoleID",_RoleId),
                           new SqlParameter("@Userid",_UserID),
                           new SqlParameter("@Status", Status),
                           new SqlParameter("@UserName",UserName),
                           new SqlParameter("@Flag",Flag),
                           new SqlParameter("@ClientId",_ClientID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_GetManageUsers";
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
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccounts() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet FillGridUserAccessManagement()
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
                          new SqlParameter("@RoleName", _RoleId),
                          new SqlParameter("@User",_UserName),
                          new SqlParameter("@Flag", Flag),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_RoleDetails";
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
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccessManagement() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet FillGridCreateRoles()
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
                             new SqlParameter("@RoleName", _RoleId),
                             new SqlParameter("@User",_UserName),
                             new SqlParameter("@Flag", Flag),
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
                ErrorLog.AgentManagementTrace("UserManagement: FillGridCreateRoles() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet FillGridUserAccessManagementForAddRoleValidateRole()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@Role", _Role)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_ValidateRoleId";
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                              new SqlParameter("@ID", _ID),
                              new SqlParameter("@RoleName", _RoleId),
                              new SqlParameter("@User",_UserName),
                              new SqlParameter("@Flag", Flag),
                              new SqlParameter("@Reason",Reason)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_RoleDetails";
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
                ErrorLog.AgentManagementTrace("UserManagement: UserAccessManagementDeleteRole() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet FillGridUserAccessManagementModalPopupMenu()
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
                                            new SqlParameter("@Flag", Flag),
                                            new SqlParameter("@ClientID",_ClientID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_SelectEditMenu";
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
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccessManagementModalPopupMenu() : UserName: " + UserName + " Exception: " + Ex.Message);
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@RoleID", _RoleId),
                                            new SqlParameter("@UserName", _UserName),
                                            new SqlParameter("@Flag", Flag),
                                            new SqlParameter("@ParentRoleId", _ParentID),
                                            new SqlParameter("@ClientID", _ClientID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindRoleGroups_CreateUser";
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
                ErrorLog.AgentManagementTrace("UserManagement: BindUserBasedRole() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        
        public DataSet FillGridUserAccessManagementModalPopupSubMenu()
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
                                            new SqlParameter("@MenuId",_MenuID),
                                            new SqlParameter("@ParentRoleID",_ParentID),
                                            new SqlParameter("@ClientID",_ClientID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_SelectEditSubMenu";
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@RoleID", _RoleId),
                                            new SqlParameter("@MenuId",_MenuID),
                                            new SqlParameter("@ClientID",_ClientID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_SelectSubMenu";
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
                ErrorLog.AgentManagementTrace("UserManagement: FillGridUserAccessManagementMenu() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet UserAccessManagementUpdateTblRoleDetails()
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
                                            new SqlParameter("@UserRoleId",_UserID),
                                            new SqlParameter("@ClientId",_ClientID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UpdateTblRoleDetails";
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
                ErrorLog.AgentManagementTrace("UserManagement: UserAccessManagementUpdateTblRoleDetails() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
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
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@RoleName", _Role),
                                            new SqlParameter("@RoleId", _RoleId),
                                            new SqlParameter("@User", UserName),
                                            new SqlParameter("@Flag",Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UpdateTblRoleDetails";
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
        
        #endregion

        #region Fill Dropdowns
        public DataSet BindDropdowns()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@UserName",_UserName),
                                            new SqlParameter("@Flag", Flag),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindMenuSubMenu";
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
                ErrorLog.AgentManagementTrace("UserManagement: BindDropdowns() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet BindDropdownUsers()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@RoleID",_RoleId),
                                            new SqlParameter("@Userid",_UserID),
                                            new SqlParameter("@Status", Status),
                                            new SqlParameter("@UserName",UserName),
                                            new SqlParameter("@Flag",Flag),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "BindUsers";
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
                ErrorLog.AgentManagementTrace("UserManagement: BindDropdownUsers() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet BindRolesAndRoleGroup()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@RoleID",_RoleId),
                                            new SqlParameter("@UserName",UserName),
                                            new SqlParameter("@Flag",Flag),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindDropdownRoles";
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
                ErrorLog.AgentManagementTrace("UserManagement: BindRolesAndRoleGroup() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet BindRoles_CreateUser()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@RoleID",_RoleId),
                                            new SqlParameter("@UserName",UserName),
                                            new SqlParameter("@Flag",Flag),
                                            new SqlParameter("@ParentRoleId", _ParentID),
                                            new SqlParameter("@ClientID",_ClientID),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindRoles_CreateUser";
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@ID", _ID),
                                            new SqlParameter("@RoleId", _RoleId),
                                            new SqlParameter("@MenuID", _MenuID),
                                            new SqlParameter("@MenuName", _MenuName),
                                            new SqlParameter("@MenuLink", _MenuLink),
                                            new SqlParameter("@MenuIcon",MenuIcon),
                                            new SqlParameter("@SubMenuID", _SubMenuID),
                                            new SqlParameter("@SubMenName",_SubMenuName),
                                            new SqlParameter("@SubMenuLink",_SubMenuLink),
                                            new SqlParameter("@bitIsRemove",_bitIsRemove),
                                            new SqlParameter("@UserName",_UserName),
                                            new SqlParameter("@Flag", (int)_EnumMenuType),
                                            new SqlParameter("@ClientID",_ClientID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_InsertUpdateMenuPermissions";
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
                ErrorLog.AgentManagementTrace("UserManagement: InsertUpdatePagePermissions() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet CreateUser()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@username",_UserName),
                                            new SqlParameter("@password",_Password),
                                            new SqlParameter("@email",Email),
                                            new SqlParameter("@rollid",_RoleId),
                                            new SqlParameter("@UserRoleGroup",UserRoleGroup),
                                            new SqlParameter("@createdBy",UserName),
                                            new SqlParameter("@Salt",_RandomStringForSalt),
                                            new SqlParameter("@Salt1",_RandomStringForSalt),
                                            new SqlParameter("@Salt2",""),
                                            new SqlParameter("@Salt3",""),
                                            new SqlParameter("@Salt4",""),
                                            new SqlParameter("@ClientId",_ClientID),
                                            new SqlParameter("@UserId",_UserId),
                                            new SqlParameter("@Mobile",_Mobile)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_CreateUser";
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@UserName",_UserName),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_CheckIsUserExist";
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
                ErrorLog.AgentManagementTrace("UserManagement: CheckIsUserExist() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet CheckIsEmailExist()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@UserEmailID", _Email),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_CheckIsEmailExist";
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
                ErrorLog.AgentManagementTrace("UserManagement: CheckIsEmailExist() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet CheckIsMobileExist()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@mobileNo",_Mobile),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_CheckIsMobileExist";
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
                ErrorLog.AgentManagementTrace("UserManagement: CheckIsMobileExist() : UserName: " + UserName + " Exception: " + Ex.Message);
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                  new SqlParameter("@UserID", _UserID),
                                  new SqlParameter("@ActionType", _ActionTypeID),
                                  new SqlParameter("@Reason", Reason),
                                  new SqlParameter("@Password", _Password),
                                  new SqlParameter("@Salt", null),
                                  new SqlParameter("@UserName", UserName),
                                  new SqlParameter("@Flag", Flag),
                                  new SqlParameter("@Status", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output },
                                  new SqlParameter("@UserType", UserType)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_ManageUserAccounts";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        _Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        sqlConn.Close();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("UserManagement: ManageUserAccountsAction() : UserName: " + UserName + " Exception: " + Ex.Message);
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
                SqlParameter[] _sqlParameter = {
                                                 new SqlParameter("@Username",_Params[0]),
                                                 new SqlParameter("@AuditAction",_Params[1]),
                                                 new SqlParameter("@AuditDesc",_Params[2]),
                                                 new SqlParameter("@Branch",_Params[3]),
                                               };
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Proc_InsertSwitchAuditTrail";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (cmd.Connection.State == ConnectionState.Open)
                    cmd.Connection.Close();
                cmd.Connection.Open();
                cmd.Parameters.AddRange(_sqlParameter);
                cmd.ExecuteNonQuery();
                con.Close();
                //updateRecords("Proc_InsertSwitchAuditTrail", _sqlParameter);
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _sqlParameter =
                        {
                                                 new SqlParameter("@UserName",_Params[0]),
                                                 new SqlParameter("@Action",_Params[1]),
                                                 new SqlParameter("@Description",_Params[2])
                        };
                        SqlConnection con = new SqlConnection(ConnectionString);
                        con.Open();
                        cmd.CommandText = "InsertAuditLoginActivities";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        if (cmd.Connection.State == ConnectionState.Open)
                        cmd.Connection.Close();
                        cmd.Connection.Open();
                        cmd.Parameters.AddRange(_sqlParameter);
                        cmd.ExecuteNonQuery();
                        con.Close();
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

        #region Bind Clients Dropdown
        public DataSet BindClientReport()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                                 new SqlParameter("@ClientID",ClientID),
                                                 new SqlParameter("@BCID",BCID),
                                                 new SqlParameter("@AgentID", AgentID),
                                                 new SqlParameter("@UserName",UserName),
                                                 new SqlParameter("@Flag", Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindClient_Reports";
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                                 new SqlParameter("@ClientID",ClientID),
                                                 new SqlParameter("@BCcode",BCID),
                                                 new SqlParameter("@AgentID", AgentID),
                                                 new SqlParameter("@UserName",UserName),
                                                 new SqlParameter("@Flag", Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindBC_Report";
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
                ErrorLog.AgentManagementTrace("UserManagement: BindFranchiseReport() : UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion
    }
}
