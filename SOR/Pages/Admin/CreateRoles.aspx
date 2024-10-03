<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="CreateRoles.aspx.cs" Inherits="SOR.Pages.Admin.CreateRoles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../css/toggleButton.css" rel="stylesheet" />
    <%--<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>--%>
    <script src="../../js/jquery-3.5.1.js"></script>
    <script type="text/javascript">
        function SetactiveTab() {
            debugger;
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "AddRole";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        };


        function SetTab() {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "AddRole";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        };



        function SetTab() {
            debugger;
            $('button[data-bs-target="#MapRole"]').tab('show');
        }

        function ExpandCollapse(obj) {
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
        function fn_Test() {
            alert();
            return true;
        }
    </script>

    <style>
        .nav-tabs-wrapper .nav-tabs .nav-link.active {
            background-color: #fbd2ce;
            font-weight: 600;
            color: var(--color-primary-default);
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
                font-size: small;
                border: 1px solid #CCC;
            }

            .GridView tr:nth-child(even) {
                background: #eaecff !important;
                font-size: small;
            }

            .GridView tr:nth-child(odd) {
                background: #FFF !important;
                font-size: x-small;
            }
    </style>

    <style>
        .modalBackground {
            filter: alpha(opacity=70);
            background-color: lightgray;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            width: 250px;
            opacity: 0.7;
        }

        .ajax__calendar_container {
            z-index: 999999;
        }
    </style>

    <style>
        .GridView {
            width: 100% !Important;
        }

        .form-group {
            margin-bottom: 5px;
        }

        table {
            width: 0% !Important;
        }

        .tabs {
            font-family: Calibri;
            font-size: 9pt;
        }
        /*table {
            width: 100% !Important;
        }*/

        fieldset.scheduler-border {
            border: solid 1px lightgray !important;
            border-radius: 5px;
            padding: 0 0px 10px 5px;
            font-size: small;
            border-bottom: none;
            overflow-y: hidden;
            overflow-x: hidden;
        }

        .select2 {
            width: 100% !important;
        }

        legend.scheduler-border {
            width: auto !important;
            border: none;
            font-size: small;
            overflow-y: hidden;
            overflow-x: hidden;
            color: #1E3C6E;
            font-weight: bold;
        }

        .form-control {
            height: 25px;
        }

        .ajax__tab_xp .ajax__tab_body {
            font-family: inherit;
        }

        .ajax__tab_xp .ajax__tab_tab {
            height: 19px;
            padding: 4px;
            margin: 0;
        }

        span {
            font-family: var(--primary-font);
            font-size: small;
        }

        .nav-tabs-wrapper .nav-tabs .nav-link {
            background-color: var(--border-box-bg);
            border: 1px solid var(--color-border-default);
            border-radius: 8px 8px 0px 0px;
            font-weight: 500;
            font-size: small;
            color: var(--color-black);
            padding: 9px 12px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div class="breadHeader">
        <h5 class="page-title">Create / Manage Role</h5>
    </div>
    <asp:HiddenField ID="hidTAB" runat="server" Value="#home" />
    <asp:HiddenField ID="TabName" Value="MapRole" runat="server" />

    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:UpdateProgress ID="upContentBodyUpdateProgress" runat="server" AssociatedUpdatePanelID="upContentBody">
            <ProgressTemplate>
                <div style="position: fixed; left: 40%; top: 50%; opacity: 1.8;">
                    <img alt="" id="progressImage1" src='<%=Page.ResolveClientUrl("../Images/LdrPlzWait.gif") %>' />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>
    <div>
        <asp:UpdatePanel ID="upContentBody" runat="server">
            <ContentTemplate>
                <div class="nav-tabs-wrapper">
                    <%--<ul class="nav nav-tabs">
                        <li class="nav-item">
                            <button class="nav-link active" id="docket-tab" data-bs-toggle="tab" data-bs-target="#AddRole" type="button" role="tab" aria-controls="docket-tab-pane" aria-selected="true">Roles</button>
                        </li>
                        <li class="nav-item">
                            <button class="nav-link" id="escalation-tab" data-bs-toggle="tab" data-bs-target="#MapRole" type="button" role="tab" aria-controls="escalation-tab-pane" aria-selected="false" tabindex="-1">Map Roles</button>
                        </li>
                    </ul>--%>

                    <%-- <div class="tab-content border mb-3">--%>
                    <%--<div class="tab-content">--%>
                        <div id="AddRole" class="tab-pane fade show active" tabindex="0">
                            <asp:Panel ID="pnlAddPages" runat="server" Width="100%" Style="overflow: hidden;">
                                <div class="accordion summary-accordion" id="history-accordions">
                                    <div class="accordion-item">
                                        <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes">
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
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Role:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlRoleID" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlRoleID_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                     <div class="searchbox-btns">
                                                        <button type="button" id="lbtnAddRole" runat="server" class="themeBtn themeApplyBtn" onclick="$('#dvLoading').show();" onserverclick="lbtnAddRole_ServerClick">
                                                            Add New Role</button>
                                                            <div class="col" style="margin-left: 610px;">
                                                                <asp:ImageButton ID="BtnCsv" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                                    ToolTip="Csv" OnClick="BtnCsv_Click" data-toggle="modal" data-target="#myModal" />
                                                            </div>
                                                         <div class="col">
                                                             <asp:ImageButton ID="BtnXls" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                                    ToolTip="Xls" OnClick="BtnXls_Click" data-toggle="modal" data-target="#myModal" />
                                                         </div>
                                                        
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                   
                                                </div>

                                                <div class="row">
                                                    <div class="form-group">
                                                        <div class="col-sm-12 col-md-12 col-lg-12">
                                                            <center><strong><asp:Label ID="lblRecordsCount" runat="server" Text=""></asp:Label></strong></center>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panelGrid" runat="server" Width="100%" HorizontalScroll="false" ScrollBars="None" Style="margin: 5px 10px 0px 0px; padding-right: 0px;">
                                <div class="table-box" id="divRoleDetails" runat="server">
                                    <div class="tableBorderBox" style="padding: 10px 10px;">
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
                                                <asp:BoundField DataField="RoleId" Visible="false" />
                                                <asp:TemplateField HeaderText="Role">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRolename" runat="server" Text='<%#Eval("RoleName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtRleName" runat="server" Width="100px" Text='<%#Eval("RoleName")%>' />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Created By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Created On">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCreatedOn" runat="server" Text='<%#Eval("CreatedOn") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Modified By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModifiedBy" runat="server" Text='<%#Eval("ModifiedBy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Modified On">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModifiedOn" runat="server" Text='<%#Eval("ModifiedOn") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </asp:Panel>

                        </div>

                        <%--<div id="MapRole" class="tab-pane fade">
                            <asp:Panel ID="PanelMappingPages" runat="server" Width="100%" Style="overflow: hidden; margin: 0px 0px;">
                                <div class="accordion summary-accordion" id="history-accordion">
                                    <div class="accordion-item">
                                        <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOne">
                                            <h6 class="searchHeader-heading">Filter</h6>
                                            <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOneds"
                                                aria-expanded="true" aria-controls="collapseOne">
                                                <span class="icon-hide"></span>
                                                <p>Show / Hide</p>
                                            </button>
                                        </div>
                                        <div id="collapseSummaryOneds" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                            data-bs-parent="#summary-accordion">
                                            <div class="accordion-body">
                                                <hr class="hr-line">
                                                <div class="row">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <div class="col-sm-6">
                                                                <div class="form-horizontal">
                                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                                        <div class="col-sm-4">
                                                                            <label class="selectInputLabel" for="selectInputLabel">Client:</label>
                                                                        </div>
                                                                        <div class="col-sm-4">
                                                                            <div class="selectInputBox">
                                                                                <asp:DropDownList ID="ddlClient" runat="server" CssClass="maximus-select w-100" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                                        <div class="col-sm-4">
                                                                            <label class="selectInputLabel" for="selectInputLabel">User Role Group:</label>
                                                                        </div>
                                                                        <div class="col-sm-4">
                                                                            <div class="selectInputBox">
                                                                                <asp:DropDownList ID="ddluserRoleGroup" runat="server" CssClass="maximus-select w-100" OnSelectedIndexChanged="ddluserRoleGroup_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                                        <div class="col-sm-4">
                                                                            <label class="selectInputLabel" for="selectInputLabel">Role:</label>
                                                                        </div>
                                                                        <div class="col-sm-4">
                                                                            <div class="selectInputBox">
                                                                                <asp:DropDownList ID="ddlRoles" runat="server" CssClass="maximus-select w-100" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <style>
                                                        .ajax__tab_xp .ajax__tab_tab {
                                                            height: 19px;
                                                            padding: 4px;
                                                            margin: 0;
                                                        }

                                                        .ajax__tab_xp .ajax__tab_body {
                                                            font-family: inherit;
                                                        }
                                                    </style>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="table-box">
                                    <div class="tableBorderBox" style="padding: 10px 10px;">
                                        <asp:Panel ID="PanelMappingRoles" runat="server">
                                            <div class="col-sm-12">
                                                <div class="form-group row">
                                                    <div class="col-sm-12 col-xs-12">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
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
                                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                                                                    <ContentTemplate>
                                                                                        <tr>
                                                                                            <td colspan="100%">
                                                                                                <div id="div<%# Eval("MenuId") %>" style="display: none; position: center; overflow-y: hidden; width: 99%;">
                                                                                                    <asp:GridView ID="gvEditSubMenu"
                                                                                                        runat="server"
                                                                                                        AutoGenerateColumns="false"
                                                                                                        Font-Size="Smaller"
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

                                                <div class="form-group row">
                                                    <div class="searchbox-btns">
                                                        <asp:Button ID="btnEditRole" runat="server" CausesValidation="false" CssClass="themeBtn themeApplyBtn" Text="Submit" OnClick="btnEditRole_Click"></asp:Button>
                                                        &nbsp;&nbsp;
                                                    <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnEditClose" type="button" causesvalidation="false" onserverclick="btnEditClose_ServerClick" runat="server" visible="false">
                                                        Cancel
                                                    </button>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <%--</fieldset>--%>
                            <%--</asp:Panel>
                        </div>--%>
                    </div>

                    <asp:Button ID="btnhidresetDelete" runat="server" Text="Button" Visible="true" Style="visibility: hidden" />
                    <cc1:ModalPopupExtender ID="ModalPopupExtender_Declincard" runat="server"
                        TargetControlID="btnhidresetDelete" PopupControlID="Panel_Declincard"
                        PopupDragHandleControlID="PopupHeader_Declincard" Drag="true"
                        BackgroundCssClass="modalBackground">
                    </cc1:ModalPopupExtender>


                    <asp:Panel ID="Panel_Declincard" Style="display: none;" runat="server">
                        <center>
                        <div class="modal-dialog modal-dialog-centered" role="document">
                            <div class="modal-content" style="width: 469px;height: 200px;">
                                <!-- Modal Header -->
                                <div class="modal-header">
                                    <h3 class="modal-title">
                                        <asp:Label ID="lblModalHeaderName" runat="server"></asp:Label>
                                    </h3>
                                </div>
                                <!-- Modal body Aarti-->
                                <div class="modal-body ">
                                    <asp:Label ID="lblconfirm" runat="server" Font-Bold="true"></asp:Label>
                                    <div class="form-group">
                                        <div class="row">
                                            <div class="form-group" id="showrolename" runat="server">
                                                <div class="col-sm-8 col-xs-12">
                                                    <label for="lfname">Role Name:<asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label></label>
                                                    <asp:TextBox ID="txtRole" runat="server" CssClass="input-text form-control" MaxLength="15"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers" ValidChars=" " TargetControlID="txtRole" />
                                                    <%--<asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "txtRole" ID="RegularExpressionValidator3" ValidationExpression = "^[\s\S]{2,15}$" runat="server" ErrorMessage="Minimum 2 and Maximum 15 characters required."></asp:RegularExpressionValidator>--%>
                                                </div>
                                            </div>
                                            <div class="form-group" id="DivEnterReason" runat="server">
                                                <div class="row">
                                                    <div class="col-sm-12" style="padding: 0 28px 0 28px;">
                                                        <asp:TextBox ID="txtResone" runat="server" TextMode="multiline" Rows="5" Placeholder="Please enter reason" Height="100"  width="300" CssClass="input-text form-control" Style="resize: none"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- Modal footer -->
                               

                                 <div class="form-group row">
                                  <div class="searchbox-btns">
                                    <asp:Button ID="btnSaveAction" runat="server" Text="Delete" CssClass="themeBtn themeApplyBtn" OnClick="btnSaveAction_Click" />
                                    <asp:Button ID="btnupdate" runat="server" Text="Update" CssClass="themeBtn themeApplyBtn" OnClick="btnSaveAction_Click" />
                                    <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnCancelAction" type="button" runat="server" causesvalidation="false" onserverclick="btnCancelAction_ServerClick">Cancel</button>
                                </div>
                               </div>
                             
                            </div>
                        </div>
                         </center>
                    </asp:Panel>
                    </center>
                <asp:Button ID="btnRoleEdit" runat="server" Text="Button" Style="display: none;" />
                    <cc1:ModalPopupExtender ID="ModalPopupExtender_EditRole" runat="server"
                        TargetControlID="btnRoleEdit" PopupControlID="Panel_EditRole"
                        PopupDragHandleControlID="PopupHeader_EditRole" Drag="true"
                        BackgroundCssClass="modalBackground">
                    </cc1:ModalPopupExtender>

                    <asp:Panel ID="Panel_EditRole" TabIndex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true"
                        Style="display: none;" runat="server">
                        <center>
                    <div class="modal-content" style="width: 469px;height: 200px;">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Add New Role</h4>
                        </div>
                        <!-- Modal body -->
                       
                            <div class="form-group"  style="padding-top: 10px;">
                                <asp:Label ID="lblID" runat="server" Font-Bold="true" Visible="false"></asp:Label>
                                <label class="control-label col-sm-2" for="email" >Role Name:</label></h1>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtRolename" runat="server" CssClass="input-text form-control" Height="50" Width="200" placeholder="Enter Role Name" AutoCompleteType="None" AutoComplete="off"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers" ValidChars=" " TargetControlID="txtRolename" />
                                </div>
                            </div>
                      
                        <div class="Prefix">
                        </div>
                        <div class="form-group row">
                         <div class="searchbox-btns">
                        <div class="row-cols-2" >
                            <button class="themeBtn themeApplyBtn" id="lbtnNewRole" runat="server" onserverclick="lbtnNewRole_ServerClick" type="button" causesvalidation="false">
                                Submit
                            </button>
                            <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnclose" type="button" causesvalidation="false" onserverclick="btnclose_ServerClick" runat="server" >
                                Cancel
                            </button>
                        </div>
                    </div>
                 </div>
                    </div>
                    </center>
                    </asp:Panel>

                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="BtnCsv" />
                <asp:PostBackTrigger ControlID="BtnXls" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" BackgroundCssClass="modalBackground" DropShadow="false" />
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                  <%--  $("#<%=ddlClient.ClientID%>").select2();
                    $("#<%=ddluserRoleGroup.ClientID%>").select2();--%>
                    $("#<%=ddlRoleID.ClientID%>").select2();
                    <%--$("#<%=ddlRoles.ClientID%>").select2();--%>
                }
            });
        };
    </script>

    <script>   // select2
        $('.maximus-select').select2({
            placeholder: "All",
        });</script>

</asp:Content>
