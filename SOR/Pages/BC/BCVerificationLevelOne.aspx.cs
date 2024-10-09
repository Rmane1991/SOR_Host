using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessAccessLayer;
//using DataAccessLayer;
using AppLogger;
using System.Data;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Net;

namespace SOR.Pages.BC
{
    public partial class BCVerificationLevelOne : System.Web.UI.Page
    {
        #region Property Declaration
        BCEntity _BCEntity = new BCEntity();
        #endregion

        #region Objects Declaration

        public string UserName { get; set; }
        ClientRegistrationEntity clientMngnt = new ClientRegistrationEntity();
        //public static DbAccess _dbAccess = new DbAccess();
        AppSecurity _AppSecurity = new AppSecurity();
        public string pathId, PathAdd, PathSig;
        #endregion;

        #region Variable and Objects

        static bool _IsRefresh = true;
        DataSet _dsVerification = null;

        // User Credentials
        string[] _auditParams = new string[4];

        string[]
            _BCActiveParams = new string[8];

        int _successful = 0,
            _unsuessful = 0,
            _reocrdsProcessed = 0,
            _DeclineCardCount = 0,
            _Flag = 0,
            _StatusFlag = 0;

        string

            _strBCFullName = null;
        string
            _strAlertMessage_Header = null,
            _strAlertMessage_Success = null,
            _strAlertMessage_UnSuccess = null,
            _strAlertMessage_Total = null,
            _fileLineNo = null;

        public enum ActionType
        {
            Pending = 0,
            Approve = 1,
            Decline = 2,
            Terminate = 3,
            Activate = 4,
            Deactivate = 5,
            ReProcess = 6
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
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)

        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {

                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "BCVerificationLevelOne.aspx", "10");
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
                        UserPermissions.RegisterStartupScriptForNavigationListActive("4", "10");
                        if (!IsPostBack && HasPagePermission)
                        {
                            Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                            BindDropdownClients();
                            BindDropdownsBC();
                            FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                            ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                        }
                        else
                        {
                            if (hid1.Value == "aaa")
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#ModalBC').modal('show')", true);
                                //ModalBC.Visible = true;
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
                    catch (ThreadAbortException)
                    {
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BCVerificationLevelOne.cs \nFunction : Page_Load() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Verification Level One-BC');", true);
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
                            ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
                        }
                        else
                        {
                            ddlClientCode.Items.Clear();
                            ddlClientCode.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientCode.DataTextField = "ClientName";
                            ddlClientCode.DataValueField = "ClientID";
                            ddlClientCode.DataBind();
                            ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
                        }
                    }
                    else
                    {
                        ddlClientCode.Items.Clear();
                        ddlClientCode.DataSource = null;
                        ddlClientCode.DataBind();
                        ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
                    }
                }
                else
                {
                    ddlClientCode.Items.Clear();
                    ddlClientCode.DataSource = null;
                    ddlClientCode.DataBind();
                    ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCc.cs \nFunction : BindDropdownClients() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Verification Level One-BC');", true);
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
                _BCEntity.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerApprove);
                _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
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
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : BindDropdownsBC() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Verification Level One-BC');", true);
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
                //ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableMakerChecker;

                _BCEntity.Clientcode = ddlClientCode.SelectedValue != "0" ? ddlClientCode.SelectedValue : null;
                _BCEntity.BCID = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;
                _BCEntity.Activity = ddlActivityType.SelectedValue != "-1" ? ddlActivityType.SelectedValue : null;
                _BCEntity.UserName = Session["Username"].ToString();
                _BCEntity.IsRemoved = "0";
                _BCEntity.IsActive = "0";
                _BCEntity.IsdocUploaded = "1";
                _BCEntity.VerificationStatus = "0";
                _BCEntity.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerApprove);
                _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
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
                    panelGrid.Visible = false;
                    panelGrid.Visible = false;
                    lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : FillGrid() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Verification Level One-BC');", true);
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

                    if (ViewState["ActionType"].ToString() == EnumCollection.Onboarding.Decline.ToString())
                    {
                        _strAlertMessage_Header = "Decline BC ";
                        _strAlertMessage_Success = "Decline Process ";
                        _strAlertMessage_UnSuccess = "Decline Process ";
                        _strAlertMessage_Total = "Total Record Processed for Decline BC(s) :  ";
                        _BCEntity.CHstatus = Convert.ToString((int)(EnumCollection.Onboarding.CheckerDecline));
                        _Flag = (int)EnumCollection.EnumDBOperationType.CheckerDecline;
                    }
                    else if (ViewState["ActionType"].ToString() == EnumCollection.Onboarding.Approve.ToString())
                    {
                        _strAlertMessage_Header = "Approve BC ";
                        _strAlertMessage_Success = "Approve Process ";
                        _strAlertMessage_UnSuccess = "Approve Process ";
                        _strAlertMessage_Total = "Total Record Processed for Approve BC(s) :  ";
                        _BCEntity.CHstatus = Convert.ToString((int)(EnumCollection.Onboarding.CheckerApprove));
                        _Flag = (int)EnumCollection.EnumDBOperationType.CheckerApprove;
                        _BCEntity.ATStatus = Convert.ToString((int)(EnumCollection.Onboarding.AuthorizerPending));
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
                                    _reocrdsProcessed = _reocrdsProcessed + 1;
                                    //_BCEntity.BCID = Convert.ToString(BCDataSet.Rows[i]["BCReqID"]);
                                    _BCEntity.BCReqId = Convert.ToString(BCDataSet.Rows[i]["BC ID"]);
                                    _BCEntity.FirstName = Convert.ToString(BCDataSet.Rows[i]["BC Name"]);
                                    _BCEntity.BusinessEmail = Convert.ToString(BCDataSet.Rows[i]["Email"]);
                                    _BCEntity.PersonalContact = Convert.ToString(BCDataSet.Rows[i]["Contact No."]);
                                    _BCEntity.Clientcode = Convert.ToString(BCDataSet.Rows[i]["Client ID"]);
                                    _BCEntity._fileLineNo = (Convert.ToInt32(_fileLineNo) + 1).ToString();
                                    _BCEntity.MakerRemark = TxtRemarks.Text.Trim();
                                    _BCEntity.UserName = Session["Username"].ToString();
                                    _BCEntity.ActionType = Convert.ToInt32(_Flag);

                                    _BCEntity.Flag = 2;
                                    _dsVerification = _BCEntity.ChangeBCStatus();


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
                                            _reocrdsProcessed = _reocrdsProcessed + 1;
                                            SingleSave(_reocrdsProcessed, _Flag, _Flag, row.Cells[2].Text, row.Cells[3].Text, Convert.ToString(Session["Username"]), TxtRemarks.Text.Trim(), row.Cells[7].Text, row.Cells[6].Text, row.Cells[4].Text);
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
                            _reocrdsProcessed = 1;
                            SingleSave(_reocrdsProcessed, _Flag, _Flag, Convert.ToString(ViewState["BCReqId"]), txtBCName.Text, Convert.ToString(Session["Username"]), txtFinalRemarks.Text.Trim(), ViewState["ReceiverEmailID"].ToString(), ViewState["ReceiverContactNo"].ToString(), Convert.ToString(ViewState["ClientID"]));

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
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : CommonSave() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Verification Level One-BC');", true);
            }
        }

        public void SingleSave(int _reocrdsProcessed, int _StatusFlag, int _Flag, string _BCReqId, string _strBCFullName, string User, string Remarks, string ReceiverEmailID, string ContactNo, string _ClientCode)
        {
            try
            {
                ErrorLog.BCManagementTrace("Verification Level 1-BC: SingleSave: Received - Checker Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                _reocrdsProcessed = _reocrdsProcessed + 1;
                _BCEntity.ActionType = Convert.ToInt16(_Flag);
                _BCEntity.Flag = _Flag;
                _BCEntity.FirstName = _strBCFullName;
                _BCEntity.BCReqId = _BCReqId;
                _BCEntity.BusinessEmail = ReceiverEmailID;
                _BCEntity.PersonalContact = ContactNo;
                _BCEntity.Clientcode = _ClientCode;
                _BCEntity.UserName = User;
                _BCEntity.CheckerRemark = Remarks;
                _BCEntity.UserName = Session["Username"].ToString();

                _dsVerification = _BCEntity.ChangeBCStatus();
                if (Convert.ToInt32(Convert.ToString(_dsVerification.Tables[0].Rows[0][0])) > 0)
                {
                    _successful = _successful + 1;

                    if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Decline.ToString())
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Declined Successfully', 'Verification Level 1-BC');", true);
                        ErrorLog.BCManagementTrace("Verification Level 1-BC: SingleSave: Successful - Checker Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks); ErrorLog.BCManagementTrace("Verification Level 2-BC: SingleSave: Successful - Authorizer Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Approved Successfully', 'Verification Level 1-BC');", true);
                        ErrorLog.BCManagementTrace("Verification Level 1-BC: SingleSave: Successful - Checker Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks); ErrorLog.BCManagementTrace("Verification Level 2-BC: SingleSave: Successful - Authorizer Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                    }
                }
                else
                {
                    _unsuessful = _unsuessful + 1;
                    ErrorLog.BCManagementTrace("Verification Level 1-BC: SingleSave: Failed - Checker Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks); ErrorLog.BCManagementTrace("Verification Level 2-BC: SingleSave: Successful - Authorizer Verification.  StatusFlag: " + ViewState["ActionType"].ToString() + " Flag: " + _Flag + " AgentCode: " + _BCReqId + " Fullname: " + _strBCFullName + " User: " + User + " ReceiverEmailID: " + ReceiverEmailID + " ContactNo: " + ContactNo + " Remarks: " + Remarks);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : SingleSave() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Verification Level One-BC');", true);
                return;
            }
        }

        #endregion

        #region Search And Clear
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : btnSearch_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Verification Level One-BC');", true);
                return;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ddlClientCode.SelectedValue = "0";
                BindDropdownsBC();
                ddlBCCode.SelectedValue = "0";
                ddlActivityType.SelectedValue = "-1";
                FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : btnClear_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Verification Level One-BC');", true);
                return;
            }
        }
        #endregion

        #region Approve
        protected void btnApprove_ServerClick(object sender, EventArgs e)
        {
            if (HiddenConfirm.Value.ToString() == "Yes")
            {

                ViewState["ActionType"] = ActionType.Approve.ToString();
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
                        lblconfirm.Text = "Are you sure want to Approve Business Correspondents:(s)?";
                        ModalPopupExtender_Declincard.Show();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Approve Business Correspondents:(s)');", true);
                        return;
                    }
                }
                catch (Exception Ex)
                {
                    ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : btnApprove_ServerClick() \nException Occured\n" + Ex.Message);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Approve Business Correspondents:(s)');", true);
                    return;
                }
            }
        }
        #endregion

        #region Decline
        protected void btnDecline_ServerClick(object sender, EventArgs e)
        {
            if (HiddenConfirm.Value.ToString() == "Yes")
            {
                ViewState["ActionType"] = ActionType.Decline.ToString();
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
                        lblconfirm.Text = "Are you sure want to Decline Business Correspondents:(s)?";
                        ModalPopupExtender_Declincard.Show();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Decline Business Correspondents:(s)');", true);
                        return;
                    }
                }
                catch (Exception Ex)
                {
                    ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : btnDecline_ServerClick() \nException Occured\n" + Ex.Message);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Business Correspondents: Verification');", true);
                    return;
                }
            }
        }
        #endregion

        #region Terminate
        protected void btnTerminate_ServerClick(object sender, EventArgs e)
        {
            if (HiddenConfirm.Value.ToString() == "Yes")
            {
                ViewState["ActionType"] = ActionType.Decline.ToString();
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
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Terminate BC(s)');", true);
                        return;
                    }
                }
                catch (Exception Ex)
                {
                    ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : btnTerminate_ServerClick() \nException Occured\n" + Ex.Message);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Verification BC(s)');", true);
                    return;
                }
            }
        }
        #endregion

        #region Approve \ Decline \ Terminate Event Handler
        protected void btnSaveAction_Click(object sender, EventArgs e)
        {
            if (HiddenConfirm.Value.ToString() == "Yes")
            {
                try
                {
                    if (TxtRemarks.Text == null || TxtRemarks.Text == "")
                    {
                        ModalPopupExtender_Declincard.Show();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter Remarks', 'Verification Level One-BC');", true);
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
                        TxtRemarks.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + _strAlertMessage_Total + _reocrdsProcessed + " Successful : " + _successful + "  Unsuccessful : " + _unsuessful + " ');", true);
                        ViewState["ActionType"] = null;
                        ViewState["SelectionType"] = null;
                        return;
                    }
                }
                catch (Exception Ex)
                {
                    ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : btnSaveAction_Click() \nException Occured\n" + Ex.Message);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Verification Level One-BC');", true);
                    ViewState["ActionType"] = null;
                    ViewState["SelectionType"] = null;
                    return;
                }
            }
        }
        #endregion

        #region Cancel Action
        protected void btnCancelAction_Click(object sender, EventArgs e)
        {
            if (HiddenConfirm.Value.ToString() == "Yes")
            {
                try
                {
                    string _alertMessage = string.Empty;
                    if (ViewState["ActionType"].ToString() == ActionType.Approve.ToString())
                    {
                        _alertMessage = "Business Correspondent Approve";
                    }
                    else if (ViewState["ActionType"].ToString() == ActionType.Decline.ToString())
                    {
                        _alertMessage = "Business Correspondent";
                    }
                    else if (ViewState["ActionType"].ToString() == ActionType.Terminate.ToString())
                    {
                        _alertMessage = "Business Correspondent";
                    }
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Operation has cancelled.','" + _alertMessage + "');", true);
                    ModalPopupExtender_Declincard.Hide();
                    TxtRemarks.Text = string.Empty;
                    ViewState["ActionType"] = null;
                    FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                    return;
                }
                catch (Exception Ex)
                {
                    ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : btnCancelAction_Click() \nException Occured\n" + Ex.Message);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong..! Please try again','Verification Level One-BC');", true);
                    ViewState["ActionType"] = null;
                    return;
                }
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
                        btnTerminate.Visible = true;
                        lblRecordsTotal.Text = "Total " + Convert.ToString(dsTemp.Rows.Count) + " Record(s) Found.";
                    }
                    else
                    {
                        gvBusinessCorrespondents.Visible = false;
                        btnApprove.Visible = false;
                        btnDecline.Visible = false;
                        btnTerminate.Visible = false;
                        lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                    }
                }
                else
                    FillGrid(EnumCollection.EnumPermissionType.EnableRoles);
                CheckBoxAllOperationOnPageIndex();
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : gvBusinessCorrespondents_PageIndexChanging() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),"Warning", "showWarning('Something went wrong..! Please try again','Verification Level One-BC');", true);
                return;
            }
        }
        #endregion

        #region Check Box Events
        protected void chBoxSelectRow_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (HiddenConfirm.Value.ToString() == "Yes")
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
                        ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : chBoxSelectRow_CheckedChanged() \nException Occured\n" + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong..! Please try again','Verification Business Correspondent');", true);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : CheckBoxAllOperationOnPageIndex() \nException Occured\n" + ex.Message);

            }
        }
        protected void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (HiddenConfirm.Value.ToString() == "Yes")
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
                        ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : CheckBoxAll_CheckedChanged() \nException Occured\n" + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong..! Please try again','Verification Business Correspondent');", true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : CheckBoxAllOperationOnPageIndex() \nException Occured\n" + ex.Message);
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
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : CheckBoxAllOperationOnPageIndex() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),"Warning", "showWarning('Something went wrong..! Please try again','Verification Business Correspondent');", true);
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

                _BCEntity.Mode = "GetBCDetails";
                _BCEntity.BCID = BCReqId;
                ViewState["BCReqId"] = BCReqId;
                ViewState["ReceiverEmailID"] = ReceiverEmailID;
                string contactNo = Convert.ToString(commandArgs[2]);
                ViewState["ReceiverContactNo"] = contactNo;

                DataSet ds = _BCEntity.GetBCDocuments();
                _BCEntity.Mode = "GetBCDocumentById";
                DataSet get = _BCEntity.GetBCDocuments();

                string defaultimage = "images/Question.png";

                string idthumb = !string.IsNullOrEmpty(get.Tables[0].Rows[0]["IdentityProofDocument"].ToString())?get.Tables[0].Rows[0]["IdentityProofDocument"].ToString(): defaultimage.ToString();
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

                string Addthumb = !string.IsNullOrEmpty(get.Tables[1].Rows[0]["AddressProofDocument"].ToString()) ? get.Tables[1].Rows[0]["AddressProofDocument"].ToString() : defaultimage.ToString();
                if (!string.IsNullOrEmpty(get.Tables[1].Rows[0]["AddressProofDocument"].ToString()))
                {
                    string FileThumbnailAdd = Path.GetDirectoryName(Addthumb) + "\\" + Path.GetFileNameWithoutExtension(Addthumb) + "_Thumbnail.png";
                    PathAdd = "../../" + FileThumbnailAdd;
                }
                else
                {
                    PathAdd = "../../" + Addthumb;
                }
                Session["pdfPathAdd"] = Addthumb;

                string Sigthumb = !string.IsNullOrEmpty(get.Tables[2].Rows[0]["SignatureProofDocument"].ToString()) ? get.Tables[2].Rows[0]["SignatureProofDocument"].ToString() : defaultimage.ToString();
                if (!string.IsNullOrEmpty(get.Tables[2].Rows[0]["SignatureProofDocument"].ToString()))
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
                    //gvDownloadDocuments.DataSource = ds.Tables[0];
                    //gvDownloadDocuments.DataBind();
                    lblApplicationID.Text = ds.Tables[0].Rows[0]["BCID"].ToString();
                    //lblCompanyName.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                    txtBCName.Text = ds.Tables[0].Rows[0]["BCName"].ToString();
                    // txtRegistrationType.Text = ds.Tables[0].Rows[0]["BCBCType"].ToString();
                    txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    ViewState["BCType"] = ds.Tables[0].Rows[0]["RoleID"].ToString();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Verification');", true);
                return;
            }
        }

        protected void EyeImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //EyeImage.Visible = true;
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentById";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
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
                    hid1.Value = "aaa";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : EyeImage_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Verification');", true);
                throw;
            }
        }

        protected void EyeImage1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentById";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();

                DataSet Ds = _BCEntity.GetBCDocuments();
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
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : EyeImage1_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Verification');", true);
                throw;
            }
        }

        protected void EyeImage3_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentById";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
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
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : EyeImage3_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Verification');", true);
                throw;
            }
        }
        #endregion

        #region View Download
        protected void btnViewDownload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentById";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                   // string fileName = Ds.Tables[0].Rows[0]["IdentityProofType"].ToString();
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
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : btnViewDownload_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Verification');", true);
                return;
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentById";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    //string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();
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
                    byte[] data = req.DownloadData(strURL);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : ImageButton1_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Business Correspondents Verification');", true);
                return;
            }
        }

        protected void imgbtnform_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentById";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
                if (Ds.Tables[2].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    //string fileName = Ds.Tables[2].Rows[0]["SignatureProofType"].ToString();
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
                    byte[] data = req.DownloadData(strURL);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : ImageButton2_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Business Correspondents Verification');", true);
                return;
            }
        }
        #endregion

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
                            ViewState["ActionType"] = ActionType.Approve.ToString();
                        }
                        else
                        {
                            ViewState["ActionType"] = ActionType.Decline.ToString();
                        }
                        CommonSave("RadionButtonClick");
                        FillGrid(EnumCollection.EnumPermissionType.Export);
                        txtFinalRemarks.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + _strAlertMessage_Total + _reocrdsProcessed + " Successful : " + _successful + "  Unsuccessful : " + _unsuessful + " ');", true);
                        ViewState["ActionType"] = null;
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter Remarks', 'Verification Level One-BC');", true);
                    }
                }
                catch (Exception Ex)
                {
                    ErrorLog.BCManagementTrace("Class : BCVerificationLevelOne.cs \nFunction : btnSubmitDetails_Click() \nException Occured\n" + Ex.Message);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Verification Level One-BC');", true);
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

        protected void ddlClientCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDropdownsBC();
            }
            catch (Exception ex)
            {
                ErrorLog.BCManagementTrace("Page : BCVerificationLevelOne.cs \nFunction : ddlClientCode_SelectedIndexChanged\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Verification Level One-BC');", true);
                return;
            }
        }

        #endregion

        #region Export

        private string SetPageFiltersExport()
        {
            string pageFilters = string.Empty;
            try
            {
                pageFilters = "Generated By " + Convert.ToString(Session["Username"]);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BCVerificationLevelOne.cs \nFunction : SetPageFiltersExport\nException Occured\n" + Ex.Message);
            }
            return pageFilters;
        }

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.EnableRoles);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "L1 Approval Business Correspondents Details", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BCVerificationLevelOne.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);

            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.EnableRoles);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "L1 Approval Business Correspondents Details", dt);
                }
                {
                    lblRecordsTotal.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BCVerificationLevelOne.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);
            }
        }
        #endregion
    }
}