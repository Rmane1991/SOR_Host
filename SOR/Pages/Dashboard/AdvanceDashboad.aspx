<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SOR.Master" CodeBehind="AdvanceDashboad.aspx.cs" Inherits="SOR.Pages.Dashboard.AdvanceDashboad" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
    <link rel="stylesheet" id="main-stylesheet" data-version="1.1.0" href="../../DashStyles/shards-dashboards.1.1.0.min.css">
    <link rel="stylesheet" href="../../DashStyles/extras.1.1.0.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
    <!--for chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@3.0.0"></script>

    <!--for ApexChart -->
    <%--<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>--%>
    <script src="https://cdn.jsdelivr.net/npm/apexcharts@3.34.0"></script>
    <style>
        .go-stats__label {
            margin: 0;
            padding: 0;
            font-size: .95rem;
            font-weight: 500;
        }

        .Scroller {
            max-height: 300px;
            overflow-y: auto;
            scroll-behavior: smooth;
        }
    </style>

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

        <!-- Card Header -->
        <div class="row" hidden="hidden">
            <div class="col-lg col-md-6 col-sm-6 mb-4">
                <div class="stats-small stats-small--1 card card-small">
                    <div class="card-body p-0 d-flex">
                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                            </div>
                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                            </div>
                        </div>
                        <div class="d-flex flex-column m-auto">
                            <div class="stats-small__data text-center">
                                <span class="stats-small__label text-uppercase">Customer</span>
                                <h6 class="stats-small__value count my-3">2,390</h6>
                            </div>
                            <div class="stats-small__data">
                                <span class="stats-small__percentage stats-small__percentage--increase">4.7%</span>
                            </div>
                        </div>
                        <canvas height="92" class="blog-overview-stats-small-1 chartjs-render-monitor" width="230" style="display: block; width: 230px; height: 92px;"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg col-md-6 col-sm-6 mb-4">
                <div class="stats-small stats-small--1 card card-small">
                    <div class="card-body p-0 d-flex">
                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                            </div>
                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                            </div>
                        </div>
                        <div class="d-flex flex-column m-auto">
                            <div class="stats-small__data text-center">
                                <span class="stats-small__label text-uppercase">Pages</span>
                                <h6 class="stats-small__value count my-3">182</h6>
                            </div>
                            <div class="stats-small__data">
                                <span class="stats-small__percentage stats-small__percentage--increase">12.4%</span>
                            </div>
                        </div>
                        <canvas height="92" class="blog-overview-stats-small-2 chartjs-render-monitor" width="230" style="display: block; width: 230px; height: 92px;"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg col-md-4 col-sm-6 mb-4">
                <div class="stats-small stats-small--1 card card-small">
                    <div class="card-body p-0 d-flex">
                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                            </div>
                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                            </div>
                        </div>
                        <div class="d-flex flex-column m-auto">
                            <div class="stats-small__data text-center">
                                <span class="stats-small__label text-uppercase">Comments</span>
                                <h6 class="stats-small__value count my-3">8,147</h6>
                            </div>
                            <div class="stats-small__data">
                                <span class="stats-small__percentage stats-small__percentage--decrease">3.8%</span>
                            </div>
                        </div>
                        <canvas height="92" class="blog-overview-stats-small-3 chartjs-render-monitor" width="230" style="display: block; width: 230px; height: 92px;"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg col-md-4 col-sm-6 mb-4">
                <div class="stats-small stats-small--1 card card-small">
                    <div class="card-body p-0 d-flex">
                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                            </div>
                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                            </div>
                        </div>
                        <div class="d-flex flex-column m-auto">
                            <div class="stats-small__data text-center">
                                <span class="stats-small__label text-uppercase">Users</span>
                                <h6 class="stats-small__value count my-3">2,413</h6>
                            </div>
                            <div class="stats-small__data">
                                <span class="stats-small__percentage stats-small__percentage--increase">12.4%</span>
                            </div>
                        </div>
                        <canvas height="92" class="blog-overview-stats-small-4 chartjs-render-monitor" width="230" style="display: block; width: 230px; height: 92px;"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg col-md-4 col-sm-12 mb-4">
                <div class="stats-small stats-small--1 card card-small">
                    <div class="card-body p-0 d-flex">
                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                            </div>
                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                            </div>
                        </div>
                        <div class="d-flex flex-column m-auto">
                            <div class="stats-small__data text-center">
                                <span class="stats-small__label text-uppercase">Subscribers</span>
                                <h6 class="stats-small__value count my-3">17,281</h6>
                            </div>
                            <div class="stats-small__data">
                                <span class="stats-small__percentage stats-small__percentage--decrease">2.4%</span>
                            </div>
                        </div>
                        <canvas height="92" class="blog-overview-stats-small-5 chartjs-render-monitor" width="230" style="display: block; width: 230px; height: 92px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <!-- End Card Header -->

        <!-- Body Connent -->
        <div class="row">
            <!-- Monthly-Transaction Stats -->
            <div class="col-lg-12 col-md-12 col-sm-12 mb-4">
                <div class="card card-small">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">Channel wise Monthly Transaction</h6>
                    </div>
                    <div class="card-body pt-0">
                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                            </div>
                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                            </div>
                        </div>
                        <div class="row border-bottom py-2 bg-light">
                            <div class="col d-flex mb-2 mb-sm-0 col-sm-9">
                                <div class="btn-group" data-attribute="ChannelWiseTxn">
                                    <%--<button type="button" class="btn btn-white active">Hour</button>
                                    <button type="button" class="btn btn-white">Day</button>--%>
                                    <button type="button" class="btn btn-white active" data-attribute="ChannelWiseTxn" data-text="Week">Week</button>
                                    <button type="button" class="btn btn-white" data-attribute="ChannelWiseTxn" data-text="Month">Month</button>
                                </div>
                            </div>

                            <div class="col-12 col-sm-3">
                                <div id="blog-overview-date-range" class="input-daterange input-group input-group-sm my-auto ml-auto mr-auto ml-sm-auto mr-sm-0" style="max-width: 350px;">
                                    <input type="text" class="input-sm form-control" name="start" placeholder="Start Date" id="blog-overview-date-range-1">
                                    <input type="text" class="input-sm form-control" name="end" placeholder="End Date" id="blog-overview-date-range-2">
                                    <span class="input-group-append">
                                        <span class="input-group-text">
                                            <i class="material-icons"></i>
                                        </span>
                                    </span>
                                </div>
                            </div>
                            <%-- <div class="col-12 col-sm-6 d-flex mb-2 mb-sm-0">
                                <button type="button" class="btn btn-sm btn-white ml-auto mr-auto ml-sm-auto mr-sm-0 mt-3 mt-sm-0">View Full Report →</button>
                            </div>--%>
                        </div>
                        <canvas height="348" style="max-width: 100% !important; display: block; width: 805px; height: 348px;" class="blog-overview-users chartjs-render-monitor" width="805"></canvas>
                    </div>
                </div>
            </div>
            <!-- End Monthly-Transaction Stats -->

            <!-- Users By Device Stats -->
            <div class="col-lg-4 col-md-6 col-sm-12 mb-4" hidden="hidden">
                <div class="card card-small h-100">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">Users by device</h6>
                    </div>
                    <div class="card-body d-flex py-0">
                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                            </div>
                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                            </div>
                        </div>
                        <canvas height="272" class="blog-users-by-device m-auto chartjs-render-monitor" width="372" style="display: block; width: 372px; height: 272px;"></canvas>
                    </div>
                    <div class="card-footer border-top">
                        <div class="row">
                            <div class="col">
                                <select class="custom-select custom-select-sm" style="max-width: 130px;">
                                    <option selected="">Last Week</option>
                                    <option value="1">Today</option>
                                    <option value="2">Last Month</option>
                                    <option value="3">Last Year</option>
                                </select>
                            </div>
                            <div class="col text-right view-report">
                                <a href="#">Full report →</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- End Users By Device Stats -->

            <!-- Agent-Wise Onboard Trend Stats -->
            <div class="col-lg-6 col-md-12 col-sm-12 mb-4">
                <div class="card card-small h-100 blog-comments">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">Agent Onborad Trend</h6>
                    </div>
                    <div class="card-body pt-0">
                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                            </div>
                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                            </div>
                        </div>
                        <div class="row border-bottom py-2 bg-light">
                            <div class="col d-flex mb-2 mb-sm-0 col-sm-6">
                                <div class="btn-group" data-attribute="AgentOnbordingData">
                                    <%--<button type="button" class="btn btn-white active">Hour</button>
                                    <button type="button" class="btn btn-white">Day</button>--%>
                                    <button type="button" class="btn btn-white active" data-attribute="AgentOnbordingData" data-text="Week">Week</button>
                                    <button type="button" class="btn btn-white" data-attribute="AgentOnbordingData" data-text="Month">Month</button>
                                </div>
                            </div>

                            <div class="col-12 col-sm-6">
                                <div id="Agent-daterange" class="input-daterange input-group input-group-sm my-auto ml-auto mr-auto ml-sm-auto mr-sm-0" style="max-width: 350px;">
                                    <input type="text" class="input-sm form-control" name="start" placeholder="Start Date" id="Agent-range-1">
                                    <input type="text" class="input-sm form-control" name="end" placeholder="End Date" id="Agent-range-2">
                                    <span class="input-group-append">
                                        <span class="input-group-text">
                                            <i class="material-icons"></i>
                                        </span>
                                    </span>
                                </div>
                            </div>
                            <%-- <div class="col-12 col-sm-6 d-flex mb-2 mb-sm-0">
                                <button type="button" class="btn btn-sm btn-white ml-auto mr-auto ml-sm-auto mr-sm-0 mt-3 mt-sm-0">View Full Report →</button>
                            </div>--%>
                        </div>
                        <canvas height="348" style="max-width: 100% !important; display: block; width: 805px; height: 348px;" class="AgentOnboardTrend chartjs-render-monitor" width="805"></canvas>
                    </div>
                </div>
            </div>
            <!-- End Agent-Wise Onboard Stats -->

            <!-- Rule-Wise Stats -->
            <div class="col-lg-6 col-md-12 col-sm-12 mb-4">
                <div class="card card-small h-100 blog-comments">
                    <div class="card-header border-bottom">
                        <h6 class="m-0"><i class="fas fa-chart-bar"></i>Top 5 Rule Wise Performance Overview</h6>
                    </div>
                    <div class="card-body p-0 Scroller" style="max-height: 300px; overflow-y: auto;">
                        <asp:Literal ID="Rule" runat="server"></asp:Literal>
                    </div>
                    <div class="card-footer border-top">
                        <div class="row">
                            <div class="col text-center view-report">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- End Rule-Wise Stats -->


            <!-- Agent-Wise Transaction Stats -->
            <div class="col-lg-12 col-md-12 col-sm-12 mb-4">
                <div class="card card-small">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">Agent-wise Monthly Transaction View</h6>
                    </div>
                    <div class="card-body pt-0">
                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                            </div>
                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                            </div>
                        </div>
                        <div class="row border-bottom py-2 bg-light">
                            <div class="col d-flex mb-2 mb-sm-0 col-sm-9">
                                <div class="btn-group" data-attribute="AgentTxnData">
                                    <%--<button type="button" class="btn btn-white active">Hour</button>
                                    <button type="button" class="btn btn-white">Day</button>--%>
                                    <button type="button" class="btn btn-white active" data-attribute="AgentTxnData" data-text="Week">Week</button>
                                    <button type="button" class="btn btn-white" data-attribute="AgentTxnData" data-text="Month">Month</button>
                                </div>
                            </div>

                            <div class="col-12 col-sm-3">
                                <div id="Aggregator-daterange" class="input-daterange input-group input-group-sm my-auto ml-auto mr-auto ml-sm-auto mr-sm-0" style="max-width: 350px;">
                                    <input type="text" class="input-sm form-control" name="start" placeholder="Start Date" id="Aggregatordate-range-1">
                                    <input type="text" class="input-sm form-control" name="end" placeholder="End Date" id="Aggregatordate-range-2">
                                    <span class="input-group-append">
                                        <span class="input-group-text">
                                            <i class="material-icons"></i>
                                        </span>
                                    </span>
                                </div>
                            </div>
                            <%-- <div class="col-12 col-sm-6 d-flex mb-2 mb-sm-0">
                                <button type="button" class="btn btn-sm btn-white ml-auto mr-auto ml-sm-auto mr-sm-0 mt-3 mt-sm-0">View Full Report →</button>
                            </div>--%>
                        </div>
                        <canvas height="348" style="max-width: 100% !important; display: block; width: 805px; height: 348px;" class="AggregatorView chartjs-render-monitor" width="805"></canvas>
                    </div>
                </div>
            </div>
            <!-- End Agnet-Wise Stats -->
        </div>
        <!-- End Body Connent -->

    </div>

    <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.min.js"></script>
    <script src="https://unpkg.com/shards-ui@latest/dist/js/shards.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Sharrre/2.0.1/jquery.sharrre.min.js"></script>
    <script src="../../AdvanceDashScripts/extras.1.1.0.min.js"></script>
    <script src="../../AdvanceDashScripts/shards-dashboards.1.1.0.min.js"></script>
    <script src="../../AdvanceDashScripts/app/AdvnaceDash.1.1.0.js"></script>
    <input type="hidden" id="hiddenChannelData" value='<%= HttpUtility.JavaScriptStringEncode(ViewState["ChannelData"]?.ToString() ?? "") %>' />
    <input type="hidden" id="hiddenAgentOnboradingData" value='<%= HttpUtility.JavaScriptStringEncode(ViewState["AgentOnboardingData"]?.ToString() ?? "") %>' />

    <script>
        function generateDonutChart(canvasId, successPercentage, failurePercentage) {
            var ctx = document.getElementById(canvasId).getContext('2d');
            var chart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    datasets: [{
                        data: [successPercentage, failurePercentage],
                        backgroundColor: ['#007bff', '#f8d7da'],
                        borderWidth: 0
                    }]
                },
                options: {
                    cutoutPercentage: 70,
                    responsive: true,
                    plugins: {
                        legend: {
                            display: true
                        },
                        tooltip: {
                            enabled: true,
                            callbacks: {
                                label: function (tooltipItem) {
                                    var value = tooltipItem.raw;
                                    var label = tooltipItem.index === 0 ? 'Success' : 'Failure';
                                    return label + ': ' + value.toFixed(2) + '%';  // Display percentage
                                }
                            }
                        }
                    }
                }
            });
        }
    </script>


</asp:Content>
