using System;
using AppLogger;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Collections.Generic;
using static BussinessAccessLayer.EnumCollection;
using NpgsqlTypes;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Drawing;
using System.Net.NetworkInformation;

namespace BussinessAccessLayer
{
    public class BCEntity
    {
        #region Objects Declaration
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        DataSet dataSet = null;
        #endregion

        #region Property Declaration
        public string _Status;
        public string _StatusMsg;
        public string BCRequest { get; set; }
        public string UserName { get; set; }
        public string _fileLineNo { get; set; }
        public string Mode { get; set; }
        public string FirstName { get; set; }
        public string AgentId { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PersonalEmail { get; set; }
        public string AadharNo { get; set; }
        public string PanNo { get; set; }

        public string RequestTypeId { get; set; }
        public string GSTNo { get; set; }
        public string RegisteredAddress { get; set; }
        public string Country { get; set; }
        public string BC_Code { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string BusinessEmail { get; set; }
        public string PersonalContact { get; set; }
        public string LandlineContact { get; set; }
        public byte[] ProfileImage { get; set; }
        public string CreatedBy { get; set; }
        public int DocumentID { get; set; }
        public byte[] DocData { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string MakerRemark { get; set; }
        public string IdCardType { get; set; }
        public string IdCardValue { get; set; }
        public string IFSCCode { get; set; }
        public string District { get; set; }
        public string LocalAddress { get; set; }
        public int LocalCountry { get; set; }
        public int LocalState { get; set; }
        public int LocalCity { get; set; }
        public string AccountNumber { get; set; }
        public string LocalDistrict { get; set; }
        public string LocalPincode { get; set; }
        public string AlternetNo { get; set; }
        public string ShopAddress { get; set; }
        public int ShopCountry { get; set; }
        public int ShopState { get; set; }
        public string ShopDistrict { get; set; }
        public int ShopCity { get; set; }
        public string ShopPincode { get; set; }
        public string IdentityProofType { get; set; }
        public string IdentityProofDocument { get; set; }
        public string AddressProofType { get; set; }
        public string AddressProofDocument { get; set; }
        public string SignatureProofType { get; set; }
        public string SignatureProofDocument { get; set; }
        public int IsValidaddress { get; set; }
        public int IsValidProof { get; set; }
        public int RoleID { get; set; }
        public int? ForAEPS { get; set; }
        public string Gender { get; set; }
        public string FatherName { get; set; }
        public string Category { get; set; }
        public string Education { get; set; }
        public string Clientcode { get; set; }
        public int? ForMicroATM { get; set; }
        public string ModifiedBy { get; set; }
        //public string BCFranchiseID { get; set; }
        public string BCID { get; set; }
        public string BCCode { get; set; }
        public string bccode { get; set; }
        public int Status { get; set; }
        public string CheckerRemark { get; set; }
        public int Flag { get; set; }
        public int IsAPIEnable { get; set; }
        public string Password { get; set; }
        public string _RandomStringForSalt { get; set; }
        public string VerificationStatus { get; set; }
        public string IsCheckDecline { get; set; }
        public string IsAuthDecline { get; set; }
        public string IsActive { get; set; }
        public string IsdocUploaded { get; set; }
        public int ActionType { get; set; }
        public string IsRemoved { get; set; }
        public string IsRecycle { get; set; }
        public string BcCategory { get; set; }
        public string ClientId { get; set; }
        public string TypeOfOrg { get; set; }
        public string BCReqId { get; set; }
        public string CHstatus { get; set; }
        public string ATStatus { get; set; }
        public string ATRemark { get; set; }
        public string Activity { get; set; }
        public int ActivityType { get; set; }
        public string Mstatus { get; set; }
        public string BCStatus { get; set; }
        public string VerificationLevel { get; set; }
        public string OperationType { get; set; }
        public int mode { get; set; }
        #endregion;

        #region getCountryAndDocument
        public DataSet getCountryAndDocument()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@Mode", Mode)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_MasterDetails";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : getAllAgentData() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Get State  And City
        public DataSet getStateAndCity()
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
                             new SqlParameter("@CountryID", Country),
                             new SqlParameter("@StateID", State)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_MasterDetails";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : getStateAndCity() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region getReceiptData
        public DataSet getReceiptData()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Call the procedure with the correct parameter
                    using (var cmd = new NpgsqlCommand("CALL sp_getReceiptData(@p_bccode)", sqlConn))
                    {
                        // Use the correct parameter name with the @ prefix
                        cmd.Parameters.AddWithValue("p_bccode", BCCode);
                        cmd.ExecuteNonQuery(); // Execute the procedure
                    }

                    // Now retrieve the data from the temporary table
                    using (var cmd = new NpgsqlCommand("SELECT * FROM temp_receipt_data", sqlConn))
                    {
                        DataSet dataSet = new DataSet();
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet); // Fill the DataSet with the results
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: BCEntity.cs \nFunction: GetReceiptData() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        #endregion

        #region EditValidate
        public string EditValidate()
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
                              new SqlParameter("@BCID ",BCReqId),
                              new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_EditValidate";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : EditValidate() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region SetInsertUpdateBCDetails
        public DataSet SetInsertUpdateBCDetails()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@Flag", Flag),
                            new SqlParameter("@ClientId", ClientId),
                            new SqlParameter("@MasterID",BCReqId),
                            new SqlParameter("@BcReqId", BC_Code),
                            new SqlParameter("@CreatedBy", CreatedBy),
                            new SqlParameter("@FirstName", FirstName),
                            new SqlParameter("@MiddleName", MiddleName),
                            new SqlParameter("@LastName", LastName),
                            new SqlParameter("@BcCategory", BcCategory),
                            new SqlParameter("@BcAddress", RegisteredAddress),
                            new SqlParameter("@Pincode", Pincode),
                            new SqlParameter("@TypeOfOrg",TypeOfOrg ),
                            new SqlParameter("@AccountName","" ),
                            new SqlParameter("@AccountNumber",AccountNumber ),
                            new SqlParameter("@IFSCCode",IFSCCode ),
                            new SqlParameter("@Bank","" ),
                            new SqlParameter("@AEPS",ForAEPS ),
                            new SqlParameter("@MATM",ForMicroATM ),
                            new SqlParameter("@Gender",Gender ),
                            new SqlParameter("@AadharNo",AadharNo ),
                            new SqlParameter("@PanNo",PanNo ),
                            new SqlParameter("@GSTNo",GSTNo ),
                            new SqlParameter("@EmailID",PersonalEmail ),
                            new SqlParameter("@ContactNo",PersonalContact ),
                            new SqlParameter("@LandlineNo",LandlineContact ),
                            new SqlParameter("@AlternateNo",AlternetNo ),
                            new SqlParameter("@Country",Country ),
                            new SqlParameter("@State",State ),
                            new SqlParameter("@City",City ),
                            new SqlParameter("@District",District ),
                            new SqlParameter("@IdentityProofType",IdentityProofType ),
                            new SqlParameter("@IdentityProofDocument",IdentityProofDocument),
                            new SqlParameter("@AddressProofType",AddressProofType ),
                            new SqlParameter("@AddressProofDocument",AddressProofDocument),
                            new SqlParameter("@SignatureProofType",SignatureProofType ),
                            new SqlParameter("@SignatureProofDocument",SignatureProofDocument)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "sp_InsertOrUpdateBCDetails";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : SetInsertUpdateBCDetails() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Insert_BCRequest out string
        public bool Insert_BCRequest(string UserName, out int requestId, out string status, out string statusMsg)
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

                    using (var cmd = new NpgsqlCommand("CALL public.sp_bc_request_registration_insert(NULL,NULL,NULL, @p_clientid, @p_createdby, @p_firstname, @p_middlename, @p_lastname, @p_gender, @p_emailid, @p_contactno, @p_landlineno, @p_alternateno, @p_aadharno, @p_panno, @p_gstno, @p_bcaddress, @p_country, @p_state, @p_city, @p_pincode, @p_district, @p_typeoforg, @p_bccategory, @p_bccode, @p_accountname, @p_accountnumber, @p_ifsccode, @p_bank, @p_identityprooftype, @p_identityproofdocument, @p_addressprooftype, @p_addressproofdocument, @p_signatureprooftype, @p_signatureproofdocument, @p_bcreqid, @p_masterid, @p_aeps, @p_matm, @p_flag, @p_activitytype)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;
                        var outputRequestId = new NpgsqlParameter("p_reqid", NpgsqlTypes.NpgsqlDbType.Integer)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputRequestId);
                        var outputStatus = new NpgsqlParameter("p_status", NpgsqlTypes.NpgsqlDbType.Integer)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputStatus);

                        // Output parameter for Status Message
                        var outputStatusMsg = new NpgsqlParameter("p_statusmsg", NpgsqlTypes.NpgsqlDbType.Text)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputStatusMsg);

                        cmd.Parameters.AddWithValue("p_clientid", (object)ClientId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_createdby", (object)CreatedBy ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_firstname", (object)FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_middlename", (object)MiddleName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_lastname", (object)LastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_gender", (object)Gender ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_emailid", (object)PersonalEmail ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_contactno", (object)PersonalContact ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_landlineno", (object)LandlineContact ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_alternateno", (object)AlternetNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_aadharno", (object)AadharNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_panno", (object)PanNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_gstno", (object)GSTNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bcaddress", (object)RegisteredAddress ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_country", (object)Country ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_state", (object)State ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_city", (object)City ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_pincode", (object)Pincode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_district", (object)District ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_typeoforg", (object)TypeOfOrg ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bccategory", (object)BcCategory ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bccode", (object)BCCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_accountname", (object)string.Empty ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_accountnumber", (object)AccountNumber ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ifsccode", (object)IFSCCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bank", (object)string.Empty ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_identityprooftype", (object)IdentityProofType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_identityproofdocument", (object)IdentityProofDocument ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_addressprooftype", (object)AddressProofType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_addressproofdocument", (object)AddressProofDocument ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_signatureprooftype", (object)SignatureProofType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_signatureproofdocument", (object)SignatureProofDocument ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bcreqid", BCReqId);
                        cmd.Parameters.AddWithValue("p_masterid", (object)BCCode ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("p_aeps", ForAEPS);
                        //cmd.Parameters.AddWithValue("p_matm", ForMicroATM);
                        cmd.Parameters.AddWithValue("p_aeps", ForAEPS.HasValue ? ForAEPS.Value : 0);
                        cmd.Parameters.AddWithValue("p_matm", ForMicroATM.HasValue ? ForMicroATM.Value : 0);
                        cmd.Parameters.AddWithValue("p_flag", Flag);
                        cmd.Parameters.AddWithValue("p_activitytype", string.IsNullOrEmpty(Activity) ? "0" : Activity);


                        try
                        {
                            cmd.ExecuteNonQuery();
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
                        catch (PostgresException ex)
                        {
                            ErrorLog.CommonTrace($"Postgres Error: {ex.Message}");
                            return false;
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.CommonTrace($"General Error: {ex.Message}");
                            return false;
                        }

                        if (notices.Count > 0)
                        {
                            foreach (var notice in notices)
                            {
                                ErrorLog.CommonTrace($"NOTICE: {notice}");
                            }
                        }
                        else
                        {
                            ErrorLog.CommonTrace("No notices captured.");
                        }
                        return true; // Assuming success if no exceptions were thrown
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Error connecting to database: {ex.Message}");
                return false;
            }
        }
        
        #endregion

        #region validatreq
        public DataSet validatreq()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                              new SqlParameter("@BcReqId", BCReqId)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_ValidateBCOnboarding";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : validatreq() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Get Onborading BC Details
        public DataSet GetOnboradingbcDetails()
        {
            try
            {
                var result = new DataSet();

                // Create and open the connection
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Create the command for the stored procedure using CALL
                    using (var npgsqlCommand = new NpgsqlCommand("CALL Proc_BindOnBoarededBCData(:p_BCRequest);", connection))
                    {
                        // Add parameter for the request
                        npgsqlCommand.Parameters.AddWithValue("p_BCRequest", BCRequest);

                        // Execute the stored procedure
                        npgsqlCommand.ExecuteNonQuery();
                    }

                    // Now, retrieve data from the temporary table
                    using (var tempTableCommand = new NpgsqlCommand("SELECT * FROM temp_BC_Request;", connection))
                    {
                        using (var npgsqlDataAdapter = new NpgsqlDataAdapter(tempTableCommand))
                        {
                            npgsqlDataAdapter.Fill(result);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetOnboardingBcDetails() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }



        #endregion

        #region Get Pending BC Details
        public DataSet GetPendingbcDetails()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                             new SqlParameter("@BCRequest ", BCRequest)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_BindPendingBCData";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetOnboradingbcDetails() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Get BC Details For Registration
        public DataSet GetBCDetailsForRegistration()
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDetailsForRegistration() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetCityOnStateChange
        public DataSet GetCityOnStateChange()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@StateId",State)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_GetCityByStateId";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetCityOnStateChange() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetBCRequestList

        public DataSet GetBCRequestList()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Open the connection

                    // Execute the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL public.sp_bcrequest_registration_get(@P_BCCode, @P_StageId, @P_RequestTypeId, NULL, NULL, @P_PanNo, @P_AadharNo, @P_GSTNo, NULL, @P_ContactNo, @P_PersonalEmailID, @P_Makerstatus, @P_Checkstatus, @P_Authstatus, @P_Flag)", sqlConn))
                    {
                        // Add parameters with explicit types
                        cmd.Parameters.AddWithValue("P_BCCode", (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("P_StageId", 0);
                        cmd.Parameters.AddWithValue("P_RequestTypeId", (object)RequestTypeId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_PanNo", (object)PanNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_AadharNo", (object)AadharNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_GSTNo", (object)GSTNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_ContactNo", (object)PersonalContact ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_PersonalEmailID", (object)PersonalEmail ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Makerstatus", (object)Mstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Checkstatus", (object)CHstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Authstatus", (object)ATStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Flag", Flag); // Use the appropriate variable for Flag

                        cmd.ExecuteNonQuery(); // Execute the stored procedure
                    }

                    // Now retrieve the data from the temporary table
                    using (var dataAdapter = new NpgsqlDataAdapter("SELECT * FROM temp_results", sqlConn))
                    {
                        DataSet dataTable = new DataSet();
                        dataAdapter.Fill(dataTable); // Fill the DataSet with data from the temp table
                        return dataTable; // Return the populated DataSet
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("BCEntity: GetBCRequestList: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw; // Rethrow the exception for further handling
            }
        }


        #endregion

        #region BindClient  
        public DataSet BindClient()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                               new SqlParameter("@ClientID",ClientId),
                               new SqlParameter("@BCID",BCID),
                               new SqlParameter("@AgentID", AgentId),
                               new SqlParameter("@IsDocUploaded",IsdocUploaded),
                               new SqlParameter("@IsVerified", VerificationStatus),
                               new SqlParameter("@IsActive",IsActive),
                               new SqlParameter("@BankStatus",null),
                               new SqlParameter("@IsRemoved", IsRemoved),
                               new SqlParameter("@UserName",UserName),
                               //new SqlParameter("@Flag", Flag)                           
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindClient";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : BindClient() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region BindExport
        public DataSet BindExport()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                              new SqlParameter("@UserName",UserName),
                              //new SqlParameter("@Flag", Flag)
                              ///new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_LoadMasters_Export";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : BindExport() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetBCDetails
        public DataSet GetBCDetails()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                new SqlParameter("@RoleId", RoleID),
                                new SqlParameter("@BCID", BCID),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_BCDetails";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDetails() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region UpdateBCDetails
        public DataSet UpdateBCDetails()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                new SqlParameter("@ClientId", ClientId),
                                new SqlParameter("@CreatedBy", CreatedBy),
                                new SqlParameter("@FirstName", FirstName),
                                new SqlParameter("@MiddleName", MiddleName),
                                new SqlParameter("@LastName", LastName),
                                new SqlParameter("@BCCode", BC_Code),
                                new SqlParameter("@BcCategory", BcCategory),
                                new SqlParameter("@BcAddress", RegisteredAddress),
                                new SqlParameter("@Pincode", Pincode),
                                new SqlParameter("@TypeOfOrg",TypeOfOrg ),
                                new SqlParameter("@AccountName","" ),
                                new SqlParameter("@AccountNumber",AccountNumber ),
                                new SqlParameter("@IFSCCode",IFSCCode ),
                                new SqlParameter("@Bank","" ),
                                new SqlParameter("@AEPS",ForAEPS ),
                                new SqlParameter("@MATM",ForMicroATM ),
                                new SqlParameter("@Gender",Gender ),
                                new SqlParameter("@AadharNo",AadharNo ),
                                new SqlParameter("@PanNo",PanNo ),
                                new SqlParameter("@GSTNo",GSTNo ),
                                new SqlParameter("@EmailID",PersonalEmail ),
                                new SqlParameter("@ContactNo",PersonalContact ),
                                new SqlParameter("@LandlineNo",LandlineContact ),
                                new SqlParameter("@AlternateNo",AlternetNo ),
                                new SqlParameter("@Country",Country ),
                                new SqlParameter("@State",State ),
                                new SqlParameter("@City",City ),
                                new SqlParameter("@District",District ),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_BcEditDetails";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : UpdateBCDetails() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region SetInsertUpdateBCTrackerDetails
        public string SetInsertUpdateBCTrackerDetails()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (var cmd = new NpgsqlCommand("CALL sp_InsertOrUpdateBCtrackerDetails(null, @p_Flag, @p_CreatedBy, @p_BcReqId, @p_ActivityType)", sqlConn))
                    {
                        // Ensure parameters are added correctly
                        var statusParam = new NpgsqlParameter("v_status", NpgsqlTypes.NpgsqlDbType.Varchar)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(statusParam);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.Parameters.AddWithValue("p_CreatedBy", CreatedBy);
                        cmd.Parameters.AddWithValue("p_BcReqId", BCReqId); // Ensure this is an integer
                        cmd.Parameters.AddWithValue("p_ActivityType", Activity ?? (object)DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();
                        string statusMsg = statusParam.Value.ToString();
                        // Retrieve the status message
                        return statusMsg.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: BCEntity.cs \nFunction: SetInsertUpdateBCTrackerDetails() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }

        #endregion

        #region BCStatusReport
        public DataSet BCStatusReport()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                  new SqlParameter("@Checkstatus",CHstatus),
                                  new SqlParameter("@User", UserName),
                                  new SqlParameter("@Flag", Flag),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_GetBCDetailsToProcessOnboaring";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : BCStatusReport() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region BindStatusBC
        public DataSet BindStatusBC()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                              new SqlParameter("@ClientID",Clientcode),
                              new SqlParameter("@FranchiseID",BCID),
                              new SqlParameter("@IsDocUploaded",IsdocUploaded),
                              new SqlParameter("@IsVerified", VerificationStatus),
                              new SqlParameter("@IsActive",IsActive),
                              new SqlParameter("@IsRemoved", IsRemoved),
                              new SqlParameter("@UserName",UserName),
                              new SqlParameter("@Flag", Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_DropDownStatusFranchise";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : BindStatusBC() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region SaveTHePan
        public DataSet SaveTHePan()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                new SqlParameter("@BCID ",BCReqId),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "sp_GetPan";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : SaveTHePan() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Bind OnBoarded BC
        public DataSet BindOnBoardedBC()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                   new SqlParameter("@ClientID",Clientcode),
                                   new SqlParameter("@UserName",UserName),
                                   new SqlParameter("@Flag", Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "sp_GetBcDropDown";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : BindOnBoardedBC() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region BindBC ddl
        public DataSet BindBCddl()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Call the function and specify the expected columns
                        cmd.CommandText = "SELECT BCCode, BCName, Status FROM SP_BindBC(@p_ClientID, @p_IsDocUploaded, @p_IsVerified, @p_IsActive, @p_IsRemoved, @p_UserName)";
                        cmd.CommandType = CommandType.Text;

                        // Set parameters with null handling
                        cmd.Parameters.AddWithValue("p_ClientID", (object)Clientcode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_IsDocUploaded", (object)IsdocUploaded ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_IsVerified", (object)VerificationStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_IsActive", (object)IsActive ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_IsRemoved", (object)IsRemoved ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_UserName", (object)UserName ?? DBNull.Value);

                        // Execute the query and fill the DataSet
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            return dataSet;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : BindBCddl() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }



        #endregion

        //////// Verification L1 & L2 //////

        #region BindBC Verification
        public DataSet BindBCVerification()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand("SELECT * FROM fn_BindBCVerification(@ClientID, @UserName, @BCCode, @Checkstatus, @Authstatus, @Makerstatus, @IsRemoved, @Flag)", sqlConn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("ClientID", (object)Clientcode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("UserName", (object)UserName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("BCCode", (object)BCID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Checkstatus", (object)CHstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Authstatus", (object)ATStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Makerstatus", (object)Mstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("IsRemoved", (object)IsRemoved ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Flag", Flag); // Use the appropriate variable for Flag

                        sqlConn.Open();
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
                ErrorLog.CommonTrace($"Class : BCEntity.cs \nFunction : BindBCVerification() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }

        #endregion

        #region GetBCDocuments
        public DataSet GetBCDocuments()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is open

                    using (var cmd = new NpgsqlCommand("CALL public.sp_getbcdocuments(@p_mode, @p_bcid)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        // Adding parameters
                        cmd.Parameters.AddWithValue("p_mode", Mode);
                        cmd.Parameters.AddWithValue("p_bcid", BCReqId);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        // Prepare the SQL query for the appropriate temporary table based on the mode
                        string tempTableQuery;
                        if (Mode == "GetBCDetails")
                        {
                            tempTableQuery = "SELECT * FROM temp_bc_details"; // Query for GetBCDetails
                        }
                        else if (Mode == "GetBCDocumentByID")
                        {
                            tempTableQuery = "SELECT * FROM temp_bc_documents"; // Query for GetBCDocumentById
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid mode specified."); // Handle unexpected mode
                        }

                        // Now retrieve the data from the appropriate temporary table
                        using (var dataAdapter = new NpgsqlDataAdapter(tempTableQuery, sqlConn))
                        {
                            var dataSet = new DataSet();
                            dataAdapter.Fill(dataSet); // Fill the DataSet with data from the temp table
                            return dataSet; // Return the populated DataSet
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDocuments() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw; // Rethrow the exception for further handling
            }
        }


        public DataSet GetBCDocumentByID()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is open

                    using (var cmd = new NpgsqlCommand("CALL public.sp_getbcdocuments(@p_mode, @p_bcid)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        // Adding parameters
                        cmd.Parameters.AddWithValue("p_mode", Mode);
                        cmd.Parameters.AddWithValue("p_bcid", BCReqId);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        // Prepare the SQL query for the appropriate temporary table based on the mode
                        string tempTableQuery;
                        if (Mode == "GetBCDetails")
                        {
                            tempTableQuery = "SELECT * FROM temp_bc_details"; // Query for GetBCDetails
                        }
                        else if (Mode == "GetBCDocumentById")
                        {
                            tempTableQuery = "SELECT * FROM temp_bc_documents"; // Query for GetBCDocumentById
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid mode specified."); // Handle unexpected mode
                        }

                        // Now retrieve the data from the appropriate temporary table
                        using (var dataAdapter = new NpgsqlDataAdapter(tempTableQuery, sqlConn))
                        {
                            var dataSet = new DataSet();
                            dataAdapter.Fill(dataSet); // Fill the DataSet with data from the temp table
                            return dataSet; // Return the populated DataSet
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDocuments() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw; // Rethrow the exception for further handling
            }
        }
        #endregion


        #region GetAggDocuments
        public DataSet GetAggDocuments()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is open

                    using (var cmd = new NpgsqlCommand("CALL public.sp_getAggdocuments(@p_mode, @p_bcid)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        // Adding parameters
                        cmd.Parameters.AddWithValue("p_mode", Mode);
                        cmd.Parameters.AddWithValue("p_bcid", BCReqId);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        // Prepare the SQL query for the appropriate temporary table based on the mode
                        string tempTableQuery;
                        if (Mode == "GetBCDetails")
                        {
                            tempTableQuery = "SELECT * FROM temp_bc_details"; // Query for GetBCDetails
                        }
                        else if (Mode == "GetBCDocumentByID")
                        {
                            tempTableQuery = "SELECT * FROM temp_bc_documents"; // Query for GetBCDocumentById
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid mode specified."); // Handle unexpected mode
                        }

                        // Now retrieve the data from the appropriate temporary table
                        using (var dataAdapter = new NpgsqlDataAdapter(tempTableQuery, sqlConn))
                        {
                            var dataSet = new DataSet();
                            dataAdapter.Fill(dataSet); // Fill the DataSet with data from the temp table
                            return dataSet; // Return the populated DataSet
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDocuments() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw; // Rethrow the exception for further handling
            }
        }


        public DataSet GetAggDocumentByID()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is open

                    using (var cmd = new NpgsqlCommand("CALL public.sp_getAggdocuments(@p_mode, @p_bcid)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text; // Use CommandType.Text for CALL

                        // Adding parameters
                        cmd.Parameters.AddWithValue("p_mode", Mode);
                        cmd.Parameters.AddWithValue("p_bcid", BCReqId);

                        // Execute the procedure
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        // Prepare the SQL query for the appropriate temporary table based on the mode
                        string tempTableQuery;
                        if (Mode == "GetBCDetails")
                        {
                            tempTableQuery = "SELECT * FROM temp_bc_details"; // Query for GetBCDetails
                        }
                        else if (Mode == "GetBCDocumentById")
                        {
                            tempTableQuery = "SELECT * FROM temp_bc_documents"; // Query for GetBCDocumentById
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid mode specified."); // Handle unexpected mode
                        }

                        // Now retrieve the data from the appropriate temporary table
                        using (var dataAdapter = new NpgsqlDataAdapter(tempTableQuery, sqlConn))
                        {
                            var dataSet = new DataSet();
                            dataAdapter.Fill(dataSet); // Fill the DataSet with data from the temp table
                            return dataSet; // Return the populated DataSet
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDocuments() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw; // Rethrow the exception for further handling
            }
        }
        #endregion

        #region ChangeBCStatus
        public DataSet ChangeBCStatus()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Ensure the connection is open

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL SP_UpdateBCOnboardProcess(:p_BCCode, :p_BCID, :p_BCReqId, :p_Remarks, :p_ActionType, :p_UserName, :p_Flag, :p_ATStatus, :p_ATRemarks, :p_CHRemarks, :p_CHStatus, :p_CHUserId, :p_ActivityType)";
                        cmd.CommandType = CommandType.Text;

                        // Add parameters (ensure types match your procedure)
                        cmd.Parameters.AddWithValue("p_BCCode", string.IsNullOrEmpty(BCCode) ? (object)DBNull.Value : BCCode);
                        cmd.Parameters.AddWithValue("p_BCID", string.IsNullOrEmpty(BCID) ? (object)DBNull.Value : BCID);
                        cmd.Parameters.AddWithValue("p_BCReqId", BCReqId);
                        cmd.Parameters.AddWithValue("p_Remarks", string.IsNullOrEmpty(ATRemark) ? (object)DBNull.Value : ATRemark);
                        cmd.Parameters.AddWithValue("p_ActionType", ActionType);
                        cmd.Parameters.AddWithValue("p_UserName", UserName);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.Parameters.AddWithValue("p_ATStatus", ATStatus);
                        cmd.Parameters.AddWithValue("p_ATRemarks", string.IsNullOrEmpty(ATRemark) ? (object)DBNull.Value : ATRemark);
                        cmd.Parameters.AddWithValue("p_CHRemarks", string.IsNullOrEmpty(CheckerRemark) ? (object)DBNull.Value : CheckerRemark);
                        cmd.Parameters.AddWithValue("p_CHStatus", string.IsNullOrEmpty(CHstatus) ? (object)DBNull.Value : CHstatus);
                        cmd.Parameters.AddWithValue("p_CHUserId", string.IsNullOrEmpty(UserName) ? (object)DBNull.Value : UserName);
                        cmd.Parameters.AddWithValue("p_ActivityType", ActivityType);

                        // Execute the command
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : ChangeBCStatus() \nException Occurred\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Insert Bc master Details
        public DataSet SetInsertUpdateBCMasterDetails()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM SP_InsertOrUpdateBCMaterDetails(:P_Flag, :P_CreatedBy, :P_BcReqId, :P_ClientId, :P_Salt, :P_Salt1, :P_Salt2, :P_Salt3, :P_Salt4, :P_Password)", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("P_Flag", Flag);
                        cmd.Parameters.AddWithValue("P_CreatedBy", CreatedBy);
                        cmd.Parameters.AddWithValue("P_BcReqId", BCReqId);
                        cmd.Parameters.AddWithValue("P_ClientId", Clientcode);
                        cmd.Parameters.AddWithValue("P_Salt", _RandomStringForSalt);
                        cmd.Parameters.AddWithValue("P_Salt1", _RandomStringForSalt);
                        cmd.Parameters.AddWithValue("P_Salt2", DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Salt3", DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Salt4", DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Password", Password);

                        // Execute the function and read results
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            return dataSet; // Return the filled DataSet
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error details
                ErrorLog.CommonTrace($"Class: BCEntity.cs \nFunction: SetInsertUpdateBCMasterDetails() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        #endregion

        #region ActiveDeactiveBC
        public DataSet ActiveDeactiveBC()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                new SqlParameter("@BCCode",BCCode),
                                new SqlParameter("@BCReqId",BCReqId),
                                new SqlParameter("@ActionType",ActionType),
                                new SqlParameter("@UserName",UserName),
                                new SqlParameter("@ActivityType",ActivityType)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "sp_ActiveDeactiveBC";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : ActiveDeactiveBC() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region ChangeBCStatusReEdit
        public DataSet ChangeBCStatusReEdit()
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))  // Use NpgsqlConnection for PostgreSQL
                    {
                        // Define parameters for the stored procedure
                        NpgsqlParameter[] _Params =
                        {
                    new NpgsqlParameter("p_newbcreqid", BCReqId),  // Replace SqlParameter with NpgsqlParameter
                    new NpgsqlParameter("p_username", UserName)  // Pass the username parameter as well
                };

                        cmd.Connection = sqlConn;
                        cmd.CommandType = CommandType.Text;  // Use CommandType.Text for raw SQL commands

                        // Open the connection
                        sqlConn.Open();

                        // Execute the procedure using a CALL statement
                        cmd.CommandText = "CALL sp_reeditbcdetails(@p_newbcreqid, @p_username)";
                        cmd.Parameters.AddRange(_Params);
                        cmd.ExecuteNonQuery();  // Execute the stored procedure

                        // After calling the procedure, select the result from the temporary table
                        cmd.CommandText = "SELECT * FROM temp_status_messages";  // Query the temp table for the result
                        NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);

                        // Create DataSet to hold the results
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);

                        sqlConn.Close();
                        return dataSet;  // Return the result
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : ChangeBCStatusReEdit() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        #endregion

        #region GetBCDetailsToProcessOnboaring
        //public DataSet GetBCDetailsToProcessOnboaring()
        //{
        //    try
        //    {
        //        using (var sqlConn = new NpgsqlConnection(ConnectionString))
        //        {
        //            sqlConn.Open();
        //            using (var cmd = new NpgsqlCommand())
        //            {
        //                cmd.Connection = sqlConn;

        //                // Call the procedure with SELECT
        //                cmd.CommandText = "CALL Proc_GetBCDetailsToProcessOnboarding(@bccode, @BCID, @ClientID, @City, @State, @User, @IsDocUploaded, @Makerstatus, @Checkstatus, @Authstatus, @IsVerified, @IsActive, @ActivityType, @IsRemoved, @Flag)";

        //                // Add parameters with null handling and correct types
        //                cmd.Parameters.AddWithValue("bccode", (object)BCID ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("BCID", (object)BCID ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("ClientID", (object)Clientcode ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("City", (object)City ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("State", (object)State ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("User", UserName);
        //                cmd.Parameters.AddWithValue("IsDocUploaded", (object)IsdocUploaded ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("Makerstatus", (object)Mstatus ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("Checkstatus", (object)CHstatus ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("Authstatus", (object)ATStatus ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("IsVerified", (object)VerificationStatus ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("IsActive", (object)IsActive ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("ActivityType", (object)Activity ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("IsRemoved", (object)IsRemoved ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("Flag", Flag);

        //                cmd.ExecuteNonQuery(); // Execute the procedure

        //                // Now select from the temp table to get the results
        //                cmd.CommandText = "SELECT * FROM temp_results"; // Change to select from temp table

        //                // Use NpgsqlDataAdapter to fill the DataSet
        //                using (var dataAdapter = new NpgsqlDataAdapter(cmd))
        //                {
        //                    DataSet dataSet = new DataSet();
        //                    dataAdapter.Fill(dataSet);
        //                    return dataSet;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDetailsToProcessOnboarding() \nException Occurred\n" + ex.Message);
        //        ErrorLog.DBError(ex);
        //        throw;
        //    }
        //}
        public DataSet GetBCDetailsToProcessOnboaring()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Use a transaction to ensure the temp table is accessible after the call
                    using (var transaction = sqlConn.BeginTransaction())
                    {
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = sqlConn;
                            cmd.Transaction = transaction; // Associate with the transaction

                            // Call the procedure
                            cmd.CommandText = "CALL Proc_GetBCDetailsToProcessOnboarding(@bccode, @BCID, @ClientID, @City, @State, @User, @IsDocUploaded, @Makerstatus, @Checkstatus, @Authstatus, @IsVerified, @IsActive, @ActivityType, @IsRemoved, @Flag)";

                            // Add parameters
                            cmd.Parameters.AddWithValue("bccode", (object)BCID ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("BCID", (object)BCID ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("ClientID", (object)Clientcode ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("City", (object)City ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("State", (object)State ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("User", UserName);
                            cmd.Parameters.AddWithValue("IsDocUploaded", (object)IsdocUploaded ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("Makerstatus", (object)Mstatus ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("Checkstatus", (object)CHstatus ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("Authstatus", (object)ATStatus ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("IsVerified", (object)VerificationStatus ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("IsActive", (object)IsActive ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("ActivityType", (object)Activity ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("IsRemoved", (object)IsRemoved ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("Flag", Flag);

                            cmd.ExecuteNonQuery(); // Execute the procedure

                            // Now select from the temp table to get the results
                            cmd.CommandText = "SELECT * FROM temp_results"; // Change to select from temp table

                            // Use NpgsqlDataAdapter to fill the DataSet
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                DataSet dataSet = new DataSet();
                                dataAdapter.Fill(dataSet);
                                transaction.Commit(); // Commit the transaction
                                return dataSet;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDetailsToProcessOnboarding() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        #endregion

        #region Change BcOnBoard Status
        public DataSet ChangeBcOnBoardStatus()
        {

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {

                                            new SqlParameter("@Remarks",MakerRemark),
                                            new SqlParameter("@ActivityType",ActivityType),
                                            new SqlParameter("@UserName",UserName),
                                            new SqlParameter("@Flag",Flag),
                                            new SqlParameter("@BCCode",BC_Code),
                                            new SqlParameter("@BCReqId",BCReqId),
                                            new SqlParameter("@Mstatus",Mstatus),
                                            new SqlParameter("@CHStatus",CHstatus),
                                            new SqlParameter("@ATStatus",ATStatus),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UpdateBCOnboardProcessHandler";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : ChangeBcOnBoardStatus() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

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
                ErrorLog.AgentManagementTrace("BCEntity: GetCountryStateCity: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }


        #region BCStatusReportGrid
        public DataSet BCStatusReportGrid()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        cmd.CommandText = @"
                    CALL SP_BCStatus(
                        p_VerificationLevel => @p_VerificationLevel,
                        p_OperationType => @p_OperationType,
                        p_Status => @p_Status,
                        p_BcStatus => @p_BcStatus,
                        p_CHStatus => @p_CHStatus,
                        p_Authstatus => @p_Authstatus,
                        p_ActivityType => @p_ActivityType,
                        p_User => @p_User,
                        p_Flag => @p_Flag
                    );";
                        
                        cmd.Parameters.AddWithValue("p_VerificationLevel", VerificationLevel ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_OperationType", OperationType ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Status", Status);
                        cmd.Parameters.AddWithValue("p_BcStatus", BCStatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_CHStatus", CHstatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Authstatus", ATStatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ActivityType", ActivityType);
                        cmd.Parameters.AddWithValue("p_User", UserName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.ExecuteNonQuery();
                        
                        cmd.CommandText = "SELECT * FROM temp_results;";

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            adapter.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BCEntity: BCStatusReportGrid: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
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
                    using (var npgsqlCommand = new NpgsqlCommand("CALL public.sp_get_bc_documents(@p_bcreqid)", sqlConn))
                    {
                        npgsqlCommand.Parameters.AddWithValue("p_bcreqid", BCReqId);
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

        #region SetInsertUpdateBCDetails
        public bool SetInsertUpdateBCDetails(string UserName, out string RequestId, out int Status, out string StatusMsg)
        {
            RequestId = "0";
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
                            new SqlParameter("@Flag", Flag),
                            new SqlParameter("@ClientId", ClientId),
                            new SqlParameter("@MasterID",BCReqId),
                            new SqlParameter("@BcReqId", BC_Code),
                            new SqlParameter("@CreatedBy", CreatedBy),
                            new SqlParameter("@FirstName", FirstName),
                            new SqlParameter("@MiddleName", MiddleName),
                            new SqlParameter("@LastName", LastName),
                            new SqlParameter("@BcCategory", BcCategory),
                            new SqlParameter("@BcAddress", RegisteredAddress),
                            new SqlParameter("@Pincode", Pincode),
                            new SqlParameter("@TypeOfOrg",TypeOfOrg ),
                            new SqlParameter("@AccountName","" ),
                            new SqlParameter("@AccountNumber",AccountNumber ),
                            new SqlParameter("@IFSCCode",IFSCCode ),
                            new SqlParameter("@Bank","" ),
                            new SqlParameter("@AEPS",ForAEPS ),
                            new SqlParameter("@MATM",ForMicroATM ),
                            new SqlParameter("@Gender",Gender ),
                            new SqlParameter("@AadharNo",AadharNo ),
                            new SqlParameter("@PanNo",PanNo ),
                            new SqlParameter("@GSTNo",GSTNo ),
                            new SqlParameter("@EmailID",PersonalEmail ),
                            new SqlParameter("@ContactNo",PersonalContact ),
                            new SqlParameter("@LandlineNo",LandlineContact ),
                            new SqlParameter("@AlternateNo",AlternetNo ),
                            new SqlParameter("@Country",Country ),
                            new SqlParameter("@State",State ),
                            new SqlParameter("@City",City ),
                            new SqlParameter("@District",District ),
                            new SqlParameter("@IdentityProofType",IdentityProofType ),
                            new SqlParameter("@IdentityProofDocument",IdentityProofDocument),
                            new SqlParameter("@AddressProofType",AddressProofType ),
                            new SqlParameter("@AddressProofDocument",AddressProofDocument),
                            new SqlParameter("@SignatureProofType",SignatureProofType ),
                            new SqlParameter("@SignatureProofDocument",SignatureProofDocument),
                            new SqlParameter("@ActivityType",Activity),
                            new SqlParameter("@RequestId", SqlDbType.Int){ Direction = ParameterDirection.Output },
                            new SqlParameter("@Status", SqlDbType.Int){ Direction = ParameterDirection.Output },
                            new SqlParameter("@StatusMsg", SqlDbType.VarChar,100){ Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "sp_InsertOrUpdateBCDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        cmd.Dispose();
                        return Status == 1 ? true : false;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : SetInsertUpdateBCDetails() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region ReEdit Validate BC

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
                    using (var cmd = new NpgsqlCommand("CALL public.spBCRequestValidationGetValidate(null,null,null,@p_BCReqId, @p_flag)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;
                        
                        cmd.Parameters.Add("p_Status", NpgsqlTypes.NpgsqlDbType.Varchar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_RequestId", NpgsqlTypes.NpgsqlDbType.Varchar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_StatusMsg", NpgsqlTypes.NpgsqlDbType.Varchar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("p_BCReqId", (object)BCReqId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        
                        cmd.ExecuteNonQuery();
                        
                        _Status = cmd.Parameters["p_Status"].Value?.ToString();
                        _Requestid = cmd.Parameters["p_RequestId"].Value?.ToString();
                        _StatusMsg = cmd.Parameters["p_StatusMsg"].Value?.ToString();

                        return _Status;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace($"Class : BCEntity.cs \nFunction : ValidateEditBcDetails() \nException Occurred\n{Ex.Message}");
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        #endregion

        #region Delete Agent
        public string DeleteBcDetails()
        {
            string _Status = null;
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    using (var cmd = new NpgsqlCommand("CALL BCRequest_Delete(:p_BCReqId, null)", sqlConn))
                    {
                        // Assuming BCReqId is originally a string and needs to be converted to integer
                        cmd.Parameters.AddWithValue("p_BCReqId", BCReqId); // Pass as integer

                        // Add output parameter
                        var statusParam = new NpgsqlParameter("p_Status", NpgsqlTypes.NpgsqlDbType.Varchar)
                        {
                            Direction = ParameterDirection.Output,
                            Size = 20
                        };
                        cmd.Parameters.Add(statusParam);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Retrieve the output status
                        _Status = statusParam.Value.ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : DeleteBcDetails() \nException Occurred\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }

            return _Status;
        }


        #endregion

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
                        cmd.Parameters.Add("p_CountryID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(Country) ? (object)DBNull.Value : Country;
                        cmd.Parameters.Add("p_PinCode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(Pincode) ? (object)DBNull.Value : Pincode;
                        cmd.Parameters.Add("p_StateID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(State) ? (object)DBNull.Value : State;
                        cmd.Parameters.Add("p_City", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(City) ? (object)DBNull.Value : City;
                        cmd.Parameters.Add("p_District", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(District) ? (object)DBNull.Value : District;

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



        #region Aggregator Methods
        public bool Insert_aggregatorRequest(string UserName, out int requestId, out string status, out string statusMsg)
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
                    //sqlConn.Notice += (o, e) =>
                    //{
                    //    var message = e.Notice.MessageText;
                    //    ErrorLog.CommonTrace($"NOTICE: {message}");
                    //    notices.Add(message);
                    //};

                    // Open the connection
                    sqlConn.Open();

                    using (var cmd = new NpgsqlCommand("CALL public.sp_aggregator_request_registration_insert(NULL,NULL,NULL, @p_clientid, @p_createdby, @p_firstname, @p_middlename, @p_lastname, @p_gender, @p_emailid, @p_contactno, @p_landlineno, @p_alternateno, @p_aadharno, @p_panno, @p_gstno, @p_bcaddress, @p_country, @p_state, @p_city, @p_pincode, @p_district, @p_typeoforg, @p_bccategory, @p_bccode, @p_accountname, @p_accountnumber, @p_ifsccode, @p_bank, @p_identityprooftype, @p_identityproofdocument, @p_addressprooftype, @p_addressproofdocument, @p_signatureprooftype, @p_signatureproofdocument, @p_bcreqid, @p_masterid, @p_aeps, @p_matm, @p_flag, @p_activitytype, @p_education)", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;
                        var outputRequestId = new NpgsqlParameter("p_reqid", NpgsqlTypes.NpgsqlDbType.Integer)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputRequestId);
                        var outputStatus = new NpgsqlParameter("p_status", NpgsqlTypes.NpgsqlDbType.Integer)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputStatus);

                        // Output parameter for Status Message
                        var outputStatusMsg = new NpgsqlParameter("p_statusmsg", NpgsqlTypes.NpgsqlDbType.Text)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputStatusMsg);

                        cmd.Parameters.AddWithValue("p_clientid", (object)ClientId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_createdby", (object)CreatedBy ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_firstname", (object)FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_middlename", (object)MiddleName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_lastname", (object)LastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_gender", (object)Gender ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_emailid", (object)PersonalEmail ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_contactno", (object)PersonalContact ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_landlineno", (object)LandlineContact ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_alternateno", (object)AlternetNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_aadharno", (object)AadharNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_panno", (object)PanNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_gstno", (object)GSTNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bcaddress", (object)RegisteredAddress ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_country", (object)Country ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_state", (object)State ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_city", (object)City ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_pincode", (object)Pincode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_district", (object)District ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_typeoforg", (object)TypeOfOrg ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bccategory", (object)BcCategory ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bccode", (object)bccode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_accountname", (object)string.Empty ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_accountnumber", (object)AccountNumber ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ifsccode", (object)IFSCCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bank", (object)string.Empty ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_identityprooftype", (object)IdentityProofType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_identityproofdocument", (object)IdentityProofDocument ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_addressprooftype", (object)AddressProofType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_addressproofdocument", (object)AddressProofDocument ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_signatureprooftype", (object)SignatureProofType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_signatureproofdocument", (object)SignatureProofDocument ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_bcreqid", BCReqId);
                        cmd.Parameters.AddWithValue("p_masterid", (object)BCCode ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("p_aeps", ForAEPS);
                        //cmd.Parameters.AddWithValue("p_matm", ForMicroATM);
                        cmd.Parameters.AddWithValue("p_aeps", ForAEPS.HasValue ? ForAEPS.Value : 0);
                        cmd.Parameters.AddWithValue("p_matm", ForMicroATM.HasValue ? ForMicroATM.Value : 0);
                        cmd.Parameters.AddWithValue("p_flag", Flag);
                        cmd.Parameters.AddWithValue("p_activitytype", string.IsNullOrEmpty(Activity) ? "0" : Activity);
                        cmd.Parameters.AddWithValue("p_education", (object)Education ?? DBNull.Value);

                        try
                        {
                            cmd.ExecuteNonQuery();

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
                            //requestId = Convert.ToInt32(outputRequestId.Value);
                            //status = Convert.ToString(outputStatus.Value);
                            //statusMsg = outputStatusMsg.Value.ToString();
                            //Console.WriteLine($"Request ID: {requestId}, Status: {status}, Status Message: {statusMsg}");
                            return true;
                        }
                        catch (PostgresException ex)
                        {
                            ErrorLog.CommonTrace($"Postgres Error: {ex.Message}");
                            return false;
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.CommonTrace($"General Error: {ex.Message}");
                            return false;
                        }

                        //if (notices.Count > 0)
                        //{
                        //    foreach (var notice in notices)
                        //    {
                        //        ErrorLog.CommonTrace($"NOTICE: {notice}");
                        //    }
                        //}
                        //else
                        //{
                        //    ErrorLog.CommonTrace("No notices captured.");
                        //}
                        //return true; // Assuming success if no exceptions were thrown
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Error connecting to database: {ex.Message}");
                return false;
            }
        }
        public string SetInsertUpdateaggregatorTrackerDetails()
          {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (var cmd = new NpgsqlCommand("CALL sp_insertorupdateaggtrackerdetails(null, @p_Flag, @p_CreatedBy, @p_BcReqId, @p_ActivityType)", sqlConn))
                    {
                        // Ensure parameters are added correctly
                        var statusParam = new NpgsqlParameter("v_status", NpgsqlTypes.NpgsqlDbType.Varchar)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(statusParam);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.Parameters.AddWithValue("p_CreatedBy", CreatedBy);
                        cmd.Parameters.AddWithValue("p_BcReqId", BCReqId); // Ensure this is an integer
                        cmd.Parameters.AddWithValue("p_ActivityType", Activity ?? (object)DBNull.Value);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();
                        string statusMsg = statusParam.Value.ToString();
                        // Retrieve the status message
                        return statusMsg.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: BCEntity.cs \nFunction: SetInsertUpdateBCTrackerDetails() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        public DataSet GetAggregatorRequestList()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Open the connection

                    // Execute the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL public.sp_aggrequest_registration_get(@P_BCCode, @P_StageId, @P_RequestTypeId, NULL, NULL, @P_PanNo, @P_AadharNo, @P_GSTNo, NULL, @P_ContactNo, @P_PersonalEmailID, @P_Makerstatus, @P_Checkstatus, @P_Authstatus, @P_Flag)", sqlConn))
                    {
                        // Add parameters with explicit types
                        cmd.Parameters.AddWithValue("P_BCCode", (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("P_StageId", 0);
                        cmd.Parameters.AddWithValue("P_RequestTypeId", (object)RequestTypeId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_PanNo", (object)PanNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_AadharNo", (object)AadharNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_GSTNo", (object)GSTNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_ContactNo", (object)PersonalContact ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_PersonalEmailID", (object)PersonalEmail ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Makerstatus", (object)Mstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Checkstatus", (object)CHstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Authstatus", (object)ATStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Flag", Flag); // Use the appropriate variable for Flag

                        cmd.ExecuteNonQuery(); // Execute the stored procedure
                    }

                    // Now retrieve the data from the temporary table
                    using (var dataAdapter = new NpgsqlDataAdapter("SELECT * FROM temp_results", sqlConn))
                    {
                        DataSet dataTable = new DataSet();
                        dataAdapter.Fill(dataTable); // Fill the DataSet with data from the temp table
                        return dataTable; // Return the populated DataSet
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("BCEntity: GetBCRequestList: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw; // Rethrow the exception for further handling
            }
        }

        public DataSet GetOnboradingAggDetails()
        {
            try
            {
                var result = new DataSet();

                // Create and open the connection
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Create the command for the stored procedure using CALL
                    using (var npgsqlCommand = new NpgsqlCommand("CALL proc_bindonboarededaggdata(:p_BCRequest);", connection))
                    {
                        // Add parameter for the request
                        npgsqlCommand.Parameters.AddWithValue("p_BCRequest", BCRequest);

                        // Execute the stored procedure
                        npgsqlCommand.ExecuteNonQuery();
                    }

                    // Now, retrieve data from the temporary table
                    using (var tempTableCommand = new NpgsqlCommand("SELECT * FROM temp_BC_Request;", connection))
                    {
                        using (var npgsqlDataAdapter = new NpgsqlDataAdapter(tempTableCommand))
                        {
                            npgsqlDataAdapter.Fill(result);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetOnboardingBcDetails() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }


        #region ChangeAggregatorStatus
        public DataSet ChangeAggregatorStatus()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "CALL sp_updateaggonboardprocess(:p_BCCode, :p_BCID, :p_BCReqId, :p_Remarks, :p_ActionType, :p_UserName, :p_Flag, :p_ATStatus, :p_ATRemarks, :p_CHRemarks, :p_CHStatus, :p_CHUserId, :p_ActivityType)";
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("p_BCCode", string.IsNullOrEmpty(BCCode) ? (object)DBNull.Value : BCCode);
                        cmd.Parameters.AddWithValue("p_BCID", string.IsNullOrEmpty(BCID) ? (object)DBNull.Value : BCID);
                        cmd.Parameters.AddWithValue("p_BCReqId", BCReqId);
                        cmd.Parameters.AddWithValue("p_Remarks", string.IsNullOrEmpty(ATRemark) ? (object)DBNull.Value : ATRemark);
                        cmd.Parameters.AddWithValue("p_ActionType", ActionType);
                        cmd.Parameters.AddWithValue("p_UserName", UserName);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.Parameters.AddWithValue("p_ATStatus", ATStatus);
                        cmd.Parameters.AddWithValue("p_ATRemarks", string.IsNullOrEmpty(ATRemark) ? (object)DBNull.Value : ATRemark);
                        cmd.Parameters.AddWithValue("p_CHRemarks", string.IsNullOrEmpty(CheckerRemark) ? (object)DBNull.Value : CheckerRemark);
                        cmd.Parameters.AddWithValue("p_CHStatus", string.IsNullOrEmpty(CHstatus) ? (object)DBNull.Value : CHstatus);
                        cmd.Parameters.AddWithValue("p_CHUserId", string.IsNullOrEmpty(UserName) ? (object)DBNull.Value : UserName);
                        cmd.Parameters.AddWithValue("p_ActivityType", ActivityType);
                        
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);

                            cmd.CommandText = "SELECT * FROM temp_row_count";
                            DataTable tempRowCountTable = new DataTable();
                            dataAdapter.Fill(tempRowCountTable);
                            dataSet.Tables.Add(tempRowCountTable);

                            return dataSet;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : ChangeBCStatus() \nException Occurred\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region Insert Agg master Details
        public DataSet SetInsertUpdateAggMasterDetails()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM sp_iouaggmaterdetails(:P_Flag, :P_CreatedBy, :P_BcReqId, :P_ClientId, :P_Salt, :P_Salt1, :P_Salt2, :P_Salt3, :P_Salt4, :P_Password)", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("P_Flag", Flag);
                        cmd.Parameters.AddWithValue("P_CreatedBy", CreatedBy);
                        cmd.Parameters.AddWithValue("P_BcReqId", BCReqId);
                        cmd.Parameters.AddWithValue("P_ClientId", Clientcode);
                        cmd.Parameters.AddWithValue("P_Salt", _RandomStringForSalt);
                        cmd.Parameters.AddWithValue("P_Salt1", _RandomStringForSalt);
                        cmd.Parameters.AddWithValue("P_Salt2", DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Salt3", DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Salt4", DBNull.Value);
                        cmd.Parameters.AddWithValue("P_Password", Password);

                        // Execute the function and read results
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            return dataSet; // Return the filled DataSet
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error details
                ErrorLog.CommonTrace($"Class: BCEntity.cs \nFunction: SetInsertUpdateBCMasterDetails() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        #endregion
        #region AggStatusReportGrid
        public DataSet AggStatusReportGrid()
        {
            try
            {
                using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open(); // Open the connection

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        // Prepare the command
                        cmd.Connection = sqlConn;

                        // Construct the command text to call the procedure with parameters
                        cmd.CommandText = @"
                    CALL sp_aggregatorstatus(
                        p_VerificationLevel => @p_VerificationLevel,
                        p_OperationType => @p_OperationType,
                        p_Status => @p_Status,
                        p_BcStatus => @p_BcStatus,
                        p_CHStatus => @p_CHStatus,
                        p_Authstatus => @p_Authstatus,
                        p_ActivityType => @p_ActivityType,
                        p_User => @p_User,
                        p_Flag => @p_Flag
                    );";

                        // Add parameters with explicit types if necessary
                        cmd.Parameters.AddWithValue("p_VerificationLevel", VerificationLevel ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_OperationType", OperationType ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Status", Status);
                        cmd.Parameters.AddWithValue("p_BcStatus", BCStatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_CHStatus", CHstatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Authstatus", ATStatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_ActivityType", ActivityType);
                        cmd.Parameters.AddWithValue("p_User", UserName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_Flag", Flag);
                        cmd.ExecuteNonQuery(); // Execute the procedure

                        // Now, retrieve the data from the temp table
                        cmd.CommandText = "SELECT * FROM temp_results;"; // Assuming temp_results exists

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            adapter.Fill(ds); // Fill the DataSet with the results
                            return ds; // Return the DataSet containing the results
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BCEntity: BCStatusReportGrid: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        #endregion

        public DataSet GetAggDetailsToProcessOnboaring()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Use a transaction to ensure the temp table is accessible after the call
                    using (var transaction = sqlConn.BeginTransaction())
                    {
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = sqlConn;
                            cmd.Transaction = transaction; // Associate with the transaction

                            // Call the procedure
                            cmd.CommandText = "CALL proc_getaggdetailstoprocessonboarding(@bccode, @BCID, @ClientID, @City, @State, @User, @IsDocUploaded, @Makerstatus, @Checkstatus, @Authstatus, @IsVerified, @IsActive, @ActivityType, @IsRemoved, @Flag)";

                            // Add parameters
                            cmd.Parameters.AddWithValue("bccode", (object)BCID ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("BCID", (object)BCID ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("ClientID", (object)Clientcode ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("City", (object)City ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("State", (object)State ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("User", UserName);
                            cmd.Parameters.AddWithValue("IsDocUploaded", (object)IsdocUploaded ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("Makerstatus", (object)Mstatus ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("Checkstatus", (object)CHstatus ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("Authstatus", (object)ATStatus ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("IsVerified", (object)VerificationStatus ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("IsActive", (object)IsActive ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("ActivityType", (object)Activity ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("IsRemoved", (object)IsRemoved ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("Flag", Flag);

                            cmd.ExecuteNonQuery(); // Execute the procedure

                            // Now select from the temp table to get the results
                            cmd.CommandText = "SELECT * FROM temp_results"; // Change to select from temp table

                            // Use NpgsqlDataAdapter to fill the DataSet
                            using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                            {
                                DataSet dataSet = new DataSet();
                                dataAdapter.Fill(dataSet);
                                transaction.Commit(); // Commit the transaction
                                return dataSet;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDetailsToProcessOnboarding() \nException Occurred\n" + ex.Message);
                ErrorLog.DBError(ex);
                throw;
            }
        }

        public DataSet ChangeAggStatusReEdit()
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(ConnectionString))
                    {
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SELECT * FROM public.sp_reeditaggdetails(@p_NewBcReqId, @p_UserName)";
                        cmd.CommandType = CommandType.Text;

                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("p_NewBcReqId", (object)BCReqId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("p_UserName", (object)UserName ?? DBNull.Value);

                        sqlConn.Open();

                        DataSet dataSet = new DataSet();
                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        cmd.Dispose();
                        return dataSet;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : ChangeBCStatusReEdit() \nException Occurred\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #region GetDocs
        public DataSet GetAggregatorDocs()
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
                    using (var npgsqlCommand = new NpgsqlCommand("CALL public.sp_get_agg_documents(@p_bcreqid)", sqlConn))
                    {
                        npgsqlCommand.Parameters.AddWithValue("p_bcreqid", BCReqId);
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

        #region getReceiptData
        public DataSet getAggReceiptData()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Call the procedure with the correct parameter
                    using (var cmd = new NpgsqlCommand("CALL sp_getAggreceiptdata(@p_bccode)", sqlConn))
                    {
                        // Use the correct parameter name with the @ prefix
                        cmd.Parameters.AddWithValue("p_bccode", BCCode);
                        cmd.ExecuteNonQuery(); // Execute the procedure
                    }

                    // Now retrieve the data from the temporary table
                    using (var cmd = new NpgsqlCommand("SELECT * FROM temp_receipt_data", sqlConn))
                    {
                        DataSet dataSet = new DataSet();
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataSet); // Fill the DataSet with the results
                        }

                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace($"Class: BCEntity.cs \nFunction: GetReceiptData() \nException Occurred\n{ex.Message}");
                ErrorLog.DBError(ex);
                throw;
            }
        }
        #endregion

        #endregion
        #region InheritServicesFromParent
        public DataSet InheritServicesFromParent()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    // Call the procedure with parameters
                    using (var cmd = new NpgsqlCommand("CALL SP_GetServicesByClientID(@p_ClientID, @p_UserName, @p_Flag)", sqlConn))
                    {
                        // Add parameters with the correct names
                        cmd.Parameters.AddWithValue("p_ClientID", Clientcode);
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
                ErrorLog.CommonTrace($"Class: FranchiseEntity.cs \nFunction: InheritServicesFromParent() \nException Occurred\n{ex.Message}");
                // Optionally log more details or rethrow the exception
            }
            return dataSet;
        }

        #endregion
    }
}

