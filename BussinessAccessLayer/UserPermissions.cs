using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;

namespace BussinessAccessLayer
{
    public class UserPermissions
    {
        public static UserPermissions _UserPermissions = null;
        public static bool IsUserHasPageAccess = false;
        public static string _StatusOutput = null;
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        #region RegisterStartupScriptForNavigationListActive
        public static void RegisterStartupScriptForNavigationListActive(string MenuID, string SubMenuID)
        {
            _UserPermissions = new UserPermissions();
            try
            {
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page,
                                                    typeof(Page),
                                                    "Message",
                                                    @"var els=document.querySelectorAll("".active"");
                                                    for (var i = 0; i < els.length; i++) {
                                                     els[i].classList.remove('active')
                                                               }
                                                    document.getElementById(""subPages" + MenuID + @""").classList.add(""show"");
                                                    document.getElementById(""subPagess" + MenuID + @""").classList.add(""show"");
                                                    document.getElementById(""subm" + SubMenuID + @""").classList.add(""active"");
                                                    document.getElementById(""submm" + SubMenuID + @""").classList.add(""active"");
                                              
                                                    ",
                                                    true);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("Class : UserPermissions.cs \nFunction : RegisterStartupScriptForNavigationListActive() \nException Occured\n" + Ex.Message);
            }
        }
        #endregion

        #region RegisterStartupScriptForNavigationList
        public static void RegisterStartupScriptForNavigationList(string MenuID, string SubMenuID, string tab, string nav)
        {
            _UserPermissions = new UserPermissions();
            try
            {
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page,
                                                    typeof(Page),
                                                    "Message",
                                                    @"var els=document.querySelectorAll("".active"");
                                                    for (var i = 0; i < els.length; i++) {
                                                     els[i].classList.remove('active')
                                                               }
                                                    document.getElementById(""subm" + SubMenuID + @""").classList.add(""active"");
                                                    document.getElementById(""submm" + SubMenuID + @""").classList.add(""active"");
                                                    document.getElementById(""subPages" + MenuID + @""").classList.add(""show"");
                                                    document.getElementById(""subPagess" + MenuID + @""").classList.add(""show"");
                                                    document.getElementById(""" + tab + @""").classList.add(""show"");
                                                    document.getElementById(""" + tab + @""").classList.add(""active"");
                                                    document.getElementById(""" + nav + @""").classList.add(""active"");
                                                    ",
                                                    true);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("Class : UserPermissions.cs \nFunction : RegisterStartupScriptForNavigationList() \nException Occured\n" + Ex.Message);
            }
        }
        #endregion

        #region IsPageAccessibleToUser
        //public static bool IsPageAccessibleToUser(string UserName, string RoleId, string PageName, string PageID)
        //{
        //    _UserPermissions = new UserPermissions();
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
        //            {
        //                SqlParameter[] _Params =
        //                {
        //                     new SqlParameter("@UserName", UserName),
        //                     new SqlParameter("@RoleId", RoleId),
        //                     new SqlParameter("@PageName",PageName),
        //                     new SqlParameter("@PageID", PageID),
        //                     new SqlParameter("@Status", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
        //                };
        //                cmd.Connection = sqlConn;
        //                cmd.CommandText = "SP_CheckPageAcessByPageID";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddRange(_Params);
        //                DataSet dataSet = new DataSet();
        //                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //                dataAdapter.Fill(dataSet);
        //                _StatusOutput = Convert.ToString(cmd.Parameters["@Status"].Value);
        //                cmd.Dispose();
        //                if (String.IsNullOrEmpty(_StatusOutput) || _StatusOutput != "1")
        //                {
        //                    IsUserHasPageAccess = false;
        //                }
        //                else
        //                    IsUserHasPageAccess = true;
        //                return IsUserHasPageAccess;
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.CommonTrace("Class : UserPermissions.cs \nFunction : IsPageAccessibleToUser() \nException Occured\n" + Ex.Message);
        //        return IsUserHasPageAccess;
        //    }
        //}
        public static bool IsPageAccessibleToUser(string UserName, string RoleId, string PageName, string PageID)
        {
            bool IsUserHasPageAccess = false;
            string _StatusOutput = string.Empty;

            // Define your PostgreSQL connection string
            string connString = "Host=your_host;Username=your_username;Password=your_password;Database=your_database";

            try
            {
                // Create and open a connection to the database
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Create the command to execute the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL SP_CheckPageAcessByPageID(@p_username, @p_role_id, @p_page_name, @p_page_id, @p_status)", conn))
                    {
                        // Add input parameters
                        cmd.Parameters.AddWithValue("p_username", UserName);
                        cmd.Parameters.AddWithValue("p_role_id", RoleId);
                        cmd.Parameters.AddWithValue("p_page_name", PageName);
                        cmd.Parameters.AddWithValue("p_page_id", PageID);

                        // Define the input/output parameter
                        var statusParam = new NpgsqlParameter("p_status", NpgsqlTypes.NpgsqlDbType.Varchar);
                        statusParam.Size = 100;
                        statusParam.Direction = ParameterDirection.InputOutput; // Use InputOutput for INOUT parameters
                        cmd.Parameters.Add(statusParam);

                        // Initialize p_status before execution
                        cmd.Parameters["p_status"].Value = DBNull.Value;

                        // Execute the command
                        cmd.ExecuteNonQuery();

                        // Retrieve the output value
                        _StatusOutput = (string)cmd.Parameters["p_status"].Value;

                        // Determine page access based on status
                        IsUserHasPageAccess = !String.IsNullOrEmpty(_StatusOutput) && _StatusOutput == "1";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (replace with your actual logging logic)
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

            return IsUserHasPageAccess;
        }



        #endregion

        #region IsPageControlAccessibleToUser
        public static bool IsPageControlAccessibleToUser(string UserName, string RoleId, string PageName, string PageID, string ControlName, string ControlID, EnumCollection.enumChannelType _enumChannelType, EnumCollection.enumControlType _enumControlType)
        {
            _UserPermissions = new UserPermissions();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                              new SqlParameter("@UserName", UserName),
                              new SqlParameter("@RoleId", RoleId),
                              new SqlParameter("@PageName",PageName),
                              new SqlParameter("@PageID", PageID),
                              new SqlParameter("@ControlName",ControlName),
                              new SqlParameter("@ControlID", ControlID),
                              new SqlParameter("@ChannelTypeId", (int)_enumChannelType),
                              new SqlParameter("@AccessTypeId", (int)_enumControlType),
                              new SqlParameter("@Status", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_CheckPageControlAcessByPageID";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        _StatusOutput = Convert.ToString(cmd.Parameters["@Status"].Value);
                        cmd.Dispose();
                        if (String.IsNullOrEmpty(_StatusOutput) || _StatusOutput != "1")
                        {
                            IsUserHasPageAccess = false;
                        }
                        else
                            IsUserHasPageAccess = true;
                        return IsUserHasPageAccess;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : UserPermissions.cs \nFunction : IsPageControlAccessibleToUser() \nException Occured\n" + Ex.Message);
                IsUserHasPageAccess = false;
            }
            return IsUserHasPageAccess;
        }
        #endregion
    }
}
