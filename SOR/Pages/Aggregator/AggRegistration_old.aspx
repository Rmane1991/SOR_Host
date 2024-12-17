<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="AggRegistration.aspx.cs" Inherits="SOR.Pages.Aggregator.AggRegistration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/aes.js"></script>
    <script src="../../Scripts/jquery-1.4.1-vsdoc.js"></script>
    <script src="../../Scripts/executeSec.js"></script>
    <script src="../../assets1/multi/jquery.min.js"></script>
    <style>
        span {
            font-size: 12px;
        }

        .err {
            color: #fe1a03;
        }

        .selectGrid-m-z {
            /* margin-top: 8px; */
            margin-bottom: 1px;
        }

        .col-md-99 {
            flex: -15 0 auto;
            width: 66%;
            margin-left: 49px;
        }

        .col-sm-77 {
            flex: 0 0 auto;
            width: 68%;
        }

        .select2 {
            width: 100% !important;
        }

        .col1 {
            width: 40%;
        }

        .col2 {
            width: 15%;
        }

        .col3 {
            width: 32%;
        }

        .col4 {
            width: 38%;
        }

        label {
            height: 20px;
        }
    </style>

    <style>
        .col-lg-3 {
            width: 20%;
        }

        .mb50 {
            margin-left: 277px;
        }
    </style>
    <style>
        .spanbtn {
            padding-top: 60px;
        }
    </style>
    <style>
        .iconButtonBox {
            margin-bottom: -11px;
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

    <style>
        #ctl00_cpbdCarde_gvsummary {
            width: 100% !Important;
        }

        .adjustMarginTB {
            margin: 10px 0px 0px 0px;
            height: 30px;
        }

        .select2 {
            width: 100% !important;
        }

        .flex {
            width: 100% !important;
        }

        fieldset.scheduler-border {
            border: solid 1px lightgray !important;
            border-radius: 5px;
            padding: 0 0px 10px 5px;
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
            color: #1E3C6E;
            font-weight: bold;
        }

        /*Calendar Control CSS*/
        .cal_Theme1 .ajax__calendar_container {
            background-color: #AFC7F7;
            border: solid 1px #0c2d50;
            z-index: 999;
        }

        .cal_Theme1 .ajax__calendar_header {
            background-color: #ffffff;
            margin-bottom: 4px;
        }

        .cal_Theme1 .ajax__calendar_title, .cal_Theme1 .ajax__calendar_next, .cal_Theme1 .ajax__calendar_prev {
            color: #004080;
            padding-top: 1px;
        }

        .cal_Theme1 .ajax__calendar_body {
            background-color: #ffffff;
            border: solid 1px black;
            height: 130px;
            width: 160px;
        }

        .cal_Theme1 .ajax__calendar_dayname {
            text-align: center;
            font-weight: bold;
            margin-bottom: 4px;
            margin-top: 2px;
            color: #004080;
        }

        .cal_Theme1 .ajax__calendar_day {
            color: #004080;
            text-align: center;
        }

        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_day, .cal_Theme1 .ajax__calendar_hover .ajax__calendar_month, .cal_Theme1 .ajax__calendar_hover .ajax__calendar_year, .cal_Theme1 .ajax__calendar_active {
            color: #004080;
            font-weight: bold;
            background-color: #DEF1F4;
        }

        .cal_Theme1 .ajax__calendar_today {
            font-weight: bold;
        }

        .cal_Theme1 .ajax__calendar_other, .cal_Theme1 .ajax__calendar_hover .ajax__calendar_today, .cal_Theme1 .ajax__calendar_hover .ajax__calendar_title {
            color: #bbbbbb;
        }
    </style>

    <style type="text/css">
        .radioButtonList input {
            display: inline-block;
            margin-right: 1em;
        }

        .radioButtonList label {
            display: inline-block;
            margin-right: 1.25em;
        }
    </style>
    <script type="text/javascript">
        var updateProgress = null;

        function funShowOnboardDiv() {
            $("#divMainDetailsGrid").hide();
            $("#divlogoupdate").hide();
            $("#divOnboardFranchise").show();
            $("#divAction").hide();

        }
        function funShowgridDiv() {
            $("#divMainDetailsGrid").show();
            $("#divOnboardFranchise").hide();
            $("#divlogoupdate").hide();
            $("#divAction").show();
        }
        function funshowupdate() {
            $("#divMainDetailsGrid").hide();
            $("#divOnboardFranchise").hide();
            $("#divlogoupdate").show();
            $("#divAction").hide();
        }

        //function hidealldiv() {
        //      $("#divMainDetailsGrid").hide();
        //    $("#divOnboardFranchise").hide();
        //    $("#divlogoupdate").hide();
        //    $("#divAction").show();
        //}

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
        function Confirm1() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Upload Your Data?")) {
                debugger;
                document.getElementById("<%= hdnUploadConfirmation.ClientID %>").value = "Yes";
        }
        else {
            document.getElementById("<%= hdnUploadConfirmation.ClientID %>").value = "No";
            }

            //document.forms[0].appendChild(confirm_value);
        }
        function Confirm3() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Save Your Data?")) {
                debugger;
                document.getElementById("<%= hdnFinalConfirmation.ClientID %>").value = "Yes";
        }
        else {
            document.getElementById("<%= hdnFinalConfirmation.ClientID %>").value = "No";
            }

            //document.forms[0].appendChild(confirm_value);
        }

        function Confirm2() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Delete Data?")) {
                debugger;
                document.getElementById("<%= hdnDeleteConfirmation.ClientID %>").value = "Yes";
        }
        else {
            document.getElementById("<%= hdnDeleteConfirmation.ClientID %>").value = "No";
            }

            //document.forms[0].appendChild(confirm_value);
        }

        function FunImageFIllLogo(input) {
            debugger;
            var fileType = $('#MainContent_flgUplodMyImage').val().split('.').pop().toLowerCase();
            var flgUplodMyImage = document.getElementById('MainContent_flgUplodMyImage');

            // var fileType = input.value.split('.').pop().toLowerCase();
            var count = flgUplodMyImage.Filename.Split('.').Length - 1;
            if (fileType == 'jpg' || fileType == 'jpeg' || fileType == 'png') {
                if (input.files[0].size > 500000)//500 kb
                {
                    alert('File size should not exceed 500 KB in Attachments');
                    input.value = null;
                    return;
                }

                else if (count > '0') {
                    alert("upload Valid file");
                }

                else if (input.files && input.files[0]) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#MainContent_imageupload').attr('src', e.target.result);
                    }
                    reader.readAsDataURL(input.files[0]);
                }
            }
            else {
                alert("File type should be only in jpg, jpeg and png  format");
                input.value = null;
                return;
            }
        }
        function FunImageUpdateLogo(input) {
            debugger;
            var fileType = $('#MainContent_filelogo').val().split('.').pop().toLowerCase();
            var filelogo = document.getElementById('MainContent_filelogo');

            // var fileType = input.value.split('.').pop().toLowerCase();
            var count = filelogo.Filename.Split('.').Length - 1;
            if (fileType == 'jpg' || fileType == 'jpeg' || fileType == 'png') {
                if (input.files[0].size > 500000)//500 kb
                {
                    alert('File size should not exceed 500 KB in Attachments');
                    input.value = null;
                    return;
                }

                else if (count > '0') {
                    alert("upload Valid file");
                }

                else if (input.files && input.files[0]) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#MainContent_imageupload').attr('src', e.target.result);
                    }

                    reader.readAsDataURL(input.files[0]);
                }
            }
            else {
                alert("File type should be only in jpg, jpeg and png  format");
                input.value = null;
                return;
            }
        }


        function Disable() {
            document.getElementById("MainContent_btnSubmitDetails").disabled = true;
        }

        function Clear(e) {
            e.preventDefault();
            document.getElementById("<%=txtFirstName.ClientID %>").value = '';

        document.getElementById("<%=txtMiddleName.ClientID %>").value = '';
        document.getElementById("<%=txtLastName.ClientID %>").value = '';

        document.getElementById("<%=txtEmailID.ClientID %>").value = '';
        document.getElementById("<%=txtPinCode.ClientID %>").value = '';


        document.getElementById("<%=txtRegisteredAddress.ClientID %>").value = '';

        document.getElementById("<%=txtContactNo.ClientID %>").value = '';
        document.getElementById("<%=txtLandlineNo.ClientID %>").value = '';


        <%--document.getElementById("<%=txtDistrict.ClientID %>").value = '';
        document.getElementById("<%=txtshopDistrict.ClientID %>").value = '';--%>


        document.getElementById("<%=txtLandlineNo.ClientID %>").value = '';

        document.getElementById("<%=ddlCity.ClientID %>").selectedIndex = 0;
        document.getElementById("<%=ddlCountry.ClientID %>").selectedIndex = 0;
        document.getElementById("<%=ddlState.ClientID %>").selectedIndex = 0;

        document.getElementById("<%=txtAlterNateNo.ClientID %>").value = '';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
    </asp:Panel>
    <div style="clear: both;" />
    <asp:HiddenField ID="hidPan" runat="server" />
    <asp:HiddenField ID="hidAadh" runat="server" />
    <asp:HiddenField ID="hidSgst" runat="server" />
    <asp:HiddenField ID="hidAccIFC" runat="server" />
    <asp:HiddenField ID="hidAccNo" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:HiddenField ID="HiddenField3" runat="server" />
    <asp:HiddenField ID="hdnDeleteConfirmation" runat="server" />
    <asp:HiddenField ID="HidBCID" runat="server" />
    <asp:HiddenField ID="hdnUserConfirmation" runat="server" />
    <asp:HiddenField ID="hdnUploadConfirmation" runat="server" />
    <asp:HiddenField ID="hdnFinalConfirmation" runat="server" />
    <div class="container-fluid">
        <div class="breadHeader">
            <h5 class="page-title">Aggregator Registration</h5>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div id="divAction" runat="server">

                    <div class="accordion summary-accordion" id="history-accordionss">
                        <div class="accordion-item">
                            <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOns">
                                <h6 class="searchHeader-heading"></h6>
                                <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOns"
                                    aria-expanded="true" aria-controls="collapseOne">
                                    <span class="icon-hide"></span>
                                    <p>Show / Hide</p>
                                </button>
                            </div>
                            <div id="collapseSummaryOns" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                data-bs-parent="#summary-accordion">
                                <div class="accordion-body">
                                    <hr class="hr-line">

                                    <span class="spanbtn">
                                        <asp:Button ID="btnAddnew" runat="server" Text="Add New" CssClass="themeBtn themeApplyBtn" Width="102px" Height="32px" OnClick="btnAddnew_Click" />
                                    </span>
                                    <asp:ImageButton ID="btnExportCSV" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                        ToolTip="Csv" data-toggle="modal" data-target="#myModal" OnClick="btnExportCSV_Click" />
                                    <%--OnClick="btnExportCSV_ServerClick"--%>
                                    <asp:ImageButton ID="btndownload" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                        ToolTip="Xls" data-toggle="modal" data-target="#myModal" OnClick="btndownload_Click" />
                                    <%--OnClick="btndownload_ServerClick"--%>
                                    <hr class="hr-line">
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">RequestType</label>
                                            <asp:DropDownList ID="ddlRequestType" runat="server" CssClass="maximus-select w-100" Width="100%" AutoPostBack="false">
                                                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Registration" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Activation" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="De-activation" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Termination" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col" style="display: none">
                                            <label class="selectInputLabel" for="selectInputLabel">Bucket</label>
                                            <asp:DropDownList ID="ddlBucketId" runat="server" CssClass="maximus-select w-100" Width="100%" AutoPostBack="false">
                                                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Self" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Level -1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Level -2" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Level -3" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">RequestStatus</label>
                                            <asp:DropDownList ID="ddlRequestStatus" runat="server" CssClass="maximus-select w-100" Width="100%" AutoPostBack="false">
                                                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Declined" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <%-- <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">PanNo</label>
                                            <div class="inputBox w-100">
                                                <input type="text" id="txtPanNoF" runat="server" class="input-text form-control" style="width: 100%" placeholder="Pan No" />
                                            </div>
                                        </div>--%>
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">PanNo</label>
                                            <div class="inputBox w-100">
                                                <%--<input type="text" id="txtPanNoF" runat="server" class="input-text form-control" PlaceHolder="Enter 10 digit PAN No." Width="100%" MaxLength="10" />--%>
                                                <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtPanNoF" PlaceHolder="Enter 10 digit PAN No." Width="100%" MaxLength="10"></asp:TextBox>
                                                <asp:HiddenField ID="hd_txtPanNoF" runat="server" Value="1" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="FranchiseReg" runat="server" Display="None" ErrorMessage="Provided PAN No is not valid! Please enter PAN No start with 'eg.ABCDE1234K'." ControlToValidate="txtPanNoF" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}" />
                                                <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender52" runat="server" FilterType="UppercaseLetters,Numbers" TargetControlID="txtPanNoF" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                        <div>
                                            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" class="themeBtn themeApplyBtn" Text="Search"></asp:Button>
                                            <asp:Button ID="butnCancel" runat="server" CausesValidation="false" class="themeBtn resetBtn themeCancelBtn me-0" Text="Clear" OnClick="butnCancel_Click"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                </div>
                <div id="divMainDetailsGrid" runat="server">
                    <asp:UpdatePanel ID="updCommen" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidFranchiseID" runat="server" />
                            <asp:HiddenField ID="hidEmailId" runat="server" />
                            <asp:HiddenField ID="hidCashLimit" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Panel ID="panelGrid" runat="server" HorizontalScroll="false" ScrollBars="None" Style="padding: 5px 10px 0px 0px;">
                        <div class="form-group row">
                            <div class="tableBorderBox HeaderStyle" style="width: 100%; padding: 10px 10px; overflow: scroll; max-height: 400px;">
                                <div class="table-box">
                                    <asp:GridView ID="gvBCOnboard" runat="server"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        AllowPaging="true"
                                        CssClass="GridView"
                                        Visible="true"
                                        PagerSettings-Mode="NumericFirstLast"
                                        PagerSettings-FirstPageText="First Page"
                                        PagerSettings-LastPageText="Last Page"
                                        OnRowCommand="gvBCOnboard_RowCommand"
                                        DataKeyNames="Bucket,Request Status,Request Type,ActivityType,BCReqId"
                                        OnPageIndexChanging="gvBCOnboard_PageIndexChanging"
                                        OnRowDataBound="gvBCOnboard_RowDataBound">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                        <RowStyle Wrap="false" />
                                        <AlternatingRowStyle BackColor="#F1FCEE" />
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
                                                    <%-- CommandArgument='<%#Eval("BC Code")+","+ Eval("numBCID")%>'--%>
                                                    <asp:ImageButton ID="btnView" runat="server" ImageUrl="../../images/Edit-01-512.png" Width="20px" Height="20px" CommandName="EditDetails"
                                                        ToolTip="Click Here To Edit Data" CommandArgument='<%#Eval("BCReqId")+"="+Eval("Bucket")%>' data-toggle="modal" data-target="#ModalBC" />
                                                    <%--OnClick="btnView_Click"--%>
                                                    <asp:ImageButton ID="btndelete" runat="server" ImageUrl="../../images/document-delete.png" OnClientClick="Confirm2();" Width="20px" Height="20px" CommandName="DeleteDetails"
                                                        ToolTip="Click Here To Delete Data" data-toggle="modal" data-target="#ModalBC" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--   <asp:BoundField DataField="Sr.No." HeaderText="Sr.No." />--%>
                                            <asp:BoundField DataField="BCCode" HeaderText="BCCode" />
                                            <asp:BoundField DataField="BCReqId" HeaderText="BC ID" />
                                            <asp:BoundField DataField="BCName" HeaderText="BCName" />
                                            <asp:BoundField DataField="BCAddress" HeaderText="BCAddress" />
                                            <asp:BoundField DataField="ContactNo" HeaderText="ContactNo" />
                                            <asp:BoundField DataField="Request Type" HeaderText="Request Type" />
                                            <asp:BoundField DataField="Bucket" HeaderText="Bucket" />
                                            <asp:BoundField DataField="Request Status" HeaderText="Request Status" />
                                            <asp:BoundField DataField="ActivityType" HeaderText="ActivityType" Visible="false" />
                                            <asp:BoundField DataField="Onboarding Status" HeaderText="Onboarding Status" />
                                        </Columns>
                                        <HeaderStyle BackColor="#8DCCF6" ForeColor="#3D62B6" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>



                <div id="DIVDetails" runat="server" visible="false">
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

                                                    <div class="col">
                                                        <label class="exampleInputEmail1" for="selectInputLabel">Business Correspondence:<span class="err">*</span></label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" ControlToValidate="ddlbcCode" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select BC."></asp:RequiredFieldValidator>

                                                        <div class="selectInputDateBox w-100">
                                                            <asp:DropDownList ID="ddlbcCode" runat="server" CssClass="maximus-select w-100" Width="100%" AutoPostBack="true">
                                                                <asp:ListItem Value="0">--Business Correspondents--</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                    <div class="col" id="Div_Chk" runat="server" style="width: 800px">
                                                        <asp:Label ID="lblServicesOffer" runat="server" Text="Services Offer "></asp:Label><span class="err">*</span><br />

                                                        <asp:CheckBox ID="chkAEPS" runat="server" Style="font-weight: 500" />
                                                        <asp:Label ID="lblchkAEPS" runat="server">AEPS &emsp;</asp:Label>

                                                        <asp:CheckBox ID="chkdmt" runat="server" />
                                                        <asp:Label ID="lblchkdmt" runat="server">DMT &emsp;</asp:Label>

                                                        <asp:CheckBox ID="chkMATM" runat="server" />
                                                        <asp:Label ID="lblchkMATM" runat="server">Micro ATM &emsp;</asp:Label>

                                                    </div>

                                                    <div class="col" style="display: none" runat="server" visible="false">
                                                        <label for="exampleInputEmail1">BC Profile Image  <span class="err">*</span></label>
                                                        <asp:Image ID="imgMyImge" runat="server" Height="110px" Width="110px" BorderStyle="Double" ImageUrl="../../../images/Profile1.png" />
                                                        <asp:FileUpload ID="flgUplodMyImage" Width="120px" runat="server" Style="margin-top: 5px" onchange="return FunImageFIll(this);" CssClass="btn btn-small btn-default" />
                                                        <asp:Label runat="server" ID="lblMessage"></asp:Label>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <label for="exampleInputEmail1">BC Name <span class="err">*</span></label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator0" ControlToValidate="txtFirstName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter first name"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtFirstName" PlaceHolder="First Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                        <asp:HiddenField ID="hd_txtFirstName" runat="server" Value="1" />
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtFirstName" />
                                                    </div>
                                                    <div class="col" runat="server">
                                                        <label for="exampleInputEmail1">Middle Name </label>
                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtMiddleName" PlaceHolder="Middle Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtMiddleName" />
                                                    </div>
                                                    <div class="col" runat="server">
                                                        <label for="exampleInputEmail1">Last Name </label>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtLastName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter last name"></asp:RequiredFieldValidator>--%>
                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtLastName" PlaceHolder="Last Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtLastName" />
                                                    </div>
                                                    <div class="col">
                                                        <label for="exampleInputEmail1">Father Name </label>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldlValidator2" ControlToValidate="txtFatherName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Father name"></asp:RequiredFieldValidator>--%>
                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtFatherName" PlaceHolder="Father Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtFatherName" />

                                                    </div>
                                                    <div class="col">
                                                        <label for="exampleInputEmail1">Gender </label>
                                                        <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlGender" Width="100%">
                                                            <asp:ListItem Value="Male">Male</asp:ListItem>
                                                            <asp:ListItem Value="Female">Female</asp:ListItem>
                                                            <asp:ListItem Value="Transgender">Transgender</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col" runat="server">
                                                        <label for="exampleInputEmail1">Company Name <span class="err">*</span></label>
                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtCompanyName" PlaceHolder="Company Name" Width="100%" MaxLength="90"></asp:TextBox>
                                                        <asp:HiddenField ID="hd_txtCompanyName" runat="server" Value="1" />
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtMiddleName" />
                                                        <asp:RequiredFieldValidator ID="requCompnyName" ControlToValidate="txtCompanyName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Company Name "></asp:RequiredFieldValidator>
                                                        <Ajax:FilteredTextBoxExtender ID="filteredtxtCompanyName" runat="server" FilterType="Custom,Numbers,LowercaseLetters,UppercaseLetters" TargetControlID="txtCompanyName" ValidChars=" .," />
                                                    </div>
                                                    <div class="col">
                                                            <label for="exampleInputEmail1">Personal Email Id <span class="err">*</span></label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtEmailID" PlaceHolder="Email ID" Width="100%" MaxLength="50"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ControlToValidate="txtEmailID" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Personal email id"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="validateEmail" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Please enter valid Personal email id" ControlToValidate="txtEmailID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Numbers,UppercaseLetters, LowercaseLetters, Custom" ValidChars=".@!#$%^&*()_,/\-" TargetControlID="txtEmailID" />
                                                        </div>
                                                    <div class="col" id="dvfield_PANNo" runat="server" style="display: normal;">
                                                        <label for="exampleInputEmail1">PAN No. <span class="err">*</span></label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ControlToValidate="txtPANNo" Style="display: none" runat="server" CssClass="err" ValidationGroup="FranchiseReg" ErrorMessage="Please enter pan no"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtPANNo" PlaceHolder="Enter 10 digit PAN No." Width="100%" MaxLength="10"></asp:TextBox>
                                                        <asp:HiddenField ID="hd_txtPANNo" runat="server" Value="1" />
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationGroup="FranchiseReg" runat="server" Display="None" ErrorMessage="Provided PAN No is not valid! Please enter PAN No start with 'eg.ABCDE1234K'." ControlToValidate="txtPANNo" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}" />
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="UppercaseLetters,Numbers, LowercaseLetters" TargetControlID="txtPANNo" />
                                                    </div>
                                                    <div class="col">
                                                        <label for="exampleInputEmail1">GST No. </label>
                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtGSTNo" MaxLength="15" Width="100%" PlaceHolder="Enter 15 digit GST no."></asp:TextBox>
                                                        <asp:HiddenField ID="hd_txtGSTNo" runat="server" Value="1" />
                                                    </div>

                                                    <div class="col">
                                                        <label for="exampleInputEmail1">Aadhaar card No. <span class="err">*</span></label>
                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtaadharno" Width="100%" PlaceHolder="Enter 12 digit Aadhaarcard No." MaxLength="12" onkeypress="return isNumber(event)"></asp:TextBox>
                                                        <asp:HiddenField ID="hd_txtaadharno" runat="server" Value="1" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator41" ControlToValidate="txtaadharno" Display="None" runat="server" CssClass="err" ValidationGroup="FranchiseReg" ErrorMessage="Please enter Aadhaarcard no"></asp:RequiredFieldValidator>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" runat="server" FilterType="Numbers" TargetControlID="txtaadharno" />
                                                    </div>
                                                </div>
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Aggregator DOB <span class="err">*</span> </label>
                                                        <div class="selectInputDateBox w-100">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtdob" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please Select AgentDOB"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtdob" runat="server" Width="100%" CssClass="input-text form-control" TextMode="Date" onchange="javascript:ValidateDob();"
                                                                oncopy="return false;" oncut="return false;" onkeypress="return block(event);"
                                                                onpaste="return false;"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col">
                                                        <label for="exampleInputEmail1">Spouse Name </label>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtspousename" PlaceHolder="Spouse Name" MaxLength="20"></asp:TextBox>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" runat="server" FilterType="UppercaseLetters, LowercaseLetters" TargetControlID="txtspousename" />
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Category <span class="err">*</span> </label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlCategory">
                                                            <asp:ListItem Value="General">General</asp:ListItem>
                                                            <asp:ListItem Value="OBC">OBC</asp:ListItem>
                                                            <asp:ListItem Value="SC">SC</asp:ListItem>
                                                            <asp:ListItem Value="ST">ST</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Physically Handicapped <span class="err">*</span> </label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlPhysicallyHandicapped">
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Edu. Qualification <span class="err">*</span> </label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlHighestEducationQualification">
                                                            <asp:ListItem Value="Under 10th">Under 10th</asp:ListItem>
                                                            <asp:ListItem Value="10th">10th</asp:ListItem>
                                                            <asp:ListItem Value="12th">12th</asp:ListItem>
                                                            <asp:ListItem Value="Graduate">Graduate</asp:ListItem>
                                                            <asp:ListItem Value="Post Graduate">Post Graduate</asp:ListItem>
                                                            <asp:ListItem Value="Diploma Qualification">Diploma Qualification</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Course<span class="err">*</span> </label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlCourse">
                                                            <asp:ListItem Value="None">None</asp:ListItem>
                                                            <asp:ListItem Value="IIBF Advance">IIBF Advance</asp:ListItem>
                                                            <asp:ListItem Value="IIBF Basic">IIBF Basic</asp:ListItem>
                                                            <asp:ListItem Value="Certified By Bank">Certified By Bank</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Institute Name<span class="err">*</span> </label>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtInstituteName" PlaceHolder="Institute  Name" MaxLength="45"></asp:TextBox>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender32" runat="server" FilterType="UppercaseLetters, LowercaseLetters,Custom" ValidChars=" " TargetControlID="txtInstituteName" />
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Date of Passing</label>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtDateofPassing" PlaceHolder="Date of Passing" MaxLength="12"></asp:TextBox>
                                                        <Ajax:CalendarExtender ID="CalendarExtender2" CssClass="cal_Theme1" Animated="true" PopupPosition="BottomRight" TargetControlID="txtDateofPassing" Enabled="true" PopupButtonID="txtDateofPassing" Format="dd/MM/yyyy" runat="server"></Ajax:CalendarExtender>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Alternate Occupation Type</label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlAlternateOccupationType">
                                                            <asp:ListItem Value="Government">Government</asp:ListItem>
                                                            <asp:ListItem Value="Public Sector">Public Sector</asp:ListItem>
                                                            <asp:ListItem Value="Self Employed">Self Employed</asp:ListItem>
                                                            <asp:ListItem Value="Private">Private</asp:ListItem>
                                                            <asp:ListItem Value="Others">Others</asp:ListItem>
                                                            <asp:ListItem Value="None">None</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Alternate Occupation Description</label>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtAlternateOccupationDescription" PlaceHolder="Alternate Occupation Description" MaxLength="45"></asp:TextBox>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender33" runat="server" FilterType="UppercaseLetters, LowercaseLetters,Custom" ValidChars=" " TargetControlID="txtAlternateOccupationDescription" />
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Shop Opens at</label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlShopOpensat">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Shop Closes at </label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlClosesat">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Device Name <span class="err">*</span> </label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlDeviceName">
                                                            <asp:ListItem Value="Hand Held">Hand Held</asp:ListItem>
                                                            <asp:ListItem Value="Laptop">Laptop</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Connectivity Type<span class="err">*</span> </label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlConnectivityType">
                                                            <asp:ListItem Value="Land Line">Land Line</asp:ListItem>
                                                            <asp:ListItem Value="Mobile">Mobile</asp:ListItem>
                                                            <asp:ListItem Value="VSAT">VSAT</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Bank Reference Number </label>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtprovider" PlaceHolder="eg. Airtel" MaxLength="20"></asp:TextBox>
                                                    </div>

                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Corporate/Individual BC</label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlCorporateIndividualBC">
                                                            <asp:ListItem Value="0">CorporateBC</asp:ListItem>
                                                            <asp:ListItem Value="1">IndividualBC</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>


                                                    <div class="col-md-6 col-xm-12">
                                                        <label class="selectInputLabel" for="selectInputLabel">Weekly Off</label>
                                                        <asp:CheckBoxList runat="server" class="checkbox-default" ID="chkweekoff" RepeatDirection="Horizontal" Font-Bold="true" SelectionMode="Multiple">
                                                            <asp:ListItem Value="none">None</asp:ListItem>
                                                            <asp:ListItem Value="Sunday#">Sunday</asp:ListItem>
                                                            <asp:ListItem Value="Monday#">Monday</asp:ListItem>
                                                            <asp:ListItem Value="Tuesday#">Tuesday</asp:ListItem>
                                                            <asp:ListItem Value="Wednesday#">Wednesday</asp:ListItem>
                                                            <asp:ListItem Value="Thursday#">Thursday</asp:ListItem>
                                                            <asp:ListItem Value="Friday#">Friday</asp:ListItem>
                                                            <asp:ListItem Value="Saturday#">Saturday</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
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
                                            <h6 class="searchHeader-heading">Account Details</h6>
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
                                                    <div class="col">
                                                        <label for="exampleInputAccountNumber">Account Number <span class="err">*</span></label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator36" ControlToValidate="txtAccountNumber" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter Account Number"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtAccountNumber" PlaceHolder="Account Number" MaxLength="16"></asp:TextBox>
                                                        <asp:HiddenField ID="hd_txtAccountNumber" runat="server" Value="1" />
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" FilterType="Numbers" TargetControlID="txtAccountNumber" />
                                                    </div>
                                                    <div class="col">
                                                        <label for="exampleInputIFSCode">IFSC Code<span class="err">*</span></label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator35" ControlToValidate="txtIFsccode" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter IFSC Code"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtIFsccode" PlaceHolder="IFSC Code" MaxLength="11"></asp:TextBox>
                                                        <asp:HiddenField ID="hd_txtIFsccode" runat="server" Value="1" />
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" FilterType="UppercaseLetters,Numbers" TargetControlID="txtIFsccode" />
                                                    </div>
                                                    <div class="col">
                                                        <label for="exampleInputNoOfTransaction">No of transactions per day <span class="err">*</span></label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator37" ControlToValidate="txtNoOfTransactions" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter No of transactions Per day"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtNoOfTransactions" PlaceHolder="No of transactions per day" MaxLength="3"></asp:TextBox>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" FilterType="Numbers" TargetControlID="txtNoOfTransactions" />
                                                    </div>
                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">Transfer Amount per day <span class="err">*</span></label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator38" ControlToValidate="txttransferAmountPerday" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Tarnsfer Amount per day"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" class="form-control" ID="txttransferAmountPerday" PlaceHolder="Tarnsfer Amount per day" MaxLength="5"></asp:TextBox>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" FilterType="Numbers" TargetControlID="txttransferAmountPerday" />
                                                    </div>
                                                    <div class="col">
                                                        <label for="exampleInputEmail1">Aggregator Wallet Transfer<span class="err">*</span></label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlFranchiseWalletTransfer">
                                                            <asp:ListItem Value="0">Dis-Allowed</asp:ListItem>
                                                            <asp:ListItem Value="1">Allowed</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">Aggregator Account Name<span class="err">*</span></label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" ControlToValidate="txtFranchiseAccountName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Franchise AccountName"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtFranchiseAccountName" PlaceHolder="Franchise Account Name" MaxLength="60"></asp:TextBox>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Custom" TargetControlID="txtFranchiseAccountName" ValidChars=" " />
                                                    </div>
                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">IsUPI Partner</label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlIsUPIPartner">
                                                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                            <asp:ListItem Value="No">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">Wallet Loading/Withdrawal</label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlwalletLoadingWithdrawl">
                                                            <asp:ListItem Value="0">Both</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">BC-Aggregator Type</label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlBcFranchisetype">
                                                            <asp:ListItem Value="1">Merchant Transactions</asp:ListItem>
                                                            <asp:ListItem Value="0">Remittance(BC-Franchise)</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">Parent Flag</label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlParentFlag">
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">Parent Aggregator ID</label>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtFranchiseParentID" PlaceHolder="Aggreagator Parent ID" MaxLength="5"></asp:TextBox>
                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" FilterType="Numbers" TargetControlID="txtFranchiseParentID" />
                                                    </div>
                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">Registration Type</label>
                                                        <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddlRegistrationType">
                                                            <asp:ListItem Value="New">New</asp:ListItem>
                                                            <asp:ListItem Value="Migrant">Migrant</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <%-- <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                            <ContentTemplate>--%>
                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">Type Of Commission<span class="err">*</span></label>
                                                        <asp:DropDownList ID="ddlTypeOfCommission" runat="server" CssClass="maximus-select w-100" AutoPostBack="false"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator45" ControlToValidate="ddlTypeOfCommission" Style="display: none" InitialValue="0" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select Type Of Commsssion"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <%-- </ContentTemplate>
                                                        </asp:UpdatePanel>--%>

                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">Threshold Amount<span class="err">*</span></label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator46" ControlToValidate="txtThresholdAmt" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Threshold Amount"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" class="form-control" ID="txtThresholdAmt" PlaceHolder="Threshold Amount" autocomplete="off" MaxLength="6" onkeypress="return isNumber(event)"></asp:TextBox>
                                                        <asp:HiddenField ID="hd_txtThresholdAmt" runat="server" Value="1" />
                                                    </div>

                                                    <div class="col" id="Div2" runat="server">
                                                        <label>Alert Configuration <span class="err">*</span></label><br />
                                                        <asp:CheckBox ID="chkEmail" runat="server" Style="font-weight: 500" />
                                                        <label for="lblchkemail" style="font-weight: 500">Email &emsp;</label>
                                                        <asp:CheckBox ID="chkSMS" runat="server" />
                                                        <label for="lblchksms">SMS &emsp;</label>
                                                    </div>

                                                    <div class="col">
                                                        <label for="exampleInputTransferAmount">Configuration Category<span class="err">*</span></label>
                                                        <asp:RadioButtonList ID="RadioConfigCategory" runat="server" CssClass="radioButtonList" ClientIDMode="static"
                                                            RepeatDirection="Horizontal" ToolTip="Select Configuration Category">
                                                            <asp:ListItem Value="1" Selected="True">Unlimited</asp:ListItem>
                                                            <asp:ListItem Value="2">Limited</asp:ListItem>
                                                        </asp:RadioButtonList><%--AutoPostBack="true" OnSelectedIndexChanged="RadioConfigCategory_SelectedIndexChanged"--%>
                                                        <asp:HiddenField ID="hd_RadioConfigCategory" runat="server" Value="1" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
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
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                        <div class="col1">
                                                            <label for="exampleInputEmail1">Registered Address <span class="err">*</span></label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtRegisteredAddress" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter registered address"></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" class="form-control" ID="txtRegisteredAddress" TextMode="MultiLine" Width="100%" PlaceHolder="Registered Address " Style="resize: none" MaxLength="250"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtRegisteredAddress" runat="server" Value="1" />
                                                        </div>
                                                        <!-- input -->
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Pincode <span class="err">*</span></label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" PlaceHolder="eg.400601" Width="100%" ID="txtPinCode" MaxLength="6" AutoPostBack="true" OnTextChanged="txtPinCode_TextChanged"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtPinCode" runat="server" Value="1" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" FilterType="Numbers" TargetControlID="txtPinCode" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtPinCode" ValidationGroup="FranchiseReg" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter pincode"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <!-- input -->
                                                        <div class="col" style="display: none">
                                                            <label for="exampleInputEmail1">Country <span class="err">*</span></label>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" ControlToValidate="ddlCountry" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select country"></asp:RequiredFieldValidator>--%>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlCountry" Width="100%" AutoPostBack="true"></asp:DropDownList>
                                                            <%--OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"--%>
                                                            <asp:HiddenField ID="hd_ddlCountry" runat="server" Value="1" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">State <span class="err">*</span></label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlState" Width="100%" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            <%--OnSelectedIndexChanged="ddlState_SelectedIndexChanged"--%>
                                                            <asp:HiddenField ID="hd_ddlState" runat="server" Value="1" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ControlToValidate="ddlState" Width="100%" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select state"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">District<span class="err">*</span></label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlDistrict" Width="100%" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            <%--<asp:TextBox runat="server" CssClass="input-text form-control" ID="txtDistrict" Width="100%" PlaceHolder="Enter district" MaxLength="25"></asp:TextBox>--%>
                                                            <asp:HiddenField ID="hd_ddlDistrict" runat="server" Value="1" />
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="ddlDistrict" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter district Name "></asp:RequiredFieldValidator>--%>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="ddlDistrict" Width="100%" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select district"></asp:RequiredFieldValidator>
                                                            <%--<Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="LowercaseLetters, UppercaseLetters" TargetControlID="ddlDistrict" />--%>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">City <span class="err">*</span></label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" Width="100%" ID="ddlCity"></asp:DropDownList>
                                                            <asp:HiddenField ID="hd_ddlCity" runat="server" Value="1" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" ControlToValidate="ddlCity" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select city"></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Personal Email Id <span class="err">*</span></label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox1" PlaceHolder="Email ID" Width="100%" MaxLength="50"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtEmailID" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Personal email id"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Please enter valid Personal email id" ControlToValidate="txtEmailID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers,UppercaseLetters, LowercaseLetters, Custom" ValidChars=".@!#$%^&*()_,/\-" TargetControlID="txtEmailID" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Contact No. <span class="err">*</span></label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtContactNo" PlaceHolder="Enter contact No." Width="100%" MaxLength="10"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtContactNo" runat="server" Value="1" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="txtContactNo" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter contact no"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Provided mobile number is not valid! Please enter valid 10 digit mobile number start with '6/7/8/9'." ControlToValidate="txtContactNo" ValidationExpression="^[6789]\d{9}$" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="Numbers" TargetControlID="txtContactNo" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Landline No. </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtLandlineNo" PlaceHolder="Enter landline no." Width="100%" MaxLength="10"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtLandlineNo" runat="server" Value="1" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterType="Numbers" TargetControlID="txtLandlineNo" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Alternate No </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtAlterNateNo" PlaceHolder="Enter contact No." Width="100%" MaxLength="10"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtAlterNateNo" runat="server" Value="0" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Provided mobile number is not valid! Please enter valid 10 digit mobile number start with '7/8/9'." ControlToValidate="txtAlterNateNo" ValidationExpression="^[789]\d{9}$" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" FilterType="Numbers" TargetControlID="txtAlterNateNo" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                            <ContentTemplate>
                                <%--<div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="form-group col-md-6">
                                                            <asp:CheckBox ID="CheckBoxagent" runat="server" AutoPostBack="true" onchange="javascript:ShowAddress();" OnCheckedChanged="CheckBoxAddress_CheckedChanged" />
                                                            <label>Shop Details Same As Communication Details</label>
                                                        </div>
                                                    </div>
                                                </div>--%>
                            </ContentTemplate>
                            <%--<Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="CheckBoxagent" EventName="CheckedChanged" />

                                            </Triggers>--%>
                        </asp:UpdatePanel>
                        <br />
                        <div style="height: 10px"></div>

                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                </div>
                                <div class="clearfix"></div>
                                <div class="panel-body">
                                    <div class="accordion summary-accordion" id="history-accordionsds">
                                        <div class="accordion-item" id="divshopcountry" runat="server">
                                            <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes1">
                                                <h6 class="searchHeader-heading">Shop Details</h6>
                                                <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOnes1"
                                                    aria-expanded="true" aria-controls="collapseOne">
                                                    <span class="icon-hide"></span>
                                                    <p>Show / Hide</p>
                                                </button>
                                            </div>

                                            <div id="collapseSummaryOnes1" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                                data-bs-parent="#summary-accordion">
                                                <div class="accordion-body">
                                                    <hr class="hr-line">
                                                    <!-- grid -->

                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                        <div class="col1">
                                                            <label for="exampleInputEmail1">Shop Address </label>
                                                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtshopadd" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Shop address"></asp:RequiredFieldValidator>--%>
                                                            <asp:TextBox runat="server" class="form-control" ID="txtshopadd" TextMode="MultiLine" Width="100%" PlaceHolder="Shop Address " Style="resize: none"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField4" runat="server" Value="1" />
                                                        </div>
                                                        <!-- input -->
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop Pincode </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" PlaceHolder="eg.400601" Width="100%" ID="txtshoppin" AutoPostBack="true" MaxLength="6" OnTextChanged="txtshoppin_TextChanged"></asp:TextBox>
                                                            <%--  <asp:HiddenField ID="HiddenField9" runat="server" Value="1" />--%>
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers" TargetControlID="txtshoppin" />
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator19" ControlToValidate="txtPinCode" ValidationGroup="FranchiseReg" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter Shop pincode"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                        <!-- input -->
                                                        <div class="col" style="display: none">
                                                            <label for="exampleInputEmail1">Shop Country </label>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="ddlshopCountry" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select Shop country"></asp:RequiredFieldValidator>--%>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlshopCountry" Width="100%" AutoPostBack="true"></asp:DropDownList>
                                                            <%--OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"--%>
                                                            <%--<asp:HiddenField ID="HiddenField5" runat="server" Value="1" />--%>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop State </label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlShopState" Width="100%" OnSelectedIndexChanged="ddlShopState_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            <%--OnSelectedIndexChanged="ddlState_SelectedIndexChanged"--%>
                                                            <%--<asp:HiddenField ID="HiddenField6" runat="server" Value="1" />--%>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="ddlShopState" Width="100%" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select Shop state"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop District</label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlShopDistrict" Width="100%" OnSelectedIndexChanged="ddlShopDistrict_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator19" ControlToValidate="ddlShopDistrict" Width="100%" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select state"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                        <br />
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop City </label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" Width="100%" ID="ddlShopCity"></asp:DropDownList>
                                                            <%--  <asp:HiddenField ID="HiddenField8" runat="server" Value="1" />--%>
                                                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator18" ControlToValidate="ddlShopCity" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select city"></asp:RequiredFieldValidator>--%>
                                                        </div>

                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop EmailId </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtshopEmailID" PlaceHolder="Email ID" Width="100%" MaxLength="50"></asp:TextBox>

                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Please enter valid Shop email id" ControlToValidate="txtEmailID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Numbers,UppercaseLetters, LowercaseLetters, Custom" ValidChars=".@!#$%^&*()_,/\-" TargetControlID="txtEmailID" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop Contact No. </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox5" PlaceHolder="Enter contact No." Width="100%" MaxLength="10"></asp:TextBox>
                                                            <%-- <asp:HiddenField ID="hdnUserConfirmation0" runat="server" Value="1" />--%>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator21" ControlToValidate="TextBox5" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Shop contact no"></asp:RequiredFieldValidator>--%>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Provided mobile number is not valid! Please enter valid 10 digit mobile number start with '6/7/8/9'." ControlToValidate="txtContactNo" ValidationExpression="^[6789]\d{9}$" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" FilterType="Numbers" TargetControlID="TextBox5" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop Landline No. </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox6" PlaceHolder="Enter landline no." Width="100%" MaxLength="10"></asp:TextBox>
                                                            <%--<asp:HiddenField ID="hdnUserConfirmation1" runat="server" Value="1" />--%>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator22" ControlToValidate="TextBox6" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Shop landline no"></asp:RequiredFieldValidator>--%>
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" FilterType="Numbers" TargetControlID="TextBox6" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Alternate No </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox9" PlaceHolder="Enter contact No." Width="100%" MaxLength="10"></asp:TextBox>
                                                            <%--<asp:HiddenField ID="hdnUserConfirmation4" runat="server" Value="0" />--%>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Provided mobile number is not valid! Please enter valid 10 digit mobile number start with '7/8/9'." ControlToValidate="txtAlterNateNo" ValidationExpression="^[789]\d{9}$" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" FilterType="Numbers" TargetControlID="TextBox9" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                                <div>
                                                                    <asp:Button runat="server" ID="btnSubmitDetails" ValidationGroup="FranchiseReg" OnClientClick="Confirm();" OnClick="btnSubmitDetails_Click" Class="themeBtn themeApplyBtn" CommandArgument='<%#Eval("AgentReqId")%>' Text="Submit"></asp:Button>
                                                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="false" class="themeBtn resetBtn themeCancelBtn me-0" Text="Back" OnClick="btnCancel_Click"></asp:Button>
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
                                                                ValidationGroup="FranchiseReg"
                                                                runat="server" />
                                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                            <div>
                                <asp:Button runat="server" ID="btnSubmitDetails" ValidationGroup="FranchiseReg" OnClientClick="Confirm();" OnClick="btnSubmitDetails_Click" Class="themeBtn themeApplyBtn" CommandArgument='<%#Eval("AgentReqId")%>' Text="Submit"></asp:Button>
                                <asp:Button ID="btnCancel" runat="server" CausesValidation="false" class="themeBtn resetBtn themeCancelBtn me-0" Text="Back" OnClick="btnCancel_Click"></asp:Button>
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
                            ValidationGroup="FranchiseReg"
                            runat="server" />
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlbcCode.ClientID%>").select2();
                    $("#<%=ddlGender.ClientID%>").select2();
                    $("#<%=ddlCategory.ClientID%>").select2();
                    $("#<%=ddlHighestEducationQualification.ClientID%>").select2();
                    $("#<%=ddlCourse.ClientID%>").select2();
                    $("#<%=ddlAlternateOccupationType.ClientID%>").select2();
                    $("#<%=ddlShopOpensat.ClientID%>").select2();

                    $("#<%=ddlClosesat.ClientID%>").select2();
                    $("#<%=ddlDeviceName.ClientID%>").select2();
                    $("#<%=ddlConnectivityType.ClientID%>").select2();
                    $("#<%=ddlCorporateIndividualBC.ClientID%>").select2();
                    $("#<%=ddlFranchiseWalletTransfer.ClientID%>").select2();
                    $("#<%=ddlIsUPIPartner.ClientID%>").select2();
                    $("#<%=ddlwalletLoadingWithdrawl.ClientID%>").select2();
                    $("#<%=ddlBcFranchisetype.ClientID%>").select2();

                    $("#<%=ddlParentFlag.ClientID%>").select2();
                    $("#<%=ddlRegistrationType.ClientID%>").select2();
                    $("#<%=ddlRequestStatus.ClientID%>").select2();
                    $("#<%=ddlState.ClientID%>").select2();


                    $("#<%=ddlDistrict.ClientID%>").select2();
                    $("#<%=ddlCity.ClientID%>").select2();
                    $("#<%=ddlshopCountry.ClientID%>").select2();
                    $("#<%=ddlShopState.ClientID%>").select2();
                    $("#<%=ddlShopDistrict.ClientID%>").select2();
                    $("#<%=ddlShopCity.ClientID%>").select2();
                    $("#<%=ddlPhysicallyHandicapped.ClientID%>").select2();
                    $("#<%=ddlTypeOfCommission.ClientID%>").select2();
                }
            });
        };
    </script>
</asp:Content>
