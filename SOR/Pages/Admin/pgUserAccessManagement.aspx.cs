
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppLogger;
using BussinessAccessLayer;
//using DataAccessLayer;
using System.Configuration;
using System.Threading;
using System.IO;

namespace SOR.Pages.Admin
{
    public partial class pgUserAccessManagement : System.Web.UI.Page
    {
        #region Variables and Objects
        public UserManagement user = null;
        public UserManagement _UserManagement
        {
            get { if (user == null) user = new UserManagement(); return user; }
            set { user = value; }
        }
        public ExportFormat _exportFormat = null;
        public ExportFormat exportFormat
        {
            get { if (_exportFormat == null) _exportFormat = new ExportFormat(); return _exportFormat; }
            set { _exportFormat = value; }
        }

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

        DataSet _gvUserAccessControl = null;
        string
           _ID = string.Empty,
         _Status = string.Empty,
        _Accessibility = string.Empty;

        DataTable _tblRoleDetails = null;

        LinkButton _LinkBtnPermissions = null;
        CheckBox
            _chkEditMainMenuOnOff = null,
            _chkEditSubMenuOnOff = null;
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
            _IntlblSub_SubMenuName = null,
            _IntlblSub_MenuID = null,
            _IntlblSub_SubMenuID = null,
            _IntlblSub_Accessibility = null,
            _Intlbl_Accessibility = null,
            _IntlblMenuName = null,
            _IntlblMenuID = null;

        public enum ActionType
        {
            Update = 1,
            Insert = 2
        }
        #endregion

        #region Constructor
        public pgUserAccessManagement()
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

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "pgUserAccessManagement.aspx", "5");
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
                        if (!IsPostBack && HasPagePermission)
                        {
                            UserPermissions.RegisterStartupScriptForNavigationListActive("2", "5");
                            BindDropdownClientDetails();
                            fillRoleDetailsGrid();
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
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: Page_Load: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Edit Menu Grid
        public void fillEditMenuDetailsGrid()
        {
            try
            {
                string message = string.Empty;

                _UserManagement.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
                _UserManagement._UserName = Session["Username"].ToString();
                _UserManagement._RoleId = lblID.Text.ToString();
                _UserManagement._ClientID = !string.IsNullOrEmpty(lblID.Text) && lblID.Text == "1" ? "Maximus" : ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;

                if (!string.IsNullOrEmpty(_UserManagement._ClientID))
                {
                    _gvUserAccessControl = _UserManagement.FillGridUserAccessManagementModalPopupMenu(); //

                    if (_gvUserAccessControl != null && _gvUserAccessControl.Tables[0].Rows.Count > 0)
                    {
                        gdvEditMenu.DataSource = _gvUserAccessControl.Tables[0];
                        gdvEditMenu.DataBind();
                        gdvEditMenu.Visible = true;
                    }
                    else
                    {
                        gdvEditMenu.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No records found.');", true);
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: fillEditMenuDetailsGrid: Exception: " + Ex.Message);
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
                            ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

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
                        ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));
                    }
                }
                else
                {
                    ddlClient.Items.Clear();
                    ddlClient.DataSource = null;
                    ddlClient.DataBind();
                    ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));


                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: BindDropdownClientDetails: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Fill Role Details
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
                _gvUserAccessControl = _UserManagement.FillGridUserAccessManagement();

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
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No records found.');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: fillRoleDetailsGrid: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return _gvUserAccessControl;
            }
            return _gvUserAccessControl;
        }
        #endregion

        #region Processing Add New Role

        // Add Role - Button Event To Open Modal
        protected void lbtnNewRole_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ViewState["ActionType"] = ActionType.Insert.ToString();
                //fillMenuDetailsGrid(); // fillMenuDetailsGrid();
                //ModalPopupExtender_InsertRole.Show();
                lbltitlename.Text = "Add New Role";
                lblName.Text = Session["Username"].ToString();
                lblID.Text = Session["UserRoleID"].ToString();
                // txtEditRoleName.Text = Session["Username"].ToString();
                fillEditMenuDetailsGrid();
                btnsubmit.Visible = true;
                btnEditRole.Visible = false;
                ModalPopupExtender_EditRole.Show();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: lbtnNewRole_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        // Add Role - Menu bind.
        public void fillMenuDetailsGrid()
        {
            try
            {
                //_UserManagement.UserName = Session["Username"].ToString();
                //_gvUserAccessControl = _UserManagement.FillGridUserAccessManagementForAddRoleMenu();
                _UserManagement.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
                _UserManagement._UserName = Session["Username"].ToString();
                _UserManagement._RoleId = lblID.Text.ToString();
                _gvUserAccessControl = _UserManagement.FillGridUserAccessManagementModalPopupMenu();

                if (_gvUserAccessControl.Tables[0].Rows.Count > 0)
                {
                    gvMenuAccess.DataSource = _gvUserAccessControl.Tables[0];
                    gvMenuAccess.DataBind();
                    gvMenuAccess.Visible = true;
                }
                else
                {
                    gvMenuAccess.Visible = false;
                }
                return;
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: fillMenuDetailsGrid: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        // Add Role - Sub Menu bind By Menu ID.
        protected void gvMenuAccess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grid = (GridView)sender;
                    _ID = grid.DataKeys[e.Row.RowIndex].Values[1].ToString();

                    CheckBox _chkMainMenuOnOff = (CheckBox)e.Row.FindControl("chkMainMenuOnOff");
                    GridView _gvSubMenu = (GridView)e.Row.FindControl("gvSubMenu");
                    //_UserManagement.UserName = Session["Username"].ToString();
                    //_UserManagement._MenuID = _ID;
                    //DataSet _dsgvSubMenu = _UserManagement.FillGridUserAccessManagementForAddRoleSubMenu();
                    _UserManagement.UserName = Session["Username"].ToString();
                    _UserManagement._RoleId = lblID.Text.ToString();
                    _UserManagement._MenuID = _ID;
                    DataSet _dsgvSubMenu = _UserManagement.FillGridUserAccessManagementModalPopupSubMenu();

                    GridView gvSubMenu = e.Row.FindControl("gvSubMenu") as GridView;
                    if (_dsgvSubMenu != null)
                    {
                        if (_dsgvSubMenu.Tables != null)
                        {
                            gvSubMenu.DataSource = _dsgvSubMenu.Tables[0];
                            gvSubMenu.DataBind();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: gvMenuAccess_RowDataBound: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        // Add Role - Save Data.
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable _dtMenuSubMenudata = new DataTable();
                CheckBox _chkOnOff = null;
                GridView _gvSubMenu = null;

                _dtMenuSubMenudata.Columns.Add("MenuId", typeof(int));
                _dtMenuSubMenudata.Columns.Add("SubMenuId", typeof(int));
                _dtMenuSubMenudata.Columns.Add("BitIsRemoved", typeof(bool));

                foreach (GridViewRow _rowMainMenu in gvMenuAccess.Rows)
                {
                    if (_rowMainMenu.RowType == DataControlRowType.DataRow)
                    {
                        _gvSubMenu = (GridView)_rowMainMenu.FindControl("gvSubMenu");

                        if (_gvSubMenu.Rows.Count > 0)
                        {
                            foreach (GridViewRow _rowSubMenu in _gvSubMenu.Rows)
                            {
                                if (_rowSubMenu.RowType == DataControlRowType.DataRow)
                                {
                                    _chkOnOff = (CheckBox)_rowSubMenu.FindControl("chkSubMenuOnOff"); //---- Accessability

                                    _dtMenuSubMenudata.Rows.Add(
                                        Convert.ToString(_gvSubMenu.DataKeys[_rowSubMenu.RowIndex].Values[2]).Equals(string.Empty) ? 0 : Convert.ToInt32(Convert.ToString(_gvSubMenu.DataKeys[_rowSubMenu.RowIndex].Values[2])), //---- Main menu id
                                        Convert.ToString(_gvSubMenu.DataKeys[_rowSubMenu.RowIndex].Values[1]).Equals(string.Empty) ? 0 : Convert.ToInt32(Convert.ToString(_gvSubMenu.DataKeys[_rowSubMenu.RowIndex].Values[1])), //---- Sub menu id
                                        _chkOnOff == null ? false : _chkOnOff.Checked ? true : false
                                        );
                                }
                            }
                        }
                        else
                        {
                            _chkOnOff = (CheckBox)_rowMainMenu.FindControl("chkMainMenuOnOff"); //---- Accessability

                            _dtMenuSubMenudata.Rows.Add(
                                Convert.ToString(gvMenuAccess.DataKeys[_rowMainMenu.RowIndex].Values[1]).Equals(string.Empty) ? 0 : Convert.ToInt32(Convert.ToString(gvMenuAccess.DataKeys[_rowMainMenu.RowIndex].Values[1])), //---- Main menu id
                                0, //---- Sub menu id
                                _chkOnOff == null ? false : _chkOnOff.Checked ? true : false
                             );
                        }
                    }
                }

                _UserManagement.UserName = Session["Username"].ToString();
                _UserManagement._Role = txtRole.Text.Trim();
                DataSet _gvTerminalModalss = _UserManagement.FillGridUserAccessManagementForAddRoleValidateRole();

                if (_gvTerminalModalss == null || Convert.ToString(_gvTerminalModalss.Tables[0].Rows[0][0]) != "0")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Role is already exist.', 'New User');", true);
                    return;
                }
                else
                {
                    _UserManagement.UserName = Session["Username"].ToString();
                    _UserManagement._Role = txtRole.Text.Trim();
                    _UserManagement.dataTable = _dtMenuSubMenudata;
                    _UserManagement._RoleId = null;
                    DataSet _dsRoles = _UserManagement.FillGridUserAccessManagementForAddRoleInsertRole();
                    if (_dsRoles != null || Convert.ToString(_dsRoles.Tables[0].Rows[0][0]) != "0")
                    {
                        txtRole.Text = "";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Role saved successfully.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable to add new Role. Please Try Again.', 'Add Role');", true);
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: btnSave_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        #endregion

        #region ON OFF Button
        protected void chkMainMenuOnOff_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                GridViewRow gr = (GridViewRow)chk.Parent.Parent;
                CheckBox _chkMainMenuOnOff = (CheckBox)gr.FindControl("chkMainMenuOnOff");
                GridView _gvSubMenu = (GridView)gr.FindControl("gvSubMenu");

                if (!_chkMainMenuOnOff.Checked)
                {
                    if (_gvSubMenu.Rows.Count > 0)
                    {
                        foreach (GridViewRow _rowSubMenu in _gvSubMenu.Rows)
                        {
                            CheckBox _chkSubMenuOnOff = (CheckBox)_rowSubMenu.FindControl("chkSubMenuOnOff");
                            _chkSubMenuOnOff.Checked = false;
                        }
                    }
                }
                else
                {
                    if (_gvSubMenu.Rows.Count > 0)
                    {
                        foreach (GridViewRow _rowSubMenu in _gvSubMenu.Rows)
                        {
                            CheckBox _chkSubMenuOnOff = (CheckBox)_rowSubMenu.FindControl("chkSubMenuOnOff");
                            _chkSubMenuOnOff.Checked = true;
                        }
                    }
                }
                ModalPopupExtender_InsertRole.Show();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: chkMainMenuOnOff_CheckedChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Grid Events
        protected void gvRoleDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRoleDetails.PageIndex = e.NewPageIndex;
                fillRoleDetailsGrid();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: gvRoleDetails_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void gvRoleDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton _lbtbAction = (ImageButton)e.CommandSource;
                GridViewRow myGrid = (GridViewRow)_lbtbAction.Parent.Parent;

                lblName.Text = Convert.ToString(gvRoleDetails.DataKeys[myGrid.RowIndex].Values["RoleName"]);
                lblID.Text = Convert.ToString(gvRoleDetails.DataKeys[myGrid.RowIndex].Values["RoleId"]);

                switch (e.CommandName.ToString().Trim())
                {
                    case "EditRole":
                        ViewState["ActionType"] = ActionType.Update.ToString();
                        lbltitlename.Text = "Edit Role";
                        lblName.Text = Convert.ToString(gvRoleDetails.DataKeys[myGrid.RowIndex].Values["RoleName"]);
                        lblID.Text = Convert.ToString(gvRoleDetails.DataKeys[myGrid.RowIndex].Values["RoleId"]);
                        txtEditRoleName.Text = Convert.ToString(gvRoleDetails.DataKeys[myGrid.RowIndex].Values["RoleName"]);
                        //fillEditMenuDetailsGrid();
                        btnsubmit.Visible = false;
                        btnEditRole.Visible = true;
                        divEditClient.Visible = !string.IsNullOrEmpty(lblID.Text) && lblID.Text == "1" ? false : true;
                        fillEditMenuDetailsGrid();
                        ModalPopupExtender_EditRole.Show();
                        break;
                    case "DeleteRole":
                        ModalPopupExtender_DeclinRole.Show();
                        break;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: gvRoleDetails_RowCommand: Exception: " + Ex.Message);
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void gdvEditMenu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grid = (GridView)sender;
                    _ID = grid.DataKeys[e.Row.RowIndex].Values[1].ToString();
                    _Accessibility = grid.DataKeys[e.Row.RowIndex].Values[2].ToString();

                    CheckBox _chkEditMainMenuOnOff = (CheckBox)e.Row.FindControl("chkEditMainMenuOnOff");
                    _chkEditMainMenuOnOff.Checked = Convert.ToBoolean(Convert.ToInt32(_Accessibility));
                    GridView _gvEditSubMenu = (GridView)e.Row.FindControl("gvEditSubMenu");

                    _UserManagement.UserName = Session["Username"].ToString();
                    _UserManagement._RoleId = lblID.Text.ToString();
                    _UserManagement._MenuID = _ID;
                    _UserManagement._ClientID = !string.IsNullOrEmpty(lblID.Text) && lblID.Text == "1" ? "Maximus" : ddlClient.SelectedValue.ToString();
                    DataSet _dsgvEditSubMenu = _UserManagement.FillGridUserAccessManagementMenu();

                    GridView gvEditSubMenu = e.Row.FindControl("gvEditSubMenu") as GridView;
                    if (_dsgvEditSubMenu != null && _dsgvEditSubMenu.Tables.Count > 0)
                    {
                        if (_dsgvEditSubMenu.Tables != null)
                        {
                            gvEditSubMenu.DataSource = _dsgvEditSubMenu.Tables[0];
                            gvEditSubMenu.DataBind();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: gdvEditMenu_RowDataBound: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

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
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: gvEditSubMenu_RowDataBound: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Button Events
        protected void btnEditRole_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbersCharacterSpace, txtEditRoleName.Text))
                {
                    ModalPopupExtender_EditRole.Show();
                    txtEditRoleName.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Enter valid Role Name.', 'Edit Role');", true);
                    return;
                }
                if (ddlClient.SelectedValue == "0" && lblID.Text != "1")
                {
                    ModalPopupExtender_EditRole.Show();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please select client');", true);
                    return;
                }
                else
                {
                    PrepareChangesToUpdate();
                    DataTable _DataTable = (DataTable)ViewState["tblRoleDetails"];

                    if (_DataTable != null)
                    {
                        if (ViewState["ActionType"].ToString() == ActionType.Update.ToString())
                        {
                            _UserManagement.UserName = Session["Username"].ToString();
                            _UserManagement.dataTable = _DataTable;
                            _UserManagement._RoleId = lblID.Text.ToString();
                            _UserManagement._MenuID = lblID.Text.ToString();
                            _UserManagement._ID = lblID.Text.ToString();
                            _UserManagement._Role = txtEditRoleName.Text.ToString();
                            _UserManagement.Flag = 1; // Update Page Permissions.
                            _UserManagement._ClientID = !string.IsNullOrEmpty(lblID.Text) && lblID.Text == "1" ? "Maximus" : ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                            DataSet _dsRoles = _UserManagement.UserAccessManagementUpdateTblRoleDetails();

                            if (_dsRoles != null && Convert.ToString(_dsRoles.Tables[0].Rows[0][0]) == "SUCCESS")
                            {
                                txtEditRoleName.Text = string.Empty;
                                fillRoleDetailsGrid();
                                ddlClient.SelectedValue = "0";
                                gdvEditMenu.Visible = false;
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + Convert.ToString(_dsRoles.Tables[0].Rows[0][0]) + "', 'Edit Role Details');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable to add new Role. Please Try Again.', 'Add Role');", true);
                            }
                        }
                        if (ViewState["ActionType"].ToString() == ActionType.Insert.ToString())
                        {
                            _UserManagement.UserName = Session["Username"].ToString();
                            _UserManagement.dataTable = _DataTable;
                            _UserManagement._RoleId = lblID.Text.ToString();
                            _UserManagement._MenuID = lblID.Text.ToString();
                            _UserManagement._ID = lblID.Text.ToString();
                            _UserManagement._Role = txtEditRoleName.Text.ToString();
                            _UserManagement.Flag = 2; // Insert Role & Page Permissions.
                            _UserManagement._ClientID = ddlClient.SelectedValue.ToString();
                            DataSet _dsRoles = _UserManagement.UserAccessManagementUpdateTblRoleDetails();

                            if (_dsRoles != null || Convert.ToString(_dsRoles.Tables[0].Rows[0][0]) == "SUCCESS")
                            {
                                txtEditRoleName.Text = string.Empty;
                                fillRoleDetailsGrid();
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + txtEditRoleName.Text + " Role added successfully.', 'Edit Role Details');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable to add new Role. Please Try Again.', 'Add Role');", true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact Systeam administartor.', ' Edit Role');", true);
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: btnEditRole_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnEditClose_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtEditRoleName.Text = string.Empty;
                ddlClient.SelectedValue = "0";
                gdvEditMenu.Visible = false;
                ModalPopupExtender_EditRole.Hide();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: btnEditClose_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnDeleteClose_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtResone.Text = string.Empty;
                ModalPopupExtender_DeclinRole.Hide();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: btnDeleteClose_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnCancelmi_Click(object sender, EventArgs e)
        {
            try
            {
                txtRole.Text = string.Empty;
                ModalPopupExtender_InsertRole.Hide();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: btnCancelmi_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        // Modal - Button Event To Delete Role.
        protected void btnSaveResone_ServerClick(object sender, EventArgs e)
        {
            try
            {
                _UserManagement.Flag = (int)EnumCollection.EnumDBOperationTypeOverall.Terminate;
                _UserManagement._UserName = Session["Username"].ToString();
                _UserManagement._ID = lblID.Text.ToString();
                _UserManagement._RoleId = Session["UserRoleID"].ToString();
                _UserManagement.Reason = txtResone.Text.ToString();
                _gvUserAccessControl = _UserManagement.UserAccessManagementDeleteRole();

                if (_gvUserAccessControl != null && _gvUserAccessControl.Tables.Count > 0 && Convert.ToString(_gvUserAccessControl.Tables[0].Rows[0][0]) == "Done")
                {
                    txtRole.Text = "";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + txtEditRoleName.Text + " Role deleted successfully.');", true);
                    fillRoleDetailsGrid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable to deleted Role. Please Try Again.', 'Delete Role');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: btnSaveResone_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void LinkBtnPermissions_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton _linkBtn = (LinkButton)sender;
                GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;
                ////GridViewRow row = (GridViewRow)_linkBtn.Parent.Parent;
                _lblMenuID = (Label)row.FindControl("lblSub_MenuID");
                _lblSub_SubMenuID = (Label)row.FindControl("lblSub_SubMenuID");
                string MenuID = _lblMenuID != null ? _lblMenuID.Text : null;
                string SubMenuID = _lblSub_SubMenuID != null ? _lblSub_SubMenuID.Text : null;
                if (!string.IsNullOrEmpty(_linkBtn.Text))
                {
                    _linkBtn.Text = _linkBtn.Text == "Grant" ? "Revoke" : "Grant";
                }

                ModalPopupExtender_EditRole.Show();

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Script", "<script>ExpandCollapse('div" + MenuID + "');</script>", false);
                _linkBtn.Focus();

            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: LinkBtnPermissions_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region CheckBox Events
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
                ModalPopupExtender_EditRole.Show();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: chkEditMainMenuOnOff_CheckedChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        // New Block : To Preserve Changes Of Menu and SubMenu Before Submit.
        protected void chkEditSubMenuOnOff_CheckedChanged2(object sender, EventArgs e)
        {
            try
            {
                PrepareChangesToUpdate();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: chkEditSubMenuOnOff_CheckedChanged2: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        // New Block : To Preserve Changes Of Menu and SubMenu Before Submit.
        private void PrepareChangesToUpdate()
        {
            try
            {
                foreach (GridViewRow _rowEditMainMenu in gdvEditMenu.Rows)
                {
                    if (_rowEditMainMenu.RowType == DataControlRowType.DataRow)
                    {
                        GridView _gvEditSubMenu = (GridView)_rowEditMainMenu.FindControl("gvEditSubMenu");
                        _lblMenuID = (Label)_rowEditMainMenu.FindControl("lblMenuID");
                        _lblMenuName = (Label)_rowEditMainMenu.FindControl("lblMenuName");
                        _chkEditMainMenuOnOff = (CheckBox)_rowEditMainMenu.FindControl("chkEditMainMenuOnOff");
                        _IntchkEditMainMenuOnOff = _chkEditMainMenuOnOff.Checked ? 1 : 0;
                        foreach (GridViewRow _rowSubMenu in _gvEditSubMenu.Rows)
                        {
                            if (_rowSubMenu.RowType == DataControlRowType.DataRow)
                            {
                                //_chkEditSubMenuOnOff = (CheckBox)_rowSubMenu.FindControl("chkEditSubMenuOnOff");
                                _lblSub_SubMenuName = (Label)_rowSubMenu.FindControl("lblSub_SubMenuName");
                                _lblSub_MenuID = (Label)_rowSubMenu.FindControl("lblSub_MenuID");
                                _lblSub_SubMenuID = (Label)_rowSubMenu.FindControl("lblSub_SubMenuID");
                                _lblSub_Accessibility = (Label)_rowSubMenu.FindControl("lblSub_Accessibility");
                                _LinkBtnPermissions = (LinkButton)_rowSubMenu.FindControl("LinkBtnPermissions");

                                // _IntchkEditSubMenuOnOff = _chkEditSubMenuOnOff.Checked ? 1 : 0;
                                _IntchkEditSubMenuOnOff = _LinkBtnPermissions.Text == "Revoke" ? 1 : 0;

                                _IntlblSub_SubMenuID = !string.IsNullOrEmpty(_lblSub_SubMenuID.Text) ? Convert.ToInt32(_lblSub_SubMenuID.Text) : 0;
                                _IntlblSub_MenuID = !string.IsNullOrEmpty(_lblSub_MenuID.Text) ? Convert.ToInt32(_lblSub_MenuID.Text) : 0;
                                _IntlblMenuID = !string.IsNullOrEmpty(_lblMenuID.Text) ? Convert.ToInt32(_lblMenuID.Text) : 0;

                                DataRow _newRow = _tblRoleDetails.NewRow();
                                _newRow["RoleId"] = lblID.Text;
                                _newRow["MenuId"] = _IntlblMenuID;
                                _newRow["SubMenuId"] = _IntlblSub_SubMenuID;
                                _newRow["Accessibility"] = _IntchkEditSubMenuOnOff;
                                _newRow["IsMenuAccessed"] = _IntchkEditMainMenuOnOff;
                                _tblRoleDetails.Rows.Add(_newRow);
                            }
                        }
                    }
                }
                ViewState["tblRoleDetails"] = _tblRoleDetails;
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: PrepareChangesToUpdate: Exception: " + Ex.Message);
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
                fillRoleDetailsGrid();
                exportFormat.ExportInCSV(Session["Username"].ToString(), "Payrakam", "User Access Management Report", _gvUserAccessControl);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: btnExportCSV_Click: Exception: " + Ex.Message);
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
                fillRoleDetailsGrid();
                exportFormat.ExporttoExcel(Session["Username"].ToString(), "Payrakam", "User Access Management Report", _gvUserAccessControl);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: btnExportXLS_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Dropdown Events
        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtender_EditRole.Show();
                if (ddlClient.SelectedValue.ToString() != "0")
                {
                    fillEditMenuDetailsGrid();
                }
                else
                {
                    gdvEditMenu.Visible = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Client');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: ddlClient_SelectedIndexChanged: Exception: " + Ex.Message);
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
                //DataSet dt = fillRoleDetailsGrid();
                // FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                _UserManagement.Flag = (int)EnumCollection.EnumBindingType.AgBulkL1;
                _UserManagement._UserName = Session["Username"].ToString();
                _UserManagement._ID = null;
                _UserManagement._RoleId = Session["UserRoleID"].ToString();
                DataSet dt = _UserManagement.FillGridUserAccessManagement();
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "User Access Management Report", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: BtnCsv_Click: Exception: " + Ex.Message);
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
                //DataSet dt = fillRoleDetailsGrid();
                _UserManagement.Flag = (int)EnumCollection.EnumBindingType.AgBulkL1;
                _UserManagement._UserName = Session["Username"].ToString();
                _UserManagement._ID = null;
                _UserManagement._RoleId = Session["UserRoleID"].ToString();
                DataSet dt = _UserManagement.FillGridUserAccessManagement();
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "User Access Management Report", dt);
                }
                {
                    //lblRecordCount.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: BtnXls_Click: Exception: " + Ex.Message);
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
                FileName = @"" + "User Access Management" + ".XLS";
                System.IO.DirectoryInfo di = new DirectoryInfo(PathLocation);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                return PathLocation + FileName;
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: GenerateToExcel: Exception: " + Ex.Message);
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
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: SetPageFiltersExport: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return pageFilters;
        }
        #endregion
    }
}