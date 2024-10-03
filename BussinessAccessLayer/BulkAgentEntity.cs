using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLogger;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Xml;
using System.Security.Cryptography;
using MaxiSwitch.EncryptionDecryption;
using System.Configuration;

namespace BussinessAccessLayer
{
    public class BulkAgentEntity
    {
        #region Objects Declaration
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        DataSet dataSet = null;

        AppSecurity appSecurity = null;
        public AppSecurity _AppSecurity
        {
            get { return appSecurity ?? (appSecurity = new AppSecurity()); }
            set { appSecurity = value; }
        }
        public DataTable dataTable = new DataTable();
        #endregion


        #region Property Declaration
        public int RecordID { get; set; }
        public string StatusDescription { get; set; }
        public int IsValidRecord { get; set; }
        public string Mode { get; set; }
        public int UserID { get; set; }
        public int AgentID { get; set; }
        public int RoleID { get; set; }
        public string DeviceSerialNumber { get; set; }
        public string TerminalId { get; set; }
        public string Password { get; set; }
        public string _RandomStringForSalt { get; set; }

        public string DistributorID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string MotherName { get; set; }
        public string RRN { get; set; }

        public string AgentAddress { get; set; }

        public string AccountNo { get; set; }
        public string PersonalEmail { get; set; }
        public string AadharNo { get; set; }
        public string PanNo { get; set; }
        public string GSTNo { get; set; }
        public string CashLimit { get; set; }
        public string TranID { get; set; }
        public string AgentType { get; set; }
        public string RegisteredAddress { get; set; }
        public string BCAgentID { get; set; }
        public string Country { get; set; }
 
   
        public string Pincode { get; set; }
        public string BusinessEmail { get; set; }
        public string PersonalContact { get; set; }

        public string LandlineContact { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string AgentCategory { get; set; }

        public string shopname { get; set; }
        public string referenceid { get; set; }
        public string OTPToken { get; set; }


        public string partnerkey { get; set; }

        public string MarritalStatus { get; set; }
        public int Status { get; set; }
        public int OverAllStatus { get; set; }
        public string DocRemarks { get; set; }
        public string CheckerRemark { get; set; }
        public int CheckerID { get; set; }

        public byte[] ProfileImage { get; set; }
        public byte[] Logo { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string IsRemoved { get; set; }


        public int DocumentID { get; set; }
        public byte[] DocData { get; set; }
        public string MakerRemark { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }

        public float TransferAmount { get; set; }
        public Decimal Amount { get; set; }
        public Decimal BalanceAmount { get; set; }
        public Decimal PrePaymentAmount { get; set; }
        public float Commission { get; set; }
        public string IdCardType { get; set; }
        public string IdCardValue { get; set; }
        public string Dob { get; set; }

        public string DateOfBirth { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        

       public string PassPortNo { get; set; }
        public string NoOfTransactionPerDay { get; set; }
        public string TransferAmountPerDay { get; set; }
        public string AgentWalletTransfer { get; set; }
        public string AgentAccountName { get; set; }
        public string IsUPIPartner { get; set; }
        public string WalletLoadingWithdrawal { get; set; }
        public string BCAgentType { get; set; }
        public string ParentAgentID { get; set; }
        public string RegistrationType { get; set; }
        public string CompanyName { get; set; }
        public string Area { get; set; }
        public string District { get; set; }
        public string LocalAddress { get; set; }
        public string LocalArea { get; set; }
        public int LocalCountry { get; set; }
      /////  public int LocalState { get; set; }
        /////public int LocalCity { get; set; }
        public string LocalDistrict { get; set; }
        public string LocalPincode { get; set; }
        public string AlternetNo { get; set; }
        public string ShopAddress { get; set; }
        public string ShopArea { get; set; }
        //public int ShopCountry { get; set; }
        //public int ShopState { get; set; }

        public string Shopstate { get; set; }
        public string ShopDistrict { get; set; }
        //public int ShopCity { get; set; }

        public string Shopcity { get; set; }
        public string ShopPincode { get; set; }
        public string IdentityProofType { get; set; }


        public string IdentityProofDocument { get; set; }
        public string AddressProofType { get; set; }
        public string AddressProofDocument { get; set; }
        public string SignatureProofType { get; set; }
        public string SignatureProofDocument { get; set; }
        public int IsValidaddress { get; set; }
        public int IsValidProof { get; set; }
        public string Full_VA_Number { get; set; }
        public string Status_Reason { get; set; }
        public string Short_Name { get; set; }
        public string CIF { get; set; }
        public string VA_Number { get; set; }
        public string Comments { get; set; }
        public DateTime RequestDate { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string Flag { get; set; }

        public int? ForAEPS { get; set; }
        public int? ForBBPS { get; set; }
        public int? ForDMT { get; set; }
        public int? ForMATM { get; set; }
        public int? ForRecharge { get; set; }

        public string WeeklyOff { get; set; }
        public string CorporateIndividualBC { get; set; }
        public string BankReferenceNumber { get; set; }
        public string Provider { get; set; }
        public string ConnectivityType { get; set; }
        public string DeviceName { get; set; }
        public string EntityType { get; set; }
        public string ShopClosesat { get; set; }
        public string ShopOpensat { get; set; }
        public string AlternateOccupationDescription { get; set; }
        public string DateofPassing { get; set; }
        public string InstituteName { get; set; }
        public string Course { get; set; }
        public string AlternateOccupationType { get; set; }
        public string HighestEducationQualification { get; set; }
        public string PhysicallyHandicapped { get; set; }
        public string SpouseName { get; set; }
        public string FatherName { get; set; }
        public string Category { get; set; }
        public string AepsIdentificationCode { get; set; }

        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string AggegatorCode { get; set; }
        public string AggegatorName { get; set; }
        public string BCName { get; set; }
        public string BCID { get; set; }
        public string ContactNumber { get; set; }

        public string AlternateContactNumber { get; set; }
        public string BCMaxID { get; set; }
        public string User { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string _strBCFullName { get; set; }
        public string Remarks { get; set; }
        public string _fileLineNo { get; set; }
        public int SFlag { get; set; }
        public string VerificationStatus { get; set; }
        public string IsActive { get; set; }
        public string IsdocUploaded { get; set; }
        public string BankStatus { get; set; }
        public string ActionType { get; set; }
        public string Clientcode { get; set; }

        public string ClientID { get; set; }
        public string Franchisecode { get; set; }

        public string FranchiseID { get; set; }

        public int RegistrationID { get; set; }
        public string AgentRegistrationType { get; set; }
        public string LocalCityName { get; set; }
        public string ShopCityName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string ShopStateName { get; set; }
        public string LocalStateName { get; set; }
        public string SessionToken { get; set; }
        public string ViD { get; set; }
        public string uidtoken { get; set; }
        public string paymentmode { get; set; }
        public string parentTranid { get; set; }
        public string UTRNo { get; set; }
        public string RequestType { get; set; }
        public string RequestID { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentStatusId { get; set; }
        public string ParentID { get; set; }
        public string ParentTransactionID { get; set; }

        public string TypeOfCommission { get; set; }
        public string ThresholdAmount { get; set; }
        public int? AlertEmail { get; set; }
       /// public int? AlertSMS { get; set; }
        public string Limited { get; set; }
        public string UnLimited { get; set; }
        public string AlertAmount { get; set; }

        public string eKYCUserID { get; set; }
        public string eKYCUserName { get; set; }

        public string AadhaarNo { get; set; }
        public string DeviceCertExpiryDate { get; set; }
        public string DeviceDataXml { get; set; }
        public string DeviceHmacXml { get; set; }
        public string DeviceSessionKey { get; set; }
        public int DeviceType { get; set; }
        public string DeviceVersionNumber { get; set; }
        public string AadhaarNumberType { get; set; }
        public string Wadh { get; set; }
        public int EkycStatus { get; set; }

        public string Description { get; set; }
        public string DMTBCAgentID { get; set; }
        public string IsagentIdUpdated { get; set; }

        public string DeviceTypeId { get; set; }
        public string DeviceId { get; set; }
        public string ZOM { get; set; }
        public string ZOMID { get; set; }
        public string InduskenID { get; set; }
        public string DEviceIEMINo { get; set; }

        public int IsGSTRegistered { get; set; }
        public string IsRecycle { get; set; }
        public string ChannelType { get; set; }

        public string RequestNum { get; set; }
        public string AgentRefID { get; set; }

        public string KYCToken { get; set; }

        public string _AgentPan { get; set; }

        public string _AgentAdProof { get; set; }

        public string _AgentAdType { get; set; }

        /// <summary>
        /// ///////////////////////////////////////
        /// </summary>
        public string ContactNo { get; set; }/// <summary>
                                             /// //
        public string AlertMail { get; set; }
        public string Unlimited { get; set; }
        public string limited { get; set; }
        public string AlertStartfrom { get; set; }
        public string BBPS { get; set; }
        public string DMT { get; set; }
        public string MATM { get; set; }
        public string AEPS { get; set; }
        public string ReCharge { get; set; }

       public string AlertSMS { get; set; }
        public string RecordStatus { get; set; }
        public string FileStatus { get; set; }
      
        public string Createdon { get; set; }
        public string Isremoved { get; set; }
        public string LocalState { get; set; }
        public string LocalCity { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string LandlineNo { get; set; }

        public string Bank { get; set; }

        public string devicetype { get; set; }

        public string isgstregistered { get; set; }

        public string DeviceCode { get; set; }
        public string PopulationGroup { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public string AlternateNo { get; set; }
        public string ShopCountry { get; set; }

        public string ShopState { get; set; }

        public string ShopCity { get; set; }

        public string ShopEmailId { get; set; }

        public string ShopContactNo { get; set; }

        public string ShopLandlineNo { get; set; }

        public string ShopAlternateNo { get; set; }

        public string AgentRefId { get; set; }

        public string MsgID { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }

        

        /// </summary>
        #endregion

        #region ImportBulkKycValidate
        public void ImportBulkKycValidate(out string StatusCode, out string SatatusDesc)
        {
            StatusCode = string.Empty; SatatusDesc = string.Empty;
            try
            {
                SqlParameter[] _Params = {
                                             new SqlParameter("@AadharNo", AadharNo),
                                             new SqlParameter("@PanNo", PanNo),
                                             new SqlParameter("@ContactNo",PersonalContact),
                                             new SqlParameter("@PersonalEmail",PersonalEmail),
                                             new SqlParameter("@OutStatus", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output },
                                             new SqlParameter("@OutStatusMsg", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
                                         };
                //dataSet = _dbAccess.SelectRecordsWithOutParams("Proc_BulkCheckAgent", _Params, out StatusCode, out SatatusDesc);
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : ImportEntity.cs \nFunction : ImportBulkKycValidate() \nException Occured\n" + Ex.Message);
            }
        }
        #endregion

        #region ImportBulkKycValidateData
        public string ImportBulkKycValidateData(out string StatusCode, out string SatatusDesc)
        {
            StatusCode = string.Empty; SatatusDesc = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                    {
                        SqlParameter[] _Params = {

                                             new SqlParameter("@flag",Flag),
                                             new SqlParameter("@State",State),
                                             new SqlParameter("@City",City),
                                             new SqlParameter("@ClientID",ClientID),
                                             new SqlParameter("@FranchiseID",BCID),
                                             new SqlParameter("@AadharNo",AadharNo),
                                             new SqlParameter("@PanNo",PanNo),
                                             new SqlParameter("@AgentPincode",Pincode),
                                             new SqlParameter("@PersonalEmail",EmailId),
                                             new SqlParameter("@ContactNo",ContactNo),
                                             new SqlParameter("@AgentCode",AgentCode),
                                             new SqlParameter("@TerminalID",TerminalId),
                                             new SqlParameter("@OutStatus", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output },
                                             new SqlParameter("@OutStatusMsg", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
                                         };
                        cmd.Connection = sqlConn;
                        cmd.CommandText = "Proc_BulkCheckAgent";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(_Params);
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        StatusCode = Convert.ToString(cmd.Parameters["@OutStatus"].Value);
                        SatatusDesc = Convert.ToString(cmd.Parameters["@OutStatusMsg"].Value);
                        //StatusMsg = Convert.ToString(cmd.Parameters["@Status_Out"].Value);
                        sqlConn.Close();
                        cmd.Dispose();
                        
                    }
                }
                //dataSet = _dbAccess.SelectRecordsWithOutParams("Proc_BulkCheckAgent", _Params, out StatusCode, out SatatusDesc);
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : ImportEntity.cs \nFunction : ImportBulkKycValidate() \nException Occured\n" + Ex.Message);
            }
            return StatusCode;
        }
        #endregion
    }
}
