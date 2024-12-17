<%@ Page Title="" Language="C#" MasterPageFile="~/PayRakamSBM.Master" AutoEventWireup="true" CodeBehind="AgLvl2.aspx.cs" Inherits="PayRakamSBM.Pages.Agent.AgLvl2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 121px;
        }

        .auto-style2 {
            width: 149px
        }

        .auto-style4 {
            width: 288px
        }

        .auto-style5 {
            width: 174px
        }
    </style>

    <style>
        .col-lg-3 {
            width: 25%;
        }

        .mb50 {
            margin-left: 277px;
        }
    </style>
    <style>
        .pagination-ys {
            color: black;
            background-color: #11AEA3;
            border: 1px solid #dddddd;
            margin-left: -1px;
            font-family: Arial Black;
        }

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
                width: 1%;
                border-collapse: collapse;
            }

            .GridView th {
                background: #FFB2B2;
                color: black;
                font-weight: bold !important;
                padding: 4px 20px 4px 4px !important;
            }

            .GridView td {
                background: none;
                color: #420629;
                padding: 5px;
                border: 1px solid #CCC;
            }


        .select-grid-gap.row > * {
            margin-bottom: 10px;
        }

        .GridView tr:nth-child(even) {
            background: #eaecff !important;
        }

        .GridView tr:nth-child(odd) {
            background: #FFF !important;
        }

        .table tbody tr th {
            background: none;
            color: #fbd2ce;
        }

        th {
            background-color: #fbd2ce;
            color: black;
        }

        .col-md-2 {
            flex: 0 0 auto;
            width: 12.666667%;
        }
    </style>
    <style>
        .HeaderStyle table tbody tr th {
            background-color: #fbd2ce !important;
            font-weight: 400;
            position: sticky;
            white-space: nowrap;
            width: auto;
            z-index: 100;
            top: -8px;
        }
    </style>

    <style type="text/css">
        .btnCommonCSS {
            height: 30px;
            margin-top: 8px;
        }

        .GridView tr:nth-child(odd) {
            background: #202565;
            color: #FFF;
        }

        .form-group {
            margin-bottom: 5px;
        }

        .btnClass {
            margin: 5px;
        }
    </style>
    <style type="text/css">
        .ajax__calendar_container {
            z-index: 1000;
            left: 0px !important;
            /*position:static;*/
            border: 1px solid #646464;
            background-color: white;
            color: black;
            visibility: visible;
            */ display: block;
            margin-left: 26px;
        }
    </style>

    <style>
        #ctl00_cpbdCarde_gvBusinessCorrespondents {
            width: 100% !Important;
        }


        .select2 {
            width: 100% !important;
        }

        .btnClass {
            margin: 5px;
        }
    </style>

    <style type="text/css">
        .input-group-addon {
            border: 1px solid;
        }

        .themeCancelBtn {
            background-color: transparent;
            border: 1px solid var(--color-primary-default);
            color: var(--color-primary-default);
            margin-top: 19px;
        }

        .col-md-3 {
            flex: 0 0 auto;
            width: 20%;
        }
    </style>

    <style>
        .nav-tabs-wrapper .nav-tabs .nav-link.active {
            background-color: #fbd2ce;
            font-weight: 600;
            color: var(--color-primary-default);
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

        .datepicker tbody, td, tfoot, th, thead, tr {
            border-color: inherit;
            border-style: solid;
            border-width: 0;
            padding: 0px;
        }
    </style>

    <script type="text/javascript">
        function ExportPDf() {
            debugger;
            window.open('PdfExport.aspx', '_blank')
        }
    </script>
    <script type="text/javascript">

        function CloseModal() {
            debugger;
            $('#ModalAgent').modal('hide');
        }

        function openModal() {
            debugger;
            $('#ModalAgent').modal('show');
        }

        function ClearRemark() {
            debugger;

            document.getElementById("<%=txtFinalRemarks.ClientID %>").value = '';
            var elementRef = document.getElementById('<%= rdbtnApproveDecline.ClientID %>');
            var inputElementArray = elementRef.getElementsByTagName('input');
            for (var i = 0; i < inputElementArray.length; i++) {
                var inputElement = inputElementArray[i];
                inputElement.checked = false;
            }
            $('#ModalAgent').modal('hide');
        }
    </script>
    <script type="text/javascript">
        function disableMultipleClick() {
            debugger;
            document.getElementById("cpbdCarde_btnSaveAction").disabled = true;
            document.getElementById("cpbdCarde_btnSaveAction").innerHTML = "Processing";
            __doPostBack('ctl00$cpbdCarde$btnSaveAction', '');
        }
        function Confirm() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Save Data?")) {
                debugger;
                document.getElementById("<%= hdnUserConfirmation.ClientID %>").value = "Yes";
            }
            else {
                document.getElementById("<%= hdnUserConfirmation.ClientID %>").value = "No";
            }

            //document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">
        function disableMultipleClickSub() {
            debugger;
            document.getElementById("cpbdCarde_btnSubmitDetails").disabled = true;
            document.getElementById("cpbdCarde_btnSubmitDetails").innerHTML = "Processing";
            __doPostBack('ctl00$cpbdCarde$btnSubmitDetails', '');
        }
    </script>
    <script type="text/javascript">
        function SetTab() {
            debugger;
            $('button[data-bs-target="#Bulk"]').tab('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:UpdateProgress ID="upContentBodyUpdateProgress" runat="server" AssociatedUpdatePanelID="upContentBody">
            <ProgressTemplate>
                <div style="position: fixed; left: 50%; top: 50%; opacity: 1.8;">
                    <img alt="" id="progressImage1" src='<%=Page.ResolveClientUrl("../images/LdrPlzWait.gif") %>' />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>
    <asp:UpdatePanel ID="upContentBody" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnUserConfirmation" runat="server" />
            <div class="container-fluid">
                <div class="breadHeader">
                    <h5 class="page-title">Agent Verification Level Two</h5>
                </div>
                <!-- Filter Accordion -->
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
                                <asp:UpdatePanel ID="RegButtons" runat="server">
                                    <ContentTemplate>
                                        <!-- grid -->
                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                            <!-- input -->
                                            <div class="col">
                                                <label class="selectInputLabel" for="selectInputLabel">Business Correspondents:</label>
                                                <div class="selectInputBox">
                                                    <asp:DropDownList ID="ddlBCCode" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlBCCode_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <!-- input -->
                                            <div class="col">
                                                <label class="selectInputLabel" for="selectInputLabel">Agent:</label>
                                                <div class="selectInputBox">
                                                    <asp:DropDownList ID="ddlAgentCode" runat="server" CssClass="maximus-select w-100" AutoPostBack="true">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <!-- input -->
                                            <!-- input -->
                                            <div class="col">
                                                <label class="selectInputLabel" for="selectInputLabel">Activity Type:</label>
                                                <div class="selectInputBox">
                                                    <asp:DropDownList ID="ddlActivityType" runat="server" CssClass="maximus-select w-100" AutoPostBack="true">
                                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="0">Onboard</asp:ListItem>
                                                        <asp:ListItem Value="1">Activate</asp:ListItem>
                                                        <asp:ListItem Value="2">Deactivate</asp:ListItem>
                                                        <asp:ListItem Value="3">Terminate</asp:ListItem>
                                                        <asp:ListItem Value="4">ReEdit</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <!-- input -->
                                            <div class="col">
                                                <label class="selectInputLabel" for="selectInputLabel">File Id:</label>
                                                <div class="selectInputBox">
                                                    <asp:DropDownList ID="ddlFileID" runat="server" CssClass="maximus-select w-100" AutoPostBack="true">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <!-- input -->
                                        </div>
                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">

                                            <button type="button" id="btnSearch" runat="server" class="themeBtn themeApplyBtn" backcolor="#003087" onserverclick="btnSearch_Click">
                                                Search</button>

                                            <button type="button" id="btnClear" runat="server" class="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal"
                                                onserverclick="btnClear_Click">
                                                Reset</button>
                                            <div class="col">
                                                <asp:ImageButton ID="BtnCsv" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                    ToolTip="Csv" OnClick="BtnCsv_Click" data-toggle="modal" data-target="#myModal" />

                                                <asp:ImageButton ID="BtnXls" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                    ToolTip="Xls" OnClick="BtnXls_Click" data-toggle="modal" data-target="#myModal" />
                                            </div>
                                        </div>
                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                            <center><strong><asp:Label ID="lblRecordsTotal" runat="server" Text=""></asp:Label></strong></center>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <!-- bottom btns -->
                    <%--<asp:Panel ID="panelButtonGroup" runat="server" Width="100%" Visible="true" HorizontalScroll="false" ScrollBars="None" Style="padding: 0px 0px 0px 0px; margin: 10px 0px 10px 0px;">
                        <div class="form-group-sm row" style="margin-left: -21px">
                            <div class="col-sm-12 col-xs-12" style="margin: -21px; margin-left: -10px">
                                <button class="themeBtn themeApplyBtn" id="btnApprove" runat="server" type="button" causesvalidation="false" onserverclick="btnApprove_ServerClick">Approve</button>
                                <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnDecline" runat="server" type="button" onserverclick="btnDecline_ServerClick">Decline</button>
                            </div>
                        </div>
                    </asp:Panel>--%>
                </div>
                <div class="nav-tabs-wrapper">
                    <ul class="nav nav-tabs">
                        <li class="nav-item">
                            <button class="nav-link active" id="docket-tab" data-bs-toggle="tab" data-bs-target="#Manual" type="button" role="tab" aria-controls="docket-tab-pane" aria-selected="true">Manual</button>
                        </li>
                        <li class="nav-item">
                            <button class="nav-link" id="escalation-tab" data-bs-toggle="tab" data-bs-target="#Bulk" type="button" role="tab" aria-controls="escalation-tab-pane" aria-selected="false" tabindex="-1">Bulk</button>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div id="Manual" class="tab-pane fade show active" tabindex="0">
                            <asp:Panel ID="PanelMappingPages" runat="server" Width="100%" Style="overflow: hidden; margin: 0px 0px;">
                                </br>
                                <asp:Panel ID="panelButtonGroup" runat="server" Width="100%" Visible="true" HorizontalScroll="false" ScrollBars="None" Style="padding: 0px 0px 0px 0px; margin: 10px 0px 10px 0px;">
                                    <div class="form-group-sm row" style="margin-left: -21px">
                                        <div class="col-sm-12 col-xs-12" style="margin: -21px; margin-left: -10px">
                                            <button class="themeBtn themeApplyBtn" id="btnApprove" runat="server" type="button" causesvalidation="false" onserverclick="btnApprove_ServerClick">Approve</button>
                                            <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnDecline" runat="server" type="button" onserverclick="btnDecline_ServerClick">Decline</button>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="panelGrid" runat="server" HorizontalScroll="false" ScrollBars="None" Style="padding: 5px 10px 0px 0px;">
                                    <div class="form-group row">
                                        <div class="tableBorderBox HeaderStyle" style="width: 100%; padding: 10px 10px; overflow: scroll; max-height: 400px;">
                                            <asp:GridView ID="gvAgent" runat="server"
                                                AutoGenerateColumns="true"
                                                GridLines="None"
                                                AllowPaging="true"
                                                CssClass="GridView"
                                                PageSize="10"
                                                Visible="true"
                                                PagerSettings-Mode="NumericFirstLast"
                                                PagerSettings-FirstPageText="First Page"
                                                PagerSettings-LastPageText="Last Page"
                                                OnPageIndexChanging="gvAgent_PageIndexChanging">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                <RowStyle Wrap="false" />
                                                <Columns>
                                                    <asp:TemplateField Visible="true">
                                                        <HeaderTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="background: #FBD2CE; color: black; border: none;">
                                                                        <label>All </label>
                                                                    </td>
                                                                    <td style="background: #FBD2CE; color: black; border: none;">
                                                                        <asp:CheckBox ID="CheckBoxAll" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxAll_CheckedChanged" /></td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chBoxSelectRow" runat="server" AutoPostBack="true" OnCheckedChanged="chBoxSelectRow_CheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="background: #FBD2CE; color: black; border: none;">
                                                                        <label>Action </label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%-- CommandArgument='<%#Eval("Agent Code")+","+ Eval("numAgentID")%>'--%>
                                                            <asp:ImageButton ID="btnView" runat="server" ImageUrl="../../images/Edit-01-512.png" Width="20px" Height="20px"
                                                                ToolTip="Click here to verify" OnClick="btnView_Click" CommandArgument='<%#Eval("Agent ID")+"="+Eval("Email")+"="+Eval("Mobile No")%>' data-toggle="modal" data-target="#ModalAgent" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="background: #FBD2CE; color: black; border: none;">
                                                                        <label>Screening Response</label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%-- CommandArgument='<%#Eval("Agent Code")+","+ Eval("numAgentID")%>'--%>
                                                            <asp:ImageButton ID="btnViewResp" runat="server" ImageUrl="../../images/eyeview.png" Width="20px" Height="20px"
                                                                ToolTip="Click here to verify" OnClick="btnViewResp_Click" CommandArgument='<%#Eval("Agent ID")+"="+Eval("Email")+"="+Eval("Mobile No")%>' data-toggle="modal" data-target="#ModalAgent" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="Agent ID" HeaderText="Agent ID" />
                                                    <asp:BoundField DataField="Agent Code" HeaderText="Agent Code" />
                                    <asp:BoundField DataField="BC Code" HeaderText="BC Code" />
                                    <asp:BoundField DataField="Agent Name" HeaderText="Agent Name" />
                                    <asp:BoundField DataField="Client ID" HeaderText="Client ID" />
                                    <asp:BoundField DataField="Client Name" HeaderText="Client Name" />
                                    <asp:BoundField DataField="Mobile No" HeaderText="Mobile No" />
                                    <asp:BoundField DataField="Email" HeaderText="Email Id" />
                                    <asp:BoundField DataField="Pincode" HeaderText="Pincode" />
                                     <asp:BoundField DataField="AgentAddress" HeaderText="Agent Address" />
                                     <asp:BoundField DataField="State" HeaderText="State" />
                                      <asp:BoundField DataField="City" HeaderText="City" />
                                     <asp:BoundField DataField="AadharNo" HeaderText="Aadhar No" />
                                    <asp:BoundField DataField="AgentDOB" HeaderText="Agent DOB" />
                                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                                     <asp:BoundField DataField="LandlineNo" HeaderText="Landline No" />
                                     <asp:BoundField DataField="PopulationGroup" HeaderText="Population Group" />
                                     <asp:BoundField DataField="DeviceCode" HeaderText="Device Code" />
                                    <asp:BoundField DataField="Created By" HeaderText="Created By" />
                                    <asp:BoundField DataField="CreatedOn" HeaderText="Created On" />
                                    <%-- <asp:BoundField DataField="BC Status" HeaderText="BC Status" />--%>
                                    <%--<asp:BoundField DataField="Maker Status" HeaderText="Maker Status" />
                                    <asp:BoundField DataField="Checker Status" HeaderText="Checker Status" />
                                    <asp:BoundField DataField="Authorizer Status" HeaderText="Authorizer Status" />
                                    <asp:BoundField DataField="Activity Type" HeaderText="Activity Type" />
                                     <asp:BoundField DataField="TerminalId" HeaderText="Terminal Id" />--%>

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </asp:Panel>
                        </div>
                        <div id="Bulk" class="tab-pane fade">
                            <asp:Panel ID="Panel2" runat="server" Width="100%" Style="overflow: hidden; margin: 0px 0px;">
                                </br>
                            
                                <asp:Panel ID="panel1" runat="server" HorizontalScroll="false" ScrollBars="None" Style="padding: 5px 10px 0px 0px;">
                                    <div class="form-group row">
                                        <div class="tableBorderBox HeaderStyle" style="width: 100%; padding: 10px 10px; overflow: scroll; max-height: 400px;">
                                            <asp:GridView ID="gvAgBulkL2" runat="server"
                                                AutoGenerateColumns="false"
                                                GridLines="None"
                                                AllowPaging="true"
                                                CssClass="GridView"
                                                PageSize="10"
                                                DataKeyNames="FileID,FileStatus"
                                                Visible="true"
                                                PagerSettings-Mode="NumericFirstLast"
                                                PagerSettings-FirstPageText="First Page"
                                                PagerSettings-LastPageText="Last Page"
                                                OnRowCommand="gvAgBulkL2_RowCommand"
                                                OnPageIndexChanging="gvAgBulkL2_PageIndexChanging">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                <RowStyle Wrap="false" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="background: #FBD2CE; color: black; border: none;">
                                                                        <label>Action </label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>
                                                            <%-- CommandArgument='<%#Eval("Agent Code")+","+ Eval("numAgentID")%>'--%>
                                                            <asp:UpdatePanel ID="UpButtons" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="btnViewBulk" runat="server" ImageUrl="../../images/Edit-01-512.png" Width="20px" Height="20px" CommandName="FileEdit"
                                                                        ToolTip="Click here to verify" OnClick="btnViewBulk_Click" CommandArgument='<%#Eval("FileID")%>' data-toggle="modal" data-target="#ModalAgent" />
                                                                    &nbsp;&nbsp;
                                                                <asp:ImageButton ID="btnViewDownloadPre" runat="server" ImageUrl="../../images/4726040.png" Width="20px" Height="20px"
                                                                    CommandName="DownloadDoc" ToolTip="Download Document" CommandArgument='<%# Eval("FileID") %>' />
                                                                &nbsp;&nbsp;
                                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../../images/zip-file-format.png" Width="20px" Height="20px"
                                                                    CommandName="DownloadZip" ToolTip="Download Document" CommandArgument='<%# Eval("FileID") %>' />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="btnViewDownloadPre" />
                                                                    <asp:PostBackTrigger ControlID="ImageButton1" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FileID" HeaderText="File ID" ItemStyle-HorizontalAlign="Center" SortExpression="FileID" />
                                                    <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName" />
                                                    <asp:BoundField DataField="CreatedBy" HeaderText="Imported By" SortExpression="CreatedBy" />
                                                    <asp:BoundField DataField="CreatedOn" HeaderText="Imported On" SortExpression="CreatedOn" />
                                                    <asp:BoundField DataField="FileStatus" HeaderText="File Status" SortExpression="FileStatus" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <asp:Button ID="btnhidresetDelete" runat="server" Text="Button" Visible="true" Style="visibility: hidden" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender_Declincard" runat="server"
                    TargetControlID="btnhidresetDelete" PopupControlID="Panel_Declincard"
                    PopupDragHandleControlID="PopupHeader_Declincard" Drag="true"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel_Declincard" Style="display: none;" runat="server">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content" style="width: 500px; height: 250px;">
                            <!-- Modal Header -->
                            <div class="modal-header">
                                <h4 class="modal-title">
                                    <asp:Label ID="lblModalHeaderName" runat="server"></asp:Label><span class="err">*</span>
                                </h4>
                            </div>
                            <!-- Modal body -->
                            <div class="modal-body">
                                <asp:Label ID="lblconfirm" runat="server" Font-Bold="true"></asp:Label>
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <asp:TextBox ID="TxtRemarks" runat="server" Style="resize: none; margin-top: 10px;" TextMode="MultiLine" Rows="5" Height="80" Width="430" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator0" ControlToValidate="TxtRemarks" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please enter Remarks"></asp:RequiredFieldValidator>
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
                            <div class="form-group" style="margin-bottom: 0px; margin-top: -30px; height: 85px">
                                <asp:Button ID="btnSaveAction" runat="server" Text="Save" Style="width: 15%" class="themeBtn themeApplyBtn" OnClientClick="disableMultipleClick();" OnClick="btnSaveAction_Click" ValidationGroup="AgentReg" />
                                <asp:ValidationSummary
                                    HeaderText="You must enter or select a value in the following fields:"
                                    DisplayMode="BulletList"
                                    EnableClientScript="true"
                                    CssClass="err"
                                    ShowMessageBox="true"
                                    ShowSummary="false"
                                    ForeColor="Red"
                                    ValidationGroup="AgentReg"
                                    runat="server" />
                                <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnCancelAction" style="width: 15%" type="button" runat="server" causesvalidation="false" onserverclick="btnCancelAction_Click">Cancel</button>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" Visible="true" Style="visibility: hidden" />
                <cc1:ModalPopupExtender ID="ModalPopupResponse" runat="server"
                    TargetControlID="Button1" PopupControlID="PanelResponse"
                    PopupDragHandleControlID="PopupHeader_Declincard" Drag="true"
                    BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="PanelResponse" Style="display: none;" runat="server">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content" style="width: 850px; height: auto;">
                            <!-- Modal Header -->
                            <div class="modal-header">
                                <h4 class="modal-title">
                                    <asp:Label ID="Label1" runat="server" Text="Response"></asp:Label>
                                </h4>
                            </div>
                            <!-- Modal body -->
                            <div class="modal-body">
                                <asp:Label ID="Label2" runat="server" Font-Bold="true"></asp:Label>
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-sm-12">
                                           <div class="tableBorderBox HeaderStyle" style="padding: 10px 10px; overflow: scroll; max-height: 400px;">
                                                <%--<textarea id="txtResponse" runat="server" style="padding: 77px 184px; margin: 5px 5px; font-weight: bold; border: none" aria-multiline="true" spellcheck="false" rows="1" cols="30" readonly="readonly"></textarea>--%>
                                                <%--<textarea id="txtResponse" runat="server" style="font: 20px Arial, sans-serif; padding: 77px 184px; margin: 5px 5px; border: none" aria-multiline="true" spellcheck="false" readonly="readonly"></textarea>--%>

                                                
                                                 <asp:GridView ID="GVPreverification" runat="server"
                                            AutoGenerateColumns="true"
                                            GridLines="None"
                                            AllowPaging="true"
                                            CssClass="GridView"
                                            PageSize="10"
                                            Visible="true"
                                          
                                            PagerSettings-Mode="NumericFirstLast"
                                            PagerSettings-FirstPageText="First Page"
                                            PagerSettings-LastPageText="Last Page"
                                                     OnPageIndexChanging="GVPreverification_PageIndexChanging">
                                            
                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                            <RowStyle Wrap="false" />
                                           
                                        </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Modal footer -->
                            <div class="form-group" style="margin-bottom: 0px; margin-top: -30px; height: 85px">
                                <asp:ValidationSummary
                                    HeaderText="You must enter or select a value in the following fields:"
                                    DisplayMode="BulletList"
                                    EnableClientScript="true"
                                    CssClass="err"
                                    ShowMessageBox="true"
                                    ShowSummary="false"
                                    ForeColor="Red"
                                    ValidationGroup="AgentReg"
                                    runat="server" />
                                <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnClose" style="width: 15%; margin-left: 10px;" type="button" runat="server" causesvalidation="false" onserverclick="btnClose_ServerClick">Close</button>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

            </div>

            <div class="modal fade" id="ModalAgent" data-backdrop="static" aria-hidden="true" aria-labelledby="ModalAgent"
                role="dialog" tabindex="-1">
                <div class="modal-dialog" style="width: 800px; padding-left: 10px; margin-left: 25%; height: 425px">
                    <div class="modal-content" style="width: 725px; padding-left: 11px; height: 476px">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>--%>
                            <h4 class="modal-title" id="exampleModalTitle">Agent Documents</h4>
                        </div>
                        <div class="row" id="divDownloadDocgrid">
                            <div class="col-md-12">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                            <div class="col-md-3 col-xm-12" style="display: none;">
                                                <label for="exampleInputEmail1">Application ID:  </label>
                                                <asp:Label runat="server" class="form-control" ID="lblApplicationID" Visible="false"></asp:Label>
                                            </div>

                                            <div class="col">
                                                <label class="selectInputLabel" for="exampleInputEmail1">Agent Name: </label>
                                                <asp:TextBox ID="txtAgentName" CssClass="form-control" Width="100%" Font-Size="13px" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col">
                                                <label class="selectInputLabel" for="exampleInputEmail1">Contact Number </label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtContactNo" Width="100%" Font-Size="13px" ReadOnly="true"></asp:TextBox>
                                            </div>

                                            <div class="col">
                                                <label class="selectInputLabel" for="exampleInputEmail1">Passport: </label>
                                                <asp:TextBox ID="TxtPassport" CssClass="form-control" Width="100%" Font-Size="13px" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col">
                                                <label class="selectInputLabel" for="exampleInputEmail1">Pan Number: </label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="Txtpanno" Width="100%" Font-Size="13px" ReadOnly="true"></asp:TextBox>
                                            </div>

                                              <div class="col">
                                                <label class="selectInputLabel" for="exampleInputEmail1">IFSC CODE : </label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtIFsccode" Width="100%" Font-Size="13px" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div class="panel-body">
                                    <div class="form-group" style="margin: -32px; height: 100px;">
                                        <center>
                                        <div  class="whos-speaking-area speakers pad100">
                                            <div class="container">
                                                <div class="row">
                                                    <div class="col-lg-12" style="width: 100%; height: 16px;">
                                                        <div class="section-title text-center">
                                                            <div style="margin-top: 25px; margin-bottom: 35px;">
                                                                <h2>Documents</h2>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- /col end-->
                                                </div>
                                                <!-- /.row  end-->
                                                <div class="row mb50" style="margin-left: 154px; height: 75px; margin-top: 42px; padding-bottom: 23px;">
                                                    <!-- /col end-->
                                                    <div class="col-xl-3 col-lg-3 col-md-4 col-sm-12">
                                                        <div class="speakers xs-mb30" style="width: 132px; height: 108px;">
                                                            <%--width: 196px;--%>
                                                            <div class="spk-img">
                                                                <center>
                                                                    <img class="img-fluid" src="<%=pathId%>" alt="trainer-img" style="height: 85px;width: 133px;"  /></center>
                                                                <ul>
                                                                    
                                                                    <li>
                                                                        <asp:ImageButton ID="btnViewDownload" ImageUrl="../../images/download.png" runat="server" Width="36px" Height="36px" Style="margin-left: -41px; margin-top: 9px;" ToolTip="View Doc" OnClick="btnViewDownload_Click" />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px; padding-bottom: 10px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-download"></i></button>--%>

                                                                        <%--<a href="#"><i class="fa fa-download"></i></a>--%>
                                                                    </li>
                                                                    <li>

                                                                        <i><img  src="../../images/eyeview.png" style="width:36px; height:36px; margin-left: -47px; margin-top: -31px; font-size:35px;" onclick="SetValue('IdProof');"></i>
                                                                        <%--<asp:ImageButton ID="EyeImage" ImageUrl="../../images/eyeview.png" runat="server" Width="36px" Height="36px" ToolTip="View Doc" Style="margin-left: -43px; margin-top: 9px;" OnClientClick="SetValue('IdProof');" /> <%--OnClick="EyeImage_Click"--%>
                                                                        <%--<i class="fa fa-eye" id="IDen"  runat="server" style="width:49px; height:36px; margin-left: -43px; margin-top: 9px; font-size:35px;" onclick="SetValue('IdProof');"></i>--%>
                                                                       
                                                                    </li>
                                                                
                                                                </ul>
                                                            </div>
                                                            <div class="spk-info" style="height: 17px;">  <%--margin-top: -7px;--%>
                                                                <h3>Identity Proof</h3>
                                                                <%-- <p>Address Proof</p>--%>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="col-xl-3 col-lg-3 col-md-4 col-sm-12">
                                                        <div class="speakers xs-mb30"  style="width: 132px; height: 108px;">
                                                            <div class="spk-img">
                                                                <center>
                                                                         <img class="img-fluid" src="<%=PathAdd%>" alt="trainer-img"  style="height: 85px;width: 133px;" /></center>
                                                                <ul>
                                                                    <li>
                                                                         <asp:ImageButton ID="ImageButton2"  ImageUrl="../../images/download.png" runat="server" Width="36px" Height="36px" style="margin-left: -41px; margin-top: 9px;" ToolTip="View Doc" OnClick="ImageButton1_Click" CommandArgument='<%#Eval("Agentid") %>' />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px; padding-bottom: 10px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-download"></i></button>--%>
                                                                    </li>
                                                                    <li>
                                                                          <%--<asp:ImageButton ID="EyeImage1" ImageUrl="../../images/View.png" runat="server" Width="49px" Height="36px" ToolTip="View Doc" Style="margin-left: -43px; margin-top: 9px;" OnClick="EyeImage1_Click" />--%>
                                                                        <i><img  src="../../images/eyeview.png" style="width:36px; height:36px; margin-left: -47px; margin-top: -31px; font-size:35px;" onclick="SetValue('AddProof');"></i>
                                                                          <%--<i class="fa fa-eye" id="IDAdd"  runat="server" style="width:49px; height:36px; margin-left: -43px; margin-top: 9px; font-size:35px;" onclick="SetValue('AddProof');"></i>--%>

                                                                    </li>
                                                                </ul>
                                                            </div>
                                                            <div class="spk-info" style="height: 17px;">
                                                                <h3>Address Proof</h3>
                                                                <%--<p>Address Proof</p>--%>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="col-xl-3 col-lg-3 col-md-4 col-sm-12">
                                                        <div class="speakers xs-mb30"  style="width: 132px; height: 108px;">
                                                            <div class="spk-img">
                                                                <center>
                                                                  
                                                                          <img class="img-fluid" src="<%=PathSig%>" alt="trainer-img" style="height: 85px;width: 133px;"  /></center>
                                                                <ul>
                                                                    <li>
                                                                         <asp:ImageButton ID="ImageButton4"  ImageUrl="../../images/download.png" runat="server" Width="36px" Height="36px" style="margin-left: -41px; margin-top: 9px;" ToolTip="View Doc" OnClick="imgbtnform_Click" CommandArgument='<%#Eval("Agentid") %>' />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px; padding-bottom: 10px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-download"></i></button>--%>
                                                                        <%--  <a href="#"><i class="fa fa-download"></i></a>--%>
                                                                    </li>
                                                                    <li>
                                                                        <%--<asp:ImageButton ID="EyeImage3" ImageUrl="../../images/View.png" runat="server" Width="49px" Height="36px" ToolTip="View Doc" Style="margin-left: -43px; margin-top: 9px;" OnClick="EyeImage3_Click" CommandArgument='<%#Eval("Agentid") %>' />--%>
                                                                        <%--<i class="fa fa-eye" id="IDSig"  runat="server" style="width:49px; height:36px; margin-left: -43px; margin-top: 9px; font-size:35px;" onclick="SetValue('SigProof');"></i>--%>
                                                                        <i><img  src="../../images/eyeview.png" style="width:36px; height:36px; margin-left: -47px; margin-top: -31px; font-size:35px;" onclick="SetValue('SigProof');"></i>
                                                                       
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                            <div class="spk-info" style="height: 17px;">
                                                                <h3>Signature Proof</h3>
                                                                <%--<p>Address Proof</p>--%>
                                                            </div>
                                                        </div>

                                                    </div>

                                                </div>
                                                <!-- /row end-->

                                            </div>
                                            <!-- /container end-->
                                        </div>
                                        </center>
                                    </div>
                                </div>

                                <div class="panel-body" style="margin-top: 50px; padding-top: 70px;">
                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-md-3 col-xm-12">
                                            </div>
                                            <div class="col-md-3 col-xm-12" style="margin-bottom: 2%;">
                                                <label class="selectInputLabel" for="exampleInputEmail1">Action: </label>
                                                <asp:RadioButtonList ID="rdbtnApproveDecline" CssClass="rbl" runat="server" RepeatLayout="Table">
                                                    <asp:ListItem Value="Approve">Approve   &nbsp;&nbsp;&nbsp;&nbsp; </asp:ListItem>
                                                    <asp:ListItem Value="Decline">Decline</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="None" CssClass="err" ErrorMessage="Please select status" ControlToValidate="rdbtnApproveDecline" ValidationGroup="Veri" runat="server"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-3 col-xm-12">
                                                <label class="selectInputLabel" for="exampleInputEmail1">Remarks: </label>
                                                </br>
                                                <asp:TextBox runat="server" class="form-control" Style="resize: none; height: 65px; width: 288px;" ID="txtFinalRemarks" MaxLength="200"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqName" Display="None" CssClass="err" ErrorMessage="Please enter Remarks" ControlToValidate="txtFinalRemarks" ValidationGroup="Veri" runat="server"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <hr class="hr-line">
                        <div>
                            <div class="row">
                                <div class="col-md-3 offset-5 col-xm-12" style="margin-left: 235px">
                                    <asp:Button runat="server" ID="btnSubmitDetails" Style="margin-left: auto" ValidationGroup="Veri" OnClick="btnSubmitDetails_Click" class="themeBtn themeApplyBtn" Text="Submit" OnClientClick="Confirm();"></asp:Button>
                                </div>
                                <div class="col-md-3 col-xm-12">
                                    <button type="button" class="themeBtn resetBtn themeCancelBtn me-0" onclick="ClearRemark()">Close</button>
                                </div>
                            </div>
                            <asp:ValidationSummary
                                HeaderText="You must enter or select a value in the following fields:"
                                DisplayMode="BulletList"
                                EnableClientScript="true"
                                CssClass="err"
                                ShowMessageBox="true"
                                ShowSummary="false"
                                ForeColor="Red"
                                ValidationGroup="Veri"
                                runat="server" />
                        </div>

                    </div>
                </div>
            </div>
            <asp:Button ID="btnRoleEdit" runat="server" Text="Button" Style="display: none;" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender_EditRole" runat="server"
                TargetControlID="btnRoleEdit" PopupControlID="Panel_EditRole"
                PopupDragHandleControlID="PopupHeader_EditRole" Drag="true"
                BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>

            <asp:Panel ID="Panel_EditRole" Style="display: none;" runat="server">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content" style="width: 500px;">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Enter Reason </h4>
                        </div>
                        <!-- Modal body -->
                        <div class="modal-body">
                            <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Are you sure to Approve/Decline the File : ">
                                <asp:Label ID="lblName" runat="server" Font-Bold="true"></asp:Label>
                                <asp:Label ID="Label15" Text="" runat="server" Font-Bold="true"></asp:Label>
                            </asp:Label>
                            <div class="form-group">
                                <asp:Label ID="lblID" runat="server" Font-Bold="true" Visible="false"></asp:Label>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:TextBox ID="txtResone" runat="server" TextMode="multiline" Rows="5" Placeholder="Please enter reason" Height="80" Width="430" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="None" CssClass="err" ErrorMessage="Please enter Remarks" ControlToValidate="txtResone" ValidationGroup="Verif" runat="server"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <%--<button class="themeBtn themeApplyBtn" id="BulkApprove" runat="server" style="width: 15%" onserverclick="BulkApprove_ServerClick">
                                    Approve
                                </button>
                                &nbsp;&nbsp;
                            <button class="themeBtn themeApplyBtn" id="BulkDecline" runat="server" style="width: 15%" onserverclick="BulkDecline_ServerClick">
                                Decline
                            </button>
                                &nbsp;&nbsp;
                            <button class="themeBtn resetBtn themeCancelBtn me-0" id="bulkClose" runat="server" style="width: 15%" onserverclick="bulkClose_ServerClick">
                                Close
                            </button>--%>
                                <button class="themeBtn themeApplyBtn" id="BulkApprove" runat="server" validationgroup="Verif" type="button" style="width: 15%" onserverclick="BulkApprove_ServerClick">Approve</button>
                                <button class="themeBtn resetBtn themeCancelBtn me-0" id="BulkDecline" validationgroup="Verif" runat="server" style="width: 15%" type="button" onserverclick="BulkDecline_ServerClick">Decline</button>
                                <button class="themeBtn resetBtn themeCancelBtn me-0" id="bulkClose" runat="server"  style="width: 15%" type="button" onserverclick="bulkClose_ServerClick">Close</button>
                            </div>
                             <asp:ValidationSummary
                                HeaderText="You must enter or select a value in the following fields:"
                                DisplayMode="BulletList"
                                EnableClientScript="true"
                                CssClass="err"
                                ShowMessageBox="true"
                                ShowSummary="false"
                                ForeColor="Red"
                                ValidationGroup="Verif"
                                runat="server" />
                        </div>
                        <!-- Modal footer -->

                    </div>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="hid1" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnViewDownload" />
            <%--<asp:PostBackTrigger ControlID="EyeImage" />--%>
            <%--<asp:AsyncPostBackTrigger ControlID="EyeImage" EventName="click" />--%>
            <%--   <asp:PostBackTrigger ControlID="EyeImage3" />
            <asp:PostBackTrigger ControlID="EyeImage1" />--%>
            <asp:PostBackTrigger ControlID="ImageButton2" />
            <asp:PostBackTrigger ControlID="ImageButton4" />
            <asp:PostBackTrigger ControlID="btnSubmitDetails" />
            <asp:PostBackTrigger ControlID="BtnCsv" />
            <asp:PostBackTrigger ControlID="BtnXls" />
        </Triggers>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" BackgroundCssClass="modalBackground" DropShadow="false" />
    <script type="text/javascript">
        function SetValue(myval) {
            //var surl = "http://localhost:63840/pdfhandler.aspx?value="+myval+"";
            window.open("/pdfhandler.aspx?value=" + myval + "");
        }
    </script>

    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlBCCode.ClientID%>").select2();
                    $("#<%=ddlAgentCode.ClientID%>").select2();
                    $("#<%=ddlActivityType.ClientID%>").select2();
                    $("#<%=ddlFileID.ClientID%>").select2();
                }
            });
        };
    </script>
</asp:Content>
