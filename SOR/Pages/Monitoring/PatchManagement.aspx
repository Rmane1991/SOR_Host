<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SOR.Master" CodeBehind="PatchManagement.aspx.cs" Inherits="SOR.Pages.Monitoring.PatchManagement" Async="true" %>

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
        /* Reset styles */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        /* Container Styling */
        .main-content-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }

        .card-header {
            /*padding: 15px 20px;*/
            font-size: 20px;
            font-weight: bold;
            color: #333; /* Dark text color */
            border-bottom: 2px solid #ddd; /* Light grey border */
        }

        .flex-space {
            flex-grow: 0.5;
        }

        .card-body {
            padding: 20px;
        }

        /* Input container styling */
        .col-md-4 {
            margin-bottom: 15px;
        }

        /* Label styling */
        label {
            font-size: 14px;
            font-weight: bold;
            color: #333;
            display: block;
            margin-bottom: 8px;
        }

        /* Input field styling */
        .input-field {
            width: 100%;
            padding: 12px;
            font-size: 16px;
            border: 1px solid #ccc;
            border-radius: 6px;
            outline: none;
            background-color: #f8f8f8;
            transition: all 0.3s ease;
        }

            .input-field:focus {
                border-color: #007BFF;
                background-color: #fff;
            }

        /* Button styling */
        .action-btn {
            padding: 12px 20px;
            background-color: #007BFF;
            color: #fff;
            /*font-size: 16px;*/
            font-weight: bold;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

            .action-btn:hover {
                background-color: #0056b3;
            }

        /* Align the button properly within its column */
        .d-flex {
            display: flex;
            justify-content: flex-start; /* Align the button to the left side of the column */
        }

        /* Spacing between the rows */
        .mb-4 {
            margin-bottom: 25px;
        }

        /* Add square brackets around each row */
        .bracketed-row {
            border: 1px solid #007BFF;
            padding: 10px;
            border-radius: 8px;
        }

        /* Responsive Design */
        @media (max-width: 768px) {
            .col-md-4 {
                width: 100%; /* Full width on smaller screens */
                margin-bottom: 15px;
            }

            .action-btn {
                width: auto; /* Let the button size adjust to its content */
            }

            /*.bracketed-row {
                margin-bottom: 15px;
            }*/
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">


    <div class="main-content-container container-fluid px-4">
        <div class="page-header py-4 no-gutters row">
            <div class="text-sm-left mb-3 text-center text-md-left mb-sm-0 col-12 col-sm-4">
            </div>
        </div>

        <!-- Card Container -->
        <div class="card shadow-sm mb-4 card-custom">
            <div class="card-header">
                <div class="col-md-12 row">
                    <div class="col-md-11">
                        <h4 class="m-0">Backup and Replace files</h4>
                    </div>
                    <div class="col-md-1">
                        <button type="button" class="btn btn-secondary float-end" id="resetBtn">Reset</button>
                    </div>
                </div>

                <%--<div class="bg-info clearfix">
                    <button type="button" class="btn btn-secondary float-start">Backup and Replace Paths</button>
                    <button type="button" class="btn btn-secondary float-end">Reset</button>
                </div>--%>
            </div>

            <!-- Empty Space using Flex -->
            <div class="flex-space"></div>

            <div class="card-body">
                <!-- First Row: Source and Destination with Backup Button -->
                <div class="row mb-4 bracketed-row" hidden="hidden">
                    <div class="col-md-4">
                        <input type="text" id="sourcePath" class="input-field" placeholder="Enter or select source path" required>
                    </div>
                    <div class="col-md-4">
                        <input type="text" id="destPath" class="input-field" placeholder="Enter or select destination path" required>
                    </div>
                    <div class="col-md-4">
                        <button type="button" class="action-btn" id="backupBtn">Backup</button>
                    </div>
                </div>

                <!-- Second Row: Source and Destination with Replace Button -->
                <div class="row mb-4 bracketed-row">
                    <div class="col-md-4">
                        <input type="text" id="sourcePath2" class="input-field" placeholder="Enter or select source path">
                    </div>
                    <div class="col-md-4">
                        <input type="text" id="destPath2" class="input-field" placeholder="Enter or select destination path">
                    </div>
                    <div class="col-md-4">
                        <button type="button" class="action-btn" id="replaceBtn">Replace</button>
                    </div>
                </div>
                <!-- Third Row: Source and Destination with Revert Button -->
                <div class="row mb-4 bracketed-row">
                    <div class="col-md-4">
                        <input type="text" id="sourcePath3" class="input-field" placeholder="Enter or select back path">
                    </div>
                    <div class="col-md-4">
                        <input type="text" id="destPath3" class="input-field" placeholder="Enter or select destination path">
                    </div>
                    <div class="col-md-4">
                        <button type="button" class="action-btn" id="revertBtn">Revert Patch</button>
                    </div>
                </div>

            </div>
        </div>
    </div>


    <script type="text/javascript">
        document.getElementById('backupBtn').addEventListener('click', function () {
            const source = document.getElementById('sourcePath').value;
            const destination = document.getElementById('destPath').value;

            if (source && destination) {
                $.ajax({
                    type: "POST",
                    url: "PatchManagement.aspx/ManageFileReplacement",
                    data: JSON.stringify({
                        Source: source,
                        Distination: destination,
                        Type: 'Backup'
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var services = JSON.parse(response.d);
                        if (services.success) {
                            alert(services.message);
                        } else {
                            alert('Error: ' + response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert('An error occurred: ' + error);
                    }
                });
            } else {
                alert('Please enter both source and destination paths.');
            }
        });


        document.getElementById('replaceBtn').addEventListener('click', function () {
            const source = document.getElementById('sourcePath2').value;
            const destination = document.getElementById('destPath2').value;

            if (source && destination) {
                $.ajax({
                    type: "POST",
                    url: "PatchManagement.aspx/ManageFileReplacement",
                    data: JSON.stringify({
                        Source: source,
                        Distination: destination,
                        Type: 'Replace'
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var services = JSON.parse(response.d);
                        if (services) {
                            alert(services);
                        } else {
                            alert('Error: ' + response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert('An error occurred: ' + error);
                    }
                });
            } else {
                alert('Please enter both source and destination paths.');
            }
        });

        document.getElementById('revertBtn').addEventListener('click', function () {
            const source = document.getElementById('sourcePath3').value;
            const destination = document.getElementById('destPath3').value;

            if (source && destination) {
                $.ajax({
                    type: "POST",
                    url: "PatchManagement.aspx/ManageFileReplacement",
                    data: JSON.stringify({
                        Source: source,
                        Distination: destination,
                        Type: 'Revert'
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var services = JSON.parse(response.d);
                        if (services.success) {
                            alert(services.message);
                        } else {
                            alert('Error: ' + response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert('An error occurred: ' + error);
                    }
                });
            } else {
                alert('Please enter both source and destination paths.');
            }
        });

        

        document.getElementById('resetBtn').addEventListener('click', function () {

            const source1 = document.getElementById('sourcePath').value;
            const destination1 = document.getElementById('destPath').value;
            const source2 = document.getElementById('sourcePath2').value;
            const destination2 = document.getElementById('destPath2').value;
            const source3 = document.getElementById('sourcePath3').value;
            const destination3 = document.getElementById('destPath3').value;

            document.getElementById('sourcePath').value = '';
            document.getElementById('destPath').value = '';
            document.getElementById('sourcePath2').value = '';
            document.getElementById('destPath2').value = '';
            document.getElementById('sourcePath3').value = '';
            document.getElementById('destPath3').value = '';

        });



    </script>

</asp:Content>
