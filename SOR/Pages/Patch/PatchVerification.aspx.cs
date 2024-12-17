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

namespace SOR.Pages.Patch
{
    public partial class PatchVerification : System.Web.UI.Page
    {
        #region Property Declaration
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();
        ImportEntity _ImportEntity = new ImportEntity();
        PatchEntity _PatchEntity = new PatchEntity();
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
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserName"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["UserName"].ToString(), Session["UserRoleID"].ToString(), "PatchVerification.aspx", "35");
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
                            txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                            FillGrid(EnumCollection.EnumBindingType.BindGrid);
                            ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                            UserPermissions.RegisterStartupScriptForNavigationListActive("11", "35");
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
                ErrorLog.CommonTrace("Page : PatchVerification.cs \nFunction : Page_Load() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Methods

        //private void BindDropdownsBC()
        //{
        //    try
        //    {
        //        ddlBCCode.Items.Clear();
        //        ddlBCCode.DataSource = null;
        //        ddlBCCode.DataBind();
        //        _AgentRegistrationDAL.UserName = Session["Username"].ToString();
        //        _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
        //        _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
        //        _AgentRegistrationDAL.IsRemoved = "0";
        //        _AgentRegistrationDAL.IsActive = "0";
        //         _AgentRegistrationDAL.IsdocUploaded = "1";
        //        _AgentRegistrationDAL.VerificationStatus = 0;
        //        _AgentRegistrationDAL.BCCode = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;

        //        DataTable ds = _AgentRegistrationDAL.GetBC(_AgentRegistrationDAL.UserName, 0, 1, 0, ClientID, 1);

        //        if (ds != null && ds.Rows.Count > 0 && ds.Rows.Count > 0)
        //        {
        //            if (ds.Rows.Count == 1)
        //            {
        //                ddlBCCode.Items.Clear();
        //                ddlBCCode.DataSource = ds.Copy();
        //                ddlBCCode.DataTextField = "BCNameDetails";
        //                ddlBCCode.DataValueField = "BCCode";
        //                ddlBCCode.DataBind();
        //                ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
        //            }
        //            else
        //            {
        //                ddlBCCode.Items.Clear();
        //                ddlBCCode.DataSource = ds.Copy();
        //                ddlBCCode.DataTextField = "BCNameDetails";
        //                ddlBCCode.DataValueField = "BCCode";
        //                ddlBCCode.DataBind();
        //                ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
        //            }
        //        }
        //        else
        //        {
        //            ddlBCCode.DataSource = null;
        //            ddlBCCode.DataBind();
        //            ddlBCCode.Items.Insert(0, new ListItem("-- Select --", "0"));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("PatchVerification: BindDropdownsBC: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //    }
        //}

        //private void BindDropdownsAgent()
        //{
        //    try
        //    {
        //        ddlAgentCode.Items.Clear();
        //        ddlAgentCode.DataSource = null;
        //        ddlAgentCode.DataBind();
        //        _AgentRegistrationDAL.UserName = Session["Username"].ToString();
        //        _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.EnableMakerChecker;// EnableRoles;
        //        _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
        //        _AgentRegistrationDAL.IsRemoved = "0";
        //        _AgentRegistrationDAL.IsActive = "0";
        //        _AgentRegistrationDAL.IsdocUploaded = "1";
        //        _AgentRegistrationDAL.VerificationStatus = 0;
        //        _AgentRegistrationDAL.BCCode = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;
        //        _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
        //        _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
        //        DataSet ds = _AgentRegistrationDAL.BindAgentVerifyddl();
        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count == 1)
        //            {
        //                ddlAgentCode.Items.Clear();
        //                ddlAgentCode.DataSource = ds.Tables[0].Copy();
        //                ddlAgentCode.DataTextField = "AgentName";
        //                ddlAgentCode.DataValueField = "ID";
        //                ddlAgentCode.DataBind();
        //                ddlAgentCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
        //            }
        //            else
        //            {
        //                ddlAgentCode.Items.Clear();
        //                ddlAgentCode.DataSource = ds.Tables[0].Copy();
        //                ddlAgentCode.DataTextField = "AgentName";
        //                ddlAgentCode.DataValueField = "ID";
        //                ddlAgentCode.DataBind();
        //                ddlAgentCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
        //            }
        //        }
        //        else
        //        {
        //            ddlAgentCode.DataSource = null;
        //            ddlAgentCode.DataBind();
        //            ddlAgentCode.Items.Insert(0, new ListItem("-- Select --", "0"));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("PatchVerification: BindDropdownsAgent: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //    }
        //}

        public DataSet FillGrid(EnumCollection.EnumBindingType _EnumBindingType, string sortExpression = null)
        {
            DataSet ds = new DataSet();
            try
            {
                gvVerification.DataSource = null;
                gvVerification.DataBind();
                SetPropertise();
                ds = _PatchEntity.GetVerificationDetails();

                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            DataView dv = ds.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";
                            dv.Sort = sortExpression + " " + this.SortDirection;
                            gvVerification.DataSource = dv;
                            gvVerification.DataBind();
                            gvVerification.Visible = true;
                        }
                        else
                        {
                            gvVerification.DataSource = ds.Tables[0];
                            gvVerification.DataBind();
                            gvVerification.Visible = true;
                            ViewState["DataSet"] = ds.Tables[0];
                        }
                    }
                    else
                    {
                        gvVerification.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Script", "alert('No Data Found in Search Criteria. Try again', 'Warning');", true);
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("PatchVerification: FillGrid(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Something went wrong. Try again', 'Warning');</script>", false);
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
                        _strAlertMessage_Header = "Decline ";
                        _strAlertMessage_Success = "Decline Process ";
                        _strAlertMessage_UnSuccess = "Decline Process ";
                        _strAlertMessage_Total = "Total Record Processed for Decline :  ";
                        _Flag = (int)EnumCollection.EnumDBOperationType.Decline;
                    }
                    else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Approve.ToString())
                    {
                        _strAlertMessage_Header = "Approve ";
                        _strAlertMessage_Success = "Approve Process ";
                        _strAlertMessage_UnSuccess = "Approve Process ";
                        _strAlertMessage_Total = "Total Record Processed for Approve :  ";
                        _Flag = (int)EnumCollection.EnumDBOperationType.Approve;
                    }
                }
                #endregion
                switch (fromWhere)
                {
                    case "CheckedAll":
                        #region All Rows of GridView.
                        DataTable DS = (DataTable)ViewState["DataSet"];
                        if ((DS != null) && Convert.ToString(ViewState["SelectionType"]) == SelectionType.CheckedAll.ToString())
                        {
                            _fileLineNo = "0";
                            for (int i = 0; i < DS.Rows.Count; i++)
                            {
                                try
                                {
                                    _reocrdsProcessed = _reocrdsProcessed + 1;

                                    _PatchEntity.UserName = Session["Username"].ToString();
                                    _PatchEntity.Id = Convert.ToString(DS.Rows[i]["id"]);
                                    _PatchEntity.Remarks = TxtRemarks.Text.Trim();
                                    _PatchEntity.Flag = (Convert.ToInt32(_Flag));
                                    _dsVerification = _PatchEntity.ChangePatchStatus();
                                    
                                    if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                                    {
                                        _dsVerification = _PatchEntity.ChangePatchStatus();
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Request Declined Successfully', 'Patch Verification');", true);
                                        ErrorLog.AgentManagementTrace("PatchVerification: Commonsave:CheckAll(): Successful - Patch Verification. StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " id: " + _PatchEntity.Id + " UserName: " + Session["Username"].ToString());
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
                            foreach (GridViewRow row in gvVerification.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                                    if (chBoxRow.Checked)
                                    {
                                        try
                                        {
                                            string verificationstatus = string.Empty;

                                            verificationstatus = row.Cells[5].Text;
                                            if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString() && verificationstatus == "Prod Pending")
                                            { 
                                                _reocrdsProcessed = _reocrdsProcessed + 1;
                                                SingleSave(_reocrdsProcessed, Convert.ToString(Session["Username"]), row.Cells[1].Text, TxtRemarks.Text.Trim(), 4);
                                            }
                                            else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Approve.ToString() && verificationstatus == "Prod Pending")
                                            {
                                            
                                                _reocrdsProcessed = _reocrdsProcessed + 1;
                                                SingleSave(_reocrdsProcessed, Convert.ToString(Session["Username"]), row.Cells[1].Text, TxtRemarks.Text.Trim(), 3);
                                            }
                                            else
                                            {
                                                _reocrdsProcessed = _reocrdsProcessed + 1;
                                                SingleSave(_reocrdsProcessed, Convert.ToString(Session["Username"]), row.Cells[1].Text, TxtRemarks.Text.Trim(), _Flag);
                                            }
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
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: CommonSave: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        public void SingleSave(int _reocrdsProcessed, string User, string id, string Remarks, int _Flag)
        {
            try
            {
                _PatchEntity.UserName = User;
                _PatchEntity.Id = Convert.ToString(id);
                _PatchEntity.Remarks = Remarks;
                _PatchEntity.Flag = _Flag;
                _dsVerification = _PatchEntity.ChangePatchStatus();

                if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                {
                    _dsVerification = _PatchEntity.ChangePatchStatus();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Request Declined Successfully', 'Patch Verification');", true);
                    ErrorLog.AgentManagementTrace("PatchVerification: Commonsave:SingleSave(): Successful - Patch Verification. StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " id: " + _PatchEntity.Id + " UserName: " + Session["Username"].ToString());
                }
                
                if (Convert.ToInt32(Convert.ToString(_dsVerification.Tables[0].Rows[0][0])) > 0)
                {
                    _successful = _successful + 1;
                }
                else
                {
                    _unsuessful = _unsuessful + 1;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Approve/Decline Request Unsuccessful', 'Agent Verification');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: SingleSave: Failed - Maker Verification. Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        #endregion

        #region Search And Clear
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: btnSearch_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //ddlBCCode.SelectedValue = "0";
                //ddlAgentCode.SelectedValue = "0";
                //ddlActivityType.SelectedValue = "-1";
                //ddlFileID.ClearSelection();

                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: btnClear_Click: Exception: " + Ex.Message);
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
                foreach (GridViewRow row in gvVerification.Rows)
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
                    lblModalHeaderName.Text = "Approve Reason";
                    TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                    lblconfirm.Text = "Are you sure want to Approve?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Approve');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: btnApprove_ServerClick: Exception: " + Ex.Message);
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
                foreach (GridViewRow row in gvVerification.Rows)
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
                    lblModalHeaderName.Text = "Decline Reason";
                    TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                    lblconfirm.Text = "Are you sure want to Decline?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Decline');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: btnDecline_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter Remarks', 'Verification');", true);
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

                    FillGrid(EnumCollection.EnumBindingType.BindGrid);

                    TxtRemarks.Text = string.Empty;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _strAlertMessage_Total + _reocrdsProcessed + "  Successful : " + _successful + "  Unsuccessful : " + _unsuessful + " ');", true);
                    ViewState["ActionType"] = null;
                    ViewState["SelectionType"] = null;
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: btnSaveAction_Click: Exception: " + Ex.Message);
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
                string _alertMessage = string.Empty;
                if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Approve.ToString())
                {
                    _alertMessage = "Approve";
                }
                else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                {
                    _alertMessage = "Decline";
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Operation has cancelled.','" + _alertMessage + "');", true);
                ModalPopupExtender_Declincard.Hide();
                TxtRemarks.Text = string.Empty;
                ViewState["ActionType"] = null;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                return;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgLvl1: btnCancelAction_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                ViewState["ActionType"] = null;
                return;
            }
        }
        #endregion

        #region Grid Events
        protected void gvVerification_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvVerification.PageIndex = e.NewPageIndex;
                if (ViewState["AgentDataSet"] != null)
                {
                    DataTable dsTemp = (DataTable)ViewState["AgentDataSet"];
                    if (dsTemp != null && dsTemp.Rows.Count > 0)
                    {
                        gvVerification.Visible = true;
                        gvVerification.DataSource = dsTemp;
                        gvVerification.DataBind();

                        btnApprove.Visible = true;
                        btnDecline.Visible = true;
                        //btnTerminate.Visible = true;
                        lblRecordsTotal.Text = "Total " + Convert.ToString(dsTemp.Rows.Count) + " Record(s) Found.";
                    }
                    else
                    {
                        gvVerification.Visible = false;
                        btnApprove.Visible = false;
                        btnDecline.Visible = false;
                        //btnTerminate.Visible = false;
                        lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                    }
                }
                else
                    FillGrid(EnumCollection.EnumBindingType.BindGrid);
                CheckBoxAllOperationOnPageIndex();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: gvVerification_PageIndexChanging: Exception: " + Ex.Message);
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
                        CheckBox CheckBoxAll = gvVerification.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvVerification.Rows)
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
                        CheckBox _CheckBoxAll = gvVerification.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvVerification.Rows)
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
                ErrorLog.AgentManagementTrace("PatchVerification: chBoxSelectRow_CheckedChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox CheckBoxAll = gvVerification.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                foreach (GridViewRow row in gvVerification.Rows)
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
                ErrorLog.AgentManagementTrace("PatchVerification: CheckBoxAll_CheckedChanged: Exception: " + Ex.Message);
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
                        CheckBox CheckBoxAll = gvVerification.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = true;
                        foreach (GridViewRow row in gvVerification.Rows)
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
                        CheckBox CheckBoxAll = gvVerification.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = false;
                        foreach (GridViewRow row in gvVerification.Rows)
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
                ErrorLog.AgentManagementTrace("PatchVerification: CheckBoxAllOperationOnPageIndex: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("PatchVerification: btnView_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("PatchVerification: EyeImage_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void EyeImage1_Click(object sender, ImageClickEventArgs e)
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
                    string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();
                    strURL = Ds.Tables[1].Rows[0]["AddressProofDocument"].ToString();

                    string pdfPath = strURL;
                    Session["pdfPath"] = pdfPath;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PdfExport.aspx');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: EyeImage1_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void EyeImage3_Click(object sender, ImageClickEventArgs e)
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
                    string fileName = Ds.Tables[2].Rows[0]["SignatureProofType"].ToString();
                    strURL = Ds.Tables[2].Rows[0]["SignatureProofDocument"].ToString();
                    string pdfPath = strURL;
                    Session["pdfPath"] = pdfPath;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PdfExport.aspx');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: EyeImage3_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

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
                ErrorLog.AgentManagementTrace("AgLvl1: btnViewResp_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("PatchVerification: btnViewDownload_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        //protected void ddlBCCode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        BindDropdownsAgent();
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("PatchVerification: ddlBCCode_SelectedIndexChanged: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //    }
        //}

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
                ErrorLog.AgentManagementTrace("PatchVerification: ImageButton1_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("PatchVerification: imgbtnform_Click: Exception: " + Ex.Message);
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
                        FillGrid(EnumCollection.EnumBindingType.BindGrid);
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
                }
                catch (Exception Ex)
                {
                    ErrorLog.AgentManagementTrace("PatchVerification: btnSubmitDetails_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("PatchVerification: ddlState_SelectedIndexChanged: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("AgLvl1: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }

        #endregion

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string pdfPath = Server.MapPath("~/Thumbnail/Aadhar/document-1_220422_110414 (1) (1).pdf");
                Session["pdfPath"] = pdfPath;
                string script = "<script type='text/javascript'>window.open('" + "PdfExport.aspx" + "')</script>";
                this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: Unnamed_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        protected void gvVerification_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string verificationStatus = DataBinder.Eval(e.Row.DataItem, "verificationstatus") as string;

                    CheckBox chBoxSelectRow = e.Row.FindControl("chBoxSelectRow") as CheckBox;

                    if (chBoxSelectRow != null)
                    {
                        if (verificationStatus == "CUG Pending" || verificationStatus == "Prod Pending")
                        {
                            chBoxSelectRow.Visible = true;
                        }
                        else
                        {
                            chBoxSelectRow.Visible = false;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning: gvVerification_RowDataBound(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                throw;
            }
        }


        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                //_AgentRegistrationDAL.Flag = ((int)EnumCollection.EnumPermissionType.AgL2Export);
                //DataSet dt = _AgentRegistrationDAL.GetAgentDetailsToProcessOnboaring();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "L3 Approval Business Correspondents Details", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: BtnCsv_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                //_AgentRegistrationDAL.Flag = ((int)EnumCollection.EnumPermissionType.AgL2Export);
                //DataSet dt = _AgentRegistrationDAL.GetAgentDetailsToProcessOnboaring();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "L3 Approval Business Correspondents Details", dt);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("PatchVerification: BtnXls_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
       
        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
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
                ErrorLog.AgentManagementTrace("AgLvl1: btnView_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BulkApprove_ServerClick(object sender, EventArgs e)
        {
            try
            {
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
                                    //ErrorLog.AgentManagementTrace("Page : Aglvl3.cs \nFunction : BulkApprove_ServerClick() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                                    //ErrorLog.SMSTrace("Page : Aglvl3.cs \nFunction : BulkApprove_ServerClick() => Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.PersonalContact);
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
                                    // ErrorLog.AgentManagementTrace("PatchVerification: Commonsave:CheckAll(): Successful - Authorizer Verification. StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
                                }
                                else
                                {
                                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Registration Unscuccessful : '" + status + " ');", true);
                                   // ErrorLog.AgentManagementTrace("PatchVerification: Commonsave:CheckAll(): Failed - Authorizer Verification. Agent Already Terminated ");
                                   // ErrorLog.AgentManagementTrace("PatchVerification: Commonsave:CheckAll(): Successful - Authorizer Verification. StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentrequestID: " + _AgentRegistrationDAL.AgentReqId + " User: " + User + "ActivityType: " + _ActivityType);
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
                    FillGrid(EnumCollection.EnumBindingType.BindGrid);
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
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approve Successfully', 'Verification Level 3-agent');", true);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                //}
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgLvl1: BulkApprove_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        }


        protected void BulkDecline_ServerClick(object sender, EventArgs e)
        {
            try
            {
                _AgentRegistrationDAL.Mstatus = Convert.ToString((int)(EnumCollection.Onboarding.MakerDecline));
                _AgentRegistrationDAL.ChStatus = Convert.ToString((int)(EnumCollection.Onboarding.CheckerPending));
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.MakerRemark = txtResone.Text;
                _AgentRegistrationDAL.ActionType = "8";
                _AgentRegistrationDAL.FileID = Session["FILEID"].ToString();
                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatusBulk();
                if (_dsVerification != null && _dsVerification.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Decline Successfully', 'Verification Level 3-agent');", true);
                    ErrorLog.AgentManagementTrace("Page : AgLvl3.cs \nFunction : BulkDecline_ServerClick() => SMS/Email sending process started... Count :" + _dsVerification.Tables[1].Rows.Count);
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
                        ErrorLog.AgentManagementTrace("Page : Aglvl3.cs \nFunction : BulkDecline_ServerClick() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                        ErrorLog.SMSTrace("Page : Aglvl3.cs \nFunction : BulkDecline_ServerClick() => Details forwarded for SMS Preparation. MobileNo : " + Convert.ToString(_dsVerification.Tables[1].Rows[i]["ContactNo"]));
                        _EmailSMSAlertscs.HttpGetRequest();
                        #endregion
                        ErrorLog.AgentManagementTrace("Page : AgLvl3.cs \nFunction : BulkDecline_ServerClick() => Email sending process started for Email...");
                        #region EMAIL
                        _EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                        _EmailSMSAlertscs.to = Convert.ToString(_dsVerification.Tables[1].Rows[i]["PersonalEmailID"]);
                        _EmailSMSAlertscs.tempname = "SR24659_EBCP3";
                        _EmailSMSAlertscs.OTPFlag = "0";
                        _EmailSMSAlertscs.var1 = "SBM";
                        _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                        ErrorLog.AgentManagementTrace("Page : Aglvl3.cs \nFunction : BulkDecline_ServerClick() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                        ErrorLog.SMSTrace("Page : Aglvl3.cs \nFunction : BulkDecline_ServerClick() => Reistration Details forwarded for Email Preparation. Email : " + Convert.ToString(_dsVerification.Tables[1].Rows[i]["PersonalEmailID"]));
                        _EmailSMSAlertscs.HttpGetRequestEmail();
                        #endregion
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgLvl1: BulkDecline_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        }


        protected void BulkDecline_SerHJHJHJverClick(object sender, EventArgs e)
        {
            try
            {
                _AgentRegistrationDAL.Mstatus = Convert.ToString((int)(EnumCollection.Onboarding.MakerDecline));
                _AgentRegistrationDAL.ChStatus = Convert.ToString((int)(EnumCollection.Onboarding.CheckerPending));
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.MakerRemark = txtResone.Text;
                _AgentRegistrationDAL.ActionType = "8";
                _AgentRegistrationDAL.FileID = Session["FILEID"].ToString();
                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatusBulk();
                if (_dsVerification != null && _dsVerification.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Decline Successfully', 'Verification');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgLvl1: BulkDecline_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
        }

        protected void bulkClose_ServerClick(object sender, EventArgs e)
        {
            ModalPopupExtender_EditRole.Hide();
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);
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
        #region setproperty
        private void SetPropertise()
        {
            try
            {
                _PatchEntity.UserName = Session["Username"].ToString();
                _PatchEntity.FromDate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                _PatchEntity.ToDate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                _PatchEntity.Flag = Convert.ToInt32(EnumCollection.EnumBindingType.BindGrid);
                //importEntity.FileDescID = importEntity.FileDescID = Convert.ToString((int)EnumCollection.EnumFileDesciption.UploadRestrictedPIN);
                //importEntity.FileStatus = (ddlFileTypeStatus.SelectedValue.ToString() != null && ddlFileTypeStatus.SelectedValue.ToString() != "" ? ddlFileTypeStatus.SelectedValue.ToString() : "0");
            }
            catch (Exception Ex)
            {
                ErrorLog.UploadTrace("Versioning: SetPropertise(): Exception: " + Ex.Message);
                ErrorLog.UploadError(Ex);
                throw;
            }
        }
        #endregion
    }
}