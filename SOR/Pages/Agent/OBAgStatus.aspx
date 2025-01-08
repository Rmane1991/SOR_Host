<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="OBAgStatus.aspx.cs" Inherits="SOR.Pages.Agent.OBAgStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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

        .GridView th, .GridView td {
            text-align: center;
            vertical-align: middle;
        }
    </style>


    <script type="text/javascript">
        function disableMultipleClick() {
            debugger;
            document.getElementById("cpbdCarde_btnSaveAction").disabled = true;
            document.getElementById("cpbdCarde_btnSaveAction").innerHTML = "Processing";
            __doPostBack('ctl00$cpbdCarde$btnSaveAction', '');
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:UpdateProgress ID="upContentBodyUpdateProgress" runat="server" AssociatedUpdatePanelID="upContentBody">
            <ProgressTemplate>
                <div style="position: fixed; left: 50%; top: 50%; opacity: 1.8;">
                    <img alt="" id="progressImage1" src='<%=Page.ResolveClientUrl("../Images/indicator_waitanim.gif") %>' />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>

    <asp:UpdatePanel ID="upContentBody" runat="server">
        <ContentTemplate>

            <div class="breadHeader">
                <h5 class="page-title">Onboard Agent Status</h5>
                <%-- <i class="lnr lnr-home"></i>/ AGENT MANAGEMENT / DEACTIVE AGENTS--%>
                <input id="btnRedirectAudit" type="button" value="View Audit Logs" class="btn-default pull-right" style="margin-right: -15px; display: none" onclick="window.location = '../../YES/Report/pgAgentAuditReports.aspx';" />

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
                            <!-- grid -->
                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                <!-- input -->
                                <div class="col">
                                    <label class="selectInputLabel" for="selectInputLabel">Business Correspondents:</label>
                                    <div class="selectInputBox">
                                        <asp:DropDownList runat="server" ID="ddlBCCode" CssClass="maximus-select w-100" OnSelectedIndexChanged="ddlBCCode_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col">
                                    <label class="selectInputLabel" for="selectInputLabel">Agent Code:</label>
                                    <div class="selectInputBox">
                                        <asp:DropDownList runat="server" ID="ddlAgent" CssClass="maximus-select w-100">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col">
                                    <label class="selectInputLabel" for="selectInputLabel">Action Type:</label>
                                    <div class="selectInputBox">
                                        <asp:DropDownList runat="server" ID="ddlActiontype" CssClass="maximus-select w-100">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Activated" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Deactivated" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Terminated" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <!-- input -->


                                <!-- input -->
                                <div class="col" style="display: none">
                                    <label class="selectInputLabel" for="selectInputLabel" style="display: none">State</label>
                                    <div class="selectInputBox">
                                        <asp:DropDownList runat="server" ID="ddlState" CssClass="maximus-select w-100" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col" style="display: none">
                                    <label class="selectInputLabel" for="selectInputLabel" style="display: none">city</label>
                                    <div class="selectInputBox w-100">
                                        <asp:DropDownList runat="server" ID="ddlCity" CssClass="maximus-select w-100" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <!-- input -->




                                <!-- input -->
                                <div class="col" style="display: none">
                                    <label class="selectInputLabel" for="selectInputLabel" style="display: none">city</label>
                                    <div class="selectInputBox">

                                        <asp:TextBox ID="txtCitySeatch" runat="server" Width="160" CssClass="input-text form-control" Style="width: 100%" placeholder="District"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col" style="display: none">
                                    <label class="selectInputLabel" for="selectInputLabel" style="display: none">Phase</label>
                                    <div class="selectInputBox">
                                        <asp:DropDownList runat="server" ID="ddlStatusSearch" CssClass="maximus-select w-100" Style="display: none" AutoPostBack="false">
                                            <asp:ListItem Text="--Status--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="In-Active" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                            </div>

                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                <center><strong><asp:Label ID="lblRecordsTotal" runat="server" Text=""></asp:Label></strong></center>
                            </div>
                            <div class="row d-flex justify-content-center align-items-center">
                                <div class="d-flex justify-content-between align-items-center w-100 ">

                                    <div class="d-flex justify-content-center gap-3 w-100">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" Width="120px" OnClientClick="return CheckValidations();"
                                            OnClick="btnSearch_Click" TabIndex="5" CssClass="themeBtn themeApplyBtn" BackColor="#003087" />
                                        <asp:Button ID="btnClear_Click" runat="server" Text="Clear" Style="margin-top: 18px"
                                            TabIndex="5" CssClass="themeBtn resetBtn themeCancelBtn me-0" OnClick="btnClear_Click_Click" />
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

            <asp:Panel ID="panelButtonGroup" runat="server" Width="100%" HorizontalScroll="false" ScrollBars="None" Style="margin-bottom: -20px">
                <div class="form-group-sm row">
                    <div style="margin-left: 8px">
                        <asp:Button ID="btnActivate" runat="server" Text="Activate" Visible="false"
                            OnClick="btnActivate_ServerClick" CssClass="themeBtn themeApplyBtn" />

                        <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate" Style="margin-left: 0px" Visible="false"
                            OnClick="btnDeactivate_ServerClick" CssClass="themeBtn themeApplyBtn" />

                        <%--<asp:Button ID="btnTerminate" CssClass="themeBtn resetBtn themeCancelBtn me-0" runat="server" Text="Terminate" Style="height: 30px;"
                            OnClick="btnTerminate_ServerClick" Visible="false" />--%>
                    </div>
                </div>

            </asp:Panel>



            <asp:Panel ID="panelGrid" runat="server" Width="99%" Style="margin: 5px 10px 0px 0px; padding-right: 0px; overflow-x: auto;">

                <div class="table-box">
                    <div class="tableBorderBox HeaderStyle" style="padding: 10px 10px; overflow: scroll; max-height: 400px;">
                        <asp:GridView ID="gvBlockAG" runat="server"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            AllowPaging="true"
                            CssClass="GridView"
                            PageSize="10"
                            Visible="true"
                            BorderColor="White"
                            PagerStyle-Font-Size="Smaller"
                            Font-Size="12px"
                            PagerSettings-Mode="NumericFirstLast"
                            PagerSettings-FirstPageText="First Page"
                            PagerSettings-LastPageText="Last Page"
                            OnRowCommand="gvBlockAG_RowCommand"
                            OnRowDataBound="gvBlockAG_RowDataBound"
                            DataKeyNames="Agent Id,Status"
                            OnPageIndexChanging="gvBlockAG_PageIndexChanging">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                            <RowStyle Wrap="false" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <table>
                                            <tr>
                                                <td style="background: #fbd2ce; color: black; display: none; border: none;">
                                                    <label>All </label>
                                                </td>
                                                <td style="background: #fbd2ce; color: #FFF; border: none;">
                                                    <asp:CheckBox ID="CheckBoxAll" runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="CheckBoxAll_CheckedChanged" /></td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chBoxSelectRow" runat="server" AutoPostBack="true" OnCheckedChanged="chBoxSelectRow_CheckedChanged" />

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderText="Action" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UpButtons" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="lbtnEdit" runat="server" CommandArgument='<%# Eval("Agent Id") %>' runnat="server"
                                                    CommandName="EditDetails" ToolTip="Edit role" Width="16" Height="16" ImageUrl="~/Images/Edit-01-512.png" />
                                                <asp:ImageButton ID="lbtnView" runat="server" CommandArgument='<%# Eval("Agent Id") %>' runnat="server"
                                                    CommandName="ViewDetails" ToolTip="Edit role" Width="16" Height="16" ImageUrl="~/Images/eyeview.png" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text-center" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Agent Id" HeaderText="Agent Id" />
                                <asp:BoundField DataField="Client Id" HeaderText="Client Id" />
                                <asp:BoundField DataField="BC Code" HeaderText="BC Code" />
                                <asp:BoundField DataField="Aggregator Code" HeaderText="Aggregator Code" />
                                <asp:BoundField DataField="Code" HeaderText="Code" />
                                <asp:BoundField DataField="Name" HeaderText="Name" />
                                <asp:BoundField DataField="Mobile No" HeaderText="Mobile No" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="Lattitude" HeaderText="Latitude" />
                                <asp:BoundField DataField="Longitude" HeaderText="Longitude" />
                                <asp:BoundField DataField="State" HeaderText="State" />
                                <asp:BoundField DataField="District" HeaderText="District" />
                                <asp:BoundField DataField="City" HeaderText="City" />
                                <asp:BoundField DataField="Pin Code" HeaderText="Pin Code" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:BoundField DataField="Shop Address" HeaderText="Shop Address" />
                                <asp:BoundField DataField="Shop City" HeaderText="Shop City" />
                                <asp:BoundField DataField="Shop Country" HeaderText="Shop Country" />
                                <asp:BoundField DataField="Shop District" HeaderText="Shop District" />
                                <asp:BoundField DataField="Shop Pin Code" HeaderText="Shop Pin Code" />
                                <asp:BoundField DataField="Shop State" HeaderText="Shop State" />
                                <asp:BoundField DataField="Onboarded By" HeaderText="Onboarded By" />
                                <asp:BoundField DataField="Onboarded On" HeaderText="Onboarded On" />
                                <asp:BoundField DataField="Verification Remarks" HeaderText="Verification Remarks" />
                                <asp:BoundField DataField="Verification On" HeaderText="Verification On" />
                                <asp:BoundField DataField="Verification By" HeaderText="Verification By" />
                                <asp:BoundField DataField="Activity Type" HeaderText="Activity Type" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </asp:Panel>

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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator0" ControlToValidate="TxtRemarks" Style="display: none" ValidationGroup="BCReg" runat="server" CssClass="err" ErrorMessage="Please enter Remarks"></asp:RequiredFieldValidator>
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
                            <asp:Button ID="btnSaveAction" runat="server" Text="Save" Style="width: 15%" class="themeBtn themeApplyBtn" OnClientClick="disableMultipleClick();" OnClick="btnSaveAction_Click" ValidationGroup="BCReg" />
                            <asp:ValidationSummary
                                HeaderText="You must enter or select a value in the following fields:"
                                DisplayMode="BulletList"
                                EnableClientScript="true"
                                CssClass="err"
                                ShowMessageBox="true"
                                ShowSummary="false"
                                ForeColor="Red"
                                ValidationGroup="BCReg"
                                runat="server" />
                            <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnCancelAction" style="width: 15%" type="button" runat="server" causesvalidation="false" onserverclick="btnCancelAction_Click">Cancel</button>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" Visible="true" Style="visibility: hidden" />
            <ajax:ModalPopupExtender ID="ModalPopupResponse" runat="server"
                TargetControlID="Button1" PopupControlID="PanelResponse"
                PopupDragHandleControlID="PopupHeader_Declincard" Drag="true"
                BackgroundCssClass="modalBackground">
            </ajax:ModalPopupExtender>

            <asp:Panel ID="PanelResponse" Style="display: none;" runat="server">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content" style="width: 650px; height: auto;">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">
                                <asp:Label ID="Label1" runat="server" Text="Declined Data"></asp:Label>
                            </h4>
                        </div>
                        <!-- Modal body -->
                        <div class="modal-body">
                            <asp:Label ID="Label2" runat="server" Font-Bold="true"></asp:Label>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="tableBorderBox HeaderStyle" style="padding: 10px 10px; overflow: scroll; max-height: 400px;">
                                            <%-- <textarea id="txtResponse" runat="server" style="padding: 77px 184px; margin: 5px 5px; font-weight: bold; border: none" aria-multiline="true" spellcheck="false" rows="1" cols="30" readonly="readonly"></textarea>--%>
                                            <%--<textarea id="txtResponse" runat="server"  style="font: 20px Arial, sans-serif; padding: 77px 184px; margin: 5px 5px; border: none" aria-multiline="true" spellcheck="false" readonly="readonly"></textarea>--%>
                                            <%--<asp:Label ID="lblCount" Text="Re-Process Count: " runat="server"></asp:Label>--%>
                                            <asp:GridView ID="GVPreverification" runat="server"
                                                AutoGenerateColumns="true"
                                                GridLines="None"
                                                AllowPaging="true"
                                                CssClass="GridView"
                                                PageSize="10"
                                                Visible="true"
                                                OnPageIndexChanging="GVPreverification_PageIndexChanging"
                                                PagerSettings-Mode="NumericFirstLast"
                                                PagerSettings-FirstPageText="First Page"
                                                PagerSettings-LastPageText="Last Page">

                                                <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                <RowStyle Wrap="false" />

                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Modal footer -->
                        <button class="themeBtn resetBtn themeCancelBtn me-0" id="Button2" style="width: 15%; margin-left: 275px; margin-bottom: 10px;" type="button" runat="server" causesvalidation="false" onserverclick="btnClose_ServerClick">Close</button>
                        <%--<button class="btn-CommonCss" id="btnClose" runat="server" causesvalidation="false" type="button" onserverclick="btnClose_ServerClick" style="margin-left: 300px; margin-bottom: 15px;">Close</button>--%>
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

    <script>
        $(function () {
            $("#<%=ddlBCCode.ClientID%>").select2();
            $("#<%=ddlState.ClientID%>").select2();
            $("#<%=ddlCity.ClientID%>").select2();
            $("#<%=ddlAgent.ClientID%>").select2();
            $("#<%=ddlActiontype.ClientID%>").select2();
        })
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlBCCode.ClientID%>").select2();
                    $("#<%=ddlState.ClientID%>").select2();
                    $("#<%=ddlCity.ClientID%>").select2();
                    $("#<%=ddlAgent.ClientID%>").select2();
                    $("#<%=ddlActiontype.ClientID%>").select2();
                }
            });
        };
    </script>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlBCCode.ClientID%>").select2();
                    $("#<%=ddlState.ClientID%>").select2();
                    $("#<%=ddlCity.ClientID%>").select2();
                    $("#<%=ddlAgent.ClientID%>").select2();
                    $("#<%=ddlActiontype.ClientID%>").select2();

                }
            });
        };
    </script>
</asp:Content>
