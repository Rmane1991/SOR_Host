<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PayRakamSBM.Login" %>

<%@ OutputCache Duration="500" VaryByParam="None" %>
<%--<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cap" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<html class="no-js before-run" lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <meta name="author" content="" />

    <title>Login | Proxima SOR</title>

    <link rel="apple-touch-icon" href="images/apple-touch-icon.png" />
    <link rel="shortcut icon" href="images/Rupee.png" />

    <!-- Stylesheets -->
    <link rel="stylesheet" href="assets1/css/bootstrap.min.css" />
    <link rel="stylesheet" href="assets1/css/bootstrap-extend.min.css" />
    <link rel="stylesheet" href="assets1/css/site.min.css" />
    <link rel="stylesheet" href="assets1/vendor/animsition/animsition.css" />
    <link rel="stylesheet" href="../../assets/vendor/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="assets/vendor/toastr/toastr.min.css" />
    <!-- Page -->
    <link rel="stylesheet" href="assets1/css/pages/login.css" />
    <script src="assets1/vendor/modernizr/modernizr.js"></script>
    <script src="assets1/vendor/breakpoints/breakpoints.js"></script>

    <script src="assets/scripts/jquery-1.11.0.min.js"></script>
    <script src="Scripts/aes.js"></script>
    <script src="Scripts/jquery-1.4.1-vsdoc.js"></script>
    <script src="Scripts/executeSec.js"></script>
    <script src="Scripts/JQuerySec-1.0.js"></script>

    <script>
        window.history.forward();
        function noBack() { window.history.forward(); }
    </script>
    <script>
        $(document).ready(function () {
            GetInfo();
        });
    </script>

    <script type="text/javascript">
        function ChangeBackround() {
            if (document.getElementById("<%=txtPassword.ClientID %>") != "" && document.getElementById("<%=txtPassword.ClientID %>") != null) {
                var result = "";
                var lblValue = document.getElementById("<%=txtPassword.ClientID %>");
                var newpwd = document.getElementById("<%=txtPassword.ClientID %>");
                newpwd.value = fetchAscii(lblValue.value);
                var Data = document.getElementById("<%=hidpassword.ClientID %>");
                Data.value = newpwd.value;
                result = Encry(document.getElementById("<%=hidpassword.ClientID %>").value);
                document.getElementById("<%=hidpassword.ClientID %>").value = result;
                document.getElementById("<%=txtPassword.ClientID %>").value = '**********';
            }
        }
        function edValueKeyPress() {
            var lblValue = document.getElementById("<%=txtPassword.ClientID %>");
            var newpwd = document.getElementById("<%=txtPassword.ClientID %>");
            newpwd.value = fetchAscii(lblValue.value);
            var Data = document.getElementById("<%=hidpassword.ClientID %>");
            Data.value = newpwd.value;
        }
        function fetchAscii(obj) {
            var convertedObj = '';
            for (i = 0; i < obj.length; i++) {
                var asciiChar = obj.charCodeAt(i);
                convertedObj += asciiChar + ';';
            }
            return convertedObj;
        }
        function fetchtext(obj) {
            var convertedObj = '';
            var res = obj.split(";");
            for (i = 0; i <= res.length; i++) {
                if (res == "") {
                }
                else {
                    var asciiChar = obj.fromCharCode(i);
                    convertedObj += asciiChar;
                }
            }
            return convertedObj;
        }
    </script>

    <style>
        .fa {
            display: inline-block;
            margin-right: 5px;
            font: normal normal normal 14px/1 FontAwesome;
            font-size: inherit;
            text-rendering: auto;
            -webkit-font-smoothing: antialiased;
            -moz-osx-font-smoothing: grayscale;
            position: absolute;
        }
    </style>
    <style>
        .btn-primary {
            color: #fff;
            background-color: #F5BF03;
            border-color: #F5BF03;
            
        }
        .btn-primary:hover{
            background-color: #F5BF03;
            border-color: #F5BF03;
        }
    </style>
    <script>
        var state = false;
        function togglePassword() {
            debugger;
            if (state) {
                document.getElementById("<%=txtPassword.ClientID%>").setAttribute("type", "Password");
                document.getElementById("eye").style.color = '#7a797e';
                state = false;
            }
            else {

                document.getElementById("<%=txtPassword.ClientID%>").setAttribute("type", "text");
                document.getElementById("eye").style.color = '#5887ef';
                state = true;
            }
        }
        function toggleOTP() {
            debugger;
            if (state) {
                document.getElementById("<%=txtForgrtOTP.ClientID%>").setAttribute("type", "Password");
                document.getElementById("eyeOTP").style.color = '#7a797e';
                state = false;
            }
            else {

                document.getElementById("<%=txtForgrtOTP.ClientID%>").setAttribute("type", "text");
                document.getElementById("eyeOTP").style.color = '#5887ef';
                state = true;
            }
        }
    </script>

    <script>
        function GetInfo() {
            // alert('Call...');
            $.getJSON('http://ip-api.com/json', function (data) {
                // http://ip-api.com/json
                // https://json.geoiplookup.io/?callback=?
                document.getElementById("<%=hidInfo.ClientID %>").value = JSON.stringify(data, null, 2);
                // alert(document.getElementById("<%=hidInfo.ClientID %>").value);
            });
        };
    </script>
</head>
<body class="page-login layout-full">
    <!-- Page -->
    <div class="page animsition vertical-align text-center" data-animsition-in="fade-in"
        data-animsition-out="fade-out">
        <div class="page-content loginbox vertical-align-middle">
            <br />
            <br />

            <div class="brand">
                <div class="form-group row">
                    <div class="col-sm-5" style="margin-left: -24px;">
                        <img class="brand-img" src="images/OurBank.png" height="25px" style="margin-bottom:-36px" alt="..." />
                    </div>
                   <div class="col-sm-2">
                       </div>
                    <div class="col-sm-5" style="margin-right:-40px;">
                        <img class="brand-img" src="images/proxima_logo.png" height="45px" alt="..." />
                    </div>
                </div>
            </div>
            <p>Sign into your account</p>
            <form runat="server">
                <div>
                    <label runat="server" visible="false" id="lblErrorMsg" style="margin: 14px 0 0 0px; font-size: 13px; width: 250px; color: red; margin-bottom: 5px; font-weight: bold"
                        class="control-label">
                        Incorrect Username or Password</label>
                    <div style="font-size: 20px; width: 200px; font: bold; color: white;" id="dvShowErrorMessage1" runat="server" visible="false">
                    </div>
                </div>
                <div runat="server" id="divMainLoginBox">
                    <div runat="server" class="form-group">
                        <label class="sr-only" for="inputName">User Name</label>
                        <asp:TextBox ID="txtUserName" runat="server" class="form-control" placeholder="Username or 10 digits Mobile No." MaxLength="30"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtUserName" SetFocusOnError="true" runat="server"></asp:RequiredFieldValidator>--%>
                    </div>
                    <div runat="server" class="form-group" id="PasswordBox">
                        <label class="sr-only" for="inputPassword">Password</label>
                        <%--<asp:TextBox ID="txtPassword" runat="server" MaxLength="12" TextMode="Password" class="form-control" placeholder="Password" Visible="false"></asp:TextBox>--%>
                        <input type="Password" id="txtPassword" class="form-control" maxlength="12" placeholder="Password" runat="server" />
                        <span><i class="fa fa-eye" aria-hidden="true" id="eye" style="margin-left: 150px; margin-top: -20px; color: grey;" onclick="togglePassword()"></i></span>
                    </div>
                    <div runat="server" class="form-group" style="display: none;">
                        <%-- <cap:CaptchaControl ID="captcha1" runat="server" CaptchaLength="5" CaptchaHeight="50" CaptchaWidth="200" CaptchaLineNoise="None" CaptchaMinTimeout="3" CaptchaMaxTimeout="240" ForeColor="Blue" BackColor="#c8b0ae" CaptchaCshars="ABCDEFGHIJKLMNOPQRSTUVWX123456789" />--%>
                    </div>
                    <div runat="server" class="form-group" style="display: none;">
                        <label class="sr-only" for="inputcaptcha">Enter Captcha</label>
                        <asp:TextBox ID="txtCaptcha" runat="server" class="form-control" placeholder="Enter Captcha" AutoCompleteType="Disabled"></asp:TextBox>
                    </div>
                    <div runat="server" class="form-group" visible="false" id="divForgrtOTP">
                        <label class="sr-only" for="inputcaptcha">Enter OTP</label>
                        <%--  <asp:TextBox ID="txtForgrtOTP" runat="server" class="form-control" placeholder="Enter OTP" MaxLength="6"></asp:TextBox>--%>
                        <input type="Password" id="txtForgrtOTP" class="form-control" maxlength="6" placeholder="OTP" runat="server" />
                        <span><i class="fa fa-eye" aria-hidden="true" id="eyeOTP" style="margin-left: 150px; margin-top: -20px; color: grey;" onclick="toggleOTP()"></i></span>
                    </div>
                    <asp:HiddenField ID="hidpassword" runat="server" />
                    <asp:HiddenField ID="hidUsername" runat="server" />
                    <asp:HiddenField ID="hidCount" runat="server" />
                    <asp:HiddenField ID="hidInfo" runat="server" Value="" />
                    <asp:Button ID="btnLogin" Style="margin-left: 0px;" runat="server" Class="btn btn-primary btn-block" OnClick="btnLogin_Click" Text="Sign in" />
                    <%--<asp:Button ID="btnLogin" Style="margin-left: 0px;" runat="server" Class="btn btn-primary btn-block" OnClientClick="ChangeBackround();" OnClick="btnLogin_Click" Text="Submit" />--%>
                    <asp:Button ID="btnValidateOTP" Visible="false" Style="margin-left: 0px;" runat="server" CssClass="btn btn-primary btn-block" Text="Validate OTP" OnClick="btnValidateOTP_Click"></asp:Button>
                    <div class="form-group">
                        <div class="form-che"></div>
                    </div>
                    <div class="bottom" id="lnkForgetPassword" runat="server">
                        <span class="helper-text"><i class="fa fa-lock"></i>
                            <asp:LinkButton ID="lbtnForgotPassword" runat="server" Font-Bold="true" OnClick="lbtnForgotPassword_Click">Forgot password?</asp:LinkButton></span>
                    </div>
                    <div class="bottom" id="lnkResendOTP" runat="server" visible="false">
                        <span class="helper-text"><i class="fa fa-lock"></i>
                            <asp:LinkButton ID="lnkbtnResendOTP" runat="server" Font-Bold="true" OnClick="lnkResendOTP_Click">Resend OTP?</asp:LinkButton></span>
                    </div>
                </div>
                <div style="display: none">
                    <asp:Button runat="server" ID="btnserversidehit" OnClick="btnserversidehit_Click" />
                </div>

                <div runat="server" id="divChangePwd" visible="false">
                    <label runat="server" id="Label1" style="margin: 14px 0 0 0px; font-size: 13px; width: 250px; color: red; margin-bottom: 5px; font-weight: bold"
                        class="control-label">
                        Change Your Password</label>
                    <div runat="server" class="form-group">
                        <label for="exampleInputEmail1">Old Password<span class="err"> *</span></label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator0" ControlToValidate="txtold" SetFocusOnError="true" Style="display: none" ValidationGroup="chngpass" runat="server" CssClass="err" ErrorMessage="Please enter Old Password "></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="REValid" runat="server" Display="none" ValidationGroup="chngpass" ErrorMessage="Old Password should be 8 to 12 characters." ForeColor="Red" ControlToValidate="txtold" ValidationExpression="^(?=.*\d)(?=.*[a-zA-Z]).{8,12}$" />
                        <asp:TextBox ID="txtold" runat="server" class="form-control" Type="Password" MaxLength="12"></asp:TextBox>
                    </div>
                    <div runat="server" class="form-group">
                        <label for="exampleInputEmail1">New Password <span class="err">*</span></label>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="none" ValidationGroup="chngpass" ErrorMessage="New Password should be 8 to 12 characters & alphanumeric only." ForeColor="Red" ControlToValidate="txtnew" ValidationExpression="^(?=.*\d)(?=.*[a-zA-Z]).{8,12}$" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtnew" SetFocusOnError="true" Style="display: none" ValidationGroup="chngpass" runat="server" CssClass="err" ErrorMessage="Please enter new Password"></asp:RequiredFieldValidator>
                        <asp:TextBox runat="server" class="form-control" ID="txtnew" Type="Password" MaxLength="12"></asp:TextBox>
                    </div>
                    <div runat="server" class="form-group">
                        <label for="exampleInputEmail1">Confirm Password <span class="err">*</span></label>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="none" ValidationGroup="chngpass" ErrorMessage="Confirm Password should be 8 to 12 characters & alphanumeric only." ForeColor="Red" ControlToValidate="txtconfirm" ValidationExpression="^(?=.*\d)(?=.*[a-zA-Z]).{8,12}$" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtconfirm" Style="display: none" SetFocusOnError="true" ValidationGroup="chngpass" runat="server" CssClass="err" ErrorMessage="Please enter confirm Password"></asp:RequiredFieldValidator>
                        <asp:TextBox runat="server" class="form-control" Type="Password" ID="txtconfirm" MaxLength="12"></asp:TextBox>
                    </div>
                    <div runat="server" class="form-group">
                        <label for="exampleInputEmail1">OTP <span class="err">*</span></label>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="none" ValidationGroup="chngpass" ErrorMessage="Enter Valid OTP." ForeColor="Red" ControlToValidate="txtOTPFirstLogin" ValidationExpression="^\d{6}$" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtOTPFirstLogin" Style="display: none" SetFocusOnError="true" ValidationGroup="chngpass" runat="server" CssClass="err" ErrorMessage="Please enter OTP"></asp:RequiredFieldValidator>
                        <asp:TextBox runat="server" class="form-control" Type="Password" ID="txtOTPFirstLogin" MaxLength="6"></asp:TextBox>
                    </div>
                    <div runat="server" class="form-group">
                        <span class="helper-text"><i class="fa fa-lock"></i>
                            <asp:LinkButton ID="lnkbtnResendOTPFirstLogin" runat="server" Font-Bold="true" OnClick="lnkResendOTP_Click">Resend OTP?</asp:LinkButton></span>
                    </div>
                    <div runat="server" class="form-group">
                        <asp:CheckBox ID="chkConfimartion" runat="server" Text="I Agree to Terms and Conditions." />
                    </div>
                    <div runat="server" class="form-group">
                        <%--<a class="pull-right" style="color: #304087;" id="btnChangePwd" runat="server" onserverclick="btnChangePwd_ServerClick" href="#">Change Password</a>--%>
                        <asp:Button runat="server" ID="btnChangePwd" ValidationGroup="chngpass" OnClick="btnChangePwd_ServerClick" Class="btn btn-success" Text="Change Password"></asp:Button>
                        <asp:ValidationSummary
                            HeaderText="You must enter or select a value in the following fields:"
                            DisplayMode="BulletList"
                            EnableClientScript="true"
                            CssClass="err"
                            ShowMessageBox="true"
                            ShowSummary="false"
                            ForeColor="Red"
                            ValidationGroup="chngpass"
                            runat="server" />
                    </div>
                </div>


                <div runat="server" id="divChangePwdForForget" visible="false">
                    <label runat="server" id="Label2" style="margin: 14px 0 0 0px; font-size: 13px; width: 250px; color: red; margin-bottom: 5px; font-weight: bold"
                        class="control-label">
                        First Login / Your Password is expired. Kindly change your password.</label>
                    <div runat="server" class="form-group">
                        <label for="exampleInputEmail1">New Password <span class="err">*</span></label>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="none" ValidationGroup="chngpass" ErrorMessage="New Password should be 8 to 12 characters & alphanumeric only." ForeColor="Red" ControlToValidate="txtForgotNew" ValidationExpression="^(?=.*\d)(?=.*[a-zA-Z]).{8,12}$" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txtForgotNew" SetFocusOnError="true" Style="display: none" ValidationGroup="chngpass" runat="server" CssClass="err" ErrorMessage="Please enter new Password"></asp:RequiredFieldValidator>
                        <asp:TextBox runat="server" class="form-control" ID="txtForgotNew" Type="Password" MaxLength="12"></asp:TextBox>
                    </div>
                    <div runat="server" class="form-group">
                        <label for="exampleInputEmail1">Confirm Password <span class="err">*</span></label>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="none" ValidationGroup="chngpass" ErrorMessage="Confirm Password should be 8 to 12 characters & alphanumeric only." ForeColor="Red" ControlToValidate="txtForgotConfirm" ValidationExpression="^(?=.*\d)(?=.*[a-zA-Z]).{8,12}$" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="txtForgotConfirm" Style="display: none" SetFocusOnError="true" ValidationGroup="chngpass" runat="server" CssClass="err" ErrorMessage="Please enter confirm Password"></asp:RequiredFieldValidator>
                        <asp:TextBox runat="server" class="form-control" Type="Password" ID="txtForgotConfirm" MaxLength="12"></asp:TextBox>
                    </div>
                    <div runat="server" class="form-group">
                        <asp:CheckBox ID="chkForgotConfirmation" runat="server" Text=" I Agree to Terms and Conditions." />
                    </div>
                    <div runat="server" class="form-group">
                        <asp:Button runat="server" ID="btnChaneForgotPassword" ValidationGroup="chngpass" OnClick="btnChaneForgotPassword_Click" Class="btn btn-success" Text="Change Password"></asp:Button>
                        <asp:ValidationSummary
                            HeaderText="You must enter or select a value in the following fields:"
                            DisplayMode="BulletList"
                            EnableClientScript="true"
                            CssClass="err"
                            ShowMessageBox="true"
                            ShowSummary="false"
                            ForeColor="Red"
                            ValidationGroup="chngpass"
                            runat="server" />
                    </div>
                </div>


                <script type="text/javascript">
                    function showWarning(msg, title) {
                        toastr.options = {
                            "closeButton": false,
                            "debug": false,
                            "positionClass": "toast-top-center",
                            "newestOnTop": false,
                            "progressBar": true,
                            "preventDuplicates": true,
                            "onclick": null,
                            "showDuration": "300",
                            "hideDuration": "1000",
                            "timeOut": "2000",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        }
                        toastr.warning(msg, title);
                        return false;
                    }
                </script>
                <%--//for chatprocess of client--%>
                <script type="text/javascript">
                    function showSuccess(msg, title) {
                        toastr.options = {
                            "closeButton": false,
                            "debug": false,
                            "positionClass": "toast-top-center",
                            "newestOnTop": false,
                            "progressBar": true,
                            "preventDuplicates": true,
                            "onclick": null,
                            "showDuration": "300",
                            "hideDuration": "1000",
                            "timeOut": "5000",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        }
                        toastr.success(msg, title);
                        return false;
                    }
                </script>
                <%--for chat process of consultant--%>
                <script type="text/javascript">
                    function showError(msg, title) {
                        toastr.options = {
                            "closeButton": false,
                            "debug": false,
                            "positionClass": "toast-top-center",
                            "newestOnTop": false,
                            "progressBar": true,
                            "preventDuplicates": true,
                            "onclick": null,
                            "showDuration": "300",
                            "hideDuration": "1000",
                            "timeOut": "3000",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        }
                        toastr.error(msg, title);
                        return false;
                    }
                </script>

            </form>
            <footer class="page-copyright">
                <div class="row">
                    <div class="col-md-12">
                        <p align="center">&copy; <a style="color: white" href="http://www.maximusinfoware.in/" target="_blank">Powered by Maximus Infoware (India) Pvt. Ltd.</a></p>
                    </div>
                </div>
                <div class="social">
                    <a href="javascript:void(0)">
                        <i class="icon bd-twitter" aria-hidden="true"></i>
                    </a>
                    <a href="javascript:void(0)">
                        <i class="icon bd-facebook" aria-hidden="true"></i>
                    </a>
                    <a href="javascript:void(0)">
                        <i class="icon bd-dribbble" aria-hidden="true"></i>
                    </a>
                </div>
            </footer>
        </div>
    </div>
    <!-- End Page -->
    <!-- Core  -->
    <script src="assets1/vendor/jquery/jquery.js"></script>
    <script src="assets1/vendor/bootstrap/bootstrap.js"></script>
    <script src="assets1/vendor/animsition/jquery.animsition.js"></script>
    <!-- Plugins -->
    <script src="assets1/vendor/screenfull/screenfull.js"></script>
    <!-- Scripts -->
    <script src="assets1/js/core.js"></script>
    <script src="assets1/js/site.js"></script>
    <script src="assets1/js/sections/menu.js"></script>
    <script src="assets1/js/sections/menubar.js"></script>
    <script src="assets1/js/sections/sidebar.js"></script>
    <script src="assets1/js/components/animsition.js"></script>
    <script src="../../assets/vendor/toastr/toastr.js"></script>
    <script>
        (function (document, window, $) {
            'use strict';

            var Site = window.Site;
            $(document).ready(function () {
                Site.run();
            });
        })(document, window, jQuery);
    </script>

</body>
</html>
