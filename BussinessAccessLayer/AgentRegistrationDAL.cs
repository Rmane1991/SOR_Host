using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace BussinessAccessLayer
{
    public class AgentRegistrationDAL
    {
        #region Object Declaration
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        static int CommandTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeOut"]);
        #endregion

        #region Properties
        public string AgentCode { get; set; }
        public string ClientId { get; set; }
        public string CreatedBy { get; set; }
        public string AgentName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string PassportNo { get; set; }
        public string PersonalEmailID { get; set; }
        public string FirstName { get; set; }
        public string AgentDOB { get; set; }
        public string LandlineNo { get; set; }
        public string AlternateNo { get; set; }
        public string AadharNo { get; set; }
        public string PanNo { get; set; }
        public string DeviceCode { get; set; }
        public string GSTNo { get; set; }
        public string AgentDistrict { get; set; }
        public string AgentAddress { get; set; }
        public string AgentCountry { get; set; }
        public string AgentState { get; set; }
        public string PopulationGroup { get; set; }
        public string AgentCity { get; set; }
        public string AgentPincode { get; set; }
        public string BCCode { get; set; }
        public string AggCode { get; set; }
        public string AgentCategory { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string Bank { get; set; }
        public string ShopAddress { get; set; }
        public string shopemail { get; set; }
        public string ShopCountry { get; set; }
        public string ShopState { get; set; }
        public string ShopCity { get; set; }
        public string ShopDistrict { get; set; }
        public string ShopPinCode { get; set; }
        public int IsPANVerified { get; set; }
        public int IsNSVerified { get; set; }
        public int IsIBAVerified { get; set; }
        public int KYCTypeId { get; set; }
        public string KYCType { get; set; }
        public string IdentityProofType { get; set; }
        public string IdentityProofDocument { get; set; }
        public string AddressProofType { get; set; }
        public string SignatureProofType { get; set; }
        public string AddressProofDocument { get; set; }
        public string SignatureProofDocument { get; set; }
        public string BusinessEmailID { get; set; }
        public int agReqId { get; set; }
        public string Documents { get; set; }
        public string DocumentType { get; set; }
        public int AEPS { get; set; }
        public int MATM { get; set; }
        public int StageId { get; set; }
        public int Stage { get; set; }
        public int Flag { get; set; }
        public int RequestId { get; set; }
        public int Status { get; set; }
        public string StatusMsg = null;
        public string UserName { get; set; }
        public string RequestTypeId { get; set; }
        public string BucketId { get; set; }
        public string RequestStatusId { get; set; }
        public string IsRemoved { get; set; }
        public string IsActive { get; set; }
        public int VerificationStatus { get; set; }
        public int SFlag { get; set; }
        public string IsdocUploaded { get; set; }
        public string AgentID { get; set; }
        public string TerminalId { get; set; }
        public string BCstatus { get; set; }
        public string Mstatus { get; set; }
        public int ApiReqId { get; set; }

        public string Prestatus { get; set; }
        public string ChStatus { get; set; }
        public string AtStatus { get; set; }
        public string AgentReqId { get; set; }
        public string Activity { get; set; }
        public string ATRemark { get; set; }
        public string CheckerRemark { get; set; }
        public string MakerRemark { get; set; }
        public string ActionType { get; set; }
        public int ActivityType { get; set; }
        public string BusinessEmail { get; set; }
        public string PersonalContact { get; set; }
        public string _fileLineNo { get; set; }
        public string Mode { get; set; }
        public string _RandomStringForSalt { get; set; }
        public string Password { get; set; }
        public int DocumentID { get; set; }
        public string BcRemarks { get; set; }
        public string VerificationLevel { get; set; }
        public int RoleID { get; set; }
        public string OperationType { get; set; }
        public string FileName { get; set; }
        public int mode { get; set; }
        public string FileID { get; set; }

        public string Fromdate { get; set; }
        public string Todate { get; set; }

        public string ParentRoleID { get; set; }

        public string Lattitude { get; set; }

        public string Longitude { get; set; }
        #endregion

        #region Registration
        public DataTable GetBC(string UserName, int? VerificationStatus, int? IsActive, int? IsRemoved, string ClientID, int? IsdocUploaded)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SELECT * FROM SP_BindBC_Reports(@p_UserName, @p_IsVerified, @p_IsActive, @p_IsRemoved, @p_ClientID, @p_IsDocUploaded)";

                        // Set parameters with p_ prefix
                        cmd.Parameters.Add(new NpgsqlParameter("p_UserName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)UserName ?? DBNull.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("p_IsVerified", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)VerificationStatus ?? DBNull.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("p_IsActive", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)IsActive ?? DBNull.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("p_IsRemoved", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)IsRemoved ?? DBNull.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("p_ClientID", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)ClientID ?? DBNull.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("p_IsDocUploaded", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)IsdocUploaded ?? DBNull.Value });

                        DataTable dataTable = new DataTable();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataTable);
                        }

                        cmd.Dispose();
                        return dataTable;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetBC: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        public DataTable GetAggregator(string UserName, int? VerificationStatus, int? IsActive, int? IsRemoved, string ClientID, int? IsdocUploaded)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SELECT * FROM sp_bind_aggregator_reports(@p_UserName, @p_IsVerified, @p_IsActive, @p_IsRemoved, @p_ClientID, @p_IsDocUploaded)";

                        // Set parameters with p_ prefix
                        cmd.Parameters.Add(new NpgsqlParameter("p_UserName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)UserName ?? DBNull.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("p_IsVerified", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)VerificationStatus ?? DBNull.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("p_IsActive", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)IsActive ?? DBNull.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("p_IsRemoved", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)IsRemoved ?? DBNull.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("p_ClientID", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)ClientID ?? DBNull.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("p_IsDocUploaded", NpgsqlTypes.NpgsqlDbType.Integer) { Value = (object)IsdocUploaded ?? DBNull.Value });

                        DataTable dataTable = new DataTable();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataTable);
                        }

                        cmd.Dispose();
                        return dataTable;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetBC: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        

        public DataSet GetCountryStateCity(string UserName, int Mode, int CountryID, int StateID)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@Mode", Mode),
                            new SqlParameter("@CountryID", CountryID),
                            new SqlParameter("@StateID", StateID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_GetCountryStateCity";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetCountryStateCity: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        //public DataTable GetAgentRequestList(string UserName, string BCCode, int RequestTypeId, int BucketId, int RequestStatusId, string PanNo, string AadharNo, string GSTNo, string PassportNo, int ContactNo, string PersonalEmailID, int Flag)
        public DataSet GetAgentRequestList()
        {
            DataSet ds = new DataSet();
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Prepare the command to call the stored procedure
                    using (NpgsqlCommand cmd = new NpgsqlCommand("CALL sp_agentrequest_registration_get(:p_RequestTypeId, :p_BucketId, :p_RequestStatusId, :p_AgentCode, :p_PanNo, :p_AadharNo, :p_GSTNo, :p_PassportNo, :p_ContactNo, :p_PersonalEmailID, :p_Prestatus, :p_Makerstatus, :p_Checkstatus, :p_Authstatus, :p_username, :p_RequestId, :p_Flag)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Adding parameters with proper handling for nulls
                        cmd.Parameters.AddWithValue("p_RequestTypeId", (object)RequestTypeId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_BucketId", (object)BucketId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_RequestStatusId", (object)RequestStatusId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_AgentCode", (object)AgentCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_PanNo", (object)PanNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_AadharNo", (object)AadharNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_GSTNo", (object)GSTNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_PassportNo", (object)PassportNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ContactNo", (object)ContactNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_PersonalEmailID", (object)PersonalEmailID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Prestatus", (object)Prestatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Makerstatus", (object)Mstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Checkstatus", (object)ChStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Authstatus", (object)AtStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_RequestId", RequestId);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);

                        // Execute the stored procedure
                        cmd.ExecuteNonQuery(); // Use ExecuteNonQuery since the procedure may not return a result set directly
                    }

                    // Now retrieve the data from the temporary table
                    using (NpgsqlCommand selectCmd = new NpgsqlCommand("SELECT * FROM temp_AgentRequests", sqlConn))
                    {
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            dataAdapter.Fill(ds); // Fill the DataSet with the results
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error details for debugging purposes
                ErrorLog.AgentManagementTrace($"AgentRegistrationDAL: GetAgentRequestList: UserName: {UserName} Exception: {ex.Message}");
                ErrorLog.DBError(ex);
                throw; // Rethrow the exception for handling upstream if necessary
            }

            // Check if any tables were populated
            //if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            //{
            //    // Log or handle the case where no data was returned
            //    throw new Exception("No data returned from the stored procedure.");
            //}

            return ds; // Return the populated DataSet
        }


        public bool Validate_AgentRequest(string UserName, out int Status, out string StatusMsg)
        {
            Status = -1;
            StatusMsg = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@PersonalEmailID",PersonalEmailID),
                            new SqlParameter("@ContactNo",ContactNo),
                            new SqlParameter("@PassportNo",PassportNo),
                            new SqlParameter("@AadharNo",AadharNo),
                            new SqlParameter("@PanNo",PanNo),
                            new SqlParameter("@DeviceCode",DeviceCode),
                            new SqlParameter("@GSTNo",GSTNo),
                            new SqlParameter("@AccountNumber",AccountNumber),
                            new SqlParameter("@AgentName",AgentName),
                            new SqlParameter("@MiddleName",MiddleName),
                            new SqlParameter("@LastName",LastName),
                            new SqlParameter("@AgentCity",AgentCity),
                            new SqlParameter("@AgentPincode",AgentPincode),
                            new SqlParameter("@RequestId",RequestId),
                            new SqlParameter("@Status", SqlDbType.Int){ Direction = ParameterDirection.Output },
                            new SqlParameter("@StatusMsg", SqlDbType.VarChar,100){ Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_AgentRequest_Registration_Validate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        Status = Convert.ToInt32(cmd.Parameters["@Status"].Value);
                        StatusMsg = Convert.ToString(cmd.Parameters["@StatusMsg"].Value);
                        sqlConn.Close();
                        cmd.Dispose();
                        return Status == 1 ? true : false;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: Validate_AgentRequest: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public bool Insert_AgentRequest(string UserName, out int requestId, out string status, out string statusMsg)
        {
            requestId = 0; // Default value
            status = "-1";
            statusMsg = string.Empty;

            var notices = new List<string>();

            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    // Subscribe to the Notice event
                    sqlConn.Notice += (o, e) =>
                    {
                        var message = e.Notice.MessageText;
                        ErrorLog.CommonTrace($"NOTICE: {message}");
                        notices.Add(message);
                    };

                    // Open the connection
                    sqlConn.Open();

                    using (var cmd = new NpgsqlCommand("CALL public.SP_AgentRequest_Registration_Insert(NULL, NULL, NULL, @p_clientid, @p_createdby, @p_agentname, @p_middlename, @p_lastname, @p_gender, @p_emailid, @p_contactno, @p_passportno, @p_personalemailid, @p_firstname, @p_agentdob, @p_devicecode, @p_landlineno, @p_alternateno, @p_bccode, @p_aadharno, @p_panno, @p_gstno, @p_agentdistrict, @p_agentaddress, @p_agentcountry, @p_agentstate, @p_agentcity, @p_agentpincode, @p_shopaddress, @p_populationgroup, @p_agentcategory, @p_accountname, @p_accountnumber, @p_ifscode, @p_bank, @p_shopcountry, @p_shopstate, @p_shopcity, @p_shopemail, @p_shopdistrict, @p_shoppincode, @p_ispanverified, @p_isnsverified, @p_isibaverified, @p_kyctypeid, @p_kyctype, @p_identityprooftype, @p_identityproofdocument, @p_addressprooftype, @p_signatureprooftype, @p_addressproofdocument, @p_signatureproofdocument, @p_businessemailid, @p_agreqid, @p_documents, @p_documenttype, @p_terminalid, @p_lattitude, @p_longitude, @p_agentcode, @p_aeps, @p_matm, @p_stageid, @p_stage, @p_flag, @p_activitytype,@p_aggCode)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Output parameters
                        var outputRequestId = new NpgsqlParameter("p_RequestId", NpgsqlTypes.NpgsqlDbType.Integer)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputRequestId);

                        var outputStatus = new NpgsqlParameter("p_Status", NpgsqlTypes.NpgsqlDbType.Integer)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputStatus);

                        var outputStatusMsg = new NpgsqlParameter("p_StatusMsg", NpgsqlTypes.NpgsqlDbType.Text)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputStatusMsg);

                        // Add other parameters
                        cmd.Parameters.AddWithValue("p_clientid", (object)ClientId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_createdby", (object)CreatedBy ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agentname", (object)AgentName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_middlename", (object)MiddleName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_lastname", (object)LastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_gender", (object)Gender ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_emailid", (object)PersonalEmailID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_contactno", (object)ContactNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_passportno", (object)PassportNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_personalemailid", (object)PersonalEmailID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_firstname", (object)FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agentdob", (object)AgentDOB ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_devicecode", (object)DeviceCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_landlineno", (object)LandlineNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_alternateno", (object)AlternateNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bccode", (object)BCCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_aadharno", (object)AadharNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_panno", (object)PanNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_gstno", (object)GSTNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agentdistrict", (object)AgentDistrict ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agentaddress", (object)AgentAddress ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agentcountry", (object)AgentCountry ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agentstate", (object)AgentState ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agentcity", (object)AgentCity ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agentpincode", (object)AgentPincode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_shopaddress", (object)ShopAddress ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_populationgroup", (object)PopulationGroup ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agentcategory", (object)AgentCategory ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_accountname", (object)AccountName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_accountnumber", (object)AccountNumber ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ifscode", (object)IFSCCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bank", (object)Bank ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_shopcountry", (object)ShopCountry ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_shopstate", (object)ShopState ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_shopcity", (object)ShopCity ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_shopemail", (object)shopemail ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_shopdistrict", (object)ShopDistrict ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_shoppincode", (object)ShopPinCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ispanverified", (object)IsPANVerified ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_isnsverified", (object)IsNSVerified ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_isibaverified", (object)IsIBAVerified ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_kyctypeid", (object)KYCTypeId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_kyctype", (object)KYCType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_identityprooftype", (object)IdentityProofType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_identityproofdocument", (object)IdentityProofDocument ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_addressprooftype", (object)AddressProofType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_signatureprooftype", (object)SignatureProofType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_addressproofdocument", (object)AddressProofDocument ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_signatureproofdocument", (object)SignatureProofDocument ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_businessemailid", (object)BusinessEmailID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agreqid", (object)agReqId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_documents", (object)Documents ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_documenttype", (object)DocumentType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_terminalid", (object)TerminalId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_lattitude", (object)Lattitude ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_longitude", (object)Longitude ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_agentcode", (object)AgentID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_aeps", AEPS);
                        cmd.Parameters.AddWithValue("p_matm", MATM);
                        cmd.Parameters.AddWithValue("p_stageid", (object)StageId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_stage", (object)Stage ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", Flag);
                        cmd.Parameters.AddWithValue("p_activitytype", string.IsNullOrEmpty(Activity) ? "0" : Activity);
                        cmd.Parameters.AddWithValue("p_aggCode", (object)AggCode ?? DBNull.Value);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            // Retrieve output parameters
                            //requestId = Convert.ToInt32(outputRequestId.Value);
                            //requestId = (cmd.Parameters["p_RequestId"].Value is DBNull) ? 0 : Convert.ToInt32(cmd.Parameters["p_RequestId"].Value);

                            //status = Convert.ToInt32(outputStatus.Value).ToString();
                            //statusMsg = Convert.ToString(outputStatusMsg.Value);
                            //return status == "1"; // Return true if status is "1"
                            if (outputRequestId.Value != DBNull.Value)
                            {
                                requestId = Convert.ToInt32(outputRequestId.Value);
                            }
                            else
                            {
                                // Handle the case where outputRequestId is DBNull
                                requestId = 0; // or some other default value or logic
                            }
                            //requestId = Convert.ToInt32(outputRequestId.Value);
                            status = Convert.ToString(outputStatus.Value);
                            statusMsg = outputStatusMsg.Value.ToString();
                            Console.WriteLine($"Request ID: {requestId}, Status: {status}, Status Message: {statusMsg}");
                            if (status != "1")
                            {
                                return false;
                            }
                            return true;
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.CommonTrace($"Error executing command: {ex.Message}");
                            statusMsg = ex.Message;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Connection error: {ex.Message}");
                statusMsg = ex.Message;
                return false;
            }
            finally
            {
                // Log notices if any
                if (notices.Count > 0)
                {
                    foreach (var notice in notices)
                    {
                        ErrorLog.CommonTrace($"NOTICE: {notice}");
                    }
                }
            }
        }

        #endregion

        #region BindAgentVerifyddl

        public DataSet BindAgentVerifyddl()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // First, call the stored procedure to populate the temp table
                    using (var cmd = new NpgsqlCommand("CALL public.sp_bindagentverification(" +
                        "@p_clientid, @p_bccode, @p_isremoved, @p_username, @p_checkstatus, @p_authstatus, @p_makerstatus, @p_flag)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text; // Use Text for calling functions/procedures

                        // Define parameters
                        cmd.Parameters.AddWithValue("p_clientid", (object)ClientId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bccode", (object)AggCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_isremoved", (object)IsRemoved ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_username", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_checkstatus", (object)ChStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_authstatus", (object)AtStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_makerstatus", (object)Mstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", (object)Flag ?? DBNull.Value);

                        // Execute the stored procedure to populate the temp table
                        cmd.ExecuteNonQuery();
                    }

                    // After calling the stored procedure, select from the temporary table
                    using (var selectCmd = new NpgsqlCommand("SELECT * FROM temp_agent_verification", sqlConn))
                    {
                        DataSet ds = new DataSet();
                        using (var dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            dataAdapter.Fill(ds); // Fill the DataSet with the results
                        }

                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error details for debugging purposes
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: BindAgentVerifyddl: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw; // Rethrow the exception for handling upstream if necessary
            }
        }



        #endregion

        #region GetAgentDetailsToProcessOnboaring
        public DataSet GetAgentDetailsToProcessOnboaring()
        {
            DataSet ds = new DataSet();
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Prepare the command to call the stored procedure
                    using (NpgsqlCommand cmd = new NpgsqlCommand("CALL Proc_GetAgentDetailsToProcessOnboaring(:p_BCID, :p_ClientID, :p_AgentReqId, :p_User, :p_IsDocUploaded, :p_BCstatus, :p_Makerstatus, :p_Checkstatus, :p_Authstatus, :p_IsVerified, :p_ActivityType, :p_IsActive, :p_IsRemoved, :p_TerminalId, :p_Flag)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Adding parameters with proper handling for nulls
                        cmd.Parameters.AddWithValue("p_BCID", (object)BCCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ClientID", (object)ClientId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_AgentReqId", (object)AgentCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_User", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_IsDocUploaded", (object)IsdocUploaded ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_BCstatus", (object)BCstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Makerstatus", (object)Mstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Checkstatus", (object)ChStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Authstatus", (object)AtStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_IsVerified", (object)VerificationStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ActivityType", (object)Activity ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_IsActive", (object)IsActive ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_IsRemoved", (object)IsRemoved ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_TerminalId", (object)TerminalId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", (object)Flag ?? DBNull.Value);

                        // Execute the stored procedure
                        cmd.ExecuteNonQuery(); // Call the procedure
                    }

                    // Now retrieve the data from the temporary table
                    using (NpgsqlCommand selectCmd = new NpgsqlCommand("SELECT * FROM temp_AgentRequests", sqlConn))
                    {
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectCmd))
                        {
                            dataAdapter.Fill(ds); // Fill the DataSet with the results
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error details for debugging purposes
                ErrorLog.AgentManagementTrace($"AgentRegistrationDAL: GetAgentDetailsToProcessOnboaring: UserName: {UserName} Exception: {ex.Message}");
                ErrorLog.DBError(ex);
                throw; // Rethrow the exception for handling upstream if necessary
            }
            return ds;
        }

        #endregion


        #region ChangeAgentStatus
        public DataSet ChangeAgentStatus()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Step 1: Call the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL public.sp_updateagentonboardprocess(@p_AgentCode, @p_ActionType, @p_ActivityType, @p_UserName, @p_Flag, @p_BcID, @p_AgentReqId, @p_BCRemarks, @p_BCStatus, @p_MRemarks, @p_MStatus, @p_CHRemarks, @p_CHStatus, @p_ATRemarks, @p_ATStatus)", sqlConn))
                    {
                        // Define parameters
                        cmd.Parameters.AddWithValue("p_AgentCode", (object)AgentCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ActionType", ActionType);
                        cmd.Parameters.AddWithValue("p_ActivityType", (object)ActivityType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_UserName", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", (object)Flag ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_BcID", (object)BCCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_AgentReqId", (object)AgentReqId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_BCRemarks", (object)BcRemarks ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_BCStatus", (object)BCstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_MRemarks", (object)MakerRemark ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_MStatus", (object)Mstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_CHRemarks", (object)CheckerRemark ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_CHStatus", (object)ChStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ATRemarks", (object)ATRemark ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ATStatus", (object)AtStatus ?? DBNull.Value);

                        // Execute the procedure
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

        #region ChangeAgentStatus
        public DataSet BlockAgentStatus()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {

                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@AgentCode",AgentCode),
                                            new SqlParameter("@ActionType",ActionType),
                                            new SqlParameter("@ActivityType",ActivityType),
                                            new SqlParameter("@UserName",UserName),
                                            new SqlParameter("@Flag",Flag),
                                            new SqlParameter("@BcID",BCCode),
                                            new SqlParameter("@AgentReqId",AgentReqId),
                                            new SqlParameter("@BCRemarks",BcRemarks),
                                            new SqlParameter("@BCStatus",BCstatus),
                                            new SqlParameter("@MRemarks",MakerRemark),
                                            new SqlParameter("@MStatus",Mstatus),
                                            new SqlParameter("@CHRemarks",CheckerRemark),
                                            new SqlParameter("@CHStatus",ChStatus),
                                            new SqlParameter("@ATRemarks",ATRemark),
                                            new SqlParameter("@ATStatus",AtStatus)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UpdateBlockAgentOnboardProcess";//SP_UpdateBlockAgent
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentStatus: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        # region GetAgentDocuments
        public DataSet GetAgentDocuments()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Call the procedure with the correct parameters
                    using (var cmd = new NpgsqlCommand("CALL Proc_GetAgentDocuments(@p_Mode, @p_AgentReqID, @p_DeviceSerialNumber, @p_ID)", sqlConn))
                    {
                        // Use the correct parameter names with the @ prefix
                        cmd.Parameters.AddWithValue("p_Mode", Mode);
                        cmd.Parameters.AddWithValue("p_AgentReqID", AgentReqId);
                        cmd.Parameters.AddWithValue("p_DeviceSerialNumber", DBNull.Value); // Replace with actual value if needed
                        cmd.Parameters.AddWithValue("p_ID", DBNull.Value); // Replace with actual value if needed

                        cmd.ExecuteNonQuery(); // Execute the procedure
                    }

                    // Now retrieve data from the temporary tables based on the mode
                    DataSet dataSet = new DataSet();

                    if (Mode == "GetAgentDetails")
                    {
                        using (var cmd = new NpgsqlCommand("SELECT * FROM temp_AgentDetails", sqlConn))
                        {
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                dataAdapter.Fill(dataSet, "AgentDetails"); // Fill the DataSet with results from temp_AgentDetails
                            }
                        }

                        using (var cmd = new NpgsqlCommand("SELECT * FROM temp_ProofTypes", sqlConn))
                        {
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                dataAdapter.Fill(dataSet, "ProofTypes"); // Fill the DataSet with results from temp_ProofTypes
                            }
                        }
                    }
                    else if (Mode == "GetAgentDocumentById")
                    {
                        using (var cmd = new NpgsqlCommand("SELECT * FROM temp_IdentityProof", sqlConn))
                        {
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                dataAdapter.Fill(dataSet, "IdentityProof"); // Fill the DataSet with results from temp_IdentityProof
                            }
                        }

                        using (var cmd = new NpgsqlCommand("SELECT * FROM temp_AddressProof", sqlConn))
                        {
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                dataAdapter.Fill(dataSet, "AddressProof"); // Fill the DataSet with results from temp_AddressProof
                            }
                        }

                        using (var cmd = new NpgsqlCommand("SELECT * FROM temp_SignatureProof", sqlConn))
                        {
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                dataAdapter.Fill(dataSet, "SignatureProof"); // Fill the DataSet with results from temp_SignatureProof
                            }
                        }

                        using (var cmd = new NpgsqlCommand("SELECT * FROM temp_Documents", sqlConn))
                        {
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                dataAdapter.Fill(dataSet, "Documents"); // Fill the DataSet with results from temp_Documents
                            }
                        }
                    }

                    return dataSet; // Return the filled DataSet
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetAgentDocuments: Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        #endregion

        #region Insert Agent master Details

        public DataSet SetInsertUpdateAgentMasterDetails()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Execute the stored procedure
                    using (var cmd = new NpgsqlCommand("call sp_insertorupdateagentmaterdetails(:P_Flag, :P_CreatedBy, :p_agentreqid, :P_ClientId, :P_Salt, :P_Salt1, :P_Salt2, :P_Salt3, :P_Salt4, :P_Password)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("P_Flag", Flag);
                        cmd.Parameters.AddWithValue("P_CreatedBy", CreatedBy);
                        cmd.Parameters.AddWithValue("p_agentreqid", AgentReqId);
                        cmd.Parameters.AddWithValue("P_ClientId", ClientId);
                        cmd.Parameters.AddWithValue("P_Salt", _RandomStringForSalt);
                        cmd.Parameters.AddWithValue("P_Salt1", _RandomStringForSalt);
                        cmd.Parameters.AddWithValue("P_Salt2", DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Salt3", DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Salt4", DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Password", Password);

                        cmd.ExecuteNonQuery(); // Execute the stored procedure
                    }

                    // Now retrieve results from the temporary table
                    DataSet ds = new DataSet();
                    using (var resultCmd = new NpgsqlCommand("SELECT * FROM temp_StatusMessages", sqlConn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(resultCmd))
                        {
                            DataTable dt = new DataTable("ResultsFromTempTable");
                            dataAdapter.Fill(dt); // Fill the DataTable with the results
                            ds.Tables.Add(dt);
                        }
                    }

                    return ds; // Return the DataSet with results
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: AgentStatusReportGrid: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw; // Re-throw the exception after logging
            }
        }



        #endregion

        #region ChangeAgentStatus
        public DataSet ActiveDeactiveAgent()
        {
            try
            {
                using (var cmd = new NpgsqlCommand())
                {
                    using (var sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        // Prepare parameters for the stored procedure
                        var _Params = new[]
                        {
                    new NpgsqlParameter("@Remarks", MakerRemark ?? (object)DBNull.Value),
                    new NpgsqlParameter("@AgentCode", AgentCode ?? (object)DBNull.Value),
                    new NpgsqlParameter("@ActionType", ActionType ?? (object)DBNull.Value),
                    new NpgsqlParameter("@ActivityType", (object)ActivityType ?? DBNull.Value), // Correct for integer
                    new NpgsqlParameter("@UserName", UserName ?? (object)DBNull.Value),
                    new NpgsqlParameter("@AgentReqId", AgentReqId ?? (object)DBNull.Value)
                };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "public.sp_activatedeactivateagents"; // Call the converted PostgreSQL procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);

                        // Open the connection
                        sqlConn.Open();
                        DataSet ds = new DataSet();
                        // Use NpgsqlDataAdapter to fill the DataSet
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(ds); // Fill the DataSet with results from the temporary table
                        }

                        // Close the connection
                        sqlConn.Close();

                        return ds; // Return the DataSet containing results
                    }
                }
            }
            catch (Exception Ex)
            {
                // Log the error and rethrow it
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ActiveDeactiveAgent: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        #endregion

        #region ChangeAgentStatusReEdit
        public DataSet ChangeAgentStatusReEdit()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Step 1: Execute the stored procedure to update data
                    using (var cmd = new NpgsqlCommand("CALL public.sp_reeditagentdetails(:p_AgReqid, :p_UserName)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("p_AgReqid", AgentReqId);
                        cmd.Parameters.AddWithValue("p_UserName", UserName);

                        cmd.ExecuteNonQuery(); // Execute the stored procedure
                    }

                    // Step 2: Retrieve results from the temporary table
                    DataSet ds = new DataSet();
                    using (var resultCmd = new NpgsqlCommand("SELECT * FROM temp_status", sqlConn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(resultCmd))
                        {
                            DataTable dt = new DataTable("ResultsFromTempTable");
                            dataAdapter.Fill(dt); // Fill the DataTable with the results
                            ds.Tables.Add(dt);
                        }
                    }

                    return ds; // Return the DataSet with results
                }
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentStatusReEdit: AgentReqId: " + AgentReqId + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw; // Re-throw the exception after logging
            }
        }




        #endregion


        #region GetAgentDocumentByID
        public DataSet GetAgentDocumentByID()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                         {
                             new SqlParameter("@Mode", Mode),
                             new SqlParameter("@ID", DocumentID)
                         };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_GetAgentDocuments";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetAgentDocumentByID: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region BindAgentddl
        public DataSet BindAgentddl()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {
                                                 new SqlParameter("@UserName", UserName),
                                                 new SqlParameter("@IsVerified", VerificationStatus),
                                                 new SqlParameter("@IsActive", IsActive),
                                                 new SqlParameter("@IsRemoved", IsRemoved),
                                                 new SqlParameter("@BCCode", BCCode),
                                                 new SqlParameter("@ClientID", ClientId),
                                                 new SqlParameter("@IsDocUploaded", IsdocUploaded),
                                                 new SqlParameter("@Flag", Flag),
                                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindAgent";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: BindAgentddl: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region ChangeAgentStatus
        public DataSet ChangeBlockAgentOnboardStatus()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {
                                            new SqlParameter("@Remarks",BcRemarks),
                                            new SqlParameter("@UserName",UserName),
                                            new SqlParameter("@Bcstatus",BCstatus),
                                            new SqlParameter("@Mstatus",Mstatus),
                                            new SqlParameter("@CHStatus",ChStatus),
                                            new SqlParameter("@Flag",Flag),
                                            new SqlParameter("@AgentCode",AgentCode),
                                            new SqlParameter("@AgentReqId",AgentReqId),
                                            new SqlParameter("@ATStatus",AtStatus),
                                            new SqlParameter("@ActivityType",ActivityType),
                                            new SqlParameter("@ParentRoleId",ParentRoleID),
                };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UpdateBlockAgent";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentOnboardStatus: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet ChangeAgentOnboardStatus()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {
                                            new SqlParameter("@Remarks",BcRemarks),
                                            new SqlParameter("@UserName",UserName),
                                            new SqlParameter("@Bcstatus",BCstatus),
                                            new SqlParameter("@Mstatus",Mstatus),
                                            new SqlParameter("@CHStatus",ChStatus),
                                            new SqlParameter("@Flag",Flag),
                                            new SqlParameter("@AgentCode",AgentCode),
                                            new SqlParameter("@AgentReqId",AgentReqId),
                                            new SqlParameter("@ATStatus",AtStatus),
                                             new SqlParameter("@ActivityType",ActivityType),
                };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UpdateAgentOnboardProcessHandler";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentOnboardStatus: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        #endregion

        #region AgentStatusReportGrid
        //public DataSet AgentStatusReportGrid()
        //{
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
        //            {
        //                SqlParameter[] _Params = {
        //                                             new SqlParameter("@VerificationLevel", VerificationLevel),
        //                                               new SqlParameter("@OperationType", OperationType),
        //                                               new SqlParameter("@Status", Status),
        //                                               new SqlParameter ("@BCStatus", BCstatus),
        //                                               new SqlParameter ("@MStatus", Mstatus),
        //                                               new SqlParameter ("@CHStatus", ChStatus),
        //                                               new SqlParameter("@Authstatus",AtStatus),
        //                                               new SqlParameter("@FromDate", Fromdate),
        //                                               new SqlParameter("@ToDate", Todate),
        //                                               new SqlParameter("@ActivityType",ActivityType),
        //                                               new SqlParameter("@User", UserName),
        //                                               new SqlParameter("@Flag", Flag),
        //                                        };
        //                cmd.Connection = sqlConn;
        //                cmd.CommandText = "SP_OverallAgentStatus";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddRange(_Params);
        //                DataSet ds = new DataSet();
        //                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //                dataAdapter.Fill(ds);
        //                cmd.Dispose();
        //                return ds;
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentOnboardStatus: UserName: " + UserName + " Exception: " + Ex.Message);
        //        ErrorLog.DBError(Ex);
        //        throw;
        //    }
        //}

        public DataSet AgentStatusReportGrid()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Execute the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL SP_OverallAgentStatus(" +
                        ":p_VerificationLevel, :p_OperationType, :p_Status, :p_BCStatus, :p_MStatus, " +
                        ":p_CHStatus, :p_Authstatus, :p_FromDate, :p_ToDate, :p_ActivityType, :p_User, :p_Flag)", sqlConn))
                    {
                        // Define parameters with 'p_' prefix
                        cmd.Parameters.AddWithValue("p_VerificationLevel", VerificationLevel ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_OperationType", OperationType ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Status", Status);
                        cmd.Parameters.AddWithValue("p_BCStatus", BCstatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_MStatus", Mstatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_CHStatus", ChStatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Authstatus", AtStatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_FromDate", Fromdate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ToDate", Todate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ActivityType", ActivityType);
                        cmd.Parameters.AddWithValue("p_User", UserName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);

                        cmd.ExecuteNonQuery(); // Execute the stored procedure
                    }

                    // Now retrieve results from the temporary table
                    DataSet ds = new DataSet();
                    using (var resultCmd = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(resultCmd))
                        {
                            DataTable dt = new DataTable("ResultsFromTempTable");
                            dataAdapter.Fill(dt); // Fill the DataTable with the results
                            ds.Tables.Add(dt);
                        }
                    }

                    return ds; // Return the DataSet with results
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: AgentStatusReportGrid: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw; // Re-throw the exception after logging
            }
        }


        #endregion

        #region GetAgentDetailsForRegistration
        public DataSet GetAgentDetailForRegistration()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                            {
                             };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_OnboardedBC";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetAgentDetailForRegistration: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region SetInsertUpdateAgentRequestHadlerDetails
        public DataSet SetInsertUpdateAgentRequestHadlerDetails()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (var cmd = new NpgsqlCommand("CALL sp_InsertOrUpdateAgentHandlerDetails(@p_CreatedBy, @p_ActivityType, @p_Flag, @p_agReqId)", sqlConn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.Parameters.AddWithValue("p_CreatedBy", CreatedBy);
                        cmd.Parameters.AddWithValue("p_ActivityType", string.IsNullOrEmpty(Activity) ? (object)DBNull.Value : Activity);
                        cmd.Parameters.AddWithValue("p_agReqId", AgentCode);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Prepare to retrieve status messages
                        DataSet ds = new DataSet();
                        using (var messageCmd = new NpgsqlCommand("SELECT * FROM temp_StatusMessages", sqlConn))
                        {
                            using (var dataAdapter = new NpgsqlDataAdapter(messageCmd))
                            {
                                DataTable dt = new DataTable("StatusMessages");
                                dataAdapter.Fill(dt); // Fill the DataTable with the results
                                ds.Tables.Add(dt);
                            }
                        }

                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: SetInsertUpdateAgentRequestHandlerDetails: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        #endregion

        #region GetAgentDetails
        public DataSet GetAgentDetails()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    using (var cmd = new NpgsqlCommand("CALL public.proc_bindagentdata(:p_agentrequest)", sqlConn))
                    {
                        cmd.Parameters.AddWithValue("p_agentrequest", AgentReqId ?? (object)DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                    DataSet ds = new DataSet();
                    using (var messageCmd = new NpgsqlCommand("SELECT * FROM temp_agent_data", sqlConn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(messageCmd))
                        {
                            DataTable dt = new DataTable("AgentData");
                            dataAdapter.Fill(dt); // Fill the DataTable with the results
                            ds.Tables.Add(dt);
                        }
                    }
                    return ds; // Return the DataSet with agent data
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetAgentDetailForRegistration: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        #endregion

        #region GetDocs
        public DataSet GetDocs()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is open

                    // Create temporary table
                    using (var createTableCommand = new NpgsqlCommand(@"
                CREATE TEMP TABLE temp_results (
                    IdentityProofType character varying,
                    IdentityProofDocument character varying,
                    AddressProofType character varying,
                    AddressProofDocument character varying,
                    SignatureProofType character varying,
                    SignatureProofDocument character varying
                )", sqlConn))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }

                    // Call the stored procedure
                    using (var npgsqlCommand = new NpgsqlCommand("CALL public.sp_get_documents(@p_bcreqid)", sqlConn))
                    {
                        npgsqlCommand.Parameters.AddWithValue("p_bcreqid", AgentReqId);
                        npgsqlCommand.ExecuteNonQuery();
                    }

                    // Now select from the temporary table
                    var ds = new DataSet();
                    using (var selectCommand = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(selectCommand))
                        {
                            dataAdapter.Fill(ds);
                        }
                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("BCEntity: GetDocs: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }
        #endregion

        #region Validate Pan Details
        public bool ValidatePanDetails(out string Status, out string StatusMsg)
        {
            Status = "-1";
            StatusMsg = string.Empty;

            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("CALL sp_AgentOnboarding_ValidatePan(@p_PanNo, @p_BCCode, NULL,NULL)", sqlConn))
                    {
                        cmd.Parameters.AddWithValue("p_PanNo", PanNo);
                        cmd.Parameters.AddWithValue("p_BCCode", BCCode);


                        var outputStatus = new NpgsqlParameter("p_Status", NpgsqlTypes.NpgsqlDbType.Text)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputStatus);

                        var outputStatusMsg = new NpgsqlParameter("p_StatusMsg", NpgsqlTypes.NpgsqlDbType.Text)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputStatusMsg);

                        sqlConn.Open();
                        cmd.ExecuteNonQuery(); // Call the procedure

                        Status = Convert.ToInt32(outputStatus.Value).ToString();
                        StatusMsg = Convert.ToString(outputStatusMsg.Value);

                    }
                }

                return Status == "0"; // Return true if status is "00"
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ValidatePanDetails: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        #endregion

        #region ReEdit Validate Agent
        //public string ValidateEditBcDetails(out string _Requestid)
        //{
        //    string _Status = null;
        //    string _StatusMsg = null;
        //    _Requestid = string.Empty;
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
        //            {
        //                SqlParameter[] _Params =
        //                {
        //                      new SqlParameter("@AgentReqId",AgentReqId),
        //                      new SqlParameter("@Flag",Flag),
        //                      new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
        //                       new SqlParameter("@RequestId", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
        //                       new SqlParameter("@StatusMsg", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output }
        //                };
        //                cmd.Connection = sqlConn;
        //                cmd.CommandText = "sp_AgentRequest_Validation_GetValidate";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddRange(_Params);
        //                DataSet dataSet = new DataSet();
        //                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //                dataAdapter.Fill(dataSet);
        //                _Status = Convert.ToString(cmd.Parameters["@Status"].Value);
        //                _StatusMsg = Convert.ToString(cmd.Parameters["@StatusMsg"].Value);
        //                _Requestid = Convert.ToString(cmd.Parameters["@RequestId"].Value);
        //                cmd.Dispose();
        //                return _Status;
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("Class : AgentRegistrationDAL.cs \nFunction : EditValidate() \nException Occured\n" + Ex.Message);
        //        ErrorLog.DBError(Ex);
        //        throw;
        //    }
        //}
        public string ValidateEditBcDetails(out string _Requestid)
        {
            string _Status = null;
            string _StatusMsg = null;
            _Requestid = string.Empty;

            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    using (var cmd = new NpgsqlCommand("CALL public.sp_agentrequest_validation_getvalidate(null,null,null,@p_agentreqid, @p_flag)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Adding parameters with correct types
                        cmd.Parameters.Add("p_status", NpgsqlTypes.NpgsqlDbType.Varchar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_statusmsg", NpgsqlTypes.NpgsqlDbType.Varchar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_requestid", NpgsqlTypes.NpgsqlDbType.Varchar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("p_agentreqid", (object)AgentReqId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_flag", Flag);

                        // Adding output parameters


                        // Execute the command
                        cmd.ExecuteNonQuery();

                        // Retrieve output parameters
                        _Status = cmd.Parameters["p_status"].Value?.ToString();
                        _StatusMsg = cmd.Parameters["p_statusmsg"].Value?.ToString();
                        _Requestid = cmd.Parameters["p_requestid"].Value?.ToString();

                        return _Status;
                    }
                }
            }
            catch (Exception Ex)
            {
                // Log your exception as needed
                ErrorLog.CommonTrace($"Class : BCEntity.cs \nFunction : ValidateEditBcDetails() \nException Occurred\n{Ex.Message}");
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region ReEdit Validate BC
        public string DeleteBcDetails()
        {
            string _Status = null;
            string _StatusMsg = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                              new SqlParameter("@AgentReqId",AgentReqId),
                              new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_AgentRequest_Delete";
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
                ErrorLog.AgentManagementTrace("Class : AgentRegistrationDAL.cs \nFunction : EditValidate() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetAgentDocuments
        public DataSet GetAgentResponse()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@AgentReqID", AgentReqId)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_GetAgentResponse";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentStatus: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region ChangeAgentStatus
        public DataSet ChangeAgentStatusBulk()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@AgentCode",AgentID),
                                            new SqlParameter("@ActionType",ActionType),
                                            //new SqlParameter("@ActivityType",Flag),
                                            new SqlParameter("@UserName",UserName),
                                            //new SqlParameter("@Flag",Flag),
                                            new SqlParameter("@BcID",BCCode),
                                            new SqlParameter("@AgentReqId",AgentReqId),
                                            new SqlParameter("@MRemarks",MakerRemark),
                                            new SqlParameter("@BCStatus",BCstatus),
                                            new SqlParameter("@BCRemarks",BcRemarks),
                                            new SqlParameter("@MStatus",Mstatus),
                                            new SqlParameter("@CHRemarks",CheckerRemark),
                                            new SqlParameter("@CHStatus",ChStatus),
                                            new SqlParameter("@ATRemarks",ATRemark),
                                            new SqlParameter("@ATStatus",AtStatus),
                                            new SqlParameter("@FileID",FileID)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UpdateBulkAgOnboardProcess";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentStatus: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        public DataTable GetFileDetails()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                    new SqlParameter("@UserName", UserName),

                                    new SqlParameter("@BCstatus", BCstatus),
                                    new SqlParameter("@Makerstatus", Mstatus),
                                    new SqlParameter("@Checkstatus", ChStatus),
                                    new SqlParameter("@Authstatus", AtStatus),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindFileDetails";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataTable dataTable = new DataTable();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataTable);
                        cmd.Dispose();
                        return dataTable;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetFileDetails: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataTable GetFileData()
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // First, call the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL SP_BindFileData(@p_FileID)", sqlConn))
                    {
                        // Add the parameter
                        cmd.Parameters.AddWithValue("p_FileID", FileID);
                        cmd.ExecuteNonQuery(); // Execute the procedure
                    }

                    // Now, select from the temp table
                    using (var cmd = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataTable); // Fill the DataTable with the temp table data
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetFileDetails: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }

            return dataTable;
        }


        public DataSet GetCountryStateCityD()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    // Prepare to call the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL sp_getstatecity(:p_Mode, :p_CountryID, :p_StateID, :p_PinCode, :p_City, :p_District)", sqlConn))
                    {
                        cmd.Parameters.Add("p_Mode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = mode == 0 ? (object)DBNull.Value : mode.ToString(); // Changed to string
                        cmd.Parameters.Add("p_CountryID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(AgentCountry) ? (object)DBNull.Value : AgentCountry;
                        cmd.Parameters.Add("p_PinCode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(AgentPincode) ? (object)DBNull.Value : AgentPincode;
                        cmd.Parameters.Add("p_StateID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(AgentState) ? (object)DBNull.Value : AgentState;
                        cmd.Parameters.Add("p_City", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(AgentCity) ? (object)DBNull.Value : AgentCity;
                        cmd.Parameters.Add("p_District", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(AgentDistrict) ? (object)DBNull.Value : AgentDistrict;

                        sqlConn.Open();

                        // Call the procedure
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        // Now, select from the appropriate temporary table based on the mode
                        DataSet dataSet = new DataSet();

                        if (mode == 7) // Country
                        {
                            using (var countryCmd = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn))
                            using (var countryAdapter = new NpgsqlDataAdapter(countryCmd))
                            {
                                countryAdapter.Fill(dataSet, "Countries");
                            }
                        }
                        else if (mode == 8) // All
                        {
                            using (var countryCmd = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn))
                            using (var countryAdapter = new NpgsqlDataAdapter(countryCmd))
                            {
                                countryAdapter.Fill(dataSet, "Countries");
                            }

                            using (var statesCmd = new NpgsqlCommand("SELECT * FROM temp_states", sqlConn))
                            using (var statesAdapter = new NpgsqlDataAdapter(statesCmd))
                            {
                                statesAdapter.Fill(dataSet, "States");
                            }

                            using (var districtsCmd = new NpgsqlCommand("SELECT * FROM temp_districts", sqlConn))
                            using (var districtsAdapter = new NpgsqlDataAdapter(districtsCmd))
                            {
                                districtsAdapter.Fill(dataSet, "Districts");
                            }

                            using (var citiesCmd = new NpgsqlCommand("SELECT * FROM temp_cities", sqlConn))
                            using (var citiesAdapter = new NpgsqlDataAdapter(citiesCmd))
                            {
                                citiesAdapter.Fill(dataSet, "Cities");
                            }
                        }
                        else if (mode == 9) // District
                        {
                            using (var districtsCmd = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn)) // Assuming districts are stored in temp_results for this mode
                            using (var districtsAdapter = new NpgsqlDataAdapter(districtsCmd))
                            {
                                districtsAdapter.Fill(dataSet, "Districts");
                            }
                        }
                        else if (mode == 3) // City
                        {
                            using (var citiesCmd = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn)) // Assuming cities are stored in temp_results for this mode
                            using (var citiesAdapter = new NpgsqlDataAdapter(citiesCmd))
                            {
                                citiesAdapter.Fill(dataSet, "Cities");
                            }
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.writeLogEmailError($"Class: ClientRegistrationEntity.cs \nFunction: GetCountryStateCityD() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;  // Rethrow to allow further handling if necessary
            }
        }
        public DataSet GetShopCountryStateCity()
        {
            //try
            //{
            //    using (var sqlConn = new NpgsqlConnection(ConnectionString))
            //    {
            //        // Prepare to call the stored procedure
            //        using (var cmd = new NpgsqlCommand("CALL SP_GetStateCity(:p_Mode, :p_CountryID, :p_StateID, :p_PinCode, :p_City, :p_District)", sqlConn))
            //        {
            //            cmd.Parameters.Add("p_Mode", NpgsqlTypes.NpgsqlDbType.Integer).Value = mode == 0 ? (object)DBNull.Value : mode; // Change to Integer
            //            cmd.Parameters.Add("p_CountryID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ShopCountry) ? (object)DBNull.Value : ShopCountry;
            //            cmd.Parameters.Add("p_PinCode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ShopPinCode) ? (object)DBNull.Value : ShopPinCode;
            //            cmd.Parameters.Add("p_StateID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ShopState) ? (object)DBNull.Value : ShopState;
            //            cmd.Parameters.Add("p_City", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ShopCity) ? (object)DBNull.Value : ShopCity;
            //            cmd.Parameters.Add("p_District", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ShopDistrict) ? (object)DBNull.Value : ShopDistrict;

            //            sqlConn.Open();

            //            // Call the procedure
            //            cmd.ExecuteNonQuery(); // Since we are executing a procedure, we use ExecuteNonQuery

            //            // Now, select from the temporary tables to fill the DataSet
            //            DataSet dataSet = new DataSet();

            //            // Example: Get country data
            //            using (var countryCmd = new NpgsqlCommand("SELECT * FROM temp_country", sqlConn))
            //            using (var countryAdapter = new NpgsqlDataAdapter(countryCmd))
            //            {
            //                countryAdapter.Fill(dataSet, "Countries");
            //            }

            //            // Get states data
            //            using (var statesCmd = new NpgsqlCommand("SELECT * FROM temp_states", sqlConn))
            //            using (var statesAdapter = new NpgsqlDataAdapter(statesCmd))
            //            {
            //                statesAdapter.Fill(dataSet, "States");
            //            }

            //            // Get districts data
            //            using (var districtsCmd = new NpgsqlCommand("SELECT * FROM temp_districts", sqlConn))
            //            using (var districtsAdapter = new NpgsqlDataAdapter(districtsCmd))
            //            {
            //                districtsAdapter.Fill(dataSet, "Districts");
            //            }

            //            // Get cities data
            //            using (var citiesCmd = new NpgsqlCommand("SELECT * FROM temp_cities", sqlConn))
            //            using (var citiesAdapter = new NpgsqlDataAdapter(citiesCmd))
            //            {
            //                citiesAdapter.Fill(dataSet, "Cities");
            //            }

            //            return dataSet;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ErrorLog.writeLogEmailError($"Class: ClientRegistrationEntity.cs \nFunction: GetCountryStateCityD() \nException Occurred\n{ex.Message}");
            //    ErrorLog.DBError(ex);
            //    throw;  // Rethrow to allow further handling if necessary
            //}
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    // Prepare to call the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL sp_getstatecity(:p_Mode, :p_CountryID, :p_StateID, :p_PinCode, :p_City, :p_District)", sqlConn))
                    {
                        cmd.Parameters.Add("p_Mode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = mode == 0 ? (object)DBNull.Value : mode.ToString(); // Changed to string
                        cmd.Parameters.Add("p_CountryID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ShopCountry) ? (object)DBNull.Value : ShopCountry;
                        cmd.Parameters.Add("p_PinCode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ShopPinCode) ? (object)DBNull.Value : ShopPinCode;
                        cmd.Parameters.Add("p_StateID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ShopState) ? (object)DBNull.Value : ShopState;
                        cmd.Parameters.Add("p_City", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ShopCity) ? (object)DBNull.Value : ShopCity;
                        cmd.Parameters.Add("p_District", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(ShopDistrict) ? (object)DBNull.Value : ShopDistrict;

                        sqlConn.Open();

                        // Call the procedure
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        // Now, select from the appropriate temporary table based on the mode
                        DataSet dataSet = new DataSet();

                        if (mode == 7) // Country
                        {
                            using (var countryCmd = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn))
                            using (var countryAdapter = new NpgsqlDataAdapter(countryCmd))
                            {
                                countryAdapter.Fill(dataSet, "Countries");
                            }
                        }
                        else if (mode == 8) // All
                        {
                            using (var countryCmd = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn))
                            using (var countryAdapter = new NpgsqlDataAdapter(countryCmd))
                            {
                                countryAdapter.Fill(dataSet, "Countries");
                            }

                            using (var statesCmd = new NpgsqlCommand("SELECT * FROM temp_states", sqlConn))
                            using (var statesAdapter = new NpgsqlDataAdapter(statesCmd))
                            {
                                statesAdapter.Fill(dataSet, "States");
                            }

                            using (var districtsCmd = new NpgsqlCommand("SELECT * FROM temp_districts", sqlConn))
                            using (var districtsAdapter = new NpgsqlDataAdapter(districtsCmd))
                            {
                                districtsAdapter.Fill(dataSet, "Districts");
                            }

                            using (var citiesCmd = new NpgsqlCommand("SELECT * FROM temp_cities", sqlConn))
                            using (var citiesAdapter = new NpgsqlDataAdapter(citiesCmd))
                            {
                                citiesAdapter.Fill(dataSet, "Cities");
                            }
                        }
                        else if (mode == 9) // District
                        {
                            using (var districtsCmd = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn)) // Assuming districts are stored in temp_results for this mode
                            using (var districtsAdapter = new NpgsqlDataAdapter(districtsCmd))
                            {
                                districtsAdapter.Fill(dataSet, "Districts");
                            }
                        }
                        else if (mode == 3) // City
                        {
                            using (var citiesCmd = new NpgsqlCommand("SELECT * FROM temp_results", sqlConn)) // Assuming cities are stored in temp_results for this mode
                            using (var citiesAdapter = new NpgsqlDataAdapter(citiesCmd))
                            {
                                citiesAdapter.Fill(dataSet, "Cities");
                            }
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.writeLogEmailError($"Class: ClientRegistrationEntity.cs \nFunction: GetCountryStateCityD() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;  // Rethrow to allow further handling if necessary
            }
        }

        #region GetAgentDocuments
        public DataSet GetAgentResponseNew()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@AgentReqID", AgentReqId),
                            new SqlParameter("@flag", Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "InsertOrUpdateNameScreeing";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentStatus: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        public DataSet ChangeBlockAgentStatus()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {

                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@AgentCode",AgentCode),
                                            new SqlParameter("@ActionType",ActionType),
                                            new SqlParameter("@ActivityType",ActivityType),
                                            new SqlParameter("@UserName",UserName),
                                            new SqlParameter("@Flag",Flag),
                                            new SqlParameter("@BcID",BCCode),
                                            new SqlParameter("@AgentReqId",AgentReqId),
                                            new SqlParameter("@BCRemarks",BcRemarks),
                                            new SqlParameter("@BCStatus",BCstatus),
                                            new SqlParameter("@MRemarks",MakerRemark),
                                            new SqlParameter("@MStatus",Mstatus),
                                            new SqlParameter("@CHRemarks",CheckerRemark),
                                            new SqlParameter("@CHStatus",ChStatus),
                                            new SqlParameter("@ATRemarks",ATRemark),
                                            new SqlParameter("@ATStatus",AtStatus)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UpdateBlockAgentOnboardProcess";//SP_UpdateBlockAgent
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentStatus: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #region ChangeAggregatorStatusReEdit
        public DataSet ChangeAggregatorStatusReEdit()
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        NpgsqlParameter[] _Params =
                        {
                            new NpgsqlParameter("@AgReqid", AgentReqId),
                            new NpgsqlParameter("@UserName", UserName)
                        };

                        cmd.Connection = sqlConn;
                        cmd.CommandText = "sp_reedit_aggregator_details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);

                        DataSet ds = new DataSet();

                        sqlConn.Open();

                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(ds); // Fill the DataSet with the result
                        }

                        cmd.Dispose();
                        return ds; // Return the DataSet
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: SetInsertUpdateAgentMasterDetails: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion
        #region ChangeAgentStatusReEdit
        //public DataSet ChangeAgentStatusReEdit()
        //{
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
        //            {
        //                SqlParameter[] _Params =
        //                {
        //                                            new SqlParameter("@AgReqid",AgentReqId),
        //                };
        //                cmd.Connection = sqlConn;
        //                cmd.CommandText = "SP_ReeditAgentDetails";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddRange(_Params);
        //                DataSet ds = new DataSet();
        //                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //                dataAdapter.Fill(ds);
        //                cmd.Dispose();
        //                return ds;
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("AgentRegistrationDAL: SetInsertUpdateAgentMasterDetails: UserName: " + UserName + " Exception: " + Ex.Message);
        //        ErrorLog.DBError(Ex);
        //        throw;
        //    }
        //}
        #endregion
        public DataSet InheritServicesFromParent()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Call the procedure with parameters
                    using (var cmd = new NpgsqlCommand("CALL SP_GetServicesByFranchiseID(@p_FranchiseID, @p_UserName, @p_Flag)", sqlConn))
                    {
                        // Add parameters with the correct names
                        cmd.Parameters.AddWithValue("p_FranchiseID", AggCode);
                        cmd.Parameters.AddWithValue("p_UserName", UserName);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);

                        cmd.ExecuteNonQuery(); // Execute the procedure
                    }

                    // Retrieve data from the temporary table
                    using (var cmd = new NpgsqlCommand("SELECT * FROM TempServiceResults", sqlConn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet); // Fill the DataSet with the results
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: SetInsertUpdateAgentMasterDetails: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;

            }
            return dataSet;
        }
        #region Reprocess
        
        public DataSet GetReprocessData()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (var cmd = new NpgsqlCommand("CALL SP_GetReprocessData(@p_username, @p_agentid)", sqlConn))
                    {
                        cmd.Parameters.AddWithValue("p_username", UserName);
                        cmd.Parameters.AddWithValue("p_agentid", AgentReqId);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new NpgsqlCommand("SELECT * FROM TempServiceResults", sqlConn))
                    {
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: SetInsertUpdateAgentMasterDetails: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;

            }
            return dataSet;
        }
        #endregion
    }
}