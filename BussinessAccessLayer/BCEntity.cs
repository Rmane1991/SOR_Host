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
        public string Clientcode { get; set; }
        public int? ForMicroATM { get; set; }
        public string ModifiedBy { get; set; }
        //public string BCFranchiseID { get; set; }
        public string BCID { get; set; }
        public string BCCode { get; set; }
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
                            requestId = Convert.ToInt32(outputRequestId.Value);
                            status = Convert.ToString(outputStatus.Value);
                            statusMsg = outputStatusMsg.Value.ToString();
                            Console.WriteLine($"Request ID: {requestId}, Status: {status}, Status Message: {statusMsg}");
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                             new SqlParameter("@BCRequest ", BCRequest)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_BindOnBoarededBCData";
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                    new SqlParameter("@UserName", UserName),
                                    new SqlParameter("@IsVerified", VerificationStatus),
                                    new SqlParameter("@IsActive", IsActive),
                                    new SqlParameter("@IsRemoved", IsRemoved),
                                    new SqlParameter("@ClientID", Clientcode),
                                    new SqlParameter("@IsDocUploaded", IsdocUploaded),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindBC";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : BindBCddl() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
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

        #region ChangeBCStatus
        public DataSet ChangeBCStatus()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                             new SqlParameter("@BCReqId",BCReqId),
                             new SqlParameter("@BCID",BCID),
                             new SqlParameter("@BCCode",BCID),
                             new SqlParameter("@ActionType",ActionType),
                             new SqlParameter("@UserName",UserName),
                             new SqlParameter("@Flag",Flag),
                             new SqlParameter("@ATRemarks",ATRemark),
                             new SqlParameter("@ATStatus",ATStatus),
                             new SqlParameter("@CHRemarks",CheckerRemark),
                             new SqlParameter("@CHStatus",CHstatus),
                             new SqlParameter("@ActivityType",ActivityType)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_UpdateBCOnboardProcess";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : ChangeBCStatus() \nException Occured\n" + Ex.Message);
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                   new SqlParameter("@Flag", Flag),
                                   new SqlParameter("@CreatedBy", CreatedBy),
                                   new SqlParameter("@BcReqId",BCReqId),
                                   new SqlParameter("@ClientId",Clientcode),
                                   new SqlParameter("@Salt",_RandomStringForSalt),
                                   new SqlParameter("@Salt1",_RandomStringForSalt),
                                   new SqlParameter("@Salt2",null),
                                   new SqlParameter("@Salt3",null),
                                   new SqlParameter("@Salt4",null),
                                   new SqlParameter("@Password", Password)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_InsertOrUpdateBCMaterDetails";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : SetInsertUpdateBCMasterDetails() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                    new SqlParameter("@Mastereqid",BCReqId),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_ReeditBCDetails";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : ChangeBCStatusReEdit() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetBCDetailsToProcessOnboaring
        public DataSet GetBCDetailsToProcessOnboaring()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = sqlConn;

                        // Call the procedure with SELECT
                        cmd.CommandText = "CALL Proc_GetBCDetailsToProcessOnboarding(@bccode, @BCID, @ClientID, @City, @State, @User, @IsDocUploaded, @Makerstatus, @Checkstatus, @Authstatus, @IsVerified, @IsActive, @ActivityType, @IsRemoved, @Flag)";

                        // Add parameters with null handling and correct types
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

                        // Execute the command and fill the DataSet
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@VerificationLevel", VerificationLevel),
                                                       new SqlParameter("@OperationType", OperationType),
                                                       new SqlParameter("@Status", Status),
                                                       new SqlParameter ("@BCStatus", BCStatus),
                                                       new SqlParameter ("@CHStatus", CHstatus),
                                                       new SqlParameter("@Authstatus",ATStatus),
                                                       new SqlParameter("@ActivityType",ActivityType),
                                                       new SqlParameter("@User", UserName),
                                                       new SqlParameter("@Flag", Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BCStatus";
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@BCReqId", BCReqId),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Sp_GetBCDocuments";
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
                ErrorLog.AgentManagementTrace("BCEntity: ChangeAgentOnboardStatus: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
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
                    using (var cmd = new NpgsqlCommand("public.spBCRequestValidationGetValidate", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding parameters with correct types
                        cmd.Parameters.AddWithValue("p_BCReqId", (object)BCReqId ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("p_Flag", int.TryParse(Flag, out int flagValue) ? (object)flagValue : DBNull.Value);
                        cmd.Parameters.Add("p_Status", NpgsqlTypes.NpgsqlDbType.Varchar).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_RequestId", NpgsqlTypes.NpgsqlDbType.Varchar).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_StatusMsg", NpgsqlTypes.NpgsqlDbType.Varchar).Direction = ParameterDirection.Output;

                        // Execute the command
                        cmd.ExecuteNonQuery();

                        // Retrieve output parameters
                        _Status = cmd.Parameters["p_Status"].Value?.ToString();
                        _Requestid = cmd.Parameters["p_RequestId"].Value?.ToString();
                        _StatusMsg = cmd.Parameters["p_StatusMsg"].Value?.ToString();

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

        #region Delete Agent
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
                              new SqlParameter("@BCReqId",BCReqId),
                              new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BCRequest_Delete";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : DeleteBcDetails() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        public DataSet GetCountryStateCityD()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnectionString))
                {
                    // Prepare to call the stored procedure
                    using (var cmd = new NpgsqlCommand("CALL SP_GetStateCity(:p_Mode, :p_CountryID, :p_StateID, :p_PinCode, :p_City, :p_District)", sqlConn))
                    {
                        cmd.Parameters.Add("p_Mode", NpgsqlTypes.NpgsqlDbType.Integer).Value = mode == 0 ? (object)DBNull.Value : mode; // Change to Integer
                        cmd.Parameters.Add("p_CountryID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(Country) ? (object)DBNull.Value : Country;
                        cmd.Parameters.Add("p_PinCode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(Pincode) ? (object)DBNull.Value : Pincode;
                        cmd.Parameters.Add("p_StateID", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(State) ? (object)DBNull.Value : State;
                        cmd.Parameters.Add("p_City", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(City) ? (object)DBNull.Value : City;
                        cmd.Parameters.Add("p_District", NpgsqlTypes.NpgsqlDbType.Varchar).Value = string.IsNullOrEmpty(District) ? (object)DBNull.Value : District;

                        sqlConn.Open();

                        // Call the procedure
                        cmd.ExecuteNonQuery(); // Since we are executing a procedure, we use ExecuteNonQuery

                        // Now, select from the temporary tables to fill the DataSet
                        DataSet dataSet = new DataSet();

                        // Example: Get country data
                        using (var countryCmd = new NpgsqlCommand("SELECT * FROM temp_country", sqlConn))
                        using (var countryAdapter = new NpgsqlDataAdapter(countryCmd))
                        {
                            countryAdapter.Fill(dataSet, "Countries");
                        }

                        // Get states data
                        using (var statesCmd = new NpgsqlCommand("SELECT * FROM temp_states", sqlConn))
                        using (var statesAdapter = new NpgsqlDataAdapter(statesCmd))
                        {
                            statesAdapter.Fill(dataSet, "States");
                        }

                        // Get districts data
                        using (var districtsCmd = new NpgsqlCommand("SELECT * FROM temp_districts", sqlConn))
                        using (var districtsAdapter = new NpgsqlDataAdapter(districtsCmd))
                        {
                            districtsAdapter.Fill(dataSet, "Districts");
                        }

                        // Get cities data
                        using (var citiesCmd = new NpgsqlCommand("SELECT * FROM temp_cities", sqlConn))
                        using (var citiesAdapter = new NpgsqlDataAdapter(citiesCmd))
                        {
                            citiesAdapter.Fill(dataSet, "Cities");
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




    }
}

