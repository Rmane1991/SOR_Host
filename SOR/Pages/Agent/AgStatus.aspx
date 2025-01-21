<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="AgStatus.aspx.cs" Inherits="SOR.Pages.Agent.AgStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/bootstrap-modal.js"></script>
    <script type="text/javascript" src="../../JavaScripts/FranchiseManagement.js"></script>
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

        .select-grid-gap.row > * {
            margin-bottom: 10px;
        }

        .GridView {
            width: 100%;
            height: auto;
            overflow-y: auto;
            border: solid 0px #B9B9B9 !important;
        }

            .GridView table {
                width: 1%;
                /*border-collapse: collapse;*/
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


        .form-control {
            height: 25px;
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

        .form-control {
            height: 25px;
        }
    </style>
    <script type="text/javascript">

        function openModal() {
            debugger;
            $('#ModalAgents').modal('show');
        }


        function ClearRemark() {
            debugger;


            $('#ModalAgents').modal('hide');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <asp:HiddenField ID="hdFromDate" runat="server" />
    <asp:HiddenField ID="hdToDate" runat="server" />
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
            <asp:HiddenField ID="hidAccIFC" runat="server" />
            <asp:HiddenField ID="hidAccNo" runat="server" />
            <asp:HiddenField ID="HdnDMT" runat="server" />
            <asp:HiddenField ID="HdnAEPS" runat="server" />
            <asp:HiddenField ID="HdnBBPS" runat="server" />
            <asp:HiddenField ID="HdnMATM" runat="server" />

            <div id="DIVFilter" runat="server" visible="true">
                <div class="container-fluid">
                    <div class="breadHeader">
                        <h5 class="page-title">Overall Agent Status</h5>
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
                                                <div class="col" style="display: none;">
                                                    <label class="selectInputLabel" for="selectInputLabel">Status</label>
                                                    <div class="selectInputBox" style="display: none;">
                                                        <%-- <asp:DropDownList ID="ddlClientCode" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlClientCode_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">--Client--</asp:ListItem>
                                                </asp:DropDownList>--%>

                                                        <asp:DropDownList ID="ddlStatusType" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                                            <%--      <asp:ListItem Text="Aproove" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="" Value=""></asp:ListItem>--%>
                                                            <asp:ListItem Text="Checker Approve" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Checke Decline" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Maker Approve" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Maker Decline" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="Deactive" Value="5"></asp:ListItem>
                                                            <%-- <asp:ListItem Text="Terminated" Value="6"></asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="col" style="display: none;">
                                                    <label class="selectInputLabel" for="selectInputLabel">Client:</label>
                                                    <div class="selectInputBox" style="display: none;">
                                                        <asp:DropDownList ID="ddlClient" runat="server" CssClass="maximus-select w-100" AutoPostBack="true"></asp:DropDownList>
                                                        <%-- OnSelectedIndexChanged="ddlClient_SelectedIndexChanged"--%>
                                                    </div>
                                                </div>

                                                <div class="col">
                                                    <label class="selectInputLabel" for="selectInputLabel">Operation Type</label>
                                                    <div class="selectInputBox">
                                                        <%-- <asp:DropDownList ID="ddlClientCode" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlClientCode_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">--Client--</asp:ListItem>
                                                </asp:DropDownList>--%>

                                                        <asp:DropDownList ID="ddlOperationType" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                                            <%--      <asp:ListItem Text="Aproove" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="" Value=""></asp:ListItem>--%>
                                                            <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Declined" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Pending" Value="3"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="col">
                                                    <label class="selectInputLabel" for="selectInputLabel">Verification Level</label>
                                                    <div class="selectInputBox">
                                                        <%-- <asp:DropDownList ID="ddlClientCode" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlClientCode_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">--Client--</asp:ListItem>
                                                </asp:DropDownList>--%>

                                                        <asp:DropDownList ID="ddlVerification" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Maker" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Checker" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Authorizer" Value="4"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>


                                                <div class="col">
                                                    <label class="selectInputLabel" for="selectInputLabel">From Date</label>
                                                    <div class="selectInputDateBox w-100">
                                                        <input type="date" runat="server" id="txtFromDate" class="multiple-dates select-date form-control" style="width: 100%; height: auto" placeholder="Select Date" onchange="checkDate()" />
                                                    </div>
                                                </div>

                                                <div class="col ">
                                                    <label class="selectInputLabel" for="selectInputLabel">To Date</label>
                                                    <div class="selectInputDateBox w-100">
                                                        <input type="date" runat="server" id="txtToDate" class="multiple-dates select-date form-control" style="width: 100%; height: auto" placeholder="Select Date" onchange="checkToDate()" />
                                                    </div>
                                                </div>

                                                <div class="col" style="display: none;">
                                                    <label class="selectInputLabel" for="selectInputLabel">BC Type</label>
                                                    <div class="selectInputBox" style="display: none;">
                                                        <%-- <asp:DropDownList ID="ddlClientCode" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlClientCode_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">--Client--</asp:ListItem>
                                                </asp:DropDownList>--%>

                                                        <asp:DropDownList ID="ddlBCType" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                                            <%--      <asp:ListItem Text="Aproove" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="" Value=""></asp:ListItem>--%>
                                                            <asp:ListItem Text="BC" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="SubBc" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="col" style="display: none;">
                                                    <label class="selectInputLabel" for="selectInputLabel">Status</label>
                                                    <div class="selectInputBox" style="display: none;">
                                                        <%-- <asp:DropDownList ID="ddlClientCode" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlClientCode_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">--Client--</asp:ListItem>
                                                </asp:DropDownList>--%>

                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                                            <%--      <asp:ListItem Text="Aproove" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="" Value=""></asp:ListItem>--%>
                                                            <asp:ListItem Text="Onbord" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Active" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Deactive" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Terminate" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="ReEdit" Value="5"></asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <!-- input -->
                                                <div class="col" style="display: none;">
                                                    <label class="selectInputLabel" for="selectInputLabel">Agent</label>
                                                    <div class="selectInputBox">
                                                        <asp:DropDownList ID="ddlAgent" runat="server" CssClass="maximus-select w-100" AutoPostBack="false"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <!-- input -->
                                            </div>

                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                <center><strong><asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label></strong></center>
                                            </div>
                                            <div class="row d-flex justify-content-center align-items-center">
                                                <div class="d-flex justify-content-between align-items-center w-100 ">

                                                    <div class="d-flex justify-content-center gap-3 w-100">
                                                        <button type="button" id="btnSearch" runat="server" class="themeBtn themeApplyBtn" backcolor="#003087" onserverclick="btnSearch_Click">
                                                            Search</button>

                                                        <button type="button" id="btnClear" runat="server" class="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal"
                                                            onserverclick="btnReset_Click" Style="margin-top: 18px">
                                                            Reset</button>
                                                    </div>

                                                    <div class="d-flex justify-content-end">
                                                        <asp:ImageButton ID="BtnCsv" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                            ToolTip="Csv" OnClick="BtnCsv_Click" data-toggle="modal" data-target="#myModal" />

                                                        <asp:ImageButton ID="BtnXls" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                            ToolTip="Xls" OnClick="BtnXls_Click" data-toggle="modal" data-target="#myModal"  />
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
                            </div>
                        </div>
                        <!-- bottom btns -->

                    </div>

                    <asp:Panel ID="panelGrid" runat="server" HorizontalScroll="false" ScrollBars="None" Style="padding: 5px 10px 0px 0px;">
                        <div class="form-group row">
                            <div class="table-box">
                                <div class="tableBorderBox HeaderStyle" style="width: 100%; padding: 10px 10px; overflow: scroll; max-height: 400px;">
                                    <asp:GridView ID="gvTransactions" runat="server"
                                        AutoGenerateColumns="true"
                                        GridLines="None"
                                        AllowPaging="true"
                                        CssClass="GridView"
                                        PageSize="10"
                                        Visible="true"
                                        PagerSettings-Mode="NumericFirstLast"
                                        PagerSettings-FirstPageText="First Page"
                                        PagerSettings-LastPageText="Last Page"
                                        OnPageIndexChanging="gvTransactions_PageIndexChanging">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                        <RowStyle Wrap="false" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No." HeaderStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text='<%  #(Container.DataItemIndex) + 1%>'></asp:Label></td>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <table>
                                                        <tr>
                                                            <td style="background: #fbd2ce;  border: none;">
                                                                <label>Action </label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/Edit-01-512.png" Width="16px" Height="16px"
                                                        ToolTip="Click to Edit Details" OnClick="btnEdit_Click" CommandArgument='<%#Eval("Agent ID") %>' data-toggle="modal" data-target="#myModal"/>
                                                    &nbsp;&nbsp;
                                                <asp:ImageButton ID="btnReulpoad" runat="server" ImageUrl="~/images/UploadNew.png" Width="16px" Height="16px"
                                                    ToolTip="Click to reupload documets" OnClick="btnReulpoad_Click" CommandArgument='<%#Eval("Agent ID") %>' data-toggle="modal" />&nbsp;&nbsp;
                                                <asp:ImageButton ID="btnView" runat="server" ImageUrl="~/images/View.png" Width="16px" Height="16px"
                                                    ToolTip="Click to view documents" OnClick="btnView_Click" CommandArgument='<%#Eval("Agent ID") %>' data-toggle="modal" data-target="#myModal" />&nbsp;&nbsp;
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>




                    <asp:Button ID="btnhidresetDelete" runat="server" Text="Button" Visible="true" Style="visibility: hidden" />

                    <asp:Panel ID="Panel_Declincard" Style="display: none;" runat="server">
                        <div class="modal-dialog modal-dialog-centered" role="document">
                            <div class="modal-content">
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
                                                <asp:TextBox ID="TxtRemarks" runat="server" Style="resize: none" TextMode="MultiLine" Width="400px" Rows="5" Height="70" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="TxtRemarks" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please enter Remarks"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- Modal footer -->

                            </div>
                        </div>

                    </asp:Panel>
                </div>
            </div>


            <%-- Edit Image Click--%>

            <div id="DIVRegister" runat="server" visible="false">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                        </div>
                        <div class="clearfix"></div>
                        <div class="panel-body">
                            <div class="accordion summary-accordion" id="history-accordion">
                                <div class="accordion-item">
                                    <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOne">
                                        <h6 class="searchHeader-heading">General Information</h6>
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



                                                <div class="col" id="Div_Chk" runat="server" style="width: 800px">
                                                    <asp:Label ID="lblServicesOffer" runat="server" Text="Services Offer "></asp:Label><span class="err">*</span><br />

                                                    <asp:CheckBox ID="chkAEPS" runat="server" Style="font-weight: 500" />
                                                    <asp:Label ID="lblchkAEPS" runat="server">AEPS &emsp;</asp:Label>

                                                    <asp:CheckBox ID="chkMATM" runat="server" />
                                                    <asp:Label ID="lblchkMATM" runat="server">Micro ATM &emsp;</asp:Label>
                                                </div>

                                                <div class="col" style="display: none" runat="server" visible="false">
                                                    <label for="exampleInputEmail1">Agent Profile Image  <span class="err">*</span></label>
                                                    <asp:Image ID="imgMyImge" runat="server" Height="110px" Width="110px" BorderStyle="Double" ImageUrl="../../../images/Profile1.png" />
                                                    <asp:FileUpload ID="flgUplodMyImage" Width="120px" runat="server" Style="margin-top: 5px" onchange="return FunImageFIll(this);" CssClass="btn btn-small btn-default" />
                                                    <asp:Label runat="server" ID="lblMessage"></asp:Label>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                <div class="col">
                                                    <label for="exampleInputEmail1">First Name <span class="err">*</span></label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator0" ControlToValidate="txtFirstName" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please enter first name"></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtFirstName" PlaceHolder="First Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtFirstName" runat="server" Value="1" />
                                                    <cc1 id="FilteredTextBoxExtender10" runat="server" filtertype="LowercaseLetters, UppercaseLetters,Custom" validchars=" " targetcontrolid="txtFirstName" />
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">Middle Name <span class="err">*</span></label>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtMiddleName" PlaceHolder="Middle Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <cc1 id="FilteredTextBoxExtender1" runat="server" filtertype="LowercaseLetters, UppercaseLetters,Custom" validchars=" " targetcontrolid="txtMiddleName" />
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">Last Name <span class="err">*</span></label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtLastName" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please enter last name"></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtLastName" PlaceHolder="Last Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <cc1 id="FilteredTextBoxExtender2" runat="server" filtertype="LowercaseLetters, UppercaseLetters,Custom" validchars=" " targetcontrolid="txtLastName" />
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">Father Name <span class="err">*</span></label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldlValidator2" ControlToValidate="txtFatherName" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please enter Father name"></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtFatherName" PlaceHolder="Father Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <cc1 id="FilteredTextBoxExtender3" runat="server" filtertype="LowercaseLetters, UppercaseLetters,Custom" validchars=" " targetcontrolid="txtFatherName" />

                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">Gender  <span class="err"></span></label>
                                                    <asp:DropDownList runat="server" class="form-control" ID="ddlGender" Width="100%" CssClass="maximus-select w-100">
                                                        <asp:ListItem Value="Male">Male</asp:ListItem>
                                                        <asp:ListItem Value="Female">Female</asp:ListItem>
                                                        <asp:ListItem Value="Transgender">Transgender</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                            </div>
                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                <div class="col" id="dvfield_PANNo" runat="server" style="display: normal;">
                                                    <label for="exampleInputEmail1">PAN No. <span class="err">*</span></label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ControlToValidate="txtPANNo" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter pan no"></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtPANNo" PlaceHolder="Enter 10 digit PAN No." Width="100%" MaxLength="10"></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtPANNo" runat="server" Value="1" />
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationGroup="AgentReg" runat="server" Display="None" ErrorMessage="Provided PAN No is not valid! Please enter PAN No start with 'eg.ABCDE1234K'." ControlToValidate="txtPANNo" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}" />
                                                    <cc1 id="FilteredTextBoxExtender6" runat="server" filtertype="UppercaseLetters,Numbers, LowercaseLetters" targetcontrolid="txtPANNo" />
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">GST No. <span class="err">*</span></label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="txtGSTNo" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter GST no"></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtGSTNo" MaxLength="15" Width="100%" PlaceHolder="Enter 15 digit GST no."></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtGSTNo" runat="server" Value="1" />
                                                </div>

                                                <div class="col">
                                                    <label for="exampleInputEmail1">Aadhaar card No. <span class="err">*</span></label>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox1" Width="100%" PlaceHolder="Enter 12 digit Aadhaarcard No." MaxLength="12" onkeypress="return isNumber(event)"></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtaadharno" runat="server" Value="1" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator41" ControlToValidate="txtaadharno" Display="None" runat="server" CssClass="err" ErrorMessage="Please enter Aadhaarcard no"></asp:RequiredFieldValidator>
                                                    <cc1 id="FilteredTextBoxExtender34" runat="server" filtertype="Numbers" targetcontrolid="txtaadharno" />
                                                </div>

                                                <div class="col">
                                                    <label for="exampleInputEmail1">AgentCategory <span class="err">*</span></label>
                                                    <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlCategory" Width="100%" CssClass="maximus-select w-100">
                                                        <asp:ListItem Value="General">General</asp:ListItem>
                                                        <asp:ListItem Value="OBC">OBC</asp:ListItem>
                                                        <asp:ListItem Value="SC">SC</asp:ListItem>
                                                        <asp:ListItem Value="ST">ST</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">Type Of Organization <span class="err">*</span></label>
                                                    <asp:DropDownList runat="server" class="maximus-select w-100" ID="DDlOrg" Width="100%" CssClass="maximus-select w-100">
                                                        <asp:ListItem Value="General">General</asp:ListItem>
                                                        <asp:ListItem Value="OBC">OBC</asp:ListItem>
                                                        <asp:ListItem Value="SC">SC</asp:ListItem>
                                                        <asp:ListItem Value="ST">ST</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>




                                            </div>
                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                <div class="col">
                                                    <label for="exampleInputAccountNumber">Account Number <span class="err">*</span></label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator36" ControlToValidate="txtAccountNumber" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter Account Number"></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtAccountNumber" Width="100%" PlaceHolder="Account Number" MaxLength="16"></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtAccountNumber" runat="server" Value="1" />
                                                    <cc1 id="FilteredTextBoxExtender22" runat="server" filtertype="Numbers" targetcontrolid="txtAccountNumber" />
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputIFSCode">IFSC Code<span class="err">*</span></label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator35" ControlToValidate="txtIFsccode" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter IFSC Code"></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtIFsccode" PlaceHolder="IFSC Code" Width="100%" MaxLength="11"></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtIFsccode" runat="server" Value="1" />
                                                    <cc1 id="FilteredTextBoxExtender23" runat="server" filtertype="UppercaseLetters,Numbers" targetcontrolid="txtIFsccode" />
                                                </div>
                                            </div>

                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <br />
                <div style="height: 10px"></div>
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                        </div>
                        <div class="clearfix"></div>
                        <div class="panel-body">
                            <div class="accordion summary-accordion" id="history-accordions">
                                <div class="accordion-item">
                                    <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes">
                                        <h6 class="searchHeader-heading">Communication Details</h6>
                                        <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOnes"
                                            aria-expanded="true" aria-controls="collapseOne">
                                            <span class="icon-hide"></span>
                                            <p>Show / Hide</p>
                                        </button>
                                    </div>

                                    <div id="collapseSummaryOnes" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                        data-bs-parent="#summary-accordion">
                                        <div class="accordion-body">
                                            <hr class="hr-line">
                                            <!-- grid -->

                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                <div class="col1">
                                                    <label for="exampleInputEmail1">Registered Address <span class="err">*</span></label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtRegisteredAddress" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please enter registered address"></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" class="input-text form-control" ID="txtRegisteredAddress" TextMode="MultiLine" Width="100%" PlaceHolder="Registered Address " Style="resize: none"></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtRegisteredAddress" runat="server" Value="1" />
                                                </div>
                                                <!-- input -->
                                                <div class="col">
                                                    <label for="exampleInputEmail1">Country <span class="err">*</span></label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ControlToValidate="ddlCountry" InitialValue="0" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please select country"></asp:RequiredFieldValidator>
                                                    <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlCountry" Width="100%" CssClass="maximus-select w-100" AutoPostBack="true"></asp:DropDownList>
                                                    <%--OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"--%>
                                                    <asp:HiddenField ID="hd_ddlCountry" runat="server" Value="1" />
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">State <span class="err">*</span></label>
                                                    <asp:DropDownList runat="server" class="form-control" ID="ddlState" Width="100%" CssClass="maximus-select w-100" AutoPostBack="true"></asp:DropDownList>
                                                    <%--OnSelectedIndexChanged="ddlState_SelectedIndexChanged"--%>
                                                    <asp:HiddenField ID="hd_ddlState" runat="server" Value="1" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ControlToValidate="ddlState" Width="100%" InitialValue="0" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please select state"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">District  <span class="err">*</span></label>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtDistrict" Width="100%" PlaceHolder="Enter district" MaxLength="25"></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtDistrict" runat="server" Value="1" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="txtDistrict" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please enter district Name "></asp:RequiredFieldValidator>
                                                    <cc1 id="FilteredTextBoxExtender8" runat="server" filtertype="LowercaseLetters, UppercaseLetters" targetcontrolid="txtDistrict" />

                                                </div>
                                                <br />
                                            </div>

                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                <div class="col">
                                                    <label for="exampleInputEmail1">City <span class="err">*</span></label>
                                                    <asp:DropDownList runat="server" class="form-control" Width="100%" ID="ddlCity" CssClass="maximus-select w-100"></asp:DropDownList>
                                                    <asp:HiddenField ID="hd_ddlCity" runat="server" Value="1" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" ControlToValidate="ddlCity" InitialValue="0" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please select city"></asp:RequiredFieldValidator>
                                                </div>

                                                <div class="col">
                                                    <label for="exampleInputEmail1">Pincode <span class="err">*</span></label>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" PlaceHolder="eg.400601" Width="100%" ID="txtPinCode" MaxLength="6"></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtPinCode" runat="server" Value="1" />
                                                    <cc1 id="FilteredTextBoxExtender28" runat="server" filtertype="Numbers" targetcontrolid="txtPinCode" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtPinCode" ValidationGroup="AgentReg" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter pincode"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">Personal Email Id <span class="err">*</span></label>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtEmailID" PlaceHolder="Email ID" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ControlToValidate="txtEmailID" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please enter Personal email id"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="validateEmail" runat="server" Display="None" ValidationGroup="AgentReg" ErrorMessage="Please enter valid Personal email id" ControlToValidate="txtEmailID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                    <cc1 id="FilteredTextBoxExtender7" runat="server" filtertype="Numbers,UppercaseLetters, LowercaseLetters, Custom" validchars=".@!#$%^&*()_,/\-" targetcontrolid="txtEmailID" />
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">Contact No. <span class="err">*</span></label>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox2" PlaceHolder="Enter contact No." Width="100%" MaxLength="10"></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtContactNo" runat="server" Value="1" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="txtContactNo" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please enter contact no"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="None" ValidationGroup="AgentReg" ErrorMessage="Provided mobile number is not valid! Please enter valid 10 digit mobile number start with '6/7/8/9'." ControlToValidate="txtContactNo" ValidationExpression="^[6789]\d{9}$" />
                                                    <cc1 id="FilteredTextBoxExtender11" runat="server" filtertype="Numbers" targetcontrolid="txtContactNo" />
                                                </div>
                                                <div class="col">
                                                    <label for="exampleInputEmail1">Landline No. <span class="err">*</span></label>
                                                    <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtLandlineNo" PlaceHolder="Enter landline no." Width="100%" MaxLength="10"></asp:TextBox>
                                                    <asp:HiddenField ID="hd_txtLandlineNo" runat="server" Value="1" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="txtLandlineNo" Style="display: none" ValidationGroup="AgentReg" runat="server" CssClass="err" ErrorMessage="Please enter landline no"></asp:RequiredFieldValidator>
                                                    <cc1 id="FilteredTextBoxExtender12" runat="server" filtertype="Numbers" targetcontrolid="txtLandlineNo" />
                                                </div>

                                            </div>

                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">


                                                <div class="col">
                                                    <label for="exampleInputEmail1">Alternate No </label>
                                                    <div class="inputBox w-100">
                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtAlterNateNo" PlaceHolder="Enter contact No." Width="100%" MaxLength="10"></asp:TextBox>
                                                        <asp:HiddenField ID="hd_txtAlterNateNo" runat="server" Value="0" />
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="None" ValidationGroup="AgentReg" ErrorMessage="Provided mobile number is not valid! Please enter valid 10 digit mobile number start with '7/8/9'." ControlToValidate="txtAlterNateNo" ValidationExpression="^[789]\d{9}$" />
                                                        <cc1 id="FilteredTextBoxExtender20" runat="server" filtertype="Numbers" targetcontrolid="txtAlterNateNo" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                <asp:Button runat="server" ID="btnSubmitDetails" ValidationGroup="AgentReg" OnClientClick="javascript:return  disableMultipleClick();" CssClass="themeBtn themeApplyBtn" BackColor="#003087" Text="Submit"></asp:Button>
                                                <%--OnClick="btnSubmitDetails_Click"--%>
                                                <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="themeBtn resetBtn themeCancelBtn me-0" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
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
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" BackgroundCssClass="modalBackground" DropShadow="false" />

    <%--    <script>
        $(function () {
            $("#<%=ddlStatusType.ClientID%>").select2();
            $("#<%=ddlFranchise.ClientID%>").select2();
            $("#<%=ddlClient.ClientID%>").select2();
            $("#<%=ddlExport.ClientID%>").select2();
        })
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlStatusType.ClientID%>").select2();
                    $("#<%=ddlFranchise.ClientID%>").select2();
                    $("#<%=ddlClient.ClientID%>").select2();
                    $("#<%=ddlExport.ClientID%>").select2();
                }
            });
        };
    </script>--%>

    <script>

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlStatusType.ClientID%>").select2();
                    $("#<%=ddlAgent.ClientID%>").select2();
                    $("#<%=ddlClient.ClientID%>").select2();
                    $("#<%=ddlGender.ClientID%>").select2();
                    $("#<%=ddlCategory.ClientID%>").select2();
                    $("#<%=DDlOrg.ClientID%>").select2();
                    $("#<%=ddlCountry.ClientID%>").select2();
                    $("#<%=ddlState.ClientID%>").select2();
                    $("#<%=ddlCity.ClientID%>").select2();
                    $("#<%=ddlOperationType.ClientID%>").select2();
                    $("#<%=ddlVerification.ClientID%>").select2();
                    $("#<%=ddlBCType.ClientID%>").select2();
                    $("#<%=ddlStatus.ClientID%>").select2();

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
