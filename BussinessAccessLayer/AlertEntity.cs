using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using Newtonsoft.Json;
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
    public class AlertEntity
    {
        #region Objects Declaration
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        #endregion


        #region Alert 
        public string UserName { get; set; }
        public string Id { get; set; }
        public string AlertGroupId { get; set; }
        public string AlertDescription { get; set; }
        public string AlertMemberId { get; set; }
        public string AlertSwitch { get; set; }
        public string AlertGroupName { get; set; }
        public string GroupType { get; set; }
        public string MemeberName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CreatedBy { get; set; }
        public int flag { get; set; }
        public string EmailCC { get; set; }
        public string EmailBcc { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public string SMSBody { get; set; }
        public string AlertType { get; set; }
        public string AlertSentOn { get; set; }
        public string TimerInterval { get; set; }
        public string MaxRetryCount { get; set; }
        public string NextInterval { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Bc { get; set; }
        public string Channels { get; set; }
        public string ColumSelector { get; set; }
        public string QuerySelector { get; set; }
        public string TransactionType { get; set; }
        public string DeclineCount { get; set; }
        public string Thresholdlimit { get; set; }
        public string InsType { get; set; }
        public string TemplateID { get; set; }
        public string TemplateName { get; set; }

        #endregion


        #region Property Declaration


        public string groupName { get; set; }
        public string SwitchName { get; set; }
        public string Switchurl { get; set; }
        public int groupId { get; set; }
        public string Aggregator { get; set; }
        public string TxnType { get; set; }
        public string Switch { get; set; }
        public string Channel { get; set; }
        public string IIN { get; set; }
        public string Ratio { get; set; }
        public string SwithMode { get; set; }
        public string ruleName { get; set; }
        public string groupDescription { get; set; }
        public string SwitchDescription { get; set; }
        public string ruleDescription { get; set; }
        public int priority { get; set; }
        public int percentage { get; set; }
        public int Count { get; set; }
        public int Failoverpercentage { get; set; }
        public int FailoverCount { get; set; }
        public int GroupId { get; set; }
        public int SwitchId { get; set; }
        public string FailoverSwitchId { get; set; }
        public int RuleId { get; set; }

        public int IsActive { get; set; }
        public int IsDelete { get; set; }
        public int IsEdit { get; set; }
        public int Flag { get; set; }
        public DataTable dt { get; set; }
        public DataTable dt2 { get; set; }
        public DataTable dt3 { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Severity { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string ComplianceFamily { get; set; }
        public string SelectedPriority { get; set; }
        public string Excep { get; set; }
        #endregion

        #region Alert MemberContact Details
        public DataTable getAlertMembers()
        {
            dt = new DataTable();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("public.Fn_getAletGroupMemberDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_groupid", groupId);
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        dataAdapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Excep = ex.Message + " " + ex.Source + " " + ex.StackTrace;

                ErrorLog.CommonTrace("Class : RuleEntity.cs \nFunction : BindRule() \nException Occurred\n" + Excep);
                ErrorLog.DBError(ex);

                return dt = null;
            }
        }

        public Dictionary<string, DataTable> GetDropDownValues()
        {
            var resultTables = new Dictionary<string, DataTable>();

            try
            {

                string procedureName = "fetch_group_rule_ddl";

                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand($"CALL {procedureName}(@p_username)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_username", UserName);
                        cmd.ExecuteNonQuery();
                    }

                    string[] tempTableNames = { "temp_bindAlertGroup", "temp_bindAlertBCList" };

                    foreach (string tableName in tempTableNames)
                    {
                        Console.WriteLine($"Querying data from: {tableName}");

                        using (var cmd = new NpgsqlCommand($"SELECT * FROM {tableName}", conn))
                        using (var reader = cmd.ExecuteReader())
                        {
                            DataTable tempTable = new DataTable();
                            tempTable.Load(reader);
                            resultTables[tableName] = tempTable;
                        }
                    }

                    using (var dropCmd = new NpgsqlCommand("DROP TABLE IF EXISTS temp_bindgroup, temp_bcpartnerid, temp_bindiin, temp_bindtxntype", conn))
                    {
                        dropCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred: " + ex.Message);
                throw;
            }

            return resultTables;
        }

        public DataTable getChannelListofBCs(string id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.fn_getBcChannelList(@p_bcid)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_bcid", id);
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.CommonTrace("Class: AlertEnitity.cs \nFunction: getChannelListofBCs() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataTable getAlertTypeList(string id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getAlertTypeNameList(@p_id)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_id", string.IsNullOrEmpty(id) ? DBNull.Value : (object)id);
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.CommonTrace("Class: AlertEnitity.cs \nFunction: getChannelListofBCs() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataTable getColumnListByChannel(string id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getColumnSelctionsList(@p_id)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_id", string.IsNullOrEmpty(id) ? DBNull.Value : (object)id);
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.CommonTrace("Class: AlertEnitity.cs \nFunction: getChannelListofBCs() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataTable getResponseCode(string id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getResponseCode(@p_id)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_id", string.IsNullOrEmpty(id) ? DBNull.Value : (object)id);
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.CommonTrace("Class: AlertEnitity.cs \nFunction: getChannelListofBCs() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataTable getOperators(string id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getOperatorsList()", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.CommonTrace("Class: AlertEnitity.cs \nFunction: getChannelListofBCs() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataSet GetAllDllData(string id = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Get Channel List by BCs
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.fn_getBcChannelList(@p_bcid)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_bcid", id ?? (object)DBNull.Value);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "ChannelList");
                        }
                    }

                    // Get Alert Type List
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getAlertTypeNameList(@p_id)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_id", string.IsNullOrEmpty(id) ? DBNull.Value : (object)id);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "AlertTypeList");
                        }
                    }

                    // Get Column List by Channel
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getColumnSelctionsList(@p_id)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_id", string.IsNullOrEmpty(id) ? DBNull.Value : (object)id);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "ColumnList");
                        }
                    }

                    // Get Response Code List
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getResponseCode(@p_id)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_id", string.IsNullOrEmpty(id) ? DBNull.Value : (object)id);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "ResponseCode");
                        }
                    }

                    // Get Operators List
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getOperatorsList()", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "Operators");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Exception in GetAllData: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
            return ds;
        }

        public DataTable GetGroup()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getAletGroupDetails();", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : RuleEntity.cs \nFunction : GetGroup() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataTable SwitchBind(string sw)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getSwitchName(@pid);", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("pid", (object)sw ?? DBNull.Value);
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);  

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : AlertEntity.cs \nFunction : gridBind() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }
         
        public string UpdateGroupStatus()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.update_group_status(@p_username, @p_groupid, @p_isactive, @p_isdelete, @p_isedit, @p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("@p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_groupid", (object)GroupId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_isactive", (object)IsActive ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_isdelete", (object)IsDelete ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_isedit", (object)IsEdit ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_flag", (object)Flag ?? DBNull.Value);

                        // Execute the function and read results
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string statusCode = reader.GetString(reader.GetOrdinal("p_status"));

                                // Return status code
                                return statusCode;
                            }
                            else
                            {
                                return "No result returned from the function.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: AlertEntity.cs \nFunction: UpdateStatus() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public string InsUpdateAlertDetails()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT public.Fn_AlertGroupDetails(@p_groupid, @p_memberid, @p_switch, @p_alerttype, @p_subject, @p_groupname, @p_grouptype, @p_description, @p_memberemail, @p_memberphone, @p_membername, @p_createdby, @p_flag, @p_type, @p_mailto, @p_mailcc, @p_mailbcc, @p_mailbody, @p_smsbody, @p_alertOn, @p_timerinterval, @p_maxretrycount, @p_nextinterval, @p_starttime, @p_endtime,@p_bc,@p_channel,@p_columnselection,@p_querySelector,@p_transactiontype,@p_declinecount)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@p_groupid", (object)AlertGroupId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_memberid", (object)AlertMemberId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_switch", (object)AlertSwitch ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_alerttype", (object)AlertType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_subject", (object)Subject ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_groupname", (object)AlertGroupName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_grouptype", (object)GroupType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_description", (object)AlertDescription ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_memberemail", (object)Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_memberphone", (object)Phone ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_membername", (object)MemeberName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_createdby", (object)CreatedBy ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_flag", "3");
                        cmd.Parameters.AddWithValue("@p_type", (object)InsType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_mailto", (object)Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_mailcc", (object)EmailCC ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_mailbcc", (object)EmailBcc ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_mailbody", (object)EmailBody ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_smsbody", (object)SMSBody ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_alertOn", (object)AlertSentOn ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_timerinterval", (object)TimerInterval ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_maxretrycount", (object)MaxRetryCount ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_nextinterval", (object)NextInterval ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_starttime", (object)StartTime ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_endtime", (object)EndTime ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_bc", (object)Bc ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_channel", (object)Channels ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_columnselection", (object)ColumSelector ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_querySelector", (object)QuerySelector ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_transactiontype", (object)TransactionType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_declinecount", (object)DeclineCount ?? DBNull.Value);
                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "0";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: AlertEntity.cs \nFunction: InsUpdateAlertDetails() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataTable GetEditGroupDetails()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("public.Fn_EditAlertGroupDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_username", UserName);
                        cmd.Parameters.AddWithValue("p_groupid", GroupId);
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataSet = new DataTable();
                        dataAdapter.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : AlertEntity.cs \nFunction : BindRule() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataTable getAletConfigEdit()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_getAletConfigEdit(@p_groupid, @p_memberid)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_groupid", Convert.ToInt32(AlertGroupId));
                        cmd.Parameters.AddWithValue("p_memberid", (object)Convert.ToInt32(AlertMemberId) ?? DBNull.Value);
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.CommonTrace("Class: RuleEntity.cs \nFunction: GetEditRuleDetails() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }
        #endregion

        public string DataTableToJson(DataTable dt)
        {
            return JsonConvert.SerializeObject(dt);
        }

        public DataTable gridBind()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_GetTemplateMaster();", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : AlertEntity.cs \nFunction : gridBind() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataTable GetTemplateMaster(string TemplateType)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_GetTemplateList(@P_templatetype);", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("P_templatetype", TemplateType);
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : AlertEntity.cs \nFunction : gridBind() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public string InsUpdTmpltContentMaster()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT public.Fn_InsUpdateTemplateMaster(@p_id, @p_templateid, @p_templatetype, @p_templatename, @p_subject, @p_mailbody, @p_smsbody, @p_createdby, @P_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@p_id", (object)Id ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_templateid", (object)TemplateID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_templatetype", (object)AlertType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_templatename", (object)TemplateName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_subject", (object)Subject ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_mailbody", (object)EmailBody ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_smsbody", (object)SMSBody ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_createdby", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@P_flag", flag.ToString());
                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "0";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: AlertEntity.cs \nFunction: InsUpdateAlertDetails() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataTable getEditData()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.Fn_GetEditTemplateContent(@P_id,@P_tempid);", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("P_id", AlertGroupId);
                        cmd.Parameters.AddWithValue("P_tempid", AlertType);
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : AlertEntity.cs \nFunction : gridBind() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

    }
}
