using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using MaxiSwitch.EncryptionDecryption;
using BussinessAccessLayer;
using AppLogger;
using System.Threading;

namespace SOR.Pages.Admin
{
    public partial class pgCreateUser : System.Web.UI.Page
    {
        #region Object Declaration
        DataSet dsUserRoles = new DataSet();
        string _RandomStringForSalt = string.Empty;
        string[] _auditParams = new string[4];
        public AppSecurity _AppSecurity = new AppSecurity();
        UserManagement _UserManagement = new UserManagement();
        public ClientRegistrationEntity clientMngnt = new ClientRegistrationEntity();
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    //bool HasPagePermission = true;
                        bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "pgCreateUser.aspx", "2");
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
                            UserPermissions.RegisterStartupScriptForNavigationListActive("2", "2");
                        if (!Page.IsPostBack && HasPagePermission)
                        {
                            ddlRole.Items.Insert(0, (new ListItem("--Select--", "0")));
                            ddlUserRoleGroup.Items.Insert(0, (new ListItem("--Select--", "0")));
                            ddlUsers.Items.Insert(0, (new ListItem("--Select--", "0")));

                            txtUserName.Attributes.Add("autocomplete", "off");
                            txtPassword.Attributes.Add("autocomplete", "off");
                            FillBc();
                            //BindDropdownRoles();
                            hidIsAllowToValidateText.Value = "1";
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
                //}
                //}
                //if (!String.IsNullOrEmpty(txtPassword.Text.Trim()) || !String.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
                //{
                //    txtPassword.Attributes["value"] = txtPassword.Text;
                //    txtConfirmPassword.Attributes["value"] = txtConfirmPassword.Text;
                //}
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgCreateUser: Page_Load: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Something went wrong. Try again', 'Warning');</script>", false);
                return;
            }
        }
        #endregion

        #region Bind Masters
        private void BindDropdownRoles()
        {
            try
            {
                _UserManagement._ClientID = ddlClient.SelectedValue == "1" ? "Maximus" : ddlClient.SelectedValue.ToString();
                _UserManagement.UserName = Session["Username"].ToString();

                dsUserRoles = _UserManagement.BindRoles_CreateUser();
                if (dsUserRoles != null && dsUserRoles.Tables.Count > 0 && dsUserRoles.Tables[0].Rows.Count > 0)
                {
                    ddlRole.Items.Clear();
                    ddlRole.DataSource = dsUserRoles.Tables[0];
                    ddlRole.DataTextField = "RoleName";
                    ddlRole.DataValueField = "RoleId";
                    ddlRole.DataBind();
                    ddlRole.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                }
                else
                {
                    ddlRole.Items.Clear();
                    ddlRole.DataSource = null;
                    ddlRole.DataBind();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgCreateUser: BindDropdownRoles(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Something went wrong. Try again', 'Warning');</script>", false);
                return;
            }
        }

        //private void BindDropdownClientDetails()
        //{
        //    DataSet _dsClient = new DataSet();
        //    try
        //    {
        //        _UserManagement.UserName = Session["Username"].ToString();
        //        _UserManagement.ClientID = ddlClient.SelectedValue != "0" ? (ddlClient.SelectedValue) : null;
        //        _dsClient = _UserManagement.BindClientReport();
        //        if (_dsClient != null && _dsClient.Tables.Count > 0)
        //        {
        //            if (_dsClient.Tables.Count > 0 && _dsClient.Tables[0].Rows.Count > 0)
        //            {
        //                if (_dsClient.Tables[0].Rows.Count == 1)
        //                {
        //                    ddlClient.Items.Clear();
        //                    ddlClient.DataSource = _dsClient.Tables[0].Copy();
        //                    ddlClient.DataTextField = "ClientName";
        //                    ddlClient.DataValueField = "ClientID";
        //                    ddlClient.DataBind();
        //                    if (Session["ParentRoleID"] != null && Session["ParentRoleID"].ToString() == "1")
        //                    {
        //                        ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
        //                    }
        //                    BindDropdownRoles();
        //                }
        //                else
        //                {
        //                    ddlClient.Items.Clear();
        //                    ddlClient.DataSource = _dsClient.Tables[0].Copy();
        //                    ddlClient.DataTextField = "ClientName";
        //                    ddlClient.DataValueField = "ClientID";
        //                    ddlClient.DataBind();
        //                    ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
        //                }
        //            }
        //            else
        //            {
        //                ddlClient.Items.Clear();
        //                ddlClient.DataSource = null;
        //                ddlClient.DataBind();
        //                ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));
        //            }
        //        }
        //        else
        //        {
        //            ddlClient.Items.Clear();
        //            ddlClient.DataSource = null;
        //            ddlClient.DataBind();
        //            ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AdminManagementTrace("pgCreateUser: BindDropdownClientDetails(): Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Something went wrong. Try again', 'Warning');</script>", false);
        //        return;
        //    }
        //}

        private void fillClient()
        {
            DataSet _dsClient = new DataSet();
            try
            {
                _UserManagement.UserName = Session["Username"].ToString();
                _UserManagement.ClientID = ddlClient.SelectedValue != "0" ? (ddlClient.SelectedValue) : null;
                _dsClient = _UserManagement.BindClientReport();
                if (_dsClient != null && _dsClient.Tables.Count > 0)
                {
                    if (_dsClient.Tables.Count > 0 && _dsClient.Tables[0].Rows.Count > 0)
                    {
                        if (_dsClient.Tables[0].Rows.Count == 1)
                        {
                            ddlUsers.Items.Clear();
                            ddlUsers.DataSource = _dsClient.Tables[0].Copy();
                            ddlUsers.DataTextField = "ClientName";
                            ddlUsers.DataValueField = "ClientID";
                            ddlUsers.DataBind();
                        }
                        else
                        {
                            ddlUsers.Items.Clear();
                            ddlUsers.DataSource = _dsClient.Tables[0].Copy();
                            ddlUsers.DataTextField = "ClientName";
                            ddlUsers.DataValueField = "ClientID";
                            ddlUsers.DataBind();
                            ddlUsers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                        }
                    }
                    else
                    {
                        ddlUsers.Items.Clear();
                        ddlUsers.DataSource = null;
                        ddlUsers.DataBind();
                        ddlUsers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlUsers.Items.Clear();
                    ddlUsers.DataSource = null;
                    ddlUsers.DataBind();
                    ddlUsers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgCreateUser: fillClient(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Something went wrong. Try again', 'Warning');</script>", false);
                return;
            }
        }

        // Need to Check
        private void fillFranchise()
        {
            try
            {
                ErrorLog.CommonTrace("Create User Request Failed With fillFranchise. ");
                _UserManagement.UserName = Session["Username"].ToString();
                _UserManagement.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
                _UserManagement.ClientID = ddlClient.SelectedValue != "0" ? (ddlClient.SelectedValue) : null;
                DataSet ds = _UserManagement.BindFranchiseReport();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ddlUsers.Items.Clear();
                            ddlUsers.DataSource = ds.Tables[0].Copy();
                            ddlUsers.DataTextField = "BCName";
                            ddlUsers.DataValueField = "BCcode";
                            ddlUsers.DataBind();
                            ddlUsers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--BC--", "0"));
                        }
                        else
                        {
                            ddlUsers.Items.Clear();
                            ddlUsers.DataSource = ds.Tables[0].Copy();
                            ddlUsers.DataTextField = "BCName";
                            ddlUsers.DataValueField = "BCcode";
                            ddlUsers.DataBind();
                            ddlUsers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--BC--", "0"));

                        }
                    }
                    else
                    {
                        ddlUsers.Items.Clear();
                        ddlUsers.DataSource = null;
                        ddlUsers.DataBind();
                        ddlUsers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--BC--", "0"));
                    }
                }
                else
                {
                    ddlUsers.Items.Clear();
                    ddlUsers.DataSource = null;
                    ddlUsers.DataBind();
                    ddlUsers.Items.Insert(0, new ListItem("BC", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgCreateUser: fillFranchise(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Something went wrong. Try again', 'Warning');</script>", false);
                return;
            }
        }
        #endregion

        #region Button Events
        protected void btnSave_Click(object sender, EventArgs e)
         {
            try
            {
                // Unmasking Form - Variables Which are masked on Page Level.
                txtUserName.Text = !string.IsNullOrEmpty(hidUsername.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidUsername.Value)) : txtUserName.Text;
                txtPassword.Text = !string.IsNullOrEmpty(hidpassword.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidpassword.Value)) : txtPassword.Text;
                txtConfirmPassword.Text = !string.IsNullOrEmpty(hidconfmpassword.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidconfmpassword.Value)) : txtConfirmPassword.Text;

                if (string.IsNullOrEmpty(txtUserName.Text))
                {
                    txtUserName.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Username must contain minimum 6 characters','Create User');</script>", false);
                    return;
                }
                if (txtUserName.Text.Contains('%') || txtUserName.Text.Contains('!') || txtUserName.Text.Contains('?') || txtUserName.Text.Contains(':') || txtUserName.Text.Contains(',') || txtUserName.Text.Contains(';') || txtUserName.Text.Contains('+') || txtUserName.Text.Contains('-') || txtUserName.Text.Contains('*') || txtUserName.Text.Contains('/') || txtUserName.Text.Contains('^') || txtUserName.Text.Contains('<') || txtUserName.Text.Contains('>') || txtUserName.Text.Contains('|') || txtUserName.Text.Contains('}') || txtUserName.Text.Contains('{') || txtUserName.Text.Contains(')') || txtUserName.Text.Contains('(') || txtUserName.Text.Contains('~') || txtUserName.Text.Contains('`') || txtUserName.Text.Contains('^') || txtUserName.Text.Contains('=') || txtUserName.Text.Contains('[') || txtUserName.Text.Contains(']'))
                {
                    txtUserName.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Username only allow alphabet and numbers','Create User');</script>", false);
                    return;
                }
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('New Password and Confirm Password should match','Create User');</script>", false);
                    return;
                }
                else if (txtPassword.Text.Length < 8 || txtPassword.Text.Length > 16 ||
                        !System.Text.RegularExpressions.Regex.IsMatch(txtPassword.Text, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$") ||
                        !System.Text.RegularExpressions.Regex.IsMatch(txtPassword.Text, @"") || !System.Text.RegularExpressions.Regex.IsMatch(txtPassword.Text, @"[a-zA-Z]"))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Length of password should be between 8 -16 charachters. Password should contain atleast one special character. Password should contain atleast one numeric character. Password should contain atleast one alphabet','Create User');</script>", false);
                    return;
                }
                if (ddlClient.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Please select client','Create User');</script>", false);
                    return;
                }
                else
                {
                    ErrorLog.CommonTrace("Request Received From Create User. Username : "+ Session["Username"].ToString());
                    if (!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtConfirmPassword.Text) && ddlRole.SelectedValue.ToString() != "0")
                    {
                        if (txtEmail.Text.Contains('.') && txtEmail.Text.Contains('@') && txtEmail.Text[txtEmail.Text.Length - 1] != '.')
                        {
                            _UserManagement._UserName = txtUserName.Text;
                            DataSet TempDataSet = _UserManagement.CheckIsUserExist();
                            int _userCount = Convert.ToInt32(TempDataSet.Tables[0].Rows[0][0].ToString());

                            _UserManagement._Email = txtEmail.Text;   
                            DataSet TempDataSetEmail = _UserManagement.CheckIsEmailExist();
                            int _EmailCount = Convert.ToInt32(TempDataSetEmail.Tables[0].Rows[0][0].ToString());

                            _UserManagement._Mobile = txtMobile.Text; 
                            DataSet TempDataSetMobile = _UserManagement.CheckIsMobileExist();
                            int _MobileCount = Convert.ToInt32(TempDataSetMobile.Tables[0].Rows[0][0].ToString());
                            _RandomStringForSalt = _AppSecurity.RandomStringGenerator();
                            if (_userCount == 0 && _EmailCount == 0 && _MobileCount == 0)
                            {
                                _UserManagement._UserName = txtUserName.Text;
                                _UserManagement._Password = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_RandomStringForSalt + txtPassword.Text);
                                _UserManagement.Email = txtEmail.Text;
                                _UserManagement._Mobile = txtMobile.Text;
                                _UserManagement._RoleId = ddlRole.SelectedValue.ToString();
                                _UserManagement.UserRoleGroup = ddlUserRoleGroup.SelectedValue.ToString();
                                _UserManagement.UserName = Session["Username"].ToString();
                                _UserManagement.__RandomStringForSalt = _RandomStringForSalt;
                                _UserManagement._RandomStringForSalt = _RandomStringForSalt;
                                _UserManagement._ClientID = ddlClient.SelectedValue == "1" ? "Maximus" : ddlClient.SelectedValue.ToString();
                                _UserManagement._UserId = ddlUsers.SelectedValue.ToString();
                                //try {
                                //    ErrorLog.CommonTrace("Request Received From Create User." + Environment.NewLine + ErrorLog.XmlSerialser(_UserManagement));
                                //}
                                //catch (Exception Ex)
                                //{
                                //    ErrorLog.CommonError(Ex);
                                //    ErrorLog.CommonTrace("Request Received From Create User. UserName : " +_UserManagement._UserName  + " Password : " + _UserManagement._Password  +" Email : " + _UserManagement.Email +" Mobile : " + _UserManagement._Mobile +" RoleId : " + _UserManagement._RoleId +" UserRoleGroup : " + _UserManagement.UserRoleGroup+" UserName : " + _UserManagement.UserName +" RandomStringForSalt : " + _UserManagement.__RandomStringForSalt +" RandomStringForSalt : " + _UserManagement._RandomStringForSalt +" ClientID : " + _UserManagement._ClientID +" UserId : " + _UserManagement._UserId);
                                //}
                                DataSet ds = _UserManagement.CreateUser();
                                if (ds != null && ds.Tables[0].Rows[0]["Flag"].ToString() == "Inserted")
                                {
                                    ErrorLog.CommonTrace("Create User Request Successful. Username : " + Session["Username"].ToString() + " User Name : " + txtUserName.Text);
                                    _auditParams[0] = Convert.ToString(Session["Username"]);
                                    _auditParams[1] = "User Creation";
                                    _auditParams[2] = "User " + txtUserName.Text + " Created Successfully";
                                    _UserManagement.StoreLoginActivities(_auditParams);
                                    ClearContoles();
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('User created successfully', 'Create User');", true);
                                }
                                else
                                {
                                    ErrorLog.CommonTrace("Create User Request Failed With Unknown Error Occured In Database. Username : " + Session["Username"].ToString() + " User Name : " + txtUserName.Text);
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again.','Create User');", true);
                                }
                            }
                            else
                            {
                                ErrorLog.CommonTrace("Create User Request Failed With Record with the same Username or EmailID or MobileNo. is already exist. Username : " + Session["Username"].ToString() + " User Name : " + txtUserName.Text);
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Record with the same Username or EmailID or MobileNo. is already exist.','Create User');", true);
                            }
                        }
                        else
                        {
                            ErrorLog.CommonTrace("Create User Request Failed With Please enter the correct email id. Username : " + Session["Username"].ToString() + " Email : "+ txtEmail.Text);
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter the correct email id.','Create Userr');", true);
                        }
                    }
                    else
                    {
                        ErrorLog.CommonTrace("Create User Request Failed With All Fields Are Mandatory. Username : " + Session["Username"].ToString());
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('All fields are mandatory.','Create User');", true);
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgCreateUser: btnSave_Click(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearContoles();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgCreateUser: btnClear_Click(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void ClearContoles()
        {
            try
            {
                txtUserName.Text = string.Empty;
                hidIsAllowToValidateText.Value = "0";
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;
                txtPassword.Attributes["value"] = string.Empty;
                txtConfirmPassword.Attributes["value"] = string.Empty;
                txtEmail.Text = string.Empty;
                txtMobile.Text = string.Empty;
                ddlClient.ClearSelection();
                ddlClient.Items.Clear();
                ddlClient.DataSource = null;
                ddlClient.DataBind();
                ddlClient.Items.Insert(0, (new ListItem("--Select--", "0")));
                ddlRole.ClearSelection();
                ddlRole.Items.Clear();
                ddlRole.DataSource = null;
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, (new ListItem("--Select--", "0")));
                ddlUserRoleGroup.ClearSelection();
                ddlUserRoleGroup.Items.Clear();
                ddlUserRoleGroup.DataSource = null;
                ddlUserRoleGroup.DataBind();
                ddlUserRoleGroup.Items.Insert(0, (new ListItem("--Select--", "0")));
                ddlUsers.ClearSelection();
                ddlUsers.Items.Clear();
                ddlUsers.DataSource = null;
                ddlUsers.DataBind();
                ddlUsers.Items.Insert(0, (new ListItem("--Select--", "0")));
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgCreateUser: ClearContoles(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Textbox Events
        protected void txtUserName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (hidIsAllowToValidateText.Value == "1" && !string.IsNullOrEmpty(txtUserName.Text))
                {
                    string _appval = string.Empty;
                    string _comval = string.Empty;
                    string _createdUser = string.Empty;
                    _createdUser = txtUserName.Text;
                    _comval = "Valid";

                    if (txtUserName.Text.Length > 30 || txtUserName.Text.Length < 6)
                    {
                        lblusernameError.Text = "Username must contain minimum 6 and maximum 30 characters."; return;
                    }
                    else
                    {
                        lblusernameError.Text = "";
                    }
                    foreach (var s in txtUserName.Text)
                    {
                        lblusernameError.Text = "";
                        var zxc = s.ToString();
                        _appval = _appval + zxc;
                        if (System.Text.RegularExpressions.Regex.IsMatch(_appval, "test", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                            _comval = "Unvalid";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Generic user not valid.','Create User');</script>", false);
                            txtUserName.Text = "";
                            txtUserName.Focus(); return;
                        }
                    }
                    txtPassword.Focus();
                }
                hidIsAllowToValidateText.Value = "1";
                lblusernameError.Text = "";
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgCreateUser: txtUserName_TextChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Something went wrong. Try again', 'Warning');</script>", false);
                return;
            }
        }
        #endregion

        #region Dropdown Events
        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _UserManagement._ParentID = ddlRole.SelectedValue.ToString();

                DataSet ds = _UserManagement.BindUserBasedRole();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlUserRoleGroup.DataSource = ds.Tables[0];
                    ddlUserRoleGroup.DataTextField = "RoleName";
                    ddlUserRoleGroup.DataValueField = "RoleId";
                    ddlUserRoleGroup.DataBind();
                    ddlUserRoleGroup.Items.Insert(0, (new ListItem("--Select--", "0")));
                }
                switch (ddlRole.SelectedValue.ToString())
                {
                    case "1":  // Need to Check -- Added 29-12-2022
                        fillClient();
                        break;
                    case "2":
                        fillClient();
                        break;
                    case "3":
                        fillFranchise(); //fillFranchise();  Changed 29-12-2022// Need to Check
                        break;
                    default: ErrorLog.CommonTrace("Create User Request Failed With switch. "); break;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgCreateUser: ddlRole_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlRole.Items.Insert(0, (new ListItem("--Select--", "0")));
                ddlUserRoleGroup.Items.Insert(0, (new ListItem("--Select--", "0")));
                ddlUsers.Items.Insert(0, (new ListItem("--Select--", "0")));
                ddlUsers.ClearSelection();
                BindDropdownRoles();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgCreateUser: ddlClient_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Bind Client
        public void FillBc()
        {
            try
            {
                ddlClient.Items.Clear();
                ddlClient.DataSource = null;
                ddlClient.DataBind();
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
                        ddlClient.DataSource = dsbc;
                        ddlClient.DataValueField = "BCCode";
                        ddlClient.DataTextField = "BCName";
                        ddlClient.DataBind();
                    }
                    else
                    {
                        ddlClient.DataSource = dsbc;
                        ddlClient.DataValueField = "BCCode";
                        ddlClient.DataTextField = "BCName";
                        ddlClient.DataBind();
                        ddlClient.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlClient.DataSource = null;
                    ddlClient.DataBind();
                    ddlClient.Items.Insert(0, new ListItem("No Data Found", "0"));
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
    }
}