<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="pgUserAccessManagement.aspx.cs" Inherits="SOR.Pages.Admin.pgUserAccessManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
        <link href="../../css/toggleButton.css" rel="stylesheet" />

        <script type="text/javascript">


            function ExpandCollapse(obj) {
                debugger;
                var div = document.getElementById(obj);
                var img = document.getElementById('img' + obj);

                if (div) {
                    if (div.style.display == "none") {
                        div.style.display = "block";
                        img.src = "../../Images/minus.png";
                    }
                    else {
                        div.style.display = "none";
                        img.src = "../../Images/plus.png";
                    }
                }
            }
        </script>

        <script>
            function ExpandGrid() {
                debugger;
                $(this).attr("src", "../../Images/plus.png");
                $(this).closest("tr").next().remove();

                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                $(this).attr("src", "../../Images/minus.png");

            };


            function expandcollapse(obj, row) {
                debugger;
                var div = document.getElementById(obj);
                var img = document.getElementById('img' + obj);

                if (div.style.display == "none") {
                    div.style.display = "block";
                    if (row == 'alt') {
                        img.src = "../../Images/minus.png";
                    }
                    else {
                        img.src = "../../Images/minus.png";
                    }
                    img.alt = "Close View Sub Menu";
                }
                else {
                    div.style.display = "none";
                    if (row == 'alt') {
                        img.src = "../../Images/plus.png";
                    }
                    else {
                        img.src = "../../Images/plus.png";
                    }
                    img.alt = "Expand Close Menu";
                }
            }
        </script>

        <script type="text/javascript">

            function toggleVisibility(cb) {
                var x = document.getElementById("NotificationPanel");
                x.style.display = cb.checked ? "block" : "none";
            }

            function updateCheckbox(cntrl) {
                if (cntrl.checked) {

                    cntrl.checked = true;
                    cntrl.setAttribute("checked", "checked");
                    cntrl.setAttribute("Value", "checked");
                }
                else {
                    cntrl.checked = false;
                    cntrl.removeAttribute("checked");
                }
            }

        </script>


        <style>
            .GridView {
                width: 100% !Important;
            }

            table {
                width: 0% !Important;
            }

            .select2 {
                width: 100% !important;
            }

            .HeaderStyle table tbody tr th {
                position: sticky;
                white-space: nowrap;
                width: auto;
                z-index: 100;
                top: -8px;
            }

            .GridView td {
                background: none;
                color: #420629;
                padding: 1px 5px 1px 10px;
                border: 1px solid #CCC;
            }

            .slider.round {
                border-radius: 24px;
            }

            .slider {
                position: absolute;
                cursor: pointer;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background-color: #d9534f;
                -webkit-transition: .4s;
                transition: .4s;
            }


            .switch {
                position: relative;
                display: inline-block;
                width: 34px;
                height: 18px;
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

            element.style {
                padding-left: 13px;
                padding-right: 24px;
                padding-top: 11px;
            }

            .inputalign {
                display: flex;
                flex-direction: row;
                font-weight: 500;
                font-size: small;
            }

            span {
                font-family: var(--primary-font);
                font-size: small;
            }

            .form-control {
                font-size: 12px;
            }
        </style>



        <script type="text/javascript">

            function HideModalPopup() {
                debugger;
                $find("mpe").hide();
                return false;
            }

            function getData(e) {
                e.preventDefault();
            }
            function ConfirmResignation() {
                debugger;
                txtRole = document.getElementById("<%=txtRole.ClientID %>").value;
                if (txtRole == "" || txtRole == null) {
                    showWarning('Please enter role name.', 'New Role');
                    document.getElementById("<%=txtRole.ClientID%>").focus();
                    return false;
                }
                else {
                    __doPostBack('ctl00$MainContent$btnSave', '');
                }
            }

            function EditConfirmResignation() {
                debugger;
                txtEditRoleName = document.getElementById("<%=txtEditRoleName.ClientID %>").value;
                if (txtEditRoleName == "" || txtEditRoleName == null) {
                    showWarning('Please enter role name.', 'Edit Role');
                    document.getElementById("<%=txtEditRoleName.ClientID%>").focus();
                    return false;
                }
                else {
                    __doPostBack('ctl00$MainContent$btnSaveSchemeDetails', '');
                }
            }

            function allLetterNumber(inputtxt) {
                var letters = /^([\d|\w]|[-/=$*#_?.,@!|\s])+$/;
                if (inputtxt.value.match(letters)) {
                    return true;
                }
                else {
                    //alert('Please input alphabet characters only youentr');
                    inputtxt.value = '';
                    return false;
                }
            }
        </script>
        <style>
            .ajax__tab_xp .ajax__tab_body {
                /* font-family: verdana,tahoma,helvetica; */
                font-size: 10pt;
                border: 1px solid #999999;
                border-top: 0;
                padding: 8px;
                background-color: #ffffff;
            }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div class="breadHeader">
        <h5 class="page-title">Manage Permissions</h5>
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
                                <h6 class="searchHeader-heading"></h6>
                                <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOne"
                                    aria-expanded="true" aria-controls="collapseOne">
                                    <span class="icon-hide"></span>
                                    <p>Show / Hide</p>
                                </button>
                            </div>
                            <div id="collapseSummaryOne" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                data-bs-parent="#summary-accordion">
                                <div class="accordion-body">
                                    <%--<hr class="hr-line">--%>

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap" style="display: none">
                                        <div class="col">
                                            <div class="selectInputBox">
                                                <button class="themeBtn themeApplyBtn" id="lbtnAddRole" runat="server" onclick="$('#dvLoading').show();" onserverclick="lbtnNewRole_ServerClick" type="button" causesvalidation="false">
                                                    <i class="fa fa-plus"></i>&nbsp;New Role
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                    <%--<div class="form-group row">
                                        <div class="searchbox-btns">
                                            <div class="col" style="margin-left: 935px;">
                                            </div>
                                        </div>
                                    </div>--%>
                                    <div class="row">
                                        <div class="form-group">
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
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="panelGrid" CssClass="tableBorderBox" runat="server" Width="100%" HorizontalScroll="false" ScrollBars="None" Style="margin: 5px 10px 0px 0px; padding-left: 15px; padding-right: 15px; padding-top: 15px;">
                    <div class="form-group row">
                        <div class="col-sm-12 col-xs-12">
                            <asp:GridView ID="gvRoleDetails" runat="server"
                                AutoGenerateColumns="false"
                                Font-Size="Small"
                                GridLines="None"
                                AllowPaging="true"
                                CssClass="GridView"
                                PageSize="10"
                                Visible="true"
                                OnPageIndexChanging="gvRoleDetails_PageIndexChanging"
                                DataKeyNames="RoleId,RoleName"
                                OnRowCommand="gvRoleDetails_RowCommand">
                                <HeaderStyle HorizontalAlign="left" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderText="Action" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RoleId") %>'
                                                CommandName="EditRole" ToolTip="Edit role" Width="16" Height="16" ImageUrl="~/Images/Edit-01-512.png" />

                                            <asp:ImageButton ID="lbtnDelete" runat="server" ToolTip="Delete role" Width="16" Height="16" ImageUrl="~/Images/document-delete.png" CommandName="DeleteRole"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RoleId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ID" HeaderText="Id" />
                                    <asp:BoundField DataField="RoleId" Visible="false" />
                                    <asp:BoundField DataField="RoleName" HeaderText="Role" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                                    <asp:BoundField DataField="CreatedOn" HeaderText="Created On" />
                                    <asp:BoundField DataField="ModifiedBy" HeaderText="Modified By" />
                                    <asp:BoundField DataField="ModifiedOn" HeaderText="Modified On" />

                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Button ID="btnhidresetDelete" runat="server" Text="Button" Visible="true" Style="visibility: hidden" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender_DeclinRole" runat="server"
                    TargetControlID="btnhidresetDelete" PopupControlID="Panel_DeclinRole"
                    PopupDragHandleControlID="PopupHeader_Declincard" Drag="true" BehaviorID="mpe2"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>

                <asp:Panel ID="Panel_DeclinRole" Style="display: none;" runat="server">
                    <center>
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content" style="width: 700px;">
                            <!-- Modal Header -->
                            <div class="modal-header">
                                <h4 class="modal-title">Enter reason to remove</h4>
                            </div>
                            <!-- Modal body -->
                            <div class="modal-body">
                                <asp:Label ID="lblconfirm" runat="server" Font-Bold="true" Text="Are you sure want to delete ">
                                    <asp:Label ID="lblName" runat="server" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="Label15" Text="?" runat="server" Font-Bold="true"></asp:Label>
                                </asp:Label>
                                <div class="form-group">
                                    <asp:Label ID="lblID" runat="server" Font-Bold="true" Visible="false"></asp:Label>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <asp:TextBox ID="txtResone" runat="server" TextMode="multiline" Rows="5" Placeholder="Please enter reason" Height="100" Width="300" CssClass="input-text form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Modal footer -->
                            <div class="form-group row">
                                <div class="searchbox-btns">
                                    <button class="themeBtn themeApplyBtn" id="btnSaveResone" runat="server" onserverclick="btnSaveResone_ServerClick" type="button" causesvalidation="false">
                                        Delete
                                    </button>
                                    &nbsp;&nbsp;
                            <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnDeleteClose" runat="server" onserverclick="btnDeleteClose_ServerClick">
                                Cancel
                            </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    </center>
                </asp:Panel>

                <asp:Button ID="btnRoleEdit" runat="server" Text="Button" Style="display: none;" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender_EditRole" runat="server"
                    TargetControlID="btnRoleEdit" PopupControlID="Panel_EditRole"
                    PopupDragHandleControlID="PopupHeader_EditRole" Drag="true"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>

                <asp:Panel ID="Panel_EditRole" TabIndex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" runat="server" Style="display: none;">
                    <div class="modal-content" style="width: 700px">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h1 class="modal-title">
                                <asp:Label ID="lbltitlename" runat="server"></asp:Label></h1>
                        </div>
                        <!-- Modal body -->

                        <div class="modal-body">
                            <asp:Label ID="lblEditName" runat="server" Font-Bold="true"></asp:Label>
                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                <div class="col" runat="server">
                                    <asp:Label ID="lblRoleName" runat="server" Text="Role Name:"></asp:Label>
                                    <asp:TextBox ID="txtEditRoleName" runat="server" CssClass="form-control" onkeyup="allLetterNumber(this)" Width="200px" MaxLength="20" onpaste="return false" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col" runat="server">
                                </div>
                                <div class="col" id="divEditClient" runat="server">
                                    <asp:Label ID="lblClient" runat="server" Text="Client:"></asp:Label>
                                    <div class="selectInputBox">
                                        <asp:DropDownList runat="server" ID="ddlClient" CssClass="form-control" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-group" style="max-height: 280px; overflow-x: hidden; margin-bottom: 0px; padding: 5px;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gdvEditMenu" runat="server"
                                                AutoGenerateColumns="false"
                                                Font-Size="Small"
                                                GridLines="None"
                                                AllowPaging="false"
                                                CssClass="GridView"
                                                Visible="true"
                                                DataKeyNames="Name,MenuId,Accessibility"
                                                OnRowDataBound="gdvEditMenu_RowDataBound">
                                                <RowStyle Height="10px" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Page Access">
                                                        <ItemTemplate>
                                                            <a href="javascript:ExpandCollapse('div<%# Eval("MenuId") %>');">
                                                                <img id="imgdiv<%# Eval("MenuId") %>" alt="" style="cursor: pointer" src="../../Images/plus.png" class="float-lg-right" />
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Menu Access">
                                                        <ItemTemplate>
                                                            <label class="switch">
                                                                <asp:CheckBox runat="server" ID="chkEditMainMenuOnOff" Checked="false" AutoPostBack="true" OnCheckedChanged="chkEditMainMenuOnOff_CheckedChanged" />
                                                                <span class="slider round"></span>
                                                            </label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Main Menu">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMenuName" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Menu ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMenuID" runat="server" Text='<%#Eval("MenuId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <ItemTemplate>

                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <tr>
                                                                        <td colspan="100%">
                                                                            <div id="div<%# Eval("MenuId") %>" style="display: none; position: center; overflow-x: hidden; width: 99%;">
                                                                                <asp:GridView ID="gvEditSubMenu"
                                                                                    runat="server"
                                                                                    AutoGenerateColumns="false"
                                                                                    Font-Size="Small"
                                                                                    GridLines="None"
                                                                                    CssClass="GridView"
                                                                                    DataKeyNames="Name,MenuId,SubMenuId,Accessibility"
                                                                                    OnRowDataBound="gvEditSubMenu_RowDataBound"
                                                                                    Visible="true">
                                                                                    <RowStyle Height="10px" />
                                                                                    <Columns>

                                                                                        <asp:TemplateField HeaderText="Access">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="LinkBtnPermissions" runat="server" OnClick="LinkBtnPermissions_Click" Text="" Font-Bold="true" ToolTip="Click To Change Access."></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>

                                                                                        <asp:TemplateField HeaderText="Sub Menu Name">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblSub_SubMenuName" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>

                                                                                        <asp:TemplateField HeaderText="MenuID" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblSub_MenuID" runat="server" Text='<%#Eval("MenuId") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>

                                                                                        <asp:TemplateField HeaderText="Sub MenuID" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblSub_SubMenuID" runat="server" Text='<%#Eval("SubMenuId") %>'>     </asp:Label>

                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>

                                                                                        <asp:TemplateField HeaderText="Access" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblSub_Accessibility" runat="server" Text='<%#Eval("Accessibility") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>

                                                                                    </Columns>
                                                                                </asp:GridView>

                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </ContentTemplate>

                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>


                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <div class="form-group row">
                                <div class="searchbox-btns">
                                    <button class="themeBtn themeApplyBtn" id="btnEditRole" runat="server" type="button" causesvalidation="false" onserverclick="btnEditRole_ServerClick" onclientclick="javascript:return EditConfirmResignation()" visible="false">
                                        Update
                                    </button>
                                    <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnsubmit" runat="server" type="button" causesvalidation="false" onserverclick="btnEditRole_ServerClick" onclientclick="javascript:return EditConfirmResignation()" visible="false">
                                        Submit
                                    </button>
                                    <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnEditClose" type="button" causesvalidation="false" onserverclick="btnEditClose_ServerClick" runat="server">
                                        Cancel
                                    </button>
                                    <%-- </div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Button ID="lbtnNewRoles" runat="server" Text="Button" Style="display: none;" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender_InsertRole" runat="server"
                    TargetControlID="lbtnNewRoles" PopupControlID="Panel_InsertRole"
                    PopupDragHandleControlID="ppdhcIdManualInsert" Drag="true" BehaviorID="mpe"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>

                <asp:Panel ID="Panel_InsertRole" TabIndex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" Style="display: none;" runat="server">
                    <center>
                    <div class="modal commmonModal SearchFilterModal filtersmodal">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Insert Role</h4>
                        </div>
                        <!-- Modal body -->
                        <center>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="col-sm-4 col-xs-12">
                                    <label for="lfname">Role Name<asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label></label>
                                    <asp:TextBox ID="txtRole" runat="server" CssClass="input-text form-control" MaxLength="50" onkeyup="allLetterNumber(this)"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="max-height: 400px; overflow-x: hidden;">

                                <asp:GridView ID="gvMenuAccess" runat="server"
                                    AutoGenerateColumns="false"
                                    Font-Size="Small"
                                    GridLines="None"
                                    AllowPaging="false"
                                    CssClass="GridView"
                                    Visible="true"
                                    DataKeyNames="Name,MenuId"
                                    OnRowDataBound="gvMenuAccess_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <img alt="" style="cursor: pointer; display: none;" src="../../Images/plus.png" class="float-lg-right" />
                                                <asp:Panel ID="pnlStation" runat="server" Style="display: none">
                                                    <asp:GridView ID="gvSubMenu"
                                                        runat="server"
                                                        AutoGenerateColumns="false"
                                                        Font-Size="Small"
                                                        GridLines="None"
                                                        CssClass="GridView"
                                                        DataKeyNames="Name,SubMenuId,MenuId"
                                                        Visible="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <label class="switch">
                                                                        <asp:CheckBox runat="server" ID="chkSubMenuOnOff" /><%--onclick="updateCheckbox(this);"--%>
                                                                        <span class="slider round"></span>
                                                                    </label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Name" HeaderText="Sub Menu" />
                                                            <asp:BoundField DataField="SubMenuId" Visible="false" />
                                                            <asp:BoundField DataField="MenuId" Visible="false" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <label class="switch">
                                                    <asp:CheckBox runat="server" ID="chkMainMenuOnOff" Checked="false" />
                                                    <%-- AutoPostBack="true" OnCheckedChanged="chkMainMenuOnOff_CheckedChanged" --%>
                                                    <span class="slider round"></span>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" ItemStyle-CssClass="text-nowrap" HeaderText="Main Menu" />
                                        <asp:BoundField DataField="MenuId" Visible="false" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        </center>
                        <div class="form-group row">
                            <div class="searchbox-btns">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnSave_Click" OnClientClick="javascript:return ConfirmResignation()"></asp:Button>
                                        <asp:Button ID="btnCancelmi" CssClass="btn btn-secondary" runat="server" Text="Cancel" OnClick="btnCancelmi_Click"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </center>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnXls" />
            <asp:PostBackTrigger ControlID="BtnCsv" />
            <asp:AsyncPostBackTrigger ControlID="ddlClient" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" BackgroundCssClass="modalBackground" DropShadow="false" />
    <%--<script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlClient.ClientID%>").select2();
                }
            });
        };
    </script>--%>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {


                }
            });
        };
    </script>

    <%-- $("#<%=ddlClient.ClientID%>").select2();--%>
</asp:Content>
