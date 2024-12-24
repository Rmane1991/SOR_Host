<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SOR.Master" CodeBehind="DashBoard.aspx.cs" Inherits="SOR.Pages.Dashboard.DashBoard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" id="main-stylesheet" data-version="1.1.0" href="../../DashStyles/shards-dashboards.1.1.0.min.css">
    <link rel="stylesheet" href="../../DashStyles/extras.1.1.0.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
    <!--for Chart.js 4.x -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@2.0.0"></script>
    <!--for ApexChart -->
    <script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
    <!--for Google chart -->
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <!--for High chart -->
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/export-data.js"></script>
    <script src="https://code.highcharts.com/modules/accessibility.js"></script>

    <!-- Add required JavaScript libraries -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/0.4.1/html2canvas.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.3/xlsx.full.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>



    <%--**MILIND|18102024--%>
    <style>
        .go-stats__chart canvas {
            width: 100%; /* Make the canvas responsive */
            height: auto; /* Maintain aspect ratio */
        }

        .btn-white {
            background-color: white;
            color: black;
        }

            .btn-white.active {
                background-color: #007bff; /* Change to your desired active color */
                color: white;
            }
    </style>
    <style>
        [class^="icon-"], [class*=" icon-"] {
            margin-bottom: 10px;
        }
    </style>
    <style>
        .navbar {
            padding: 0 !important; /* Override the existing padding */
        }
    </style>

    <%-- *Hide left bar menu defult*--%>
    <%--<script>
        $(document).ready(function () {
            $('#slider-btn').addClass('rotate');
            $('.sidebar').addClass('closed');
        });
        $('#slider-btn').click(function () {
            $(this).removeClass('rotate');
            $('.sidebar').removeClass('closed');
            $(this).toggleClass('rotate');
            $('.sidebar').toggleClass('closed');
        });
    </script>--%>
    <%-- *Hide left bar menu defult*--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div class="main-content-container container-fluid px-4">
        <!-- Page Header -->
        <div class="page-header py-4 no-gutters row">
            <div class="text-sm-left mb-3 text-center text-md-left mb-sm-0 col-12 col-sm-4">
            </div>
            <div class="col d-flex align-items-center col-12 col-sm-4">
                <div class="d-inline-flex mb-3 mb-sm-0 mx-auto btn-group-sm btn-group"></div>
            </div>
            <div class="d-flex col-sm-4">
                <div class="justify-content-end d-flex my-auto date-range input-group">
                    <input type="text" id="startDate" placeholder="Start Date" class="text-center form-control form-control-sm" value="">
                    <input type="text" id="endDate" placeholder="End Date" class="text-center form-control form-control-sm" value="">
                    <span class="input-group-append">
                        <button type="button" class="btn btn-primary GlobalFilter" data-attribute="GlobalFilter" data-text="DateRange">Search &rarr;</button>
                    </span>
                </div>
            </div>
        </div>
        <!-- End Page Header -->
        <!-- Small Stats Blocks -->
        <div class="row">
            <div class="col-lg col-md-6 col-sm-6 mb-4">
                <div class="stats-small stats-small--1 card card-small">
                    <div class="card-body p-0 d-flex">
                        <div class="d-flex flex-column m-auto">
                            <div class="stats-small__data text-center">
                                <span class="stats-small__label text-uppercase">Transactions</span>
                                <h6 id="TxnCount" runat="server" class="stats-small__value count my-3"></h6>
                            </div>
                            <div class="stats-small__data">
                                <%-- <span class="stats-small__percentage stats-small__percentage--increase">4.7%</span>--%>
                            </div>
                        </div>
                        <canvas height="120" class="blog-overview-stats-small-1"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg col-md-6 col-sm-6 mb-4">
                <div class="stats-small stats-small--1 card card-small">
                    <div class="card-body p-0 d-flex">
                        <div class="d-flex flex-column m-auto">
                            <div class="stats-small__data text-center">
                                <span class="stats-small__label text-uppercase">BCs</span>
                                <h6 id="bcCount" runat="server" class="stats-small__value count my-3"></h6>
                            </div>
                            <div class="stats-small__data">
                                <%--<span class="stats-small__percentage stats-small__percentage--increase">12.4%</span>--%>
                            </div>
                        </div>
                        <canvas height="120" class="blog-overview-stats-small-2"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg col-md-4 col-sm-6 mb-4">
                <div class="stats-small stats-small--1 card card-small">
                    <div class="card-body p-0 d-flex">
                        <div class="d-flex flex-column m-auto">
                            <div class="stats-small__data text-center">
                                <span class="stats-small__label text-uppercase">Aggregators</span>
                                <h6 id="aggregatorCount" runat="server" class="stats-small__value count my-3"></h6>
                            </div>
                            <div class="stats-small__data">
                                <%--<span class="stats-small__percentage stats-small__percentage--decrease">3.8%</span>--%>
                            </div>
                        </div>
                        <canvas height="120" class="blog-overview-stats-small-3"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg col-md-4 col-sm-6 mb-4">
                <div class="stats-small stats-small--1 card card-small">
                    <div class="card-body p-0 d-flex">
                        <div class="d-flex flex-column m-auto">
                            <div class="stats-small__data text-center">
                                <span class="stats-small__label text-uppercase">Agents</span>
                                <h6 id="agentCount" runat="server" class="stats-small__value count my-3"></h6>
                            </div>
                            <div class="stats-small__data">
                                <%-- <span class="stats-small__percentage stats-small__percentage--increase">12.4%</span>--%>
                            </div>
                        </div>
                        <canvas height="120" class="blog-overview-stats-small-4"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg col-md-4 col-sm-12 mb-4">
                <div class="stats-small stats-small--1 card card-small">
                    <div class="card-body p-0 d-flex">
                        <div class="d-flex flex-column m-auto">
                            <div class="stats-small__data text-center">
                                <span class="stats-small__label text-uppercase">Partners</span>
                                <h6 id="customerCount" runat="server" class="stats-small__value count my-3">17,281</h6>
                            </div>
                            <div class="stats-small__data">
                                <%--<span class="stats-small__percentage stats-small__percentage--decrease">2.4%</span>--%>
                            </div>
                        </div>
                        <canvas height="120" class="blog-overview-stats-small-5"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <!-- End Small Stats Blocks -->
        <div class="row">
            <!-- Transaction Summary Stats -->
            <div class="col-lg-8 col-md-12 col-sm-12 mb-4">
                <div class="card card-small h-100">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">Transaction Summary</h6>
                    </div>
                    <div class="card-body pt-0">
                        <div class="row border-bottom py-2 bg-light">
                            <div class="col d-flex mb-2 mb-sm-0 col-sm-6">
                                <div class="btn-group" data-attribute="TransactionSummary">
                                    <%--<button type="button" class="btn btn-white active">Hour</button>
                                    <button type="button" class="btn btn-white">Day</button>--%>
                                    <button type="button" class="btn btn-white active" data-attribute="TransactionSummary" data-text="Week">Week</button>
                                    <button type="button" class="btn btn-white" data-attribute="TransactionSummary" data-text="Month">Month</button>
                                </div>
                            </div>

                            <div class="col d-flex justify-content-end mb-2 mb-sm-0 col-12 col-sm-6">
                                <div id="blog-overview-date-range" class="input-daterange input-group input-group-sm my-auto ml-auto mr-auto ml-sm-auto mr-sm-0" style="max-width: 350px;">
                                    <input type="text" class="input-sm form-control" name="start" placeholder="Start Date" id="blog-overview-date-range-1">
                                    <input type="text" class="input-sm form-control" name="end" placeholder="End Date" id="blog-overview-date-range-2">
                                    <span class="input-group-append">
                                        <button type="button" class="btn btn-primary searchTxnSummary" data-attribute="TransactionSummary" data-text="DateRange">Search →</button>
                                    </span>
                                </div>
                            </div>
                            <%-- <div class="col-12 col-sm-6 d-flex mb-2 mb-sm-0">
                                <button type="button" class="btn btn-sm btn-white ml-auto mr-auto ml-sm-auto mr-sm-0 mt-3 mt-sm-0">View Full Report &rarr;</button>
                            </div>--%>
                        </div>
                        <canvas height="130" style="max-width: 100% !important;" class="blog-overview-users"></canvas>
                    </div>
                </div>
            </div>
            <!-- End Transaction Summary Stats -->

            <!--Aggregator wise data show -->
            <div class="col-lg-4 col-md-6 col-sm-12 mb-4">
                <div class="card card-small h-100">
                    <div class="border-bottom card-header d-flex justify-content-between align-items-center">
                        <h6 class="m-0">
                            <i class="fas fa-chart-line" style="font-size: 1.5rem; margin-right: 0.5rem;"></i>
                            Top 5 Aggregators: Transaction Insights
                        </h6>
                        <div class="block-handle"></div>
                    </div>
                    <div class="p-0 card-body">
                        <div id="aggregatorCarousel" class="carousel slide" data-ride="carousel" style="box-shadow: none;">
                            <!-- Positioning context for absolute positioning -->
                            <div class="carousel-inner" id="aggregator-cards">
                                <!-- Dynamic content will be injected here -->
                                <asp:Literal ID="Aggregator" runat="server"></asp:Literal>
                                <!-- End Aggregator Card -->
                            </div>
                            <a class="carousel-control-prev" href="#aggregatorCarousel" role="button" data-slide="prev">
                                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                <span class="sr-only">Previous</span>
                                <span class="carousel-control-label">
                                    <i class="fas fa-chevron-left"></i>
                                </span>
                            </a>
                            <a class="carousel-control-next" href="#aggregatorCarousel" role="button" data-slide="next">
                                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                <span class="sr-only">Next</span>
                                <span class="carousel-control-label">
                                    <i class="fas fa-chevron-right"></i>
                                </span>
                            </a>
                        </div>
                    </div>
                    <div class="border-top card-footer">
                        <div class="row">
                            <div class="col">
                                <select class="form-control custom-select form-control-sm custom-select-sm filter-0" id="ddlAggreFilter" style="max-width: 130px;">
                                    <option selected value="day">Today</option>
                                    <option value="last_week">Last Week</option>
                                    <option value="last_month">Last Month</option>
                                    <option value="last_year">Last Year</option>
                                </select>
                            </div>
                            <div class="col d-flex justify-content-end mb-2 mb-sm-0">
                                <button type="button" class="btn btn-sm btn-white ml-auto mr-auto" data-toggle="modal" data-target="#reportModal" data-report-type="aggregator">
                                    <i class="fas fa-download"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!--END-->


            <!-- Users By Switch Stats -->
            <div class="col-lg-4 col-md-6 col-sm-12 mb-4">
                <div class="card card-small h-100">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">Switch Transactions Overview</h6>
                    </div>
                    <div class="card-body d-flex py-0">
                        <canvas height="220" class="blog-users-by-device m-auto"></canvas>
                        <%--<div class="blog-users-by-device m-auto" style="width: 100%; height: 220px;"></div>--%>
                    </div>
                    <div class="card-footer border-top">
                        <div class="row">
                            <div class="col">
                                <select class="form-control custom-select form-control-sm custom-select-sm filter-0" id="ddlSwitchFilter" style="max-width: 130px;">
                                    <option selected value="day">Today</option>
                                    <option value="last_week">Last Week</option>
                                    <option value="last_month">Last Month</option>
                                    <option value="last_year">Last Year</option>
                                </select>
                            </div>
                            <%--<div class="col text-right view-report">
                                <a href="#">Full report &rarr;</a>
                            </div>--%>
                            <div class="col d-flex justify-content-end mb-2 mb-sm-0">
                                <button type="button" class="btn btn-sm btn-white ml-auto mr-auto ml-sm-auto mr-sm-0 mt-3 mt-sm-0" id="VewSwitchOveriewRpt"><i class="fas fa-download"></i></button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Modal for Switch Overview Report -->
            <div class="modal fade" id="switchOverviewModal" tabindex="-1" role="dialog" aria-labelledby="switchOverviewModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="switchOverviewModalLabel">Switch Transactions Overview - Full Report</h5>
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                        <div class="modal-body" id="reportContent">
                            <!-- Chart or Graph inside the modal -->
                            <canvas id="modalGraphCanvas" height="220"></canvas>

                            <div class="mt-3">
                                <h6>Details</h6>
                                <p id="modalDetails">Loading...</p>
                            </div>

                            <div class="mt-3">
                                <button id="downloadExcelBtn" class="btn btn-primary">Download Excel Report</button>
                            </div>
                        </div>
                        <div class="modal-footer">
                        </div>
                    </div>
                </div>
            </div>
            <!-- Modal for Switch Overview Report -->
            <!-- End Users By switch Stats -->

            <!-- New Revenue Chart Component -->
            <div class="col-lg-8 col-md-12 col-sm-12 mb-4">
                <div class="card card-small">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">Revenue</h6>
                    </div>
                    <div class="card-body pt-0">
                        <div class="row border-bottom py-2 bg-light">
                            <div class="col d-flex mb-2 mb-sm-0 col-sm-6">
                                <div class="btn-group" data-attribute="RevenueSummary">
                                    <button type="button" class="btn btn-white active" data-attribute="RevenueSummary" data-text="Week">Week</button>
                                    <button type="button" class="btn btn-white" data-attribute="RevenueSummary" data-text="Month">Month</button>
                                </div>
                            </div>
                            <div class="col d-flex justify-content-end mb-2 mb-sm-0 col-12 col-sm-6">
                                <div id="RevenueSummary" class="input-daterange input-group input-group-sm my-auto ml-auto mr-auto ml-sm-auto mr-sm-0" style="max-width: 350px;">
                                    <input type="text" class="input-sm form-control" name="start" placeholder="Start Date" id="RevenueSummary-d1">
                                    <input type="text" class="input-sm form-control" name="end" placeholder="End Date" id="RevenueSummary-d2">
                                    <span class="input-group-append">
                                        <button type="button" class="btn btn-primary searchRevenueSummary" data-attribute="RevenueSummary" data-text="DateRange">Search →</button>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div id="overview-RevenueSummary" style="height: 400px; max-width: 100%;"></div>
                    </div>
                </div>
            </div>
            <!-- END Revenue Chart Component -->


            <!-- New Rule-wise Component hidden-->
            <div class="col-lg-8 col-md-12 col-sm-12 mb-4" hidden="hidden">
                <div class="card card-small h-100">
                    <div class="border-bottom card-header">
                        <h6 class="m-0">Rule Performance Overview</h6>
                        <div class="block-handle"></div>
                    </div>
                    <div class="py-0 card-body" style="max-height: 200px; overflow-y: auto;">
                        <table class="table m-0">
                            <tbody id="tbRule">
                                <asp:Literal ID="ruleLiteralControl" runat="server"></asp:Literal>
                            </tbody>
                        </table>
                    </div>

                    <div class="border-top card-footer">
                        <div class="row">
                            <div class="col">
                                <select class="form-control custom-select form-control-sm custom-select-sm filter-0" id="ddlRuleFilter" style="max-width: 130px;">
                                    <option selected value="today">Today</option>
                                    <option value="last_week">Last Week</option>
                                    <option value="last_month">Last Month</option>
                                    <option value="last_year">Last Year</option>
                                </select>
                            </div>
                            <div class="col d-flex justify-content-end mb-2 mb-sm-0">
                                <button type="button" class="btn btn-sm btn-white ml-auto mr-auto" data-toggle="modal" data-target="#reportModal" data-report-type="rule"><i class="fas fa-download"></i></button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- End Rule-wise Component -->

            <!-- Switch Performance hidden-->
            <div class="mb-4 col-lg-4" hidden="hidden">
                <div class="card card-small h-100 country-stats card card-small">
                    <div class="border-bottom card-header">
                        <h6 class="m-0">Switch Performance Overview</h6>
                        <div class="block-handle"></div>
                    </div>
                    <div class="p-0 card-body">
                        <table class="table m-0">
                            <tbody id="tbSwitch">
                                <asp:Literal ID="SwitchLteralControl" runat="server"></asp:Literal>
                            </tbody>
                        </table>
                    </div>
                    <div class="border-top card-footer">
                        <div class="row">
                            <div class="col">
                                <select class="form-control custom-select form-control-sm custom-select-sm filter-0" id="ddlTopSwitchsFilter" style="max-width: 130px;">
                                    <option selected value="day">Today</option>
                                    <option value="last_week">Last Week</option>
                                    <option value="last_month">Last Month</option>
                                    <option value="last_year">Last Year</option>
                                </select>
                            </div>
                            <%--<div class="text-right view-report col"><a href="#">View full report →</a></div>--%>
                            <div class="col d-flex justify-content-end mb-2 mb-sm-0">
                                <button type="button" class="btn btn-sm btn-white ml-auto mr-auto" data-toggle="modal" data-target="#reportModal" data-report-type="switch">
                                    <i class="fas fa-download"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- End Switch Performance -->

            <!-- Common Modal for Agg|Rule|Switch-->
            <div class="modal fade" id="reportModal" tabindex="-1" role="dialog" aria-labelledby="reportModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="reportModalLabel">Full Report</h5>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                        <div class="modal-body">
                            <div id="modalReportContent">
                                <!-- Content will be populated dynamically -->
                            </div>
                        </div>
                        <div class="modal-footer">
                        </div>
                    </div>
                </div>
            </div>
            <!-- End Common Modal -->

            <!-- BC-wise Summary Txn -->
            <div class="col-lg-8 col-md-12 col-sm-12 mb-4">
                <div class="card card-small">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">BC-Wise Summary</h6>
                    </div>
                    <div class="card-body pt-0">
                        <div class="row border-bottom py-2 bg-light">
                            <div class="col d-flex mb-2 mb-sm-0 col-sm-6">
                                <div class="btn-group" data-attribute="BCTransactionSummary">
                                    <%--<button type="button" class="btn btn-white active">Hour</button>
                                        <button type="button" class="btn btn-white">Day</button>--%>
                                    <button type="button" class="btn btn-white active" data-attribute="BCTransactionSummary" data-text="Week">Week</button>
                                    <button type="button" class="btn btn-white" data-attribute="BCTransactionSummary" data-text="Month">Month</button>
                                </div>
                            </div>
                            <div class="col d-flex justify-content-end mb-2 mb-sm-0 col-12 col-sm-6">
                                <div id="overview-date-bc-wise" class="input-daterange input-group input-group-sm my-auto ml-auto mr-auto ml-sm-auto mr-sm-0" style="max-width: 350px;">
                                    <input type="text" class="input-sm form-control" name="start" placeholder="Start Date" id="overview-date-bc-wise-1">
                                    <input type="text" class="input-sm form-control" name="end" placeholder="End Date" id="overview-date-bc-wise-2">
                                    <span class="input-group-append">
                                        <button type="button" class="btn btn-primary searchBCSummary" data-attribute="BCTransactionSummary" data-text="DateRange">Search →</button>
                                    </span>
                                </div>
                            </div>

                        </div>
                        <%--<canvas height="130" style="max-width: 100% !important;" class="blog-overview-BCWise"></canvas>--%>
                        <div id="blog-overview-BCWise" style="height: 400px; max-width: 100%;"></div>
                    </div>
                </div>
            </div>
            <!-- BC-wise Summary Txn -->


            <!-- Channel-wise  Summary Txn -->
            <div class="col-lg-4 col-md-6 col-sm-12 mb-4">
                <div class="card card-small h-100">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">Channel Wise Transactions Overview</h6>
                    </div>
                    <div class="card-body d-flex py-0">
                        <%--  <canvas height="220" class="blog-Channel-Summary m-auto"></canvas>--%>
                        <div id="piechart_3d" class="blog-Channel-Summary m-auto" style="width: 100%; height: 220px;"></div>
                    </div>
                    <div class="card-footer border-top">
                        <div class="row">
                            <div class="col">
                                <select class="form-control custom-select form-control-sm custom-select-sm filter-0" id="ddlChannelFilter" style="max-width: 130px;">
                                    <option selected value="day">Today</option>
                                    <option value="last_week">Last Week</option>
                                    <option value="last_month">Last Month</option>
                                    <option value="last_year">Last Year</option>
                                </select>
                            </div>
                            <div class="col d-flex justify-content-end mb-2 mb-sm-0">
                                <button type="button" class="btn btn-sm btn-white ml-auto mr-auto ml-sm-auto mr-sm-0 mt-3 mt-sm-0" id="ViewChannelOveriewRpt"><i class="fas fa-download"></i></button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Channel-wise  Summary Txn -->

        </div>
    </div>

    <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.min.js"></script>
    <script src="https://unpkg.com/shards-ui@latest/dist/js/shards.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Sharrre/2.0.1/jquery.sharrre.min.js"></script>
    <script src="../../DashScripts/extras.1.1.0.min.js"></script>
    <script src="../../DashScripts/shards-dashboards.1.1.0.min.js"></script>
    <script src="../../DashScripts/app/app-blog-overview.1.1.0.js"></script>

    <input type="hidden" id="hiddenTransactions" value='<%= HttpUtility.JavaScriptStringEncode(ViewState["Transactions"]?.ToString() ?? "") %>' />
    <input type="hidden" id="hiddenSummaryData" value='<%= HttpUtility.JavaScriptStringEncode(ViewState["summary"]?.ToString() ?? "") %>' />
    <input type="hidden" id="hiddenBCwiseData" value='<%= HttpUtility.JavaScriptStringEncode(ViewState["BcMonthlyData"]?.ToString() ?? "") %>' />
    <input type="hidden" id="hiddenSwitchwiseData" value='<%= HttpUtility.JavaScriptStringEncode(ViewState["SwitchMonthlyData"]?.ToString() ?? "") %>' />
    <input type="hidden" id="hiddenChannelwiseData" value='<%= HttpUtility.JavaScriptStringEncode(ViewState["ChannelwiseData"]?.ToString() ?? "") %>' />
    <input type="hidden" id="hiddenRevenueData" value='<%= HttpUtility.JavaScriptStringEncode(ViewState["BankRevenueData"]?.ToString() ?? "") %>' />

    <script>
        //Filter data of Global Page
        $('.GlobalFilter, .searchTxnSummary, .btn-group button, .searchBCSummary, .searchRevenueSummary').click(function () {
            //var Filter = $(this).text(); //day month
            //var FilterType = $(this).closest('.btn-group').data('attribute'); // Report Name
            var Filter = $(this).data('text');
            var FilterType = $(this).data('attribute');
            var id = $(this).prop('id');
            var fromDate;
            var toDate;

            if ($(this).hasClass('GlobalFilter')) {
                fromDate = $('#startDate').val();
                toDate = $('#endDate').val();
                FilterType = "GlobalFilter";
                Filter = "DateRange";
            }

            if (FilterType == "TransactionSummary") {
                fromDate = $('#blog-overview-date-range-1').val();
                toDate = $('#blog-overview-date-range-2').val();

            } else if (FilterType == "BCTransactionSummary") {
                fromDate = $('#overview-date-bc-wise-1').val();
                toDate = $('#overview-date-bc-wise-2').val();
            }
            else if (FilterType == "RevenueSummary") {
                fromDate = $('#RevenueSummary-d1').val();
                toDate = $('#RevenueSummary-d2').val();
            }

            else {

            }

            $.ajax({
                type: "POST",
                url: "DashBoard.aspx/ChartdataFilter",
                data: JSON.stringify({ fromDate: fromDate, toDate: toDate, FilterType: FilterType, Filter: Filter }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //alert(response.d);
                    //console.log(response.d);
                    const newData = JSON.parse(response.d);

                    if (FilterType === "TransactionSummary" || FilterType === "GlobalFilter") {
                        const txnData = newData.TxnSummaryChart;
                        const currentMonthData = txnData.map(item => item.TxnsummaryCurrentMonthCount);
                        const previousMonthData = txnData.map(item => item.TxnsummaryPreviousMonthCount);

                        BlogOverviewUsers.data.datasets[0].data = currentMonthData;
                        BlogOverviewUsers.data.datasets[1].data = previousMonthData;
                        BlogOverviewUsers.data.labels = txnData.map(item => item.Day);
                        BlogOverviewUsers.update();
                    }

                    if (FilterType === "BCTransactionSummary" || FilterType === "GlobalFilter") {
                        const bcData = newData.BCSummaryChart;
                        const Success = bcData.map(item => parseFloat(item.Success));
                        const Failure = bcData.map(item => parseFloat(item.Failure));
                        const SuccessRate = bcData.map(item => parseFloat(item.SuccessRate));
                        const FailureRate = bcData.map(item => parseFloat(item.FailureRate));
                        const userNames = bcData.map(item => item.bcname);

                        if (window.BCWiseSummaryChart) {
                            window.BCWiseSummaryChart.updateSeries([
                                { name: 'Success', data: Success },
                                { name: 'Failure', data: Failure }
                                /* Uncomment if you want to include SuccessRate and FailureRate
                                { name: 'SuccessRate', data: SuccessRate },
                                { name: 'FailureRate', data: FailureRate }
                                */
                            ], true);

                            window.BCWiseSummaryChart.updateOptions({
                                xaxis: {
                                    categories: userNames
                                }
                            }, true);

                        } else {
                            alert('Chart is not initialized.');
                        }
                    }
                    if (FilterType === "RevenueSummary" || FilterType === "GlobalFilter") {
                        // Ensure newData.RevenueChart is not null or undefined
                        const txnData = newData.RevenueChart;
                        
                        if (Array.isArray(txnData) && txnData.length > 0) {
                            // Map the data for revenue, conversion rate, and periods
                            const revenueData = txnData.map(item => item.TotalRevenue === 0 ? null : item.TotalRevenue);  
                            const conversionRateData = txnData.map(item => item.ConversionRate === 0 ? null : item.ConversionRate); 
                            const categories = txnData.map(item => item.PeriodName);
                            
                            if (window.RevenueComboChart) {
                                window.RevenueComboChart.updateOptions({
                                    xaxis: {
                                        categories: categories 
                                    }
                                });
                                
                                window.RevenueComboChart.updateSeries([
                                    { name: 'Revenue', data: revenueData },
                                    { name: 'Conversion Rate', data: conversionRateData }
                                ], true); 
                            } else {
                                alert("RevenueComboChart is not initialized.");
                            }
                        } else {
                            alert("Invalid txnData: Ensure it is an array with data.");
                        }
                    }

                },
                error: function (xhr, status, error) {
                    console.error("Error occurred: " + error);
                    alert("An error occurred while processing your request.");
                }
            });
        });


        $(".filter-0").on("change", function () {
            var Type = $(this).prop('id');
            var filterType = $("#" + Type).find("option:selected").val();
            $.ajax({
                type: "POST",
                url: "DashBoard.aspx/GetDataByDatePeriod",
                data: JSON.stringify({ Type: Type, filterType: filterType }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === null ||
                        response.d === "" ||
                        (Array.isArray(response.d) && response.d.length === 0)) {
                        alert("No data found");
                    } else {
                        if (Type == "ddlSwitchFilter") {
                            SwitchChart(response.d);
                        } else if (Type == "ddlAggreFilter") {
                            $("#tbAggregator").empty().html(response.d);

                        } else if (Type == "ddlRuleFilter") {
                            $("#tbRule").empty().html(response.d);
                            initializeCharts();

                        } else if (Type == "ddlTopSwitchsFilter") {
                            $("#tbSwitch").empty().html(response.d);
                        }
                        else if (Type == "ddlTopSwitchsFilter") {
                            //$("#tbSwitch").empty().html(response.d);
                        }
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error occurred: " + error);
                    alert("An error occurred while processing your request.");
                }
            });
        });

        var switchCounts;
        var switchNames;
        var switchPercent;

        function SwitchChart(data) {
            data = data.replace(/\\/g, '');
            data = JSON.parse(data);

            switchCounts = data.map(item => item.Counts);
            switchNames = data.map(item => item.SwitchName);
            switchPercent = data.map(item => item.Percent);
            const hasData = switchCounts.some(count => count > 0);
            var ubdData = {
                datasets: [{
                    borderWidth: 2,
                    hoverBorderColor: '#ffffff',
                    data: hasData ? switchPercent : [1],
                    backgroundColor: hasData ? [
                        'rgba(0,123,255,0.9)',
                        'rgba(0,123,255,0.5)',
                        'rgba(0,123,255,0.3)',
                        'rgba(0,123,255,0.7)'
                    ] : ['rgba(144,238,144,0.5)']
                }],
                labels: hasData ? switchNames : ['No Data']
            };

            var ubdOptions = {
                responsive: true,
                layout: {
                    padding: {
                        bottom: 20
                    }
                },
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            padding: 25,
                            boxWidth: 20,
                            generateLabels: function (chart) {
                                const data = chart.data;
                                return data.labels.map((label, index) => {
                                    const percent = switchPercent[index];
                                    return {
                                        text: `${percent}% ${label}`,
                                        fillStyle: data.datasets[0].backgroundColor[index],
                                        hidden: false,
                                        index: index
                                    };
                                });
                            }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function (tooltipItem) {
                                const percent = switchPercent[tooltipItem.index];
                                return `(${percent}%)`;
                            }
                        }
                    }
                },
                cutoutPercentage: 65,
                tooltips: {
                    enabled: true,
                    mode: 'index',
                    position: 'nearest',
                    callbacks: {
                        label: function (tooltipItem, data) {
                            const percent = switchPercent[tooltipItem.index];
                            return `(${percent}%)`;
                        }
                    }
                }
            };

            var ubdCtx = document.getElementsByClassName('blog-users-by-device')[0];

            if (window.ubdChart) {
                window.ubdChart.destroy();
            }

            window.ubdChart = new Chart(ubdCtx, {
                type: 'doughnut',
                data: ubdData,
                options: ubdOptions
            });
        }



        //Chart initializer Functions Rule
        function initializeCharts() {
            $('#tbRule canvas').each(function () {
                const ctx = this.getContext('2d');
                const id = this.id;
                const dataCounts = retrieveDataCounts(id);

                const backgroundColors = dataCounts.map((count, index) => {
                    return index === 0 ? 'rgba(54, 162, 235, 0.8)' : 'rgba(255, 99, 132, 0.8)';
                });

                const data = {
                    datasets: [{
                        label: id.split('_').slice(0, -1).join('_'),
                        data: dataCounts,
                        backgroundColor: backgroundColors,
                        borderWidth: 1
                    }]
                };

                new Chart(ctx, {
                    type: 'doughnut',
                    data: data,
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        cutoutPercentage: 50,
                        plugins: {
                            tooltip: {
                                enabled: true
                            },
                            legend: {
                                display: true
                            }
                        }
                    }
                });
            });
        }

        function retrieveDataCounts(id) {
            const idParts = id.split('_');
            const groupName = idParts[0];
            const ruleName = idParts[1];
            const row = $(`tr td canvas#${id}`).closest('tr');
            const successCount = parseInt(row.find('input[name="Success"]').val()) || 0;
            const failureCount = parseInt(row.find('input[name="Fail"]').val()) || 0;
            return [successCount, failureCount];
        }
        //end

    </script>

    <%-- //scripts for modal bind all chart details--%>
    <script>
        $(document).ready(function () {
            //Switch Modal Data
            $('#VewSwitchOveriewRpt').on('click', function () {
                if (window.modalChart) {
                    window.modalChart.destroy(); // Destroy any previous chart
                }

                $('#switchOverviewModal').on('shown.bs.modal', function () {
                    var ctx = document.getElementById('modalGraphCanvas').getContext('2d');
                    var ubdData = {
                        datasets: [{
                            hoverBorderColor: '#ffffff',
                            data: switchCounts, // Using global `switchCounts`
                            backgroundColor: ['rgba(0,123,255,0.9)', 'rgba(0,123,255,0.5)', 'rgba(0,123,255,0.3)', 'rgba(0,123,255,0.7)']
                        }],
                        labels: switchNames // Using global `switchNames`
                    };

                    var ubdOptions = {
                        legend: { position: 'bottom', labels: { padding: 25, boxWidth: 20 } },
                        cutoutPercentage: 75,
                        tooltips: { custom: false, mode: 'index', position: 'nearest' }
                    };

                    window.modalChart = new Chart(ctx, {
                        type: 'doughnut',
                        data: ubdData,
                        options: ubdOptions
                    });

                    var selectedFilter = $('#ddlSwitchFilter').val();
                    var detailsText = "Showing data for: " + getFilterText(selectedFilter);
                    $('#modalDetails').text(detailsText);

                    resizeCanvas();
                });

                $('#switchOverviewModal').modal('show');
            });

            // Helper function to get filter text based on selected option
            function getFilterText(filterValue) {
                switch (filterValue) {
                    case 'today': return 'Today';
                    case 'last_week': return 'Last Week';
                    case 'last_month': return 'Last Month';
                    case 'last_year': return 'Last Year';
                    default: return 'Custom Period';
                }
            }

            // **Excel Download with Data and Chart Image**
            $('#downloadExcelBtn').on('click', function (event) {
                event.preventDefault(); // Prevent the default action (form submission or page reload)

                // Prepare data for Excel
                var ws_data = [['Switch Name', 'Switch Count']];
                for (let i = 0; i < switchNames.length; i++) {
                    ws_data.push([switchNames[i], switchCounts[i]]);
                }

                // Create the worksheet with the data
                var ws = XLSX.utils.aoa_to_sheet(ws_data);

                // Get the chart image as a base64 string
                var chartImage = window.modalChart.toBase64Image();

                // Add the image to the Excel sheet
                if (!ws['!images']) ws['!images'] = []; // Initialize images array if not already done
                ws['!images'].push({
                    t: 'z', // Image type 'z' for base64 images
                    h: 100, // Height of the image (adjust as needed)
                    w: 200, // Width of the image (adjust as needed)
                    x: 0,   // X position in the sheet (leftmost cell)
                    y: ws_data.length + 2, // Y position in the sheet (place the image after the data rows)
                    image: chartImage // Base64 chart image data
                });

                // Create a new workbook and append the worksheet with the image
                var wb = XLSX.utils.book_new();
                XLSX.utils.book_append_sheet(wb, ws, 'Switch Overview');

                // Write the Excel file with data and chart image
                XLSX.writeFile(wb, 'switch_overview_report_with_chart.xlsx');
            });

            // Resize the canvas based on the modal's width/height
            function resizeCanvas() {
                var modalCanvas = document.getElementById('modalGraphCanvas');
                modalCanvas.width = modalCanvas.offsetWidth;
                modalCanvas.height = modalCanvas.offsetHeight;
            }
        });


        //Common Modal for Report Agg|Rule|Switch
        $('#reportModal').on('show.bs.modal', function (e) {
            var button = $(e.relatedTarget); // Button that triggered the modal
            var reportType = button.data('report-type'); // Get the report type (aggregator, rule, switch)

            // Clear previous content and set default title
            $('#modalReportContent').empty();
            $('#reportModalLabel').text('Full Report'); // Default title

            // Based on report type, set the title and get corresponding content from Literal controls
            switch (reportType) {
                case 'aggregator':
                    // Set title for Aggregator
                    $('#reportModalLabel').text('Aggregator Report');
                    // Get content from the Aggregator Literal
                    var aggregatorContent = $('#tbAggregator').html();
                    $('#modalReportContent').html(aggregatorContent);
                    break;
                case 'rule':
                    // Set title for Rule Performance Report
                    $('#reportModalLabel').text('Rule Performance Report');
                    // Get content from the Rule Literal
                    var ruleContent = $('#tbRule').html();
                    $('#modalReportContent').html(ruleContent);
                    break;
                case 'switch':
                    // Set title for Switch Performance Report
                    $('#reportModalLabel').text('Switch Performance Report');
                    // Get content from the Switch Literal
                    var switchContent = $('#tbSwitch').html();
                    $('#modalReportContent').html(switchContent);
                    break;
                default:
                    $('#modalReportContent').html('<p>No report available.</p>');
                    break;
            }

        });

    </script>

    <style>
        #aggregatorCarousel {
            position: relative;
        }

        .carousel-control-prev,
        .carousel-control-next {
            position: absolute;
            top: 10%; /* Centering vertically */
            width: 30px;
            height: 30px;
            background: rgba(0, 123, 255, 0.8);
            border-radius: 50%;
            display: flex;
            justify-content: center;
            align-items: center;
            border: 2px solid #007bff;
            transform: translateY(-50%);
            font-size: 16px;
            color: #fff;
            font-weight: bold;
            transition: background-color 0.3s, transform 0.3s, box-shadow 0.3s ease;
            box-shadow: 0 2px 8px rgba(0, 123, 255, 0.3);
        }

        /* Left (previous) button Hidden */
        .carousel-control-prev {
            left: 15px;
            display: none;
        }

        /* Right (next) button */
        .carousel-control-next {
            right: 15px;
        }

            /* Hover effect */
            .carousel-control-prev:hover,
            .carousel-control-next:hover {
                background: #0062cc;
                transform: scale(1.1);
                box-shadow: 0 4px 12px rgba(0, 123, 255, 0.5);
            }

            /* Optional: Accessibility for screen readers */
            .carousel-control-prev:focus,
            .carousel-control-next:focus {
                outline: none;
                box-shadow: 0 0 0 4px rgba(0, 123, 255, 0.5);
            }

        .carousel-control-label {
            display: none;
        }

        /* Accessibility */
        .sr-only {
            display: inline;
        }

        /* Centering arrow icon */
        .carousel-control-prev i,
        .carousel-control-next i {
            display: flex;
            justify-content: center;
            align-items: center;
            font-size: 18px;
            line-height: 0;
        }
    </style>
</asp:Content>
