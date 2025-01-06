using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessAccessLayer;
using AppLogger;
using System.Threading;
using System.Configuration;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ionic.Zip;

namespace SOR.Pages.TransactionReport
{
    public partial class AEPSTransactions : System.Web.UI.Page
    {
        #region Object Declration
        TransactionReportDAL _TransactionReportDAL = new TransactionReportDAL();
        AggregatorEntity _AggregatorEntity = new AggregatorEntity();
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions | Page_Load | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "AEPSTransactions.aspx", "22");
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
                            txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            FillBc();
                            FillAggregator();
                            BindAction();
                            UserPermissions.RegisterStartupScriptForNavigationListActive("7", "22");
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
                ErrorLog.TransactionReportTrace("AEPSTransactions: Page_Load: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #region Grid Methods
        public DataSet FillGrid(EnumCollection.EnumBindingType _EnumBindingType, string sortExpression = null)
        {
            DataSet _dsTransactionLogs = null;
            try
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                Setpropertise();
                
                _TransactionReportDAL.flag = Convert.ToString((int)_EnumBindingType);
                _dsTransactionLogs = _TransactionReportDAL.GetAEPSTransactionReport();
                
                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                {
                    if (_dsTransactionLogs != null && _dsTransactionLogs.Tables.Count > 0 && _dsTransactionLogs.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            DataView dv = _dsTransactionLogs.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";

                            dv.Sort = sortExpression + " " + this.SortDirection;
                            gvAEPSTransaction.DataSource = dv;
                            gvAEPSTransaction.DataBind();
                            gvAEPSTransaction.Visible = true;
                            lblRecordCount.Text = "Total " + Convert.ToString(_dsTransactionLogs.Tables[1].Rows.Count) + " Record(s) Found.";
                        }
                        {
                            gvAEPSTransaction.VirtualItemCount = Convert.ToInt32(_dsTransactionLogs.Tables[1].Rows[0][0]);
                            gvAEPSTransaction.DataSource = _dsTransactionLogs.Tables[0];
                            gvAEPSTransaction.DataBind();
                            gvAEPSTransaction.Visible = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideAccordion", "hideAccordion();", true);
                            lblRecordCount.Text = "Total " + Convert.ToString(_dsTransactionLogs.Tables[1].Rows[0][0]) + " Record(s) Found.";
                        }
                    }
                    else
                    {
                        gvAEPSTransaction.Visible = false;
                        lblRecordCount.Text = "Total 0 Record(s) Found.";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found In Search Criteria. Try again', 'Warning');", true);
                    }
                }
                ErrorLog.TransactionReportTrace("AEPSTransactions | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions: FillGrid(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return _dsTransactionLogs;
        }
        #endregion

        #region Setpropertise
        public void Setpropertise()
        {
            try
            {
                _TransactionReportDAL.UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                _TransactionReportDAL.Fromdate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                _TransactionReportDAL.Todate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                _TransactionReportDAL.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _TransactionReportDAL.AggregatorCode = ddlAggregator.SelectedValue != "0" ? ddlAggregator.SelectedValue : null;
                _TransactionReportDAL.AgentCode = !string.IsNullOrEmpty(txtAgentCode.Value) ? txtAgentCode.Value.Trim() : null;
                _TransactionReportDAL.RRN = !string.IsNullOrEmpty(txtRRNo.Value) ? txtRRNo.Value.Trim() : null;
                _TransactionReportDAL.TransStatus = Convert.ToInt32(ddlTransactionStatus.SelectedValue) != 0 ? Convert.ToInt32(ddlTransactionStatus.SelectedValue) : 0;
                _TransactionReportDAL.CType = Convert.ToInt32(ddlChannelType.SelectedValue) != 0 ? Convert.ToInt32(ddlChannelType.SelectedValue) : 0;
                _TransactionReportDAL.TransType = ddlTranType.SelectedValue != "0" ? (ddlTranType.SelectedValue) : null;
                _TransactionReportDAL.PageIndex = gvAEPSTransaction.PageIndex;
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions: Setpropertise(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion


        #region Search Button
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions | btnSearch_ServerClick | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Convert.ToDateTime(txtFromDate.Value) > Convert.ToDateTime(txtToDate.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('From date should be less than To date. Try again', 'Warning');", true);
                    return;
                }
                if(ddlbcCode.SelectedValue!="0")
                {
                    if (ddlAggregator.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select Aggregator. Try again', 'Warning');", true);
                        return;
                    }
                }
                else
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Report-AEPS";
                    _auditParams[2] = "btnSearch";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    FillGrid(EnumCollection.EnumBindingType.BindGrid);
                }
                ErrorLog.TransactionReportTrace("AEPSTransactions | btnSearch_ServerClick | End. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions: btnSearch_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Clear
        protected void btnClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions | btnClear_ServerClick | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Report-AEPS";
                _auditParams[2] = "btnClear";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                ddlChannelType.ClearSelection();
                ddlbcCode.ClearSelection();
                ddlAggregator.ClearSelection();
                txtAgentCode.Value = string.Empty;
                ddlTranType.ClearSelection();
                ddlTransactionStatus.ClearSelection();
                txtRRNo.Value = string.Empty;
                gvAEPSTransaction.DataSource = null;
                gvAEPSTransaction.DataBind();
                gvAEPSTransaction.PageIndex = 0;
                lblRecordCount.Text = string.Empty;
                ErrorLog.TransactionReportTrace("AEPSTransactions | btnClear_ServerClick | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions: btnClear_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
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
                pageFilters = "Generated By" + Convert.ToString(Session["Username"]);
                pageFilters += "From Date" + (Convert.ToDateTime(_TransactionReportDAL.Fromdate)).ToString("dd/MM/yyyy");
                pageFilters += "To Date" + (Convert.ToDateTime(_TransactionReportDAL.Todate)).ToString("dd/MM/yyyy");
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }
        #endregion

        #region Grid Events
        protected void gvAEPSTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAEPSTransaction.PageIndex = e.NewPageIndex;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions: gvAEPSTransaction_PageIndexChanging: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Sorting
        protected void gvAEPSTransaction_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                this.FillGrid(EnumCollection.EnumBindingType.BindGrid, e.SortExpression);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions: gvAEPSTransaction_Sorting: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        #endregion

        #region Dropdown
        //public void BindAggregator()
        //{
        //    try
        //    {
        //        ddlAggregator.Items.Clear();
        //        ddlAggregator.DataSource = null;
        //        ddlAggregator.DataBind();
        //        string UserName = Session["Username"].ToString();
        //        int IsRemoved = 0;
        //        int IsActive = 1;
        //        int IsdocUploaded = 1;
        //        int VerificationStatus = 1;
        //        string ClientID = null;
        //        DataTable dsbc = _AgentRegistrationDAL.GetAggregator(UserName, VerificationStatus, IsActive, IsRemoved, ClientID, IsdocUploaded);
        //        if (dsbc != null && dsbc.Rows.Count > 0 && dsbc.Rows.Count > 0)
        //        {
        //            if (dsbc.Rows.Count == 1)
        //            {
        //                ddlAggregator.DataSource = dsbc;
        //                ddlAggregator.DataValueField = "aggcode";
        //                ddlAggregator.DataTextField = "agname";
        //                ddlAggregator.DataBind();
        //            }
        //            else
        //            {
        //                ddlAggregator.DataSource = dsbc;
        //                ddlAggregator.DataValueField = "aggcode";
        //                ddlAggregator.DataTextField = "agname";
        //                ddlAggregator.DataBind();
        //                ddlAggregator.Items.Insert(0, new ListItem("-- Select --", "0"));
        //            }
        //        }
        //        else
        //        {
        //            ddlAggregator.DataSource = null;
        //            ddlAggregator.DataBind();
        //            ddlAggregator.Items.Insert(0, new ListItem("No Data Found", "0"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.AgentManagementTrace("Page : AEPSTransactions.cs \nFunction : BindAggregator()\nException Occured\n" + ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
        //        return;
        //    }
        //}
        public void FillBc()
        {
            try
            {
                ddlbcCode.Items.Clear();
                ddlbcCode.DataSource = null;
                ddlbcCode.DataBind();
                string UserName = Session["Username"].ToString();
                int IsRemoved = 0;
                int IsActive = 1;
                int IsdocUploaded = 1;
                int VerificationStatus = 1;
                DataTable dsbc = _AgentRegistrationDAL.GetBC(UserName, VerificationStatus, IsActive, IsRemoved, null, IsdocUploaded);
                if (dsbc != null && dsbc.Rows.Count > 0 && dsbc.Rows.Count > 0)
                {
                    if (dsbc.Rows.Count == 1)
                    {
                        ddlbcCode.DataSource = dsbc;
                        ddlbcCode.DataValueField = "BCCode";
                        ddlbcCode.DataTextField = "BCName";
                        ddlbcCode.DataBind();
                    }
                    else
                    {
                        ddlbcCode.DataSource = dsbc;
                        ddlbcCode.DataValueField = "BCCode";
                        ddlbcCode.DataTextField = "BCName";
                        ddlbcCode.DataBind();
                        ddlbcCode.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlbcCode.DataSource = null;
                    ddlbcCode.DataBind();
                    ddlbcCode.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AEPSTransactions.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }
        protected void ddlbcCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillAggregator();
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AEPSTransactions.cs \nFunction : ddlbcCode_SelectedIndexChanged()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }
        public void FillAggregator()
        {
            try
            {
                ddlAggregator.Items.Clear();
                ddlAggregator.DataSource = null;
                ddlAggregator.DataBind();
                string UserName = Session["Username"].ToString();
                int IsRemoved = 0;
                int IsActive = 1;
                int IsdocUploaded = 1;
                int VerificationStatus = 1;
                string ClientID = ddlbcCode.SelectedValue.ToString();
                DataTable dsbc = _AgentRegistrationDAL.GetAggregator(UserName, VerificationStatus, IsActive, IsRemoved, ClientID, IsdocUploaded);
                if (dsbc != null && dsbc.Rows.Count > 0 && dsbc.Rows.Count > 0)
                {
                    if (dsbc.Rows.Count == 1)
                    {
                        ddlAggregator.DataSource = dsbc;
                        ddlAggregator.DataValueField = "aggcode";
                        ddlAggregator.DataTextField = "agname";
                        ddlAggregator.DataBind();
                    }
                    else
                    {
                        ddlAggregator.DataSource = dsbc;
                        ddlAggregator.DataValueField = "aggcode";
                        ddlAggregator.DataTextField = "agname";
                        ddlAggregator.DataBind();
                        ddlAggregator.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlAggregator.DataSource = null;
                    ddlAggregator.DataBind();
                    ddlAggregator.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AEPSTransactions.cs \nFunction : FillAggregator()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }
        #endregion

        #region Export
        public void BindAction()
        {
            try
            {
                ddlAction.Items.Clear();
                ddlAction.DataSource = null;
                ddlAction.DataBind();

                DataTable dt = _TransactionReportDAL.GetAction();
                if (dt != null && dt.Rows.Count > 0 && dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1)
                    {
                        ddlAction.DataSource = dt;
                        ddlAction.DataValueField = "id";
                        ddlAction.DataTextField = "name";
                        ddlAction.DataBind();
                        ddlAction.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlAction.DataSource = dt;
                        ddlAction.DataValueField = "id";
                        ddlAction.DataTextField = "name";
                        ddlAction.DataBind();
                        ddlAction.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlAction.DataSource = null;
                    ddlAction.DataBind();
                    ddlAction.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AEPSTransactions.cs \nFunction : BindAction()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }
        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlAction.SelectedValue == "0")
                {
                    Button1.Visible = false;
                    Button2.Visible = false;
                    btnexport.Visible = false;
                }
                else if (ddlAction.SelectedValue=="1")
                {
                    Button1.Visible = true;
                    Button2.Visible = true;
                    btnexport.Visible = false;
                }
                else
                {
                    Button1.Visible = false;
                    Button2.Visible = false;
                    btnexport.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AEPSTransactions.cs \nFunction : ddlAction_SelectedIndexChanged()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }

        protected void btnexport_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlAction.SelectedValue == "2")
                {
                    try
                    {
                        ErrorLog.TransactionReportTrace("AEPSTransactions | btnexport_ServerClick | Export-To-Excel | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        ExportFormat _ExportFormat = new ExportFormat();
                        DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                        if (dt != null && dt.Tables[0].Rows.Count > 0)
                        {
                            #region Audit
                            _auditParams[0] = Session["Username"].ToString();
                            _auditParams[1] = "Report-AEPS";
                            _auditParams[2] = "Export-To-Excel";
                            _auditParams[3] = Session["LoginKey"].ToString();
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            #endregion
                            _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "AEPS Transactions", dt);
                        }
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                            return;
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("AEPSTransactions: btnexport_ServerClick | Excel : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                if (ddlAction.SelectedValue == "3")
                {
                    try
                    {
                        ErrorLog.TransactionReportTrace("AEPSTransactions | btnexport_ServerClick | Export-To-CSV | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        ExportFormat _ExportFormat = new ExportFormat();
                        DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                        if (dt != null && dt.Tables[0].Rows.Count > 0)
                        {
                            #region Audit
                            _auditParams[0] = Session["Username"].ToString();
                            _auditParams[1] = "Report-AEPS";
                            _auditParams[2] = "Export-To-CSV";
                            _auditParams[3] = Session["LoginKey"].ToString();
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            #endregion
                            _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "AEPS Transactions", dt);
                        }
                        else
                        {
                            txtFromDate.Value = txtFromDate.Value;
                            txtToDate.Value = txtToDate.Value;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                            return;
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("AEPSTransactions: btnexport_ServerClick | CSV : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                if (ddlAction.SelectedValue == "4")
                {
                    try
                    {
                        ErrorLog.TransactionReportTrace("AEPSTransactions | btnexport_ServerClick | Export-To-ZIP | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        #region Audit
                        _auditParams[0] = Session["Username"].ToString();
                        _auditParams[1] = "Report-AEPS";
                        _auditParams[2] = "Export-To-ZIP";
                        _auditParams[3] = Session["LoginKey"].ToString();
                        _LoginEntity.StoreLoginActivities(_auditParams);
                        #endregion
                        int ReportThreads = Convert.ToInt32(ConfigurationManager.AppSettings["ReportThreads"].ToString());
                        int ReportRecordsPerSheet = Convert.ToInt32(ConfigurationManager.AppSettings["ReportRecordsPerSheet"].ToString());
                        string ReportDownPath = ConfigurationManager.AppSettings["ReportDownPath"].ToString();
                        string StartTime = DateTime.Now.ToLongTimeString();
                        ErrorLog.TransactionReportTrace("Page : AEPS Transaction Report - Request Received For Export Report. Start-Time : " + StartTime);
                        Setpropertise();
                        List<DateRangeList> dateRangeList = DateRangeHandlerDynamic(_TransactionReportDAL.Fromdate, _TransactionReportDAL.Todate, ReportThreads);  //string , string , string , string 
                        DataTable dtTotalTxns = RunParallel(dateRangeList, _TransactionReportDAL.UserName, _TransactionReportDAL.AggregatorCode, _TransactionReportDAL.AgentCode, _TransactionReportDAL.RRN, _TransactionReportDAL.TransStatus, _TransactionReportDAL.CType, _TransactionReportDAL.TransType);
                        DataSet dsTxnTables = SplitDataTable(dtTotalTxns, ReportRecordsPerSheet);
                        if (dsTxnTables != null && dsTxnTables.Tables.Count > 0)
                        {
                            DeleteFiles(ReportDownPath);
                            string DownloadPath = GenerateCSVFiles(dsTxnTables, "Txn_Report_");
                            using (ZipFile zip = new ZipFile())
                            {
                                String[] files = Directory.GetFiles(DownloadPath);
                                foreach (string file in files)
                                {
                                    zip.AddFile(file, "Files");
                                }
                                Response.Clear();
                                Response.BufferOutput = false;
                                string zipName = String.Format("AEPS_Transactions_{0}.zip", DateTime.Now.ToString("yyyyMMMdd_HHmmss"));
                                Response.ContentType = "application/zip";
                                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                                zip.Save(Response.OutputStream);
                                Response.End();
                            }
                            ErrorLog.TransactionReportTrace("Page : TxnExport - Request Received For Export Report. Start-Time : " + StartTime + " End-Time : " + DateTime.Now.ToLongTimeString() + Environment.NewLine);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found', 'Warning');", true);
                            return;
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("AEPSTransactions: btnexport_ServerClick | ZIP : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                else
                {
                    ErrorLog.AgentManagementTrace("Page : AEPSTransactions.cs \nFunction : ddlAction_SelectedIndexChanged() | Please select action type for export.");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please select action type for export', 'AEPS Transactions');", true);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AEPSTransactions.cs \nFunction : ddlAction_SelectedIndexChanged()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }

        #region ZIP Export Methods
        public DataSet SplitDataTable(DataTable tableData, int max)
        {
            DataSet newDs = new DataSet();
            string StartTime = DateTime.Now.ToLongTimeString();
            try
            {
                if (tableData != null && tableData.Rows.Count > 0)
                {
                    int i = 0;
                    int j = 1;
                    int countOfRows = tableData.Rows.Count;

                    DataTable newDt = tableData.Clone();
                    newDt.TableName = "Txn_Sheet_" + j;
                    newDt.Clear();
                    foreach (DataRow row in tableData.Rows)
                    {
                        DataRow newRow = newDt.NewRow();
                        newRow.ItemArray = row.ItemArray;

                        newDt.Rows.Add(newRow);
                        i++;

                        countOfRows--;

                        if (i == max)
                        {
                            newDs.Tables.Add(newDt);
                            j++;
                            newDt = tableData.Clone();
                            newDt.TableName = tableData.TableName + "_" + j;
                            newDt.Clear();
                            i = 0;
                        }

                        if (countOfRows == 0 && i < max)
                        {
                            newDs.Tables.Add(newDt);
                            j++;
                            newDt = tableData.Clone();
                            newDt.TableName = tableData.TableName + "_" + j;
                            newDt.Clear();
                            i = 0;
                        }
                    }
                }
                ErrorLog.TransactionReportTrace("Page : TxnExport - Request Received For Split Data Table. Per Sheet Record Range : " + max + " Total Records : " + tableData.Rows.Count + " Start-Time : " + StartTime + " End-Time : " + DateTime.Now.ToLongTimeString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportError(Ex);
            }
            return newDs;
        }

        private List<DateRangeList> DateRangeHandlerDynamic(string FromDate, string ToDate, int TaskCount)
        {
            ErrorLog.TransactionReportTrace("Page : TxnExport - Request Received For Date Range Handler. FromDate : " + FromDate + " ToDate : " + ToDate);
            List<DateRangeList> _DateRangeList = null;
            if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
            {
                DateTime _FromDate = Convert.ToDateTime(FromDate);
                DateTime _ToDate = Convert.ToDateTime(ToDate);
                var NoOfDays = (_ToDate.AddDays(1) - _FromDate).TotalDays;
                _DateRangeList = new List<DateRangeList>();
                if (NoOfDays <= 3)
                {
                    DateRangeList _DateRange = new DateRangeList();
                    _DateRange.StartDate = Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd");
                    _DateRange.EndDate = Convert.ToDateTime(_ToDate).ToString("yyyy-MM-dd");
                    _DateRangeList.Add(_DateRange);
                }
                else
                {
                    int MainThreads = Convert.ToInt32(NoOfDays) / TaskCount;
                    int RemDays = Convert.ToInt32(NoOfDays) % TaskCount;
                    if (MainThreads == 0)
                    {
                        DateRangeList _DateRange = new DateRangeList();
                        _DateRange.StartDate = Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd");
                        _DateRange.EndDate = Convert.ToDateTime(_ToDate).ToString("yyyy-MM-dd");
                        _DateRangeList.Add(_DateRange);
                    }
                    else if (MainThreads > 0 && RemDays == 0)
                    {
                        DateTime tempDate = _FromDate;
                        for (int ImainThreads = 1; ImainThreads <= MainThreads; ImainThreads++)
                        {
                            DateRangeList _DateRange = new DateRangeList();
                            _DateRange.StartDate = ImainThreads == 1 ? Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd") : tempDate.AddDays(1).ToString("yyyy-MM-dd");
                            _DateRange.EndDate = Convert.ToDateTime(_DateRange.StartDate).AddDays(TaskCount - 1).ToString("yyyy-MM-dd");
                            _DateRangeList.Add(_DateRange);
                            tempDate = Convert.ToDateTime(_DateRange.StartDate).AddDays(TaskCount - 1);
                        }
                    }
                    else if (MainThreads > 0 && RemDays > 0)
                    {
                        DateTime tempDate = _FromDate;
                        for (int ImainThreads = 1; ImainThreads <= MainThreads; ImainThreads++)
                        {
                            DateRangeList _DateRange = new DateRangeList();
                            _DateRange.StartDate = ImainThreads == 1 ? Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd") : tempDate.AddDays(1).ToString("yyyy-MM-dd");
                            _DateRange.EndDate = Convert.ToDateTime(_DateRange.StartDate).AddDays(TaskCount - 1).ToString("yyyy-MM-dd");
                            _DateRangeList.Add(_DateRange);
                            tempDate = Convert.ToDateTime(_DateRange.EndDate);
                        }
                        DateRangeList _DateRangeRemDays = new DateRangeList();
                        _DateRangeRemDays.StartDate = tempDate.AddDays(1).ToString("yyyy-MM-dd");
                        _DateRangeRemDays.EndDate = Convert.ToDateTime(_ToDate).ToString("yyyy-MM-dd");
                        _DateRangeList.Add(_DateRangeRemDays);
                    }
                }
            }
            return _DateRangeList;
        }

        private void DeleteFiles(string FileDirectory)
        {
            try
            {
                string[] files = Directory.GetFiles(FileDirectory);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (IOException Ex)
            {
                ErrorLog.TransactionReportError(Ex);
            }
        }

        private string GenerateCSVFiles(DataSet _DataSet, string FileName)
        {
            string FilesPath = ConfigurationManager.AppSettings["ReportDownPath"].ToString();
            string StartTime = DateTime.Now.ToLongTimeString();
            try
            {
                int fileRange = 1;
                foreach (DataTable _DataTable in _DataSet.Tables)
                {
                    StreamWriter CsvfileWriter = new StreamWriter(FilesPath + "\\" + FileName + fileRange.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".csv");
                    using (CsvfileWriter)
                    {
                        for (int k = 0; k < _DataTable.Columns.Count; k++)
                        {
                            CsvfileWriter.Write(_DataTable.Columns[k].ToString() + ',');
                        }
                        CsvfileWriter.WriteLine();
                        foreach (DataRow row in _DataTable.Rows)
                        {
                            CsvfileWriter.WriteLine(string.Join(",", row.ItemArray));
                        }
                    }
                    fileRange += 1;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportError(Ex);
            }
            return FilesPath;
        }

        private string GenerateCSVFiles1(DataSet _DataSet, string FileName)
        {
            string FilesPath = ConfigurationManager.AppSettings["ReportDownPath"].ToString();
            string StartTime = DateTime.Now.ToLongTimeString();
            try
            {
                int fileRange = 1;
                foreach (DataTable _DataTable in _DataSet.Tables)
                {
                    StreamWriter CsvfileWriter = new StreamWriter(FilesPath + "\\" + FileName + fileRange.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".csv");
                    using (CsvfileWriter)
                    {
                        for (int k = 1; k < _DataTable.Columns.Count; k++)
                        {
                            CsvfileWriter.Write(_DataTable.Columns[k].ToString() + ',');
                        }
                        CsvfileWriter.WriteLine();
                        foreach (DataRow row in _DataTable.Rows)
                        {
                            CsvfileWriter.WriteLine(string.Join(",", row.ItemArray));
                        }
                    }
                    fileRange += 1;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportError(Ex);
            }
            return FilesPath;
        }

        public static DataTable FetchTxnsDB(string startDate, string EndDate, string UserName, string Aggregator, string AgentCode, string RRN, int TransactionStatus, int CType, string TransType)
        {
            DataTable _dtTxns = new DataTable();
            try
            {
                TransactionReportDAL _TransactionReportDAL = new TransactionReportDAL();
                _TransactionReportDAL.UserName = UserName;
                _TransactionReportDAL.Fromdate = startDate;
                _TransactionReportDAL.Todate = EndDate;
                _TransactionReportDAL.AggregatorCode = Aggregator;
                _TransactionReportDAL.AgentCode = AgentCode;
                _TransactionReportDAL.RRN = RRN;
                _TransactionReportDAL.TransStatus = TransactionStatus;
                _TransactionReportDAL.CType = CType;
                _TransactionReportDAL.TransType = TransType;
                //_TransactionReportDAL.PageIndex = null;
                _TransactionReportDAL.flag = Convert.ToString((int)EnumCollection.EnumBindingType.Export);
                DataSet dsTransactionLogs = _TransactionReportDAL.GetAEPSTransactionReport();
                _dtTxns = dsTransactionLogs != null && dsTransactionLogs.Tables.Count > 0 ? dsTransactionLogs.Tables[0] : null;
            }
            catch (Exception)
            {
                throw;
            }
            return _dtTxns;
        }

        private static object _lockObj = new object();
        public static DataTable RunParallel(List<DateRangeList> _DateRangeList, string UserName, string Aggregator, string AgentCode, string RRN, int TransactionStatus, int CType, string TransType)
        {
            DataTable _dataTable = new DataTable();
            Parallel.ForEach(_DateRangeList, (dateRange) =>
            {
                DataTable dtResultSET = FetchTxnsDB(dateRange.StartDate, dateRange.EndDate, UserName, Aggregator, AgentCode, RRN, TransactionStatus, CType, TransType);
                lock (_lockObj)
                {
                    if (dtResultSET != null && dtResultSET.Rows.Count > 0)
                    {
                        _dataTable.Merge(dtResultSET.Copy());
                        _dataTable.AcceptChanges();
                    }
                }
            });
            return _dataTable;
        }
        #endregion

        #endregion
    }
    public class DateRangeList
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}