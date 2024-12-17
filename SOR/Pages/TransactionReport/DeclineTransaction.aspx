<%@ Page Title="" Language="C#" MasterPageFile="~/PayRakamSBM.Master" AutoEventWireup="true" CodeBehind="DeclineTransaction.aspx.cs" Inherits="PayRakamSBM.Pages.TransactionReport.DeclineTransaction" %>
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
            text-align: center;
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
            <%--<div style="clear: both;" />--%>
            <div class="container-fluid">
                  <div class="breadHeader">
                    <h5 class="page-title">Decline Transaction</h5>
                </div>
                <%--<div class="breadcrumb"><i class="lnr lnr-home"></i>/ TRANSACTION REPORT  / DECLINE TRANSACTION</div>--%>
                <%--<asp:Panel ID="pnlFilter" runat="server" Width="100%" HorizontalScroll="false" ScrollBars="None" Style="border: 1px solid black; padding: 5px 10px 0px 10px; margin: 5px 0px 5px 0px;">--%>
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

                                    <div class="Col" style="width:20%";>
                                        <label class="selectInputLabel" for="selectInputLabel">Channel Type</label>
                                        <asp:DropDownList ID="ddlChannelType" runat="server" CssClass="maximus-select w-100"  AutoPostBack="false">
                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="AEPS" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="MATM" Value="2"></asp:ListItem>
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

                                    <div class="Col" style="width:20%";>
                                        <label class="selectInputLabel" for="selectInputLabel">Txn Type</label>
                                        <asp:DropDownList ID="ddlTranType" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Withdrawal" Value="010000"></asp:ListItem>
                                            <asp:ListItem Text="Balance Enquiry" Value="310000"></asp:ListItem>
                                            <asp:ListItem Text="MiniStatement" Value="900000"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="Col" style="width:20%";>
                                        <label class="selectInputLabel" for="selectInputLabel">Txn Status</label>
                                        <asp:DropDownList ID="ddlTransactionStatus" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                            <asp:ListItem Text="Failed" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                     </div>
                                 <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
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
                            
                                     <div class="col-sm-6 col-xs-4 cssRecordsTotal" style="margin-left: 234px">
                                        <center><strong><asp:Label ID="lblRecordCount" CssClass="form-contol" runat="server" Text=""></asp:Label></strong></center>
                                    </div>
                                </div>

                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="BtnCsv" />
                                    <asp:PostBackTrigger ControlID="BtnXls" />

                                </Triggers>
                                <ContentTemplate>

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">

                                            <button type="button" id="Button1" runat="server" style="margin-top: -7px" class="themeBtn themeApplyBtn" onserverclick="buttonSearch_Click">
                                                Search</button>
                                            <button type="button" id="Button2" runat="server" style="margin-top: -7px" onserverclick="btnClear_Click" class="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal">
                                                Clear</button>

                                            <div style="margin-top: unset">
                                                <asp:ImageButton ID="BtnCsv" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                    ToolTip="Csv" OnClick="BtnCsv_Click" data-toggle="modal" data-target="#myModal" />

                                                <asp:ImageButton ID="BtnXls" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                    ToolTip="Xls" OnClick="BtnXls_Click" data-toggle="modal" data-target="#myModal" visible="false"/>
                                            </div>
                                        </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                 <%--   </asp:Panel>--%>
                 <asp:Panel ID="panelGrid" runat="server" Width="100%" HorizontalScroll="false" ScrollBars="None" Style="margin: 5px 10px 0px 0px; padding-right: 0px; overflow: auto">
                <div class="HeaderStyle gv-responsive">    
                    <asp:GridView ID="gvDeclineTranaction" runat="server"
                        AutoGenerateColumns="true"
                        AllowCustomPaging="true"
                        GridLines="None"
                        AllowPaging="true"
                        CssClass="GridView"
                        PageSize="500"
                        Visible="true"
                        PagerSettings-Mode="NumericFirstLast"
                        PagerSettings-FirstPageText="First Page"
                        PagerSettings-LastPageText="Last Page"
                        OnPageIndexChanging="gvDeclineTranaction_PageIndexChanging">
                        <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                        <RowStyle Wrap="false" />
                    </asp:GridView>
                         </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" BackgroundCssClass="modalBackground" DropShadow="false" />
    <%--<script>
        $(function () {
            $("#<%=ddlClient.ClientID%>").select2();
            $("#<%=ddlChannelType.ClientID%>").select2();
            $("#<%=ddlSelectDate.ClientID%>").select2();
        })
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlClient.ClientID%>").select2();
                    $("#<%=ddlChannelType.ClientID%>").select2();
                    $("#<%=ddlSelectDate.ClientID%>").select2();
                }
            });
        };
    </script>--%>

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
