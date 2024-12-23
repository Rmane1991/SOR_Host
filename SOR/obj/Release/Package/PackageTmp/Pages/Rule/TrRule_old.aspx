<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="TrRule_old.aspx.cs" Inherits="SOR.Pages.Rule.TrRule_old" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .multi-select-container {
            position: relative;
            display: inline-block;
            width: 200px;
        }

        .selected-items {
            border: 1px solid #ccc;
            padding: 5px;
            cursor: pointer;
            background-color: #f9f9f9;
        }

        .dropdown-content {
            display: none;
            position: absolute;
            border: 1px solid #ccc;
            background-color: #fff;
            z-index: 1;
            width: 100%;
            box-sizing: border-box;
            max-height: 200px;
            overflow-y: auto;
        }

            .dropdown-content label {
                display: block;
                padding: 5px;
                cursor: pointer;
            }

                .dropdown-content label input {
                    margin-right: 10px;
                }

        .selected-item {
            background-color: #007bff;
            color: white;
            border-radius: 4px;
            padding: 2px 8px;
            margin: 2px;
            display: inline-block;
        }

            .selected-item .close {
                margin-left: 5px;
                cursor: pointer;
                font-size: 12px;
                line-height: 1;
            }

        .placeholderr {
            color: #999;
        }

        .search-box {
            padding: 5px;
            border-bottom: 1px solid #ccc;
            width: 100%;
        }
    </style>

    <%-- <link href="../../css/DropDown.css" rel="stylesheet" />--%>
    <style>
        .custom-switch {
            position: relative;
            display: inline-block;
            width: 34px;
            height: 20px;
        }

            .custom-switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .custom-slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            transition: .4s;
            border-radius: 20px;
        }

            .custom-slider:before {
                position: absolute;
                content: "";
                height: 12px;
                width: 12px;
                border-radius: 50%;
                left: 4px;
                bottom: 4px;
                background-color: white;
                transition: .4s;
            }

        input:checked + .custom-slider {
            background-color: #2196F3;
        }

            input:checked + .custom-slider:before {
                transform: translateX(14px);
            }

        .row {
            padding-bottom: 20px; /* Adjust the value as needed for spacing */
        }
    </style>


    <style>
        .slider {
            width: 50px;
            height: 25px;
            background-color: red;
            border-radius: 25px;
            position: relative;
            cursor: pointer;
            transition: background-color 0.2s ease-in-out;
        }

        .slider-button {
            width: 23px;
            height: 23px;
            background-color: white;
            border-radius: 50%;
            position: absolute;
            top: 1px;
            left: 1px;
            transition: transform 0.2s ease-in-out;
        }

        .form-selectt {
            width: 100%;
            height: 150px; /* Adjust height as needed */
            overflow-y: auto;
            border: 1px solid #ccc;
            border-radius: 4px;
            padding: 5px;
            background-color: #fff;
        }
    </style>



    <script type="text/javascript">

        function toggleSlider(element) {
            var checkBox = element.querySelector('input[type=checkbox]');
            if (checkBox) {
                checkBox.checked = !checkBox.checked;
                // Optionally, update the style of the slider based on the new checked state
                if (checkBox.checked) {
                    element.style.backgroundColor = 'green';
                    element.querySelector('.slider-button').style.transform = 'translateX(26px)';
                } else {
                    element.style.backgroundColor = 'red';
                    element.querySelector('.slider-button').style.transform = 'translateX(0px)';
                }
                // Optionally, call an AJAX method to update the server
                // e.g., UpdateRowStatus(dataId, checkBox.checked);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div class="breadHeader">
        <h5 class="page-title">TR Rule</h5>
    </div>
    <%--<asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:UpdateProgress ID="upContentBodyUpdateProgress" runat="server" AssociatedUpdatePanelID="upContentBody">
            <ProgressTemplate>
                <%-- <div style="width: 100%; height: 100%; opacity: 0.8; background-color: black; position: fixed; top: 0; left: 0">
                    <img alt="" id="progressImage1" style="margin-top: 20%" src='<%=Page.ResolveClientUrl("../../Images/loading2_1.gif") %>' />
                </div>--%>
    <%--</ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>--%>
    <asp:UpdatePanel ID="upContentBody" runat="server">
        <ContentTemplate>

            <div id="divMain" runat="server" class="container mt-5">

                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnAddRule" runat="server"
                            CssClass="themeBtn themeApplyBtn"
                            Text="Create Rule"
                            OnClick="btnAddRule_Click" />
                    </div>
                </div>

                <asp:Repeater ID="gvRule" runat="server" OnItemDataBound="gvRule_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th>rule_id</th>
                                    <th>rulename</th>
                                    <th>ruledescription</th>
                                    <th>Action</th>
                                    <!-- Column for the slider button -->
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("rule_id") %></td>
                            <td><%# Eval("rulename") %></td>
                            <td><%# Eval("ruledescription") %></td>
                            
                            <td>
                                <!-- Slider button -->
                                <div id="sliderDiv" class="slider" onclick="toggleSlider(this)" data-id='<%# Eval("rule_id") %>'>
                                    <asp:CheckBox ID="SliderButton" runat="server" Style="display: none;" Checked='<%# Convert.ToBoolean(Eval("is_active")) %>' />
                                    <div class="slider-button"></div>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
        </table>
   
                    </FooterTemplate>
                </asp:Repeater>

                <!-- Action Button -->
                <asp:Button ID="btnAction" runat="server" Text="Perform Action" OnClick="btnAction_Click" CssClass="themeBtn themeApplyBtn" />
            </div>

            <div class="accordion summary-accordion" id="history-accordions">
                <div class="accordion-item">
                    <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes">
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

                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                <div class="col">
                                    <label class="selectInputLabel" for="selectInputLabel">Aggregator:</label>
                                    <div class="selectInputBox">
                                        <asp:DropDownList ID="ddlAggregator" runat="server" CssClass="maximus-select w-100" Visible="false">
                                        </asp:DropDownList>
                                    </div>

                                    <asp:HiddenField ID="hfSelectedValues" runat="server" />
                                    <div class="multi-select-container">
                                        <div class="selected-items" id="selectedItems" onclick="toggleDropdown()">
                                            <span class="placeholderr">Select options...</span>
                                        </div>
                                        <div class="dropdown-content" id="dropdownContent">
                                            <input type="text" id="searchBox" class="search-box" placeholder="Search..." onkeyup="filterDropdown()" />
                                            <div id="dropdownItems">
                                                <asp:Literal ID="litAggregator" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:Button ID="btnhidresetDelete" runat="server" Text="Button" Visible="true" Style="visibility: hidden" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender_AddRule" runat="server"
                TargetControlID="btnhidresetDelete" PopupControlID="Panel_Declincard"
                PopupDragHandleControlID="PopupHeader_Declincard" Drag="true"
                BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel_Declincard" Style="display: none; position: fixed; z-index: 100001; left: 353.5px; top: -119px;" runat="server">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content" style="width: 800px;">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">
                                <asp:Label ID="lblModalHeaderName" runat="server" Text="Rule Creation"></asp:Label><%--<span class="err">*</span>--%>
                            </h4>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>

                        <!-- Modal Body -->
                        <div class="modal-body">
                            <asp:Label ID="lblconfirm" runat="server" Font-Bold="true"></asp:Label>

                            <!-- Dropdowns -->
                            <div class="row">
                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblAggregator" runat="server" Text="Aggregator :"></asp:Label></h4>
                                </div>
                                <div class="col-md-3">
                                    <%--<asp:DropDownList ID="ddlAggregator" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="" />
                                    </asp:DropDownList>--%>

                                    <%--<asp:DropDownList ID="ddlAggregator" runat="server" Visible="false">
                                    </asp:DropDownList>--%>
                                </div>


                                <div class="col-md-2">
                                </div>

                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblddlTxnType" runat="server" Text="TxnType :" AssociatedControlID="ddlTxnType"></asp:Label></h4>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlTxnType" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="" />
                                        <asp:ListItem Text="Option 1" Value="1" />
                                        <asp:ListItem Text="Option 2" Value="2" />
                                        <asp:ListItem Text="Option 3" Value="3" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblSwitch" runat="server" Text="Switch :" AssociatedControlID="ddlSwitch"></asp:Label></h4>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlSwitch" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="" />
                                        <asp:ListItem Text="Maximus" Value="1" />
                                        <asp:ListItem Text="Sarvatra" Value="2" />
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-2">
                                </div>

                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblIIN" runat="server" Text="IIN :" AssociatedControlID="ddlIIN"></asp:Label></h4>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlIIN" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="" />
                                    </asp:DropDownList>
                                </div>
                            </div>



                            <div class="row">
                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblPriority" runat="server" Text="Priority :" AssociatedControlID="ddlPriority"></asp:Label></h4>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="" />
                                        <asp:ListItem Text="High" Value="1" />
                                        <asp:ListItem Text="Medium" Value="2" />
                                        <asp:ListItem Text="Low" Value="3" />
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-2">
                                </div>

                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblRatio" runat="server" Text="Ratio :"></asp:Label></h4>
                                </div>
                                <%--<div class="col-md-3">
                                    <asp:DropDownList ID="ddlRatio" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="" />
                                        <asp:ListItem Text="60,40" Value="60,40" />
                                        <asp:ListItem Text="50,50" Value="50,50" />
                                        <asp:ListItem Text="70,30" Value="70,30" />
                                        <asp:ListItem Text="40,60" Value="40,60" />
                                    </asp:DropDownList>
                                    
                                </div>--%>
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblManualReset" runat="server" Text="Manual/Reset :" AssociatedControlID="ddlManualReset"></asp:Label></h4>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlManualReset" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="" />
                                        <asp:ListItem Text="Manual" Value="1" />
                                        <asp:ListItem Text="Auto" Value="2" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                </div>
                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblFailPercentage" runat="server" Text="Failover % :" AssociatedControlID="txtFPercentage"></asp:Label></h4>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtFPercentage" runat="server" CssClass="form-control" Width="160%" />
                                </div>
                            </div>
                            <!-- Text Boxes -->
                            <div class="row">
                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblTxnCnt" runat="server" Text="Txn Count :" AssociatedControlID="txtTxnCnt"></asp:Label></h4>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtTxnCnt" runat="server" CssClass="form-control" Width="160%" />
                                </div>

                                <div class="col-md-3">
                                </div>

                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblPercentageRouting" runat="server" Text="Routing % :" AssociatedControlID="txtPercentageRouting"></asp:Label></h4>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtPercentageRouting" runat="server" CssClass="form-control" Width="160%" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblTimeframe" runat="server" Text="Failover Timeframe :" AssociatedControlID="txtFTimeframe"></asp:Label></h4>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtFTimeframe" runat="server" CssClass="form-control" Width="160%" />
                                </div>

                                <div class="col-md-3">
                                </div>

                                <div class="col-md-2">
                                    <h4>
                                        <asp:Label ID="lblMinCount" runat="server" Text="Failover MinCount :" AssociatedControlID="txtFMinCount"></asp:Label></h4>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtFMinCount" runat="server" CssClass="form-control" Width="160%" />
                                </div>
                            </div>

                        </div>
                        <!-- Modal footer -->
                        <div class="form-group" style="margin-bottom: 0px; margin-top: -30px; margin-left: 295px; height: 85px">
                            <asp:Button ID="btnSaveAction" runat="server" Text="Save" Style="width: 15%" class="themeBtn themeApplyBtn" OnClick="btnSaveAction_Click" ValidationGroup="AgentReg" />
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
                            <button class="themeBtn resetBtn themeCancelBtn me-0" id="btnCancelAction" style="width: 15%" type="button" runat="server" causesvalidation="false" onserverclick="btnCancelAction_Click">Cancel</button>
                        </div>
                    </div>
                </div>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        document.addEventListener('click', function (event) {
            var isClickInside = document.querySelector('.multi-select-container').contains(event.target);
            if (!isClickInside) {
                document.getElementById('dropdownContent').style.display = 'none';
            }
        });

        function toggleDropdown() {
            debugger;
            var dropdown = document.getElementById('dropdownContent');
            dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
            if (dropdown.style.display === 'block') {
                document.getElementById('searchBox').focus(); // Focus on search box when dropdown is opened
            }
        }

        document.querySelectorAll('#dropdownContent label input').forEach(function (checkbox) {
            checkbox.addEventListener('change', function () {
                updateSelectedItems();
            });
        });

        function updateSelectedItems() {
            const selectedItemsContainer = document.getElementById('selectedItems');
            const dropdownContent = document.getElementById('dropdownContent');
            const hiddenField = document.getElementById('<%= hfSelectedValues.ClientID %>');
            selectedItemsContainer.innerHTML = ''; // Clear previous items
            let hasSelection = false;
            let selectedValues = [];

            document.querySelectorAll('#dropdownContent label input:checked').forEach(function (checkbox) {
                const value = checkbox.value;
                const text = checkbox.parentNode.textContent.trim();
                const selectedItem = document.createElement('div');
                selectedItem.className = 'selected-item';
                selectedItem.innerHTML = `${text} <span class="close" data-value="${value}">&times;</span>`;
                selectedItemsContainer.appendChild(selectedItem);

                selectedValues.push(value);
                hasSelection = true;
            });

            hiddenField.value = selectedValues.join(','); // Update hidden field with selected values

            if (!hasSelection) {
                selectedItemsContainer.innerHTML = '<span class="placeholder">Select options...</span>';
            }

            // Update the close button functionality
            document.querySelectorAll('.selected-item .close').forEach(function (closeBtn) {
                closeBtn.addEventListener('click', function () {
                    const value = this.getAttribute('data-value');
                    document.querySelector(`#dropdownContent label input[value="${value}"]`).checked = false;
                    updateSelectedItems();
                });
            });
        }

        function filterDropdown() {
            const filter = document.getElementById('searchBox').value.toLowerCase();
            document.querySelectorAll('#dropdownContent #dropdownItems label').forEach(label => {
                const text = label.textContent.toLowerCase();
                label.style.display = text.includes(filter) ? 'block' : 'none';
            });
        }
    </script>
    <%--<cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" BackgroundCssClass="modalBackground" DropShadow="false" />--%>
</asp:Content>


