using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Configuration;
using BussinessAccessLayer;
using AppLogger;
//using DataAccessLayer;
using System.Net;
using System.Threading;
using MaxiSwitch.EncryptionDecryption;

namespace SOR.Pages.BC
{
    public partial class BCVerificationLevelTwo : System.Web.UI.Page
    {
        #region Property Declaration
        BCEntity _BCEntity = new BCEntity();
        EmailAlerts _EmailAlerts = new EmailAlerts();
        #endregion

        #region Objects Declaration

        public string UserName { get; set; }
        ClientRegistrationEntity clientMngnt = new ClientRegistrationEntity();
        //public static DbAccess _dbAccess = new DbAccess();
        public string pathId, PathAdd, PathSig;

        string _DefaultPassword = ConnectionStringEncryptDecrypt.DecryptEncryptedDEK(AppSecurity.GenerateDfPw(), ConnectionStringEncryptDecrypt.ClearMEK);
        
        AppSecurity appSecurity = new AppSecurity();
        public AppSecurity _AppSecurity
        {
            get { if (appSecurity == null) appSecurity = new AppSecurity(); return appSecurity; }
            set { appSecurity = value; }
        }
        #endregion;

        #region Variable and Objects
        DataSet _dsVerification = null;
        DataSet _dsActivate = null;

        // User Credentials
        string[] _auditParams = new string[4];

        string[]
            _BCActiveParams = new string[8];

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
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserName"] != null && Session["UserRoleID"] != null)
                {
                        bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["UserName"].ToString(), Session["UserRoleID"].ToString(), "BCVerificationLevelTwo.aspx", "11");
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
                        BindDropdownClients();
                        BindDropdownsBC();
                        FillGrid(EnumCollection.EnumPermissionType.EnableMakerChecker);
                        ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                    }
                    UserPermissions.RegisterStartupScriptForNavigationListActive("4", "11");
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
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : Page_Load() \nException Occured\n" + Ex.Message);
                ////_dbAccess.StoreErrorDescription(UserName, "BCVerificationLevelTwo.cs", "Page_Load()", Ex);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
                return;
            }
        }
        #endregion

        #region Methods

        private void BindDropdownClients()
        {
            DataSet _dsClient = new DataSet();
            try
            {
                ddlClientCode.Items.Clear();
                ddlClientCode.DataSource = null;
                ddlClientCode.DataBind();
                clientMngnt.UserName = Session["Username"].ToString();
                clientMngnt.IsRemoved = "0";
                clientMngnt.IsActive = "1";
                clientMngnt.IsdocUploaded = "1";
                clientMngnt.VerificationStatus = "1";
                clientMngnt.SFlag = (int)EnumCollection.EnumPermissionType.EnableRoles;

                _dsClient = clientMngnt.BindClient();
                if (_dsClient != null && _dsClient.Tables.Count > 0)
                {
                    if (_dsClient.Tables.Count > 0 && _dsClient.Tables[0].Rows.Count > 0)
                    {
                        if (_dsClient.Tables[0].Rows.Count == 1)
                        {
                            ddlClientCode.Items.Clear();
                            ddlClientCode.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientCode.DataTextField = "ClientName";
                            ddlClientCode.DataValueField = "ClientID";
                            ddlClientCode.DataBind();
                            ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                        }
                        else
                        {
                            ddlClientCode.Items.Clear();
                            ddlClientCode.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientCode.DataTextField = "ClientName";
                            ddlClientCode.DataValueField = "ClientID";
                            ddlClientCode.DataBind();
                            ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                        }
                    }
                    else
                    {
                        ddlClientCode.Items.Clear();
                        ddlClientCode.DataSource = null;
                        ddlClientCode.DataBind();
                        ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlClientCode.Items.Clear();
                    ddlClientCode.DataSource = null;
                    ddlClientCode.DataBind();
                    ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : BindDropdownClients() \nException Occured\n" + Ex.Message);
                ////_dbAccess.StoreErrorDescription(UserName, "BCVerificationLevelTwo.cs", "BindDropdownClients()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Verified BC');", true);
                return;
            }
        }
        private void BindDropdownsBC()
        {
            try
            {
                ddlBCCode.Items.Clear();
                ddlBCCode.DataSource = null;
                ddlBCCode.DataBind();
                _BCEntity.UserName = Session["Username"].ToString();
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableMakerChecker;// EnableRoles;
                _BCEntity.CreatedBy = Session["Username"].ToString();
                _BCEntity.IsRemoved = "0";
                _BCEntity.IsActive = "0";
                _BCEntity.IsdocUploaded = "1";
                _BCEntity.VerificationStatus = "0";
                _BCEntity.Clientcode = ddlClientCode.SelectedValue != "0" ? ddlClientCode.SelectedValue : null;
                _BCEntity.BCID = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;
                _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                DataSet ds = _BCEntity.BindBCVerification();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        ddlBCCode.Items.Clear();
                        ddlBCCode.DataSource = ds.Tables[0].Copy();
                        ddlBCCode.DataTextField = "BCName";
                        ddlBCCode.DataValueField = "BC ID";
                        ddlBCCode.DataBind();
                        ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlBCCode.Items.Clear();
                        ddlBCCode.DataSource = ds.Tables[0].Copy();
                        ddlBCCode.DataTextField = "BCName";
                        ddlBCCode.DataValueField = "BC ID";
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
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : BindDropdownsBC() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Verified BC');", true);

                return;
            }
        }
        public DataSet FillGrid(EnumCollection.EnumPermissionType _EnumPermissionType)
        {
            DataSet ds = new DataSet();
            try
            {
                gvBusinessCorrespondents.DataSource = null;
                gvBusinessCorrespondents.DataBind();
                lblRecordsTotal.Text = "";
                ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                //_BCEntity.Flag = ((int)EnumCollection.EnumPermissionType.EnableMakerChecker);
                _BCEntity.Flag = (int)_EnumPermissionType;
                _BCEntity.Clientcode = ddlClientCode.SelectedValue != "0" ? ddlClientCode.SelectedValue : null;
                _BCEntity.BCID = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;
                _BCEntity.Activity = ddlActivityType.SelectedValue != "-1" ? ddlActivityType.SelectedValue : null;
                _BCEntity.UserName = Session["Username"].ToString();
                _BCEntity.IsRemoved = "0";
                _BCEntity.IsActive = "0";
                _BCEntity.IsdocUploaded = "1";
                _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                ds = _BCEntity.GetBCDetailsToProcessOnboaring();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvBusinessCorrespondents.Visible = true;
                    gvBusinessCorrespondents.DataSource = ds.Tables[0];
                    gvBusinessCorrespondents.DataBind();
                    btnApprove.Visible = true;
                    btnDecline.Visible = true;
                    panelGrid.Visible = true;
                    ViewState["BCDataSet"] = ds.Tables[0];
                    lblRecordsTotal.Text = "Total " + Convert.ToString(ds.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    gvBusinessCorrespondents.Visible = false;
                    btnApprove.Visible = false;
                    btnDecline.Visible = false;
                    //btnReprocess.Visible = false;
                    panelGrid.Visible = false;
                    panelGrid.Visible = false;
                    lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : FillGrid() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
            }
            return ds;
        }

        public void CommonSave(string fromWhere)
        {
            try
            {
                #region  Alert and Log Messages
                if (!String.IsNullOrEmpty(ViewState["ActionType"].ToString()))
                {
                    if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                    {
                        _strAlertMessage_Header = "Decline BC ";
                        _strAlertMessage_Success = "Decline Process ";
                        _strAlertMessage_UnSuccess = "Decline Process ";
                        _strAlertMessage_Total = "Total Record Processed for Decline BC(s) :  ";
                        _BCEntity.ATStatus = Convert.ToString((int)(EnumCollection.Onboarding.AuthorizerDecline));
                        _Flag = (int)EnumCollection.EnumDBOperationType.AuthDecline;
                    }
                    else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Approve.ToString())
                    {
                        _strAlertMessage_Header = "Approve BC ";
                        _strAlertMessage_Success = "Approve Process ";
                        _strAlertMessage_UnSuccess = "Approve Process ";
                        _strAlertMessage_Total = "Total Record Processed for Approve BC(s) :  ";
                        _BCEntity.ATStatus = Convert.ToString((int)(EnumCollection.Onboarding.AuthorizerApprove));
                        _Flag = (int)EnumCollection.EnumDBOperationType.AuthApprove;
                    }
                }
                #endregion
                switch (fromWhere)
                {
                    case "CheckedAll":
                        #region All Rows of GridView.
                        DataTable BCDataSet = (DataTable)ViewState["BCDataSet"];
                        if ((BCDataSet != null) && Convert.ToString(ViewState["SelectionType"]) == SelectionType.CheckedAll.ToString())
                        {
                            _fileLineNo = "0";
                            for (int i = 0; i < BCDataSet.Rows.Count; i++)
                            {
                                try
                                {

                                    int _ActivityType = 0;

                                    switch (BCDataSet.Rows[i]["Activity Type"])
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

                                    string BCCode_ = null;
                                    if (!string.IsNullOrEmpty(Convert.ToString(BCDataSet.Rows[i]["BC Code"])))
                                    {
                                        BCCode_ = Convert.ToString(BCDataSet.Rows[i]["BC Code"]);
                                    }
                                    else
                                    {
                                        BCCode_ = string.Empty;
                                    }

                                    _reocrdsProcessed = _reocrdsProcessed + 1;
                                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                                    _BCEntity.Flag = (Convert.ToInt32(_Flag));
                                    _BCEntity.ActionType = _Flag;
                                    _BCEntity.ActivityType = _ActivityType;
                                    _BCEntity.BCReqId = Convert.ToString(BCDataSet.Rows[i]["BC ID"]);
                                    _BCEntity.Clientcode = Convert.ToString(BCDataSet.Rows[i]["Client ID"]);

                                    _BCEntity.ATRemark = TxtRemarks.Text.Trim();
                                    _BCEntity.UserName = Session["Username"].ToString();
                                    _dsVerification = _BCEntity.ChangeBCStatus();


                                    if (_ActivityType == 0)
                                    {
                                        // DivBCDetails.Visible = true;
                                        _salt = _AppSecurity.RandomStringGenerator();
                                        _BCEntity.CreatedBy = Session["Username"].ToString();//Session["Username"].ToString();
                                        _BCEntity.BC_Code = _dsVerification.Tables[2].Rows[0]["BCReqId"].ToString();
                                        _BCEntity.Flag = 1;
                                        _BCEntity._RandomStringForSalt = null;
                                        _BCEntity._RandomStringForSalt = _AppSecurity.RandomStringGenerator();
                                        string _tempPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_BCEntity._RandomStringForSalt + _DefaultPassword);
                                        _BCEntity.Password = _tempPassword;
                                        _BCEntity._RandomStringForSalt = _salt;
                                        _BCEntity.Password = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_salt + _DefaultPassword);

                                        DataSet dsBCMaster = _BCEntity.SetInsertUpdateBCMasterDetails();
                                        if (dsBCMaster != null && dsBCMaster.Tables.Count > 0 && dsBCMaster.Tables[1].Rows[0]["Status"].ToString() == "Inserted")
                                        {
                                            _dsVerification = _BCEntity.ChangeBCStatus();
                                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Data Registered Successfully', 'BC-Verification');", true);
                                        }
                                    }
                                    if (_ActivityType == 1)
                                    {
                                        _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.Activate;
                                        _BCEntity.BCCode = BCCode_;
                                        _dsActivate = _BCEntity.ActiveDeactiveBC();
                                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                                            _dsVerification = _BCEntity.ChangeBCStatus();
                                    }

                                    if (_ActivityType == 2)
                                    {
                                        _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.Deactivate;
                                        _BCEntity.BCCode = BCCode_;
                                        _dsActivate = _BCEntity.ActiveDeactiveBC();
                                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                                        {
                                            _dsVerification = _BCEntity.ChangeBCStatus();
                                        }
                                    }


                                    if (_ActivityType == 3)
                                    {
                                        _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.OnboardTerminate;
                                        _BCEntity.BCCode = BCCode_;
                                        _dsActivate = _BCEntity.ActiveDeactiveBC();
                                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                                        {
                                            _dsVerification = _BCEntity.ChangeBCStatus();
                                        }
                                    }
                                    if (_ActivityType == 4)
                                    {
                                        _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.Reprocess;
                                        _dsActivate = _BCEntity.ChangeBCStatusReEdit();
                                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                                        {
                                            _dsVerification = _BCEntity.ChangeBCStatus();
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
                            ViewState["RecordCountDecine"] = _reocrdsProcessed;
                        }
                        #endregion
                        break;
                    case "MultiCheck":
                        #region Multiple Section Row of GridView.
                        // else
                        {
                            _fileLineNo = "0";
                            foreach (GridViewRow row in gvBusinessCorrespondents.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                                    if (chBoxRow.Checked)
                                    {
                                        try
                                        {
                                            int val = 0;
                                            switch (row.Cells[18].Text)
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
                                            string BCCode_ = null;
                                            if (!string.IsNullOrEmpty(row.Cells[4].Text))
                                            {
                                                BCCode_ = row.Cells[4].Text;
                                            }
                                            else
                                            {
                                                BCCode_ = string.Empty;
                                            }


                                            _reocrdsProcessed = _reocrdsProcessed + 1;
                                            SingleSave(_reocrdsProcessed, _Flag, _Flag, row.Cells[2].Text, row.Cells[5].Text, Convert.ToString(Session["Username"]), TxtRemarks.Text.Trim(), row.Cells[9].Text, row.Cells[8].Text, row.Cells[6].Text, val, BCCode_);
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
                            SingleSave(_reocrdsProcessed, _Flag, _Flag, Convert.ToString(ViewState["BCReqId"]), txtBCName.Text, Convert.ToString(Session["Username"]), txtFinalRemarks.Text.Trim(), ViewState["ReceiverEmailID"].ToString(), ViewState["ReceiverContactNo"].ToString(), Convert.ToString(Session["Client"]), _ActivityType, Convert.ToString(ViewState["BC Code"]));
                        }
                        catch (Exception Ex)
                        {
                            _unsuessful = _unsuessful + 1;
                        }
                        #endregion
                        break;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : CommonSave() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
            }
        }

        public void SingleSave(int _reocrdsProcessed, int _StatusFlag, int _Flag, string _BCReqId, string _strBCFullName, string User, string Remarks, string ReceiverEmailID, string ContactNo, string _ClientCode, int _ActivityType, string _BCCode)
        {
            try
            {
                ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Received - Maker Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                _BCEntity.ActivityType = _ActivityType;
                _BCEntity.BCCode = _BCCode;
                _BCEntity.BCReqId = _BCReqId;
                _reocrdsProcessed = _reocrdsProcessed + 1;
                //_BCEntity.ATStatus = Convert.ToInt16(_Flag);
                _BCEntity.Flag = (Convert.ToInt32(_Flag));
                _BCEntity.ActionType = _Flag;
                _BCEntity.ATRemark = Remarks;
                _BCEntity.UserName = Session["Username"].ToString();
                _BCEntity.Clientcode = _ClientCode;

                if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                {
                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                    _dsVerification = _BCEntity.ChangeBCStatus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showSuccess('Request Declined Successfully', 'BC-Verification');", true);
                    ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                }
                else
                {
                    if (_ActivityType == 0)
                    {
                        // DivBCDetails.Visible = true;
                        _salt = _AppSecurity.RandomStringGenerator();
                        _BCEntity.CreatedBy = Session["Username"].ToString();
                        _BCEntity.BCReqId = _BCReqId;
                        _BCEntity.Flag = 1;
                        _BCEntity._RandomStringForSalt = null;
                        _BCEntity._RandomStringForSalt = _AppSecurity.RandomStringGenerator();
                        string _tempPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_BCEntity._RandomStringForSalt + _DefaultPassword);
                        _BCEntity.Password = _tempPassword;
                        _BCEntity._RandomStringForSalt = _salt;
                        _BCEntity.Password = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_salt + _DefaultPassword);
                        _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Onboard); ///category wise mail
                        DataSet dsBCMaster = _BCEntity.SetInsertUpdateBCMasterDetails();
                        string status = dsBCMaster.Tables[0].Rows[0]["Status"].ToString();

                        if (dsBCMaster != null && dsBCMaster.Tables.Count > 0 && status == "Inserted")
                        {
                            _BCCode = dsBCMaster.Tables[0].Rows[0]["BCCode"].ToString();
                            _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                            _dsVerification = _BCEntity.ChangeBCStatus();
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showSuccess('Data Registered Successfully', 'BC-Verification');", true);
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Registration Unscuccessful : '" + status + " ');", true);
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                            return;
                        }
                    }

                    if (_ActivityType == 1)
                    {
                        _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.Activate;
                        _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Active); ///category wise mail
                        _BCEntity.BCCode = _BCCode;
                        _dsActivate = _BCEntity.ActiveDeactiveBC();
                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.AuthApprove;
                            _dsVerification = _BCEntity.ChangeBCStatus();
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showSuccess('Request Approved Successfully', 'BC-Verification');", true);
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);

                        }
                        else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('BC Already Terminated', 'BC-Verification');", true);
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                            return;
                        }
                    }

                    if (_ActivityType == 2)
                    {
                        _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.Deactivate;
                        _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Deactivate); ///category wise mail
                        _BCEntity.BCCode = _BCCode;
                        _dsActivate = _BCEntity.ActiveDeactiveBC();
                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.AuthApprove;
                            _dsVerification = _BCEntity.ChangeBCStatus();
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showSuccess('Request Approved Successfully', 'BC-Verification');", true);
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);

                        }
                        else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('BC Already Terminated', 'BC-Verification');", true);
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                            return;
                        }
                    }

                    if (_ActivityType == 3)
                    {
                        _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.OnboardTerminate;
                        _EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.Terminated); ///category wise mail
                        _BCEntity.BCCode = _BCCode;
                        _dsActivate = _BCEntity.ActiveDeactiveBC();
                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.AuthApprove;
                            _dsVerification = _BCEntity.ChangeBCStatus();
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showSuccess('Request Approved Successfully', 'BC-Verification');", true);
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);

                        }
                        else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "3")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('BC Already Terminated', 'BC-Verification');", true);
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                            return;
                        }
                    }

                    if (_ActivityType == 4)
                    {
                        _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.Reprocess;
                        _dsActivate = _BCEntity.ChangeBCStatusReEdit();
                        if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "00")
                        {
                            _BCEntity.ActionType = (int)EnumCollection.EnumDBOperationType.AuthApprove;
                            _dsVerification = _BCEntity.ChangeBCStatus();
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showSuccess('Request Approved Successfully', 'BC-Verification');", true);
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Successful - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);

                        }
                        else if (_dsActivate != null && _dsActivate.Tables.Count > 0 && _dsActivate.Tables[0].Rows[0][0].ToString() == "03")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('BC Already Terminated', 'BC-Verification');", true);
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification. Agent Already Terminated ");
                            ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks + "ActivityType: " + _ActivityType);
                            return;
                        }
                    }
                }
                if (_dsVerification != null &&
                    _dsVerification.Tables.Count > 0 &&
                    _dsVerification.Tables[0].Rows.Count > 0 &&
                    _dsVerification.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    _successful = _successful + 1;
                }
                else
                {
                    _unsuessful = _unsuessful + 1;
                }
                FillGrid(EnumCollection.EnumPermissionType.EnableMakerChecker);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("BC-Verification : SingleSave: Failed - Verification. Exception: " + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
                return;
            }
        }

        #endregion

        #region Search And Clear
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid(EnumCollection.EnumPermissionType.EnableMakerChecker);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnSearch_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
                return;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ddlClientCode.SelectedValue = "0";
                ddlBCCode.SelectedValue = "0";
                ddlActivityType.SelectedValue = "-1";
                //ddlState.SelectedValue = "0";

                //ddlCity.Items.Clear();
                //ddlCity.DataSource = null;
                //ddlCity.DataBind();
                //ddlCity.Items.Insert(0, new ListItem("-- City --", "0"));

                FillGrid(EnumCollection.EnumPermissionType.EnableMakerChecker);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnClear_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
                return;
            }
        }
        #endregion

        #region Approve
        protected void btnApprove_ServerClick(object sender, EventArgs e)
        {
            ViewState["ActionType"] = EnumCollection.EnumDBOperationType.Approve.ToString();
            try
            {
                foreach (GridViewRow row in gvBusinessCorrespondents.Rows)
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
                    lblModalHeaderName.Text = "BC(s) Approve Reason";
                    TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                    lblconfirm.Text = "Are you sure want to Approve BC(s)?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Select at least one record.','Approve BC(s)');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnApprove_ServerClick() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
                return;
            }
        }
        #endregion

        #region Decline
        protected void btnDecline_ServerClick(object sender, EventArgs e)
        {
            ViewState["ActionType"] = EnumCollection.EnumDBOperationType.Decline.ToString();
            try
            {
                foreach (GridViewRow row in gvBusinessCorrespondents.Rows)
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
                    lblModalHeaderName.Text = "BC(s) Decline Reason";
                    TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                    lblconfirm.Text = "Are you sure want to Decline BC(s)?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Select at least one record.','Decline BC(s)');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnDecline_ServerClick() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
                return;
            }
        }
        #endregion

        #region Terminate
        protected void btnTerminate_ServerClick(object sender, EventArgs e)
        {
            ViewState["ActionType"] = EnumCollection.EnumDBOperationType.Decline.ToString();
            try
            {
                foreach (GridViewRow row in gvBusinessCorrespondents.Rows)
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
                    lblModalHeaderName.Text = "BC(s) Terminate Reason";
                    TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                    lblconfirm.Text = "Are you sure want to Terminate BC(s)?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Select at least one record.','Terminate BC(s)');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnTerminate_ServerClick() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Select at least one record.','Terminate BC(s)');", true);
                return;
            }
        }
        #endregion

        #region Approve \ Decline \ Terminate Event Handler
        protected void btnSaveAction_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtRemarks.Text == null || TxtRemarks.Text == "")
                {
                    ModalPopupExtender_Declincard.Show();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter Remarks', 'BC-Verification');", true);
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

                    FillGrid(EnumCollection.EnumPermissionType.EnableMakerChecker);
                    TxtRemarks.Text = string.Empty;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('" + _strAlertMessage_Total + _reocrdsProcessed + "  Successful : " + _successful + "  Unsuccessful : " + _unsuessful + " ');", true);
                    ViewState["ActionType"] = null;
                    ViewState["SelectionType"] = null;
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnCancelAction_Click() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
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
                string _alertMessage = string.Empty;
                if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Approve.ToString())
                {
                    _alertMessage = "BC Approve";
                }
                else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                {
                    _alertMessage = "BC Decline";
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Operation has cancelled.','" + _alertMessage + "');", true);
                ModalPopupExtender_Declincard.Hide();
                TxtRemarks.Text = string.Empty;
                ViewState["ActionType"] = null;
                FillGrid(EnumCollection.EnumPermissionType.EnableMakerChecker);
                return;
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnCancelAction_Click() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong..! Please try again','Verification BC');", true);
                ViewState["ActionType"] = null;
                return;
            }
        }
        #endregion

        #region Grid Events
        protected void gvBusinessCorrespondents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBusinessCorrespondents.PageIndex = e.NewPageIndex;
                if (ViewState["BCDataSet"] != null)
                {
                    DataTable dsTemp = (DataTable)ViewState["BCDataSet"];
                    if (dsTemp != null && dsTemp.Rows.Count > 0)
                    {
                        gvBusinessCorrespondents.Visible = true;
                        gvBusinessCorrespondents.DataSource = dsTemp;
                        gvBusinessCorrespondents.DataBind();

                        btnApprove.Visible = true;
                        btnDecline.Visible = true;
                        //btnTerminate.Visible = true;
                        lblRecordsTotal.Text = "Total " + Convert.ToString(dsTemp.Rows.Count) + " Record(s) Found.";
                    }
                    else
                    {
                        gvBusinessCorrespondents.Visible = false;
                        btnApprove.Visible = false;
                        btnDecline.Visible = false;
                        //btnTerminate.Visible = false;
                        lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                    }
                }
                else
                    FillGrid(EnumCollection.EnumPermissionType.EnableMakerChecker);
                CheckBoxAllOperationOnPageIndex();
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : gvBusinessCorrespondents_PageIndexChanging() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong..! Please try again','Verification BC');", true);
                return;
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
                        CheckBox CheckBoxAll = gvBusinessCorrespondents.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvBusinessCorrespondents.Rows)
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
                        CheckBox _CheckBoxAll = gvBusinessCorrespondents.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvBusinessCorrespondents.Rows)
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
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : chBoxSelectRow_CheckedChanged() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong..! Please try again','Verification BC');", true);
                return;
            }
        }

        protected void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox CheckBoxAll = gvBusinessCorrespondents.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                foreach (GridViewRow row in gvBusinessCorrespondents.Rows)
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
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : CheckBoxAll_CheckedChanged() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong..! Please try again','Verification BC');", true);
                return;
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
                        CheckBox CheckBoxAll = gvBusinessCorrespondents.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = true;
                        foreach (GridViewRow row in gvBusinessCorrespondents.Rows)
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
                        CheckBox CheckBoxAll = gvBusinessCorrespondents.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = false;
                        foreach (GridViewRow row in gvBusinessCorrespondents.Rows)
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
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : CheckBoxAllOperationOnPageIndex() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong..! Please try again','Verification BC');", true);
                return;
            }
        }
        #endregion

        #region CSV
        protected void btnExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.ExportGrid);
                ExportFormat _ExportFormat = new ExportFormat();
                if (dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["UserName"]), "Proxima", "BC-Verification", dt);
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Verification Level 2-BC Detailss');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnExportCSV_Click() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong..! Please try again','Verification BC');", true);
                return;
            }
        }
        #endregion

        #region Excel
        protected void btnExportExcel_ServerClick1(object sender, EventArgs e)
        {
            try
            {
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.ExportGrid);
                ExportFormat _ExportFormat = new ExportFormat();
                if (dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["UserName"]), "Proxima", "BC-Verification", dt);
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Verification Level 2-BC Details');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btndownload_ServerClick() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong..! Please try again','Verification BC');", true);
                return;
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
                //_BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                string BCReqId = commandArgs[0];
                _BCEntity.BCReqId = BCReqId;
                ViewState["BCReqId"] = BCReqId;
                string ReceiverEmailID = Convert.ToString(commandArgs[1]);
                string Activity_Type = Convert.ToString(commandArgs[3]);
                ViewState["Activity Type"] = Activity_Type;
                _BCEntity.Mode = "GetBCDetails";
                //_BCEntity.BCBCID = BCReqId;
                //ViewState["BCReqId"] = BCReqId;
                ViewState["ReceiverEmailID"] = ReceiverEmailID;
                string contactNo = Convert.ToString(commandArgs[2]);
                ViewState["ReceiverContactNo"] = contactNo;
                string BCCode = Convert.ToString(commandArgs[3]);
                ViewState["BC Code"] = BCCode;

                DataSet ds = _BCEntity.GetBCDocuments();

                _BCEntity.Mode = "GetBCDocumentByID";
                DataSet get = _BCEntity.GetBCDocuments();

                string defaultimage = "images/Question.png";

                string idthumb = !string.IsNullOrEmpty(get.Tables[0].Rows[0]["IdentityProofDocument"].ToString()) ? get.Tables[0].Rows[0]["IdentityProofDocument"].ToString() : defaultimage.ToString();
                if (!string.IsNullOrEmpty(get.Tables[0].Rows[0]["IdentityProofDocument"].ToString()))
                {
                    string FileThumbnailId = Path.GetDirectoryName(idthumb) + "\\" + Path.GetFileNameWithoutExtension(idthumb) + "_Thumbnail.png";
                    pathId = "../../" + FileThumbnailId;
                }
                else
                {
                    pathId = "../../" + idthumb;
                }
                Session["pdfPathID"] = idthumb;

                string Addthumb = !string.IsNullOrEmpty(get.Tables[0].Rows[0]["AddressProofDocument"].ToString()) ? get.Tables[0].Rows[0]["AddressProofDocument"].ToString() : defaultimage.ToString();
                if (!string.IsNullOrEmpty(get.Tables[0].Rows[0]["AddressProofDocument"].ToString()))
                {
                    string FileThumbnailAdd = Path.GetDirectoryName(Addthumb) + "\\" + Path.GetFileNameWithoutExtension(Addthumb) + "_Thumbnail.png";
                    PathAdd = "../../" + FileThumbnailAdd;
                }
                else
                {
                    PathAdd = "../../" + Addthumb;
                }
                Session["pdfPathAdd"] = Addthumb;

                string Sigthumb = !string.IsNullOrEmpty(get.Tables[0].Rows[0]["SignatureProofDocument"].ToString()) ? get.Tables[0].Rows[0]["SignatureProofDocument"].ToString() : defaultimage.ToString();
                if (!string.IsNullOrEmpty(get.Tables[0].Rows[0]["SignatureProofDocument"].ToString()))
                {
                    string FileThumbnailSig = Path.GetDirectoryName(Sigthumb) + "\\" + Path.GetFileNameWithoutExtension(Sigthumb) + "_Thumbnail.png";
                    PathSig = "../../" + FileThumbnailSig;
                }
                else
                {
                    PathSig = "../../" + Sigthumb;
                }
                Session["pdfPathSig"] = Sigthumb;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblApplicationID.Text = ds.Tables[0].Rows[0]["BCID"].ToString();
                    txtBCName.Text = ds.Tables[0].Rows[0]["BCName"].ToString();
                    txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    ViewState["BCType"] = ds.Tables[0].Rows[0]["RoleID"].ToString();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
                return;
            }
        }

        protected void EyeImage_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton Imgbtn = (ImageButton)sender;
            _BCEntity.Mode = "GetBCDocumentById";
            _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
            DataSet Ds = _BCEntity.GetBCDocuments();
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

        protected void EyeImage1_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton Imgbtn = (ImageButton)sender;
            _BCEntity.Mode = "GetBCDocumentById";
            _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
            DataSet Ds = _BCEntity.GetBCDocuments();
            if (Ds.Tables[0].Rows.Count > 0)
            {
                string strURL = string.Empty;
                string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();
                strURL = Ds.Tables[1].Rows[0]["AddressProofDocument"].ToString();

                string pdfPath = strURL;
                Session["pdfPath"] = pdfPath;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PdfExport.aspx');", true);
            }
        }

        protected void EyeImage3_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton Imgbtn = (ImageButton)sender;
            _BCEntity.Mode = "GetBCDocumentById";
            _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
            //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
            DataSet Ds = _BCEntity.GetBCDocuments();
            if (Ds.Tables[0].Rows.Count > 0)
            {
                string strURL = string.Empty;
                string fileName = Ds.Tables[2].Rows[0]["SignatureProofType"].ToString();
                strURL = Ds.Tables[2].Rows[0]["SignatureProofDocument"].ToString();
                string pdfPath = strURL;
                Session["pdfPath"] = pdfPath;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PdfExport.aspx');", true);
            }

        }
        #endregion

        #region View Download
        protected void btnViewDownload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    //string fileName = Ds.Tables[0].Rows[0]["IdentityProofType"].ToString();
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
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnViewDownload_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);

                return;
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {

                    string strURL = string.Empty;
                    //string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    strURL = Ds.Tables[0].Rows[0]["AddressProofDocument"].ToString();
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
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : ImageButton1_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC Verification');", true);

                return;
            }
        }

        protected void imgbtnform_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {

                    string strURL = string.Empty;
                   // string fileName = Ds.Tables[2].Rows[0]["SignatureProofType"].ToString();
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    strURL = Ds.Tables[0].Rows[0]["SignatureProofDocument"].ToString();
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
                ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : ImageButton2_Click() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC Verification');", true);

                return;
            }
        }

        #region Submit
        protected void btnSubmitDetails_Click(object sender, EventArgs e)
        {
            if (HiddenConfirm.Value.ToString() == "Yes")
            {
                try
                {
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
                        FillGrid(EnumCollection.EnumPermissionType.EnableMakerChecker);
                        txtFinalRemarks.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showSuccess('" + _strAlertMessage_Total + _reocrdsProcessed + "  Successful : " + _successful + "  Unsuccessful : " + _unsuessful + " ');", true);
                        ViewState["ActionType"] = null;
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Please enter Remarks', 'BC-Verification');", true);
                    }
                }
                catch (Exception Ex)
                {
                    ErrorLog.BCManagementTrace("Class : BC-Verification.cs \nFunction : btnSubmitDetails_Click() \nException Occured\n" + Ex.Message);

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BC-Verification');", true);
                    return;
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
                _BCEntity.Mode = "getStateAndCity";
                //_BCEntity.State = ddlState.SelectedValue.ToString();
                DataSet ds_State = _BCEntity.getStateAndCity();
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
            catch (Exception ex)
            {
                ErrorLog.BCManagementTrace("Page : BC-Verification.cs \nFunction : ddlState_SelectedIndexChanged\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'Verification BC');", true);
                return;
            }
        }

        protected void ddlClientCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDropdownsBC();
            }
            catch (Exception ex)
            {
                ErrorLog.BCManagementTrace("Page : BC-Verification.cs \nFunction : ddlClientCode_SelectedIndexChanged\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'Verification BC');", true);
                return;
            }
        }
        #endregion

        #region Export
        private string GenerateToExcel(DataTable Tbl)
        {
            string FileName;
            try
            {
                if (!Directory.Exists(Convert.ToString(ConfigurationManager.AppSettings.Get("ReportDownPath")) + "\\" + "Files" + "\\"))
                {
                    Directory.CreateDirectory(Convert.ToString(ConfigurationManager.AppSettings.Get("ReportDownPath")) + "\\" + "Files" + "\\");
                }
                string PathLocation = (Convert.ToString(ConfigurationManager.AppSettings.Get("ReportDownPath")) + "\\" + "Files" + "\\");


                //string strMntYr = Convert.ToDateTime(datePicker.Value).ToString("MMMM-yyyy");

                FileName = @"" + "EJ Pulling Status CDMake" + ".XLS";


                System.IO.DirectoryInfo di = new DirectoryInfo(PathLocation);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                //CreateExcelFile.CreateExcelDocument(Tbl, PathLocation + FileName);

                return PathLocation + FileName;
            }
            catch (Exception Ex)
            {
                //_systemLogger.WriteErrorLog(this, Ex);
            }
            return "";
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
                //_systemLogger.WriteErrorLog(this, Ex);
            }
            return pageFilters;
        }

        #endregion


        #endregion

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.ExportGrid);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "BC-Verification", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);
                }

            }

            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BC-Verification.cs \nFunction : BtnCsv_Click\nException Occured\n" + Ex.Message);

            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.ExportGrid);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "BC-Verification", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BC-Verification.cs \nFunction : BtnXls_Click\nException Occured\n" + Ex.Message);

            }
        }
    }
}