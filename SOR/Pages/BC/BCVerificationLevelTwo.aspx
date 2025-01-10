<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="BCVerificationLevelTwo.aspx.cs" Inherits="SOR.Pages.BC.BCVerificationLevelTwo" %>

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
        .pagination-ys {
            color: black;
            background-color: #11AEA3;
            border: 1px solid #dddddd;
            margin-left: -1px;
            font-family: Arial Black;
        }

        .form-group {
            margin-bottom: 2px;
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

        .select-grid-gap.row > * {
            margin-bottom: 10px;
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
    <script type="text/javascript">
        function Confirm() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you want to process the data?")) {
                debugger;
                document.getElementById("<%= HiddenConfirm.ClientID %>").value = "Yes";
            }
            else {
                document.getElementById("<%= HiddenConfirm.ClientID %>").value = "No";
            }
        }
    </script>
    <script type="text/javascript">
        function CloseModal() {
            debugger;
            $('#ModalBC').modal('hide');
        }
        function openModal() {
            debugger;
            $('#ModalBC').modal('show');
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
            $('#ModalBC').modal('hide');
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
    <asp:HiddenField ID="HiddenConfirm" runat="server" />
    <asp:UpdatePanel ID="upContentBody" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="breadHeader">
                    <h5 class="page-title">BC Verification</h5>
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
                                            <div class="col" style="display: none">
                                                <label class="selectInputLabel" for="selectInputLabel">Client:</label>
                                                <div class="selectInputBox">
                                                    <asp:DropDownList ID="ddlClientCode" runat="server" CssClass="maximus-select w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlClientCode_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <!-- input -->
                                            <div class="col">
                                                <label class="selectInputLabel" for="selectInputLabel">Business Correspondents:</label>
                                                <div class="selectInputBox">
                                                    <asp:DropDownList ID="ddlBCCode" runat="server" CssClass="maximus-select w-100" AutoPostBack="true">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <!-- input -->
                                            <div class="col">
                                                <label class="selectInputLabel" for="selectInputLabel">Activity Type:</label>
                                                <div class="selectInputBox">
                                                    <asp:DropDownList ID="ddlActivityType" runat="server" CssClass="maximus-select w-100" AutoPostBack="true">
                                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="0">Onboarded</asp:ListItem>
                                                        <asp:ListItem Value="1">Activated</asp:ListItem>
                                                        <asp:ListItem Value="2">Deactivated</asp:ListItem>
                                                        <asp:ListItem Value="3">Terminated</asp:ListItem>
                                                        <asp:ListItem Value="4">ReEdit</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div >
                                        </div>

                                        <div class="row d-flex justify-content-center align-items-center">
                                            <div class="col-auto text-center">
                                                <strong>
                                                    <asp:Label ID="lblRecordsTotal" runat="server" Text=""></asp:Label>
                                                </strong>
                                            </div>
                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                <button type="button" id="btnSearch" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnSearch_Click">
                                                    Search</button>
                                                <button type="button" id="btnClear" runat="server" onserverclick="btnClear_Click" class="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal">
                                                    Reset</button>
                                            </div>
                                            <div class="d-flex justify-content-end">
                                                <asp:ImageButton ID="BtnCsv" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                    ToolTip="Csv" OnClick="BtnCsv_Click" data-toggle="modal" data-target="#myModal" />

                                                <asp:ImageButton ID="BtnXls" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                    ToolTip="Xls" OnClick="BtnXls_Click" data-toggle="modal" data-target="#myModal" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- bottom btns -->

                <asp:Panel ID="panelButtonGroup" runat="server" Width="100%" HorizontalScroll="false" ScrollBars="None" Style="padding: 0px 0px 0px 0px; margin: 10px 0px 10px 0px;" Visible="false">
                    <div class="form-group-sm row" style="margin-left: -21px">
                        <div class="col-sm-12 col-xs-12" style="margin: -21px; margin-left: -10px">
                            <button class="themeBtn themeApplyBtn" style="height: 30px;" id="btnApprove" runat="server" type="button" causesvalidation="false" onserverclick="btnApprove_ServerClick">Approve</button>
                            <button class="themeBtn resetBtn themeCancelBtn me-0" style="height: 30px; margin-left: 0px;" id="btnDecline" runat="server" type="button" onserverclick="btnDecline_ServerClick">Decline</button>
                        </div>
                    </div>
                </asp:Panel>


                <asp:Panel ID="panelGrid" runat="server" HorizontalScroll="false" ScrollBars="None" Style="padding: 5px 10px 0px 0px;">
                    <div class="form-group row">
                        <div class="tableBorderBox HeaderStyle" style="width: 100%; padding: 10px 10px; overflow: scroll; max-height: 400px;">
                            <asp:GridView ID="gvBusinessCorrespondents" runat="server"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                AllowPaging="true"
                                CssClass="GridView"
                                PageSize="10"
                                Visible="true"
                                PagerSettings-Mode="NumericFirstLast"
                                PagerSettings-FirstPageText="First Page"
                                PagerSettings-LastPageText="Last Page"
                                OnPageIndexChanging="gvBusinessCorrespondents_PageIndexChanging">
                                <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                <RowStyle Wrap="false" />
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            <table>
                                                <tr>
                                                    <td style="background: #FBD2CE; color: black; border: none;">
                                                        <label>All </label>
                                                    </td>
                                                    <td style="background: #FBD2CE; color: black; border: none;">
                                                        <asp:CheckBox ID="CheckBoxAll" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxAll_CheckedChanged" Style="display: none" /></td>
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
                                            <%-- CommandArgument='<%#Eval("BC Code")+","+ Eval("numBCID")%>'--%>
                                            <asp:ImageButton ID="btnView" runat="server" ImageUrl="../../images/edit.png" Width="20px" Height="20px"
                                                ToolTip="Click here to verify" OnClick="btnView_Click" CommandArgument='<%#Eval("BC ID")+"="+Eval("Email")+"="+Eval("Mobile No")+"="+Eval("Activity Type")+"="+Eval("BC Code")%>' data-toggle="modal" data-target="#myModal" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BC ID" HeaderText="BC ID" Visible="false" />
                                    <asp:BoundField DataField="BC Code" HeaderText="Code" />
                                    <asp:BoundField DataField="BC Name" HeaderText="Name" />
                                    <asp:BoundField DataField="Mobile No" HeaderText="Mobile No." />
                                    <asp:BoundField DataField="Email" HeaderText="Email Id" />
                                    <asp:BoundField DataField="State" HeaderText="State" />
                                    <asp:BoundField DataField="District" HeaderText="District" />
                                    <asp:BoundField DataField="City" HeaderText="City" />
                                    <asp:BoundField DataField="PinCode" HeaderText="Pin Code" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Onboarded By" />
                                    <asp:BoundField DataField="CreatedOn" HeaderText="Onboarded On" />
                                    <asp:BoundField DataField="Status" HeaderText="Verification Status" />
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
            </div>

            <div class="modal fade" id="ModalBC" aria-hidden="true" aria-labelledby="ModalBC"
                role="dialog" tabindex="-1">
                <div class="modal-dialog" style="width: 800px; padding-left: 10px; margin-left: 25%; height: 425px">
                    <div class="modal-content" style="width: 800px; padding-left: 9px; height: 481px">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>--%>
                            <h4 class="modal-title" id="exampleModalTitle">BC Documents</h4>
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
                                                <label class="selectInputLabel" for="exampleInputEmail1">BC Name: </label>
                                                <asp:TextBox ID="txtBCName" CssClass="form-control" Width="100%" Font-Size="13px" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col">
                                                <label class="selectInputLabel" for="exampleInputEmail1">Contact Number: </label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtContactNo" Width="100%" Font-Size="13px" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div class="panel-body">
                                    <div class="form-group" style="margin: -32px; height: 100px;">
                                        <center>
                                        <div class="whos-speaking-area speakers pad100">
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
                                                                        <asp:ImageButton ID="btnViewDownload" ImageUrl="../../images/download.png" runat="server" Width="36px" Height="36px" Style="margin-left: -41px; margin-top: 9px;" OnClick="btnViewDownload_Click" CommandArgument='<%#Eval("BCid") %>' />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px; padding-bottom: 10px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-download"></i></button>--%>

                                                                        <%--<a href="#"><i class="fa fa-download"></i></a>--%>
                                                                    </li>
                                                                    <li>
                                                                        <i><img  src="../../images/eyeview.png" style="width:36px; height:36px; margin-left: -47px; margin-top: -31px; font-size:35px;" onclick="SetValue('IdProof');"></i>
                                                                       <%-- <i class="fa fa-eye" id="IDen"  runat="server" style="width:49px; height:36px; margin-left: -43px; margin-top: 9px; font-size:35px;" onclick="SetValue('IdProof');"></i>--%>
                                                                       <i><%--<img  src="../../images/eyeview.png" style="width:36px; height:36px; margin-left: -47px; margin-top: -31px; font-size:35px;" onclick="SetValue('IdProof');">--%></i>
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
                                                                         <asp:ImageButton ID="ImageButton2"  ImageUrl="../../images/download.png" runat="server" Width="36px" Height="36px" style="margin-left: -41px; margin-top: 9px;"  OnClick="ImageButton1_Click" CommandArgument='<%#Eval("BCid") %>' />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px; padding-bottom: 10px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-download"></i></button>--%>
                                                                    </li>
                                                                    <li>
                                                                      <i><img  src="../../images/eyeview.png" style="width:36px; height:36px; margin-left: -47px; margin-top: -31px; font-size:35px;" onclick="SetValue('AddProof');"></i>
                                                                        <%--  <i class="fa fa-eye" id="IDAdd"  runat="server" style="width:49px; height:36px; margin-left: -43px; margin-top: 9px; font-size:35px;" onclick="SetValue('AddProof');"></i>                                                                 --%>
                                                                        <%--<asp:ImageButton ID="ImageButton1" ImageUrl="../../images/eyeview.png" runat="server" Width="36px" Height="36px" Style="margin-left: -43px; margin-top: 9px;" OnClientClick="SetValue('AddProof');"  />--%>  <%--OnClick="EyeImage_Click"--%>

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
                                                                         <asp:ImageButton ID="ImageButton4"  ImageUrl="../../images/download.png" runat="server" Width="36px" Height="36px" style="margin-left: -41px; margin-top: 9px;"  OnClick="imgbtnform_Click" CommandArgument='<%#Eval("BCid") %>' />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px; padding-bottom: 10px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-download"></i></button>--%>
                                                                        <%--  <a href="#"><i class="fa fa-download"></i></a>--%>
                                                                    </li>
                                                                    <li>
                                                                        <%--<asp:ImageButton ID="EyeImage3" ImageUrl="../../images/View.png" runat="server" Width="49px" Height="36px" ToolTip="View Doc" Style="margin-left: -43px; margin-top: 9px;" OnClick="EyeImage3_Click" CommandArgument='<%#Eval("BCid") %>' />--%>
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
                                            <div class="col-md-3 col-xm-12" style="width: 20%;">
                                            </div>
                                            <div class="col-md-3 col-xm-12" style="margin-bottom: 2%; width: 20%;">
                                                <label class="selectInputLabel" for="exampleInputEmail1">Action: </label>
                                                <asp:RadioButtonList ID="rdbtnApproveDecline" CssClass="rbl" runat="server" RepeatLayout="Table">
                                                    <asp:ListItem Value="Approve">Approve   &nbsp;&nbsp;&nbsp;&nbsp; </asp:ListItem>
                                                    <asp:ListItem Value="Decline">Decline</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="None" CssClass="err" ErrorMessage="Please select Decision" ControlToValidate="rdbtnApproveDecline" ValidationGroup="Veri" runat="server"></asp:RequiredFieldValidator>
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
                                <div class="col-md-3 offset-5 col-xm-12" style="margin-left: 220px; width: 40%;">
                                    <asp:Button runat="server" ID="btnSubmitDetails" ValidationGroup="Veri" OnClick="btnSubmitDetails_Click" class="themeBtn themeApplyBtn" Text="Submit" OnClientClick="Confirm();CloseModal();"></asp:Button>
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

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnViewDownload" />
            <%-- <asp:PostBackTrigger ControlID="EyeImage" />--%>
            <%-- <asp:PostBackTrigger ControlID="EyeImage3" />--%>
            <%-- <asp:PostBackTrigger ControlID="EyeImage1" />--%>
            <asp:PostBackTrigger ControlID="btnSubmitDetails" />
            <asp:PostBackTrigger ControlID="ImageButton2" />
            <asp:PostBackTrigger ControlID="ImageButton4" />
            <asp:PostBackTrigger ControlID="BtnCsv" />
            <asp:PostBackTrigger ControlID="BtnXls" />
        </Triggers>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" BackgroundCssClass="modalBackground" DropShadow="false" />

    <script>
        $('.maximus-select').select2({
            placeholder: "All",
        });
    </script>
    <script type="text/javascript">
        function SetValue(myval) {

            //var surl = "http://localhost:63840/pdfhandler.aspx?value="+myval+"";
            window.open("/pdfhandler.aspx?value=" + myval + "");
        }
    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlClientCode.ClientID%>").select2();
                    $("#<%=ddlBCCode.ClientID%>").select2();
                    $("#<%=ddlActivityType.ClientID%>").select2();
                }
            });
        };
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
