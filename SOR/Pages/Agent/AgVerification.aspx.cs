using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using BussinessAccessLayer;
using AppLogger;
using System.Net;
using System.Threading;
using MaxiSwitch.EncryptionDecryption;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace SOR.Pages.Agent
{
    public partial class AgVerification : System.Web.UI.Page
    {
        #region Property Declaration
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();
        ImportEntity _ImportEntity = new ImportEntity();
        public ExportFormat _exportFormat = null;
        public ExportFormat exportFormat
        {
            get { if (_exportFormat == null) _exportFormat = new ExportFormat(); return _exportFormat; }
            set { _exportFormat = value; }
        }
        #endregion

        #region Variable and Objects Declaration
        EmailSMSAlertscs _EmailSMSAlertscs = new EmailSMSAlertscs();
        EmailAlerts _EmailAlerts = new EmailAlerts();
        AppSecurity appSecurity = new AppSecurity();
        public AppSecurity _AppSecurity
        {
            get { if (appSecurity == null) appSecurity = new AppSecurity(); return appSecurity; }
            set { appSecurity = value; }
        }
        public string UserName { get; set; }
        public string pathId, PathAdd, PathSig;
        string _DefaultPassword = ConnectionStringEncryptDecrypt.DecryptEncryptedDEK(AppSecurity.GenerateDfPw(), ConnectionStringEncryptDecrypt.ClearMEK);
        DataSet _dsVerification = null;
        DataSet _dsActivate = null;

        int _successful = 0,
            _unsuessful = 0,
            _reocrdsProcessed = 0,
            _DeclineCardCount = 0,
            _Flag = 0;

        string
            _strAlertMessage_Header = null,
            _strAlertMessage_Success = null,
            _strAlertMessage_UnSuccess = null,
            _strAlertMessage_Total = null,
            _fileLineNo = null;

        string _salt = string.Empty;
        public enum SelectionType
        {
            CheckedAll = 1,
            UnCheckAll = 2
        }
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["UserName"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["UserName"].ToString(), Session["UserRoleID"].ToString(), "AgVerification.aspx", "19");
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
                        if (!IsPostBack)
                        {
                            Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                            BindDropdownsBC();
                            //FillAggregator();
                            //BindDropdownsAgent();
                            //BindFileId();
                            FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                            FillGridBulk(EnumCollection.EnumBindingType.AgBulkL3);
                            ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                            UserPermissions.RegisterStartupScriptForNavigationList("6", "19", "Manual", "docket-tab");
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
                ErrorLog.CommonTrace("Page : AgentVerification.cs \nFunction : Page_Load() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Methods

        private void BindDropdownsBC()
        {
            try
            {
                ddlBCCode.Items.Clear();
                ddlBCCode.DataSource = null;
                ddlBCCode.DataBind();
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
                _AgentRegistrationDAL.IsRemoved = "0";
                _AgentRegistrationDAL.IsActive = "0";
                 _AgentRegistrationDAL.IsdocUploaded = "1";
                _AgentRegistrationDAL.VerificationStatus = 0;
                _AgentRegistrationDAL.BCCode = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;

                DataTable ds = _AgentRegistrationDAL.GetBC(_AgentRegistrationDAL.UserName, 0, 1, 0, ClientID, 1);

                if (ds != null && ds.Rows.Count > 0 && ds.Rows.Count > 0)
                {
                    if (ds.Rows.Count == 1)
                    {
                        ddlBCCode.Items.Clear();
                        ddlBCCode.DataSource = ds.Copy();
                        ddlBCCode.DataTextField = "BCNameDetails";
                        ddlBCCode.DataValueField = "BCCode";
                        ddlBCCode.DataBind();
                        ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlBCCode.Items.Clear();
                        ddlBCCode.DataSource = ds.Copy();
                        ddlBCCode.DataTextField = "BCNameDetails";
                        ddlBCCode.DataValueField = "BCCode";
                        ddlBCCode.DataBind();
                        ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlBCCode.DataSource = null;
                    ddlBCCode.DataBind();
                    ddlBCCode.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: BindDropdownsBC: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        private void BindDropdownsAgent()
        {
            try
            {
                ddlAgentCode.Items.Clear();
                ddlAgentCode.DataSource = null;
                ddlAgentCode.DataBind();
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.EnableMakerChecker;// EnableRoles;
                _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
                _AgentRegistrationDAL.IsRemoved = "0";
                _AgentRegistrationDAL.IsActive = "0";
                _AgentRegistrationDAL.IsdocUploaded = "1";
                _AgentRegistrationDAL.VerificationStatus = 0;
                _AgentRegistrationDAL.AggCode = ddlaggregatorCode.SelectedValue != "0" ? ddlaggregatorCode.SelectedValue : null;
                _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                DataSet ds = _AgentRegistrationDAL.BindAgentVerifyddl();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        ddlAgentCode.Items.Clear();
                        ddlAgentCode.DataSource = ds.Tables[0].Copy();
                        ddlAgentCode.DataTextField = "AgentName";
                        ddlAgentCode.DataValueField = "ID";
                        ddlAgentCode.DataBind();
                        ddlAgentCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlAgentCode.Items.Clear();
                        ddlAgentCode.DataSource = ds.Tables[0].Copy();
                        ddlAgentCode.DataTextField = "AgentName";
                        ddlAgentCode.DataValueField = "ID";
                        ddlAgentCode.DataBind();
                        ddlAgentCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlAgentCode.DataSource = null;
                    ddlAgentCode.DataBind();
                    ddlAgentCode.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: BindDropdownsAgent: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        public DataSet FillGrid(EnumCollection.EnumPermissionType _EnumPermissionType)
        {
            DataSet ds = new DataSet();
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                gvAgent.DataSource = null;
                gvAgent.DataBind();
                lblRecordsTotal.Text = "";
                ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                _AgentRegistrationDAL.Flag = ((int)EnumCollection.EnumPermissionType.AgL2Export);
                _AgentRegistrationDAL.BCCode = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;
                _AgentRegistrationDAL.AgentCode = ddlAgentCode.SelectedValue != "0" ? ddlAgentCode.SelectedValue : null;
                _AgentRegistrationDAL.Activity = ddlActivityType.SelectedValue != "-1" ? ddlActivityType.SelectedValue : null;
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.IsRemoved = "0";
                _AgentRegistrationDAL.IsActive = "0";
                _AgentRegistrationDAL.IsdocUploaded = "1";
                _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                ds = _AgentRegistrationDAL.GetAgentDetailsToProcessOnboaring();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvAgent.Visible = true;
                    gvAgent.DataSource = ds.Tables[0];
                    gvAgent.DataBind();
                    btnApprove.Visible = true;
                    btnDecline.Visible = true;
                    //btnReprocess.Visible = true;
                    panelGrid.Visible = true;
                    ViewState["AgentDataSet"] = ds.Tables[0];
                    lblRecordsTotal.Text = "Total " + Convert.ToString(ds.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    gvAgent.Visible = false;
                    btnApprove.Visible = false;
                    btnDecline.Visible = false;
                    //btnReprocess.Visible = false;
                    panelGrid.Visible = false;
                    panelGrid.Visible = false;
                    lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                }
                ErrorLog.AgentManagementTrace("AgVerification | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: FillGrid: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return ds;
        }

        public void CommonSave(string fromWhere)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | CommonSave() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region  Alert and Log Messages
                if (!String.IsNullOrEmpty(ViewState["ActionType"].ToString()))
                {
                    if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                    {
                        _strAlertMessage_Header = "Decline Agent ";
                        _strAlertMessage_Success = "Decline Process ";
                        _strAlertMessage_UnSuccess = "Decline Process ";
                        _strAlertMessage_Total = "Total Record Processed for Decline Agent(s) :  ";
                        _AgentRegistrationDAL.AtStatus = Convert.ToString((int)(EnumCollection.Onboarding.AuthorizerDecline));
                        _Flag = (int)EnumCollection.EnumDBOperationType.AuthDecline;
                    }
                    else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Approve.ToString())
                    {
                        _strAlertMessage_Header = "Approve Agent ";
                        _strAlertMessage_Success = "Approve Process ";
                        _strAlertMessage_UnSuccess = "Approve Process ";
                        _strAlertMessage_Total = "Total Record Processed for Approve Agent(s) :  ";
                        _AgentRegistrationDAL.AtStatus = Convert.ToString((int)(EnumCollection.Onboarding.AuthorizerApprove));
                        _Flag = (int)EnumCollection.EnumDBOperationType.AuthApprove;
                    }
                }
                #endregion
                switch (fromWhere)
                {
                    case "CheckedAll":
                        #region All Rows of GridView.
                        DataTable AgentDataSet = (DataTable)ViewState["AgentDataSet"];
                        if ((AgentDataSet != null) && Convert.ToString(ViewState["SelectionType"]) == SelectionType.CheckedAll.ToString())
                        {
                            _fileLineNo = "0";
                            for (int i = 0; i < AgentDataSet.Rows.Count; i++)
                            {
                                try
                                {

                                    int _ActivityType = 0;

                                    switch (AgentDataSet.Rows[i]["ActivityType"])
                                    {
                                        case "Onboard":
                                            _ActivityType = (int)EnumCollection.ActivityType.Onboard;
                                            break;
                                        case "Activate":
                                            _ActivityType = (int)EnumCollection.ActivityType.Activate;
                                            break;
                                        case "Deactivate":
                                            _ActivityType = (int)EnumCollection.ActivityType.Deactivate;
                                            break;
                                        case "Terminate":
                                            _ActivityType = (int)EnumCollection.ActivityType.Terminate;
                                            break;
                                        case "ReEdit":
                                            _ActivityType = (int)EnumCollection.ActivityType.ReEdit;
                                            break;
                                            //default  = 4
                                    }

                                    string AgentCode_ = null;
                                    if (!string.IsNullOrEmpty(Convert.ToString(AgentDataSet.Rows[i]["AgentCode"])))
                                    {
                                        AgentCode_ = Convert.ToString(AgentDataSet.Rows[i]["AgentCode"]);
                                    }
                                    else
                                    {
                                        AgentCode_ = string.Empty;
                                    }

                                    _reocrdsProcessed = _reocrdsProcessed + 1;

                                    _AgentRegistrationDAL.Flag = (Convert.ToInt32(_Flag));
                                    _AgentRegistrationDAL.ActionType = (Convert.ToInt32(_Flag)).ToString();
                                    _AgentRegistrationDAL.ActivityType = _ActivityType;         
                                    //_AgentEntity.ActivityType = Convert.ToString((int)EnumCollection.ActivityType.Onboard);
                                    _AgentRegistrationDAL.AgentReqId = Convert.ToString(AgentDataSet.Rows[i]["AgentID"]);
                                    _AgentRegistrationDAL.ClientId = Convert.ToString(AgentDataSet.Rows[i]["ClientID"]);
                                    _AgentRegistrationDAL.PersonalContact = Convert.ToString(AgentDataSet.Rows[i]["MobileNo"]);
                                    _AgentRegistrationDAL.BusinessEmail = Convert.ToString(AgentDataSet.Rows[i]["Email"]);
                                   

                                    _AgentRegistrationDAL.ATRemark = TxtRemarks.Text.Trim();
                                    _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                                    //_dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                                    #region Audit
                                    _auditParams[0] = Session["Username"].ToString();
                                    _auditParams[1] = "Agent-Verification";
                                    _auditParams[2] = _strAlertMessage_Header;
                                    _auditParams[3] = Session["LoginKey"].ToString();
                                    _LoginEntity.StoreLoginActivities(_auditParams);
                                    #endregion
                                    if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                                    {
                                        _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                                        // ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Request Declined Successfully', 'Agent Verification');", true);
                                        ErrorLog.AgentManagementTrace("AgentVerification: Commonsave:CheckAll(): Successful - Maker Verification. StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                    }
                                    else
                                    {
                                        if (_ActivityType == 0)
                                        {
                                            // DivAgentDetails.Visible = true;
                                            _salt = _AppSecurity.RandomStringGenerator();
                                            _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();//Session["Username"].ToString();
                                            _AgentRegistrationDAL.Flag = 1;
                                            _AgentRegistrationDAL._RandomStringForSalt = null;
                                            _AgentRegistrationDAL._RandomStringForSalt = _AppSecurity.RandomStringGenerator();
                                            string _tempPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_AgentRegistrationDAL._RandomStringForSalt + _DefaultPassword);
                                            _AgentRegistrationDAL.Password = _tempPassword;
                                            _AgentRegistrationDAL._RandomStringForSalt = _salt;
                                            _AgentRegistrationDAL.Password = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_salt + _DefaultPassword);
                                            _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Onboard); ///category wise mail
                                            DataSet dsAgentMaster = _AgentRegistrationDAL.SetInsertUpdateAgentMasterDetails();
                                            string status = dsAgentMaster.Tables[0].Rows[0]["Status"].ToString();

                                            if (dsAgentMaster != null && dsAgentMaster.Tables.Count > 0 && status == "Inserted")
                                            {
                                                AgentCode_ = dsAgentMaster.Tables[0].Rows[0]["agentcode"].ToString();
                                                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                                                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Registered Successfully', 'Agent Registration');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification: Commonsave:CheckAll(): Successful - Verification. StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);

                                                //#region SMS
                                                //_EmailSMSAlertscs.FROM = Session["Username"].ToString();
                                                //_EmailSMSAlertscs.to = _AgentRegistrationDAL.PersonalContact;
                                                //_EmailSMSAlertscs.tempname = "SR24659_BCP1";
                                                //_EmailSMSAlertscs.OTPFlag = "0";
                                                //_EmailSMSAlertscs.var1 = "SBM";
                                                //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                                //ErrorLog.AgentManagementTrace("Page : AgentVerification.cs \nFunction : CheckedAll() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                                                //ErrorLog.SMSTrace("Page : AgentVerification.cs \nFunction : CheckedAll() => Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.PersonalContact);
                                                //_EmailSMSAlertscs.HttpGetRequest();
                                                //#endregion

                                                //#region EMAIL
                                                //_EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                                                //_EmailSMSAlertscs.to = _AgentRegistrationDAL.BusinessEmail;
                                                //_EmailSMSAlertscs.tempname = "SR24659_EBCP4";
                                                //_EmailSMSAlertscs.OTPFlag = "0";
                                                //_EmailSMSAlertscs.var1 = "SBM";
                                                //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                                //ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : CheckedAll() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                                                //ErrorLog.SMSTrace("Page : AgentRegistration.cs \nFunction : CheckedAll() => Reistration Details forwarded for Email Preparation. Email : " + _AgentRegistrationDAL.BusinessEmail);
                                                //_EmailSMSAlertscs.HttpGetRequestEmail();
                                                //#endregion
                                            }
                                            else
                                            {
                                                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Registration Unscuccessful : '" + status + " ');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification: Commonsave:CheckAll(): Failed - Verification. Agent Already Terminated ");
                                                ErrorLog.AgentManagementTrace("AgentVerification: Commonsave:CheckAll(): Successful - Verification. StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                            }
                                        }

                                        else if (_ActivityType == 1)
                                        {
                                            _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.Activate);
                                            _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Active); ///category wise mail
                                            _AgentRegistrationDAL.AgentCode = AgentCode_;
                                            _dsActivate = _AgentRegistrationDAL.ActiveDeactiveAgent();
                                            if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                                            {
                                                _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.AuthApprove);
                                                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                                                // ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approved Successfully', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Successful - Verification.   StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                            }
                                            else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                                            {
                                                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent Already Terminated', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Verification. Agent Already Terminated ");
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Verification.   StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                            }
                                            else
                                            {
                                                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Approve Request Unsuccessful', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Approve Request Unsuccessful.   StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                            }
                                        }

                                        else if (_ActivityType == 2)
                                        {
                                            _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.Deactivate);
                                            _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Deactivate); ///category wise mail
                                            _AgentRegistrationDAL.AgentCode = AgentCode_;
                                            _dsActivate = _AgentRegistrationDAL.ActiveDeactiveAgent();
                                            if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                                            {
                                                _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.AuthApprove);
                                                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                                                // ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approved Successfully', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Successful - Verification.    StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                            }
                                            else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                                            {
                                                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent Already Terminated', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Verification. Agent Already Terminated ");
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Verification.    StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);

                                            }
                                            else
                                            {
                                                // ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Approve Request Unsuccessful', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Approve Request Unsuccessful.   StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);

                                            }
                                        }

                                        else if (_ActivityType == 3)
                                        {
                                            _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.OnboardTerminate);
                                            _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Terminated); ///category wise mail
                                            _AgentRegistrationDAL.AgentCode = AgentCode_;
                                            _dsActivate = _AgentRegistrationDAL.ActiveDeactiveAgent();
                                            if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                                            {
                                                _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.AuthApprove);
                                                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approved Successfully', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                            }
                                            else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                                            {
                                                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent Already Terminated', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Verification. Agent Already Terminated ");
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);

                                            }
                                            else
                                            {
                                                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Approve Request Unsuccessful', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Approve Request Unsuccessful.   StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);

                                            }
                                        }

                                        else if (_ActivityType == 4)
                                        {
                                            _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.Reprocess);
                                            _dsActivate = _AgentRegistrationDAL.ChangeAgentStatusReEdit();
                                            if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                                            {
                                                _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.AuthApprove);
                                                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approved Successfully', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification: Commonsave:CheckAll() : Successful - Verification.   StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                            }
                                            else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                                            {
                                                // ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent Already Terminated', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Verification. Agent Already Terminated ");
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Verification.   StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);

                                            }
                                            else
                                            {
                                                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Approve Request Unsuccessful', 'Agent Verification');", true);
                                                ErrorLog.AgentManagementTrace("AgentVerification:  Commonsave:CheckAll() : Failed - Approve Request Unsuccessful.   StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                            }
                                        }
                                    }

                                    if (Convert.ToInt32(Convert.ToString(_dsVerification.Tables[0].Rows[0][0])) > 0)
                                    {
                                        _successful = _successful + 1;
                                    }
                                    else
                                    {
                                        _unsuessful = _unsuessful + 1;
                                    }
                                    if (_Flag == 6)
                                    {
                                        #region SMS
                                        _EmailSMSAlertscs.FROM = Session["Username"].ToString();
                                        _EmailSMSAlertscs.to = _AgentRegistrationDAL.PersonalContact;
                                        _EmailSMSAlertscs.tempname = "SR24659_BCP2";
                                        _EmailSMSAlertscs.OTPFlag = "0";
                                        _EmailSMSAlertscs.var1 = "SBM";
                                        _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                        ErrorLog.AgentManagementTrace("Page : AgentVerification.cs \nFunction : CheckedAll() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                                        ErrorLog.SMSTrace("Page : AgentVerification.cs \nFunction : CheckedAll() => Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.PersonalContact);
                                        _EmailSMSAlertscs.HttpGetRequest();
                                        #endregion

                                        #region EMAIL
                                        _EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                                        _EmailSMSAlertscs.to = _AgentRegistrationDAL.BusinessEmail;
                                        _EmailSMSAlertscs.tempname = "SR24659_EBCP3";
                                        _EmailSMSAlertscs.OTPFlag = "0";
                                        _EmailSMSAlertscs.var1 = "SBM";
                                        _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                        ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : CheckedAll() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                                        ErrorLog.SMSTrace("Page : AgentRegistration.cs \nFunction : CheckedAll() => Reistration Details forwarded for Email Preparation. Email : " + _AgentRegistrationDAL.BusinessEmail);
                                        _EmailSMSAlertscs.HttpGetRequestEmail();
                                        #endregion
                                    }
                                }
                                catch (Exception Ex)
                                {
                                    _unsuessful = _unsuessful + 1;
                                }
                            }
                            TxtRemarks.Text = string.Empty;
                            ViewState["RecordCountDecine"] = _reocrdsProcessed;
                        }
                        #endregion
                        break;
                    case "MultiCheck":
                        #region Multiple Section Row of GridView.
                        // else
                        {
                            _fileLineNo = "0";
                            foreach (GridViewRow row in gvAgent.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                                    if (chBoxRow.Checked)
                                    {
                                        try
                                        {
                                            int val = 0;
                                            switch (row.Cells[52].Text)
                                            {
                                                case "Onboard":
                                                    val = (int)EnumCollection.ActivityType.Onboard;
                                                    break;
                                                case "Activate":
                                                    val = (int)EnumCollection.ActivityType.Activate;
                                                    break;
                                                case "Deactivate":
                                                    val = (int)EnumCollection.ActivityType.Deactivate;
                                                    break;
                                                case "Terminate":
                                                    val = (int)EnumCollection.ActivityType.Terminate;
                                                    break;
                                                case "ReEdit":
                                                    val = (int)EnumCollection.ActivityType.ReEdit;
                                                    break;
                                                    //default  = 4
                                            }
                                            string AgentCode_ = null;
                                            if (!string.IsNullOrEmpty(row.Cells[23].Text))
                                            {
                                                AgentCode_ = row.Cells[23].Text;
                                            }
                                            else
                                            {
                                                AgentCode_ = string.Empty;
                                            }


                                            _reocrdsProcessed = _reocrdsProcessed + 1;
                                            SingleSave(_reocrdsProcessed, _Flag, _Flag, row.Cells[3].Text, row.Cells[22].Text, Convert.ToString(Session["Username"]), TxtRemarks.Text.Trim(), row.Cells[40].Text, row.Cells[41].Text, row.Cells[51].Text, val, row.Cells[23].Text);
                                            //SingleSave(_reocrdsProcessed, _Flag, _Flag, row.Cells[2].Text, row.Cells[5].Text, Convert.ToString(Session["Username"]), TxtRemarks.Text.Trim(), row.Cells[9].Text, row.Cells[8].Text, row.Cells[6].Text, val, AgentCode_);
                                        }
                                        catch (Exception Ex)
                                        {
                                            _unsuessful = _unsuessful + 1;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                    case "RadionButtonClick":

                        #region Action Click of Gridview for single insert
                        try
                        {
                            ErrorLog.AgentManagementTrace("AgentVerification: CommonSave:  Request : " + _AgentRegistrationDAL.Mstatus + " Received. Username : " + UserName + " RequestId : " + Convert.ToString(ViewState["AgentReqId"]));
                            int _ActivityType = 0;

                            switch (Convert.ToString(ViewState["Activity Type"]))
                            {
                                case "Onboard":
                                    _ActivityType = (int)EnumCollection.ActivityType.Onboard;
                                    break;
                                case "Activate":
                                    _ActivityType = (int)EnumCollection.ActivityType.Activate;
                                    break;
                                case "Deactivate":
                                    _ActivityType = (int)EnumCollection.ActivityType.Deactivate;
                                    break;
                                case "Terminate":
                                    _ActivityType = (int)EnumCollection.ActivityType.Terminate;
                                    break;
                                case "ReEdit":
                                    _ActivityType = (int)EnumCollection.ActivityType.ReEdit;
                                    break;
                                    //default  = 4
                            }
                            _reocrdsProcessed = 1;
                            SingleSave(_reocrdsProcessed, _Flag, _Flag, Convert.ToString(ViewState["AgentReqId"]), txtAgentName.Text, Convert.ToString(Session["Username"]), txtFinalRemarks.Text.Trim(), ViewState["ReceiverEmailID"].ToString(), ViewState["ReceiverContactNo"].ToString(), Convert.ToString(Session["Client"]), _ActivityType, Convert.ToString(ViewState["Agent Code"]));
                        }
                        catch (Exception Ex)
                        {
                            _unsuessful = _unsuessful + 1;
                        }
                        #endregion
                        break;
                }
                ErrorLog.AgentManagementTrace("AgVerification | CommonSave() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: CommonSave: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        public void SingleSave(int _reocrdsProcessed, int _StatusFlag, int _Flag, string AgentID, string _strAgentFullName, string User, string Remarks, string ReceiverEmailID, string ContactNo, string _ClientCode, int _ActivityType, string _AgentCode)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | SingleSave() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Received - Maker Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                _AgentRegistrationDAL.ActivityType = _ActivityType;
                _AgentRegistrationDAL.AgentCode = _AgentCode;
                _reocrdsProcessed = _reocrdsProcessed + 1;
                //_AgentEntity.ATStatus = Convert.ToInt16(_Flag);
                _AgentRegistrationDAL.Flag = (Convert.ToInt32(_Flag));
                _AgentRegistrationDAL.ActionType = (Convert.ToInt16(_Flag)).ToString();
                //_AgentEntity.ActivityType = EnumCollection.ActivityType. 
                _AgentRegistrationDAL.AgentReqId = AgentID;
                _AgentRegistrationDAL.ATRemark = Remarks;
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.ClientId = _ClientCode;
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Verification";
                _auditParams[2] = _strAlertMessage_Header;
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                {
                    _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Request Declined Successfully', 'Agent Verification');", true);
                    ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Successful - Maker Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                    //#region SMS
                    //_EmailSMSAlertscs.FROM = Session["Username"].ToString();
                    //_EmailSMSAlertscs.to = ContactNo;
                    //_EmailSMSAlertscs.tempname = "SR24659_BCP2";
                    //_EmailSMSAlertscs.OTPFlag = "0";
                    //_EmailSMSAlertscs.var1 = "SBM";
                    //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                    //ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : Registration() => Reistration Details forwarded for SMS Preparation. => HttpGetRequest()");
                    //_EmailSMSAlertscs.HttpGetRequest();
                    //#endregion

                    //#region EMAIL
                    //_EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                    //_EmailSMSAlertscs.to = ReceiverEmailID;
                    //_EmailSMSAlertscs.tempname = "SR24659_EBCP3";
                    //_EmailSMSAlertscs.OTPFlag = "0";
                    //_EmailSMSAlertscs.var1 = "SBM";
                    //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                    //ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : CheckedAll() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                    //ErrorLog.SMSTrace("Page : AgentRegistration.cs \nFunction : CheckedAll() => Reistration Details forwarded for Email Preparation. Email : " + ReceiverEmailID);
                    //_EmailSMSAlertscs.HttpGetRequestEmail();
                    //#endregion
                }
                else
                {
                    if (_ActivityType == 0)
                    {
                        // DivAgentDetails.Visible = true;
                        _salt = _AppSecurity.RandomStringGenerator();
                        _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();//Session["Username"].ToString();
                        _AgentRegistrationDAL.AgentReqId = AgentID;
                        _AgentRegistrationDAL.Flag = 1;
                        _AgentRegistrationDAL._RandomStringForSalt = null;
                        _AgentRegistrationDAL._RandomStringForSalt = _AppSecurity.RandomStringGenerator();
                        string _tempPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_AgentRegistrationDAL._RandomStringForSalt + _DefaultPassword);
                        _AgentRegistrationDAL.Password = _tempPassword;
                        _AgentRegistrationDAL._RandomStringForSalt = _salt;
                        _AgentRegistrationDAL.Password = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_salt + _DefaultPassword);
                        _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Onboard); ///category wise mail
                        DataSet dsAgentMaster = _AgentRegistrationDAL.SetInsertUpdateAgentMasterDetails();
                        string status = dsAgentMaster.Tables[0].Rows[0]["Status"].ToString();

                        if (dsAgentMaster != null && dsAgentMaster.Tables.Count > 0 && status == "Inserted")
                        {
                            _AgentCode = dsAgentMaster.Tables[0].Rows[0]["agentcode"].ToString();
                            _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();

                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Registered Successfully', 'Agent Registration');", true);
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);

                            //#region SMS
                            //_EmailSMSAlertscs.FROM = Session["Username"].ToString();
                            //_EmailSMSAlertscs.to = ContactNo;// _AgentRegistrationDAL.PersonalContact;
                            //_EmailSMSAlertscs.tempname = "SR24659_BCP1";
                            //_EmailSMSAlertscs.OTPFlag = "0";
                            //_EmailSMSAlertscs.var1 = "SBM";
                            //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                            //ErrorLog.AgentManagementTrace("Page : AgentVerification.cs \nFunction : SingleSave() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                            //ErrorLog.SMSTrace("Page : AgentVerification.cs \nFunction : SingleSave() => Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.PersonalContact);
                            //_EmailSMSAlertscs.HttpGetRequest();
                            //#endregion
                             
                            //#region EMAIL
                            //_EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                            //_EmailSMSAlertscs.to = ReceiverEmailID;// _AgentRegistrationDAL.PersonalEmailID;
                            //_EmailSMSAlertscs.tempname = "SR24659_EBCP4";
                            //_EmailSMSAlertscs.OTPFlag = "0";
                            //_EmailSMSAlertscs.var1 = "SBM";
                            //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                            //ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : SingleSave() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                            //ErrorLog.SMSTrace("Page : AgentRegistration.cs \nFunction : SingleSave() => Reistration Details forwarded for Email Preparation. Email : " + _AgentRegistrationDAL.PersonalEmailID);
                            //_EmailSMSAlertscs.HttpGetRequestEmail();
                            //#endregion
                        }
                        else                                      
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Registration Unscuccessful : '" + status + " ');", true);
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                            return;
                        }
                    }

                    else if (_ActivityType == 1)
                    {
                        _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.Activate);
                        _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Active); ///category wise mail
                        _AgentRegistrationDAL.AgentCode = _AgentCode;
                        _dsActivate = _AgentRegistrationDAL.ActiveDeactiveAgent();
                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.AuthApprove);
                            _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approved Successfully', 'Agent Verification');", true);
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                        }
                        else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent Already Terminated', 'Agent Verification');", true);
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Approve Request Unsuccessful', 'Agent Verification');", true);
                            return;
                        }
                    }

                    else if (_ActivityType == 2)
                    {
                        _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.Deactivate);
                        _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Deactivate); ///category wise mail
                        _AgentRegistrationDAL.AgentCode = _AgentCode;
                        _dsActivate = _AgentRegistrationDAL.ActiveDeactiveAgent();
                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.AuthApprove);
                            _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approved Successfully', 'Agent Verification');", true);
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                        }
                        else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent Already Terminated', 'Agent Verification');", true);
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Failed - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Approve Request Unsuccessful', 'Agent Verification');", true);
                            return;
                        }
                    }

                    else if (_ActivityType == 3)
                    {
                        _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.OnboardTerminate);
                        _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Terminated); ///category wise mail
                        _AgentRegistrationDAL.AgentCode = _AgentCode;
                        _dsActivate = _AgentRegistrationDAL.ActiveDeactiveAgent();
                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.AuthApprove);
                            _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approved Successfully', 'Agent Verification');", true);
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                        }
                        else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent Already Terminated', 'Agent Verification');", true);
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Failed - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Approve Request Unsuccessful', 'Agent Verification');", true);
                            return;
                        }
                    }

                    else if (_ActivityType == 4)
                    {
                        _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.Reprocess);
                        _dsActivate = _AgentRegistrationDAL.ChangeAgentStatusReEdit();
                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            _AgentRegistrationDAL.ActionType = Convert.ToString((int)EnumCollection.EnumDBOperationType.AuthApprove);
                            _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approved Successfully', 'Agent Verification');", true);
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                        }
                        else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent Already Terminated', 'Agent Verification');", true);
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Failed - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + AgentID + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Approve Request Unsuccessful', 'Agent Verification');", true);
                            return;
                        }
                    }
                }

                if (_dsVerification != null && _dsVerification.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(Convert.ToString(_dsVerification.Tables[0].Rows[0][0])) > 0)
                    {
                        string AgentReqId = string.Empty;
                        string _ClientID = string.Empty;
                        AgentReqId = _AgentRegistrationDAL.AgentReqId;
                        _ClientID = _AgentRegistrationDAL.ClientId;
                        //if (_Flag == 5)  //Approve
                        //{
                        //    _EmailAlerts.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.EmailAlert);
                        //    _EmailAlerts.SubCategoryTypeId = null;
                        //    _EmailAlerts.ClientID = _ClientCode;
                        //    _EmailAlerts.UserName = Session["Username"].ToString();
                        //    _EmailAlerts.UserID = _AgentCode;
                        //    _EmailAlerts.Flag = "3";
                        //    ErrorLog.BCManagementTrace("Page : AgentVerification.cs \nFunction : SingleSave() => active Agent  Success. Details forwarded for Email Preparation. => PrepareEmailFormat()");
                        //    _EmailAlerts.PrepareEmailFormat();

                        //    //_EmailAlerts.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.SMSAlert);
                        //    //_EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Active);
                        //    //_EmailAlerts.SubCategoryTypeId = null;
                        //    //_EmailAlerts.ClientID = _ClientID;
                        //    //_EmailAlerts.UserName = Session["Username"].ToString();
                        //    //_EmailAlerts.UserID = AgentReqId;
                        //    //_EmailAlerts.Flag = "3";
                        //    //ErrorLog.BCManagementTrace("Page : AgentVerification.cs \nFunction : SingleSave() => active Agent Success. Details forwarded for SMS Preparation. => PrepareSMSFormat()");
                        //    //_EmailAlerts.PrepareSMSFormat();
                        //}
                        //if (_Flag == 6) //Decline
                        //{
                        //    _EmailAlerts.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.EmailAlert);
                        //    _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Decline);
                        //    _EmailAlerts.SubCategoryTypeId = null;
                        //    _EmailAlerts.ClientID = _ClientCode;
                        //    _EmailAlerts.UserName = Session["Username"].ToString();
                        //    _EmailAlerts.UserID = _AgentCode;
                        //    _EmailAlerts.Flag = "3"; //1
                        //    ErrorLog.BCManagementTrace("Page : BCVerificationLevelTwo.cs \nFunction : BCVerificationLevelTwo() => decline BC  Success. Details forwarded for Email Preparation. => PrepareEmailFormat()");
                        //    _EmailAlerts.PrepareEmailFormat();

                        //    //_EmailAlerts.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.SMSAlert);
                        //    //_EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Decline);
                        //    //_EmailAlerts.SubCategoryTypeId = null;
                        //    //_EmailAlerts.ClientID = _ClientID;
                        //    //_EmailAlerts.UserName = Session["Username"].ToString();
                        //    //_EmailAlerts.UserID = AgentReqId;
                        //    //_EmailAlerts.Flag = "1";
                        //    //ErrorLog.BCManagementTrace("Page : BCVerificationLevelTwo.cs \nFunction : BCVerificationLevelTwo() => decline BC  Success. Details forwarded for SMS Preparation. => PrepareSMSFormat()");
                        //    //_EmailAlerts.PrepareSMSFormat();
                        //}
                        _successful = _successful + 1;
                    }
                    else
                    {
                        _unsuessful = _unsuessful + 1;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Approve/Decline Request Unsuccessful', 'Agent Verification');", true);
                    return;
                }
                ErrorLog.AgentManagementTrace("AgVerification | SingleSave() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: SingleSave: Failed - Maker Verification. Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        #endregion

        #region Search And Clear
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | btnSearch_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Verification";
                _auditParams[2] = "btnSearch";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                ErrorLog.AgentManagementTrace("AgVerification | btnSearch_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: btnSearch_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | btnClear_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Verification";
                _auditParams[2] = "btnClear";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                ddlBCCode.SelectedValue = "0";
                ddlAgentCode.SelectedValue = "0";
                ddlActivityType.SelectedValue = "-1";
                ddlFileID.ClearSelection();

                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                ErrorLog.AgentManagementTrace("AgVerification | btnClear_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: btnClear_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Approve
        protected void btnApprove_ServerClick(object sender, EventArgs e)
        {
            ViewState["ActionType"] = EnumCollection.EnumDBOperationType.Approve.ToString();
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | btnApprove_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Verification";
                _auditParams[2] = "btnApprove";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                foreach (GridViewRow row in gvAgent.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chBoxRow = (CheckBox)row.FindControl("chBoxSelectRow");
                        if (chBoxRow.Checked)
                        {
                            _DeclineCardCount++;
                        }
                    }
                }
                if (_DeclineCardCount > 0)
                {
                    lblModalHeaderName.Text = "Agent(s) Approve Reason";
                    TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                    lblconfirm.Text = "Are you sure want to Approve Agent(s)?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Approve Agent(s)');", true);
                    return;
                }
                ErrorLog.AgentManagementTrace("AgVerification | btnApprove_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: btnApprove_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Decline
        protected void btnDecline_ServerClick(object sender, EventArgs e)
        {
            ViewState["ActionType"] = EnumCollection.EnumDBOperationType.Decline.ToString();
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | btnDecline_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Verification";
                _auditParams[2] = "btnDecline";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                foreach (GridViewRow row in gvAgent.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chBoxRow = (CheckBox)row.FindControl("chBoxSelectRow");
                        if (chBoxRow.Checked)
                        {
                            _DeclineCardCount++;
                        }
                    }
                }
                if (_DeclineCardCount > 0)
                {
                    lblModalHeaderName.Text = "Agent(s) Decline Reason";
                    TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                    lblconfirm.Text = "Are you sure want to Decline Agent(s)?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Decline Agent(s)');", true);
                    return;
                }
                ErrorLog.AgentManagementTrace("AgVerification | btnDecline_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: btnDecline_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Approve \ Decline \ Terminate Event Handler
        protected void btnSaveAction_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | btnSaveAction_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Verification";
                _auditParams[2] = "btnSaveAction";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                if (TxtRemarks.Text == null || TxtRemarks.Text == "")
                {
                    ModalPopupExtender_Declincard.Show();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter Remarks', 'Agent Verification');", true);
                    return;
                }
                else
                {
                    Button btn = (Button)sender;
                    _successful = 0;
                    _unsuessful = 0;

                    if (Convert.ToString(ViewState["SelectionType"]) == SelectionType.CheckedAll.ToString())
                        CommonSave("CheckedAll");
                    else
                        CommonSave("MultiCheck");

                    FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                    BindDropdownsAgent();
                    TxtRemarks.Text = string.Empty;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _strAlertMessage_Total + _reocrdsProcessed + "  Successful : " + _successful + "  Unsuccessful : " + _unsuessful + " ');", true);
                    ViewState["ActionType"] = null;
                    ViewState["SelectionType"] = null;
                    return;
                }
                //ErrorLog.AgentManagementTrace("AgVerification | btnSaveAction_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: btnSaveAction_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                ViewState["ActionType"] = null;
                ViewState["SelectionType"] = null;
                return;
            }
        }
        #endregion

        #region Cancel Action
        protected void btnCancelAction_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | btnCancelAction_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Verification";
                _auditParams[2] = "btnSaveAction";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                string _alertMessage = string.Empty;
                if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Approve.ToString())
                {
                    _alertMessage = "Agent Approve";
                }
                else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                {
                    _alertMessage = "Agent Decline";
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Operation has cancelled.','" + _alertMessage + "');", true);
                ModalPopupExtender_Declincard.Hide();
                TxtRemarks.Text = string.Empty;
                ViewState["ActionType"] = null;
                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                ErrorLog.AgentManagementTrace("AgVerification | btnCancelAction_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                return;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: btnCancelAction_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                ViewState["ActionType"] = null;
                return;
            }
        }
        #endregion

        #region Grid Events
        protected void gvAgent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAgent.PageIndex = e.NewPageIndex;
                if (ViewState["AgentDataSet"] != null)
                {
                    DataTable dsTemp = (DataTable)ViewState["AgentDataSet"];
                    if (dsTemp != null && dsTemp.Rows.Count > 0)
                    {
                        gvAgent.Visible = true;
                        gvAgent.DataSource = dsTemp;
                        gvAgent.DataBind();

                        btnApprove.Visible = true;
                        btnDecline.Visible = true;
                        //btnTerminate.Visible = true;
                        lblRecordsTotal.Text = "Total " + Convert.ToString(dsTemp.Rows.Count) + " Record(s) Found.";
                    }
                    else
                    {
                        gvAgent.Visible = false;
                        btnApprove.Visible = false;
                        btnDecline.Visible = false;
                        //btnTerminate.Visible = false;
                        lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                    }
                }
                else
                    FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                CheckBoxAllOperationOnPageIndex();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: gvAgent_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Check Box Events
        protected void chBoxSelectRow_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectionType"] != null)
                {
                    if (ViewState["SelectionType"].ToString() == SelectionType.CheckedAll.ToString())
                    {
                        CheckBox CheckBoxAll = gvAgent.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvAgent.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                                if (!chBoxRow.Checked)
                                {
                                    ViewState["SelectionType"] = null;
                                    CheckBoxAll.Checked = false;
                                    break;
                                }
                            }
                        }
                    }
                    else // New Block
                    {
                        int rowCount = 0, rowCountSelected = 0;
                        CheckBox _CheckBoxAll = gvAgent.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvAgent.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                rowCount += 1;
                                CheckBox _chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                                if (!_chBoxRow.Checked)
                                {
                                    break;
                                }
                                else
                                {
                                    rowCountSelected += 1;
                                }
                            }
                        }
                        _CheckBoxAll.Checked = rowCount == rowCountSelected ? true : false;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: chBoxSelectRow_CheckedChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox CheckBoxAll = gvAgent.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                foreach (GridViewRow row in gvAgent.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (CheckBoxAll.Checked)
                        {
                            CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                            chBoxRow.Checked = true;
                        }
                        else
                        {
                            CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                            chBoxRow.Checked = false;
                        }
                    }
                }
                if (CheckBoxAll.Checked)
                {
                    ViewState["SelectionType"] = SelectionType.CheckedAll.ToString();
                }
                else
                {
                    ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: CheckBoxAll_CheckedChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void CheckBoxAllOperationOnPageIndex()
        {
            try
            {
                if (ViewState["SelectionType"] != null)
                {
                    if (ViewState["SelectionType"].ToString() == SelectionType.CheckedAll.ToString())
                    {
                        CheckBox CheckBoxAll = gvAgent.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = true;
                        foreach (GridViewRow row in gvAgent.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                                chBoxRow.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        CheckBox CheckBoxAll = gvAgent.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = false;
                        foreach (GridViewRow row in gvAgent.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                                chBoxRow.Checked = false;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: CheckBoxAllOperationOnPageIndex: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Action View
        protected void btnView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;
                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { '=' });
                //_AgentEntity.AgentReqId = ViewState["AgentReqId"].ToString();
                string AgentReqId = commandArgs[0];
                _AgentRegistrationDAL.AgentReqId = AgentReqId;
                ViewState["AgentReqId"] = AgentReqId;
                string ReceiverEmailID = Convert.ToString(commandArgs[1]);
                string Activity_Type = Convert.ToString(commandArgs[4]);
                ViewState["Activity Type"] = Activity_Type;
                _AgentRegistrationDAL.Mode = "GetAgentDetails";
                //_AgentEntity.AgentAgentID = AgentReqId;
                //ViewState["AgentReqId"] = AgentReqId;
                ViewState["ReceiverEmailID"] = ReceiverEmailID;
                string contactNo = Convert.ToString(commandArgs[2]);
                ViewState["ReceiverContactNo"] = contactNo;
                string AgentCode = Convert.ToString(commandArgs[5]);
                ViewState["Agent Code"] = AgentCode;

                DataSet ds = _AgentRegistrationDAL.GetAgentDocuments();

                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                DataSet get = _AgentRegistrationDAL.GetAgentDocuments();
                string defaultimage = "images/Question.png";
                string idthumb = !string.IsNullOrEmpty(get.Tables[0].Rows[0]["IdentityProofDocument"].ToString()) ? get.Tables[0].Rows[0]["IdentityProofDocument"].ToString() : defaultimage.ToString();
                if (!string.IsNullOrEmpty(get.Tables[0].Rows[0]["IdentityProofDocument"].ToString()))
                {
                    string FileThumbnailId = Path.GetDirectoryName(idthumb) + "\\" + Path.GetFileNameWithoutExtension(idthumb) + "_Thumbnail.png";
                    string filepath = AppDomain.CurrentDomain.BaseDirectory + FileThumbnailId;
                    //pathId = "../../" + FileThumbnailId;
                    if (File.Exists(filepath))
                    {
                        pathId = "../../" + FileThumbnailId;
                        Session["pdfPathID"] = idthumb;
                    }
                    else
                    {
                        pathId = "../../" + defaultimage;
                        Session["pdfPathID"] = defaultimage;
                    }
                }
                else
                {
                    pathId = "../../" + idthumb;
                    Session["pdfPathID"] = idthumb;
                }

                string Addthumb = !string.IsNullOrEmpty(get.Tables[1].Rows[0]["AddressProofDocument"].ToString()) ? get.Tables[1].Rows[0]["AddressProofDocument"].ToString() : defaultimage.ToString();
                if (!string.IsNullOrEmpty(get.Tables[1].Rows[0]["AddressProofDocument"].ToString()))
                {
                    string FileThumbnailAdd = Path.GetDirectoryName(Addthumb) + "\\" + Path.GetFileNameWithoutExtension(Addthumb) + "_Thumbnail.png";
                    string filepath = AppDomain.CurrentDomain.BaseDirectory + FileThumbnailAdd;
                    if (File.Exists(filepath))
                    {
                        PathAdd = "../../" + FileThumbnailAdd;
                        Session["pdfPathAdd"] = Addthumb;
                    }
                    else
                    {
                        PathAdd = "../../" + defaultimage;
                        Session["pdfPathAdd"] = defaultimage;
                    }
                }
                else
                {
                    PathAdd = "../../" + Addthumb;
                    Session["pdfPathAdd"] = Addthumb;
                }

                string Sigthumb = !string.IsNullOrEmpty(get.Tables[2].Rows[0]["SignatureProofDocument"].ToString()) ? get.Tables[2].Rows[0]["SignatureProofDocument"].ToString() : defaultimage.ToString();
                if (!string.IsNullOrEmpty(get.Tables[2].Rows[0]["SignatureProofDocument"].ToString()))
                {
                    string FileThumbnailSig = Path.GetDirectoryName(Sigthumb) + "\\" + Path.GetFileNameWithoutExtension(Sigthumb) + "_Thumbnail.png";
                    string filepath = AppDomain.CurrentDomain.BaseDirectory + FileThumbnailSig;
                    if (File.Exists(filepath))
                    {
                        PathSig = "../../" + FileThumbnailSig;
                        Session["pdfPathSig"] = Sigthumb;
                    }
                    else
                    {
                        PathSig = "../../" + defaultimage;
                        Session["pdfPathSig"] = defaultimage;
                    }
                }
                else
                {
                    PathSig = "../../" + Sigthumb;
                    Session["pdfPathSig"] = Sigthumb;
                }


                if (ds.Tables[0].Rows.Count > 0)
                {
                    //gvDownloadDocuments.DataSource = ds.Tables[0];
                    //gvDownloadDocuments.DataBind();
                    lblApplicationID.Text = ds.Tables[0].Rows[0]["Agent ID"].ToString();
                    //lblCompanyName.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                    txtAgentName.Text = ds.Tables[0].Rows[0]["AgentName"].ToString();
                    // txtRegistrationType.Text = ds.Tables[0].Rows[0]["AgentAgentType"].ToString();
                    txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    TxtPassport.Text = ds.Tables[0].Rows[0]["PassportNo"].ToString();
                    Txtpanno.Text = ds.Tables[0].Rows[0]["PanNo"].ToString();
                    txtIFsccode.Text = ds.Tables[0].Rows[0]["IFSCCode"].ToString();

                    ViewState["AgentType"] = ds.Tables[0].Rows[0]["RoleID"].ToString();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: btnView_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void EyeImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                _AgentRegistrationDAL.AgentReqId = ViewState["AgentReqId"].ToString();
                //_AgentEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    string fileName = Ds.Tables[0].Rows[0]["IdentityProofType"].ToString();
                    strURL = Ds.Tables[0].Rows[0]["IdentityProofDocument"].ToString();
                    string FinalPath = filepath + strURL;

                    string pdfPath = FinalPath;
                    Session["pdfPath"] = pdfPath;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PdfExport.aspx');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: EyeImage_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        //protected void EyeImage1_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ImageButton Imgbtn = (ImageButton)sender;
        //        _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
        //        _AgentRegistrationDAL.AgentReqId = ViewState["AgentReqId"].ToString();
        //        //_AgentEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
        //        DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
        //        if (Ds.Tables[0].Rows.Count > 0)
        //        {
        //            string strURL = string.Empty;
        //            string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();
        //            strURL = Ds.Tables[1].Rows[0]["AddressProofDocument"].ToString();

        //            string pdfPath = strURL;
        //            Session["pdfPath"] = pdfPath;
        //            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PdfExport.aspx');", true);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("AgentVerification: EyeImage1_Click: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //    }
        //}

        //protected void EyeImage3_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ImageButton Imgbtn = (ImageButton)sender;
        //        _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
        //        _AgentRegistrationDAL.AgentReqId = ViewState["AgentReqId"].ToString();
        //        //_AgentEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
        //        DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
        //        if (Ds.Tables[0].Rows.Count > 0)
        //        {
        //            string strURL = string.Empty;
        //            string fileName = Ds.Tables[2].Rows[0]["SignatureProofType"].ToString();
        //            strURL = Ds.Tables[2].Rows[0]["SignatureProofDocument"].ToString();
        //            string pdfPath = strURL;
        //            Session["pdfPath"] = pdfPath;
        //            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PdfExport.aspx');", true);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("AgentVerification: EyeImage3_Click: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //    }

        //}

        protected void Button1_Click(object sender, EventArgs e)
        {

            ///txtResponse.InnerText = "{\"TotalRecords\": 3,\"ResultedRecords\": [{\"sourceInfo\": \"NAME1:Dawood Ibrahim\",\"uniqueNumber\": \"AAB1pKACMAAARMEAAiJYWWLRLFABPEENNIZYEF\",\"listName\": \"OFACSDNLIST\",\"listId\": \"9758\",\"rank\": \"100\",\"matchedInfo\": \"Dawood IBRAHIM\",\"matchDate\": \"23 JAN 2018 13:35\",\"status\": \"N\",\"comments\": \"N.A.\",\"serialNo1\": \"2\",\"customerName\": null,\"isDOBMatched\": \"Y\",\"countryOfOrigin\": \"UK\",\"sourceListMatch\": \"FCA (UK) Final Notices\"},{\"sourceInfo\": \"NAME1~Dawood Ibrahim\",\"uniqueNumber\": \"AAB1pKACMAAARMEAAiEEYNTGOOBXFPUIXTXABE\",\"listName\": \"OFACSDNLIST\",\"listId\": \"9759\",\"rank\": \"85\",\"matchedInfo\": \"DAWOOD IBRAHIM ORGANIZATION\",\"matchDate\": \"23 JAN 2018 13:35\",\"status\": \"N\",\"comments\": \"N.A.\",\"serialNo1\": \"1\",\"customerName\": null,\"isDOBMatched\": \"N\",\"countryOfOrigin\": \"UK\",\"sourceListMatch\": \"FCA (UK) Final Notices\"},{\"sourceInfo\": \"NAME2~osama bin laden\",\"uniqueNumber\": \"AAB1pKACMAAARMEAAiHGOVDVWTNYQHUOEXLXJQ\",\"listName\": \"OFACSDNLIST\",\"listId\": \"11378\",\"rank\": \"86\",\"matchedInfo\": \"Saad BIN LADEN\",\"matchDate\": \"23 JAN 2018 13:35\",\"status\": \"N\",\"comments\": \"N.A.\",\"serialNo1\": \"3\",\"customerName\": null,\"isDOBMatched\": \"N\",\"countryOfOrigin\": \"UK\",\"sourceListMatch\": \"FCA (UK) Final Notices\"}],\"FileName\": \"15166947219414059255\",\"FileImport\": \"N\"} ";
            //  txtResponse.InnerText = "Akash Yeda Ahefgdshihhhiufdghfuidghfduighfduighfdghfidhgfhdgfuidghfidhgfdhgfdhgfdhguifdhguifdhguifdhguifdhgfdhgfhdghfdgufdhguifdhgfudhgfudhgufdhgfhdghfdghfdghfduighfdughfdughfdhgfdhgfdughfdghfdughfdhgfdughfdughfdughfdughfdughfdghfdhgfduighfdughfughfughfughfdughfughfdughfdguhfdguhfdguihfdguhfdughfdguhfdughfughfguhfdguhfdguhfdughfughfguhfghfghfhgfhghfghfghfhgfhghfguhfguhfughfughfughfughfughfughfughfguhfguhfguhfghfhgfgfguhfguhfguhfghfghfghfhgfhgfughfguhfgufghfughfghfguhfgufhgufhgfughfguhfghfguhfghfguiifhgfhgfughfughfugihfguhfgufhgufghfughfughfghfgfhgufghfughfuigihfgh";
            ModalPopupResponse.Show();
        }

        protected void btnViewResp_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;
                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { '=' });
                string AgentReqId = commandArgs[0];
                ViewState["AgReqID"] = AgentReqId;
                _AgentRegistrationDAL.AgentReqId = AgentReqId;
                _AgentRegistrationDAL.Flag = 3;
                DataSet ds = _AgentRegistrationDAL.GetAgentResponseNew();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    GVPreverification.DataSource = ds.Tables[0];
                    GVPreverification.DataBind();
                    ModalPopupResponse.Show();


                }
                else
                {
                    ModalPopupResponse.Hide();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found', 'Warning');", true);
                }

               


            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: btnViewResp_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnClose_ServerClick(object sender, EventArgs e)
        {
            ModalPopupResponse.Hide();
        }

        #endregion

        #region View Download
        protected void btnViewDownload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                _AgentRegistrationDAL.AgentReqId = ViewState["AgentReqId"].ToString();
                //_AgentEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
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
                ErrorLog.AgentManagementTrace("AgentVerification: btnViewDownload_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        protected void ddlBCCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillAggregator();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: ddlBCCode_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                _AgentRegistrationDAL.AgentReqId = ViewState["AgentReqId"].ToString();
                //_AgentEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
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
                ErrorLog.AgentManagementTrace("AgentVerification: ImageButton1_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void imgbtnform_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                _AgentRegistrationDAL.AgentReqId = ViewState["AgentReqId"].ToString();
                //_AgentEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
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
                ErrorLog.AgentManagementTrace("AgentVerification: imgbtnform_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        #region Submit
        protected void btnSubmitDetails_Click(object sender, EventArgs e)
        {
            if (hdnUserConfirmation.Value.ToString() == "Yes")
            {
                try
                {
                    ErrorLog.AgentManagementTrace("AgVerification | btnSubmitDetails_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-Verification";
                    _auditParams[2] = "btnSubmitDetails";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    if (txtFinalRemarks.Text != null && txtFinalRemarks.Text != "")
                    {
                        _fileLineNo = "0";
                        if (rdbtnApproveDecline.SelectedValue == "Approve")
                        {
                            ViewState["ActionType"] = EnumCollection.EnumDBOperationType.Approve.ToString();
                        }
                        else
                        {
                            ViewState["ActionType"] = EnumCollection.EnumDBOperationType.Decline.ToString();
                        }
                        Session["Username"] = Session["Username"].ToString();
                        CommonSave("RadionButtonClick");
                        _AgentRegistrationDAL.AgentReqId = string.Empty;
                        FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                        txtFinalRemarks.Text = string.Empty;
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>showSuccess('" + _strAlertMessage_Total + _reocrdsProcessed + "  Successful : " + _successful + "  Unsuccessful : " + _unsuessful + " ');", true);
                        ViewState["ActionType"] = null;
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter Remarks', 'Agent Verification');", true);
                    }
                    ErrorLog.AgentManagementTrace("AgVerification | btnSubmitDetails_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }
                catch (Exception Ex)
                {
                    ErrorLog.AgentManagementTrace("AgentVerification: btnSubmitDetails_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ClearRemark();", true);
            }
        }
        #endregion

        #region Dropdown Events
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int Mode = (int)EnumCollection.StateCityMode.City;
                //_AgentEntity.State = ddlState.SelectedValue.ToString();
                DataSet ds_State = _AgentRegistrationDAL.GetCountryStateCity(UserName, Mode, 0, Convert.ToInt32(_AgentRegistrationDAL.AgentState));
                if (ds_State != null && ds_State.Tables.Count > 0 && ds_State.Tables[1].Rows.Count > 0)
                {
                    //ddlCity.Items.Clear();
                    //ddlCity.DataSource = ds_State.Tables[1];
                    //ddlCity.DataValueField = "ID";
                    //ddlCity.DataTextField = "Name";
                    //ddlCity.DataBind();
                    //ddlCity.Items.Insert(0, new ListItem("-- City --", "0"));
                }
                else
                {
                    //ddlCity.DataSource = null;
                    //ddlCity.DataBind();
                    //ddlCity.Items.Clear();
                    //ddlCity.Items.Insert(0, new ListItem("-- City --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: ddlState_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region SetPageFiltersExport

        private string SetPageFiltersExport()
        {
            string pageFilters = string.Empty;
            try
            {
                pageFilters = "Generated By " + Convert.ToString(Session["Username"]);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }

        #endregion

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | Unnamed_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                string pdfPath = Server.MapPath("~/Thumbnail/Aadhar/document-1_220422_110414 (1) (1).pdf");
                Session["pdfPath"] = pdfPath;
                string script = "<script type='text/javascript'>window.open('" + "PdfExport.aspx" + "')</script>";
                this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);
                ErrorLog.AgentManagementTrace("AgVerification | Unnamed_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: Unnamed_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | BtnCsv_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                //_AgentRegistrationDAL.Flag = ((int)EnumCollection.EnumPermissionType.AgL2Export);
                //DataSet dt = _AgentRegistrationDAL.GetAgentDetailsToProcessOnboaring();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.AgL2Export);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-Verification";
                    _auditParams[2] = "Export-To-CSV";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "L3 Approval Business Correspondents Details", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: BtnCsv_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | BtnXls_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                //_AgentRegistrationDAL.Flag = ((int)EnumCollection.EnumPermissionType.AgL2Export);
                //DataSet dt = _AgentRegistrationDAL.GetAgentDetailsToProcessOnboaring();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.AgL2Export);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-Verification";
                    _auditParams[2] = "Export-To-Excel";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "L3 Approval Business Correspondents Details", dt);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: BtnXls_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        protected void gvAgBulkL3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAgBulkL3.PageIndex = e.NewPageIndex;
                if (ViewState["AgentDataSet"] != null)
                {
                    DataTable dsTemp = (DataTable)ViewState["AgentDataSet"];
                    if (dsTemp != null && dsTemp.Rows.Count > 0)
                    {
                        gvAgBulkL3.Visible = true;
                        gvAgBulkL3.DataSource = dsTemp;
                        gvAgBulkL3.DataBind();

                        btnApprove.Visible = true;
                        btnDecline.Visible = true;
                        //btnTerminate.Visible = true;
                        lblRecordsTotal.Text = "Total " + Convert.ToString(dsTemp.Rows.Count) + " Record(s) Found.";
                    }
                    else
                    {
                        gvAgBulkL3.Visible = false;
                        btnApprove.Visible = false;
                        btnDecline.Visible = false;
                        //btnTerminate.Visible = false;
                        lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                    }
                }
                else
                    FillGridBulk(EnumCollection.EnumBindingType.AgBulkL3);
                CheckBoxAllOperationOnPageIndex();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: gvAgent_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        public DataSet FillGridBulk(EnumCollection.EnumBindingType _EnumBindingType, string sortExpression = null)
        {
            DataSet ds = null;
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | FillGridBulk() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());

                gvAgBulkL3.DataSource = null;
                gvAgBulkL3.DataBind();
                // SetPropertise();

                _ImportEntity.Flag = 1;
                _ImportEntity.FileID = ddlFileID.SelectedValue != "0" ? (ddlFileID.SelectedValue) : null;
                // _ImportEntity.FromDate = !string.IsNullOrEmpty(_FromDate) ? Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd") : null;
                // _ImportEntity.ToDate = !string.IsNullOrEmpty(_ToDate) ? Convert.ToDateTime(_ToDate).ToString("yyyy-MM-dd") : null;
                _ImportEntity.ChStatus = ((int)EnumCollection.Onboarding.CheckerApprove).ToString();
                _ImportEntity.AtStatus = ((int)EnumCollection.Onboarding.AuthorizerPending).ToString();
                gvAgBulkL3.Visible = true;
                ds = _ImportEntity.Get_AgentBulkUpload();
                if (_EnumBindingType == EnumCollection.EnumBindingType.AgBulkL3)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            DataView dv = ds.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";
                            dv.Sort = sortExpression + " " + this.SortDirection;
                            gvAgBulkL3.DataSource = dv;
                            gvAgBulkL3.DataBind();
                            gvAgBulkL3.Visible = true;

                        }
                        else
                        {
                            gvAgBulkL3.DataSource = ds.Tables[0];
                            gvAgBulkL3.DataBind();
                            gvAgBulkL3.Visible = true;

                        }
                    }
                    else
                    {
                        gvAgBulkL3.Visible = false;

                    }
                }
                ErrorLog.AgentManagementTrace("AgVerification | FillGridBulk() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgVerification: FillGridBulk(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return ds;
        }

        protected void ddlaggregatorCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDropdownsAgent();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: ddlaggregatorCode_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        private void BindFileId()
        {
            try
            {
                ddlFileID.Items.Clear();
                ddlFileID.DataSource = null;
                ddlFileID.DataBind();
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.ChStatus = ((int)EnumCollection.Onboarding.CheckerApprove).ToString();
                _AgentRegistrationDAL.AtStatus = ((int)EnumCollection.Onboarding.AuthorizerPending).ToString();
                DataTable ds = _AgentRegistrationDAL.GetFileDetails();

                if (ds != null && ds.Rows.Count > 0 && ds.Rows.Count > 0)
                {
                    if (ds.Rows.Count == 1)
                    {
                        ddlFileID.Items.Clear();
                        ddlFileID.DataSource = ds.Copy();
                        ddlFileID.DataTextField = "FileID";
                        ddlFileID.DataValueField = "FileID";
                        ddlFileID.DataBind();
                        ddlFileID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlFileID.Items.Clear();
                        ddlFileID.DataSource = ds.Copy();
                        ddlFileID.DataTextField = "FileID";
                        ddlFileID.DataValueField = "FileID";
                        ddlFileID.DataBind();
                        ddlFileID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlFileID.DataSource = null;
                    ddlFileID.DataBind();
                    ddlFileID.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: BindFileId: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        protected void btnViewBulk_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;
                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { '=' });
                //_AgentEntity.AgentReqId = ViewState["AgentReqId"].ToString();
                Session["FileID"] = commandArgs[0];
                ModalPopupExtender_EditRole.Show();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: btnView_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BulkApprove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | BulkApprove_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Verification";
                _auditParams[2] = "BulkApprove";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                _strAlertMessage_Total = "Total Record Processed for Approve Agent(s) :  ";

                _AgentRegistrationDAL.FileID = Session["FileID"].ToString();
                DataTable AgentDataSet = _AgentRegistrationDAL.GetFileData();
                if ((AgentDataSet != null))
                {
                    _fileLineNo = "0";
                    for (int i = 0; i < AgentDataSet.Rows.Count; i++)
                    {
                        try
                        {
                            _AgentRegistrationDAL.AtStatus = Convert.ToString((int)(EnumCollection.Onboarding.AuthorizerApprove));
                            _Flag = (int)EnumCollection.EnumDBOperationType.AuthApprove;

                            int _ActivityType = 0;

                            switch (AgentDataSet.Rows[i]["Activity Type"])
                            {
                                case "Onboard":
                                    _ActivityType = (int)EnumCollection.ActivityType.Onboard;
                                    break;
                                case "Activate":
                                    _ActivityType = (int)EnumCollection.ActivityType.Activate;
                                    break;
                                case "Deactivate":
                                    _ActivityType = (int)EnumCollection.ActivityType.Deactivate;
                                    break;
                                case "Terminate":
                                    _ActivityType = (int)EnumCollection.ActivityType.Terminate;
                                    break;
                                case "ReEdit":
                                    _ActivityType = (int)EnumCollection.ActivityType.ReEdit;
                                    break;
                                    //default  = 4
                            }

                            string AgentCode_ = null;
                            if (!string.IsNullOrEmpty(Convert.ToString(AgentDataSet.Rows[i]["Agent Code"])))
                            {
                                AgentCode_ = Convert.ToString(AgentDataSet.Rows[i]["Agent Code"]);
                            }
                            else
                            {
                                AgentCode_ = string.Empty;
                            }

                            _reocrdsProcessed = _reocrdsProcessed + 1;

                            _AgentRegistrationDAL.Flag = (Convert.ToInt32(_Flag));
                            _AgentRegistrationDAL.ActionType = (Convert.ToInt32(_Flag)).ToString();
                            _AgentRegistrationDAL.ActivityType = _ActivityType;         
                            _AgentRegistrationDAL.AgentReqId = Convert.ToString(AgentDataSet.Rows[i]["Agent ID"]);
                            _AgentRegistrationDAL.ClientId = Convert.ToString(AgentDataSet.Rows[i]["Client ID"]);

                            _AgentRegistrationDAL.ATRemark = txtResone.Text;
                            _AgentRegistrationDAL.UserName = Session["Username"].ToString();;

                            _AgentRegistrationDAL.PersonalContact = Convert.ToString(AgentDataSet.Rows[i]["Contact No."]);
                            _AgentRegistrationDAL.PersonalEmailID = Convert.ToString(AgentDataSet.Rows[i]["Email"]);

                            if (_ActivityType == 0)
                            {
                                // DivAgentDetails.Visible = true;
                                _salt = _AppSecurity.RandomStringGenerator();
                                _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();//Session["Username"].ToString();
                                _AgentRegistrationDAL.Flag = 1;
                                _AgentRegistrationDAL._RandomStringForSalt = null;
                                _AgentRegistrationDAL._RandomStringForSalt = _AppSecurity.RandomStringGenerator();
                                string _tempPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_AgentRegistrationDAL._RandomStringForSalt + _DefaultPassword);
                                _AgentRegistrationDAL.Password = _tempPassword;
                                _AgentRegistrationDAL._RandomStringForSalt = _salt;
                                _AgentRegistrationDAL.Password = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_salt + _DefaultPassword);
                                _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Onboard); ///category wise mail
                                DataSet dsAgentMaster = _AgentRegistrationDAL.SetInsertUpdateAgentMasterDetails();
                                string status = dsAgentMaster.Tables[0].Rows[0]["Status"].ToString();

                                if (dsAgentMaster != null && dsAgentMaster.Tables.Count > 0 && status == "Inserted")
                                {
                                    AgentCode_ = dsAgentMaster.Tables[0].Rows[0]["agentcode"].ToString();
                                    _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();

                                    //#region SMS
                                    //_EmailSMSAlertscs.FROM = Session["Username"].ToString();
                                    //_EmailSMSAlertscs.to = _AgentRegistrationDAL.PersonalContact;
                                    //_EmailSMSAlertscs.tempname = "SR24659_BCP1";
                                    //_EmailSMSAlertscs.OTPFlag = "0";
                                    //_EmailSMSAlertscs.var1 = "SBM";
                                    //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                    //ErrorLog.AgentManagementTrace("Page : AgentVerification.cs \nFunction : BulkApprove_ServerClick() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                                    //ErrorLog.SMSTrace("Page : AgentVerification.cs \nFunction : BulkApprove_ServerClick() => Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.PersonalContact);
                                    //_EmailSMSAlertscs.HttpGetRequest();
                                    //#endregion

                                    //#region EMAIL
                                    //_EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                                    //_EmailSMSAlertscs.to = _AgentRegistrationDAL.PersonalEmailID;
                                    //_EmailSMSAlertscs.tempname = "SR24659_EBCP4";
                                    //_EmailSMSAlertscs.OTPFlag = "0";
                                    //_EmailSMSAlertscs.var1 = "SBM";
                                    //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                    //ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : BulkApprove_ServerClick() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                                    //ErrorLog.SMSTrace("Page : AgentRegistration.cs \nFunction : BulkApprove_ServerClick() => Reistration Details forwarded for Email Preparation. Email : " + _AgentRegistrationDAL.PersonalEmailID);
                                    //_EmailSMSAlertscs.HttpGetRequestEmail();
                                    //#endregion
                                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Registered Successfully', 'Agent Registration');", true);
                                    // ErrorLog.AgentManagementTrace("AgentVerification: Commonsave:CheckAll(): Successful - Verification. StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                }
                                else
                                {
                                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Registration Unscuccessful : '" + status + " ');", true);
                                   // ErrorLog.AgentManagementTrace("AgentVerification: Commonsave:CheckAll(): Failed - Verification. Agent Already Terminated ");
                                   // ErrorLog.AgentManagementTrace("AgentVerification: Commonsave:CheckAll(): Successful - Verification. StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                }
                            }

                            if (Convert.ToInt32(Convert.ToString(_dsVerification.Tables[0].Rows[0][0])) > 0)
                            {
                                _successful = _successful + 1;
                            }
                            else
                            {
                                _unsuessful = _unsuessful + 1;
                            }


                        }
                        catch (Exception Ex)
                        {
                            _unsuessful = _unsuessful + 1;
                        }
                    }
                    TxtRemarks.Text = string.Empty;
                    Session["FileID"] = string.Empty;
                    ViewState["RecordCountDecine"] = _reocrdsProcessed;
                    FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                    FillGridBulk(EnumCollection.EnumBindingType.AgBulkL3);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _strAlertMessage_Total + _reocrdsProcessed + "  Successful : " + _successful + "  Unsuccessful : " + _unsuessful + " ');", true);
                }
                //_AgentRegistrationDAL.Mstatus = Convert.ToString((int)(EnumCollection.Onboarding.MakerApprove));
                //_AgentRegistrationDAL.ChStatus = Convert.ToString((int)(EnumCollection.Onboarding.CheckerPending));
                //_AgentRegistrationDAL.UserName = Session["Username"].ToString();
                //_AgentRegistrationDAL.MakerRemark = txtResone.Text;
                //_AgentRegistrationDAL.ActionType = "7";
                //_AgentRegistrationDAL.FileID = Session["FILEID"].ToString();
                //_dsVerification = _AgentRegistrationDAL.ChangeAgentStatusBulk();
                //if (_dsVerification != null && _dsVerification.Tables[0].Rows.Count > 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approve Successfully', 'Agent Verification');", true);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                //}
                ErrorLog.AgentManagementTrace("AgVerification | BulkApprove_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: BulkApprove_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        }


        protected void BulkDecline_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgVerification | BulkDecline_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-Verification";
                _auditParams[2] = "BulkDecline";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                _AgentRegistrationDAL.Mstatus = Convert.ToString((int)(EnumCollection.Onboarding.MakerDecline));
                _AgentRegistrationDAL.ChStatus = Convert.ToString((int)(EnumCollection.Onboarding.CheckerPending));
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.MakerRemark = txtResone.Text;
                _AgentRegistrationDAL.ActionType = "8";
                _AgentRegistrationDAL.FileID = Session["FILEID"].ToString();
                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatusBulk();
                if (_dsVerification != null && _dsVerification.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Decline Successfully', 'Agent Verification');", true);
                    ErrorLog.AgentManagementTrace("Page : AgentVerification.cs \nFunction : BulkDecline_ServerClick() => SMS/Email sending process started... Count :" + _dsVerification.Tables[1].Rows.Count);
                    for (int i = 0; i < _dsVerification.Tables[1].Rows.Count; i++)
                    {
                        ErrorLog.AgentManagementTrace("Page : AgLvl3 \nFunction : BulkDecline_ServerClick() => SMS/Email sending process started for SMS...");
                        #region SMS
                        _EmailSMSAlertscs.FROM = Session["Username"].ToString();
                        _EmailSMSAlertscs.to = Convert.ToString(_dsVerification.Tables[1].Rows[i]["ContactNo"]);
                        _EmailSMSAlertscs.tempname = "SR24659_BCP2";
                        _EmailSMSAlertscs.OTPFlag = "0";
                        _EmailSMSAlertscs.var1 = "SBM";
                        _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                        ErrorLog.AgentManagementTrace("Page : AgentVerification.cs \nFunction : BulkDecline_ServerClick() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                        ErrorLog.SMSTrace("Page : AgentVerification.cs \nFunction : BulkDecline_ServerClick() => Details forwarded for SMS Preparation. MobileNo : " + Convert.ToString(_dsVerification.Tables[1].Rows[i]["ContactNo"]));
                        _EmailSMSAlertscs.HttpGetRequest();
                        #endregion
                        ErrorLog.AgentManagementTrace("Page : AgentVerification.cs \nFunction : BulkDecline_ServerClick() => Email sending process started for Email...");
                        #region EMAIL
                        _EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                        _EmailSMSAlertscs.to = Convert.ToString(_dsVerification.Tables[1].Rows[i]["PersonalEmailID"]);
                        _EmailSMSAlertscs.tempname = "SR24659_EBCP3";
                        _EmailSMSAlertscs.OTPFlag = "0";
                        _EmailSMSAlertscs.var1 = "SBM";
                        _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                        ErrorLog.AgentManagementTrace("Page : AgentVerification.cs \nFunction : BulkDecline_ServerClick() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                        ErrorLog.SMSTrace("Page : AgentVerification.cs \nFunction : BulkDecline_ServerClick() => Reistration Details forwarded for Email Preparation. Email : " + Convert.ToString(_dsVerification.Tables[1].Rows[i]["PersonalEmailID"]));
                        _EmailSMSAlertscs.HttpGetRequestEmail();
                        #endregion
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                FillGridBulk(EnumCollection.EnumBindingType.AgBulkL3);
                ErrorLog.AgentManagementTrace("AgVerification | BulkDecline_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentVerification: BulkDecline_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        }


        //protected void BulkDecline_SerHJHJHJverClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        _AgentRegistrationDAL.Mstatus = Convert.ToString((int)(EnumCollection.Onboarding.MakerDecline));
        //        _AgentRegistrationDAL.ChStatus = Convert.ToString((int)(EnumCollection.Onboarding.CheckerPending));
        //        _AgentRegistrationDAL.UserName = Session["Username"].ToString();
        //        _AgentRegistrationDAL.MakerRemark = txtResone.Text;
        //        _AgentRegistrationDAL.ActionType = "8";
        //        _AgentRegistrationDAL.FileID = Session["FILEID"].ToString();
        //        _dsVerification = _AgentRegistrationDAL.ChangeAgentStatusBulk();
        //        if (_dsVerification != null && _dsVerification.Tables[0].Rows.Count > 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Decline Successfully', 'Agent Verification');", true);
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("AgentVerification: BulkDecline_ServerClick: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //    }
        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        //}

        protected void bulkClose_ServerClick(object sender, EventArgs e)
        {
            ErrorLog.AgentManagementTrace("AgVerification | bulkClose_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            ModalPopupExtender_EditRole.Hide();
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
            ErrorLog.AgentManagementTrace("AgVerification | bulkClose_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
        }
        protected void gvAgBulkL3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Contains("DownloadDoc"))
                {
                    ErrorLog.AgentManagementTrace("AgVerification | RowCommand-DownloadDoc | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    string status = string.Empty;
                    ImageButton lb = (ImageButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                    int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    _ImportEntity.Fileid = Convert.ToInt32(gvAgBulkL3.DataKeys[gvr.RowIndex].Values["FileID"]);
                    Session["FILEID"] = _ImportEntity.Fileid;
                    _ImportEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                    _ImportEntity.Flag = 3;
                    DataSet Ds = _ImportEntity.Get_AgentManualKycUpload();
                    if (string.IsNullOrEmpty(status)) status = "Bulk Manual Kyc Upload";
                    if (Ds != null && Ds.Tables.Count > 0)
                    {
                        #region Audit
                        _auditParams[0] = Session["Username"].ToString();
                        _auditParams[1] = "Agent-Verification";
                        _auditParams[2] = "RowCommand-DownloadDoc";
                        _auditParams[3] = Session["LoginKey"].ToString();
                        _LoginEntity.StoreLoginActivities(_auditParams);
                        #endregion
                        exportFormat.ExporttoExcel(Session["Username"].ToString(), Session["BankName"].ToString(), status, Ds);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Something went wrong. Try again', 'Warning');", true);
                        return;
                    }
                    ErrorLog.AgentManagementTrace("AgVerification | RowCommand-DownloadDoc | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }
                if (e.CommandName.Contains("DownloadZip"))
                {
                    try
                    {
                        ErrorLog.AgentManagementTrace("AgVerification | RowCommand-DownloadZip | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        #region Audit
                        _auditParams[0] = Session["Username"].ToString();
                        _auditParams[1] = "Agent-Verification";
                        _auditParams[2] = "RowCommand-DownloadZip";
                        _auditParams[3] = Session["LoginKey"].ToString();
                        _LoginEntity.StoreLoginActivities(_auditParams);
                        #endregion
                        ImageButton lb = (ImageButton)e.CommandSource;
                        GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                        int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());
                        string Fileid = Convert.ToString(gvAgBulkL3.DataKeys[gvr.RowIndex].Values["FileID"]);

                        ErrorLog.UploadTrace(string.Format("Initiated Document Upload Sample File Download Request."));
                        string strURL = string.Empty;
                        strURL = "~/TempFiles/Agent Manual KYC Bulk Import/" + Fileid + "/" + Fileid + ".zip";
                        WebClient req = new WebClient();
                        HttpResponse response = HttpContext.Current.Response;
                        response.Clear();
                        response.ClearContent();
                        response.ClearHeaders();
                        response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment;filename=" + Fileid + ".zip");
                        byte[] data = req.DownloadData(Server.MapPath(strURL));
                        response.BinaryWrite(data);
                        response.End();
                        ErrorLog.UploadTrace(string.Format("Completed Document Upload Sample File Download."));
                        ErrorLog.AgentManagementTrace("AgVerification | RowCommand-DownloadZip | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.AgentManagementTrace("Pre-Verification: DownloadZip_Click: Exception: " + ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                        return;
                    }
                }
                if (e.CommandName.Contains("FileEdit"))
                {
                    string status = string.Empty;
                    ImageButton lb = (ImageButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                    int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    _ImportEntity.Fileid = Convert.ToInt32(gvAgBulkL3.DataKeys[gvr.RowIndex].Values["FileID"]);
                    Session["FILEID"] = _ImportEntity.Fileid;
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.CommissionError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        protected void GVPreverification_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVPreverification.PageIndex = e.NewPageIndex;

            _AgentRegistrationDAL.AgentReqId = ViewState["AgReqID"].ToString();
            _AgentRegistrationDAL.Flag = 3;
            DataSet ds = _AgentRegistrationDAL.GetAgentResponseNew();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                GVPreverification.DataSource = ds.Tables[0];
                GVPreverification.DataBind();
                ModalPopupResponse.Show();
            }
            else
            {
                ModalPopupResponse.Hide();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found', 'Warning');", true);
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
                string ClientId = ddlBCCode.SelectedValue.ToString();
                int IsRemoved = 0;
                int IsActive = 1;
                int IsdocUploaded = 1;
                int VerificationStatus = 1;
                DataTable dsbc = _AgentRegistrationDAL.GetAggregator(UserName, VerificationStatus, IsActive, IsRemoved, ClientId, IsdocUploaded);
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
    }
}