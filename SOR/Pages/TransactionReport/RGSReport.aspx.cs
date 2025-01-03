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
    public partial class RGSReport : System.Web.UI.Page
    {
        #region Object Declration
        TransactionReportDAL _TransactionReportDAL = new TransactionReportDAL();
        AggregatorEntity _AggregatorEntity = new AggregatorEntity();
        RuleEntity _RuleEntity = new RuleEntity();
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.TransactionReportTrace("RGSReport | Page_Load | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "RGSReport.aspx", "26");
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
                            BindGroup();
                            BindAction();
                            UserPermissions.RegisterStartupScriptForNavigationListActive("7", "26");
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
                ErrorLog.TransactionReportTrace("RGSReport: Page_Load: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
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
                ErrorLog.TransactionReportTrace("RGSReport | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                Setpropertise();
                
                _TransactionReportDAL.flag = Convert.ToString((int)_EnumBindingType);
                _dsTransactionLogs = _TransactionReportDAL.GetRGSTransactionReport();
                
                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                {
                    if (_dsTransactionLogs != null && _dsTransactionLogs.Tables.Count > 0 && _dsTransactionLogs.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            DataView dv = _dsTransactionLogs.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";

                            dv.Sort = sortExpression + " " + this.SortDirection;
                            gvRuleWiseTransaction.DataSource = dv;
                            gvRuleWiseTransaction.DataBind();
                            gvRuleWiseTransaction.Visible = true;
                            lblRecordCount.Text = "Total " + Convert.ToString(_dsTransactionLogs.Tables[1].Rows.Count) + " Record(s) Found.";
                        }
                        {
                            gvRuleWiseTransaction.VirtualItemCount = Convert.ToInt32(_dsTransactionLogs.Tables[1].Rows[0][0]);
                            gvRuleWiseTransaction.DataSource = _dsTransactionLogs.Tables[0];
                            gvRuleWiseTransaction.DataBind();
                            gvRuleWiseTransaction.Visible = true;
                            lblRecordCount.Text = "Total " + Convert.ToString(_dsTransactionLogs.Tables[1].Rows[0][0]) + " Record(s) Found.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideAccordion", "hideAccordion();", true);
                        }
                    }
                    else
                    {
                        gvRuleWiseTransaction.Visible = false;
                        lblRecordCount.Text = "Total 0 Record(s) Found.";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found In Search Criteria. Try again', 'Warning');", true);
                    }
                }
                ErrorLog.TransactionReportTrace("RGSReport | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RGSReport: FillGrid: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
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
                _TransactionReportDAL.GroupId = ddlGroup.SelectedValue != "0" ? ddlGroup.SelectedValue : null;
                _TransactionReportDAL.RuleId = ddlRule.SelectedValue != "0" ? ddlRule.SelectedValue : null;
                _TransactionReportDAL.CType = Convert.ToInt32(ddlChannelType.SelectedValue) != 0 ? Convert.ToInt32(ddlChannelType.SelectedValue) : 0;
                _TransactionReportDAL.PageIndex = gvRuleWiseTransaction.PageIndex;
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RGSReport: Setpropertise: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion


        #region Search Button
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.TransactionReportTrace("RGSReport | btnSearch_ServerClick | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Convert.ToDateTime(txtFromDate.Value) > Convert.ToDateTime(txtToDate.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('From date should be less than To date. Try again', 'Warning');", true);
                    return;
                }
                else
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Report-RGSReport";
                    _auditParams[2] = "btnSearch";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    FillGrid(EnumCollection.EnumBindingType.BindGrid);
                }
                ErrorLog.TransactionReportTrace("RGSReport | btnSearch_ServerClick | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RGSReport: btnSearch_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Clear
        protected void btnClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.TransactionReportTrace("RGSReport | btnClear_ServerClick | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Report-RGSReport";
                _auditParams[2] = "btnClear";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                ddlChannelType.ClearSelection();
                ddlGroup.ClearSelection();
                ddlRule.ClearSelection();
                gvRuleWiseTransaction.DataSource = null;
                gvRuleWiseTransaction.DataBind();
                gvRuleWiseTransaction.PageIndex = 0;
                lblRecordCount.Text = string.Empty;
                ErrorLog.TransactionReportTrace("RGSReport | btnClear_ServerClick | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RGSReport: btnClear_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
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
                ErrorLog.TransactionReportTrace("RGSReport: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }
        #endregion

        #region Grid Events
        protected void gvRuleWiseTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRuleWiseTransaction.PageIndex = e.NewPageIndex;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RGSReport: gvRuleWiseTransaction_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Sorting
        protected void gvRuleWiseTransaction_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                this.FillGrid(EnumCollection.EnumBindingType.BindGrid, e.SortExpression);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RGSReport: gvRuleWiseTransaction_Sorting: Exception: " + Ex.Message);
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
        private void BindGroup()
        {
            try
            {
                ddlGroup.Items.Clear();
                ddlGroup.DataSource = null;
                ddlGroup.DataBind();
                string UserName = Session["Username"].ToString();
                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                var tables = _RuleEntity.GetDropDownValues();

                if (tables != null)
                {
                    if (tables.ContainsKey("temp_bindgroup"))
                    {
                        DataTable bindGroupTable = tables["temp_bindgroup"];

                        if (bindGroupTable.Rows.Count > 0)
                        {
                            ddlGroup.Items.Clear();
                            ddlGroup.DataSource = bindGroupTable;
                            ddlGroup.DataTextField = "group_name";
                            ddlGroup.DataValueField = "id";
                            ddlGroup.DataBind();
                            ddlGroup.Items.Insert(0, new ListItem("-- All --", "0"));
                        }
                        else
                        {
                            ddlGroup.Items.Clear();
                            ddlGroup.DataSource = null;
                            ddlGroup.DataBind();
                        }
                    }
                    else
                    {
                        ddlGroup.DataSource = null;
                        ddlGroup.DataBind();
                        ddlGroup.Items.Insert(0, new ListItem("No Data Found", "0"));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : RGSReport.cs \nFunction : BindGroup()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'RGS Report');", true);
                return;
            }
        }
        private void BindRule()
        {
            try
            {
                ddlRule.Items.Clear();
                ddlRule.DataSource = null;
                ddlRule.DataBind();
                string UserName = Session["Username"].ToString();
                _TransactionReportDAL.GroupId = ddlGroup.SelectedValue.ToString();
                DataTable dsrule = _TransactionReportDAL.GetRule();
                if (dsrule != null && dsrule.Rows.Count > 0 && dsrule.Rows.Count > 0)
                {
                    if (dsrule.Rows.Count == 1)
                    {
                        ddlRule.DataSource = dsrule;
                        ddlRule.DataValueField = "rule_id";
                        ddlRule.DataTextField = "rulename";
                        ddlRule.DataBind();
                        ddlRule.Items.Insert(0, new ListItem("-- All --", "0"));
                    }
                    else
                    {
                        ddlRule.DataSource = dsrule;
                        ddlRule.DataValueField = "rule_id";
                        ddlRule.DataTextField = "rulename";
                        ddlRule.DataBind();
                        ddlRule.Items.Insert(0, new ListItem("-- All --", "0"));
                    }
                }
                else
                {
                    ddlRule.DataSource = null;
                    ddlRule.DataBind();
                    ddlRule.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : RGSReport.cs \nFunction : BindRule()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'RGS Report');", true);
                return;
            }
        }
        #endregion

        #region Export
        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                //string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "Rule Wise Transactions", dt);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RGSReport: BtnXls_Click: Exception: " + Ex.Message);
            }
        }
        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                //string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "Rule Wise Transactions", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RGSReport: BtnCsv_Click: Exception: " + Ex.Message);
            }
        }
        #endregion

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindRule();
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RGSReport: ddlGroup_SelectedIndexChanged(): Exception: " + Ex.Message);
            }
        }
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
                ErrorLog.AgentManagementTrace("Page : RGSReport.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }
        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlAction.SelectedValue == "1")
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
                ErrorLog.AgentManagementTrace("Page : RGSReport.cs \nFunction : ddlAction_SelectedIndexChanged()\nException Occured\n" + ex.Message);
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
                        ErrorLog.TransactionReportTrace("RGSReport | btnexport_ServerClick | Export-To-Excel | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        ExportFormat _ExportFormat = new ExportFormat();
                        //string pageFilters = SetPageFiltersExport();
                        DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                        if (dt != null && dt.Tables[0].Rows.Count > 0)
                        {
                            #region Audit
                            _auditParams[0] = Session["Username"].ToString();
                            _auditParams[1] = "Report-RGSReport";
                            _auditParams[2] = "Export-To-Excel";
                            _auditParams[3] = Session["LoginKey"].ToString();
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            #endregion
                            _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "Rule Wise Transactions", dt);
                        }
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("RGSReport: btnexport_ServerClick | Excel : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                if (ddlAction.SelectedValue == "3")
                {
                    try
                    {
                        ErrorLog.TransactionReportTrace("RGSReport | btnexport_ServerClick | Export-To-CSV | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        ExportFormat _ExportFormat = new ExportFormat();
                        //string pageFilters = SetPageFiltersExport();
                        DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                        if (dt != null && dt.Tables[0].Rows.Count > 0)
                        {
                            #region Audit
                            _auditParams[0] = Session["Username"].ToString();
                            _auditParams[1] = "Report-RGSReport";
                            _auditParams[2] = "Export-To-CSV";
                            _auditParams[3] = Session["LoginKey"].ToString();
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            #endregion
                            _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "Rule Wise Transactions", dt);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("RGSReport: btnexport_ServerClick | CSV : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                if (ddlAction.SelectedValue == "4")
                {
                    try
                    {
                        ErrorLog.TransactionReportTrace("RGSReport | btnexport_ServerClick | Export-To-Zip | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        #region Audit
                        _auditParams[0] = Session["Username"].ToString();
                        _auditParams[1] = "Report-RGSReport";
                        _auditParams[2] = "Export-To-ZIP";
                        _auditParams[3] = Session["LoginKey"].ToString();
                        _LoginEntity.StoreLoginActivities(_auditParams);
                        #endregion
                        int ReportThreads = Convert.ToInt32(ConfigurationManager.AppSettings["ReportThreads"].ToString());
                        int ReportRecordsPerSheet = Convert.ToInt32(ConfigurationManager.AppSettings["ReportRecordsPerSheet"].ToString());
                        string ReportDownPath = ConfigurationManager.AppSettings["ReportDownPath"].ToString();
                        string StartTime = DateTime.Now.ToLongTimeString();
                        ErrorLog.TransactionReportTrace("Page : Rule Wise Transaction Report - Request Received For Export Report. Start-Time : " + StartTime);
                        Setpropertise();
                        List<DateRangeList> dateRangeList = DateRangeHandlerDynamic(_TransactionReportDAL.Fromdate, _TransactionReportDAL.Todate, ReportThreads);
                        DataTable dtTotalTxns = RunParallel(dateRangeList, _TransactionReportDAL.UserName, _TransactionReportDAL.GroupId, _TransactionReportDAL.RuleId,_TransactionReportDAL.CType);
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
                                string zipName = String.Format("Rule_Wise_Transactions_{0}.zip", DateTime.Now.ToString("yyyyMMMdd_HHmmss"));
                                Response.ContentType = "application/zip";
                                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                                zip.Save(Response.OutputStream);
                                Response.End();
                            }
                            ErrorLog.TransactionReportTrace("Page : RGSReport - Request Received For Export Report. Start-Time : " + StartTime + " End-Time : " + DateTime.Now.ToLongTimeString() + Environment.NewLine);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found', 'Warning');", true);
                            return;
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("RGSReport: btnexport_ServerClick | ZIP : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                else
                {
                    ErrorLog.AgentManagementTrace("Page : RGSReport.cs \nFunction : ddlAction_SelectedIndexChanged() | Please select action type for export.");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please select action type for export', 'AEPS Transactions');", true);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : RGSReport.cs \nFunction : ddlAction_SelectedIndexChanged()\nException Occured\n" + ex.Message);
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
                ErrorLog.TransactionReportTrace("Page : RGSReport - Request Received For Split Data Table. Per Sheet Record Range : " + max + " Total Records : " + tableData.Rows.Count + " Start-Time : " + StartTime + " End-Time : " + DateTime.Now.ToLongTimeString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportError(Ex);
            }
            return newDs;
        }

        private List<DateRangeList> DateRangeHandlerDynamic(string FromDate, string ToDate, int TaskCount)
        {
            ErrorLog.TransactionReportTrace("Page : RGSReport - Request Received For Date Range Handler. FromDate : " + FromDate + " ToDate : " + ToDate);
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

        public static DataTable FetchTxnsDB(string startDate, string EndDate, string UserName, string GroupId, string RuleId, int CType)
        {
            DataTable _dtTxns = new DataTable();
            try
            {
                TransactionReportDAL _TransactionReportDAL = new TransactionReportDAL();
                _TransactionReportDAL.UserName = UserName;
                _TransactionReportDAL.Fromdate = startDate;
                _TransactionReportDAL.Todate = EndDate;
                _TransactionReportDAL.GroupId = GroupId;
                _TransactionReportDAL.RuleId = RuleId;
                _TransactionReportDAL.CType = CType;
                _TransactionReportDAL.flag = Convert.ToString((int)EnumCollection.EnumBindingType.Export);
                DataSet dsTransactionLogs = _TransactionReportDAL.GetRGSTransactionReport();
                _dtTxns = dsTransactionLogs != null && dsTransactionLogs.Tables.Count > 0 ? dsTransactionLogs.Tables[0] : null;
            }
            catch (Exception)
            {
                throw;
            }
            return _dtTxns;
        }

        private static object _lockObj = new object();
        public static DataTable RunParallel(List<DateRangeList> _DateRangeList, string UserName, string GroupId, string RuleId, int CType)
        {
            DataTable _dataTable = new DataTable();
            Parallel.ForEach(_DateRangeList, (dateRange) =>
            {
                DataTable dtResultSET = FetchTxnsDB(dateRange.StartDate, dateRange.EndDate, UserName, GroupId, RuleId, CType);
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
}