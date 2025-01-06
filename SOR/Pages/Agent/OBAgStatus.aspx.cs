using AppLogger;
using BussinessAccessLayer;
using System;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOR.Pages.Agent
{
    public partial class OBAgStatus : System.Web.UI.Page
    {
        #region Objects Declaration
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();
        public string UserName { get; set; }
        public bool HasPagePermission { get; private set; }

        EmailAlerts _EmailAlerts = new EmailAlerts();
        ClientRegistrationEntity clientMngnt = new ClientRegistrationEntity();
        #endregion;

        #region Variable and Objects

        DataSet _dsDeactivateBC = null;

        string[]
            _BCActiveParams = new string[8];

        string
            BCMaxID = null,
            Remarks = null,
            _strBCFullName = null;
        string
            _strAlertMessage_Header = null,
            _strAlertMessage_Success = null,
            _strAlertMessage_UnSuccess = null,
            _strAlertMessage_Total = null,
            _fileLineNo = null;
        int _successful = 0,
               _unsuessful = 0,
               _reocrdsProcessed = 0,
               _DeclineCardCount = 0,
               _StatusFlag = 0,
               _Flag = 0;

        public enum operationType
        {
            _single = 1,
            _bulk = 2
        }
        public enum FlagType
        {
            ActiveDeactive = 1,
            Delete = 2
        }
        public enum ActionType
        {
            Pending = 0,
            Approve = 1,
            Decline = 2,
            Terminate = 3,
            Activate = 4,
            Deactivate = 5,
            Reprocess = 6,
            MakerApprove = 7,
            CheckerApprove = 8,
            AuthApprove = 9
        }
        public enum SelectionType
        {
            CheckedAll = 1,
            UnCheckAll = 2
        }
        public enum PermissionType
        {
            EnableMakerChecker = 1,
            EnableRoles = 2
        }
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("OBAgStatus | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                //bool HasPagePermission = true;
                 bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "OBAgStatus.aspx", "20");
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
                        FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                        BindBcDropdown();
                        ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                        //BindExport();
                    }
                    UserPermissions.RegisterStartupScriptForNavigationListActive("6", "20");
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
                ErrorLog.AgentManagementTrace("OBAgStatus: Page_Load: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Methods

        private void BindBcDropdown()
        {
            try
            {
                ddlBCCode.Items.Clear();
                ddlBCCode.DataSource = null;
                ddlBCCode.DataBind();

                _AgentRegistrationDAL.UserName = Session["Username"].ToString();

                _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
                _AgentRegistrationDAL.IsRemoved = "0";
                _AgentRegistrationDAL.IsActive = "1";
                _AgentRegistrationDAL.IsdocUploaded = "1";
                _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.EnableMakerChecker;
                DataTable ds = _AgentRegistrationDAL.GetBC(_AgentRegistrationDAL.UserName, 0, 1, 0, ClientID, 1);
                if (ds != null && ds.Rows.Count > 0 && ds.Rows.Count > 0)
                {
                    if (ds.Rows.Count == 1)
                    {
                        ddlBCCode.Items.Clear();
                        ddlBCCode.DataSource = ds.Copy();
                        ddlBCCode.DataTextField = "BCName";
                        ddlBCCode.DataValueField = "BCCode";
                        ddlBCCode.DataBind();
                        ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlBCCode.Items.Clear();
                        ddlBCCode.DataSource = ds.Copy();
                        ddlBCCode.DataTextField = "BCName";
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
                ErrorLog.AgentManagementTrace("OBAgStatus: BindDropdownsBC: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        private void BindAgentDropdown()
        {
            try
            {
                //ddlBCCode.Items.Clear();
                //ddlBCCode.DataSource = null;
                //ddlBCCode.DataBind();

                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
                _AgentRegistrationDAL.IsRemoved = "0";
                _AgentRegistrationDAL.IsActive = "1";
                _AgentRegistrationDAL.IsdocUploaded = "1";
                _AgentRegistrationDAL.VerificationStatus = 1;
                _AgentRegistrationDAL.BCCode = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;
                _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.EnableMakerChecker;

                // _AgentEntity.BCID = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;

                DataSet ds = _AgentRegistrationDAL.BindAgentddl();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        ddlAgent.Items.Clear();
                        ddlAgent.DataSource = ds.Tables[0].Copy();
                        ddlAgent.DataTextField = "AgentName";
                        ddlAgent.DataValueField = "AgentCode";
                        ddlAgent.DataBind();
                        ddlAgent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlAgent.Items.Clear();
                        ddlAgent.DataSource = ds.Tables[0].Copy();
                        ddlAgent.DataTextField = "AgentName";
                        ddlAgent.DataValueField = "AgentCode";
                        ddlAgent.DataBind();
                        ddlAgent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlAgent.Items.Clear();
                    ddlAgent.DataSource = null;
                    ddlAgent.DataBind();
                    ddlAgent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: BindAgentDropdown: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        public DataSet FillGrid(EnumCollection.EnumPermissionType _EnumPermissionType)
        {
            DataSet _dsDeActivate = null;
            try
            {
                ErrorLog.AgentManagementTrace("OBAgStatus | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                gvBlockAG.DataSource = null;
                gvBlockAG.DataBind();
                lblRecordsTotal.Text = "";
                ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();

                _AgentRegistrationDAL.BCCode = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;
                _AgentRegistrationDAL.AgentReqId = ddlAgent.SelectedValue != "0" ? ddlAgent.SelectedValue : null;

                _AgentRegistrationDAL.Flag = (int)_EnumPermissionType;
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                //_AgentEntity.IsRemoved = "0";
                //_AgentEntity.IsActive = "2";
                _AgentRegistrationDAL.IsdocUploaded = "1";
                _AgentRegistrationDAL.VerificationStatus = 1;

                _dsDeActivate = _AgentRegistrationDAL.GetAgentDetailsToProcessOnboaring();
                if (_dsDeActivate != null && _dsDeActivate.Tables.Count > 0 && _dsDeActivate.Tables[0].Rows.Count > 0)
                {
                    gvBlockAG.DataSource = null;
                    gvBlockAG.DataBind();
                    panelGrid.Visible = true;
                    gvBlockAG.DataSource = _dsDeActivate.Tables[0];
                    gvBlockAG.DataBind();
                    gvBlockAG.Visible = true;

                    ViewState["BCDataSet"] = _dsDeActivate.Tables[0];
                    lblRecordsTotal.Text = "Total " + Convert.ToString(_dsDeActivate.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    gvBlockAG.Visible = false;
                    panelGrid.Visible = false;
                    lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                }
                ErrorLog.AgentManagementTrace("OBAgStatus | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: FillGrid: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return _dsDeActivate;
        }

        #endregion

        #region Search And Clear
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("OBAgStatus | btnSearch_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (ddlActiontype.SelectedValue == "1")
                {
                    btnActivate.Visible = false;
                    btnDeactivate.Visible = true;
                   // btnTerminate.Visible = true;
                    _AgentRegistrationDAL.IsActive = Convert.ToString((int)EnumCollection.AgentStatus.Active);
                    FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                }
                else if (ddlActiontype.SelectedValue == "2")
                {
                    btnDeactivate.Visible = false;
                    btnActivate.Visible = true;
                  //  btnTerminate.Visible = true;
                    _AgentRegistrationDAL.IsActive = Convert.ToString((int)EnumCollection.AgentStatus.InActive);
                    FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                }
                else if (ddlActiontype.SelectedValue == "3")
                {
                    btnDeactivate.Visible = false;
                    btnActivate.Visible = false;
                  //  btnTerminate.Visible = false;
                    _AgentRegistrationDAL.IsActive = Convert.ToString((int)EnumCollection.AgentStatus.Terminated);
                    FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                }
                else
                {
                    FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                }
                ErrorLog.AgentManagementTrace("OBAgStatus | btnSearch_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: btnSearch_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }


        #endregion
        protected void btnClear_Click_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("OBAgStatus | btnClear_Click_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ddlState.SelectedValue = "0";
                ddlActiontype.SelectedValue = "0";
                ddlCity.DataSource = null;
                ddlCity.DataBind();
                ddlCity.Items.Clear();
                btnActivate.Visible = false;
                btnDeactivate.Visible = false;
                
                ddlCity.Items.Insert(0, new ListItem("-- City --", "0"));
                ddlAgent.Items.Clear();
                ddlAgent.DataSource = null;
                ddlAgent.DataBind();
                ddlAgent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                ddlBCCode.Items.Clear();
                ddlBCCode.DataSource = null;
                ddlBCCode.DataBind();
                FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                BindBcDropdown();
                UserPermissions.RegisterStartupScriptForNavigationListActive("4", "22");
                ErrorLog.AgentManagementTrace("OBAgStatus | btnClear_Click_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: btnClear_Click_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        #region Grid Events
        protected void gvBlockAG_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBlockAG.PageIndex = e.NewPageIndex;

                if (ViewState["BCDataSet"] != null)
                {
                    DataTable dsTemp = (DataTable)ViewState["BCDataSet"];
                    if (dsTemp != null && dsTemp.Rows.Count > 0)
                    {
                        gvBlockAG.DataSource = dsTemp;
                        gvBlockAG.DataBind();
                        gvBlockAG.Visible = true;

                    }
                    else
                    {
                        gvBlockAG.Visible = false;

                        ///////// btnExportCSV.Visible = false;
                        /////// btnExportExcel.Visible = false;
                    }
                }
                else
                    FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                CheckBoxAllOperationOnPageIndex();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: gvBlockAG_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Activate
        protected void btnActivate_ServerClick(object sender, EventArgs e)
        {
            if (ddlActiontype.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Action Type.','Activate Agent(s)');", true);
            }
            else
            {
                ViewState["ActionType"] = Convert.ToString((int)EnumCollection.EnumDBOperationType.Activate);
                try
                {
                    ErrorLog.AgentManagementTrace("OBAgStatus | btnActivate_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-OnboardingStatus";
                    _auditParams[2] = "btnActivate";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    foreach (GridViewRow row in gvBlockAG.Rows)
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
                        lblModalHeaderName.Text = "Agent(s) Activate Reason";
                        TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                        lblconfirm.Text = "Are you sure want to Activate Agent(s)?";
                        ModalPopupExtender_Declincard.Show();
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Activate Agent(s)');", true);
                        return;
                    }
                    ErrorLog.AgentManagementTrace("OBAgStatus | btnActivate_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }
                catch (Exception Ex)
                {
                    ErrorLog.AgentManagementTrace("OBAgStatus: btnActivate_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
            }
        }

        protected void gvBlockAG_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grid = (GridView)sender;

                    string AGStatus = grid.DataKeys[e.Row.RowIndex].Values[1].ToString();
                    ImageButton imageButton = e.Row.FindControl("lbtnEdit") as ImageButton;
                    CheckBox chBoxRow = e.Row.FindControl("chBoxSelectRow") as CheckBox;
                    if (imageButton != null)
                    {
                        if (AGStatus.ToLower() == "terminated" || AGStatus.ToLower() == "deactive")
                        {
                            if(AGStatus.ToLower() == "terminated")
                            {
                                imageButton.Enabled = false;
                                imageButton.Visible = false;
                                chBoxRow.Visible = false;
                            }
                            else
                            {
                                imageButton.Enabled = false;
                                imageButton.Visible = false;
                                chBoxRow.Visible = true;
                            }
                        }
                        else
                        {
                            imageButton.Enabled = true;
                            imageButton.Visible = true;
                            chBoxRow.Visible = true;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: gvBlockAG_RowDataBound: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'alert');", true);
                return;
            }
        }
        #endregion

        #region Deactivate 
        protected void btnDeactivate_ServerClick(object sender, EventArgs e)
        {
            if (ddlActiontype.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Action Type.','Activate Agent(s)');", true);
            }
            else
            {
                ViewState["ActionType"] = Convert.ToString((int)EnumCollection.EnumDBOperationType.Deactivate);
                try
                {
                    ErrorLog.AgentManagementTrace("OBAgStatus | btnDeactivate_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-OnboardingStatus";
                    _auditParams[2] = "btnDeactivate";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    foreach (GridViewRow row in gvBlockAG.Rows)
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
                        lblModalHeaderName.Text = "Agent(s) Deactivate Reason";
                        TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                        lblconfirm.Text = "Are you sure want to Deactivate Agent(s)?";
                        ModalPopupExtender_Declincard.Show();
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Deactivate Agent(s)');", true);
                        return;
                    }
                    ErrorLog.AgentManagementTrace("OBAgStatus | btnDeactivate_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }
                catch (Exception Ex)
                {
                    ErrorLog.AgentManagementTrace("OBAgStatus: btnDeactivate_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
            }
        }

        protected void ddlBCCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindAgentDropdown();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: ddlBCCode_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        public bool ValidateReEditRequest()
        {
            bool IsvalidRecord = false;
            try
            {
                string _requestid = string.Empty;
                _AgentRegistrationDAL.Flag = 1;
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
                ErrorLog.AgentManagementTrace("OBAgStatus: gvBlockAG_RowCommand: Exception: " + Ex.Message);
            }
            return IsvalidRecord;
        }

        public bool CheckSelfRequest(out string requestid)
        {
            bool IsvalidRecord = false;
            string _requestid = string.Empty;
            try
            {
                _AgentRegistrationDAL.Flag = 3;
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
                ErrorLog.AgentManagementTrace("OBAgStatus: gvBlockAG_RowCommand: Exception: " + Ex.Message);
            }
            requestid = _requestid;
            return IsvalidRecord;
        }

        protected void gvBlockAG_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Contains("EditDetails"))
                {
                    ErrorLog.AgentManagementTrace("OBAgStatus | RowCommand-EditDetails | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-OnboardingStatus";
                    _auditParams[2] = "RowCommand-EditDetails";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    string requestid = string.Empty;
                    ImageButton lb = (ImageButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                    _AgentRegistrationDAL.AgentReqId = (gvBlockAG.DataKeys[gvr.RowIndex].Values["Agent Id"]).ToString();
                    if (ValidateReEditRequest())
                    {
                        if (CheckSelfRequest(out requestid))
                        {
                            //txtResone.Text = string.Empty;
                            string status = string.Empty;
                            int IsSelfData = 0;
                            string reqtype = "4";
                            Response.Redirect("../../Pages/Agent/EditRegistrationDetails.aspx?AgentReqId=" + _AgentRegistrationDAL.AgentReqId + "&" + "RequestType=" + reqtype + "&" + "IsSelfData=" + IsSelfData + " ", false);
                        }
                        else
                        {
                            _AgentRegistrationDAL.AgentReqId = requestid;
                            string status = string.Empty;
                            int IsSelfData = 1;
                            string reqtype = "4";
                            Response.Redirect("../../Pages/Agent/EditRegistrationDetails.aspx?AgentReqId=" + _AgentRegistrationDAL.AgentReqId + "&" + "RequestType=" + reqtype + "&" + "IsSelfData=" + IsSelfData + " ", false);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent request already pending for verification.', 'Warning');", true);
                        return;
                    }
                    ErrorLog.AgentManagementTrace("OBAgStatus | RowCommand-EditDetails | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }
                if (e.CommandName.Contains("ViewDetails"))
                {
                    ErrorLog.AgentManagementTrace("OBAgStatus | RowCommand-ViewDetails | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-OnboardingStatus";
                    _auditParams[2] = "RowCommand-ViewDetails";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    string requestid = string.Empty;
                    ImageButton lb = (ImageButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                    _AgentRegistrationDAL.AgentReqId = (gvBlockAG.DataKeys[gvr.RowIndex].Values["Agent Id"]).ToString();
                    _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                    ViewState["AgentID"] = _AgentRegistrationDAL.AgentReqId;
                    DataSet ds = _AgentRegistrationDAL.GetReprocessData();
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
                    ErrorLog.AgentManagementTrace("OBAgStatus | RowCommand-ViewDetails | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: gvBlockAG_RowCommand: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion
        protected void GVPreverification_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVPreverification.PageIndex = e.NewPageIndex;
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.AgentReqId = ViewState["AgentID"].ToString();
                DataSet ds = _AgentRegistrationDAL.GetReprocessData();
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
                ErrorLog.CommonTrace("Page : AgentStatus.cs \nFunction : gvTransactions_PageIndexChanging() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Agent Status');", true);
                return;
            }
        }
        protected void btnTerminate_ServerClick(object sender, EventArgs e)
        {
            ViewState["ActionType"] = Convert.ToString((int)EnumCollection.EnumDBOperationType.OnboardTerminate);
            try
            {
                ErrorLog.AgentManagementTrace("OBAgStatus | btnTerminate_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-OnboardingStatus";
                _auditParams[2] = "btnTerminate";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                foreach (GridViewRow row in gvBlockAG.Rows)
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
                    lblModalHeaderName.Text = "Agent(s) Terminate Reason";
                    TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                    lblconfirm.Text = "Are you sure want to Terminate BC(s)?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Terminate Agent(s)');", true);
                    return;
                }
                ErrorLog.AgentManagementTrace("OBAgStatus | btnTerminate_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : OBAgStatus.cs \nFunction : btnTerminate_ServerClick() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Deactive Agent');", true);
                return;
            }
        }

        #region Activate\Terminate Event Handler
        protected void btnSaveAction_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("OBAgStatus | btnSaveAction_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-OnboardingStatus";
                _auditParams[2] = "btnSaveAction";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                if (TxtRemarks.Text == null || TxtRemarks.Text == "")
                {
                    ModalPopupExtender_Declincard.Show();

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter Remarks', 'Deactive Agent');", true);
                    return;
                }
                else
                {
                    string reason = string.Empty;
                    _successful = 0;
                    _unsuessful = 0;
                    DataTable BCDataSet = (DataTable)ViewState["BCDataSet"];

                    #region  Alert and Log Messages
                    if (!String.IsNullOrEmpty(ViewState["ActionType"].ToString()))
                    {

                        if (ViewState["ActionType"].ToString() == Convert.ToString((int)EnumCollection.EnumDBOperationType.OnboardTerminate))
                        {
                            _strAlertMessage_Header = "Terminate Agent ";
                            _strAlertMessage_Success = "Terminate Process ";
                            _strAlertMessage_UnSuccess = "Terminate Process ";
                            _strAlertMessage_Total = "Total Record Processed for Terminate Agent(s) :  ";
                            _StatusFlag = (int)EnumCollection.EnumDBOperationType.OnboardTerminate;
                            _Flag = 1;
                            _AgentRegistrationDAL.ActivityType = (int)EnumCollection.ActivityType.Terminate;
                        }
                        else if (ViewState["ActionType"].ToString() == Convert.ToString((int)EnumCollection.EnumDBOperationType.Activate))
                        {
                            _strAlertMessage_Header = "Activate Agent";
                            _strAlertMessage_Success = "Activate Process ";
                            _strAlertMessage_UnSuccess = "Activate Process ";
                            _strAlertMessage_Total = "Total Record Processed for Activate Agent(s) :  ";
                            _StatusFlag = (int)EnumCollection.EnumDBOperationType.Activate;
                            _Flag = 1;
                            _AgentRegistrationDAL.ActivityType = (int)EnumCollection.ActivityType.Activate;
                        }
                        else if (ViewState["ActionType"].ToString() == Convert.ToString((int)EnumCollection.EnumDBOperationType.Deactivate))
                        {
                            _strAlertMessage_Header = "De-Activate Agent";
                            _strAlertMessage_Success = "De-Activate Agent Process ";
                            _strAlertMessage_UnSuccess = "De-Activate Agent Process ";
                            _strAlertMessage_Total = "Total Record Processed for De-activate Agent(s) :  ";
                            _StatusFlag = (int)EnumCollection.EnumDBOperationType.Deactivate;
                            _Flag = 1;
                            _AgentRegistrationDAL.ActivityType = (int)EnumCollection.ActivityType.Deactivate;
                        }
                    }
                    #endregion

                    _AgentRegistrationDAL.ActionType = (Convert.ToInt32(_Flag)).ToString();
                    _AgentRegistrationDAL.Flag = _Flag;
                    if (!string.IsNullOrEmpty(TxtRemarks.Text.Trim()))
                    {
                        #region Multiple Section Rows of GridView.
                        if ((BCDataSet != null) && Convert.ToString(ViewState["SelectionType"]) == SelectionType.CheckedAll.ToString())
                        {
                            _fileLineNo = "0";
                            for (int i = 0; i < BCDataSet.Rows.Count; i++)
                            {
                                _AgentRegistrationDAL.AgentReqId = Convert.ToString(BCDataSet.Rows[i]["Agent Id"]);
                                if (ValidateReEditRequest())
                                {
                                    try
                                    {
                                        _reocrdsProcessed = _reocrdsProcessed + 1;
                                        _strBCFullName = Convert.ToString(BCDataSet.Rows[i]["Name"]);
                                        _AgentRegistrationDAL.AgentCode = Convert.ToString(BCDataSet.Rows[i]["Code"]);
                                        _AgentRegistrationDAL.AgentReqId = Convert.ToString(BCDataSet.Rows[i]["Agent Id"]);
                                        _AgentRegistrationDAL.BCstatus = Convert.ToString((int)EnumCollection.Onboarding.Pending);
                                        _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerPending);
                                        _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                                        _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                                        _AgentRegistrationDAL.Flag = _Flag;

                                        _AgentRegistrationDAL.BcRemarks = TxtRemarks.Text.Trim();
                                        _fileLineNo = (Convert.ToInt32(_fileLineNo) + 1).ToString();
                                        _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                                        _dsDeactivateBC = _AgentRegistrationDAL.ChangeAgentOnboardStatus();

                                        if (Convert.ToInt32(Convert.ToString(_dsDeactivateBC.Tables[0].Rows[0][0])) > 0)
                                        {
                                            _successful = _successful + 1;
                                        }

                                    }
                                    catch (Exception Ex)
                                    {
                                        _unsuessful = _unsuessful + 1;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent request already exist for verification.', 'Warning');", true);
                                    return;
                                }
                            }
                            TxtRemarks.Text = string.Empty;
                            ViewState["RecordCountDecine"] = _reocrdsProcessed;
                        }
                        #endregion

                        #region Single Section Row of GridView.
                        else
                        {
                            ErrorLog.AgentManagementTrace("OBAgStatus: btnSaveAction_Click: Request Received.  StatusFlag: " + _StatusFlag + " Flag: " + _Flag + " AgentCode: " + _AgentRegistrationDAL.AgentCode + " AgentReqId: " + _AgentRegistrationDAL.AgentReqId);
                            _fileLineNo = "0";
                            foreach (GridViewRow row in gvBlockAG.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                                    if (chBoxRow.Checked)
                                    {
                                        _AgentRegistrationDAL.AgentReqId = row.Cells[2].Text;
                                        if (ValidateReEditRequest())
                                        {
                                            try
                                            {
                                                _reocrdsProcessed = _reocrdsProcessed + 1;
                                                _AgentRegistrationDAL.AgentCode = row.Cells[6].Text;
                                                _AgentRegistrationDAL.AgentReqId = row.Cells[2].Text;
                                                _AgentRegistrationDAL.BCstatus = Convert.ToString((int)EnumCollection.Onboarding.Pending);
                                                _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerPending);
                                                _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                                                _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                                                _strBCFullName = Convert.ToString(row.Cells[3].Text);
                                                _AgentRegistrationDAL.BcRemarks = TxtRemarks.Text.Trim();
                                                _AgentRegistrationDAL.ClientId = Convert.ToString(row.Cells[3].Text);
                                                _fileLineNo = (Convert.ToInt32(_fileLineNo) + 1).ToString();
                                                _AgentRegistrationDAL.UserName = Session["UserName"].ToString();
                                                _AgentRegistrationDAL.Flag = _Flag;

                                                if (ValidateReEditRequest())
                                                {

                                                    _dsDeactivateBC = _AgentRegistrationDAL.ChangeAgentOnboardStatus();

                                                    if (Convert.ToInt32(Convert.ToString(_dsDeactivateBC.Tables[0].Rows[0][0])) > 0)
                                                    {
                                                        _successful = _successful + 1;
                                                        ErrorLog.AgentManagementTrace("OBAgStatus: btnSaveAction_Click: Successful - OnBoard AG Status.  StatusFlag: " + _StatusFlag + " Flag: " + _Flag + " AgentCode: " + _AgentRegistrationDAL.AgentCode + " AgentReqId: " + _AgentRegistrationDAL.AgentReqId);
                                                    }
                                                    else
                                                    {
                                                        _unsuessful = _unsuessful + 1;
                                                        ErrorLog.AgentManagementTrace("OBAgStatus: btnSaveAction_Click: Failed - OnBoard AG Status.  StatusFlag: " + _StatusFlag + " Flag: " + _Flag + " AgentCode: " + _AgentRegistrationDAL.AgentCode + " AgentReqId: " + _AgentRegistrationDAL.AgentReqId);
                                                    }
                                                }
                                                else
                                                {
                                                    reason = "Reason : Agent request already pending for verification.";
                                                    _unsuessful = _unsuessful + 1;
                                                }
                                            }
                                            catch (Exception Ex)
                                            {
                                                _unsuessful = _unsuessful + 1;
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent request already exist for verification.', 'Warning');", true);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ModalPopupExtender_Declincard.Show();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter the reason for " + _strAlertMessage_Header + "');", true);
                        return;
                    }
                    FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                    TxtRemarks.Text = string.Empty;

                   ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _strAlertMessage_Total + _reocrdsProcessed + " <br/> " + " Successful : " + _successful + " <br/> " + " Unsuccessful : " + _unsuessful + " <br/> " + reason + " ');", true);
                    ViewState["ActionType"] = null;
                    ViewState["SelectionType"] = null;
                    return;
                }
                //ErrorLog.AgentManagementTrace("OBAgStatus | btnSaveAction_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: btnSaveAction_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
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
                ErrorLog.AgentManagementTrace("OBAgStatus | btnCancelAction_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-OnboardingStatus";
                _auditParams[2] = "btnCancelAction";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                string _alertMessage = string.Empty;
                if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.OnboardTerminate.ToString())
                {
                    _alertMessage = "Agent Terminate";
                }
                else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Activate.ToString())
                {
                    _alertMessage = "Agent Activate";
                }


                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Operation has cancelled.','" + _alertMessage + "');", true);
                ModalPopupExtender_Declincard.Hide();
                TxtRemarks.Text = string.Empty;
                ViewState["ActionType"] = null;
                ErrorLog.AgentManagementTrace("OBAgStatus | btnCancelAction_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                //FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                return;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: btnCancelAction_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
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
                        CheckBox CheckBoxAll = gvBlockAG.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvBlockAG.Rows)
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
                        CheckBox _CheckBoxAll = gvBlockAG.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvBlockAG.Rows)
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
                ErrorLog.AgentManagementTrace("OBAgStatus: chBoxSelectRow_CheckedChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox CheckBoxAll = gvBlockAG.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                foreach (GridViewRow row in gvBlockAG.Rows)
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
                ErrorLog.AgentManagementTrace("OBAgStatus: CheckBoxAll_CheckedChanged: Exception: " + Ex.Message);
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
                        CheckBox CheckBoxAll = gvBlockAG.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = true;
                        foreach (GridViewRow row in gvBlockAG.Rows)
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
                        CheckBox CheckBoxAll = gvBlockAG.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = false;
                        foreach (GridViewRow row in gvBlockAG.Rows)
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
                ErrorLog.AgentManagementTrace("OBAgStatus: CheckBoxAllOperationOnPageIndex: Exception: " + Ex.Message);
            }
        }
        #endregion

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("OBAgStatus | BtnCsv_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-OnboardingStatus";
                    _auditParams[2] = "Export-To-CSV";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "Onboard_Agent_Details", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: BtnCsv_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("OBAgStatus | BtnXls_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-OnboardingStatus";
                    _auditParams[2] = "Export-To-Excel";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "Onboard_Agent_Details", dt);
                }
                {
                    //lblRecordCount.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);

                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: BtnXls_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
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
                ErrorLog.AgentManagementTrace("OBAgStatus: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }
        #region Dropdown Events
        #endregion
        protected void Button1_Click(object sender, EventArgs e)
        {
            ModalPopupResponse.Show();
        }
        protected void btnClose_ServerClick(object sender, EventArgs e)
        {
            ModalPopupResponse.Hide();
        }
    }
}