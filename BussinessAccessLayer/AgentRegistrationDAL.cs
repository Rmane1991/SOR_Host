using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using System;
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
        public DataTable GetBC(string UserName, int VerificationStatus, int IsActive, int IsRemoved, string ClientID, int IsdocUploaded)
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
                            new SqlParameter("@ClientID",ClientID ),
                            new SqlParameter("@IsDocUploaded", IsdocUploaded),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindBC_Reports";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: BindBC: UserName: " + UserName + " Exception: " + Ex.Message);
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@BCCode",UserName),
                            new SqlParameter("@AgentCode",AgentCode),
                            new SqlParameter("@RequestTypeId",RequestTypeId),
                            new SqlParameter("@RequestId",RequestId),
                            new SqlParameter("@BucketId",BucketId),
                            new SqlParameter("@RequestStatusId",RequestStatusId),
                            new SqlParameter("@PanNo",PanNo),
                            new SqlParameter("@AadharNo",AadharNo),
                            new SqlParameter("@GSTNo",GSTNo),
                            new SqlParameter("@PassportNo",PassportNo),
                            new SqlParameter("@ContactNo",ContactNo),
                            new SqlParameter("@PersonalEmailID",PersonalEmailID),
                            new SqlParameter("@Prestatus",Prestatus),
                             new SqlParameter("@Makerstatus",Mstatus),
                            new SqlParameter("@Checkstatus",ChStatus),
                            new SqlParameter("@Authstatus",AtStatus),
                            new SqlParameter("@username",UserName),
                            new SqlParameter("@Flag",Flag)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_AgentRequest_Registration_Get";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        DataTable dataTable = new DataTable();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(ds);
                        cmd.Dispose();
                        return ds;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: GetAgentRequestList: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
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

        public bool Insert_AgentRequest(string UserName, out string RequestId, out int Status, out string StatusMsg)
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
                            new SqlParameter("@ClientId",ClientId),
                            new SqlParameter("@CreatedBy", CreatedBy),
                            new SqlParameter("@AgentName", AgentName),
                            new SqlParameter("@MiddleName", MiddleName),
                            new SqlParameter("@LastName", LastName),
                            new SqlParameter("@Gender", Gender),
                            new SqlParameter("@EmailID", PersonalEmailID),
                            new SqlParameter("@ContactNo", ContactNo),
                            new SqlParameter("@PassportNo", PassportNo),
                            new SqlParameter("@PersonalEmailID", PersonalEmailID),
                            new SqlParameter("@FirstName", FirstName),
                            new SqlParameter("@AgentDOB", AgentDOB),
                            new SqlParameter("@DeviceCode", DeviceCode),
                            new SqlParameter("@LandlineNo", LandlineNo),
                            new SqlParameter("@AlternateNo", AlternateNo),
                            new SqlParameter("@BCCode", BCCode),
                            new SqlParameter("@AadharNo", AadharNo),
                            new SqlParameter("@PanNo", PanNo),
                            new SqlParameter("@GSTNo", GSTNo),
                            new SqlParameter("@AgentDistrict", AgentDistrict),
                            new SqlParameter("@AgentAddress", AgentAddress),
                            new SqlParameter("@AgentCountry", AgentCountry),
                            new SqlParameter("@AgentState", AgentState),
                            new SqlParameter("@AgentCity", AgentCity),
                            new SqlParameter("@AgentPincode", AgentPincode),
                            new SqlParameter("@ShopAddress", ShopAddress),
                            new SqlParameter("@PopulationGroup",PopulationGroup),
                            new SqlParameter("@AgentCategory", AgentCategory),
                            new SqlParameter("@AccountName", AccountName),
                            new SqlParameter("@AccountNumber",AccountNumber),
                            new SqlParameter("@IFSCCode",IFSCCode),
                            new SqlParameter("@Bank", Bank),
                            new SqlParameter("@ShopCountry", ShopCountry),
                            new SqlParameter("@ShopState", ShopState),
                            new SqlParameter("@ShopCity", ShopCity),
                            new SqlParameter("@ShopEmail", shopemail),
                            new SqlParameter("@ShopDistrict", ShopDistrict),
                            new SqlParameter("@ShopPinCode", ShopPinCode),
                            new SqlParameter("@IsPANVerified", IsPANVerified),
                            new SqlParameter("@IsNSVerified", IsNSVerified),
                            new SqlParameter("@IsIBAVerified", IsIBAVerified),
                            new SqlParameter("@KYCTypeId",KYCTypeId),
                            new SqlParameter("@KYCType",KYCType),
                            new SqlParameter("@IdentityProofType",IdentityProofType),
                            new SqlParameter("@IdentityProofDocument",IdentityProofDocument),
                            new SqlParameter("@AddressProofType",AddressProofType),
                            new SqlParameter("@SignatureProofType",SignatureProofType),
                            new SqlParameter("@AddressProofDocument",AddressProofDocument),
                            new SqlParameter("@SignatureProofDocument",SignatureProofDocument),
                            new SqlParameter("@BusinessEmailID",BusinessEmailID),
                            new SqlParameter("@agReqId",agReqId ),
                            new SqlParameter("@Documents",Documents),
                            new SqlParameter("@DocumentType",DocumentType),
                            new SqlParameter("@TerminalId",TerminalId),
                            new SqlParameter("@Lattitude",Lattitude),
                            new SqlParameter("@Longitude",Longitude),
                            new SqlParameter("@AgentCode",AgentID),
                            new SqlParameter("@AEPS",AEPS),
                            new SqlParameter("@MATM",MATM),
                            new SqlParameter("@StageId",StageId),
                            new SqlParameter("@Stage",Stage),
                            new SqlParameter("@Flag",Flag),
                            new SqlParameter("@ActivityType",Activity),
                            new SqlParameter("@RequestId", SqlDbType.Int){ Direction = ParameterDirection.Output },
                            new SqlParameter("@Status", SqlDbType.Int){ Direction = ParameterDirection.Output },
                            new SqlParameter("@StatusMsg", SqlDbType.VarChar,100){ Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_AgentRequest_Registration_Insert";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        Status = Convert.ToInt32(cmd.Parameters["@Status"].Value);
                        StatusMsg = Convert.ToString(cmd.Parameters["@StatusMsg"].Value);
                        RequestId = Convert.ToString(cmd.Parameters["@RequestId"].Value);
                        sqlConn.Close();
                        cmd.Dispose();
                        return Status == 1 ? true : false;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: Insert_AgentRequest: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region BindAgentVerifyddl
        public DataSet BindAgentVerifyddl()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {
                                                 new SqlParameter("@UserName", UserName),
                                                 new SqlParameter("@IsRemoved", IsRemoved),
                                                 new SqlParameter("@BCCode", BCCode),
                                                 new SqlParameter("@ClientID", ClientId),
                                                 new SqlParameter("@Flag", Flag),
                                                 new SqlParameter("@Makerstatus",Mstatus),
                                                 new SqlParameter("@Checkstatus",ChStatus),
                                                 new SqlParameter("@Authstatus",AtStatus)
                                                 };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindAgentVerification";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: BindAgentVerifyddl: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region GetAgentDetailsToProcessOnboaring
        public DataSet GetAgentDetailsToProcessOnboaring()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {
                                            new SqlParameter("@BCID",BCCode),
                                            new SqlParameter("@ClientID",ClientId),
                                            new SqlParameter("@AgentReqId", AgentReqId),
                                            new SqlParameter("@User",UserName),
                                            new SqlParameter("@IsDocUploaded",IsdocUploaded),
                                            new SqlParameter("@BCstatus",BCstatus),
                                            new SqlParameter("@Makerstatus",Mstatus),
                                            new SqlParameter("@Checkstatus",ChStatus),
                                            new SqlParameter("@Authstatus",AtStatus),
                                            new SqlParameter("@IsVerified",VerificationStatus),
                                            new SqlParameter("@ActivityType",Activity),
                                            new SqlParameter("@IsActive",IsActive),
                                            new SqlParameter("@IsRemoved",IsRemoved),
                                            new SqlParameter("@TerminalId",TerminalId),
                                            new SqlParameter("@Flag",Flag),
                                            };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_GetAgentDetailsToProcessOnboaring";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: BindBC: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region ChangeAgentStatus
        public DataSet ChangeAgentStatus()
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
                        cmd.CommandText = "SP_UpdateAgentOnboardProcess";//SP_UpdateBlockAgent
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@Mode", Mode),
                            new SqlParameter("@AgentReqID", AgentReqId)
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ChangeAgentStatus: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion
       
        #region Insert Agent master Details
        public DataSet SetInsertUpdateAgentMasterDetails()
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
                    new SqlParameter("@AgentReqId",AgentReqId),
                    new SqlParameter("@ClientId",ClientId),
                    new SqlParameter("@Salt",_RandomStringForSalt),
                    new SqlParameter("@Salt1",_RandomStringForSalt),
                    new SqlParameter("@Salt2",null),
                    new SqlParameter("@Salt3",null),
                    new SqlParameter("@Salt4",null),
                    new SqlParameter("@Password", Password)
                    };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_InsertOrUpdateAgentMaterDetails";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: SetInsertUpdateAgentMasterDetails: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }
        #endregion

        #region ChangeAgentStatus
        public DataSet ActiveDeactiveAgent()
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
                                            new SqlParameter("@AgentCode",AgentCode),
                                            new SqlParameter("@ActionType",ActionType),
                                            new SqlParameter("@ActivityType",ActivityType),
                                            new SqlParameter("@UserName",UserName),
                                            new SqlParameter("@AgentReqId",AgentReqId)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "sp_ActiveDeactiveAgents";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: SetInsertUpdateAgentMasterDetails: UserName: " + UserName + " Exception: " + Ex.Message);
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                                                    new SqlParameter("@AgReqid",AgentReqId),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_ReeditAgentDetails";
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
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: SetInsertUpdateAgentMasterDetails: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
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
        public DataSet AgentStatusReportGrid()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {
                                                     new SqlParameter("@VerificationLevel", VerificationLevel),
                                                       new SqlParameter("@OperationType", OperationType),
                                                       new SqlParameter("@Status", Status),
                                                       new SqlParameter ("@BCStatus", BCstatus),
                                                       new SqlParameter ("@MStatus", Mstatus),
                                                       new SqlParameter ("@CHStatus", ChStatus),
                                                       new SqlParameter("@Authstatus",AtStatus),
                                                       new SqlParameter("@FromDate", Fromdate),
                                                       new SqlParameter("@ToDate", Todate),
                                                       new SqlParameter("@ActivityType",ActivityType),
                                                       new SqlParameter("@User", UserName),
                                                       new SqlParameter("@Flag", Flag),
                                                };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_OverallAgentStatus";
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                             new SqlParameter("@Flag", Flag),
                             new SqlParameter("@CreatedBy",CreatedBy),
                             new SqlParameter("@ActivityType",Activity),
                             new SqlParameter("@agReqId",AgentCode),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_InsertOrUpdateAgentHandlerDetails";
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

        #region GetAgentDetails
        public DataSet GetAgentDetails()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@AgentRequest", AgentReqId),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_BindAgentData";
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
                            new SqlParameter("@AgentReqId", AgentReqId),
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Sp_GetDocuments";
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

        #region Validate Pan Details
        public bool ValidatePanDetails(out string Status, out string StatusMsg)
        {
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
                            new SqlParameter("@PanNo",PanNo),
                              new SqlParameter("@BCCode",BCCode),
                            new SqlParameter("@Status", SqlDbType.VarChar,100){ Direction = ParameterDirection.Output },
                            new SqlParameter("@StatusMsg", SqlDbType.VarChar,100){ Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "sp_AgentOnbording_ValidatePan";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        StatusMsg = Convert.ToString(cmd.Parameters["@StatusMsg"].Value);
                        sqlConn.Close();
                        cmd.Dispose();
                        return Status == "00" ? true : false;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistrationDAL: ValidatePanDetails: UserName: " + UserName + " Exception: " + Ex.Message);
                ErrorLog.DBError(Ex);
                throw;
            }
        }

        #endregion

        #region ReEdit Validate Agent
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
                              new SqlParameter("@AgentReqId",AgentReqId),
                              new SqlParameter("@Flag",Flag),
                              new SqlParameter("@Status", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
                               new SqlParameter("@RequestId", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output },
                               new SqlParameter("@StatusMsg", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output }
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "sp_AgentRequest_Validation_GetValidate";
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
                ErrorLog.AgentManagementTrace("Class : AgentRegistrationDAL.cs \nFunction : EditValidate() \nException Occured\n" + Ex.Message);
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
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                                                {
                                                  new SqlParameter("@FileID", FileID),
                                                };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_BindFileData";
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
        public DataSet GetCountryStateCityD()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@Mode", mode),
                            new SqlParameter("@CountryID", AgentCountry),
                            new SqlParameter("@PinCode", AgentPincode),
                            new SqlParameter("@StateID", AgentState),
                            new SqlParameter("@City", AgentCity),
                            new SqlParameter("@District", AgentDistrict)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_GetStateCity";
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
        public DataSet GetShopCountryStateCity()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params =
                        {
                            new SqlParameter("@Mode", mode),
                            new SqlParameter("@CountryID", ShopCountry),
                            new SqlParameter("@PinCode", ShopPinCode),
                            new SqlParameter("@StateID", ShopState),
                            new SqlParameter("@City", ShopCity),
                            new SqlParameter("@District", ShopDistrict)
                        };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "SP_GetStateCity";
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
    }
}