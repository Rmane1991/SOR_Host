using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using BussinessAccessLayer;
using AppLogger;

namespace SOR.Pages.Agent
{
    public partial class EditRegistrationDetails : System.Web.UI.Page
    {
        #region Object Declarations
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();

        public clsCustomeRegularExpressions customeRegExpValidation = null;
        public clsCustomeRegularExpressions _CustomeRegExpValidation
        {
            get { if (customeRegExpValidation == null) customeRegExpValidation = new clsCustomeRegularExpressions(); return customeRegExpValidation; }
            set { customeRegExpValidation = value; }
        }
        string _salt = string.Empty;

        public string pathId, PathAdd, PathSig, pathlnkId, PathlnkAdd, PathlnkSig;

        bool _IsValidFileAttached = false;
        #endregion

        #region String builder Instantiation.

        StringBuilder FilePath = null;
        StringBuilder _FilePath
        {
            get { if (FilePath == null) FilePath = new StringBuilder(); return FilePath; }
            set { FilePath = value; }
        }
        StringBuilder fileRecords = null;
        StringBuilder _fileRecords
        {
            get { if (fileRecords == null) fileRecords = new StringBuilder(); return fileRecords; }
            set { fileRecords = value; }
        }

        StringBuilder fileName = null;
        StringBuilder _fileName
        {
            get { if (fileName == null) fileName = new StringBuilder(); return fileName; }
            set { fileName = value; }
        }
        #endregion

        #region Objects Declaration
        public string UserName { get; set; }
        // string _DefaultPassword = ConnectionStringEncryptDecrypt.DecryptEncryptedDEK(AppSecurity.GenerateDfPw(), ConnectionStringEncryptDecrypt.ClearMEK);
        AppSecurity appSecurity = null;
        public AppSecurity _AppSecurity
        {
            get { if (appSecurity == null) appSecurity = new AppSecurity(); return appSecurity; }
            set { appSecurity = value; }
        }

        public object fileUpload { get; private set; }

        public enum TypeOfCommssion
        {
            RealTime = 93,
            Offline = 94
        }
        #endregion;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack == true)
            {

                BindDropdownCountryState();
                BindShopCountryState();
                FillBc();
                FillAggregator();
                // _AgentRegistrationDAL.BCCode = "64";
                string AgentReqId = Page.Request.QueryString["AgentReqId"].ToString();
                string RequestType = Page.Request.QueryString["RequestType"].ToString(); 
              
                Session["AgentReqId"] = AgentReqId;
                Session["RequestType"] = RequestType;
                HiddenField13.Value = _AgentRegistrationDAL.AgentReqId;

                GetDetails(AgentReqId);
            }
            else
            {
                if (hidimgId.Value != null)
                {
                    pathId = hidimgId.Value;
                }
                if (hidimgAdd.Value != null)
                {
                    PathAdd = hidimgAdd.Value;
                }
                if (hidimgSig.Value != null)
                {
                    PathSig = hidimgSig.Value;
                }
            }
        }

        #region Bind Dropdown
        public void BindDropdownCountryState()
        {
            try
            {
                _AgentRegistrationDAL.mode = (int)(EnumCollection.StateCityMode.PinCode);
                DataSet dsCountry = _AgentRegistrationDAL.GetCountryStateCityD();
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
                    _AgentRegistrationDAL.mode = (int)(EnumCollection.StateCityMode.ALLStateCity);
                    DataSet ds = _AgentRegistrationDAL.GetCountryStateCityD();

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
                ErrorLog.AgentManagementTrace("AgentRegistration: BindDropdownCountryState(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }


        }

        public void BindShopCountryState()
        {
            try
            {
                _AgentRegistrationDAL.mode = (int)(EnumCollection.StateCityMode.PinCode);
                DataSet ds = _AgentRegistrationDAL.GetShopCountryStateCity();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    ddlshopCountry.DataSource = ds;
                    ddlshopCountry.DataValueField = "Country";
                    ddlshopCountry.DataTextField = "Country";
                    ddlshopCountry.DataBind();

                    ddlShopState.DataSource = ds;
                    ddlShopState.DataValueField = "States";
                    ddlShopState.DataTextField = "States";
                    ddlShopState.DataBind();

                    ddlShopDistrict.DataSource = ds;
                    ddlShopDistrict.DataValueField = "District";
                    ddlShopDistrict.DataTextField = "District";
                    ddlShopDistrict.DataBind();

                    ddlShopCity.DataSource = ds;
                    ddlShopCity.DataValueField = "City";
                    ddlShopCity.DataTextField = "City";
                    ddlShopCity.DataBind();
                }
                else
                {
                    _AgentRegistrationDAL.mode = (int)(EnumCollection.StateCityMode.ALLStateCity);
                    DataSet dse = _AgentRegistrationDAL.GetShopCountryStateCity();

                    ddlshopCountry.DataSource = dse.Tables[0];
                    ddlshopCountry.DataValueField = "Country";
                    ddlshopCountry.DataTextField = "Country";
                    ddlshopCountry.DataBind();
                    ddlshopCountry.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlShopState.DataSource = dse.Tables[1];
                    ddlShopState.DataValueField = "States";
                    ddlShopState.DataTextField = "States";
                    ddlShopState.DataBind();
                    ddlShopState.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlShopDistrict.DataSource = dse.Tables[2];
                    ddlShopDistrict.DataValueField = "District";
                    ddlShopDistrict.DataTextField = "District";
                    ddlShopDistrict.DataBind();
                    ddlShopDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));

                    ddlShopCity.DataSource = dse.Tables[3];
                    ddlShopCity.DataValueField = "City";
                    ddlShopCity.DataTextField = "City";
                    ddlShopCity.DataBind();
                    ddlShopCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: BindDropdownCountryState(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }


        }
        #endregion
        #region Bind BC
        public void FillBc()
        {
            try
            {
                //_AgentRegistrationDAL.
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
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: FillBc: Exception: " + Ex.Message);
            }
        }

        #endregion

        protected void btnSubmitDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (HiddenField1.Value.ToString() == "Yes")
                {
                    //int Status = 0;
                    //string StatusMsg = string.Empty;
                    //string RequestId = string.Empty;
                    string Status = string.Empty;
                    string StatusMsg = string.Empty;
                    int RequestId = 0;
                    Session["PanNo"] = txtPANNo.Text;

                    txtPANNo.Text = !string.IsNullOrEmpty(hidPan.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidPan.Value)) : txtPANNo.Text;
                    txtaadharno.Text = !string.IsNullOrEmpty(hidAadh.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAadh.Value)) : txtaadharno.Text;
                    txtGSTNo.Text = !string.IsNullOrEmpty(hidSgst.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidSgst.Value)) : txtGSTNo.Text;
                    txtAccountNumber.Text = !string.IsNullOrEmpty(hidAccNo.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAccNo.Value)) : txtAccountNumber.Text;
                    txtIFsccode.Text = !string.IsNullOrEmpty(hidAccIFC.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAccIFC.Value)) : txtIFsccode.Text;
                    if (ValidateSetProperties())
                    {

                        _AgentRegistrationDAL.AEPS = chkAEPS.Checked == true ? 1 : 0;
                        _AgentRegistrationDAL.MATM = chkMATM.Checked == true ? 1 : 0;
                        _AgentRegistrationDAL.FirstName = txtFirstName.Text.Trim();
                        _AgentRegistrationDAL.MiddleName = txtMiddleName.Text.Trim();
                        _AgentRegistrationDAL.LastName = txtLastName.Text.Trim();
                        _AgentRegistrationDAL.PersonalEmailID = txtEmailID.Text.Trim();
                        _AgentRegistrationDAL.AadharNo = txtaadharno.Text.Trim();
                        _AgentRegistrationDAL.PanNo = txtPANNo.Text.Trim();
                        _AgentRegistrationDAL.GSTNo = txtGSTNo.Text.Trim();
                        _AgentRegistrationDAL.AgentDOB = txtdob.Text.Trim();
                        _AgentRegistrationDAL.AgentAddress = txtRegisteredAddress.Text.Trim();
                        _AgentRegistrationDAL.ContactNo = txtContactNo.Text.Trim();
                        _AgentRegistrationDAL.LandlineNo = txtLandlineNo.Text.Trim();
                        _AgentRegistrationDAL.RoleID = (int)EnumCollection.EnumPermissionType.EnableRoles;
                        _AgentRegistrationDAL.AccountNumber = txtAccountNumber.Text.ToString();
                        _AgentRegistrationDAL.IFSCCode = txtIFsccode.Text.Trim().ToString();
                        _AgentRegistrationDAL.AgentCountry = ddlCountry.SelectedValue.ToString().Trim();
                        _AgentRegistrationDAL.AgentState = ddlState.SelectedValue.ToString().Trim();
                        _AgentRegistrationDAL.AgentCity = ddlCity.SelectedValue.ToString().Trim();
                        _AgentRegistrationDAL.AgentDistrict = ddlDistrict.SelectedValue.ToString().Trim();
                        _AgentRegistrationDAL.AgentPincode = txtPinCode.Text.Trim();
                        _AgentRegistrationDAL.AlternateNo = txtAlterNateNo.Text.Trim().ToString();
                        _AgentRegistrationDAL.BCCode = ddlbcCode.SelectedValue.ToString();
                        _AgentRegistrationDAL.AggCode = ddlaggregatorCode.SelectedValue.ToString();
                        _AgentRegistrationDAL.Gender = ddlGender.SelectedValue.ToString();
                        _AgentRegistrationDAL.AgentCategory = ddlCategory.SelectedValue.Trim();
                        _AgentRegistrationDAL.PassportNo = txtPass.Text.Trim();
                        _AgentRegistrationDAL.PopulationGroup = ddlarea.SelectedValue.Trim();

                        _AgentRegistrationDAL.Lattitude = txtLatitude.Text.Trim();
                        _AgentRegistrationDAL.Longitude = txtLongitude.Text.Trim();

                        _AgentRegistrationDAL.DeviceCode = txtdevicecode.Text.Trim();
                        _AgentRegistrationDAL.ShopCity = ddlShopCity.SelectedValue.ToString();
                        _AgentRegistrationDAL.ShopAddress = txtshopadd.Text.Trim();
                        _AgentRegistrationDAL.ShopDistrict = ddlShopDistrict.SelectedValue.ToString().Trim();
                        _AgentRegistrationDAL.ShopCountry = ddlshopCountry.SelectedValue.ToString().Trim();
                        _AgentRegistrationDAL.ShopState = ddlShopState.SelectedValue.ToString();
                        _AgentRegistrationDAL.ShopPinCode = txtshoppin.Text.Trim();
                        _AgentRegistrationDAL.ClientId = Session["Client"].ToString();


                        if (CheckBox.Checked == true)
                        {
                            _AgentRegistrationDAL.ShopAddress = txtRegisteredAddress.Text.Trim();
                            _AgentRegistrationDAL.ShopDistrict = ddlShopDistrict.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.ShopCountry = ddlCountry.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.ShopCity = ddlCity.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.ShopState = ddlState.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.shopemail = txtEmailID.Text.Trim();
                        }
                        else
                        {
                            _AgentRegistrationDAL.ShopAddress = txtRegisteredAddress.Text.Trim();
                            _AgentRegistrationDAL.ShopDistrict = ddlShopDistrict.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.ShopCountry = ddlCountry.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.ShopCity = ddlCity.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.ShopState = ddlState.SelectedValue.ToString().Trim();
                            _AgentRegistrationDAL.shopemail = txtshopEmailID.Text.Trim();
                        }
                        _AgentRegistrationDAL.AgentAddress = txtRegisteredAddress.Text.Trim().ToString();
                        _AgentRegistrationDAL.AgentCountry = ddlCountry.SelectedValue.ToString().Trim();
                        _AgentRegistrationDAL.AgentState = ddlState.SelectedValue.ToString().Trim();
                        _AgentRegistrationDAL.AgentCity = ddlCity.SelectedValue.ToString().Trim();
                        _AgentRegistrationDAL.AgentDistrict = ddlDistrict.SelectedValue.ToString().Trim();
                        _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();

                        _AgentRegistrationDAL.agReqId = HidAGID.Value != null && !string.IsNullOrEmpty(HidAGID.Value) ? Convert.ToInt32(HidAGID.Value) : Convert.ToInt32(Page.Request.QueryString["AgentReqId"].ToString());

                        string IsSelfData = Page.Request.QueryString["IsSelfData"].ToString();
                        if (IsSelfData == "1")
                        {
                            _AgentRegistrationDAL.Flag = 3;
                        }
                        else
                        {
                            if (_AgentRegistrationDAL.agReqId == Convert.ToInt32(Page.Request.QueryString["AgentReqId"]))
                            {
                                _AgentRegistrationDAL.Flag = 5;
                            }
                            else
                            {
                                _AgentRegistrationDAL.Flag = 3;
                            }
                        }

                        _AgentRegistrationDAL.Activity = Page.Request.QueryString["RequestType"].ToString();
                       
                        //DataSet dsCheck = _AgentRegistrationDAL.SetInsertUpdateAgentDetails();
                        if (_AgentRegistrationDAL.Insert_AgentRequest(Convert.ToString(Session["Username"]), out RequestId, out Status, out StatusMsg))
                        {
                            _AgentRegistrationDAL.AgentReqId = Page.Request.QueryString["AgentReqId"].ToString();
                            DataSet ds = _AgentRegistrationDAL.GetDocs();

                            if (ds.Tables[0] != null)
                            {
                                divAddressProof1.Visible = true;
                                divIdProof1.Visible = true;
                                divSigProof1.Visible = true;
                                
                                string idthumb = ds.Tables[0].Rows[0]["IdentityProofDocument"].ToString();
                                pathlnkId = "../../" + idthumb;
                                Session["IdAgentFilePath"] = idthumb;

                                string idType = ds.Tables[0].Rows[0]["IdentityProofType"].ToString();

                                string Addthumb = ds.Tables[0].Rows[0]["AddressProofDocument"].ToString();
                                PathlnkAdd = "../../" + Addthumb;
                                Session["AddAgentFilePath"] = Addthumb;

                                string AddType = ds.Tables[0].Rows[0]["AddressProofType"].ToString();

                                string Sigthumb = ds.Tables[0].Rows[0]["SignatureProofDocument"].ToString();
                                PathlnkSig = "../../" + Sigthumb;
                                Session["SigAgentFilePath"] = Sigthumb;

                                string SigType = ds.Tables[0].Rows[0]["SignatureProofType"].ToString();

                                ddlIdentityProof.SelectedValue = idType;
                                ddlAddressProof.SelectedValue = AddType;
                                ddlSignature.SelectedValue = SigType;
                                ddlIdentityProof.Enabled = false;
                                ddlAddressProof.Enabled = false;
                                ddlSignature.Enabled = false;
                            }

                            HidAGID.Value = RequestId.ToString();
                            div_Upload.Visible = true;
                            DivAgntDetails.Visible = false;
                            DIVDetails.Visible = false;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Processed for Document Upload');", true);
                            return;
                        }
                        else
                        {
                            ErrorLog.AgentManagementTrace("EditRegistationDetails: btnSubmitDetails_Click: Failed - Agent Registration Request Dump In DB. UserName: " + UserName + " Status: " + Status + " StatusMsg: " + StatusMsg + " RequestId: " + RequestId);
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + StatusMsg + "', 'Alert');", true);
                            return;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: btnSubmitDetails_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnExportCSV_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid();

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "De-Active Business Correspondents Details", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: btnExportCSV_Click: Exception: " + Ex.Message);
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
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }

        protected void btndownload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid();

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "De-Active Business Correspondents Details", dt);
                }
                {
                    //lblRecordCount.Text = "No Record's Found.";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: btndownload_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        #region FillGrid
        public DataSet FillGrid()
        {
            DataSet ds = null;
            try
            {

                ds = _AgentRegistrationDAL.GetAgentDetailForRegistration();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvAgentOnboard.DataSource = ds.Tables[0];
                    gvAgentOnboard.DataBind();
                    btnExportCSV.Visible = true;
                    btndownload.Visible = true;
                    // lblRecordsTotal.Text = "Total " + Convert.ToString(ds.Tables[0].Rows.Count) + " Record(s) Found.";
                }
                else
                {
                    gvAgentOnboard.Visible = false;
                    btndownload.Visible = false;
                    btnExportCSV.Visible = false;
                    //lblRecordsTotal.Text = "Total 0 Record(s) Found.";
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: FillGrid: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return ds;
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
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithoutSpace, txtFirstName.Text))
                    {
                        txtFirstName.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid First Name.', 'First Name');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.FileName = txtFirstName.Text.ToString();


                // PanNo
                if (hd_txtPANNo.Value == "1" || !string.IsNullOrEmpty(txtPANNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.PanCard, txtPANNo.Text))
                    {
                        txtPANNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid PAN Card.', ' PAN Card');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.PanNo = txtPANNo.Text.Trim();

              
                // Registered Address
                if (hd_txtRegisteredAddress.Value == "1" || !string.IsNullOrEmpty(txtRegisteredAddress.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Address, txtRegisteredAddress.Text))
                    {
                        txtRegisteredAddress.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Address.', 'Registered Address');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.AgentAddress = txtRegisteredAddress.Text.Trim();

                // Pincode
                if (hd_txtPinCode.Value == "1" || !string.IsNullOrEmpty(txtPinCode.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Pincode, txtPinCode.Text))
                    {
                        txtPinCode.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Pincode.', 'Pincode');", true);

                        return false;
                    }
                    else _AgentRegistrationDAL.AgentPincode = txtPinCode.Text.Trim();

               
                // State
                if (ddlState.SelectedValue == "0")
                {
                    ddlState.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select State.', 'State');", true);
                    return false;
                }
                else _AgentRegistrationDAL.AgentState = ddlState.SelectedValue.ToString().Trim();

                // City
                if (ddlCity.SelectedValue == "0")
                {
                    ddlCity.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select City.', 'City');", true);
                    return false;
                }
                else _AgentRegistrationDAL.AgentCity = ddlCity.SelectedValue.ToString().Trim();

                //District
                if (ddlDistrict.SelectedValue == "0")
                {
                    ddlCity.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select District.', 'City');", true);
                    return false;
                }
                else _AgentRegistrationDAL.AgentDistrict = ddlDistrict.SelectedValue.ToString().Trim();
              
                // Personal Contact
                if (hd_txtContactNo.Value == "1" || !string.IsNullOrEmpty(txtContactNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, txtContactNo.Text))
                    {
                        txtContactNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Contact No.', 'Personal Contact No.');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.PersonalContact = txtContactNo.Text.Trim();

                // Landline Contact
                //if (hd_txtLandlineNo.Value == "1" )
                //    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.NumberOnly, txtLandlineNo.Text))
                //    {
                //        txtLandlineNo.Focus();
                //        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Landline No.', 'Invalid Landline No.');", true);
                //        return false;
                //    }
                //    else _AgentRegistrationDAL.LandlineNo = txtLandlineNo.Text.Trim();

                // Alternate Mobile No
                if (hd_txtAlterNateNo.Value == "1" || !string.IsNullOrEmpty(txtAlterNateNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, txtAlterNateNo.Text))
                    {
                        txtAlterNateNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Mobile No.', 'Alternate Mobile No.');", true);
                        return false;
                    }
                    else _AgentRegistrationDAL.AlternateNo = txtAlterNateNo.Text.Trim().ToString();

            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: ValidateSetProperties: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return true;
        }
        #endregion

        #region SelectedIndexChanged

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlDistrict.DataSource = null;

                _AgentRegistrationDAL.AgentState = string.Empty;
                _AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.District;
                _AgentRegistrationDAL.AgentState = ddlState.SelectedValue.ToString();
                DataSet ds_District = _AgentRegistrationDAL.GetCountryStateCityD();
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
                ErrorLog.AgentManagementTrace("AgentRegistration: ddlState_SelectedIndexChanged(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void ddlShopState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.District;
                _AgentRegistrationDAL.ShopState = ddlShopState.SelectedValue.ToString();
                DataSet ds_District = _AgentRegistrationDAL.GetShopCountryStateCity();
                if (ds_District != null && ds_District.Tables.Count > 0 && ds_District.Tables.Count > 0)
                {
                    ddlShopDistrict.DataSource = ds_District;
                    ddlShopDistrict.DataValueField = "District";
                    ddlShopDistrict.DataTextField = "District";
                    ddlShopDistrict.DataBind();
                    ddlShopDistrict.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    ddlShopDistrict.DataSource = null;
                    ddlShopDistrict.DataBind();
                    ddlShopDistrict.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: ddlShopState_SelectedIndexChanged(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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
                ddlDistrict.SelectedValue = null;
                ddlShopDistrict.SelectedValue = null;
                ddlCity.SelectedValue = null;
                //flgUplodMyImage = null;
                txtAccountNumber.Text = null;
                txtIFsccode.Text = null;
                txtLandlineNo.Text = null;
                txtAlterNateNo.Text = null;
                dvfield_PANNo.Visible = true;
                txtaadharno.Text = null;
                chkAEPS.Checked = false;
                chkMATM.Checked = false;
                ddlCategory.ClearSelection();


            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: ClearAllControls: Exception: " + Ex.Message);
            }
        }
        #endregion

        #region PolulateSummary

        protected void PolulateSummary()
        {
            try
            {
                if (HidAGID.Value != null || !string.IsNullOrEmpty(txtPANNo.Text.Trim()))
                {
                    _AgentRegistrationDAL.PanNo = txtPANNo.Text.Trim();
                    _AgentRegistrationDAL.RequestId = Convert.ToInt32(HidAGID.Value);
                    _AgentRegistrationDAL.Flag = (int)EnumCollection.EnumPermissionType.Export;
                    DataSet ds_Receipt = _AgentRegistrationDAL.GetAgentRequestList();
                    if (ds_Receipt != null && ds_Receipt.Tables.Count > 0 && ds_Receipt.Tables[0].Rows.Count > 0)
                    {
                        ErrorLog.AgentManagementTrace("AgentRegistration: PolulateSummary: Failed - Fetch Agent Registration Details Using RequestID/PAN From DB. UserName: " + UserName + " PanNo: " + _AgentRegistrationDAL.PanNo + " RequestId: " + HidAGID.Value);
                        txtaadharno.Text = ds_Receipt.Tables[0].Rows[0]["AadharNo"].ToString();
                        txtacc.Text = ds_Receipt.Tables[0].Rows[0]["AccountNumber"].ToString();
                        txtAccountNumber.Text = ds_Receipt.Tables[0].Rows[0]["AccountNumber"].ToString();
                        txtadd.Text = ds_Receipt.Tables[0].Rows[0]["AgentAddress"].ToString();
                        txtbcname.Text = ds_Receipt.Tables[0].Rows[0]["AgentName"].ToString();
                        txtagentcategory.Text = ds_Receipt.Tables[0].Rows[0]["AgentCategory"].ToString();
                        ddlcl.Text = ds_Receipt.Tables[0].Rows[0]["ClientName"].ToString();
                        txtbccode.Text = ds_Receipt.Tables[0].Rows[0]["BCCode"].ToString();
                        ddlcitys.Text = ds_Receipt.Tables[0].Rows[0]["LocalCity"].ToString();
                        txtcontact.Text = ds_Receipt.Tables[0].Rows[0]["ContactNo"].ToString();
                        txtshopcontact.Text = ds_Receipt.Tables[0].Rows[0]["ShopContact"].ToString();
                        txtLandlineNo.Text = ds_Receipt.Tables[0].Rows[0]["LandlineNo"].ToString();
                        ddlcountrys.Text = ds_Receipt.Tables[0].Rows[0]["LocalCountry"].ToString();
                        ddlstates.Text = ds_Receipt.Tables[0].Rows[0]["LocalState"].ToString();
                        ddldist.Text = ds_Receipt.Tables[0].Rows[0]["AgentDistrict"].ToString();
                        txtemail.Text = ds_Receipt.Tables[0].Rows[0]["PersonalEmailID"].ToString();
                        TextBox1.Text = ds_Receipt.Tables[0].Rows[0]["ShopAddress"].ToString();
                        TextBox3.Text = ds_Receipt.Tables[0].Rows[0]["ShopCity"].ToString();
                        TextBox3.Text = ds_Receipt.Tables[0].Rows[0]["LocalCountry"].ToString();
                        TextBox2.Text = ds_Receipt.Tables[0].Rows[0]["ShopState"].ToString();
                        TextBox5.Text = ds_Receipt.Tables[0].Rows[0]["ShopContact"].ToString();
                        DDlgen.Text = ds_Receipt.Tables[0].Rows[0]["Gender"].ToString();
                        txtgst.Text = ds_Receipt.Tables[0].Rows[0]["GSTNo"].ToString();
                        txtpan.Text = ds_Receipt.Tables[0].Rows[0]["PanNo"].ToString();
                        txtpin.Text = ds_Receipt.Tables[0].Rows[0]["AgentPinCode"].ToString();
                        rectextshopoin.Text = ds_Receipt.Tables[0].Rows[0]["ShopPinCode"].ToString();
                        txtaadh.Text = ds_Receipt.Tables[0].Rows[0]["AadharNo"].ToString();
                        txtifsc.Text = ds_Receipt.Tables[0].Rows[0]["IFSCCode"].ToString();
                        txtAgrdob.Text = ds_Receipt.Tables[0].Rows[0]["AgentDOB"].ToString();
                        string idthumb = ds_Receipt.Tables[0].Rows[0]["IdentityProofDocument"].ToString();
                        string FileThumbnailId = Path.GetDirectoryName(idthumb) + "\\" + Path.GetFileNameWithoutExtension(idthumb) + "_Thumbnail.png";
                        pathId = "../../" + FileThumbnailId;
                        Session["pdfPathID"] = idthumb;
                        hidimgId.Value = pathId;
                        string Addthumb = ds_Receipt.Tables[0].Rows[0]["AddressProofDocument"].ToString();
                        string FileThumbnailAdd = Path.GetDirectoryName(Addthumb) + "\\" + Path.GetFileNameWithoutExtension(Addthumb) + "_Thumbnail.png";
                        PathAdd = "../../" + FileThumbnailAdd;
                        Session["pdfPathAdd"] = Addthumb;
                        hidimgAdd.Value = PathAdd;
                        string Sigthumb = ds_Receipt.Tables[0].Rows[0]["SignatureProofDocument"].ToString();
                        string FileThumbnailSig = Path.GetDirectoryName(Sigthumb) + "\\" + Path.GetFileNameWithoutExtension(Sigthumb) + "_Thumbnail.png";
                        PathSig = "../../" + FileThumbnailSig;
                        Session["pdfPathSig"] = Sigthumb;
                        hidimgSig.Value = PathSig;
                        ViewState["agReqId"] = ds_Receipt.Tables[0].Rows[0]["agentreqId"].ToString();
                    }
                    else
                    {
                        ErrorLog.AgentManagementTrace("EditRegistrationDetails: PolulateSummary: Failed - Fetch Agent Registration Details Using RequestID/PAN From DB. UserName: " + UserName + " PanNo: " + _AgentRegistrationDAL.PanNo + " RequestId: " + HidAGID.Value);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again later', 'Alert');", true);
                        return;
                    }
                }
                else
                {
                    ErrorLog.AgentManagementTrace("EditRegistrationDetails: PolulateSummary: Failed - Fetch Agent Registration Details Using RequestID/PAN From DB. UserName: " + UserName + " PanNo: " + _AgentRegistrationDAL.PanNo + " RequestId: " + HidAGID.Value);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again later', 'Alert');", true);
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: PolulateSummary: Failed - Fetch Agent Registration Details Using RequestID/PAN From DB. Exception: " + Ex.Message + " UserName: " + UserName + " PanNo: " + _AgentRegistrationDAL.PanNo + " RequestId: " + HidAGID.Value);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again later', 'Alert');", true);
                return;
            }
        }

        #endregion

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (HiddenField2.Value.ToString() == "Yes")
                {
                    //int Status = 0;
                    //string StatusMsg = string.Empty;
                    //string RequestId = string.Empty;
                    _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
                    _AgentRegistrationDAL.Flag = (int)EnumCollection.DBFlag.Update;
                    // _AgentEntity.Clientcode = ddlclient.SelectedValue.ToString();

                    _AgentRegistrationDAL.ClientId = Session["Client"].ToString();
                    if (divIdProof.Visible == true && !FileUploadagent.HasFile)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Upload Identity Proof Document', 'Alert');", true);
                        return;
                    }
                    if (divAddressProof.Visible == true && !FileUploadagent1.HasFile)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Upload Address Proof Document', 'Alert');", true);
                        return;
                    }
                    if (divSigProof.Visible == true && !FileUploadagent2.HasFile)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Upload Signature Proof Document', 'Alert');", true);
                        return;
                    }
                    _AgentRegistrationDAL.IdentityProofDocument = Session["IdAgentFilePath"].ToString();
                    _AgentRegistrationDAL.IdentityProofType = ddlIdentityProof.SelectedValue;
                    _AgentRegistrationDAL.AddressProofDocument = Session["AddAgentFilePath"].ToString();
                    _AgentRegistrationDAL.AddressProofType = ddlAddressProof.SelectedValue;
                    _AgentRegistrationDAL.SignatureProofDocument = Session["SigAgentFilePath"].ToString();
                    _AgentRegistrationDAL.SignatureProofType = ddlSignature.SelectedValue;

                    _AgentRegistrationDAL.agReqId = Convert.ToInt32(HidAGID.Value);



                    if (_AgentRegistrationDAL.Insert_AgentRequest(Convert.ToString(Session["Username"]), out int RequestId, out string Status, out string StatusMsg))
                    {
                        ErrorLog.AgentManagementTrace("AgentRegistration: BtnSubmit_Click: Successful - Upload Documents. UserName: " + UserName + " Status: " + Status + " StatusMsg: " + StatusMsg + " RequestId: " + RequestId);
                        DivAgntDetails.Visible = true;
                        _AgentRegistrationDAL.AgentCode = RequestId.ToString();
                        HidAGID.Value = RequestId.ToString();
                        PolulateSummary();
                        div_Upload.Visible = false;
                        DivAgntDetails.Visible = true;
                        DIVDetails.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Documents Uploaded Successfully', 'alert');", true);
                    }
                    else
                    {
                        ErrorLog.AgentManagementTrace("AgentRegistration: BtnSubmit_Click: Failed - Upload Documents. UserName: " + UserName + " Status: " + Status + " StatusMsg: " + StatusMsg + " RequestId: " + RequestId);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + StatusMsg + "', 'Alert');", true);
                        return;
                    }
                }
                else
                {
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("AgentRegistration: BtnSubmit_Click: Failed - Upload Documents. Exception: " + Ex.Message + " UserName: " + UserName);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again later', 'Alert');", true);
                return;
            }
        }

        protected void btnViewDownloadDoc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                _AgentRegistrationDAL.AgentReqId = ViewState["agReqId"].ToString();
                DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    //string fileName = Ds.Tables[0].Rows[0]["IdentityProofType"].ToString();
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
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: btnViewDownloadDoc_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnViewDownloadDoc_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                _AgentRegistrationDAL.AgentReqId = ViewState["agReqId"].ToString();
                DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
                if (Ds.Tables[1].Rows.Count > 0)
                {
                    string strURL = string.Empty;
                    //string fileName = Ds.Tables[1].Rows[0]["AddressProofType"].ToString();
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
                ErrorLog.AgentManagementTrace("EditRegistrationDetails: btnViewDownloadDoc_Click1: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnViewDownloadDoc_Click2(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Imgbtn = (ImageButton)sender;
                _AgentRegistrationDAL.Mode = "GetAgentDocumentById";
                _AgentRegistrationDAL.AgentReqId = ViewState["agReqId"].ToString();
                DataSet Ds = _AgentRegistrationDAL.GetAgentDocuments();
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
                ErrorLog.AgentManagementTrace("EditRegitsration: btnViewDownloadDoc_Click2: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        protected void btnCloseReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (HiddenField1.Value.ToString() == "Yes")
                {
                    DIVDetails.Visible = true;
                    divAction.Visible = false;
                    divMainDetailsGrid.Visible = false;
                    DivAgntDetails.Visible = false;
                    ddlSignature.SelectedValue = null;
                    ddlAddressProof.ClearSelection();
                    ddlIdentityProof.ClearSelection();
                    string val = HidAGID.Value;
                }
                else
                {

                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: btnCloseReceipt_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        #region SaveFile IdentityProof
        private string SaveFile(HttpPostedFile fileUpload, string id)
        {
            try
            {
                string PathLocation = ConfigurationManager.AppSettings["AgentDocumentPath"].ToString();
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
                ErrorLog.AgentManagementTrace("EditRegitsration: SaveFile: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return string.Empty; ;
            }
        }
        #endregion

        #region SaveFile AddressProof
        private string SaveFileAddprof(HttpPostedFile fileUpload, string id)
        {
            try
            {
                string PathLocation = ConfigurationManager.AppSettings["AgentDocumentPath"].ToString();
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
                ErrorLog.AgentManagementTrace("EditRegitsration: SaveFileAddprof: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return string.Empty;
            }
        }
        #endregion

        #region SaveFile SignatureProof
        private string SaveFileSignproof(HttpPostedFile fileUpload, string id)
        {
            try
            {
                string PathLocation = ConfigurationManager.AppSettings["AgentDocumentPath"].ToString();
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
                ErrorLog.AgentManagementTrace("EditRegitsration: SaveFileSignproof: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Upload Valid File with Original extension.','" + _TypeOfDocument + "');", true);
                    return false;
                }
                _fileExtension = Path.GetExtension(_FileUpload.FileName);
                if (_fileExtension.ToLower() != ".jpg" && _fileExtension.ToLower() != ".jpeg" && _fileExtension.ToLower() != ".png" && _fileExtension.ToLower() != ".pdf")
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid File Format. Only .jpg/.png/.jpeg/.pdf formats are allowed.', '" + _TypeOfDocument + "');", true);
                    return false;
                }
                long fileSize = _FileUpload.ContentLength;
                if (_FileUpload.ContentLength <= 0)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid File Size.Should Not Be Empty','" + _TypeOfDocument + "');", true);
                    return false;
                }
                if (_fileExtension.ToLower() != ".pdf" && _FileUpload.ContentLength > 1000000)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid File Size.Should be less than 1 MB.','" + _TypeOfDocument + "');", true);
                    return false;
                }
                else if (_fileExtension.ToLower() == ".pdf" && _FileUpload.ContentLength > 2000000)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid File Size.Should be less than 2 MB.','" + _TypeOfDocument + "');", true);
                    return false;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: ValidateFile: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Upload Valid File with Original extension.','" + _TypeOfDocument + "');", true);
                    return false;
                }
                _fileExtension = Path.GetExtension(postedFile.FileName);
                if (_fileExtension.ToLower() != ".jpg" && _fileExtension.ToLower() != ".jpeg" && _fileExtension.ToLower() != ".png" && _fileExtension.ToLower() != ".pdf")
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid File Format. Only .jpg/.png/.jpeg/.pdf formats are allowed.', '" + _TypeOfDocument + "');", true);
                    return false;
                }
                long fileSize = postedFile.ContentLength;
                if (postedFile.ContentLength <= 0)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid File Size.Should Not Be Empty','" + _TypeOfDocument + "');", true);
                    return false;
                }
                if (_fileExtension.ToLower() != ".pdf" && postedFile.ContentLength > 1000000)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid File Size.Should be less than 1 MB.','" + _TypeOfDocument + "');", true);
                    return false;
                }
                else if (_fileExtension.ToLower() == ".pdf" && postedFile.ContentLength > 2000000)
                {
                    _IsValidFileAttached = false;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid File Size.Should be less than 2 MB.','" + _TypeOfDocument + "');", true);
                    return false;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: ValidateFiles: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: DeleteFile: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: IsValidImage: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return true;
        }
        #endregion

        protected void downloadPass_Click(object sender, EventArgs e)
        {
            try
            {
                if (HiddenField3.Value.ToString() == "Yes")
                {
                    if (ChkConfirmBC.Checked == true)
                    {

                        _AgentRegistrationDAL.CreatedBy = Session["Username"].ToString();
                        _AgentRegistrationDAL.AgentCode = HidAGID.Value.ToString();

                        _AgentRegistrationDAL.Activity = Convert.ToString(Session["RequestType"]);

                        _AgentRegistrationDAL.Flag = 2;
                        DataSet dsAgentMaster = _AgentRegistrationDAL.SetInsertUpdateAgentRequestHadlerDetails();
                        if (dsAgentMaster != null && dsAgentMaster.Tables.Count > 0 && dsAgentMaster.Tables[0].Rows[0]["Message"].ToString() == "Inserted")
                        {

                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Data Registered Successfully', 'Agent ReEdit');", true);
                            div_Upload.Visible = false;
                            divOnboardFranchise.Visible = false;
                            DivAgntDetails.Visible = false;
                            DIVDetails.Visible = false;
                            divAction.Visible = true;
                            divMainDetailsGrid.Visible = true;
                            ClearAllControls();

                            Response.Redirect("../../Pages/Agent/OBAgStatus.aspx", false);
                        }
                        else
                        {
                            ClearAllControls();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Confirmation on all above BC Deatils are properly filled');", true);
                        return;
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong');", true);
                    //return;
                }
            }

            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: downloadPass_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        protected void btntest_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtenderEditRequest.Show();
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: btntest_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DIVDetails.Visible = false;
                div_Upload.Visible = false;
                divPaymentReceipt.Visible = false;
                btndownload.Visible = true;
                btnExportCSV.Visible = true;
                divOnboardFranchise.Visible = false;
                divAction.Visible = true;
                divMainDetailsGrid.Visible = true;
                Response.Redirect("~/Pages/Agent/OBAgStatus.aspx", false);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: btnCancel_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                divPaymentReceipt.Visible = false;
                btndownload.Visible = true;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: Button2_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnAddnew_Click(object sender, EventArgs e)
        {
            try
            {
                divOnboardFranchise.Visible = true;
                divAction.Visible = false;
                divMainDetailsGrid.Visible = false;
                DIVDetails.Visible = true;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: btnAddnew_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
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
                _AgentRegistrationDAL.BCCode = BCReqId;
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: btnViewDownload_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string pdfPath = Server.MapPath("~/Thumbnail/Aadhar/document-1_220422_110414 (1) (1).pdf");
                Session["pdfPath"] = pdfPath;
                string script = "<script type='text/javascript'>window.open('" + "PdfExport.aspx" + "')</script>";
                this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: Unnamed_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }


        #endregion

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                DIVDetails.Visible = true;
                div_Upload.Visible = false;
                DivAgntDetails.Visible = false;
                divPaymentReceipt.Visible = false;
                divAddressProof.Visible = false;
                divAddressProof1.Visible = false;
                divIdProof.Visible = false;
                divIdProof1.Visible = false;
                divSigProof.Visible = false;
                divSigProof1.Visible = false;
                string val = HidAGID.Value;
                GetDetails(val);
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: BtnBack_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }


        protected void lbtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            if (hdnUserConfirmation.Value.ToString() == "Yes")
            {
                divIdProof1.Visible = false;
                divIdProof.Visible = true;
                ddlIdentityProof.Enabled = true;
            }

        }

        protected void lbtnDelete2_Click(object sender, ImageClickEventArgs e)
        {
            if (hdnUserConfirmation.Value.ToString() == "Yes")
            {
                divAddressProof.Visible = true;
                divAddressProof1.Visible = false;
                ddlAddressProof.Enabled = true;
            }
        }

        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {
            if (hdnUserConfirmation.Value.ToString() == "Yes")
            {
                divSigProof.Visible = true;
                divSigProof1.Visible = false;
                ddlSignature.Enabled = true;
            }
        }
     
        protected void CheckBoxAddress_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckBox.Checked == true)
                {
                    RequiredFieldValidator14.Enabled = false;
                    RequiredFieldValidator14.ValidationGroup = "";

                    RequiredFieldValidator4.Enabled = false;
                    RequiredFieldValidator4.ValidationGroup = "";

                    //RequiredFieldValidator15.Enabled = false;
                    //RequiredFieldValidator15.ValidationGroup = "";

                    divshopcountry.Visible = false;
                }
                else
                {
                    RequiredFieldValidator14.Enabled = false;
                    RequiredFieldValidator14.ValidationGroup = "AgentReg";

                    RequiredFieldValidator4.Enabled = false;
                    RequiredFieldValidator4.ValidationGroup = "AgentReg";

                    //RequiredFieldValidator15.Enabled = false;
                    //RequiredFieldValidator15.ValidationGroup = "AgentReg";

                    divshopcountry.Visible = true;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : EditRegitsration.cs \nFunction : CheckBoxAddress_CheckedChanged() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }





        public void GetDetails(string AgentReqId)
        {
            try
            {
                _AgentRegistrationDAL.AgentReqId = AgentReqId;

                DataSet ds = _AgentRegistrationDAL.GetAgentDetails();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["AgentState"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["AgentState"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["AgentDistrict"].ToString() != "")
                    {
                        ddlDistrict.SelectedValue = ds.Tables[0].Rows[0]["AgentDistrict"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["AgentCity"].ToString() != "")
                    {
                        ddlCity.SelectedValue = ds.Tables[0].Rows[0]["AgentCity"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ShopState"].ToString() != "")
                    {
                        ddlShopState.SelectedValue = ds.Tables[0].Rows[0]["ShopState"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ShopDistrict"].ToString() != "")
                    {
                        ddlShopDistrict.SelectedValue = ds.Tables[0].Rows[0]["ShopDistrict"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ShopCity"].ToString() != "")
                    {
                        ddlShopCity.SelectedValue = ds.Tables[0].Rows[0]["ShopCity"].ToString();
                    }
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlbcCode.DataSource = ds.Tables[0];
                        ddlbcCode.DataValueField = "BC Code";
                        ddlbcCode.DataTextField = "BCName";
                        ddlbcCode.DataBind();
                        ddlbcCode.Items.Insert(0, new ListItem("-- Select --", "0"));

                    }

                    if (ds.Tables[0].Rows[0]["BC Code"].ToString() != "")
                    {
                        ddlbcCode.SelectedValue = ds.Tables[0].Rows[0]["BC Code"].ToString();
                    }
                    if (ds.Tables[0].Columns.Contains("aggcode") && ds.Tables[0].Rows[0]["aggcode"] != DBNull.Value)
                    {
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            ddlaggregatorCode.DataSource = ds.Tables[0];
                            ddlaggregatorCode.DataValueField = "aggcode";
                            ddlaggregatorCode.DataTextField = "aggname";
                            ddlaggregatorCode.DataBind();
                            ddlaggregatorCode.Items.Insert(0, new ListItem("-- Select --", "0"));

                        }

                        if (ds.Tables[0].Rows[0]["aggcode"].ToString() != "")
                        {
                            ddlaggregatorCode.SelectedValue = ds.Tables[0].Rows[0]["aggcode"].ToString();
                        }
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
                    if (ds.Tables[0].Rows[0]["AgentCategory"].ToString() != "")
                    {
                        ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["AgentCategory"].ToString();
                    }
                    txtAccountNumber.Text = ds.Tables[0].Rows[0]["AccountNumber"].ToString();
                    txtIFsccode.Text = ds.Tables[0].Rows[0]["IFSCCode"].ToString();
                    txtPass.Text = ds.Tables[0].Rows[0]["PassportNo"].ToString();
                    if (ds.Tables[0].Rows[0]["PopulationGroup"].ToString() != "")
                    {
                        ddlarea.SelectedValue = ds.Tables[0].Rows[0]["PopulationGroup"].ToString();
                    }

                    txtLatitude.Text = ds.Tables[0].Rows[0]["Latitude"].ToString();
                    txtLongitude.Text = ds.Tables[0].Rows[0]["Longitude"].ToString();

                    txtRegisteredAddress.Text = ds.Tables[0].Rows[0]["AgentAddress"].ToString();
                    txtdevicecode.Text = ds.Tables[0].Rows[0]["DeviceCode"].ToString();
                    txtshopEmailID.Text = ds.Tables[0].Rows[0]["ShopEmailId"].ToString();
                    if (ds.Tables[0].Rows[0]["AgentCountry"].ToString() != "")
                    {
                        ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["AgentCountry"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["AgentState"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["AgentState"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["AgentDistrict"].ToString() != "")
                    {
                        ddlDistrict.SelectedValue = ds.Tables[0].Rows[0]["AgentDistrict"].ToString();
                    }
                    //txtDistrict.Text = ds.Tables[0].Rows[0]["AgentDistrict"].ToString();

                    if (ds.Tables[0].Rows[0]["AgentCity"].ToString() != "")

                    {

                        ddlCity.SelectedValue = ds.Tables[0].Rows[0]["AgentCity"].ToString();
                    }
                    txtPinCode.Text = ds.Tables[0].Rows[0]["AgentPinCode"].ToString();

                    txtEmailID.Text = ds.Tables[0].Rows[0]["PersonalEmailID"].ToString();
                    txtcontact.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtLandline.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();
                    txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtLandlineNo.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();
                    txtdob.Text = ds.Tables[0].Rows[0]["AgentDOB"].ToString();


                    txtshopadd.Text = ds.Tables[0].Rows[0]["ShopAddress"].ToString();
                    if (ds.Tables[0].Rows[0]["ShopCountry"].ToString() != "")
                    {
                        ddlshopCountry.SelectedValue = ds.Tables[0].Rows[0]["ShopCountry"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ShopState"].ToString() != "")
                    {
                        ddlShopState.SelectedValue = ds.Tables[0].Rows[0]["ShopState"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ShopDistrict"].ToString() != "")
                    {
                        ddlShopDistrict.SelectedValue = ds.Tables[0].Rows[0]["ShopDistrict"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ShopCity"].ToString() != "")
                    {
                        ddlShopCity.SelectedValue = ds.Tables[0].Rows[0]["ShopCity"].ToString();
                    }
                    txtshoppin.Text = ds.Tables[0].Rows[0]["ShopPinCode"].ToString();
                    TextBox5.Text = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    txtLandline.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();



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
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("EditRegitsration: GetDetails: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        protected void txtPinCode_TextChanged(object sender, EventArgs e)
        {
            _AgentRegistrationDAL.AgentPincode = txtPinCode.Text.Trim();
            BindDropdownCountryState();
        }
        protected void txtshoppin_TextChanged(object sender, EventArgs e)
        {
            _AgentRegistrationDAL.ShopPinCode = txtshoppin.Text.Trim();
            BindShopCountryState();
        }
        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlCity.DataSource = null;
                ddlCity.SelectedValue = null;
                ddlCity.ClearSelection();

                _AgentRegistrationDAL.AgentDistrict = string.Empty;
                _AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.City;
                _AgentRegistrationDAL.AgentDistrict = ddlDistrict.SelectedValue.ToString();
                //DataSet ds_District = _AgentRegistrationDAL.GetCountryStateCity(UserName, Mode, 0, Convert.ToInt32(_AgentRegistrationDAL.AgentState));
                DataSet ds_City = _AgentRegistrationDAL.GetCountryStateCityD();
                if (ds_City != null && ds_City.Tables.Count > 0 && ds_City.Tables[0].Rows.Count > 0)
                {
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

                _AgentRegistrationDAL.ShopCity = string.Empty;
                _AgentRegistrationDAL.mode = (int)EnumCollection.StateCityMode.City;
                _AgentRegistrationDAL.ShopDistrict = ddlShopDistrict.SelectedValue.ToString();
                //DataSet ds_District = _AgentRegistrationDAL.GetCountryStateCity(UserName, Mode, 0, Convert.ToInt32(_AgentRegistrationDAL.AgentState));
                DataSet ds_City = _AgentRegistrationDAL.GetShopCountryStateCity();
                if (ds_City != null && ds_City.Tables.Count > 0 && ds_City.Tables[0].Rows.Count > 0)
                {
                    ddlShopCity.DataSource = ds_City;
                    ddlShopCity.DataValueField = "City";
                    ddlShopCity.DataTextField = "City";
                    ddlShopCity.DataBind();
                    ddlShopCity.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    ddlShopCity.DataSource = null;
                    ddlShopCity.DataBind();
                    ddlShopCity.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : ddlShopDistrict_SelectedIndexChanged() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.', 'Alert');", true);
            }
        }
        public void FillAggregator()
        {
            try
            {
                ddlaggregatorCode.Items.Clear();
                ddlaggregatorCode.DataSource = null;
                ddlaggregatorCode.DataBind();
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
                        ddlaggregatorCode.DataSource = dsbc;
                        ddlaggregatorCode.DataValueField = "aggcode";
                        ddlaggregatorCode.DataTextField = "aggname";
                        ddlaggregatorCode.DataBind();
                    }
                    else
                    {
                        ddlaggregatorCode.DataSource = dsbc;
                        ddlaggregatorCode.DataValueField = "aggcode";
                        ddlaggregatorCode.DataTextField = "aggname";
                        ddlaggregatorCode.DataBind();
                        ddlaggregatorCode.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlaggregatorCode.DataSource = null;
                    ddlaggregatorCode.DataBind();
                    ddlaggregatorCode.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Agent Registration');", true);
                return;
            }
        }
    }
}