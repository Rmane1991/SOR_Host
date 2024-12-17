using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessAccessLayer;
using AppLogger;
using System.Data;
using System.IO;
using System.Configuration;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PayRakamSBM.Pages.Agent
{
    public partial class AgPreVerification : System.Web.UI.Page
    {
        #region Property Declaration
        EmailSMSAlertscs _EmailSMSAlertscs = new EmailSMSAlertscs();
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();
        EmailAlerts _EmailAlerts = new EmailAlerts();
        ImportEntity _ImportEntity = new ImportEntity();
        public ExportFormat _exportFormat = null;
        public ExportFormat exportFormat
        {
            get { if (_exportFormat == null) _exportFormat = new ExportFormat(); return _exportFormat; }
            set { _exportFormat = value; }
        }
        #endregion

        #region Variable and Objects
        string UserName { get; set; }
        public bool HasPagePermission { get; private set; }

        public string pathId, PathAdd, PathSig;
        DataSet _dsVerification = null;

        // User Credentials
        string[] _auditParams = new string[4];
        string[] _AgentActiveParams = new string[8];

        int _successful = 0,
            _unsuessful = 0,
            _reocrdsProcessed = 0,
            _DeclineCardCount = 0,
            _Flag = 0;
        string _strAlertMessage_Header = null,
               _strAlertMessage_Success = null,
               _strAlertMessage_UnSuccess = null,
               _strAlertMessage_Total = null,
               _fileLineNo = null;
        public enum SelectionType
        {
            CheckedAll = 1,
            UnCheckAll = 2
        }
        string
       _FromDate = null,
       _ToDate = null,
       _AgentName = null,
       _AgentId = null,
        ClientID = null,
       _MobileNumber = null,
       _clientId = null,
       _FranchiseId = null,
       _Status = null,
       _ChannelID = null,
       // _Flag = null,
       _Username = null,
          CreatedBy = null;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "AgPreVerification.aspx", "41");
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
                        //  UserPermissions.RegisterStartupScriptForNavigationListActive("4", "41");

                        if (!IsPostBack && HasPagePermission)
                        {
                            Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                            BindDropdownsBC();
                            BindFileId();
                            FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                            FillGridBulk(EnumCollection.EnumBindingType.BindGrid);
                            ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                            UserPermissions.RegisterStartupScriptForNavigationList("4", "41", "Manual", "docket-tab");
                        }
                        else
                        {
                            if (hid1.Value == "aaa")
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#ModalAgent').modal('show')", true);
                                //ModalAgent.Visible = true;
                            }

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
                    catch (ThreadAbortException Ex)
                    {
                        ErrorLog.AgentManagementTrace("Pre-Verification: Page_Load: Exception: " + Ex.Message);
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: Page_Load: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Bind Masters
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
                ErrorLog.AgentManagementTrace("Pre-Verification: BindDropdownsBC: Exception: " + Ex.Message);
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
                _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.EnableMakerChecker;
                _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
                _AgentRegistrationDAL.IsRemoved = "0";
                _AgentRegistrationDAL.IsActive = "0";
                _AgentRegistrationDAL.IsdocUploaded = "1";
                _AgentRegistrationDAL.VerificationStatus = 0;
                _AgentRegistrationDAL.BCCode = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;
                _AgentRegistrationDAL.BCstatus = ((int)EnumCollection.EnumDBOperationType.Approve).ToString();
                _AgentRegistrationDAL.Mstatus = ((int)EnumCollection.Onboarding.MakerPending).ToString();
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
                ErrorLog.AgentManagementTrace("Pre-Verification: BindDropdownsAgent: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        public DataSet FillGrid(EnumCollection.EnumPermissionType _EnumPermissionType)
        {
            DataSet ds = new DataSet();
            try
            {
                gvAgent.DataSource = null;
                gvAgent.DataBind();
                lblRecordsTotal.Text = "";
                ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();

                _AgentRegistrationDAL.Flag = ((int)EnumCollection.EnumPermissionType.EnableMakerChecker);
                _AgentRegistrationDAL.BCCode = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;
                _AgentRegistrationDAL.AgentReqId = ddlAgentCode.SelectedValue != "0" ? ddlAgentCode.SelectedValue : null;
                _AgentRegistrationDAL.Activity = ddlActivityType.SelectedValue != "-1" ? ddlActivityType.SelectedValue : null;
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.IsRemoved = "0";
                _AgentRegistrationDAL.IsActive = "0";
                _AgentRegistrationDAL.IsdocUploaded = "1";
                _AgentRegistrationDAL.VerificationStatus = 1;
                //_AgentRegistrationDAL.BCstatus = ((int)EnumCollection.EnumDBOperationType.Approve).ToString();
                _AgentRegistrationDAL.BCstatus = ((int)EnumCollection.Onboarding.Pending).ToString();

                ds = _AgentRegistrationDAL.GetAgentDetailsToProcessOnboaring();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvAgent.Visible = true;
                    gvAgent.DataSource = ds.Tables[0];
                    gvAgent.DataBind();
                    btnApprove.Visible = true;
                    btnDecline.Visible = true;
                    panelGrid.Visible = true;
                    ViewState["AgentDataSet"] = ds.Tables[0];
                    lblRecordsTotal.Text = "Total " + Convert.ToString(ds.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    gvAgent.Visible = false;
                    btnApprove.Visible = false;
                    btnDecline.Visible = false;
                    panelGrid.Visible = false;
                    panelGrid.Visible = false;
                    lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: FillGrid: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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
                        _strAlertMessage_Header = "Decline Agent ";
                        _strAlertMessage_Success = "Decline Process ";
                        _strAlertMessage_UnSuccess = "Decline Process ";
                        _strAlertMessage_Total = "Total Record Processed for Decline Agent(s) :  ";
                        _AgentRegistrationDAL.BCstatus = Convert.ToString((int)(EnumCollection.Onboarding.Decline));
                        _Flag = (int)EnumCollection.EnumDBOperationType.BCDecline;
                    }
                    else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Approve.ToString())
                    {
                        _strAlertMessage_Header = "Approve Agent ";
                        _strAlertMessage_Success = "Approve Process ";
                        _strAlertMessage_UnSuccess = "Approve Process ";
                        _strAlertMessage_Total = "Total Record Processed for Approve Agent(s) :  ";
                        _AgentRegistrationDAL.BCstatus = Convert.ToString((int)(EnumCollection.Onboarding.Approve));
                        _Flag = (int)EnumCollection.EnumDBOperationType.BCApprove;
                        _AgentRegistrationDAL.Mstatus = Convert.ToString((int)(EnumCollection.Onboarding.MakerPending));

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
                                    _reocrdsProcessed = _reocrdsProcessed + 1;
                                    //_AgentEntity.AgentID = Convert.ToString(AgentDataSet.Rows[i]["AgentReqID"]);
                                    _AgentRegistrationDAL.AgentReqId = Convert.ToString(AgentDataSet.Rows[i]["Agent ID"]);
                                    _AgentRegistrationDAL.FirstName = Convert.ToString(AgentDataSet.Rows[i]["Agent Name"]);
                                    _AgentRegistrationDAL.BusinessEmail = Convert.ToString(AgentDataSet.Rows[i]["Email"]);
                                    _AgentRegistrationDAL.PersonalContact = Convert.ToString(AgentDataSet.Rows[i]["Mobile No"]);
                                    _AgentRegistrationDAL.ClientId = Convert.ToString(AgentDataSet.Rows[i]["Client ID"]);
                                    _AgentRegistrationDAL._fileLineNo = (Convert.ToInt32(_fileLineNo) + 1).ToString();
                                    _AgentRegistrationDAL.BcRemarks = TxtRemarks.Text.Trim();
                                    _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                                    _AgentRegistrationDAL.ActionType = (Convert.ToInt32(_Flag)).ToString();
                                    _AgentRegistrationDAL.Flag = 2;
                                    _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                                    if (Convert.ToInt32(Convert.ToString(_dsVerification.Tables[0].Rows[0][0])) > 0)
                                    {
                                        _successful = _successful + 1;
                                    }
                                    else
                                    {
                                        _unsuessful = _unsuessful + 1;
                                    }
                                    
                                    if (_Flag == 13)
                                    {
                                        #region SMS
                                        _EmailSMSAlertscs.FROM = Session["Username"].ToString();
                                        _EmailSMSAlertscs.to = _AgentRegistrationDAL.PersonalContact;
                                        _EmailSMSAlertscs.tempname = "SR24659_BCP2";
                                        _EmailSMSAlertscs.OTPFlag = "0";
                                        _EmailSMSAlertscs.var1 = "SBM";
                                        _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                                        ErrorLog.AgentManagementTrace("Page : AgPreVerification.cs \nFunction : CheckedAll() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                                        ErrorLog.SMSTrace("Page : AgPreVerification.cs \nFunction : CheckedAll() => Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.PersonalContact);
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
                                            _reocrdsProcessed = _reocrdsProcessed + 1;
                                            SingleSave(_reocrdsProcessed, _Flag, _Flag, row.Cells[3].Text, row.Cells[6].Text, Convert.ToString(Session["Username"]), TxtRemarks.Text.Trim(), row.Cells[24].Text, row.Cells[25].Text, row.Cells[7].Text);
                                            //SingleSave(_reocrdsProcessed, _Flag, _Flag, row.Cells[3].Text, row.Cells[6].Text, Convert.ToString(Session["Username"]), TxtRemarks.Text.Trim(), row.Cells[10].Text, row.Cells[9].Text, row.Cells[7].Text);
                                            //SingleSave(_reocrdsProcessed, _Flag, _Flag, row.Cells[2].Text, row.Cells[3].Text, Convert.ToString(Session["Username"]), TxtRemarks.Text.Trim(), row.Cells[7].Text, row.Cells[6].Text, row.Cells[4].Text);
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
                            ErrorLog.AgentManagementTrace("Pre-Verification: CommonSave:  Request : " + _AgentRegistrationDAL.Mstatus + " Received. Username : " + UserName + " RequestId : " + Convert.ToString(ViewState["AgentReqId"]));
                            _reocrdsProcessed = 1;
                            SingleSave(_reocrdsProcessed, _Flag, _Flag, Convert.ToString(ViewState["AgentReqId"]), txtAgentName.Text, Convert.ToString(Session["Username"]), txtFinalRemarks.Text.Trim(), ViewState["ReceiverEmailID"].ToString(), ViewState["ReceiverContactNo"].ToString(), Convert.ToString(ViewState["ClientID"]));

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
                ErrorLog.AgentManagementTrace("Pre-Verification: CommonSave: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        public void SingleSave(int _reocrdsProcessed, int _StatusFlag, int _Flag, string AgentCode, string _strAgentFullName, string User, string Remarks, string ReceiverEmailID, string ContactNo, string _ClientCode)
        {
            try
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: SingleSave: Received - Maker Verification.  StatusFlag: " + _StatusFlag + " Flag: " + _Flag + " AgentCode: " + AgentCode + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                _reocrdsProcessed = _reocrdsProcessed + 1;
                _AgentRegistrationDAL.ActionType = (Convert.ToInt16(_Flag)).ToString();
                _AgentRegistrationDAL.Flag = _Flag;
                _AgentRegistrationDAL.FirstName = _strAgentFullName;
                _AgentRegistrationDAL.AgentReqId = AgentCode;
                _AgentRegistrationDAL.BusinessEmail = ReceiverEmailID;
                _AgentRegistrationDAL.PersonalContact = ContactNo;
                _AgentRegistrationDAL.ClientId = _ClientCode;
                _AgentRegistrationDAL.UserName = User;
                _AgentRegistrationDAL.BcRemarks = Remarks;
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatus();
                if (Convert.ToInt32(Convert.ToString(_dsVerification.Tables[0].Rows[0][0])) > 0)
                {
                    _successful = _successful + 1;
                    ErrorLog.AgentManagementTrace("Pre-Verification: SingleSave: Successful - Maker Verification.  StatusFlag: " + _StatusFlag + " Flag: " + _Flag + " AgentCode: " + AgentCode + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                    if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                    {
                        #region SMS
                        _EmailSMSAlertscs.FROM = Session["Username"].ToString();
                        _EmailSMSAlertscs.to = _AgentRegistrationDAL.PersonalContact;
                        _EmailSMSAlertscs.tempname = "SR24659_BCP2";
                        _EmailSMSAlertscs.OTPFlag = "0";
                        _EmailSMSAlertscs.var1 = "SBM";
                        _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                        ErrorLog.AgentManagementTrace("Page : AgPreVerification.cs \nFunction : SingleSave() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                        ErrorLog.SMSTrace("Page : AgPreVerification.cs \nFunction : SingleSave() => Details forwarded for SMS Preparation. MobileNo : " + _AgentRegistrationDAL.PersonalContact);
                        _EmailSMSAlertscs.HttpGetRequest();
                        #endregion

                        #region EMAIL
                        _EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                        _EmailSMSAlertscs.to = _AgentRegistrationDAL.BusinessEmail;
                        _EmailSMSAlertscs.tempname = "SR24659_EBCP3";
                        _EmailSMSAlertscs.OTPFlag = "0";
                        _EmailSMSAlertscs.var1 = "SBM";
                        _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                        ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : SingleSave() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                        ErrorLog.SMSTrace("Page : AgentRegistration.cs \nFunction : SingleSave() => Reistration Details forwarded for Email Preparation. Email : " + _AgentRegistrationDAL.BusinessEmail);
                        _EmailSMSAlertscs.HttpGetRequestEmail();
                        #endregion
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Declined Successfully', 'Pre Verification -agent');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approved Successfully', 'Pre Verification -agent');", true);
                    }
                }
                else
                {
                    _unsuessful = _unsuessful + 1;
                    ErrorLog.AgentManagementTrace("Pre-Verification: SingleSave: Failed - Maker Verification.  StatusFlag: " + _StatusFlag + " Flag: " + _Flag + " AgentCode: " + AgentCode + " Fullname: " + _strAgentFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Request Process Unsuccessful', 'Pre Verification -agent');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: SingleSave: Failed - Maker Verification. Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Search And Clear
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                FillGridBulk(EnumCollection.EnumBindingType.BindGrid);
                //TabName.Value = "Bulk";
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false); return;
                // string hdval = TabName.Value;
                //var value = this.TabName.Value;

                //TabName.Value = "Manual";
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Script", "<script>SetTab();</script>", false);

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: btnSearch_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ddlBCCode.SelectedValue = "0";
                ddlAgentCode.SelectedValue = null;
                ddlActivityType.SelectedValue = "-1";
                ddlFileID.ClearSelection();
                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                FillGridBulk(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: btnClear_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Approve : Grid Selection
        protected void btnApprove_ServerClick(object sender, EventArgs e)
        {
            ViewState["ActionType"] = EnumCollection.EnumDBOperationType.Approve.ToString();
            try
            {
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
                    lblconfirm.Text = "Are you sure want to Approve Agent:(s)?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Approve Agent:(s)');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: btnApprove_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Decline : Grid Selection
        protected void btnDecline_ServerClick(object sender, EventArgs e)
        {
            ViewState["ActionType"] = EnumCollection.EnumDBOperationType.Decline.ToString();
            try
            {
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
                    lblconfirm.Text = "Are you sure want to Decline Agent:(s)?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Decline Agent:(s)');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: btnDecline_ServerClick: Exception: " + Ex.Message);
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
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter Remarks', 'Agent: Verification');", true);
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
                    FillGridBulk(EnumCollection.EnumBindingType.BindGrid);
                    BindDropdownsAgent();
                    TxtRemarks.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _strAlertMessage_Total + _reocrdsProcessed + " Successful : " + _successful + "  Unsuccessful : " + _unsuessful + " ');", true);
                    // ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>showSuccess('" + _strAlertMessage_Total + _reocrdsProcessed + "  </br>Successful : " + _successful + "  </br>Unsuccessful : " + _unsuessful + " ');", true);
                    ViewState["ActionType"] = null;
                    ViewState["SelectionType"] = null;
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: btnSaveAction_Click: Exception: " + Ex.Message);
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

                // ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>showSuccess('Operation has cancelled.','" + _alertMessage + "');", true);
                ModalPopupExtender_Declincard.Hide();
                TxtRemarks.Text = string.Empty;
                ViewState["ActionType"] = null;
                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                FillGridBulk(EnumCollection.EnumBindingType.BindGrid);
                return;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: btnCancelAction_Click: Exception: " + Ex.Message);
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
                        lblRecordsTotal.Text = "Total " + Convert.ToString(dsTemp.Rows.Count) + " Record(s) Found.";
                    }
                    else
                    {
                        gvAgent.Visible = false;
                        btnApprove.Visible = false;
                        btnDecline.Visible = false;
                        lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                    }
                }
                else
                    FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                CheckBoxAllOperationOnPageIndex();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: gvAgent_PageIndexChanging: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("Pre-Verification: chBoxSelectRow_CheckedChanged: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("Pre-Verification: CheckBoxAll_CheckedChanged: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("Pre-Verification: CheckBoxAllOperationOnPageIndex: Exception: " + Ex.Message);
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

                _AgentRegistrationDAL.Mode = "GetAgentDetails";
                //_AgentEntity.AgentAgentID = AgentReqId;
                //ViewState["AgentReqId"] = AgentReqId;
                ViewState["ReceiverEmailID"] = ReceiverEmailID;
                string contactNo = Convert.ToString(commandArgs[2]);
                ViewState["ReceiverContactNo"] = contactNo;

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
                    lblApplicationID.Text = ds.Tables[0].Rows[0]["Agent ID"].ToString();
                    txtAgentName.Text = ds.Tables[0].Rows[0]["AgentName"].ToString();
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
                ErrorLog.AgentManagementTrace("Pre-Verification: btnView_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }



        protected void EyeImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //EyeImage.Visible = true;
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: EyeImage_Click: Exception: " + Ex.Message);
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

                DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();

                    strURL = Ds.Tables[1].Rows[0]["AddressProofDocument"].ToString();
                    string FinalPath = filepath + strURL;
                    string pdfPath = strURL;
                    Session["pdfPath"] = pdfPath;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PdfExport.aspx');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: EyeImage1_Click: Exception: " + Ex.Message);
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
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    string fileName = Ds.Tables[2].Rows[0]["SignatureProofType"].ToString();

                    strURL = Ds.Tables[2].Rows[0]["SignatureProofDocument"].ToString();
                    string FinalPath = filepath + strURL;

                    string pdfPath = FinalPath;
                    Session["pdfPath"] = pdfPath;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PdfExport.aspx');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: EyeImage3_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

           /// txtResponse.InnerText = "{\"TotalRecords\": 3,\"ResultedRecords\": [{\"sourceInfo\": \"NAME1:Dawood Ibrahim\",\"uniqueNumber\": \"AAB1pKACMAAARMEAAiJYWWLRLFABPEENNIZYEF\",\"listName\": \"OFACSDNLIST\",\"listId\": \"9758\",\"rank\": \"100\",\"matchedInfo\": \"Dawood IBRAHIM\",\"matchDate\": \"23 JAN 2018 13:35\",\"status\": \"N\",\"comments\": \"N.A.\",\"serialNo1\": \"2\",\"customerName\": null,\"isDOBMatched\": \"Y\",\"countryOfOrigin\": \"UK\",\"sourceListMatch\": \"FCA (UK) Final Notices\"},{\"sourceInfo\": \"NAME1~Dawood Ibrahim\",\"uniqueNumber\": \"AAB1pKACMAAARMEAAiEEYNTGOOBXFPUIXTXABE\",\"listName\": \"OFACSDNLIST\",\"listId\": \"9759\",\"rank\": \"85\",\"matchedInfo\": \"DAWOOD IBRAHIM ORGANIZATION\",\"matchDate\": \"23 JAN 2018 13:35\",\"status\": \"N\",\"comments\": \"N.A.\",\"serialNo1\": \"1\",\"customerName\": null,\"isDOBMatched\": \"N\",\"countryOfOrigin\": \"UK\",\"sourceListMatch\": \"FCA (UK) Final Notices\"},{\"sourceInfo\": \"NAME2~osama bin laden\",\"uniqueNumber\": \"AAB1pKACMAAARMEAAiHGOVDVWTNYQHUOEXLXJQ\",\"listName\": \"OFACSDNLIST\",\"listId\": \"11378\",\"rank\": \"86\",\"matchedInfo\": \"Saad BIN LADEN\",\"matchDate\": \"23 JAN 2018 13:35\",\"status\": \"N\",\"comments\": \"N.A.\",\"serialNo1\": \"3\",\"customerName\": null,\"isDOBMatched\": \"N\",\"countryOfOrigin\": \"UK\",\"sourceListMatch\": \"FCA (UK) Final Notices\"}],\"FileName\": \"15166947219414059255\",\"FileImport\": \"N\"} ";
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
                ErrorLog.AgentManagementTrace("Pre-Verification: btnViewResp_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("Pre-Verification: btnViewDownload_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("Pre-Verification: ImageButton1_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("Pre-Verification: imgbtnform_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

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

                        CommonSave("RadionButtonClick");
                        _AgentRegistrationDAL.AgentReqId = string.Empty;
                        FillGrid(EnumCollection.EnumPermissionType.Export);
                        FillGridBulk(EnumCollection.EnumBindingType.BindGrid);
                        txtFinalRemarks.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ClearRemark();", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _strAlertMessage_Total + _reocrdsProcessed + " Successful : " + _successful + "  Unsuccessful : " + _unsuessful + " ');", true);
                        ViewState["ActionType"] = null;
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter Remarks', 'Warning');", true);
                    }
                }
                catch (Exception Ex)
                {
                    ErrorLog.AgentManagementTrace("Pre-Verification: btnSubmitDetails_Click: Exception: " + Ex.Message);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ClearRemark();", true);
            }
        }

        protected void gvAgPreBulk_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Contains("DownloadDoc"))
                {
                    string status = string.Empty;
                    ImageButton lb = (ImageButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                    int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    _ImportEntity.Fileid = Convert.ToInt32(gvAgPreBulk.DataKeys[gvr.RowIndex].Values["FileID"]);
                    Session["FILEID"] = _ImportEntity.Fileid;
                    _ImportEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                    _ImportEntity.Flag = 3;
                    DataSet Ds = _ImportEntity.Get_AgentManualKycUpload();
                    if (string.IsNullOrEmpty(status)) status = "Bulk Manual Kyc Upload";
                    if (Ds != null && Ds.Tables.Count > 0)
                    {
                        exportFormat.ExporttoExcel(Session["Username"].ToString(), Session["BankName"].ToString(), status, Ds);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Something went wrong. Try again', 'Warning');", true);
                        return;
                    }
                }
                if (e.CommandName.Contains("DownloadZip"))
                {
                    try
                    {
                        ImageButton lb = (ImageButton)e.CommandSource;
                        GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                        int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());
                        string Fileid = Convert.ToString(gvAgPreBulk.DataKeys[gvr.RowIndex].Values["FileID"]);

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
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.AgentManagementTrace("Pre-Verification: DownloadZip_Click: Exception: " + Ex.Message);
                        return ;
                    }
                }
                if (e.CommandName.Contains("FileEdit"))
                {
                    string status = string.Empty;
                    ImageButton lb = (ImageButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                    int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    _ImportEntity.Fileid = Convert.ToInt32(gvAgPreBulk.DataKeys[gvr.RowIndex].Values["FileID"]);
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

        #endregion

        #region Dropdown Events

        protected void ddlBCCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDropdownsAgent();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: ddlBCCode_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        protected void ddlClientCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDropdownsBC();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: ddlClientCode_SelectedIndexChanged: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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
                //_AgentRegistrationDAL.Flag = ((int)EnumCollection.EnumPermissionType.EnableMakerChecker);
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.EnableMakerChecker);//_AgentRegistrationDAL.GetAgentDetailsToProcessOnboaring();

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "Pre-Ver Approval Agent Details", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }

            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: BtnCsv_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                //_AgentRegistrationDAL.Flag = ((int)EnumCollection.EnumPermissionType.EnableMakerChecker);
                //DataSet dt = _AgentRegistrationDAL.GetAgentDetailsToProcessOnboaring();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.EnableMakerChecker);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "Pre-Ver Approval Agent Details", dt);
                }
                {
                    //lblRecordCount.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);

                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: BtnXls_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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
                ErrorLog.AgentManagementTrace("Pre-Verification: GenerateToExcel: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("Pre-Verification: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }

        #endregion
        protected void gvAgPreBulk_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAgPreBulk.PageIndex = e.NewPageIndex;
                FillGridBulk(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.CommissionError(Ex);
            }
        }

        public DataSet FillGridBulk(EnumCollection.EnumBindingType _EnumBindingType, string sortExpression = null)
        {
            DataSet ds = null;
            try
            {

                gvAgPreBulk.DataSource = null;
                gvAgPreBulk.DataBind();
                // SetPropertise();

                _ImportEntity.Flag = 1;
                _ImportEntity.FileID = ddlFileID.SelectedValue != "0" ? (ddlFileID.SelectedValue) : null;
                // _ImportEntity.FromDate = !string.IsNullOrEmpty(_FromDate) ? Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd") : null;
                // _ImportEntity.ToDate = !string.IsNullOrEmpty(_ToDate) ? Convert.ToDateTime(_ToDate).ToString("yyyy-MM-dd") : null;
                _ImportEntity.BCstatus = ((int)EnumCollection.Onboarding.Pending).ToString();
                gvAgPreBulk.Visible = true;
                ds = _ImportEntity.Get_AgentBulkUpload();
                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            DataView dv = ds.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";
                            dv.Sort = sortExpression + " " + this.SortDirection;
                            gvAgPreBulk.DataSource = dv;
                            gvAgPreBulk.DataBind();
                            gvAgPreBulk.Visible = true;

                        }
                        else
                        {
                            gvAgPreBulk.DataSource = ds.Tables[0];
                            gvAgPreBulk.DataBind();
                            gvAgPreBulk.Visible = true;

                        }
                    }
                    else
                    {
                        gvAgPreBulk.Visible = false;

                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.ZOMError(Ex);
            }
            return ds;
        }
        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        //public void SetPropertise()
        //{
        //    try
        //    {
        //        _FromDate = !string.IsNullOrEmpty(txt.Value) ? txtFromDate.Value.Trim() : null;
        //        _ToDate = !string.IsNullOrEmpty(txtToDate.Value) ? txtToDate.Value.Trim() : null;
        //        _Username = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;

        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.CommissionError(Ex);
        //    }
        //}
        private void BindFileId()
        {
            try
            {
                ddlFileID.Items.Clear();
                ddlFileID.DataSource = null;
                ddlFileID.DataBind();
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.BCstatus = ((int)EnumCollection.Onboarding.Pending).ToString();
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
                ErrorLog.AgentManagementTrace("Pre-Verification: BindFileId: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        protected void btnViewBulk_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ModalPopupExtender_EditRole.Show();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: btnView_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BulkApprove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                _AgentRegistrationDAL.BCstatus = Convert.ToString((int)(EnumCollection.Onboarding.Approve));
                _AgentRegistrationDAL.Mstatus = Convert.ToString((int)(EnumCollection.Onboarding.MakerPending));
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.BcRemarks = txtResone.Text;
                _AgentRegistrationDAL.ActionType = "1";
                _AgentRegistrationDAL.FileID = Session["FILEID"].ToString();
                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatusBulk();
                if (Convert.ToInt32(Convert.ToString(_dsVerification.Tables[0].Rows[0][0])) > 0)
                {
                    ErrorLog.AgentManagementTrace("Pre-Verification: BulkApprove_ServerClick: Successful - Pre Verification.  FileID: " + _AgentRegistrationDAL.FileID + " Flag: " + _Flag + " User: " + User +" Remarks: " + _AgentRegistrationDAL.BcRemarks);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('File Approved Successfully', 'Pre Verification -agent');", true);

                }
                else
                {
                    ErrorLog.AgentManagementTrace("Pre-Verification: BulkApprove_ServerClick: Failed - Pre Verification.   FileID: " + _AgentRegistrationDAL.FileID + " Flag: " + _Flag + " User: " + User +" Remarks: " + _AgentRegistrationDAL.BcRemarks);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('File Approve Process Unsuccessful', 'Pre Verification -agent');", true);
                }
                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                FillGridBulk(EnumCollection.EnumBindingType.BindGrid);

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: BulkApprove_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        protected void BulkDecline_ServerClick(object sender, EventArgs e)
        {
            try
            {
                _AgentRegistrationDAL.BCstatus = Convert.ToString((int)(EnumCollection.Onboarding.Decline));
                _AgentRegistrationDAL.Mstatus = Convert.ToString((int)(EnumCollection.Onboarding.MakerPending));
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.MakerRemark = txtResone.Text;
                _AgentRegistrationDAL.ActionType = "2";
                _AgentRegistrationDAL.FileID = Session["FILEID"].ToString();
                _dsVerification = _AgentRegistrationDAL.ChangeAgentStatusBulk();
                if (Convert.ToInt32(Convert.ToString(_dsVerification.Tables[0].Rows[0][0])) > 0)
                {
                    ErrorLog.AgentManagementTrace("Pre-Verification: BulkDecline_ServerClick: Successful - Pre Verification.  FileID: " + _AgentRegistrationDAL.FileID + " Flag: " + _Flag + " User: " + User + " Remarks: " + _AgentRegistrationDAL.BcRemarks);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('File Declined Successfully', 'Pre Verification -agent');", true);

                    ErrorLog.AgentManagementTrace("Page : Pre-Verification.cs \nFunction : BulkDecline_ServerClick() => SMS/Email sending process started... Count :" + _dsVerification.Tables[1].Rows.Count);

                    for (int i = 0; i < _dsVerification.Tables[1].Rows.Count; i++)
                    {
                        ErrorLog.AgentManagementTrace("Page : Pre-Verification.cs \nFunction : BulkDecline_ServerClick() => SMS/Email sending process started for SMS...");
                        #region SMS
                        _EmailSMSAlertscs.FROM = Session["Username"].ToString();
                        _EmailSMSAlertscs.to = Convert.ToString(_dsVerification.Tables[1].Rows[i]["ContactNo"]);
                        _EmailSMSAlertscs.tempname = "SR24659_BCP2";
                        _EmailSMSAlertscs.OTPFlag = "0";
                        _EmailSMSAlertscs.var1 = "SBM";
                        _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                        ErrorLog.AgentManagementTrace("Page : Pre-Verification.cs \nFunction : BulkDecline_ServerClick() => Details forwarded for SMS Preparation. => HttpGetRequest()");
                        ErrorLog.SMSTrace("Page : Pre-Verification.cs \nFunction : BulkDecline_ServerClick() => Details forwarded for SMS Preparation. MobileNo : " + Convert.ToString(_dsVerification.Tables[1].Rows[i]["ContactNo"]));
                        _EmailSMSAlertscs.HttpGetRequest();
                        #endregion

                        ErrorLog.AgentManagementTrace("Page : Pre-Verification.cs \nFunction : BulkDecline_ServerClick() => Email sending process started for Email...");
                        #region EMAIL
                        _EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                        _EmailSMSAlertscs.to = Convert.ToString(_dsVerification.Tables[1].Rows[i]["PersonalEmailID"]);
                        _EmailSMSAlertscs.tempname = "SR24659_EBCP3";
                        _EmailSMSAlertscs.OTPFlag = "0";
                        _EmailSMSAlertscs.var1 = "SBM";
                        _EmailSMSAlertscs.var2 = DateTime.Now.ToString();
                        ErrorLog.AgentManagementTrace("Page : Pre-Verification.cs \nFunction : BulkDecline_ServerClick() => Reistration Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                        ErrorLog.SMSTrace("Page : Pre-Verification.cs \nFunction : BulkDecline_ServerClick() => Reistration Details forwarded for Email Preparation. Email : " + Convert.ToString(_dsVerification.Tables[1].Rows[i]["PersonalEmailID"]));
                        _EmailSMSAlertscs.HttpGetRequestEmail();
                        #endregion
                    }
                }
                else
                {
                    ErrorLog.AgentManagementTrace("Pre-Verification: BulkApprove_ServerClick: Failed - Pre Verification.   FileID: " + _AgentRegistrationDAL.FileID + " Flag: " + _Flag + " User: " + User + " Remarks: " + _AgentRegistrationDAL.BcRemarks);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('File Decline Process Unsuccessful', 'Pre Verification -agent');", true);
                }
                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                FillGridBulk(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Pre-Verification: BulkDecline_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void bulkClose_ServerClick(object sender, EventArgs e)
        {
            ModalPopupExtender_EditRole.Hide();
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
    }
}