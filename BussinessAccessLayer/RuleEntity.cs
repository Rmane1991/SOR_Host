using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessAccessLayer
{
    public class RuleEntity
    {

      

        #region Objects Declaration
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string OutStatus = null; string OutStatusMsg = null;
        #endregion

        #region Property Declaration

        public string UserName { get; set; }
        public string AcquirerId { get; set; }
        public string OrgId { get; set; }
        public string Bin { get; set; }
        public string AuaCode { get; set; }
        public string SubCode { get; set; }
        public string LicenceKey { get; set; }
        public string Id { get; set; }

        public string ReqId { get; set; }
        public string ActionType { get; set; }
        public string Remark { get; set; }
        public string BankCode { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string groupName { get; set; }
        public string SwitchName { get; set; }
        public string BatchhName { get; set; }
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

        #region TrRule
        public DataTable GetRule()
        {
            dt = new DataTable();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    //using (var cmd = new NpgsqlCommand("public.sp_ruledetails", conn))
                    using (var cmd = new NpgsqlCommand("public.sp_ruledetails_Grapgh", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("p_username", UserName);
                        cmd.Parameters.AddWithValue("p_groupid", GroupId);

                        // Create a data adapter
                        var dataAdapter = new NpgsqlDataAdapter(cmd);

                        // Fill the DataTable
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
                // Define the procedure name and parameter
                string procedureName = "fetch_group_rule_ddl";

                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Call the procedure using CALL statement
                    using (var cmd = new NpgsqlCommand($"CALL {procedureName}(@p_username)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_username", UserName);
                        cmd.ExecuteNonQuery(); // Execute the procedure
                    }

                    // Define table names
                    string[] tempTableNames = { "temp_bindgroup", "temp_bcpartnerid", "temp_bindiin", "temp_bindtxntype", "temp_bindswitch" };

                    foreach (string tableName in tempTableNames)
                    {
                        Console.WriteLine($"Querying data from: {tableName}");

                        using (var cmd = new NpgsqlCommand($"SELECT * FROM {tableName}", conn))
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Load data into DataTable
                            DataTable tempTable = new DataTable();
                            tempTable.Load(reader);

                            // Debug output
                            //Console.WriteLine($"Table {tableName} has {tempTable.Rows.Count} rows.");
                            //foreach (DataRow row in tempTable.Rows)
                            //{
                            //    foreach (var item in row.ItemArray)
                            //    {
                            //        Console.Write($"{item}\t");
                            //    }
                            //    Console.WriteLine();
                            //}

                            // Store DataTable in dictionary
                            resultTables[tableName] = tempTable;
                        }
                    }

                    // Clean up temporary tables
                    using (var dropCmd = new NpgsqlCommand("DROP TABLE IF EXISTS temp_bindgroup, temp_bcpartnerid, temp_bindiin, temp_bindtxntype",  conn))
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
        public DataTable GetGroup()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("public.get_group_details", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("p_username", UserName);

                        // Create a data adapter
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataSet = new DataTable();

                        // Fill the DataSet
                        dataAdapter.Fill(dataSet);

                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : RuleEntity.cs \nFunction : BindRule() \nException Occurred\n" + ex.Message);
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: UpdateStatus() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }}

        public string UpdateRuleStatus()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.update_rule_status(@p_username, @p_ruleid, @p_isactive, @p_isdelete, @p_isedit, @p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("@p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ruleid", (object)RuleId ?? DBNull.Value);
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: UpdateStatus() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public string CheckGrpStatus()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.checkisgrpactive(@p_username, @p_ruleid, @p_isactive, @p_isdelete, @p_isedit, @p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("@p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ruleid", (object)RuleId ?? DBNull.Value);
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: UpdateStatus() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public string InsertOrUpdateGroup()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("insert_or_update_group", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_groupname", (object)groupName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_groupdescription", (object)groupDescription ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_groupid", (object)GroupId ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("p_priority", (object)priority ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        // Execute the stored procedure
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
                // Log the exception
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertGroupRule() \nException Occurred\n{ex.Message}");
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
                    using (var cmd = new NpgsqlCommand("public.sp_geteditgroupdetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("p_username", UserName);
                        cmd.Parameters.AddWithValue("p_groupid", GroupId);

                        // Create a data adapter
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataSet = new DataTable();

                        // Fill the DataSet
                        dataAdapter.Fill(dataSet);

                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : RuleEntity.cs \nFunction : BindRule() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public string InsertOrUpdateRule()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("public.fn_insert_or_edit_rule", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_groupid", (object)groupId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_rulename", (object)ruleName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ruledescription", (object)ruleDescription ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_aggregator", (object)Aggregator ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_txntype", (object)TxnType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_switch", string.IsNullOrWhiteSpace(Switch) ? DBNull.Value : (object)Convert.ToInt32(Switch)); // integer
                        cmd.Parameters.AddWithValue("p_iin", (object)IIN ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_channel", string.IsNullOrWhiteSpace(Channel) ? DBNull.Value : (object)Convert.ToInt32(Channel)); // integer
                        cmd.Parameters.AddWithValue("p_percentage", (object)percentage ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_count", (object)Count ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ruleid", (object)RuleId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);

                        // Execute the function and retrieve the result
                        string statusCode = (string)cmd.ExecuteScalar();

                        // Return status code
                        return statusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertGroupRule() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public DataTable GetEditRuleDetails()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("CALL public.sp_geteditruledetails(@p_username, @p_rule_id, @p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_rule_id", (object)RuleId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Query the temporary table to get the results
                        using (var queryCmd = new NpgsqlCommand("SELECT * FROM temp_rule_details", conn))
                        {
                            var dataAdapter = new NpgsqlDataAdapter(queryCmd);
                            var dataTable = new DataTable();
                            dataAdapter.Fill(dataTable);

                            return dataTable;
                        }
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

        #region Switch Configuration
        public DataSet GetSwitch()
        {
            DataSet dataSet = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("CALL public.sp_getswitchdetails(@p_username, @p_switchid, @p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_switchid", (object)SwitchId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();
                    }

                    if (Flag == 1)
                    {
                        // Create a DataTable for temp_switch_details
                        DataTable tempDetailsTable = new DataTable();
                        using (var queryCmd = new NpgsqlCommand("SELECT * FROM temp_switch_details", conn))
                        {
                            var dataAdapter = new NpgsqlDataAdapter(queryCmd);
                            dataAdapter.Fill(tempDetailsTable);
                        }
                        dataSet.Tables.Add(tempDetailsTable);
                    }
                    else if (Flag == 2)
                    {
                        // Create DataTable for each temporary table
                        DataTable tempConfigTable = new DataTable();
                        using (var queryCmd1 = new NpgsqlCommand("SELECT * FROM temp_switch_configuration", conn))
                        {
                            var dataAdapter1 = new NpgsqlDataAdapter(queryCmd1);
                            dataAdapter1.Fill(tempConfigTable);
                        }
                        dataSet.Tables.Add(tempConfigTable);

                        DataTable tempFailoverTable = new DataTable();
                        using (var queryCmd2 = new NpgsqlCommand("SELECT * FROM temp_switch_failover", conn))
                        {
                            var dataAdapter2 = new NpgsqlDataAdapter(queryCmd2);
                            dataAdapter2.Fill(tempFailoverTable);
                        }
                        dataSet.Tables.Add(tempFailoverTable);

                        DataTable tempRoutingTable = new DataTable();
                        using (var queryCmd3 = new NpgsqlCommand("SELECT * FROM temp_switch_routing", conn))
                        {
                            var dataAdapter3 = new NpgsqlDataAdapter(queryCmd3);
                            dataAdapter3.Fill(tempRoutingTable);
                        }
                        dataSet.Tables.Add(tempRoutingTable);
                    }
                }

                return dataSet;
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class : RuleEntity.cs\nFunction : GetSwitch()\nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public string UpdateSwitchStatus()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM public.update_switch_status(@p_username, @p_switchid, @p_isactive, @p_isdelete, @p_isedit, @p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("@p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_switchid", (object)SwitchId ?? DBNull.Value);
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: UpdateStatus() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public string InsertOrUpdateSwitch()
        {
            try
            {
                string jsonData = DataTableToJson(dt);
                string jsonData2 = DataTableToJson(dt2);
                string jsonData3 = DataTableToJson(dt3);
                if (jsonData3 == "null")
                {
                    jsonData3 = "[]"; // Set it to null if it's the string "null"
                }
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("public.insert_or_update_switch", conn)) 
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_switchname", (object)SwitchName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_switchdescription", (object)SwitchDescription ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_percentage", (object)percentage ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_switchid", (object)SwitchId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_count", (object)Count ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_dt", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonData);
                        cmd.Parameters.AddWithValue("p_dt2", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonData2);
                        cmd.Parameters.AddWithValue("p_dt3", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonData3);
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(reader.GetOrdinal("p_status"));
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdateSwitch() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public string DataTableToJson(DataTable dt)
        {
            return JsonConvert.SerializeObject(dt);
        }
        
        public DataTable Get_ddl_switches()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();  // Open connection

                    // Query the function
                    using (var cmd = new NpgsqlCommand("SELECT * FROM fn_get_ddl_switches()", conn))
                    {
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for SELECT queries

                        // Create a data adapter
                        var dataAdapter = new NpgsqlDataAdapter(cmd);
                        var dataSet = new DataTable();

                        // Fill the DataSet
                        dataAdapter.Fill(dataSet);

                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : RuleEntity.cs \nFunction : Get_ddl_switches() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public DataTable BindSwitch()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("public.fn_bindswitch", conn))
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
        public string ValidateSwitch()
        {
            try
            {
                
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("public.validate_switch", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_switchname", (object)SwitchName ?? DBNull.Value);
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(reader.GetOrdinal("p_status"));
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdateSwitch() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public string ValidateGroup()
        {
            try
            {

                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("public.validate_group", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_groupname", (object)groupName ?? DBNull.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(reader.GetOrdinal("p_status"));
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdateSwitch() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public string ValidateFailoverSwitch()
        {
            try
            {
                
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("public.validate_failover_switch", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_switchid", (object)SwitchId ?? DBNull.Value);
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(reader.GetOrdinal("p_status"));
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdateSwitch() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
       

        //public string ValidateRule()
        //{
        //    try
        //    {

        //        using (var conn = new NpgsqlConnection(ConnectionString))
        //        {
        //            conn.Open();
        //            using (var cmd = new NpgsqlCommand("public.validate_rule", conn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("p_groupid", (object)groupId ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("p_rulename", (object)ruleName ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("p_aggregator", (object)Aggregator ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("p_txntype", (object)TxnType ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("p_iin", (object)IIN ?? DBNull.Value);

        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        string status = reader.GetString(reader.GetOrdinal("p_status"));
        //                        string statusMessage = reader.GetString(reader.GetOrdinal("p_statusmessage"));

        //                        // Return or use the status and message
        //                        return $"Status: {status}, Message: {statusMessage}";
        //                    }
        //                    else
        //                    {
        //                        return "No result returned from the function.";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdateSwitch() \nException Occurred\n{ex.Message}");
        //        ErrorLog.DBError(ex);
        //        throw;
        //    }
        //}
        public (string status, string statusMessage) ValidateRule()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("public.validate_rule", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_groupid", (object)groupId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_rulename", (object)ruleName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_aggregator", (object)Aggregator ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_txntype", (object)TxnType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_iin", (object)IIN ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_channel", (object)Channel ?? DBNull.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return (reader.GetString(reader.GetOrdinal("p_status")),
                                        reader.GetString(reader.GetOrdinal("p_statusmessage")));
                            }
                            else
                            {
                                return ("No result", "No result returned from the function.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and rethrow the exception
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: ValidateRule() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public string ValidateSwitchConifg()
        {
            try
            {

                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("public.validate_switch_config", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_switchid", (object)SwitchId ?? DBNull.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(reader.GetOrdinal("p_status"));
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdateSwitch() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public DataSet GetManualDisableSwitch()
        {
            DataSet dataSet = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("CALL public.sp_getmanualswitchdetails(@p_username, @p_switchid, @p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_switchid", (object)SwitchId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();
                    }

                    if (Flag == 1)
                    {
                        // Create a DataTable for temp_switch_details
                        DataTable tempDetailsTable = new DataTable();
                        using (var queryCmd = new NpgsqlCommand("SELECT * FROM temp_switch_details", conn))
                        {
                            var dataAdapter = new NpgsqlDataAdapter(queryCmd);
                            dataAdapter.Fill(tempDetailsTable);
                        }
                        dataSet.Tables.Add(tempDetailsTable);
                    }
                    else if (Flag == 2)
                    {
                        // Create DataTable for each temporary table
                        DataTable tempConfigTable = new DataTable();
                        using (var queryCmd1 = new NpgsqlCommand("SELECT * FROM temp_switch_configuration", conn))
                        {
                            var dataAdapter1 = new NpgsqlDataAdapter(queryCmd1);
                            dataAdapter1.Fill(tempConfigTable);
                        }
                        dataSet.Tables.Add(tempConfigTable);

                        DataTable tempFailoverTable = new DataTable();
                        using (var queryCmd2 = new NpgsqlCommand("SELECT * FROM temp_switch_failover", conn))
                        {
                            var dataAdapter2 = new NpgsqlDataAdapter(queryCmd2);
                            dataAdapter2.Fill(tempFailoverTable);
                        }
                        dataSet.Tables.Add(tempFailoverTable);

                        DataTable tempRoutingTable = new DataTable();
                        using (var queryCmd3 = new NpgsqlCommand("SELECT * FROM temp_switch_routing", conn))
                        {
                            var dataAdapter3 = new NpgsqlDataAdapter(queryCmd3);
                            dataAdapter3.Fill(tempRoutingTable);
                        }
                        dataSet.Tables.Add(tempRoutingTable);
                    }
                }

                return dataSet;
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class : RuleEntity.cs\nFunction : GetSwitch()\nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public string InsertOrUpdateBatch()
        {
            try
            {
                string jsonData = DataTableToJson(dt);
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("public.insert_or_update_batch", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_switchid", (object)SwitchId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_dt", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonData);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(reader.GetOrdinal("p_status"));
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdateSwitch() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public string getmenualswitch()
        {
            try
            {
                string jsonData = DataTableToJson(dt);
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("public.sp_manualswitch", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_switchid", (object)SwitchId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("p_dt", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonData);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(reader.GetOrdinal("p_status"));
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
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertOrUpdateSwitch() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public string InsertOrUpdateBankCOnfiguration()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("public.fn_ioubankconfiguration", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_fromdate", (object)Fromdate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_todate", (object)Todate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_acquirerid", (object)AcquirerId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_orgid", (object)OrgId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bin", (object)Bin ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_auacode", (object)AuaCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_subcode", (object)SubCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_licencekey", (object)LicenceKey ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bankcode", (object)BankCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_reqid", (object)ReqId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);

                        // Execute the function and retrieve the result
                        string statusCode = (string)cmd.ExecuteScalar();

                        // Return status code
                        return statusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: InsertGroupRule() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public DataTable GetBankConfiguration()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("CALL public.sp_getbankconfiguration(:p_username, :p_fromdate, :p_todate, :p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_fromdate", (object)Fromdate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_todate", (object)Todate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                    using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_bankconfiguration", conn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            dataAdapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: YourClassName \nFunction: GetBankConfiguration() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
            return dt;
        }
        public DataTable GetBatchConfiguration()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("CALL public.sp_getbatchconfiguration(:p_username, :p_fromdate, :p_todate, :p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_fromdate", (object)Fromdate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_todate", (object)Todate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                    using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_bankconfiguration", conn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            dataAdapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: YourClassName \nFunction: GetBankConfiguration() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
            return dt;
        }
        public DataTable GetBankConfigurationChecker()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("CALL public.sp_getbankconfigurationchecker(:p_username, :p_fromdate, :p_todate, :p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_fromdate", (object)Fromdate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_todate", (object)Todate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                    using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_bankconfiguration", conn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            dataAdapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: YourClassName \nFunction: GetBankConfiguration() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
            return dt;
        }
        
        public DataSet iouBankConfigVerification()
        {
            DataSet dt = new DataSet();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("CALL public.iouBankConfigVerification(:p_username, :p_id, :p_reqid, :p_remark, :p_flag)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_id", (object)Id ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_reqid", (object)ReqId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_remark", (object)Remark ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                    using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_response_codes", conn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            dataAdapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: YourClassName \nFunction: GetBankConfiguration() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
            return dt;
        }
        #endregion
    }
}

