using AppLogger;
using BussinessAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOR.Pages.Rule
{

    public partial class BatchConfiguration : System.Web.UI.Page
    {
        RuleEntity _ruleEntity = new RuleEntity();
        CommonEntity _CommonEntity = new CommonEntity();
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("BatchConfiguration | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "BatchConfiguration.aspx", "30");
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
                            //txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            //txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtFrom.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtTo.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            UserPermissions.RegisterStartupScriptForNavigationListActive("3", "30");
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
                ErrorLog.DashboardTrace("BatchConfiguration : Page_Load(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("BatchConfiguration | btnSearch_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "BatchConfiguration";
                _auditParams[2] = "btnSearch";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                ErrorLog.RuleTrace("BatchConfiguration | btnSearch_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.LimitTrace("BatchConfiguration: btnSearch_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }

        }
        protected void btnClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("BatchConfiguration | btnClear_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "BatchConfiguration";
                _auditParams[2] = "btnClear";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                gvBankConfiguration.DataSource = null;
                gvBankConfiguration.DataBind();
                ErrorLog.RuleTrace("BatchConfiguration | btnClear_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                //ddlBCCode.ClearSelection();
                //ddlClientCode.ClearSelection();
                //gvLimitStatus.Visible = false;
            }
            catch (Exception Ex)
            {
                ErrorLog.LimitTrace("BatchConfiguration: btnClear_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        public DataTable FillGrid(EnumCollection.EnumBindingType _EnumPermissionType)
        {
            DataTable dt = new DataTable();
            try
            {
                ErrorLog.RuleTrace("BatchConfiguration | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                gvBankConfiguration.DataSource = null;
                gvBankConfiguration.DataBind();
                _ruleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _ruleEntity.Fromdate = !string.IsNullOrEmpty(txtFrom.Value) ? Convert.ToDateTime(txtFrom.Value).ToString("yyyy-MM-dd") : null;
                _ruleEntity.Todate = !string.IsNullOrEmpty(txtTo.Value) ? Convert.ToDateTime(txtTo.Value).ToString("yyyy-MM-dd") : null;
                _ruleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                dt = _ruleEntity.GetBatchConfiguration();
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvBankConfiguration.Visible = true;
                    panel1.Visible = true;
                    gvBankConfiguration.DataSource = dt;
                    gvBankConfiguration.DataBind();
                }
                else
                {
                    gvBankConfiguration.DataSource = null;
                    gvBankConfiguration.DataBind();
                }
                ErrorLog.RuleTrace("BatchConfiguration | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.LimitTrace("BatchConfiguration: FillGrid(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return dt;
        }
        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("BatchConfiguration | btnAdd_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "BatchConfiguration";
                _auditParams[2] = "btnAdd";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                BindSwitch();
                formone.Visible = false;
                formTwo.Visible = true;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "AddBatchRows", "addRow();", true);
                ErrorLog.RuleTrace("BatchConfiguration | btnAdd_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.LimitTrace("BatchConfiguration: btnAdd_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        protected void btnSubmit_ServerClick(object sender, EventArgs e)
        { 
            try
            {
                ErrorLog.RuleTrace("BatchConfiguration | btnSubmit_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                
                if ((ddlSwitch.SelectedValue == "0") && string.IsNullOrEmpty(ddlSwitch.SelectedValue))
                {
                    ShowWarning("Please select switch. Try again", "Warning");
                    return;
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("Name");
                dt.Columns.Add("Percentage");
                dt.Columns.Add("Count");

                const int maxRows = 100; // Adjust this to the maximum number of rows you expect

                for (int rowIndex = 0; rowIndex < maxRows; rowIndex++)
                {
                    string name = Request.Form[$"name{rowIndex}"];
                    string percentage = Request.Form[$"percentage{rowIndex}"];
                    string count = Request.Form[$"count{rowIndex}"];

                    // Check if at least one field is not null or empty
                    if (!string.IsNullOrWhiteSpace(name) ||
                        !string.IsNullOrWhiteSpace(percentage) ||
                        !string.IsNullOrWhiteSpace(count))
                    {
                        // Create a new DataRow
                        DataRow row = dt.NewRow();
                        row["Name"] = name;
                        row["Percentage"] = percentage; // Can be null or empty
                        row["Count"] = count; // Can be null or empty
                        dt.Rows.Add(row);
                    }
                }
                if (dt.Rows.Count == 0)
                {
                    ShowWarning("Please select batch details. Try again", "Warning");
                    return;
                }
                _ruleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _ruleEntity.SwitchId = Convert.ToInt32(ddlSwitch.SelectedValue) != 0 ? Convert.ToInt32(ddlSwitch.SelectedValue) : 0;
                _ruleEntity.dt = dt;
                _ruleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "BatchConfiguration";
                _auditParams[2] = "btnSubmit";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                string statusCode = _ruleEntity.InsertOrUpdateBatch();
                if (statusCode == "INS00")
                {
                    _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                    _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                }
                else
                {
                    _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                    _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                }
                var response = new
                {
                    StatusMessage = _CommonEntity.ResponseMessage
                };
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                ErrorLog.RuleTrace("BatchConfiguration | btnSubmit_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ErrorLog.RuleTrace("BatchConfiguration: btnSubmit_ServerClick() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("BatchConfiguration: btnSubmit_ServerClick() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
            }
        }

        protected void btnReset_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("BatchConfiguration | btnReset_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "BatchConfiguration";
                _auditParams[2] = "btnReset";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                formTwo.Visible = false;
                formone.Visible = true;
                ErrorLog.RuleTrace("BatchConfiguration | btnReset_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("BatchConfiguration: btnReset_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void gvBankConfiguration_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBankConfiguration.PageIndex = e.NewPageIndex;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.LimitTrace("BatchConfiguration: gvBankConfiguration_PageIndexChanging(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("BatchConfiguration | btnReset_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataTable dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                    _auditParams[1] = "BatchConfiguration";
                    _auditParams[2] = "Export-To-CSV";
                    _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "BatchConfiguration", ds);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BatchConfiguration: BtnCsv_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("BatchConfiguration | btnReset_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataTable dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                    _auditParams[1] = "BatchConfiguration";
                    _auditParams[2] = "Export-To-Excel";
                    _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "BatchConfiguration", ds);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BatchConfiguration: BtnXls_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BatchConfiguration: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }
        public void BindSwitch()
        {
            try
            {
                DataTable ds = _ruleEntity.BindSwitch();
                ddlSwitch.Items.Clear();

                if (ds != null && ds.Rows.Count > 0 && ds.Rows.Count > 0)
                {
                    if (ds.Rows.Count == 1)
                    {
                        ddlSwitch.Items.Clear();
                        ddlSwitch.DataSource = ds.Copy();
                        ddlSwitch.DataTextField = "switchname";
                        ddlSwitch.DataValueField = "id";
                        ddlSwitch.DataBind();
                        ddlSwitch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlSwitch.Items.Clear();
                        ddlSwitch.DataSource = ds.Copy();
                        ddlSwitch.DataTextField = "switchname";
                        ddlSwitch.DataValueField = "id";
                        ddlSwitch.DataBind();
                        ddlSwitch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlSwitch.DataSource = null;
                    ddlSwitch.DataBind();
                    ddlSwitch.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void ShowWarning(string message, string title)
        {
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Page),
                "ShowWarning",
                $"showWarning('{message}', '{title}');",
                true
            );
        }
    }
}