using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Net;
using BussinessAccessLayer;
using AppLogger;
using ClosedXML.Excel;
using AgentVerification;
using System.Configuration;
using System.Threading;
using System.Data.OleDb;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Ionic.Zip;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SOR.Pages.Agent
{
    public partial class AgentRegistration : System.Web.UI.Page
    {
        #region Object Declarations
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();
        AgentRegistrationAPI registrationAPI = new AgentRegistrationAPI();
        DbAccess dbAccess = new DbAccess();
        public clsCustomeRegularExpressions _CustomeRegExpValidation = new clsCustomeRegularExpressions();
        ImportEntity importEntity = new ImportEntity();
        public string pathId, PathAdd, PathSig;
        DataTable TypeTable_BulkAgentRegistrationt = new DataTable();

        BulkAgentEntity _BulkAgentEntity = new BulkAgentEntity();
        ImportEntity _ImportEntity = new ImportEntity();
        OleDbConnection _OleDbConnection = null;
        DateTime CreatedOn = DateTime.Now;
        public ExportFormat _exportFormat = null;
        DataTable _dtAudit = new DataTable();

        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];

        public ExportFormat exportFormat
        {
            get { if (_exportFormat == null) _exportFormat = new ExportFormat(); return _exportFormat; }
            set { _exportFormat = value; }
        }
        StringBuilder fileRecords = null;
        StringBuilder _fileRecords
        {
            get { if (fileRecords == null) fileRecords = new StringBuilder(); return fileRecords; }
            set { fileRecords = value; }
        }
        StringBuilder strLogMessage_3 = null;
        StringBuilder _strLogMessage_3
        {
            get { if (strLogMessage_3 == null) strLogMessage_3 = new StringBuilder(); return strLogMessage_3; }
            set { strLogMessage_3 = value; }
        }
        StringBuilder strLogMessage_2 = null;
        StringBuilder _strLogMessage_2
        {
            get { if (strLogMessage_2 == null) strLogMessage_2 = new StringBuilder(); return strLogMessage_2; }
            set { strLogMessage_2 = value; }
        }
        StringBuilder strLogMessage_1 = null;
        StringBuilder _strLogMessage_1
        {
            get { if (strLogMessage_1 == null) strLogMessage_1 = new StringBuilder(); return strLogMessage_1; }
            set { strLogMessage_1 = value; }
        }

        string
    _FileImport = string.Empty,
    _FileRecord = string.Empty,
    _fileExtension = string.Empty,
    _BCIDConcate = string.Empty;

        string
        _Domain = string.Empty,
            _PartnerKey = string.Empty,
        _OTPSent = string.Empty;
        string filePath = null;
        int _ImportFileFields = 0;

        int _successful = 0,
      _unsuessful = 0,
      _reocrdsProcessed = 0,
      _ActiveDeactiveAgentCount = 0,
      _AgentImportFileFields = 0,
      _StatusFlag = 0,
      _Flag = 0;

        int _dupRcdsCnt = 0;

        string responsecode = string.Empty,
         _OtpToken = string.Empty,
            AgentRefID = string.Empty,
            _KYCToken = string.Empty,
            statusmsg = string.Empty,
            _requestID = string.Empty;
        StringBuilder ConnctionString = null;
        StringBuilder _ConnctionString
        {
            get { if (ConnctionString == null) ConnctionString = new StringBuilder(); return ConnctionString; }
            set { ConnctionString = value; }
        }

        string
           DistributorID = null,
           AgentName = null,
           AadharNo = null,
           PanNo = null,
           GSTNo = null,
           PersonalEmail = null,
           BusinessEmail = null,
           ContactNo = null,
           Landlineno = null,
           pincode = null,
           idcardvalue = null,
           Dob = null,
           AccountNumber = null,
           IFSCCode = null,
           NoOfTransactionPerDay = null,
           AgentaccntName = null,
           State = null,
           City = null,
           AlternateNo = null,
           ThresholdAmount = null,
           TransferAmountPerDay = null;
        string
      _strAlertMessage_Header = null,
      _strAlertMessage_Success = null,
      _strAlertMessage_UnSuccess = null,
      _strAlertMessage_Total = null,
      _fileLineNo = null;



        string
        _FromDate = null,
        _ToDate = null,
        _AgentName = null,
        _AgentId = null,
         ClientID = null,
        _MobileNumber = null,
        _clientId = null,
        _FranchiseId = null,
        _Status = null,
        _ChannelID = null,
        _Username = null,
           CreatedBy = null;
        #endregion
        
        #region Objects Declaration
        EmailAlerts _EmailAlerts = new EmailAlerts();
        
        EmailSMSAlertscs _EmailSMSAlertscs = new EmailSMSAlertscs();
        public string UserName { get; set; }
        AppSecurity appSecurity = null;
        public AppSecurity _AppSecurity
        {
            get { if (appSecurity == null) appSecurity = new AppSecurity(); return appSecurity; }
            set { appSecurity = value; }
        }
        int RequestId = 0;

        #endregion;

        #region Constructor
        public AgentRegistration()
        {
            try
            {
                TypeTable_BulkAgentRegistrationt = new DataTable();
                if (TypeTable_BulkAgentRegistrationt.Columns.Count == 0)
                {
                    TypeTable_BulkAgentRegistrationt.Columns.Add("BCID");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("Aggregatorid");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("AEPS");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("MATM");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("DMT");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("PanNo");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("FirstName");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("MiddleName");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("LastName");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("Gender");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("AadharNo");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("AgentCategory");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("Dob");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("AccountNumber");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("IFSCCode");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("PassPortNo");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("DeviceCode");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("PopulationGroup");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("Address");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("Country");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("State");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("District");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("City");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("Pincode");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("EmailId");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ContactNo");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("LandlineNo");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("AlternateNo");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ShopAddress");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ShopCountry");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ShopState");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ShopDistrict");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ShopCity");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ShopPincode");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ShopEmailId");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ShopContactNo");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ShopLandlineNo");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("ShopAlternateNo");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("IdentityProofType");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("IdentityProofDocument");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("AddressProofType");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("AddressProofDocument");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("SignatureProofType");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("SignatureProofDocument");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("AgentRefId");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("AgentCode");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("TerminalId");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("IsValidRecord", typeof(Int32));
                    TypeTable_BulkAgentRegistrationt.Columns.Add("RecordStatus");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("FileStatus");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("RecordID", typeof(Int32));
                    TypeTable_BulkAgentRegistrationt.Columns.Add("Status", typeof(long));
                    TypeTable_BulkAgentRegistrationt.Columns.Add("StatusDescription");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("MSGID");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("Lattitude");
                    TypeTable_BulkAgentRegistrationt.Columns.Add("Longitude");
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementError(Ex);

            }
        }
        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)

        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "AgentRegistration.aspx", "18");
                    if (!HasPagePermission)
                    {
                        try
                        {
                            Response.Redirect(ConfigurationManager.AppSettings["RedirectTo404"].ToString(), false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        catch (ThreadAbortException)
                        {
                        }
                    }
                    else
                    {
                        Page.Form.Attributes.Add("enctype", "multipart/form-data");
                        if (!IsPostBack == true)
                        {
                           //  Processor processor = new Processor();

                            //if (processor.PanVerification(txtPANNo.Text.Trim(), out RespCode, out RespMessage, out firstName, out middleName, out lastName))
                            //{
                                
                            //}

                          
                             //processor.PanVerification(txtPANNo.Text.Trim(), out string RespCode, out string RespMessage, out string firstName, out string middleName, out string lastName);
                            //processor.NameScreening(txtFirstName.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim(), "", "", "", "","", out string RespCode, out string RespMessage, out int MsgId);
                            txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            BindDropdownCountryState();
                            BindShopCountryState();
                            FillBc();
                            FillGrid(EnumCollection.EnumBindingType.BindGrid);
                            UserPermissions.RegisterStartupScriptForNavigationList("6", "18", "AGReg", "docket-tab");
                            Page.Form.Attributes.Add("enctype", "multipart/form-data");
                        }
                        else
                        {
                            if (hidimgId.Value != null)
                            {
                                pathId = hidimgId.Value;
                            }
                            if (hidimgAdd.Value != null)
                            {
                                PathAdd = hidimgAdd.Value;
                            }
                            if (hidimgSig.Value != null)
                            {
                                PathSig = hidimgSig.Value;
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        Response.Redirect(ConfigurationManager.AppSettings["RedirectToLogin"].ToString(), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    catch (ThreadAbortException)
                    {
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("AgentRegistration: Page_Load: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Bind Masters - Dropdowns
        public void BindDropdownCountryState()
        {
            try
            {
                _AgentRegistrationDAL.mode = (int)(EnumCollection.StateCityMode.PinCode);
                DataSet dsCountry = _AgentRegistrationDAL.GetCountryStateCityD();
                if (dsCountry != null && dsCountry.Tables.Count > 0 && dsCountry.Tables[0].Rows.Count > 0)
                {
                    ddlCountry.DataSource = dsCountry;
                    ddlCountry.DataValueField = "Country";
                    ddlCountry.DataTextField = "Country";
                    ddlCountry.DataBind();

                    ddlState.DataSource = dsCountry;
                    ddlState.DataValueField = "States";
                    ddlState.DataTextField = "States";
                    ddlState.DataBind();

                    ddlDistrict.DataSource = dsCountry;
                    ddlDistrict.DataValueField = "District";
                    ddlDistrict.DataTextField = "District";
                    ddlDistrict.DataBind();

                    ddlCity.DataSource = dsCountry;
                    ddlCity.DataValueField = "City";
                    ddlCity.DataTextField = "City";
                    ddlCity.DataBind();
                }
                else
                {
                    _AgentRegistrationDAL.mode = (int)(EnumCollection.StateCityMode.ALLStateCity);
                    DataSet ds = _AgentRegistrationDAL.GetCountryStateCityD();

                    ddlCountry.DataSource = ds.Tables[0];
                    ddlCountry.DataValueField = "Country";
                    ddlCountry.DataTextField = "Country";
                    ddlCountry.DataBind();
                    ddlCountry.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlState.DataSource = ds.Tables[1];
                    ddlState.DataValueField = "States";
                    ddlState.DataTextField = "States";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlDistrict.DataSource = ds.Tables[2];
                    ddlDistrict.DataValueField = "District";
                    ddlDistrict.DataTextField = "District";
                    ddlDistrict.DataBind();
                    ddlDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlCity.DataSource = ds.Tables[3];
                    ddlCity.DataValueField = "City";
                    ddlCity.DataTextField = "City";
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: BindDropdownCountryState(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        public void BindShopCountryState()
        {
            try
            {
                _AgentRegistrationDAL.mode = (int)(EnumCollection.StateCityMode.PinCode);
                DataSet ds = _AgentRegistrationDAL.GetShopCountryStateCity();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    ddlshopCountry.DataSource = ds;
                    ddlshopCountry.DataValueField = "Country";
                    ddlshopCountry.DataTextField = "Country";
                    ddlshopCountry.DataBind();

                    ddlShopState.DataSource = ds;
                    ddlShopState.DataValueField = "States";
                    ddlShopState.DataTextField = "States";
                    ddlShopState.DataBind();

                    ddlShopDistrict.DataSource = ds;
                    ddlShopDistrict.DataValueField = "District";
                    ddlShopDistrict.DataTextField = "District";
                    ddlShopDistrict.DataBind();

                    ddlShopCity.DataSource = ds;
                    ddlShopCity.DataValueField = "City";
                    ddlShopCity.DataTextField = "City";
                    ddlShopCity.DataBind();
                }
                else
                {
                    _AgentRegistrationDAL.mode = (int)(EnumCollection.StateCityMode.ALLStateCity);
                    DataSet dse = _AgentRegistrationDAL.GetShopCountryStateCity();

                    ddlshopCountry.DataSource = dse.Tables[0];
                    ddlshopCountry.DataValueField = "Country";
                    ddlshopCountry.DataTextField = "Country";
                    ddlshopCountry.DataBind();
                    ddlshopCountry.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlShopState.DataSource = dse.Tables[1];
                    ddlShopState.DataValueField = "States";
                    ddlShopState.DataTextField = "States";
                    ddlShopState.DataBind();
                    ddlShopState.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlShopDistrict.DataSource = dse.Tables[2];
                    ddlShopDistrict.DataValueField = "District";
                    ddlShopDistrict.DataTextField = "District";
                    ddlShopDistrict.DataBind();
                    ddlShopDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlShopCity.DataSource = dse.Tables[3];
                    ddlShopCity.DataValueField = "City";
                    ddlShopCity.DataTextField = "City";
                    ddlShopCity.DataBind();
                    ddlShopCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: BindDropdownCountryState(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        public void FillBc()
        {
            try
            {
                ddlbcCode.Items.Clear();
                ddlbcCode.DataSource = null;
                ddlbcCode.DataBind();
                string UserName = Session["Username"].ToString();
                int IsRemoved = 0;
                int IsActive = 1;
                int IsdocUploaded = 1;
                int VerificationStatus = 1;
                DataTable dsbc = _AgentRegistrationDAL.GetBC(UserName, VerificationStatus, IsActive, IsRemoved, null, IsdocUploaded);
                if (dsbc != null && dsbc.Rows.Count > 0 && dsbc.Rows.Count > 0)
                {
                    if (dsbc.Rows.Count == 1)
                    {
                        ddlbcCode.DataSource = dsbc;
                        ddlbcCode.DataValueField = "BCCode";
                        ddlbcCode.DataTextField = "BCName";
                        ddlbcCode.DataBind();
                    }
                    else
                    {
                        ddlbcCode.DataSource = dsbc;
                        ddlbcCode.DataValueField = "BCCode";
                        ddlbcCode.DataTextField = "BCName";
                        ddlbcCode.DataBind();
                        ddlbcCode.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlbcCode.DataSource = null;
                    ddlbcCode.DataBind();
                    ddlbcCode.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Agent Registration');", true);
                return;
            }
        }
        public void FillAggregator()
        {
            try
            {
                ddlaggregatorCode.Items.Clear();
                ddlaggregatorCode.DataSource = null;
                ddlaggregatorCode.DataBind();
                string UserName = Session["Username"].ToString();
                int IsRemoved = 0;
                int IsActive = 1;
                int IsdocUploaded = 1;
                int VerificationStatus = 1;
                string ClientID = ddlbcCode.SelectedValue.ToString();
                DataTable dsbc = _AgentRegistrationDAL.GetAggregator(UserName, VerificationStatus, IsActive, IsRemoved, ClientID, IsdocUploaded);
                if (dsbc != null && dsbc.Rows.Count > 0 && dsbc.Rows.Count > 0)
                {
                    if (dsbc.Rows.Count == 1)
                    {
                        ddlaggregatorCode.DataSource = dsbc;
                        ddlaggregatorCode.DataValueField = "aggcode";
                        ddlaggregatorCode.DataTextField = "agname";
                        ddlaggregatorCode.DataBind();
                    }
                    else
                    {
                        ddlaggregatorCode.DataSource = dsbc;
                        ddlaggregatorCode.DataValueField = "aggcode";
                        ddlaggregatorCode.DataTextField = "agname";
                        ddlaggregatorCode.DataBind();
                        ddlaggregatorCode.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlaggregatorCode.DataSource = null;
                    ddlaggregatorCode.DataBind();
                    ddlaggregatorCode.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Agent Registration');", true);
                return;
            }
        }
        #endregion

        #region Dropdown Events
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlDistrict.DataSource = null;

                _AgentRegistrationDAL.AgentState = string.Empty;
                _AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.District;
                _AgentRegistrationDAL.AgentState = ddlState.SelectedValue.ToString();
                DataSet ds_District = _AgentRegistrationDAL.GetCountryStateCityD();
                if (ds_District != null && ds_District.Tables.Count > 0 && ds_District.Tables[0].Rows.Count > 0)
                {

                    ddlDistrict.DataSource = ds_District;
                    ddlDistrict.DataValueField = "District";
                    ddlDistrict.DataTextField = "District";
                    ddlDistrict.DataBind();
                    ddlDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    ddlDistrict.DataSource = null;
                    ddlDistrict.DataBind();
                    ddlDistrict.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: ddlState_SelectedIndexChanged(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void ddlShopState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.District;
                _AgentRegistrationDAL.ShopState = ddlShopState.SelectedValue.ToString();
                DataSet ds_District = _AgentRegistrationDAL.GetShopCountryStateCity();
                if (ds_District != null && ds_District.Tables.Count > 0 && ds_District.Tables.Count > 0)
                {
                    ddlShopDistrict.DataSource = ds_District;
                    ddlShopDistrict.DataValueField = "District";
                    ddlShopDistrict.DataTextField = "District";
                    ddlShopDistrict.DataBind();
                    ddlShopDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    ddlShopDistrict.DataSource = null;
                    ddlShopDistrict.DataBind();
                    ddlShopDistrict.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: ddlShopState_SelectedIndexChanged(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region FillGrid
        public DataSet FillGrid(EnumCollection.EnumBindingType enumBinding)
        {
            DataSet ds = null;
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                SetProperties();
                _AgentRegistrationDAL.UserName = Session["UserName"].ToString();
                _AgentRegistrationDAL.Flag = (int)enumBinding;
                ds = _AgentRegistrationDAL.GetAgentRequestList();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvAgentOnboard.DataSource = ds.Tables[0];
                    gvAgentOnboard.DataBind();
                    btnExportCSV.Visible = true;
                    btndownload.Visible = true;
                }
                else
                {
                    gvAgentOnboard.DataSource = null;
                    gvAgentOnboard.DataBind();
                    btndownload.Visible = false;
                    btnExportCSV.Visible = false;
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: FillGrid(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return ds;
        }

        private void SetProperties()
        {
            try
            {
                //if (ddlBucketId.SelectedValue == "1" && ddlRequestStatus.SelectedValue == "0")
                //{
                //    _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerPending);
                //}

                //else if (ddlBucketId.SelectedValue == "1" && ddlRequestStatus.SelectedValue == "1")
                //{
                //    _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerApprove);
                //}

                //else if (ddlBucketId.SelectedValue == "1" && ddlRequestStatus.SelectedValue == "2")
                //{
                //    _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerDecline);
                //}
                //else if (ddlBucketId.SelectedValue == "2" && ddlRequestStatus.SelectedValue == "0")
                //{
                //    _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                //}

                //else if (ddlBucketId.SelectedValue == "2" && ddlRequestStatus.SelectedValue == "1")
                //{
                //    _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                //}

                //else if (ddlBucketId.SelectedValue == "2" && ddlRequestStatus.SelectedValue == "2")
                //{
                //    _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerDecline);
                //}
                //else if (ddlBucketId.SelectedValue == "3" && ddlRequestStatus.SelectedValue == "0")
                //{
                //    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                //}

                //else if (ddlBucketId.SelectedValue == "3" && ddlRequestStatus.SelectedValue == "1")
                //{
                //    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                //}

                //else if (ddlBucketId.SelectedValue == "3" && ddlRequestStatus.SelectedValue == "2")
                //{
                //    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerDecline);
                //}
                //else if (ddlBucketId.SelectedValue == "0" && ddlRequestStatus.SelectedValue == "0")
                //{
                //    _AgentRegistrationDAL.Prestatus = Convert.ToString((int)EnumCollection.Onboarding.Pending);
                //}
                //else if (ddlBucketId.SelectedValue == "0" && ddlRequestStatus.SelectedValue == "1")
                //{
                //    _AgentRegistrationDAL.Prestatus = Convert.ToString((int)EnumCollection.Onboarding.Approve);
                //}
                //else if (ddlBucketId.SelectedValue == "0" && ddlRequestStatus.SelectedValue == "2")
                //{
                //    _AgentRegistrationDAL.Prestatus = Convert.ToString((int)EnumCollection.Onboarding.Decline);
                //}
                if (ddlRequestStatus.SelectedValue == "0")
                {
                    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                }

                else if (ddlRequestStatus.SelectedValue == "1")
                {
                    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                }

                else if (ddlRequestStatus.SelectedValue == "2")
                {
                    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerDecline);
                }
                //_BCEntity.RequestTypeId = ddlRequestType.SelectedValue != "-1" ? (ddlRequestType.SelectedValue) : null;
                //_BCEntity.PanNo = !string.IsNullOrEmpty(txtPanNoF.Text) ? txtPanNoF.Text.Trim() : null;

                _AgentRegistrationDAL.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _AgentRegistrationDAL.RequestTypeId = ddlRequestType.SelectedValue != "-1" ? (ddlRequestType.SelectedValue) : null;
                _AgentRegistrationDAL.BucketId = ddlBucketId.SelectedValue != "-1" ? (ddlBucketId.SelectedValue) : null;
                _AgentRegistrationDAL.RequestStatusId = ddlRequestStatus.SelectedValue != "-1" ? (ddlRequestStatus.SelectedValue) : null;
                //_AgentRegistrationDAL.AgentCode = !string.IsNullOrEmpty(txtAgentCode.Value) ? txtAgentCode.Value.Trim() : null;
                _AgentRegistrationDAL.PanNo = !string.IsNullOrEmpty(txtPanNoF.Value) ? txtPanNoF.Value.Trim() : null;

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: SetProperties(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Helper Methods
        public void ClearAllControls()
        {
            try
            {
                txtFirstName.Text = null;
                txtMiddleName.Text = null;
                txtLastName.Text = null;
                txtEmailID.Text = null;
                txtPinCode.Text = null;
                txtPANNo.Text = null;
                txtGSTNo.Text = null;
                txtRegisteredAddress.Text = null;
                txtContactNo.Text = null;
                txtLandlineNo.Text = null;
                txtaadharno.Text = null;
                ddlCountry.SelectedValue = null;
                ddlState.SelectedValue = null;
                ddlCity.SelectedValue = null;
                ddlDistrict.SelectedValue = null;
                ddlShopDistrict.SelectedValue = null;
                txtAccountNumber.Text = null;
                txtIFsccode.Text = null;
                txtLandlineNo.Text = null;
                txtAlterNateNo.Text = null;
                dvfield_PANNo.Visible = true;
                txtaadharno.Text = null;
                chkAEPS.Checked = false;
                chkMATM.Checked = false;
                ddlclient.SelectedValue = null;
                ddlshopCountry.SelectedValue = null;
                ddlShopState.SelectedValue = null;
                ddlShopCity.SelectedValue = null;
                txtshopadd.Text = null;
                txtshopadd.Text = null;
                txtshoppin.Text = null;
                TextBox5.Text = null;
                TextBox6.Text = null;
                txtPass.Text = null;
                ddlbcCode.SelectedValue = null;
                ddlCategory.ClearSelection();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : ClearAllControls() \nException Occured\n" + Ex.Message);
            }
        }

        protected void PolulateSummary()
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | PolulateSummary() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (HidAGID.Value != null || !string.IsNullOrEmpty(txtPANNo.Text.Trim()))
                {
                    _AgentRegistrationDAL.PanNo = txtPANNo.Text.Trim();
                    _AgentRegistrationDAL.RequestId = Convert.ToInt32(HidAGID.Value);
                    _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.Export;
                    DataSet ds_Receipt = _AgentRegistrationDAL.GetAgentRequestList();
                    if (ds_Receipt != null && ds_Receipt.Tables.Count > 0 && ds_Receipt.Tables[0].Rows.Count > 0)
                    {
                        ErrorLog.AgentManagementTrace("AgentRegistration: PolulateSummary: Failed - Fetch Agent Registration Details Using RequestID/PAN From DB. UserName: " + UserName + " PanNo: " + _AgentRegistrationDAL.PanNo + " RequestId: " + HidAGID.Value);
                        txtaadharno.Text = ds_Receipt.Tables[0].Rows[0]["AadharNo"].ToString();
                        txtacc.Text = ds_Receipt.Tables[0].Rows[0]["AccountNumber"].ToString();
                        txtAccountNumber.Text = ds_Receipt.Tables[0].Rows[0]["AccountNumber"].ToString();
                        txtadd.Text = ds_Receipt.Tables[0].Rows[0]["AgentAddress"].ToString();
                        txtbcname.Text = ds_Receipt.Tables[0].Rows[0]["AgentName"].ToString();
                        txtagentcategory.Text = ds_Receipt.Tables[0].Rows[0]["AgentCategory"].ToString();
                        ddlcl.Text = ds_Receipt.Tables[0].Rows[0]["ClientName"].ToString();
                        txtbccode.Text = ds_Receipt.Tables[0].Rows[0]["bcname"].ToString();
                        ddlcitys.Text = ds_Receipt.Tables[0].Rows[0]["LocalCity"].ToString();
                        txtcontact.Text = ds_Receipt.Tables[0].Rows[0]["ContactNo"].ToString();
                        txtshopcontact.Text = ds_Receipt.Tables[0].Rows[0]["ShopContact"].ToString();
                        txtLandlineNo.Text = ds_Receipt.Tables[0].Rows[0]["LandlineNo"].ToString();
                        ddlcountrys.Text = ds_Receipt.Tables[0].Rows[0]["LocalCountry"].ToString();
                        ddlstates.Text = ds_Receipt.Tables[0].Rows[0]["LocalState"].ToString();
                        ddldist.Text = ds_Receipt.Tables[0].Rows[0]["AgentDistrict"].ToString();
                        txtemail.Text = ds_Receipt.Tables[0].Rows[0]["PersonalEmailID"].ToString();
                        TextBox1.Text = ds_Receipt.Tables[0].Rows[0]["ShopAddress"].ToString();
                        TextBox3.Text = ds_Receipt.Tables[0].Rows[0]["ShopCity"].ToString();
                        TextBox3.Text = ds_Receipt.Tables[0].Rows[0]["LocalCountry"].ToString();
                        TextBox2.Text = ds_Receipt.Tables[0].Rows[0]["ShopState"].ToString();
                        TextBox5.Text = ds_Receipt.Tables[0].Rows[0]["ShopContact"].ToString();
                        DDlgen.Text = ds_Receipt.Tables[0].Rows[0]["Gender"].ToString();
                        txtgst.Text = ds_Receipt.Tables[0].Rows[0]["GSTNo"].ToString();
                        txtpan.Text = ds_Receipt.Tables[0].Rows[0]["PanNo"].ToString();
                        txtpin.Text = ds_Receipt.Tables[0].Rows[0]["AgentPinCode"].ToString();
                        rectextshopoin.Text = ds_Receipt.Tables[0].Rows[0]["ShopPinCode"].ToString();
                        txtaadh.Text = ds_Receipt.Tables[0].Rows[0]["AadharNo"].ToString();
                        txtifsc.Text = ds_Receipt.Tables[0].Rows[0]["IFSCCode"].ToString();
                        txtAgrdob.Text = ds_Receipt.Tables[0].Rows[0]["AgentDOB"].ToString();
                        string idthumb = ds_Receipt.Tables[0].Rows[0]["IdentityProofDocument"].ToString();
                        string FileThumbnailId = Path.GetDirectoryName(idthumb) + "\\" + Path.GetFileNameWithoutExtension(idthumb) + "_Thumbnail.png";
                        pathId = "../../" + FileThumbnailId;
                        Session["pdfPathID"] = idthumb;
                        hidimgId.Value = pathId;
                        string Addthumb = ds_Receipt.Tables[0].Rows[0]["AddressProofDocument"].ToString();
                        string FileThumbnailAdd = Path.GetDirectoryName(Addthumb) + "\\" + Path.GetFileNameWithoutExtension(Addthumb) + "_Thumbnail.png";
                        PathAdd = "../../" + FileThumbnailAdd;
                        Session["pdfPathAdd"] = Addthumb;
                        hidimgAdd.Value = PathAdd;
                        string Sigthumb = ds_Receipt.Tables[0].Rows[0]["SignatureProofDocument"].ToString();
                        string FileThumbnailSig = Path.GetDirectoryName(Sigthumb) + "\\" + Path.GetFileNameWithoutExtension(Sigthumb) + "_Thumbnail.png";
                        PathSig = "../../" + FileThumbnailSig;
                        Session["pdfPathSig"] = Sigthumb;
                        hidimgSig.Value = PathSig;
                        ViewState["agReqId"] = ds_Receipt.Tables[0].Rows[0]["agentReqId"].ToString();
                    }
                    else
                    {
                        ErrorLog.AgentManagementTrace("AgentRegistration: PolulateSummary: Failed - Fetch Agent Registration Details Using RequestID/PAN From DB. UserName: " + UserName + " PanNo: " + _AgentRegistrationDAL.PanNo + " RequestId: " + HidAGID.Value);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again later', 'Alert');", true);
                        return;
                    }
                }
                else
                {
                    ErrorLog.AgentManagementTrace("AgentRegistration: PolulateSummary: Failed - Fetch Agent Registration Details Using RequestID/PAN From DB. UserName: " + UserName + " PanNo: " + _AgentRegistrationDAL.PanNo + " RequestId: " + HidAGID.Value);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again later', 'Alert');", true);
                    return;
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | PolulateSummary() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: PolulateSummary: Failed - Fetch Agent Registration Details Using RequestID/PAN From DB. Exception: " + Ex.Message + " UserName: " + UserName + " PanNo: " + _AgentRegistrationDAL.PanNo + " RequestId: " + HidAGID.Value);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again later', 'Alert');", true);
                return;
            }
        }



        public void GetDetails(string agReqId)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | GetDetails() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                _AgentRegistrationDAL.AgentReqId = agReqId;
                DataSet ds = _AgentRegistrationDAL.GetAgentDetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables[2].Rows.Count > 0)
                    {
                        ddlState.DataSource = ds.Tables[2];
                        ddlState.DataValueField = "ID";
                        ddlState.DataTextField = "Name";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                    if (ds.Tables[0].Rows[0]["AgentState"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["AgentState"].ToString();
                    }
                    if (ds != null && ds.Tables[3].Rows.Count > 0)
                    {
                        ddlCity.DataSource = ds.Tables[3];
                        ddlCity.DataValueField = "ID";
                        ddlCity.DataTextField = "Name";
                        ddlCity.DataBind();
                        ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                    if (ds.Tables[0].Rows[0]["AgentCity"].ToString() != "")
                    {
                        ddlCity.SelectedValue = ds.Tables[0].Rows[0]["AgentCity"].ToString();
                    }
                    if (ds != null && ds.Tables[2].Rows.Count > 0)
                    {
                        ddlShopState.DataSource = ds.Tables[2];
                        ddlShopState.DataValueField = "ID";
                        ddlShopState.DataTextField = "Name";
                        ddlShopState.DataBind();
                        ddlShopState.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                    if (ds.Tables[0].Rows[0]["ShopState"].ToString() != "")
                    {
                        ddlShopState.SelectedValue = ds.Tables[0].Rows[0]["ShopState"].ToString();
                    }
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlShopCity.DataSource = ds.Tables[3];
                        ddlShopCity.DataValueField = "ID";
                        ddlShopCity.DataTextField = "Name";
                        ddlShopCity.DataBind();
                        ddlShopCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                    if (ds.Tables[0].Rows[0]["ShopCity"].ToString() != "")
                    {
                        ddlShopCity.SelectedValue = ds.Tables[0].Rows[0]["ShopCity"].ToString();
                    }
                    if (ds != null && ds.Tables[4].Rows.Count > 0)
                    {
                        ddlbcCode.DataSource = ds.Tables[4];
                        ddlbcCode.DataValueField = "BCCode";
                        ddlbcCode.DataTextField = "BCName";
                        ddlbcCode.DataBind();
                        ddlbcCode.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                    if (ds.Tables[0].Rows[0]["BCCode"].ToString() != "")
                    {
                        ddlbcCode.SelectedValue = ds.Tables[0].Rows[0]["BCCode"].ToString();
                    }
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlclient.DataSource = ds.Tables[0];
                        ddlclient.DataValueField = "ClientID";
                        ddlclient.DataTextField = "ClientName";
                        ddlclient.DataBind();
                        ddlclient.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                    if (ds.Tables[0].Rows[0]["ClientID"].ToString() != "")
                    {
                        ddlclient.SelectedValue = ds.Tables[0].Rows[0]["ClientID"].ToString();
                    }
                    txtFirstName.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    txtMiddleName.Text = ds.Tables[0].Rows[0]["Middle"].ToString();
                    txtLastName.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                    txtcode.Text = ds.Tables[0].Rows[0]["DeviceCode"].ToString();
                    if (ds.Tables[0].Rows[0]["Gender"].ToString() != "")
                    {
                        ddlGender.SelectedValue = ds.Tables[0].Rows[0]["Gender"].ToString();
                    }
                    txtPANNo.Text = ds.Tables[0].Rows[0]["PanNo"].ToString();
                    txtGSTNo.Text = ds.Tables[0].Rows[0]["GSTNo"].ToString();
                    txtaadharno.Text = ds.Tables[0].Rows[0]["AadharNo"].ToString();

                    txtAccountNumber.Text = ds.Tables[0].Rows[0]["AccountNumber"].ToString();
                    txtIFsccode.Text = ds.Tables[0].Rows[0]["IFSCCode"].ToString();
                    txtPass.Text = ds.Tables[0].Rows[0]["PassportNo"].ToString();
                    if (ds.Tables[0].Rows[0]["AgentCategory"].ToString() != "")
                    {
                        ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["AgentCountry"].ToString();
                    }
                    txtRegisteredAddress.Text = ds.Tables[0].Rows[0]["AgentAddress"].ToString();
                    if (ds.Tables[0].Rows[0]["AgentCountry"].ToString() != "")
                    {
                        ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["AgentCountry"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["AgentState"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["AgentState"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["AgentDistrict"].ToString() != "")
                    {
                        ddlDistrict.SelectedValue = ds.Tables[0].Rows[0]["AgentDistrict"].ToString();
                    }
                    txtPinCode.Text = ds.Tables[0].Rows[0]["AgentPinCode"].ToString();
                    txtEmailID.Text = ds.Tables[0].Rows[0]["PersonalEmailID"].ToString();
                    txtcontact.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtLandlineNo.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();
                    txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtLandlineNo.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();
                    txtshopadd.Text = ds.Tables[0].Rows[0]["ShopAddress"].ToString();
                    if (ds.Tables[0].Rows[0]["ShopCountry"].ToString() != "")
                    {
                        ddlshopCountry.SelectedValue = ds.Tables[0].Rows[0]["ShopCountry"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ShopState"].ToString() != "")
                    {
                        ddlShopState.SelectedValue = ds.Tables[0].Rows[0]["ShopState"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ShopDistrict"].ToString() != "")
                    {
                        ddlShopDistrict.SelectedValue = ds.Tables[0].Rows[0]["ShopDistrict"].ToString();
                    }
                    txtshoppin.Text = ds.Tables[0].Rows[0]["ShopPinCode"].ToString();
                    txtdob.Text = ds.Tables[0].Rows[0]["AgentDOB"].ToString();
                    TextBox5.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    TextBox6.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();
                    if (ds.Tables[0].Rows[0]["AEPS"].ToString() != "" || ds.Tables[0].Rows[0]["AEPS"].ToString() != null)
                    {
                        //  HdnAEPS.Value = ds.Tables[0].Rows[0]["AEPS"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["AEPS"].ToString() == "1")
                    {
                        chkAEPS.Checked = true;
                    }
                    else
                    {
                        chkAEPS.Checked = false;
                    }
                    if (ds.Tables[0].Rows[0]["MATM"].ToString() != "" || ds.Tables[0].Rows[0]["MATM"].ToString() != null)
                    {
                        //HdnMATM.Value = ds.Tables[0].Rows[0]["MATM"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["MATM"].ToString() == "1")
                    {
                        chkMATM.Checked = true;
                    }
                    else
                    {
                        chkMATM.Checked = false;
                    }

                    divOnboardFranchise.Visible = true;
                    divAction.Visible = false;
                    divMainDetailsGrid.Visible = false;
                    DIVDetails.Visible = true;
                }
                else
                {
                    //clearselection();
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | GetDetails() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: GetDetails: Failed - Fetch Agent Registration Details. Exception: " + Ex.Message + " UserName: " + UserName + " RequestId: " + HidAGID.Value + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }
        #endregion

        #region Button Events - Main
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnSearch_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "btnSearch";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                ErrorLog.AgentManagementTrace("AgentRegistration | btnSearch_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: btnSearch_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnClear_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "btnClear";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                ddlRequestType.SelectedValue = null;
                ddlBucketId.SelectedValue = null;
                ddlRequestStatus.SelectedValue = null;
                //txtAgentCode.Value = string.Empty;
                txtPanNoF.Value = string.Empty;

                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                ErrorLog.AgentManagementTrace("AgentRegistration | btnClear_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegitsration: btnClear_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnAddnew_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnAddnew_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                divOnboardFranchise.Visible = true;
                divAction.Visible = false;
                divMainDetailsGrid.Visible = false;
                DIVDetails.Visible = true;
                ErrorLog.AgentManagementTrace("AgentRegistration | btnAddnew_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegitsration: btnAddnew_Click(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnZipsample_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnZipsample_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadTrace(string.Format("Initiated Document Upload Sample File Download Request."));
                string strURL = string.Empty;


                strURL = "~/ExportedExcelFiles/AgentBulkDocuments.zip";
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=AgentBulkDocuments.zip");
                byte[] data = req.DownloadData(Server.MapPath(strURL));
                response.BinaryWrite(data);
                response.End();
                ErrorLog.UploadTrace(string.Format("Completed Document Upload Sample File Download."));
                ErrorLog.AgentManagementTrace("AgentRegistration | btnZipsample_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (System.Threading.ThreadAbortException Ex)
            {
                ErrorLog.UploadTrace(string.Format("Error AgentRegistration Document Sample File Download."));
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void ddlbcCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillAggregator();
                SetInheritedSerivcesFromParent(ddlaggregatorCode.SelectedValue);
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void ddlaggregatorCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetInheritedSerivcesFromParent(ddlaggregatorCode.SelectedValue != "0" ? ddlaggregatorCode.SelectedValue : string.Empty);
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void SetInheritedSerivcesFromParent(string _FranchiseCode)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | SetInheritedSerivcesFromParent() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.AggCode = _FranchiseCode;
                _AgentRegistrationDAL.Flag = '1';
                DataSet dsServices = _AgentRegistrationDAL.InheritServicesFromParent();
                if (dsServices != null && dsServices.Tables.Count > 0 && dsServices.Tables[0].Rows.Count > 0)
                {
                    if (dsServices.Tables[0].Rows[0]["AEPS"] != null)
                    {
                        if (dsServices.Tables[0].Rows[0]["AEPS"].ToString() == "1")
                        {
                            chkAEPS.Checked = true;
                            chkAEPS.Enabled = true;
                            chkAEPS.Visible = true;
                            lblchkAEPS.Visible = true;
                            //lblServicesOffer.Visible = true;
                        }
                        else
                        {
                            chkAEPS.Checked = false;
                            chkAEPS.Enabled = false;
                            chkAEPS.Visible = false;
                            lblchkAEPS.Visible = false;
                        }
                    }
                    else
                    {
                        chkAEPS.Checked = false;
                        chkAEPS.Enabled = false;
                        chkAEPS.Visible = false;
                        lblchkAEPS.Visible = false;
                    }

                    //if (dsServices.Tables[0].Rows[0]["BBPS"] != null)
                    //{
                    //    if (dsServices.Tables[0].Rows[0]["BBPS"].ToString() == "1")
                    //    {
                    //        chkbbps.Checked = true;
                    //        chkbbps.Enabled = true;
                    //        chkbbps.Visible = true;
                    //        lblchkbbps.Visible = true;
                    //        lblServicesOffer.Visible = true;
                    //    }
                    //    else
                    //    {
                    //        chkbbps.Checked = false;
                    //        chkbbps.Enabled = false;
                    //        chkbbps.Visible = false;
                    //        lblchkbbps.Visible = false;
                    //    }
                    //}
                    //else
                    //{
                    //    chkbbps.Checked = false;
                    //    chkbbps.Enabled = false;
                    //    chkbbps.Visible = false;
                    //    lblchkbbps.Visible = false;
                    //}

                    if (dsServices.Tables[0].Rows[0]["DMT"] != null)
                    {
                        if (dsServices.Tables[0].Rows[0]["DMT"].ToString() == "1")
                        {
                            chkdmt.Checked = true;
                            chkdmt.Enabled = true;
                            chkdmt.Visible = true;
                            lblchkdmt.Visible = true;
                            //lblServicesOffer.Visible = true;
                        }
                        else
                        {
                            chkdmt.Checked = false;
                            chkdmt.Enabled = false;
                            chkdmt.Visible = false;
                            lblchkdmt.Visible = false;
                        }
                    }
                    else
                    {
                        chkdmt.Checked = false;
                        chkdmt.Enabled = false;
                        chkdmt.Visible = false;
                        lblchkdmt.Visible = false;
                    }

                    if (dsServices.Tables[0].Rows[0]["MATM"] != null)
                    {
                        if (dsServices.Tables[0].Rows[0]["MATM"].ToString() == "1")
                        {
                            chkMATM.Checked = true;
                            chkMATM.Enabled = true;
                            chkMATM.Visible = true;
                            lblchkMATM.Visible = true;
                            //lblServicesOffer.Visible = true;
                        }
                        else
                        {
                            chkMATM.Checked = false;
                            chkMATM.Enabled = false;
                            chkMATM.Visible = false;
                            lblchkMATM.Visible = false;
                        }
                    }
                    else
                    {
                        chkMATM.Checked = false;
                        chkMATM.Enabled = false;
                        chkMATM.Visible = false;
                        lblchkMATM.Visible = false;
                    }
                }
                else
                {
                    //lblServicesOffer.Visible = false;

                    chkAEPS.Checked = false;
                    chkAEPS.Enabled = false;
                    chkAEPS.Visible = false;
                    lblchkAEPS.Visible = false;

                    //chkbbps.Checked = false;
                    //chkbbps.Enabled = false;
                    //chkbbps.Visible = false;
                    //lblchkbbps.Visible = false;

                    chkdmt.Checked = false;
                    chkdmt.Enabled = false;
                    chkdmt.Visible = false;
                    lblchkdmt.Visible = false;

                    chkMATM.Checked = false;
                    chkMATM.Enabled = false;
                    chkMATM.Visible = false;
                    lblchkMATM.Visible = false;
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | SetInheritedSerivcesFromParent() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : SetInheritedSerivcesFromParent\nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Agent Registration');", false);
                return;
            }
        }
        #endregion

        #region Grid Events
        protected void gvAgentOnboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAgentOnboard.PageIndex = e.NewPageIndex;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : gvAgentOnboard_PageIndexChanging() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.', 'Alert');", true);
            }
        }

        protected void gvAgentOnboard_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grid = (GridView)sender;
                    string Bucket = grid.DataKeys[e.Row.RowIndex].Values[0].ToString();
                    string ReqStatus = grid.DataKeys[e.Row.RowIndex].Values[1].ToString();
                    string ReqType = grid.DataKeys[e.Row.RowIndex].Values[2].ToString();
                    ImageButton imageButton = e.Row.FindControl("btnView") as ImageButton;
                    ImageButton imageButton2 = e.Row.FindControl("btndelete") as ImageButton;
                    if (imageButton != null)
                    {
                        if ((Bucket.ToLower() == "self") || (ReqStatus.ToLower() == "declined") && (ReqType.ToLower() == "registration"))
                        {
                            imageButton.Enabled = true;
                            imageButton.Visible = true;
                            imageButton2.Enabled = true;
                            imageButton2.Visible = true;
                        }
                        else
                        {
                            imageButton.Enabled = false;
                            imageButton.Visible = false;
                            imageButton2.Enabled = false;
                            imageButton2.Visible = false;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: gvAgentOnboard_RowDataBound: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Alert');", true);
                return;
            }
        }
        // Edit Row
        protected void btnView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;
                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { '=' });
                string agReqId = commandArgs[0];
                string reqtype = commandArgs[2];
                _AgentRegistrationDAL.agReqId = Convert.ToInt32(agReqId);
                HidAGID.Value = agReqId;
                if (reqtype.ToLower() == "4" || reqtype.ToLower() == "5")
                {
                    Response.Redirect("../../Pages/Agent/EditRegistrationDetails.aspx?AgentReqId=" + _AgentRegistrationDAL.agReqId + "&" + "RequestType=" + reqtype + " ", false);
                }
                else
                {
                    GetDetails(agReqId);
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Alert');", true);
            }
        }

        #endregion

        #region Button Events - Registration Form Submit         
        protected void btnSubmitDetails_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnSubmitDetails_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (hdnUserConfirmation.Value.ToString() == "Yes")
                {
                    Session["PanNo"] = txtPANNo.Text;

                    txtPANNo.Text = !string.IsNullOrEmpty(hidPan.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidPan.Value)) : txtPANNo.Text;
                    txtaadharno.Text = !string.IsNullOrEmpty(hidAadh.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAadh.Value)) : txtaadharno.Text;
                    txtGSTNo.Text = !string.IsNullOrEmpty(hidSgst.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidSgst.Value)) : txtGSTNo.Text;
                    txtAccountNumber.Text = !string.IsNullOrEmpty(hidAccNo.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAccNo.Value)) : txtAccountNumber.Text;
                    txtIFsccode.Text = !string.IsNullOrEmpty(hidAccIFC.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAccIFC.Value)) : txtIFsccode.Text;
                    //txtagentId.Text = !string.IsNullOrEmpty(hdAgentId.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hdAgentId.Value)) : txtagentId.Text;
                    txtTerminalId.Text = !string.IsNullOrEmpty(hdTerminalId.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hdTerminalId.Value)) : txtTerminalId.Text;

                    if (ValidateSetProperties())
                    {
                        int Status = 0;
                        //string StatusMsg = string.Empty, RequestId = string.Empty;
                        //int RequestId = 0;
                        //if (_AgentRegistrationDAL.Validate_AgentRequest(UserName, out Status, out StatusMsg))
                        //{
                            ErrorLog.AgentManagementTrace("AgentRegistration: btnSubmitDetails_Click: Successful - Agent Registration KYC Validation. UserName: " + UserName);
                            Processor processor = new Processor();
                            string RespCode = string.Empty;
                            string RespMessage = string.Empty;
                            int MsgId = 0;
                            string outstatus = string.Empty;
                            string firstName = string.Empty;
                            string middleName = string.Empty;
                            string lastName = string.Empty;

                            _AgentRegistrationDAL.AEPS = chkAEPS.Checked == true ? 1 : 0;
                            _AgentRegistrationDAL.MATM = chkMATM.Checked == true ? 1 : 0;
                            _AgentRegistrationDAL.DMT = chkdmt.Checked == true ? 1 : 0;
                            _AgentRegistrationDAL.FirstName = txtFirstName.Text.Trim();
                            _AgentRegistrationDAL.MiddleName = txtMiddleName.Text.Trim();
                            _AgentRegistrationDAL.LastName = txtLastName.Text.Trim();
                            _AgentRegistrationDAL.PersonalEmailID = txtEmailID.Text.Trim();
                            _AgentRegistrationDAL.AadharNo = txtaadharno.Text.Trim();
                            _AgentRegistrationDAL.AadharNo = MaskingAadhar(txtaadharno.Text.Trim());
                            _AgentRegistrationDAL.PanNo = txtPANNo.Text.Trim();
                            _AgentRegistrationDAL.GSTNo = txtGSTNo.Text.Trim();
                            _AgentRegistrationDAL.AgentAddress = txtRegisteredAddress.Text.Trim();
                            _AgentRegistrationDAL.ContactNo = txtContactNo.Text.Trim();
                            _AgentRegistrationDAL.LandlineNo = txtLandlineNo.Text.Trim();
                            _AgentRegistrationDAL.AccountNumber = txtAccountNumber.Text.ToString();
                            _AgentRegistrationDAL.IFSCCode = txtIFsccode.Text.Trim().ToString();
                            _AgentRegistrationDAL.AgentCountry = ddlCountry.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.AgentState = ddlState.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.AgentCity = ddlCity.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.AgentDistrict = ddlDistrict.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.AgentPincode = txtPinCode.Text.Trim();
                            _AgentRegistrationDAL.DeviceCode = txtcode.Text.Trim();
                            _AgentRegistrationDAL.AlternateNo = txtAlterNateNo.Text.Trim().ToString();
                            _AgentRegistrationDAL.BCCode = ddlbcCode.SelectedValue.ToString();
                            _AgentRegistrationDAL.AggCode = ddlaggregatorCode.SelectedValue.ToString();
                            _AgentRegistrationDAL.Gender = ddlGender.SelectedValue.ToString();
                            _AgentRegistrationDAL.AgentCategory = ddlCategory.SelectedValue.Trim();
                            _AgentRegistrationDAL.PassportNo = txtPass.Text.Trim();
                            _AgentRegistrationDAL.PopulationGroup = ddlarea.SelectedValue.Trim();
                            //_AgentRegistrationDAL.AgentID = txtagentId.Text.Trim();

                            _AgentRegistrationDAL.TerminalId = txtTerminalId.Text.Trim();

                            _AgentRegistrationDAL.Lattitude = txtLatitude.Text.Trim();
                            _AgentRegistrationDAL.Longitude = txtLongitude.Text.Trim();

                            _AgentRegistrationDAL.Activity = ((int)EnumCollection.ActionType.Onboard).ToString();
                            _AgentRegistrationDAL.AgentDOB = txtdob.Text.Trim();


                            if (CheckBoxagent.Checked == true)
                            {
                                _AgentRegistrationDAL.ShopAddress = txtRegisteredAddress.Text.Trim();
                                _AgentRegistrationDAL.ShopDistrict = ddlShopDistrict.SelectedValue.ToString().Trim();
                                _AgentRegistrationDAL.ShopCountry = ddlCountry.SelectedValue.ToString().Trim();
                                _AgentRegistrationDAL.ShopCity = ddlCity.SelectedValue.ToString().Trim();
                                _AgentRegistrationDAL.ShopState = ddlState.SelectedValue.ToString().Trim();
                                _AgentRegistrationDAL.ShopPinCode = txtPinCode.Text.Trim();
                                _AgentRegistrationDAL.shopemail = txtEmailID.Text.Trim();
                            }
                            else
                            {
                                _AgentRegistrationDAL.ShopAddress = txtshopadd.Text.Trim();
                                _AgentRegistrationDAL.ShopDistrict = ddlShopDistrict.SelectedValue.ToString().Trim();
                                _AgentRegistrationDAL.ShopCountry = ddlshopCountry.SelectedValue.ToString().Trim();
                                _AgentRegistrationDAL.ShopState = ddlShopState.SelectedValue.ToString();
                                _AgentRegistrationDAL.ShopPinCode = txtshoppin.Text.Trim();
                                _AgentRegistrationDAL.ShopCity = ddlShopCity.SelectedValue.ToString().Trim();
                                _AgentRegistrationDAL.shopemail = txtshopEmailID.Text.Trim();
                            }
                            _AgentRegistrationDAL.AgentAddress = txtRegisteredAddress.Text.Trim().ToString();
                            _AgentRegistrationDAL.AgentCountry = ddlCountry.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.AgentState = ddlState.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.AgentCity = ddlCity.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.AgentDistrict = ddlDistrict.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
                            _AgentRegistrationDAL.StageId = Convert.ToInt32(EnumCollection.StageId.PendingDocs);
                            _AgentRegistrationDAL.Stage = Convert.ToInt32(EnumCollection.StageId.PendingDocs);
                            _AgentRegistrationDAL.Flag = 1;
                            _AgentRegistrationDAL.ClientId = Session["Client"].ToString();
                            _AgentRegistrationDAL.agReqId = HidAGID.Value != null && !string.IsNullOrEmpty(HidAGID.Value) ? Convert.ToInt32(HidAGID.Value) : 0;
                            if (_AgentRegistrationDAL.Insert_AgentRequest(UserName, out int RequestId, out string _status, out string _statusmsg))
                            {
                            #region Audit
                            _auditParams[0] = Session["Username"].ToString();
                            _auditParams[1] = "Agent-Registration";
                            _auditParams[2] = "btnSubmitDetails";
                            _auditParams[3] = Session["LoginKey"].ToString();
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            #endregion
                            ErrorLog.AgentManagementTrace("AgentRegistration: btnSubmitDetails_Click: Successful - Agent Registration Request Dump In DB. UserName: " + UserName + " RequestId: " + RequestId);
                                HidAGID.Value = RequestId.ToString();
                                dbAccess.AgentReqID = RequestId.ToString();
                                //dbAccess.InsertMessageLog(MsgId, RequestId, (int)AgentVerification.Configuration.EndPoints.NameScreening, null, null, (int)AgentVerification.Configuration.DBFlag.Select, out MsgId);
                                //dbAccess.InsertNameScreeningResp(MsgId, RequestId, (int)AgentVerification.Configuration.EndPoints.NameScreening, null, null, (int)AgentVerification.Configuration.DBFlag.Update, out outstatus);
                                //if (outstatus == "1")
                                //{
                                //    ErrorLog.AgentManagementTrace("AgentRegistration: btnSubmitDetails_Click: Successful - Name Screening Response Agent Request ID Dump In DB. UserName: " + UserName + "Agent Request Id: " + RequestId);
                                //    Logger.WriteTraceLog(null, "NameScreening: Success - Agent Request ID of name screening Response Dump In DB.  APIReqID: " + MsgId + "  UserName: " + UserName + "Agent Request Id: " + RequestId);

                                //    //Need To Remove Comment

                                //    #region EMAIL
                                //    _EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                                //    _EmailSMSAlertscs.to = _AgentRegistrationDAL.PersonalEmailID;
                                //    _EmailSMSAlertscs.tempname = "SR24659_EBCP1";
                                //    _EmailSMSAlertscs.OTPFlag = "0";
                                //    _EmailSMSAlertscs.var1 = "SBM";
                                //    _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                //    ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : Registration() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                                //    ErrorLog.SMSTrace("Page : AgentRegistration.cs \nFunction : Registration() => Reistration Details forwarded for Email Preparation. Email : " + _AgentRegistrationDAL.PersonalEmailID);
                                //    _EmailSMSAlertscs.HttpGetRequestEmail();
                                //    #endregion

                                //    #region SMS
                                //    _EmailSMSAlertscs.FROM = Session["Username"].ToString();
                                //    _EmailSMSAlertscs.to = _AgentRegistrationDAL.ContactNo;
                                //    _EmailSMSAlertscs.tempname = "SR24659_BCP4";
                                //    _EmailSMSAlertscs.OTPFlag = "0";
                                //    _EmailSMSAlertscs.var1 = "SBM";
                                //    _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                //    ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : Registration() => Reistration Details forwarded for SMS Preparation. => HttpGetRequest()");
                                //    ErrorLog.SMSTrace("Page : AgentRegistration.cs \nFunction : Registration() => Reistration Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.ContactNo);
                                //    _EmailSMSAlertscs.HttpGetRequest();
                                //    #endregion
                                //}
                                //else
                                //{
                                //    ErrorLog.AgentManagementTrace("AgentRegistration: btnSubmitDetails_Click: Failed - Name Screening Response Agent Request ID Dump In DB. UserName: " + UserName + "Agent Request Id: " + RequestId);
                                //    Logger.WriteTraceLog(null, "NameScreening: Failed - Agent Request ID of name screening Response Dump In DB.  APIReqID: " + MsgId + "  UserName: " + UserName + "Agent Request Id: " + RequestId);
                                //}
                                DivAgntDetails.Visible = true;
                                div_Upload.Visible = true;
                                DivAgntDetails.Visible = false;
                                DIVDetails.Visible = false;
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Processed for Document Upload.');", true);
                                return;
                            }
                            else
                            {
                                ErrorLog.AgentManagementTrace("AgentRegistration: btnSubmitDetails_Click: Failed - Agent Registration Request Dump In DB. UserName: " + UserName + " Status: " + Status + " StatusMsg: " + _statusmsg + " RequestId: " + RequestId);
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + _statusmsg + "');", true);
                                return;
                            }
                        //}
                        //else
                        //{
                        //    ErrorLog.AgentManagementTrace("AgentRegistration: btnSubmitDetails_Click: Failed - Agent Registration KYC Validation. UserName: " + UserName + " Status: " + Status + " StatusMsg: " + StatusMsg);
                        //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + StatusMsg + "');", true);
                        //    return;
                        //}
                    }
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | btnSubmitDetails_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : btnSubmitDetails_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator');", true);
                return;
            }
        }

        public string MaskingAadhar(string adharno)
        {
            string masked = string.Empty;
            var cardNumber = adharno;
            //var firstDigits = cardNumber.Substring(0, 6);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);
            var requiredMask = new String('X', cardNumber.Length - lastDigits.Length);
            var maskedString = string.Concat(requiredMask, lastDigits);
            var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");
            masked = maskedCardNumberWithSpaces;
            return masked;
        }
        public bool AreContactNumbersDifferent(string contactNo, string alternateNo)
        {
            return contactNo != alternateNo;
        }
        private bool ValidateSetProperties()
        {
            _CustomeRegExpValidation = new clsCustomeRegularExpressions();
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | ValidateSetProperties() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                string contactNo = txtContactNo.Text;
                string alternateNo = txtAlterNateNo.Text;

                if (!AreContactNumbersDifferent(contactNo, alternateNo))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact Number and Alternate Number must be different.', 'First Name');", true);
                    return false; // Stop further processing
                }
                // ClientName
                if (hd_txtFirstName.Value == "1" || !string.IsNullOrEmpty(txtFirstName.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithoutSpace, txtFirstName.Text))
                    {
                        txtFirstName.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid First Name.', 'Registered Address');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.FileName = txtFirstName.Text.ToString();


                // PanNo
                if (hd_txtPANNo.Value == "1" || !string.IsNullOrEmpty(txtPANNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.PanCard, txtPANNo.Text))
                    {
                        txtPANNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid PAN Card.', 'PAN Card');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.PanNo = txtPANNo.Text.Trim();

                // AadharNo
                //if (hd_txtaadharno.Value == "1" || !string.IsNullOrEmpty(txtaadharno.Text))
                //    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.AdharCard, txtaadharno.Text))
                //    {
                //        txtaadharno.Focus();
                //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Aadhar No.', 'Aadhar No.');", true);
                //        return false;
                //    }
                //    else _AgentRegistrationDAL.AadharNo = txtaadharno.Text.ToString();
                // AccountNumber
                if (hd_txtAccountNumber.Value == "1" || !string.IsNullOrEmpty(txtAccountNumber.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.AccountNumber, txtAccountNumber.Text))
                    {
                        txtAccountNumber.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Account Number.', 'AccountNumber');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.AccountNumber = txtAccountNumber.Text.ToString();

                // IFSCCode
                if (hd_txtIFsccode.Value == "1" || !string.IsNullOrEmpty(txtIFsccode.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.IFSC, txtIFsccode.Text))
                    {
                        txtIFsccode.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid IFSC Code.', 'IFSC Code');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.IFSCCode = txtIFsccode.Text.Trim().ToString();
                // Registered Address
                if (hd_txtRegisteredAddress.Value == "1" || !string.IsNullOrEmpty(txtRegisteredAddress.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Address, txtRegisteredAddress.Text))
                    {
                        txtRegisteredAddress.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Address.', 'Registered Address');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.AgentAddress = txtRegisteredAddress.Text.Trim();

                // Pincode
                if (hd_txtPinCode.Value == "1" || !string.IsNullOrEmpty(txtPinCode.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Pincode, txtPinCode.Text))
                    {
                        txtPinCode.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Pincode.', 'Pincode');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.AgentPincode = txtPinCode.Text.Trim();

                // Country
                //if (ddlCountry.SelectedValue == "0")
                //{
                //    ddlCountry.Focus();
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select Country.', 'Country');", true);
                //    return false;
                //}
                //else _AgentRegistrationDAL.AgentCountry = ddlCountry.SelectedValue.ToString().Trim();

                // State
                if (ddlState.SelectedValue == "0")
                {
                    ddlState.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select State.', 'State');", true);
                    return false;
                }
                else _AgentRegistrationDAL.AgentState = ddlState.SelectedValue.ToString().Trim();

                // City
                if (ddlCity.SelectedValue == "0")
                {
                    ddlCity.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select City.', 'City');", true);
                    return false;
                }
                else _AgentRegistrationDAL.AgentCity = ddlCity.SelectedValue.ToString().Trim();

                //District
                if (ddlDistrict.SelectedValue == "0")
                {
                    ddlCity.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select District.', 'City');", true);
                    return false;
                }
                else _AgentRegistrationDAL.AgentDistrict = ddlDistrict.SelectedValue.ToString().Trim();
                
                // Personal Contact
                if (hd_txtContactNo.Value == "1" || !string.IsNullOrEmpty(txtContactNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, txtContactNo.Text))
                    {
                        txtContactNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Contact No.', 'Personal Contact No.');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.PersonalContact = txtContactNo.Text.Trim();

                // Alternate Mobile No
                if (hd_txtAlterNateNo.Value == "1" || !string.IsNullOrEmpty(txtAlterNateNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, txtAlterNateNo.Text))
                    {
                        txtAlterNateNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Mobile No.', 'Alternate Mobile No.');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.AlternateNo = txtAlterNateNo.Text.Trim().ToString();


                //if (hdAgentId.Value == "1" || !string.IsNullOrEmpty(txtagentId.Text))
                //    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbers, txtagentId.Text))
                //    {
                //        txtagentId.Focus();
                //        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid AgentId.', 'AgentId');", true);
                //        return false;
                //    }
                //    else _AgentRegistrationDAL.AgentID = txtagentId.Text.Trim().ToString();

                //if (hdAgentId.Value == "1" || !string.IsNullOrEmpty(txtTerminalId.Text))
                //    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbers, txtTerminalId.Text))
                //    {
                //        txtagentId.Focus();
                //        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid TerminalId.', 'TerminalId');", true);
                //        return false;
                //    }
                //    else _AgentRegistrationDAL.TerminalId = txtTerminalId.Text.Trim().ToString();

                //if (chkAEPS.Checked == false && chkMATM.Checked == false)
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select Atleast One Service');", true);
                //    return false;
                //}
                ErrorLog.AgentManagementTrace("AgentRegistration | ValidateSetProperties() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : ValidateSetProperties\nException Occured\n" + ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong..! Please try again','Agent Registration');", true);
                return false;
            }
            return true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnCancel_Click() | Startd. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "btnCancel";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                DIVDetails.Visible = false;
                div_Upload.Visible = false;
                divPaymentReceipt.Visible = false;
                btndownload.Visible = true;
                btnExportCSV.Visible = true;
                divOnboardFranchise.Visible = false;
                divAction.Visible = true;
                divMainDetailsGrid.Visible = true;
                ClearAllControls();
                HidAGID.Value = string.Empty;
                ErrorLog.AgentManagementTrace("AgentRegistration | btnCancel_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                //Session["AGCode"] = string.Empty;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs:btnCancel_Click \nFunction : ValidateFile \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                throw Ex;
            }
        }
        #endregion

        #region Button Events - Documents Upload Submit
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | BtnSubmit_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "BtnSubmit-Docs";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                if (hdnUploadConfirmation.Value.ToString() == "Yes" && HidAGID.Value != null && !string.IsNullOrEmpty(HidAGID.Value))
                {
                    _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
                    _AgentRegistrationDAL.Flag = (int)EnumCollection.DBFlag.Update;
                    _AgentRegistrationDAL.ClientId = Session["Client"].ToString();
                    _AgentRegistrationDAL.IdentityProofDocument = Session["IdAgentFilePath"].ToString();
                    _AgentRegistrationDAL.IdentityProofType = ddlIdentityProof.SelectedValue;
                    _AgentRegistrationDAL.AddressProofDocument = Session["AddAgentFilePath"].ToString();
                    _AgentRegistrationDAL.AddressProofType = ddlAddressProof.SelectedValue;
                    _AgentRegistrationDAL.SignatureProofDocument = Session["SigAgentFilePath"].ToString();
                    _AgentRegistrationDAL.SignatureProofType = ddlSignature.SelectedValue;
                    _AgentRegistrationDAL.agReqId = Convert.ToInt32(HidAGID.Value);
                    //int Status = 0;
                    //string StatusMsg, RequestId = string.Empty;
                    // int RequestId = 0;
                    if (_AgentRegistrationDAL.Insert_AgentRequest(Convert.ToString(Session["Username"]), out int RequestId, out string _status, out string _statusmsg))
                    {
                        ErrorLog.AgentManagementTrace("AgentRegistration: BtnSubmit_Click: Successful - Upload Documents. UserName: " + UserName + " Status: " + _status + " StatusMsg: " + _statusmsg + " RequestId: " + RequestId);
                        DivAgntDetails.Visible = true;
                        _AgentRegistrationDAL.AgentCode = RequestId.ToString();
                        HidAGID.Value = RequestId.ToString();
                        PolulateSummary();
                        div_Upload.Visible = false;
                        DivAgntDetails.Visible = true;
                        DIVDetails.Visible = false;
                        Session["IdAgentFilePath"] = string.Empty;
                        Session["AddAgentFilePath"] = string.Empty;
                        Session["SigAgentFilePath"] = string.Empty;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Documents Uploaded Successfully', 'Agent Registration');", true);
                    }
                    else
                    {
                        ErrorLog.AgentManagementTrace("AgentRegistration: BtnSubmit_Click: Failed - Upload Documents. UserName: " + UserName + " Status: " + _status + " StatusMsg: " + _statusmsg + " RequestId: " + RequestId);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + _statusmsg + "', 'Alert');", true);
                        return;
                    }
                }
                else
                {
                    ErrorLog.AgentManagementTrace("AgentRegistration: BtnSubmit_Click: Failed - Upload Documents. User Confirmation Or RequestId Are Empty. UserName: " + UserName);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong..! Please try again','Agent Registration');", true);
                    return;
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | BtnSubmit_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: BtnSubmit_Click: Failed - Upload Documents. Exception: " + Ex.Message + " UserName: " + UserName + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong..! Please try again','Agent Registration');", true);
                return;
            }
        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | BtnBack_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "BtnBack";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                div_Upload.Visible = false;
                DIVDetails.Visible = true;
                divPaymentReceipt.Visible = false;
                btndownload.Visible = false;
                divAction.Visible = false;
                divMainDetailsGrid.Visible = false;
                ErrorLog.AgentManagementTrace("AgentRegistration | BtnBack_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : BtnBack_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator','Agent Registration');", true);
                return;
            }

        }

        #endregion

        #region Button Events - Final Submit
        // Submit Summary
        protected void downloadPass_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | downloadPass_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "BtnSubmit-Final";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                if (hdnFinalConfirmation.Value.ToString() == "Yes")
                {
                    if (ChkConfirmBC.Checked == true)
                    {
                        _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
                        _AgentRegistrationDAL.AgentCode = HidAGID.Value.ToString();
                        _AgentRegistrationDAL.BCCode = ddlbcCode.SelectedValue.ToString();

                        _AgentRegistrationDAL.Flag = 1;

                        DataSet dsAgentMaster = _AgentRegistrationDAL.SetInsertUpdateAgentRequestHadlerDetails();
                        if (dsAgentMaster != null && dsAgentMaster.Tables.Count > 0 && dsAgentMaster.Tables[0].Rows[0]["Message"].ToString() == "Inserted")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Registered Successfully', 'Agent Registration');", true);
                            div_Upload.Visible = false;
                            divOnboardFranchise.Visible = false;
                            DivAgntDetails.Visible = false;
                            DIVDetails.Visible = false;
                            divAction.Visible = true;
                            divMainDetailsGrid.Visible = true;
                            ClearAllControls();
                            HidAGID.Value = string.Empty;
                        }
                        else
                        {
                            ClearAllControls();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Confirmation on all above Agent Deatils are properly filled', 'Agent Registration');", true);
                        return;
                    }
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | downloadPass_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : downloadPass_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong..! Please try again','Agent Registration');", true);
            }
        }

        // Close Summary
        protected void btnCloseReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnCloseReceipt_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "btnCloseReceipt";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                if (hdnUserConfirmation.Value.ToString() == "Yes")
                {
                    ErrorLog.AgentManagementTrace("AgentRegistration: btnCloseReceipt_Click: Successful - Summary Close. UserName: " + UserName);
                    DIVDetails.Visible = true;
                    divAction.Visible = false;
                    divMainDetailsGrid.Visible = false;
                    DivAgntDetails.Visible = false;
                    ddlSignature.SelectedValue = null;
                    ddlAddressProof.ClearSelection();
                    ddlIdentityProof.ClearSelection();
                }
                else
                {
                    ErrorLog.AgentManagementTrace("AgentRegistration: btnCloseReceipt_Click: Failed - Summary Close. UserName: " + UserName);
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | btnCloseReceipt_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: btnCloseReceipt_Click: Failed - Summary Close. UserName: " + UserName + " Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong..! Please try again','Agent Registration');", true);
                return;
            }
        }

        // Download Identity Proof Document
        protected void btnViewDownloadDoc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                _AgentRegistrationDAL.AgentReqId = ViewState["agReqId"].ToString();
                DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    strURL = Ds.Tables[0].Rows[0]["IdentityProofDocument"].ToString();
                    string FinalPath = filepath + strURL;
                    string fileName = Path.GetFileName(FinalPath);
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    byte[] data = req.DownloadData(FinalPath);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : btnViewDownload_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator','Agent Registration');", true);
                return;
            }
        }

        // Download Address Proof Document
        protected void btnViewDownloadDoc_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                _AgentRegistrationDAL.AgentReqId = ViewState["agReqId"].ToString();
                DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
                if (Ds.Tables[1].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    strURL = Ds.Tables[1].Rows[0]["AddressProofDocument"].ToString();
                    string FinalPath = filepath + strURL;
                    string fileName = Path.GetFileName(FinalPath);
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    byte[] data = req.DownloadData(FinalPath);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : btnViewDownloadDoc_Click1() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator','Agent Registration');", true);
                return;
            }
        }

        // Download Signature Proof Document
        protected void btnViewDownloadDoc_Click2(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                _AgentRegistrationDAL.AgentReqId = ViewState["agReqId"].ToString();
                DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
                if (Ds.Tables[2].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    strURL = Ds.Tables[2].Rows[0]["SignatureProofDocument"].ToString();
                    string FinalPath = filepath + strURL;
                    string fileName = Path.GetFileName(FinalPath);
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    byte[] data = req.DownloadData(FinalPath);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : btnViewDownloadDoc_Click2() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator','Agent Registration');", true);
                return;
            }
        }


        #endregion

        #region Button Events - Others
        protected void btntest_Click(object sender, EventArgs e)
        {
            ModalPopupExtenderEditRequest.Show();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            divPaymentReceipt.Visible = false;
            btndownload.Visible = true;
        }
        #endregion

        #region Export Button Events
        protected void btnExportCSV_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnExportCSV_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-Registration";
                    _auditParams[2] = "Export-To-CSV";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "Agent_Registration_Details", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: btnExportCSV_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Alert');", true);
                return;
            }
        }

        protected void btndownloadXLS_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btndownloadXLS_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-Registration";
                    _auditParams[2] = "Export-To-Excel";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "Agent_Registration_Details", dt);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : btndownload_Click\nException Occured\n" + Ex.Message);
            }
        }

        public bool ValidateReEditRequest()
        {
            bool IsvalidRecord = false;
            try
            {
                string _requestid = string.Empty;
                _AgentRegistrationDAL.Flag = 2;
                string status = _AgentRegistrationDAL.ValidateEditBcDetails(out _requestid);
                if (status == "00")
                {
                    IsvalidRecord = true;
                }
                else
                {
                    IsvalidRecord = false;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : ValidateReEditRequest\nException Occured\n" + Ex.Message);
            }
            return IsvalidRecord;
        }

        protected void gvAgentOnboard_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName.Contains("EditDetails"))
                {
                    try
                    {
                        ErrorLog.AgentManagementTrace("AgentRegistration | RowCommand-EditDetails | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        #region Audit
                        _auditParams[0] = Session["Username"].ToString();
                        _auditParams[1] = "Agent-Registration";
                        _auditParams[2] = "RowCommand-EditDetails";
                        _auditParams[3] = Session["LoginKey"].ToString();
                        _LoginEntity.StoreLoginActivities(_auditParams);
                        #endregion
                        ImageButton lb = (ImageButton)e.CommandSource;
                        GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                        string agReqId = (gvAgentOnboard.DataKeys[gvr.RowIndex].Values["AgentReqId"]).ToString();
                        string reqtype = (gvAgentOnboard.DataKeys[gvr.RowIndex].Values["ActivityType"]).ToString();
                        string reqstatus = (gvAgentOnboard.DataKeys[gvr.RowIndex].Values["RequestStatus"]).ToString();
                        string bucket = (gvAgentOnboard.DataKeys[gvr.RowIndex].Values["Bucket"]).ToString();
                        _AgentRegistrationDAL.AgentReqId = (gvAgentOnboard.DataKeys[gvr.RowIndex].Values["AgentReqId"]).ToString();
                        if (ValidateReEditRequest())
                        {
                            _AgentRegistrationDAL.agReqId = Convert.ToInt32(agReqId);
                            HidAGID.Value = agReqId;

                            if (bucket.ToLower() == "self")
                            {
                                Response.Redirect("../../Pages/Agent/ReprocessRegDetails.aspx?AgentReqId=" + _AgentRegistrationDAL.agReqId + "&" + "RequestType=" + reqtype + " ", false);
                            }
                            else if ((reqtype == "0") && reqstatus.ToLower() == "declined")
                            {
                                int IsSelfData = 0;
                                Response.Redirect("../../Pages/Agent/EditRegistrationDetails.aspx?AgentReqId=" + _AgentRegistrationDAL.AgentReqId + "&" + "RequestType=" + reqtype + "&" + "IsSelfData=" + IsSelfData + " ", false);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent request already pending for verification.', 'Warning');", true);
                            return;
                        }
                        ErrorLog.AgentManagementTrace("AgentRegistration | RowCommand-EditDetails | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                        throw Ex;
                    }
                }
                if (e.CommandName.Contains("DeleteDetails"))
                {
                    ErrorLog.AgentManagementTrace("AgentRegistration | RowCommand-DeleteDetails | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-Registration";
                    _auditParams[2] = "RowCommand-DeleteDetails";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    if (hdnDeleteConfirmation.Value.ToString() == "Yes")
                    {
                        try
                        {
                            ImageButton lb = (ImageButton)e.CommandSource;
                            GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                            string agReqId = (gvAgentOnboard.DataKeys[gvr.RowIndex].Values["AgentReqId"]).ToString();
                            _AgentRegistrationDAL.AgentReqId = agReqId;
                            string _status = _AgentRegistrationDAL.DeleteBcDetails();
                            if (_status == "00")
                            {
                                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Deleted Successfully.', 'Warning');", true);
                                return;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Data Delete Unsuccessful.', 'Warning');", true);
                                return;
                            }

                        }
                        catch (Exception Ex)
                        {
                            ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message);
                            throw Ex;
                        }
                    }
                    ErrorLog.AgentManagementTrace("AgentRegistration | RowCommand-DeleteDetails | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: gvBlockAG_RowCommand: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        protected void txtPANNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string Status, statusmsg = string.Empty;
                _AgentRegistrationDAL.PanNo = txtPANNo.Text.ToString();
                _AgentRegistrationDAL.BCCode = ddlbcCode.SelectedValue.ToString();
                if (Convert.ToString(ddlbcCode.SelectedValue) != "0")
                {
                    string pan = _AgentRegistrationDAL.PanNo[3].ToString().ToUpper();
                    if (pan == "P")
                    {
                        if (_AgentRegistrationDAL.ValidatePanDetails(out Status, out statusmsg))
                        {
                            lblPanNoError.Text = "";
                        }
                        else
                        {
                            lblPanNoError.Text = statusmsg;
                            txtPANNo.Text = "";
                            txtPANNo.Focus();
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Pan No. Try again', 'Warning');", true);
                    }
                }
                else
                {
                    lblPanNoError.Text = "";
                    txtPANNo.Text = "";
                    txtPANNo.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select Business Correspondence', 'Warning');", true);
                    return;
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: gvBlockAG_RowCommand: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        private string SetPageFiltersExport()
        {
            string pageFilters = string.Empty;
            try
            {
                pageFilters = "Generated By " + Convert.ToString(Session["Username"]);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace(Ex.Message);
            }
            return pageFilters;
        }
        #endregion


        protected void CheckBoxAddress_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckBoxagent.Checked == true)
                {
                    divshopcountry.Visible = false;
                }
                else
                {
                    divshopcountry.Visible = true;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : CheckBoxAddress_CheckedChanged() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Agent Registration');", true);
                return;
            }
        }

        #region Bulk Upload
        protected void btnUpSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnUpSearch_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "btnUpSearch";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                if (string.IsNullOrEmpty(txtFromDate.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please Select From Date.', 'Warning');</script>", false);
                    return;
                }
                if (string.IsNullOrEmpty(txtToDate.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please Select To Date.', 'Warning');</script>", false);
                    return;
                }

                else
                {
                    FillGridSearch(EnumCollection.EnumBindingType.BindGrid);
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | btnUpSearch_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false); return;
            }

            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("AgentRegistration : Error At btnUpSearch_ServerClick(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnUpClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnUpClear_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "btnUpClear";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                ddlFileTypeStatus.ClearSelection();
                gvBulkManualKyc.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false); return;
                //ErrorLog.AgentManagementTrace("AgentRegistration | btnUpClear_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("AgentRegistration : Error At btnUpClear_ServerClick(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void gvManualEkyc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRequest")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            }
        }

        protected void btnsample_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnsample_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadTrace(string.Format("Initiated Agent Registration Upload Sample File Download Request."));
                string strURL = string.Empty;


                strURL = "~/ExportedExcelFiles/AgentRegistration.xlsx";
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=AgentRegistration.xlsx");
                byte[] data = req.DownloadData(Server.MapPath(strURL));
                response.BinaryWrite(data);
                response.End();
                ErrorLog.UploadTrace(string.Format("Completed Agent Registration Sample File Download."));

            }
            catch (System.Threading.ThreadAbortException Ex)
            {
                ErrorLog.UploadTrace(string.Format("Error AgentRegistration Sample File Download."));
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void gvManualEkyc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBulkManualKyc.PageIndex = e.NewPageIndex;
                FillGridSearch(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementError(Ex);
            }
        }

        private void SetPropertise(ref ImportEntity importEntity)
        {
            try
            {
                importEntity.UserName = Session["Username"].ToString();
                importEntity._FromDate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                importEntity._ToDate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                importEntity.Flag = Convert.ToInt32(EnumCollection.EnumBindingType.BindGrid);
                importEntity.FileDescID = importEntity.FileDescID = Convert.ToString((int)EnumCollection.EnumFileDesciption.UploadNegAgentList);
                importEntity.FileStatus = (ddlFileTypeStatus.SelectedValue.ToString() != null && ddlFileTypeStatus.SelectedValue.ToString() != "" ? ddlFileTypeStatus.SelectedValue.ToString() : "0");
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("AgentRegistration: SetPropertise(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                throw;
            }
        }


        protected void gvBulkManualKyc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Contains("DownloadDoc"))
                {
                    ErrorLog.AgentManagementTrace("AgentRegistration | RowCommand-DownloadDoc | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-Registration";
                    _auditParams[2] = "RowCommand-DownloadDoc";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    string status = string.Empty;
                    ImageButton lb = (ImageButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                    int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    _ImportEntity.Fileid = Convert.ToInt32(gvBulkManualKyc.DataKeys[gvr.RowIndex].Values["FileID"]);
                    _ImportEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                    _ImportEntity.Flag = 3;
                    DataSet Ds = _ImportEntity.Get_AgentManualKycUpload();
                    if (string.IsNullOrEmpty(status)) status = "Bulk_Agent_Registration_Details";
                    if (Ds != null && Ds.Tables.Count > 0)
                    {
                        exportFormat.ExporttoExcel(Session["Username"].ToString(), Session["BankName"].ToString(), status, Ds);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Something went wrong. Try again', 'Warning');", true);
                        return;
                    }
                    ErrorLog.AgentManagementTrace("AgentRegistration | RowCommand-DownloadDoc | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        public void SetPropertise()
        {
            try
            {
                _FromDate = !string.IsNullOrEmpty(txtFromDate.Value) ? txtFromDate.Value.Trim() : null;
                _ToDate = !string.IsNullOrEmpty(txtToDate.Value) ? txtToDate.Value.Trim() : null;
                _Username = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementError(Ex);
            }
        }
        protected void gvBulkManualKyc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBulkManualKyc.PageIndex = e.NewPageIndex;
                FillGridSearch(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementError(Ex);
            }
        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        public DataSet FillGridSearch(EnumCollection.EnumBindingType _EnumBindingType, string sortExpression = null)
        {
            DataSet ds = null;
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | FillGridSearch() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                gvBulkManualKyc.DataSource = null;
                gvBulkManualKyc.DataBind();
                SetPropertise();

                _ImportEntity.UserName = _Username;
                _ImportEntity.Flag = 1;
                _ImportEntity.FromDate = !string.IsNullOrEmpty(_FromDate) ? Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd") : null;
                _ImportEntity.ToDate = !string.IsNullOrEmpty(_ToDate) ? Convert.ToDateTime(_ToDate).ToString("yyyy-MM-dd") : null;
                gvBulkManualKyc.Visible = true;
                ds = _ImportEntity.Get_AgentManualKycUpload();
                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            DataView dv = ds.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";
                            dv.Sort = sortExpression + " " + this.SortDirection;
                            gvBulkManualKyc.DataSource = dv;
                            gvBulkManualKyc.DataBind();
                            gvBulkManualKyc.Visible = true;

                        }
                        else
                        {
                            gvBulkManualKyc.DataSource = ds.Tables[0];
                            gvBulkManualKyc.DataBind();
                            gvBulkManualKyc.Visible = true;

                        }
                    }
                    else
                    {
                        gvBulkManualKyc.Visible = false;

                    }
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | FillGridSearch() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AgentRegistration: FillGridSearch(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return ds;
        }
        protected void btnBulkUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnBulkUpload_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                string TotalRecords = null;
                string _FileImport = fileUpload.FileName;
                string _fileExtension = Path.GetExtension(_FileImport);
                bool isFolderExist = false;
                string _FileImportDoc = DocUpload.FileName;
                string _DocfileExtension = Path.GetExtension(_FileImportDoc);
                string FILENAME = Path.GetFileNameWithoutExtension(_FileImportDoc);


                DataTable dtExcelData = new DataTable();
                if (_FileImport != "" && fileUpload.HasFile && DocUpload.HasFile)
                {
                    ErrorLog.AgentManagementTrace("Received For Agent Restriction Bulk Upload Request. FileType : " + _FileImport);

                    if (ValidateFileFormat(_fileExtension) && ValidateZipFileFormat(_DocfileExtension))
                    {

                        //Check Uploaded zip file folder format.
                        //checks uploaded zip file contains folder name as "AgentBulkDocuments"
                        ZipFile zip = ZipFile.Read(DocUpload.PostedFile.InputStream);
                        foreach (ZipEntry Zipentry in zip.Entries)
                        {
                            if (Zipentry.FileName.Contains("AgentBulkDocuments/"))
                            {
                                isFolderExist = true;
                                break;
                            }
                        }


                        if (isFolderExist) //FILENAME == "AgentBulkDocuments" &&
                        {

                            ErrorLog.AgentManagementTrace("Successful Validate File Format For Agent Registration . FileType : " + _FileImport);
                            filePath = null; string FileId = string.Empty;

                            //saves uploaded excel file.
                            filePath = SaveFile(fileUpload);

                            _ImportFileFields = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["AgentRegistrationFileFields"].ToString()) ? Convert.ToInt32(ConfigurationManager.AppSettings["AgentRegistrationFileFields"].ToString()) : 0;
                            FileValidator _FileValidator = new FileValidator();
                            _FileValidator.FileName = _FileImport;
                            _FileValidator.FileDateTime = DateTime.Now.ToString();
                            _FileValidator.FilePath = filePath; _FileValidator.FileType = _fileExtension;
                            _FileValidator.ClientID = null;
                            _FileValidator.Username = Session["Username"].ToString();
                            if (FileImportEntry(_FileValidator, out FileId))
                            {
                                if (ProcessFile(_fileExtension, filePath, out dtExcelData))
                                {
                                    for (int rows = 0; rows < dtExcelData.Rows.Count; rows++)
                                    {
                                        string StatusCode = null, StatusDesc = null;
                                        int MsgID = 0;
                                        _BulkAgentEntity.BCID = !string.IsNullOrEmpty(dtExcelData.Rows[rows][0].ToString()) ? dtExcelData.Rows[rows][0].ToString() : "";
                                        _BulkAgentEntity.AggegatorCode = !string.IsNullOrEmpty(dtExcelData.Rows[rows][1].ToString()) ? dtExcelData.Rows[rows][1].ToString() : "";
                                        _BulkAgentEntity.AEPS = !string.IsNullOrEmpty(dtExcelData.Rows[rows][2].ToString()) ? dtExcelData.Rows[rows][2].ToString() : "";
                                        _BulkAgentEntity.MATM = !string.IsNullOrEmpty(dtExcelData.Rows[rows][3].ToString()) ? dtExcelData.Rows[rows][3].ToString() : "";
                                        _BulkAgentEntity.DMT = !string.IsNullOrEmpty(dtExcelData.Rows[rows][4].ToString()) ? dtExcelData.Rows[rows][4].ToString() : "";
                                        _BulkAgentEntity.PanNo = !string.IsNullOrEmpty(dtExcelData.Rows[rows][5].ToString()) ? dtExcelData.Rows[rows][5].ToString() : "";
                                        _BulkAgentEntity.FirstName = !string.IsNullOrEmpty(dtExcelData.Rows[rows][6].ToString()) ? dtExcelData.Rows[rows][6].ToString() : "";
                                        _BulkAgentEntity.MiddleName = !string.IsNullOrEmpty(dtExcelData.Rows[rows][7].ToString()) ? dtExcelData.Rows[rows][7].ToString() : "";
                                        _BulkAgentEntity.LastName = !string.IsNullOrEmpty(dtExcelData.Rows[rows][8].ToString()) ? dtExcelData.Rows[rows][8].ToString() : "";
                                        _BulkAgentEntity.Gender = !string.IsNullOrEmpty(dtExcelData.Rows[rows][9].ToString()) ? dtExcelData.Rows[rows][9].ToString() : "";
                                        _BulkAgentEntity.AadharNo = !string.IsNullOrEmpty(dtExcelData.Rows[rows][10].ToString()) ? dtExcelData.Rows[rows][10].ToString() : "";
                                        _BulkAgentEntity.AgentCategory = !string.IsNullOrEmpty(dtExcelData.Rows[rows][11].ToString()) ? dtExcelData.Rows[rows][11].ToString() : "";
                                        _BulkAgentEntity.Dob = !string.IsNullOrEmpty(dtExcelData.Rows[rows][12].ToString()) ? dtExcelData.Rows[rows][12].ToString() : "";
                                        _BulkAgentEntity.AccountNumber = !string.IsNullOrEmpty(dtExcelData.Rows[rows][13].ToString()) ? dtExcelData.Rows[rows][13].ToString() : "";
                                        _BulkAgentEntity.IFSCCode = !string.IsNullOrEmpty(dtExcelData.Rows[rows][14].ToString()) ? dtExcelData.Rows[rows][14].ToString() : "";
                                        _BulkAgentEntity.PassPortNo = !string.IsNullOrEmpty(dtExcelData.Rows[rows][15].ToString()) ? dtExcelData.Rows[rows][15].ToString() : "";
                                        _BulkAgentEntity.DeviceCode = !string.IsNullOrEmpty(dtExcelData.Rows[rows][16].ToString()) ? dtExcelData.Rows[rows][16].ToString() : "";
                                        _BulkAgentEntity.PopulationGroup = !string.IsNullOrEmpty(dtExcelData.Rows[rows][17].ToString()) ? dtExcelData.Rows[rows][17].ToString() : "";
                                        _BulkAgentEntity.Address = !string.IsNullOrEmpty(dtExcelData.Rows[rows][18].ToString()) ? dtExcelData.Rows[rows][18].ToString() : "";
                                        _BulkAgentEntity.Country = !string.IsNullOrEmpty(dtExcelData.Rows[rows][19].ToString()) ? dtExcelData.Rows[rows][19].ToString() : "";
                                        _BulkAgentEntity.State = !string.IsNullOrEmpty(dtExcelData.Rows[rows][20].ToString()) ? dtExcelData.Rows[rows][20].ToString() : "";
                                        _BulkAgentEntity.District = !string.IsNullOrEmpty(dtExcelData.Rows[rows][21].ToString()) ? dtExcelData.Rows[rows][21].ToString() : "";
                                        _BulkAgentEntity.City = !string.IsNullOrEmpty(dtExcelData.Rows[rows][22].ToString()) ? dtExcelData.Rows[rows][22].ToString() : "";
                                        _BulkAgentEntity.Pincode = !string.IsNullOrEmpty(dtExcelData.Rows[rows][23].ToString()) ? dtExcelData.Rows[rows][23].ToString() : "";
                                        _BulkAgentEntity.EmailId = !string.IsNullOrEmpty(dtExcelData.Rows[rows][24].ToString()) ? dtExcelData.Rows[rows][24].ToString() : "";
                                        _BulkAgentEntity.ContactNo = !string.IsNullOrEmpty(dtExcelData.Rows[rows][25].ToString()) ? dtExcelData.Rows[rows][25].ToString() : "";
                                        _BulkAgentEntity.LandlineNo = !string.IsNullOrEmpty(dtExcelData.Rows[rows][26].ToString()) ? dtExcelData.Rows[rows][26].ToString() : "";
                                        _BulkAgentEntity.AlternateNo = !string.IsNullOrEmpty(dtExcelData.Rows[rows][27].ToString()) ? dtExcelData.Rows[rows][27].ToString() : "";
                                        _BulkAgentEntity.ShopAddress = !string.IsNullOrEmpty(dtExcelData.Rows[rows][28].ToString()) ? dtExcelData.Rows[rows][28].ToString() : "";
                                        _BulkAgentEntity.ShopCountry = !string.IsNullOrEmpty(dtExcelData.Rows[rows][29].ToString()) ? dtExcelData.Rows[rows][29].ToString() : "";
                                        _BulkAgentEntity.ShopState = !string.IsNullOrEmpty(dtExcelData.Rows[rows][30].ToString()) ? dtExcelData.Rows[rows][30].ToString() : "";
                                        _BulkAgentEntity.ShopDistrict = !string.IsNullOrEmpty(dtExcelData.Rows[rows][31].ToString()) ? dtExcelData.Rows[rows][31].ToString() : "";
                                        _BulkAgentEntity.ShopCity = !string.IsNullOrEmpty(dtExcelData.Rows[rows][32].ToString()) ? dtExcelData.Rows[rows][32].ToString() : "";
                                        _BulkAgentEntity.ShopPincode = !string.IsNullOrEmpty(dtExcelData.Rows[rows][33].ToString()) ? dtExcelData.Rows[rows][33].ToString() : "";
                                        _BulkAgentEntity.ShopEmailId = !string.IsNullOrEmpty(dtExcelData.Rows[rows][34].ToString()) ? dtExcelData.Rows[rows][34].ToString() : "";
                                        _BulkAgentEntity.ShopContactNo = !string.IsNullOrEmpty(dtExcelData.Rows[rows][35].ToString()) ? dtExcelData.Rows[rows][35].ToString() : "";
                                        _BulkAgentEntity.ShopLandlineNo = !string.IsNullOrEmpty(dtExcelData.Rows[rows][36].ToString()) ? dtExcelData.Rows[rows][36].ToString() : "";
                                        _BulkAgentEntity.ShopAlternateNo = !string.IsNullOrEmpty(dtExcelData.Rows[rows][37].ToString()) ? dtExcelData.Rows[rows][37].ToString() : "";
                                        _BulkAgentEntity.IdentityProofType = !string.IsNullOrEmpty(dtExcelData.Rows[rows][38].ToString()) ? dtExcelData.Rows[rows][38].ToString() : "";
                                        _BulkAgentEntity.IdentityProofDocument = ConfigurationManager.AppSettings["UploadBulkDocument"] + dtExcelData.Rows[rows][39].ToString();
                                        _BulkAgentEntity.AddressProofType = dtExcelData.Rows[rows][40].ToString();
                                        _BulkAgentEntity.AddressProofDocument = ConfigurationManager.AppSettings["UploadBulkDocument"] + dtExcelData.Rows[rows][41].ToString();
                                        _BulkAgentEntity.SignatureProofType = dtExcelData.Rows[rows][42].ToString();
                                        _BulkAgentEntity.SignatureProofDocument = ConfigurationManager.AppSettings["UploadBulkDocument"] + dtExcelData.Rows[rows][43].ToString();
                                        //_BulkAgentEntity.AgentCode = dtExcelData.Rows[rows][43].ToString();
                                        _BulkAgentEntity.TerminalId = !string.IsNullOrEmpty(dtExcelData.Rows[rows][44].ToString()) ? dtExcelData.Rows[rows][44].ToString() : "";

                                        _BulkAgentEntity.Lattitude = !string.IsNullOrEmpty(dtExcelData.Rows[rows][45].ToString()) ? dtExcelData.Rows[rows][45].ToString() : "";
                                        _BulkAgentEntity.Longitude = !string.IsNullOrEmpty(dtExcelData.Rows[rows][46].ToString()) ? dtExcelData.Rows[rows][46].ToString() : "";


                                        Random random = new Random();
                                        _BulkAgentEntity.RRN = GenerateRRN(random);

                                        if (ValidateFileData(_BulkAgentEntity, out StatusCode, out StatusDesc, out MsgID))
                                        {
                                            _successful += 1;
                                            _BulkAgentEntity.StatusDescription = StatusDesc;
                                            _BulkAgentEntity.Status = 1;
                                            _BulkAgentEntity.IsValidRecord = 1;
                                            _BulkAgentEntity.RecordID = rows;
                                            _BulkAgentEntity.MsgID = Convert.ToString(MsgID);
                                            AddRowToDataTable(_BulkAgentEntity);
                                        }
                                        else
                                        {
                                            _unsuessful += 1;
                                            _BulkAgentEntity.StatusDescription = StatusDesc;
                                            _BulkAgentEntity.Status = 0;
                                            _BulkAgentEntity.IsValidRecord = 0;
                                            _BulkAgentEntity.RecordID = rows;
                                            _BulkAgentEntity.MsgID = Convert.ToString(MsgID);
                                            AddRowToDataTable(_BulkAgentEntity);
                                        }
                                    }
                                    if (InsertFileData(FileId, Session["Username"].ToString(), TypeTable_BulkAgentRegistrationt, out statusmsg))
                                    {
                                        #region Audit
                                        _auditParams[0] = Session["Username"].ToString();
                                        _auditParams[1] = "Agent-Registration";
                                        _auditParams[2] = "btnBulkUpload";
                                        _auditParams[3] = Session["LoginKey"].ToString();
                                        _LoginEntity.StoreLoginActivities(_auditParams);
                                        #endregion
                                        _successful = _successful - _dupRcdsCnt;
                                        _unsuessful = _unsuessful + _dupRcdsCnt;

                                        //Upload documents present in zip file.
                                        UploadDocuments(zip, FileId); 
                                        if (_unsuessful == 0)
                                        {
                                            
                                            TotalRecords = Convert.ToString(_successful + _unsuessful);
                                            ErrorLog.AgentManagementTrace("Successful File Process For Agent Registration. FileType : " + _FileImport + " Total Record Processed : " + TotalRecords + " Successful : " + _successful + " Unsuccessful : " + _unsuessful);
                                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>showSuccess('Total Record Processed : " + TotalRecords + "</br>Successful : " + _successful + "</br>Unsuccessful : " + _unsuessful + " ', 'Successful');</script>", false);

                                            for (int i = 0;  i < TypeTable_BulkAgentRegistrationt.Rows.Count; i++)
                                            {
                                                //#region EMAIL
                                                //_EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                                                //_EmailSMSAlertscs.to = TypeTable_BulkAgentRegistrationt.Rows[i]["EmailId"].ToString();
                                                //_EmailSMSAlertscs.tempname = "SR24659_EBCP1";
                                                //_EmailSMSAlertscs.OTPFlag = "0";
                                                //_EmailSMSAlertscs.var1 = "SBM";
                                                //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                                //ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : Registration() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                                                //ErrorLog.SMSTrace("Page : AgentRegistration.cs \nFunction : Registration() => Reistration Details forwarded for Email Preparation. Email : " + _AgentRegistrationDAL.PersonalEmailID);
                                                //_EmailSMSAlertscs.HttpGetRequestEmail();
                                                //#endregion

                                                //#region SMS
                                                //_EmailSMSAlertscs.FROM = Session["Username"].ToString();
                                                //_EmailSMSAlertscs.to = TypeTable_BulkAgentRegistrationt.Rows[i]["ContactNo"].ToString();
                                                //_EmailSMSAlertscs.tempname = "SR24659_BCP4";
                                                //_EmailSMSAlertscs.OTPFlag = "0";
                                                //_EmailSMSAlertscs.var1 = "SBM";
                                                //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                                //ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : Registration() => Reistration Details forwarded for SMS Preparation. => HttpGetRequest()");
                                                //ErrorLog.SMSTrace("Page : AgentRegistration.cs \nFunction : Registration() => Reistration Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.ContactNo);
                                                //_EmailSMSAlertscs.HttpGetRequest();
                                                //#endregion

                                            }

                                        }
                                        else if (_unsuessful > 0)
                                        {
                                            TotalRecords = Convert.ToString(_successful + _unsuessful);
                                            ErrorLog.AgentManagementTrace("Successful File Process For Agent Registration. FileType : " + _FileImport + " Total Record Processed : " + TotalRecords + " Successful : " + _successful + " Unsuccessful : " + _unsuessful);
                                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Total record processed : " + TotalRecords + "</br>Valid records : " + _successful + " | Invalid records : " + _unsuessful + " ');</script>", false);
                                        }
                                    }
                                    else
                                    {
                                        ErrorLog.AgentManagementTrace("Failed To Insert File Data In Database For Agent Registration. FileType : " + _FileImport + " Description :" + statusmsg);
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Failed To Insert File Data. </br>" + statusmsg + "','Warning');</script>", false);
                                        return;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('System does not supports " + _fileExtension + " file format.','Warning');</script>", false);
                                    return;
                                }
                            }
                            else
                            {
                                ErrorLog.AgentManagementTrace(string.Format("Failed File Import Entry For Agent Registration. File Type : ", _FileImport));
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Something went wrong. Try again', 'Warning');", true);
                                return;
                            }
                        }
                        else
                        {
                            ErrorLog.AgentManagementTrace("Failed Validate ZIP File Folders Format For AgentId Registration Request. File Type : " + _FileImport);
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid file .</br> Invalid folder format uploaded in document file.');</script>", false);
                            return;
                        }
                    }
                    else
                    {
                        ErrorLog.AgentManagementTrace("Failed Validate File Format For AgentId Updation Request. File Type : " + _FileImport);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid file .</br> Extension of file should be .xls or .xlsx only.');</script>", false);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please select both excel and document file to upload .','Warning');</script>", false);
                    return;
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | btnBulkUpload_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }

            catch (ArgumentException) { }

            catch (Exception Ex)
            {
                ErrorLog.AgentManagementError(Ex);
                ErrorLog.UploadTrace("AgentRegistration : Error At btnBulkUpload_ServerClick(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Something went wrong. Try again', 'Warning');", true);
                return;
            }
            TabName.Value = "BulkAgentReg";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false); return;
        }
        public bool InsertFileData(string FileId, string Username, DataTable dtTable, out string _statusmsg)
        {
            ErrorLog.AgentManagementTrace("AgentRegistration | InsertFileData() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            ErrorLog.AgentManagementTrace(string.Format("Initiated Insert File Data For Agent Registration. FileType : {0}. FileID : {1}.", "Excel", FileId));
            try
            {
                if (!string.IsNullOrEmpty(FileId) && !string.IsNullOrEmpty(Username) && dtTable != null && dtTable.Rows.Count > 0)
                {
                    ErrorLog.AgentManagementTrace(string.Format("Initiated Insert File Data For Agent Registration. FileType : {0}. FileID : {1}.", "Excel", FileId));
                    _ImportEntity.FileId = FileId;
                    _ImportEntity.dataTable = dtTable;
                    _ImportEntity.UserName = Username;
                    _ImportEntity.FileTypeId = ((int)EnumCollection.EnumFileDesciption.UploadBulkAgentManualKYC);
                    string Status = _ImportEntity.InsertBulk(out statusmsg);
                    _statusmsg = statusmsg;
                    if (Status == "00")
                        ErrorLog.AgentManagementTrace(string.Format("Successful Insert File Data For Agent Registration. Username : {0}. FileID : {1}.", Username, FileId));
                    else
                        ErrorLog.AgentManagementTrace(string.Format("Failed Insert File Data For Agent Registration. Username : {0}. FileID : {1}.", Username, FileId));
                    return Status == "00" ? true : false;
                }
                else
                {
                    ErrorLog.AgentManagementTrace(string.Format("Failed Insert File Data For Agent Registration. Mandatory Fields Are Empty. Username : {0}. FileID : {1}.", Username, FileId));
                    _statusmsg = string.Empty;
                    return false;

                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace(string.Format("Failed Insert File Data For Agent Registration. FileType : {0}. FileID : {1}.", "Excel", FileId));
                _statusmsg = string.Empty;
                ErrorLog.AgentManagementError(Ex);
                return false;
            }
        }
        public bool ValidateFileFormat(string FileExtension)
        {
            try
            {
                ErrorLog.AgentManagementTrace(string.Format("Initiated Validate File Format For Agent Regirstration Upload. FileType : {0}.", FileExtension));
                return FileExtension.ToUpper() != ".XLS" && FileExtension.ToUpper() != ".XLSX" ? false : true;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace(string.Format("Failed Validate File Format For Agent Regirstration Upload. FileType : {0}.", FileExtension));
                ErrorLog.AgentManagementError(Ex);
                return false;
            }
        }
        public string SaveFile(FileUpload fileUpload)
        {
            try
            {
                ErrorLog.AgentManagementTrace(string.Format("Initiated Save File Request. File Type : {0}", fileUpload.PostedFile.FileName));
                if (fileUpload.HasFile)
                {
                    string PathLocation = AppDomain.CurrentDomain.BaseDirectory + "TempFiles" + "\\" + "Agent Manual KYC Bulk Import" + "\\";
                    if (!Directory.Exists(PathLocation))
                    {
                        Directory.CreateDirectory(PathLocation);
                    }
                    PathLocation += fileUpload.PostedFile.FileName + "_" + DateTime.Now.ToString("ddMMMyyHHmmss");
                    if (File.Exists(PathLocation))
                    {
                        File.Delete(PathLocation);
                        fileUpload.SaveAs(PathLocation);
                        ErrorLog.AgentManagementTrace("Successful Save File Request. File Type : " + fileUpload.PostedFile.FileName);
                    }
                    else
                    {
                        fileUpload.SaveAs(PathLocation);
                        ErrorLog.AgentManagementTrace("Successful Save File Request. File Type : " + fileUpload.PostedFile.FileName);
                    }
                    return PathLocation;
                }
                else
                {
                    ErrorLog.AgentManagementTrace("Failed Save File Request. FileUpload Object Has No Files.");
                    return string.Empty;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Failed Save File Request. File Type : " + fileUpload.PostedFile.FileName);
                ErrorLog.AgentManagementError(Ex);
                return string.Empty;
            }
        }
        public bool FileImportEntry(FileValidator _FileValidator, out string FileId)
        {
            {
                FileId = string.Empty;
                try
                {
                    ErrorLog.AgentManagementTrace("AgentRegistration | FileImportEntry() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    ErrorLog.AgentManagementTrace(string.Format("Initiated File Import  Request. Username : {0}. FilePath: {1}.FileID:", Session["Username"].ToString(), filePath, FileId));
                    _ImportEntity.filename = _FileValidator.FileName;
                    _ImportEntity.FilePath = _FileValidator.FilePath;
                    _ImportEntity.FileType = _FileValidator.FileType;
                    _ImportEntity.UserName = _FileValidator.Username;
                    _ImportEntity.FileDescID = Convert.ToString((int)EnumCollection.EnumFileDesciption.UploadBulkAgentManualKYC);
                    _ImportEntity.FileDescName = EnumCollection.EnumFileDesciption.UploadBulkAgentManualKYC.ToString();
                    _ImportEntity.Mode = "Insert";
                    string Status = _ImportEntity.InsertManuaBulkImportDetailsAgent(out FileId);
                    ErrorLog.AgentManagementTrace(string.Format("Completed File Import  Request. Username : {0}. FilePath: {1}.FileID:.", Session["Username"].ToString(), filePath, FileId));
                    ErrorLog.AgentManagementTrace("AgentRegistration | FileImportEntry() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    return Status == "00" ? true : false;
                }

                catch (Exception Ex)
                {
                    ErrorLog.AgentManagementTrace(string.Format("Failed Insert File Data Request For InsertFileData. FileId : {0}. Exception : {1}", FileId, Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString()));
                    ErrorLog.AgentManagementError(Ex);
                    return false;
                }
            }
        }
        public bool ProcessFile(string FileExtension, string FilePath, out DataTable dataTableExcel)
        {
            ErrorLog.AgentManagementTrace("AgentRegistration | ProcessFile() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            bool IsFileProcessed = true;
            dataTableExcel = new DataTable();
            ErrorLog.AgentManagementTrace(string.Format("Initiated Process File For Agent Registration. FileType : {0}", FilePath));
            if (FileExtension.ToLower() == ".xls")
            {
                _ConnctionString.Append("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath.ToString() + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
            }
            else if (FileExtension.ToLower() == ".xlsx")
            {
                _ConnctionString.Append("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath.ToString() + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"");
            }
            try
            {
                _OleDbConnection = new OleDbConnection(_ConnctionString.ToString());
                OleDbCommand cmd;
                _OleDbConnection.Open();
                cmd = new OleDbCommand("select * from [BulkData$]", _OleDbConnection);
                DataTable dtExcelSheetName1 = _OleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string getExcelSheetName = dtExcelSheetName1.Rows[0]["Table_Name"].ToString();
                cmd = new OleDbCommand("select * from [" + getExcelSheetName + "]", _OleDbConnection);
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
                dAdapter.SelectCommand = cmd;
                dAdapter.Fill(dataTableExcel);
                if (_OleDbConnection.State == ConnectionState.Open) _OleDbConnection.Close();
                cmd.Dispose();
                dAdapter.Dispose();
                ErrorLog.AgentManagementTrace(string.Format("Successful Process File ForAgent Registration. FileType : {0}", FilePath));

                if (dataTableExcel.Rows.Count > 0)
                {
                    if (_ImportFileFields != dataTableExcel.Columns.Count || _ImportFileFields < dataTableExcel.Columns.Count)
                    {
                        ErrorLog.AgentManagementTrace(string.Format("Failed Process File Request For Agent Regirstration . Username : {0}. FilePath : {1}.FileExtension : {2} Reason : No Files In File And Config Are Not Matching.", Page.User.Identity.Name.ToString(), FilePath, FileExtension));
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please check the number of fields in the  file.','File Format');</script>", false);
                        IsFileProcessed = false;
                    }
                }
                else
                {
                    ErrorLog.AgentManagementTrace(string.Format("Failed Process File Request For Agent Regirstration . Username : {0}. FilePath : {1}.FileExtension : {2} Reason : No Rows Found In Excel Data Table.", Page.User.Identity.Name.ToString(), FilePath, FileExtension));
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please check the number of fields in the file.','File Format');</script>", false);
                    IsFileProcessed = false;
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | ProcessFile() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.AgentManagementTrace(string.Format("Successful Process File Request For Bulk Ticket Creation. Username : {0}. FilePath : {1}.FileExtension : {2}.", Page.User.Identity.Name.ToString(), FilePath, FileExtension));
            }

            
            catch (ArgumentException Ex)
            {
                ErrorLog.AgentManagementError(Ex);
                ErrorLog.AgentManagementTrace(string.Format("Failed Process File For Agent Registration. FileType : {0}", FilePath));
                IsFileProcessed = false;
            }
            catch (OleDbException Ex)
            {
                ErrorLog.AgentManagementError(Ex);
                ErrorLog.AgentManagementTrace(string.Format("Failed Process File For Agent Registration. FileType : {0} Exception : {1}.", FilePath, Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString()));
                IsFileProcessed = false;
            }
            catch (InvalidOperationException Ex)
            {
                ErrorLog.AgentManagementTrace(string.Format("Failed Process File For Agent Registration. FileType : {0} Exception : {1} Message : {2].", FilePath, Ex.Message, "System does not supports " + FileExtension + " file format." + " | LoginKey : " + Session["LoginKey"].ToString()));
                IsFileProcessed = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('System does not supports " + FileExtension + " file format.','Warning');</script>", false);
                ErrorLog.AgentManagementError(Ex);
            }
            return IsFileProcessed;
        }
        public bool ValidateFileData<T>(T TObj, out string _StatusCode, out string _StatusDesc, out int MsgId)
        {
            ErrorLog.AgentManagementTrace("AgentRegistration | ValidateFileData() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            bool IsValidRecord = true;
            _StatusCode = string.Empty;
            _StatusDesc = string.Empty;
            MsgId = 0;
            // _StatusDesc = "Valid";
            bool Con;
            bool Email;
            bool Aadhar;
            bool pan;
            bool Agent;
            bool Terminal;
            String Mob;
            String mail;
            String adhar;
            String PanCard;
            string AgentCode;
            string TerminalId;
            try
            {
                BulkAgentEntity _bulkAgentEntity = (BulkAgentEntity)(object)TObj;
                ErrorLog.AgentManagementTrace(string.Format("Initiated Validate File Data For Agent Regirstration Request"));
                _CustomeRegExpValidation = new clsCustomeRegularExpressions();
                Processor processor = new Processor();
                string RespCode = string.Empty;
                string RespMessage = string.Empty;

                string firstName = string.Empty;
                string middleName = string.Empty;
                string lastName = string.Empty;
                
                if (string.IsNullOrEmpty(_bulkAgentEntity.BCID) || _bulkAgentEntity.BCID == "NULL")
                {
                    _StatusDesc = "Empty BCID";
                    IsValidRecord = false;
                    return false;
                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.AggegatorCode) || _bulkAgentEntity.AggegatorCode == "NULL")
                {
                    _StatusDesc = "Empty Aggegatorid";
                    IsValidRecord = false;
                    return false;
                }
                
                //if (string.IsNullOrEmpty(_bulkAgentEntity.AEPS) || _bulkAgentEntity.AEPS == "NULL")
                //{
                //    _StatusDesc = "Empty AEPS";
                //    IsValidRecord = false;
                //    return false;
                //}
                //if (string.IsNullOrEmpty(_bulkAgentEntity.MATM) || _bulkAgentEntity.MATM == "NULL")
                //{
                //    _StatusDesc = "Empty MATM";
                //    IsValidRecord = false;
                //    return false;
                //}
                //if (string.IsNullOrEmpty(_bulkAgentEntity.AgentCode) || _bulkAgentEntity.AgentCode == "NULL")
                //{
                //    _StatusDesc = "Empty AgentCode";
                //    IsValidRecord = false;
                //    return false;
                //}
                if (string.IsNullOrEmpty(_bulkAgentEntity.TerminalId) || _bulkAgentEntity.TerminalId == "NULL")
                {
                    _StatusDesc = "Empty TerminalId";
                    IsValidRecord = false;
                    return false;
                }

                if (string.IsNullOrEmpty(_bulkAgentEntity.PanNo))
                {
                    _StatusDesc = "Empty PanNo";
                    IsValidRecord = false;
                    return false;
                }

                if (string.IsNullOrEmpty(_bulkAgentEntity.FirstName))
                {
                    _StatusDesc = "Empty AgentName";
                    IsValidRecord = false;
                    return false;
                }

                //if (string.IsNullOrEmpty(_bulkAgentEntity.LastName))
                //{
                //    _StatusDesc = "Empty AgentName";
                //    IsValidRecord = false;
                //    return false;
                //}
                if (string.IsNullOrEmpty(_bulkAgentEntity.AadharNo))
                {
                    _StatusDesc = "Empty AadharNo";
                    IsValidRecord = false;
                    return false;
                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.Dob))
                {
                    _StatusDesc = "Empty Dob";
                    IsValidRecord = false;
                    return false;

                }


                if (string.IsNullOrEmpty(_bulkAgentEntity.DeviceCode) || _bulkAgentEntity.DeviceCode == "NULL")
                {
                    _StatusDesc = "Empty DeviceCode";
                    IsValidRecord = false;
                    return false;
                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.PopulationGroup))
                {
                    _StatusDesc = "Empty PopulationGroup";
                    IsValidRecord = false;
                    return false;
                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.Address))
                {
                    _StatusDesc = "Empty Address";
                    IsValidRecord = false;
                    return false;
                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.Country))
                {
                    _StatusDesc = "Empty Country";
                    IsValidRecord = false;
                    return false;
                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.State))
                {
                    _StatusDesc = "Empty State";
                    IsValidRecord = false;
                    return false;

                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.District))
                {
                    _StatusDesc = "Empty State";
                    IsValidRecord = false;
                    return false;

                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.City))
                {
                    _StatusDesc = "Empty City";
                    IsValidRecord = false;
                    return false;

                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.Pincode))
                {
                    _StatusDesc = "Empty Pincode";
                    IsValidRecord = false;
                    return false;
                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.EmailId))
                {
                    _StatusDesc = "Empty EmailId";
                    IsValidRecord = false;
                    return false;
                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.ContactNo))
                {
                    _StatusDesc = "Empty ContactNo";
                    IsValidRecord = false;
                    return false;
                }
                //if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbers, _bulkAgentEntity.AgentCode))
                //{
                //    _StatusDesc = "Invalid AgentCode";
                //    IsValidRecord = false;
                //    return false;
                //}
                //if (_bulkAgentEntity.AgentCode.Length > 15 || _bulkAgentEntity.AgentCode.Length < 3)
                //{
                //    _StatusDesc = "Invalid Agent Code";
                //    IsValidRecord = false;
                //    return false;
                //}

                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbers, _bulkAgentEntity.TerminalId))
                {
                    _StatusDesc = "Invalid TerminalId";
                    IsValidRecord = false;
                    return false;

                }
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbers, _bulkAgentEntity.BCID))
                {
                    _StatusDesc = "Invalid BCID";
                    IsValidRecord = false;
                    return false;

                }
                /////
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.date2, _bulkAgentEntity.Dob))
                {
                    _StatusDesc = "Invalid Dob";
                    IsValidRecord = false;
                    return false;

                }
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithSpace, _bulkAgentEntity.FirstName))
                {
                    _StatusDesc = " Invalid FirstName ";
                    IsValidRecord = false;
                    return false;
                }
                //if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithSpaceSC, _bulkAgentEntity.LastName))
                //{
                //    _StatusDesc = " Invalid LastName ";
                //    IsValidRecord = false;
                //    return false;

                //}
                if (_bulkAgentEntity.AadharNo.Length != 12)
                {
                    _StatusDesc = "Invalid Aadhar No";
                    IsValidRecord = false;
                    return false;
                }

                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.PanCard, _bulkAgentEntity.PanNo))
                {
                    _StatusDesc = " Invalid PanNo";
                    IsValidRecord = false;
                    return false;
                }
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Email, _bulkAgentEntity.EmailId))
                {
                    _StatusDesc = " Invalid EmailId";
                    IsValidRecord = false;
                    return false;
                }
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, _bulkAgentEntity.ContactNo))
                {
                    _StatusDesc = "Invalid ContactNo";
                    IsValidRecord = false;
                    return false;
                }
                if (_bulkAgentEntity.Pincode.Length != 6)
                {
                    _StatusDesc = "Invalid Pincode";
                    IsValidRecord = false;
                    return false;
                }
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Pincode, _bulkAgentEntity.Pincode))
                {
                    _StatusDesc = "Invalid Pincode";
                    IsValidRecord = false;
                    return false;
                }
                //if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.IFSC, _bulkAgentEntity.IFSCCode))
                //{
                //    _StatusDesc = "Invalid IFSCCode";
                //    IsValidRecord = false;
                //    return false;
                //}

                #region GeoCode Validation
                if (string.IsNullOrEmpty(_bulkAgentEntity.Lattitude))
                {
                    _StatusDesc = "Empty Latitude";
                    IsValidRecord = false;
                    return false;
                }
                if (string.IsNullOrEmpty(_bulkAgentEntity.Longitude))
                {
                    _StatusDesc = "Empty Longitude";
                    IsValidRecord = false;
                    return false;
                }
                //if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.GeoCode, _bulkAgentEntity.Lattitude))
                //{
                //    _StatusDesc = "Invalid Latitude";
                //    IsValidRecord = false;
                //}
                //if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.GeoCode, _bulkAgentEntity.Longitude))
                //{
                //    _StatusDesc = "Invalid Longitude";
                //    IsValidRecord = false;
                //} 
                #endregion

                if (!string.IsNullOrEmpty(_bulkAgentEntity.BCID))
                {
                    _bulkAgentEntity.BCID = _bulkAgentEntity.BCID;
                    _bulkAgentEntity.Flag = "1";
                    _bulkAgentEntity.ImportBulkKycValidateData(out string StatusCode, out string SatatusDesc);
                    _StatusDesc = SatatusDesc;
                    _StatusCode = StatusCode;
                    IsValidRecord = _StatusCode == "00" ? true : false;
                    if (IsValidRecord == false)
                    {
                        return false;
                    }
                }
                //if (!string.IsNullOrEmpty(_bulkAgentEntity.Pincode))
                //{
                //    _bulkAgentEntity.Pincode = _bulkAgentEntity.Pincode;
                //    _bulkAgentEntity.Flag = "6";
                //    _bulkAgentEntity.ImportBulkKycValidateData(out string StatusCode, out string SatatusDesc);
                //    _StatusDesc = SatatusDesc;
                //    _StatusCode = StatusCode;
                //    IsValidRecord = _StatusCode == "00" ? true : false;

                //    if (IsValidRecord == false)
                //    {
                //        return false;
                //    }
                //}
                if (!string.IsNullOrEmpty(_bulkAgentEntity.PanNo))
                {
                    _bulkAgentEntity.PanNo = _bulkAgentEntity.PanNo;
                    _bulkAgentEntity.BCID = _BulkAgentEntity.BCID;
                    _bulkAgentEntity.Flag = "7";
                    _bulkAgentEntity.ImportBulkKycValidateData(out string StatusCode, out string SatatusDesc);
                    _StatusDesc = SatatusDesc;
                    _StatusCode = StatusCode;
                    IsValidRecord = _StatusCode == "00" ? true : false;

                    if (IsValidRecord == false)
                    {
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(_bulkAgentEntity.AgentCode))
                {
                    _bulkAgentEntity.AgentCode = _bulkAgentEntity.AgentCode;
                    _bulkAgentEntity.Flag = "8";
                    _bulkAgentEntity.ImportBulkKycValidateData(out string StatusCode, out string SatatusDesc);
                    _StatusDesc = SatatusDesc;
                    _StatusCode = StatusCode;
                    IsValidRecord = _StatusCode == "00" ? true : false;

                    if (IsValidRecord == false)
                    {
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(_bulkAgentEntity.TerminalId))
                {
                    _bulkAgentEntity.TerminalId = _bulkAgentEntity.TerminalId;
                    _bulkAgentEntity.Flag = "9";
                    _bulkAgentEntity.ImportBulkKycValidateData(out string StatusCode, out string SatatusDesc);
                    _StatusDesc = SatatusDesc;
                    _StatusCode = StatusCode;
                    IsValidRecord = _StatusCode == "00" ? true : false;

                    if (IsValidRecord == false)
                    {
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(_bulkAgentEntity.AadharNo) || !string.IsNullOrEmpty(_bulkAgentEntity.PanNo) || !string.IsNullOrEmpty(_bulkAgentEntity.EmailId) || !string.IsNullOrEmpty(_bulkAgentEntity.ContactNo) || !string.IsNullOrEmpty(_bulkAgentEntity.AgentCode) || !string.IsNullOrEmpty(_bulkAgentEntity.TerminalId))
                {
                    _bulkAgentEntity.AadharNo = _bulkAgentEntity.AadharNo;
                    _bulkAgentEntity.PanNo = _bulkAgentEntity.PanNo;
                    _bulkAgentEntity.EmailId = _bulkAgentEntity.EmailId;
                    _bulkAgentEntity.ContactNo = _bulkAgentEntity.ContactNo;
                    _bulkAgentEntity.Flag = "5";

                    _bulkAgentEntity.ImportBulkKycValidateData(out string StatusCode, out string SatatusDesc);
                    _StatusDesc = SatatusDesc;
                    _StatusCode = StatusCode;
                    IsValidRecord = _StatusCode == "00" ? true : false;
                    if (IsValidRecord == false)
                    {
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(_bulkAgentEntity.AadharNo) || !string.IsNullOrEmpty(_bulkAgentEntity.PanNo) || !string.IsNullOrEmpty(_bulkAgentEntity.EmailId) || !string.IsNullOrEmpty(_bulkAgentEntity.ContactNo) || !string.IsNullOrEmpty(_bulkAgentEntity.AgentCode) || !string.IsNullOrEmpty(_bulkAgentEntity.TerminalId))
                {
                    Mob = _BulkAgentEntity.ContactNo;
                    if (Con = TypeTable_BulkAgentRegistrationt.AsEnumerable().Any(row => Mob == row.Field<String>("ContactNo")))
                    {
                        _StatusDesc = "Duplicate Contact Number";
                        IsValidRecord = false;
                        return false;

                    }
                    adhar = _BulkAgentEntity.AadharNo;
                    if (Aadhar = TypeTable_BulkAgentRegistrationt.AsEnumerable().Any(row => adhar == row.Field<String>("AadharNo")))
                    {

                        _StatusDesc = "Duplicate Aadhar Number";
                        IsValidRecord = false;
                        return false;
                    }
                    mail = _BulkAgentEntity.EmailId;
                    if (Email = TypeTable_BulkAgentRegistrationt.AsEnumerable().Any(row => mail == row.Field<String>("EmailId")))
                    {
                        _StatusDesc = "Duplicate EmailId";

                        IsValidRecord = false;
                        return false;

                    }
                    PanCard = _BulkAgentEntity.PanNo;
                    if (pan = TypeTable_BulkAgentRegistrationt.AsEnumerable().Any(row => PanCard == row.Field<String>("PanNo")))
                    {
                        _StatusDesc = "Duplicate PanNo";

                        IsValidRecord = false;
                        return false;

                    }

                    //AgentCode = _BulkAgentEntity.AgentCode;
                    //if (pan = TypeTable_BulkAgentRegistrationt.AsEnumerable().Any(row => PanCard == row.Field<String>("AgentCode")))
                    //{
                    //    _StatusDesc = "Duplicate AgentCode";

                    //    IsValidRecord = false;
                    //    return false;

                    //}
                    TerminalId = _BulkAgentEntity.TerminalId;
                    if (pan = TypeTable_BulkAgentRegistrationt.AsEnumerable().Any(row => PanCard == row.Field<String>("TerminalId")))
                    {
                        _StatusDesc = "Duplicate TerminalId";

                        IsValidRecord = false;
                        return false;

                    }
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | ValidateFileData() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }

            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AgentRegistration: ValidateFileData(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                IsValidRecord = false;
            }
            return IsValidRecord;
        }
        public void AddRowToDataTable<T>(T TObj)
        {
            bool Conact;
            bool Email;
            bool Aadhar;
            bool pan;
            bool Agent;
            bool Terminal;


            String Mob;
            String mail;
            String adhar;
            String PanCard;
            string AgentCode;
            string TerminalId;


            try
            {
                BulkAgentEntity _BulkAgentEntity = (BulkAgentEntity)(object)TObj;

                DataRow _RowTypeTable = TypeTable_BulkAgentRegistrationt.NewRow();
                _RowTypeTable["BCID"] = _BulkAgentEntity.BCID;
                _RowTypeTable["Aggregatorid"] = _BulkAgentEntity.AggegatorCode;
                _RowTypeTable["AEPS"] = _BulkAgentEntity.AEPS;
                _RowTypeTable["MATM"] = _BulkAgentEntity.MATM;
                _RowTypeTable["DMT"] = _BulkAgentEntity.DMT;
                _RowTypeTable["PanNo"] = _BulkAgentEntity.PanNo;
                _RowTypeTable["FirstName"] = _BulkAgentEntity.FirstName;
                _RowTypeTable["MiddleName"] = _BulkAgentEntity.MiddleName;
                _RowTypeTable["LastName"] = _BulkAgentEntity.LastName;
                _RowTypeTable["Gender"] = _BulkAgentEntity.Gender;
                _RowTypeTable["AadharNo"] = _BulkAgentEntity.AadharNo;
                string adr = _BulkAgentEntity.AadharNo;
                _BulkAgentEntity.AadharNo = MaskingAadhar(adr);
                _RowTypeTable["AadharNo"] = _BulkAgentEntity.AadharNo;
                _RowTypeTable["AgentCategory"] = _BulkAgentEntity.AgentCategory;
                _RowTypeTable["Dob"] = _BulkAgentEntity.Dob;
                _RowTypeTable["AccountNumber"] = _BulkAgentEntity.AccountNumber;
                _RowTypeTable["IFSCCode"] = _BulkAgentEntity.IFSCCode;
                _RowTypeTable["PassPortNo"] = _BulkAgentEntity.PassPortNo;
                _RowTypeTable["DeviceCode"] = _BulkAgentEntity.DeviceCode;
                _RowTypeTable["PopulationGroup"] = _BulkAgentEntity.PopulationGroup;
                _RowTypeTable["Address"] = _BulkAgentEntity.Address;
                _RowTypeTable["Country"] = _BulkAgentEntity.Country;
                _RowTypeTable["State"] = _BulkAgentEntity.State;
                _RowTypeTable["District"] = _BulkAgentEntity.District;
                _RowTypeTable["City"] = _BulkAgentEntity.City;
                _RowTypeTable["Pincode"] = _BulkAgentEntity.Pincode;
                _RowTypeTable["EmailId"] = _BulkAgentEntity.EmailId;
                _RowTypeTable["ContactNo"] = _BulkAgentEntity.ContactNo;
                _RowTypeTable["LandlineNo"] = _BulkAgentEntity.LandlineNo;
                _RowTypeTable["AlternateNo"] = _BulkAgentEntity.AlternateNo;
                _RowTypeTable["ShopAddress"] = _BulkAgentEntity.ShopAddress;
                _RowTypeTable["ShopCountry"] = _BulkAgentEntity.ShopCountry;
                _RowTypeTable["ShopState"] = _BulkAgentEntity.ShopState;
                _RowTypeTable["ShopDistrict"] = _BulkAgentEntity.ShopDistrict;
                _RowTypeTable["ShopCity"] = _BulkAgentEntity.ShopCity;
                _RowTypeTable["ShopPincode"] = _BulkAgentEntity.ShopPincode;
                _RowTypeTable["ShopEmailId"] = _BulkAgentEntity.ShopEmailId;
                _RowTypeTable["ShopContactNo"] = _BulkAgentEntity.ShopContactNo;
                _RowTypeTable["ShopLandlineNo"] = _BulkAgentEntity.ShopLandlineNo;
                _RowTypeTable["ShopAlternateNo"] = _BulkAgentEntity.ShopAlternateNo;
                _RowTypeTable["IdentityProofType"] = _BulkAgentEntity.IdentityProofType;
                _RowTypeTable["IdentityProofDocument"] = _BulkAgentEntity.IdentityProofDocument;
                _RowTypeTable["AddressProofType"] = _BulkAgentEntity.AddressProofType;
                _RowTypeTable["AddressProofDocument"] = _BulkAgentEntity.AddressProofDocument;
                _RowTypeTable["SignatureProofType"] = _BulkAgentEntity.SignatureProofType;
                _RowTypeTable["SignatureProofDocument"] = _BulkAgentEntity.SignatureProofDocument;
                _RowTypeTable["AgentCode"] = _BulkAgentEntity.AgentCode;
                _RowTypeTable["TerminalId"] = _BulkAgentEntity.TerminalId;
                _RowTypeTable["AgentRefId"] = _BulkAgentEntity.RRN;

                _RowTypeTable["IsValidRecord"] = _BulkAgentEntity.IsValidRecord;
                _RowTypeTable["RecordStatus"] = _BulkAgentEntity.RecordStatus;
                _RowTypeTable["FileStatus"] = _BulkAgentEntity.FileStatus;
                _RowTypeTable["RecordID"] = _BulkAgentEntity.RecordID;

                _RowTypeTable["Status"] = _BulkAgentEntity.Status;
                _RowTypeTable["StatusDescription"] = _BulkAgentEntity.StatusDescription;
                _RowTypeTable["MSGID"] = _BulkAgentEntity.MsgID;

                _RowTypeTable["Lattitude"] = _BulkAgentEntity.Lattitude;
                _RowTypeTable["Longitude"] = _BulkAgentEntity.Longitude;


                TypeTable_BulkAgentRegistrationt.Rows.Add(_RowTypeTable);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementError(Ex);
            }
        }

        #region ConvertDate
        public string ConvertDate(string inputDate)
        {
            string _returnString = string.Empty, DateFormat = null, TempinputDate = null;
            char[] RemoveSpecilChars = { '.', '-', '/', ' ' };
            try
            {
                if (!string.IsNullOrEmpty(inputDate))
                {
                    CultureInfo ci = CultureInfo.CurrentCulture;
                    DateTimeFormatInfo dtfi = ci.DateTimeFormat;
                    string[] SystemDateTimePatterns = new string[250];
                    int i = 0;
                    foreach (string name in dtfi.GetAllDateTimePatterns('d'))
                    {
                        SystemDateTimePatterns[i] = name;
                        i++;
                    }
                    DateFormat = SystemDateTimePatterns[0].ToString();



                    // writeLog("Page : clsCommandFunctions.cs \nFunction : ConvertDate() \n Server Date Format  : " + DateFormat + " inputDate : " + inputDate);



                    TempinputDate = inputDate;
                    TempinputDate = TempinputDate.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "").Trim();
                    DateFormat = DateFormat.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "").Trim();

                    string[] splitedDate = inputDate.Split(RemoveSpecilChars, StringSplitOptions.RemoveEmptyEntries);
                    if (splitedDate.Count() > 0)
                    {
                        _returnString = splitedDate[0].ToString().Trim() + "/" + splitedDate[1].ToString().Trim() + "/" + splitedDate[2].ToString().Trim();
                    }

                }
            }
            catch (Exception Ex)
            {

            }
            return _returnString;
        }
        #endregion

        #endregion
        private string GenerateRRN(Random random)
        {
            try
            {
                return DateTime.Now.ToString("yyMMddHHmmssss") + random.Next(111, 999).ToString("D3");
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Generate requestNumber Failed (Agent Registration). Exception : " + ex.Message);
                return string.Empty;
            }
        }
        protected void btnUploadCancel_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnUploadCancel_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "btnUploadCancel";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                txtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                fileUpload.Dispose();
                gvBulkManualKyc.Visible = false;
                ErrorLog.AgentManagementTrace("AgentRegistration | btnUploadCancel_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false); return;
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("RestrictedName : Error At btnUploadCancel_ServerClick(). Username : " + Session["UserName"].ToString() + " Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        protected void UploadDocuments(ZipFile zip, string fileid)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | UploadDocuments() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "UploadDocuments";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                bool isFolderExist = false;
                string FileThumbnail = string.Empty;
                string _FileImport = DocUpload.FileName;
                string _fileExtension = Path.GetExtension(_FileImport);
                string FILENAME = Path.GetFileNameWithoutExtension(_FileImport);
                DataTable dtExcelData = new DataTable();
                if (_FileImport != "" && DocUpload.HasFile)
                {
                    ErrorLog.AgentManagementTrace("Received For Agent Restriction Bulk Upload Request. FileType : " + _FileImport);
                    if (ValidateZipFileFormat(_fileExtension))
                    {
                        ErrorLog.AgentManagementTrace("Successful Validate File Format For Agent Registration . FileType : " + _FileImport);
                        filePath = null; string FileId = string.Empty;
                        filePath = SaveZipFile(DocUpload, fileid); //saves zip file

                        string extractPath = Server.MapPath("~/Files/");
                        string PathLocation = AppDomain.CurrentDomain.BaseDirectory + "Agent Manual KYC Bulk Import" + "\\";
                        if (!Directory.Exists(PathLocation))
                        {
                            Directory.CreateDirectory(PathLocation);
                        }

                        zip.ExtractAll(PathLocation, ExtractExistingFileAction.OverwriteSilently);

                        foreach (ZipEntry Zipentry in zip.Entries)
                        {
                            if (Zipentry.FileName.Contains("AgentBulkDocuments"))
                            {
                                FileThumbnail = PathLocation + "AgentBulkDocuments\\" + Path.GetFileNameWithoutExtension(Zipentry.FileName) + "_Thumbnail.png";
                                string file = PathLocation + "\\" + Zipentry.FileName;
                                FileInfo fi1 = new FileInfo(file);
                                if (File.Exists(file))
                                {
                                    if (Path.GetExtension(Zipentry.FileName) == ".pdf")
                                    {
                                        string png_filename = PathLocation + "\\" + "AgentBulkDocuments" + "\\" + Path.GetFileNameWithoutExtension(Zipentry.FileName) + "_Thumbnail.png";
                                        List<string> errors = cs_pdf_to_image.Pdf2Image.Convert(file, png_filename);
                                    }
                                    else
                                    {
                                        if (!File.Exists(FileThumbnail))
                                        {
                                            fi1.CopyTo(FileThumbnail);
                                        }
                                    }
                                }

                            }
                        }

                    }
                    else
                    {
                        ErrorLog.UploadTrace("AgentRegistration : Error At btnDocUpload_ServerClick().Invalid File Format. FILENAME : " + _FileImport);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning(Please upload ZIP file', 'Warning');", true);
                    }
                }
                else
                {
                    ErrorLog.UploadTrace("AgentRegistration : Error At btnDocUpload_ServerClick().Invalid File Uploaded. FILENAME : " + _FileImport);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning(Please upload valid ZIP file name', 'Warning');", true);
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | UploadDocuments() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.UploadTrace("Agent Bulk Registration : Error At UploadDocuments(). Username : " + Session["UserName"].ToString() + " Exception : " + ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
            }
        }
        protected void btnDocUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentRegistration | btnDocUpload_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Registration";
                _auditParams[2] = "btnDocUpload";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                string TotalRecords = null;
                //lblUploadMessage.Text = string.Empty;
                bool isFolderExist = false;
                string FileThumbnail, fileid = string.Empty;
                string _FileImport = DocUpload.FileName;
                string _fileExtension = Path.GetExtension(_FileImport);
                string FILENAME = Path.GetFileNameWithoutExtension(_FileImport);
                DataTable dtExcelData = new DataTable();
                if (_FileImport != "" && DocUpload.HasFile && FILENAME == "AgentBulkDocuments")
                {
                    ErrorLog.AgentManagementTrace("Received For Agent Restriction Bulk Upload Request. FileType : " + _FileImport);
                    if (ValidateZipFileFormat(_fileExtension))
                    {
                        ErrorLog.AgentManagementTrace("Successful Validate File Format For Agent Registration . FileType : " + _FileImport);
                        filePath = null; string FileId = string.Empty;
                        filePath = SaveZipFile(DocUpload, fileid);

                        string extractPath = Server.MapPath("~/Files/");
                        string PathLocation = AppDomain.CurrentDomain.BaseDirectory + "Agent Manual KYC Bulk Import" + "\\";
                        if (!Directory.Exists(PathLocation))
                        {
                            Directory.CreateDirectory(PathLocation);
                        }
                        using (ZipFile zip = ZipFile.Read(DocUpload.PostedFile.InputStream))
                        {
                            //  int totalentries = zip.Entries.Count;
                            foreach (ZipEntry Zipentry in zip.Entries)
                            {
                                if (Zipentry.FileName.Contains("AgentBulkDocuments/"))
                                {
                                    isFolderExist = true;
                                    break;
                                }
                            }
                            if (isFolderExist)
                            {
                                zip.ExtractAll(PathLocation, ExtractExistingFileAction.OverwriteSilently);

                                foreach (ZipEntry Zipentry in zip.Entries)
                                {
                                    if (Zipentry.FileName.Contains("AgentBulkDocuments/"))
                                    {
                                        FileThumbnail = PathLocation + "AgentBulkDocuments/" + Path.GetFileNameWithoutExtension(Zipentry.FileName) + "_Thumbnail.png";
                                        string file = PathLocation + "\\" + Zipentry.FileName;

                                        FileInfo fi1 = new FileInfo(file);
                                        //  FileInfo fi2 = new FileInfo(FileThumbnail);
                                        if (File.Exists(file))
                                        {
                                            if (Path.GetExtension(Zipentry.FileName) == ".pdf")
                                            {
                                                string png_filename = PathLocation + "\\" + "AgentBulkDocuments" + "\\" + Path.GetFileNameWithoutExtension(Zipentry.FileName) + "_Thumbnail.png";
                                                List<string> errors = cs_pdf_to_image.Pdf2Image.Convert(file, png_filename);
                                            }
                                            else
                                            {
                                                if (!File.Exists(FileThumbnail))
                                                {
                                                    fi1.CopyTo(FileThumbnail);
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ErrorLog.UploadTrace("AgentRegistration : Error At btnDocUpload_ServerClick().Invalid File Format. FILENAME : " + _FileImport);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning(Please upload ZIP file', 'Warning');", true);
                    }
                }
                else
                {
                    ErrorLog.UploadTrace("AgentRegistration : Error At btnDocUpload_ServerClick().Invalid File Uploaded. FILENAME : " + _FileImport);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning(Please upload valid ZIP file name', 'Warning');", true);
                }
                ErrorLog.AgentManagementTrace("AgentRegistration | btnDocUpload_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: PbtnDocUpload_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        public bool ValidateZipFileFormat(string FileExtension)
        {
            try
            {
                ErrorLog.AgentManagementTrace(string.Format("Initiated Validate File Format For Agent Regirstration Upload. FileType : {0}.", FileExtension));
                return FileExtension.ToUpper() != ".ZIP" ? false : true;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace(string.Format("Failed Validate File Format For Agent Regirstration Upload. FileType : {0}.", FileExtension));
                ErrorLog.AgentManagementError(Ex);
                return false;
            }
        }
        public string SaveZipFile(FileUpload DocUpload, string fileid)
        {
            try
            {
                ErrorLog.AgentManagementTrace(string.Format("Initiated Save File Request. File Type : {0}", DocUpload.PostedFile.FileName));
                if (DocUpload.HasFile)
                {
                    string PathLocation = AppDomain.CurrentDomain.BaseDirectory + "TempFiles" + "\\" + "Agent Manual KYC Bulk Import" + "\\" + fileid + "\\";
                    if (!Directory.Exists(PathLocation))
                    {
                        Directory.CreateDirectory(PathLocation);
                    }
                    PathLocation += fileid + ".zip";
                    if (File.Exists(PathLocation))
                    {
                        File.Delete(PathLocation);
                        DocUpload.SaveAs(PathLocation);
                        ErrorLog.AgentManagementTrace("Successful Save File Request. File Type : " + DocUpload.PostedFile.FileName);
                    }
                    else
                    {
                        DocUpload.SaveAs(PathLocation);
                        ErrorLog.AgentManagementTrace("Successful Save File Request. File Type : " + DocUpload.PostedFile.FileName);
                    }
                    return PathLocation;
                }
                else
                {
                    ErrorLog.AgentManagementTrace("Failed Save File Request. DocUpload Object Has No Files.");
                    return string.Empty;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Failed Save File Request. File Type : " + DocUpload.PostedFile.FileName);
                ErrorLog.AgentManagementError(Ex);
                return string.Empty;
            }
        }
        protected void txtPinCode_TextChanged(object sender, EventArgs e)
        {
            _AgentRegistrationDAL.AgentPincode = txtPinCode.Text.Trim();
            BindDropdownCountryState();
        }
        protected void txtshoppin_TextChanged(object sender, EventArgs e)
        {
            _AgentRegistrationDAL.ShopPinCode = txtshoppin.Text.Trim();
            BindShopCountryState();
        }
        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlCity.DataSource = null;
                ddlCity.SelectedValue = null;
                ddlCity.ClearSelection();

                _AgentRegistrationDAL.AgentDistrict = string.Empty;
                _AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.City;
                _AgentRegistrationDAL.AgentDistrict = ddlDistrict.SelectedValue.ToString();
                //DataSet ds_District = _AgentRegistrationDAL.GetCountryStateCity(UserName, Mode, 0, Convert.ToInt32(_AgentRegistrationDAL.AgentState));
                DataSet ds_City = _AgentRegistrationDAL.GetCountryStateCityD();
                if (ds_City != null && ds_City.Tables.Count > 0 && ds_City.Tables[0].Rows.Count > 0)
                {
                    //ddlCity.DataSource = ds_State;
                    //ddlCity.DataValueField = "ID";
                    //ddlCity.DataTextField = "Name";
                    //ddlCity.DataBind();
                    //ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                    ddlCity.DataSource = ds_City;
                    ddlCity.DataValueField = "City";
                    ddlCity.DataTextField = "City";
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    ddlCity.DataSource = null;
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : ddlDistrict_SelectedIndexChanged() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.', 'Alert');", true);
            }
        }
        protected void ddlShopDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlShopCity.DataSource = null;
                ddlShopCity.SelectedValue = null;
                ddlShopCity.ClearSelection();

                _AgentRegistrationDAL.ShopCity = string.Empty;
                _AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.City;
                _AgentRegistrationDAL.ShopDistrict = ddlShopDistrict.SelectedValue.ToString();
                DataSet ds_City = _AgentRegistrationDAL.GetShopCountryStateCity();
                if (ds_City != null && ds_City.Tables.Count > 0 && ds_City.Tables[0].Rows.Count > 0)
                {
                    ddlShopCity.DataSource = ds_City;
                    ddlShopCity.DataValueField = "City";
                    ddlShopCity.DataTextField = "City";
                    ddlShopCity.DataBind();
                    ddlShopCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    ddlShopCity.DataSource = null;
                    ddlShopCity.DataBind();
                    ddlShopCity.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : ddlShopDistrict_SelectedIndexChanged() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.', 'Alert');", true);
            }
        }
    }
}



