using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using BussinessAccessLayer;
using AppLogger;
using System.Threading;
using System;

namespace SOR.Pages.Agent
{
    public partial class AgStatus : System.Web.UI.Page
    {
        #region Objects Declaration
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();
        EmailAlerts _EmailAlerts = new EmailAlerts();
        ClientRegistrationEntity clientMngnt = new ClientRegistrationEntity();
        public bool HasPagePermission { get; private set; }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentStatus | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    //bool HasPagePermission = true;
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "AgStatus", "21");
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
                            fillGrid();
                            UserPermissions.RegisterStartupScriptForNavigationListActive("6", "21");
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
                ErrorLog.AgentManagementTrace("AgentStatus: Page_Load: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #region FillGrid
        public DataSet fillGrid()
        {
            DataSet _dsAllAgents = new DataSet();
            try
            {
                ErrorLog.AgentManagementTrace("AgentStatus | fillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
                _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
                _AgentRegistrationDAL.OperationType = ddlOperationType.SelectedValue != "0" ? (ddlOperationType.SelectedValue) : null;
                _AgentRegistrationDAL.Fromdate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                _AgentRegistrationDAL.Todate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                _dsAllAgents = _AgentRegistrationDAL.AgentStatusReportGrid();
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
                ErrorLog.AgentManagementTrace("AgentStatus | fillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentStatus: fillGrid: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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
                    _AgentRegistrationDAL.BCstatus = Convert.ToString((int)EnumCollection.Onboarding.Approve);

                    fillGrid();
                }

                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "2")
                {
                    _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerApprove);
                    fillGrid();
                }

                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "3")
                {
                    _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "4")
                {
                    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "1")
                {
                    _AgentRegistrationDAL.BCstatus = Convert.ToString((int)EnumCollection.Onboarding.Decline);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "2")
                {
                    _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerDecline);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "3")
                {
                    _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerDecline);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "4")
                {
                    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerDecline);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "1")
                {
                    _AgentRegistrationDAL.BCstatus = Convert.ToString((int)EnumCollection.Onboarding.Pending);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "2")
                {
                    _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerPending);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "3")
                {
                    _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "4")
                {
                    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                    fillGrid();
                }

                else
                {
                    gvTransactions.DataSource = null;
                    gvTransactions.DataBind();

                }


                _AgentRegistrationDAL.ClientId = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;


                // _AgentEntity.AgentID = ddlAgent.SelectedValue != "All" ? ddlAgent.SelectedValue : null;


                _AgentRegistrationDAL.Status = ddlStatusType.SelectedValue != "All" ? Convert.ToInt32(ddlStatusType.SelectedValue.ToString()) : 0;
                _AgentRegistrationDAL.UserName = Session["Username"].ToString();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentStatus: setProperties: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("AgentStatus: gvTransactions_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        #endregion
        
        #region Search
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentStatus | btnSearch_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Agent-AgentStatus";
                _auditParams[2] = "btnSearch";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "1")
                {
                    _AgentRegistrationDAL.BCstatus = Convert.ToString((int)EnumCollection.Onboarding.Approve);
                    fillGrid();
                }

                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "2")
                {
                    _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerApprove);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "3")
                {
                    _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "4")
                {
                    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "1")
                {
                    _AgentRegistrationDAL.BCstatus = Convert.ToString((int)EnumCollection.Onboarding.Decline);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "2")
                {
                    _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerDecline);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "3")
                {
                    _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerDecline);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "4")
                {
                    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerDecline);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "1")
                {
                    _AgentRegistrationDAL.BCstatus = Convert.ToString((int)EnumCollection.Onboarding.Pending);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "2")
                {
                    _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerPending);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "3")
                {
                    _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "4")
                {
                    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                    fillGrid();
                }

                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "0")
                {
                    _AgentRegistrationDAL.BCstatus = Convert.ToString((int)EnumCollection.Onboarding.Approve);
                    fillGrid();
                }

                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "1")
                {
                    _AgentRegistrationDAL.Mstatus = Convert.ToString((int)EnumCollection.Onboarding.MakerApprove);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "2")
                {
                    _AgentRegistrationDAL.ChStatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "3")
                {
                    _AgentRegistrationDAL.AtStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "0" && ddlVerification.SelectedValue == "0")
                {
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "1" && ddlVerification.SelectedValue == "0")
                {
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "2" && ddlVerification.SelectedValue == "0")
                {
                    fillGrid();
                }
                else if (ddlOperationType.SelectedValue == "3" && ddlVerification.SelectedValue == "0")
                {
                    fillGrid();
                }
                else
                {
                    gvTransactions.DataSource = null;
                    gvTransactions.DataBind();
                    lblRecordCount.Text = "Total 0 Record(s) Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select Operation Type. Try again', 'Warning');", true);
                }
                ErrorLog.AgentManagementTrace("AgentStatus | btnSearch_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentStatus: btnSearch_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Reset Button
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentStatus | btnReset_Click() | Start. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ddlClient.SelectedValue = "0";
                ddlStatusType.SelectedValue = "0";
                ddlAgent.SelectedValue = "0";
                ddlOperationType.SelectedValue = "0";
                ddlVerification.SelectedValue = "0";
                ddlStatus.SelectedValue = "0";
                ddlBCType.SelectedValue = "0";
                txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
                ErrorLog.AgentManagementTrace("AgentStatus | btnReset_Click() | End. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentStatus: btnReset_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
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
                ErrorLog.AgentManagementTrace("AgentStatus: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }
        #endregion

        #region Clear

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentStatus | btnCancel_Click() | Start. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                DIVFilter.Visible = true;
                DIVRegister.Visible = false;
                ErrorLog.AgentManagementTrace("AgentStatus | btnCancel_Click() | End. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentStatus: btnCancel_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        protected void clearselection()
        {
            try
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
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentStatus: clearselection: Exception: " + Ex.Message);
            }
        }
        #endregion
        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentStatus | BtnCsv_Click() | Start. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                setProperties();
                DataSet _dsAllAgents = _AgentRegistrationDAL.AgentStatusReportGrid();
                if (_dsAllAgents != null && _dsAllAgents.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-AgentStatus";
                    _auditParams[2] = "Export-To-CSV";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "Agent_Status", _dsAllAgents);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentStatus: BtnCsv_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AgentManagementTrace("AgentStatus | BtnXls_Click() | Start. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                setProperties();
                DataSet _dsAllAgents = _AgentRegistrationDAL.AgentStatusReportGrid();
                if (_dsAllAgents != null && _dsAllAgents.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Agent-AgentStatus";
                    _auditParams[2] = "Export-To-Excel";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "Agent_Status", _dsAllAgents);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentStatus: BtnXls_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
    }
}