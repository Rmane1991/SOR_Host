<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="RestrictedPIN.aspx.cs" Inherits="SOR.Pages.Upload.RestrictedPIN" %>
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
        /* width */
        ::-webkit-scrollbar {
            width: 6px;
        }

            ::-webkit-scrollbar:horizontal {
                height: 6px;
            }

        /* Track */
        ::-webkit-scrollbar-track {
            background: #f1f1f1;
        }

        /* Handle */
        ::-webkit-scrollbar-thumb {
            background: #888;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #555;
            }

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

        .select-date {
            height: 32px;
            width: 204px;
            background-color: var(--border-box-bg);
            border: 1px solid var(--color-border-default);
            border-radius: 4px;
            margin: 0;
            letter-spacing: 0.02em !important;
            color: var(--color-black) !important;
            font-size: var(--font-m-size12) !important;
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
        }

        .GridView td {
            background: none;
            color: #420629;
            padding: 5px;
            border: 1px solid #CCC;
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
        .table tbody tr th {
            background: none;
            color: #fbd2ce;
        }

        th {
            background-color: #fbd2ce;
            color: black;
        }
        .element.style {
            height: 33px;
            width: 133px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:UpdateProgress ID="upContentBodyUpdateProgress" runat="server" AssociatedUpdatePanelID="upContentBody">
            <ProgressTemplate>
                <div style="position: fixed; left: 50%; top: 50%; opacity: 1.8;">
                    <img alt="" id="progressImage1" src='<%=Page.ResolveClientUrl("../../Images/loading2_1.gif") %>' />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>
    <asp:UpdatePanel ID="upContentBody" runat="server">
        <ContentTemplate>
            <div class="breadHeader">
                                <h5 class="page-title">Restricted PIN</h5>
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
                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
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
                                                <!-- input -->
                                                <div class="col">
                                                    <label class="selectInputLabel" for="selectInputLabel">View In</label>
                                                    <div class="selectInputBox">
                                                        <asp:DropDownList runat="server" ID="ddlFileTypeStatus" CssClass="maximus-select w-100" TabIndex="1" AppendDataBoundItems="true" AutoPostBack="False">
                                                            <asp:ListItem Value="-1" Text="-- File Status --"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Success"></asp:ListItem>
                                                            <asp:ListItem Value="0" Text="Failed"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <!-- input -->
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                    <button type="button" id="btnsearch" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnsearch_Click">
                                                        Search</button>
                                                    <button type="button" id="Btnclear" runat="server" class="themeBtn resetBtn themeCancelBtn me-0" style="margin-top: -5px"
                                                        onserverclick="Btnclear_Click">
                                                        Clear</button>
                                                </div>
                                            </div>

                                            <br />
                                            <h6 class="searchHeader-heading">File Upload</h6>
                                            <br />
                                            <hr class="hr-line">
                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                <div class="col col-all-filter" style="display: none">
                                                    <label class="selectInputLabel" for="selectInputLabel">Date</label>
                                                    <div class="selectInputDateBox w-100">
                                                        <input type="date" runat="server" id="txtDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" />
                                                    </div>
                                                </div>

                                                <%--<div class="col">
                                                    <label class="selectInputLabel" for="selectInputLabel">BC</label>
                                                    <div class="selectInputBox">
                                                        <asp:DropDownList runat="server" ID="ddlBCCode" CssClass="maximus-select w-100" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>--%>
                                                <div class="col">
                                                    <label class="selectInputLabel" for="selectInputLabel">File Upload</label>
                                                    <div class="selectInputBox">
                                                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control" Height="33px" Width="270px" Font-Size="Small" />
                                                    </div>
                                                </div>



                                            </div>
                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                <button type="button" id="btnSave" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnSave_Click"
                                                    data-bs-target="#SearchFilterModal">
                                                    Upload</button>
                                                <button type="button" id="btncancel" runat="server" class="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal" style="margin-top: -5px"
                                                    onserverclick="btncancel_ServerClick">
                                                    Cancel</button>
                                                <button type="button" id="btnsample" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnsample_Click"
                                                    data-bs-target="#SearchFilterModal">
                                                    Sample</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="table-box">
                                    <div class="tableBorderBox HeaderStyle" style="width: 100%; padding: 06px 10px; overflow: scroll; max-height: 400px;">
                                        <asp:GridView ID="gvRestrictedPIN" runat="server" CssClass="GridView"
                                            AutoGenerateColumns="false"
                                            Width="100%"
                                            GridLines="None"
                                            AllowPaging="true"
                                            PageSize="10"
                                            Visible="true"
                                            DataKeyNames="FileID"
                                            AllowSorting="True"
                                            OnRowCommand="gvRestrictedPIN_RowCommand"
                                            OnPageIndexChanging="gvRestrictedPIN_PageIndexChanging"
                                            HeaderStyle-CssClass="pagination-ys">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" BackColor="#FBD2CE" Font-Bold="true" CssClass="bg" />
                                            <RowStyle Wrap="false" VerticalAlign="Middle" HorizontalAlign="Center" />

                                            <Columns>
                                                <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderText="Action" HeaderStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="UpButtons" runat="server">
                                                            <ContentTemplate>

                                                                <%--      <asp:ImageButton ID="lbtnEdit" runat="server" CommandArgument='<%# Eval("FileID") %>'
                                            CommandName="EditUpload" ToolTip="Edit Record" Width="16" Height="16" ImageUrl="~/Images/Edit-01-512.png" />--%>

                                                                <asp:ImageButton ID="btnViewDownload" runat="server" ImageUrl="../../Images/download.png" Width="16" Height="16"
                                                                    CommandName="DownloadDoc" ToolTip="Download Document" CommandArgument='<%# Eval("FileID") %>' />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="btnViewDownload" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text-center" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="FileID" HeaderText="File ID" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FileName" HeaderText="File Name" />
                                                <asp:BoundField DataField="CreatedBy" HeaderText="Imported By" />
                                                <asp:BoundField DataField="CreatedOn" HeaderText="Imported On" />
                                                <asp:BoundField DataField="Valid Records" HeaderText="Valid Records" />
                                                <asp:BoundField DataField="InValid Records" HeaderText="InValid Records" />
                                                <asp:BoundField DataField="Total Records" HeaderText="Total Records" />

                                                <asp:BoundField DataField="FileStatus" HeaderText="File Status" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
            </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnsample" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlFileTypeStatus.ClientID%>").select2();
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

    <%-- //Loader Start--%>
    <script>

        $(function () {
            window.onsubmit = submitAction;
        });

        function submitAction() {
            var updateProgress = $find("<%= upContentBodyUpdateProgress.ClientID %>");
            window.setTimeout(function () {
                updateProgress.set_visible(true);
            }, 1000);
        }
    </script>
</asp:Content>
