<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="pgUserPageManagement.aspx.cs" Inherits="SOR.Pages.Admin.pgUserPageManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
        <link href="../../css/toggleButton.css" rel="stylesheet" />
        <%--<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>--%>
        <script src="../../js/jquery-3.5.1.js"></script>
        <script language="javascript" type="text/javascript">

            function SetTab() {
                debugger;
               // var names = name;
          //  var tab = document.getElementById('<%= TabName.ClientID%>').value;
                var tabs = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "SearchPages";
                $('button[data-bs-target="#' + tabs + '"]').tab('show');
                //$('button[data-bs-target="#AddPages"]').tab('show');
            };
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

            .form-check-inputs {
                background: #FFF !important;
                border: 1px solid var(--color-border-default);
                border-radius: 20px;
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

            .input-text {
                height: 32px;
                background-color: var(--border-box-bg);
                border: 1px solid var(--color-border-default);
                border-radius: 4px;
                margin: 0;
                letter-spacing: 0.02em !important;
                color: var(--color-black) !important;
                font-size: var(--font-m-size12) !important;
            }
        </style>

        <script type="text/javascript">
            $(document).ready(function () {
                var tab = document.getElementById('<%= hidTAB.ClientID%>').value;
                $('#myTabs a[href="' + tab + '"]').tab('show');
            });
        </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div class="breadHeader">
        <h5 class="page-title">Manage Pages</h5>
    </div>
    <asp:HiddenField ID="hidTAB" runat="server" Value="#home" />
    <asp:HiddenField ID="AddPage" Value="#AddPage" runat="server" />
    <asp:HiddenField ID="MapPage" Value="#MapPage" runat="server" />
    <asp:HiddenField ID="EditPage" Value="EditPage" runat="server" />
    <asp:HiddenField ID="SwapPage" Value="#SwapPage" runat="server" />


    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:UpdateProgress ID="upContentBodyUpdateProgress" runat="server" AssociatedUpdatePanelID="upContentBody">
            <ProgressTemplate>
                <img alt="" id="progressImage1" src='<%=Page.ResolveClientUrl("../Images/LdrPlzWait.gif") %>' />
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>

    <div>
        <asp:UpdatePanel ID="upContentBody" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="TabName" runat="server" />
                <div class="nav-tabs-wrapper">
                    <ul class="nav nav-tabs">
                        <li class="nav-item">
                            <button class="nav-link active" id="docket-tab" data-bs-toggle="tab" data-bs-target="#SearchPages" type="button" role="tab" aria-controls="Search-tab-pane" aria-selected="true">Search Pages</button>
                        </li>
                        <li class="nav-item">
                            <button class="nav-link" id="escalation-tab" data-bs-toggle="tab" data-bs-target="#AddPages" type="button" role="tab" aria-controls="Add-tab-pane" aria-selected="false" tabindex="-1">Add Pages</button>
                        </li>
                        <li class="nav-item">
                            <button class="nav-link" id="Map-tab" data-bs-toggle="tab" data-bs-target="#MapPages" type="button" role="tab" aria-controls="Map-tab-pane" aria-selected="false" tabindex="-1">Map Pages</button>
                        </li>
                        <%--<li class="nav-item">
                            <button class="nav-link" id="Edit-tab" data-bs-toggle="tab" data-bs-target="#EditPages" type="button" role="tab" aria-controls="Edit-tab-pane" aria-selected="false" tabindex="-1">Edit Pages</button>
                        </li>
                        <li class="nav-item">
                            <button class="nav-link" id="Swap-tab" data-bs-toggle="tab" data-bs-target="#SwapPages" type="button" role="tab" aria-controls="Swap-tab-pane" aria-selected="false" tabindex="-1">Swap Pages</button>
                        </li>--%>
                    </ul>

                    <div class="tab-content">

                        <div id="SearchPages" class="tab-pane fade show active" tabindex="0">
                            <br />
                            <asp:Panel ID="PanelSearch" runat="server" Width="100%" Style="overflow: hidden;">
                                <div class="accordion summary-accordion" id="history-accordions1">
                                    <div class="accordion-item">
                                        <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes1">
                                            <h6 class="searchHeader-heading">Filter</h6>
                                            <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOneSearch"
                                                aria-expanded="true" aria-controls="collapseOne">
                                                <span class="icon-hide"></span>
                                                <p>Show / Hide</p>
                                            </button>
                                        </div>
                                        <div id="collapseSummaryOneSearch" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                            data-bs-parent="#summary-accordion">
                                            <div class="accordion-body">
                                                <hr class="hr-line">
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <asp:Label ID="lblRole" runat="server" CssClass="selectInputLabel" Text="Role:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlRoleFilter" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="col">
                                                        <asp:Label ID="lblMenu" runat="server" CssClass="selectInputLabel" Text="Menu:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlMenuFilter" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="col">
                                                        <asp:Label ID="lblSubMenu" runat="server" CssClass="selectInputLabel" Text="Sub Menu:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlSubMenuFilter" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <br />
                                                    <div class="col">
                                                        <asp:Label ID="lblAccess" runat="server" CssClass="selectInputLabel" Text="Access:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlAccessFilter" runat="server" CssClass="maximus-select w-100">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Disabled" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Enabled" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="col">
                                                        <div class="selectInputBox">
                                                            <asp:Label ID="lblClient" runat="server" CssClass="selectInputLabel" Text="Client:"></asp:Label>
                                                            <asp:DropDownList ID="ddlClient" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <div class="searchbox-btns">
                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="themeBtn themeApplyBtn" OnClick="btnSearch_Click" data-bs-target="#SearchFilterModal" />
                                                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="themeBtn resetBtn themeCancelBtn me-0" OnClick="btnReset_Click" data-bs-toggle="modal"
                                                            data-bs-target="#SearchFilterModal" />
                                                        <div class="col-md-2">
                                                        </div>
                                                    </div>
                                                    <div class="row d-flex justify-content-center align-items-center">
                                                        <div class="col-auto text-center">
                                                            <strong>
                                                                <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label></strong>
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
                                    </div>
                                </div>
                            </asp:Panel>

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
                            <asp:Panel ID="panelGrid" runat="server" Width="100%" Style="margin: 5px 10px 0px 0px; padding-right: 0px; overflow-x: auto;">
                                <div class="table-box" id="divAddPages" runat="server">
                                    <div class="tableBorderBox" style="padding: 10px 10px;">
                                        <asp:GridView ID="gvPages" runat="server"
                                            AutoGenerateColumns="true"
                                            GridLines="None"
                                            AllowPaging="true"
                                            CssClass="GridView"
                                            PageSize="10"
                                            Visible="true"
                                            PagerSettings-Mode="NumericFirstLast"
                                            PagerSettings-FirstPageText="First Page"
                                            PagerSettings-LastPageText="Last Page"
                                            OnPageIndexChanging="gvPages_PageIndexChanging">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                            <RowStyle Wrap="false" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No." HeaderStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text='<%  #(Container.DataItemIndex) + 1%>'></asp:Label></td>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="Role ID" HeaderText="Role ID" />
                                                <asp:BoundField DataField="Role Name" HeaderText="Role Name" />
                                                <asp:BoundField DataField="Menu ID" HeaderText="Menu ID" />
                                                <asp:BoundField DataField="Menu" HeaderText="Menu" />
                                                <asp:BoundField DataField="Sub Menu ID" HeaderText="Sub Menu ID" />
                                                <asp:BoundField DataField="Sub Menu" HeaderText="Sub Menu" />
                                                <asp:BoundField DataField="Menu Access" HeaderText="Menu Access" />
                                                <asp:BoundField DataField="Sub Menu Access" HeaderText="Sub Menu Access" />
                                                <asp:BoundField DataField="Page Path" HeaderText="Page Path" />
                                                <asp:BoundField DataField="Created By" HeaderText="Created By" />
                                                <asp:BoundField DataField="Created On" HeaderText="Created On" />
                                                <asp:BoundField DataField="Last Modified By" HeaderText="Last Modified By" />
                                                <asp:BoundField DataField="Last Modified On" HeaderText="Last Modified On" />--%>
                                            </Columns>
                                        </asp:GridView>

                                    </div>
                                </div>
                            </asp:Panel>
                        </div>


                        <div id="AddPages" class="tab-pane fade">
                            <br />
                            <asp:Panel ID="PanelAddPages" runat="server" Width="100%" Style="overflow: hidden;">
                                <div class="accordion summary-accordion" id="history-accordions">
                                    <div class="accordion-item">
                                        <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes">
                                            <h6 class="searchHeader-heading">Filter</h6>
                                            <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOneAdd"
                                                aria-expanded="true" aria-controls="collapseOne">
                                                <span class="icon-hide"></span>
                                                <p>Show / Hide</p>
                                            </button>
                                        </div>
                                        <div id="collapseSummaryOneAdd" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                            data-bs-parent="#summary-accordion">
                                            <div class="accordion-body">
                                                <hr class="hr-line">
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col" style="display: none">
                                                        <label class="selectInputLabel" for="selectInputLabel">Client:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlClientForMenu" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col">
                                                        <asp:Label ID="lblUserName" runat="server" CssClass="selectInputLabel" Text="Menu Name:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:TextBox ID="txtMenu" runat="server" CssClass="input-text form-control" placeholder="Enter Menu Name" Style="width: 100%"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtMenu" />
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="lblpagepath" runat="server" CssClass="selectInputLabel" Text="Page Path::"></asp:Label>

                                                        <div class="selectInputBox">
                                                            <asp:TextBox ID="txtMenuPath" runat="server" CssClass="input-text form-control" placeholder="Enter Page Path" Style="width: 100%"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars="./" TargetControlID="txtMenuPath" />
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <div class="selectInputBox">
                                                            <asp:Label ID="lblacc" runat="server" CssClass="selectInputLabel" Text="Access:"></asp:Label>

                                                            <asp:DropDownList ID="ddlMenuAccess" runat="server" CssClass="maximus-select w-100">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Disabled" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Enabled" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="Label14" runat="server" CssClass="selectInputLabel" Text="Icon Options:"></asp:Label>
                                                        <div class="d-md-flex align-items-center d-block w-100">
                                                            <asp:RadioButtonList ID="RadioIcons" runat="server" CssClass="form-check-inputs" ClientIDMode="static" RepeatDirection="Horizontal" ToolTip="Select Configuration Category">
                                                                <asp:ListItem Text='<span class="icon-dashboard sidebar-icons"></span>' Value="icon-dashboard sidebar-icons" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text='<span class="icon-ticket-centre sidebar-icons"></span>' Value="icon-ticket-centre sidebar-icons"></asp:ListItem>
                                                                <asp:ListItem Text='<span class="icon-new-ticket sidebar-icons"></span>' Value="icon-new-ticket sidebar-icons"></asp:ListItem>
                                                                <asp:ListItem Text='<span class="icon-reports sidebar-icons"></span>' Value="icon-reports sidebar-icons"></asp:ListItem>
                                                                <asp:ListItem Text='<span class="icon-ticket-history sidebar-icons"></span>' Value="icon-ticket-history sidebar-icons"></asp:ListItem>
                                                                <asp:ListItem Text='<span class="icon-terminal-status sidebar-icons"></span>' Value="icon-terminal-status sidebar-icons"></asp:ListItem>
                                                                <asp:ListItem Text='<span class="icon-uploads sidebar-icons"></span>' Value="icon-uploads sidebar-icons"></asp:ListItem>
                                                                <asp:ListItem Text='<span class="icon-housekeeping-activity sidebar-icons"></span>' Value="icon-housekeeping-activity sidebar-icons"></asp:ListItem>
                                                                <asp:ListItem Text='<span class="icon-manual sidebar-icons"></span>' Value="2"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <button type="button" id="btnAddMenu" runat="server" class="themeBtn themeApplyBtn" causesvalidation="false" onserverclick="btnAddMenu_ServerClick">Add</button>
                                                    </div>
                                                </div>

                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col" style="display: none">
                                                        <label class="selectInputLabel" for="selectInputLabel">Client:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlClientForSubMenu" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="Label1" runat="server" CssClass="selectInputLabel" Text="Sub Menu Name:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:TextBox ID="txtSubMenuName" runat="server" CssClass="input-text form-control" placeholder="Enter Sub Menu Name" Style="width: 100%"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtMenu" />
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="Label2" runat="server" CssClass="selectInputLabel" Text="Page Path::"></asp:Label>

                                                        <div class="selectInputBox">
                                                            <asp:TextBox ID="txtSubMenuPath" runat="server" CssClass="input-text form-control" placeholder="Enter Page Path" Style="width: 100%"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars="./" TargetControlID="txtMenuPath" />
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <div class="selectInputBox">
                                                            <asp:Label ID="Label3" runat="server" CssClass="selectInputLabel" Text="Access:"></asp:Label>
                                                            <asp:DropDownList ID="ddlSubMenuAccess" runat="server" CssClass="maximus-select w-100">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Disabled" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Enabled" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <button type="button" id="btnAddSubMenu" runat="server" class="themeBtn themeApplyBtn" causesvalidation="false" onserverclick="btnAddSubMenu_ServerClick">Add</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>


                        <div id="MapPages" class="tab-pane fade">
                            <br />
                            <asp:Panel ID="Panel1" runat="server" Width="100%" Style="overflow: hidden;">
                                <div class="accordion summary-accordion" id="history-accordions3">
                                    <div class="accordion-item">
                                        <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes3">
                                            <h6 class="searchHeader-heading">Filter</h6>
                                            <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOneMap"
                                                aria-expanded="true" aria-controls="collapseOne">
                                                <span class="icon-hide"></span>
                                                <p>Show / Hide</p>
                                            </button>
                                        </div>
                                        <div id="collapseSummaryOneMap" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                            data-bs-parent="#summary-accordion">
                                            <div class="accordion-body">
                                                <hr class="hr-line">
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col" style="display: none">
                                                        <label class="selectInputLabel" for="selectInputLabel">Client:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlClientForMap" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Role:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Menu:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlMenuMapping" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlMenuMapping_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Sub Menu:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlSubMenuMapping" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Accesss:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlMappingAccess" runat="server" CssClass="maximus-select w-100">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Disabled" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Enabled" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <button type="button" id="btnMapping" runat="server" class="themeBtn themeApplyBtn" causesvalidation="false" onserverclick="btnMapping_ServerClick">Add</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>


                        <div id="EditPages" class="tab-pane fade">
                            <br />
                            <asp:Panel ID="PanelEdit" runat="server" Width="100%" Style="overflow: hidden;">
                                <div class="accordion summary-accordion" id="history-accordions4">
                                    <div class="accordion-item">
                                        <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes4">
                                            <h6 class="searchHeader-heading">Filter</h6>
                                            <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOneEdit"
                                                aria-expanded="true" aria-controls="collapseOne">
                                                <span class="icon-hide"></span>
                                                <p>Show / Hide</p>
                                            </button>
                                        </div>
                                        <div id="collapseSummaryOneEdit" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                            data-bs-parent="#summary-accordion">
                                            <div class="accordion-body">
                                                <hr class="hr-line">
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col" style="display: none">
                                                        <label class="selectInputLabel" for="selectInputLabel">Client:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlClientForEditMenu" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="Label4" runat="server" CssClass="selectInputLabel" Text="Menu:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlMenuEdit" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlMenuEdit_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col">
                                                        <asp:Label ID="Label5" runat="server" CssClass="selectInputLabel" Text="Display As:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:TextBox ID="txtMenuEdit" runat="server" CssClass="input-text form-control" placeholder="Enter Display As" Style="width: 100%"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                    <div class="col">
                                                        <asp:Label ID="Label6" runat="server" CssClass="selectInputLabel" Text="Page Path:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:TextBox ID="txtMenuPathEdit" runat="server" CssClass="input-text form-control" placeholder="Enter Page Path" Style="width: 100%"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="Label7" runat="server" CssClass="selectInputLabel" Text="Menu:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlMenuEditAccess" runat="server" CssClass="maximus-select w-100">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Disabled" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Enabled" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <asp:Label ID="Label12" runat="server" CssClass="selectInputLabel" Text="Icon Options:"></asp:Label>
                                                        <asp:RadioButtonList ID="RadioMIcon" runat="server" CssClass="form-check-inputs" ClientIDMode="static" RepeatDirection="Horizontal" ToolTip="Select Configuration Category">
                                                            <asp:ListItem Text='<span class="icon-dashboard sidebar-icons"></span>' Value="icon-dashboard sidebar-icons" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text='<span class="icon-ticket-centre sidebar-icons"></span>' Value="icon-ticket-centre sidebar-icons"></asp:ListItem>
                                                            <asp:ListItem Text='<span class="icon-new-ticket sidebar-icons"></span>' Value="icon-new-ticket sidebar-icons"></asp:ListItem>
                                                            <asp:ListItem Text='<span class="icon-reports sidebar-icons"></span>' Value="icon-reports sidebar-icons"></asp:ListItem>
                                                            <asp:ListItem Text='<span class="icon-ticket-history sidebar-icons"></span>' Value="icon-ticket-history sidebar-icons"></asp:ListItem>
                                                            <asp:ListItem Text='<span class="icon-terminal-status sidebar-icons"></span>' Value="icon-terminal-status sidebar-icons"></asp:ListItem>
                                                            <asp:ListItem Text='<span class="icon-uploads sidebar-icons"></span>' Value="icon-uploads sidebar-icons"></asp:ListItem>
                                                            <asp:ListItem Text='<span class="icon-housekeeping-activity sidebar-icons"></span>' Value="icon-housekeeping-activity sidebar-icons"></asp:ListItem>
                                                            <asp:ListItem Text='<span class="icon-manual sidebar-icons"></span>' Value="2"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <button type="button" id="btnMenuEdit" runat="server" class="themeBtn themeApplyBtn" causesvalidation="false" onserverclick="btnMenuEdit_ServerClick">Add</button>
                                                    </div>
                                                </div>

                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col" style="display: none">
                                                        <label class="selectInputLabel" for="selectInputLabel">Client:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlClientEditSubMenu" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="Label8" runat="server" CssClass="selectInputLabel" Text="Sub Menu:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlSubMenuEdit" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlSubMenuEdit_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col">
                                                        <asp:Label ID="Label9" runat="server" CssClass="selectInputLabel" Text="Display As:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:TextBox ID="txtSubMenuEdit" runat="server" CssClass="input-text form-control" placeholder="Enter Display As" Style="width: 100%"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                    <div class="col">
                                                        <asp:Label ID="Label10" runat="server" CssClass="selectInputLabel" Text="Page Path:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:TextBox ID="txtSubMenuPathEdit" runat="server" CssClass="input-text form-control" placeholder="Enter Page Path" Style="width: 100%"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="Label11" runat="server" CssClass="selectInputLabel" Text="Menu:"></asp:Label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlSubMenuEditAccess" runat="server" CssClass="maximus-select w-100">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Disabled" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Enabled" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <button type="button" id="btnSubMenuEdit" runat="server" class="themeBtn themeApplyBtn" causesvalidation="false" onserverclick="btnSubMenuEdit_ServerClick">Add</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>


                        <div id="SwapPages" class="tab-pane fade">
                            <br />

                            <asp:Panel ID="Panel2" runat="server" Width="100%" Style="overflow: hidden;">
                                <div class="accordion summary-accordion" id="history-accordions5">
                                    <div class="accordion-item">
                                        <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes5">
                                            <h6 class="searchHeader-heading">Filter</h6>
                                            <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOneSwap"
                                                aria-expanded="true" aria-controls="collapseOne">
                                                <span class="icon-hide"></span>
                                                <p>Show / Hide</p>
                                            </button>
                                        </div>
                                        <div id="collapseSummaryOneSwap" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                            data-bs-parent="#summary-accordion">
                                            <div class="accordion-body">
                                                <hr class="hr-line">
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Client:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlClientForSwap" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Role:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlEditRole" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlEditRole_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Menu:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlEditMenu" runat="server" CssClass="maximus-select w-100">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Sub Menu:</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList ID="ddlEditSubMenu" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlEditSubMenu_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Page Path:</label>
                                                        <div class="inputBox w-100">
                                                            <input type="text" id="TxtEditSubMenuPath" runat="server" class="input-text form-control" style="width: 100%" placeholder="Enter Page Path">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <button type="button" id="btnSwapPages" runat="server" class="themeBtn themeApplyBtn" causesvalidation="false" onserverclick="btnSwapPages_ServerClick">Add</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
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
                    $("#<%=ddlMenuMapping.ClientID%>").select2();
                    $("#<%=ddlSubMenuMapping.ClientID%>").select2();
                    $("#<%=ddlRole.ClientID%>").select2();
                    $("#<%=ddlMenuEdit.ClientID%>").select2();
                    $("#<%=ddlSubMenuEdit.ClientID%>").select2();
                    $("#<%=ddlSubMenuFilter.ClientID%>").select2();
                    $("#<%=ddlMenuFilter.ClientID%>").select2();
                    $("#<%=ddlRoleFilter.ClientID%>").select2();
                    $("#<%=ddlEditMenu.ClientID%>").select2();
                    $("#<%=ddlEditSubMenu.ClientID%>").select2();
                    $("#<%=ddlEditRole.ClientID%>").select2();
                    $("#<%=ddlAccessFilter.ClientID%>").select2();
                    $("#<%=ddlClient.ClientID%>").select2();
                    $("#<%=ddlMenuAccess.ClientID%>").select2();
                    $("#<%=ddlSubMenuAccess.ClientID%>").select2();
                    $("#<%=ddlClientForSwap.ClientID%>").select2();
                    $("#<%=ddlMappingAccess.ClientID%>").select2();
                    $("#<%=ddlSubMenuEditAccess.ClientID%>").select2();
                    $("#<%=ddlMenuEditAccess.ClientID%>").select2();
                }
            });
        };
    </script>
</asp:Content>
