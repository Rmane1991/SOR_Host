<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TrRule.aspx.cs" Inherits="SOR.Pages.Rule.TrRule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<head>--%>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Standard List View | PatternFly</title>
    <link rel="canonical" href="https://pf3.patternfly.org" />
    <meta name="description" content="PatternFly is an open source web user interface framework promoting consistency and usability across IT applications through UX patterns and widgets">
    <link rel="shortcut icon" href="../../v3/components/patternfly/dist/img/favicon.ico">
    <!-- iPad retina icon -->
    <link rel="apple-touch-icon-precomposed" sizes="152x152" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-152.png">
    <!-- iPad retina icon (iOS < 7) -->
    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-144.png">
    <!-- iPad non-retina icon -->
    <%--<link rel="apple-touch-icon-precomposed" sizes="76x76" href=../../"v3/components/patternfly/dist/img/apple-touch-icon-precomposed-76.png">--%>
    <!-- iPad non-retina icon (iOS < 7) -->
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-72.png">
    <!-- iPhone 6 Plus icon -->
    <link rel="apple-touch-icon-precomposed" sizes="120x120" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-180.png">
    <!-- iPhone retina icon (iOS < 7) -->
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-114.png">
    <!-- iPhone non-retina icon (iOS < 7) -->
    <link rel="apple-touch-icon-precomposed" sizes="57x57" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-57.png">
    <link rel="stylesheet" href="../../v3/assets/css/patternfly-adjusted.min.css">
    <link rel="stylesheet" href="../../v3/assets/css/patternfly-adjusted.min.css">
    <link rel="stylesheet" href="../../v3/assets/css/patternfly-adjusted.min.css">
    <link rel="stylesheet" href="../../v3/components/patternfly/dist/css/patternfly-additions.min.css">
    <link rel="stylesheet" href="../../v3/assets/css/patternfly-site.min.css">
    <script type="text/javascript">
        window.IMAGE_PATH = "/v3/assets/img"
    </script>
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
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels"></script>

    <%--mid sorting--%>
    <!-- jQuery library -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- jQuery UI library -->
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <!-- jQuery UI CSS (optional, for default styling) -->
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <!-- jQuery UI Touch Punch -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui-touch-punch/0.2.3/jquery.ui.touch-punch.min.js"></script>
    <%--mi sorting--%>


    <script type="text/javascript">
        function handleActionClick(groupId) {
            // Perform an AJAX call or redirect with the groupId
            alert('Group ID: ' + groupId);

            // Example of redirect
            window.location.href = 'YourPage.aspx?groupId=' + groupId;

            return false; // Prevent default button action if any
        }
    </script>
    <script>
        $(document).ready(function () {
            $('.clsslider').change(function () {
                var isChecked = $(this).is(':checked');
                var hdnGroupId = $(this).closest('.list-group-item').find('input[type="hidden"]').val();


                $(this).prop('disabled', true);

                $.ajax({
                    type: 'POST',
                    url: 'TrRule.aspx/ToggleSlider',
                    data: JSON.stringify({ IsChecked: isChecked, Id: hdnGroupId }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (response) {
                        var data = JSON.parse(response.d);
                        //alert(data.StatusMessage);
                        showSuccess(data.StatusMessage)
                        $('.clsslider').prop('disabled', false);
                        window.location.replace(window.location.href);
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
    <script>
        $(document).ready(function () {
            $('.clssliderRule').change(function () {
                var isChecked = $(this).is(':checked');
                //var hdnRuleId = $(this).closest('.list-group-item').find('input[type="hidden"]').val();
                //var hdnRuleId = $(this).closest('.list-group-item').find('input[id$="hdRuleId"]').val();
                var hdnRuleId = $(this).closest('.list-group-item-container').find('input[id="hdRuleId"]').val();

                $(this).prop('disabled', true);

                $.ajax({
                    type: 'POST',
                    url: 'TrRule.aspx/ToggleRuleSlider',
                    data: JSON.stringify({ IsChecked: isChecked, Id: hdnRuleId }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (response) {
                        var data = JSON.parse(response.d);
                        //alert(data.StatusMessage);
                        showSuccess(data.StatusMessage)
                        $('.clssliderRule').prop('disabled', false);
                        //window.location.replace(window.location.href);
                    },
                    error: function (error) {
                        alert('An error occurred: ' + error.responseText);
                        showWarning(data.StatusMessage)
                        $('.clssliderRule').prop('disabled', false);
                    }
                });
            });
        });
    </script>
    <style>
        /*priority*/
        .priority-label {
            cursor: pointer;
            /*border-radius: 12px;*/
            border-color: black;
        }

            .priority-label.active {
                background-color: #0d6efd;
                color: white;
            }
    </style>

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
            color: black;
        }

        .search-box {
            padding: 5px;
            border-bottom: 1px solid #ccc;
            width: 100%;
        }
    </style>

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
    <script>
        $(document).ready(function () {
            $('#ddlTxn').change(function () {
                if ($(this).is(':checked')) {
                    // Show the second dropdown if the toggle is checked
                    $('#dropdownFinNonFin').show();
                } else {
                    // Hide the second dropdown if the toggle is unchecked
                    $('#dropdownFinNonFin').hide();
                }
            });
        });
        $(document).ready(function () {
            $('#Switch').change(function () {
                if ($(this).is(':checked')) {
                    $('#countDiv').show();
                } else {
                    $('#countDiv').hide();
                }
            });
        });

    </script>

    <style>
        .toast-success {
            background-color: #51A351;
        }

        .toast-warning {
            background-color: #F89406;
        }
    </style>

    <%--mi for garph & details Container div used in repeater  --%>
    <style>
        .details-container {
            padding: 15px;
            background-color: #f9f9f9;
            border: 1px solid #ddd;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            margin-top: 20px;
            margin-left: 130px;
            width: max-content;
            margin-bottom: 10PX;
        }

        .details-header {
            font-size: 24px;
            font-weight: bold;
            color: #333;
            margin-bottom: 10px;
        }

        .sub-header {
            font-size: 16px;
            color: #777;
            margin-bottom: 20px;
        }

        .dl-horizontal {
            display: block; /* Ensure block display */
        }

            .dl-horizontal dt {
                font-weight: bold;
                color: #555;
                text-align: left; /* Align dt to the left */
                margin: 0; /* Remove margin */
            }

            .dl-horizontal dd {
                margin: 0 0 5px; /* Set bottom margin to 5px */
                color: #666;
                line-height: 1.5;
            }

        .btn-primary {
            margin-top: 2px;
            transition: background-color 0.3s ease;
        }

            .btn-primary:hover {
                background-color: #0056b3; /* Darker blue on hover */
            }

        .adujustcl {
            float: left;
        }

        .sortable-placeholder {
            height: 50px; /* Match the height of your rows */
            background: #f0f0f0;
            border: 1px dashed #ccc;
        }
        /* Dropdown start */
        .dropdown-custom {
            width: 100%; /* Ensure it takes full width */
            height: 32px;
            padding: 10px; /* Add some padding */
            border: 1px solid #d3d3d3; /* Light grey border */
            border-radius: 5px; /* Rounded corners */
            background-color: #fff; /* Background color */
            font-size: 11px; /* Font size */
            color: #333; /* Text color */
            appearance: none; /* Remove default styling */
            background-image: url('data:image/png;base64,...'); /* Custom dropdown icon */
            background-repeat: no-repeat;
            background-position: right 10px center; /* Position the icon */
            background-size: 12px; /* Size of the icon */
        }

            .dropdown-custom:focus {
                outline: none; /* Remove outline on focus */
                border-color: #a9a9a9; /* Slightly darker grey on focus */
                box-shadow: 0 0 5px rgba(169, 169, 169, .5); /* Grey box shadow on focus */
            }

            .dropdown-custom option {
                padding: 10px; /* Add padding for options */
                color: #333; /* Text color for options */
            }



        /* Dropdown End */
    </style>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css">
    <style>
        /*#toggleButton {
            cursor: pointer; 
            background-color: #f0f0f0; 
            border: none; 
            padding: 10px;
            border-radius: 5px;
            transition: background-color 0.3s;
        }*/

        #toggleButton {
            cursor: pointer;
            background-color: #0d6efd;
            border: none;
            padding: 10px;
            border-radius: 14px;
            transition: background-color 0.3s;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            #toggleButton:hover {
                background-color: #e0e0e0; /* Darken on hover */
            }

        #buttonContainer {
            margin-left: 10px; /* Spacing from toggle button */
            display: none; /* Hide initially */
        }
    </style>


    <%--for garph & details Container div used in repeater --%>
    <%--Sorting groups & Txn Rule milind|19Sep2024--%>


    <script>
        //jQuery.noConflict();
        //jQuery(function ($) {
        //    $(".list-group").sortable({
        //        items: ".sortable-item",
        //        cursor: "move",
        //        placeholder: "sortable-placeholder",
        //        update: function (event, ui) {
        //            var newOrder = $(this).sortable("toArray");
        //            console.log(newOrder);
        //        }
        //    }).disableSelection();
        //});

        jQuery.noConflict();
        jQuery(function ($) {
            $(".list-group").sortable({
                items: ".sortable-item",
                cursor: "move",
                placeholder: "sortable-placeholder",
                update: function (event, ui) {
                    var parentOrder = [];
                    var childOrders = {};

                    $(".list-group > .sortable-item > .mainGroup").each(function (index) {
                        var parentId = $(this).data('id');
                        parentOrder.push({
                            id: parentId,
                            order: index + 1
                        });
                        var childOrder = [];
                        $(this).find(".list-group > .sortable-item > .childGroup").each(function (childIndex) {
                            childOrder.push({
                                ruleid: $(this).data('id'),
                                order: childIndex + 1
                            });
                        });
                        childOrders[parentId] = childOrder;
                    });
                    //console.log('Parent Order:', parentOrder);
                    //console.log('Child Orders:', childOrders);
                    $.ajax({
                        url: 'TrRule.aspx/ShortingTxnRuleGroups',
                        method: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify({
                            orderData: {
                                ParentOrder: parentOrder,
                                ChildOrders: childOrders
                            }
                        }),
                        success: function (response) {
                            //console.log('Order saved successfully:', response);
                            alert(response.d);

                        },
                        error: function (xhr, status, error) {
                            //console.error('Error saving order:', xhr.responseText);
                            alert("Somthing went to wrong!!!.....");
                        }
                    });
                }
            }).disableSelection();
        });
    </script>
    <style>
        .col-md-1 {
            flex: 0 0 auto;
            width: 6.333333%;
        }

        .col-md-11 {
            flex: 0 0 auto;
            width: 6.333333%;
        }

        .col-md-111 {
            flex: 0 0 auto;
            width: 5.333333%;
        }
        .col-md-1111 {
            flex: 0 0 auto;
            width: 10.0%;
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
            width: 5.3%;
        }
        .col-md-24 {
            flex: 0 0 auto;
            width: 16.9%;
        }
        .col-md-25 {
            flex: 0 0 auto;
            width: 4.9%;
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
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div class="breadHeader">
        <h5 class="page-title" style="font-size: larger">Rule Configuration</h5>
    </div>
    <%--<asp:HiddenField ID="hdnPriority1" runat="server" />--%>
    <%--<asp:HiddenField ID="hdnPriority2" runat="server" />--%>
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
            <asp:Button ID="btnAddGroup" runat="server" CssClass="btn btn-primary" Text="Add Group" OnClick="btnAddGroup_Click" Style="margin: 19px; margin-left: 126px;" />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnAddRule" runat="server" CssClass="btn btn-primary" Text="Add Rule" OnClick="btnAddRule_Click" Style="margin: 19px; margin-left: 60px;" />
        </div>
    </div>--%>

    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-2"></div>
        <div class="col-md-2"></div>
        <div class="col-md-2"></div>
        <div class="col-md-2"></div>
        <div class="col-md-2">
            <!-- Flex container for alignment -->
            <%--<button id="toggleButton" class="btn btn-secondary" style="margin: 19px;">
                <i class="fas fa-chevron-right"></i>
            </button>--%>
            <%--<div id="buttonContainer" style="display: none; margin-left: 10px;">--%>
                <!-- Initially hidden -->
                <asp:Button ID="btnAddGroup" runat="server" CssClass="btn btn-primary" Text="Add Group" OnClick="btnAddGroup_Click" />
                &nbsp;&nbsp;
            <asp:Button ID="btnAddRule" runat="server" CssClass="btn btn-primary" Text="Add Rule" OnClick="btnAddRule_Click" />
            <%--</div>--%>
        </div>
        
    </div>
    &nbsp;&nbsp;
    <!-- Modal Group -->
    <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Group Details</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <!-- Body -->
                <div class="modal-body">
                    <div class="form-group row mb-3">
                        <label for="groupName" class="col-md-4 col-form-label">Group Name</label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtGroupName" CssClass="form-control" runat="server" placeholder="Enter Group Name"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row mb-3">
                        <label for="groupDescription" class="col-md-4 col-form-label">Group Description</label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtGroupDescription" CssClass="form-control" TextMode="MultiLine" Rows="3" runat="server" placeholder="Enter Group Description"></asp:TextBox>
                        </div>
                    </div>

                    <%--<div class="form-group row mb-3">
                        <label for="lblPriority" class="col-md-4 col-form-label">Priority</label>
                        <div class="col-md-8">
                            <div class="btn-group" role="group">
                                <asp:LinkButton ID="LinkButton1" CssClass="btn btn-outline-secondary" OnClick="Priority1_Click" CommandArgument="1" runat="server">Low</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton2" CssClass="btn btn-outline-secondary" OnClick="Priority1_Click" CommandArgument="2" runat="server">Medium</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton3" CssClass="btn btn-outline-secondary" OnClick="Priority1_Click" CommandArgument="3" runat="server">High</asp:LinkButton>
                            </div>
                        </div>
                    </div>--%>
                    <%--<div class="row mb-3">
                        <label for="lblPriority" runat="server" class="col-md-4 col-form-label">Priority</label>
                        <div class="col-md-8">
                            <div class="justify-content-around">
                                <asp:LinkButton ID="LinkButton1" CssClass="priority-label btn btn-outline-secondary" OnClick="Priority1_Click" CommandArgument="1" runat="server">Low</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton2" CssClass="priority-label btn btn-outline-secondary" OnClick="Priority1_Click" CommandArgument="2" runat="server">Medium</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton3" CssClass="priority-label btn btn-outline-secondary" OnClick="Priority1_Click" CommandArgument="3" runat="server">High</asp:LinkButton>
                            </div>
                        </div>
                    </div>--%>
                </div>
                <!-- Footer -->
                <div class="modal-footer">
                    <asp:Button ID="btnCreGroup" CssClass="btn btn-primary" runat="server" Text="Submit" OnClick="btnCreGroup_Click" />
                    <asp:Button ID="btnCloseGroup" CssClass="btn btn-secondary" runat="server" Text="Cancel" OnClick="btnCloseGroup_Click" data-bs-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnShowModalG" runat="server" Value="false" />

    <!-- Modal Rule -->
    <div class="modal fade" id="exampleModalR" tabindex="-1" aria-labelledby="exampleModalLabelR" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabelR">Rule Details</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <!-- Body -->
                <div class="modal-body">
                    <div class="row mb-3">
                        <label for="groupName" class="col-md-2 col-form-label">Group Name</label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlGroupName" runat="server" Width="100%" CssClass="dropdown-custom">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <label for="RuleName" class="col-md-2 col-form-label">Rule Name</label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtRuleName" CssClass="form-control" runat="server" placeholder="Enter Rule Name"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <label for="RuleDescription" class="col-md-2 col-form-label">Rule Description</label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtRuleDescription" CssClass="form-control" TextMode="MultiLine" Rows="3" runat="server" placeholder="Enter Rule Description"></asp:TextBox>
                        </div>
                    </div>

                    <%--<div class="row mb-3">
                        <label for="lblPriority" runat="server" class="col-md-2 col-form-label">Priority</label>
                        <div class="col-md-8">
                            <div class="justify-content-around">
                                <asp:LinkButton ID="LinkButton4" CssClass="priority-label btn btn-outline-secondary" OnClick="Priority2_Click" CommandArgument="1" runat="server">Low</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton5" CssClass="priority-label btn btn-outline-secondary" OnClick="Priority2_Click" CommandArgument="2" runat="server">Medium</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton6" CssClass="priority-label btn btn-outline-secondary" OnClick="Priority2_Click" CommandArgument="3" runat="server">High</asp:LinkButton>
                            </div>
                        </div>
                    </div>--%>

                    <%--<div class="row mb-3">
                        <asp:HiddenField ID="hfSelectedValues" runat="server" />
                        <label for="lblAggregator" class="col-md-4 col-form-label">Aggregator</label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlAggregator" runat="server" CssClass="maximus-select w-100" Visible="false">
                            </asp:DropDownList>
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
                    </div>--%>
                    <div class="row mb-3">
                        <asp:HiddenField ID="hfSelectedValues" runat="server" />
                        <label for="lblAggregator" class="col-md-2 col-form-label">Aggregator</label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlAggregator" runat="server" CssClass="maximus-select w-100" Visible="false">
                            </asp:DropDownList>
                            <div class="multi-select-container">
                                <div class="selected-items" id="selectedItems" onclick="toggleDropdown('dropdownContent', 'searchBox')">
                                    <span class="placeholderr">Select options...</span>
                                </div>
                                <div class="dropdown-content" id="dropdownContent">
                                    <input type="text" id="searchBox" class="search-box" placeholder="Search..." onkeyup="filterDropdown('dropdownContent', 'searchBox')" />
                                    <div id="dropdownItems">
                                        <asp:Literal ID="litAggregator" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <asp:HiddenField ID="hfSelectedSecondValues" runat="server" />
                        <label for="lblIIN" class="col-md-2 col-form-label">IIN</label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlIIN" runat="server" CssClass="maximus-select w-100" Visible="false">
                            </asp:DropDownList>
                            <div class="multi-select-container">
                                <div class="selected-items" id="selectedSecondItems" onclick="toggleDropdown('secondDropdownContent', 'secondSearchBox')">
                                    <span class="placeholderr">Select options...</span>
                                </div>
                                <div class="dropdown-content" id="secondDropdownContent">
                                    <input type="text" id="secondSearchBox" class="search-box" placeholder="Search..." onkeyup="filterDropdown('secondDropdownContent', 'secondSearchBox')" />
                                    <div id="secondDropdownItems">
                                        <asp:Literal ID="litIIN" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <label for="lblChannel" class="col-md-2 col-form-label">Channel</label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlChannel" runat="server" CssClass="dropdown-custom" Width="100%">
                                <asp:ListItem Text="--Select--" Value="" />
                                <asp:ListItem Text="AEPS" Value="1" />
                                <asp:ListItem Text="BBPS" Value="2" />
                                <asp:ListItem Text="DMT" Value="3" />
                                <asp:ListItem Text="MATM" Value="4" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row mb-3 align-items-center">
                        <!-- First Dropdown -->
                        <asp:HiddenField ID="hdnTxnType" runat="server" />
                        <%--<div class="col-md-4">
                            <div class="form-group row">
                                <label for="lblTxnType" class="col-md-6 col-form-label">Txn Type</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ddlTxnType" runat="server" CssClass="maximus-select w-100" Width="100%" Visible="false">
                                        <asp:ListItem Text="--Select--" Value="" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>--%>
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label for="lblTxnType" class="col-md-6 col-form-label">Txn Type</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlTxnType" runat="server" CssClass="maximus-select w-100" Width="100%" Visible="false">
                                        <asp:ListItem Text="--Select--" Value="" />
                                    </asp:DropDownList>
                                    <div class="multi-select-container">
                                        <div class="selected-items" id="selectedTItems" onclick="toggleDropdown('TDropdownContent', 'TSearchBox')">
                                            <span class="placeholderr">Select options...</span>
                                        </div>
                                        <div class="dropdown-content" id="TDropdownContent">
                                            <input type="text" id="TSearchBox" class="search-box" placeholder="Search..." onkeyup="filterDropdown('TDropdownContent', 'TSearchBox')" />
                                            <div id="TDropdownItems">
                                                <asp:Literal ID="ltrtxntype" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Toggle slider for FIN/Non-FIN visibility -->
                        <div class="col-md-3 text-center" style="margin-left: 13px; margin-top: -11px;">
                            <label class="switchh">
                                <input type="checkbox" id="ddlTxn" />
                                <span class="sliderr"></span>
                            </label>
                        </div>

                        <!-- Second Dropdown -->
                        <div class="col-md-4" id="dropdownFinNonFin" style="display: none; margin-left: -67px;">
                            <div class="form-group row">
                                <label for="lblTxnTypeFIN" class="col-md-6 col-form-label">FIN/NON-FIN</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ddlTxnTypeFIN" runat="server" CssClass="maximus-select w-100" Width="89%">
                                        <asp:ListItem Text="--Select--" Value="" />
                                        <asp:ListItem Text="FIN" Value="4,5,7,8" />
                                        <asp:ListItem Text="NONFIN" Value="2,3,6" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <label for="lblSwitch" class="col-md-2 col-form-label">Switch</label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlSwitch" runat="server" CssClass="dropdown-custom" Width="100%">
                                <%--<asp:ListItem Text="--Select--" Value="" />
                                <asp:ListItem Text="Maximus" Value="1" />
                                <asp:ListItem Text="Sarvatra" Value="2" />
                                <asp:ListItem Text="PayRakam" Value="3" />--%>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row mb-3 align-items-center">
                        <!-- Percentage TextBox -->
                        <div class="col-md-4">
                            <div class="form-group row">
                                <label for="txtPercentage" class="col-md-6 col-form-label">Percentage %</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtPercentage" CssClass="form-control" runat="server" MaxLength="3" placeholder="Enter Percentage"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <!-- Toggle slider for FIN/Non-FIN visibility -->
                        <div class="col-md-2 text-center">
                            <label class="switchh">
                                <input type="checkbox" id="Switch" />
                                <span class="sliderr"></span>
                            </label>
                        </div>

                        <!-- Count TextBox -->
                        <div class="col-md-4" id="countDiv" style="display: none;">
                            <div class="form-group row">
                                <label for="txtCount" class="col-md-4 col-form-label">Count</label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtCount" CssClass="form-control" runat="server" placeholder="Enter Count"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" />
                    </div>
                </div>
                <!-- Footer -->
                <div class="modal-footer">
                    <asp:Button ID="btnCreateRule" CssClass="btn btn-primary" runat="server" Text="Submit" OnClick="btnCreateRule_Click" />
                    <asp:Button ID="btnCloseRule" CssClass="btn btn-secondary" runat="server" Text="Cancel" OnClick="btnCloseRule_Click" data-bs-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnShowModalR" runat="server" Value="false" />

    <div id="pf-list-simple-expansion" class="list-group list-view-pf list-view-pf-view">
        <asp:PlaceHolder ID="headerPlaceholder" runat="server"></asp:PlaceHolder>
        <asp:Repeater ID="rptrGroup" runat="server" OnItemCommand="rptrGroup_ItemCommand" OnItemDataBound="rptrGroup_ItemDataBound">
            <HeaderTemplate>
                <div class="row" style="width: 103%; background-color: #fbd2ce; height: 30px; font-size: medium; align-items: center; font-weight: bold;">
                <%--<div class="row">--%>
                    <div class="col-md-25"></div>
                    <div class="col-md-1111">Name</div>
                    <div class="col-md-3">Description</div>
                    <div class="col-md-11"></div>
                    <div class="col-md-111">Count</div>
                     <div class="col-md-22"></div>
                    <div class="col-md-11111">Priority</div>
                    <div class="col-md-23"></div>
                    <div class="col-md-1">Status</div>
                    <div class="col-md-24"></div>
                    <div class="col-md-1111">Action</div>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="sortable-item" data-id='<%# Eval("id") %>'>
                    <div class="mainGroup" data-id='<%# Eval("id") %>'>
                        <asp:HiddenField ID="hd1" Value='<%# Eval("id") %>' runat="server" />
                        <div id="pf-list-simple-expansion" class="list-group list-view-pf list-view-pf-view">
                            <div class="list-group-item">
                                <div class="list-group-item-header">
                                    <div class="list-view-pf-expand">
                                        <span class="fa fa-angle-right"></span>
                                    </div>
                                    <%-- <div class="list-view-pf-checkbox">
                                        <input type="checkbox">
                                    </div>--%>
                                    <%-- <asp:ImageButton ID="imgAddRule" runat="server" ImageUrl="../../images/img-plus.jpg" AlternateText="Click me!" OnClick="imgAddRule_Click" />--%>


                                    <div class="list-view-pf-actions" style="padding-top: 7px;">
                                        <%--<button class="btn btn-default">Action</button>--%>

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

                                    <div class="list-view-pf-actions">
                                        <asp:ImageButton ID="imgAddRule" runat="server" ImageUrl="../../images/icons/plus_.png" CommandName="AddRule" CommandArgument='<%# Eval("id") %>' Text="Delete" Style="width: 35px; height: 35px;" />
                                    </div>

                                    <div class="list-view-pf-main-info">
                                        <%--<div class="list-view-pf-left">
                                    <span class="fa fa-plane list-view-pf-icon-sm"></span>
                                </div>--%>
                                        <div class="list-view-pf-left">
                                            <img src="../../images/icons/group_.png" style="width: 25px; height: 25px;" />
                                        </div>
                                        <div class="list-view-pf-body">
                                            <div class="list-view-pf-description">
                                                <div class="list-group-item-heading">
                                                    <%# Eval("group_name") %>
                                                </div>
                                                <div class="list-group-item-text">
                                                    <%# Eval("group_description") %>
                                                </div>
                                            </div>
                                            <input type="hidden" id="hdnGroupId" value='<%# Eval("id") %>' />
                                            <div class="list-view-pf-additional-info">
                                                <div class="list-view-pf-additional-info-item">
                                                    <img src="../../images/icons/rules_2.png" style="width: 25px; height: 25px;" />
                                                    <strong><%# Eval("rule_count") %></strong>

                                                </div>
                                                
                                                <div class="list-view-pf-additional-info-item">
                                                    <img src="../../images/icons/priority_.png" style="width: 25px; height: 25px;" />
                                                    <strong><%# Eval("priority") %></strong>

                                                </div>
                                                
                                                <div class="list-view-pf-additional-info-item">
                                                    <img src="../../images/icons/status.png" style="width: 25px; height: 25px;" />
                                                    <strong><%# Eval("isactive") %></strong>

                                                </div>
                                                <%-- <div class="list-view-pf-additional-info-item">
                                                    <span class="pficon pficon-image"></span>
                                                    <%--<strong>8</strong> Images--%>
                                                <%--</div>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="list-group" style="width: 98%">
                                    <asp:Repeater ID="rptRule" runat="server" OnItemCommand="rptRule_ItemCommand" OnItemDataBound="rptRule_ItemDataBound">
                                        <ItemTemplate>
                                            <div class="sortable-item" data-id='<%# Eval("rule_id") %>'>
                                                <div class="childGroup" data-id='<%# Eval("rule_id") %>'>
                                                    <div class="list-group-item-container container-fluid hidden" style="background: #ffe8e5; padding: 0px !important; margin: 0px 0px 0px 12px;">
                                                        <%--<div class="close">
                                                            <span class="pficon pficon-close"></span>
                                                        </div>--%>
                                                        <div class="col-md-12 row" style="box-shadow: 0px 3px 5px 0px gray; margin: 0px 0px 0px 0px;">
                                                            <%-- <div class="col-md-1"></div>--%>
                                                            <div class="col-md-12">
                                                                <div class="list-group-item-header_Next">
                                                                    <div class="list-view-pf-actions">
                                                                        <label class="switchh">
                                                                            <input type="checkbox" runat="server" id="chkSliderRule" class="clssliderRule">
                                                                            <span class="sliderr"></span>
                                                                        </label>
                                                                        <input type="hidden" id="hdRuleId" value='<%# Eval("rule_id") %>' />
                                                                        <span class="slider round"></span>
                                                                        <div class="dropdown pull-right dropdown-kebab-pf">
                                                                            <button class="btn btn-link dropdown-toggle" type="button" id="dropdownKebabRight9" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                                                <span class="fa fa-ellipsis-v"></span>
                                                                            </button>
                                                                            <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownKebabRight9">
                                                                                <li>
                                                                                    <asp:HiddenField ID="HiddenField2" Value='<%# Eval("rule_id") %>' runat="server" />
                                                                                    <asp:LinkButton ID="btnRuleEdit" runat="server" CommandName="EditRule" CommandArgument='<%# Eval("rule_id") %>' Text="Edit" />
                                                                                </li>
                                                                                <li>
                                                                                    <asp:LinkButton ID="btnRuleDelete" runat="server" CommandName="DeleteRule" CommandArgument='<%# Eval("rule_id") %>' Text="Delete" />
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </div>
                                                                    <div class="list-view-pf-main-info">
                                                                        <div class="list-view-pf-left">
                                                                            <img src="../../images/icons/rules_1.png" style="width: 25px; height: 25px;" />
                                                                        </div>
                                                                        <div class="list-view-pf-body">
                                                                            <div class="list-view-pf-description">
                                                                                <div class="list-group-item-heading">
                                                                                    <%# Eval("rulename") %>
                                                                                </div>
                                                                                <div class="list-group-item-text">
                                                                                    <%# Eval("ruledescription") %>
                                                                                </div>
                                                                            </div>
                                                                            <div class="list-view-pf-additional-info">
                                                                                <%--<input type="hidden" id="hdRuleId" value='<%# Eval("rule_id") %>' />--%>
                                                                                <div class="list-view-pf-additional-info-item">
                                                                                    <%--<span class="pficon pficon-cluster"></span>--%>
                                                                                    <img src="../../images/icons/priority_.png" style="width: 25px; height: 25px;" />
                                                                                    <strong><%# Eval("priority") %></strong>
                                                                                </div>
                                                                                <div class="list-view-pf-additional-info-item">
                                                                                    <%--<span class="pficon pficon-container-node"></span>--%>
                                                                                    <img src="../../images/icons/status.png" style="width: 25px; height: 25px;" />
                                                                                    <strong><%# Eval("is_active") %></strong>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="list-group-item-container_next container-fluid hidden">
                                                                    <div class="row col-md-12">
                                                                        <div class="col-md-6">

                                                                            <div class="chart-container">
                                                                                <canvas id="myChart_<%# Eval("rule_id") %>"></canvas>
                                                                            </div>
                                                                            <script>
                                                                                // milind | 21Sep2024
                                                                                var Success = <%# Eval("success_count") %>;
                                                                                var Failure = <%# Eval("failure_count") %>;
                                                                                var TechnicalFailure = <%# Eval("technical_failure_count") %>;
                                                                                var TotalFailures = Failure + TechnicalFailure;
                                                                                var OverallTotal = Success + TotalFailures;

                                                                                var ruleId = '<%# Eval("rule_id") %>';
                                                                                var ctx = document.getElementById('myChart_' + ruleId).getContext('2d');
                                                                                var allZero = Success === 0 && Failure === 0 && TechnicalFailure === 0 && TotalFailures === 0 && OverallTotal === 0;
                                                                                var chartData = allZero ? [1] : [Success, Failure, TechnicalFailure, TotalFailures, OverallTotal];

                                                                                var backgroundColor = allZero ? ['rgba(173, 216, 230, 0.7)'] : [
                                                                                    'rgba(76, 175, 80, 0.7)',    // Success (Green)
                                                                                    'rgba(255, 99, 132, 0.7)',   // Failure (Red)
                                                                                    'rgba(255, 165, 0, 0.7)',    // Technical Failure (Orange)
                                                                                    'rgba(192, 192, 192, 0.7)',  // Total Failures (Gray)
                                                                                    'rgba(0, 123, 255, 0.7)'     // Overall Total (Blue)
                                                                                ];

                                                                                var borderColor = allZero ? ['rgba(173, 216, 230,1.05)'] : [
                                                                                    'rgba(76, 175, 80, 1)',      // Solid Green
                                                                                    'rgba(255, 99, 132, 1)',     // Solid Red
                                                                                    'rgba(255, 165, 0, 1)',      // Solid Orange
                                                                                    'rgba(192, 192, 192, 1)',    // Solid Gray
                                                                                    'rgba(0, 123, 255, 1)'       // Solid Blue
                                                                                ];

                                                                                var data = {
                                                                                    labels: allZero ? ['No Data'] : ['Success', 'Failures', 'Technical Failures', 'Total Failures', 'Overall Total'],
                                                                                    datasets: [{
                                                                                        data: chartData,
                                                                                        backgroundColor: backgroundColor,
                                                                                        borderColor: borderColor,
                                                                                        borderWidth: 1,
                                                                                        hoverOffset: 15,
                                                                                        tension: 0.4,
                                                                                        pointRadius: 5,
                                                                                        pointHoverRadius: 7,
                                                                                        fill: true,
                                                                                        lineTension: 0.2,
                                                                                        rotation: 45,
                                                                                        showLine: true,
                                                                                        responsive: true,
                                                                                        maintainAspectRatio: false,
                                                                                        clip: true
                                                                                    }]
                                                                                };

                                                                                var options = {
                                                                                    responsive: true,
                                                                                    maintainAspectRatio: false,
                                                                                    plugins: {
                                                                                        legend: {
                                                                                            position: 'bottom',
                                                                                            labels: {
                                                                                                font: {
                                                                                                    size: 15,
                                                                                                    weight: 'bold'
                                                                                                },
                                                                                                color: '#333'
                                                                                            }
                                                                                        },
                                                                                        title: {
                                                                                            display: true,
                                                                                            text: allZero ? 'No Data Available for Rule' : 'Rule Transactions Details',
                                                                                            font: {
                                                                                                size: 20,
                                                                                                weight: 'bold'
                                                                                            },
                                                                                            color: '#333'
                                                                                        },
                                                                                        tooltip: {
                                                                                            backgroundColor: 'rgba(0, 0, 0, 0.8)',
                                                                                            titleColor: '#fff',
                                                                                            bodyColor: '#fff',
                                                                                            borderColor: 'rgba(255, 255, 255, 0.5)',
                                                                                            borderWidth: 1,
                                                                                            callbacks: {
                                                                                                label: function (tooltipItem) {
                                                                                                    var dataset = tooltipItem.dataset;
                                                                                                    var total = dataset.data.reduce((sum, value) => sum + value, 0);
                                                                                                    var value = dataset.data[tooltipItem.dataIndex];
                                                                                                    var percentage = ((value / total) * 100).toFixed(2);
                                                                                                    return `${tooltipItem.label}: ${value} (${percentage}%)`;
                                                                                                }
                                                                                            }
                                                                                        },
                                                                                        datalabels: {
                                                                                            display: true,
                                                                                            anchor: 'center',
                                                                                            align: 'center',
                                                                                            formatter: function (value, context) {
                                                                                                var total = context.dataset.data.reduce((sum, val) => sum + val, 0);
                                                                                                var percentage = ((value / total) * 100).toFixed(2);
                                                                                                return `${value} (${percentage}%)`;
                                                                                            },
                                                                                            color: '#fff',
                                                                                            font: {
                                                                                                weight: 'bold',
                                                                                                size: '20'
                                                                                            }
                                                                                        },

                                                                                        afterDraw: function (chart) {
                                                                                            var ctx = chart.ctx;
                                                                                            var width = chart.width;
                                                                                            var height = chart.height;
                                                                                            var centerX = width / 2;
                                                                                            var centerY = height / 2;
                                                                                            ctx.save();

                                                                                            var centerText = allZero ? 'No Data Available' : OverallTotal;
                                                                                            var fontSize = allZero ? "16px Arial" : "30px Arial";

                                                                                            ctx.font = fontSize;
                                                                                            ctx.fillStyle = '#333';
                                                                                            ctx.textAlign = 'center';
                                                                                            ctx.textBaseline = 'middle';
                                                                                            ctx.fillText(centerText, centerX, centerY);
                                                                                            ctx.restore();
                                                                                        }
                                                                                    },
                                                                                    cutout: '50%',
                                                                                    rotation: -0.1,
                                                                                };

                                                                                // Create the chart
                                                                                var myChart = new Chart(ctx, {
                                                                                    type: 'doughnut',
                                                                                    data: data,
                                                                                    options: options
                                                                                });
                                                                            </script>

                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="details-container">
                                                                                <h2 class="details-header">Detailed Transaction Overview</h2>
                                                                                <p class="sub-header">Comprehensive insights into your transaction metrics</p>
                                                                                <dl class="dl-horizontal">
                                                                                    <dt>Switch Name</dt>
                                                                                    <dd>Switch-XYZ</dd>
                                                                                    <dt>Rule Name</dt>
                                                                                    <dd>Rule-ABC</dd>
                                                                                    <dt>Combination of Rules</dt>
                                                                                    <dd>Rule-ABC + Rule-DEF</dd>
                                                                                    <dt>Time Created</dt>
                                                                                    <dd>January 10, 2016 09:00:00 AM</dd>
                                                                                    <dt>Last Transaction</dt>
                                                                                    <dd>January 15, 2016 10:45:11 AM</dd>
                                                                                    <dt>Severity Level</dt>
                                                                                    <dd>Warning</dd>
                                                                                    <dt>Cluster Name</dt>
                                                                                    <dd>Cluster 1</dd>
                                                                                </dl>
                                                                                <%--<button class="btn btn-primary" onclick="alert('More details coming soon!')">View More Details</button>--%>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <style>
                                                                        .chart-container {
                                                                            position: relative;
                                                                            width: 100%;
                                                                            height: 300px;
                                                                            margin: 20px 0;
                                                                        }
                                                                    </style>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
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

    <%--<script>
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
                selectedItemsContainer.innerHTML = '<span class="placeholderr">Select options...</span>';
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
    </script>--%>

    <%-- IIN --%>
    <%--<script>
        document.addEventListener('click', function (event) {
            // Check if the click was outside the first dropdown
            if (!document.querySelector('.multi-select-container').contains(event.target)) {
                document.getElementById('dropdownContent').style.display = 'none';
            }

            // Check if the click was outside the second dropdown
            if (!document.querySelector('#secondDropdownContent').contains(event.target) &&
                !document.querySelector('#selectedSecondItems').contains(event.target)) {
                document.getElementById('secondDropdownContent').style.display = 'none';
            }
        });

        function toggleDropdown() {
            var dropdown = document.getElementById('dropdownContent');
            dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
            if (dropdown.style.display === 'block') {
                document.getElementById('searchBox').focus(); // Focus on search box when dropdown is opened
            }
        }

        function toggleSecondDropdown() {
            var dropdown = document.getElementById('secondDropdownContent');
            dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
            if (dropdown.style.display === 'block') {
                document.getElementById('secondSearchBox').focus(); // Focus on search box when dropdown is opened
            }
        }

        // Update event listeners for checkboxes
        document.querySelectorAll('#dropdownContent label input').forEach(function (checkbox) {
            checkbox.addEventListener('change', function () {
                updateSelectedItems();
            });
        });

        document.querySelectorAll('#secondDropdownContent label input').forEach(function (checkbox) {
            checkbox.addEventListener('change', function () {
                updateSecondSelectedItems();
            });
        });

        function updateSelectedItems() {
            const selectedItemsContainer = document.getElementById('selectedItems');
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
                selectedItemsContainer.innerHTML = '<span class="placeholderr">Select options...</span>';
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

        function updateSecondSelectedItems() {
            const selectedItemsContainer = document.getElementById('selectedSecondItems');
            const hiddenField = document.getElementById('<%= hfSelectedSecondValues.ClientID %>');
            selectedItemsContainer.innerHTML = ''; // Clear previous items
            let hasSelection = false;
            let selectedValues = [];

            document.querySelectorAll('#secondDropdownContent label input:checked').forEach(function (checkbox) {
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
                selectedItemsContainer.innerHTML = '<span class="placeholderr">Select options...</span>';
            }

            // Update the close button functionality
            document.querySelectorAll('#selectedSecondItems .close').forEach(function (closeBtn) {
                closeBtn.addEventListener('click', function () {
                    const value = this.getAttribute('data-value');
                    document.querySelector(`#secondDropdownContent label input[value="${value}"]`).checked = false;
                    updateSecondSelectedItems();
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

        function filterSecondDropdown() {
            const filter = document.getElementById('secondSearchBox').value.toLowerCase();
            document.querySelectorAll('#secondDropdownContent #secondDropdownItems label').forEach(label => {
                const text = label.textContent.toLowerCase();
                label.style.display = text.includes(filter) ? 'block' : 'none';
            });
        }


    </script>--%>


    <%-- Edit multicheckbox --%>
    <script>
        <%--document.addEventListener('click', function (event) {
    var isClickInside = document.querySelector('.multi-select-container').contains(event.target);
    if (!isClickInside) {
        document.getElementById('dropdownContent').style.display = 'none';
    }
});

function toggleDropdown() {
    var dropdown = document.getElementById('dropdownContent');
    dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
    if (dropdown.style.display === 'block') {
        document.getElementById('searchBox').focus();
    }
}

document.addEventListener('change', function (event) {
    if (event.target.matches('#dropdownItems input[type="checkbox"]')) {
        updateSelectedItems();
    }
});

function updateSelectedItems() {
    const selectedItemsContainer = document.getElementById('selectedItems');
    const hiddenField = document.getElementById('<%= hfSelectedValues.ClientID %>');
    selectedItemsContainer.innerHTML = ''; // Clear previous items
    let selectedValues = [];

    document.querySelectorAll('#dropdownItems input[type="checkbox"]:checked').forEach(function (checkbox) {
        const value = checkbox.value;
        const text = checkbox.parentNode.textContent.trim();
        const selectedItem = document.createElement('div');
        selectedItem.className = 'selected-item';
        selectedItem.innerHTML = `${text} <span class="close" data-value="${value}">&times;</span>`;
        selectedItemsContainer.appendChild(selectedItem);
        selectedValues.push(value);
    });

    hiddenField.value = selectedValues.join(','); // Update hidden field with selected values

    if (selectedValues.length === 0) {
        selectedItemsContainer.innerHTML = '<span class="placeholderr">Select options...</span>';
    }

    // Add close button functionality
    document.querySelectorAll('.selected-item .close').forEach(function (closeBtn) {
        closeBtn.addEventListener('click', function () {
            const value = this.getAttribute('data-value');
            document.querySelector(`#dropdownItems input[value="${value}"]`).checked = false;
            updateSelectedItems();
        });
    });
}

function filterDropdown() {
    const filter = document.getElementById('searchBox').value.toLowerCase();
    document.querySelectorAll('#dropdownItems label').forEach(label => {
        const text = label.textContent.toLowerCase();
        label.style.display = text.includes(filter) ? 'block' : 'none';
    });
}

// Call updateSelectedItems on page load
document.addEventListener('DOMContentLoaded', function () {
    updateSelectedItems();
});--%>
        <%--
        debugger;
        // Close the dropdown when clicking outside
        document.addEventListener('click', function (event) {
            document.querySelectorAll('.multi-select-container').forEach(container => {
                const dropdown = container.querySelector('.dropdown-content');
                if (!container.contains(event.target)) {
                    dropdown.style.display = 'none';
                }
            });
        });

        // Toggle dropdown visibility
        function toggleDropdown(dropdownId, searchBoxId) {
            const dropdown = document.getElementById(dropdownId);
            dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
            if (dropdown.style.display === 'block') {
                const searchBox = document.getElementById(searchBoxId);
                if (searchBox) searchBox.focus();
            }
        }

        // Update selected items
        function updateSelectedItems(dropdownId, selectedItemsContainerId, hiddenFieldId) {
            const selectedItemsContainer = document.getElementById(selectedItemsContainerId);
            const hiddenField = document.getElementById(hiddenFieldId);
            selectedItemsContainer.innerHTML = ''; // Clear previous items
            let selectedValues = [];

            // Update selected items container
            document.querySelectorAll(`#${dropdownId} input[type="checkbox"]:checked`).forEach(function (checkbox) {
                const value = checkbox.value;
                const text = checkbox.parentNode.textContent.trim();
                const selectedItem = document.createElement('div');
                selectedItem.className = 'selected-item';
                selectedItem.innerHTML = `${text} <span class="close" data-value="${value}">&times;</span>`;
                selectedItemsContainer.appendChild(selectedItem);
                selectedValues.push(value);
            });

            hiddenField.value = selectedValues.join(','); // Update hidden field with selected values
            // Check for special condition to disable other checkboxes
            const checkboxes = document.querySelectorAll(`#${dropdownId} input[type="checkbox"]`);
            debugger;
            // Check if the hidden field value includes '1'
            if (hiddenField.value.includes('1')) {
                disableOtherCheckboxes(dropdownId, '1'); // Disable all except checkbox with value '1'
            } else {
                // Check if any other checkbox is checked
                const anyChecked = Array.from(checkboxes).some(checkbox => checkbox.checked && checkbox.value !== '1');

                if (anyChecked) {
                    disableCheckbox(dropdownId, '1'); // Disable checkbox with value '1'
                } else {
                    enableCheckbox(dropdownId, '1'); // Enable checkbox with value '1' if none are checked
                }

                // Enable all other checkboxes
                enableOtherCheckboxes(dropdownId);
            }
            if (selectedValues.length === 0) {
                selectedItemsContainer.innerHTML = '<span class="placeholderr">Select options...</span>';
            }
            function disableOtherCheckboxes(dropdownId, keepValue) {
                debugger;
                document.querySelectorAll(`#${dropdownId} input[type="checkbox"]`).forEach(checkbox => {
                    checkbox.disabled = (checkbox.value !== keepValue); // Disable all except the checkbox with keepValue
                });
            }

            // Disable a specific checkbox
            function disableCheckbox(dropdownId, valueToDisable) {
                debugger;
                const checkbox = document.querySelector(`#${dropdownId} input[type="checkbox"][value="${valueToDisable}"]`);
                if (checkbox) {
                    checkbox.disabled = true; // Disable the checkbox with the specific value
                }
            }

            // Enable a specific checkbox
            function enableCheckbox(dropdownId, valueToEnable) {
                debugger;
                const checkbox = document.querySelector(`#${dropdownId} input[type="checkbox"][value="${valueToEnable}"]`);
                if (checkbox) {
                    checkbox.disabled = false; // Enable the checkbox with the specific value
                }
            }

            // Enable all checkboxes
            function enableOtherCheckboxes(dropdownId) {
                debugger;
                document.querySelectorAll(`#${dropdownId} input[type="checkbox"]`).forEach(checkbox => {
                    checkbox.disabled = false; // Enable all checkboxes
                });
            }

            // Add close button functionality
            document.querySelectorAll(`#${selectedItemsContainerId} .selected-item .close`).forEach(function (closeBtn) {
                closeBtn.addEventListener('click', function () {
                    const value = this.getAttribute('data-value');
                    document.querySelector(`#${dropdownId} input[value="${value}"]`).checked = false;
                    updateSelectedItems(dropdownId, selectedItemsContainerId, hiddenFieldId); // Update after removal
                });
            });
        }

        // Filter dropdown options
        function filterDropdown(dropdownId, searchBoxId) {
            const filter = document.getElementById(searchBoxId).value.toLowerCase();
            document.querySelectorAll(`#${dropdownId} label`).forEach(label => {
                const text = label.textContent.toLowerCase();
                label.style.display = text.includes(filter) ? 'block' : 'none';
            });
        }

        // Initialize dropdowns on page load
        document.addEventListener('DOMContentLoaded', function () {
            // Initialize Aggregator Dropdown
            updateSelectedItems('dropdownContent', 'selectedItems', '<%= hfSelectedValues.ClientID %>');

            // Initialize IIN Dropdown
            updateSelectedItems('secondDropdownContent', 'selectedSecondItems', '<%= hfSelectedSecondValues.ClientID %>');

            updateSelectedItems('TDropdownContent', 'selectedTItems', '<%= hdnTxnType.ClientID %>');
            // Add event listeners for new selections
            document.addEventListener('change', function (event) {
                if (event.target.matches('#dropdownItems input[type="checkbox"]')) {
                    updateSelectedItems('dropdownContent', 'selectedItems', '<%= hfSelectedValues.ClientID %>');
                } else if (event.target.matches('#secondDropdownItems input[type="checkbox"]')) {
                    updateSelectedItems('secondDropdownContent', 'selectedSecondItems', '<%= hfSelectedSecondValues.ClientID %>');
                }
                else if (event.target.matches('#TDropdownItems input[type="checkbox"]')) {
                    updateSelectedItems('TDropdownContent', 'selectedTItems', '<%= hdnTxnType.ClientID %>');
                }
            });
        });--%>


</script>
    <script>
        // Close the dropdown when clicking outside
        document.addEventListener('click', function (event) {
            document.querySelectorAll('.multi-select-container').forEach(container => {
                const dropdown = container.querySelector('.dropdown-content');
                if (!container.contains(event.target)) {
                    dropdown.style.display = 'none';
                }
            });
        });

        // Toggle dropdown visibility
        function toggleDropdown(dropdownId, searchBoxId) {
            const dropdown = document.getElementById(dropdownId);
            dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
            if (dropdown.style.display === 'block') {
                const searchBox = document.getElementById(searchBoxId);
                if (searchBox) searchBox.focus();
            }
        }

        // Update selected items
        function updateSelectedItems(dropdownId, selectedItemsContainerId, hiddenFieldId) {
            const selectedItemsContainer = document.getElementById(selectedItemsContainerId);
            const hiddenField = document.getElementById(hiddenFieldId);
            selectedItemsContainer.innerHTML = ''; // Clear previous items
            let selectedValues = [];

            // Update selected items container
            document.querySelectorAll(`#${dropdownId} input[type="checkbox"]:checked`).forEach(function (checkbox) {
                const value = checkbox.value;
                const text = checkbox.parentNode.textContent.trim();
                const selectedItem = document.createElement('div');
                selectedItem.className = 'selected-item';
                selectedItem.innerHTML = `${text} <span class="close" data-value="${value}">&times;</span>`;
                selectedItemsContainer.appendChild(selectedItem);
                selectedValues.push(value);
            });

            hiddenField.value = selectedValues.join(','); // Update hidden field with selected values

            // Check for special condition to disable other checkboxes
            const checkboxes = document.querySelectorAll(`#${dropdownId} input[type="checkbox"]`);

            // Check if the hidden field value includes '1'
            //if (hiddenField.value.includes('1')) {

            //    disableOtherCheckboxes(dropdownId, '1'); // Disable all except checkbox with value '1'
            //} else {
            //    const anyChecked = Array.from(checkboxes).some(checkbox => checkbox.checked && checkbox.value !== '1');

            //    if (anyChecked) {
            //        disableCheckbox(dropdownId, '1');  // Disable checkbox with value '1'
            //    } else {
            //        enableCheckbox(dropdownId, '1'); // Enable checkbox with value '1' if none are checked
            //    }
            //    enableOtherCheckboxes(dropdownId); // Enable all other checkboxes
            //}

            if (selectedValues.length === 0) {
                selectedItemsContainer.innerHTML = '<span class="placeholderr">Select options...</span>';
            }

            // Add close button functionality
            document.querySelectorAll(`#${selectedItemsContainerId} .selected-item .close`).forEach(function (closeBtn) {
                closeBtn.addEventListener('click', function () {
                    const value = this.getAttribute('data-value');
                    document.querySelector(`#${dropdownId} input[value="${value}"]`).checked = false;
                    updateSelectedItems(dropdownId, selectedItemsContainerId, hiddenFieldId); // Update after removal
                });
            });
        }

        // Filter dropdown options
        function filterDropdown(dropdownId, searchBoxId) {
            const filter = document.getElementById(searchBoxId).value.toLowerCase();
            document.querySelectorAll(`#${dropdownId} label`).forEach(label => {
                const text = label.textContent.toLowerCase();
                label.style.display = text.includes(filter) ? 'block' : 'none';
            });
        }

        // Functions for enabling and disabling checkboxes
        function disableOtherCheckboxes(dropdownId, keepValue) {

            document.querySelectorAll(`#${dropdownId} input[type="checkbox"]`).forEach(checkbox => {
                checkbox.disabled = (checkbox.value !== keepValue); // Disable all except the checkbox with keepValue
            });
        }

        function disableCheckbox(dropdownId, valueToDisable) {

            const checkbox = document.querySelector(`#${dropdownId} input[type="checkbox"][value="${valueToDisable}"]`);
            if (checkbox) {
                checkbox.disabled = true; // Disable the checkbox with the specific value
            }
        }
        function disableSingleCheckboxes(dropdownId, keepValue) {

            document.querySelectorAll(`#${dropdownId} input[type="checkbox"]`).forEach(checkbox => {
                checkbox.disabled = (checkbox.value == keepValue); // Disable all except the checkbox with keepValue
            });
        }
        function enableCheckbox(dropdownId, valueToEnable) {
            const checkbox = document.querySelector(`#${dropdownId} input[type="checkbox"][value="${valueToEnable}"]`);
            if (checkbox) {
                checkbox.disabled = false; // Enable the checkbox with the specific value
            }
        }

        function enableOtherCheckboxes(dropdownId) {
            document.querySelectorAll(`#${dropdownId} input[type="checkbox"]`).forEach(checkbox => {
                checkbox.disabled = false; // Enable all checkboxes
            });
        }

        // Initialize dropdowns on page load
        document.addEventListener('DOMContentLoaded', function () {
            // Initialize Dropdowns
            updateSelectedItems('dropdownContent', 'selectedItems', '<%= hfSelectedValues.ClientID %>');
            updateSelectedItems('secondDropdownContent', 'selectedSecondItems', '<%= hfSelectedSecondValues.ClientID %>');
            updateSelectedItems('TDropdownContent', 'selectedTItems', '<%= hdnTxnType.ClientID %>');

            // Add event listeners for new selections
            document.addEventListener('change', function (event) {
                if (event.target.matches('#dropdownItems input[type="checkbox"]')) {
                    updateSelectedItems('dropdownContent', 'selectedItems', '<%= hfSelectedValues.ClientID %>');
                } else if (event.target.matches('#secondDropdownItems input[type="checkbox"]')) {
                    updateSelectedItems('secondDropdownContent', 'selectedSecondItems', '<%= hfSelectedSecondValues.ClientID %>');
                } else if (event.target.matches('#TDropdownItems input[type="checkbox"]')) {
                    updateSelectedItems('TDropdownContent', 'selectedTItems', '<%= hdnTxnType.ClientID %>');
                }
            });
        });

    </script>


    <%-- Model Group --%>
    <script>
        $(document).ready(function () {
            var showModal = document.getElementById('<%= hdnShowModalG.ClientID %>').value;

            if (showModal === "true") {
                var exampleModal = new bootstrap.Modal(document.getElementById('exampleModal'));
                exampleModal.show();
            }
        });
    </script>
    <%-- Model Rule --%>
    <script>
        $(document).ready(function () {
            var showModal = document.getElementById('<%= hdnShowModalR.ClientID %>').value;

            if (showModal === "true") {
                var exampleModalR = new bootstrap.Modal(document.getElementById('exampleModalR'));
                exampleModalR.show();
            }
        });
    </script>
    <script>
        document.getElementById("toggleButton").addEventListener("click", function (event) {
            event.preventDefault(); // Prevent form submission
            var buttonContainer = document.getElementById("buttonContainer");
            var icon = this.querySelector("i");

            if (buttonContainer.style.display === "none" || buttonContainer.style.display === "") {
                buttonContainer.style.display = "flex"; // Show the buttons in a row
                icon.classList.remove("fa-chevron-right"); // Remove right arrow
                icon.classList.add("fa-chevron-left"); // Add left arrow
            } else {
                buttonContainer.style.display = "none"; // Hide the buttons
                icon.classList.remove("fa-chevron-left"); // Remove left arrow
                icon.classList.add("fa-chevron-right"); // Add right arrow
            }
        });
    </script>

    <script type="text/javascript">
        function validatePercentage() {
            var percentage = document.getElementById('<%= txtPercentage.ClientID %>').value;

            // Check if the percentage is greater than 100
            if (parseInt(percentage) > 100) {
                alert("The percentage cannot be greater than 100.");
                // Optionally, you can clear the value or reset it to a valid value
                document.getElementById('<%= txtPercentage.ClientID %>').value = '';
            }
        }
    </script>

</asp:Content>
