<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="AllTransactions.aspx.cs" Inherits="SOR.Pages.TransactionReport.AllTransactions" %>

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
            border: double 3px #7CB9E8 !important;
            border-radius: 10px;
            padding-top: 10px;
            padding-bottom: 10px;
            padding-right: 2px;
            font-size: small;
            border-bottom: none;
            overflow-y: hidden;
            overflow-x: hidden;
            display: inline-block;
        }



        legend.scheduler-border {
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
    </style>

    <script type="text/javascript">
        function ExpandCollapse(obj) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div) {
                if (div.style.display == "none") {
                    div.style.display = "block";
                    img.src = "../Images/minus.png";

                }
                else {
                    div.style.display = "none";
                    img.src = "../Images/plus.png";
                }
            }
        }
        function GoToSurvey(TID) {
            window.open("../Survey/Survey.aspx?TID=" + TID);
        }
    </script>



    <script type="text/javascript">
        function isokmaxlength(e, val, maxlengt) {
            var charCode = (typeof e.which == "number") ? e.which : e.keyCode
            if (!(charCode == 44 || charCode == 46 || charCode == 0 || charCode == 8 || (val.value.length < maxlengt))) {
                return false;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <asp:HiddenField ID="hdFromDate" runat="server" />
    <asp:HiddenField ID="hdToDate" runat="server" />

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
            <div style="clear: both;" />
            <asp:HiddenField ID="hidCardNo" runat="server" />
            <asp:HiddenField ID="hidRRN" runat="server" />
            <asp:HiddenField ID="hidTxn" runat="server" />
            <div class="container-fluid">
                <div class="breadHeader">
                    <h5 class="page-title">All Transactions</h5>
                </div>


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

                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Channel</label>
                                            <asp:DropDownList ID="ddlChannelType" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="AEPS" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="MATM" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col col-all-filter">
                                            <label class="selectInputLabel" for="selectInputLabel">From Date</label>
                                            <div class="selectInputDateBox w-100">
                                                <input type="date" runat="server" id="txtFromDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkDate()" />
                                            </div>
                                        </div>
                                        <div class="col col-all-filter">
                                            <label class="selectInputLabel" for="selectInputLabel">To Date</label>
                                            <div class="selectInputDateBox w-100">
                                                <input type="date" runat="server" id="txtToDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkToDate()" />
                                            </div>
                                        </div>
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Txn Type</label>
                                            <asp:DropDownList ID="ddlTranType" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Withdrawal" Value="010000"></asp:ListItem>
                                                <asp:ListItem Text="Balance Enquiry" Value="310000"></asp:ListItem>
                                                <asp:ListItem Text="MiniStatement" Value="900000"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Txn Status</label>
                                            <asp:DropDownList ID="ddlTransactionStatus" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Success" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Failed" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Reversal" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Aggregator</label>
                                            <div class="selectInputBox">
                                                <asp:DropDownList runat="server" ID="ddlAggregator" CssClass="maximus-select w-100" AutoPostBack="true">
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
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Action</label>
                                            <asp:DropDownList ID="ddlAction" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnexport" />
                                    </Triggers>
                                    <ContentTemplate>

                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">

                                            <button type="button" id="btnSearch" runat="server" class="themeBtn themeApplyBtn" onserverclick="buttonSearch_Click" visible="false">
                                                Search</button>
                                            <button type="button" id="btnClear" runat="server" style="margin-top: -6px" onserverclick="btnReset_ServerClick" class="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal" visible="false">
                                                Clear</button>

                                            <button type="button" id="btnexport" runat="server" style="margin-top: -7px" class="themeBtn themeApplyBtn" onserverclick="btnexport_ServerClick" visible="false">
                                                Export</button>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                </br>

                <asp:Panel ID="pnlTicketSummary" runat="server" Style="width: 75%; padding: 06px 0px; overflow: auto; margin: 0 auto;">
                    <table class="OuterTable">
                        <tr>
                            <td class="OuterTD">
                                <asp:Panel ID="Panel2" runat="server" HorizontalAlign="Center" Style="margin: 0px 5px 0px 0px;">
                                    <fieldset class="scheduler-border" style="margin-left: 30px">
                                        <legend class="scheduler-border">Total Transactions</legend>
                                        <table class="InnerTable">
                                            <tr>
                                                <td class="InnerTD" align="center"><span class="PanelTDStyle">Total</span></td>
                                                <td class="InnerTD" align="center"><span class="PanelTDStyle">Success</span></td>
                                                <td class="InnerTD" align="center"><span class="PanelTDStyle">Failed</span></td>
                                                <td class="InnerTD"><span class="PanelTDStyle">Reversal</span></td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:LinkButton ID="TxnCountTotal" runat="server" Text="0" ForeColor="#ff0000" Width="70px" Font-Bold="true" CommandArgument="Total" CommandName="0"></asp:LinkButton></td>
                                                <td align="center">
                                                    <asp:LinkButton ID="TxnCountApproved" runat="server" Text="0" ForeColor="#005d7e" Width="70px" Font-Bold="true" CommandArgument="Success" CommandName="0"></asp:LinkButton></td>
                                                <td align="center">
                                                    <asp:LinkButton ID="TxnCountFailed" runat="server" Text="0" ForeColor="#a2460e" Width="70px" Font-Bold="true" CommandArgument="Failed" CommandName="0"></asp:LinkButton></td>
                                                <td align="center">
                                                    <asp:LinkButton ID="TxnCountReversal" runat="server" Text="0" CommandArgument="Timeout" Width="70px" CommandName="0"></asp:LinkButton></td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </asp:Panel>
                            </td>

                            <td class="OuterTD">
                                <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" Style="margin: 0px 5px 0px 0px;">
                                    <fieldset class="scheduler-border">
                                        <legend class="scheduler-border">AEPS Transactions </legend>
                                        <table class="InnerTable">
                                            <tr>
                                                <td class="InnerTD"><span class="PanelTDStyle">Total</span></td>
                                                <td class="InnerTD"><span class="PanelTDStyle">Success</span></td>
                                                <td class="InnerTD"><span class="PanelTDStyle">Failed</span></td>
                                                <td class="InnerTD"><span class="PanelTDStyle">Reversal</span></td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:LinkButton ID="TxnAmountTotal" runat="server" Text="0" Width="70px" ForeColor="#ff0000" Font-Bold="true" CommandArgument="Total" CommandName="1"></asp:LinkButton></td>
                                                <td align="center">
                                                    <asp:LinkButton ID="TxnAmountApproved" runat="server" Text="0" Width="70px" ForeColor="#005d7e" Font-Bold="true" CommandArgument="Success" CommandName="1"></asp:LinkButton></td>
                                                <td align="center">
                                                    <asp:LinkButton ID="TxnAmountFailed" runat="server" Text="0" Width="70px" ForeColor="#a2460e" Font-Bold="true" CommandArgument="Failed" CommandName="1"></asp:LinkButton></td>
                                                <td align="center">
                                                    <asp:LinkButton ID="TxnAmountReversal" runat="server" Text="0" Width="70px" CommandArgument="Timeout" CommandName="1"></asp:LinkButton></td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </asp:Panel>
                            </td>

                            <td class="OuterTD">
                                <asp:Panel ID="Panel3" runat="server" HorizontalAlign="Center" Style="margin: 0px 5px 0px 0px;">
                                    <fieldset class="scheduler-border" style="margin-left: -6px">
                                        <legend class="scheduler-border">MATM Transactions </legend>
                                        <table class="InnerTable">
                                            <tr>
                                                <td class="InnerTD"><span class="PanelTDStyle">Total</span></td>
                                                <td class="InnerTD"><span class="PanelTDStyle">Success</span></td>
                                                <td class="InnerTD"><span class="PanelTDStyle">Failed</span></td>
                                                <td class="InnerTD"><span class="PanelTDStyle">Reversal</span></td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:LinkButton ID="NFinTxnTotal" runat="server" Text="0" Width="70px" ForeColor="#ff0000" Font-Bold="true" CommandArgument="Total" CommandName="4"></asp:LinkButton></td>
                                                <td align="center">
                                                    <asp:LinkButton ID="NFinTxnApproved" runat="server" Text="0" Width="70px" ForeColor="#005d7e" Font-Bold="true" CommandArgument="Success" CommandName="4"></asp:LinkButton></td>
                                                <td align="center">
                                                    <asp:LinkButton ID="NFinTxnFailed" runat="server" Text="0" Width="70px" ForeColor="#a2460e" Font-Bold="true" CommandArgument="Failed" CommandName="4"></asp:LinkButton></td>
                                                <td align="center">
                                                    <asp:LinkButton ID="NFinTxnReversal" runat="server" Text="0" Width="70px" Font-Bold="true" CommandArgument="Timeout" CommandName="4"></asp:LinkButton></td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel ID="panelGrid" runat="server" Style="width: 75%; padding: 06px 0px; overflow: auto; margin: 0 auto;">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <div class="HeaderStyle gv-responsive" style="width: 100%; overflow-x: auto;">
                                <asp:GridView ID="gvAllTransactions" runat="server"
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
                                    Font-Size="Small"
                                    GridLines="Both"
                                    AllowPaging="true"
                                    AllowCustomPaging="true"
                                    PagerSettings-Mode="NumericFirstLast"
                                    PagerSettings-FirstPageText="First Page"
                                    PagerSettings-LastPageText="Last Page"
                                    AllowSorting="True"
                                    PageSize="50"
                                    OnSorting="gvAllTransactions_Sorting"
                                    Visible="true"
                                    OnPageIndexChanging="gvAllTransactions_PageIndexChanging"
                                    OnRowDataBound="gvAllTransactions_RowDataBound"
                                    OnDataBound="gvAllTransactions_DataBound">
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
                </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" BackgroundCssClass="modalBackground" DropShadow="false" />

    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlChannelType.ClientID%>").select2();
                    $("#<%=ddlTranType.ClientID%>").select2();
                    $("#<%=ddlTransactionStatus.ClientID%>").select2();
                    $("#<%=ddlAggregator.ClientID%>").select2();
                    $("#<%=ddlAction.ClientID%>").select2();
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
    <script src="../../Scripts/executeSec.js"></script>

    <script type="text/javascript">
        function hideAccordion() {
            var accordionElement = document.getElementById('collapseSummaryOne');
            if (accordionElement) {
                var accordionCollapse = new bootstrap.Collapse(accordionElement, {
                    toggle: false
                });
                accordionCollapse.hide();
            }
        }
    </script>


</asp:Content>
