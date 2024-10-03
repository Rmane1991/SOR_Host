using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessAccessLayer;
using System.Data;
using System.Configuration;
using System.Threading;
using AppLogger;
using System.IO;

namespace SOR.Pages.Admin
{
    public partial class CreateRoles : System.Web.UI.Page
    {
        #region Declaration
        UserManagement _UserManagement = new UserManagement();
        public clsCustomeRegularExpressions CustomeRegularExpressions = null;
        public clsCustomeRegularExpressions _CustomeRegExpValidation
        {
            get { if (CustomeRegularExpressions == null) CustomeRegularExpressions = new clsCustomeRegularExpressions(); return CustomeRegularExpressions; }
            set { CustomeRegularExpressions = value; }
        }

        ClientRegistrationEntity _clientMngnt = null;
        public ClientRegistrationEntity clientMngnt
        {
            get { if (_clientMngnt == null) _clientMngnt = new ClientRegistrationEntity(); return _clientMngnt; }
            set { _clientMngnt = value; }
        }

        public ExportFormat _exportFormat = null;
        public ExportFormat exportFormat
        {
            get { if (_exportFormat == null) _exportFormat = new ExportFormat(); return _exportFormat; }
            set { _exportFormat = value; }
        }

        string
        _ID = string.Empty,
        _Status = string.Empty,
        _Accessibility = string.Empty;

        #endregion

        #region Enum
        public enum ActionType
        {
            Update = 1,
            Insert = 2
        }
        #endregion

        #region Constructor
        public CreateRoles()
        {
            try
            {
                _tblRoleDetails = new DataTable();
                if (_tblRoleDetails.Columns.Count == 0)
                {
                    _tblRoleDetails.Columns.Add("RoleId");
                    _tblRoleDetails.Columns.Add("MenuId");
                    _tblRoleDetails.Columns.Add("SubMenuId");
                    _tblRoleDetails.Columns.Add("Accessibility");
                    _tblRoleDetails.Columns.Add("IsMenuAccessed");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Variable declaration
        DataTable _tblRoleDetails = null;

        LinkButton _LinkBtnPermissions = null;
        CheckBox
               _chkEditSubMenuOnOff = null,
            _chkEditMainMenuOnOff = null;
        Label
            _lblSub_SubMenuName = null,
            _lblSub_MenuID = null,
            _lblSub_SubMenuID = null,
            _lblSub_Accessibility = null,
            _lblMenuName = null,
            _lblMenuID = null;

        int?
          _IntchkEditMainMenuOnOff = null,
          _IntchkEditSubMenuOnOff = null,
          _IntlblSub_MenuID = null,
          _IntlblSub_SubMenuID = null,
          _IntlblMenuID = null;

        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    //    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "CreateRoles.aspx", "182");
                    //    if (!HasPagePermission)
                    //    {
                    //        try
                    //        {
                    //            Response.Redirect(ConfigurationManager.AppSettings["RedirectTo404"].ToString(), false);
                    //            HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //        }
                    //        catch (ThreadAbortException)
                    //        {
                    //        }
                    //    }
                    //    else
                    //    {
                    //        UserPermissions.RegisterStartupScriptForNavigationListActive("16", "182");
                    if (!IsPostBack) //&& HasPagePermission
                    {
                        
                            //BindDropdownsRoles();
                            BindDropdownRolesFilter();
                            //BindDropdownClientDetails();
                            fillRoleDetailsGrid();
                        }
                    //}
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
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: Page_Load: Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        #endregion

        #region Methods
        //public void BindDropdownsRoles()
        //{
        //    try
        //    {
        //        _UserManagement._RoleId = Session["UserRoleID"].ToString();
        //        _UserManagement.UserName = Session["Username"].ToString();
        //        DataSet dsUserRoles = _UserManagement.BindRolesAndRoleGroup();

        //        if (dsUserRoles != null && dsUserRoles.Tables.Count > 0 && dsUserRoles.Tables[0].Rows.Count > 0)
        //        {
        //            ddluserRoleGroup.Items.Clear();
        //            ddluserRoleGroup.DataSource = dsUserRoles.Tables[0];
        //            ddluserRoleGroup.DataTextField = "RoleName";
        //            ddluserRoleGroup.DataValueField = "RoleId";
        //            ddluserRoleGroup.DataBind();
        //            ddluserRoleGroup.Items.Insert(0, new ListItem("-- Select --", "0"));
        //        }
        //        else
        //        {
        //            ddluserRoleGroup.Items.Clear();
        //            ddluserRoleGroup.DataSource = null;
        //            ddluserRoleGroup.DataBind();
        //            ddluserRoleGroup.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));
        //        }
        //        if (dsUserRoles != null && dsUserRoles.Tables.Count > 1 && dsUserRoles.Tables[1].Rows.Count > 0)
        //        {
        //            ddlRoles.Items.Clear();
        //            ddlRoles.DataSource = dsUserRoles.Tables[1];
        //            ddlRoles.DataTextField = "RoleName";
        //            ddlRoles.DataValueField = "RoleId";
        //            ddlRoles.DataBind();
        //            ddlRoles.Items.Insert(0, new ListItem("-- Select --", "0"));
        //        }
        //        else
        //        {
        //            ddlRoles.Items.Clear();
        //            ddlRoles.DataSource = null;
        //            ddlRoles.DataBind();
        //            ddlRoles.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.AgentManagementTrace("CreateRoles: BindDropdownsRoles(): Exception: " + ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}

        private void BindDropdownRolesFilter()
        {
            try
            {
                _UserManagement.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
                _UserManagement._UserName = Session["Username"].ToString();
                _UserManagement._ID = null;
                _UserManagement._RoleId = Session["UserRoleID"].ToString();
                DataSet _gvUserAccessControl = _UserManagement.FillGridCreateRoles();

                if (_gvUserAccessControl.Tables[0].Rows.Count > 0)
                {
                    ddlRoleID.Items.Clear();
                    ddlRoleID.DataSource = _gvUserAccessControl.Tables[0];
                    ddlRoleID.DataTextField = "RoleName";
                    ddlRoleID.DataValueField = "RoleId";
                    ddlRoleID.DataBind();
                    ddlRoleID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Role --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: BindDropdownRolesFilter(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        //private void BindDropdownClientDetails()
        //{
        //    DataSet _dsClient = new DataSet();
        //    try
        //    {
        //        clientMngnt.UserName = Session["Username"].ToString();
        //        clientMngnt.IsRemoved = "0";
        //        clientMngnt.IsActive = "1";
        //        clientMngnt.IsdocUploaded = "1";
        //        clientMngnt.VerificationStatus = "1";
        //        clientMngnt.SFlag = (int)EnumCollection.EnumPermissionType.EnableRoles;

        //        _dsClient = clientMngnt.BindClient();
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
        //                    Fillddl();
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
        //        ErrorLog.AgentManagementTrace("CreateRoles: BindDropdownClientDetails(): Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}

        //public void fillEditMenuDetailsGrid()
        //{
        //    try
        //    {
        //        string message = string.Empty;

        //        _UserManagement.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
        //        _UserManagement._UserName = Session["Username"].ToString();
        //        _UserManagement._RoleId = ddluserRoleGroup.SelectedValue.ToString();
        //        _UserManagement._ParentID = ddlRoles.SelectedValue.ToString();
        //        _UserManagement._ClientID =ddlClient.SelectedValue=="1"?"Maximus": ddlClient.SelectedValue.ToString();
        //        DataSet _gvUserAccessControl = _UserManagement.FillMenuSubmenuGrid();

        //        if (_gvUserAccessControl != null && _gvUserAccessControl.Tables.Count > 0 && _gvUserAccessControl.Tables[0].Rows.Count > 0)
        //        {
        //            gdvEditMenu.DataSource = _gvUserAccessControl.Tables[0];
        //            gdvEditMenu.DataBind();

        //            PanelMappingRoles.Visible = true;
        //            btnEditRole.Visible = true;
        //            gdvEditMenu.Visible = true;
        //        }
        //        else
        //        {
        //            PanelMappingRoles.Visible = false;
        //            btnEditRole.Visible = false;
        //            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No mapping found.','Create Roles');", true);
        //        }
        //        return;
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("CreateRoles: fillEditMenuDetailsGrid(): Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}

        public DataSet fillRoleDetailsGrid()
        {
            DataSet _gvUserAccessControl = null;
            try
            {
                string message = string.Empty;

                _UserManagement.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
                _UserManagement._UserName = Session["Username"].ToString();
                _UserManagement._ID = null;
                _UserManagement._RoleId = Session["UserRoleID"].ToString();
                 _gvUserAccessControl = _UserManagement.FillGridCreateRoles();

                if (_gvUserAccessControl.Tables[0].Rows.Count > 0)
                {
                    gvRoleDetails.DataSource = _gvUserAccessControl.Tables[0];
                    gvRoleDetails.DataBind();
                    gvRoleDetails.Visible = true;
                    lblRecordsCount.Text = "Total " + Convert.ToString(_gvUserAccessControl.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    gvRoleDetails.Visible = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No records found.','Create Roles');", true);
                }
               
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: fillRoleDetailsGrid(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return _gvUserAccessControl;
        }

        //private void PrepareChangesToUpdate()
        //{
        //    try
        //    {
        //        foreach (GridViewRow _rowEditMainMenu in gdvEditMenu.Rows)
        //        {
        //            if (_rowEditMainMenu.RowType == DataControlRowType.DataRow)
        //            {
        //                GridView _gvEditSubMenu = (GridView)_rowEditMainMenu.FindControl("gvEditSubMenu");
        //                _lblMenuID = (Label)_rowEditMainMenu.FindControl("lblMenuID");
        //                _lblMenuName = (Label)_rowEditMainMenu.FindControl("lblMenuName");
        //                _chkEditMainMenuOnOff = (CheckBox)_rowEditMainMenu.FindControl("chkEditMainMenuOnOff");
        //                _IntchkEditMainMenuOnOff = _chkEditMainMenuOnOff.Checked ? 1 : 0;
        //                foreach (GridViewRow _rowSubMenu in _gvEditSubMenu.Rows)
        //                {
        //                    if (_rowSubMenu.RowType == DataControlRowType.DataRow)
        //                    {
        //                        //_chkEditSubMenuOnOff = (CheckBox)_rowSubMenu.FindControl("chkEditSubMenuOnOff");
        //                        _lblSub_SubMenuName = (Label)_rowSubMenu.FindControl("lblSub_SubMenuName");
        //                        _lblSub_MenuID = (Label)_rowSubMenu.FindControl("lblSub_MenuID");
        //                        _lblSub_SubMenuID = (Label)_rowSubMenu.FindControl("lblSub_SubMenuID");
        //                        _lblSub_Accessibility = (Label)_rowSubMenu.FindControl("lblSub_Accessibility");
        //                        _LinkBtnPermissions = (LinkButton)_rowSubMenu.FindControl("LinkBtnPermissions");

        //                        // _IntchkEditSubMenuOnOff = _chkEditSubMenuOnOff.Checked ? 1 : 0;
        //                        if (_chkEditMainMenuOnOff.Checked)
        //                            _IntchkEditSubMenuOnOff = _LinkBtnPermissions.Text == "Revoke" ? 1 : 0;
        //                        else
        //                            _IntchkEditSubMenuOnOff = 0;

        //                        _IntlblSub_SubMenuID = !string.IsNullOrEmpty(_lblSub_SubMenuID.Text) ? Convert.ToInt32(_lblSub_SubMenuID.Text) : 0;
        //                        _IntlblSub_MenuID = !string.IsNullOrEmpty(_lblSub_MenuID.Text) ? Convert.ToInt32(_lblSub_MenuID.Text) : 0;
        //                        _IntlblMenuID = !string.IsNullOrEmpty(_lblMenuID.Text) ? Convert.ToInt32(_lblMenuID.Text) : 0;

        //                        DataRow _newRow = _tblRoleDetails.NewRow();
        //                        _newRow["RoleId"] = ddluserRoleGroup.Text;
        //                        _newRow["MenuId"] = _IntlblMenuID;
        //                        _newRow["SubMenuId"] = _IntlblSub_SubMenuID;
        //                        _newRow["Accessibility"] = _IntchkEditSubMenuOnOff;
        //                        _newRow["IsMenuAccessed"] = _IntchkEditMainMenuOnOff;
        //                        _tblRoleDetails.Rows.Add(_newRow);
        //                    }
        //                }
        //            }
        //        }
        //        ViewState["tblRoleDetails"] = _tblRoleDetails;
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("CreateRoles: PrepareChangesToUpdate(): Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}

        //public void Fillddl()
        //{
        //    try
        //    {
        //        _UserManagement.UserName = ddlClient.SelectedValue == "1" ? "Maximus" : ddlClient.SelectedValue.ToString();
        //        _UserManagement.Flag = 2;
        //        DataSet dsUserRoles = _UserManagement.BindRolesAndRoleGroup();

        //        if (dsUserRoles != null && dsUserRoles.Tables.Count > 0 && dsUserRoles.Tables[0].Rows.Count > 0)
        //        {
        //            ddluserRoleGroup.DataSource = dsUserRoles.Tables[0];
        //            ddluserRoleGroup.DataTextField = "RoleName";
        //            ddluserRoleGroup.DataValueField = "RoleId";
        //            ddluserRoleGroup.DataBind();
        //            ddluserRoleGroup.Items.Insert(0, new ListItem("-- Select --", "0"));
        //        }
        //        else
        //        {
        //            ddluserRoleGroup.Items.Clear();
        //            ddluserRoleGroup.DataSource = null;
        //            ddluserRoleGroup.DataBind();
        //            ddluserRoleGroup.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));
        //        }
        //        if (dsUserRoles != null && dsUserRoles.Tables.Count > 1 && dsUserRoles.Tables[1].Rows.Count > 0)
        //        {
        //            ddlRoles.DataSource = dsUserRoles.Tables[1];
        //            ddlRoles.DataTextField = "RoleName";
        //            ddlRoles.DataValueField = "RoleId";
        //            ddlRoles.DataBind();
        //            ddlRoles.Items.Insert(0, new ListItem("-- Select --", "0"));
        //        }
        //        else
        //        {
        //            ddlRoles.Items.Clear();
        //            ddlRoles.DataSource = null;
        //            ddlRoles.DataBind();
        //            ddlRoles.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("CreateRoles: Fillddl(): Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}
        #endregion

        #region Button Events

        protected void btnSaveAction_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["ActionType"].ToString() == ActionType.Update.ToString())
                {
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbersCharacterSpace, txtRole.Text))
                    {
                        txtRole.Focus();
                        ModalPopupExtender_Declincard.Show();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Enter valid Role Name.', 'Edit Role');</script>", false);
                        return;
                    }
                    else _UserManagement._MenuName = txtRole.Text.Trim();
                    _UserManagement.UserName = Session["Username"].ToString();
                    _UserManagement._Role = txtRole.Text.Trim();
                    DataSet _gvTerminalModalss = _UserManagement.FillGridUserAccessManagementForAddRoleValidateRole();

                    if (_gvTerminalModalss == null || Convert.ToString(_gvTerminalModalss.Tables[0].Rows[0][0]) != "0")
                    {
                        ModalPopupExtender_Declincard.Show();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Role is already exist.','Create Roles');", true);
                        return;
                    }

                    else
                        _UserManagement.UserName = Session["Username"].ToString();
                    _UserManagement._RoleId = lblID.Text.ToString();
                    _UserManagement._MenuID = lblID.Text.ToString();
                    _UserManagement._ID = lblID.Text.ToString();
                    _UserManagement._Role = txtRole.Text.ToString();
                    _UserManagement.Flag = 4;// Insert Role & Page Permissions.
                    DataSet _dsRoles = _UserManagement.InsertNewRole();

                    if (_dsRoles != null || Convert.ToString(_dsRoles.Tables[0].Rows[0][0]) == "SUCCESS")
                    {
                        txtRole.Text = "";
                        fillRoleDetailsGrid();
                        //BindDropdownsRoles();
                        BindDropdownRolesFilter();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + txtRole.Text + " Record updated successfully.', 'Edit Role Details');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable to add new Role. Please Try Again.', 'Add Role');", true);
                    }
                }
                else if (ViewState["ActionType"].ToString() == Convert.ToString((int)EnumCollection.EnumDBOperationTypeOverall.Terminate))
                {
                    if (!string.IsNullOrEmpty(txtResone.Text.Trim()))
                    {
                        _UserManagement.Flag = (int)EnumCollection.EnumDBOperationTypeOverall.Terminate;
                        _UserManagement._UserName = Session["Username"].ToString();
                        _UserManagement._ID = lblID.Text.ToString();
                        _UserManagement._RoleId = Session["UserRoleID"].ToString();
                        _UserManagement.Reason = txtResone.Text.Trim();
                        DataSet _gvUserAccessControl = _UserManagement.UserAccessManagementDeleteRole();

                        if (_gvUserAccessControl != null && _gvUserAccessControl.Tables.Count > 0 && Convert.ToString(_gvUserAccessControl.Tables[0].Rows[0][1]) == "Done")
                        {
                            txtRole.Text = "";
                            txtResone.Text = "";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Role deleted successfully.', 'Create Role');", true);
                            fillRoleDetailsGrid();
                            //BindDropdownsRoles();
                            BindDropdownRolesFilter();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable to deleted Role. Please Try Again.', 'Delete Role');", true);
                        }
                    }
                    else
                    {
                        txtRole.Text = "";
                        txtResone.Text = "";
                        ModalPopupExtender_Declincard.Show();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter the reason.', 'Create Role');", true);
                        return;
                    }
                }
                else
                {

                }

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: btnSaveAction_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnCancelAction_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtResone.Text = "";
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: btnCancelAction_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void lbtnNewRole_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int minlength = 2;
                
                if (txtRolename.Text.Length < minlength)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Minimum 2 characters required..', 'Insert Role');", true);
                    return;
                }

                ViewState["ActionType"] = ActionType.Insert.ToString();
                lblID.Text = Session["UserRoleID"].ToString();
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbersCharacterSpace, txtRolename.Text))
                {
                    txtRolename.Focus();
                    ModalPopupExtender_EditRole.Show();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Enter valid Role Name.', 'Insert Role');", true);
                    return;
                }
                else
                    _UserManagement._Role = txtRolename.Text.Trim();
                DataSet _gvTerminalModalss = _UserManagement.FillGridUserAccessManagementForAddRoleValidateRole();

                if (_gvTerminalModalss == null || Convert.ToString(_gvTerminalModalss.Tables[0].Rows[0][0]) != "0")
                {
                    ModalPopupExtender_EditRole.Show();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Role is already exist.', 'New User');", true);
                    return;
                }
                if (ViewState["ActionType"].ToString() == ActionType.Insert.ToString())
                {
                    _UserManagement.UserName = Session["Username"].ToString();
                    _UserManagement._RoleId = lblID.Text.ToString();
                    _UserManagement._MenuID = lblID.Text.ToString();
                    _UserManagement._ID = lblID.Text.ToString();
                    _UserManagement._Role = txtRolename.Text.ToString();
                    _UserManagement.Flag = 3;// Insert Role & Page Permissions.
                    DataSet _dsRoles = _UserManagement.InsertNewRole();

                    if (_dsRoles != null || Convert.ToString(_dsRoles.Tables[0].Rows[0][0]) == "SUCCESS")
                    {
                        txtRolename.Text = string.Empty;
                        //BindDropdownsRoles();
                        fillRoleDetailsGrid();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + txtRolename.Text + " Role added successfully.', 'Edit Role Details');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable to add new Role. Please Try Again.', 'Add Role');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact Systeam administartor.', ' Edit Role');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: lbtnNewRole_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        //protected void btnEditRole_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if(ddlClient.SelectedValue=="0")
        //        {
        //            TabName.Value = "MapRole";
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);

        //            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please select client.', ' Edit Role');", true);
        //            return;
        //        }
        //        else  if (ddluserRoleGroup.SelectedValue != "0" && ddlRoles.SelectedValue.ToString() != "0")
        //        {
        //            PrepareChangesToUpdate();
        //            ViewState["ActionType"] = ActionType.Insert.ToString();
        //            DataTable _DataTable = (DataTable)ViewState["tblRoleDetails"];

        //            if (_DataTable != null)
        //            {
        //                if (ViewState["ActionType"].ToString() == ActionType.Insert.ToString())
        //                {
        //                    _UserManagement.UserName = Session["Username"].ToString();
        //                    _UserManagement.dataTable = _DataTable;
        //                    // _UserManagement._RoleId = lblID.Text.ToString();
        //                    _UserManagement._MenuID = lblID.Text.ToString();
        //                    _UserManagement._ID = lblID.Text.ToString();
        //                    _UserManagement._RoleId = ddluserRoleGroup.Text.ToString();
        //                    _UserManagement.Flag = 1;
        //                    _UserManagement._ParentID = ddlRoles.SelectedValue.ToString();
        //                    _UserManagement._ClientID = ddlClient.SelectedValue=="1"?"Maximus":ddlClient.SelectedValue.ToString();// Insert Role & Page Permissions.
        //                    DataSet _dsRoles = _UserManagement.InsertUpdateRoleDetails();

        //                    if (_dsRoles != null || Convert.ToString(_dsRoles.Tables[0].Rows[0][0]) == "SUCCESS")
        //                    {
        //                        ddluserRoleGroup.SelectedIndex = 0;
        //                        ddlRoles.SelectedIndex = 0;
        //                        gdvEditMenu.Visible = false;
        //                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Role added successfully.', 'Edit Role Details');", true);
        //                    }
        //                    else
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable to add new Role. Please Try Again.', 'Add Role');", true);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                TabName.Value = "MapRole";
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        //                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact Systeam administartor.', ' Edit Role');", true);
        //                return;
        //            }
        //            TabName.Value = "MapRole";
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select User Role Group and Role.', 'Manage Roles');", true);
        //        }
        //        TabName.Value = "MapRole";
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("CreateRoles: btnEditRole_Click: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}

        //protected void btnEditClose_ServerClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        TabName.Value = "MapRole";
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("CreateRoles: btnEditClose_ServerClick: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}

        protected void LinkBtnPermissions_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton _linkBtn = (LinkButton)sender;
                GridViewRow row = (GridViewRow)_linkBtn.Parent.Parent;
                _lblMenuID = (Label)row.FindControl("lblSub_MenuID");
                _lblSub_SubMenuID = (Label)row.FindControl("lblSub_SubMenuID");
                string MenuID = _lblMenuID != null ? _lblMenuID.Text : null;
                if (!string.IsNullOrEmpty(_linkBtn.Text))
                {
                    _linkBtn.Text = _linkBtn.Text == "Grant" ? "Revoke" : "Grant";
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>ExpandCollapse('div" + MenuID + "', ' Edit Role');</script>", false);
                _linkBtn.Focus();
                TabName.Value = "MapRole";
                //UpdatePanel3.Update();  
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: LinkBtnPermissions_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        #endregion

        #region Gridview Events
        protected void gvEditSubMenu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grid = (GridView)sender;
                    _Accessibility = grid.DataKeys[e.Row.RowIndex].Values[3].ToString();
                    LinkButton linkBtn = (LinkButton)e.Row.FindControl("LinkBtnPermissions");
                    if (!string.IsNullOrEmpty(_Accessibility))
                    {
                        linkBtn.Text = _Accessibility == "1" ? "Revoke" : "Grant";
                        TabName.Value = "MapRole";
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    }
                    //LinkButton lb = e.Row.FindControl("LinkBtnPermissions") as LinkButton;
                    //ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lb);  
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: gvEditSubMenu_RowDataBound: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        //protected void gdvEditMenu_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            GridView grid = (GridView)sender;
        //            _ID = grid.DataKeys[e.Row.RowIndex].Values[1].ToString();
        //            _Accessibility = grid.DataKeys[e.Row.RowIndex].Values[2].ToString();
        //            //if (_Accessibility == "1")
        //            //{
        //            CheckBox _chkEditMainMenuOnOff = (CheckBox)e.Row.FindControl("chkEditMainMenuOnOff");
        //              _chkEditMainMenuOnOff.Checked = Convert.ToBoolean(Convert.ToInt32(_Accessibility));
        //            GridView _gvEditSubMenu = (GridView)e.Row.FindControl("gvEditSubMenu");
        //            LinkButton linkBtn = (LinkButton)e.Row.FindControl("LinkBtnPermissions");
        //            _UserManagement.UserName = Session["Username"].ToString();
        //            _UserManagement._RoleId = ddluserRoleGroup.SelectedValue.ToString();
        //            _UserManagement._MenuID = _ID;
        //            _UserManagement._ParentID = ddlRoles.SelectedValue.ToString();
        //            _UserManagement._ClientID =ddlClient.SelectedValue=="1"?"Maximus": ddlClient.SelectedValue.ToString();
        //            DataSet _dsgvEditSubMenu = _UserManagement.FillGridUserAccessManagementModalPopupSubMenu();

        //            GridView gvEditSubMenu = e.Row.FindControl("gvEditSubMenu") as GridView;
        //            if (_dsgvEditSubMenu != null && _dsgvEditSubMenu.Tables.Count > 0)
        //            {
        //                if (_dsgvEditSubMenu.Tables != null)
        //                {
        //                    gvEditSubMenu.DataSource = _dsgvEditSubMenu.Tables[0];
        //                    gvEditSubMenu.DataBind();

        //                    if (_dsgvEditSubMenu.Tables[0].Rows.Count > 0)
        //                    {
        //                        _chkEditMainMenuOnOff.Checked = true;
        //                    }
        //                    else  
        //                    {
        //                        _chkEditMainMenuOnOff.Checked = false;
        //                    }
        //                }
        //                else
        //                {
        //                    _chkEditMainMenuOnOff.Checked = false;
        //                }
        //            }
        //            else
        //            {
        //                _chkEditMainMenuOnOff.Checked = false;
        //            }
        //            TabName.Value = "MapRole";
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("CreateRoles: gdvEditMenu_RowDataBound: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}

        protected void gvRoleDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton _lbtbAction = (ImageButton)e.CommandSource;
                GridViewRow myGrid = (GridViewRow)_lbtbAction.Parent.Parent;

                lblID.Text = Convert.ToString(gvRoleDetails.DataKeys[myGrid.RowIndex].Values["RoleId"]);
                txtRole.Text = Convert.ToString(gvRoleDetails.DataKeys[myGrid.RowIndex].Values["RoleName"]);

                switch (e.CommandName.ToString().Trim())
                {
                    case "EditRole":
                        ViewState["ActionType"] = ActionType.Update.ToString();
                        lblModalHeaderName.Text = "Update Role Name";
                        showrolename.Visible = true;
                        DivEnterReason.Visible = false;
                        //    lblconfirm.Text = "Are you sure want to Update?";
                        btnupdate.Visible = true;
                        btnSaveAction.Visible = false;
                        ModalPopupExtender_Declincard.Show();
                        break;

                    case "DeleteRole":
                        _UserManagement.Flag = (int)EnumCollection.EnumDBOperationTypeOverall.Terminate;
                        ViewState["ActionType"] = (int)EnumCollection.EnumDBOperationTypeOverall.Terminate;
                        lblModalHeaderName.Text = "Delete Reason";
                        //  TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                        lblconfirm.Text = "Are you sure want to Delete " + txtRole.Text + "  ?";
                        showrolename.Visible = false;
                        DivEnterReason.Visible = true;
                        btnupdate.Visible = false;
                        btnSaveAction.Visible = true;
                        ModalPopupExtender_Declincard.Show();
                        break;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: gvRoleDetails_RowCommand: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void gvRoleDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRoleDetails.PageIndex = e.NewPageIndex;
                fillRoleDetailsGrid();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: gvRoleDetails_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        #endregion

        #region Dropdown Events
        //protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (ddlRoles.SelectedValue != "0")
        //        {
        //            fillEditMenuDetailsGrid();
        //        }
        //        else
        //        {
        //            PanelMappingRoles.Visible = false;
        //            btnEditRole.Visible = false;
        //            //btnEditClose.Visible = false;
        //        }
        //        TabName.Value = "MapRole";
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("CreateRoles: ddlRoles_SelectedIndexChanged: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}

        //protected void ddluserRoleGroup_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ddlRoles.SelectedValue = "0";
        //        gdvEditMenu.Visible = false;
        //        TabName.Value = "MapRole";
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("CreateRoles: ddluserRoleGroup_SelectedIndexChanged: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}

        protected void ddlRoleID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlRoleID.SelectedValue != "0")
                {
                    _UserManagement._RoleId = ddlRoleID.SelectedValue.ToString();
                    _UserManagement.Flag = 1;
                    DataSet _dsgvEditSubMenu = _UserManagement.FillGridAddRole();

                    if (_dsgvEditSubMenu != null && _dsgvEditSubMenu.Tables.Count > 0)
                    {
                        gvRoleDetails.DataSource = _dsgvEditSubMenu.Tables[1];
                        gvRoleDetails.DataBind();
                        gvRoleDetails.Visible = true;
                        lblRecordsCount.Text = "Total " + Convert.ToString(_dsgvEditSubMenu.Tables[1].Rows.Count) + " Record(s) Found.";
                    }
                    else
                    {
                        gvRoleDetails.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No records found.', 'Create Roles');", true);
                    }
                    return;
                }
                else
                {
                    fillRoleDetailsGrid();
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: ddlRoleID_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        //protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Fillddl();
        //        gdvEditMenu.Visible = false;
        //        TabName.Value = "MapRole";
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);

        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("CreateRoles: ddlClient_SelectedIndexChanged: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        return;
        //    }
        //}
        #endregion

        #region Checkbox Events
        protected void chkEditMainMenuOnOff_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                GridViewRow gr = (GridViewRow)chk.Parent.Parent;
                CheckBox _chkEditMainMenuOnOff = (CheckBox)gr.FindControl("chkEditMainMenuOnOff");
                _chkEditSubMenuOnOff = null;
                GridView _gvEditSubMenu = (GridView)gr.FindControl("gvEditSubMenu");
                foreach (GridViewRow _rowSubMenu in _gvEditSubMenu.Rows)
                {
                    LinkButton linkBtn = (LinkButton)_rowSubMenu.FindControl("LinkBtnPermissions");
                    linkBtn.Text = _chkEditMainMenuOnOff.Checked ? "Revoke" : "Grant";
                }
                TabName.Value = "MapRole";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: chkEditMainMenuOnOff_CheckedChanged: Exception: " + Ex.Message);
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
                DataSet dt = fillRoleDetailsGrid();
                   // FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "Create Roles Report", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: BtnCsv_Click: Exception: " + Ex.Message);
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
                DataSet dt = fillRoleDetailsGrid();

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "Create Roles Report", dt);
                }
                {
                    //lblRecordCount.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);

                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: BtnXls_Click: Exception: " + Ex.Message);
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
                FileName = @"" + "Roles" + ".XLS";
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
                ErrorLog.AgentManagementTrace("CreateRoles: GenerateToExcel: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("CreateRoles: SetPageFiltersExport: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return pageFilters;
        }
        #endregion


        #region ExportCSV
        protected void btnExportCSV_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string message = string.Empty;
                DataSet _gvUserAccessControl = new DataSet();
                if (ddlRoleID.SelectedValue != "0")
                {
                    _UserManagement._RoleId = ddlRoleID.SelectedValue.ToString();
                    _UserManagement.Flag = 4;
                    _gvUserAccessControl = _UserManagement.FillGridAddRole();
                    if (_gvUserAccessControl != null)
                    {
                        if (_gvUserAccessControl.Tables[0].Rows.Count > 0)
                        {
                            exportFormat.ExportInCSV(Session["Username"].ToString(), "Payrakam", "Create Roles Report", _gvUserAccessControl);
                        }
                    }

                }
                else
                {
                    _UserManagement.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
                    _UserManagement._UserName = Session["Username"].ToString();
                    _UserManagement._ID = null;
                    _UserManagement._RoleId = Session["UserRoleID"].ToString();
                    _gvUserAccessControl = _UserManagement.FillGridCreateRoles();

                    if (_gvUserAccessControl != null)
                    {
                        if (_gvUserAccessControl.Tables.Count > 0)
                        {
                            exportFormat.ExportInCSV(Session["Username"].ToString(), "Payrakam", "Create Roles Report", _gvUserAccessControl);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: btnExportCSV_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region ExportExcel
        protected void btnExportXLS_ServerClick(object sender, EventArgs e)
        {
            try
            {
                 DataSet _gvUserAccessControl = new DataSet();
                 if (ddlRoleID.SelectedValue != "0")
                 {
                     _UserManagement._RoleId = ddlRoleID.SelectedValue.ToString();
                     _UserManagement.Flag = 4;
                     _gvUserAccessControl = _UserManagement.FillGridAddRole();
                     if (_gvUserAccessControl != null)
                     {
                         if (_gvUserAccessControl.Tables.Count > 0)
                         {
                             exportFormat.ExporttoExcel(Session["Username"].ToString(), "Payrakam", "Create Roles Report", _gvUserAccessControl);
                         }
                     }

                 }
                 else
                 {
                     _UserManagement.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
                     _UserManagement._UserName = Session["Username"].ToString();
                     _UserManagement._ID = null;
                     _UserManagement._RoleId = Session["UserRoleID"].ToString();
                     _gvUserAccessControl = _UserManagement.FillGridCreateRoles();
                     if (_gvUserAccessControl != null)
                     {
                         if (_gvUserAccessControl.Tables.Count > 0)
                         {
                             exportFormat.ExporttoExcel(Session["Username"].ToString(), "Payrakam", "Create Roles Report", _gvUserAccessControl);
                         }
                     }
                 }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: btnExportXLS_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Add New Role

        protected void lbtnAddRole_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtRolename.Text = string.Empty;
                ModalPopupExtender_EditRole.Show();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: lbtnAddRole_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region close
        protected void btnclose_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtRolename.Text = "";
                ModalPopupExtender_EditRole.Hide();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("CreateRoles: lbtnAddRole_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion
    }
}