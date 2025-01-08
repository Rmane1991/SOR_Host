<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="MATMTransactions.aspx.cs" Inherits="SOR.Pages.TransactionReport.MATMTransactions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../Content/css/common.css" rel="stylesheet" />
    <style>
        #ctl00_cpbdCarde_gvReversalTransaction {
            width: 100% !Important;
        }

        table {
            width: 0% !Important;
        }

        .form-control {
            height: 33px;
        }

        .select2 {
            width: 100% !important;
        }
    </style>

    <style type="text/css">
        .InvisibleCol {
            display: none;
        }

        .adjustMarginTB {
            margin: 20px 0px 10px 0px;
        }



        .form-group {
            margin-bottom: 5px;
        }

        .ajax__calendar iframe {
            left: 0px !important;
            top: 0px !important;
        }

        .ajax__calendar_body {
            background-color: #ffffff;
            border: solid 1px black;
            height: 130px;
            width: 160px;
        }
    </style>

    <style type="text/css">
        .InvisibleCol {
            display: none;
        }

        .adjustMarginTB {
            margin: 10px 0px 0px 0px;
        }

        #ctl00_cpbdCarde_PnlAllot fieldset {
            height: 76px;
        }

        #ctl00_cpbdCarde_tdFollowup fieldset {
            width: 120%;
        }

        #ctl00_cpbdCarde_pnlFilter {
            z-index: 9999;
            overflow: hidden !important;
        }

        #ctl00_cpbdCarde_pnlTicketSummary {
            width: 100%;
        }

        .PanelTableStyle {
            padding: 0px 5px 0px 0px;
            margin: 0px 5px 0px 0px;
        }

        .PanelTRStyle {
            padding: 0px 5px 0px 0px;
            margin: 0px 0px 0px 0px;
        }

        .PanelTDStyle {
            font-size: 11px;
        }

        .OuterTable {
            width: 99%;
        }

        .OuterTD {
            width: 10%;
            margin-right: 5px;
        }

        .InnerTable {
            width: 100%;
        }

        .InnerTD {
            width: auto;
            padding: 0px 5px 0px 0px;
            margin: 0px 0px 0px 0px;
            font-size: small;
            font-weight: bold;
            color: #000066;
        }

        .Freezing {
            text-align: center;
        }
    </style>

    <style>
        #ctl00_cpbdCarde_gvsummary {
            width: 100% !Important;
        }

        /*table {
            width: 100% !Important;
        }*/

        td, th {
            padding: 0px 5px;
        }

        .select2 {
            width: 100% !important;
        }

        .flex {
            width: 100% !important;
        }

        fieldset.scheduler-border {
            border: solid 1px black !important;
            padding: 0 0px 0px 5px;
            font-size: small;
            border-bottom: none;
            overflow-y: hidden;
            overflow-x: hidden;
        }

        legend.scheduler-border {
            width: auto !important;
            border: none;
            font-size: small;
            overflow-y: hidden;
            overflow-x: hidden;
        }
    </style>

    <style>
        #ctl00_cpbdCarde_gvTransactions {
            width: 100% !Important;
        }
    </style>

    <style type="text/css">
        table {
            border-color: #898989 !important;
        }

        .table > thead > tr > th {
            border: 1px solid #b9b6b6 !important;
            vertical-align: middle;
            height: 30px;
            color: #2f4761;
        }

        .HeaderStyle table tbody tr th {
            background-color: #fbd2ce !important;
            font-weight: 400;
            position: sticky;
            white-space: nowrap;
            width: auto;
            z-index: 100;
            top: -8px;
            text-align: center;
        }

        .gv-responsive {
            width: 100%;
            height: auto;
            white-space: nowrap;
            padding-top: 7px;
            overflow: auto !important;
        }

        .gv-responsiveNew {
            width: 100%;
            white-space: nowrap;
            padding-top: 7px;
            overflow: auto !important;
        }

        .gv-responsiveNew {
            width: 100%;
            padding-top: 7px;
        }

        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

        .legend {
            margin-bottom: 0px;
        }


        .col-md-2 {
            flex: 0 0 auto;
            width: 12.666667%;
        }

        .element.style {
            height: 33px;
            width: 133px;
        }
            .center img {
                height: 128px;
                width: 128px;
            }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">

    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:UpdateProgress ID="upContentBodyUpdateProgress" runat="server" AssociatedUpdatePanelID="upContentBody">
            <ProgressTemplate>
                <div style="width: 100%; height: 100%; opacity: 0.8; background-color: black; position: fixed; top: 0; left: 0">
                    <img alt="" id="progressImage1" style="margin-top: 20%" src='<%=Page.ResolveClientUrl("../../Images/loading2_1.gif") %>' />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>

    <asp:UpdatePanel ID="upContentBody" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <%--   <div class="breadcrumb"><i class="lnr lnr-home"></i>/ TRANSACTION REPORT/ MATM TRANSACTION</div>--%>
                <div class="breadHeader">
                    <h5 class="page-title">MATM TRANSACTION</h5>
                </div>
                <%--<asp:Panel ID="pnlFilter" runat="server" Width="116%" Height="150%" HorizontalScroll="false" ScrollBars="None" Style=" padding: 5px 10px 53px 10px; overflow: hidden;">--%>
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
                                <hr class="hr-line">
                                <!-- grid -->

                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                    <div class="Col" style="width: 20%">
                                        <label class="selectInputLabel" for="selectInputLabel">Channel Type</label>
                                        <asp:DropDownList ID="ddlChannelType" runat="server" CssClass="maximus-select w-100" Style="width: 100%" AutoPostBack="false">
                                            <%--<asp:ListItem Text="-- Channels ---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="AEPS" Value="1"></asp:ListItem>--%>
                                            <asp:ListItem Text="MATM" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col">
                                        <label class="selectInputLabel" for="selectInputLabel">From Date</label>
                                        <div class="selectInputDateBox w-100">
                                            <input type="date" runat="server" id="txtFromDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkDate()" />
                                        </div>
                                    </div>
                                    <div class="col ">
                                        <label class="selectInputLabel" for="selectInputLabel">To Date</label>
                                        <div class="selectInputDateBox w-100">
                                            <input type="date" runat="server" id="txtToDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkToDate()" />
                                        </div>
                                    </div>

                                    <div class="Col" style="width: 20%">
                                        <label class="selectInputLabel" for="selectInputLabel">Txn Type</label>
                                        <asp:DropDownList ID="ddlTranType" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Withdrawal" Value="010000"></asp:ListItem>
                                            <asp:ListItem Text="Balance Enquiry" Value="310000"></asp:ListItem>
                                            <asp:ListItem Text="Mini Statement" Value="900000"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="Col" style="width: 20%">
                                        <label class="selectInputLabel" for="selectInputLabel">Txn Status</label>
                                        <asp:DropDownList ID="ddlTransactionStatus" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Success" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Failed" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Reversal" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col">
                                        <label class="selectInputLabel" for="selectInputLabel">BC</label>
                                        <div class="selectInputBox">
                                            <asp:DropDownList runat="server" ID="ddlBCCode" CssClass="maximus-select w-100" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <label class="selectInputLabel" for="selectInputLabel">Agent Code/Terminal ID</label>
                                        <div class="inputBox w-100">
                                            <input type="text" id="txtAgentCode" runat="server" class="input-text form-control" style="width: 100%" placeholder="Agent Code/Terminal ID" maxlength="10" />
                                        </div>
                                    </div>
                                    <div class="col">
                                        <label class="selectInputLabel" for="selectInputLabel">RRN</label>
                                        <div class="inputBox w-100">
                                            <input type="text" id="txtRRNo" runat="server" class="input-text form-control" style="width: 100%" placeholder="RRN" maxlength="12" />
                                        </div>
                                    </div>

                                    <div class="col-sm-6 col-xs-4 cssRecordsTotal" style="margin-left: 250px;">
                                        <center><strong><asp:Label ID="lblRecordCount" CssClass="form-contol" runat="server" Text=""></asp:Label></strong></center>
                                    </div>
                                </div>

                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="BtnCsv" />
                                        <asp:PostBackTrigger ControlID="BtnXls" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div id="DisableButtton" class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns" runat="server">

                                            <button type="button" id="btnSearch" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnSearch_ServerClick">
                                                Search</button>
                                            <button type="button" id="btnClear" runat="server" style="margin-top: -7px" onserverclick="btnClear_ServerClick" class="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal">
                                                Clear</button>

                                            <div style="margin-top: unset">
                                                <asp:ImageButton ID="BtnCsv" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                    ToolTip="Csv" OnClick="BtnCsv_Click" data-toggle="modal" data-target="#myModal" />

                                                <asp:ImageButton ID="BtnXls" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                    ToolTip="Xls" OnClick="BtnXls_Click" data-toggle="modal" data-target="#myModal" Visible="false" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="panelGrid" runat="server" Width="100%" HorizontalScroll="false" ScrollBars="None" Style="margin: 5px 10px 0px 0px; padding-right: 0px; overflow: auto">
                    <div class="HeaderStyle gv-responsive">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <div class="HeaderStyle gv-
                                 ">
                                    <asp:GridView ID="gvMATMTransaction" runat="server"
                                        HeaderStyle-CssClass="text-center"
                                        CssClass="GridView"
                                        HeaderStyle-Font-Names="Arial Narrow"
                                        HeaderStyle-Font-Size="12px"
                                        RowStyle-Font-Size="12px"
                                        HeaderStyle-Wrap="false"
                                        RowStyle-Wrap="false"
                                        CellSpacing="1"
                                        CellPadding="-1"
                                        AutoGenerateColumns="true"
                                        AllowCustomPaging="true"
                                        Font-Size="Small"
                                        GridLines="Vertical"
                                        AllowPaging="true"
                                        PagerSettings-Mode="NumericFirstLast"
                                        PagerSettings-FirstPageText="First Page"
                                        PagerSettings-LastPageText="Last Page"
                                        AllowSorting="True"
                                        PageSize="500"
                                        OnSorting="gvMATMTransaction_Sorting"
                                        Visible="true"
                                        OnPageIndexChanging="gvMATMTransaction_PageIndexChanging">
                                        <RowStyle ForeColor="Black" Height="22px" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" Wrap="false"></RowStyle>
                                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="Black" Height="25px" Wrap="false"></HeaderStyle>
                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="Black"></SelectedRowStyle>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <Columns>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
                <!-- END BORDERED TABLE -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" DropShadow="false" />

    <script>

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlChannelType.ClientID%>").select2();
                    $("#<%=ddlTranType.ClientID%>").select2();
                    $("#<%=ddlTransactionStatus.ClientID%>").select2();
                    $("#<%=ddlBCCode.ClientID%>").select2();

                }
            });
        };
    </script>
    <script type="text/javascript">  
        function checkDate() {
            var date = document.getElementById("<%= txtFromDate.ClientID %>").value;
            var todayDate = new Date().toISOString().slice(0, 10);
            if (date > todayDate) {
                alert("Selected date can not be greater than Current date !!");
                document.getElementById("<%= txtFromDate.ClientID %>").value = todayDate;
            }
            else
                document.getElementById("datePicker").value = todayDate;
        }
    </script>
    <script type="text/javascript">  
        function checkToDate() {
            var date = document.getElementById("<%= txtToDate.ClientID %>").value;
            var todayDate = new Date().toISOString().slice(0, 10);
            if (date > todayDate) {
                alert("Selected date can not be greater than Current date !!");
                document.getElementById("<%= txtToDate.ClientID %>").value = todayDate;
            }
            else
                document.getElementById("datePicker").value = todayDate;
        }
    </script>
</asp:Content>
