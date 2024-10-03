using System;
using AppLogger;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;

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
        public int ForMicroATM { get; set; }
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@BCCode", BCCode)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_getReceiptData";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : getReceiptData() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
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
  
        ///////////////////////////////

        #region Insert_BCRequest out string
        public bool Insert_BCRequest(string UserName, out string RequestId, out string Status, out string StatusMsg)
        {
            RequestId = "0";
            Status = "-1";
            StatusMsg = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                              new SqlParameter("@ClientId",ClientId),
                              new SqlParameter("@MasterID",BCCode),
                              new SqlParameter("@BcReqId", BCReqId),
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
                              new SqlParameter("@IsAPIEnable",IsAPIEnable),
                              new SqlParameter("@Flag",Flag),
                              new SqlParameter("@ActivityType",Activity),
                              new SqlParameter("@Status", SqlDbType.Int) { Direction = ParameterDirection.Output },
                              new SqlParameter("@StatusMsg", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
                              new SqlParameter("@RequestId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BcRequest_Registration_Insert";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        StatusMsg = Convert.ToString(cmd.Parameters["@StatusMsg"].Value);
                        RequestId = Convert.ToString(cmd.Parameters["@RequestId"].Value);
                        sqlConn.Close();
                        cmd.Dispose();
                        return Status == "1" ? true : false;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BCEntity: Insert_BCRequest: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
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
                    using (var cmd = new NpgsqlCommand("CALL SP_BCRequest_Registration_Get(:PanNo, 0, :RequestTypeId, NULL, NULL, :AadharNo, :GSTNo, NULL, :ContactNo, :PersonalEmailID, :Makerstatus, :Checkstatus, :Authstatus, :Flag)", sqlConn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("PanNo", (object)PanNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RequestTypeId", (object)RequestTypeId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("AadharNo", (object)AadharNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("GSTNo", (object)GSTNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("ContactNo", (object)PersonalContact ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("PersonalEmailID", (object)PersonalEmail ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Makerstatus", (object)Mstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Checkstatus", (object)CHstatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Authstatus", (object)ATStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Flag", Flag);

                        sqlConn.Open();
                        DataSet dataTable = new DataSet();
                        using (var dataAdapter = new NpgsqlDataAdapter(cmd))
                        {
                            dataAdapter.Fill(dataTable);
                        }
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("BCEntity: GetBCRequestList: UserName: " + UserName + " Exception: " + ex.Message);
                ErrorLog.DBError(ex);
                throw;
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
        public DataSet SetInsertUpdateBCTrackerDetails()
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
                                new SqlParameter("@BcReqId",BCReqId ),
                                new SqlParameter("@ActivityType",Activity),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_InsertOrUpdateBCtrackerDetails";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : SetInsertUpdateBCTrackerDetails() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                  new SqlParameter("@UserName", UserName),
                                  new SqlParameter("@IsRemoved", IsRemoved),
                                  new SqlParameter("@BCCode", BCID),
                                  new SqlParameter("@ClientID", Clientcode),
                                  new SqlParameter("@Flag", Flag),
                                  new SqlParameter("@Makerstatus",Mstatus),
                                  new SqlParameter("@Checkstatus",CHstatus),
                                  new SqlParameter("@Authstatus",ATStatus)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindBCVerification";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : BindBCVerification() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetBCDocuments
        public DataSet GetBCDocuments()
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
                                new SqlParameter("@BCID", BCReqId),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_GetBCDocuments";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDocuments() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        public DataSet GetBCDocumentByID()
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
                        cmd.CommandText = "Proc_GetBCDocuments";
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDocumentByID() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                            new SqlParameter("@bccode",BCID),
                                            new SqlParameter("@BCID",BCID),
                                            new SqlParameter("@ClientID",Clientcode),
                                            new SqlParameter("@City", City),
                                            new SqlParameter("@State", State),
                                            new SqlParameter("@User",UserName),
                                            new SqlParameter("@IsDocUploaded",IsdocUploaded),
                                            new SqlParameter("@Makerstatus",Mstatus),
                                            new SqlParameter("@Checkstatus",CHstatus),
                                            new SqlParameter("@Authstatus",ATStatus),
                                            new SqlParameter("@IsVerified",VerificationStatus),
                                            new SqlParameter("@IsActive",IsActive),
                                            new SqlParameter("@ActivityType",Activity),
                                            new SqlParameter("@IsRemoved",IsRemoved),
                                            new SqlParameter("@Flag",Flag),
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
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : GetBCDetailsToProcessOnboaring() \nException Occured\n" + Ex.Message);
                ErrorLog.DBError(Ex);
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                              new SqlParameter("@BCReqId",BCReqId),
                              new SqlParameter("@Flag",Flag),
                              new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
                              new SqlParameter("@RequestId", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
                               new SqlParameter("@StatusMsg", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "sp_BCRequest_Validation_GetValidate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dataSet);
                        _Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        _StatusMsg = Convert.ToString(cmd.Parameters["@StatusMsg"].Value);
                        _Requestid = Convert.ToString(cmd.Parameters["@RequestId"].Value);
                        cmd.Dispose();
                        return _Status;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : BCEntity.cs \nFunction : ValidateEditBcDetails() \nException Occured\n" + Ex.Message);
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

