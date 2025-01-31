<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" MasterPageFile="~/SOR.Master" CodeBehind="AlertGroupConfig.aspx.cs" Inherits="SOR.Pages.Monitoring.AlertGroupConfig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
    <link rel="stylesheet" href="../../css/bootstrap.min.css">
    <script src="../../v3/assets/js/jquery-3.6.0.min.js"></script>
    <script src="../../v3/assets/js/bootstrap.bundle.min.js"></script>

    <%--mid sorting--%>
    <!-- jQuery library -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- jQuery UI library -->
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <!-- jQuery UI CSS (optional, for default styling) -->
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <%--mi sorting--%>

    <script>

        function ClearModal() {
            document.getElementById('<%= txtRuleName.ClientID %>').value = "";
            document.getElementById('<%= txtemailBody.ClientID %>').value = "";
            document.getElementById('<%= txtsmsBody.ClientID %>').value = "";
            document.getElementById('<%= txtEmail.ClientID %>').value = "";
            document.getElementById('<%= txtMobile.ClientID %>').value = "";
            document.getElementById('<%= txtEmailCC.ClientID %>').value = "";
            document.getElementById('<%= txtsubject.ClientID %>').value = "";
            document.getElementById('<%= txtStartTime.ClientID %>').value = "";
            document.getElementById('<%= txtEndTime.ClientID %>').value = "";
            document.getElementById('<%= txtConsicativeDeclineCount.ClientID %>').value = "";
            document.getElementById('<%= txtmaxRetry.ClientID %>').value = "";
            document.getElementById('<%= txtTimerInterval.ClientID %>').value = "";
            document.getElementById('<%= txtQueryPreview.ClientID %>').value = "";
        }

        function toggleAlertOptions(value) {
            // Hide both by default
            document.getElementById("dvMailBody").style.display = "none";
            document.getElementById("dvSmSBody").style.display = "none";

            if (value === "1") {
                document.getElementById("dvMailBody").style.display = "block";
            } else if (value === "2") {
                document.getElementById("dvSmSBody").style.display = "block";
            } else if (value === "3") {
                document.getElementById("dvMailBody").style.display = "block";
                document.getElementById("dvSmSBody").style.display = "block";
            }
        }

        window.onload = function () {
            var ddlValue = document.getElementById("<%= ddlAlertsentOn.ClientID %>").value;
            toggleAlertOptions(ddlValue);
        };



        function validateMaxLength(txtBox, maxLength, countId, clearCountOnBlur) {
            var currentLength = txtBox.value.length;
            var charCount = document.getElementById(countId);

            if (currentLength > maxLength) {
                txtBox.value = txtBox.value.substring(0, maxLength);
            }

            if (clearCountOnBlur && !txtBox.matches(':focus')) {

                charCount.textContent = '';
            } else {

                charCount.textContent = currentLength + " / " + maxLength + " characters";
            }
        }


        function validateMobile() {
            var mobileNumber = document.getElementById('<%= txtMobile.ClientID %>').value;
            var regex = /^(?:\+91|91)?[789]\d{9}$/;
            var errorMessage = document.getElementById('<%= txtMobile.ClientID %>');

            if (regex.test(mobileNumber)) {
                //errorMessage.textContent = ''; 
                charCounttxtMobile.textContent = '';
            } else {
                errorMessage.textContent = 'Please enter a valid mobile number.';
                alert('Please enter a valid Indian mobile number.');
                charCounttxtMobile.textContent = '';

            }
            if (!mobileNumber.matches(':focus')) {

                charCounttxtMobile.textContent = '';
            }
        }

        function validateForm() {
            var txtName = document.getElementById('<%= txtRuleName.ClientID %>').value;
            var ddlBClist = document.getElementById('<%= ddlBClist.ClientID %>').value;
            var ddlSwitch = document.getElementById('<%= ddlSwitch.ClientID %>').value;
            var ddlChannels = document.getElementById('<%= ddlChannels.ClientID %>').value;
            var txtemailBody = document.getElementById('<%= txtemailBody.ClientID %>').value;
            var txtsmsBody = document.getElementById('<%= txtsmsBody.ClientID %>').value;
            var txtEmail = document.getElementById('<%= txtEmail.ClientID %>').value;
            var txtMobile = document.getElementById('<%= txtMobile.ClientID %>').value;
            var txtEmailCC = document.getElementById('<%= txtEmailCC.ClientID %>').value;
            var txtsubject = document.getElementById('<%= txtsubject.ClientID %>').value;
            var ddlAlertsentOn = document.getElementById('<%= ddlAlertsentOn.ClientID %>').value;

            if (!txtName.trim()) {
                showWarning("Please Enter Alert Name. Try again", "Warning");
                return false;
            } else if (ddlBClist === "0") {
                showWarning("Please Select BC's. Try again", "Warning");
                return false;
            } else if (ddlSwitch === "0") {
                showWarning("Please Select Switch. Try again", "Warning");
                return false;
            } else if (ddlChannels === "0") {
                showWarning("Please Select Channel. Try again", "Warning");
                return false;
            } else if (!txtEmail.trim()) {
                showWarning("Please Enter Email Id. Try again", "Warning");
                return false;
            } else if (!txtMobile.trim()) {
                showWarning("Please Enter Mobile Number. Try again", "Warning");
                return false;
            } else if (!txtEmailCC.trim()) {
                showWarning("Please Enter Email CC. Try again", "Warning");
                return false;
            } else if (!txtsubject.trim()) {
                showWarning("Please Enter Mail Subject. Try again", "Warning");
                return false;
            } else if (ddlAlertsentOn === "0") {
                showWarning("Please select Alert Mode. Try again", "Warning");
                return false;
            } else if (ddlAlertsentOn !== "0") {
                if (ddlAlertsentOn === "1" && !txtemailBody.trim()) {
                    showWarning("Please Enter Mail Body. Try again", "Warning");
                    return false;
                } else if (ddlAlertsentOn === "2" && !txtsmsBody.trim()) {
                    showWarning("Please Enter SMS Body. Try again", "Warning");
                    return false;
                } else if (ddlAlertsentOn === "3" && (!txtsmsBody.trim() || !txtemailBody.trim())) {
                    showWarning("Please Enter SMS Body & Mail Body. Try again", "Warning");
                    return false;
                }
            }

            return true;
        }

        function showWarning(message, title) {
            alert(title + ": " + message);
        }



    </script>
    <script>
        $(document).ready(function () {
            debugger;
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
            debugger;
            $('.clssliderRule').change(function () {
                var isChecked = $(this).is(':checked');
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
    <style>
        .text-danger {
            color: red; /* Make the '*' symbol red */
            font-weight: bold; /* Optional: Make it bold */
        }
    </style>

    <link rel="stylesheet" href="../../css/style.css">

    <style>
        /* Disabled style for the div */
        .DisabledOnEdit {
            opacity: 0.5;
            position: relative;
        }


            .DisabledOnEdit input,
            .DisabledOnEdit button,
            .DisabledOnEdit select,
            .DisabledOnEdit textarea {
                pointer-events: none;
            }

            /* Tooltip style */
            .DisabledOnEdit::after {
                content: "Query Edit Disabled";
                position: absolute;
                top: -25px;
                left: 50%;
                transform: translateX(-50%);
                padding: 5px 10px;
                background-color: #333;
                color: white;
                border-radius: 5px;
                font-size: 12px;
                display: none;
            }

            .DisabledOnEdit:hover::after {
                display: block; /* Show on hover */
            }


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

    <%-- for query configuration table mi--%>
    <style>
        /* Custom Styling for the Table */
        .query-table {
            margin-top: 30px;
            margin-bottom: 30px;
            border: 1px solid #dee2e6;
            border-radius: 10px;
            overflow: hidden;
            width: 100%;
            background-color: #fff;
        }

            .query-table th {
                background-color: #f8f9fa;
                text-align: center;
                font-weight: bold;
                padding: 15px;
                color: #495057;
                font-size: 14px;
            }

            .query-table td {
                padding: 12px;
                vertical-align: middle;
            }

                .query-table td select {
                    width: 100%;
                    padding: 8px;
                    border: 1px solid #ced4da;
                    border-radius: 5px;
                    font-size: 14px;
                    box-sizing: border-box; /* Ensures dropdown size doesn't overflow */
                }

            /* Fix dropdown display issue */
            .query-table .form-control {
                display: block;
                width: 100%;
                height: calc(1.5em + 0.75rem + 2px);
                padding: 0.375rem 0.75rem;
                font-size: 1rem;
                line-height: 1.5;
                color: #495057;
                background-color: #fff;
                border: 1px solid #ced4da;
                border-radius: 5px;
            }

            /* Delete Button Styling */
            .query-table .delete-btn {
                background-color: #dc3545;
                color: white;
                padding: 5px 12px;
                border-radius: 4px;
                border: none;
                cursor: pointer;
                font-size: 14px;
                width: 100px; /* Set a fixed width for delete buttons */
            }

                .query-table .delete-btn:hover {
                    background-color: #c82333;
                }

            /* Hover effect for rows */
            .query-table tbody tr:hover {
                background-color: #f1f1f1;
                cursor: pointer;
                transition: background-color 0.3s ease;
            }

        /* Add button for adding rows */
        .add-row-btn {
            margin-top: 20px;
            background-color: #007bff;
            color: white;
            padding: 10px 15px;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            cursor: pointer;
            width: 100%;
        }

            .add-row-btn:hover {
                background-color: #0056b3;
            }

        /* Spacing for the page */
        /*.container {
            padding: 30px;
            background-color: #f9f9f9;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }*/

        /* Responsive styling for smaller screens */
        @media (max-width: 767px) {
            .query-table td, .query-table th {
                font-size: 12px;
                padding: 8px;
            }

            .add-row-btn {
                font-size: 14px;
            }
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

    <%--for boxstyle section wise mi--%>
    <style>
        .mform-group {
            margin: 10px;
            padding: 10px;
            border: 1px solid #89CFF0;
            border-radius: 5px;
        }

        .col {
            float: left;
            width: 20%;
            margin: 0px 2%;
        }

        .Fieldscol {
            float: left;
            width: 20%;
            margin: 0px 2%;
        }

        .colCalDet {
            float: left;
            width: 20%;
            margin: 0px 2%;
        }

        .My-label {
            display: inline-block;
            font-weight: bold;
            margin-bottom: 5px;
            color: #333;
            font-size: 14px;
        }

        .form-value {
            display: inline-block;
            /* margin-bottom: 15px;*/
            font-size: 14px;
        }

        .clearfix::after {
            content: "";
            clear: both;
            display: table;
        }

        /*breadcrumb custom*/
        .breadcrumb {
            background-color: #f8f9fa;
            padding: 10px;
            display: flex;
            align-items: center;
            border-bottom: 1px solid #ddd;
        }

            .breadcrumb i {
                color: #007bff;
                margin-right: 5px;
            }

            .breadcrumb span {
                color: #333;
                font-size: 18px;
            }
        /*breadcrumb custom*/
    </style>

    <%--for query configuration mi--%>
    <style>
        .query-preview-card {
            border: 1px solid #007bff;
            border-radius: 10px;
            padding: 20px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .query-preview-title {
            margin-bottom: 15px;
            font-weight: bold;
            font-size: 1.25rem;
            color: #007bff;
        }

        .query-preview-text {
            white-space: pre-wrap;
            overflow-wrap: break-word;
            font-family: 'Courier New', Courier, monospace;
            background-color: #f0f0f0;
            border: 1px solid #007bff;
            border-radius: 5px;
            padding: 10px;
            height: auto;
            width: 100%;
            overflow-y: auto;
            transition: border-color 0.2s;
        }

            .query-preview-text:hover {
                border-color: #0056b3;
            }

            .query-preview-text:focus {
                outline: none;
                border-color: #0056b3;
                box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
            }
    </style>

    <!-- Add the following CSS styles -->
    <style>
        .list-group-item-container_next {
            margin-top: 20px;
            padding: 20px;
        }

        .wrapper {
            display: flex;
            justify-content: center;
        }

        .user-card {
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            width: 100%;
            padding: 20px;
            transition: transform 0.3s ease;
        }

            .user-card:hover {
                transform: translateY(-10px);
            }

        .user-card-info h2 {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }

        /* Grid Layout for the info */
        .info-grid {
            display: grid;
            grid-template-columns: repeat(2, 1fr); /* Two columns for the label and value */
            gap: 0px;
            font-size: 12px;
            color: #333;
        }

        .email-body {
            font-size: 16px;
            line-height: 1.6;
            color: #333;
        }

        .email-preview {
            display: block;
            max-height: 150px; /* Limit the height of the previewed email body */
            overflow: hidden;
            text-overflow: ellipsis;
            word-wrap: break-word;
        }

        .email-full-content {
            display: none; /* Hide the full email content by default */
        }

        #seeMoreBtn {
            display: inline-block;
            margin-top: 10px;
            color: #007bff;
            background: none;
            border: none;
            cursor: pointer;
            font-weight: bold;
        }

        .info-item {
            display: flex;
            flex-direction: row;
            align-items: flex-start;
            justify-content: space-between;
            margin-bottom: 15px;
        }

            .info-item strong {
                font-weight: bold;
                color: #555;
                margin-right: 10px;
                flex-shrink: 0;
            }

            .info-item .info-value {
                flex-grow: 1;
            }

        /* Responsive Design for smaller screens */
        @media (max-width: 768px) {
            .info-grid {
                grid-template-columns: 1fr; /* Single column for small screens */
            }
        }
    </style>

    <style>
        /* Ensure labels appear above fields with some spacing */
        .row.mb-3 {
            margin-bottom: 1rem; /* Adjust space between rows */
        }

        .col-form-label {
            font-weight: bold;
            display: block; /* Force the label to stack on top */
            margin-bottom: 0.5rem; /* Space between label and input field */
        }
    </style>
    <script>
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
        .list-view-pf-additional-info-item {
            flex-direction: row; /* Align header and content horizontally */
            align-items: flex-start; /* Align items to the top */
            margin-bottom: 8px; /* Space between items */
            position: relative; /* Ensure content flows below header */
        }

        /* Container to manage the list */
        .list-view-pf-description {
            display: table; /* Treat this as a table container */
            width: 100%;
            border-collapse: collapse; /* Ensure borders collapse (if any) */
        }

        /* Each row */


        /* Header */
        .header-item {
            /* font-weight: normal; */
            /* color: #7f8fa4; */
            text-align: left;
            margin-right: 16px;
            padding: 8px;
            /* white-space: nowrap; */
            /* background-color: #f4f6f9; */
            /* position: sticky; */
            top: 0;
            z-index: 2;
            text-align: left;
            /* border-bottom: 2px solid #ddd; */
            width: 150px;
            color: #7f8fa4;
        }
        /* Content of each row */
        .content-item {
            display: table-cell; /* Treat this as a cell in the table */
            padding: 8px; /* Padding for content cells */
            text-align: left; /* Align content text to the left */
            word-wrap: break-word; /* Allow content to wrap */
            max-width: 100%; /* Ensure content doesn't overflow */
            vertical-align: top; /* Align content to the top */
            width: 100px; /* Fixed width for content columns */
        }

            /* Optional: Add styling for strong text inside the content */
            .content-item strong {
                font-weight: bold;
                color: #555;
                word-break: break-word; /* Break long words */
            }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div class="row">
        <div class="col-md-2">
        </div>
        <div class="col-md-2">
        </div>
        <div class="col-md-5">
        </div>
        <div class="d-flex justify-content-end">
            <asp:Button ID="btnAddGroup" runat="server" CssClass="btn btn-primary" Text="Add Group" OnClick="btnAddGroup_Click" Style="margin: 19px;" />
            <asp:Button ID="btnAddRule" runat="server" CssClass="btn btn-primary" Text="Add Alert" OnClick="btnAddAlert_Click" Style="margin: 19px;" />
            <asp:Button ID="btnaddTemplate" Visible="false" runat="server" CssClass="btn btn-primary" Text="Add Template" OnClick="btnTemplateMaster_Click" Style="margin: 19px;" />
        </div>
    </div>

    <!-- ModalGroup Alert-->
    <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Alert Group Details</h5>
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


    <!--ModalAlert Config Details-->

    <div class="modal fade" id="exampleModalR" tabindex="-1" aria-labelledby="exampleModalLabelR" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabelR">Alert Configurations Details</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <!-- Body -->
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="mform-group">
                                <!-- Name Field -->
                                <div class="row mb-3">
                                    <label for="txtRuleName" class="col-md-2 col-form-label">Alert Name<span class="text-danger">*</span></label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtRuleName" CssClass="form-control" MaxLength="50" runat="server" placeholder="Enter Name" oninput="validateMaxLength(this, 70, 'charCountAlertName', false)" onblur="validateMaxLength(this, 50, 'charCountAlertName', true)"></asp:TextBox>
                                        <span id="charCountAlertName" style="color: red;"></span>
                                    </div>
                                    <!-- groupId Field -->
                                    <label for="ddlGroupName" class="col-md-2 col-form-label">Group Name<span class="text-danger">*</span></label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlGroupName" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                            </div>

                            <div class="mform-group">
                                <!-- BC Field -->
                                <div class="row mb-3">
                                    <label for="ddlBClist" class="col-md-2 col-form-label">BC<span class="text-danger">*</span></label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlBClist" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlBClist_SelectedIndexChanged">
                                            <asp:ListItem Text="--Select--" Value="0" />
                                        </asp:DropDownList>
                                    </div>

                                    <label for="lblSwitch" class="col-md-2 col-form-label">Working Time</label>
                                    <!-- Start Time -->
                                    <div class="col-md-2">
                                        <div class="">
                                            <asp:TextBox ID="txtStartTime" CssClass="form-control datetimepicker" runat="server" placeholder="from"></asp:TextBox>
                                        </div>
                                    </div>
                                    <!-- End Time -->
                                    <div class="col-md-2">
                                        <div class="">
                                            <asp:TextBox ID="txtEndTime" class="form-control datetimepicker" runat="server" placeholder="To"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <!-- Switch and Channel Fields in a single row -->
                                <div class="row mb-3">
                                    <!-- Switch Field -->
                                    <label for="lblSwitch" class="col-md-2 col-form-label">Switch<span class="text-danger">*</span></label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSwitch" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="--Select--" Value="0" />
                                        </asp:DropDownList>
                                    </div>

                                    <!-- Channel Field -->
                                    <label for="lblChannel" class="col-md-2 col-form-label">Channel<span class="text-danger">*</span></label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlChannels" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged">
                                            <asp:ListItem Text="--Select--" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row mb-3" hidden="hidden">
                                    <label for="RuleDescription" class="col-md-2 col-form-label">Descriptions</label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtRuleDescription" CssClass="form-control" TextMode="MultiLine" Rows="8" runat="server" placeholder="Enter Mail Body"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="mform-group">
                                <!-- Mobile Field -->
                                <div class="row mb-3">
                                    <label for="txtMobile" class="col-md-2 col-form-label">Mobile No.<span class="text-danger">*</span></label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtMobile" CssClass="form-control" runat="server" placeholder="Enter Mobile Number" oninput="validateMaxLength(this, 10, 'charCounttxtMobile', false)" onblur="validateMobile()"></asp:TextBox>
                                        <span id="charCounttxtMobile" style="color: red;"></span>
                                    </div>
                                </div>

                                <!-- Email Field -->
                                <div class="row mb-3">
                                    <label for="txtEmail" class="col-md-2 col-form-label">Email To<span class="text-danger">*</span></label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" placeholder="Enter Email Address" oninput="validateMaxLength(this, 50, 'charCounttxtEmail', false)" onblur="validateMaxLength(this, 50, 'charCounttxtEmail', true)"></asp:TextBox>
                                        <span id="charCounttxtEmail" style="color: red;"></span>
                                    </div>
                                </div>

                                <!-- CCEmail Field -->
                                <div class="row mb-3">
                                    <label for="txtEmail" class="col-md-2 col-form-label">Email CC<span class="text-danger">*</span></label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtEmailCC" CssClass="form-control" runat="server" placeholder="Enter EmailCC Address" oninput="validateMaxLength(this, 100, 'charCounttxtEmailCC', false)" onblur="validateMaxLength(this, 100, 'charCounttxtEmailCC', true)"></asp:TextBox>
                                        <span id="charCounttxtEmailCC" style="color: red;"></span>
                                    </div>
                                </div>

                                <!-- Subject Field -->
                                <div class="row mb-3">
                                    <label for="RuleDescription" class="col-md-2 col-form-label">Subject<span class="text-danger">*</span></label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtsubject" CssClass="form-control" TextMode="MultiLine" Rows="1" MaxLength="50" runat="server" placeholder="Enter Subject" oninput="validateMaxLength(this, 50, 'charCountSubject', false)" onblur="validateMaxLength(this, 50, 'charCountSubject', true)" />
                                        <span id="charCountSubject" style="color: red;"></span>
                                    </div>
                                </div>


                                <!-- AlertMode -->
                                <div class="row mb-3">
                                    <label for="contactType" class="col-md-2 col-form-label">Alert Mode<span class="text-danger">*</span></label>
                                    <div class="col-md-4">
                                        <asp:DropDownList
                                            runat="server"
                                            ID="ddlAlertsentOn"
                                            CssClass="form-control" onchange="toggleAlertOptions(this.value)">
                                            <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Email</asp:ListItem>
                                            <asp:ListItem Value="2">Phone</asp:ListItem>
                                            <asp:ListItem Value="3">Both</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <!-- Mailbody Field -->
                                <div class="row mb-3" id="dvMailBody" style="display: none;">
                                    <label for="RuleDescription" class="col-md-2 col-form-label">Mail Body<span class="text-danger">*</span></label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtemailBody" CssClass="form-control" TextMode="MultiLine" Rows="4" MaxLength="30" runat="server" placeholder="Enter Email Body"></asp:TextBox>
                                    </div>
                                </div>

                                <!-- SMSbody Field -->
                                <div class="row mb-3" id="dvSmSBody" style="display: none;">
                                    <label for="RuleDescription" class="col-md-2 col-form-label">SMS Body<span class="text-danger">*</span></label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtsmsBody" CssClass="form-control" TextMode="MultiLine" Rows="4" MaxLength="20" runat="server" placeholder="Enter SMS Body"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="mform-group">
                                <!-- AlertType Field -->
                                <div class="row mb-3">
                                    <label for="txtCount" class="col-md-2 col-form-label">Alert Type<span class="text-danger">*</span></label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtalertType" CssClass="form-control" runat="server" placeholder="Alert Type" Visible="false"></asp:TextBox>
                                        <asp:DropDownList ID="ddlAlertType" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>

                                    <label for="txtConsicativeDeclineCount" class="col-md-1 col-form-label">Count<span class="text-danger">*</span></label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtConsicativeDeclineCount" CssClass="form-control" runat="server" placeholder="Decline Count"></asp:TextBox>
                                    </div>

                                    <!-- MaxRetry Field -->
                                    <label for="txtmaxRetry" class="col-md-2 col-form-label">Max Retry Count<span class="text-danger">*</span></label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtmaxRetry" CssClass="form-control" runat="server" placeholder="Max Retry Count(Min.)"></asp:TextBox>
                                    </div>
                                </div>

                                <!-- Retry Interval Count -->
                                <div class="row mb-3">
                                    <!-- Retry Field -->
                                    <label for="txtCount" class="col-md-2 col-form-label">Interval Count <span class="text-danger">*</span></label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtTimerInterval" CssClass="form-control" runat="server" placeholder="Interval Count (Min.)"></asp:TextBox>
                                    </div>
                                </div>

                                <!-- Next Interval -->
                                <div class="row mb-3" hidden="hidden">
                                    <label for="txtCount" class="col-md-2 col-form-label">Next Interval</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtNxtInterval" CssClass="form-control datetimepicker" runat="server" placeholder="Next Interval (Min.)"></asp:TextBox>
                                    </div>
                                </div>


                            </div>

                            <div class="mform-group">
                                <div runat="server" id="DisabledOnEdit">
                                    <!-- Consicative Decline Interval -->
                                    <div class="row mb-3" hidden="hidden">
                                        <label for="txtCount" class="col-md-2 col-form-label">Decline Count</label>
                                    </div>
                                    <div class="row mb-3">
                                        <!-- Column Selections -->
                                        <div class="col-md-3">
                                            <label for="ddlColumnSelected" class="col-form-label">Column Selections<span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="ddlColumnSelected" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="col-md-3">
                                            <!-- SQL Operator (Conditions) -->
                                            <div class="col-md-12">
                                                <label for="ddlConditions" class="col-form-label">Operator(Conditions)<span class="text-danger">*</span></label>
                                                <asp:DropDownList ID="ddlConditions" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="0" Selected="true">--Select--</asp:ListItem>
                                                    <asp:ListItem Text="=" Value="=" />
                                                    <asp:ListItem Text="!=" Value="!=" />
                                                    <asp:ListItem Text="<" Value="<" />
                                                    <asp:ListItem Text="<=" Value="<=" />
                                                    <asp:ListItem Text=">" Value=">" />
                                                    <asp:ListItem Text=">=" Value=">=" />
                                                    <asp:ListItem Text="IS NULL" Value="IS NULL" />
                                                    <asp:ListItem Text="IS NOT NULL" Value="IS NOT NULL" />
                                                    <asp:ListItem Text="BETWEEN" Value="BETWEEN" />
                                                    <asp:ListItem Text="IN" Value="IN" />
                                                    <%--<asp:ListItem Text="LIKE" Value="LIKE" />
                                                <asp:ListItem Text="ILIKE" Value="ILIKE" />
                                                <asp:ListItem Text="SIMILAR TO" Value="SIMILAR TO" />
                                                <asp:ListItem Text="~" Value="~" />
                                                <asp:ListItem Text="~*" Value="~*" />
                                                <asp:ListItem Text="!~" Value="!~" />
                                                <asp:ListItem Text="!~*" Value="!~*" />
                                                <asp:ListItem Text="AND" Value="AND" />
                                                <asp:ListItem Text="OR" Value="OR" />
                                                <asp:ListItem Text="NOT" Value="NOT" />
                                                <asp:ListItem Text="&&" Value="&&" />
                                                <asp:ListItem Text="@>" Value="@>" />
                                                <asp:ListItem Text="<@" Value="<@" />
                                                <asp:ListItem Text="<<" Value="<<" />
                                                <asp:ListItem Text=">>" Value=">>" />
                                                <asp:ListItem Text="+" Value="+" />
                                                <asp:ListItem Text="-" Value="-" />
                                                <asp:ListItem Text="*" Value="*" />
                                                <asp:ListItem Text="/" Value="/" />
                                                <asp:ListItem Text="%" Value="%" />
                                                <asp:ListItem Text="^" Value="^" />
                                                <asp:ListItem Text="||" Value="||" />
                                                <asp:ListItem Text="::" Value="::" />
                                                <asp:ListItem Text="&" Value="&" />
                                                <asp:ListItem Text="|" Value="|" />
                                                <asp:ListItem Text="#" Value="#" />--%>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <!-- Transaction Type/Value Selection -->
                                            <div class="col-md-12">
                                                <label for="contactType" class="col-form-label">Value Selection<span class="text-danger">*</span></label>
                                                <asp:HiddenField ID="hdnValueSelc" runat="server" Value="false" />
                                                <div id="divddlTransationType" runat="server" style="display: none">
                                                    <asp:DropDownList runat="server" ID="ddlTxnType" CssClass="form-control">
                                                        <asp:ListItem Value="0" Selected="true">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">BalanceEnquiry</asp:ListItem>
                                                        <asp:ListItem Value="2">Withdrawal</asp:ListItem>
                                                        <asp:ListItem Value="3">MiniStatement</asp:ListItem>
                                                        <asp:ListItem Value="4">AuthRequest</asp:ListItem>
                                                        <asp:ListItem Value="5">Fundtransfer</asp:ListItem>
                                                        <asp:ListItem Value="6">Purchase</asp:ListItem>
                                                        <asp:ListItem Value="7">CashDeposite</asp:ListItem>
                                                        <asp:ListItem Value="8">ALL</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div id="divddlResponseCode" runat="server">
                                                    <asp:DropDownList runat="server" ID="ddlResponseCode" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- SQL Operator Selection (AND/OR) -->
                                        <div class="col-md-3">
                                            <label for="ddlOperator0" class="col-form-label">Operator(AND/OR)</label>
                                            <asp:DropDownList ID="ddlOperator0" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="0" Selected="true">--Select--</asp:ListItem>
                                                <asp:ListItem Value="AND">AND</asp:ListItem>
                                                <asp:ListItem Value="OR">OR</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-md-8"></div>
                                        <!-- Query Add-->
                                        <div class="col-md-4">
                                            <div class="d-flex justify-content-end">
                                                <asp:Button ID="btnAddCondition" runat="server" Text="Add Condition" CssClass="btn btn-primary me-3" OnClick="btnAddCondition_Click" />
                                                <asp:Button ID="btnClearCondition" runat="server" Text="Clear Query" CssClass="btn btn-secondary" OnClick="btnClearCondition_Click" />
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Query Preview-->
                                    <div class="row mb-3">
                                        <div class="col-md-12">
                                            <div class="query-preview-card">
                                                <div class="query-preview-title">Query Preview</div>
                                                <asp:HiddenField ID="hidnEncData" runat="server" />
                                                <asp:TextBox ID="txtQueryPreview" runat="server" CssClass="query-preview-text" TextMode="MultiLine" Rows="5" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>

                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlBClist" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlChannels" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlColumnSelected" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>


                    <div class="mform-group" hidden="hidden">
                        <asp:UpdatePanel runat="server" ID="updpnlQuerygrid" ChildrenAsTriggers="true" UpdateMode="Conditional">
                            <ContentTemplate>
                                <!-- Query Table Configurations Div-->
                                <div class="container">
                                    <h2 class="mt-4">Query Configurations</h2>
                                    <div class="row mb-5">
                                        <asp:GridView ID="gvConditions" runat="server" AutoGenerateColumns="False" ShowFooter="True" CssClass="table query-table"
                                            OnRowDataBound="gvConditions_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="AND/OR" SortExpression="AND/OR">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlAndOr" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="AND" Value="AND" />
                                                            <asp:ListItem Text="OR" Value="OR" />
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="FieldName" SortExpression="FieldName">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="Select Field" Value="" />
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Operator" SortExpression="Operator">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlOperator" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="Select Operator" Value="" />
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Value" SortExpression="Value">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlValue" runat="server" CssClass="form-control" Style="width: 260px;">
                                                            <asp:ListItem Text="Select Value" Value="" />
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Actions" SortExpression="Actions">
                                                    <ItemTemplate>
                                                        <div style="text-align: center;">
                                                            <button type="button" class="btn btn-primary" onclick="deleteRow(this)">Delete</button>
                                                            <button type="button" class="btn btn-secondary" onclick="addRow()">Add</button>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>


                <!-- Footer -->
                <div class="modal-footer">
                    <asp:Button
                        ID="btnCreateRule"
                        CssClass="btn btn-primary"
                        runat="server"
                        Text="Submit"
                        OnClick="btnSaveAlertConfig_Click"
                        OnClientClick="return validateForm();" />
                    <asp:Button ID="btnCloseRule" CssClass="btn btn-secondary" runat="server" Text="Cancel" OnClick="btnCloseRule_Click" data-bs-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnShowModalR" runat="server" Value="false" />


    <div id="pf-list-simple-expansion" class="list-group list-view-pf list-view-pf-view">

        <asp:Repeater ID="rptrGroup" runat="server" OnItemCommand="rptrGroup_ItemCommand" OnItemDataBound="rptrGroup_ItemDataBound">
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

                                    <div class="list-view-pf-actions" style="padding-top: 7px;">
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

                                        <div class="list-view-pf-left">
                                            <img src="../../images/icons/group_.png" style="width: 25px; height: 25px;" />
                                        </div>
                                        <div class="list-view-pf-body">
                                            <!-- Group Description Section -->
                                            <div class="list-view-pf-description">
                                                <!-- Group Name Section -->
                                                <div class="list-view-pf-additional-info-item">
                                                    <div class="header-item">
                                                        <strong>Name</strong>
                                                    </div>
                                                    <div class="content-item">
                                                        <strong><%# Eval("groupname") %></strong>
                                                    </div>
                                                </div>

                                                <!-- Group Description Section -->
                                                <div class="list-view-pf-additional-info-item">
                                                    <div class="header-item">
                                                        <strong>Description</strong>
                                                    </div>
                                                    <div class="content-item">
                                                        <strong><%# Eval("groupdescription") %></strong>
                                                    </div>
                                                </div>

                                                <!-- Rule Count Section -->
                                                <div class="list-view-pf-additional-info-item">
                                                    <div class="header-item">
                                                        <strong>Count</strong>
                                                    </div>
                                                    <div class="content-item">
                                                        <img src="../../images/icons/rules_2.png" style="width: 25px; height: 25px;" />
                                                        <strong><%# Eval("membercount") %></strong>
                                                    </div>
                                                </div>

                                                <!-- Status Section -->
                                                <div class="list-view-pf-additional-info-item">
                                                    <div class="header-item">
                                                        <strong>Status</strong>
                                                    </div>
                                                    <div class="content-item">
                                                        <img src="../../images/icons/status.png" style="width: 25px; height: 25px;" />
                                                        <strong><%# Eval("isactive") %></strong>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Hidden Group ID for backend processing -->
                                            <input type="hidden" id="hdnGroupId" value='<%# Eval("id") %>' />
                                        </div>

                                    </div>
                                </div>
                                <div class="list-group" style="width: 98%">
                                    <asp:Repeater ID="rptRule" runat="server" OnItemCommand="rptAlert_ItemCommand" OnItemDataBound="rptAlert_ItemDataBound">
                                        <ItemTemplate>
                                            <div class="sortable-item" data-id='<%# Eval("id") %>'>
                                                <div class="childGroup" data-id='<%# Eval("id") %>'>
                                                    <div class="list-group-item-container container-fluid hidden" style="background: #ffe8e5; padding: 0px !important; margin: 0px 0px 0px 12px;">
                                                        <div class="col-md-12 row" style="box-shadow: 0px 3px 5px 0px gray; margin: 0px 0px 0px 0px;">
                                                            <div class="col-md-12">
                                                                <div class="list-group-item-header_Next">
                                                                    <div class="list-view-pf-actions">
                                                                        <label class="switchh">
                                                                            <input type="checkbox" runat="server" id="chkSliderRule" class="clssliderRule">
                                                                            <span class="sliderr"></span>
                                                                        </label>
                                                                        <input type="hidden" id="hdRuleId" value='<%# Eval("id") %>' />
                                                                        <span class="slider round"></span>
                                                                        <div class="dropdown pull-right dropdown-kebab-pf">
                                                                            <button class="btn btn-link dropdown-toggle" type="button" id="dropdownKebabRight9" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                                                <span class="fa fa-ellipsis-v"></span>
                                                                            </button>
                                                                            <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownKebabRight9">
                                                                                <li>
                                                                                    <asp:HiddenField ID="HiddenField2" Value='<%# Eval("id") %>' runat="server" />
                                                                                    <asp:LinkButton ID="btnRuleEdit" runat="server" CommandName="EditRule" CommandArgument='<%# Eval("id") + "," + Eval("gid") %>' Text="Edit" />
                                                                                </li>
                                                                                <li>
                                                                                    <asp:LinkButton ID="btnRuleDelete" runat="server" CommandName="DeleteRule" CommandArgument='<%# Eval("id") + "," + Eval("gid") %>' Text="Delete" />
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </div>
                                                                    <div class="list-view-pf-main-info">
                                                                        <div class="list-view-pf-left">
                                                                            <img src="../../images/icons/rules_1.png" style="width: 25px; height: 25px;" />
                                                                        </div>
                                                                        <div class="list-view-pf-body">
                                                                            <!-- Group Description Section -->
                                                                            <div class="list-view-pf-description">
                                                                                <!-- Contact Name Section -->
                                                                                <div class="list-view-pf-additional-info-item">
                                                                                    <div class="header-item">
                                                                                        <strong>Name</strong>
                                                                                    </div>
                                                                                    <div class="content-item">
                                                                                        <strong><%# Eval("contactname") %></strong>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="list-view-pf-additional-info-item">
                                                                                    <div class="header-item">
                                                                                        <strong>Status</strong>
                                                                                    </div>
                                                                                    <div class="content-item">
                                                                                        <img src="../../images/icons/status.png" style="width: 25px; height: 25px;" />
                                                                                        <strong><%# Eval("isactive") %></strong>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <div class="list-group-item-container_next container-fluid hidden">
                                                                    <div class="wrapper">
                                                                        <div class="user-card">
                                                                            <div class="user-card-info">
                                                                                <h2 class="user-name"><%# Eval("contactname") %></h2>
                                                                                <div class="info-grid">
                                                                                    <div class="info-item">
                                                                                        <strong>BC:</strong>
                                                                                        <div class="info-value"><%# Eval("bcname") %></div>
                                                                                    </div>
                                                                                    <div class="info-item">
                                                                                        <strong>Switch:</strong>
                                                                                        <div class="info-value"><%# Eval("switchs") %></div>
                                                                                    </div>

                                                                                    <div class="info-item">
                                                                                        <strong>Channel:</strong>
                                                                                        <div class="info-value"><%# Eval("channel") %></div>
                                                                                    </div>

                                                                                    <div class="info-item">
                                                                                        <strong>Mobile:</strong>
                                                                                        <div class="info-value"><%# Eval("mobile") %></div>
                                                                                    </div>

                                                                                    <div class="info-item">
                                                                                        <strong>AlertType:</strong>
                                                                                        <div class="info-value"><%# Eval("alerttype") %></div>
                                                                                    </div>

                                                                                    <div class="info-item">
                                                                                        <strong>Transaction Type:</strong>
                                                                                        <div class="info-value"><%# Eval("transactiontype") %></div>
                                                                                    </div>

                                                                                    <div class="info-item">
                                                                                        <strong>Email:</strong>
                                                                                        <div class="info-value"><%# Eval("mailid") %></div>
                                                                                    </div>

                                                                                    <div class="info-item">
                                                                                        <strong>EmailCc:</strong>
                                                                                        <div class="info-value"><%# Eval("emailcc") %></div>
                                                                                    </div>

                                                                                    <div class="info-item">
                                                                                        <strong>Fetch Interval</strong>
                                                                                        <div class="info-value"><%# Eval("maxretrycount") %> Min</div>
                                                                                    </div>

                                                                                    <div class="my-4">
                                                                                        <div class="card">
                                                                                            <div class="card-header">
                                                                                                <strong>Email Body</strong>
                                                                                            </div>
                                                                                            <div class="card-body">
                                                                                                <div class="email-body">
                                                                                                    <div class="email-preview" id="emailPreview">
                                                                                                        <textarea class="form-control" rows="2" readonly><%# HttpUtility.HtmlEncode(Eval("mailbody").ToString()) %></textarea>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
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
        });

    </script>

    <%-- Model Group Add --%>
    <script>
        $(document).ready(function () {
            var showModal = document.getElementById('<%= hdnShowModalG.ClientID %>').value;

            if (showModal === "true") {
                var exampleModal = new bootstrap.Modal(document.getElementById('exampleModal'));
                exampleModal.show();
            }
        });
    </script>
    <%-- Model Alert Add --%>
    <script>
        $(document).ready(function () {
            var showModal = document.getElementById('<%= hdnShowModalR.ClientID %>').value;
            if (showModal === "true") {
                var exampleModalR = new bootstrap.Modal(document.getElementById('exampleModalR'));
                exampleModalR.show();
            }
        });
    </script>

    <%--<script>
        $(document).ready(function () {
            var $ddlColumnSelected = $('#<%= ddlColumnSelected.ClientID %>');
            var $divddlResponseCode = $('#<%= divddlResponseCode.ClientID %>');
            var $divddlTransationType = $('#<%= divddlTransationType.ClientID %>');
            var $divOption3 = $('#<%= divddlTransationType.ClientID %>');
            var $ddlResponseCode = $('#<%= ddlResponseCode.ClientID %>');
            var $ddlTxnType = $('#<%= ddlTxnType.ClientID %>');
            var $hdnValueSelc = $('#<%= hdnValueSelc.ClientID %>');

            $ddlColumnSelected.change(function () {
                e.preventDefault(); // Prevent default behavior
                e.stopPropagation(); // Prevent event bubbling

                alert("event occurred");
                var selectedValue = $(this).val();
                $divddlResponseCode.add($divddlTransationType).add($divOption3).hide();
                if (selectedValue === "nfs_response_code") {
                    $divddlResponseCode.show();
                } else if (selectedValue === "transaction_type") {
                    $divddlTransationType.show();
                } else {
                    $divddlResponseCode.show();
                }
            });

            $ddlResponseCode.add($ddlTxnType).change(function () {
                var selectedValue;

                if ($(this).is($ddlResponseCode)) {
                    selectedValue = $ddlResponseCode.val();
                } else if ($(this).is($ddlTxnType)) {
                    selectedValue = $ddlTxnType.find('option:selected').text();
                }
                $hdnValueSelc.val(selectedValue);
            });
        });



        function addCondition() {
            var declineCountElement = document.getElementById('CPHMasterMain_txtConsicativeDeclineCount');
            var declineCount = declineCountElement ? declineCountElement.value.trim() : '';

            var txnTypeElement = document.getElementById('<%= ddlTxnType.ClientID %>');
            var txnType = txnTypeElement ? txnTypeElement.value : '';

            var columnFieldElement = document.getElementById('<%= ddlColumnSelected.ClientID %>');
            var columnField = columnFieldElement ? columnFieldElement.value : '';

            var valueSelectedElement = document.getElementById('<%= hdnValueSelc.ClientID %>');
            var valueSelected = valueSelectedElement ? valueSelectedElement.value : '';

            var retryIntervalElement = document.getElementById('<%= txtTimerInterval.ClientID %>');
            var retryInterval = retryIntervalElement ? retryIntervalElement.value.trim() : '';

            var maxRetryCountElement = document.getElementById('<%= txtmaxRetry.ClientID %>');
            console.log(maxRetryCountElement);
            var maxRetryCount = maxRetryCountElement ? maxRetryCountElement.value.trim() : '';

            var nextIntervalElement = document.getElementById('<%= txtNxtInterval.ClientID %>');
            var nextInterval = nextIntervalElement ? nextIntervalElement.value.trim() : '';

            var newCondition = '';
            var anyConditionAdded = false;
            var conditions = JSON.parse(localStorage.getItem('Conditions')) || [];

            /*
            if (declineCount !== '') {
                var declineOperator = document.getElementById('ddlConditions').value;
                newCondition += `DeclineCount ${declineOperator} ${declineCount}`;
                anyConditionAdded = true;
            }
            */

            // Logic for Transaction Type (commented out in original code)
            /*
            if (txnType !== '0') {
                if (anyConditionAdded) newCondition += ` ${document.getElementById('ddlOperator0').value} `;
                var transactionOperator = document.getElementById('ddlConditions').value;
                newCondition += `TransactionType ${transactionOperator} ${txnType}`;
                anyConditionAdded = true;
            }
            */

            //if (columnField !== '') {
            //    if (document.getElementById('txtQueryPreview').value.trim() !== '') {
            //        newCondition += ` ${document.getElementById('ddlOperator0').value} `;
            //    }
            //    var columnOperator = document.getElementById('ddlConditions').value;
            //    newCondition += `${columnField} ${columnOperator} '${valueSelected}'`;
            //    anyConditionAdded = true;
            //}

            /*
            if (retryInterval !== '') {
                if (anyConditionAdded) newCondition += ` ${document.getElementById('ddlOperator0').value} `;
                var retryOperator = document.getElementById('ddlConditions').value;
                newCondition += `RetryCount ${retryOperator} ${retryInterval}`;
                anyConditionAdded = true;
            }
            */

            /*
            if (maxRetryCount !== '') {
                if (anyConditionAdded) newCondition += ` ${document.getElementById('ddlOperator0').value} `;
                var maxRetryOperator = document.getElementById('ddlConditions').value;
                newCondition += `MaxRetryCount ${maxRetryOperator} ${maxRetryCount}`;
                anyConditionAdded = true;
            }
            */

            /*
            if (nextInterval !== '') {
                if (anyConditionAdded) newCondition += ` ${document.getElementById('ddlOperator0').value} `;
                var nextOperator = document.getElementById('ddlConditions').value;
                newCondition += `NextInterval ${nextOperator} ${nextInterval}`;
                anyConditionAdded = true;
            }
            */


            // Reference txtQueryPreview with the correct ClientID
            var queryPreviewElement = document.getElementById('<%= txtQueryPreview.ClientID %>');

            if (columnField !== '') {
                // Ensure txtQueryPreview is not null before trying to access its value
                if (queryPreviewElement && queryPreviewElement.value.trim() !== '') {
                    newCondition += ` ${document.getElementById('<%= ddlOperator0.ClientID %>').value} `;
                }
                var columnOperator = document.getElementById('<%= ddlConditions.ClientID %>').value;
                newCondition += `${columnField} ${columnOperator} '${valueSelected}'`;
                anyConditionAdded = true;
            }

            if (newCondition !== '') {
                conditions.push(newCondition);
                localStorage.setItem('Conditions', JSON.stringify(conditions));
            }

            // Ensure txtQueryPreview is updated after the conditions are added
            if (queryPreviewElement) {
                queryPreviewElement.value = conditions.join(' ');
            }


            if (newCondition !== '') {
                conditions.push(newCondition);
                localStorage.setItem('Conditions', JSON.stringify(conditions));
            }
            document.getElementById('txtQueryPreview').value = conditions.join(' ');
            alert(document.getElementById('txtQueryPreview').value);
        }

        function btnClearCondition() {
            document.getElementById('txtQueryPreview').value = '';
            localStorage.setItem('Conditions', JSON.stringify([]));
        }

    </script>--%>

    <script type="text/javascript">

        Sys.Application.add_load(function () {
            var $ddlColumnSelected = $('#<%= ddlColumnSelected.ClientID %>');
            var $divddlResponseCode = $('#<%= divddlResponseCode.ClientID %>');
            var $divddlTransationType = $('#<%= divddlTransationType.ClientID %>');
            var $divOption3 = $('#<%= divddlTransationType.ClientID %>');  // This might be redundant, please check
            var $ddlResponseCode = $('#<%= ddlResponseCode.ClientID %>');
            var $ddlTxnType = $('#<%= ddlTxnType.ClientID %>');
            var $hdnValueSelc = $('#<%= hdnValueSelc.ClientID %>');

            $ddlColumnSelected.change(function () {
                //alert("event occurred");
                var selectedValue = $(this).val();
                $divddlResponseCode.add($divddlTransationType).add($divOption3).hide();

                if (selectedValue === "nfs_response_code") {
                    $divddlResponseCode.show();
                } else if (selectedValue === "transaction_type") {
                    $divddlTransationType.show();
                } else {
                    $divddlResponseCode.show();
                }
            });

            $ddlResponseCode.add($ddlTxnType).change(function () {
                var selectedValue;
                if ($(this).is($ddlResponseCode)) {
                    selectedValue = $ddlResponseCode.val();
                } else if ($(this).is($ddlTxnType)) {
                    selectedValue = $ddlTxnType.find('option:selected').text();
                }
                $hdnValueSelc.val(selectedValue);
            });
        });
    </script>


    <script>
        function addRow() {
            var gridView = document.getElementById('<%= gvConditions.ClientID %>');

            var firstRow = gridView.rows[1];
            var newRow = firstRow.cloneNode(true);

            var dropDowns = newRow.getElementsByTagName('select');
            for (var i = 0; i < dropDowns.length; i++) {
                dropDowns[i].selectedIndex = 0;
            }

            var buttonsContainer = newRow.querySelector("div");
            buttonsContainer.style.textAlign = "center";

            gridView.appendChild(newRow);
        }

        function deleteRow(button) {
            var row = button.closest('tr');
            row.parentNode.removeChild(row);
        }
    </script>


</asp:Content>
