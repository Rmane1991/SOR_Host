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
    <link href="../../css/DashboradLoader.css" rel="stylesheet" />

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
                    <input type="text" id="startDate" placeholder="Start Date" class="text-center form-control form-control-sm DateRangeCustom" value="">
                    <input type="text" id="endDate" placeholder="End Date" class="text-center form-control form-control-sm DateRangeCustom" value="">
                    <span class="input-group-append">
                        <button type="button" class="btn btn-primary GlobalSearch" data-attribute="GlobalFilter" data-text="DateRange">Search &rarr;</button>
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
            <!-- Channel Transaction Stats -->
            <div class="col-lg-12 col-md-12 col-sm-12 mb-4">
                <div class="card card-small">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">AePS Channel Transaction Overview</h6>
                    </div>
                    <div class="card-body pt-0">
                        <!-- Loader inside card-body (specific to this section) -->
                        <div class="loading-wrapper-ChannelTxnSummary" style="display: none;">
                            <div class="loading-text">
                                <img src="../../images/DashbordLoder.gif" alt="Loading...">
                            </div>
                        </div>
                        <!-- Content to be displayed after data is loaded -->
                        <div class="content">
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
                                        <input type="text" class="input-sm form-control DateRangeCustom" name="start" placeholder="Start Date" id="ChanneldateRange1">
                                        <input type="text" class="input-sm form-control DateRangeCustom" name="end" placeholder="End Date" id="ChanneldateRange2">
                                        <span class="input-group-append">
                                            <button type="button" class="btn btn-primary ChannelWiseSearch" data-attribute="ChannelWiseTxn" data-text="DateRange">Search &rarr;</button>
                                        </span>
                                    </div>
                                </div>
                                <%-- <div class="col-12 col-sm-6 d-flex mb-2 mb-sm-0">
                                <button type="button" class="btn btn-sm btn-white ml-auto mr-auto ml-sm-auto mr-sm-0 mt-3 mt-sm-0">View Full Report →</button>
                            </div>--%>
                            </div>
                            <div class="ChannelTxnSummary"></div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- End Channel-Transaction Stats -->


            <!-- Agent Onboard Trend Stats -->
            <div class="col-lg-6 col-md-12 col-sm-12 mb-4">
                <div class="card card-small h-100 blog-comments">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">Agent Onborad Trend</h6>
                    </div>
                    <div class="card-body pt-0">
                        <!-- Loader inside card-body (specific to this section) -->
                        <div class="loading-wrapper-AgentOnboradTrend" style="display: none;">
                            <div class="loading-text">
                                <img src="../../images/DashbordLoder.gif" alt="Loading...">
                            </div>
                        </div>
                        <!-- Content to be displayed after data is loaded -->
                        <div class="content">
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
                                        <input type="text" class="input-sm form-control DateRangeCustom" name="start" placeholder="Start Date" id="AgentDateRange1">
                                        <input type="text" class="input-sm form-control DateRangeCustom" name="end" placeholder="End Date" id="AgentDateRange2">
                                        <span class="input-group-append">
                                            <button type="button" class="btn btn-primary AgentOnboardSearch" data-attribute="AgentOnbordingData" data-text="DateRange">Search &rarr;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <canvas height="348" style="max-width: 100% !important; display: block; width: 805px; height: 348px;" class="AgentOnboardTrend chartjs-render-monitor" width="805"></canvas>
                        </div>
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
                        <h6 class="m-0">Agent Transaction OverView</h6>
                    </div>
                    <div class="card-body pt-0">
                        <div class="loading-wrapper-AgentTransactionSummary" style="display: none;">
                            <div class="loading-text">
                                <img src="../../images/DashbordLoder.gif" alt="Loading...">
                            </div>
                        </div>
                        <!-- Content to be displayed after data is loaded -->
                        <div class="content">
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
                                    <button type="button" class="btn btn-white" data-attribute="AgentTxnData">Day</button>--%>
                                        <button type="button" class="btn btn-white active" data-attribute="AgentTxnData" data-text="Week">Week</button>
                                        <button type="button" class="btn btn-white" data-attribute="AgentTxnData" data-text="Month">Month</button>
                                    </div>
                                </div>

                                <div class="col-12 col-sm-3">
                                    <div id="Agent" class="input-daterange input-group input-group-sm my-auto ml-auto mr-auto ml-sm-auto mr-sm-0" style="max-width: 350px;">
                                        <input type="text" class="input-sm form-control DateRangeCustom" name="start" placeholder="Start Date" id="AgentTxnDate1">
                                        <input type="text" class="input-sm form-control DateRangeCustom" name="end" placeholder="End Date" id="AgentTxnDate2">
                                        <span class="input-group-append">
                                            <button type="button" class="btn btn-primary AgentTxnSearch" data-attribute="AgentTxnData" data-text="DateRange">Search &rarr;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <canvas height="348" style="max-width: 100% !important; display: block; width: 805px; height: 348px;" class="AgentTxnSummary chartjs-render-monitor" width="805"></canvas>
                            <%--<div class="AgentTxnSummary"></div>--%>
                        </div>
                    </div>
                </div>
            </div>
            <!-- End Agnet-Wise Stats -->


            <!-- UnqCustomer Transaction Stats -->
            <div class="col-lg-12 col-md-12 col-sm-12 mb-4">
                <div class="card card-small">
                    <div class="card-header border-bottom">
                        <h6 class="m-0">Unique Customer Trend</h6>
                    </div>
                    <div class="card-body pt-0">
                        <!-- Loader inside card-body (specific to this section) -->
                        <div class="loading-wrapper-UniqueCustomer" style="display: none;">
                            <div class="loading-text">
                                <img src="../../images/DashbordLoder.gif" alt="Loading...">
                            </div>
                        </div>
                        <!-- Content to be displayed after data is loaded -->
                        <div class="content">
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
                                    <div class="btn-group" data-attribute="UniqueCustomer">
                                        <%--<button type="button" class="btn btn-white active">Hour</button>--%>
                                        <button type="button" class="btn btn-white active" data-attribute="UniqueCustomer" data-text="Day">Day</button>
                                        <button type="button" class="btn btn-white" data-attribute="UniqueCustomer" data-text="Week">Week</button>
                                        <button type="button" class="btn btn-white" data-attribute="UniqueCustomer" data-text="Month">Month</button>
                                    </div>
                                </div>

                                <div class="col-12 col-sm-3">
                                    <div id="UniqueCustomer-daterange" class="input-daterange input-group input-group-sm my-auto ml-auto mr-auto ml-sm-auto mr-sm-0" style="max-width: 350px;">
                                        <input type="text" class="input-sm form-control DateRangeCustom" name="start" placeholder="Start Date" id="UniqueCustomerd1">
                                        <input type="text" class="input-sm form-control DateRangeCustom" name="end" placeholder="End Date" id="UniqueCustomerd2">
                                        <span class="input-group-append">
                                            <button type="button" class="btn btn-primary UnQustomersearch" data-attribute="UniqueCustomer" data-text="DateRange">Search &rarr;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="UniqueCustomer"></div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- End UnqCustomer Stats -->
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
    <input type="hidden" id="hiddenAgentTxnData" value='<%= HttpUtility.JavaScriptStringEncode(ViewState["AgentTransactionData"]?.ToString() ?? "") %>' />
    <input type="hidden" id="hiddenUnqCustomerData" value='<%= HttpUtility.JavaScriptStringEncode(ViewState["CustomerUnqData"]?.ToString() ?? "") %>' />


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
                                    return label + ': ' + value.toFixed(2) + '%';
                                }
                            }
                        }
                    }
                }
            });
        }
    </script>

    <script>
        $('.GlobalSearch, .ChannelWiseSearch, .btn-group button, .AgentOnboardSearch, .AgentTxnSearch, .UnQustomersearch').click(function () {

            var Filter = $(this).data('text');
            var FilterType = $(this).data('attribute');
            var id = $(this).prop('id');
            var fromDate;
            var toDate;

            if ($(this).hasClass('GlobalSearch')) {
                fromDate = $('#startDate').val();
                toDate = $('#endDate').val();
                FilterType = "GlobalSearch";
                Filter = "DateRange";
            }

            if (FilterType == "ChannelWiseTxn") {
                $(".loading-wrapper-ChannelTxnSummary").css('display', 'block');
                fromDate = $('#ChanneldateRange1').val();
                toDate = $('#ChanneldateRange2').val();
            } else if (FilterType == "AgentOnbordingData") {
                $(".loading-wrapper-AgentOnboradTrend").css('display', 'block');
                fromDate = $('#AgentDateRange1').val();
                toDate = $('#AgentDateRange2').val();
            }
            else if (FilterType == "AgentTxnData") {
                $(".loading-wrapper-AgentTransactionSummary").css('display', 'block');
                fromDate = $('#AgentTxnDate1').val();
                toDate = $('#AgentTxnDate2').val();
            }
            else if (FilterType == "UniqueCustomer") {
                $(".loading-wrapper-UniqueCustomer").css('display', 'block');
                fromDate = $('#UniqueCustomerd1').val();
                toDate = $('#UniqueCustomerd2').val();
            }
            else {

            }

            $.ajax({
                type: "POST",
                url: "AdvanceDashboad.aspx/ChartdataFilter",
                data: JSON.stringify({ fromDate: fromDate, toDate: toDate, FilterType: FilterType, Filter: Filter }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    const newData = JSON.parse(response.d);

                    if (FilterType === "ChannelWiseTxn" || FilterType === "GlobalSearch") {
                        const Records = newData.ChannelTxnData;
                        var timeperiod = Records.map(t => t.timeperiod);
                        var channel = Records.map(t => t.channel);
                        var successcount = Records.map(t => t.successcount);
                        var failurecount = Records.map(t => t.failurecount);

                        BlogOverviewUsers.data.labels = timeperiod;
                        BlogOverviewUsers.data.datasets[0].data = successcount;
                        BlogOverviewUsers.data.datasets[1].data = failurecount;
                        BlogOverviewUsers.data.datasets[0].label = 'Success';
                        BlogOverviewUsers.data.datasets[1].label = 'Failure';
                        BlogOverviewUsers.update();
                    }
                    if (FilterType === "AgentOnbordingData" || FilterType === "GlobalSearch") {
                        const Records = newData.AgentOnboard;
                        var timeperiod = Records.map(t => t.timeperiod);
                        var Current = Records.map(t => t.currentcount);
                        var Previous = Records.map(t => t.previouscount);

                        BlogOverviewUsers.data.labels = timeperiod;
                        BlogOverviewUsers.data.datasets[0].data = Current;
                        BlogOverviewUsers.data.datasets[1].data = Previous;
                        // BlogOverviewUsers.data.datasets[0].backgroundColor = 'newColor';
                        // BlogOverviewUsers.data.datasets[1].borderColor = 'newBorderColor';
                        BlogOverviewUsers.update();
                    }

                    if (FilterType === "AgentTxnData" || FilterType === "GlobalSearch") {
                        const Records = newData.AgentTxnSummaryData;
                        var timeperiod = Records.map(t => t.timeperiod);
                        var Current = Records.map(t => t.currentcount);
                        var Previous = Records.map(t => t.previouscount);

                        BlogOverviewUsers.data.labels = timeperiod;
                        BlogOverviewUsers.data.datasets[0].data = Current;
                        BlogOverviewUsers.data.datasets[1].data = Previous;
                        BlogOverviewUsers.update();
                    }
                   
                    if (FilterType === "UniqueCustomer" || FilterType === "GlobalSearch") {
                        const Records = newData.UnQCustomerData;
                        console.log(Records);

                        var TimePeriod = Records.map(t => t.CurrentPeriod);
                        var currentData = Records.map(t => t.CurrentCount);
                        var PreviousTime = Records.map(t => t.PreviousPeriod);
                        var previousData = Records.map(t => t.PreviousCount);

                        if (window.UniqueCustomerChart) {
                            window.UniqueCustomerChart.updateOptions({
                                xaxis: { categories: TimePeriod }
                            });

                            window.UniqueCustomerChart.updateSeries([
                                { name: 'Current Data', type: 'line', data: currentData },
                                { name: 'Previous Data', type: 'line', data: previousData }
                            ], true); 
                        } else {
                            alert("Customer Chart is not initialized.");
                        }
                    }
                  
                    $("[class^='loading-wrapper']").css('display', 'none');

                },
                error: function (xhr, status, error) {
                    $("[class^='loading-wrapper']").css('display', 'none');
                    alert("An error occurred while processing your request.");
                }
            });
        });
    </script>


</asp:Content>
