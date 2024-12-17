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

namespace SOR.Pages.Configuration
{
    public partial class BankConfigVeri : System.Web.UI.Page
    {
        #region Property Declaration
        EmailSMSAlertscs _EmailSMSAlertscs = new EmailSMSAlertscs();
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();
        ImportEntity _ImportEntity = new ImportEntity();
        RuleEntity _ruleEntity = new RuleEntity();
        CommonEntity _CommonEntity = new CommonEntity();
        public ExportFormat _exportFormat = null;
        public ExportFormat exportFormat
        {
            get { if (_exportFormat == null) _exportFormat = new ExportFormat(); return _exportFormat; }
            set { _exportFormat = value; }
        }
        #endregion

        #region Variable and Objects Declaration
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
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["UserName"].ToString(), Session["UserRoleID"].ToString(), "BankConfigVeri.aspx", "31");
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
                            txtFrom.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtTo.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                            ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                            UserPermissions.RegisterStartupScriptForNavigationListActive("9", "31");
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
                ErrorLog.CommonTrace("Page : BankConfigVeri.cs \nFunction : Page_Load() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Methods

        public DataTable FillGrid(EnumCollection.EnumBindingType _EnumPermissionType)
        {
            DataTable dt = new DataTable();
            try
            {
                gvBankVerification.DataSource = null;
                gvBankVerification.DataBind();
                _ruleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _ruleEntity.Fromdate = !string.IsNullOrEmpty(txtFrom.Value) ? Convert.ToDateTime(txtFrom.Value).ToString("yyyy-MM-dd") : null;
                _ruleEntity.Todate = !string.IsNullOrEmpty(txtTo.Value) ? Convert.ToDateTime(txtTo.Value).ToString("yyyy-MM-dd") : null;
                _ruleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                dt = _ruleEntity.GetBankConfigurationChecker();
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvBankVerification.Visible = true;
                    gvBankVerification.DataSource = dt;
                    gvBankVerification.DataBind();
                    ViewState["Dt"] = dt;
                }
                else
                {
                    gvBankVerification.DataSource = null;
                    gvBankVerification.DataBind();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.LimitTrace("BankConfigVeri: FillGrid(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return dt;
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
                        _strAlertMessage_Header = "Decline Agent ";
                        _strAlertMessage_Success = "Decline Process ";
                        _strAlertMessage_UnSuccess = "Decline Process ";
                        _strAlertMessage_Total = "Total Record Processed for Decline Agent(s) :  ";
                        _AgentRegistrationDAL.AtStatus = Convert.ToString((int)(EnumCollection.Onboarding.AuthorizerDecline));
                        _Flag = (int)EnumCollection.EnumDBOperationType.Decline;
                    }
                    else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Approve.ToString())
                    {
                        _strAlertMessage_Header = "Approve Agent ";
                        _strAlertMessage_Success = "Approve Process ";
                        _strAlertMessage_UnSuccess = "Approve Process ";
                        _strAlertMessage_Total = "Total Record Processed for Approve Agent(s) :  ";
                        _AgentRegistrationDAL.AtStatus = Convert.ToString((int)(EnumCollection.Onboarding.AuthorizerApprove));
                        _Flag = (int)EnumCollection.EnumDBOperationType.Approve;
                    }
                }
                #endregion
                switch (fromWhere)
                {
                    case "CheckedAll":
                        #region All Rows of GridView.
                        DataTable Dt = (DataTable)ViewState["Dt"];
                        if ((Dt != null) && Convert.ToString(ViewState["SelectionType"]) == SelectionType.CheckedAll.ToString())
                        {
                            _fileLineNo = "0";
                            for (int i = 0; i < Dt.Rows.Count; i++)
                            {
                                try
                                {
                                    _reocrdsProcessed = _reocrdsProcessed + 1;

                                    _ruleEntity.Id = Convert.ToString(Dt.Rows[i]["id"]);
                                    _ruleEntity.ReqId = Convert.ToString(Dt.Rows[i]["reqid"]);
                                    _ruleEntity.Flag = (Convert.ToInt32(_Flag));
                                    _ruleEntity.Remark = TxtRemarks.Text.Trim();
                                    _ruleEntity.UserName = Session["Username"].ToString();
                                    _dsVerification = _ruleEntity.iouBankConfigVerification();

                                    var value = _dsVerification.Tables[0].Rows[0][0];
                                    if (value != DBNull.Value && Convert.ToString(value) == "00")
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
                            foreach (GridViewRow row in gvBankVerification.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                                    if (chBoxRow.Checked)
                                    {
                                        try
                                        {
                                            _reocrdsProcessed = _reocrdsProcessed + 1;
                                            SingleSave(_reocrdsProcessed, _Flag, _Flag, Convert.ToString(Session["Username"]), TxtRemarks.Text.Trim(), row.Cells[1].Text, row.Cells[2].Text);
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
                    //case "RadionButtonClick":

                    //    #region Action Click of Gridview for single insert
                    //    try
                    //    {
                    //        ErrorLog.AgentManagementTrace("BankConfigVeri: CommonSave:  Request : " + _AgentRegistrationDAL.Mstatus + " Received. Username : " + UserName + " RequestId : " + Convert.ToString(ViewState["AgentReqId"]));

                    //        _reocrdsProcessed = 1;
                    //        SingleSave(_reocrdsProcessed, _Flag, _Flag, Convert.ToString(Session["Username"]), txtFinalRemarks.Text.Trim(), Convert.ToString(ViewState["Agent Code"]), Convert.ToString(ViewState["Client Code"]));
                    //    }
                    //    catch (Exception Ex)
                    //    {
                    //        _unsuessful = _unsuessful + 1;
                    //    }
                    //    #endregion
                    //    break;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfigVeri: CommonSave: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        public void SingleSave(int _reocrdsProcessed, int _StatusFlag, int _Flag, string User, string Remarks, string id, string reqid)
        {
            try
            {
                _reocrdsProcessed = _reocrdsProcessed + 1;
                ErrorLog.AgentManagementTrace("BankConfigVeri: SingleSave: Received - Maker Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " id: " + id + " reqid: " + reqid + " Remarks: " + Remarks);
                _ruleEntity.Id = id;
                _ruleEntity.ReqId = reqid;
                _ruleEntity.Flag = (Convert.ToInt32(_Flag));
                _ruleEntity.Remark = Remarks;
                _ruleEntity.UserName = Session["Username"].ToString();

                _dsVerification = _ruleEntity.iouBankConfigVerification();

                if (_dsVerification != null && _dsVerification.Tables[0].Rows.Count > 0)
                {
                    var value = _dsVerification.Tables[0].Rows[0][0];

                    // Check if the value is not DBNull and perform the string comparison
                    if (value != DBNull.Value && Convert.ToString(value) == "00")
                    {
                        // Your logic here

                        string AgentReqId = string.Empty;
                        string _ClientID = string.Empty;
                        AgentReqId = _AgentRegistrationDAL.AgentReqId;
                        _ClientID = _AgentRegistrationDAL.ClientId;
                        if (_Flag == 5)  //Approve
                        {
                            //#region SMS
                            //_EmailSMSAlertscs.FROM = Session["Username"].ToString();
                            //_EmailSMSAlertscs.to = _AgentRegistrationDAL.PersonalContact;
                            //_EmailSMSAlertscs.tempname = "SR24659_BCP3";
                            //_EmailSMSAlertscs.OTPFlag = "0";
                            //_EmailSMSAlertscs.var1 = "SBM";
                            //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                            //ErrorLog.AgentManagementTrace("Page : BankConfigVeri.cs \nFunction : SingleSave() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                            //ErrorLog.SMSTrace("Page : BankConfigVeri.cs \nFunction : SingleSave() => Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.PersonalContact);
                            //_EmailSMSAlertscs.HttpGetRequest();
                            //#endregion

                            //#region EMAIL
                            //_EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                            //_EmailSMSAlertscs.to = _AgentRegistrationDAL.BusinessEmail;
                            //_EmailSMSAlertscs.tempname = "SR24659_EBCP2";
                            //_EmailSMSAlertscs.OTPFlag = "0";
                            //_EmailSMSAlertscs.var1 = "SBM";
                            //_EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                            //ErrorLog.AgentManagementTrace("Page : BankConfigVeri.cs \nFunction : CheckedAll() => Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                            //ErrorLog.SMSTrace("Page : BankConfigVeri.cs \nFunction : CheckedAll() => Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.BusinessEmail);
                            //_EmailSMSAlertscs.HttpGetRequestEmail();
                            //#endregion
                        }
                        if (_Flag == 6) //Decline
                        {
                            
                        }
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
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfigVeri: SingleSave: Failed - Maker Verification. Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: btnSearch_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfigVeri: btnClear_Click: Exception: " + Ex.Message);
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
                foreach (GridViewRow row in gvBankVerification.Rows)
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
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfigVeri: btnApprove_ServerClick: Exception: " + Ex.Message);
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
                foreach (GridViewRow row in gvBankVerification.Rows)
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
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfigVeri: btnDecline_ServerClick: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: btnSaveAction_Click: Exception: " + Ex.Message);
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
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                return;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfigVeri: btnCancelAction_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                ViewState["ActionType"] = null;
                return;
            }
        }
        #endregion

        #region Grid Events
        protected void gvBankVerification_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBankVerification.PageIndex = e.NewPageIndex;
                if (ViewState["Dt"] != null)
                {
                    DataTable dsTemp = (DataTable)ViewState["Dt"];
                    if (dsTemp != null && dsTemp.Rows.Count > 0)
                    {
                        gvBankVerification.Visible = true;
                        gvBankVerification.DataSource = dsTemp;
                        gvBankVerification.DataBind();

                        btnApprove.Visible = true;
                        btnDecline.Visible = true;
                        //btnTerminate.Visible = true;
                        lblRecordsTotal.Text = "Total " + Convert.ToString(dsTemp.Rows.Count) + " Record(s) Found.";
                    }
                    else
                    {
                        gvBankVerification.Visible = false;
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: gvBankVerification_PageIndexChanging: Exception: " + Ex.Message);
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
                        CheckBox CheckBoxAll = gvBankVerification.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvBankVerification.Rows)
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
                        CheckBox _CheckBoxAll = gvBankVerification.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvBankVerification.Rows)
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: chBoxSelectRow_CheckedChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox CheckBoxAll = gvBankVerification.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                foreach (GridViewRow row in gvBankVerification.Rows)
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: CheckBoxAll_CheckedChanged: Exception: " + Ex.Message);
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
                        CheckBox CheckBoxAll = gvBankVerification.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = true;
                        foreach (GridViewRow row in gvBankVerification.Rows)
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
                        CheckBox CheckBoxAll = gvBankVerification.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = false;
                        foreach (GridViewRow row in gvBankVerification.Rows)
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: CheckBoxAllOperationOnPageIndex: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: btnView_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: EyeImage_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: EyeImage1_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: EyeImage3_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: btnViewResp_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: btnViewDownload_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: ImageButton1_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: imgbtnform_Click: Exception: " + Ex.Message);
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
                    ErrorLog.AgentManagementTrace("BankConfigVeri: btnSubmitDetails_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: ddlState_SelectedIndexChanged: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: SetPageFiltersExport: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfigVeri: Unnamed_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataTable dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "Block Agent L2 Details", ds);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfigVeri: BtnCsv_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataTable dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "Block Agent L2 Details", ds);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfigVeri: BtnXls_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
    }
}