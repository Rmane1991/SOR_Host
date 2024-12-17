using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessAccessLayer;
using AppLogger;
using System.Configuration;
using System.Threading;
using System.IO;

namespace SOR.Pages.Admin
{
    public partial class pgUserPageManagement : System.Web.UI.Page
    {
        #region data Member Object Classes
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

        DataSet _dsUsersList = null;
        DataSet _dsUserUpDateCount = null;

        string[] _auditParams = new string[4];
        string _NotificationMessage = string.Empty;
        static int PageIndex = 0;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "pgUserPageManagement.aspx", "6");
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
                            hidTAB.Value = "#home";
                            UserPermissions.RegisterStartupScriptForNavigationList("2", "6", "SearchPages", "docket-tab");
                            BindDropdowns();
                            BindDropdownClientDetails();
                            FillGrid();
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
                ErrorLog.AdminManagementTrace("pgUserPageManagement: Page_Load: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region FillGrid And Methods
        private DataSet FillGrid()
        {
            DataSet _dsUsersList = null;
            try
            {
                setProperties();
                _dsUsersList = _UserManagement.FillGridUserPageManagement();
                gvPages.DataSource = null;
                gvPages.DataBind();

                if (_dsUsersList.Tables.Count > 0 && _dsUsersList.Tables[0].Rows.Count > 0)
                {
                    gvPages.Visible = true;
                    panelGrid.Visible = true;

                    gvPages.PageIndex = PageIndex;
                    gvPages.DataSource = _dsUsersList.Tables[0];
                    gvPages.DataBind();
                    lblRecordCount.Visible = true;
                    lblRecordCount.Text = "Total " + Convert.ToString(_dsUsersList.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    panelGrid.Visible = false;
                    gvPages.Visible = false;
                    lblRecordCount.Text = "Total 0 Record(s) Found.";

                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: FillGrid: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                throw;
            }
            return _dsUsersList;
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

                            ddlClientForMenu.Items.Clear();
                            ddlClientForMenu.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientForMenu.DataTextField = "ClientName";
                            ddlClientForMenu.DataValueField = "ClientID";
                            ddlClientForMenu.DataBind();

                            ddlClientForSubMenu.Items.Clear();
                            ddlClientForSubMenu.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientForSubMenu.DataTextField = "ClientName";
                            ddlClientForSubMenu.DataValueField = "ClientID";
                            ddlClientForSubMenu.DataBind();

                            ddlClientForSwap.Items.Clear();
                            ddlClientForSwap.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientForSwap.DataTextField = "ClientName";
                            ddlClientForSwap.DataValueField = "ClientID";
                            ddlClientForSwap.DataBind();

                            ddlClientForMap.Items.Clear();
                            ddlClientForMap.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientForMap.DataTextField = "ClientName";
                            ddlClientForMap.DataValueField = "ClientID";
                            ddlClientForMap.DataBind();

                            ddlClientForEditMenu.Items.Clear();
                            ddlClientForEditMenu.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientForEditMenu.DataTextField = "ClientName";
                            ddlClientForEditMenu.DataValueField = "ClientID";
                            ddlClientForEditMenu.DataBind();

                            ddlClientEditSubMenu.Items.Clear();
                            ddlClientEditSubMenu.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientEditSubMenu.DataTextField = "ClientName";
                            ddlClientEditSubMenu.DataValueField = "ClientID";
                            ddlClientEditSubMenu.DataBind();

                        }
                        else
                        {
                            ddlClient.Items.Clear();
                            ddlClient.DataSource = _dsClient.Tables[0].Copy();
                            ddlClient.DataTextField = "ClientName";
                            ddlClient.DataValueField = "ClientID";
                            ddlClient.DataBind();
                            ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                            ddlClientForMenu.Items.Clear();
                            ddlClientForMenu.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientForMenu.DataTextField = "ClientName";
                            ddlClientForMenu.DataValueField = "ClientID";
                            ddlClientForMenu.DataBind();
                            ddlClientForMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                            ddlClientForSubMenu.Items.Clear();
                            ddlClientForSubMenu.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientForSubMenu.DataTextField = "ClientName";
                            ddlClientForSubMenu.DataValueField = "ClientID";
                            ddlClientForSubMenu.DataBind();
                            ddlClientForSubMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                            ddlClientForSwap.Items.Clear();
                            ddlClientForSwap.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientForSwap.DataTextField = "ClientName";
                            ddlClientForSwap.DataValueField = "ClientID";
                            ddlClientForSwap.DataBind();
                            ddlClientForSwap.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                            ddlClientForMap.Items.Clear();
                            ddlClientForMap.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientForMap.DataTextField = "ClientName";
                            ddlClientForMap.DataValueField = "ClientID";
                            ddlClientForMap.DataBind();
                            ddlClientForMap.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));


                            ddlClientForEditMenu.Items.Clear();
                            ddlClientForEditMenu.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientForEditMenu.DataTextField = "ClientName";
                            ddlClientForEditMenu.DataValueField = "ClientID";
                            ddlClientForEditMenu.DataBind();
                            ddlClientForEditMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                            ddlClientEditSubMenu.Items.Clear();
                            ddlClientEditSubMenu.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientEditSubMenu.DataTextField = "ClientName";
                            ddlClientEditSubMenu.DataValueField = "ClientID";
                            ddlClientEditSubMenu.DataBind();
                            ddlClientEditSubMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                        }
                    }
                    else
                    {
                        ddlClient.Items.Clear();
                        ddlClient.DataSource = null;
                        ddlClient.DataBind();
                        ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlClientForMenu.Items.Clear();
                        ddlClientForMenu.DataSource = null;
                        ddlClientForMenu.DataBind();
                        ddlClientForMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlClientForSubMenu.Items.Clear();
                        ddlClientForSubMenu.DataSource = null;
                        ddlClientForSubMenu.DataBind();
                        ddlClientForSubMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlClientForSwap.Items.Clear();
                        ddlClientForSwap.DataSource = null;
                        ddlClientForSwap.DataBind();
                        ddlClientForSwap.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlClientForMap.Items.Clear();
                        ddlClientForMap.DataSource = null;
                        ddlClientForMap.DataBind();
                        ddlClientForMap.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));


                        ddlClientForEditMenu.Items.Clear();
                        ddlClientForEditMenu.DataSource = null;
                        ddlClientForEditMenu.DataBind();
                        ddlClientForEditMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlClientEditSubMenu.Items.Clear();
                        ddlClientEditSubMenu.DataSource = null;
                        ddlClientEditSubMenu.DataBind();
                        ddlClientEditSubMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                    }
                }
                else
                {
                    ddlClient.Items.Clear();
                    ddlClient.DataSource = null;
                    ddlClient.DataBind();
                    ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                    ddlClientForMenu.Items.Clear();
                    ddlClientForMenu.DataSource = null;
                    ddlClientForMenu.DataBind();
                    ddlClientForMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                    ddlClientForSubMenu.Items.Clear();
                    ddlClientForSubMenu.DataSource = null;
                    ddlClientForSubMenu.DataBind();
                    ddlClientForSubMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                    ddlClientForSwap.Items.Clear();
                    ddlClientForSwap.DataSource = null;
                    ddlClientForSwap.DataBind();
                    ddlClientForSwap.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                    ddlClientForMap.Items.Clear();
                    ddlClientForMap.DataSource = null;
                    ddlClientForMap.DataBind();
                    ddlClientForMap.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));


                    ddlClientForEditMenu.Items.Clear();
                    ddlClientForEditMenu.DataSource = null;
                    ddlClientForEditMenu.DataBind();
                    ddlClientForEditMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                    ddlClientEditSubMenu.Items.Clear();
                    ddlClientEditSubMenu.DataSource = null;
                    ddlClientEditSubMenu.DataBind();
                    ddlClientEditSubMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: BindDropdownClientDetails: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void setProperties()
        {
            try
            {
                _UserManagement._UserName = Session["Username"].ToString();
                _UserManagement._RoleId = ddlRoleFilter.SelectedValue != "0" ? ddlRoleFilter.SelectedValue : null;
                _UserManagement._MenuID = ddlMenuFilter.SelectedValue != "0" ? ddlMenuFilter.SelectedValue : null;
                _UserManagement._SubMenuID = ddlSubMenuFilter.SelectedValue != "0" ? ddlSubMenuFilter.SelectedValue : null;
                _UserManagement._ClientID = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                if (ddlAccessFilter.SelectedValue == "0")
                    _UserManagement._Access = null;
                else
                    _UserManagement._Access = ddlAccessFilter.SelectedValue == "1" ? false : true;
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: setProperties: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void BindDropdowns()
        {
            DataSet _dsPages = new DataSet();
            try
            {
                _UserManagement._UserName = Session["Username"].ToString();
                _UserManagement.Flag = (int)EnumCollection.EnumBindingType.BindGrid;

                _dsPages = _UserManagement.BindDropdowns();
                if (_dsPages.Tables.Count > 0)
                {
                    if (_dsPages.Tables.Count > 0 && _dsPages.Tables[0].Rows.Count > 0)
                    {
                        ddlMenuMapping.Items.Clear();
                        ddlMenuMapping.DataSource = _dsPages.Tables[0].Copy();
                        ddlMenuMapping.DataTextField = "MenuName";
                        ddlMenuMapping.DataValueField = "MenuID";
                        ddlMenuMapping.DataBind();
                        ddlMenuMapping.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlMenuEdit.Items.Clear();
                        ddlMenuEdit.DataSource = _dsPages.Tables[0].Copy();
                        ddlMenuEdit.DataTextField = "MenuName";
                        ddlMenuEdit.DataValueField = "MenuID";
                        ddlMenuEdit.DataBind();
                        ddlMenuEdit.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlMenuFilter.Items.Clear();
                        ddlMenuFilter.DataSource = _dsPages.Tables[0].Copy();
                        ddlMenuFilter.DataTextField = "MenuName";
                        ddlMenuFilter.DataValueField = "MenuID";
                        ddlMenuFilter.DataBind();
                        ddlMenuFilter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    if (_dsPages.Tables.Count > 1 && _dsPages.Tables[1].Rows.Count > 0)
                    {
                        ddlSubMenuMapping.Items.Clear();
                        ddlSubMenuMapping.DataSource = _dsPages.Tables[1].Copy();
                        ddlSubMenuMapping.DataTextField = "SubMenuName";
                        ddlSubMenuMapping.DataValueField = "SubMenuID";
                        ddlSubMenuMapping.DataBind();
                        ddlSubMenuMapping.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlSubMenuEdit.Items.Clear();
                        ddlSubMenuEdit.DataSource = _dsPages.Tables[1].Copy();
                        ddlSubMenuEdit.DataTextField = "SubMenuName";
                        ddlSubMenuEdit.DataValueField = "SubMenuID";
                        ddlSubMenuEdit.DataBind();
                        ddlSubMenuEdit.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlSubMenuFilter.Items.Clear();
                        ddlSubMenuFilter.DataSource = _dsPages.Tables[1].Copy();
                        ddlSubMenuFilter.DataTextField = "SubMenuName";
                        ddlSubMenuFilter.DataValueField = "SubMenuID";
                        ddlSubMenuFilter.DataBind();
                        ddlSubMenuFilter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    if (_dsPages.Tables.Count > 2 && _dsPages.Tables[3].Rows.Count > 0)
                    {
                        ddlRole.Items.Clear();
                        ddlRole.DataSource = _dsPages.Tables[3].Copy();
                        ddlRole.DataTextField = "RoleName";
                        ddlRole.DataValueField = "RoleId";
                        ddlRole.DataBind();
                        ddlRole.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlEditRole.Items.Clear();
                        ddlEditRole.DataSource = _dsPages.Tables[2].Copy();
                        ddlEditRole.DataTextField = "RoleName";
                        ddlEditRole.DataValueField = "RoleId";
                        ddlEditRole.DataBind();
                        ddlEditRole.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        ddlRoleFilter.Items.Clear();
                        ddlRoleFilter.DataSource = _dsPages.Tables[2].Copy();
                        ddlRoleFilter.DataTextField = "RoleName";
                        ddlRoleFilter.DataValueField = "RoleId";
                        ddlRoleFilter.DataBind();
                        ddlRoleFilter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: BindDropdowns: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void InsertUpdatePagePermissions()
        {
            try
            {
                _dsUsersList = _UserManagement.InsertUpdatePagePermissions();
                if (_dsUsersList.Tables.Count != 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Page operation is successfully.','Manage Pages');", true);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Script", "<script>$('#myTabs a[href=\"" + hidTAB.Value + "\"]').tab('show');</script>", false);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Problem while perform operation.','Manage Pages');", true);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Script", "<script>$('#myTabs a[href=\"" + hidTAB.Value + "\"]').tab('show');</script>", false);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: InsertUpdatePagePermissions: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Search
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: btnSearch_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Reset Button
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ddlMenuFilter.SelectedValue = "0";
                ddlSubMenuFilter.SelectedValue = "0";
                ddlRoleFilter.SelectedValue = "0";
                ddlAccessFilter.SelectedValue = "0";

                FillGrid();
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: btnReset_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Button Events
        protected void btnAddMenu_ServerClick(object sender, EventArgs e)
        {
            try
            {
                hidTAB.Value = "#menu1";
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbersCharacterSpace, txtMenu.Text))
                {
                    txtMenu.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Enter valid Menu Name.','Menu Name');", true);
                    TabName.Value = "AddPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false); return;
                }
                else
                {
                    _UserManagement._MenuName = txtMenu.Text.Trim();
                    _UserManagement._MenuLink = !string.IsNullOrEmpty(txtMenuPath.Text) ? txtMenuPath.Text : null;
                    _UserManagement._bitIsRemove = ddlMenuAccess.SelectedValue == "1" ? true : false;
                    _UserManagement._UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                    _UserManagement._EnumMenuType = EnumCollection.EnumMenuType.MenuAdd;
                    //_UserManagement._ClientID = ddlClientForMenu.SelectedValue.ToString();
                    _UserManagement.MenuIcon = RadioMIcon.SelectedValue.ToString();
                    InsertUpdatePagePermissions();
                }
                TabName.Value = "AddPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: btnAddMenu_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnAddSubMenu_ServerClick(object sender, EventArgs e)
        {
            try
            {

                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbersCharacterSpace, txtSubMenuName.Text))
                {
                    txtSubMenuName.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Enter valid Sub Menu Name.','Sub Menu Name');", true);
                    TabName.Value = "AddPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }
                else _UserManagement._SubMenuName = txtSubMenuName.Text.Trim();

                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbersCharacterSpace, txtSubMenuPath.Text))
                {
                    txtSubMenuPath.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Enter valid Sub Menu Path.', 'Sub Menu Path');", true);
                    TabName.Value = "AddPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }
                else
                {
                    _UserManagement._SubMenuLink = txtSubMenuPath.Text.Trim();

                    _UserManagement._bitIsRemove = ddlSubMenuAccess.SelectedValue == "1" ? true : false;
                    _UserManagement._UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                    _UserManagement._EnumMenuType = EnumCollection.EnumMenuType.SubMenuAdd;
                    // _UserManagement._ClientID = ddlClientForSubMenu.SelectedValue.ToString();
                    InsertUpdatePagePermissions();
                }
                TabName.Value = "AddPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);

            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: btnAddSubMenu_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnMapping_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlRole.SelectedValue == "0")
                {
                    ddlRole.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Role.', 'Mapping Pages');", true);
                    TabName.Value = "MapPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }
                else _UserManagement._RoleId = ddlRole.SelectedValue;

                if (ddlMenuMapping.SelectedItem.Text == "-- Select --")
                {
                    ddlMenuMapping.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Menu.', ' Mapping Pages');", true);
                    TabName.Value = "MapPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }
                else _UserManagement._MenuID = ddlMenuMapping.SelectedValue;

                if (ddlSubMenuMapping.SelectedValue == "0")
                {
                    ddlSubMenuMapping.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Sub Menu.', 'Mapping Pages');", true);
                    TabName.Value = "MapPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }
                else
                {
                    _UserManagement._SubMenuID = ddlSubMenuMapping.SelectedValue;

                    _UserManagement._bitIsRemove = ddlMappingAccess.SelectedValue == "1" ? false : true;
                    _UserManagement._UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                    _UserManagement._EnumMenuType = EnumCollection.EnumMenuType.MappingPages;
                    _UserManagement._ClientID = "Maximus"; //ddlClientForMap.SelectedValue.ToString();
                    _UserManagement._ParentID = ddlRole.SelectedValue;
                    InsertUpdatePagePermissions();
                }
                TabName.Value = "MapPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);

            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: btnMapping_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnMenuEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlMenuEdit.SelectedItem.Text == "-- Select --")
                {
                    ddlMenuEdit.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Menu.', ' Edit Menu');", true);
                    TabName.Value = "EditPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);

                    return;
                }
                else _UserManagement._MenuID = ddlMenuEdit.SelectedValue;
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbersCharacterSpace, txtMenuEdit.Text))
                {
                    txtMenuEdit.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Enter valid  Menu Name.', ' Edit Menu');", true);
                    TabName.Value = "EditPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);

                    return;
                }
                else
                {
                    _UserManagement._MenuName = txtMenuEdit.Text.Trim();
                    _UserManagement._MenuLink = txtMenuPathEdit.Text;
                    _UserManagement._bitIsRemove = ddlMenuEditAccess.SelectedValue == "1" ? true : false;
                    _UserManagement._UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                    _UserManagement._EnumMenuType = EnumCollection.EnumMenuType.MenuEdit;
                    // _UserManagement._ClientID = ddlClientForEditMenu.SelectedValue.ToString();
                    _UserManagement.MenuIcon = RadioMIcon.SelectedValue.ToString();
                    InsertUpdatePagePermissions();
                }
                TabName.Value = "EditPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);

            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: btnMenuEdit_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnSubMenuEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlSubMenuEdit.SelectedItem.Text == "-- Select --")
                {
                    ddlSubMenuEdit.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Sub Menu.', 'Edit Sub Menu');", true);
                    TabName.Value = "EditPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }
                else _UserManagement._SubMenuID = ddlSubMenuEdit.SelectedValue;
                if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbersCharacterSpace, txtSubMenuEdit.Text))
                {
                    txtSubMenuEdit.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Enter valid  Sub Menu Name.', ' Edit Sub Menu');", true);
                    TabName.Value = "EditPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }
                else
                {
                    _UserManagement._SubMenuName = txtSubMenuEdit.Text.Trim();
                    _UserManagement._SubMenuLink = txtSubMenuPathEdit.Text;
                    _UserManagement._bitIsRemove = ddlSubMenuEditAccess.SelectedValue == "1" ? true : false;
                    _UserManagement._UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                    _UserManagement._EnumMenuType = EnumCollection.EnumMenuType.SubMenuEdit;
                    InsertUpdatePagePermissions();
                }
                TabName.Value = "EditPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: btnSubMenuEdit_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnSwapPages_ServerClick(object sender, EventArgs e)
        {
            try
            {

                if (ddlClientForSwap.SelectedValue == "0")
                {
                    ddlClientForSwap.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Client.', ' Swap Pages');", true);
                    TabName.Value = "SwapPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }

                if (ddlEditRole.SelectedValue == "0")
                {
                    ddlEditRole.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Role.', ' Swap Pages');", true);
                    TabName.Value = "SwapPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }
                else _UserManagement._RoleId = ddlEditRole.SelectedValue;

                if (ddlEditSubMenu.SelectedValue == "0")
                {
                    ddlEditSubMenu.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Sub Menu.', ' Swap Pages');", true);
                    TabName.Value = "SwapPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }
                else _UserManagement._SubMenuID = ddlEditSubMenu.SelectedValue;

                if (ddlEditMenu.SelectedItem.Text == "-- Select --")
                {
                    ddlEditMenu.Focus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Menu.', ' Swap Pages');", true);
                    TabName.Value = "SwapPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                    return;
                }
                else
                {
                    _UserManagement._MenuID = ddlEditMenu.SelectedValue;

                    //if (String.IsNullOrEmpty(TxtEditSubMenuPath.Text))
                    //{
                    //    TxtEditSubMenuPath.Focus();
                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Enter Path.', 'Path');", true);
                    //    return;
                    //}
                    //else _SubMenuLink = TxtEditSubMenuPath.Text.Trim();

                    _UserManagement._bitIsRemove = ddlMappingAccess.SelectedValue == "1" ? false : true;
                    _UserManagement._UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                    _UserManagement._EnumMenuType = EnumCollection.EnumMenuType.SwappingPages;
                    _UserManagement._ClientID = ddlClientForSwap.SelectedValue.ToString();
                    InsertUpdatePagePermissions();
                }
                TabName.Value = "SwapPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: btnSwapPages_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Dropdown Events
        protected void ddlSubMenuEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSubMenuEdit.SelectedValue != "0" && !String.IsNullOrEmpty(ddlSubMenuEdit.SelectedItem.Text))
                {
                    _UserManagement._ID = ddlSubMenuEdit.SelectedValue;
                    _UserManagement._UserName = Session["Username"].ToString();
                    _UserManagement.Flag = (int)EnumCollection.EnumMenuSelectType.SubMenu;
                    _dsUsersList = _UserManagement.GetPagesByID();

                    if (_dsUsersList.Tables.Count > 0 && _dsUsersList.Tables[0].Rows.Count > 0)
                    {
                        txtSubMenuEdit.Text = ""; txtSubMenuPathEdit.Text = "";
                        txtSubMenuEdit.Text = _dsUsersList.Tables[0].Rows[0]["Name"].ToString();
                        txtSubMenuPathEdit.Text = _dsUsersList.Tables[0].Rows[0]["Link"].ToString();
                    }
                }
                TabName.Value = "EditPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);

            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: ddlSubMenuEdit_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void ddlMenuEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSubMenuEdit.SelectedValue != "0" && !String.IsNullOrEmpty(ddlSubMenuEdit.SelectedItem.Text))
                {
                    _UserManagement._ID = ddlMenuEdit.SelectedValue;
                    _UserManagement._UserName = Session["Username"].ToString();
                    _UserManagement.Flag = (int)EnumCollection.EnumMenuSelectType.Menu;
                    _dsUsersList = _UserManagement.GetPagesByID();

                    if (_dsUsersList.Tables.Count > 0 && _dsUsersList.Tables[0].Rows.Count > 0)
                    {
                        txtSubMenuEdit.Text = _dsUsersList.Tables[0].Rows[0]["Name"].ToString();
                        txtSubMenuPathEdit.Text = _dsUsersList.Tables[0].Rows[0]["Link"].ToString();
                    }
                    TabName.Value = "EditPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                }
                TabName.Value = "EditPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: ddlMenuEdit_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void ddlEditSubMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hidTAB.Value = "#menu4";
                if (ddlEditSubMenu.SelectedValue != "0" && !String.IsNullOrEmpty(ddlEditSubMenu.SelectedItem.Text))
                {
                    _UserManagement._ID = ddlEditSubMenu.SelectedValue;
                    _UserManagement._UserName = Session["Username"].ToString();
                    _UserManagement.Flag = (int)EnumCollection.EnumMenuSelectType.SubMenu;
                    _dsUsersList = _UserManagement.GetPagesByID();

                    if (_dsUsersList.Tables.Count > 0 && _dsUsersList.Tables[0].Rows.Count > 0)
                    {
                        TxtEditSubMenuPath.Value = _dsUsersList.Tables[0].Rows[0]["Link"].ToString();
                    }
                    TabName.Value = "SwapPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                }
                else
                {
                    TxtEditSubMenuPath.Value = "";
                }
                TabName.Value = "SwapPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: ddlEditSubMenu_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void ddlEditRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlEditRole.SelectedValue != "0" && !String.IsNullOrEmpty(ddlEditRole.SelectedItem.Text))
                {
                    _UserManagement._ID = ddlEditRole.SelectedValue;
                    _UserManagement._UserName = Session["Username"].ToString();
                    _dsUsersList = _UserManagement.GetPagesByRoleID();

                    if (_dsUsersList.Tables.Count > 0 && _dsUsersList.Tables[0].Rows.Count > 0)
                    {
                        ddlEditMenu.Items.Clear();
                        ddlEditMenu.DataSource = _dsUsersList.Tables[0].Copy();
                        ddlEditMenu.DataTextField = "MenuName";
                        ddlEditMenu.DataValueField = "MenuID";
                        ddlEditMenu.DataBind();
                        ddlEditMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlEditMenu.Items.Clear();
                        ddlEditMenu.DataSource = null;
                        ddlEditMenu.DataBind();
                        ddlEditMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    if (_dsUsersList.Tables.Count > 0 && _dsUsersList.Tables[1].Rows.Count > 0)
                    {
                        ddlEditSubMenu.Items.Clear();
                        ddlEditSubMenu.DataSource = _dsUsersList.Tables[1].Copy();
                        ddlEditSubMenu.DataTextField = "SubMenuName";
                        ddlEditSubMenu.DataValueField = "SubMenuID";
                        ddlEditSubMenu.DataBind();
                        ddlEditSubMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlEditSubMenu.Items.Clear();
                        ddlEditSubMenu.DataSource = null;
                        ddlEditSubMenu.DataBind();
                        ddlEditSubMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    TabName.Value = "SwapPages";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                }
                else
                {
                    ddlEditMenu.Items.Clear();
                    ddlEditMenu.DataSource = null;
                    ddlEditMenu.DataBind();
                    ddlEditMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                    ddlEditSubMenu.Items.Clear();
                    ddlEditSubMenu.DataSource = null;
                    ddlEditSubMenu.DataBind();
                    ddlEditSubMenu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                }
                TabName.Value = "SwapPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: ddlEditRole_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Grid Events
        protected void gvPages_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                PageIndex = e.NewPageIndex;
                gvPages.PageIndex = PageIndex;
                FillGrid();

            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: gvPages_PageIndexChanging: Exception: " + Ex.Message);
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
                FillGrid();
                exportFormat.ExportInCSV(Session["Username"].ToString(), Session["BankName"].ToString(), "Page Permissions", _dsUsersList);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: btnExportCSV_Click: Exception: " + Ex.Message);
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
                FillGrid();
                exportFormat.ExporttoExcel(Session["Username"].ToString(), Session["BankName"].ToString(), "Page Permissions", _dsUsersList);
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: btnExportXLS_Click: Exception: " + Ex.Message);
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
                DataSet dt = FillGrid();
                // FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "Page Permissions Report", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: BtnCsv_Click: Exception: " + Ex.Message);
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
                DataSet dt = FillGrid();

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "Page Permissions Report", dt);
                }
                {
                    //lblRecordCount.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);

                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserPageManagement: BtnXls_Click: Exception: " + Ex.Message);
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
                FileName = @"" + "User Page Management" + ".XLS";
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
                ErrorLog.AdminManagementTrace("pgUserPageManagement: GenerateToExcel: Exception: " + Ex.Message);
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
                ErrorLog.AdminManagementTrace("pgUserPageManagement: SetPageFiltersExport: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return pageFilters;
        }
        #endregion

        protected void ddlMenuMapping_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSubMenuMapping.Items.Clear();
                ddlSubMenuMapping.DataSource = null;
                ddlSubMenuMapping.DataBind();
                DataSet _dsPages = new DataSet();

                _UserManagement._UserName = Session["Username"].ToString();
                _UserManagement._MenuID = ddlMenuMapping.SelectedValue.ToString();
                _UserManagement.Flag = (int)EnumCollection.EnumBindingType.BindGrid;

                _dsPages = _UserManagement.BindSubMenu();
                if (_dsPages != null && _dsPages.Tables[0].Rows.Count > 0 && _dsPages.Tables[0].Rows.Count > 0)
                {
                    if (_dsPages.Tables[0].Rows.Count == 1)
                    {
                        ddlSubMenuMapping.Items.Clear();
                        ddlSubMenuMapping.DataSource = _dsPages.Tables[0].Copy();
                        ddlSubMenuMapping.DataTextField = "SubMenuName";
                        ddlSubMenuMapping.DataValueField = "SubMenuID";
                        ddlSubMenuMapping.DataBind();
                        ddlSubMenuMapping.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlSubMenuMapping.Items.Clear();
                        ddlSubMenuMapping.DataSource = _dsPages.Tables[0].Copy();
                        ddlSubMenuMapping.DataTextField = "SubMenuName";
                        ddlSubMenuMapping.DataValueField = "SubMenuID";
                        ddlSubMenuMapping.DataBind();
                        ddlSubMenuMapping.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlSubMenuMapping.DataSource = null;
                    ddlSubMenuMapping.DataBind();
                    ddlSubMenuMapping.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
                TabName.Value = "MapPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false); return;
            }
            catch (Exception ex)
            {
                TabName.Value = "MapPages";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
                ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Agent Registration');", true);
                return;
            }
        }
    }
}