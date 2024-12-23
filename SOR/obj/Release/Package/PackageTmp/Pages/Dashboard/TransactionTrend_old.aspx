<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="TransactionTrend_old.aspx.cs" Inherits="SOR.Pages.Dashboard.TransactionTrend_old" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../../charts/highcharts.js"></script>
    <script src="../../charts/highcharts-3d.js"></script>
    <script src="../../charts/serieslabel.js"></script>
    <script src="../../charts/exporting.js"></script>
    <script src="../../charts/export-data.js"></script>

    <script src="../../NewStyle/script.js"></script>
    <link href="../../NewStyle/styles.css" rel="stylesheet" />
    <%--<link href="../../NewStyle/styles.css" rel="stylesheet" />--%>
    <style>
        /*.mb-2 {
            font-weight: bold;
        }

        .StatusPanel {
            font-weight: bold;
        }*/

        .customeTheme {
            background-color: none;
        }
        
        .formcontrol {
            width: 200px;
            height: 25px;
            margin: 5px;
        }
    </style>

    <style>
        .nowrap {
            white-space: nowrap;
        }

        /*.fweight {
            font-weight: 500;
        }

        .filterdata {
            color: green;
            font-size: 12px;
            margin-left: -15px;
        }*/

        .col-md-2 {
            width: 19.66666667%;
        }

        .gv-responsive {
            height: 400px;
        }
    </style>

    <style>
        .highcharts-figure,
        .highcharts-data-table table {
            min-width: 310px;
            max-width: 800px;
            margin: 1em auto;
        }

        #container {
            height: 400px;
        }

        .highcharts-data-table table {
            font-family: Verdana, sans-serif;
            border-collapse: collapse;
            border: 1px solid #ebebeb;
            margin: 10px auto;
            text-align: center;
            width: 100%;
            max-width: 500px;
        }

        .highcharts-data-table caption {
            padding: 1em 0;
            font-size: 1.2em;
            color: #555;
        }

        .highcharts-data-table th {
            font-weight: 600;
            padding: 0.5em;
        }

        .highcharts-data-table td,
        .highcharts-data-table th,
        .highcharts-data-table caption {
            padding: 0.5em;
        }

        .highcharts-data-table thead tr,
        .highcharts-data-table tr:nth-child(even) {
            background: #f8f8f8;
        }

        .highcharts-data-table tr:hover {
            background: #f1f7ff;
        }

        .col-3 {
            flex: 0 0 auto;
            width: 25%;
            margin-left: 7%;
        }

        .c-dashboardInfo__title {
            font-size: xx-large;
        }
    </style>
    <%--<script src="https://cdn.fusioncharts.com/fusioncharts/latest/fusioncharts.js"></script>
    <script src="https://cdn.fusioncharts.com/fusioncharts/latest/themes/fusioncharts.theme.candy.js"></script>
    <style>
        #chart-container {
            width: 100%;
            height: 400px;
            margin: auto;
        }

        .chart-button {
            padding: 10px 20px;
            background-color: #007BFF;
            color: white;
            border: none;
            cursor: pointer;
            margin: 20px;
        }
    </style>--%>
    <script src="https://cdn.fusioncharts.com/fusioncharts/latest/fusioncharts.js"></script>
    <script src="https://cdn.fusioncharts.com/fusioncharts/latest/themes/fusioncharts.theme.candy.js"></script>

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
    <asp:UpdatePanel ID="upContentBody" runat="server">
        <ContentTemplate>

            <div class="row">
                <%--<div class="col-sm-5 col-xm-12">
                    <div class="selectInputDateBox w-100">
                        <input type="date" runat="server" id="Date" class="multiple-dates select-date form-control" placeholder="Select Date" onchange="fncsave(); checkDate();" />
                    </div>
                </div>--%>
                <div class="row">
                    <div class="col-sm-2 col-xm-12">
                        <label class="selectInputLabel" for="selectInputLabel">From Date</label>
                        <div class="selectInputDateBox w-100">
                            <input type="date" runat="server" id="txtFromDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkDate()" />
                        </div>
                    </div>
                    <div class="col-sm-2 col-xm-12">
                        <label class="selectInputLabel" for="selectInputLabel">To Date</label>
                        <div class="selectInputDateBox w-100">
                            <input type="date" runat="server" id="txtToDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkToDate()" />
                        </div>
                    </div>

                    <div class="col-sm-2 col-xm-12">
                        <button type="button" id="btnSearch" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnSearch_ServerClick">
                            Search</button>
                    </div>
                </div>
                <div class="col-sm-4 col-xm-12">
                    <h2 class="accordion-header hideArrowIcon" id="headingHome">
                        <h2>Transaction Trend</h2>

                    </h2>
                </div>
            </div>

            <div id="wrapper">
                <div class="content-area">
                    <div class="container-fluid">
                        <div class="main">
                            <div class="row sparkboxes mt-4">
                                <div class="col-md-3">
                                    <div class="box box1" style="height: 100px; width: 222px; box-shadow: 10px 15px 24px rgba(0, 0, 0, 0.2);">
                                        <div class="details">
                                            <h3 style="color: white"><b>AEPS</b></h3>
                                            <asp:Label runat="server" ID="lblAEPS" class="c-dashboardInfo__title"></asp:Label></span>
                                       
                                        </div>
                                        <div id="spark1" style="min-height: 80px;">
                                            <div id="apexchartsfeiydgt8l" class="apexcharts-canvas apexchartsfeiydgt8l apexcharts-theme-light" style="width: 100px; height: 35px; margin-left: 115px; margin-top: 11px">
                                                <img src="../../images/AEPS.png" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="box box2" style="height: 100px; width: 222px; box-shadow: 10px 15px 24px rgba(0, 0, 0, 0.2);">
                                        <div class="details">
                                            <h3 style="color: white"><b>MATM</b></h3>
                                            <asp:Label runat="server" ID="lblMATM" class="c-dashboardInfo__title"></asp:Label>
                                        </div>
                                        <div id="spark2" style="min-height: 80px;">
                                            <div id="apexchartsmj5y415zi" class="apexcharts-canvas apexchartsmj5y415zi apexcharts-theme-light" style="width: 40px; height: 41px; margin-left: 120px; margin-top: 10px;">
                                                <img src="../../images/MATM.png" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="box box3" style="height: 100px; width: 222px; box-shadow: 10px 15px 24px rgba(0, 0, 0, 0.2);">
                                        <div class="details">
                                            <h3 style="color: white"><b>TOTAL</b></h3>
                                            <asp:Label runat="server" ID="lblTotal" class="c-dashboardInfo__title"></asp:Label>
                                        </div>
                                        <div id="spark3" style="min-height: 80px;">
                                            <div id="apexchartsmj5y415z" class="apexcharts-canvas apexchartsmj5y415zi apexcharts-theme-light" style="width: 40px; height: 41px; margin-left: 112px; margin-top: 10px;">
                                                <img src="../../images/Total.png" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="box box4" style="height: 100px; width: 222px; box-shadow: 10px 15px 24px rgba(0, 0, 0, 0.2);">
                                        <div class="details">
                                            <h3 style="color: white"><b>AGENT</b></h3>
                                            <asp:Label runat="server" ID="lblAgent" class="c-dashboardInfo__title"></asp:Label>
                                        </div>
                                        <div id="spark4" style="min-height: 80px;">
                                            <div id="apexchartsspark4" style="width: 40px; height: 41px; margin-left: 90px; margin-top: 27px;">
                                                <img src="../../images/Agent.png" data-src="../../images/Agent.png" alt="Group " title="Group " width="64" height="64" class="lzy lazyload--done" style="margin-left: 22px; margin-top: -21px;">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            &nbsp; &nbsp;
            <div class="row">
                <div id="chart-container" style="width: 100%; height: 400px; border: 1px solid #ccc;"></div>
                <asp:HiddenField ID="hidChartData" runat="server" />
                <%--<button type="button" class="chart-button" onclick="renderChart()">Show 3D Pie Chart</button>
                <div id="chart-container"></div>--%>
                <%--<button type="button" class="chart-button" onclick="renderChart1('AEPS')">Show AEPS Chart</button>--%>
                <%--<button type="button" class="chart-button" onclick="renderChart1('OtherChannel')">Show Other Channel Chart</button>--%>
                <%--<div id="chart-container"></div>--%>

                <!-- Container for the chart -->
            </div>
            &nbsp; &nbsp;
            <div class="card">
                <br />
                <div class="accordion-item" align="center">
                    <h2 class="accordion-header hideArrowIcon" id="headingHom">
                        <h2></h2>
                    </h2>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-666" runat="server">
                        <div class="panel">
                            <div class="panel-body">
                                <div id="container"></div>
                                <asp:Literal ID="ltScriptsNew" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            &nbsp; &nbsp;
             <div class="col-sm-5 col-xm-12" style="display: none">
                 <div class="selectInputDateBox w-100">
                     <input type="date" runat="server" id="Date1" class="multiple-dates select-date form-control" placeholder="Select Date" onchange="fncsave1(); checkDate1();" />
                 </div>
             </div>


            <div class="card">
                <br />
                <div class="accordion-item" align="center">
                    <h2 class="accordion-header hideArrowIcon" id="headingHom1">
                        <h2></h2>
                    </h2>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-666" runat="server">
                        <div class="panel">
                            <div class="panel-body">
                                <div id="container1"></div>
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>

    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" DropShadow="false" />
    <!-- ECharts -->
    <script type="text/javascript">
        function fncsave() {
            document.getElementById('<%= btnSearch.ClientID %>').click();
        }
    </script>

    <script type="text/javascript">  
        function checkDate() {
            var date = document.getElementById("<%= txtFromDate.ClientID %>").value;
            var todayDate = new Date().toISOString().slice(0, 10);
            if (date > todayDate) {
                alert("Selected date can not be greater than Current date !!");
                document.getElementById("<%= txtFromDate.ClientID %>").value = todayDate;
            }
            else
                document.getElementById("datePicker").value = todayDate;
        }
    </script>
    <script type="text/javascript">  
        function checkToDate() {
            var date = document.getElementById("<%= txtToDate.ClientID %>").value;
            var todayDate = new Date().toISOString().slice(0, 10);
            if (date > todayDate) {
                alert("Selected date can not be greater than Current date !!");
                document.getElementById("<%= txtToDate.ClientID %>").value = todayDate;
            }
            else
                document.getElementById("datePicker").value = todayDate;
        }
    </script>
    <%--<script>
        function renderChart1() {
            const dataSource = {
                chart: {
                    caption: "Recommended Portfolio Split",
                    subcaption: "For a net-worth of $1M",
                    showvalues: "1",
                    showpercentintooltip: "0",
                    numberprefix: "$",
                    enablemultislicing: "1",
                    theme: "candy"
                },
                data: [
                    { label: "Equity", value: "300000" },
                    { label: "Debt", value: "230000" },
                    { label: "Bullion", value: "180000" },
                    { label: "Real-estate", value: "270000" },
                    { label: "Insurance", value: "20000" }
                ]
            };

            // Create the chart
            const myChart = new FusionCharts({
                type: "pie3d",
                renderAt: "chart-container",
                width: "100%",
                height: "100%",
                dataFormat: "json",
                dataSource
            }).render();
        }
    </script>--%>
    <%--<script>
         function renderChart(channel) {
             const dataSource = {
                 chart: {
                     caption: "Portfolio Split for " + channel,
                     subcaption: "For a net-worth of $1M",
                     showvalues: "1",
                     showpercentintooltip: "0",
                     numberprefix: "$",
                     enablemultislicing: "1",
                     theme: "candy"
                 },
                 data: []
             };

             // Fetching data dynamically from the server using an AJAX call
             fetch(`YourPage.aspx/GetChartData?channel=${channel}`, {
                 method: 'POST',
                 headers: {
                     'Content-Type': 'application/json; charset=utf-8'
                 }
             })
                 .then(response => response.json())
                 .then(data => {
                     dataSource.data = data.d; // Assuming data.d contains your data
                     const myChart = new FusionCharts({
                         type: "pie3d",
                         renderAt: "chart-container",
                         width: "100%",
                         height: "100%",
                         dataFormat: "json",
                         dataSource
                     }).render();
                 })
                 .catch(error => console.error('Error fetching data:', error));
         }
    </script>--%>
    <script>
        function renderChart1() {
            // Get the chart data from the server-side HiddenField control
            const dataSourceText = document.getElementById('<%= hidChartData.ClientID %>').value;

            // Parse the data
            const dataSource = JSON.parse(dataSourceText);

            // Enable rotation
            dataSource.chart.enableRotation = "1";  // This will enable rotation for pie chart

            // Create the chart
            const myChart = new FusionCharts({
                type: "pie3d",
                renderAt: "chart-container",
                width: "100%",
                height: "100%",
                dataFormat: "json",
                dataSource
            }).render();
        }

        // Call this function to ensure renderChart1 runs after the page is fully loaded
        function callRenderChart() {
            renderChart1();
        }
    </script>
</asp:Content>
