<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="BatchConfiguration.aspx.cs" Inherits="SOR.Pages.Rule.BatchConfiguration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
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
            display: block;
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

        .col-md-2 {
            flex: 0 0 auto;
            width: 10.666667%;
        }
    </style>

    <style type="text/css">
        .input-group-addon {
            border: 1px solid;
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
    </style>
    <style>
        .row {
            margin-bottom: 10px; /* Space between rows */
        }

        .removeBtn {
            width: 100%; /* Make button full width */
        }
    </style>
    <%--<script>
        function addRow() {
            var formContainer = document.getElementById('<%= formContainer.ClientID %>');
            var newRow = document.createElement('div');
            newRow.className = 'row';
            newRow.innerHTML = `
                <div class="col-md-3">
                    <input type="text" name="name" placeholder="Name" class="form-control" />
                </div>
                <div class="col-md-3">
                    <input type="text" name="percentage" placeholder="Percentage" class="form-control" />
                </div>
                <div class="col-md-3">
                    <input type="text" name="count" placeholder="Count" class="form-control" />
                </div>
                <div class="col-md-3">
                    <button type="button" class="themeBtn resetBtn themeCancelBtn me-0" onclick="removeRow(this)">Remove</button>
                </div>
            `;
            formContainer.appendChild(newRow);
        }

        function removeRow(button) {
            var row = button.closest('.row'); // Get the closest row element
            row.parentNode.removeChild(row); // Remove the row
        }
    </script>--%>
    <script>
        <%--let rowCount = 0; // Keep track of how many rows have been added

function addRow() {
    var formContainer = document.getElementById('<%= formContainer.ClientID %>');
    var newRow = document.createElement('div');
    newRow.className = 'row';
    newRow.style.display = 'flex'; // Ensure it shows as a flex row
    newRow.innerHTML = `
        <div class="col-md-3">
            <input type="text" name="name${rowCount}" placeholder="Name" class="form-control" />
        </div>
        <div class="col-md-3">
            <input type="text" name="percentage${rowCount}" placeholder="Percentage" class="form-control" />
        </div>
        <div class="col-md-3">
            <input type="text" name="count${rowCount}" placeholder="Count" class="form-control" />
        </div>
        <div class="col-md-3">
            <button type="button" class="themeBtn resetBtn themeCancelBtn me-0" onclick="removeRow(this)">Remove</button>
        </div>
    `;
    formContainer.appendChild(newRow);
    rowCount++; // Increment the row count for unique naming
}

function removeRow(button) {
    var row = button.closest('.row'); // Get the closest row element
    row.parentNode.removeChild(row); // Remove the row
}--%>
        var rowCount = 0; // Initialize row count globally

        function addRow() {
            var formContainer = document.getElementById('<%= formContainer.ClientID %>');
            var newRow = document.createElement('div');
            newRow.className = 'row';
            newRow.style.display = 'flex'; // Ensure it shows as a flex row
            newRow.innerHTML = `
        <div class="col-md-3">
            <input type="text" name="name${rowCount}" placeholder="Name" class="form-control" />
        </div>
        <div class="col-md-3">
            <input type="text" name="percentage${rowCount}" placeholder="Percentage" class="form-control" />
        </div>
        <div class="col-md-3">
            <input type="text" name="count${rowCount}" placeholder="Count" class="form-control" />
        </div>
        <div class="col-md-3">
            <button type="button" class="themeBtn resetBtn themeCancelBtn me-0" onclick="removeRow(this)">Remove</button>
        </div>
    `;
            formContainer.appendChild(newRow);
            rowCount++; // Increment the row count for unique naming
        }

        function removeRow(button) {
            var row = button.closest('.row'); // Get the closest row element
            row.parentNode.removeChild(row); // Remove the row
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
            <asp:HiddenField ID="TabName" runat="server" />
            <div class="container-fluid">
                <div class="breadHeader">
                    <h5 class="page-title">Batch Configuration</h5>
                </div>
                <!-- Filter Accordion -->
                <div id="formone" runat="server">
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
                                    <div>
                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                            <div class="col col-all-filter">
                                                <label class="selectInputLabel" for="selectInputLabel">From Date</label>
                                                <div class="selectInputDateBox w-100">
                                                    <input type="date" runat="server" id="txtFrom" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkDate()" />
                                                </div>
                                            </div>
                                            <div class="col col-all-filter">
                                                <label class="selectInputLabel" for="selectInputLabel">To Date</label>
                                                <div class="selectInputDateBox w-100">
                                                    <input type="date" runat="server" id="txtTo" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkToDate()" />
                                                </div>
                                            </div>
                                            <button type="button" id="btnSearch" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnSearch_ServerClick">
                                                Search</button>
                                            <button type="button" id="btnClear" runat="server" onserverclick="btnClear_ServerClick" class="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal">
                                                Clear</button>
                                            <button type="button" id="btnAdd" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnAdd_ServerClick">
                                                Add New</button>
                                            <div class="col">
                                               
                                            </div>
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

                    <asp:Panel ID="panel1" runat="server" HorizontalScroll="false" ScrollBars="None" Style="padding: 5px 10px 0px 0px;">
                        <div class="form-group row">
                            <div class="tableBorderBox HeaderStyle" style="width: 100%; padding: 10px 10px; overflow: scroll; max-height: 400px;">
                                <asp:GridView ID="gvBankConfiguration" runat="server"
                                    AutoGenerateColumns="true"
                                    GridLines="None"
                                    AllowPaging="true"
                                    CssClass="GridView"
                                    PageSize="10"
                                    Visible="true"
                                    OnPageIndexChanging="gvBankConfiguration_PageIndexChanging"
                                    PagerSettings-Mode="NumericFirstLast"
                                    PagerSettings-FirstPageText="First Page"
                                    PagerSettings-LastPageText="Last Page">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                    <RowStyle Wrap="false" />
                                    <Columns>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div id="formTwo" runat="server" visible="false">
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
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                        <%--<div class="row mb-3">
                                            <label for="lblSwitch" class="col-md-2 col-form-label">Switch</label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlSwitch" runat="server" CssClass="maximus-select w-100" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>--%>
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Switch</label>
                                            <div class="selectInputBox">
                                                <asp:DropDownList runat="server" ID="ddlSwitch" CssClass="maximus-select w-100">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                        <%--<div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Batch Name</label>
                                            <div class="inputBox w-100">
                                                <input type="text" id="txtBatchName" runat="server" class="input-text form-control" style="width: 100%" placeholder="Enter Batch Name"/>
                                            </div>
                                        </div>
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Batch Percentage</label>
                                            <div class="inputBox w-100">
                                                <input type="text" id="txtBatchPercentage" runat="server" class="input-text form-control" style="width: 100%" placeholder="Enter Batch Percentage"/>
                                            </div>
                                        </div>
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Batch Count</label>
                                            <div class="inputBox w-100">
                                                <input type="text" id="txtBatchCount" runat="server" class="input-text form-control" style="width: 100%" placeholder="Enter Batch Count"/>
                                            </div>
                                        </div>--%>
                                    </div>
                                    <div id="formContainer" runat="server">
                                        <div class="row" style="display: none;">
                                            <!-- Initially hidden -->
                                            <div class="col-md-3">
                                                <input type="text" name="name" placeholder="Name" class="form-control" />
                                            </div>
                                            <div class="col-md-3">
                                                <input type="text" name="percentage" placeholder="Percentage" class="form-control" />
                                            </div>
                                            <div class="col-md-3">
                                                <input type="text" name="count" placeholder="Count" class="form-control" />
                                            </div>
                                            <div class="col-md-3">
                                                <button type="button" class="themeBtn resetBtn themeCancelBtn me-0" onclick="removeRow(this)">Remove</button>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns" style="padding-right: 60px">
                                        <button type="button" id="btnSubmit" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnSubmit_ServerClick">
                                            Submit
   
                                        </button>
                                        <button type="button" id="btnReset" runat="server" onserverclick="btnReset_ServerClick" class="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal">
                                            Close
   
                                        </button>
                                        <button type="button" class="themeBtn themeApplyBtn" onclick="addRow()">Add Batch</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnCsv" />
            <asp:PostBackTrigger ControlID="BtnXls" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlSwitch.ClientID%>").select2();
                }
            });
        };
    </script>

</asp:Content>
