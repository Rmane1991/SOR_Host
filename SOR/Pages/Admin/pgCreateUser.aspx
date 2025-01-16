<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="pgCreateUser.aspx.cs" Inherits="SOR.Pages.Admin.pgCreateUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

   <%-- <style type="text/css">
        .main-footer-text {
            font-weight: 400;
            font-size: var(--font-m-size10);
            text-align: center;
            color: var(--color-black);
            opacity: 0.8;
            margin-top: 400px;
        }
    </style>--%>
    <style>
        .errormsg {
            font-size: 12px;
            color: red;
            font-family: Arial;
        }
    </style>

    <script type="text/javascript">
        var c = document.getElementById("<%=txtUserName.ClientID %>");
        c.select =
            function (event, ui) { this.value = ""; return false; }
    </script>
    <script type="text/javascript">
        function ClearForm() {
            document.getElementById("<%=txtUserName.ClientID %>").value = '';
            document.getElementById("<%=txtEmail.ClientID %>").value = '';
            document.getElementById("<%=txtMobile.ClientID %>").value = '';
            document.getElementById("<%=txtPassword.ClientID %>").value = '';
            document.getElementById("<%=txtConfirmPassword.ClientID %>").value = '';
            document.getElementById("<%=ddlClient.ClientID %>").selectedvalue = '0';
            document.getElementById("<%=ddlRole.ClientID %>").selectedvalue = '0';
            document.getElementById("<%=ddlUsers.ClientID %>").selectedvalue = '0';
            document.getElementById("<%=ddlUserRoleGroup.ClientID %>").selectedvalue = '0';
        }
    </script>
    <script type="text/javascript">
        function ValidateForm() {
            txtUserName = document.getElementById("<%=txtUserName.ClientID %>").value;
            txtPassword = document.getElementById("<%=txtPassword.ClientID %>").value;
            txtConfirmPassword = document.getElementById("<%=txtConfirmPassword.ClientID %>").value;
            txtEmail = document.getElementById("<%=txtEmail.ClientID %>").value;
            ddlRole = document.getElementById("<%=ddlRole.ClientID %>").value;
            ddlUserRoleGroup = document.getElementById("<%=ddlUserRoleGroup.ClientID %>").value;
            ddlApplicationType = document.getElementById("<%=ddlRole.ClientID %>").value;

            if (txtUserName.trim() == "" || txtUserName.trim() == null) {
                showWarning('Please enter user name.', 'Craete User');
                document.getElementById("<%=txtUserName.ClientID%>").focus();
                return false;
            }

            else if (txtPassword.trim() == "" || txtPassword.trim() == null) {
                showWarning('Please enter password.', 'Craete User');
                document.getElementById("<%=txtPassword.ClientID%>").focus();
                return false;
            }
            else if (txtConfirmPassword.trim() == "" || txtConfirmPassword.trim() == null) {
                showWarning('Please enter confirm password.', 'Craete User');
                document.getElementById("<%=txtConfirmPassword.ClientID%>").focus();
                return false;
            }
            else if (txtEmail.trim() == "" || txtEmail.trim() == null) {

                showWarning('Please enter email Id.', 'Craete User');
                document.getElementById("<%=txtEmail.ClientID%>").focus();
                return false;
            }

            else if (ddlRole == "0" || ddlRole == "--Select--") {

                showWarning('Please select role.', 'Craete User');
                document.getElementById("<%=ddlRole.ClientID%>").focus();
                return false;
            }

            else if (ddlUserRoleGroup == "0" || ddlUserRoleGroup == "--Select--") {

                showWarning('Please select role group.', 'Craete User');
                document.getElementById("<%=ddlUserRoleGroup.ClientID%>").focus();
                return false;
            }


            maskInputs();
        }
        function disableMultipleClick() {
            if (Page_ClientValidate("ValCreateUser")) {
                debugger;
                document.getElementById("cpbdCarde_btnSave").disabled = true;
                __doPostBack('ctl00$cpbdCarde$btnSave', '');
            }
            Page_BlockSubmit = false;
            return false;
        }
    </script>
    <script type="text/javascript">

        function maskInputs() {
            if (document.getElementById("<%=txtUserName.ClientID %>") != "" && document.getElementById("<%=txtUserName.ClientID %>") != null) {
                var result = "";
                var lblValue = document.getElementById("<%=txtUserName.ClientID %>");
                var newpwd = document.getElementById("<%=txtUserName.ClientID %>");
                newpwd.value = fetchAscii(lblValue.value);
                var Data = document.getElementById("<%=hidUsername.ClientID %>");
                Data.value = newpwd.value;
                result = Encry(document.getElementById("<%=hidUsername.ClientID %>").value);
                document.getElementById("<%=hidUsername.ClientID %>").value = result;
                document.getElementById("<%=txtUserName.ClientID %>").value = '';
            }
            if (document.getElementById("<%=txtPassword.ClientID %>") != "" && document.getElementById("<%=txtPassword.ClientID %>") != null) {
                var result = "";
                var lblValueU = document.getElementById("<%=txtPassword.ClientID %>");
                var newpwdU = document.getElementById("<%=txtPassword.ClientID %>");
                newpwdU.value = fetchAscii(lblValueU.value);
                var Data = document.getElementById("<%=hidpassword.ClientID %>");
                Data.value = newpwdU.value;
                result = Encry(document.getElementById("<%=hidpassword.ClientID %>").value);
                document.getElementById("<%=hidpassword.ClientID %>").value = result;
                document.getElementById("<%=txtPassword.ClientID %>").value = '';
            }
            if (document.getElementById("<%=txtConfirmPassword.ClientID %>") != "" && document.getElementById("<%=txtConfirmPassword.ClientID %>") != null) {
                var result = "";
                var lblValueU = document.getElementById("<%=txtConfirmPassword.ClientID %>");
                var newpwdU = document.getElementById("<%=txtConfirmPassword.ClientID %>");
                newpwdU.value = fetchAscii(lblValueU.value);
                var Data = document.getElementById("<%=hidconfmpassword.ClientID %>");
                Data.value = newpwdU.value;
                result = Encry(document.getElementById("<%=hidconfmpassword.ClientID %>").value);
                document.getElementById("<%=hidconfmpassword.ClientID %>").value = result;
                document.getElementById("<%=txtConfirmPassword.ClientID %>").value = '';
            }
        }
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div class="breadHeader">
        <h5 class="page-title">Create User</h5>
    </div>
    <br />
    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:UpdateProgress ID="upContentBodyUpdateProgress" runat="server" AssociatedUpdatePanelID="upContentBody">
            <ProgressTemplate>
                <div style="position: fixed; left: 40%; top: 50%; opacity: 1.8;">
                    <img alt="" id="progressImage1" src='<%=Page.ResolveClientUrl("../Images/LdrPlzWait.gif") %>' />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>
    <asp:UpdatePanel ID="upContentBody" runat="server">
        <ContentTemplate>
            <div class="searchbox show hide-some-filters">
                <asp:HiddenField ID="hidpassword" runat="server" />
                <asp:HiddenField ID="hidconfmpassword" runat="server" />
                <asp:HiddenField ID="hidUsername" runat="server" />
                <asp:HiddenField ID="hidCount" runat="server" />
                <asp:HiddenField ID="hidIsAllowToValidateText" runat="server" />
                <asp:Panel ID="panelForm" runat="server" Width="100%" HorizontalScroll="false" ScrollBars="None" Style="margin: 10px 10px 0px 0px; padding-right: 0px;">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-md-4">
                                <label for="exampleInputEmail1">User Name:<span class="err">*</span></label>
                                <asp:TextBox ID="txtUserName" runat="server" MaxLength="30" onpaste="return false" CssClass="input-text form-control" placeholder="User Name" OnTextChanged="txtUserName_TextChanged"
                                    AutoPostBack="true" Width="100%"></asp:TextBox>
                                <asp:Label ID="lblusernameError" runat="server" Text="" CssClass="errormsg"></asp:Label>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom,LowercaseLetters,Numbers,UppercaseLetters" TargetControlID="txtUserName" ValidChars="@._" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator36" ValidationGroup="ValCreateUser" CssClass="err" ControlToValidate="txtUserName" Style="display: none" runat="server" ErrorMessage="Please enter User Name."></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-4">
                                <label for="exampleInputEmail1">Email ID:<span class="err">*</span></label>
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%" CssClass="input-text form-control" placeholder="Email ID" onpaste="return false" MaxLength="255"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="errormsg" ControlToValidate="txtEmail" ValidationGroup="ValCreateUser"
                                    ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                    Display="Dynamic" ErrorMessage="Invalid email address." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="ValCreateUser" ControlToValidate="txtEmail" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter Email ID."></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-4">
                                <label for="exampleInputEmail1">Mobile No.:<span class="err">*</span></label>
                                <asp:TextBox ID="txtMobile" runat="server" Width="100%" CssClass="input-text form-control" autocomplete="false" placeholder="Mobile No." MaxLength="10" onpaste="return false" AutoPostBack="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="ValCreateUser" ControlToValidate="txtMobile" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter Mobile No."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="errormsg" ControlToValidate="txtMobile"
                                    ForeColor="Red" ValidationExpression="^[6789]\d{9}$" Display="Dynamic" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" TargetControlID="txtMobile" />
                            </div>

                        </div>
                    </div>

                    <div class="form-group">
                        <div class="row">
                            <div class="col-md-4">
                                <label for="exampleInputEmail1">BCs:<span class="err">*</span></label>
                                <asp:DropDownList ID="ddlClient" Width="100%" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="ValCreateUser" ControlToValidate="ddlClient" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please select Client."></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-4">
                                <label for="exampleInputEmail1">Role:<span class="err">*</span></label>
                                <asp:DropDownList ID="ddlRole" Width="100%" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="ValCreateUser" ControlToValidate="ddlRole" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please select Role."></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-4">
                                <label for="exampleInputEmail1">User:</label>
                                <asp:DropDownList ID="ddlUsers" Width="100%" runat="server" CssClass="maximus-select w-100" AutoPostBack="true"></asp:DropDownList>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="ValCreateUser" ControlToValidate="ddlUsers" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please select User."></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="row">
                            <div class="col-md-4">
                                <label for="exampleInputEmail1">Role Group:<span class="err">*</span></label>
                                <asp:DropDownList ID="ddlUserRoleGroup" Width="100%" runat="server" CssClass="maximus-select w-100" AutoPostBack="false"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="ValCreateUser" ControlToValidate="ddlUserRoleGroup" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please select Role Group."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4">
                                <label for="exampleInputEmail1">Password:<span class="err">*</span></label>
                                <asp:TextBox ID="txtPassword" Width="100%" runat="server" TextMode="Password" CssClass="input-text form-control" placeholder="Password" MaxLength="16" autocomplete="false" AutoPostBack="false" onpaste="return false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="ValCreateUser"  ControlToValidate="txtPassword" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter Password."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4">
                                <label for="exampleInputEmail1">Confirm Password:<span class="err">*</span></label>
                                <asp:TextBox ID="txtConfirmPassword" Width="100%" onpaste="return false" runat="server" MaxLength="16" placeholder="Confirm Password" TextMode="Password" CssClass="input-text form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="ValCreateUser"  ControlToValidate="txtConfirmPassword" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter Confirm Password."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="searchbox-btns">
                            <ceter>
                            <div class="col">
                                <asp:Button ID="btnSave" runat="server" CssClass="themeBtn themeApplyBtn" OnClientClick="javascript:return  disableMultipleClick();" Text="Save" OnClick="btnSave_Click"></asp:Button>
                                <asp:Button ID="btnClear" runat="server" CssClass="themeBtn resetBtn themeCancelBtn me-0" OnClientClick="return ClearForm();" Text="Reset" OnClick="btnClear_Click"></asp:Button>
                               <%-- <button type="button" id="btnClear" runat="server" onserverclick="btnClear_Click" onclick="return ClearForm();" class  ="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal">Reset</button>--%>
                            </div>
                           </ceter>
                        </div>
                        <asp:ValidationSummary
                            HeaderText="You must enter or select a value in the following fields:"
                            DisplayMode="BulletList"
                            EnableClientScript="true"
                            CssClass="err"
                            ShowMessageBox="true"
                            ShowSummary="false"
                            ForeColor="Red"
                            ValidationGroup="ValCreateUser"
                            runat="server" />
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" BackgroundCssClass="modalBackground" DropShadow="false" />
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlClient.ClientID%>").select2();
                    $("#<%=ddlUserRoleGroup.ClientID%>").select2();
                    $("#<%=ddlRole.ClientID%>").select2();
                    $("#<%=ddlUsers.ClientID%>").select2();
                }
            });
        };
    </script>
</asp:Content>
