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
using System.Text.RegularExpressions;

namespace SOR.Pages.BC
{
    public partial class AggRegistration : System.Web.UI.Page
    {

        #region Object Declarations
        //BCEntity _BCEntity = new BCEntity();
        ClientRegistrationEntity clientMngnt = new ClientRegistrationEntity();
        public clsCustomeRegularExpressions customeRegExpValidation = null;
        public clsCustomeRegularExpressions _CustomeRegExpValidation
        {
            get { if (customeRegExpValidation == null) customeRegExpValidation = new clsCustomeRegularExpressions(); return customeRegExpValidation; }
            set { customeRegExpValidation = value; }
        }

        public string pathId, PathAdd, PathSig;
        bool _IsValidFileAttached = false;
        int RequestId = 0;
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        #endregion

        #region Objects Declaration
        BCEntity _BCEntity = new BCEntity();
        AggregatorEntity _AggregatorEntity = new AggregatorEntity();
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
                ErrorLog.AggregatorTrace("AggregatorRegistration | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
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
                            UserPermissions.RegisterStartupScriptForNavigationListActive("5", "14");
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
                ErrorLog.AggregatorTrace("AggRegistration: Page_Load: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        #endregion

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
                ErrorLog.AggregatorTrace("AggRegistration: gvBCOnboard_RowDataBound: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong. Try again', 'alert');", true);
                return;
            }
        }


        protected void btnView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;
                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { '=' });
                string BCReqId = commandArgs[0];
                string Bucket = Convert.ToString(commandArgs[1]);
                _BCEntity.BCReqId = BCReqId;
                HidBCID.Value = BCReqId;

                GetDetails(BCReqId);

            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
            }
        }


        public bool valiadtereqid(string BCReqId)
        {
            DataSet ds = new DataSet();
            _BCEntity.BCReqId = BCReqId;
            bool isValid = true;
            try
            {

                _BCEntity.BCReqId = BCReqId;
                ds = _BCEntity.validatreq();
                // valiadtereqid(agReqId);
                if (ds.Tables[0].Rows[0]["Status"].ToString() == "00")
                {
                    _BCEntity.BCReqId = BCReqId;
                    isValid = true;

                    //GetDetails(agReqId);
                }
                else
                {
                    isValid = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('You Cannot Edit Recods', 'Agent Registration');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : valiadtereqid() \nException Occured\n" + Ex.Message);
            }
            return isValid;
        }


        public void GetDetails(string BCReqId)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | GetDetails() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                _BCEntity.BCRequest = BCReqId;

                DataSet ds = _BCEntity.GetPendingbcDetails();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    if (ds != null && ds.Tables[2].Rows.Count > 0)
                    {
                        ddlState.DataSource = ds.Tables[2];
                        ddlState.DataValueField = "ID";
                        ddlState.DataTextField = "Name";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-- Select --", "0"));

                    }

                    if (ds != null && ds.Tables[3].Rows.Count > 0)
                    {
                        ddlCity.DataSource = ds.Tables[3];
                        ddlCity.DataValueField = "ID";
                        ddlCity.DataTextField = "Name";
                        ddlCity.DataBind();
                        ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));

                    }
                    txtFirstName.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    txtMiddleName.Text = ds.Tables[0].Rows[0]["Middle"].ToString();
                    txtLastName.Text = ds.Tables[0].Rows[0]["LastName"].ToString();

                    if (ds.Tables[0].Rows[0]["Gender"].ToString() != "")
                    {
                        ddlGender.SelectedValue = ds.Tables[0].Rows[0]["Gender"].ToString();
                    }



                    txtPANNo.Text = ds.Tables[0].Rows[0]["PanNo"].ToString();
                    txtGSTNo.Text = ds.Tables[0].Rows[0]["GSTNo"].ToString();
                    txtaadharno.Text = ds.Tables[0].Rows[0]["AadharNo"].ToString();
                    if (ds.Tables[0].Rows[0]["BCCategory"].ToString() != "")
                    {
                        ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["BCCategory"].ToString();
                    }
                    txtAccountNumber.Text = ds.Tables[0].Rows[0]["AccountNumber"].ToString();
                    txtIFsccode.Text = ds.Tables[0].Rows[0]["IFSCCode"].ToString();
                    // txtPass.Text = ds.Tables[0].Rows[0]["PassportNo"].ToString();
                    if (ds.Tables[0].Rows[0]["TypeOfOrg"].ToString() != "")
                    {
                        DDlOrg.SelectedValue = ds.Tables[0].Rows[0]["TypeOfOrg"].ToString();
                    }

                    txtRegisteredAddress.Text = ds.Tables[0].Rows[0]["BCAddress"].ToString();
                    if (ds.Tables[0].Rows[0]["Country"].ToString() != "")
                    {
                        ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["State"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["District"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["District"].ToString();
                    }

                    //txtDistrict.Text = ds.Tables[0].Rows[0]["District"].ToString();

                    if (ds.Tables[0].Rows[0]["City"].ToString() != "")

                    {

                        ddlCity.SelectedValue = ds.Tables[0].Rows[0]["City"].ToString();
                    }
                    txtPinCode.Text = ds.Tables[0].Rows[0]["PinCode"].ToString();

                    txtEmailID.Text = ds.Tables[0].Rows[0]["EmailID"].ToString();
                    txtcontact.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtlandline.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();
                    txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtLandlineNo.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();
                    //txtAlterNateNo.Text = ds.Tables[0].Rows[0]["AgentDOB"].ToString();

                    //TextBox9.Text = ds.Tables[0].Rows[0]["PersonalEmailID"].ToString();
                    if (ds.Tables[0].Rows[0]["AEPS"].ToString() != "" || ds.Tables[0].Rows[0]["AEPS"].ToString() != null)
                    {
                        //  HdnAEPS.Value = ds.Tables[0].Rows[0]["AEPS"].ToString();
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
                        //HdnMATM.Value = ds.Tables[0].Rows[0]["MATM"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["MATM"].ToString() == "1")
                    {
                        chkMATM.Checked = true;
                    }
                    else
                    {
                        chkMATM.Checked = false;
                    }
                    //DIVDetails.Visible = true;
                    divOnboardFranchise.Visible = true;
                    divAction.Visible = false;
                    divMainDetailsGrid.Visible = false;
                    DIVDetails.Visible = true;
                }
                else
                {
                    //clearselection();
                }
                ErrorLog.AggregatorTrace("AggregatorRegistration | GetDetails() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : GetDetails() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }

        protected void btnSubmitDetails_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | btnSubmitDetails_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (HiddenField1.Value.ToString() == "Yes")
                {

                    
                    Session["PanNo"] = txtPANNo.Text;

                    txtPANNo.Text = !string.IsNullOrEmpty(hidPan.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidPan.Value)) : txtPANNo.Text;
                    txtaadharno.Text = !string.IsNullOrEmpty(hidAadh.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAadh.Value)) : txtaadharno.Text;
                    txtGSTNo.Text = !string.IsNullOrEmpty(hidSgst.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidSgst.Value)) : txtGSTNo.Text;
                    txtAccountNumber.Text = !string.IsNullOrEmpty(hidAccNo.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAccNo.Value)) : txtAccountNumber.Text;
                    txtIFsccode.Text = !string.IsNullOrEmpty(hidAccIFC.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAccIFC.Value)) : txtIFsccode.Text;
                    if (ValidateSetProperties())
                    {
                        _BCEntity.ForAEPS = chkAEPS.Checked == true ? 1 : 0;
                        _BCEntity.ForMicroATM = chkMATM.Checked == true ? 1 : 0;
                        _BCEntity.ForDMT = chkdmt.Checked == true ? 1 : 0;
                        _BCEntity.FirstName = txtFirstName.Text.Trim();
                        _BCEntity.MiddleName = txtMiddleName.Text.Trim();
                        _BCEntity.LastName = txtLastName.Text.Trim();
                        _BCEntity.PersonalEmail = txtEmailID.Text.Trim();
                        _BCEntity.AadharNo = txtaadharno.Text.Trim();
                        _BCEntity.AadharNo = MaskingAadhar(txtaadharno.Text.Trim());
                        _BCEntity.PanNo = txtPANNo.Text.Trim();
                        _BCEntity.GSTNo = txtGSTNo.Text.Trim();
                        _BCEntity.RegisteredAddress = txtRegisteredAddress.Text.Trim();
                        _BCEntity.BcCategory = ddlCategory.SelectedValue.ToString().Trim();
                        _BCEntity.PersonalContact = txtContactNo.Text.Trim();
                        _BCEntity.LandlineContact = txtLandlineNo.Text.Trim();
                        _BCEntity.RoleID = (int)EnumCollection.EnumPermissionType.EnableRoles;
                        _BCEntity.AccountNumber = txtAccountNumber.Text.ToString();
                        _BCEntity.IFSCCode = txtIFsccode.Text.Trim().ToString();
                        _BCEntity.Country = ddlCountry.SelectedValue.ToString().Trim();
                        _BCEntity.State = ddlState.SelectedValue.ToString().Trim();
                        _BCEntity.City = ddlCity.SelectedValue.ToString().Trim();
                        _BCEntity.District = ddlDistrict.SelectedValue.ToString().Trim();
                        _BCEntity.TypeOfOrg = DDlOrg.SelectedValue.ToString();
                        _BCEntity.Pincode = txtPinCode.Text.Trim();
                        _BCEntity.AlternetNo = txtAlterNateNo.Text.Trim().ToString();
                        _BCEntity.ClientId = ddlbcCode.SelectedValue.ToString();
                        _BCEntity.bccode = ddlbcCode.SelectedValue.ToString();
                        _BCEntity.Gender = ddlGender.SelectedValue.ToString();
                        _BCEntity.Category = ddlCategory.SelectedValue.Trim();
                        _BCEntity.Education = ddleducation.SelectedValue.Trim();
                        _BCEntity.CreatedBy = Session["Username"].ToString();
                        _BCEntity.Flag = 1;
                        _BCEntity.IsAPIEnable = 1;
                        _BCEntity.ClientId = Session["Client"].ToString();
                        _BCEntity.Activity = ((int)EnumCollection.ActionType.Onboard).ToString();
                        _BCEntity.BCReqId = HidBCID.Value != null && !string.IsNullOrEmpty(HidBCID.Value) ? Convert.ToString(HidBCID.Value) : "0";
                        // _BCEntity.BCReqId = HidBCID.Value;
                        // string _status = _BCEntity.SetInsertUpdateBCDetails(out string _statusmsg, out string RequestId);
                        if (_BCEntity.Insert_aggregatorRequest(Session["UserName"].ToString(), out int RequestId, out string _status, out string _statusmsg))
                        {
                            #region Audit
                            _auditParams[0] = Session["Username"].ToString();
                            _auditParams[1] = "Aggregator-Registration";
                            _auditParams[2] = "btnSubmitDetails";
                            _auditParams[3] = Session["LoginKey"].ToString();
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            #endregion
                            ErrorLog.AggregatorTrace("AggRegistration: btnSubmitDetails_Click: Failed - Aggregator Registration Request Dump In DB. UserName: " + UserName + " Status: " + _status + " StatusMsg: " + _statusmsg + " RequestId: " + RequestId);
                            DivBcDetails.Visible = true;
                            _BCEntity.BCCode = RequestId.ToString();
                            HidBCID.Value = RequestId.ToString();
                            div_Upload.Visible = true;
                            DivBcDetails.Visible = false;
                            DIVDetails.Visible = false;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Processed for Document Upload.', 'Alert');", true);
                        }
                        else
                        {
                            ErrorLog.AggregatorTrace("AggRegistration: btnSubmitDetails_Click: Failed - Aggregator Registration Request Dump In DB. UserName: " + UserName + " Status: " + _status + " StatusMsg: " + _statusmsg + " RequestId: " + RequestId);
                            ClearAllControls();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + _statusmsg + "', 'Alert');", true);
                            return;
                        }
                    }
                }
                ErrorLog.AggregatorTrace("AggregatorRegistration | btnSubmitDetails_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : btnSubmitDetails_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }
        protected void btnExportCSV_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | btnExportCSV() | Strated. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);



                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Aggregator-Registration";
                    _auditParams[2] = "Export-To-CSV";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "Aggregator Registration Details", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Page : AggRegistration.cs \nFunction : btnExportCSV_Click\nException Occured\n" + Ex.Message);
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
                ErrorLog.AggregatorTrace(Ex.Message);
            }
            return pageFilters;
        }

        protected void btndownload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | btndownload_Click() | Strated. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Aggregator-Registration";
                _auditParams[2] = "Export-To-Excel";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.BindGrid);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Aggregator-Registration";
                    _auditParams[2] = "Export-To-Excel";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "Aggregator Registration Details", dt);
                }
                {
                    //lblRecordCount.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('No data found.', 'Alert');", true);


                }

            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Page : AggRegistration.cs \nFunction : btndownload_Click\nException Occured\n" + Ex.Message);
            }
        }
        public bool AreContactNumbersDifferent(string contactNo, string alternateNo)
        {
            return contactNo != alternateNo;
        }
        #region Manual Insert Validations and Set Properties
        private bool ValidateSetProperties()
        {
            _CustomeRegExpValidation = new clsCustomeRegularExpressions();
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | ValidateSetProperties() | Strated. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                string contactNo = txtContactNo.Text;
                string alternateNo = txtAlterNateNo.Text;

                if (!AreContactNumbersDifferent(contactNo, alternateNo))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact Number and Alternate Number must be different.', 'First Name');", true);
                    return false; // Stop further processing
                }
                // ClientName
                if (hd_txtFirstName.Value == "1" || !string.IsNullOrEmpty(txtFirstName.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithoutSpace, txtFirstName.Text))
                    {
                        txtFirstName.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid First Name.', 'First Name');", true);
                        return false;
                    }
                    else _BCEntity.FileName = txtFirstName.Text.ToString();


                // PanNo
                if (hd_txtPANNo.Value == "1" || !string.IsNullOrEmpty(txtPANNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.PanCard, txtPANNo.Text))
                    {
                        txtPANNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid PAN Card.', ' PAN Card');", true);
                        return false;
                    }
                    else _BCEntity.PanNo = txtPANNo.Text.Trim();


                // AccountNumber
                if (hd_txtAccountNumber.Value == "1" || !string.IsNullOrEmpty(txtAccountNumber.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.AccountNumber, txtAccountNumber.Text))
                    {
                        txtAccountNumber.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid Account Number.', 'AccountNumber');", true);
                        return false;
                    }
                    else _BCEntity.AccountNumber = txtAccountNumber.Text.ToString();

                // IFSCCode
                if (hd_txtIFsccode.Value == "1" || !string.IsNullOrEmpty(txtIFsccode.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.IFSC, txtIFsccode.Text))
                    {
                        txtIFsccode.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid IFSC Code.', 'IFSC Code');", true);
                        return false;
                    }
                    else _BCEntity.IFSCCode = txtIFsccode.Text.Trim().ToString();

                // Registered Address
                if (hd_txtRegisteredAddress.Value == "1" || !string.IsNullOrEmpty(txtRegisteredAddress.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Address, txtRegisteredAddress.Text))
                    {
                        txtRegisteredAddress.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid Address.', 'Registered Address');", true);
                        return false;
                    }
                    else _BCEntity.RegisteredAddress = txtRegisteredAddress.Text.Trim();

                // Pincode
                if (hd_txtPinCode.Value == "1" || !string.IsNullOrEmpty(txtPinCode.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Pincode, txtPinCode.Text))
                    {
                        txtPinCode.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid Pincode.', 'Pincode');", true);

                        return false;
                    }
                    else _BCEntity.Pincode = txtPinCode.Text.Trim();

                // Country
                //if (ddlCountry.SelectedValue == "0")
                //{
                //    ddlCountry.Focus();
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                //    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Please Select Country.', 'Country');", true);
                //    return false;
                //}
                //else _BCEntity.Country = ddlCountry.SelectedValue.ToString().Trim();

                // State
                if (ddlState.SelectedValue == "0")
                {
                    ddlState.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Please Select State.', 'State');", true);
                    return false;
                }
                else _BCEntity.State = ddlState.SelectedValue.ToString().Trim();

                // City
                if (ddlCity.SelectedValue == "0")
                {
                    ddlCity.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Please Select City.', 'City');", true);
                    return false;
                }
                else _BCEntity.City = ddlCity.SelectedValue.ToString().Trim();

                //District
                if (ddlDistrict.SelectedValue == "0")
                {
                    ddlDistrict.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select District.', 'District');", true);
                    return false;
                }
                else _BCEntity.City = ddlDistrict.SelectedValue.ToString().Trim();



                // Personal Contact
                if (hd_txtContactNo.Value == "1" || !string.IsNullOrEmpty(txtContactNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, txtContactNo.Text))
                    {
                        txtContactNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid Contact No.', 'Personal Contact No.');", true);
                        return false;
                    }
                    else _BCEntity.PersonalContact = txtContactNo.Text.Trim();


                // Alternate Mobile No
                if (hd_txtAlterNateNo.Value == "1" || !string.IsNullOrEmpty(txtAlterNateNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, txtAlterNateNo.Text))
                    {
                        txtAlterNateNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid Mobile No.', 'Alternate Mobile No.');", true);
                        return false;
                    }
                    else _BCEntity.AlternetNo = txtAlterNateNo.Text.Trim().ToString();

                if (chkAEPS.Checked == false && chkMATM.Checked == false && chkdmt.Checked == false)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Please Select Atleast One Service','Warning');", true);
                    return false;
                }
                ErrorLog.AggregatorTrace("AggregatorRegistration | ValidateSetProperties() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.AggregatorTrace("Page : AggRegistration.cs \nFunction : ValidateSetProperties\nException Occured\n" + ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());

                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong..! Please try again',' Aggregator Registration');", true);
                return false;
            }
            return true;
        }
        #endregion


        #region Bind Dropdown
        public void BindDropdownCountryState()
        {
            try
            {
                _BCEntity.mode = (int)(EnumCollection.StateCityMode.PinCode);
                DataSet dsCountry = _BCEntity.GetCountryStateCityD();
                if (dsCountry != null && dsCountry.Tables.Count > 0 && dsCountry.Tables[0].Rows.Count > 0)
                {
                    ddlCountry.DataSource = dsCountry;
                    ddlCountry.DataValueField = "Country";
                    ddlCountry.DataTextField = "Country";
                    ddlCountry.DataBind();

                    ddlState.DataSource = dsCountry;
                    ddlState.DataValueField = "States";
                    ddlState.DataTextField = "States";
                    ddlState.DataBind();

                    ddlDistrict.DataSource = dsCountry;
                    ddlDistrict.DataValueField = "District";
                    ddlDistrict.DataTextField = "District";
                    ddlDistrict.DataBind();

                    ddlCity.DataSource = dsCountry;
                    ddlCity.DataValueField = "City";
                    ddlCity.DataTextField = "City";
                    ddlCity.DataBind();
                }
                else
                {
                    _BCEntity.mode = (int)(EnumCollection.StateCityMode.ALLStateCity);
                    DataSet ds = _BCEntity.GetCountryStateCityD();

                    ddlCountry.DataSource = ds.Tables[0];
                    ddlCountry.DataValueField = "Country";
                    ddlCountry.DataTextField = "Country";
                    ddlCountry.DataBind();
                    ddlCountry.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlState.DataSource = ds.Tables[1];
                    ddlState.DataValueField = "States";
                    ddlState.DataTextField = "States";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlDistrict.DataSource = ds.Tables[2];
                    ddlDistrict.DataValueField = "District";
                    ddlDistrict.DataTextField = "District";
                    ddlDistrict.DataBind();
                    ddlDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlCity.DataSource = ds.Tables[3];
                    ddlCity.DataValueField = "City";
                    ddlCity.DataTextField = "City";
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AggregatorRegistration: BindDropdownCountryState(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }            
        }
        #endregion

        #region FillGrid
        public DataSet FillGrid(EnumCollection.EnumBindingType enumBinding)
        {
            DataSet ds = null;
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                _BCEntity.BCID = Session["Username"].ToString();
                // _BCEntity.= ddlBucketId.SelectedValue=="1" && ddlRequestStatus.SelectedValue=="0"?
                _BCEntity.Flag = (int)enumBinding;
                setProperties();

                ds = _BCEntity.GetAggregatorRequestList();

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
                ErrorLog.AggregatorTrace("AggregatorRegistration | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : FillGrid() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "showError('Something went wrong.Please try again','Warning');", true);
            }
            return ds;
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
                //if (ddlRequestStatus.SelectedValue == "0")
                //{
                //    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                //}

                //else if ( ddlRequestStatus.SelectedValue == "1")
                //{
                //    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                //}

                //else if (ddlRequestStatus.SelectedValue == "2")
                //{
                //    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerDecline);
                //}

                //else if (ddlRequestStatus.SelectedValue == "0")
                //{
                //    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerPending);
                //}

                //else if (ddlRequestStatus.SelectedValue == "1")
                //{
                //    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerApprove);
                //}

                //else if (ddlRequestStatus.SelectedValue == "2")
                //{
                //    _BCEntity.ATStatus = Convert.ToString((int)EnumCollection.Onboarding.AuthorizerDecline);
                //}
                //if (ddlRequestStatus.SelectedValue == "0")
                //{
                //    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerPending);
                //}

                //else if ( ddlRequestStatus.SelectedValue == "1")
                //{
                //    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerApprove);
                //}

                //else if (ddlRequestStatus.SelectedValue == "2")
                //{
                //    _BCEntity.CHstatus = Convert.ToString((int)EnumCollection.Onboarding.CheckerDecline);
                //}

                if (ddlRequestStatus.SelectedValue == "0")
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
                ErrorLog.AggregatorTrace("Page : AggRegistration.cs \nFunction : setProperties() \nException Occured\n" + EX.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }

        #endregion
        
        #region SelectedIndexChanged
        //protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        _BCEntity.Mode = "getStateAndCity";
        //        _BCEntity.Country = ddlCountry.SelectedValue.ToString();
        //        DataSet ds_Country = _BCEntity.getStateAndCity();
        //        if (ds_Country != null && ds_Country.Tables.Count > 0 && ds_Country.Tables[0].Rows.Count > 0)
        //        {
        //            ddlState.DataSource = ds_Country.Tables[0];
        //            ddlState.DataValueField = "ID";
        //            ddlState.DataTextField = "Name";
        //            ddlState.DataBind();
        //            ddlState.Items.Insert(0, new ListItem("-- Select --", "0"));
        //        }
        //        else
        //        {
        //            ddlState.DataSource = null;
        //            ddlState.DataBind();
        //            ddlState.Items.Insert(0, new ListItem("No Data Found", "0"));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : ddlCountry_SelectedIndexChanged() \nException Occured\n" + Ex.Message);

        //        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
        //    }
        //}

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlDistrict.DataSource = null;

                _BCEntity.State = string.Empty;
                _BCEntity.mode = (int)EnumCollection.StateCityMode.District;
                _BCEntity.State = ddlState.SelectedValue.ToString();
                //DataSet ds_District = _AggregatorRegistrationDAL.GetCountryStateCity(UserName, Mode, 0, Convert.ToInt32(_AggregatorRegistrationDAL.AgentState));
                DataSet ds_District = _BCEntity.GetCountryStateCityD();
                if (ds_District != null && ds_District.Tables.Count > 0 && ds_District.Tables[0].Rows.Count > 0)
                {
                    ddlDistrict.DataSource = ds_District;
                    ddlDistrict.DataValueField = "District";
                    ddlDistrict.DataTextField = "District";
                    ddlDistrict.DataBind();
                    ddlDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    ddlDistrict.DataSource = null;
                    ddlDistrict.DataBind();
                    ddlDistrict.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AggregatorRegistration: ddlState_SelectedIndexChanged(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlCity.DataSource = null;
                ddlCity.SelectedValue = null;
                ddlCity.ClearSelection();

                _BCEntity.District = string.Empty;
                _BCEntity.mode = (int)EnumCollection.StateCityMode.City;
                _BCEntity.District = ddlDistrict.SelectedValue.ToString();
                //DataSet ds_District = _AggregatorRegistrationDAL.GetCountryStateCity(UserName, Mode, 0, Convert.ToInt32(_AggregatorRegistrationDAL.AgentState));
                DataSet ds_City = _BCEntity.GetCountryStateCityD();
                if (ds_City != null && ds_City.Tables.Count > 0 && ds_City.Tables[0].Rows.Count > 0)
                {
                    //ddlCity.DataSource = ds_State;
                    //ddlCity.DataValueField = "ID";
                    //ddlCity.DataTextField = "Name";
                    //ddlCity.DataBind();
                    //ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                    ddlCity.DataSource = ds_City;
                    ddlCity.DataValueField = "City";
                    ddlCity.DataTextField = "City";
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    ddlCity.DataSource = null;
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AggregatorRegistration.cs \nFunction : ddlDistrict_SelectedIndexChanged() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.', 'Alert');", true);
            }
        }
        #endregion

        #region ClearControl
        public void ClearAllControls()
        {
            try
            {
                txtFirstName.Text = null;
                txtMiddleName.Text = null;
                txtLastName.Text = null;
                txtEmailID.Text = null;
                txtPinCode.Text = null;
                txtFatherName.Text = null;
                txtPANNo.Text = null;
                txtGSTNo.Text = null;
                txtRegisteredAddress.Text = null;
                txtContactNo.Text = null;
                txtLandlineNo.Text = null;
                txtaadharno.Text = null;
                ddlCountry.SelectedValue = null;
                ddlState.SelectedValue = null;
                ddlCity.SelectedValue = null;
                //flgUplodMyImage = null;
                txtAccountNumber.Text = null;
                ddlDistrict.SelectedValue = null;
                txtIFsccode.Text = null;
                txtLandlineNo.Text = null;
                txtAlterNateNo.Text = null;
                dvfield_PANNo.Visible = true;
                txtaadharno.Text = null;
                chkAEPS.Checked = false;
                chkMATM.Checked = false;
                ddlbcCode.SelectedValue = null;
                ddlCategory.ClearSelection();
                ddleducation.ClearSelection();

            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : ClearAllControls() \nException Occured\n" + Ex.Message);
            }
        }
        #endregion


        protected void Bindreceipt()
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | Bindreceipt() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                DataSet ds_Receipt = _BCEntity.getAggReceiptData();
                if (ds_Receipt != null && ds_Receipt.Tables.Count > 0 && ds_Receipt.Tables[0].Rows.Count > 0)
                {
                    txtaadh.Text = ds_Receipt.Tables[0].Rows[0]["AadharNo"].ToString();
                    txtacc.Text = ds_Receipt.Tables[0].Rows[0]["AccountNumber"].ToString();
                    txtadd.Text = ds_Receipt.Tables[0].Rows[0]["aggAddress"].ToString();
                    txtbcname.Text = ds_Receipt.Tables[0].Rows[0]["aggName"].ToString();
                    //  string dfdf = ds_Receipt.Tables[0].Rows[0]["BCCategory"].ToString();
                    DDLcat.Text = ds_Receipt.Tables[0].Rows[0]["aggCategory"].ToString();
                    ddlcl.Text = ds_Receipt.Tables[0].Rows[0]["ClientName"].ToString();
                    ddlcitys.Text = ds_Receipt.Tables[0].Rows[0]["LocalCity"].ToString();
                    txtcontact.Text = ds_Receipt.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtlandline.Text = ds_Receipt.Tables[0].Rows[0]["LandlineNo"].ToString();
                    ddlcountrys.Text = ds_Receipt.Tables[0].Rows[0]["LocalCountry"].ToString();
                    ddlstates.Text = ds_Receipt.Tables[0].Rows[0]["LocalState"].ToString();
                    ddldist.Text = ds_Receipt.Tables[0].Rows[0]["LocalDistrict"].ToString();
                    txtemail.Text = ds_Receipt.Tables[0].Rows[0]["EmailID"].ToString();
                    DDlgen.Text = ds_Receipt.Tables[0].Rows[0]["Gender"].ToString();
                    txtgst.Text = ds_Receipt.Tables[0].Rows[0]["GSTNo"].ToString();
                    txtpan.Text = ds_Receipt.Tables[0].Rows[0]["PanNo"].ToString();
                    txtpin.Text = ds_Receipt.Tables[0].Rows[0]["PinCode"].ToString();
                    DDlOrgn.Text = ds_Receipt.Tables[0].Rows[0]["TypeOfOrg"].ToString();
                    txtifsc.Text = ds_Receipt.Tables[0].Rows[0]["IFSCCode"].ToString();
                    // txtDistrict.Text= ds_Receipt.Tables[0].Rows[0]["LocalDistrict"].ToString();  + "_Thumbnail";
                    string idthumb = ds_Receipt.Tables[0].Rows[0]["IdentityProofDocument"].ToString();
                    //string  FileThumbnailId = Path.GetDirectoryName(idthumb) + "\\"+Path.GetFileNameWithoutExtension(idthumb) + "_Thumbnail" + Path.GetExtension(idthumb);
                    string FileThumbnailId = Path.GetDirectoryName(idthumb) + "\\" + Path.GetFileNameWithoutExtension(idthumb) + "_Thumbnail.png";
                    pathId = "../../" + FileThumbnailId;
                    Session["pdfPathID"] = idthumb;

                    string Addthumb = ds_Receipt.Tables[0].Rows[0]["AddressProofDocument"].ToString();
                    string FileThumbnailAdd = Path.GetDirectoryName(Addthumb) + "\\" + Path.GetFileNameWithoutExtension(Addthumb) + "_Thumbnail.png";
                    PathAdd = "../../" + FileThumbnailAdd;
                    Session["pdfPathAdd"] = Addthumb;

                    string Sigthumb = ds_Receipt.Tables[0].Rows[0]["SignatureProofDocument"].ToString();
                    string FileThumbnailSig = Path.GetDirectoryName(Sigthumb) + "\\" + Path.GetFileNameWithoutExtension(Sigthumb) + "_Thumbnail.png";
                    PathSig = "../../" + FileThumbnailSig;
                    Session["pdfPathSig"] = Sigthumb;

                    ViewState["BCReqId"] = ds_Receipt.Tables[0].Rows[0]["BCReqID"].ToString();
                }
                ErrorLog.AggregatorTrace("AggregatorRegistration | Bindreceipt() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : Bindreceipt() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | BtnSubmit_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (HiddenField1.Value.ToString() == "Yes" && HidBCID.Value != null && !string.IsNullOrEmpty(HidBCID.Value))
                {

                        _BCEntity.Flag = (int)EnumCollection.DBFlag.Update;
                        _BCEntity.ClientId = ddlbcCode.SelectedValue.ToString();
                        _BCEntity.CreatedBy = Session["Username"].ToString();
                        _BCEntity.IdentityProofDocument = Session["IdFilePath"].ToString();

                        _BCEntity.IdentityProofType = ddlIdentityProof.SelectedValue;
                        _BCEntity.AddressProofDocument = Session["AddFilePath"].ToString();
                        _BCEntity.AddressProofType = ddlAddressProof.SelectedValue;
                        _BCEntity.SignatureProofDocument = Session["SigFilePath"].ToString();
                        _BCEntity.SignatureProofType = ddlSignature.SelectedValue;
                        _BCEntity.BCReqId = HidBCID.Value;

                    //string _status = _BCEntity.Insert_BCRequest(Session["UserName"], out string RequestId, out string _statusmsg);

                    if (_BCEntity.Insert_aggregatorRequest(Session["UserName"].ToString(), out int RequestId, out string _status, out string _statusmsg))
                    {
                        ErrorLog.AggregatorTrace("AggRegistration: BtnSubmit_Click: Successful - Upload Documents. UserName: " + UserName + " Status: " + _status + " StatusMsg: " + _statusmsg + " RequestId: " + RequestId);
                        DivBcDetails.Visible = true;
                        _BCEntity.BCCode = RequestId.ToString();
                        HidBCID.Value = RequestId.ToString();
                        Bindreceipt();
                        div_Upload.Visible = false;
                        DivBcDetails.Visible = true;
                        DIVDetails.Visible = false;
                        Session["IdFilePath"] = string.Empty;
                        Session["AddFilePath"] = string.Empty;
                        Session["SigFilePath"] = string.Empty;

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Uploaded Successfully', ' Aggregator Registration');", true);
                    }
                    else
                    {
                        ErrorLog.AggregatorTrace("AggRegistration: BtnSubmit_Click: Failed - Upload Documents. UserName: " + UserName + " Status: " + _status + " StatusMsg: " + _statusmsg + " RequestId: " + RequestId);
                        ClearAllControls();
                        Session["IdFilePath"] = string.Empty;
                        Session["IdFileName"] = string.Empty;
                        Session["AddFilePath"] = string.Empty;
                        Session["AddFileName"] = string.Empty;
                        Session["SigFilePath"] = string.Empty;
                        Session["SigFileName"] = string.Empty;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + _statusmsg + "', 'Alert');", true);
                        return;
                    }
                }
                
                else
                {
                    ErrorLog.AggregatorTrace("AggRegistration: BtnSubmit_Click: Failed - Upload Documents. User Confirmation Or RequestId Are Empty. UserName: " + UserName);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again later', 'Alert');", true);
                    return;
                }
                ErrorLog.AggregatorTrace("AggregatorRegistration | BtnSubmit_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : btnSubmitDetails_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }

        protected void btnCloseReceipt_Click(object sender, EventArgs e)
        {
            ErrorLog.AggregatorTrace("AggregatorRegistration | btnCloseReceipt_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            if (HiddenField1.Value.ToString() == "Yes")
            {
                DIVDetails.Visible = true;
                divAction.Visible = false;
                divMainDetailsGrid.Visible = false;
                DivBcDetails.Visible = false;
                ddlSignature.SelectedValue = null;
                ddlAddressProof.ClearSelection();
                ddlIdentityProof.ClearSelection();
            }
            else
            {

            }
            ErrorLog.AggregatorTrace("AggregatorRegistration | btnCloseReceipt_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
        }

        #region SaveFile IdentityProof
        private string SaveFile(HttpPostedFile fileUpload, string id)
        {
            try
            {
                string PathLocation = ConfigurationManager.AppSettings["BcDocumentPath"].ToString();
                string FinalPathLocation = PathLocation + "\\" + id + "\\" + "IdentityProof" + "\\";
                if (!Directory.Exists(FinalPathLocation))
                {
                    Directory.CreateDirectory(FinalPathLocation);
                }
                FinalPathLocation += fileUpload.FileName;
                if (File.Exists(FinalPathLocation))
                {
                    File.Delete(FinalPathLocation);
                    fileUpload.SaveAs(FinalPathLocation);
                }
                else
                {
                    fileUpload.SaveAs(FinalPathLocation);
                }
                return FinalPathLocation;
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : SaveFile() \nException Occured\n" + Ex.Message);
                return string.Empty; ;
            }
        }
        #endregion
        #region SaveFile AddressProof
        private string SaveFileAddprof(HttpPostedFile fileUpload, string id)
        {
            try
            {
                string PathLocation = ConfigurationManager.AppSettings["BcDocumentPath"].ToString();
                string FinalPathLocation = PathLocation + "\\" + id + "\\" + "AddressProof" + "\\";
                if (!Directory.Exists(FinalPathLocation))
                {
                    Directory.CreateDirectory(FinalPathLocation);
                }
                FinalPathLocation += fileUpload.FileName;
                if (File.Exists(FinalPathLocation))
                {
                    File.Delete(FinalPathLocation);
                    fileUpload.SaveAs(FinalPathLocation);
                }
                else
                {
                    fileUpload.SaveAs(FinalPathLocation);
                }
                return FinalPathLocation;
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : SaveFile() \nException Occured\n" + Ex.Message);
                return string.Empty; ;
            }
        }
        #endregion
        #region SaveFile SignatureProof
        private string SaveFileSignproof(HttpPostedFile fileUpload, string id)
        {
            try
            {
                string PathLocation = ConfigurationManager.AppSettings["BcDocumentPath"].ToString();
                string FinalPathLocation = PathLocation + "\\" + id + "\\" + "SignatureProof" + "\\";
                if (!Directory.Exists(FinalPathLocation))
                {
                    Directory.CreateDirectory(FinalPathLocation);
                }
                FinalPathLocation += fileUpload.FileName;
                if (File.Exists(FinalPathLocation))
                {
                    File.Delete(FinalPathLocation);
                    fileUpload.SaveAs(FinalPathLocation);
                }
                else
                {
                    fileUpload.SaveAs(FinalPathLocation);
                }
                return FinalPathLocation;
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : SaveFile() \nException Occured\n" + Ex.Message);
                return string.Empty; ;
            }
        }
        #endregion

        protected bool ValidateFile(HttpPostedFile _FileUpload, string _TypeOfDocument)
        {
            int FileExtensionsCount = 0;
            string _fileExtension = string.Empty;
            string _fileName = string.Empty;
            try
            {
                FileExtensionsCount = _FileUpload.FileName.Split('.').Length - 1;
                if (FileExtensionsCount > 1)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Please Upload Valid File with Original extension.','" + _TypeOfDocument + "');", true);
                    return false;
                }
                _fileExtension = Path.GetExtension(_FileUpload.FileName);
                if (_fileExtension.ToLower() != ".jpg" && _fileExtension.ToLower() != ".jpeg" && _fileExtension.ToLower() != ".png" && _fileExtension.ToLower() != ".pdf")
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid File Format. Only .jpg/.png/.jpeg/.pdf formats are allowed.', '" + _TypeOfDocument + "');", true);
                    return false;
                }
                long fileSize = _FileUpload.ContentLength;
                if (_FileUpload.ContentLength <= 0)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid File Size.Should Not Be Empty','" + _TypeOfDocument + "');", true);
                    return false;
                }
                if (_fileExtension.ToLower() != ".pdf" && _FileUpload.ContentLength > 1000000)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid File Size.Should be less than 1 MB.','" + _TypeOfDocument + "');", true);
                    return false;
                }
                else if (_fileExtension.ToLower() == ".pdf" && _FileUpload.ContentLength > 2000000)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid File Size.Should be less than 2 MB.','" + _TypeOfDocument + "');", true);
                    return false;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : ValidateFile \nException Occured\n" + Ex.Message);
                _IsValidFileAttached = false;
                return false;
            }
            return true;
        }

        protected bool ValidateFiles(string _TypeOfDocument)
        {
            HttpPostedFile postedFile = Request.Files["flgUplodMyIdProof"];
            int FileExtensionsCount = 0;
            string _fileExtension = string.Empty;
            string _fileName = string.Empty;
            try
            {
                FileExtensionsCount = postedFile.FileName.Split('.').Length - 1;
                if (FileExtensionsCount > 1)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Please Upload Valid File with Original extension.','" + _TypeOfDocument + "');", true);
                    return false;
                }
                _fileExtension = Path.GetExtension(postedFile.FileName);
                if (_fileExtension.ToLower() != ".jpg" && _fileExtension.ToLower() != ".jpeg" && _fileExtension.ToLower() != ".png" && _fileExtension.ToLower() != ".pdf")
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid File Format. Only .jpg/.png/.jpeg/.pdf formats are allowed.', '" + _TypeOfDocument + "');", true);
                    return false;
                }
                long fileSize = postedFile.ContentLength;
                if (postedFile.ContentLength <= 0)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid File Size.Should Not Be Empty','" + _TypeOfDocument + "');", true);
                    return false;
                }
                if (_fileExtension.ToLower() != ".pdf" && postedFile.ContentLength > 1000000)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid File Size.Should be less than 1 MB.','" + _TypeOfDocument + "');", true);
                    return false;
                }
                else if (_fileExtension.ToLower() == ".pdf" && postedFile.ContentLength > 2000000)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Invalid File Size.Should be less than 2 MB.','" + _TypeOfDocument + "');", true);
                    return false;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : ValidateFile \nException Occured\n" + Ex.Message);
                _IsValidFileAttached = false;
                return false;
            }
            return true;
        }

        #region DeleteFile
        private void DeleteFile(string FilePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(FilePath))
                {
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                    }
                }
            }
            catch (Exception EX)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : DeleteFile() \nException Occured\n" + EX.Message);
            }
        }
        #endregion

        #region IsValidImage
        private bool IsValidImage(string filename, string FileExtension)
        {
            try
            {
                if ((FileExtension.Contains(".Jpeg")) || (FileExtension.Contains(".Png")))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(filename))
                    {
                        if ((img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg)) || (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png)))
                            return true;
                        else
                            return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (OutOfMemoryException ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : IsValidImage() \nException Occured\n" + ex.Message);
                return false;
            }
        }
        #endregion

        protected void ProcessBCData_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | ProcessBCData_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (HiddenField3.Value.ToString() == "Yes")
                {
                    if (ChkConfirmBC.Checked == true)
                    {
                        //div_Upload.Visible = true;
                        //DivBcDetails.Visible = false;

                        // DivBcDetails.Visible = true;
                        _BCEntity.CreatedBy = Session["Username"].ToString();
                        _BCEntity.BCReqId = HidBCID.Value.ToString();
                        _BCEntity.Flag = 1;
                        //DataSet dsBCMaster = _BCEntity.SetInsertUpdateBCTrackerDetails();
                        string statusMessage = _BCEntity.SetInsertUpdateaggregatorTrackerDetails();

                        if (!string.IsNullOrEmpty(statusMessage) && statusMessage == "Inserted")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Registered Successfully', ' Aggregator Registration');", true);
                            div_Upload.Visible = false;
                            divOnboardFranchise.Visible = false;
                            DivBcDetails.Visible = false;
                            DIVDetails.Visible = false;
                            divAction.Visible = true;
                            divMainDetailsGrid.Visible = true;
                            ClearAllControls();
                            Session["BCCode"] = string.Empty;
                            FillGrid(EnumCollection.EnumBindingType.BindGrid);
                        }
                        else if (statusMessage == "Updated")
                        {
                            // Handle the case when the record is updated
                        }
                        else
                        {
                            ClearAllControls();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Select Confirmation on all above Aggregator Deatils are properly filled', ' Aggregator Registration');", true);
                        return;
                    }
                }
                else
                {

                }
                ErrorLog.AggregatorTrace("AggregatorRegistration | ProcessBCData_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception EX)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : DeleteFile() \nException Occured\n" + EX.Message + " | LoginKey : " + Session["LoginKey"].ToString());
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | btnCancel_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                DIVDetails.Visible = false;
                div_Upload.Visible = false;
                btndownload.Visible = true;
                btnExportCSV.Visible = true;
                divOnboardFranchise.Visible = false;
                divAction.Visible = true;
                divMainDetailsGrid.Visible = true;
                ClearAllControls();
                HidBCID.Value = string.Empty;
                ErrorLog.AggregatorTrace("AggregatorRegistration | btnCancel_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                //////Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowgridDiv()", true);
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : btnCancel_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
            }
        }



        protected void btnAddnew_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | btnAddnew_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                divOnboardFranchise.Visible = true;
                divAction.Visible = false;
                divMainDetailsGrid.Visible = false;
                DIVDetails.Visible = true;
                FillBc();
                ErrorLog.AggregatorTrace("AggregatorRegistration | btnAddnew_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : btnAddnew_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
            }
        }


        #region View Download
        protected void btnViewDownload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                string[] commandArgs = Imgbtn.CommandArgument.ToString().Split(new char[] { '=' });
                string BCReqId = commandArgs[0];
                string doc = Convert.ToString(commandArgs[1]);
                _BCEntity.BCID = BCReqId;
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : btnViewDownload_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'Aggregator Verification');", true);
                return;
            }
        }

        protected void DownloadDocTwo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
                if (Ds.Tables[1].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                  //  string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    strURL = Ds.Tables[1].Rows[0]["AddressProofDocument"].ToString();
                    string FinalPath = filepath + strURL;
                    string fileName = Path.GetFileName(FinalPath);
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    byte[] data = req.DownloadData(FinalPath);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : ImageButton1_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', ' Aggregator Registration');", true);
                return;
            }
        }
        protected void DownloadDocThree_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
                if (Ds.Tables[2].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    //string fileName = Ds.Tables[2].Rows[0]["SignatureProofType"].ToString();
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    strURL = Ds.Tables[2].Rows[0]["SignatureProofDocument"].ToString();
                    string FinalPath = filepath + strURL;
                    string fileName = Path.GetFileName(FinalPath);
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    byte[] data = req.DownloadData(FinalPath);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : ImageButton2_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', ' Aggregator Registration');", true);
                return;
            }
        }

        protected void DownloadDocOne_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                   // string fileName = Ds.Tables[0].Rows[0]["IdentityProofType"].ToString();
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    strURL = Ds.Tables[0].Rows[0]["IdentityProofDocument"].ToString();
                    string FinalPath = filepath + strURL;
                    string fileName = Path.GetFileName(FinalPath);
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    byte[] data = req.DownloadData(FinalPath);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : ImageButton2_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', ' Aggregator Registration');", true);
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
                    byte[] bytes = (byte[])(Ds.Tables[3].Rows[0]["Documents"]);
                    string fileName = Ds.Tables[3].Rows[0]["DocumentType"].ToString(); //"BC_RegistrationForm";
                    string FileType = Ds.Tables[3].Rows[0]["DocumentType"].ToString();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = FileType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + FileType);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : imgbtnform_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', ' Aggregator Registration ');", true);
                return;
            }
        }


        #endregion


        public void GetDetailsBack(string val)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | GetDetailsBack() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                _BCEntity.BCRequest = val;

                DataSet ds = _BCEntity.GetOnboradingbcDetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["State"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["City"].ToString() != "")
                    {
                        ddlCity.SelectedValue = ds.Tables[0].Rows[0]["City"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["District"].ToString() != "")
                    {
                        ddlDistrict.SelectedValue = ds.Tables[0].Rows[0]["District"].ToString();
                    }
                   
                    //if (ds != null && ds.Tables[2].Rows.Count > 0)
                    //{
                    //    ddlState.DataSource = ds.Tables[2];
                    //    ddlState.DataValueField = "ID";
                    //    ddlState.DataTextField = "Name";
                    //    ddlState.DataBind();
                    //    ddlState.Items.Insert(0, new ListItem("-- Select --", "0"));
                    //}
                    //if (ds != null && ds.Tables[3].Rows.Count > 0)
                    //{
                    //    ddlCity.DataSource = ds.Tables[3];
                    //    ddlCity.DataValueField = "ID";
                    //    ddlCity.DataTextField = "Name";
                    //    ddlCity.DataBind();
                    //    ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                    //}
                    txtFirstName.Text = ds.Tables[0].Rows[0]["BCName"].ToString();
                    //txtMiddleName.Text = ds.Tables[0].Rows[0]["Middle"].ToString();
                    //txtLastName.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                    if (ds.Tables[0].Rows[0]["Gender"].ToString() != "")
                    {
                        ddlGender.SelectedValue = ds.Tables[0].Rows[0]["Gender"].ToString();
                    }
                    txtPANNo.Text = ds.Tables[0].Rows[0]["PanNo"].ToString();
                    txtGSTNo.Text = ds.Tables[0].Rows[0]["GSTNo"].ToString();
                    txtaadharno.Text = ds.Tables[0].Rows[0]["AadharNo"].ToString();
                    if (ds.Tables[0].Rows[0]["BCCategory"].ToString() != "")
                    {
                        ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["BCCategory"].ToString();
                    }
                    txtAccountNumber.Text = ds.Tables[0].Rows[0]["AccountNumber"].ToString();
                    txtIFsccode.Text = ds.Tables[0].Rows[0]["IFSCCode"].ToString();
                    // txtPass.Text = ds.Tables[0].Rows[0]["PassportNo"].ToString();
                    if (ds.Tables[0].Rows[0]["TypeOfOrg"].ToString() != "")
                    {
                        DDlOrg.SelectedValue = ds.Tables[0].Rows[0]["TypeOfOrg"].ToString();
                    }
                    txtRegisteredAddress.Text = ds.Tables[0].Rows[0]["BCAddress"].ToString();
                    if (ds.Tables[0].Rows[0]["Country"].ToString() != "")
                    {
                        ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["State"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["District"].ToString() != "")
                    {
                        ddlDistrict.SelectedValue = ds.Tables[0].Rows[0]["District"].ToString();
                    }
                   // txtDistrict.Text = ds.Tables[0].Rows[0]["District"].ToString();
                    if (ds.Tables[0].Rows[0]["City"].ToString() != "")
                    {
                        ddlCity.SelectedValue = ds.Tables[0].Rows[0]["City"].ToString();
                    }
                    txtPinCode.Text = ds.Tables[0].Rows[0]["PinCode"].ToString();
                    txtEmailID.Text = ds.Tables[0].Rows[0]["EmailID"].ToString();
                    txtcontact.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtlandline.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();
                    txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtLandlineNo.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();
                    if (ds.Tables[0].Rows[0]["AEPS"].ToString() != "" || ds.Tables[0].Rows[0]["AEPS"].ToString() != null)
                    {
                        //  HdnAEPS.Value = ds.Tables[0].Rows[0]["AEPS"].ToString();
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
                        //HdnMATM.Value = ds.Tables[0].Rows[0]["MATM"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["MATM"].ToString() == "1")
                    {
                        chkMATM.Checked = true;
                    }
                    else
                    {
                        chkMATM.Checked = false;
                    }
                    DIVDetails.Visible = true;
                }
                else
                {
                    //clearselection();
                }
                ErrorLog.AggregatorTrace("AggregatorRegistration | GetDetailsBack() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorError(Ex);
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : GetDetailsBack() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration | BtnBack_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                //Response.Redirect("~/Pages/bc/BCRegistration.aspx", false);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                DIVDetails.Visible = true;
                div_Upload.Visible = false;
                DivBcDetails.Visible = false;
                string val = HidBCID.Value;
                GetDetailsBack(val);

                ErrorLog.AggregatorTrace("AggregatorRegistration | BtnBack_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception EX)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : BtnBack_Click() \nException Occured\n" + EX.Message + " | LoginKey : " + Session["LoginKey"].ToString());
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("btnSearch_Click | BtnBack_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                //if (ddlBucketId.SelectedValue != "-1" && ddlRequestStatus.SelectedValue == "-1")
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Please Select Request Status', ' Aggregator Registration');", true);
                //    return;
                //}
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Aggregator-Registration";
                _auditParams[2] = "btnSearch";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                ErrorLog.AggregatorTrace("btnSearch_Click | BtnBack_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggRegistration.cs \nFunction : btnSearch_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }

        protected void butnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.AggregatorTrace("btnSearch_Click | butnCancel_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                ddlRequestType.SelectedValue = null;
                //ddlBucketId.SelectedValue = null;
                ddlRequestStatus.SelectedValue = null;

                txtPanNoF.Text = string.Empty;
                //txtAadharNoF.Value = string.Empty;
                //txtGSTNoF.Value = string.Empty;
                //txtContactNoF.Value = string.Empty;
                //txtPersonalEmailIDF.Value = string.Empty;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                ErrorLog.AggregatorTrace("btnSearch_Click | butnCancel_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Aggregator-Registration";
                _auditParams[2] = "butnCancel";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggregatorRegistration.cs \nFunction : butnCancel_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }

        protected void gvBCOnboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBCOnboard.PageIndex = e.NewPageIndex;
            FillGrid(EnumCollection.EnumBindingType.BindGrid);
        }

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


        protected void gvBCOnboard_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Contains("EditDetails"))
                {
                    try
                    {
                        ErrorLog.AggregatorTrace("btnSearch_Click | RowCommand-EditDetails | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        ImageButton lb = (ImageButton)e.CommandSource;
                        GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                        string BCRequestId = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["ReqId"]).ToString();
                        string reqtype = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["ActivityType"]).ToString();
                        string reqstatus = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["RequestStatus"]).ToString();
                        string bucket = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["Bucket"]).ToString();
                        _BCEntity.BCReqId = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["ReqId"]).ToString();
                        if (ValidateReEditRequest())
                        {
                            #region Audit
                            _auditParams[0] = Session["Username"].ToString();
                            _auditParams[1] = "Aggregator-Registration";
                            _auditParams[2] = "RowCommand-EditDetails";
                            _auditParams[3] = Session["LoginKey"].ToString();
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            #endregion
                            int BCReqID = Convert.ToInt32(BCRequestId);
                            HidBCID.Value = BCRequestId;

                            if (bucket.ToLower() == "self")
                            {
                                Response.Redirect("../../Pages/Aggregator/ReprocessAggDetails.aspx?BCReqID=" + BCReqID + "&" + "RequestType=" + reqtype + " ", false);
                            }
                            else if ((reqtype == "0" ) && reqstatus.ToLower() == "declined")
                            {
                                int IsSelfData = 0;
                                Response.Redirect("../../Pages/Aggregator/EditAggRegistrationDetails.aspx?BCReqId=" + _BCEntity.BCReqId + "&" + "RequestType=" + reqtype + "&" + "IsSelfData=" + IsSelfData + " ", false);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Agent request already pending for verification.', 'Warning');", true);
                            return;
                        }
                        ErrorLog.AggregatorTrace("btnSearch_Click | RowCommand-EditDetails | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.CommonTrace("Class : AggregatorRegistration.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                        throw Ex;
                    }
                }
                if (e.CommandName.Contains("DeleteDetails"))
                {
                    if (hdnDeleteConfirmation.Value.ToString() == "Yes")
                    {
                        try
                        {
                            ErrorLog.AggregatorTrace("btnSearch_Click | RowCommand-DeleteDetails | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                            ImageButton lb = (ImageButton)e.CommandSource;
                            GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                            string BCReqId = (gvBCOnboard.DataKeys[gvr.RowIndex].Values["AgentReqId"]).ToString();
                            _BCEntity.BCReqId = BCReqId;
                            #region Audit
                            _auditParams[0] = Session["Username"].ToString();
                            _auditParams[1] = "Aggregator-Registration";
                            _auditParams[2] = "RowCommand-EditDetails";
                            _auditParams[3] = Session["LoginKey"].ToString();
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            #endregion
                            string _status = _BCEntity.DeleteBcDetails();
                            if (_status == "00")
                            {
                                FillGrid(EnumCollection.EnumBindingType.BindGrid);
                                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showSuccess('Data Deleted Successfully.', 'Warning');", true);
                                return;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Data Delete Unsuccessful.', 'Warning');", true);
                                return;
                            }
                            //ErrorLog.AggregatorTrace("btnSearch_Click | RowCommand-DeleteDetails | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        }
                        catch (Exception Ex)
                        {
                            ErrorLog.AggregatorTrace("Class : AggregatorRegistration.cs \nFunction : btnView_Click() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                            throw Ex;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("AggregatorRegistration: gvBlockAG_RowCommand: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        protected void EyeImage_Click(object sender, ImageClickEventArgs e)
        {
            try
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
                    string fileName = Ds.Tables[0].Rows[0]["IdentityProofType"].ToString();
                    string FinalPath = filepath + strURL;
                    strURL = Ds.Tables[0].Rows[0]["IdentityProofDocument"].ToString();

                    string pdfPath = strURL;
                    Session["pdfPath"] = filepath + pdfPath;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('../../PdfExport.aspx');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggregatorRegistration.cs \nFunction : BtnBack_Click() \nException Occured\n" + Ex.Message);
            }
        }


        protected void btnViewDownloadDoc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _BCEntity.Mode = "GetBCDocumentByID";
                _BCEntity.BCReqId = ViewState["BCReqId"].ToString();
                //_BCEntity.DocumentID = int.Parse(Imgbtn.CommandArgument);
                DataSet Ds = _BCEntity.GetBCDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    string fileName = Ds.Tables[0].Rows[0]["IdentityProofType"].ToString();
                    string filepath = AppDomain.CurrentDomain.BaseDirectory;
                    strURL = Ds.Tables[0].Rows[0]["IdentityProofDocument"].ToString();
                    string FinalPath = filepath + strURL;
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    byte[] data = req.DownloadData(FinalPath);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AggregatorTrace("Class : AggregatorRegistration.cs \nFunction : btnViewDownload_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page),  "Warning", "showWarning('Contact System Administrator', 'Aggregator Verification');", true);
                return;
            }
        }
        protected void txtPinCode_TextChanged(object sender, EventArgs e)
        {
            _BCEntity.Pincode = txtPinCode.Text.Trim();
            BindDropdownCountryState();
        }

        protected void ddlbcCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetInheritedSerivcesFromParent(ddlbcCode.SelectedValue != "0" ? ddlbcCode.SelectedValue : string.Empty);
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AggregatorRegistration.cs \nFunction : ddlbcCode_SelectedIndexChanged()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Aggregator Registration');", true);
                return;
            }
        }
        protected void SetInheritedSerivcesFromParent(string _Clientcode)
        {
            try
            {
                ErrorLog.AggregatorTrace("btnSearch_Click | SetInheritedSerivcesFromParent() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                _BCEntity.UserName = Session["Username"].ToString();
                _BCEntity.Clientcode = _Clientcode;
                _BCEntity.Flag = 1;
                DataSet dsServices = _BCEntity.InheritServicesFromParent();
                if (dsServices != null && dsServices.Tables.Count > 0 && dsServices.Tables[0].Rows.Count > 0)
                {
                    if (dsServices.Tables[0].Rows[0]["AEPS"] != null)
                    {
                        if (dsServices.Tables[0].Rows[0]["AEPS"].ToString() == "1")
                        {
                            chkAEPS.Checked = true;
                            chkAEPS.Enabled = true;
                            chkAEPS.Visible = true;
                            lblchkAEPS.Visible = true;
                        }
                        else
                        {
                            chkAEPS.Checked = false;
                            chkAEPS.Enabled = false;
                            chkAEPS.Visible = false;
                            lblchkAEPS.Visible = false;
                        }
                    }
                    else
                    {
                        chkAEPS.Checked = false;
                        chkAEPS.Enabled = false;
                        chkAEPS.Visible = false;
                        lblchkAEPS.Visible = false;
                    }

                    //if (dsServices.Tables[0].Rows[0]["BBPS"] != null)
                    //{
                    //    if (dsServices.Tables[0].Rows[0]["BBPS"].ToString() == "1")
                    //    {
                    //        chkbbps.Checked = true;
                    //        chkbbps.Enabled = true;
                    //        chkbbps.Visible = true;
                    //        lblchkbbps.Visible = true;
                    //    }
                    //    else
                    //    {
                    //        chkbbps.Checked = false;
                    //        chkbbps.Enabled = false;
                    //        chkbbps.Visible = false;
                    //        lblchkbbps.Visible = false;
                    //    }
                    //}
                    //else
                    //{
                    //    chkbbps.Checked = false;
                    //    chkbbps.Enabled = false;
                    //    chkbbps.Visible = false;
                    //    lblchkbbps.Visible = false;
                    //}

                    if (dsServices.Tables[0].Rows[0]["DMT"] != null)
                    {
                        if (dsServices.Tables[0].Rows[0]["DMT"].ToString() == "1")
                        {
                            chkdmt.Checked = true;
                            chkdmt.Enabled = true;
                            chkdmt.Visible = true;
                            lblchkdmt.Visible = true;
                        }
                        else
                        {
                            chkdmt.Checked = false;
                            chkdmt.Enabled = false;
                            chkdmt.Visible = false;
                            lblchkdmt.Visible = false;
                        }
                    }
                    else
                    {
                        chkdmt.Checked = false;
                        chkdmt.Enabled = false;
                        chkdmt.Visible = false;
                        lblchkdmt.Visible = false;
                    }

                    if (dsServices.Tables[0].Rows[0]["MATM"] != null)
                    {
                        if (dsServices.Tables[0].Rows[0]["MATM"].ToString() == "1")
                        {
                            chkMATM.Checked = true;
                            chkMATM.Enabled = true;
                            chkMATM.Visible = true;
                            lblchkMATM.Visible = true;
                        }
                        else
                        {
                            chkMATM.Checked = false;
                            chkMATM.Enabled = false;
                            chkMATM.Visible = false;
                            lblchkMATM.Visible = false;
                        }
                    }
                    else
                    {
                        chkMATM.Checked = false;
                        chkMATM.Enabled = false;
                        chkMATM.Visible = false;
                        lblchkMATM.Visible = false;
                    }
                }
                else
                {
                    lblServicesOffer.Visible = false;

                    chkAEPS.Checked = false;
                    chkAEPS.Enabled = false;
                    chkAEPS.Visible = false;
                    lblchkAEPS.Visible = false;

                    //chkbbps.Checked = false;
                    //chkbbps.Enabled = false;
                    //chkbbps.Visible = false;
                    //lblchkbbps.Visible = false;

                    chkdmt.Checked = false;
                    chkdmt.Enabled = false;
                    chkdmt.Visible = false;
                    lblchkdmt.Visible = false;

                    chkMATM.Checked = false;
                    chkMATM.Enabled = false;
                    chkMATM.Visible = false;
                    lblchkMATM.Visible = false;
                }
                ErrorLog.AggregatorTrace("btnSearch_Click | SetInheritedSerivcesFromParent() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : AggregatorRegistration.cs \nFunction : SetInheritedSerivcesFromParent() \nException Occured\n" + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "<script>showWarning('Something went wrong..! Please try again','Franchise Registration');</script>", false);
                return;
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
                ErrorLog.AgentManagementTrace("Page : AggregatorRegistration.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Agent Registration');", true);
                return;
            }
        }
        #endregion
        public string MaskingAadhar(string adharno)
        {
            string masked = string.Empty;
            var cardNumber = adharno;
            //var firstDigits = cardNumber.Substring(0, 6);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);
            var requiredMask = new String('X', cardNumber.Length - lastDigits.Length);
            var maskedString = string.Concat(requiredMask, lastDigits);
            var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");
            masked = maskedCardNumberWithSpaces;
            return masked;
        }
    }
}



