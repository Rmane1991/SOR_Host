﻿using AppLogger;
using BussinessAccessLayer;
//using DataAccessLayer;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOR.Pages.Aggregator
{
    public partial class OnBoardAggStatus : System.Web.UI.Page
    {
        #region Objects Declaration

        public string UserName { get; set; }
        BCEntity _BCEntity = new BCEntity();
        ClientRegistrationEntity clientMngnt = new ClientRegistrationEntity();
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
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
                ErrorLog.AggregatorTrace("OnBoardAggStatus | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission =  HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "OnBoardAggStatus.aspx", "16");
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
                            BindDropdownClients();
                            BindDropdownsBc();
                            //BindDropdownState();
                            ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();
                            //BindExport();
                        }
                        UserPermissions.RegisterStartupScriptForNavigationListActive("5", "16");
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
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : Page_Load() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Deactive Aggregator');", true);
                return;
            }
        }
        #endregion

        #region Methods

        public void BindDropdownState()
        {
            try
            {
                _BCEntity.Mode = "getStateAndCity";
                _BCEntity.Country = "101";
                DataSet ds_Country = _BCEntity.getStateAndCity();
                if (ds_Country != null && ds_Country.Tables.Count > 0 && ds_Country.Tables[0].Rows.Count > 0)
                {
                    ddlState.Items.Clear();
                    ddlState.DataSource = ds_Country.Tables[0];
                    ddlState.DataValueField = "ID";
                    ddlState.DataTextField = "Name";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("-- State --", "0"));
                }
                else
                {
                    ddlState.Items.Clear();
                    ddlState.DataSource = null;
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("-- State --", "0"));
                }

                ddlCity.DataSource = null;
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, new ListItem("-- City --", "0"));
            }
            catch (Exception ex)
            {
                ErrorLog.AggregatorTrace("Page : OnBoardAggStatus.cs \nFunction : BindDropdownState()\nException Occured\n" + ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "OnBoardBcStatus.aspx.cs", "BindDropdownState()", ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Deactive Aggregator');", true);
                return;
            }
        }

        private void BindDropdownClients()
        {
            DataSet _dsClient = new DataSet();
            try
            {
                ddlClientCode.Items.Clear();
                ddlClientCode.DataSource = null;
                ddlClientCode.DataBind();

                ///////  clientMngnt.UserName = Session["Username"].ToString();
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
                            ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- -- Select ----", "0"));
                        }
                        else
                        {
                            ddlClientCode.Items.Clear();
                            ddlClientCode.DataSource = _dsClient.Tables[0].Copy();
                            ddlClientCode.DataTextField = "ClientName";
                            ddlClientCode.DataValueField = "ClientID";
                            ddlClientCode.DataBind();
                            ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- -- Select ----", "0"));
                        }
                    }
                    else
                    {
                        ddlClientCode.Items.Clear();
                        ddlClientCode.DataSource = null;
                        ddlClientCode.DataBind();
                        ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- -- Select ----", "0"));
                    }
                }
                else
                {
                    ddlClientCode.Items.Clear();
                    ddlClientCode.DataSource = null;
                    ddlClientCode.DataBind();
                    ddlClientCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- -- Select ----", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : BindDropdownClients() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "OnBoardAggStatus.cs", "BindDropdownClients()", Ex);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Deactive Aggregator');", true);
                return;
            }
        }

        private void BindDropdownsBc()
        {
            try
            {
                ddlBCCode.Items.Clear();
                ddlBCCode.DataSource = null;
                ddlBCCode.DataBind();

                _BCEntity.UserName = Session["Username"].ToString();
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                _BCEntity.CreatedBy = Session["Username"].ToString();
                //_BCEntity.IsRemoved = "0";
                //_BCEntity.IsActive = "2";
                _BCEntity.IsdocUploaded = "1";
                _BCEntity.VerificationStatus = "1";
                _BCEntity.Clientcode = ddlClientCode.SelectedValue != "0" ? ddlClientCode.SelectedValue : null;
                _BCEntity.BCID = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;

                DataSet ds = _BCEntity.BindBCddl();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        ddlBCCode.Items.Clear();
                        ddlBCCode.DataSource = ds.Tables[0].Copy();
                        ddlBCCode.DataTextField = "BCName";
                        ddlBCCode.DataValueField = "BCCode";
                        ddlBCCode.DataBind();
                        ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlBCCode.Items.Clear();
                        ddlBCCode.DataSource = ds.Tables[0].Copy();
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
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus \nFunction : BindDropdownsFranchise() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "OnBoardBcStatus", "BindDropdownsFranchise()", Ex);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Aggregator');", true);
                return;
            }
        }

        public DataSet FillGrid(EnumCollection.EnumPermissionType _EnumPermissionType)
        {
            DataSet _dsDeActivate = null;
            try
            {
                ErrorLog.AggregatorTrace("OnBoardAggStatus | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                gvOnBoardAgg.DataSource = null;
                gvOnBoardAgg.DataBind();
                lblRecordsTotal.Text = "";
                ViewState["SelectionType"] = SelectionType.UnCheckAll.ToString();

                _BCEntity.Clientcode = ddlClientCode.SelectedValue != "0" ? ddlClientCode.SelectedValue : null;
                _BCEntity.BCID = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;
                _BCEntity.Flag = (int)_EnumPermissionType;
                _BCEntity.UserName = Session["Username"].ToString();
                //_BCEntity.IsRemoved = "0";
                //_BCEntity.IsActive = "2";
                _BCEntity.IsdocUploaded = "1";
                _BCEntity.VerificationStatus = "1";
                
                _dsDeActivate = _BCEntity.GetAggDetailsToProcessOnboaring();
                
                if (_dsDeActivate != null && _dsDeActivate.Tables.Count > 0 && _dsDeActivate.Tables[0].Rows.Count > 0)
                {
                    gvOnBoardAgg.DataSource = null;
                    gvOnBoardAgg.DataBind();
                    panelGrid.Visible = true;
                    gvOnBoardAgg.DataSource = _dsDeActivate.Tables[0];
                    gvOnBoardAgg.DataBind();
                    gvOnBoardAgg.Visible = true;

                    ViewState["BCDataSet"] = _dsDeActivate.Tables[0];
                    lblRecordsTotal.Text = "Total " + Convert.ToString(_dsDeActivate.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    gvOnBoardAgg.Visible = false;
                    panelGrid.Visible = false;
                    lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                }
                ErrorLog.AggregatorTrace("OnBoardAggStatus | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : FillGrid() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);
            }
            return _dsDeActivate;
        }

        #endregion

        #region Search And Clear
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("OnBoardAggStatus | btnSearch_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());

                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Aggregator-OnBoard-Status";
                _auditParams[2] = "btnSubmitDetails";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion

                if (ddlActiontype.SelectedValue == "0")
                {
                    _BCEntity.IsActive = "";

                    FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                }

                else if (ddlActiontype.SelectedValue == "1")
                {
                    btnActivate.Visible = false;
                    btnDeactivate.Visible = true;
                    btnTerminate.Visible = true;
                    _BCEntity.IsActive = Convert.ToString((int)EnumCollection.AgentStatus.Active);
                    FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                }
                else if (ddlActiontype.SelectedValue == "2")
                {
                    btnDeactivate.Visible = false;
                    btnActivate.Visible = true;
                    btnTerminate.Visible = true;

                    _BCEntity.IsActive = Convert.ToString((int)EnumCollection.AgentStatus.InActive);
                    FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                }
                else if (ddlActiontype.SelectedValue == "3")
                {
                    btnDeactivate.Visible = false;
                    btnActivate.Visible = false;
                    btnTerminate.Visible = false;
                    _BCEntity.IsActive = Convert.ToString((int)EnumCollection.AgentStatus.Terminated);
                    FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                }
                ErrorLog.AggregatorTrace("OnBoardAggStatus | btnSearch_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : btnSearch_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);
                return;
            }
        }


        #endregion

        #region Terminate
        protected void btnTerminate_ServerClick(object sender, EventArgs e)
        {
            ViewState["ActionType"] = Convert.ToString((int)EnumCollection.EnumDBOperationType.OnboardTerminate);
            try
            {
                ErrorLog.AggregatorTrace("OnBoardAggStatus | btnTerminate_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Aggregator-OnBoard-Status";
                _auditParams[2] = "btnTerminate";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                foreach (GridViewRow row in gvOnBoardAgg.Rows)
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
                    lblModalHeaderName.Text = "Aggregator(s) Terminate Reason";
                    TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                    lblconfirm.Text = "Are you sure want to Terminate Aggregator(s)?";
                    ModalPopupExtender_Declincard.Show();
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Terminate Aggregator(s)');", true);
                    return;
                }
                ErrorLog.AggregatorTrace("OnBoardAggStatus | btnTerminate_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : btnTerminate_ServerClick() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);
                return;
            }
        }


        protected void btnClear_Click_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("OnBoardAggStatus | btnClear_Click_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Aggregator-OnBoard-Status";
                _auditParams[2] = "btnClear_Click_Click";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                ddlClientCode.SelectedValue = "0";

                ddlState.SelectedValue = "0";
                ddlActiontype.SelectedValue = "0";
                ddlCity.DataSource = null;
                ddlCity.DataBind();
                ddlCity.Items.Clear();
                btnActivate.Visible = false;
                btnDeactivate.Visible = false;
                btnTerminate.Visible = false;
                ddlCity.Items.Insert(0, new ListItem("-- City --", "0"));
                FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                BindDropdownsBc();
                ErrorLog.AggregatorTrace("OnBoardAggStatus | btnClear_Click_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : btnClear_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);
                return;
            }
        }
        #endregion

        #region Grid Events
        protected void gvOnBoardAgg_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOnBoardAgg.PageIndex = e.NewPageIndex;

                if (ViewState["BCDataSet"] != null)
                {
                    DataTable dsTemp = (DataTable)ViewState["BCDataSet"];
                    if (dsTemp != null && dsTemp.Rows.Count > 0)
                    {
                        gvOnBoardAgg.DataSource = dsTemp;
                        gvOnBoardAgg.DataBind();
                        gvOnBoardAgg.Visible = true;

                    }
                    else
                    {
                        gvOnBoardAgg.Visible = false;
                        btnActivate.Visible = false;
                        btnTerminate.Visible = false;
                        btnDeactivate.Visible = false;
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
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : gvBlockFranchise_PageIndexChanging() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "OnBoardAggStatus.cs", "gvBlockFranchise_PageIndexChanging()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page),"Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);

                return;
            }
        }
        #endregion

        #region Activate
        protected void btnActivate_ServerClick(object sender, EventArgs e)
        {
            if (ddlActiontype.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Action Type.','Activate Aggregator(s)');", true);
            }

            else
            {
                ViewState["ActionType"] = Convert.ToString((int)EnumCollection.EnumDBOperationType.Activate);
                try
                {
                    ErrorLog.AggregatorTrace("OnBoardAggStatus | btnActivate_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Aggregator-OnBoard-Status";
                    _auditParams[2] = "btnActivate";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    foreach (GridViewRow row in gvOnBoardAgg.Rows)
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
                        lblModalHeaderName.Text = "Aggregator(s) Activate Reason";
                        TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                        lblconfirm.Text = "Are you sure want to Activate Aggregator(s)?";
                        ModalPopupExtender_Declincard.Show();
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Activate Aggregator(s)');", true);
                        return;
                    }
                    ErrorLog.AggregatorTrace("OnBoardAggStatus | btnActivate_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }
                catch (Exception Ex)
                {
                    ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : btnActivate_ServerClick() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);
                    return;
                }
            }
        }
        #endregion

        #region Deactivate 
        protected void btnDeactivate_ServerClick(object sender, EventArgs e)
        {
            if (ddlActiontype.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Action Type.','Activate Aggregator(s)');", true);
            }
            else
            {
                ViewState["ActionType"] = Convert.ToString((int)EnumCollection.EnumDBOperationType.Deactivate);
                try
                {
                    ErrorLog.AggregatorTrace("OnBoardAggStatus | btnActivate_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Aggregator-OnBoard-Status";
                    _auditParams[2] = "btnDeactivate";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    foreach (GridViewRow row in gvOnBoardAgg.Rows)
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
                        lblModalHeaderName.Text = "Aggregator(s) Deactivate Reason";
                        TxtRemarks.Style.Add("Placeholder", "Please enter the reason.");
                        lblconfirm.Text = "Are you sure want to Deactivate Aggregator(s)?";
                        ModalPopupExtender_Declincard.Show();
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select at least one record.','Deactivate Aggregator(s)');", true);
                        return;
                    }
                    ErrorLog.AggregatorTrace("OnBoardAggStatus | btnActivate_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }
                catch (Exception Ex)
                {
                    ErrorLog.AggregatorTrace("Class : ActiveBcReport.cs \nFunction : btnDeactivate_ServerClick() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Active BC');", true);
                    return;
                }
            }
        }


        #endregion


        public bool CheckSelfRequest(out string requestid)
        {
            bool IsvalidRecord = true;
            string _requestid = string.Empty;
            _BCEntity.Flag = 3;
            string status = _BCEntity.ValidateEditBcDetails(out _requestid);
            if (status == "00")
            {
                IsvalidRecord = true;
                requestid = _requestid;
            }
            else
            {
                IsvalidRecord = false;
                requestid = _requestid;
            }
            return IsvalidRecord;
        }

        protected void gvOnBoardAgg_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("OnBoardAggStatus | gvOnBoardAgg_RowCommand() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (e.CommandName.Contains("EditDetails"))
                {
                    ErrorLog.AggregatorTrace("OnBoardAggStatus | RowCommand-EditDetails | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Aggregator-RowCommand-EditDetails";
                    _auditParams[2] = "btnDeactivate";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    string requestid = string.Empty;
                    ImageButton lb = (ImageButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                    _BCEntity.BCReqId = (gvOnBoardAgg.DataKeys[gvr.RowIndex].Values["BCId"]).ToString();
                    if (ValidateReEditRequest())
                    {
                        if (CheckSelfRequest(out requestid))
                        {
                            //txtResone.Text = string.Empty;
                            string status = string.Empty;
                            int IsSelfData = 0;
                            string reqtype = "4";
                            Response.Redirect("../../Pages/Aggregator/EditAggRegistrationDetails.aspx?BCReqId=" + _BCEntity.BCReqId + "&" + "RequestType=" + reqtype + "&" + "IsSelfData=" + IsSelfData + " ", false);
                        }
                        else
                        {
                            _BCEntity.BCReqId = requestid;
                            string status = string.Empty;
                            int IsSelfData = 1;
                            string reqtype = "4";
                            Response.Redirect("../../Pages/Aggregator/EditAggRegistrationDetails.aspx?BCReqId=" + _BCEntity.BCReqId + "&" + "RequestType=" + reqtype + "&" + "IsSelfData=" + IsSelfData + " ", false);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Aggregator request already pending for verification.', 'Warning');", true);
                        return;
                    }
                    ErrorLog.AggregatorTrace("OnBoardAggStatus | RowCommand-EditDetails | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                }
                ErrorLog.AggregatorTrace("OnBoardAggStatus | gvOnBoardAgg_RowCommand() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : gvLimitRequest_RowCommand() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Verified Aggregtor');", true);
                return;
            }
        }

        public bool ValidateReEditRequest()
        {
            bool IsvalidRecord = true;
            string _requestid = string.Empty;
            _BCEntity.Flag = 1;
            string status = _BCEntity.ValidateEditAggDetails(out _requestid);
            if (status == "00")
            {
                IsvalidRecord = true;
            }
            else
            {
                IsvalidRecord = false;
            }
            return IsvalidRecord;
        }

        #region Activate\Terminate Event Handler
        protected void btnSaveAction_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("OnBoardAggStatus | btnSaveAction_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (TxtRemarks.Text == null || TxtRemarks.Text == "")
                {
                    ModalPopupExtender_Declincard.Show();

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter Remarks', 'Aggregator');", true);
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
                            _strAlertMessage_Header = "Terminate BC ";
                            _strAlertMessage_Success = "Terminate Process ";
                            _strAlertMessage_UnSuccess = "Terminate Process ";
                            _strAlertMessage_Total = "Total Record Processed for Terminate Aggregator(s) :  ";
                            _StatusFlag = (int)EnumCollection.EnumDBOperationType.OnboardTerminate;
                            _BCEntity.ActivityType = (int)EnumCollection.ActivityType.Terminate;
                            _Flag = 1;
                        }
                        else if (ViewState["ActionType"].ToString() == Convert.ToString((int)EnumCollection.EnumDBOperationType.Activate))
                        {
                            _strAlertMessage_Header = "Activate BC ";
                            _strAlertMessage_Success = "Activate Process ";
                            _strAlertMessage_UnSuccess = "Activate Process ";
                            _strAlertMessage_Total = "Total Record Processed for Activate Aggregator(s) :  ";
                            _StatusFlag = (int)EnumCollection.EnumDBOperationType.Activate;
                            _BCEntity.ActivityType = (int)EnumCollection.ActivityType.Activate;
                            _Flag = 1;

                        }

                        else if (ViewState["ActionType"].ToString() == Convert.ToString((int)EnumCollection.EnumDBOperationType.Deactivate))
                        {
                            _strAlertMessage_Header = "De-Activate BC";
                            _strAlertMessage_Success = "De-Activate BC Process ";
                            _strAlertMessage_UnSuccess = "De-Activate BC Process ";
                            _strAlertMessage_Total = "Total Record Processed for De-activate Aggregator(s) :  ";
                            _StatusFlag = (int)EnumCollection.EnumDBOperationType.Deactivate;
                            _Flag = 1;
                            _BCEntity.ActivityType = (int)EnumCollection.ActivityType.Deactivate;
                        }

                    }
                    #endregion
                    _BCEntity.ActionType = _Flag;
                    _BCEntity.Flag = _Flag;
                    if (!string.IsNullOrEmpty(TxtRemarks.Text.Trim()))
                    {
                        #region Multiple Section Rows of GridView.
                        if ((BCDataSet != null) && Convert.ToString(ViewState["SelectionType"]) == SelectionType.CheckedAll.ToString())
                        {
                            _fileLineNo = "0";
                            for (int i = 0; i < BCDataSet.Rows.Count; i++)
                            {
                                try
                                {
                                    #region Audit
                                    _auditParams[0] = Session["Username"].ToString();
                                    _auditParams[1] = "Aggregator-btnSaveAction-Multiple_Section";
                                    _auditParams[2] = "btnDeactivate";
                                    _auditParams[3] = Session["LoginKey"].ToString();
                                    _LoginEntity.StoreLoginActivities(_auditParams);
                                    #endregion
                                    _reocrdsProcessed = _reocrdsProcessed + 1;
                                    _BCEntity.BC_Code = Convert.ToString(BCDataSet.Rows[i]["BC Code"]);
                                    _strBCFullName = Convert.ToString(BCDataSet.Rows[i]["BC Name"]);
                                    _BCEntity.BCReqId = Convert.ToString(BCDataSet.Rows[i]["BCId"]);
                                    _BCEntity.Clientcode = Convert.ToString(BCDataSet.Rows[i]["Client ID"]);
                                    _BCEntity.BCStatus = Convert.ToString((int)EnumCollection.Onboarding.Approve);
                                    _BCEntity.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerApprove);
                                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                                    _BCEntity.Flag = _Flag;
                                    _BCEntity.MakerRemark = TxtRemarks.Text.Trim();
                                    _fileLineNo = (Convert.ToInt32(_fileLineNo) + 1).ToString();
                                    _BCEntity.UserName = Session["Username"].ToString();
                                    if (ValidateReEditRequest())
                                    {
                                        _dsDeactivateBC = _BCEntity.ChangeBcOnBoardStatus();

                                        if (Convert.ToInt32(Convert.ToString(_dsDeactivateBC.Tables[0].Rows[0][0])) > 0)
                                        {
                                            _successful = _successful + 1;
                                            
                                        }
                                    }
                                    else
                                    {
                                        _unsuessful = _unsuessful + 1;
                                        reason = "Aggregator request already pending for verification.";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Aggregator request already pending for verification.', 'Warning');", true);
                                        return;
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

                        #region Single Section Row of GridView.
                        else
                        {
                            _fileLineNo = "0";
                            foreach (GridViewRow row in gvOnBoardAgg.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox chBoxRow = (row.Cells[0].FindControl("chBoxSelectRow") as CheckBox);
                                    if (chBoxRow.Checked)
                                    {
                                        try
                                        {
                                            #region Audit
                                            _auditParams[0] = Session["Username"].ToString();
                                            _auditParams[1] = "Aggregator-btnSaveAction-Single_Section";
                                            _auditParams[2] = "btnDeactivate";
                                            _auditParams[3] = Session["LoginKey"].ToString();
                                            _LoginEntity.StoreLoginActivities(_auditParams);
                                            #endregion
                                            _reocrdsProcessed = _reocrdsProcessed + 1;

                                            HiddenField hfBCID = (HiddenField)row.FindControl("hfBCID");
                                            string bcId = hfBCID.Value;

                                            _BCEntity.BC_Code = row.Cells[2].Text;
                                            _BCEntity.BCReqId = bcId;

                                            _strBCFullName = Convert.ToString(row.Cells[3].Text);
                                            _BCEntity.MakerRemark = TxtRemarks.Text.Trim();
                                            _BCEntity.Clientcode = Convert.ToString(row.Cells[1].Text);
                                            _fileLineNo = (Convert.ToInt32(_fileLineNo) + 1).ToString();
                                            _BCEntity.BCStatus = Convert.ToString((int)EnumCollection.Onboarding.Approve);
                                            _BCEntity.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerApprove);
                                            _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                                            _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                                            _BCEntity.UserName = Session["UserName"].ToString();
                                            if (ValidateReEditRequest())
                                            {
                                                _dsDeactivateBC = _BCEntity.ChangeAggregatorOnBoardStatus();

                                                if (Convert.ToInt32(Convert.ToString(_dsDeactivateBC.Tables[0].Rows[0][0])) > 0)
                                                {
                                                    _successful = _successful + 1;
                                                }
                                                else
                                                {
                                                    _unsuessful = _unsuessful + 1;
                                                }
                                            }
                                            else
                                            {
                                                reason = "Aggregator request already pending for verification.";
                                                _unsuessful = _unsuessful + 1;
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
                    }
                    else
                    {
                        ModalPopupExtender_Declincard.Show();

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please enter the reason for " + _strAlertMessage_Header + "');", true);
                        return;
                    }
                    //FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                    TxtRemarks.Text = string.Empty;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _strAlertMessage_Total + _reocrdsProcessed + " Successful : " + _successful + " Unsuccessful : " + _unsuessful + "  " + reason + " ');", true);
                    ViewState["ActionType"] = null;
                    ViewState["SelectionType"] = null;
                    return;
                }
                //ErrorLog.AggregatorTrace("OnBoardAggStatus | btnSaveAction_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : btnSaveAction_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong..! Please try again','Aggregator');", true);
                ViewState["ActionType"] = null;
                ViewState["SelectionType"] = null;
                return;
            }
        }

        protected void btnView_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void gvOnBoardAgg_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grid = (GridView)sender;

                    string BCStatus = grid.DataKeys[e.Row.RowIndex].Values[1].ToString();
                    ImageButton imageButton = e.Row.FindControl("lbtnEdit") as ImageButton;
                    CheckBox chBoxRow = e.Row.FindControl("chBoxSelectRow") as CheckBox;
                    if (imageButton != null)
                    {
                        if (BCStatus.ToLower() == "terminated" || BCStatus.ToLower() == "deactive")
                        {
                            imageButton.Enabled = false;
                            imageButton.Visible = false;
                            chBoxRow.Visible = false;

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
                ErrorLog.AggregatorTrace("OnBoardBcStatus: gvBCOnboard_RowDataBound: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'alert');", true);
                return;
            }
        }
        #endregion

        #region Cancel Action
        protected void btnCancelAction_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("OnBoardAggStatus | btnCancelAction_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());

                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Aggregator-btnCancelAction";
                _auditParams[2] = "btnDeactivate";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion

                string _alertMessage = string.Empty;
                if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.OnboardTerminate.ToString())
                {
                    _alertMessage = "BC Terminate";
                }
                else if (ViewState["ActionType"].ToString() == EnumCollection.EnumDBOperationType.Activate.ToString())
                {
                    _alertMessage = "BC Activate";
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Operation has cancelled.','" + _alertMessage + "');", true);
                ModalPopupExtender_Declincard.Hide();
                TxtRemarks.Text = string.Empty;
                ViewState["ActionType"] = null;
                FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
                ErrorLog.AggregatorTrace("OnBoardAggStatus | btnCancelAction_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                return;
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : btnCancelAction_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);
                ViewState["ActionType"] = null;
                return;
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
                        CheckBox CheckBoxAll = gvOnBoardAgg.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvOnBoardAgg.Rows)
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
                        CheckBox _CheckBoxAll = gvOnBoardAgg.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        foreach (GridViewRow row in gvOnBoardAgg.Rows)
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
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : chBoxSelectRow_CheckedChanged() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "OnBoardAggStatus.cs", "chBoxSelectRow_CheckedChanged()", Ex);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);
                return;
            }
        }

        protected void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox CheckBoxAll = gvOnBoardAgg.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                foreach (GridViewRow row in gvOnBoardAgg.Rows)
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
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : CheckBoxAll_CheckedChanged() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "OnBoardAggStatus.cs", "CheckBoxAll_CheckedChanged()", Ex);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);
                return;
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
                        CheckBox CheckBoxAll = gvOnBoardAgg.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = true;
                        foreach (GridViewRow row in gvOnBoardAgg.Rows)
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
                        CheckBox CheckBoxAll = gvOnBoardAgg.HeaderRow.FindControl("CheckBoxAll") as CheckBox;
                        CheckBoxAll.Checked = false;
                        foreach (GridViewRow row in gvOnBoardAgg.Rows)
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
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : CheckBoxAllOperationOnPageIndex() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "OnBoardAggStatus.cs", "CheckBoxAllOperationOnPageIndex()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);

                return;
                throw;
            }
        }
        #endregion

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("OnBoardAggStatus | BtnCsv_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Aggregator-BtnCsv";
                    _auditParams[2] = "btnDeactivate";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "Onboard Aggregator Details", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Page : OnBoardAggStatus.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("OnBoardAggStatus | BtnXls_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Aggregator-BtnXls";
                    _auditParams[2] = "btnDeactivate";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "Onboard Aggregator Details", dt);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Page : OnBoardAggStatus.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);
            }
        }

        #region Export
        //protected void btnexport_ServerClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ExportFormat _ExportFormat = new ExportFormat();
        //        string pageFilters = SetPageFiltersExport();
        //        DataSet dt = FillGrid(EnumCollection.EnumPermissionType.ActiveDeactive);
        //        if (ddlExport.SelectedItem.Text == "Excel")
        //        {
        //            if (dt != null && dt.Tables[0].Rows.Count > 0)
        //            {
        //                _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "Active BC Details", dt);
        //            }
        //            {
        //                /////// lblRecordCount.Text = "No Record's Found.";

        //                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
        //            }
        //        }
        //        else if (ddlExport.SelectedItem.Text == "CSV")
        //        {
        //            if (dt != null && dt.Tables[0].Rows.Count > 0)
        //            {
        //                _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "Active BC Details", dt);
        //            }
        //            else
        //            {

        //                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
        //            }
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        ////_systemLogger.WriteErrorLog(this, Ex);
        //        ErrorLog.AggregatorTrace(Ex.Message);
        //    }
        //}
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
                //_systemLogger.WriteErrorLog(this, Ex);
                ErrorLog.AggregatorTrace(Ex.Message);
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
                //_systemLogger.WriteErrorLog(this, Ex);
                ErrorLog.AggregatorTrace(Ex.Message);
            }
            return pageFilters;
        }
        //private void BindExport()
        //{
        //    try
        //    {
        //        DataSet ds = _BCEntity.BindExport();
        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count == 1)
        //            {
        //                ddlExport.DataTextField = "strDetailName";
        //                ddlExport.DataValueField = "numMasterDetailID";
        //                ddlExport.DataSource = ds;
        //                ddlExport.DataBind();
        //            }
        //            else
        //            {
        //                ddlExport.DataTextField = "strDetailName";
        //                ddlExport.DataValueField = "numMasterDetailID";
        //                ddlExport.DataSource = ds;
        //                ddlExport.DataBind();
        //            }
        //        }
        //        else
        //        {
        //            ddlExport.DataSource = null;
        //            ddlExport.DataBind();
        //            ddlExport.Items.Insert(0, new ListItem("-- Select --", "0"));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : BindDropdownsFranchise() \nException Occured\n" + Ex.Message);
        //        //_dbAccess.StoreErrorDescription(UserName, "OnBoardAggStatus.cs", "BindDropdownsFranchise()", Ex);

        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Verified BC');", true);
        //        return;
        //    }
        //}
        #endregion
        
        #region Dropdown Events
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _BCEntity.Mode = "getStateAndCity";
                _BCEntity.State = ddlState.SelectedValue.ToString();
                DataSet ds_State = _BCEntity.getStateAndCity();
                if (ds_State != null && ds_State.Tables.Count > 0 && ds_State.Tables[1].Rows.Count > 0)
                {
                    ddlCity.Items.Clear();
                    ddlCity.DataSource = ds_State.Tables[1];
                    ddlCity.DataValueField = "ID";
                    ddlCity.DataTextField = "Name";
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("-- City --", "0"));
                }
                else
                {
                    ddlCity.DataSource = null;
                    ddlCity.DataBind();
                    ddlCity.Items.Clear();
                    ddlCity.Items.Insert(0, new ListItem("-- City --", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AggregatorTrace("Page : OnBoardAggStatus.cs \nFunction : ddlState_SelectedIndexChanged\nException Occured\n" + ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "OnBoardBcStatus.aspx.cs", "ddlState_SelectedIndexChanged", ex);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);
                return;
            }
        }

        //protected void ddlClientCode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        BindDropdownsFranchise();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.AggregatorTrace("Page : OnBoardAggStatus.cs \nFunction : ddlClientCode_SelectedIndexChanged\nException Occured\n" + ex.Message);
        //        //_dbAccess.StoreErrorDescription(UserName, "OnBoardBcStatus.aspx.cs", "ddlClientCode_SelectedIndexChanged", ex);

        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator');", true);

        //        return;
        //    }
        //}
        #endregion
    }
}