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
//using DataAccessLayer;

namespace SOR.Pages.BC
{
    public partial class ReprocessBCDetails : System.Web.UI.Page
    {

        #region Object Declarations

        ClientRegistrationEntity clientMngnt = new ClientRegistrationEntity();
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
        //private static DbAccess _dbAccess = new DbAccess();

        BCEntity _BCEntity = new BCEntity();
        EmailAlerts _EmailAlerts = new EmailAlerts();
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

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack == true)
            {
                fillClient();
                BindDropdownCountryState();
           
                string BCReqId = Page.Request.QueryString["BCReqId"].ToString();
                string RequestType = Page.Request.QueryString["RequestType"].ToString();
                // Session["BCReqId"] = BCReqId;
                GetDetails(BCReqId);
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


        public void GetDetails(string BCReqId)
        {
            try
            {
                //_BCEntity.BCReqId = HidBCID.Value;
                _BCEntity.BCRequest = BCReqId;

                DataSet ds = _BCEntity.GetOnboradingbcDetails();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["State"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["District"].ToString() != "")
                    {
                        ddlDistrict.SelectedValue = ds.Tables[0].Rows[0]["District"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["City"].ToString() != "")
                    {
                        ddlCity.SelectedValue = ds.Tables[0].Rows[0]["City"].ToString();
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
                    txtFirstName.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
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
                    if (ds.Tables[0].Rows[0]["dmt"].ToString() != "" || ds.Tables[0].Rows[0]["dmt"].ToString() != null)
                    {
                        //  HdnAEPS.Value = ds.Tables[0].Rows[0]["AEPS"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["dmt"].ToString() == "1")
                    {
                        chkdmt.Checked = true;
                    }
                    else
                    {
                        chkdmt.Checked = false;
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
                ErrorLog.CommonError(Ex);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);

                return;
            }
        }
        #endregion

        protected void btnSubmitDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (HiddenField1.Value.ToString() == "Yes")
                {
                    string Status = string.Empty;
                    string StatusMsg = string.Empty;
                    int RequestId = 0;
                    Session["PanNo"] = txtPANNo.Text;
                    
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Data Submitted Successfully', 'BC Registration');", true);

                    txtPANNo.Text = !string.IsNullOrEmpty(hidPan.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidPan.Value)) : txtPANNo.Text;
                    txtaadharno.Text = !string.IsNullOrEmpty(hidAadh.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAadh.Value)) : txtaadharno.Text;
                    txtGSTNo.Text = !string.IsNullOrEmpty(hidSgst.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidSgst.Value)) : txtGSTNo.Text;
                    txtAccountNumber.Text = !string.IsNullOrEmpty(hidAccNo.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAccNo.Value)) : txtAccountNumber.Text;
                    txtIFsccode.Text = !string.IsNullOrEmpty(hidAccIFC.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidAccIFC.Value)) : txtIFsccode.Text;
                    if (ValidateSetProperties())
                    {
                        _salt = _AppSecurity.RandomStringGenerator();
                        _BCEntity.ForAEPS = chkAEPS.Checked == true ? 1 : 0;
                        _BCEntity.ForDMT = chkdmt.Checked == true ? 1 : 0;
                        _BCEntity.ForMicroATM = chkMATM.Checked == true ? 1 : 0;
                        _BCEntity.FirstName = txtFirstName.Text.Trim();
                        _BCEntity.MiddleName = txtMiddleName.Text.Trim();
                        _BCEntity.LastName = txtLastName.Text.Trim();
                        _BCEntity.PersonalEmail = txtEmailID.Text.Trim();
                        _BCEntity.AadharNo = txtaadharno.Text.Trim();
                        _BCEntity.PanNo = txtPANNo.Text.Trim();
                        _BCEntity.GSTNo = txtGSTNo.Text.Trim();
                        _BCEntity.RegisteredAddress = txtRegisteredAddress.Text.Trim();
                        _BCEntity.BcCategory = ddlCategory.SelectedItem.ToString();
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
                        _BCEntity.ClientId = ddlclient.SelectedValue.ToString();
                        _BCEntity.Gender = ddlGender.SelectedValue.ToString();
                        _BCEntity.Category = ddlCategory.SelectedValue.Trim();
                        _BCEntity.CreatedBy = Session["Username"].ToString();
                        _BCEntity.Flag = 4;
                        _BCEntity.ClientId = Session["Client"].ToString();
                        string BCReqId = Page.Request.QueryString["BCReqId"].ToString();
                        _BCEntity.Activity = Page.Request.QueryString["RequestType"].ToString();
                        _BCEntity.BCReqId = HidBCID.Value != null && !string.IsNullOrEmpty(HidBCID.Value) ? Convert.ToString(HidBCID.Value) : Convert.ToString(Page.Request.QueryString["BCReqId"]);
                        // _BCEntity.BCReqId = BCReqId;

                        if (_BCEntity.Insert_BCRequest(Convert.ToString(Session["Username"]), out RequestId, out Status, out StatusMsg))
                        {
                            _BCEntity.BCReqId = Page.Request.QueryString["BCReqId"].ToString();
                            DataSet ds = _BCEntity.GetDocs();

                            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                            {
                                divAddressProof1.Visible = true;
                                divIdProof1.Visible = true;
                                divSigProof1.Visible = true;
                                string idthumb = ds.Tables[0].Rows[0]["IdentityProofDocument"].ToString();
                                pathlnkId = "../../" + idthumb;
                                Session["IdFilePath"] = idthumb;

                                string idType = ds.Tables[0].Rows[0]["IdentityProofType"].ToString();

                                string Addthumb = ds.Tables[0].Rows[0]["AddressProofDocument"].ToString();
                                PathlnkAdd = "../../" + Addthumb;
                                Session["AddFilePath"] = Addthumb;

                                string AddType = ds.Tables[0].Rows[0]["AddressProofType"].ToString();

                                string Sigthumb = ds.Tables[0].Rows[0]["SignatureProofDocument"].ToString();
                                PathlnkSig = "../../" + Sigthumb;
                                Session["SigFilePath"] = Sigthumb;

                                string SigType = ds.Tables[0].Rows[0]["SignatureProofType"].ToString();

                                ddlIdentityProof.SelectedValue = idType;
                                ddlAddressProof.SelectedValue = AddType;
                                ddlSignature.SelectedValue = SigType;

                            }

                            HidBCID.Value = RequestId.ToString();
                            div_Upload.Visible = true;
                            DIVDetails.Visible = false;
                            DivBcDetails.Visible = false;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Request Processed for Document Upload', 'BC Registration');", true);
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
                ErrorLog.CommonTrace("Class : AgentRegistration.cs \nFunction : btnSubmitDetails_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Registration');", true);
                return;
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
                ErrorLog.CommonTrace(Ex.Message);
            }
            return pageFilters;
        }

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
                    else _BCEntity.FileName = txtFirstName.Text.ToString();


                // PanNo
                if (hd_txtPANNo.Value == "1" || !string.IsNullOrEmpty(txtPANNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.PanCard, txtPANNo.Text))
                    {
                        txtPANNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid PAN Card.', ' PAN Card');", true);
                        return false;
                    }
                    else _BCEntity.PanNo = txtPANNo.Text.Trim();

              

                // AccountNumber
                if (hd_txtAccountNumber.Value == "1" || !string.IsNullOrEmpty(txtAccountNumber.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.NumberOnly, txtAccountNumber.Text))
                    {
                        txtAccountNumber.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Account Number.', 'AccountNumber');", true);
                        return false;
                    }
                    else _BCEntity.AccountNumber = txtAccountNumber.Text.ToString();

                // IFSCCode
                if (hd_txtIFsccode.Value == "1" || !string.IsNullOrEmpty(txtIFsccode.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.TextWithNumbers, txtIFsccode.Text))
                    {
                        txtIFsccode.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid IFSC Code.', 'IFSC Code');", true);
                        return false;
                    }
                    else _BCEntity.IFSCCode = txtIFsccode.Text.Trim().ToString();

                // Registered Address
                if (hd_txtRegisteredAddress.Value == "1" || !string.IsNullOrEmpty(txtRegisteredAddress.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Address, txtRegisteredAddress.Text))
                    {
                        txtRegisteredAddress.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Address.', 'Registered Address');", true);
                        return false;
                    }
                    else _BCEntity.RegisteredAddress = txtRegisteredAddress.Text.Trim();

                // Pincode
                if (hd_txtPinCode.Value == "1" || !string.IsNullOrEmpty(txtPinCode.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Pincode, txtPinCode.Text))
                    {
                        txtPinCode.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Pincode.', 'Pincode');", true);

                        return false;
                    }
                    else _BCEntity.Pincode = txtPinCode.Text.Trim();

                // Country
                //if (ddlCountry.SelectedValue == "0")
                //{
                //    ddlCountry.Focus();
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select Country.', 'Country');", true);
                //    return false;
                //}
                //else _BCEntity.Country = ddlCountry.SelectedValue.ToString().Trim();

                // State
                if (ddlState.SelectedValue == "0")
                {
                    ddlState.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select State.', 'State');", true);
                    return false;
                }
                else _BCEntity.State = ddlState.SelectedValue.ToString().Trim();

                // City
                if (ddlCity.SelectedValue == "0")
                {
                    ddlCity.Focus();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select City.', 'City');", true);
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
                else _BCEntity.District = ddlDistrict.SelectedValue.ToString().Trim();




                // Personal Contact
                if (hd_txtContactNo.Value == "1" || !string.IsNullOrEmpty(txtContactNo.Text))
                    if (!_CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, txtContactNo.Text))
                    {
                        txtContactNo.Focus();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "funShowOnboardDiv()", true);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Invalid Contact No.', 'Personal Contact No.');", true);
                        return false;
                    }
                    else _BCEntity.PersonalContact = txtContactNo.Text.Trim();

              

                if (chkAEPS.Checked == false && chkMATM.Checked == false && chkdmt.Checked == false)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select Atleast One Service','Warning');", true);
                    return false;
                }


            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Page : ClientRegistration.cs \nFunction : ValidateSetProperties\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong..! Please try again','BC Registration');", true);
                return false;
            }
            return true;
        }
        #endregion

        #region Bind Dropdown
        public void BindDropdownCountry()
        {
            try
            {
                _BCEntity.Mode = "getAllData";
                DataSet dsCountry = _BCEntity.getCountryAndDocument();
                if (dsCountry != null && dsCountry.Tables.Count > 0 && dsCountry.Tables[0].Rows.Count > 0)
                {
                    ddlCountry.DataSource = dsCountry.Tables[0];
                    ddlCountry.DataValueField = "ID";
                    ddlCountry.DataTextField = "Name";
                    ddlCountry.DataBind();
                    ddlCountry.Items.Insert(0, new ListItem("-- Select --", "0"));

                }
                else
                {
                    ddlCountry.DataSource = null;
                    ddlCountry.DataBind();
                    ddlCountry.Items.Insert(0, new ListItem("No Data Found", "0"));

                }
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Page : ClientRegistration.cs \nFunction : BindDropdownCountry()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Client Registration');", true);
                return;
            }
        }
        #endregion



        #region Method
        public void fillClient()
        {
            DataSet _dsClient = new DataSet();
            try
            {
                ddlclient.Items.Clear();
                ddlclient.DataSource = null;
                ddlclient.DataBind();

                clientMngnt.UserName = Session["Username"].ToString();
                clientMngnt.IsRemoved = "0";
                clientMngnt.IsActive = "1";
                clientMngnt.IsdocUploaded = "1";
                clientMngnt.VerificationStatus = "1";
                clientMngnt.SFlag = (int)EnumCollection.EnumPermissionType.EnableRoles;

                _dsClient = clientMngnt.BindClient();
                if (_dsClient != null && _dsClient.Tables.Count > 0)
                {
                    if (_dsClient.Tables.Count > 0 && _dsClient.Tables[0].Rows.Count > 0)
                    {
                        if (_dsClient.Tables[0].Rows.Count == 1)
                        {
                            ddlclient.Items.Clear();
                            ddlclient.DataSource = _dsClient.Tables[0].Copy();
                            ddlclient.DataTextField = "ClientName";
                            ddlclient.DataValueField = "ClientID";
                            ddlclient.DataBind();
                            ddlclient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
                        }
                        else
                        {
                            ddlclient.Items.Clear();
                            ddlclient.DataSource = _dsClient.Tables[0].Copy();
                            ddlclient.DataTextField = "ClientName";
                            ddlclient.DataValueField = "ClientID";
                            ddlclient.DataBind();
                            ddlclient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
                        }
                    }
                    else
                    {
                        ddlclient.Items.Clear();
                        ddlclient.DataSource = null;
                        ddlclient.DataBind();
                        ddlclient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
                    }
                }
                else
                {
                    ddlclient.Items.Clear();
                    ddlclient.DataSource = null;
                    ddlclient.DataBind();
                    ddlclient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Client --", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : BCRegistration.cs \nFunction : fillClient() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Registration');", true);


            }
        }
        #endregion Bind Client 

        #region SelectedIndexChanged
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _BCEntity.Mode = "getStateAndCity";
                _BCEntity.Country = ddlCountry.SelectedValue.ToString();
                DataSet ds_Country = _BCEntity.getStateAndCity();
                if (ds_Country != null && ds_Country.Tables.Count > 0 && ds_Country.Tables[0].Rows.Count > 0)
                {
                    ddlState.DataSource = ds_Country.Tables[0];
                    ddlState.DataValueField = "ID";
                    ddlState.DataTextField = "Name";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    ddlState.DataSource = null;
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : BCRegistration.cs \nFunction : ddlCountry_SelectedIndexChanged() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Registration');", true);
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlDistrict.DataSource = null;

                _BCEntity.State = string.Empty;
                _BCEntity.mode = (int)EnumCollection.StateCityMode.District;
                _BCEntity.State = ddlState.SelectedValue.ToString();
                //DataSet ds_District = _AgentRegistrationDAL.GetCountryStateCity(UserName, Mode, 0, Convert.ToInt32(_AgentRegistrationDAL.AgentState));
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
                ErrorLog.AgentManagementTrace("AgentRegistration: ddlState_SelectedIndexChanged(): Exception: " + Ex.Message);
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
                //DataSet ds_District = _AgentRegistrationDAL.GetCountryStateCity(UserName, Mode, 0, Convert.ToInt32(_AgentRegistrationDAL.AgentState));
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
                ErrorLog.AgentManagementTrace("Class : AgentRegistration.cs \nFunction : ddlDistrict_SelectedIndexChanged() \nException Occured\n" + Ex.Message);
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
                txtPANNo.Text = null;
                txtGSTNo.Text = null;
                txtRegisteredAddress.Text = null;
                txtContactNo.Text = null;
                txtLandlineNo.Text = null;
                txtaadharno.Text = null;
                ddlCountry.SelectedValue = null;
                ddlState.SelectedValue = null;
                ddlDistrict.SelectedValue = null;
                ddlCity.SelectedValue = null;
                //flgUplodMyImage = null;
                txtAccountNumber.Text = null;
                txtIFsccode.Text = null;
                txtLandlineNo.Text = null;
                txtAlterNateNo.Text = null;
                dvfield_PANNo.Visible = true;
                txtaadharno.Text = null;
                chkAEPS.Checked = false;
                chkdmt.Checked = false;
                chkMATM.Checked = false;
                ddlclient.SelectedValue = null;
                ddlCategory.ClearSelection();


            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : BCRegistration.cs \nFunction : ClearAllControls() \nException Occured\n" + Ex.Message);
            }
        }
        #endregion


        protected void Bindreceipt()
        {
            try
            {

                DataSet ds_Receipt = _BCEntity.getReceiptData();
                if (ds_Receipt != null && ds_Receipt.Tables.Count > 0 && ds_Receipt.Tables[0].Rows.Count > 0)
                {
                    txtaadh.Text = ds_Receipt.Tables[0].Rows[0]["AadharNo"].ToString();
                    txtacc.Text = ds_Receipt.Tables[0].Rows[0]["AccountNumber"].ToString();
                    txtadd.Text = ds_Receipt.Tables[0].Rows[0]["BCAddress"].ToString();
                    txtbcname.Text = ds_Receipt.Tables[0].Rows[0]["BCName"].ToString();
                    //  string dfdf = ds_Receipt.Tables[0].Rows[0]["BCCategory"].ToString();
                    DDLcat.Text = ds_Receipt.Tables[0].Rows[0]["BCCategory"].ToString();
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
                    hidimgId.Value = pathId;
                    string Addthumb = ds_Receipt.Tables[0].Rows[0]["AddressProofDocument"].ToString();
                    string FileThumbnailAdd = Path.GetDirectoryName(Addthumb) + "\\" + Path.GetFileNameWithoutExtension(Addthumb) + "_Thumbnail.png";
                    PathAdd = "../../" + FileThumbnailAdd;
                    hidimgAdd.Value = PathAdd;
                    string Sigthumb = ds_Receipt.Tables[0].Rows[0]["SignatureProofDocument"].ToString();
                    string FileThumbnailSig = Path.GetDirectoryName(Sigthumb) + "\\" + Path.GetFileNameWithoutExtension(Sigthumb) + "_Thumbnail.png";
                    PathSig = "../../" + FileThumbnailSig;
                    hidimgSig.Value = PathSig;
                    ViewState["BCReqId"] = ds_Receipt.Tables[0].Rows[0]["BCReqID"].ToString();
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (HiddenField2.Value.ToString() == "Yes")
                {
                    _BCEntity.Flag = (int)EnumCollection.DBFlag.Update;
                    _BCEntity.ClientId = Session["Client"].ToString();
                    _BCEntity.CreatedBy = Session["Username"].ToString();
                    _BCEntity.IdentityProofDocument = Session["IdFilePath"].ToString();

                    _BCEntity.IdentityProofType = ddlIdentityProof.SelectedValue;
                    _BCEntity.AddressProofDocument = Session["AddFilePath"].ToString();
                    _BCEntity.AddressProofType = ddlAddressProof.SelectedValue;
                    _BCEntity.SignatureProofDocument = Session["SigFilePath"].ToString();
                    _BCEntity.SignatureProofType = ddlSignature.SelectedValue;
                    _BCEntity.BCReqId = HidBCID.Value;

                    if (_BCEntity.Insert_BCRequest(Convert.ToString(Session["Username"]), out int RequestId, out string Status, out string StatusMsg))
                    {
                        ErrorLog.AgentManagementTrace("AgentRegistration: BtnSubmit_Click: Successful - Upload Documents. UserName: " + UserName + " Status: " + Status + " StatusMsg: " + StatusMsg + " RequestId: " + RequestId);
                        DivBcDetails.Visible = true;
                        _BCEntity.BCCode = RequestId.ToString();
                        HidBCID.Value = RequestId.ToString();
                        Bindreceipt();
                        div_Upload.Visible = false;
                        DivBcDetails.Visible = true;
                        DIVDetails.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Uploaded Successfully', 'BC Registration');", true);
                    }
                    else
                    {
                        ErrorLog.AgentManagementTrace("AgentRegistration: BtnSubmit_Click: Failed - Upload Documents. UserName: " + UserName + " Status: " + Status + " StatusMsg: " + StatusMsg + " RequestId: " + RequestId);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('" + StatusMsg + "', 'Alert');", true);
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Class : BCRegistration.cs \nFunction : btnSubmitDetails_Click() \nException Occured\n" + Ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Registration');", true);
                return;
            }
        }

        protected void btnCloseReceipt_Click(object sender, EventArgs e)
        {

            if (HiddenField1.Value.ToString() == "Yes")
            {
                DIVDetails.Visible = true;
                DivBcDetails.Visible = false;
                ddlSignature.SelectedValue = null;
                ddlAddressProof.ClearSelection();
                ddlIdentityProof.ClearSelection();
                string val = HidBCID.Value;
                GetDetails(val);
            }
            else
            {

            }
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
                ErrorLog.CommonTrace("Class : PgClicentdocumentUplolad.cs \nFunction : SaveFile() \nException Occured\n" + Ex.Message);
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
                ErrorLog.CommonTrace("Class : PgClicentdocumentUplolad.cs \nFunction : SaveFile() \nException Occured\n" + Ex.Message);
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
                ErrorLog.CommonTrace("Class : PgClicentdocumentUplolad.cs \nFunction : SaveFile() \nException Occured\n" + Ex.Message);
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
                ErrorLog.CommonTrace("Class : frmBCDocumentUpload.cs \nFunction : ValidateFile \nException Occured\n" + Ex.Message);
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
                ErrorLog.CommonTrace("Class : frmBCDocumentUpload.cs \nFunction : ValidateFile \nException Occured\n" + Ex.Message);
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
            catch (Exception)
            {
                throw;
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
                ErrorLog.CommonTrace("Class : frmBCDocumentUpload.cs \nFunction : IsValidImage() \nException Occured\n" + ex.Message);
                return false;
            }
        }
        #endregion

        protected void ProcessBCData_Click(object sender, EventArgs e)
        {
            if (HiddenField1.Value.ToString() == "Yes")
            {
                if (ChkConfirmBC.Checked == true)
                {
                   div_Upload.Visible = false;
                   DivBcDetails.Visible = false;

                    DivBcDetails.Visible = true;
                    _BCEntity.CreatedBy = Session["Username"].ToString();
                   _BCEntity.BCReqId = HidBCID.Value.ToString();
                    _BCEntity.Activity = Convert.ToString(Session["RequestType"]);
                    _BCEntity.Flag = 2;
                    //DataSet dsBCMaster = _BCEntity.SetInsertUpdateBCTrackerDetails();
                    //if (dsBCMaster != null && dsBCMaster.Tables.Count > 0 && dsBCMaster.Tables[0].Rows[0]["Status"].ToString() == "Inserted")
                    //{
                    //    //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showError('Data Registered Successfully', 'BC Registration');", true);
                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Registered Successfully', 'BC Registration');", true);
                    //    div_Upload.Visible = false;
                    //    divOnboardFranchise.Visible = false;
                    //    DivBcDetails.Visible = false;
                    //    DIVDetails.Visible = false;
                    //    ClearAllControls();
                    //     Session["AGCode"] = string.Empty;
                    //     Session["BCReqId"] = string.Empty;
                    //     Response.Redirect("~/Pages/BC/BCRegistration.aspx", false);
                    //     //HttpContext.Current.ApplicationInstance.CompleteRequest();
                    // }
                    // else
                    // {
                    //     ClearAllControls();
                    // }
                    string statusMessage = _BCEntity.SetInsertUpdateBCTrackerDetails();

                    if (!string.IsNullOrEmpty(statusMessage) && statusMessage == "Inserted")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Data Registered Successfully', 'BC Registration');", true);
                        div_Upload.Visible = false;
                        divOnboardFranchise.Visible = false;
                        DivBcDetails.Visible = false;
                        DIVDetails.Visible = false;
                        ClearAllControls();
                        Session["AGCode"] = string.Empty;
                        Session["BCReqId"] = string.Empty;
                        Response.Redirect("~/Pages/BC/OnBoardBcStatus.aspx", false);
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
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Select Confirmation on all above BC Details are properly filled', 'BC Registration');", true);
                    return;
                }
            }
            else
            {

            }

        }

        protected void btntest_Click(object sender, EventArgs e)
        {
            ModalPopupExtenderEditRequest.Show();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            DIVDetails.Visible = false;
            div_Upload.Visible = false;
            //divPaymentReceipt.Visible = false;
            divOnboardFranchise.Visible = false;
            Response.Redirect("~/Pages/BC/OnBoardBcStatus.aspx", false);
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
                ErrorLog.CommonTrace("Class : BCVerificationLevel1.cs \nFunction : btnViewDownload_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Verification');", true);
                return;
            }
        }

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            string pdfPath = Server.MapPath("~/Thumbnail/Aadhar/document-1_220422_110414 (1) (1).pdf");
            Session["pdfPath"] = pdfPath;
            string script = "<script type='text/javascript'>window.open('" + "PdfExport.aspx" + "')</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);
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
                ErrorLog.CommonTrace("Class : BCVerificationLevel1.cs \nFunction : ImageButton1_Click() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "BCVerificationLevel1.cs", "ImageButton1_Click()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Business Correspondents Verification');", true);
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
                    string fileName = Ds.Tables[2].Rows[0]["SignatureProofType"].ToString();
                    strURL = Ds.Tables[2].Rows[0]["SignatureProofDocument"].ToString();
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
                ErrorLog.CommonTrace("Class : BCVerificationLevel1.cs \nFunction : ImageButton2_Click() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "BCVerificationLevel1.cs", "ImageButton2_Click()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Business Correspondents Verification');", true);
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
                    string fileName = Ds.Tables[3].Rows[0]["DocumentType"].ToString(); 
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
                ErrorLog.CommonTrace("Class : BCVerificationLevel1.cs \nFunction : imgbtnform_Click() \nException Occured\n" + Ex.Message);
                //_dbAccess.StoreErrorDescription(UserName, "BCVerificationLevel1.cs", "imgbtnform_Click()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Business Correspondents Verification');", true);
                return;
            }
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
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Page : AgentRegistration.cs \nFunction : BindDropdownCountry()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Client Registration');", true);
                return;
            }
        }
        #endregion

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                DIVDetails.Visible = false;
                div_Upload.Visible = false;
                DivBcDetails.Visible = false;
               // divPaymentReceipt.Visible = false;
                string val = HidBCID.Value;
                GetDetails(val);
               // GetDetailsBack(val);
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Page : EditBCRegistration.cs \nFunction : BindDropdownCountry()\nException Occured\n" + ex.Message);
            }
        }

        public void GetDetailsBack(string val)
        {
            try
            {
                _BCEntity.BCRequest = val;
               
                DataSet ds = _BCEntity.GetOnboradingbcDetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
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
                    if (ds.Tables[0].Rows[0]["State"].ToString() != "")
                    {
                        ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["District"].ToString() != "")
                    {
                        ddlDistrict.SelectedValue = ds.Tables[0].Rows[0]["District"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["City"].ToString() != "")
                    {
                        ddlCity.SelectedValue = ds.Tables[0].Rows[0]["City"].ToString();
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
                ErrorLog.CommonError(Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong.Please try again','Warning');", true);
                return;
            }
        }

        protected void EyeImage_Click(object sender, ImageClickEventArgs e)
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
                ErrorLog.BCManagementTrace("Class : ReprocessBCDetails.cs \nFunction : DownloadDocTwo_Click() \nException Occured\n" + Ex.Message);
                ////_dbAccess.StoreErrorDescription(UserName, "BCRegistration.cs", "ImageButton1_Click()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Registration');", true);
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
                ErrorLog.BCManagementTrace("Class : ReprocessBCDetails.cs \nFunction : DownloadDocThree_Click() \nException Occured\n" + Ex.Message);
                ////_dbAccess.StoreErrorDescription(UserName, "BCRegistration.cs", "ImageButton2_Click()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Registration');", true);
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
                ErrorLog.BCManagementTrace("Class : ReprocessBCDetails.cs \nFunction : DownloadDocOne_Click() \nException Occured\n" + Ex.Message);
                ////_dbAccess.StoreErrorDescription(UserName, "BCRegistration.cs", "ImageButton2_Click()", Ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Registration');", true);
                return;
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
                ErrorLog.CommonTrace("Class : BCVerificationLevel1.cs \nFunction : btnViewDownload_Click() \nException Occured\n" + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'BC Verification');", true);
                return;
            }
        }
        protected void txtPinCode_TextChanged(object sender, EventArgs e)
        {
            _BCEntity.Pincode = txtPinCode.Text.Trim();
            BindDropdownCountryState();
        }
    }
}



