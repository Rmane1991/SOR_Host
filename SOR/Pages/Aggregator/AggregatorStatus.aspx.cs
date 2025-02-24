﻿using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessAccessLayer;
using AppLogger;
using System;
using System.Configuration;
using System.Web;
using System.Threading;

namespace SOR.Pages.Aggregator
{
    public partial class AggregatorStatus : System.Web.UI.Page
    {

        #region Objects Declaration
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        DataSet _dsAllAgents = null;
        public string UserName { get; set; }
        BCEntity _BCEntity = new BCEntity();
        public ExportFormat _exportFormat = null;
        public ExportFormat exportFormat
        {
            get { if (_exportFormat == null) _exportFormat = new ExportFormat(); return _exportFormat; }
            set { _exportFormat = value; }
        }
        #endregion;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorStatus | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {

                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "AggregatorStatus.aspx", "17");
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
                        UserPermissions.RegisterStartupScriptForNavigationListActive("5", "17");
                        if (!IsPostBack && HasPagePermission)
                        {
                            fillGrid();
                            ddlVerification.SelectedValue = "2";
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
                ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : Page_Load() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'AggregatorStatus');", true);
                return;
            }
        }
        
        #region FillGrid
        public DataSet fillGrid()
        {
            DataSet _dsAllAgents = new DataSet();
            try
            {
                ErrorLog.AggregatorTrace("AggregatorStatus | fillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
                _BCEntity.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
                ///// setProperties();
                _dsAllAgents = _BCEntity.AggStatusReportGrid();
                ViewState["Data"] = _dsAllAgents;

                if (_dsAllAgents.Tables.Count > 0 && _dsAllAgents.Tables[0].Rows.Count > 0)
                {
                    gvTransactions.DataSource = _dsAllAgents.Tables[0];
                    gvTransactions.DataBind();
                    gvTransactions.Visible = true;
                    panelGrid.Visible = true;
                    lblRecordCount.Visible = true;
                    lblRecordCount.Text = "Total " + Convert.ToString(_dsAllAgents.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    gvTransactions.Visible = false;
                    panelGrid.Visible = false;
                    lblRecordCount.Text = "Total 0 Record(s) Found.";

                }
                ErrorLog.AggregatorTrace("AggregatorStatus | fillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : fillGrid() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'AggregatorStatus');", true);
                return _dsAllAgents;
            }
            return _dsAllAgents;
        }


        #endregion

        #region setProperties
        private void setProperties()
        {
            try
            {
                if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "1")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);

                    fillGrid();
                }

                //else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "2")
                //{
                //    _BCEntity.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerApprove);
                //    fillGrid();
                //}
                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "2")
                {
                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                    fillGrid();
                }


                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "1")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerDecline);
                    fillGrid();
                }
                //else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "2")
                //{
                //    _BCEntity.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerDecline);
                //    fillGrid();
                //}
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "2")
                {
                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerDecline);
                    fillGrid();
                }


                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "1")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                    fillGrid();
                }
                //else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "2")
                //{
                //    _BCEntity.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerPending);
                //    fillGrid();
                //}
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "2")
                {
                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                    fillGrid();
                }

                else
                {
                    gvTransactions.DataSource = null;
                    gvTransactions.DataBind();

                }


                _BCEntity.Clientcode = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                ////_BCEntity.AgentID = ddlAgent.SelectedValue != "All" ? ddlAgent.SelectedValue : null;
                _BCEntity.Status = ddlStatusType.SelectedValue != "All" ? Convert.ToInt32(ddlStatusType.SelectedValue.ToString()) : 0;
                _BCEntity.UserName = Session["Username"].ToString();
            }
            catch (Exception EX)
            {
                ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : setProperties() \nException Occured\n" + EX.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'AggregatorStatus');", true);
                return;
            }
        }

        #endregion

        #region Grid Events

        protected void gvTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTransactions.PageIndex = e.NewPageIndex;
                fillGrid();
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : gvTransactions_PageIndexChanging() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'AggregatorStatus');", true);
                return;
            }
        }

        #endregion

        #region Search
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorStatus | btnSearch_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());

                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Aggregator-AggregatorStatus";
                _auditParams[2] = "btnSearch";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion

                if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "1")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);

                    fillGrid();
                }

                //else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "2")
                //{
                //    _BCEntity.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerApprove);
                //    fillGrid();
                //}
                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "2")
                {
                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                    fillGrid();
                }
                //else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "3")
                //{
                //    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                //    fillGrid();
                //}
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "1")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerDecline);
                    fillGrid();
                }
                //else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "2")
                //{
                //    _BCEntity.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerDecline);
                //    fillGrid();
                //}
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "2")
                {
                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerDecline);
                    fillGrid();
                }
                //else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "3")
                //{
                //    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerDecline);
                //    fillGrid();
                //}
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "1")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                    fillGrid();
                }
                //else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "2")
                //{
                //    _BCEntity.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerPending);
                //    fillGrid();
                //}
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "2")
                {
                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                    fillGrid();
                }
                //else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "3")
                //{
                //    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                //    fillGrid();
                //}
                else
                {
                    gvTransactions.DataSource = null;
                    gvTransactions.DataBind();
                    fillGrid();
                }
                ErrorLog.AggregatorTrace("AggregatorStatus | btnSearch_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : btnSearch_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','AggregatorStatus');", true);
                return;
            }
        }
        #endregion

        #region Reset Button
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorStatus | btnReset_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Aggregator-AggregatorStatus";
                _auditParams[2] = "btnReset";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                ddlClient.SelectedValue = "0";
                ddlStatusType.SelectedValue = "0";
                ddlAgent.SelectedValue = "0";
                ddlOperationType.SelectedValue = "0";
                ddlVerification.SelectedValue = "0";
                ddlStatus.SelectedValue = "0";
                ddlBCType.SelectedValue = "0";
                fillGrid();
                ErrorLog.AggregatorTrace("AggregatorStatus | btnReset_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : btnReset_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','AggregatorStatus');", true);
                return;
            }
        }
        #endregion


        #region CSV
        protected void btnExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorStatus | btnExportCSV_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                setProperties();
                DataSet _dsAllAgents = _BCEntity.BCStatusReportGrid();
                if (_dsAllAgents != null)
                {
                    if (_dsAllAgents.Tables.Count > 0)
                    {
                        #region Audit
                        _auditParams[0] = Session["Username"].ToString();
                        _auditParams[1] = "Aggregator-AggregatorStatus";
                        _auditParams[2] = "btnExportCSV";
                        _auditParams[3] = Session["LoginKey"].ToString();
                        _LoginEntity.StoreLoginActivities(_auditParams);
                        #endregion
                        exportFormat.ExportInCSV(Session["Username"].ToString(), "Proxima", "Aggregator Status Details", _dsAllAgents);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "showSuccess('No data found.', 'Aggregator Status Details');", true);
                }
            }
            catch (Exception EX)
            {
                ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : btnExportCSV_Click() \nException Occured\n" + EX.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','AggregatorStatus');", true);
                return;
            }
        }
        #endregion

        #region Export
        //protected void btnexport_ServerClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ExportFormat _ExportFormat = new ExportFormat();
        //        string pageFilters = SetPageFiltersExport();
        //        DataSet dt = fillGrid();
        //        if (ddlExport.SelectedItem.Text == "Excel")
        //        {
        //            if (dt != null && dt.Tables[0].Rows.Count > 0)
        //            {
        //                _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "Aggregator Status Detailss", dt);
        //            }
        //            {
        //                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);
        //            }
        //        }
        //        else if (ddlExport.SelectedItem.Text == "CSV")
        //        {
        //            if (dt != null && dt.Tables[0].Rows.Count > 0)
        //            {
        //                _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "Aggregator Status Details", dt);
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);
        //            }
        //        }
        //        //else if (ddlExport.SelectedItem.Text == "ZIP")
        //        //{
        //        //    if (dt != null && dt.Rows.Count > 0)
        //        //    {
        //        //        DataTable dt1 = dt.Copy();
        //        //        //_ExportFormat.ExportZIP(dt1, "PNB", "ATM Master", DateTime.Now.ToShortDateString());
        //        //    }
        //        //    else
        //        //    { ScriptManager.RegisterStartupScript(this, typeof(Page), "Script", "alert('No data found.', 'Warning');", true); }
        //        //}
        //        //else
        //        //{
        //        //}
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);
        //    }
        //}

        private string SetPageFiltersExport()
        {
            string pageFilters = string.Empty;
            try
            {
                pageFilters = "Generated By " + Convert.ToString(Session["Username"]);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : SetPageFiltersExport\nException Occured\n" + Ex.Message);
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
        //            ddlExport.Items.Insert(0, new ListItem("-- Franchise --", "0"));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : BindExport() \nException Occured\n" + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Verified Agent');", true);
        //        return;
        //    }
        //}

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorStatus | BtnCsv_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                setProperties();
                DataSet _dsAllAgents = _BCEntity.BCStatusReportGrid();
                if (_dsAllAgents != null && _dsAllAgents.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Aggregator-AggregatorStatus";
                    _auditParams[2] = "BtnCsv";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "Aggregator Status Details", _dsAllAgents);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : BtnCsv_Click\nException Occured\n" + Ex.Message);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorStatus | BtnXls_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                //_BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                //setProperties();
                //DataSet _dsAllAgents = _BCEntity.BCStatusReportGrid();
                DataSet _dsAllAgents = ViewState["Data"] as DataSet;
                if (_dsAllAgents != null && _dsAllAgents.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Aggregator-AggregatorStatus";
                    _auditParams[2] = "BtnXls";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "Aggregator Status Details", _dsAllAgents);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : AggregatorStatus.cs \nFunction : BtnXls_Click\nException Occured\n" + Ex.Message);
            }
        }
        #endregion

        #region Clear

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorStatus | btnCancel_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Aggregator-AggregatorStatus";
                _auditParams[2] = "btnCancel";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                DIVFilter.Visible = true;
                DIVRegister.Visible = false;
                ErrorLog.AggregatorTrace("AggregatorStatus | btnCancel_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : OnBoardAggStatus.cs \nFunction : btnCancel_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Deactive Aggregator');", true);
                return;
            }
        }
        protected void clearselection()
        {
            ddlCategory.SelectedValue = "0";
            ddlState.SelectedValue = "0";
            ddlCity.SelectedValue = "0";
            ddlClient.SelectedValue = "0";
            ddlCountry.SelectedValue = "0";
            ddlAgent.SelectedValue = "0";
            ddlGender.SelectedValue = "0";
            DDlOrg.SelectedValue = "0";
            ddlStatusType.SelectedValue = "0";
        }
        #endregion
    }
}