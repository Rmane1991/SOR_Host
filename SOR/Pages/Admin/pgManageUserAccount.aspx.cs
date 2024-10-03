using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessAccessLayer;
using AppLogger;
using MaxiSwitch.EncryptionDecryption;
using System.Threading;
using System.Configuration;
using System.IO;

namespace SOR.Pages.Admin
{
    public partial class pgManageUserAccount : System.Web.UI.Page
    {
        #region data Member Object Classes
        AppSecurity appSecurity = null;
        public AppSecurity _AppSecurity
        {
            get { if (appSecurity == null) appSecurity = new AppSecurity(); return appSecurity; }
            set { appSecurity = value; }
        }
        public UserManagement user = null;
        public UserManagement _UserManagement
        {
            get { if (user == null) user = new UserManagement(); return user; }
            set { user = value; }
        }
        public EmailAlerts Email = new EmailAlerts();

        public ExportFormat _exportFormat = null;
        public ExportFormat exportFormat
        {
            get { if (_exportFormat == null) _exportFormat = new ExportFormat(); return _exportFormat; }
            set { _exportFormat = value; }
        }
        DataSet _dsUsersList = null;
        string[] _auditParams = new string[4];
        string _NotificationMessage = string.Empty;
        static int PageIndex = 0;

        string _Role = string.Empty,
        _ApplicationType = string.Empty,
        _UserID = string.Empty;
        string _UserId = string.Empty,
        _Email = string.Empty,
        _UserType = string.Empty,
        _SecurityAns = string.Empty;

        String
            _Password = null,
            _Flag = null,
            _Status = null,
            _RandomStringForSalt = null,
            _DefaultPassword = null;

        static int _ActionTypeID = 0;

        public enum ActionType
        {
            Reset = 1,
            UnBlock = 2,
            Block = 3,
            Terminate = 4
        }
        public enum enumDBActionFlagType
        {
            Update = 1,
            Check = 2
        }
        ClientRegistrationEntity _clientMngnt = null;
        public ClientRegistrationEntity clientMngnt
        {
            get { if (_clientMngnt == null) _clientMngnt = new ClientRegistrationEntity(); return _clientMngnt; }
            set { _clientMngnt = value; }
        }
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                       bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "pgManageUserAccount.aspx", "11");
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
                    if (!IsPostBack  && HasPagePermission)
                    {
                            UserPermissions.RegisterStartupScriptForNavigationListActive("2", "11");
                            BindDropdownRoles();
                            BindDropdownUsers();
                            BindDropdownClientDetails();
                            FillUsers();
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
                ErrorLog.AdminManagementTrace("pgManageUserAccount: Page_Load: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region FillGrid
        public DataSet FillUsers()
        {
            DataSet _dsUsersList = null;
            try
            {
                setProperties();
                _dsUsersList = _UserManagement.FillGridUserAccounts();

                gvUserManageRole.DataSource = null;
                if (_dsUsersList != null && _dsUsersList.Tables.Count > 0 && _dsUsersList.Tables[0].Rows.Count > 0)
                {
                    gvUserManageRole.Visible = true;
                    panelGrid.Visible = true;

                    gvUserManageRole.PageIndex = PageIndex;
                    gvUserManageRole.DataSource = _dsUsersList.Tables[0];
                    gvUserManageRole.DataBind();
                    lblRecordsCount.Visible = true;
                    lblRecordsCount.Text = "Total " + Convert.ToString(_dsUsersList.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    panelGrid.Visible = false;
                    gvUserManageRole.Visible = false;
                    lblRecordsCount.Text = "Total 0 Record(s) Found.";
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: FillUsers: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return _dsUsersList;
            }
            return _dsUsersList;
        }

        private void BindDropdownRoles()
        {
            try
            {
                _UserManagement._RoleId = Session["UserRoleID"].ToString();
                _UserManagement.UserName = Session["Username"].ToString();
                DataSet dsUserRoles = _UserManagement.BindRolesAndRoleGroup();
                if (dsUserRoles != null && dsUserRoles.Tables.Count > 0 && dsUserRoles.Tables[0].Rows.Count > 0)
                {
                    ddlRoleID.DataSource = dsUserRoles.Tables[1];
                    ddlRoleID.DataTextField = "RoleName";
                    ddlRoleID.DataValueField = "RoleId";
                    ddlRoleID.DataBind();
                    ddlRoleID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: BindDropdownRoles: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void BindDropdownClientDetails()
        {
            DataSet _dsClient = new DataSet();
            try
            {
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
                            ddlClient.Items.Clear();
                            ddlClient.DataSource = _dsClient.Tables[0].Copy();
                            ddlClient.DataTextField = "ClientName";
                            ddlClient.DataValueField = "ClientID";
                            ddlClient.DataBind();
                        }
                        else
                        {
                            ddlClient.Items.Clear();
                            ddlClient.DataSource = _dsClient.Tables[0].Copy();
                            ddlClient.DataTextField = "ClientName";
                            ddlClient.DataValueField = "ClientID";
                            ddlClient.DataBind();
                            ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                        }
                    }
                    else
                    {
                        ddlClient.Items.Clear();
                        ddlClient.DataSource = null;
                        ddlClient.DataBind();
                        ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlClient.Items.Clear();
                    ddlClient.DataSource = null;
                    ddlClient.DataBind();
                    ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: BindDropdownClientDetails: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void AuditLogEntry(string ActionType, string ActionStatus)
        {
            try
            {
                _auditParams[0] = Convert.ToString(Session["Username"]);
                _auditParams[1] = ActionType.ToString() + " User";
                _auditParams[2] = "User " + lblID.Text + " " + ActionType.ToString() + " operation is " + ActionStatus + "." + ActionType.ToString() != "Reset" ? " For Reason  : " + TxtRemarks.Text : "";
                _auditParams[3] = "Mobile App User";
                _UserManagement.insertTblAuditTrail(_auditParams);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: AuditLogEntry: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void ResetUserCredentials()
        {
            try
            {
                string _tempPassword = string.Empty;
                try
                {
                    _RandomStringForSalt = null;
                    _DefaultPassword = ConnectionStringEncryptDecrypt.DecryptEncryptedDEK(AppSecurity.GenerateDfPw(), ConnectionStringEncryptDecrypt.ClearMEK);
                    _RandomStringForSalt = _AppSecurity.RandomStringGenerator();
                    _tempPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_RandomStringForSalt + _DefaultPassword);
                }
                catch (Exception Ex)
                {
                    ErrorLog.AdminManagementTrace("pgManageUserAccount: ResetUserCredentials: Exception: " + Ex.Message);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                    return;
                }

                if (!String.IsNullOrEmpty(lblID.Text))
                {
                    _UserManagement._UserId = lblID.Text;
                    _UserManagement._ActionType = (int)ActionType.Reset;
                    _UserManagement._Password = _tempPassword;
                    _UserManagement._RandomStringForSalt = _RandomStringForSalt;
                    _UserManagement.UserName = Session["Username"].ToString();
                    _UserManagement.Flag = 1;
                    DataSet ds = _UserManagement.ManageUserAccounts();

                    _Status = !String.IsNullOrEmpty(_Status) ? _Status : null;

                    if (!string.IsNullOrEmpty(_Status) && (_Status == "SUCCESSFUL" || _Status == "ALLOWED"))
                    {
                        // Send email.
                        Email.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.EmailAlert);
                        Email.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.ResetPassword);
                        Email.SubCategoryTypeId = null;
                        Email.ClientID = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                        Email.UserName = Session["Username"].ToString();
                        Email.UserID = lblID.Text;
                        Email.Flag = "3";
                        ErrorLog.writeLogEmail("Page : pgManageUserAccount.cs \nFunction : ResetPassword() => Reset  Details forwarded for Email Preparation. => PrepareEmailFormat()");
                        Email.PrepareEmailFormat();
                        // Send SMS.
                        Email.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.SMSAlert);
                        Email.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.ResetPassword);
                        Email.SubCategoryTypeId = null;
                        Email.ClientID = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                        Email.UserName = Session["Username"].ToString();
                        Email.UserID = lblID.Text;
                        Email.Flag = "3";
                        ErrorLog.writeLogEmail("Page : pgManageUserAccount.cs \nFunction : ResetPassword() =>  Reset  Details forwarded for SMS Preparation. => PrepareSMSFormat()");
                        Email.PrepareSMSFormat();

                        ModalPopupExtenderToGetReason.Hide();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Password reset successfully.','" + lblModalHeaderName.Text + "');", true);
                        TxtRemarks.Text = string.Empty;
                        lblID.Text = string.Empty;
                        AuditLogEntry(ViewState["ActionType"].ToString(), "successful");
                        FillUsers();
                        return;
                    }
                    else
                    {
                        ModalPopupExtenderToGetReason.Hide();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Problem While perform the action.','" + lblModalHeaderName.Text + "');", true);
                        TxtRemarks.Text = string.Empty;
                        lblID.Text = string.Empty;
                        AuditLogEntry(ViewState["ActionType"].ToString(), "failed");
                        return;
                    }
                }
                else
                {
                    ModalPopupExtenderToGetReason.Show();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid User.','" + lblModalHeaderName.Text + "');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: ResetUserCredentials: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void BindDropdownUsers()
        {
            DataSet _dstblMasterDetails = new DataSet();
            try
            {
                _UserManagement.UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                _UserManagement._RoleId = ddlRoleID.SelectedValue != "0" && ddlRoleID.SelectedValue != "" ? ddlRoleID.SelectedValue : null;
                _UserManagement.Status = ddlStatusType.SelectedValue != "0" && ddlStatusType.SelectedValue != "" ? ddlStatusType.SelectedValue : null;
                _UserManagement._UserID = ddlUser.SelectedValue != "0" && ddlUser.SelectedValue != "" ? ddlUser.SelectedValue : null;
                _UserManagement.Flag = 0;

                _dstblMasterDetails = _UserManagement.BindDropdownUsers();
                if (_dstblMasterDetails != null && _dstblMasterDetails.Tables.Count > 0)
                {
                    if (_dstblMasterDetails.Tables.Count > 0 && _dstblMasterDetails.Tables[0].Rows.Count > 0)
                    {
                        ddlUser.Items.Clear();
                        ddlUser.DataSource = _dstblMasterDetails.Tables[0].Copy();
                        ddlUser.DataTextField = "UserName";
                        ddlUser.DataValueField = "UserID";
                        ddlUser.DataBind();
                        ddlUser.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlUser.Items.Clear();
                        ddlUser.DataSource = null;
                        ddlUser.DataBind();
                        ddlUser.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlUser.Items.Clear();
                    ddlUser.DataSource = null;
                    ddlUser.DataBind();
                    ddlUser.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: BindDropdownUsers: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void setProperties()
        {
            try
            {
                _UserManagement.UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                _UserManagement._RoleId = ddlRoleID.SelectedValue != "0" && ddlRoleID.SelectedValue != "" ? ddlRoleID.SelectedValue : null;
                _UserManagement._UserID = ddlUser.SelectedValue != "0" && ddlUser.SelectedValue != "" ? ddlUser.SelectedValue : null;
                _UserManagement.Status = ddlStatusType.SelectedValue != "0" && ddlStatusType.SelectedValue != "" ? ddlStatusType.SelectedValue : null;
                _UserManagement.Flag = 0;
                _UserManagement._ClientID = ddlClient.SelectedValue != "0" && ddlClient.SelectedValue != "" ? ddlClient.SelectedValue : null;
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: setProperties: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

       
        private string IsOperationValid()
        {
            try
            {
                _UserManagement.UserName = Session["Username"].ToString();
                _UserManagement._UserID = lblID.Text;
                _UserManagement.Reason = TxtRemarks.Text.Trim();
                _UserManagement.Flag = (int)enumDBActionFlagType.Check;
                _UserManagement.UserType = Convert.ToString(ViewState["UserType"]);
                _UserManagement._ActionTypeID = _ActionTypeID;
                _UserManagement.ManageUserAccountsAction(out _Status);
                _Status = !String.IsNullOrEmpty(_Status) ? _Status : null;
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: IsOperationValid: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return _Status;
            }
            return _Status;
        }
        #endregion

        #region Button Events

        protected void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillUsers();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: buttonSearch_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ddlRoleID.SelectedValue = "0";
                ddlUser.SelectedValue = "0";
                ddlStatusType.SelectedValue = "0";
                ddlClient.SelectedValue = "0";
                FillUsers();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: btnReset_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Grid Events
        protected void gvUserManageRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "Reset")
                {
                    try
                    {
                        lblID.Text = e.CommandArgument.ToString();
                        ViewState["ActionType"] = ActionType.Reset.ToString();
                        _ActionTypeID = (int)ActionType.Reset;
                        _Flag = Convert.ToString((int)enumDBActionFlagType.Check);
                        _Status = IsOperationValid();
                        if (!string.IsNullOrEmpty(_Status) && _Status.ToUpper() == "ALLOWED".ToUpper())
                        {
                            lblModalHeaderName.Text = "Reset User";
                            ResetUserCredentials();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('User Not Active.','Reset Password');", true);
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.AdminManagementTrace("pgManageUserAccount: gvUserManageRole_RowCommand: Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                        return;
                    }
                }
                else if (e.CommandName.ToString() == "Block")
                {
                    try
                    {
                        lblID.Text = e.CommandArgument.ToString();
                        ViewState["ActionType"] = ActionType.Block.ToString();
                        _ActionTypeID = (int)ActionType.Block;
                        _Flag = Convert.ToString((int)enumDBActionFlagType.Check);
                        _Status = IsOperationValid();
                        if (!string.IsNullOrEmpty(_Status) && _Status.ToUpper() == "ALLOWED".ToUpper())
                        {
                            lblModalHeaderName.Text = "Block User";
                            TxtRemarks.Style.Add("Placeholder", "Enter the reason.");
                            lblconfirm.Text = "Are you sure want to Block this user?";
                            ModalPopupExtenderToGetReason.Show();

                            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                            Label lblUserType = (Label)row.FindControl("lblUserType");
                            ViewState["UserType"] = lblUserType.Text;
                        }
                        else if(!string.IsNullOrEmpty(_Status) && _Status.ToUpper() == "ACTIVE IN REGISTRATION".ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('You do not have permission please contact system administrator.','block User');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('User Already Blocked.','block User');", true);

                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.AdminManagementTrace("pgManageUserAccount: gvUserManageRole_RowCommand: Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                        return;
                    }
                }
                else if (e.CommandName.ToString() == "UnBlock")
                {
                    try
                    {
                        lblID.Text = e.CommandArgument.ToString();
                        ViewState["ActionType"] = ActionType.UnBlock.ToString();
                        _ActionTypeID = (int)ActionType.UnBlock;
                        _Flag = Convert.ToString((int)enumDBActionFlagType.Check);
                        _Status = IsOperationValid();
                        if (!string.IsNullOrEmpty(_Status) && _Status.ToUpper() == "ALLOWED".ToUpper())
                        {
                            lblModalHeaderName.Text = "UnBlock User";
                            TxtRemarks.Style.Add("Placeholder", "Enter the reason.");
                            lblconfirm.Text = "Are you sure want to UnBlock this user?";
                            ModalPopupExtenderToGetReason.Show();
                            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                            Label lblUserType = (Label)row.FindControl("lblUserType");
                            ViewState["UserType"] = lblUserType.Text;
                        }
                        else if (!string.IsNullOrEmpty(_Status) && _Status.ToUpper() == "DEACTIVE IN REGISTRATION".ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('You do not have permission please contact system administrator.','Unblock User');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('User Not Blocked.','Unblock User');", true);
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.AdminManagementTrace("pgManageUserAccount: gvUserManageRole_RowCommand: Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                        return;
                    }
                }
                else if (e.CommandName.ToString() == "Terminate")
                {
                    try
                    {
                        lblID.Text = e.CommandArgument.ToString();
                        ViewState["ActionType"] = ActionType.Terminate.ToString();
                        _ActionTypeID = (int)ActionType.Terminate;
                        _Flag = Convert.ToString((int)enumDBActionFlagType.Check);
                        _Status = IsOperationValid();
                        if (!string.IsNullOrEmpty(_Status) && _Status.ToUpper() == "ALLOWED".ToUpper())
                        {
                            IsOperationValid();
                            lblModalHeaderName.Text = "Terminate User";
                            TxtRemarks.Style.Add("Placeholder", "Enter the reason.");
                            lblconfirm.Text = "Are you sure want to Terminate this user?";
                            ModalPopupExtenderToGetReason.Show();
                            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                            Label lblUserType = (Label)row.FindControl("lblUserType");
                            ViewState["UserType"] = lblUserType.Text;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('User Already  Terminated.','Terminate User');", true);
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.AdminManagementTrace("pgManageUserAccount: gvUserManageRole_RowCommand: Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: gvUserManageRole_RowCommand: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void gvUserManageRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                PageIndex = e.NewPageIndex;
                gvUserManageRole.PageIndex = PageIndex;
                FillUsers();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: gvUserManageRole_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Reset \ Block \ UnBlock \ Terminate Event Handler
        protected void buttonModalOK_Click(object sender, EventArgs e)
        {
            string _Status;
            try
            {
                if (!string.IsNullOrEmpty(TxtRemarks.Text))
                {
                    if (!String.IsNullOrEmpty(lblID.Text))
                    {
                        _UserManagement.UserName = Session["Username"].ToString();
                        _UserManagement._UserID = lblID.Text;
                        _UserManagement.Reason = TxtRemarks.Text.Trim();
                        _UserManagement.Flag = (int)enumDBActionFlagType.Update;
                        _UserManagement.UserType = Convert.ToString(ViewState["UserType"]);
                        _UserManagement._ActionTypeID = _ActionTypeID;
                        _UserManagement.ManageUserAccountsAction(out _Status);
                        _Status = !String.IsNullOrEmpty(_Status) ? _Status : null;
                        if (!string.IsNullOrEmpty(_Status))
                        {
                            // Send email.
                            //Mail For Blocked
                            if (_ActionTypeID == 3)
                            {
                                Email.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.EmailAlert);
                                Email.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.BlockUserAccount);
                                Email.SubCategoryTypeId = null;
                                Email.ClientID = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                                Email.UserName = Session["Username"].ToString();
                                Email.UserID = lblID.Text;
                                Email.Flag = "1";
                                ErrorLog.writeLogEmail("Page : pgManageUserAccount.cs \nFunction : Blocked() =>  Blocked  Details forwarded for Email Preparation. => PrepareEmailFormat()");
                                Email.PrepareEmailFormat();

                                // Send SMS.
                                Email.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.SMSAlert);
                                Email.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.ResetPassword);
                                Email.SubCategoryTypeId = null;
                                Email.ClientID = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                                Email.UserName = Session["Username"].ToString();
                                Email.UserID = lblID.Text;
                                Email.Flag = "1";
                                ErrorLog.writeLogEmail("Page : pgManageUserAccount.cs \nFunction : Blocked() =>  Blocked  Details forwarded for SMS Preparation. => PrepareSMSFormat()");
                                Email.PrepareSMSFormat();
                            }
                            //Mail For Unblocked
                            if (_ActionTypeID == 2)
                            {
                                Email.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.EmailAlert);
                                Email.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.UnblockUserAccount);
                                Email.SubCategoryTypeId = null;
                                Email.ClientID = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                                Email.UserName = Session["Username"].ToString();
                                Email.UserID = lblID.Text;
                                Email.Flag = "1";
                                ErrorLog.writeLogEmail("Page : pgManageUserAccount.cs \nFunction : Unblocked() =>  Unblocked  Details forwarded for Email Preparation. => PrepareEmailFormat()");
                                Email.PrepareEmailFormat();

                                // Send SMS.
                                Email.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.SMSAlert);
                                Email.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.ResetPassword);
                                Email.SubCategoryTypeId = null;
                                Email.ClientID = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                                Email.UserName = Session["Username"].ToString();
                                Email.UserID = lblID.Text;
                                Email.Flag = "1";
                                ErrorLog.writeLogEmail("Page : pgManageUserAccount.cs \nFunction : Unblocked() =>  Unblocked  Details forwarded for SMS Preparation. => PrepareSMSFormat()");
                                Email.PrepareSMSFormat();
                            }
                            //Mail For Terminate
                            if (_ActionTypeID == 4)
                            {
                                Email.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.EmailAlert);
                                Email.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.TerminateUserAccount);
                                Email.SubCategoryTypeId = null;
                                Email.ClientID = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                                Email.UserName = Session["Username"].ToString();
                                Email.UserID = lblID.Text;
                                Email.Flag = "1";
                                ErrorLog.writeLogEmail("Page : pgManageUserAccount.cs \nFunction : Terminate() =>  Terminate Users  Details forwarded for Email Preparation. => PrepareEmailFormat()");
                                Email.PrepareEmailFormat();

                                // Send SMS.
                                Email.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.SMSAlert);
                                Email.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.ResetPassword);
                                Email.SubCategoryTypeId = null;
                                Email.ClientID = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                                Email.UserName = Session["Username"].ToString();
                                Email.UserID = lblID.Text;
                                Email.Flag = "1";
                                ErrorLog.writeLogEmail("Page : pgManageUserAccount.cs \nFunction : Terminate() =>   Terminate Users  Details forwarded for SMS Preparation. => PrepareSMSFormat()");
                                Email.PrepareSMSFormat();
                            }
                            ModalPopupExtenderToGetReason.Hide();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Action completed successfully','" + lblModalHeaderName.Text + "');", true);

                            //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + _Status + "','" + lblModalHeaderName.Text + "');", true);
                            TxtRemarks.Text = string.Empty;
                            lblID.Text = string.Empty;
                            AuditLogEntry(ViewState["ActionType"].ToString(), "successful");
                            FillUsers();
                            return;
                        }
                        else
                        {
                            ModalPopupExtenderToGetReason.Hide();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Problem While perform the action.','" + lblModalHeaderName.Text + "');", true);
                            TxtRemarks.Text = string.Empty;
                            lblID.Text = string.Empty;
                            AuditLogEntry(ViewState["ActionType"].ToString(), "failed");
                            return;
                        }
                    }
                    else
                    {
                        ModalPopupExtenderToGetReason.Show();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid User.','" + lblModalHeaderName.Text + "');", true);
                        return;
                    }
                }
                else
                {
                    ModalPopupExtenderToGetReason.Show();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter the reason.','" + lblModalHeaderName.Text + "');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: buttonModalOK_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Cancel Action
        protected void btnCancelAction_Click(object sender, EventArgs e)
        {
            string _alertMessage = string.Empty;
            try
            {
                ModalPopupExtenderToGetReason.Hide();

                if (ViewState["ActionType"] != null)
                {
                    if (ViewState["ActionType"].ToString() == ActionType.Terminate.ToString())
                        _alertMessage = "TERMINATE USER";
                    else if (ViewState["ActionType"].ToString() == ActionType.Block.ToString())
                        _alertMessage = "BLOCK USER";
                    else if (ViewState["ActionType"].ToString() == ActionType.UnBlock.ToString())
                        _alertMessage = "UNBLOCK USER";
                }
                else
                    _alertMessage = "Manage Device Users";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Operation has cancelled.','" + _alertMessage + "');", true);
                return;
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: btnCancelAction_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region CSV
        protected void btnExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                FillUsers();
                exportFormat.ExportInCSV(Session["Username"].ToString(), Session["BankName"].ToString(), "Manage User Account Report", _dsUsersList);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: btnExportCSV_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Excel
        protected void btnExportXLS_Click(object sender, EventArgs e)
        {
            try
            {
                FillUsers();
                exportFormat.ExporttoExcel(Session["Username"].ToString(), "PayRakam", "User Account Report", _dsUsersList);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: btnExportXLS_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Export
      
        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillUsers();
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "Manage User Account Report", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: BtnCsv_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillUsers();

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "Manage User Account Report", dt);
                }
                {
                    //lblRecordCount.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgManageUserAccount: BtnXls_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

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
                FileName = @"" + "Manage User Account" + ".XLS";
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
                ErrorLog.AdminManagementTrace("pgManageUserAccount: GenerateToExcel: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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
                ErrorLog.AdminManagementTrace("pgManageUserAccount: SetPageFiltersExport: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return pageFilters;
        }
        #endregion
    }
}