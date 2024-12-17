using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessAccessLayer;
using AppLogger;
using System;
using System.Configuration;
using System.Web;
using System.Threading;

namespace SOR.Pages.BC
{
    public partial class BCStatus : System.Web.UI.Page
    {

        #region Objects Declaration

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
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {

                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "BCStatus.aspx", "13");
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
                        UserPermissions.RegisterStartupScriptForNavigationListActive("4", "13");
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
                ErrorLog.BCManagementTrace("Page : BCStatus.cs \nFunction : Page_Load() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'BCStatus');", true);
                return;
            }
        }

        #region FillGrid
        public DataSet fillGrid()
        {
            DataSet _dsAllAgents = new DataSet();
            try
            {
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
                _BCEntity.Flag = (int)EnumCollection.EnumBindingType.BindGrid;
                ///// setProperties();
                _dsAllAgents = _BCEntity.BCStatusReportGrid();
                ViewState["Data"] = _dsAllAgents;

                if (_dsAllAgents.Tables.Count > 0 && _dsAllAgents.Tables[0].Rows.Count > 0)
                {
                    gvTransactions.DataSource = _dsAllAgents.Tables[0];
                    gvTransactions.DataBind();
                    gvTransactions.Visible = true;
                    panelGrid.Visible = true;
                    ddlExport.Visible = true;
                    btnExport.Visible = true;
                    lblRecordCount.Visible = true;
                    lblRecordCount.Text = "Total " + Convert.ToString(_dsAllAgents.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    gvTransactions.Visible = false;
                    ddlExport.Visible = false;
                    btnExport.Visible = false;
                    panelGrid.Visible = false;
                    lblRecordCount.Text = "Total 0 Record(s) Found.";

                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BC Status.cs \nFunction : fillGrid() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'BC Status');", true);
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
                ErrorLog.BCManagementTrace("Page : BC Status.cs \nFunction : setProperties() \nException Occured\n" + EX.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'BC Status');", true);
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
                ErrorLog.BCManagementTrace("Page : BCStatus.cs \nFunction : gvTransactions_PageIndexChanging() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'BC Status');", true);
                return;
            }
        }

        #endregion

        #region Search
        protected void btnSearch_Click(object sender, EventArgs e)
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
                    fillGrid();
                    //gvTransactions.DataSource = null;
                    //gvTransactions.DataBind();

                }

            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCStatus.cs \nFunction : btnSearch_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','BC Status');", true);
                return;
            }
        }
        #endregion

        #region Reset Button
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ddlClient.SelectedValue = "0";
                ddlStatusType.SelectedValue = "0";
                ddlAgent.SelectedValue = "0";
                ddlOperationType.SelectedValue = "0";
                ddlVerification.SelectedValue = "0";
                ddlStatus.SelectedValue = "0";
                ddlBCType.SelectedValue = "0";
                fillGrid();
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCStatus.cs \nFunction : btnReset_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','BC Status');", true);
                return;
            }
        }
        #endregion


        #region CSV
        protected void btnExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                setProperties();
                DataSet _dsAllAgents = _BCEntity.BCStatusReportGrid();
                if (_dsAllAgents != null)
                {
                    if (_dsAllAgents.Tables.Count > 0)
                    {
                        exportFormat.ExportInCSV(Session["Username"].ToString(), "Payrakam", "BC Status Details", _dsAllAgents);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "showSuccess('No data found.', 'BC Status Details');", true);
                }
            }
            catch (Exception EX)
            {
                ErrorLog.BCManagementTrace("Page : BCStatus.cs \nFunction : btnExportCSV_Click() \nException Occured\n" + EX.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','BC Status');", true);
                return;
            }
        }
        #endregion

        #region Excel
        protected void btnExportXLS_Click(object sender, EventArgs e)
        {
            try
            {
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                setProperties();
                DataSet _dsAllAgents = _BCEntity.BCStatusReportGrid();
                if (_dsAllAgents != null)
                {
                    if (_dsAllAgents.Tables.Count > 0)
                    {
                        exportFormat.ExporttoExcel(Session["Username"].ToString(), "Payrakam", "BC Status Details", _dsAllAgents);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "showSuccess('No data found.', 'BC Status Details');", true);
                }
            }
            catch (Exception EX)
            {
                ErrorLog.BCManagementTrace("Page : BCStatus.cs \nFunction : btnExportXLS_Click() \nException Occured\n" + EX.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','BC Status');", true);
                return;
            }
        }
        #endregion

        #region Export
        protected void btnexport_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = fillGrid();
                if (ddlExport.SelectedItem.Text == "Excel")
                {
                    if (dt != null && dt.Tables[0].Rows.Count > 0)
                    {
                        _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "Active Business Correspondents Details", dt);
                    }
                    {
                        //lblRecordCount.Text = "No Record's Found.";
                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);

                    }
                }
                else if (ddlExport.SelectedItem.Text == "CSV")
                {
                    if (dt != null && dt.Tables[0].Rows.Count > 0)
                    {
                        _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "Active Business Correspondents Details", dt);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);
                    }
                }
                //else if (ddlExport.SelectedItem.Text == "ZIP")
                //{
                //    if (dt != null && dt.Rows.Count > 0)
                //    {
                //        DataTable dt1 = dt.Copy();
                //        //_ExportFormat.ExportZIP(dt1, "PNB", "ATM Master", DateTime.Now.ToShortDateString());
                //    }
                //    else
                //    { ScriptManager.RegisterStartupScript(this, typeof(Page), "Script", "alert('No data found.', 'Warning');", true); }
                //}
                //else
                //{
                //}
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BCStatus.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);
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
                ErrorLog.BCManagementTrace("Page : BCStatus.cs \nFunction : SetPageFiltersExport\nException Occured\n" + Ex.Message);
            }
            return pageFilters;
        }

        private void BindExport()
        {
            try
            {
                DataSet ds = _BCEntity.BindExport();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        ddlExport.DataTextField = "strDetailName";
                        ddlExport.DataValueField = "numMasterDetailID";
                        ddlExport.DataSource = ds;
                        ddlExport.DataBind();
                    }
                    else
                    {
                        ddlExport.DataTextField = "strDetailName";
                        ddlExport.DataValueField = "numMasterDetailID";
                        ddlExport.DataSource = ds;
                        ddlExport.DataBind();
                    }
                }
                else
                {
                    ddlExport.DataSource = null;
                    ddlExport.DataBind();
                    ddlExport.Items.Insert(0, new ListItem("-- Franchise --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCStatus.cs \nFunction : BindExport() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Verified Agent');", true);
                return;
            }
        }

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                ExportFormat _ExportFormat = new ExportFormat();
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                setProperties();
                DataSet _dsAllAgents = _BCEntity.BCStatusReportGrid();
                if (_dsAllAgents != null && _dsAllAgents.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "BC Status Report", _dsAllAgents);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);
                }

            }

            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BCStatus.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                //_BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                //setProperties();
                //DataSet _dsAllAgents = _BCEntity.BCStatusReportGrid();
                DataSet _dsAllAgents = ViewState["Data"] as DataSet;
                if (_dsAllAgents != null && _dsAllAgents.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "BC Status Report", _dsAllAgents);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);
                }


            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BCStatus.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);
            }
        }
        #endregion

        #region Clear

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            DIVFilter.Visible = true;
            //DIVDocument.Visible = true;
            DIVRegister.Visible = false;
        }
        protected void clearselection()
        {
            ddlCategory.SelectedValue = "0";
            ddlState.SelectedValue = "0";
            ddlCity.SelectedValue = "0";
            ddlClient.SelectedValue = "0";
            ddlCountry.SelectedValue = "0";
            ddlExport.SelectedValue = "0";
            ddlAgent.SelectedValue = "0";
            ddlGender.SelectedValue = "0";
            DDlOrg.SelectedValue = "0";
            ddlStatusType.SelectedValue = "0";
        }
        #endregion




    }
}