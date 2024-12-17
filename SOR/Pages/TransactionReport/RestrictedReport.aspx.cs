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
    public partial class RestrictedReport : System.Web.UI.Page
    {
        #region Object Declration
        TransactionReportDAL _TransactionReportDAL = new TransactionReportDAL();
        AggregatorEntity _AggregatorEntity = new AggregatorEntity();
        RuleEntity _RuleEntity = new RuleEntity();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "RestrictedReport.aspx", "37");
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
                            BindAction();
                            UserPermissions.RegisterStartupScriptForNavigationListActive("7", "37");
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
                ErrorLog.TransactionReportTrace("RestrictedReport: Page_Load: Exception: " + Ex.Message);
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
                Setpropertise();
                
                _TransactionReportDAL.flag = Convert.ToString((int)_EnumBindingType);
                _dsTransactionLogs = _TransactionReportDAL.GetRestrictedDetails();
                
                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                {
                    if (_dsTransactionLogs != null && _dsTransactionLogs.Tables.Count > 0 && _dsTransactionLogs.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            DataView dv = _dsTransactionLogs.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";

                            dv.Sort = sortExpression + " " + this.SortDirection;
                            gvRestrictedDetails.DataSource = dv;
                            gvRestrictedDetails.DataBind();
                            gvRestrictedDetails.Visible = true;
                            lblRecordCount.Text = "Total " + Convert.ToString(_dsTransactionLogs.Tables[1].Rows.Count) + " Record(s) Found.";
                        }
                        {
                            gvRestrictedDetails.VirtualItemCount = Convert.ToInt32(_dsTransactionLogs.Tables[1].Rows[0][0]);
                            gvRestrictedDetails.DataSource = _dsTransactionLogs.Tables[0];
                            gvRestrictedDetails.DataBind();
                            gvRestrictedDetails.Visible = true;
                            lblRecordCount.Text = "Total " + Convert.ToString(_dsTransactionLogs.Tables[1].Rows[0][0]) + " Record(s) Found.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideAccordion", "hideAccordion();", true);
                        }
                    }
                    else
                    {
                        gvRestrictedDetails.Visible = false;
                        lblRecordCount.Text = "Total 0 Record(s) Found.";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found In Search Criteria. Try again', 'Warning');", true);
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RestrictedReport: FillGrid: Exception: " + Ex.Message);
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
                _TransactionReportDAL.NamePIN = ddlNamePIN.SelectedValue != "0" ? ddlNamePIN.SelectedValue : null;
                _TransactionReportDAL.PinCode = !string.IsNullOrEmpty(txtPinCode.Value) ? txtPinCode.Value.Trim() : null;
                _TransactionReportDAL.PageIndex = gvRestrictedDetails.PageIndex;
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RestrictedReport: Setpropertise: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion
        
        #region Search Button
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(txtFromDate.Value) > Convert.ToDateTime(txtToDate.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('From date should be less than To date. Try again', 'Warning');", true);
                    return;
                }
                if (ddlNamePIN.SelectedValue=="0")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please select any one option from Name/PIN. Try again', 'Warning');", true);
                    return;
                }
                else
                {
                    FillGrid(EnumCollection.EnumBindingType.BindGrid);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RestrictedReport: btnSearch_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Clear
        protected void btnClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                ddlNamePIN.ClearSelection();
                gvRestrictedDetails.DataSource = null;
                gvRestrictedDetails.DataBind();
                gvRestrictedDetails.PageIndex = 0;
                lblRecordCount.Text = string.Empty;
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RestrictedReport: btnClear_ServerClick: Exception: " + Ex.Message);
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
                ErrorLog.TransactionReportTrace("RestrictedReport: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }
        #endregion

        #region Grid Events
        protected void gvRestrictedDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRestrictedDetails.PageIndex = e.NewPageIndex;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RestrictedReport: gvRestrictedDetails_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Sorting
        protected void gvRestrictedDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                this.FillGrid(EnumCollection.EnumBindingType.BindGrid, e.SortExpression);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("RestrictedReport: gvRestrictedDetails_Sorting: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
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
                ErrorLog.AgentManagementTrace("Page : RestrictedReport.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);
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
                ErrorLog.AgentManagementTrace("Page : RestrictedReport.cs \nFunction : ddlAction_SelectedIndexChanged()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }

        protected void btnexport_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(txtFromDate.Value) > Convert.ToDateTime(txtToDate.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('From date should be less than To date. Try again', 'Warning');", true);
                    return;
                }
                if (ddlNamePIN.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please select any one option from Name/PIN. Try again', 'Warning');", true);
                    return;
                }
                if (ddlAction.SelectedValue == "2")
                {
                    try
                    {
                        ExportFormat _ExportFormat = new ExportFormat();
                        DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                        if (dt != null && dt.Tables[0].Rows.Count > 0)
                        {
                            _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "Restricted Report", dt);
                        }
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("RestrictedReport: btnexport_ServerClick | Excel : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                if (ddlAction.SelectedValue == "3")
                {
                    try
                    {
                        ExportFormat _ExportFormat = new ExportFormat();
                        DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                        if (dt != null && dt.Tables[0].Rows.Count > 0)
                        {
                            _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "Restricted Report", dt);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("RestrictedReport: btnexport_ServerClick | CSV : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                if (ddlAction.SelectedValue == "4")
                {
                    try
                    {
                        int ReportThreads = Convert.ToInt32(ConfigurationManager.AppSettings["ReportThreads"].ToString());
                        int ReportRecordsPerSheet = Convert.ToInt32(ConfigurationManager.AppSettings["ReportRecordsPerSheet"].ToString());
                        string ReportDownPath = ConfigurationManager.AppSettings["ReportDownPath"].ToString();
                        string StartTime = DateTime.Now.ToLongTimeString();
                        ErrorLog.TransactionReportTrace("Page : Restricted Report - Request Received For Export Report. Start-Time : " + StartTime);
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
                                string zipName = String.Format("RestrictedReport_{0}.zip", DateTime.Now.ToString("yyyyMMMdd_HHmmss"));
                                Response.ContentType = "application/zip";
                                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                                zip.Save(Response.OutputStream);
                                Response.End();
                            }
                            ErrorLog.TransactionReportTrace("Page : RestrictedReport - Request Received For Export Report. Start-Time : " + StartTime + " End-Time : " + DateTime.Now.ToLongTimeString() + Environment.NewLine);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found', 'Warning');", true);
                            return;
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("RestrictedReport: btnexport_ServerClick | ZIP : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                else
                {
                    ErrorLog.AgentManagementTrace("Page : RestrictedReport.cs \nFunction : ddlAction_SelectedIndexChanged() | Please select action type for export.");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please select action type for export', 'AEPS Transactions');", true);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : RestrictedReport.cs \nFunction : ddlAction_SelectedIndexChanged()\nException Occured\n" + ex.Message);
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
                ErrorLog.TransactionReportTrace("Page : RestrictedReport - Request Received For Split Data Table. Per Sheet Record Range : " + max + " Total Records : " + tableData.Rows.Count + " Start-Time : " + StartTime + " End-Time : " + DateTime.Now.ToLongTimeString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportError(Ex);
            }
            return newDs;
        }

        private List<DateRangeList> DateRangeHandlerDynamic(string FromDate, string ToDate, int TaskCount)
        {
            ErrorLog.TransactionReportTrace("Page : RestrictedReport - Request Received For Date Range Handler. FromDate : " + FromDate + " ToDate : " + ToDate);
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
                DataSet dsTransactionLogs = _TransactionReportDAL.GetBCTransactionReport();
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

        protected void gvRestrictedDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Center-align all data cells
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("Page : RestrictedReport.cs \nFunction : gvRestrictedDetails_RowDataBound()\nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Wise Transactions');", true);
                return;
            }
        }
    }
}