<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SwitchConfig.aspx.cs" Inherits="SOR.Pages.Rule.SwitchConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="../../v3/assets/css/patternfly-adjusted.min.css">
    <link rel="stylesheet" href="../../v3/components/patternfly/dist/css/patternfly-additions.min.css">
    <script src="../../ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="../../v3/components/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="../../v3/components/patternfly-bootstrap-combobox/js/bootstrap-combobox.js"></script>
    <script src="../../ajax/libs/moment_js/2.21.0/moment.min.js"></script>
    <script src="../../v3/components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"></script>
    <script src="../../ajax/libs/bootstrap-slider/9.9.0/bootstrap-slider.min.js"></script>
    <script src="../../v3/components/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="../../ajax/libs/bootstrap-datetimepicker/4.17.47/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../../ajax/libs/d3/3.5.0/d3.min.js"></script>
    <script src="../../ajax/libs/c3/0.4.11/c3.min.js"></script>
    <script src="../../v3/components/datatables/media/js/jquery.dataTables.js"></script>
    <script src="../../v3/components/google-code-prettify/bin/prettify.min.js"></script>
    <script src="../../v3/components/clipboard/dist/clipboard.min.js"></script>
    <script src="../../v3/components/patternfly/dist/js/patternfly.min.js"></script>
    <script src="../../v3/components/patternfly/dist/js/patternfly.dataTables.pfFilter.min.js"></script>
    <script src="../../v3/assets/js/patternfly-site.min.js"></script>
    <%--</head>--%>
    <link rel="stylesheet" href="../../css/sliderbutton.css">
    <%-- Model --%>
    <link rel="stylesheet" href="../../css/bootstrap.min.css">
    <script src="../../v3/assets/js/jquery-3.6.0.min.js"></script>
    <script src="../../v3/assets/js/bootstrap.bundle.min.js"></script>
    <%--  --%>
    <link rel="stylesheet" href="../../css/style.css">

    <style>
        .fade {
            opacity: 1;
        }

        .special-page .main-container {
            padding-top: 0; /* or whatever value you need */
        }
    </style>

    <style>
        .table {
            width: 100%;
            border-collapse: collapse;
        }

            .table th, .table td {
                border: 1px solid #ddd;
                padding: 8px;
            }

        .form-control {
            width: 100%;
        }

        .bordered-section {
            border: 1px solid #ddd; /* Light grey border */
            border-radius: 4px; /* Rounded corners (optional) */
            padding: 15px; /* Space inside the border */
            background-color: #f9f9f9; /* Light background color (optional) */
            margin-bottom: 20px; /* Space below the bordered section */
        }

        .section-header {
            border-bottom: 2px solid #007bff; /* Blue bottom border for the header */
            padding-bottom: 10px; /* Space below the header text */
            margin-bottom: 15px; /* Space between header and content */
        }

            .section-header h3 {
                margin: 0; /* Remove default margin from heading */
                color: #007bff; /* Blue text color */
            }

        .radio-spacing {
            margin-right: 20px; /* Space between radio buttons */
        }

        .toggle-field {
            margin-bottom: 15px; /* Space between fields inside the panel */
        }
    </style>

    <style>
        .toggle-field {
            display: none; /* Ensure this does not override JavaScript */
        }
    </style>
    <style>
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        th, td {
            border: 1px solid #ccc;
            padding: 10px;
            text-align: left;
        }
    </style>

    <script>
        $(document).ready(function () {

            $('.clsslider').change(function () {
                var isChecked = $(this).is(':checked');
                var hdnGroupId = $(this).closest('.list-group-item').find('input[type="hidden"]').val();


                $(this).prop('disabled', true);

                $.ajax({
                    type: 'POST',
                    url: 'SwitchConfig.aspx/ToggleSlider',
                    data: JSON.stringify({ IsChecked: isChecked, Id: hdnGroupId }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (response) {
                        var data = JSON.parse(response.d);
                        //alert(data.StatusMessage);
                        showSuccess(data.StatusMessage)
                        $('.clsslider').prop('disabled', false);
                    },
                    error: function (error) {
                        alert('An error occurred: ' + error.responseText);
                        showWarning(data.StatusMessage)
                        $('.clsslider').prop('disabled', false);
                    }
                });
            });
        });
    </script>

    <%--<script>
        $(document).ready(function () {
            $('#Switch').change(function () {
                var isChecked = $(this).is(':checked');

                // Disable the checkbox while processing
                $(this).prop('disabled', true);

                // AJAX call to the server to determine which panel to show and which textbox to clear
                $.ajax({
                    type: 'POST',
                    url: 'SwitchConfig.aspx/ToggleSwitch',
                    data: JSON.stringify({ IsChecked: isChecked }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (response) {
                        var result = response.d;

                        // Get the client IDs of the panels and textboxes
                        var pnlPercentageID = '<%= pnlPercentage.ClientID %>';
                        var pnlCountID = '<%= pnlCount.ClientID %>';
                        var txtSwitchPercentageID = '<%= txtSwitchPercentage.ClientID %>';
                        var txtCountID = '<%= txtCount.ClientID %>';

                        // Check the result and show/hide panels accordingly
                        if (result === "showCount-clearPercentage") {
                            // Show Count panel, hide Percentage panel, and clear Percentage textbox
                            $('#' + pnlPercentageID).hide();
                            $('#' + pnlCountID).show();
                            $('#' + txtSwitchPercentageID).val(''); // Clear Percentage textbox
                        } else if (result === "showPercentage-clearCount") {
                            // Show Percentage panel, hide Count panel, and clear Count textbox
                            $('#' + pnlCountID).hide();
                            $('#' + pnlPercentageID).show();
                            $('#' + txtCountID).val(''); // Clear Count textbox
                        } else {
                            console.log("Unexpected result:", result);
                        }

                        // Enable the checkbox after processing
                        $('#Switch').prop('disabled', false);
                    },
                    error: function (error) {
                        console.error('An error occurred:', error);
                        alert('An error occurred: ' + error.responseText);

                        // Enable the checkbox after an error
                        $('#Switch').prop('disabled', false);
                    }
                });
            });
        });
    </script>

    <script>
        function callToggleSwitch(isChecked) {
            $.ajax({
                type: 'POST',
                url: 'SwitchConfig.aspx/ToggleSwitch', // Ensure this is correct
                data: JSON.stringify({ IsChecked: isChecked }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var result = response.d;
                    var pnlPercentageID = '<%= pnlPercentage.ClientID %>';
                    var pnlCountID = '<%= pnlCount.ClientID %>';
                    var txtSwitchPercentageID = '<%= txtSwitchPercentage.ClientID %>';
                    var txtCountID = '<%= txtCount.ClientID %>';

                    // Check the result and show/hide panels accordingly
                    if (result === "showCount-clearPercentage") {
                        // Show Count panel, hide Percentage panel, and clear Percentage textbox
                        $('#' + pnlPercentageID).hide();
                        $('#' + pnlCountID).show();
                        $('#' + txtSwitchPercentageID).val(''); // Clear Percentage textbox
                    } else if (result === "showPercentage-clearCount") {
                        // Show Percentage panel, hide Count panel, and clear Count textbox
                        $('#' + pnlCountID).hide();
                        $('#' + pnlPercentageID).show();
                        $('#' + txtCountID).val(''); // Clear Count textbox
                    } else {
                        console.log("Unexpected result:", result);
                    }

                    // Enable the checkbox after processing
                    $('#Switch').prop('disabled', false);
                },
                error: function (error) {
                    console.error('An error occurred:', error);
                    alert('An error occurred: ' + error.responseText);
                }
            });
        }
    </script>--%>

    <style>
        *, :after, :before {
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box
        }

        body {
            font-size: 13px;
            padding-top: 0;
            margin-bottom: 158px;
        }

        body {
            margin: 0;
            font-family: "Open Sans", Helvetica, Arial, sans-serif;
            font-size: 12px;
            line-height: 1.66666667;
            color: #363636;
        }

        html {
            font-size: 10px;
            -webkit-tap-highlight-color: transparent;
        }

        html {
            font-family: sans-serif;
            -ms-text-size-adjust: 100%;
            -webkit-text-size-adjust: 100%;
        }
    </style>

    <style>
        .sidebar-headings {
            font-size: small;
        }

        .list-line-text {
            font-size: x-small;
        }
    </style>
    <style>
        table {
            width: 50%; /* Set width to half */
            border-collapse: collapse;
            margin-top: 20px;
            height: 200px; /* Set fixed height */
            overflow-y: auto; /* Enable scrolling if content overflows */
            display: block; /* Make it block to control overflow */
        }

        th, td {
            border: 1px solid #ccc;
            padding: 5px; /* Set padding for smaller row height */
            text-align: left;
            height: 30px; /* Set fixed height for rows */
        }

        tbody {
            display: block; /* Make tbody block for fixed height */
            height: 150px; /* Set height to limit visible rows */
            overflow-y: auto; /* Enable scrolling for rows */
        }

            thead, tbody tr {
                display: table; /* Keep table structure intact */
                width: 100%; /* Ensure full width */
                table-layout: fixed; /* Make sure the columns have fixed width */
            }
    </style>
    <style>
        .tab-container {
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            background-color: white; /* Tab container background */
        }

        .tab-buttons {
            display: flex; /* Flexbox for horizontal layout */
            background-color: #ffffff; /* Tab buttons background */
            border-bottom: 2px solid #007bff; /* Bottom border for active tab indication */
        }

        .tab-button {
            flex: none; /* Prevent stretching */
            padding: 8px 12px; /* Reduced padding for compactness */
            text-align: center;
            cursor: pointer;
            color: #007bff; /* Text color */
            background-color: transparent; /* No background */
            border: none; /* Remove border */
            position: relative; /* For underline positioning */
            transition: color 0.3s, background-color 0.3s;
            border-radius: 5px 5px 0 0; /* Slightly rounded corners for top */
        }

            .tab-button:hover {
                background-color: rgba(0, 123, 255, 0.1); /* Light hover effect */
            }

            .tab-button.active {
                color: #ffffff; /* Active tab text color */
                background-color: #007bff; /* Active tab background color */
                box-shadow: 0 -2px 5px rgba(0, 0, 0, 0.2); /* Shadow for active tab */
            }



        .tab {
            display: none;
        }

        .active {
            display: block;
        }

        .tab-content {
            border: 1px solid #ccc; /* Border around content */
            border-radius: 5px; /* Rounded corners */
            padding: 15px; /* Padding for content */
            background-color: white; /* Content background */
        }

        table {
            width: 100%; /* Full width */
            border-collapse: collapse; /* Remove gaps between cells */
        }

        th, td {
            padding: 10px; /* Padding inside cells */
            text-align: left; /* Align text to the left */
            border-bottom: 1px solid #ddd; /* Border below rows */
        }

        th {
            background-color: #f2f2f2; /* Background for header */
        }
    </style>

    <style>
        .toast-success {
            background-color: #51A351;
        }

        .toast-warning {
            background-color: #F89406;
        }

        .textboxx {
            margin-left: 25px;
            width: 76%;
        }
    </style>

    <script>
        function showTab(tabId) {
            // Hide all tabs
            document.querySelectorAll('.tab').forEach(tab => {
                tab.classList.remove('active');
            });

            // Remove active class from all buttons
            document.querySelectorAll('.tab-button').forEach(button => {
                button.classList.remove('active');
            });

            // Show the selected tab and activate its button
            document.getElementById(tabId).classList.add('active');
            document.querySelector(`.tab-button[onclick="showTab('${tabId}')"]`).classList.add('active');
        }
    </script>
    <style>
        .col-md-1 {
            flex: 0 0 auto;
            width: 6.333333%;
        }

        .col-md-11 {
            flex: 0 0 auto;
            width: 8.0%;
        }

        .col-md-111 {
            flex: 0 0 auto;
            width: 5.333333%;
        }

        .col-md-1111 {
            flex: 0 0 auto;
            width: 10.8%;
        }

        .col-md-11111 {
            flex: 0 0 auto;
            width: 4.333333%;
        }

        .col-md-22 {
            flex: 0 0 auto;
            width: 4.333333%;
        }

        .col-md-23 {
            flex: 0 0 auto;
            width: 4.3%;
        }

        .col-md-24 {
            flex: 0 0 auto;
            width: 26.0%;
        }

        .col-md-25 {
            flex: 0 0 auto;
            width: 3.9%;
        }
    </style>
    <style>
        .list-view-pf-additional-info {
            display: flex; /* Use flexbox to align items in a row */
            gap: 60px; /* Add a gap between each item */
            align-items: center; /* Center items vertically */
        }

        .list-view-pf-additional-info-item {
            display: flex;
            align-items: center;
        }

            /* Optional: add some margin to the items if needed */
            .list-view-pf-additional-info-item img {
                margin-right: 5px; /* Adds space between the icon and text */
            }

        .shift-left {
            position: relative;
            left: -70px; /* Moves the content 25px to the left */
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <asp:HiddenField ID="hdnSwitchId" runat="server" Value="false" />
    <div class="breadHeader">
        <h5 class="page-title" style="font-size: larger">Switch Configuration</h5>
    </div>
    <%--<div class="row">
        <div class="col-md-2">
        </div>
        <div class="col-md-2">
        </div>
        <div class="col-md-2">
        </div>
        <div class="col-md-2">
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnManual" runat="server" CssClass="btn btn-primary" Text="Manual" OnClick="btnManual_Click" Style="margin: 19px; margin-left: 60px;" />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnAddSwitch" runat="server" CssClass="btn btn-primary" Text="Add Switch" OnClick="btnAddSwitch_Click" Style="margin: 19px; margin-left: 60px;" />
        </div>
    </div>--%>
    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-2"></div>
        <div class="col-md-2"></div>
        <div class="col-md-2"></div>
        <div class="col-md-2"></div>
        <div class="col-md-2">
            <asp:Button ID="btnManual" runat="server" CssClass="btn btn-primary" Text="Manual" OnClick="btnManual_Click" />
            &nbsp;&nbsp;
            <asp:Button ID="btnAddSwitch" runat="server" CssClass="btn btn-primary" Text="Add Switch" OnClick="btnAddSwitch_Click" />
        </div>
        &nbsp;&nbsp;
    </div>
    <!-- Modal Group -->
    <div class="modal fade" id="exampleModalManual" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabelManual">Manual Details</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <!-- Body -->
                <div class="modal-body">
                    <asp:Label ID="Label1" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                    <!-- Table -->
                    <table class="table table-striped mb-3">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Switch Status</th>
                                <th>IsManual</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptManual" runat="server" OnItemCommand="rptManual_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("switchname") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("description") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSwitchStatus" runat="server" Text='<%# Eval("switchstatus") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:TextBox ID="txtPercentage" runat="server" Text='<%# Eval("percentage") %>' CssClass="form-control" TextMode="Number" Style="width: 80px;" Enabled="false"></asp:TextBox>--%>
                                            <%--<asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/edit.png" Width="20px" Height="20px" CommandName="Edit" CommandArgument='<%# Eval("id") %>' />--%>
                                            <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/images/update.png" Width="20px" Height="20px" CommandName="Update" CommandArgument='<%# Eval("id") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <!-- Footer -->
                <div class="modal-footer">
                    <%--<asp:Button ID="btnUpdManual" CssClass="btn btn-primary" runat="server" Text="Submit" OnClick="btnUpdManual_Click" />--%>
                    <asp:Button ID="btnClsManual" CssClass="btn btn-secondary" runat="server" Text="Close" OnClick="btnClsManual_Click" data-bs-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnshowmanual" runat="server" Value="false" />
    <%-- End --%>
    <!-- Modal Group -->
    <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Switch Details</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <!-- Body -->
                <div class="modal-body">
                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                    <!-- Table -->
                    <table class="table table-striped mb-3">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Switch Status</th>
                                <th>Percentage</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSwitchDetails" runat="server" OnItemCommand="rptSwitchDetails_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("switchname") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("description") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSwitchStatus" runat="server" Text='<%# Eval("switchstatus") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPercentage" runat="server" Text='<%# Eval("percentage") %>' CssClass="form-control" TextMode="Number" Style="width: 80px;" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/edit.png" Width="20px" Height="20px" CommandName="Edit" CommandArgument='<%# Eval("id") %>' />
                                            <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/images/update.png" Width="20px" Height="20px" CommandName="Update" CommandArgument='<%# Eval("id") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <div class="bordered-section">
                        <div class="section-header">
                            <h3>Switch Details</h3>
                        </div>
                        <div class="bordered-section">

                            <div class="row mb-4">
                                <!-- Row for both parts -->
                                <div class="col-md-6">
                                    <!-- Part A -->
                                    <div class="form-group">
                                        <label for="SwitchName" class="col-form-label">Name</label>
                                        <asp:TextBox ID="txtSwitchName" CssClass="form-control" runat="server" placeholder="Enter Name"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="SwitchDescription" class="col-form-label">Description</label>
                                        <asp:TextBox ID="txtSwitchDescription" CssClass="form-control" TextMode="MultiLine" Rows="3" runat="server" placeholder="Enter Description"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="txtSwitchPercentage" class="col-form-label">Percentage %</label>
                                        <asp:TextBox ID="txtSwitchPercentage" CssClass="form-control" runat="server" MaxLength="3" placeholder="Enter Percentage"></asp:TextBox>
                                    </div>
                                    <%--<div class="row mb-3">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <label class="col-md-4 col-form-label">Switch Count</label>
                                                <div class="col-md-8">
                                                    <label class="switchh">
                                                        <input type="checkbox" id="Switch" />
                                                        <span class="sliderr"></span>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <!-- Panels for Percentage and Count -->
                                    <%--<div class="row mb-3">

                                        <div class="form-group row">--%>
                                            <!-- Panel for Percentage -->
                                            <%--<asp:Panel ID="pnlPercentage" runat="server">
                                                <label for="txtSwitchPercentage" class="col-md-5 col-form-label">Percentage %</label>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="txtSwitchPercentage" CssClass="form-control" runat="server" MaxLength="3" placeholder="Enter Percentage"></asp:TextBox>
                                                </div>
                                            </asp:Panel>--%>
                                            <!-- Panel for Count -->
                                            <%--<asp:Panel ID="pnlCount" runat="server" CssClass="toggle-field">
                                                <label for="txtCount" class="col-md-5 col-form-label">Count</label>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="txtCount" CssClass="form-control" runat="server" placeholder="Enter Count"></asp:TextBox>
                                                </div>
                                            </asp:Panel>--%>
                                        <%--</div>

                                    </div>--%>

                                </div>

                                <div class="col-md-6">
                                    <!-- Part B -->
                                    <div class="tab-container">
                                        <div class="tab-buttons">
                                            <div class="tab-button active" onclick="showTab('tab1')">AEPS</div>
                                            <div class="tab-button" onclick="showTab('tab2')">DMT</div>
                                            <div class="tab-button" onclick="showTab('tab3')">MATM</div>
                                        </div>

                                        <div id="tab1" class="tab active">
                                            <div class="tab-content">
                                                <table class="table table-responsive">
                                                    <thead>
                                                        <tr>
                                                            <th>Txn Type</th>
                                                            <th>Input Value</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody id="divAEPS" runat="server">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div id="tab2" class="tab">
                                            <div class="tab-content">
                                                <table class="table table-responsive">
                                                    <thead>
                                                        <tr>
                                                            <th>Txn Type</th>
                                                            <th>Input Value</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody id="divDMT" runat="server">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div id="tab3" class="tab">
                                            <div class="tab-content">
                                                <table class="table table-responsive">
                                                    <thead>
                                                        <tr>
                                                            <th>Txn Type</th>
                                                            <th>Input Value</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody id="divMATM" runat="server">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="bordered-section">
                        <div class="section-header">
                            <h3>Failoversss Details</h3>
                        </div>

                        <!-- First Row -->
                        <div class="container">
                            <!-- First Row -->

                            <div class="form-group row mb-3">
                                <div class="col-md-4">

                                    <label for="ddlSwitch1" class="col-form-label">S-1</label>
                                    <asp:DropDownList ID="ddlSwitch1" runat="server" CssClass="maximus-select w-100" Width="77%" AutoPostBack="true" OnSelectedIndexChanged="ddlSwitch1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtswitch1" runat="server" MaxLength="3" CssClass="form-control mt-2 textboxx" placeholder="Enter Switch-1 %" />
                                </div>
                                <div class="col-md-4">
                                    <label for="ddlSwitch2" class="col-form-label">S-2</label>
                                    <asp:DropDownList ID="ddlSwitch2" runat="server" CssClass="maximus-select w-100" Width="77%" AutoPostBack="true" OnSelectedIndexChanged="ddlSwitch2_SelectedIndexChanged" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtswitch2" runat="server" MaxLength="3" CssClass="form-control mt-2 textboxx" placeholder="Enter Switch-2 %" Enabled="false" />
                                </div>
                                <div class="col-md-4">
                                    <label for="ddlSwitch3" class="col-form-label">S-3</label>
                                    <asp:DropDownList ID="ddlSwitch3" runat="server" CssClass="maximus-select w-100" Width="77%" AutoPostBack="true" OnSelectedIndexChanged="ddlSwitch3_SelectedIndexChanged" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtswitch3" runat="server" MaxLength="3" CssClass="form-control mt-2 textboxx" placeholder="Enter Switch-3 %" Enabled="false" />
                                </div>
                            </div>

                            <!-- Second Row -->
                            <div class="form-group row mb-3">
                                <div class="col-md-4">
                                    <label for="ddlSwitch4" class="col-form-label">S-4</label>
                                    <asp:DropDownList ID="ddlSwitch4" runat="server" CssClass="maximus-select w-100" Width="77%" AutoPostBack="true" OnSelectedIndexChanged="ddlSwitch4_SelectedIndexChanged" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtswitch4" runat="server" MaxLength="3" CssClass="form-control mt-2 textboxx" placeholder="Enter Switch-4 %" Enabled="false" />
                                </div>
                                <div class="col-md-4">
                                    <label for="ddlSwitch5" class="col-form-label">S-5</label>
                                    <asp:DropDownList ID="ddlSwitch5" runat="server" CssClass="maximus-select w-100" Width="77%" AutoPostBack="true" OnSelectedIndexChanged="ddlSwitch5_SelectedIndexChanged" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtswitch5" runat="server" MaxLength="3" CssClass="form-control mt-2 textboxx" placeholder="Enter Switch-5 %" Enabled="false" />
                                </div>
                                <div class="col-md-4">
                                    <label for="ddlSwitch6" class="col-form-label">S-6</label>
                                    <asp:DropDownList ID="ddlSwitch6" runat="server" CssClass="maximus-select w-100" Width="77%" AutoPostBack="true" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtswitch6" runat="server" MaxLength="3" CssClass="form-control mt-2 textboxx" placeholder="Enter Switch-6 %" Enabled="false" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <!-- Footer -->
                <div class="modal-footer">
                    <asp:Button ID="btnCreSwitch" CssClass="btn btn-primary" runat="server" Text="Submit" OnClick="btnCreSwitch_Click" />
                    <asp:Button ID="btnCloseSwitch" CssClass="btn btn-secondary" runat="server" Text="Cancel" OnClick="btnCloseSwitch_Click" data-bs-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnShowModal" runat="server" Value="false" />
    <%-- End --%>

    <!-- Modal Group -->
    <div class="modal fade" id="exampleModalDelete" tabindex="-1" aria-labelledby="exampleModalLabelDelete" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabelDelete">Switch Details</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <!-- Body -->
                <div class="modal-body">
                    <asp:Label ID="lblDeleteMessage" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                    <!-- Table -->
                    <table class="table table-striped mb-3">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Switch Status</th>
                                <th>Percentage</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSwitchDetailsDelete" runat="server" OnItemCommand="rptSwitchDetails_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("switchname") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("description") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSwitchStatus" runat="server" Text='<%# Eval("switchstatus") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPercentage" runat="server" Text='<%# Eval("percentage") %>' CssClass="form-control" TextMode="Number" Style="width: 80px;" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/edit.png" Width="20px" Height="20px" CommandName="Edit" CommandArgument='<%# Eval("id") %>' />
                                            <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/images/update.png" Width="20px" Height="20px" CommandName="Update" CommandArgument='<%# Eval("id") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <div class="bordered-section">
                        <div class="section-header">
                            <h3>Switch Details</h3>
                        </div>
                        <div class="bordered-section">

                            <div class="row mb-4">
                                <!-- Row for both parts -->
                                <div class="col-md-6">
                                    <!-- Part A -->
                                    <div class="form-group">
                                        <label for="SwitchName" class="col-form-label">Switch Name</label>
                                        <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" placeholder="Enter Switch Name"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="SwitchDescription" class="col-form-label">Switch Description</label>
                                        <asp:TextBox ID="TextBox2" CssClass="form-control" TextMode="MultiLine" Rows="3" runat="server" placeholder="Enter Switch Description"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="txtSwitchPercentage" class="col-form-label">Percentage %</label>
                                        <asp:TextBox ID="TextBox3" CssClass="form-control" runat="server" MaxLength="3" placeholder="Enter Percentage"></asp:TextBox>
                                    </div>

                                    <%--<div class="row mb-3">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <label class="col-md-4 col-form-label">Switch Count</label>
                                                <div class="col-md-8">
                                                    <label class="switchh">
                                                        <input type="checkbox" id="Switch" />
                                                        <span class="sliderr"></span>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <!-- Panels for Percentage and Count -->
                                    <%--<div class="row mb-3">

                                        <div class="form-group row">
                                            <asp:Panel ID="Panel1" runat="server">
                                                <label for="txtSwitchPercentage" class="col-md-5 col-form-label">Percentage %</label>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="TextBox3" CssClass="form-control" runat="server" MaxLength="3" placeholder="Enter Percentage"></asp:TextBox>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel2" runat="server" CssClass="toggle-field">
                                                <label for="txtCount" class="col-md-5 col-form-label">Count</label>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="TextBox4" CssClass="form-control" runat="server" placeholder="Enter Count"></asp:TextBox>
                                                </div>
                                            </asp:Panel>
                                        </div>

                                    </div>--%>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <!-- Footer -->
                <div class="modal-footer">
                    <asp:Button ID="btnDelete" CssClass="btn btn-primary" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                    <asp:Button ID="btnCloseDel" CssClass="btn btn-secondary" runat="server" Text="Cancel" OnClick="btnCloseDel_Click" data-bs-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HdnShowDelete" runat="server" Value="false" />
    <%-- End --%>

    <div id="pf-list-simple-expansion" class="list-group list-view-pf list-view-pf-view">

        <asp:Repeater ID="rptrSwitch" runat="server" OnItemDataBound="rptrSwitch_ItemDataBound" OnItemCommand="rptrSwitch_ItemCommand">
            <HeaderTemplate>
                <div class="row" style="width: 103%; background-color: #fbd2ce; height: 30px; font-size: medium; align-items: center; font-weight: bold;">
                    <%--<div class="row">--%>
                    <div class="col-md-25"></div>
                    <div class="col-md-1111">Name</div>
                    <div class="col-md-3">Description</div>
                    <div class="col-md-11"></div>
                    <%--<div class="col-md-111"></div>--%>
                    <%--<div class="col-md-22"></div>--%>
                    <div class="col-md-11111">Priority</div>
                    <div class="col-md-23"></div>
                    <div class="col-md-1">Status</div>
                    <div class="col-md-24"></div>
                    <div class="col-md-1111">Action</div>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:HiddenField ID="hd1" Value='<%# Eval("id") %>' runat="server" />
                <div id="pf-list-simple-expansion" class="list-group list-view-pf list-view-pf-view">
                    <div class="list-group-item">
                        <div class="list-group-item-header">
                            <%--<div class="list-view-pf-expand">
                                <span class="fa fa-angle-right"></span>
                            </div>--%>
                            <%--<div class="list-view-pf-checkbox">
                                <input type="checkbox">
                            </div>--%>

                            <div class="list-view-pf-actions shift-left">

                                <label class="switchh">
                                    <input type="checkbox" runat="server" id="chkSlider" class="clsslider" onitemdatabound="">
                                    <span class="sliderr"></span>
                                </label>

                                <span class="slider round"></span>
                                <div class="dropdown pull-right dropdown-kebab-pf">
                                    <button class="btn btn-link dropdown-toggle" type="button" id="dropdownKebabRight9" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                        <span class="fa fa-ellipsis-v"></span>
                                    </button>
                                    <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownKebabRight9">
                                        <li>
                                            <asp:HiddenField ID="HiddenField1" Value='<%# Eval("id") %>' runat="server" />
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("id") %>' Text="Edit" />
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete" CommandArgument='<%# Eval("id") %>' Text="Delete" />
                                        </li>
                                    </ul>
                                </div>

                            </div>

                            <div class="list-view-pf-main-info">
                                <div class="list-view-pf-left">
                                    <%--<span class="fa fa-plane list-view-pf-icon-sm"></span>--%>
                                    <img src="../../images/icons/switch.png" style="width: 25px; height: 25px;" />
                                </div>

                                <div class="list-view-pf-body">
                                    <div class="list-view-pf-description">
                                        <div class="list-group-item-heading">
                                            <%# Eval("switchname") %>
                                        </div>
                                        <div class="list-group-item-text">
                                            <%# Eval("description") %>
                                        </div>
                                    </div>
                                    <input type="hidden" id="hdnGroupId" value='<%# Eval("id") %>' />
                                    <div class="list-view-pf-additional-info">
                                        <div class="list-view-pf-additional-info-item">
                                            <%--<span class="pficon pficon-screen"></span>--%>
                                            <img src="../../images/icons/rules_2.png" style="width: 25px; height: 25px;" />
                                            <strong><%# Eval("percentage") %></strong>

                                        </div>
                                        <div class="list-view-pf-additional-info-item">
                                            <%--<span class="pficon pficon-cluster"></span>--%>
                                            <img src="../../images/icons/status.png" style="width: 25px; height: 25px;" />
                                            <strong><%# Eval("switchstatus") %></strong>

                                        </div>
                                        <%--<div class="list-view-pf-additional-info-item">
                                            <span class="pficon pficon-container-node"></span>
                                            <strong><%# Eval("isactive") %></strong>

                                        </div>
                                        <div class="list-view-pf-additional-info-item">
                                            <span class="pficon pficon-image"></span>
                                            <strong>8</strong> Images
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <script>
        $(document).ready(function () {
            // Row Checkbox Selection
            $("#pf-list-simple-expansion input[type='checkbox']").change(function (e) {
                if ($(this).is(":checked")) {
                    $(this).closest('.list-group-item').addClass("active");
                } else {
                    $(this).closest('.list-group-item').removeClass("active");
                }
            });
            // toggle dropdown menu
            $("#pf-list-simple-expansion .list-view-pf-actions").on('show.bs.dropdown', function () {
                var $this = $(this);
                var $dropdown = $this.find('.dropdown');
                var space = $(window).height() - $dropdown[0].getBoundingClientRect().top - $this.find('.dropdown-menu').outerHeight(true);
                $dropdown.toggleClass('dropup', space < 10);
            });

            // click the list-view heading then expand a row
            $("#pf-list-simple-expansion .list-group-item-header").click(function (event) {
                if (!$(event.target).is("button, a, input, .fa-ellipsis-v")) {
                    $(this).find(".fa-angle-right").toggleClass("fa-angle-down")
                        .end().parent().toggleClass("list-view-pf-expand-active")
                        .find(".list-group-item-container").toggleClass("hidden");
                } else {
                }
            })

            $("#pf-list-simple-expansion .list-group-item-header_Next").click(function (event) {
                if (!$(event.target).is("button, a, input, .fa-ellipsis-v")) {
                    $(this).find(".fa-angle-right").toggleClass("fa-angle-down")
                        .end().parent().toggleClass("list-view-pf-expand-active")
                        .find(".list-group-item-container_next").toggleClass("hidden");
                } else {
                }
            })

            // click the close button, hide the expand row and remove the active status
            $("#pf-list-simple-expansion .list-group-item-container .close").on("click", function () {
                $(this).parent().addClass("hidden")
                    .parent().removeClass("list-view-pf-expand-active")
                    .find(".fa-angle-right").removeClass("fa-angle-down");
            })

            // Initialize Donut Charts
            // (You may need to dynamically initialize charts based on item index)
        });

    </script>

    <script>
        $(document).ready(function () {
            var showModal = document.getElementById('<%= hdnShowModal.ClientID %>').value;

            if (showModal === "true") {
                var exampleModal = new bootstrap.Modal(document.getElementById('exampleModal'));
                exampleModal.show();
            }
        });
    </script>
    <script>
        $(document).ready(function () {
            var showModal = document.getElementById('<%= hdnshowmanual.ClientID %>').value;

            if (showModal === "true") {
                var exampleModalManual = new bootstrap.Modal(document.getElementById('exampleModalManual'));
                exampleModalManual.show();
            }
        });
    </script>
    <%-- Delete --%>
    <script>
        $(document).ready(function () {
            var showModal = document.getElementById('<%= HdnShowDelete.ClientID %>').value;

            if (showModal === "true") {
                var exampleModal = new bootstrap.Modal(document.getElementById('exampleModalDelete'));
                exampleModal.show();
            }
        });
    </script>
</asp:Content>
