<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SOR.Master" CodeBehind="AlertTemplateMaster.aspx.cs" Inherits="SOR.Pages.Monitoring.AlertTemplateMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style>
        span {
            font-size: 12px;
        }

        .err {
            color: #fe1a03;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .GridView {
            width: 100%;
            border: solid 0px #B9B9B9 !important;
        }

            .GridView th {
                background: #FFB2B2;
                color: black;
            }

            .GridView td {
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
        /* Other styles can be added here as needed */
    </style>

    <%--for mail content body--%>
    <style>
        .select2 {
            width: 100% !important;
        }

        .card {
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        }

        label {
            display: block;
            font-weight: 600;
            color: #333;
            margin-bottom: 0.5rem;
        }

        .form-control {
            width: 100%;
            padding: 0.75rem 1.25rem;
            margin-top: 0.25rem;
        }

        .font-weight-semibold {
            font-weight: 600;
            color: #333;
            margin-bottom: 0.5rem;
        }

        .btn {
            border-radius: 4px;
            font-weight: 600;
        }

        .btn-primary {
            background-color: #007bff;
            border-color: #007bff;
        }

            .btn-primary:hover {
                background-color: #0056b3;
                border-color: #0056b3;
            }

        .btn-secondary {
            background-color: #6c757d;
            border-color: #6c757d;
        }

            .btn-secondary:hover {
                background-color: #5a6268;
                border-color: #545b62;
            }
    </style>

    <script type="text/javascript">
        function Confirm(msg, hiddenFieldId) {
            if (confirm(msg)) {
                document.getElementById(hiddenFieldId).value = "Yes";
            } else {
                document.getElementById(hiddenFieldId).value = "No";
            }
        }

        function Confirm2() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Delete Data?")) {
                debugger;
                <%--document.getElementById("<%= hdnDeleteConfirmation.ClientID %>").value = "Yes";--%>
            }
            else {
                <%--document.getElementById("<%= hdnDeleteConfirmation.ClientID %>").value = "No";--%>
            }

        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div class="main-content-container container-fluid px-4">
        <!-- Page Header -->
        <div class="breadHeader mb-4">
            <h5 class="page-title text-dark">Email Template Details</h5>
        </div>

        <!-- Template Creation Form -->
        <div class="card p-4 shadow-sm">
            <!-- Row for Template Type, Name, and Subject -->


            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTemplateType" EventName="SelectedIndexChanged" />
                </Triggers>
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4 mb-3">
                            <label for="ddlTemplateType" class="font-weight-semibold">Template Type</label>
                            <asp:DropDownList ID="ddlTemplateType" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="ddlTemplateType_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="MAIL" Value="MAIL"></asp:ListItem>
                                <asp:ListItem Text="SMS" Value="SMS"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <!-- Template Name Field -->
                        <div class="col-md-4 mb-3">
                            <label for="txtTemplateName" class="font-weight-semibold">Template Name*</label>
                            <asp:DropDownList ID="ddlTemplateList" runat="server" CssClass="form-control select2">
                            </asp:DropDownList>
                        </div>

                        <!-- Subject Field -->
                        <div class="col-md-4 mb-3">
                            <label for="txtsubject" class="font-weight-semibold">Subject*</label>
                            <asp:TextBox ID="txtsubject" runat="server" CssClass="form-control" placeholder="Enter Subject"></asp:TextBox>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <!-- Row for CC Mail and Body Content -->
            <div class="row">
                <!-- CC Mail ID Field (hidden initially) -->
                <div class="col-md-6 mb-3" id="ccMailContainer" hidden>
                    <label for="txtCCMailID" class="font-weight-semibold">CC Mail ID (separate with semicolon)</label>
                    <asp:TextBox ID="txtCCMailID" runat="server" CssClass="form-control" MaxLength="200" placeholder="Enter CC Mail ID"></asp:TextBox>
                </div>

                <!-- Body Content -->
                <div class="col-md-6 mb-3">
                    <label for="txtBodyContent" class="font-weight-semibold">Body Content*</label>
                    <asp:TextBox ID="txtBodyContent" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" placeholder="Enter Body Content"></asp:TextBox>
                </div>
            </div>

        </div>
    </div>

    <div class="row selectInput-grid20 select-grid-gap searchbox-btns">
        <div>
            <asp:HiddenField ID="hidAction" runat="server" Value="0" />
            <asp:HiddenField ID="hidUpdateID" runat="server" Value="0" />
            <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="themeBtn themeApplyBtn" Text="Submit"></asp:Button>
            <asp:Button runat="server" ID="btnCancel" CausesValidation="false" CssClass="themeBtn resetBtn themeCancelBtn" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
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
                            <asp:GridView ID="gvTemplateMaster" runat="server"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                AllowPaging="true"
                                CssClass="GridView"
                                Visible="true"
                                PageSize="10"
                                PagerSettings-Mode="NumericFirstLast"
                                PagerSettings-FirstPageText="First Page"
                                PagerSettings-LastPageText="Last Page"
                                OnRowCommand="gvTemplateMaster_RowCommand"
                                OnPageIndexChanging="gvTemplateMaster_PageIndexChanging"
                                OnRowDataBound="gvTemplateMaster_RowDataBound">
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
                                            <asp:ImageButton ID="btnView" runat="server" ImageUrl="../../images/Edit-01-512.png" Width="20px" Height="20px" CommandName="EDT"
                                                ToolTip="Click Here To Edit Data" CommandArgument='<%#Eval("id")+"="+Eval("templateid")%>' data-toggle="modal" data-target="#ModalBC" />
                                            <asp:ImageButton ID="btndelete" runat="server" ImageUrl="../../images/document-delete.png" OnClientClick="Confirm2();" Width="20px" Height="20px" CommandName="DLT"
                                                ToolTip="Click Here To Delete Data" CommandArgument='<%#Eval("id")%>' data-toggle="modal" data-target="#ModalBC" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="templatetype" HeaderText="Template Type" />
                                    <asp:BoundField DataField="Templatename" HeaderText="Template Name" />
                                    <asp:BoundField DataField="subject" HeaderText="Subject" />
                                    <asp:BoundField DataField="mailbody" HeaderText="Mail body" />
                                    <asp:BoundField DataField="smsbody" HeaderText="SMS body" />
                                    <asp:BoundField DataField="createdby" HeaderText="Created by" />
                                    <asp:BoundField DataField="createddate" HeaderText="Created Date" />
                                    <asp:BoundField DataField="updateddate" HeaderText="Last Updated" />
                                    <asp:BoundField DataField="updatedby" HeaderText="Updated by" />
                                </Columns>
                                <HeaderStyle BackColor="#8DCCF6" ForeColor="#3D62B6" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>
    <script type="text/javascript">
        document.getElementById('<%= ddlTemplateType.ClientID %>').addEventListener('change', function () {
            var ccMailContainer = document.getElementById('ccMailContainer');
            if (this.value === 'SMS') {
                ccMailContainer.style.display = 'none';
            } else {
                ccMailContainer.style.display = '';
            }
        });

        window.onload = function () {
            var event = new Event('change');
            document.getElementById('<%= ddlTemplateType.ClientID %>').dispatchEvent(event);
        };



    </script>


    <script>
        $(document).ready(function () {
            $('#<%= ddlTemplateType.ClientID %>').select2();

            $('#<%= ddlTemplateType.ClientID %>').on('change', function () {
                __doPostBack('<%= ddlTemplateType.UniqueID %>', '');
            });
        });
    </script>
</asp:Content>


