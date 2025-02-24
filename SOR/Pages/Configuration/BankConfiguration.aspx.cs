﻿using AppLogger;
using BussinessAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOR.Pages.Configuration
{
    public partial class BankConfiguration : System.Web.UI.Page
    {
        RuleEntity _ruleEntity = new RuleEntity();
        CommonEntity _CommonEntity = new CommonEntity();
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.ConfigurationTrace("BankConfiguration | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "BankConfiguration.aspx", "9");
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
                            txtFrom.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtTo.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            UserPermissions.RegisterStartupScriptForNavigationListActive("9", "28");
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
                ErrorLog.ConfigurationTrace("BankConfiguration : Page_Load(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.ConfigurationTrace("BankConfiguration | btnSearch_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Configuration-BankConfiguration";
                _auditParams[2] = "btnSearch";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                ErrorLog.ConfigurationTrace("BankConfiguration | btnSearch_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.ConfigurationTrace("BankConfiguration: btnSearch_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }

        }
        protected void btnClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.ConfigurationTrace("BankConfiguration | btnClear_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Configuration-BankConfiguration";
                _auditParams[2] = "btnClear";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                ErrorLog.ConfigurationTrace("BankConfiguration | btnClear_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.ConfigurationTrace("BankConfiguration: btnClear_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        public DataTable FillGrid(EnumCollection.EnumBindingType _EnumPermissionType)
        {
            DataTable dt = new DataTable();
            try
            {
                ErrorLog.ConfigurationTrace("BankConfiguration | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                gvBankConfiguration.DataSource = null;
                gvBankConfiguration.DataBind();
                _ruleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _ruleEntity.Fromdate = !string.IsNullOrEmpty(txtFrom.Value) ? Convert.ToDateTime(txtFrom.Value).ToString("yyyy-MM-dd") : null;
                _ruleEntity.Todate = !string.IsNullOrEmpty(txtTo.Value) ? Convert.ToDateTime(txtTo.Value).ToString("yyyy-MM-dd") : null;
                _ruleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                dt = _ruleEntity.GetBankConfiguration();
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
                ErrorLog.ConfigurationTrace("BankConfiguration | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.ConfigurationTrace("BankConfiguration: FillGrid(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return dt;
        }
        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.ConfigurationTrace("BankConfiguration | btnAdd_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Configuration-BankConfiguration";
                _auditParams[2] = "btnAdd";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                formone.Visible = false;
                formTwo.Visible = true;
                Session["id"] = null;
                ErrorLog.ConfigurationTrace("BankConfiguration | btnAdd_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.ConfigurationTrace("BankConfiguration: btnAdd_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        protected void btnSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.ConfigurationTrace("BankConfiguration | btnSubmit_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                _ruleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _ruleEntity.Fromdate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                _ruleEntity.Todate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                _ruleEntity.AcquirerId = !string.IsNullOrEmpty(txtAcquirerId.Value) ? txtAcquirerId.Value.Trim() : null;
                _ruleEntity.OrgId = !string.IsNullOrEmpty(txtOrgId.Value) ? txtOrgId.Value.Trim() : null;
                _ruleEntity.Bin = !string.IsNullOrEmpty(txtBin.Value) ? txtBin.Value.Trim() : null;
                _ruleEntity.AuaCode = !string.IsNullOrEmpty(txtAuaCode.Value) ? txtAuaCode.Value.Trim() : null;
                _ruleEntity.SubCode = !string.IsNullOrEmpty(txtSubCode.Value) ? txtSubCode.Value.Trim() : null;
                _ruleEntity.LicenceKey = !string.IsNullOrEmpty(txtLicenceKey.Value) ? txtLicenceKey.Value.Trim() : null;
                _ruleEntity.BankCode = !string.IsNullOrEmpty(txtBankCode.Value) ? txtBankCode.Value.Trim() : null;

                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Configuration-BankConfiguration";
                _auditParams[2] = "btnSubmit";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion

                if (Session["id"] != null)
                {
                    _ruleEntity.Flag = (int)EnumCollection.EnumRuleType.Insert;
                    _ruleEntity.ReqId = !string.IsNullOrEmpty(Session["id"].ToString()) ? Session["id"].ToString().Trim() : null;
                    string statusCode = _ruleEntity.InsertOrUpdateBankCOnfiguration();
                    formTwo.Visible = false;
                    formone.Visible = true;
                    if (statusCode == "INS00")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request going for verification', 'Warning');", true);
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Unable to update request', 'Warning');", true);
                        return;
                    }
                }
                else
                {
                    _ruleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                    string statusCode = _ruleEntity.InsertOrUpdateBankCOnfiguration();
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
                    formTwo.Visible = false;
                    formone.Visible = true;
                    ErrorLog.ConfigurationTrace("BankConfiguration: btnSubmit_ServerClick() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                }
                ErrorLog.ConfigurationTrace("BankConfiguration | btnSubmit_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.ConfigurationTrace("BankConfiguration: btnSubmit_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnReset_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.ConfigurationTrace("BankConfiguration | btnReset_ServerClick() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());

                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Configuration-BankConfiguration";
                _auditParams[2] = "btnReset";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion

                formTwo.Visible = false;
                formone.Visible = true;
                ErrorLog.ConfigurationTrace("BankConfiguration | btnReset_ServerClick() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.ConfigurationTrace("BankConfiguration: btnReset_ServerClick(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
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
                ErrorLog.ConfigurationTrace("BankConfiguration: gvBankConfiguration_PageIndexChanging(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.ConfigurationTrace("BankConfiguration | BtnCsv_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataTable dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Configuration-BankConfiguration";
                    _auditParams[2] = "Export-To-CSV";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "BankConfiguration", ds);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfiguration: BtnCsv_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.ConfigurationTrace("BankConfiguration | BtnXls_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataTable dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Configuration-BankConfiguration";
                    _auditParams[2] = "Export-To-Excel";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "BankConfiguration", ds);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfiguration: BtnXls_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("BankConfiguration: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }
        protected void btnView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;
                string commandArgs = btn.CommandArgument;

                // Split the command arguments if needed
                string[] args = commandArgs.Split('=');
                string id = args[0];
                Session["id"] = id;
                //txtFromDate.Value = args[1];
                txtFromDate.Value = DateTime.ParseExact(args[1], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.ParseExact(args[2], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                txtAcquirerId.Value = args[3];
                txtOrgId.Value = args[4];
                txtBin.Value = args[5];
                txtAuaCode.Value = args[6];
                txtSubCode.Value = args[7];
                txtLicenceKey.Value = args[8];
                txtBankCode.Value = args[9];
                formTwo.Visible = true;
                formone.Visible = false;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("BankConfiguration: BtnXls_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
    }
}