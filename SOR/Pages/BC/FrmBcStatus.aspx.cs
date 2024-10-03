using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using BussinessAccessLayer;
using AppLogger;
using System.Threading;
using System;
using System.Net;

namespace SOR.Pages.BC
{
    public partial class FrmBcStatus : System.Web.UI.Page
    {

        #region Objects Declaration

        DataSet _dsAllAgents = null;
        public string UserName { get; set; }
        BCEntity _BCEntity = new BCEntity();
        public clsCustomeRegularExpressions customeRegExpValidation = null;
        public string pathId, PathAdd, PathSig;

        public clsCustomeRegularExpressions _CustomeRegExpValidation
        {
            get { if (customeRegExpValidation == null) customeRegExpValidation = new clsCustomeRegularExpressions(); return customeRegExpValidation; }
            set { customeRegExpValidation = value; }
        }
        
        AppSecurity appSecurity = null;
        public AppSecurity _AppSecurity
        {
            get { if (appSecurity == null) appSecurity = new AppSecurity(); return appSecurity; }
            set { appSecurity = value; }
        }
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

                    bool HasPagePermission = true;
                    // UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "BCVerificationLevel1.aspx", "61");
                    //if (!HasPagePermission)
                    //{
                    //    try
                    //    {
                    //        Response.Redirect(ConfigurationManager.AppSettings["RedirectTo404"].ToString(), false);
                    //        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //    }
                    //    catch (ThreadAbortException)
                    //    {
                    //    }
                    //}
                    //else
                    //{
                    //  UserPermissions.RegisterStartupScriptForNavigationListActive("18", "61");
                    if (!IsPostBack) //HasPagePermission
                    {
                        Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                        //BindDropdownState();
                        BindDropdownClients();
                        //BindFranchise();
                        fillGrid();
                        UserPermissions.RegisterStartupScriptForNavigationListActive("25", "162");
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
                ErrorLog.CommonTrace("Page : BCVerificationLevel1.cs \nFunction : Page_Load() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Contact System Administrator', 'Agents Verification');</script>", false);
                return;
            }
        }

        #region BindDropdownClients
        public void BindDropdownClients()
        {
            try
            {
                _BCEntity.IsRemoved = "0";
                _BCEntity.IsActive = "1";
                _BCEntity.VerificationStatus = "1";
                _BCEntity.UserName = Session["Username"].ToString();
                _BCEntity.BCID = Convert.ToString(Session["UserRoleID"]) == "3" ? Convert.ToString(Session["Username"]) : null;
                DataSet ds = _BCEntity.BindClient();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlClient.DataSource = ds.Tables[0];
                    ddlClient.DataValueField = "ClientID";
                    ddlClient.DataTextField = "ClientName";
                    ddlClient.DataBind();
                    ddlClient.Items.Insert(0, new ListItem("-- Client --", "0"));
                }
                else
                {
                    ddlClient.Items.Clear();
                    ddlClient.DataSource = null;
                    ddlClient.DataBind();
                    ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));

                    ddlFranchise.Items.Clear();
                    ddlFranchise.DataSource = null;
                    ddlFranchise.DataBind();
                    ddlFranchise.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Franchise --", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : BindDropdownClients()\nException Occured\n" + ex.Message);
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "BindDropdownClients()", ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
                return;
            }
        }
        #endregion

        #region FillGrid
        public DataSet fillGrid()
        {
            DataSet _dsAllAgents = new DataSet();
            try
            {
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                setProperties();
                _dsAllAgents = _BCEntity.BCStatusReport();

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
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "fillGrid()", Ex);
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : fillGrid() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
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
                if (ddlStatusType.SelectedValue == "1")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)(EnumCollection.Onboarding.CheckerApprove));
                }
                else if (ddlStatusType.SelectedValue == "2")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)(EnumCollection.Onboarding.CheckerDecline));

                }
                else if (ddlStatusType.SelectedValue == "3")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)(EnumCollection.Onboarding.MakerApprove));
                }
                else if (ddlStatusType.SelectedValue == "4")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)(EnumCollection.Onboarding.MakerDecline));
                }
                else if (ddlStatusType.SelectedValue == "5")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)(EnumCollection.Onboarding.Decline));
                }

                _BCEntity.Clientcode = ddlClient.SelectedValue != "0" ? ddlClient.SelectedValue : null;
                _BCEntity.BCID = ddlFranchise.SelectedValue != "0" ? ddlFranchise.SelectedValue : null; ;
                _BCEntity.Status = ddlStatusType.SelectedValue != "All" ? Convert.ToInt32(ddlStatusType.SelectedValue.ToString()) : 0;
                _BCEntity.UserName = Session["Username"].ToString();
            }
            catch (Exception EX)
            {
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "setProperties()", EX);
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : setProperties() \nException Occured\n" + EX.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
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
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "gvTransactions_PageIndexChanging()", Ex);
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : gvTransactions_PageIndexChanging() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
                return;
            }
        }

        #endregion

        #region BindFranchise
        public void BindFranchise()
        {
            try
            {
                _BCEntity.Clientcode = ddlClient.SelectedValue.ToString() == "0" ? null : ddlClient.SelectedValue.ToString();
                _BCEntity.IsActive = "1";
                _BCEntity.VerificationStatus = "1";
                _BCEntity.IsRemoved = "0";
                _BCEntity.UserName = Session["Username"].ToString();
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                DataSet ds_Country = _BCEntity.BindStatusBC();
                if (ds_Country.Tables.Count > 0 && ds_Country.Tables[0].Rows.Count > 0)
                {
                    ddlFranchise.DataSource = ds_Country.Tables[0];
                    ddlFranchise.DataValueField = "FranchiseID";
                    ddlFranchise.DataTextField = "FranchiseName";
                    ddlFranchise.DataBind();
                    ddlFranchise.Items.Insert(0, new ListItem("-- Franchise --", "0"));
                }
                else
                {
                    ddlFranchise.Items.Clear();
                    ddlFranchise.DataSource = null;
                    ddlFranchise.DataBind();
                    ddlFranchise.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Franchise --", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : BindFranchise()\nException Occured\n" + ex.Message);
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "BindDropdownClients()", ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
                return;
            }
        }
        #endregion

        #region Dropdown Events
        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _BCEntity.Clientcode = ddlClient.SelectedValue.ToString() == "0" ? null : ddlClient.SelectedValue.ToString();
                _BCEntity.IsActive = "1";
                _BCEntity.VerificationStatus = "1";
                _BCEntity.IsRemoved = "0";
                _BCEntity.UserName = Session["Username"].ToString();
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                DataSet ds_Country = _BCEntity.BindStatusBC();
                if (ds_Country.Tables.Count > 0 && ds_Country.Tables[0].Rows.Count > 0)
                {
                    ddlFranchise.DataSource = ds_Country.Tables[0];
                    ddlFranchise.DataValueField = "FranchiseID";
                    ddlFranchise.DataTextField = "FranchiseName";
                    ddlFranchise.DataBind();
                    ddlFranchise.Items.Insert(0, new ListItem("-- Franchise --", "0"));
                }
                else
                {
                    ddlFranchise.Items.Clear();
                    ddlFranchise.DataSource = null;
                    ddlFranchise.DataBind();
                    ddlFranchise.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Franchise --", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : ddlClient_SelectedIndexChanged() \nException Occured\n" + ex.Message);
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "ddlClient_SelectedIndexChanged()", ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
            }
        }
        #endregion

        #region Search
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                fillGrid();
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : FrmBcStatus.cs \nFunction : btnSearch_Click() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "FrmBcStatus.cs", "btnSearch_Click()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "<script>showError('Something went wrong.Please try again','Franchise Status');</script>", false);
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
                ddlFranchise.SelectedValue = "0";
                fillGrid();
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : FrmBcStatus.cs \nFunction : btnReset_Click() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "FrmBcStatus.cs", "btnReset_Click()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "<script>showError('Something went wrong.Please try again','Franchise Status');</script>", false);
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
                DataSet _dsAllAgents = _BCEntity.BCStatusReport();
                if (_dsAllAgents != null)
                {
                    if (_dsAllAgents.Tables.Count > 0)
                    {
                        exportFormat.ExportInCSV(Session["Username"].ToString(), "Payrakam", "Franchise Status Details", _dsAllAgents);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "showSuccess('No data found.', 'Franchise Status Details');", true);
                }
            }
            catch (Exception EX)
            {
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "btnExportCSV_Click()", EX);
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : btnExportCSV_Click() \nException Occured\n" + EX.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "<script>showError('Something went wrong.Please try again','Franchise Status');</script>", false);
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
                DataSet _dsAllAgents = _BCEntity.BCStatusReport();
                if (_dsAllAgents != null)
                {
                    if (_dsAllAgents.Tables.Count > 0)
                    {
                        exportFormat.ExporttoExcel(Session["Username"].ToString(), "Payrakam", "Franchise Status Details", _dsAllAgents);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "showSuccess('No data found.', 'Franchise Status Details');", true);
                }
            }
            catch (Exception EX)
            {
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "btnExportXLS_Click()", EX);
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : btnExportXLS_Click() \nException Occured\n" + EX.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "<script>showError('Something went wrong.Please try again','Franchise Status');</script>", false);
                return;
            }
        }
        #endregion

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                _BCEntity.Flag = (int)EnumCollection.EnumPermissionType.EnableRoles;
                setProperties();
                DataSet _dsAllAgents = _BCEntity.BCStatusReport();
                if (_dsAllAgents != null && _dsAllAgents.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "BC Status Report", _dsAllAgents);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('No data found.', 'Alert');</script>", false);
                }

            }

            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Page : BCVerificationLevel1.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "BCVerificationLevel1.aspx.cs", "btnexport_ServerClick", Ex);

            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = _BCEntity.BCStatusReport();

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "BC Status Report", dt);
                }
                {
                    //lblRecordCount.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('No data found.', 'Alert');</script>", false);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Page : BCVerificationLevel1.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "BCVerificationLevel1.aspx.cs", "btnexport_ServerClick", Ex);

            }
        }

        #region Image Button  events
        protected void btnView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;
                _BCEntity.Mode = "GetFranchiseDetails";
                _BCEntity.BCID = Convert.ToString(btn.CommandArgument);
                ViewState["BC ID"] = Convert.ToString(btn.CommandArgument);
                DataSet ds = _BCEntity.GetBCDocuments();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblApplicationID.Text = ds.Tables[0].Rows[0]["Bcid"].ToString();
                    string PanNo = _BCEntity.PanNo;
                    txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    ViewState["FranchiseType"] = ds.Tables[0].Rows[0]["RoleID"].ToString();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception Ex)
            {
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "btnView_Click()", Ex);
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
                return;
            }
        }

        protected void btnViewDownload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocumentByID();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    string fileName = Ds.Tables[0].Rows[0]["IdentityProofType"].ToString();
                    strURL = Ds.Tables[0].Rows[0]["IdentityProofDocument"].ToString();
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
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "ClientStatus.aspx.cs", "btnViewDownload_Click()", Ex);
                ErrorLog.CommonTrace("Page : ClientStatus.cs \nFunction : btnViewDownload_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
                return;
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocumentByID();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();
                    strURL = Ds.Tables[1].Rows[0]["AddressProofDocument"].ToString();
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
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "ImageButton1_Click()", Ex);
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : ImageButton1_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
                return;
            }
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocumentByID();
                if (Ds.Tables[2].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();
                    strURL = Ds.Tables[1].Rows[0]["AddressProofDocument"].ToString();
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
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "ClientStatus.aspx.cs", "ImageButton2_Click()", Ex);
                ErrorLog.CommonTrace("Page : ClientStatus.cs \nFunction : ImageButton2_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
                return;
            }
        }

        protected void imgbtnform_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocumentByID();
                if (Ds.Tables[3].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();
                    strURL = Ds.Tables[1].Rows[0]["AddressProofDocument"].ToString();
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
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "imgbtnform_Click()", Ex);
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : imgbtnform_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
                return;
            }
        }

        #endregion
        protected void EyeImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //EyeImage.Visible = true;
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void EyeImage1_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton Imgbtn = (ImageButton)sender;
            _BCEntity.Mode = "GetBCDocumentByID";
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

        protected void EyeImage3_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton Imgbtn = (ImageButton)sender;
            _BCEntity.Mode = "GetBCDocumentByID";
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

        #region Reupload Click
        protected void btnReulpoad_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;
                _BCEntity.Mode = "GetFranchiseDetails";
                _BCEntity.BCID = Convert.ToString(btn.CommandArgument);
                string BCReqID = _BCEntity.BCID;


                DataSet ds = _BCEntity.GetBCDocuments();
                //string PanNo = ds.Tables[0].Rows[0]["PanNo"].ToString();
                //Generating Query string for page called SearchResult.aspx  
                Response.Redirect("pgReuploadDocuments.aspx?Uid=" + BCReqID + "&SId=3", false);
            }
            catch (Exception Ex)
            {
                //_dbAccess.StoreErrorDescription(Session["Username"].ToString(), "FrmBcStatus.aspx.cs", "btnReulpoad_Click()", Ex);
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : btnReulpoad_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Contact System Administrator', 'Franchise Status');", true);
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
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('No data found.', 'Alert');</script>", false);

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
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('No data found.', 'Alert');</script>", false);
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
                //    { ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Warning');", true); }
                //}
                //else
                //{
                //}
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : btnexport_ServerClick\nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "FrmBcStatus.aspx.cs", "btnexport_ServerClick", Ex);

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
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : SetPageFiltersExport\nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "FrmBcStatus.aspx.cs", "SetPageFiltersExport();", Ex);
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
                ErrorLog.CommonTrace("Class : FrmBcStatus.cs \nFunction : BindExport() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "FrmBcStatus.cs", "BindExport()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "<script>showError('Something went wrong.Please try again','Verified Franchise');</script>", false);
                return;
            }
        }
        #endregion

        #region Image Btn Edit
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;
                _BCEntity.BCID = Convert.ToString(btn.CommandArgument);
                ViewState["BC ID"] = Convert.ToString(btn.CommandArgument);
                if (ValidateEdit())
                {
                    //DIVDocument.Visible = false;
                    DIVFilter.Visible = false;
                    DIVRegister.Visible = true;
                    string bcid = ViewState["BC ID"].ToString();
                    _BCEntity.Mode = "GetFranchiseDetailsToUpdate";
                    DataSet ds = _BCEntity.GetBCDetails();
                    GetDetailsOfFranchiseAndDistr(ViewState["BC ID"].ToString());
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Unable to edit','Warning');</script>", false);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonError(Ex);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Something went wrong.Please try again','Warning');</script>", false);

                return;
            }
        }

        public bool ValidateEdit()
        {
            bool IsValidRecord = true;
            string _Status = "Valid";
            try
            {
                string Status1 = null;
                //DataSet Status = new DataSet();
                _BCEntity.BCReqId = ViewState["BC ID"].ToString();
                Status1 = _BCEntity.EditValidate();
                if (Status1 == "96")
                {
                    _Status = Status1;
                    IsValidRecord = false;
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Unable to edit','Warning');</script>", false);
                }
                else
                {
                    IsValidRecord = true;
                    // ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Unable to edit','Warning');</script>", false);

                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Page : FrmBcStatus.cs \nFunction : ValidateEdit() \nException Occured\n" + Ex.Message);
                IsValidRecord = false;
            }
            return IsValidRecord;
        }


        public void GetDetailsOfFranchiseAndDistr(string BCID)
        {
            try
            {
                _BCEntity.BCID = BCID;

                DataSet ds = _BCEntity.GetBCDetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ddlCountry.DataSource = ds.Tables[1];
                    ddlCountry.DataValueField = "ID";
                    ddlCountry.DataTextField = "Name";
                    ddlCountry.DataBind();
                    ddlCountry.Items.Insert(0, new ListItem("-- Select --", "0"));

                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ddlState.DataSource = ds.Tables[2];
                    ddlState.DataValueField = "ID";
                    ddlState.DataTextField = "Name";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("-- Select --", "0"));


                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ddlCity.DataSource = ds.Tables[3];
                    ddlCity.DataValueField = "ID";
                    ddlCity.DataTextField = "Name";
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                }


                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    txtFirstName.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    txtMiddleName.Text = ds.Tables[0].Rows[0]["Middle"].ToString();
                    txtLastName.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                    txtFatherName.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                    //txtAadharNo.Text = ds.Tables[0].Rows[0]["AadharNo"].ToString();

                    txtEmailID.Text = ds.Tables[0].Rows[0]["EmailID"].ToString();

                    txtPANNo.Text = ds.Tables[0].Rows[0]["PanNo"].ToString();
                    ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["BCCategory"].ToString();
                    DDlOrg.SelectedValue = ds.Tables[0].Rows[0]["TypeOfOrg"].ToString();

                    //GST ENCRYPTION
                    txtGSTNo.Text = ds.Tables[0].Rows[0]["GSTNo"].ToString();

                    if (ds.Tables[0].Rows[0]["Gender"].ToString() != "")
                    {
                        ddlGender.SelectedValue = ds.Tables[0].Rows[0]["Gender"].ToString();
                    }


                    txtAccountNumber.Text = ds.Tables[0].Rows[0]["AccountNumber"].ToString();
                    txtIFsccode.Text = ds.Tables[0].Rows[0]["IFSCCode"].ToString();

                    if (ds.Tables[0].Rows[0]["Country"].ToString() != "")
                    {
                        ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                    }

                    if (ds.Tables[0].Rows[0]["State"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                    }

                    txtDistrict.Text = ds.Tables[0].Rows[0]["District"].ToString();
                    if (ds.Tables[0].Rows[0]["City"].ToString() != "")
                    {
                        ddlCity.SelectedValue = ds.Tables[0].Rows[0]["City"].ToString();
                    }

                    txtPinCode.Text = ds.Tables[0].Rows[0]["Pincode"].ToString();


                    txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtLandlineNo.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();

                    txtAlterNateNo.Text = ds.Tables[0].Rows[0]["AlternetNo"].ToString();
                    txtRegisteredAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();


                    if (ds.Tables[0].Rows[0]["AEPS"].ToString() != "" || ds.Tables[0].Rows[0]["AEPS"].ToString() != null)
                    {
                        HdnAEPS.Value = ds.Tables[0].Rows[0]["AEPS"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["AEPS"].ToString() == "1")
                    {
                        chkAEPS.Checked = true;
                    }
                    else
                    {
                        chkAEPS.Checked = false;
                    }
                    if (ds.Tables[0].Rows[0]["MATM"].ToString() != "" || ds.Tables[0].Rows[0]["MATM"].ToString() != null)
                    {
                        HdnMATM.Value = ds.Tables[0].Rows[0]["MATM"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["MATM"].ToString() == "1")
                    {
                        chkMATM.Checked = true;
                    }
                    else
                    {
                        chkMATM.Checked = false;
                    }
                    DIVRegister.Visible = true;
                }
                else
                {
                    clearselection();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonError(Ex);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Something went wrong.Please try again','Warning');</script>", false);

                return;
            }
        }
        #endregion

        #region Clear

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            DIVFilter.Visible = true;
            //DIVDocument.Visible = true;
            DIVRegister.Visible = false;

            //ddlClientCode.SelectedValue = "0";
            //ddlState.SelectedValue = "0";

            //ddlBCCode.DataSource = null;
            //ddlBCCode.DataBind();
            //ddlBCCode.Items.Clear();
            //ddlBCCode.Items.Insert(0, new ListItem("-- Franchise --", "0"));
            //ddlStatus.SelectedValue = "0";
            //ddlStatus.Enabled = true;
            //DIVRegister.Visible = false;

        }
        protected void clearselection()
        {
            ddlCategory.SelectedValue = "0";
            ddlState.SelectedValue = "0";
            ddlCity.SelectedValue = "0";
            ddlClient.SelectedValue = "0";
            ddlCountry.SelectedValue = "0";
            ddlExport.SelectedValue = "0";
            ddlFranchise.SelectedValue = "0";
            ddlGender.SelectedValue = "0";
            DDlOrg.SelectedValue = "0";
            ddlStatusType.SelectedValue = "0";

            //ddlBCCode.DataSource = null;
            //ddlBCCode.DataBind();
            //ddlBCCode.Items.Clear();
            //ddlBCCode.Items.Insert(0, new ListItem("-- Franchise --", "0"));
            //ddlStatus.SelectedValue = "0";
            //ddlStatus.Enabled = true;
            //DIVRegister.Visible = false;
        }
        #endregion

        #region Dropdown Events
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _BCEntity.State = ddlState.SelectedValue.ToString();

                DataSet ds = _BCEntity.GetCityOnStateChange();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlCity.DataSource = ds.Tables[0];
                    ddlCity.DataValueField = "ID";
                    ddlCity.DataTextField = "Name";
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonError(Ex);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Something went wrong.Please try again','Warning');</script>", false);
                return;
            }
        }
        #endregion

        #region Click Events
        protected void btnSubmitDetails_Click(object sender, EventArgs e)
        {
            try
            {
                // Unmasking Form - Variables Which are masked on Page Level.
                txtAccountNumber.Text = !string.IsNullOrEmpty(hidAccNo.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAccNo.Value)) : txtAccountNumber.Text;
                txtIFsccode.Text = !string.IsNullOrEmpty(hidAccIFC.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAccIFC.Value)) : txtIFsccode.Text;

                if (ValidateSetProperties())
                {
                    _BCEntity.Mode = "UpdateFranchiseDetails";
                    _BCEntity.UserName = Session["Username"].ToString();
                    _BCEntity.BCID = ViewState["BC ID"].ToString().Trim();
                    _BCEntity.FirstName = txtFirstName.Text.Trim();
                    _BCEntity.MiddleName = txtMiddleName.Text.Trim();
                    _BCEntity.LastName = txtLastName.Text.Trim();
                    _BCEntity.FatherName = txtFatherName.Text.Trim();
                    _BCEntity.PersonalEmail = txtEmailID.Text.Trim();
                    //_BCEntity.ClientId = ddlClientCode.SelectedValue.ToString().Trim();
                    //  _BCEntity.AadharNo = txtAadharNo.Text.Trim();

                    _BCEntity.PanNo = txtPANNo.Text.Trim();
                    _BCEntity.GSTNo = txtGSTNo.Text.Trim();
                    _BCEntity.RegisteredAddress = txtRegisteredAddress.Text.Trim();
                    _BCEntity.Country = ddlCountry.SelectedValue.ToString().Trim();
                    _BCEntity.State = ddlState.SelectedValue.ToString().Trim();
                    _BCEntity.City = ddlCity.SelectedValue.ToString().Trim();
                    _BCEntity.Pincode = txtPinCode.Text.Trim();
                    _BCEntity.BcCategory = ddlCategory.SelectedItem.ToString();
                    _BCEntity.PersonalContact = txtContactNo.Text.Trim();
                    _BCEntity.LandlineContact = txtLandlineNo.Text.Trim();
                    _BCEntity.TypeOfOrg = DDlOrg.SelectedItem.ToString();


                    _BCEntity.IdCardValue = txtPANNo.Text.Trim();
                    _BCEntity.AccountNumber = txtAccountNumber.Text.ToString();
                    _BCEntity.IFSCCode = txtIFsccode.Text.Trim().ToString();
                    _BCEntity.District = txtDistrict.Text.Trim().ToString();
                    _BCEntity.AlternetNo = txtAlterNateNo.Text.Trim().ToString();
                    _BCEntity.Gender = ddlGender.SelectedValue.Trim();
                    _BCEntity.Category = ddlCategory.SelectedValue.Trim();
                    _BCEntity.ShopAddress = txtRegisteredAddress.Text.Trim();
                    _BCEntity.ShopCountry = Convert.ToInt32(ddlCountry.SelectedValue);
                    _BCEntity.ShopState = Convert.ToInt32(ddlState.SelectedValue);
                    _BCEntity.ShopDistrict = txtDistrict.Text.Trim().ToString();
                    _BCEntity.ShopCity = Convert.ToInt32(ddlCity.SelectedValue);
                    _BCEntity.ShopPincode = txtPinCode.Text.Trim();
                    _BCEntity.LocalAddress = txtRegisteredAddress.Text.Trim();
                    _BCEntity.LocalCountry = Convert.ToInt32(ddlCountry.SelectedValue);
                    _BCEntity.LocalState = Convert.ToInt32(ddlState.SelectedValue);
                    _BCEntity.LocalCity = Convert.ToInt32(ddlCity.SelectedValue);
                    _BCEntity.LocalDistrict = txtDistrict.Text.Trim().ToString();
                    _BCEntity.LocalPincode = txtPinCode.Text.Trim();



                    //_BCEntity.ForAEPS = chkAEPS.Checked == true ? 1 : 0;
                    //_BCEntity.ForBBPS = chkbbps.Checked == true ? 1 : 0;
                    //_BCEntity.ForDMT = chkdmt.Checked == true ? 1 : 0;
                    //_BCEntity.ForMicroATM = chkMatm.Checked == true ? 1 : 0;

                    if (HdnAEPS.Value == "2" && chkAEPS.Checked == true)
                    {
                        _BCEntity.ForAEPS = 1;
                    }
                    else if (chkAEPS.Checked == true && HdnAEPS.Value == "0")
                    {
                        _BCEntity.ForAEPS = 1;
                    }
                    else if (chkAEPS.Checked == false && HdnAEPS.Value == "1")
                    {
                        _BCEntity.ForAEPS = 0;
                    }
                    else
                    {
                        _BCEntity.ForAEPS = Convert.ToInt32(HdnAEPS.Value);
                    }


                    if (chkMATM.Checked == true && HdnMATM.Value == "2")
                    {
                        _BCEntity.ForMicroATM = 1;
                    }
                    else if (chkMATM.Checked == true && HdnMATM.Value == "0")
                    {
                        _BCEntity.ForMicroATM = 1;

                    }
                    else if (chkMATM.Checked == false && HdnMATM.Value == "1")
                    {
                        _BCEntity.ForMicroATM = 0;
                    }
                    else
                    {
                        _BCEntity.ForMicroATM = Convert.ToInt32(HdnMATM.Value);
                    }


                    try { ErrorLog.CommonTrace("Franchise-Update Request Received For FranchiseID : " + _BCEntity.BCID + " : Updated By :" + Session["Username"].ToString() + " :\n" + ErrorLog.XmlSerialser(_BCEntity)); }
                    catch (Exception Ex)
                    {
                        ErrorLog.CommonTrace("Franchise-Update Request Received For FranchiseID : " + _BCEntity.BCID + " : Updated By :" + Session["Username"].ToString() + " :\n" + Ex.Message);
                    }

                    DataSet dsCheck = _BCEntity.UpdateBCDetails();
                    if (dsCheck != null && dsCheck.Tables.Count > 0 && dsCheck.Tables[0].Rows.Count > 0 && dsCheck.Tables[0].Rows[0][0].ToString() == "Success")
                    {
                        ErrorLog.CommonTrace("Franchise-Update Request Received For FranchiseID: " + _BCEntity.BCID + " DB Status: Successful");
                        try
                        {

                            DIVFilter.Visible = true;
                            //  DIVDocument.Visible = true;
                            DIVRegister.Visible = false;
                            fillGrid();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Franchise Details has been updated successfully.','Edit Franchise');</script>", false);

                            //Email Send Code
                            //_EmailAlerts.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.EmailAlert);
                            //_EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.UpdateDetails);
                            //_EmailAlerts.SubCategoryTypeId = null;
                            //_EmailAlerts.ClientID = ddlClientCode.SelectedValue;
                            //_EmailAlerts.UserName = Session["Username"].ToString();
                            //_EmailAlerts.UserID = ddlFranchiseCode.SelectedValue;
                            //_EmailAlerts.Flag = "1";
                            //ErrorLog.CommonTrace("Page : EditFranchiseDetails.cs \nFunction : EditFranchiseDetails() => Edit Franchise  Details forwarded for Email Preparation. => PrepareEmailFormat()");
                            //_EmailAlerts.PrepareEmailFormat();


                            //Email Send Code
                            //_EmailAlerts.AlertTypeId = Convert.ToString((int)EnumCollection.AlertType.SMSAlert);
                            //_EmailAlerts.CategoryTypeId = Convert.ToString((int)EnumCollection.EmailConfiguration.UpdateDetails);
                            //_EmailAlerts.SubCategoryTypeId = null;
                            //_EmailAlerts.ClientID = ddlClientCode.SelectedValue;
                            //_EmailAlerts.UserName = Session["Username"].ToString();
                            //_EmailAlerts.UserID = ddlFranchiseCode.SelectedValue;
                            //_EmailAlerts.Flag = "1";
                            //ErrorLog.CommonTrace("Page : EditFranchiseDetails.cs \nFunction : EditFranchiseDetails() => Edit Franchise  Details forwarded for SMS Preparation. => PrepareSMSFormat()");
                            //_EmailAlerts.PrepareSMSFormat();
                        }
                        catch (Exception Ex)
                        {
                            ErrorLog.CommonError(Ex);
                        }

                    }

                    else
                    {
                        ErrorLog.CommonTrace("Franchise-Update Request Received For FranchiseID: " + _BCEntity.BCID + " DB Status: Failed");

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Sorry, unable process your request.', 'Edit Franchise');</script>", false);

                    }
                    //ddlClientCode.SelectedValue = "0";
                    ddlState.SelectedValue = "0";

                    //ddlBCCode.DataSource = null;
                    //ddlBCCode.DataBind();
                    //ddlBCCode.Items.Clear();
                    //ddlBCCode.Items.Insert(0, new ListItem("No Data Found", "0"));
                    DIVRegister.Visible = false;


                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonError(Ex);
                DIVFilter.Visible = true;
                // DIVDocument.Visible = true;
                DIVRegister.Visible = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Sorry, unable process your request.', 'Edit FranchiseSomething went wrong.Please try again','Warning');</script>", false);
                return;
            }
        }
        #endregion

        #region Manual Insert Validations and Set Properties
        private bool ValidateSetProperties()
        {
            _CustomeRegExpValidation = new clsCustomeRegularExpressions();
            try
            {
                // ClientName
                if (hd_txtFirstName.Value == "1" || !string.IsNullOrEmpty(txtFirstName.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithSpace, txtFirstName.Text))
                    {
                        txtFirstName.Focus();
                        //DIVRegister.Visible = true;
                        //div_Upload.Visible = true;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid Client Name.', 'Client Name');</script>", false);
                        return false;
                    }
                    else _BCEntity.FileName = txtFirstName.Text.ToString();

                //Check Service
                if (chkAEPS.Checked == false)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Select at least one Service.', 'Service');</script>", false);
                    return false;
                }


                // AccountNumber
                //if (hd_txtAccountNumber.Value == "1" || !string.IsNullOrEmpty(txtAccountNumber.Text))
                //    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.NumberOnly, txtAccountNumber.Text))
                //    {
                //        txtAccountNumber.Focus();
                //        DIVRegister.Visible = true;
                //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid Account Number.', 'AccountNumber');</script>", false);
                //        return false;
                //    }
                //    else _BCEntity.AccountNumber = txtAccountNumber.Text.ToString();

                // IFSCCode
                if (hd_txtIFsccode.Value == "1" || !string.IsNullOrEmpty(txtIFsccode.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbers, txtIFsccode.Text))
                    {
                        txtIFsccode.Focus();
                        DIVRegister.Visible = true;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid IFSC Code.', 'IFSC Code');</script>", false);
                        return false;
                    }
                    else _BCEntity.IFSCCode = txtIFsccode.Text.Trim().ToString();

                // Registered Address
                if (hd_txtRegisteredAddress.Value == "1" || !string.IsNullOrEmpty(txtRegisteredAddress.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Address, txtRegisteredAddress.Text))
                    {
                        txtRegisteredAddress.Focus();
                        DIVRegister.Visible = true;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid Address.', 'Registered Address');</script>", false);
                        return false;
                    }
                    else _BCEntity.RegisteredAddress = txtRegisteredAddress.Text.Trim();

                // Pincode
                if (hd_txtPinCode.Value == "1" || !string.IsNullOrEmpty(txtPinCode.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Pincode, txtPinCode.Text))
                    {
                        txtPinCode.Focus();
                        DIVRegister.Visible = true;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid Pincode.', 'Pincode');</script>", false);
                        return false;
                    }
                    else _BCEntity.Pincode = txtPinCode.Text.Trim();

                // Country
                if (ddlCountry.SelectedValue == "0")
                {
                    ddlCountry.Focus();
                    DIVRegister.Visible = true;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please Select Country.', 'Country');</script>", false);
                    return false;
                }
                else _BCEntity.Country = ddlCountry.SelectedValue.ToString().Trim();

                // State
                if (ddlState.SelectedValue == "0")
                {
                    ddlState.Focus();
                    DIVRegister.Visible = true;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please Select State.', 'State');</script>", false);
                    return false;
                }
                else _BCEntity.State = ddlState.SelectedValue.ToString().Trim();

                // City
                if (ddlCity.SelectedValue == "0")
                {
                    ddlCity.Focus();
                    DIVRegister.Visible = true;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Please Select City.', 'City');</script>", false);
                    return false;
                }
                else _BCEntity.City = ddlCity.SelectedValue.ToString().Trim();

                //District
                if (hd_txtDistrict.Value == "1" || !string.IsNullOrEmpty(txtDistrict.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbersCharacterSpace, txtDistrict.Text))
                    {
                        txtDistrict.Focus();
                        DIVRegister.Visible = true;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid District.', 'District');</script>", false);
                        return false;
                    }
                    else _BCEntity.District = txtDistrict.Text.Trim().ToString();


                // Personal Contact
                if (hd_txtContactNo.Value == "1" || !string.IsNullOrEmpty(txtContactNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, txtContactNo.Text))
                    {
                        txtContactNo.Focus();
                        DIVRegister.Visible = true;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid Contact No.', 'Personal Contact No.');</script>", false);
                        return false;
                    }
                    else _BCEntity.PersonalContact = txtContactNo.Text.Trim();

                // Landline Contact
                if (hd_txtLandlineNo.Value == "1" || !string.IsNullOrEmpty(txtLandlineNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.NumberOnly, txtLandlineNo.Text))
                    {
                        txtLandlineNo.Focus();
                        DIVRegister.Visible = true;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Landline No.', 'Invalid Landline No.');</script>", false);
                        return false;
                    }
                    else _BCEntity.LandlineContact = txtLandlineNo.Text.Trim();

                // Alternate Mobile No
                if (hd_txtAlterNateNo.Value == "1" || !string.IsNullOrEmpty(txtAlterNateNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, txtAlterNateNo.Text))
                    {
                        txtAlterNateNo.Focus();
                        DIVRegister.Visible = true;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Invalid Mobile No.', 'Alternate Mobile No.');</script>", false);
                        return false;
                    }
                    else _BCEntity.AlternetNo = txtAlterNateNo.Text.Trim().ToString();



                // GSTNo
                if ((hd_txtGSTNo.Value == "1" || !string.IsNullOrEmpty(txtGSTNo.Text)))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.GSTIN, txtGSTNo.Text))
                    {
                        txtGSTNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>alert('Invalid GST No.', 'GST No');</script>", false);
                        return false;
                    }
                    else _BCEntity.GSTNo = txtGSTNo.Text.Trim();

            }
            catch (Exception Ex)
            {
                ErrorLog.CommonError(Ex);
                return false;
            }
            return true;
        }
        #endregion

    }
}