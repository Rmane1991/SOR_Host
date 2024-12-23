<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="Versioning.aspx.cs" Inherits="SOR.Pages.Patch.Versioning" %>

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
    <script type="text/javascript">
        function openReleaseNote(link) {
            var url = link.getAttribute('data-url');
            window.open(url, '_blank');

            // Optionally trigger the server-side event if needed
            __doPostBack(link.id, ''); // This simulates a postback
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

            <%--document.getElementById("<%=txtFinalRemarks.ClientID %>").value = '';
            var elementRef = document.getElementById('<%= rdbtnApproveDecline.ClientID %>');
            var inputElementArray = elementRef.getElementsByTagName('input');
            for (var i = 0; i < inputElementArray.length; i++) {
                var inputElement = inputElementArray[i];
                inputElement.checked = false;
            }--%>
            $('#ModalAgent').modal('hide');
        }
    </script>
    <!-- Bootstrap CSS -->
    <%--<link href="https://maxcdn.bootstrapcdn.com/bootstrap/5.1.0/css/bootstrap.min.css" rel="stylesheet">

    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>--%>

    <!-- Bootstrap JS -->
    <%--<script>
        $(document).ready(function () {
            var showModal = document.getElementById('<%= hdnshowmanual.ClientID %>').value;
            console.log("Modal value:", showModal);  // This will log to console

            if (showModal === "true") {
                var exampleModalManual = new bootstrap.Modal(document.getElementById('exampleModalManual'));
                exampleModalManual.show();
            }
        });
    </script>--%>
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
                <h5 class="page-title">Versioning</h5>
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
                            <h6 class="searchHeader-heading">Patch Details</h6>
                            <br />
                            <hr class="hr-line">
                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                <div class="col col-all-filter" style="display: none">
                                    <label class="selectInputLabel" for="selectInputLabel">Date</label>
                                    <div class="selectInputDateBox w-100">
                                        <input type="date" runat="server" id="txtDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" />
                                    </div>
                                </div>

                                <!-- input -->
                                <div class="col">
                                    <label class="selectInputLabel" for="selectInputLabel">Patch Type</label>
                                    <div class="selectInputBox">
                                        <asp:DropDownList runat="server" ID="ddlPatchType" CssClass="maximus-select w-100" TabIndex="1" AppendDataBoundItems="true" AutoPostBack="False">
                                            <asp:ListItem Value="0" Text="-- Patch Type --"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Portal"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="API"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col">
                                    <label class="selectInputLabel" for="selectInputLabel">Patch</label>
                                    <div class="selectInputBox">
                                        <asp:FileUpload ID="UploadPatch" runat="server" CssClass="form-control" Height="33px" Width="250px" Font-Size="Small" />
                                    </div>
                                </div>
                                &nbsp;&nbsp;
                                <div class="col">
                                    <label class="selectInputLabel" for="selectInputLabel">Version</label>
                                    <div class="inputBox w-100">
                                        <input type="text" id="txtVersion" runat="server" class="input-text form-control" style="width: 100%" placeholder="Version" />
                                    </div>
                                </div>

                                <div class="col">
                                    <label class="selectInputLabel" for="selectInputLabel">Release Note</label>
                                    <div class="selectInputBox">
                                        <asp:FileUpload ID="UploadReleaseNote" runat="server" CssClass="form-control" Height="33px" Width="250px" Font-Size="Small" />
                                    </div>
                                </div>

                            </div>
                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                <div class="col">
                                    <label class="selectInputLabel" for="selectInputLabel">Released On</label>
                                    <div class="selectInputDateBox w-100">
                                        <input type="date" runat="server" id="txtReleasedOn" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkReleasedOn()" />
                                    </div>
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
                    <asp:GridView ID="gvVersioning" runat="server" CssClass="GridView"
                        AutoGenerateColumns="false"
                        Width="100%"
                        GridLines="None"
                        AllowPaging="true"
                        PageSize="10"
                        Visible="true"
                        AllowSorting="True"
                        OnRowCommand="gvVersioning_RowCommand"
                        OnPageIndexChanging="gvVersioning_PageIndexChanging"
                        HeaderStyle-CssClass="pagination-ys"
                        OnRowDataBound="gvVersioning_RowDataBound">

                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" BackColor="#FBD2CE" Font-Bold="true" CssClass="bg" />
                        <RowStyle Wrap="false" VerticalAlign="Middle" HorizontalAlign="Center" />

                        <Columns>
                            <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server"
                                        ImageUrl="../../Images/edit.png"
                                        Width="16" Height="16"
                                        CommandName="EditPatch"
                                        ToolTip="Edit Patch"
                                        CommandArgument='<%# Eval("id") %>' />
                                    <asp:ImageButton
                                        ID="btnCancelAction"
                                        runat="server"
                                        ImageUrl="~/Images/cancel.png"
                                        ToolTip="Cancel"
                                        CommandName="Cancel"
                                        CommandArgument='<%# Eval("id") %>'
                                        Height="20px"
                                        OnClientClick="showModalCancel(); return false;" />

                                    <asp:ImageButton ID="btnReschedule" runat="server"
                                        ImageUrl="~/Images/reschedule.png"
                                        ToolTip="Reschedule"
                                        CommandName="Reschedule"
                                        CommandArgument='<%# Eval("id") %>'
                                        AlternateText="Reschedule" Height="20px" />

                                    <asp:ImageButton ID="btnProduction" runat="server"
                                        ImageUrl="~/Images/update.png"
                                        ToolTip="CUG"
                                        CommandName="Production"
                                        CommandArgument='<%# Eval("id") %>'
                                        AlternateText="Production" Height="20px"
                                        OnClientClick="showModalCUG(); return false;" />

                                    <asp:ImageButton ID="btnProd" runat="server"
                                        ImageUrl="~/Images/prod.png"
                                        ToolTip="Production"
                                        CommandName="Production"
                                        CommandArgument='<%# Eval("id") %>'
                                        AlternateText="Production" Height="20px"
                                        OnClientClick="showModalProd(); return false;" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderText="Download Patch" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="UpButtons" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btnViewDownload" runat="server"
                                                ImageUrl="../../Images/download.png"
                                                Width="16" Height="16"
                                                CommandName="DownloadDoc"
                                                ToolTip="Download Patch"
                                                CommandArgument='<%# Eval("patchpath") %>' />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnViewDownload" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Release Note" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnReleaseNote" runat="server"
                                        CommandName="OpenReleaseNote"
                                        CommandArgument='<%# Eval("releasenotepath") %>'><%# Eval("releasenotefilename") %>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="patchtype" HeaderText="Patch Type" />
                            <asp:BoundField DataField="version" HeaderText="Patch Version" />
                            <asp:BoundField DataField="releasedon" HeaderText="Released On" />
                            <asp:BoundField DataField="createdby" HeaderText="Imported By" />
                            <asp:BoundField DataField="createdon" HeaderText="Imported On" />
                            <asp:BoundField DataField="status" HeaderText="CUG Patch Status" />
                            <asp:BoundField DataField="cugstatus" HeaderText="CUG Status" />
                            <asp:BoundField DataField="approvedby" HeaderText="CUG Approved By" />
                            <asp:BoundField DataField="approvedon" HeaderText="CUG Approved On" />
                            <asp:BoundField DataField="rejectedby" HeaderText="CUG Decline By" />
                            <asp:BoundField DataField="rejectedon" HeaderText="CUG Decline On" />
                            <asp:BoundField DataField="remarks" HeaderText="CUG Remarks" />
                            <asp:BoundField DataField="prodstatus" HeaderText="Prod Status" />
                            <asp:BoundField DataField="prodpatchstatus" HeaderText="Prod Patch Status" />
                            <asp:BoundField DataField="prodapprovedby" HeaderText="Prod Approved By" />
                            <asp:BoundField DataField="prodapprovedon" HeaderText="Prod Approved On" />
                            <asp:BoundField DataField="prodrejectedby" HeaderText="Prod Decline By" />
                            <asp:BoundField DataField="prodrejectedon" HeaderText="Prod Decline On" />
                            <asp:BoundField DataField="prodremarks" HeaderText="Prod Remarks" />
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
    <div class="modal fade" id="exampleModalManual" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabelManual">Schedule Details</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <!-- Body -->
                <div class="modal-body">
                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                        <div class="col">
                            <label class="selectInputLabel" for="selectInputLabel">Patch Type</label>
                            <div class="selectInputBox">
                                <asp:DropDownList runat="server" ID="ddlpatchscheduletype" CssClass="maximus-select w-100" TabIndex="1" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpatchscheduletype_SelectedIndexChanged">
                                    <asp:ListItem Value="-1" Text="-- Patch Type --"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="CUG"></asp:ListItem>
                                    <%--<asp:ListItem Value="2" Text="Production"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col">
                            <label class="selectInputLabel" for="selectInputLabel">Services</label>
                            <div class="selectInputBox">
                                <asp:DropDownList runat="server" ID="ddlcugservices" CssClass="maximus-select w-100"
                                    TabIndex="1" AppendDataBoundItems="true" AutoPostBack="false"
                                    Size="5" multiple="multiple" OnChange="setSelectedServices()">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <asp:HiddenField runat="server" ID="hdnSelectedServices" />
                        <div class="col">
                            <label class="selectInputLabel" for="selectInputLabel">Schedule Date</label>
                            <div class="selectInputDateBox w-100">
                                <input type="date" runat="server" id="txtPatchDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkReleasedOn()" />
                            </div>
                        </div>
                        <div class="col">
                            <label class="selectInputLabel" for="selectInputLabel">Schedule Time</label>
                            <div class="selectInputDateBox w-100">
                                <input type="time" runat="server" id="txtPatchTime" class="select-time form-control" style="width: 100%; height: 32px;" placeholder="Select Time" onchange="checkReleasedOn()" />
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Footer -->
                <div class="modal-footer">
                    <asp:Button ID="btnSubmitDetails" CssClass="btn btn-primary" runat="server" Text="Submit" OnClick="btnSubmitDetails_Click" />
                    <asp:Button ID="btnClsManual" CssClass="btn btn-secondary" runat="server" Text="Cancel" OnClick="btnClsManual_Click" data-bs-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnshowmanual" runat="server" Value="false" />
    <!-- Cancel Modal -->
    <div class="modal fade" id="cancelModal" tabindex="-1" role="dialog" aria-labelledby="cancelModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="cancelModalLabel">Cancel Action</h5>
                </div>
                <div class="modal-body">
                    <div class="col">
                        <!-- Label for Remark -->
                        <label for="txtRemark">Remark <span class="err">*</span></label>

                        <asp:TextBox
                            runat="server"
                            CssClass="input-text form-control"
                            ID="txtRemark"
                            PlaceHolder="Enter your remark here..."
                            Width="100%"
                            MaxLength="500"
                            TextMode="MultiLine"
                            Rows="4"
                            Columns="50"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="hideModalCancel()">Close</button>
                    <asp:Button
                        type="button"
                        ID="confirmCancel"
                        runat="server"
                        Text="Confirm Cancel"
                        CssClass="btn btn-danger"
                        OnClick="confirmCancel_Click" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfCancel" runat="server" />

    <!-- CUG Modal -->
    <div class="modal fade" id="CUGModal" tabindex="-1" role="dialog" aria-labelledby="cancelModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="CUGModalLabel">CUG Action</h5>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col">
                            <label class="selectInputLabel" for="selectInputLabel">Status</label>
                            <div class="selectInputBox">
                                <asp:DropDownList runat="server" ID="ddlcugstatus" CssClass="maximus-select w-100">
                                    <asp:ListItem Value="-1" Text="-- select --"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Success"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Failed"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col">
                            <label class="selectInputLabel" for="selectInputLabel">Document</label>
                            <div class="selectInputBox">
                                <asp:FileUpload ID="FileUploadCUG" runat="server" CssClass="form-control" Height="33px" Width="250px" Font-Size="Small" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="hideModalCUG()">Close</button>
                    <asp:Button
                        type="button"
                        ID="btncugstatus"
                        runat="server"
                        Text="Confirm"
                        CssClass="btn btn-danger"
                        OnClick="btncugstatus_Click" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfCUG" runat="server" />

    <!-- Prod Modal -->
    <div class="modal fade" id="ProdModal" tabindex="-1" role="dialog" aria-labelledby="ProdModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ProdModalLabel">Production Action</h5>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col">
                            <label class="selectInputLabel" for="selectInputLabel">Status</label>
                            <div class="selectInputBox">
                                <asp:DropDownList runat="server" ID="DropDownList1" CssClass="maximus-select w-100">
                                    <asp:ListItem Value="-1" Text="-- select --"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Success"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Failed"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col">
                            <label class="selectInputLabel" for="selectInputLabel">Document</label>
                            <div class="selectInputBox">
                                <asp:FileUpload ID="FileUploadProd" runat="server" CssClass="form-control" Height="33px" Width="250px" Font-Size="Small" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="hideModalProd()">Close</button>
                    <asp:Button
                        type="button"
                        ID="btnprodstatus"
                        runat="server"
                        Text="Confirm"
                        CssClass="btn btn-danger"
                        OnClick="btnprodstatus_Click" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnprod" runat="server" />

    <script>
        // Show the modal and store rowId
        function showModalCancel(event) {
            $('#cancelModal').modal('show');
            window.currentRowId = event.target.getAttribute('commandargument');
        }

        // Hide the modal
        function hideModalCancel() {
            $('#cancelModal').modal('hide');
        }
        document.getElementById('confirmCancelButton').onclick = function () {
            // Set the value of the HiddenField to trigger a postback
            document.getElementById('<%= hfCancel.ClientID %>').value = 'Cancel|' + window.currentRowId;

            // Trigger the postback
            __doPostBack('<%= gvVersioning.UniqueID %>', '');
        };
    </script>

    <script>
        // Show the modal and store rowId
        function showModalCUG(event) {
            $('#CUGModal').modal('show');
            window.currentRowId = event.target.getAttribute('commandargument');
        }

        // Hide the modal
        function hideModalCUG() {
            $('#CUGModal').modal('hide');
        }
        document.getElementById('confirmCancelButton').onclick = function () {
            // Set the value of the HiddenField to trigger a postback
            document.getElementById('<%= hfCUG.ClientID %>').value = 'CUG|' + window.currentRowId;

            // Trigger the postback
            __doPostBack('<%= gvVersioning.UniqueID %>', '');
        };
    </script>

    <%-- Prod Model Script --%>
    <script>
        function showModalProd(event) {
            $('#ProdModal').modal('show');
            window.currentRowId = event.target.getAttribute('commandargument');
        }

        // Hide the modal
        function hideModalProd() {
            $('#ProdModal').modal('hide');
        }
        document.getElementById('btnprodstatus_Click').onclick = function () {
            // Set the value of the HiddenField to trigger a postback
            document.getElementById('<%= hdnprod.ClientID %>').value = 'CUG|' + window.currentRowId;

            // Trigger the postback
            __doPostBack('<%= gvVersioning.UniqueID %>', '');
        };
    </script>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlPatchType.ClientID%>").select2();
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
    <script type="text/javascript">  
        function checkReleasedOn() {
            var date = document.getElementById("<%= txtReleasedOn.ClientID %>").value;
            var todayDate = new Date().toISOString().slice(0, 10);
            if (date > todayDate) {
                alert("Selected date can not be greater than Current date !!");
                document.getElementById("<%= txtReleasedOn.ClientID %>").value = todayDate;
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
    <script>
        $(document).ready(function () {
            $('#<%= ddlcugservices.ClientID %>').select2({
                placeholder: "Select Services",
                allowClear: true
            });
        });
</script>

    <script type="text/javascript">
        function showModal() {
            // Retrieve the value of the hidden field
            var showModal = document.getElementById('<%= hdnshowmanual.ClientID %>').value;

            // Show the modal if the value is "true"
            if (showModal === "true") {
                var exampleModalManual = new bootstrap.Modal(document.getElementById('exampleModalManual'));
                exampleModalManual.show();
            }
        }


        function setSelectedServices() {
            var dropdown = document.getElementById('<%= ddlcugservices.ClientID %>');
            var selectedCount = 0;

            // Count how many items are selected
            for (var i = 0; i < dropdown.options.length; i++) {
                if (dropdown.options[i].selected) {
                    selectedCount++;
                }
            }

            // If more than 2 items are selected, alert the user and prevent selection
            if (selectedCount > 2) {
                alert("You can select a maximum of 2 services.");

                // Deselect the last selected option
                for (var i = 0; i < dropdown.options.length; i++) {
                    if (dropdown.options[i].selected) {
                        dropdown.options[i].selected = false;
                        break;  // Deselect the first extra selected item
                    }
                }
            }
        }
        function setSelectedServices() {
            var dropdown = document.getElementById('<%= ddlcugservices.ClientID %>');
            var selectedValues = [];
            var selectedCount = 0;

            // Loop through all the options and find the selected ones
            for (var i = 0; i < dropdown.options.length; i++) {
                if (dropdown.options[i].selected) {
                    selectedValues.push(dropdown.options[i].value);
                    selectedCount++;
                }
            }

            // If 2 or fewer items are selected, enable all options
            // If more than 2 are selected, alert and disable further selections
            if (selectedCount >= 2) {
                //alert("You can select a maximum of 2 services.");

                // Disable further selections by disabling the remaining unselected options
                for (var i = 0; i < dropdown.options.length; i++) {
                    if (!dropdown.options[i].selected) {
                        dropdown.options[i].disabled = true; // Disable unselected options
                    }
                }
            } else {
                // Enable all options if the number of selections is fewer than 2
                for (var i = 0; i < dropdown.options.length; i++) {
                    dropdown.options[i].disabled = false; // Enable all options
                }
            }

            // Update the hidden field with selected values (comma-separated)
            var hiddenField = document.getElementById('<%= hdnSelectedServices.ClientID %>');
            hiddenField.value = selectedValues.join(',');
        }
    </script>

</asp:Content>
