<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="pgManageUserAccount.aspx.cs" Inherits="SOR.Pages.Admin.pgManageUserAccount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <head>
        <div>
            <script type="text/javascript">
                $("[src*=plus]").live("click", function () {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                    $(this).attr("src", "images/minus.png");
                });
                $("[src*=minus]").live("click", function () {
                    $(this).attr("src", "images/plus.png");
                    $(this).closest("tr").next().remove();
                });
            </script>
            <link href="../Login/assets/css/font-awesome.min.css" rel="stylesheet" />
            <link href="css/bootstrap.min.css" rel="stylesheet" />
            <style>
                .GridView {
                    width: 100% !Important;
                }

                .form-group {
                    margin-bottom: 5px;
                }

                .form-control {
                    height: 25px;
                }

                table {
                    width: 0% !Important;
                }

                .select2 {
                    width: 100% !important;
                }

                .lbtnAction {
                    border: 1px solid ridge;
                    border-color: black;
                }

                .adjustMarginTB {
                    margin-top: 18px;
                }

                .form-control {
                    height: 25px;
                }

                .inputalign {
                    display: flex;
                    flex-direction: row;
                    font-weight: 500;
                    font-size: small;
                }

                .select2-container--default .select2-selection--single .select2-selection__rendered {
                    letter-spacing: 0.02em !important;
                    width: 120px;
                    color: var(--color-black) !important;
                    margin-left: 2px;
                    font-size: var(--font-m-size12) !important;
                    line-height: 32px;
                }
            </style>

            <style>
                .form-group {
                    margin-bottom: 15px;
                }

                .GridView {
                    width: 100%;
                    height: auto;
                    overflow-y: auto;
                    border: solid 0px #B9B9B9 !important;
                }

                    .GridView table {
                        width: 100%;
                        border-collapse: collapse;
                    }

                    .GridView th {
                        background: #fbd2ce;
                        color: black;
                        font-weight: bold !important;
                        padding: 4px 20px 4px 4px !important;
                        font-size: small;
                    }

                    .GridView td {
                        background: none;
                        color: #420629;
                        padding: 5px;
                        border: 1px solid #CCC;
                        font-size: small;
                    }

                    .GridView tr:nth-child(even) {
                        background: #eaecff !important;
                    }

                    .GridView tr:nth-child(odd) {
                        background: #FFF !important;
                    }

                span {
                    font-family: var(--primary-font);
                    font-size: small;
                }
            </style>
            <script type="text/javascript">
                function ConfirmOnDelete() {
                    if (confirm("Click OK to delete record") == true)
                        return true;
                    else
                        return false;
                }

                function HideModalPopup() {
                    debugger;
                    $find("mpeEdit").hide();
                    return false;
                }
                function HideModalPopup2() {
                    debugger;
                    $find("mpeDelete").hide();
                    return false;
                }
                function HideModalPopup2() {
                    debugger;
                    $find("mpeReset").hide();
                    return false;
                }

            </script>

            <script type="text/javascript">
                function ClearForm() {
                    document.getElementById("<%=ddlClient.ClientID %>").selectedvalue = '0';
                    document.getElementById("<%=ddlRoleID.ClientID %>").selectedvalue = '0';
                    document.getElementById("<%=ddlStatusType.ClientID %>").selectedvalue = '0';
                    document.getElementById("<%=ddlUser.ClientID %>").selectedvalue = '0';
                }
            </script>
        </div>
    </head>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div class="breadHeader">
        <h5 class="page-title">Manage User</h5>
    </div>
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
            <div class="container-fluid">
                <asp:Panel ID="panelLimitConfig" runat="server" Width="100%" HorizontalScroll="false" ScrollBars="None" Style="border: 0px; padding: 5px 10px 0px 10px;">
                    <div class="accordion summary-accordion" id="history-accordion">
                        <div class="accordion-item">
                            <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOne">
                                <h6 class="searchHeader-heading">Filter</h6>
                                <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOne"
                                    aria-expanded="true" aria-controls="collapseOne">
                                    <span class="icon-hide"></span>
                                    <p>Show / Hide</p>
                                </button>
                            </div>
                            <div id="collapseSummaryOne" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                data-bs-parent="#summary-accordion">
                                <div class="accordion-body">
                                    <hr class="hr-line">
                                    <!-- grid -->
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                        <!-- input -->
                                        <div class="col">
                                            <label class="selectInputLabel" for="email">Client:</label>
                                            <div class="selectInputBox">
                                                <asp:DropDownList ID="ddlClient" runat="server" CssClass="maximus-select w-100" AutoPostBack="false"></asp:DropDownList>
                                            </div>

                                        </div>
                                        <!-- input -->
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Role:</label>
                                            <div class="selectInputBox">
                                                <asp:DropDownList ID="ddlRoleID" runat="server" CssClass="maximus-select w-100">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <!-- input -->
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">User:</label>
                                            <div class="selectInputBox">
                                                <asp:DropDownList ID="ddlUser" runat="server" CssClass="maximus-select w-100" AutoPostBack="false"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <!-- input -->
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Status:</label>
                                            <div class="selectInputBox">
                                                <asp:DropDownList ID="ddlStatusType" runat="server" CssClass="maximus-select w-100">
                                                    <asp:ListItem Text="-- Select --" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Inactive" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Block" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Terminated" Value="4"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- bottom btns -->
                                    <center>
                                    <div class="form-group row">
                                        <div class="searchbox-btns">
                                                        <asp:Button ID="buttonSearch" runat="server" Text="Search" CssClass="themeBtn themeApplyBtn" data-bs-target="modal" OnClientClick="return HoldDateOnServerClick();" OnClick="buttonSearch_Click"/>
                                                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal" OnClientClick="return ClearForm();"  OnClick="btnReset_Click" data-bs-target="#SearchFilterModal" />
                                                        <%--<div class="col-md-1">
                                                            <div class="col">
                                                                <asp:ImageButton ID="BtnCsv" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                                    ToolTip="Csv" OnClick="BtnCsv_Click" data-toggle="modal" data-target="#myModal" />
                                                                <asp:ImageButton ID="BtnXls" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                                    ToolTip="Xls" OnClick="BtnXls_Click" data-toggle="modal" data-target="#myModal" />
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                    </div>
                                     </center>
                                    <%--<div class="row">
                                        <div class="form-group">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <center><strong><asp:Label ID="lblRecordsCount" runat="server" Text=""></asp:Label></strong></center>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <%--<div class="form-group">
                                        <div class="col-12 text-center">
                                            <strong>
                                                <asp:Label ID="lblRecordsCount" runat="server" Text="" class="selectInputLabel" for="selectInputLabel"></asp:Label></strong>
                                        </div>
                                        <div class="col-12 d-flex justify-content-end">
                                            <asp:ImageButton ID="BtnCsv" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                ToolTip="Csv" OnClick="BtnCsv_Click" data-toggle="modal" data-target="#myModal" />

                                            <asp:ImageButton ID="BtnXls" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                ToolTip="Xls" OnClick="BtnXls_Click" data-toggle="modal" data-target="#myModal" />
                                        </div>
                                    </div>--%>
                                    <div class="row d-flex justify-content-center align-items-center">
                                        <div class="col-auto text-center">
                                            <strong>
                                                <asp:Label ID="lblRecordsCount" runat="server" Text=""></asp:Label></strong>
                                        </div>
                                        <div class="d-flex justify-content-end">
                                            <asp:ImageButton ID="BtnCsv" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                ToolTip="Csv" OnClick="BtnCsv_Click" data-toggle="modal" data-target="#myModal" />
                                            <asp:ImageButton ID="BtnXls" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                ToolTip="Xls" OnClick="BtnXls_Click" data-toggle="modal" data-target="#myModal" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                </asp:Panel>

                <asp:Panel ID="panelGrid" runat="server" Width="100%" HorizontalScroll="false" ScrollBars="None" Style="margin: 5px 10px 0px 0px; padding-right: 0px;">

                    <div class="table-box">
                        <div class="tableBorderBox" style="padding: 10px 10px;">

                            <div class="form-group row">
                                <div class="col-sm-12 col-xs-12">
                                    <asp:GridView ID="gvUserManageRole" runat="server"
                                        AutoGenerateColumns="false"
                                        Width="100%"
                                        GridLines="None"
                                        AllowPaging="true"
                                        CssClass="GridView"
                                        PageSize="10"
                                        DataKeyNames="UserName"
                                        Visible="true"
                                        OnRowCommand="gvUserManageRole_RowCommand"
                                        OnPageIndexChanging="gvUserManageRole_PageIndexChanging">
                                        <HeaderStyle HorizontalAlign="left" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderText="Action" HeaderStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnResetPassword" runat="server" OnClientClick="return ValidateReset();"
                                                        CommandArgument='<%#Eval("UserName") %>' CommandName="Reset" ToolTip="Reset Password"><img src="../../images/1Resetpassword.png"  Width="15" Height="15"/></asp:LinkButton>&nbsp;&nbsp; 
                                                
                                                    <asp:LinkButton ID="btnUnblockUser" runat="server" OnClientClick="return ValidateUnblock();"
                                                        CommandArgument='<%#Eval("UserName") %>' CommandName="UnBlock" ToolTip="Unblock User"><img src="../../images/2Unblockuser.png" Width="15" Height="15" /></asp:LinkButton>&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnblock" runat="server" OnClientClick="return Validateblock();"
                                                        CommandArgument='<%#Eval("UserName") %>' CommandName="Block" ToolTip="Block User"><img src="../../images/3Blockuser.png" Width="15" Height="15"/></asp:LinkButton>&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnTerminate" runat="server" OnClientClick="return Validateblock();"
                                                        CommandArgument='<%#Eval("UserName") %>' CommandName="Terminate" ToolTip="Terminate User"><img src="../../images/4TerminateUser.png" Width="15" Height="15" /></asp:LinkButton>
                                                    <%--<asp:ImageButton ID="lbtnDeleteRole" runat="server" ToolTip="Terminate User" Width="16" Height="16" ImageUrl="~/Images/new img/document-delete.png" OnClientClick="return ConfirmOnDelete();" CommandName="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Userid") %>' />   <i class='fa fa-remove'>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                            <asp:TemplateField HeaderText="Role">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblUserType" Text='<%#Eval("User Type") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AccessStatus" HeaderText="Access Status" />
                                            <asp:BoundField DataField="Created On" HeaderText="Created On" />
                                            <asp:BoundField DataField="Status Changed On" HeaderText="Status Changed On" />

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            </div>
            <asp:Button ID="btnhidresetDelete" runat="server" Text="Button" Visible="true" Style="visibility: hidden" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderToGetReason" runat="server"
                TargetControlID="btnhidresetDelete" PopupControlID="PanelModalPopupToGetReason"
                PopupDragHandleControlID="PopupHeader_PanelModalPopupToGetReason" Drag="true"
                BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="PanelModalPopupToGetReason" Style="display: none;" runat="server">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content" style="width: 700px; height: 300px">
                        <div class="modal-header">
                            <h4 class="modal-title">
                                <asp:Label ID="lblModalHeaderName" runat="server"></asp:Label>
                            </h4>
                        </div>
                        <center>
                        <div class="modal-body">
                            <asp:Label ID="lblconfirm" runat="server" Font-Bold="true"></asp:Label>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:TextBox ID="TxtRemarks" runat="server" TextMode="multiline" Rows="5" Height="100" Width="300" CssClass="input-text form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="lblID" runat="server" Font-Bold="true" Text="" Visible="false"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="searchbox-btns">
                                <asp:Button ID="btnSaveAction" runat="server" Text="Save" CssClass="themeBtn themeApplyBtn" OnClick="buttonModalOK_Click" />
                                <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnCancelAction" type="button" runat="server" causesvalidation="false" onserverclick="btnCancelAction_Click">Cancel</button>
                            </div>
                        </div>
                            </center>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnCsv" />
            <asp:PostBackTrigger ControlID="BtnXls" />
        </Triggers>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" BackgroundCssClass="modalBackground" DropShadow="false" />
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlUser.ClientID%>").select2();
                    $("#<%=ddlStatusType.ClientID%>").select2();
                    $("#<%=ddlRoleID.ClientID%>").select2();
                    $("#<%=ddlClient.ClientID%>").select2();
                }
            });
        };
    </script>
</asp:Content>
