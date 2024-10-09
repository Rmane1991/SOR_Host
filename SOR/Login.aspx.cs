using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Configuration;
using MaxiSwitch.EncryptionDecryption;
using AppLogger;
using BussinessAccessLayer;
using System.Threading;

namespace PayRakamSBM
{
    public partial class Login : System.Web.UI.Page
    {
        #region Objects Declaration
        EmailSMSAlertscs _EmailSMSAlertscs = new EmailSMSAlertscs();
        LoginEntity _LoginEntity = new LoginEntity();
        EmailAlerts _EmailAlerts = new EmailAlerts();
        AppSecurity _AppSecurity = new AppSecurity();

        string _ChangePasswordResponse = string.Empty, _Status = string.Empty, _ChangepasswordResponse = string.Empty;
        string[] _auditParams = new string[3];
        string[] _attemptAction = new string[2];
        string
            _strUserName = string.Empty,
            _strPassWord = string.Empty,
            _strEncryptedPassword = string.Empty;

        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtUserName.Text = "Maximus";

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!Page.IsPostBack)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);

                divMainLoginBox.Visible = true;
                divForgrtOTP.Visible = false;
                // divChangePwd.Visible = false;            
            }
            Session["Provider"] = ConfigurationManager.AppSettings["Provider"].ToString();
            Session["ConnectionString"] = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Session["DataEncryptionKey"] = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.GetDataEncryptionKey(Session["ConnectionString"].ToString(), Session["Provider"].ToString()) : "";
            Session["filedir"] = @"E:\SBM mATM\Logs\";
            Session["BankName"] = "SBM";
        }
        #endregion

        #region Buttom Event : Login
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
             {
                LoginMe();
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Login Request Failed (Login Page). Exception : " + Ex.Message + " Username : " + txtUserName.Text);
                ErrorLog.CommonError(Ex);
                Response.Write("<script>alert('Something Went Wrong. Try again later');</script>");
                return;
            }
        }

        protected void LoginMe()
        {
            try
            {
                clsCustomeRegularExpressions _CustomeRegExpValidation = new clsCustomeRegularExpressions();
                DataSet _dsValiDateUser = new DataSet();
                string OutStatus = null; string OutStatusMsg = null;

                _strPassWord = !string.IsNullOrEmpty(hidpassword.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidpassword.Value)) : txtPassword.Value;
                //_strPassWord = "admin@1234";// !string.IsNullOrEmpty(hidpassword.Value) ? AppSecurity.UnMaskString(_AppSecurity.DecryptStringAES(hidpassword.Value)) : txtPassword.Value;
                _strUserName = txtUserName.Text;
                #region Track Users
                _LoginEntity.UserName = _strUserName;
                _LoginEntity.GeoDetails = hidInfo.Value;
                _LoginEntity.TrackUsers(out OutStatus, out OutStatusMsg);
                #endregion
                if (!string.IsNullOrEmpty(_strUserName) && _CustomeRegExpValidation.CustomeRegExpValidation(clsCustomeRegularExpressions.Validators.Mobile, _strUserName))
                {
                    _LoginEntity.MobileNo = _strUserName;
                    _LoginEntity.GetUsernameByMobileNo(out OutStatus, out OutStatusMsg);
                    if (OutStatus == "00" && !string.IsNullOrEmpty(OutStatusMsg))
                    {
                        _strUserName = OutStatusMsg;
                    }
                    else if (OutStatus == "00" && string.IsNullOrEmpty(OutStatusMsg))
                    {
                        ErrorLog.CommonTrace("Fetching Username By Mobile No (" + _strUserName + "). Mobile no is Exist But Username Not Mapped.");
                    }
                    else if (OutStatus == "11")
                    {
                        ErrorLog.CommonTrace("Fetching Username By Mobile No(" + _strUserName + "). Mobile Number Not Exist In The System.");
                    }
                }
                if (string.IsNullOrEmpty(_strUserName))
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.InnerHtml = "Please Enter Username";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "HideLabel();", true);
                    return;
                }
                if (string.IsNullOrEmpty(_strPassWord))
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.InnerHtml = "Please Enter Password";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "HideLabel();", true);
                    return;
                }
                else
                {
                    _LoginEntity.UserName = _strUserName;
                    DataSet _dsLoginAttempt = _LoginEntity.GetLoginAttemptCount();
                    try
                    {
                        if (_dsLoginAttempt != null && _dsLoginAttempt.Tables.Count > 0 && _dsLoginAttempt.Tables[0].Rows[0]["IsAttemptFailed"].ToString().Trim() != "3")
                        {
                            _strEncryptedPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_dsLoginAttempt.Tables[0].Rows[0]["Salt"].ToString() + _strPassWord);
                            // LoginEntity.OTP = txtOTP.Text;
                            _LoginEntity.UserName = _strUserName;
                            _LoginEntity.Password = _strEncryptedPassword;
                            _dsValiDateUser = _LoginEntity.Login();

                            if (_dsValiDateUser != null && _dsValiDateUser.Tables.Count > 0 && _dsValiDateUser.Tables[0].Rows.Count > 0)
                            {
                                Session["LoggedIn"] = _strUserName;
                                Session["Username"] = _strUserName;
                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;
                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                Response.Cookies["AuthToken"].Path = "SBM";
                                string IsPwdChange = _dsValiDateUser.Tables[0].Rows[0]["ChangePWD"].ToString().Trim();

                                if (IsPwdChange != "FirstLogin")
                                {
                                    #region Login Successful
                                    if (_dsValiDateUser != null && _dsValiDateUser.Tables.Count > 0 && _dsValiDateUser.Tables[0].Rows.Count > 0)
                                    {
                                        try
                                        {
                                            #region Login Success
                                            _auditParams[0] = Convert.ToString(_strUserName);
                                            _auditParams[1] = "User Login";
                                            _auditParams[2] = Convert.ToString(_strUserName) + " Login Successfully.";
                                            _LoginEntity.StoreLoginActivities(_auditParams);
                                            ErrorLog.CommonTrace("Login Successful. Username " + _strUserName + ". SessionID : " + HttpContext.Current.Session.SessionID);
                                            Session["UserID"] = _dsValiDateUser.Tables[0].Rows[0]["UserID"].ToString().Trim();
                                            Session["BCAgentID"] = _dsValiDateUser.Tables[0].Rows[0]["UserName"].ToString().Trim();
                                            Session["RoleType"] = _dsValiDateUser.Tables[0].Rows[0]["RoleType"].ToString().Trim();
                                            Session["UserRoleID"] = _dsValiDateUser.Tables[0].Rows[0]["UserRoleID"].ToString().Trim();
                                            Session["ParentRoleID"] = _dsValiDateUser.Tables[0].Rows[0]["ParentRoleID"].ToString().Trim();
                                            Session["HomePage"] = _dsValiDateUser.Tables[0].Rows[0]["homepage"].ToString().Trim().Replace("../../", string.Empty);
                                            Session["mobileNo"] = _dsValiDateUser.Tables[0].Rows[0]["mobileNo"].ToString().Trim();
                                            // Session["TERMINALID"] = _dsValiDateUser.Tables[0].Rows[0]["TERMINALID"].ToString().Trim();
                                            Session["BBPSAgentID"] = _dsValiDateUser.Tables[0].Rows[0]["BBPSAgentID"].ToString().Trim();
                                            // Session["Client"] = "CL000001";
                                            Session["Client"] = ConfigurationManager.AppSettings["Client"].ToString();
                                            Session["TxnAgentID"] = _dsValiDateUser.Tables[0].Rows[0]["TxnAgentID"].ToString().Trim();
                                            Session["AppVer"] = _dsValiDateUser.Tables[0].Rows[0]["AppVersion"].ToString().Trim();
                                            Session["TxnAgentID"] = _dsValiDateUser.Tables[0].Rows[0]["TxnAgentID"].ToString().Trim();
                                            Session["ClientID"] = _dsValiDateUser.Tables[0].Rows[0]["BankID"].ToString().Trim();
                                            _LoginEntity.UserName = _strUserName;
                                            _LoginEntity.Flag = "RemoveCount";
                                            _LoginEntity.SessionID = HttpContext.Current.Session.SessionID;
                                            _LoginEntity.IsValidLogin = "1";

                                            // Display Username & Balance
                                            if (_dsValiDateUser != null && _dsValiDateUser.Tables.Count > 2 && _dsValiDateUser.Tables[2].Rows.Count > 0)
                                            {
                                                try
                                                {
                                                    Session["DispUsername"] = _dsValiDateUser.Tables[2].Rows[0]["Name"].ToString().Trim();
                                                    Session["DispBalance"] = _dsValiDateUser.Tables[2].Rows[0]["ClosingBalance"].ToString().Trim();
                                                    Session["DispCommission"] = _dsValiDateUser.Tables[2].Rows[0]["TotalCommission"].ToString().Trim();
                                                }
                                                catch (Exception Ex)
                                                {
                                                    ErrorLog.CommonTrace("Display Username & Balance In Home Page. Error While Read Dataset Table:2. " + Ex.Message);
                                                    ErrorLog.CommonError(Ex);
                                                }
                                            }
                                            _LoginEntity.SetLoginAttemptCount();
                                            #endregion

                                            try
                                            {
                                                // Home Page Validation. If User Role Has Not Mapped With Home Page, Then Should Not Allowed User To Login.
                                                if (Session["HomePage"] != null)
                                                {
                                                    try
                                                    {
                                                        Response.Redirect(Session["HomePage"].ToString(), false);
                                                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                                                    }
                                                    catch (ThreadAbortException)
                                                    {
                                                    }
                                                }
                                                else
                                                {
                                                    Response.Redirect(Session["HomePage"].ToString(), false);
                                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                                    ErrorLog.CommonTrace("Login Unsuccessful. Home Page Is Empty. Unable To Process The Login Request. Username : " + _strUserName);
                                                }
                                            }
                                            catch (Exception Ex)
                                            {
                                                ErrorLog.CommonTrace("Class : frmLogin.aspx.cs \nFunction : LoginMe() \nException Occured. \n" + "Unable To Store/Encrypt The Keys In User's Browser. \n" + Ex.Message);
                                            }
                                        }
                                        catch (Exception Ex)
                                        {
                                            ErrorLog.CommonTrace("Login Unsuccessful. Home Page Not Found. Unable To Process The Login Request. Exception : " + Ex.Message + " Username : " + _strUserName + " Home Page Path: " + Session["HomePage"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        _auditParams[0] = Convert.ToString(_strUserName);
                                        _auditParams[1] = "User Login";
                                        _auditParams[2] = Convert.ToString(_strUserName) + " has no Home Page mapped.";
                                        _LoginEntity.StoreLoginActivities(_auditParams);
                                        ErrorLog.CommonTrace("Login Unsuccessful. Home Page Not Mapped. Unable To Process The Login Request. Username : " + _strUserName);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region If First Time Login, Promt To User To Change Default Password.
                                    ErrorLog.CommonTrace("Login successful. First Time Login, Promt To User To Change Default Password. Username : " + _strUserName);
                                    Session["UserID"] = _dsValiDateUser.Tables[0].Rows[0]["UserID"].ToString().Trim();
                                    Session["BCAgentID"] = _dsValiDateUser.Tables[0].Rows[0]["UserName"].ToString().Trim();
                                    //Session["RoleType"] = _dsValiDateUser.Tables[0].Rows[0]["RoleType"].ToString().Trim();
                                    Session["UserRoleID"] = _dsValiDateUser.Tables[0].Rows[0]["UserRoleID"].ToString().Trim();
                                    divMainLoginBox.Visible = false;
                                    divForgrtOTP.Visible = false;
                                    divChangePwd.Visible = true;
                                    SendOTP("Change Password(FristLogin/Expiry/Reset)", false);
                                    #endregion
                                }
                            }
                            else
                            {
                                #region Login Unsuccessful. Invalid Credentials.
                                divMainLoginBox.Visible = true;
                                divForgrtOTP.Visible = false;
                                divChangePwd.Visible = false;
                                lblErrorMsg.Visible = true;
                                lblErrorMsg.InnerHtml = "Incorrect Username or Password";
                                _LoginEntity.UserName = _strUserName;
                                _LoginEntity.Flag = "SetCount";
                                _LoginEntity.SetLoginAttemptCount();
                                txtCaptcha.Text = "";
                                _auditParams[0] = Convert.ToString(_strUserName);
                                _auditParams[1] = "User Login";
                                _auditParams[2] = Convert.ToString(_strUserName) + "Login Unsuccessful. Invalid Credentials.";
                                _LoginEntity.StoreLoginActivities(_auditParams);
                                ErrorLog.CommonTrace("Login Unsuccessful. Invalid Credentials. Username : " + _strUserName);
                                return;
                                #endregion
                            }
                        }
                        else
                        {
                            #region Reached maximum attempts, Account has been locked.
                            _auditParams[0] = Convert.ToString(_strUserName);
                            _auditParams[1] = "User Login";
                            _auditParams[2] = "Unsuccessful";
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            ErrorLog.CommonTrace("Login Unsuccessful. Username " + _strUserName + ". Reached maximum attempts, Account has been locked");
                            _attemptAction[0] = Convert.ToString(_strUserName);
                            _attemptAction[1] = "Unsuccessful";
                            _LoginEntity.updateAttemptFailed(_attemptAction);
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('You have tried 3 failed login attempts. Your account has been blocked .Kindly contact Admin.');", true);
                            txtUserName.Text = null;
                            txtPassword.Value = null;
                            #endregion
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.CommonTrace("Login Request Failed (Login Page). Exception : " + Ex.Message + " Username : " + txtUserName.Text);
                        ErrorLog.CommonError(Ex);
                        //Response.Write("<script>alert('Invalid Username/Password. Try again later');</script>");
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.InnerHtml = "Incorrect Username or Password";
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Login Request Failed (Login Page). Exception : " + Ex.Message + " Username : " + txtUserName.Text);
                ErrorLog.CommonError(Ex);
                Response.Write("<script>alert('Something Went Wrong. Try again later');</script>");
                return;
            }
        }

        protected void btnserversidehit_Click(object sender, EventArgs e)
        {
            try
            {
                hidpassword.Value = _AppSecurity.Encrypt(txtPassword.Value);
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Login Request Failed (Login Page). Exception : " + Ex.Message + " Username : " + txtUserName.Text);
                ErrorLog.CommonError(Ex);
                Response.Write("<script>alert('Something Went Wrong. Try again later');</script>");
                return;
            }
        }
        #endregion

        #region Change Password (Frist Login/Expiry/Reset) Event
        protected void btnChangePwd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (txtold.Text.Trim() == txtnew.Text.Trim())
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(' New Password Cannot be same as Old Password')", true);
                    txtnew.Text = string.Empty;
                }
                if (string.IsNullOrEmpty(txtOTPFirstLogin.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(' Please enter OTP')", true);
                    txtOTPFirstLogin.Text = string.Empty;
                }
                if (chkConfimartion.Checked == false)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('You must accept Terms and Conditions.')", true);
                    return;
                }
                else
                {
                    if (txtnew.Text.Trim() == txtconfirm.Text.Trim())
                    {
                        ErrorLog.CommonTrace("Change Password (FristLogin/Expiry/Reset) Request (Login Page) Received. Username : " + txtUserName.Text);

                        if (ValidateOTP("Change Password (FristLogin/Expiry/Reset)", txtOTPFirstLogin.Text.Trim()))
                        {
                            _LoginEntity.UserName = Session["Username"].ToString();
                            DataSet _dsLoginAttempt = _LoginEntity.GetLoginAttemptCount();
                            _LoginEntity.oldPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_dsLoginAttempt.Tables[0].Rows[0]["Salt"].ToString() + txtold.Text.Trim());
                            _LoginEntity.NewPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_dsLoginAttempt.Tables[0].Rows[0]["Salt"].ToString() + txtnew.Text.Trim());
                            _LoginEntity.ConfirmPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_dsLoginAttempt.Tables[0].Rows[0]["Salt"].ToString() + txtconfirm.Text.Trim());
                            _LoginEntity.CreatedBy = Session["UserID"].ToString().Trim();
                            _LoginEntity.UsersID = int.Parse(Session["UserID"].ToString().Trim());

                            DataSet ds = _LoginEntity.SetChangePassword();
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Type"].ToString() == "Save")
                            {
                                ErrorLog.CommonTrace("Change Password (Frist Login/Expiry) Request Successful (Login Page). Username : " + txtUserName.Text);
                                try
                                {
                                    //Audit Log
                                    _auditParams[0] = Convert.ToString(Session["Username"]);
                                    _auditParams[1] = " Password Change";
                                    _auditParams[2] = Convert.ToString(Session["Username"]) + " Password Changed successfully.";
                                    _LoginEntity.StoreLoginActivities(_auditParams);
                                }
                                catch (Exception Ex)
                                {
                                    ErrorLog.CommonTrace("Change Password (Frist Login/Expiry) Request Successful (Login Page). Exception While Do Audit Entry In DB : " + Ex.Message + " Username : " + txtUserName.Text);
                                    ErrorLog.CommonError(Ex);
                                }
                                txtCaptcha.Text = "";
                                divMainLoginBox.Visible = true;
                                divForgrtOTP.Visible = false;
                                divChangePwd.Visible = false;
                                divChangePwdForForget.Visible = false;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password has been changed successfully.')", true);
                            }
                            else
                            {
                                ErrorLog.CommonTrace("Change Password (Frist Login/Expiry) Request Failed (Login Page). Old Credentials Did Not Match. Username : " + txtUserName.Text);
                                divMainLoginBox.Visible = false;
                                divForgrtOTP.Visible = false;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Old Password is wrong, Re-enter old password.')", true);
                            }
                        }
                    }
                    else
                    {
                        txtconfirm.Text = string.Empty;
                        txtnew.Text = string.Empty;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Password Mis-match, re-enter New password and Confirm Password.')", true);
                        divMainLoginBox.Visible = false;
                        divForgrtOTP.Visible = false;
                        //divChangePwd.Visible = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Change Password (Frist Login/Expiry) Request Failed (Login Page). Exception : " + Ex.Message + " Username : " + txtUserName.Text);
                ErrorLog.CommonError(Ex);
                Response.Write("<script>alert('Something Went Wrong. Try again later');</script>");
                return;
            }
        }
        #endregion

        #region Change Password (Forgot Password) Events
        // Send OTP
        protected void lbtnForgotPassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserName.Text.ToString()))
                {
                    txtUserName.Focus();
                    Response.Write("<script>alert('Enter Username.');</script>");
                    return;
                }
                else
                {
                    ErrorLog.CommonTrace("Change Password For Forgot Password Request (OTP) Initiated. Username : " + txtUserName.Text);
                    ProcessForgotPassword();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Change Password For Forgot Password Request (OTP) Failed. Exception : " + Ex.Message + " Username : " + txtUserName.Text);
                ErrorLog.CommonError(Ex);
                //Response.Write("<script>alert('Sorry, something went wrong. Try again later');</script>");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "showError('Something went wrong. Try again.', 'Error');", true);
                return;
            }
        }

        // Resend OTP
        protected void lnkResendOTP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserName.Text.ToString()))
                {
                    txtUserName.Focus();
                    Response.Write("<script>alert('Enter Username.');</script>");
                    return;
                }
                else
                {
                    ErrorLog.CommonTrace("Change Password For Forgot Password Request (Resend OTP) Initiated. Username : " + txtUserName.Text);
                    ProcessForgotPassword();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Change Password For Forgot Password Request (Resend OTP) Failed. Exception : " + Ex.Message + " Username : " + txtUserName.Text);
                ErrorLog.CommonError(Ex);
                Response.Write("<script>alert('Sorry, something went wrong. Try again later');</script>");
                return;
            }
        }

        // Process OTP Request
        private void ProcessForgotPassword()
        {
            string _StatusData;
            string _EmailBody, _EmailId, _Mobilenumber, _RollID, _ClientId, _Subejct, _Status;
            try
            {
                _LoginEntity.UserName = txtUserName.Text;
                _LoginEntity.Flag = "1";
                ErrorLog.CommonTrace("Change Password For Forgot Password Request Received. Username : " + txtUserName.Text);
                _StatusData = _LoginEntity.UserIsExistForForgotPassword();
                if (_StatusData == "USER EXISTS")
                {
                    _StatusData = string.Empty;
                    _LoginEntity.UserName = txtUserName.Text;
                    _LoginEntity.Flag = "2";
                    _StatusData = _LoginEntity.UserIsExistForForgotPassword();
                    if (_StatusData.ToUpper() == "USER EMAIL AND NUMBER EXISTS")
                    {
                        #region Get Email and Mobile Number
                        _LoginEntity.UserName = txtUserName.Text;
                        _LoginEntity.Flag = "3";
                        DataSet _dsGetEmailNumber = _LoginEntity.GetUserDetails();
                        if (_dsGetEmailNumber != null && _dsGetEmailNumber.Tables[0] != null && _dsGetEmailNumber.Tables.Count > 0)
                        {
                            _EmailId = _dsGetEmailNumber.Tables[0].Rows[0]["UserEmailID"].ToString();
                            _Mobilenumber = _dsGetEmailNumber.Tables[0].Rows[0]["mobileNo"].ToString();
                            _RollID = _dsGetEmailNumber.Tables[0].Rows[0]["UserRoleID"].ToString();
                            _ClientId = _dsGetEmailNumber.Tables[0].Rows[0]["BankID"].ToString();
                            ViewState["UserID"] = _dsGetEmailNumber.Tables[0].Rows[0]["UserID"].ToString();

                            #region Genrate autogenrated   OTP
                            Random randomgenrated = new Random();
                            string AutoPassword = (randomgenrated.Next(100000, 1000000)).ToString();
                            ErrorLog.CommonTrace("OTP Generated For Change Password For Forgot Password. OTP : " + AutoPassword + " Username : " + txtUserName.Text);
                            #endregion

                            #region insert OTP Into DB
                            Random random = new Random();
                            _LoginEntity.UserName = txtUserName.Text;
                            _LoginEntity.DeviceID = EnumCollection.enumApplicationType.WEB.ToString();
                            string RRN = GenerateRRN(random);
                            _LoginEntity.RRN = RRN;
                            _LoginEntity.OTP = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(AutoPassword);
                            _LoginEntity.OTPType = EnumCollection.enumOTPType.ForgotPassword.ToString();
                            _LoginEntity.Flag = "1";
                            string DBResp = _LoginEntity.InsertOTPIntoTable();
                            #endregion

                            if (DBResp.ToUpper().Contains("SUCCESS"))
                            {
                                ErrorLog.CommonTrace("Success OTP Insertion In Database For Change Password For Forgot Password. Status : " + DBResp + " OTP : " + AutoPassword + " Username : " + txtUserName.Text + " RRN : " + RRN);

                                #region EMAIL and SMS
                                string EmailResp = string.Empty;
                                string SMSResp = string.Empty;

                                ErrorLog.SMSTrace("Page : Login.cs \nFunction : ProcessForgotPassword() => Login Details forwarded for SMS/Email Preparation. Contact No : " + _Mobilenumber + "Email : " + _EmailId);

                                #region SMS
                                _EmailSMSAlertscs.FROM = "SBMIND";
                                _EmailSMSAlertscs.to = _Mobilenumber;
                                _EmailSMSAlertscs.tempname = "SR24659_BCPOTP";
                                _EmailSMSAlertscs.OTPFlag = "1";
                                _EmailSMSAlertscs.var1 = AutoPassword;
                                _EmailSMSAlertscs.var2 = null;
                                ErrorLog.SMSTrace("Page : Login.cs \nFunction : ProcessForgotPassword() => Login Details forwarded for SMS Preparation. => HttpGetRequest()");
                                EmailResp = _EmailSMSAlertscs.HttpGetRequest();
                                ErrorLog.SMSTrace("Page : Login.cs \nFunction : ProcessForgotPassword() => SMS Response : " + EmailResp);
                                #endregion

                                #region EMAIL
                                _EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                                _EmailSMSAlertscs.to = _EmailId;
                                _EmailSMSAlertscs.tempname = "SR24659_EBCPOTP";
                                _EmailSMSAlertscs.OTPFlag = "1";
                                _EmailSMSAlertscs.var1 = AutoPassword;
                                _EmailSMSAlertscs.var2 = null;
                                ErrorLog.SMSTrace("Page : Login.cs \nFunction : ProcessForgotPassword() => Login Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                                EmailResp = _EmailSMSAlertscs.HttpGetRequestEmail();
                                ErrorLog.SMSTrace("Page : Login.cs \nFunction : ProcessForgotPassword() => Email Response : " + EmailResp);
                                #endregion

                                #endregion

                                //if (EmailResp == "Done" || SMSResp.ToLower().Contains("success"))
                                if (EmailResp.ToUpper().Contains("APP-MAXIMUS") || SMSResp.ToLower().Contains("success"))
                                {
                                    PasswordBox.Visible = false;
                                    btnLogin.Visible = false;
                                    lbtnForgotPassword.Visible = false;
                                    lnkForgetPassword.Visible = false;
                                    lnkResendOTP.Visible = true;
                                    divForgrtOTP.Visible = true;
                                    btnValidateOTP.Visible = true;
                                    if (txtUserName.Text.Trim().ToLower() == "maximus")
                                        Response.Write("<script>alert('OTP has been sent to Registered Email Id.');</script>");
                                    else
                                        Response.Write("<script>alert('OTP has been sent to Registered Mobile Number.');</script>");
                                    return;
                                }
                                else
                                {
                                    ErrorLog.CommonTrace("Failed EMAIL & SMS For Change Password For Forgot Password.  Username : " + txtUserName.Text + " RRN : " + RRN);
                                    Response.Write("<script>alert('Sorry, something went wrong. Try again later');</script>");
                                    return;
                                }
                            }
                            else
                            {
                                ErrorLog.CommonTrace("Failed OTP Insertion In Database For Change Password For Forgot Password. Status : " + DBResp + " OTP : " + AutoPassword + " Username : " + txtUserName.Text + " RRN : " + RRN);
                                Response.Write("<script>alert('Sorry, something went wrong. Try again later');</script>");
                                return;
                            }
                        }
                        else
                        {
                            ErrorLog.CommonTrace("Change Password For Forgot Password Request Failed. Unable To Fetch User Details From Database." + " Username : " + txtUserName.Text);
                            Response.Write("<script>alert('Sorry, unable to process your request. Try again later');</script>");
                            return;
                        }
                        #endregion
                    }
                    else
                    {
                        ErrorLog.CommonTrace("Change Password For Forgot Password Request Failed. Mandatory Fields Should Not Empty (EMAIL And Mobile Number). Database Status : " + _StatusData + " Username : " + txtUserName.Text);
                        Response.Write("<script>alert('Enter valid Email and Mobile Number.');</script>");
                        return;
                    }
                }
                else
                {
                    ErrorLog.CommonTrace("Change Password For Forgot Password Request Failed. Username Not Exists. Database Status : " + _StatusData + " Username : " + txtUserName.Text);
                    Response.Write("<script>alert('Enter valid username.');</script>");
                    return;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.SMSTrace("Page : Login.cs \nFunction : ProcessForgotPassword() => Exception : " + Ex.Message);
                throw;
            }
        }

        private string GenerateRRN(Random random)
        {
            try
            {
                if (txtUserName.Text != null)
                {
                    if (txtUserName.Text.Length > 4)
                    {
                        return DateTime.Now.ToString("yyMMddHHmmssss") + random.Next(111, 999).ToString("D3");
                    }
                    else
                        return _AppSecurity.RandomStringGenerator();
                }
                else
                    return _AppSecurity.RandomStringGenerator();
            }
            catch (Exception ex)
            {
                ErrorLog.CommonTrace("Generate RRN Failed (Login Page). Exception : " + ex.Message);
                return string.Empty;
            }
        }

        // Validate OTP 
        protected void btnValidateOTP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtForgrtOTP.Value.ToString()))
                {
                    Response.Write("<script>alert('Please Enter valid OTP.');</script>");
                    return;
                }
                else
                {
                    ErrorLog.CommonTrace("OTP Validate Request Received. Change Password For Forgot Password Request. Username : " + txtUserName.Text + " OTP : " + txtForgrtOTP.Value.ToString());

                    #region validate OTP Into Table

                    _LoginEntity.UserName = txtUserName.Text;
                    _LoginEntity.OTP = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(txtForgrtOTP.Value);
                    _LoginEntity.Flag = "2";
                    _Status = _LoginEntity.InsertOTPIntoTable();
                    if (_Status == "2")
                    {
                        ErrorLog.CommonTrace("OTP Expired. Change Password For Forgot Password Request. Username : " + txtUserName.Text + " OTP : " + txtForgrtOTP.Value.ToString());
                        txtForgrtOTP.Value = string.Empty;
                        txtUserName.Text = string.Empty;
                        Response.Write("<script>alert('OTP expired click on resend button.');</script>");
                        return;
                    }
                    if (_Status == "0")
                    {
                        ErrorLog.CommonTrace("OTP Validation Successful. Change Password For Forgot Password Request. Username : " + txtUserName.Text + " OTP : " + txtForgrtOTP.Value.ToString());
                        divForgrtOTP.Visible = false;
                        btnValidateOTP.Visible = false;
                        lnkbtnResendOTP.Visible = false;
                        divMainLoginBox.Visible = false;
                        divChangePwdForForget.Visible = true;
                        Response.Write("<script>alert('OTP validated succesfully. Please change your password.');</script>");
                        return;
                    }
                    if (_Status == "Maximum Attempt")
                    {
                        ErrorLog.CommonTrace("OTP Validation Failed. Maximum retry attempts exceeded. Change Password For Forgot Password Request. Username : " + txtUserName.Text + " OTP : " + txtForgrtOTP.Value.ToString());
                        txtForgrtOTP.Value = string.Empty;
                        txtUserName.Text = string.Empty;
                        Response.Write("<script>alert('Maximum retry attempts exceeded. Please click on resend OTP and Try again.');</script>");
                        return;
                    }
                    else
                    {
                        ErrorLog.CommonTrace("OTP Is InValid. Change Password For Forgot Password Request. Username : " + txtUserName.Text + " OTP : " + txtForgrtOTP.Value.ToString());
                        txtForgrtOTP.Value = string.Empty;
                        txtUserName.Text = string.Empty;
                        Response.Write("<script>alert('Invalid OTP. Please try again later');</script>");
                        return;
                    }
                    #endregion
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Validate OTP Failed (Login Page). Exception : " + Ex.Message);
                ErrorLog.CommonError(Ex);
                Response.Write("<script>alert('Sorry, something went wrong. Try again later');</script>");
                return;
            }
        }

        // Change password After OTP Validation
        protected void btnChaneForgotPassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkForgotConfirmation.Checked == false)
                {
                    Response.Write("<script>alert('You must accept Terms and Conditions.');</script>");
                    return;
                }
                else
                {
                    if (txtForgotNew.Text.Trim() == txtForgotConfirm.Text.Trim())
                    {
                        ErrorLog.CommonTrace("Change Password (Forgot Password) Request (Login Page) Received. Username : " + txtUserName.Text);
                        _LoginEntity.UserName = txtUserName.Text;
                        DataSet _dsLoginAttempt = _LoginEntity.GetLoginAttemptCount();
                        _LoginEntity.NewPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_dsLoginAttempt.Tables[0].Rows[0]["Salt"].ToString() + txtForgotNew.Text.Trim());
                        _LoginEntity.ConfirmPassword = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(_dsLoginAttempt.Tables[0].Rows[0]["Salt"].ToString() + txtForgotConfirm.Text.Trim());
                        _LoginEntity.CreatedBy = txtUserName.Text;
                        _LoginEntity.UsersID = int.Parse(ViewState["UserID"].ToString().Trim());
                        DataSet ds = _LoginEntity.SetChangePasswordForForget();
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Type"].ToString() == "Save")
                        {
                            ErrorLog.CommonTrace("Change Password (Forgot Password) Request Successful (Login Page). Username : " + txtUserName.Text);
                            try
                            {
                                //Audit Log
                                _auditParams[0] = Convert.ToString(Session["Username"]);
                                _auditParams[1] = " Password Change";
                                _auditParams[2] = Convert.ToString(Session["Username"]) + " Password Changed successfully.";
                                _LoginEntity.StoreLoginActivities(_auditParams);
                            }
                            catch (Exception Ex)
                            {
                                ErrorLog.CommonTrace("Change Password (Forgot Password) Request Successful (Login Page). Exception While Do Audit Entry In DB : " + Ex.Message + " Username : " + txtUserName.Text);
                                ErrorLog.CommonError(Ex);
                            }

                            divMainLoginBox.Visible = true;
                            PasswordBox.Visible = true;
                            btnLogin.Visible = true;
                            divForgrtOTP.Visible = false;
                            btnValidateOTP.Visible = false;
                            lnkResendOTP.Visible = false;
                            divChangePwdForForget.Visible = false;
                            Response.Write("<script>alert('Password has been changed successfully.');</script>");
                            return;
                        }
                        else
                        {
                            ErrorLog.CommonTrace("Change Password (Forgot Password) Request Failed (Login Page). Old Credentials Did Not Match. Username : " + txtUserName.Text);
                            Response.Write("<script>alert('Something Went Wrong contact system administrator');</script>");
                            return;
                        }
                    }
                    else
                    {
                        txtForgotConfirm.Text = string.Empty;
                        txtForgotNew.Text = string.Empty;
                        divMainLoginBox.Visible = false;
                        divForgrtOTP.Visible = false;
                        divChangePwdForForget.Visible = true;
                        Response.Write("<script>alert('Password mis-match, re-enter New Password and Confirm Password.');</script>");
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Change Password (Forgot Password) Request Failed (Login Page). Exception : " + Ex.Message + " Username : " + txtUserName.Text);
                ErrorLog.CommonError(Ex);
                Response.Write("<script>alert('Something Went Wrong. Try again later');</script>");
                return;
            }
        }
        #endregion

        private bool SendOTP(string RequestType, bool IsResend)
        {
            string SendType = IsResend ? "ReSendOTP" : "SendOTP";
            string _StatusData;
            string _EmailBody, _EmailId, _Mobilenumber, _RollID, _ClientId, _Subejct, _Status;
            try
            {
                ErrorLog.CommonTrace(SendType + " Request Received For " + RequestType + " Username : " + txtUserName.Text);
                _LoginEntity.UserName = txtUserName.Text;
                _LoginEntity.Flag = "1";
                _StatusData = _LoginEntity.UserIsExistForForgotPassword();
                if (_StatusData == "USER EXISTS")
                {
                    _StatusData = string.Empty;
                    _LoginEntity.UserName = txtUserName.Text;
                    _LoginEntity.Flag = "2";
                    _StatusData = _LoginEntity.UserIsExistForForgotPassword();
                    if (_StatusData.ToUpper() == "USER EMAIL AND NUMBER EXISTS")
                    {
                        #region Get Email and Mobile Number
                        _LoginEntity.UserName = txtUserName.Text;
                        _LoginEntity.Flag = "3";
                        DataSet _dsGetEmailNumber = _LoginEntity.GetUserDetails();
                        if (_dsGetEmailNumber != null && _dsGetEmailNumber.Tables[0] != null && _dsGetEmailNumber.Tables.Count > 0)
                        {
                            _EmailId = _dsGetEmailNumber.Tables[0].Rows[0]["UserEmailID"].ToString();
                            _Mobilenumber = _dsGetEmailNumber.Tables[0].Rows[0]["mobileNo"].ToString();
                            _RollID = _dsGetEmailNumber.Tables[0].Rows[0]["UserRoleID"].ToString();
                            _ClientId = _dsGetEmailNumber.Tables[0].Rows[0]["BankID"].ToString();
                            ViewState["UserID"] = _dsGetEmailNumber.Tables[0].Rows[0]["UserID"].ToString();

                            #region Genrate autogenrated   OTP
                            Random randomgenrated = new Random();
                            string AutoPassword = (randomgenrated.Next(100000, 1000000)).ToString();
                            ErrorLog.CommonTrace(SendType + " OTP Generated For " + RequestType + " Username : " + txtUserName.Text);
                            #endregion

                            #region insert OTP Into DB
                            Random random = new Random();
                            _LoginEntity.UserName = txtUserName.Text;
                            _LoginEntity.DeviceID = EnumCollection.enumApplicationType.WEB.ToString();
                            string RRN = GenerateRRN(random);
                            _LoginEntity.RRN = RRN;
                            _LoginEntity.OTP = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(AutoPassword);
                            _LoginEntity.OTPType = EnumCollection.enumOTPType.ForgotPassword.ToString();
                            _LoginEntity.Flag = "1";
                            string DBResp = _LoginEntity.InsertOTPIntoTable();
                            #endregion

                            if (DBResp.ToUpper().Contains("SUCCESS"))
                            {
                                ErrorLog.CommonTrace(SendType + " OTP Inserted In Database For " + RequestType + ". Status : " + DBResp + " Username : " + txtUserName.Text + " RRN : " + RRN);
                                #region EMAIL and SMS
                                string EmailResp = string.Empty;
                                string SMSResp = string.Empty;

                                ErrorLog.SMSTrace("Page : Login.cs \nFunction : SendOTP() => Login Details forwarded for SMS/Email Preparation. Contact No : " + _Mobilenumber + "Email : " + _EmailId);

                                #region SMS
                                _EmailSMSAlertscs.FROM = "SBMIND";
                                _EmailSMSAlertscs.to = _Mobilenumber;
                                _EmailSMSAlertscs.tempname = "SR24659_BCPOTP";
                                _EmailSMSAlertscs.OTPFlag = "1";
                                _EmailSMSAlertscs.var1 = AutoPassword;
                                _EmailSMSAlertscs.var2 = null;
                                ErrorLog.SMSTrace("Page : Login.cs \nFunction : SendOTP() => Login Details forwarded for SMS Preparation. => HttpGetRequest()");
                                EmailResp = _EmailSMSAlertscs.HttpGetRequest();
                                ErrorLog.SMSTrace("Page : Login.cs \nFunction : SendOTP() => SMS Response : " + EmailResp);
                                #endregion

                                #region EMAIL
                                _EmailSMSAlertscs.FROM = "info@sbmbank.co.in";
                                _EmailSMSAlertscs.to = _EmailId;
                                _EmailSMSAlertscs.tempname = "SR24659_EBCPOTP";
                                _EmailSMSAlertscs.OTPFlag = "1";
                                _EmailSMSAlertscs.var1 = AutoPassword;
                                _EmailSMSAlertscs.var2 = null;
                                ErrorLog.SMSTrace("Page : Login.cs \nFunction : SendOTP() => Login Details forwarded for Email Preparation. => HttpGetRequestEmail()");
                                EmailResp = _EmailSMSAlertscs.HttpGetRequestEmail();
                                ErrorLog.SMSTrace("Page : Login.cs \nFunction : SendOTP() => Email Response : " + EmailResp);
                                #endregion

                                #endregion

                                if (EmailResp.ToUpper().Contains("APP-MAXIMUS") || SMSResp.ToLower().Contains("success"))
                                {
                                    ErrorLog.SMSTrace("Page : Login.cs \nFunction : SendOTP() => OTP has been sent to Registered Email/Mobile Number. EmailResp : " + EmailResp);
                                    ErrorLog.CommonTrace("Page : Login.cs \nFunction : SendOTP() => OTP has been sent to Registered Email/Mobile Number. EmailResp : " + EmailResp);
                                    Response.Write("<script>alert('OTP has been sent to Registered Mobile Number/Email Id.');</script>");
                                    return true;
                                }
                                else
                                {
                                    ErrorLog.CommonTrace(SendType + " Failed EMAIL & SMS For " + RequestType + ". Username : " + txtUserName.Text + " RRN : " + RRN);
                                    Response.Write("<script>alert('Sorry, something went wrong. Try again later');</script>");
                                    return false;
                                }
                            }
                            else
                            {
                                ErrorLog.CommonTrace(SendType + " Failed OTP Insertion In Database For " + RequestType + ". Status : " + DBResp + " Username : " + txtUserName.Text + " RRN : " + RRN);
                                Response.Write("<script>alert('Sorry, something went wrong. Try again later');</script>");
                                return false;
                            }
                        }
                        else
                        {
                            ErrorLog.CommonTrace(SendType + " Request Failed For " + RequestType + ". Unable To Fetch User Details From Database." + " Username : " + txtUserName.Text);
                            Response.Write("<script>alert('Sorry, unable to process your request. Try again later');</script>");
                            return false;
                        }
                        #endregion
                    }
                    else
                    {
                        ErrorLog.CommonTrace(SendType + " Request Failed For " + RequestType + ". Mandatory Fields Should Not Empty (EMAIL And Mobile Number). Database Status : " + _StatusData + " Username : " + txtUserName.Text);
                        Response.Write("<script>alert('Enter valid Email and Mobile Number.');</script>");
                        return false;
                    }
                }
                else
                {
                    ErrorLog.CommonTrace(SendType + " Request Failed For " + RequestType + ". Username Not Exists. Database Status : " + _StatusData + " Username : " + txtUserName.Text);
                    Response.Write("<script>alert('Enter valid username.');</script>");
                    return false;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace(SendType + " Request Failed For " + RequestType + ". Username : " + txtUserName.Text + " Exception : " + Ex.Message);
                return false;
            }
        }

        private bool ValidateOTP(string RequestType, string OTP)
        {
            try
            {
                #region validate OTP Into Table
                ErrorLog.CommonTrace("OTP Validation Started For " + RequestType + ". Username : " + txtUserName.Text + " OTP : " + ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(OTP));
                _LoginEntity.UserName = txtUserName.Text;
                _LoginEntity.OTP = ConnectionStringEncryptDecrypt.EncryptUsingSHA2Algorithm(OTP);
                _LoginEntity.Flag = "2";
                _Status = _LoginEntity.InsertOTPIntoTable();
                if (_Status == "2")
                {
                    ErrorLog.CommonTrace("OTP Expired For " + RequestType + ". Username : " + txtUserName.Text + " OTP : " + _LoginEntity.OTP);
                    txtForgrtOTP.Value = string.Empty;
                    txtUserName.Text = string.Empty;
                    Response.Write("<script>alert('OTP expired click on resend button.');</script>");
                    return false;
                }
                else if (_Status == "0")
                {
                    ErrorLog.CommonTrace("OTP Validation Successful For " + RequestType + ". Username : " + txtUserName.Text + " OTP : " + _LoginEntity.OTP);
                    divForgrtOTP.Visible = false;
                    btnValidateOTP.Visible = false;
                    lnkbtnResendOTP.Visible = false;
                    divMainLoginBox.Visible = false;
                    divChangePwdForForget.Visible = true;
                    return true;
                }
                else if (_Status == "Maximum Attempt")
                {
                    ErrorLog.CommonTrace("OTP Validation Failed. Maximum retry attempts exceeded For " + RequestType + ". Username : " + txtUserName.Text + " OTP : " + _LoginEntity.OTP);
                    txtForgrtOTP.Value = string.Empty;
                    txtUserName.Text = string.Empty;
                    Response.Write("<script>alert('Maximum retry attempts exceeded. Please click on resend OTP and Try again.');</script>");
                    return false;
                }
                else
                {
                    ErrorLog.CommonTrace("OTP Is InValid For " + RequestType + ". Username : " + txtUserName.Text + " OTP : " + _LoginEntity.OTP);
                    txtForgrtOTP.Value = string.Empty;
                    txtUserName.Text = string.Empty;
                    Response.Write("<script>alert('Invalid OTP. Please try again later');</script>");
                    return false;
                }
                #endregion
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("OTP Validation Failed For " + RequestType + ". Username : " + txtUserName.Text + " Exception : " + Ex.Message);
                return false;
            }
        }
    }
}