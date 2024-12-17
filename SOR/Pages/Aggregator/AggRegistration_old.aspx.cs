using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using BussinessAccessLayer;
using AppLogger;
using System.Threading;

namespace SOR.Pages.Aggregator
{
    public partial class AggRegistration : System.Web.UI.Page
    {
        #region Object Declarations
        //BCEntity _BCEntity = new BCEntity();
        ClientRegistrationEntity clientMngnt = new ClientRegistrationEntity();
        AggregatorEntity _AggregatorEntity = new AggregatorEntity();
        public clsCustomeRegularExpressions customeRegExpValidation = null;
        public clsCustomeRegularExpressions _CustomeRegExpValidation
        {
            get { if (customeRegExpValidation == null) customeRegExpValidation = new clsCustomeRegularExpressions(); return customeRegExpValidation; }
            set { customeRegExpValidation = value; }
        }
        //BussinessAccessLayer.AggregatorEntity _AggregatorEntity = new AggregatorEntity();

        public string pathId, PathAdd, PathSig;
        bool _IsValidFileAttached = false;
        string RequestId = string.Empty;
        #endregion

        #region Objects Declaration
        BCEntity _BCEntity = new BCEntity();
        public string UserName { get; set; }
        AppSecurity appSecurity = null;
        public AppSecurity _AppSecurity
        {
            get { if (appSecurity == null) appSecurity = new AppSecurity(); return appSecurity; }
            set { appSecurity = value; }
        }
        #endregion;

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "AggRegistration.aspx", "14");
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

                        if (!IsPostBack == true)
                        {
                            //fillClient();
                            //BindDropdownCountryState();
                            FillGrid(EnumCollection.EnumBindingType.BindGrid);
                            UserPermissions.RegisterStartupScriptForNavigationListActive("4", "14");
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
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: Page_Load: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion

        #region Button_Click-FillGrid
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //if (ddlBucketId.SelectedValue != "-1" && ddlRequestStatus.SelectedValue == "-1")
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Please Select Request Status', 'BC Registration');", true);
                //    return;
                //}
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCRegistration.cs \nFunction : btnSearch_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }
        protected void butnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ddlRequestType.SelectedValue = null;
                //ddlBucketId.SelectedValue = null;
                ddlRequestStatus.SelectedValue = null;

                txtPanNoF.Text = string.Empty;
                //txtAadharNoF.Value = string.Empty;
                //txtGSTNoF.Value = string.Empty;
                //txtContactNoF.Value = string.Empty;
                //txtPersonalEmailIDF.Value = string.Empty;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCRegistration.cs \nFunction : butnCancel_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }
        #endregion

        #region FillGrid
        public DataSet FillGrid(EnumCollection.EnumBindingType enumBinding)
        {
            DataSet ds = null;
            try
            {
                _BCEntity.BCID = Session["Username"].ToString();
                // _BCEntity.= ddlBucketId.SelectedValue=="1" && ddlRequestStatus.SelectedValue=="0"?
                _BCEntity.Flag = (int)enumBinding;
                setProperties();

                ds = _BCEntity.GetBCRequestList();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvBCOnboard.DataSource = ds.Tables[0];
                    gvBCOnboard.DataBind();
                    btnExportCSV.Visible = true;
                    btndownload.Visible = true;
                    // lblRecordsTotal.Text = "Total " + Convert.ToString(ds.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    gvBCOnboard.DataSource = null;
                    gvBCOnboard.DataBind();
                    btndownload.Visible = false;
                    btnExportCSV.Visible = false;
                    //lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCRegistration.cs \nFunction : FillGrid() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "showError('Something went wrong.Please try again','Warning');", true);
            }
            return ds;
        }
        #endregion

        #region Grid Method
        protected void gvBCOnboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBCOnboard.PageIndex = e.NewPageIndex;
            FillGrid(EnumCollection.EnumBindingType.BindGrid);
        }
        protected void gvBCOnboard_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grid = (GridView)sender;
                    string Bucket = grid.DataKeys[e.Row.RowIndex].Values[0].ToString();
                    string ReqStatus = grid.DataKeys[e.Row.RowIndex].Values[1].ToString();
                    string ReqType = grid.DataKeys[e.Row.RowIndex].Values[2].ToString();
                    ImageButton imageButton = e.Row.FindControl("btnView") as ImageButton;
                    ImageButton imageButton2 = e.Row.FindControl("btndelete") as ImageButton;
                    if (imageButton != null)
                    {
                        if ((Bucket.ToLower() == "self" || ReqStatus.ToLower() == "declined") && (ReqType.ToLower() == "registration"))
                        {
                            imageButton.Enabled = true;
                            imageButton.Visible = true;
                            imageButton2.Enabled = true;
                            imageButton2.Visible = true;
                        }
                        else
                        {
                            imageButton.Enabled = false;
                            imageButton.Visible = false;
                            imageButton2.Enabled = false;
                            imageButton2.Visible = false;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("BCRegistration: gvBCOnboard_RowDataBound: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'alert');", true);
                return;
            }
        }
        protected void gvBCOnboard_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Contains("EditDetails"))
                {
                    try
                    {
                        ImageButton lb = (ImageButton)e.CommandSource;
                        GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                        string BCRequestId = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["BCReqId"]).ToString();
                        string reqtype = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["ActivityType"]).ToString();
                        string reqstatus = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["RequestStatus"]).ToString();
                        string bucket = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["Bucket"]).ToString();
                        _BCEntity.BCReqId = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["BCReqId"]).ToString();
                        if (ValidateReEditRequest())
                        {
                            int BCReqID = Convert.ToInt32(BCRequestId);
                            HidBCID.Value = BCRequestId;

                            if (bucket.ToLower() == "self")
                            {
                                Response.Redirect("../../Pages/BC/ReprocessBCDetails.aspx?BCReqID=" + BCReqID + "&" + "RequestType=" + reqtype + " ", false);
                            }
                            else if ((reqtype == "0") && reqstatus.ToLower() == "declined")
                            {
                                int IsSelfData = 0;
                                Response.Redirect("../../Pages/BC/EditBCRegistrationDetails.aspx?BCReqId=" + _BCEntity.BCReqId + "&" + "RequestType=" + reqtype + "&" + "IsSelfData=" + IsSelfData + " ", false);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Agent request already pending for verification.', 'Warning');", true);
                            return;
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.CommonTrace("Class : AgentRegistration.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message);
                        throw Ex;
                    }
                }
                if (e.CommandName.Contains("DeleteDetails"))
                {
                    if (hdnDeleteConfirmation.Value.ToString() == "Yes")
                    {
                        try
                        {
                            ImageButton lb = (ImageButton)e.CommandSource;
                            GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                            string BCReqId = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["AgentReqId"]).ToString();
                            _BCEntity.BCReqId = BCReqId;
                            string _status = _BCEntity.DeleteBcDetails();
                            if (_status == "00")
                            {
                                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Deleted Successfully.', 'Warning');", true);
                                return;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Data Delete Unsuccessful.', 'Warning');", true);
                                return;
                            }

                        }
                        catch (Exception Ex)
                        {
                            ErrorLog.CommonTrace("Class : AgentRegistration.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message);
                            throw Ex;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("OBAgStatus: gvBlockAG_RowCommand: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }
        #endregion

        #region setProperties
        private void setProperties()
        {
            try
            {
                //if (ddlBucketId.SelectedValue == "1" && ddlRequestStatus.SelectedValue == "0")
                //{
                //    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                //}

                //else if (ddlBucketId.SelectedValue == "1" && ddlRequestStatus.SelectedValue == "1")
                //{
                //    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                //}

                //else if (ddlBucketId.SelectedValue == "1" && ddlRequestStatus.SelectedValue == "2")
                //{
                //    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerDecline);
                //}

                //else if (ddlBucketId.SelectedValue == "2" && ddlRequestStatus.SelectedValue == "0")
                //{
                //    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                //}

                //else if (ddlBucketId.SelectedValue == "2" && ddlRequestStatus.SelectedValue == "1")
                //{
                //    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                //}

                //else if (ddlBucketId.SelectedValue == "2" && ddlRequestStatus.SelectedValue == "2")
                //{
                //    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerDecline);
                //}
                ////////////////////////
                if (ddlRequestStatus.SelectedValue == "0")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                }

                else if (ddlRequestStatus.SelectedValue == "1")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                }

                else if (ddlRequestStatus.SelectedValue == "2")
                {
                    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerDecline);
                }

                else if (ddlRequestStatus.SelectedValue == "0")
                {
                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                }

                else if (ddlRequestStatus.SelectedValue == "1")
                {
                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                }

                else if (ddlRequestStatus.SelectedValue == "2")
                {
                    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerDecline);
                }
                _BCEntity.RequestTypeId = ddlRequestType.SelectedValue != "-1" ? (ddlRequestType.SelectedValue) : null;
                _BCEntity.PanNo = !string.IsNullOrEmpty(txtPanNoF.Text) ? txtPanNoF.Text.Trim() : null;
                //_BCEntity.AadharNo = !string.IsNullOrEmpty(txtAadharNoF.Value) ? txtAadharNoF.Value.Trim() : null;
                //_BCEntity.GSTNo = !string.IsNullOrEmpty(txtGSTNoF.Value) ? txtGSTNoF.Value.Trim() : null;
                //_BCEntity.PersonalContact = !string.IsNullOrEmpty(txtContactNoF.Value) ? txtContactNoF.Value.Trim() : null;
                //_BCEntity.PersonalEmail = !string.IsNullOrEmpty(txtPersonalEmailIDF.Value) ? txtPersonalEmailIDF.Value.Trim() : null;

            }
            catch (Exception EX)
            {
                ErrorLog.BCManagementTrace("Page : BcRegistration.cs \nFunction : setProperties() \nException Occured\n" + EX.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }

        #endregion

        #region Bind Client
        //public void fillClient()
        //{
        //    DataSet _dsClient = new DataSet();
        //    try
        //    {
        //        ddlclient.Items.Clear();
        //        ddlclient.DataSource = null;
        //        ddlclient.DataBind();

        //        clientMngnt.UserName = Session["Username"].ToString();
        //        clientMngnt.IsRemoved = "0";
        //        clientMngnt.IsActive = "1";
        //        clientMngnt.IsdocUploaded = "1";
        //        clientMngnt.VerificationStatus = "1";
        //        clientMngnt.SFlag = (int)EnumCollection.EnumPermissionType.EnableRoles;

        //        _dsClient = clientMngnt.BindClient();
        //        if (_dsClient != null && _dsClient.Tables.Count > 0)
        //        {
        //            if (_dsClient.Tables.Count > 0 && _dsClient.Tables[0].Rows.Count > 0)
        //            {
        //                if (_dsClient.Tables[0].Rows.Count == 1)
        //                {
        //                    ddlclient.Items.Clear();
        //                    ddlclient.DataSource = _dsClient.Tables[0].Copy();
        //                    ddlclient.DataTextField = "ClientName";
        //                    ddlclient.DataValueField = "ClientID";
        //                    ddlclient.DataBind();
        //                    ddlclient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
        //                }
        //                else
        //                {
        //                    ddlclient.Items.Clear();
        //                    ddlclient.DataSource = _dsClient.Tables[0].Copy();
        //                    ddlclient.DataTextField = "ClientName";
        //                    ddlclient.DataValueField = "ClientID";
        //                    ddlclient.DataBind();
        //                    ddlclient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
        //                }
        //            }
        //            else
        //            {
        //                ddlclient.Items.Clear();
        //                ddlclient.DataSource = null;
        //                ddlclient.DataBind();
        //                ddlclient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
        //            }
        //        }
        //        else
        //        {
        //            ddlclient.Items.Clear();
        //            ddlclient.DataSource = null;
        //            ddlclient.DataBind();
        //            ddlclient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.BCManagementTrace("Class : BCRegistration.cs \nFunction : fillClient() \nException Occured\n" + Ex.Message);

        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);


        //    }
        //}
        #endregion

        //#region Bind State/City
        //public void BindDropdownCountryState()
        //{
        //    try
        //    {
        //        _BCEntity.mode = (int)(EnumCollection.StateCityMode.PinCode);
        //        DataSet dsCountry = _BCEntity.GetCountryStateCityD();
        //        if (dsCountry != null && dsCountry.Tables.Count > 0 && dsCountry.Tables[0].Rows.Count > 0)
        //        {
        //            ddlCountry.DataSource = dsCountry;
        //            ddlCountry.DataValueField = "Country";
        //            ddlCountry.DataTextField = "Country";
        //            ddlCountry.DataBind();

        //            ddlState.DataSource = dsCountry;
        //            ddlState.DataValueField = "States";
        //            ddlState.DataTextField = "States";
        //            ddlState.DataBind();

        //            ddlDistrict.DataSource = dsCountry;
        //            ddlDistrict.DataValueField = "District";
        //            ddlDistrict.DataTextField = "District";
        //            ddlDistrict.DataBind();

        //            ddlCity.DataSource = dsCountry;
        //            ddlCity.DataValueField = "City";
        //            ddlCity.DataTextField = "City";
        //            ddlCity.DataBind();
        //        }
        //        else
        //        {
        //            _BCEntity.mode = (int)(EnumCollection.StateCityMode.ALLStateCity);
        //            DataSet ds = _BCEntity.GetCountryStateCityD();

        //            ddlCountry.DataSource = ds.Tables[0];
        //            ddlCountry.DataValueField = "Country";
        //            ddlCountry.DataTextField = "Country";
        //            ddlCountry.DataBind();
        //            ddlCountry.Items.Insert(0, new ListItem("-- Select --", "0"));

        //            ddlState.DataSource = ds.Tables[1];
        //            ddlState.DataValueField = "States";
        //            ddlState.DataTextField = "States";
        //            ddlState.DataBind();
        //            ddlState.Items.Insert(0, new ListItem("-- Select --", "0"));

        //            ddlDistrict.DataSource = ds.Tables[2];
        //            ddlDistrict.DataValueField = "District";
        //            ddlDistrict.DataTextField = "District";
        //            ddlDistrict.DataBind();
        //            ddlDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));

        //            ddlCity.DataSource = ds.Tables[3];
        //            ddlCity.DataValueField = "City";
        //            ddlCity.DataTextField = "City";
        //            ddlCity.DataBind();
        //            ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AgentManagementTrace("AgentRegistration: BindDropdownCountryState(): Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //    }


        //}
        //#endregion

        #region Validation Method
        public bool ValidateReEditRequest()
        {
            bool IsvalidRecord = true;
            string _requestid = string.Empty;
            _BCEntity.Flag = 2;
            string status = _BCEntity.ValidateEditBcDetails(out _requestid);
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
        #endregion

        protected void btnAddnew_Click(object sender, EventArgs e)
        {
            try
            {
                FillBc();
                divAction.Visible = false;
                divMainDetailsGrid.Visible = false;
                panelGrid.Attributes.CssStyle.Add("display", "none");
                
                DIVDetails.Visible = true;
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Class : BCRegistration.cs \nFunction : btnAddnew_Click() \nException Occured\n" + Ex.Message);
            }
        }
        #region Export

        protected void btnExportCSV_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);



                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "Business Correspondents Details", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BCRegistration.cs \nFunction : btnExportCSV_Click\nException Occured\n" + Ex.Message);
            }
        }
        
        protected void btndownload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "BC Details", dt);
                }
                {
                    //lblRecordCount.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);


                }

            }
            catch (Exception Ex)
            {
                ErrorLog.BCManagementTrace("Page : BCRegistration.cs \nFunction : btndownload_Click\nException Occured\n" + Ex.Message);
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
                //_systemLogger.WriteErrorLog(this, Ex);
                ErrorLog.BCManagementTrace(Ex.Message);
            }
            return pageFilters;
        }
        #endregion

        #region 
        protected void txtPinCode_TextChanged(object sender, EventArgs e)
        {
            //_AgentRegistrationDAL.AgentPincode = txtPinCode.Text.Trim();
            //BindDropdownCountryState();
        }
        protected void txtshoppin_TextChanged(object sender, EventArgs e)
        {
            //_AgentRegistrationDAL.ShopPinCode = txtshoppin.Text.Trim();
            //BindShopCountryState();
        }
        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlCity.DataSource = null;
                ddlCity.SelectedValue = null;
                ddlCity.ClearSelection();

                //_AgentRegistrationDAL.AgentDistrict = string.Empty;
                //_AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.City;
                //_AgentRegistrationDAL.AgentDistrict = ddlDistrict.SelectedValue.ToString();
                ////DataSet ds_District = _AgentRegistrationDAL.GetCountryStateCity(UserName, Mode, 0, Convert.ToInt32(_AgentRegistrationDAL.AgentState));
                //DataSet ds_City = _AgentRegistrationDAL.GetCountryStateCityD();
                //if (ds_City != null && ds_City.Tables.Count > 0 && ds_City.Tables[0].Rows.Count > 0)
                //{
                //    //ddlCity.DataSource = ds_State;
                //    //ddlCity.DataValueField = "ID";
                //    //ddlCity.DataTextField = "Name";
                //    //ddlCity.DataBind();
                //    //ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                //    ddlCity.DataSource = ds_City;
                //    ddlCity.DataValueField = "City";
                //    ddlCity.DataTextField = "City";
                //    ddlCity.DataBind();
                //    ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                //}
                //else
                //{
                //    ddlCity.DataSource = null;
                //    ddlCity.DataBind();
                //    ddlCity.Items.Insert(0, new ListItem("No Data Found", "0"));
                //}
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : ddlDistrict_SelectedIndexChanged() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.', 'Alert');", true);
            }
        }
        protected void ddlShopDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlShopCity.DataSource = null;
                ddlShopCity.SelectedValue = null;
                ddlShopCity.ClearSelection();

                //_AgentRegistrationDAL.ShopCity = string.Empty;
                //_AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.City;
                //_AgentRegistrationDAL.ShopDistrict = ddlShopDistrict.SelectedValue.ToString();
                //DataSet ds_City = _AgentRegistrationDAL.GetShopCountryStateCity();
                //if (ds_City != null && ds_City.Tables.Count > 0 && ds_City.Tables[0].Rows.Count > 0)
                //{
                //    ddlShopCity.DataSource = ds_City;
                //    ddlShopCity.DataValueField = "City";
                //    ddlShopCity.DataTextField = "City";
                //    ddlShopCity.DataBind();
                //    ddlShopCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                //}
                //else
                //{
                //    ddlShopCity.DataSource = null;
                //    ddlShopCity.DataBind();
                //    ddlShopCity.Items.Insert(0, new ListItem("No Data Found", "0"));
                //}
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : ddlShopDistrict_SelectedIndexChanged() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.', 'Alert');", true);
            }
        }
        #endregion
        #region Dropdown Events
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlDistrict.DataSource = null;

                //_AgentRegistrationDAL.AgentState = string.Empty;
                //_AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.District;
                //_AgentRegistrationDAL.AgentState = ddlState.SelectedValue.ToString();
                //DataSet ds_District = _AgentRegistrationDAL.GetCountryStateCityD();
                //if (ds_District != null && ds_District.Tables.Count > 0 && ds_District.Tables[0].Rows.Count > 0)
                //{

                //    ddlDistrict.DataSource = ds_District;
                //    ddlDistrict.DataValueField = "District";
                //    ddlDistrict.DataTextField = "District";
                //    ddlDistrict.DataBind();
                //    ddlDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));
                //}
                //else
                //{
                //    ddlDistrict.DataSource = null;
                //    ddlDistrict.DataBind();
                //    ddlDistrict.Items.Insert(0, new ListItem("No Data Found", "0"));
                //}
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: ddlState_SelectedIndexChanged(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void ddlShopState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //_AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.District;
                //_AgentRegistrationDAL.ShopState = ddlShopState.SelectedValue.ToString();
                //DataSet ds_District = _AgentRegistrationDAL.GetShopCountryStateCity();
                //if (ds_District != null && ds_District.Tables.Count > 0 && ds_District.Tables.Count > 0)
                //{
                //    ddlShopDistrict.DataSource = ds_District;
                //    ddlShopDistrict.DataValueField = "District";
                //    ddlShopDistrict.DataTextField = "District";
                //    ddlShopDistrict.DataBind();
                //    ddlShopDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));
                //}
                //else
                //{
                //    ddlShopDistrict.DataSource = null;
                //    ddlShopDistrict.DataBind();
                //    ddlShopDistrict.Items.Insert(0, new ListItem("No Data Found", "0"));
                //}
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: ddlShopState_SelectedIndexChanged(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion
        protected void btnSubmitDetails_Click(object sender, EventArgs e)
        {
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DIVDetails.Visible = false;
                //div_Upload.Visible = false;
                //divPaymentReceipt.Visible = false;
                btndownload.Visible = true;
                btnExportCSV.Visible = true;
                //divOnboardFranchise.Visible = false;
                divAction.Visible = true;
                divMainDetailsGrid.Visible = true;
                panelGrid.Attributes.CssStyle["display"] = "block"; // To show
                //ClearAllControls();
                //HidAGID.Value = string.Empty;
                //Session["AGCode"] = string.Empty;

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs:btnCancel_Click \nFunction : ValidateFile \nException Occured\n" + Ex.Message);
                throw Ex;
            }
        }
        #region DropDown
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
                DataTable dsbc = _AggregatorEntity.GetBC(UserName, VerificationStatus, IsActive, IsRemoved, null, IsdocUploaded);
                if (dsbc != null && dsbc.Rows.Count > 0 && dsbc.Rows.Count > 0)
                {
                    if (dsbc.Rows.Count == 1)
                    {
                        ddlbcCode.DataSource = dsbc;
                        ddlbcCode.DataValueField = "bccode";
                        ddlbcCode.DataTextField = "bcname";
                        ddlbcCode.DataBind();
                    }
                    else
                    {
                        ddlbcCode.DataSource = dsbc;
                        ddlbcCode.DataValueField = "bccode";
                        ddlbcCode.DataTextField = "bcname";
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
                ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Agent Registration');", true);
                return;
            }
        }
        #endregion
    }
}