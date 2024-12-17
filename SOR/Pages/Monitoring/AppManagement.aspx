<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SOR.Master" CodeBehind="AppManagement.aspx.cs" Inherits="SOR.Pages.Monitoring.AppManagement" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
    <link href="../../css/CustomeDataTable.css" rel="stylesheet" />
    <!-- Include jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- Include DataTables with Bootstrap styling -->
    <link rel="stylesheet" href="https://cdn.datatables.net/1.11.5/css/dataTables.bootstrap4.min.css">
    <script src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.11.5/js/dataTables.bootstrap4.min.js"></script>

    <style>
        .service-container {
            display: flex;
            align-items: center; /* Align items to the center */
        }

        .header-label {
            font-weight: bold;
        }

        .service-name {
            /* Your original styling */
        }

        .btn {
            /* Button styles */
        }

        .footer-container {
            /* Footer styles */
        }

        .service-container {
            background: #fff;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
            padding: 5px;
            margin-bottom: 0px;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .service-name {
            font-size: 12px;
            margin: 0;
        }

        .btn {
            padding: 5px 25px;
            border: none;
            border-radius: 5px;
            color: #fff;
            cursor: pointer;
            transition: background 0.3s, transform 0.2s; /* Add transform transition */
            font-size: 12px; /* Improve readability */
        }

        .btn-start {
            background-color: #007bff; /* Bright Blue */
        }

        .btn-stop {
            background-color: #ff7f50; /* Coral Orange */
        }

        /* Hover Effects */
        .btn:hover {
            opacity: 0.9; /* Slightly less opaque */
            transform: translateY(-2px); /* Lift effect */
        }

        .btn:active {
            transform: translateY(1px); /* Slight push down when clicked */
        }

        /* Additional Colors for Disabled State */
        .btn.disabled {
            background-color: #6c757d; /* Gray */
            cursor: not-allowed;
            opacity: 0.6;
        }


        /* Table container and structure */
        .service-table {
            width: 100%;
            border-collapse: collapse;
            font-family: Arial, sans-serif;
        }

        thead {
            background-color: #f8f9fa; /* Light background for the header */
            border-bottom: 0px solid #dee2e6; /* Thin border under the header */
        }

        th {
            padding: 10px 15px; /* Add padding for spacing */
            text-align: left; /* Left-align text for a clean look */
            font-size: 14px; /* Slightly smaller font size for headers */
            font-weight: bold; /* Bold font for headers */
            color: #333; /* Dark text color for contrast */
            /*background-color: #4682b4;*/ /* Steel Blue */
            color: white; /* White text color */
            border-right: 1px solid #dee2e6; /* Light border between columns */
        }

            th:last-child {
                border-right: none;
            }

        tbody td {
            padding: 1px 15px; /* Padding for the table data cells */
            border-bottom: 1px solid #e9ecef; /* Light border between rows */
            font-size: 14px; /* Standard font size for body */
            color: #495057; /* Dark text color for body rows */
        }

        tbody tr:nth-child(even) {
            background-color: #f8f9fa;
        }

        table.dataTable thead tr {
            background-color: #4682b4;
            background-size: 20%;
        }


        /* Table container and structure */
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">

    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:UpdateProgress ID="upContentBodyUpdateProgress" runat="server" AssociatedUpdatePanelID="upContentBody">
            <ProgressTemplate>
                <div style="width: 100%; height: 100%; opacity: 0.8; background-color: black; position: fixed; top: 0; left: 0">
                    <img alt="" id="progressImage1" style="margin-top: 20%" src='<%=Page.ResolveClientUrl("../../Images/loading2_1.gif") %>' />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>

    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" DropShadow="false" />

    <asp:UpdatePanel ID="upContentBody" runat="server">
        <ContentTemplate>
            <div class="main-content-container container-fluid px-4">
                <div class="page-header py-4 no-gutters row">
                    <div class="text-sm-left mb-3 text-center text-md-left mb-sm-0 col-12 col-sm-4">
                        <h3 class="page-title">Service Management</h3>
                    </div>
                </div>

                <div class="page-header py-4 no-gutters row">
                    <div class="text-sm-left mb-3 text-center text-md-left mb-sm-0 col-12 col-sm-4">
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="">
                        <button type="button" class="btn btn-start" data-attribure="Start" onclick="handleServices(this)">Start All</button>
                        <button type="button" class="btn btn-stop" data-attribure="Stop" onclick="handleServices(this)">Stop All</button>
                    </div>
                </div>

                <table class="service-table" id="tblServices" runat="server">
                    <thead>
                        <tr class="service-container">
                            <th class="col-md-1 header-label font-weight-bold">
                                <input type="checkbox" id="selectAll" onchange="toggleCheckboxes(this)" />
                                Select
                            </th>
                            <th class="header-label font-weight-bold">Service Name</th>
                            <th class="header-label font-weight-bold">Status</th>
                            <th class="header-label font-weight-bold">Error</th>
                            <th class="header-label font-weight-bold">Up Time</th>
                            <th class="header-label font-weight-bold">Down Time</th>
                            <th class="header-label font-weight-bold">Actions</th>
                        </tr>
                    </thead>
                    <tbody id="tbody">
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="7" class="footer-container"></td>
                        </tr>
                    </tfoot>
                </table>
                <!-- Modal Structure -->
                <div class="modal fade" id="serviceModal" tabindex="-1" aria-labelledby="serviceModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="serviceModalLabel">Manage Selected Services</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <p id="serviceList"></p>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-start" onclick="startSelectedServices()">Start</button>
                                <button type="button" class="btn btn-stop" onclick="stopSelectedServices()">Stop</button>
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- End Modal Structure -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        let selectedServices = [];
        function toggleCheckboxes(selectAllCheckbox) {
            const checkboxes = document.querySelectorAll('#tbody input[type="checkbox"]');
            selectedServices = [];
            checkboxes.forEach(checkbox => {
                checkbox.checked = selectAllCheckbox.checked;
                if (checkbox.checked) {
                    const serviceRow = checkbox.closest('tr');
                    const serviceName = serviceRow.querySelector('.service-name').innerText;
                    selectedServices.push(serviceName);
                }
            });
            if (selectAllCheckbox.checked) {
                const serviceListText = selectedServices.join(', ');
                document.getElementById('serviceList').innerText = serviceListText;
                //$('#serviceModal').modal('show');
            }
        }

        function fetchUpdatedServices() {
            var popup = $find('<%= mpeProgress.ClientID %>');
            popup.show();
            $.ajax({
                type: "POST",
                url: "AppManagement.aspx/GetUpdatedServices",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var services = JSON.parse(response.d);
                    let rowsHtml = '';
                    let headerHtml = `
                <thead>
                    <tr class="service-container">
                        <th class="header-label font-weight-bold">
                            <input type="checkbox" id="selectAll" onchange="toggleCheckboxes(this)" />
                            Select
                        </th>
                        <th class="header-label font-weight-bold">Service Name</th>
                        <th class="header-label font-weight-bold">Status</th>
                        <th class="header-label font-weight-bold">Error</th>
                        <th class="header-label font-weight-bold">Up Time</th>
                        <th class="header-label font-weight-bold">Down Time</th>
                        <th class="header-label font-weight-bold">Actions</th>
                    </tr>
                </thead>
            `;

                    $.each(services, function (index, service) {
                        rowsHtml += '<tr class="service-container">';
                        rowsHtml += '<td class=""><input type="checkbox" /></td>';
                        rowsHtml += '<td class=""><label class="service-name">' + service.ServiceName + '</label></td>';
                        rowsHtml += '<td class=""><label class="service-name Status">' + service.Status + '</label></td>';
                        rowsHtml += '<td class=""><label class="service-name Error">' + service.Error + '</label></td>';
                        rowsHtml += '<td class=""><label class="service-name UpTime">' + service.UpTime + '</label></td>';
                        rowsHtml += '<td class=""><label class="service-name DownTime">' + service.DownTime + '</label></td>';
                        rowsHtml += '<td class="">';
                        rowsHtml += '<button class="btn btn-start" onclick="ServiceStart(\'' + service.ServiceName + '\', \'' + service.Status + '\', \'' + service.Error + '\')">Start</button>';
                        rowsHtml += '<button class="btn btn-stop" onclick="ServiceStop(\'' + service.ServiceName + '\', \'' + service.Status + '\', \'' + service.Error + '\')">Stop</button>';
                        rowsHtml += '</td></tr>';
                    });
                    let fullHtml = headerHtml + '<tbody id="tbody">' + rowsHtml + '</tbody>';
                    $('#<%= tblServices.ClientID %>').html(fullHtml);
                    document.getElementById('selectAll').checked = false;

                    if (!$.fn.dataTable.isDataTable('#<%= tblServices.ClientID %>')) {
                        $('#<%= tblServices.ClientID %>').DataTable({
                            paging: true,
                            searching: true,
                            lengthChange: false,
                            language: {
                                searchPlaceholder: "Search services..."
                            }
                        });
                    }
                },
                error: function (response) {
                    console.error("Error fetching services:", response);
                },
                complete: function () {
                    popup.hide();
                }

            });
        }

        setInterval(function () {
            var popup = $find('<%= mpeProgress.ClientID %>');
            popup.show();
            $.ajax({
                type: "POST",
                url: "AppManagement.aspx/GetUpdatedServices",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var services = JSON.parse(response.d);
                    let rowsHtml = '';
                    let headerHtml = `
                <thead>
                    <tr class="service-container">
                        <th class="header-label font-weight-bold">
                            <input type="checkbox" id="selectAll" onchange="toggleCheckboxes(this)" />
                            Select
                        </th>
                        <th class="header-label font-weight-bold">Service Name</th>
                        <th class="header-label font-weight-bold">Status</th>
                        <th class="header-label font-weight-bold">Error</th>
                        <th class="header-label font-weight-bold">Up Time</th>
                        <th class="header-label font-weight-bold">Down Time</th>
                        <th class="header-label font-weight-bold">Actions</th>
                    </tr>
                </thead>
            `;
                    $.each(services, function (index, service) {
                        rowsHtml += '<tr class="service-container">';
                        rowsHtml += '<td class=""><input type="checkbox" /></td>';
                        rowsHtml += '<td class=""><label class="service-name">' + service.ServiceName + '</label></td>';
                        rowsHtml += '<td class=""><label class="service-name Status">' + service.Status + '</label></td>';
                        rowsHtml += '<td class=""><label class="service-name Error">' + service.Error + '</label></td>';
                        rowsHtml += '<td class=""><label class="service-name UpTime">' + service.UpTime + '</label></td>';
                        rowsHtml += '<td class=""><label class="service-name DownTime">' + service.DownTime + '</label></td>';
                        rowsHtml += '<td class="">';
                        rowsHtml += '<button class="btn btn-start" onclick="ServiceStart(\'' + service.ServiceName + '\', \'' + service.Status + '\', \'' + service.Error + '\')">Start</button>';
                        rowsHtml += '<button class="btn btn-stop" onclick="ServiceStop(\'' + service.ServiceName + '\', \'' + service.Status + '\', \'' + service.Error + '\')">Stop</button>';
                        rowsHtml += '</td></tr>';
                    });
                    let fullHtml = headerHtml + '<tbody id="tbody">' + rowsHtml + '</tbody>';
                    $('#<%= tblServices.ClientID %>').html(fullHtml);
                    document.getElementById('selectAll').checked = false;
                    if (!$.fn.dataTable.isDataTable('#<%= tblServices.ClientID %>')) {
                        $('#<%= tblServices.ClientID %>').DataTable({
                            paging: true,
                            searching: true,
                            lengthChange: false,
                            language: {
                                searchPlaceholder: "Search services..."
                            }
                        });
                    }
                },
                error: function (response) {
                    console.error("Error fetching services:", response);
                },
                complete: function () {
                    popup.hide();
                }
            });
        }, 50000);//50sec


        function ServiceStop(serviceName, status, error) {
            var popup = $find('<%= mpeProgress.ClientID %>');
            popup.show();
            $.ajax({
                type: "POST",
                url: "AppManagement.aspx/StopService",
                data: JSON.stringify({ serviceName: serviceName, status: status, error: error }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var services = JSON.parse(response.d);
                    alert("status:", "Sucess");
                    fetchUpdatedServices();
                    return;
                },
                error: function (response) {
                    console.error("Error stopping service:", response);
                },
                complete: function () {
                    popup.hide();
                }
            });
        }

        function ServiceStart(serviceName, status, error) {
            var popup = $find('<%= mpeProgress.ClientID %>');
            popup.show();
            $.ajax({
                type: "POST",
                url: "AppManagement.aspx/StartService",
                data: JSON.stringify({ serviceName: serviceName, status: status, error: error }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var services = JSON.parse(response.d);
                    alert("status:", "Sucess");
                    fetchUpdatedServices();
                    return;
                },
                error: function (response) {
                    console.error("Error starting service:", response);
                },
                complete: function () {
                    popup.hide();
                }
            });
        }

        function handleServices(button) {
            var popup = $find('<%= mpeProgress.ClientID %>');
            popup.show();

            var Type = $(button).attr("data-attribure");
            var selectedServices = [];

            const checkboxes = document.querySelectorAll('#tbody input[type="checkbox"]:checked');
            checkboxes.forEach(checkbox => {
                const serviceRow = checkbox.closest('tr');
                const serviceName = serviceRow.querySelector('.service-name').innerText;
                const status = serviceRow.querySelector('.Status').innerText;
                const error = serviceRow.querySelector('.Error').innerText;
                selectedServices.push({
                    ServiceName: serviceName,
                    ServiceStatus: status,
                    UserId: "1",
                    DownTime: "",
                    UpTime: "",
                    Error: error
                });
            });

            if (selectedServices.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "AppManagement.aspx/ManageServices",
                    data: JSON.stringify({ services: selectedServices, Type: Type }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var services = JSON.parse(response.d);
                        console.log(services);
                        if (Type === "Start") {
                            alert("Services started successfully");
                        } else if (Type === "Stop") {
                            alert("Services stopped successfully");
                        }
                        fetchUpdatedServices();

                    },
                    error: function (response) {
                        console.error("Error processing services:", response);
                        alert("There was an error with the operation.");
                    },
                    complete: function () {
                        popup.hide();
                    }
                });
            } else {
                alert("Please select at least one service.");
            }
        }

    </script>

    <script type="text/javascript">
        var $j = jQuery.noConflict();
        $j(document).ready(function () {
            fetchUpdatedServices();
        });
    </script>



</asp:Content>
