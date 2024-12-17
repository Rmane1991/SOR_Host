
/*
 |--------------------------------------------------------------------------
 | Shards Dashboards: Blog Overview Template
 |--------------------------------------------------------------------------
 */

'use strict';

(function ($) {
    $(document).ready(function () {

        //date range init.

        /*  $('#startDate').datepicker({
              format: 'dd-mm-yyyy',
              autoclose: true
          });
          $('#endDate').datepicker({
              format: 'dd-mm-yyyy',
              autoclose: true
          });
  
          $('#blog-overview-date-range-1').datepicker({
              format: 'dd-mm-yyyy',
              autoclose: true
          });
          $('#blog-overview-date-range-2').datepicker({
              format: 'dd-mm-yyyy',
              autoclose: true
          });
  
          $('#overview-date-bc-wise-1').datepicker({
              format: 'dd-mm-yyyy',
              autoclose: true
          });
          $('#overview-date-bc-wise-2').datepicker({
              format: 'dd-mm-yyyy',
              autoclose: true
          });*/

        $('#startDate').datepicker({
            format: 'dd-mm-yyyy',
            autoclose: true

        }).on('changeDate', function (e) {

            var startDate = e.date;
            $('#endDate').datepicker('setStartDate', startDate);
            var minEndDate = new Date(startDate);
            minEndDate.setMonth(minEndDate.getMonth() + 1);

            $('#endDate').datepicker('setEndDate', minEndDate);
            if (!$('#endDate').val()) {
                $('#endDate').datepicker('setDate', minEndDate);
            }

        });

        $('#endDate').datepicker({
            format: 'dd-mm-yyyy',
            autoclose: true

        }).on('changeDate', function (e) {

            var startDate = $('#startDate').datepicker('getDate');
            var endDate = e.date;

            if (endDate < startDate) {
                alert("End date cannot be before the start date.");
                $('#endDate').datepicker('setDate', startDate);

            } else {
                var diffMonths = (endDate.getFullYear() - startDate.getFullYear()) * 12 + endDate.getMonth() - startDate.getMonth();

                if (diffMonths > 1) {
                    alert("End date should not be more than 1 month after the start date.");
                    var minEndDate = new Date(startDate);
                    minEndDate.setMonth(minEndDate.getMonth() + 1);
                    $('#endDate').datepicker('setDate', minEndDate);

                }

            }

        });

        function setupDatepickerWithValidation(startDateSelector, endDateSelector) {
            $(startDateSelector).datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true
            }).on('changeDate', function (e) {
                var startDate = e.date;

                $(endDateSelector).datepicker('setStartDate', startDate);
                var minEndDate = new Date(startDate);

                minEndDate.setMonth(minEndDate.getMonth() + 1);
                $(endDateSelector).datepicker('setEndDate', minEndDate);
                if (!$(endDateSelector).val()) {
                    $(endDateSelector).datepicker('setDate', minEndDate);
                }
            });

            $(endDateSelector).datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true
            }).on('changeDate', function (e) {

                var startDate = $(startDateSelector).datepicker('getDate');
                var endDate = e.date;

                if (endDate < startDate) {
                    alert("End date cannot be before the start date.");
                    $(endDateSelector).datepicker('setDate', startDate);

                } else {
                    var diffMonths = (endDate.getFullYear() - startDate.getFullYear()) * 12 + endDate.getMonth() - startDate.getMonth();

                    if (diffMonths > 1) {

                        alert("End date should not be more than 1 month after the start date.");
                        var minEndDate = new Date(startDate);
                        minEndDate.setMonth(minEndDate.getMonth() + 1);
                        $(endDateSelector).datepicker('setDate', minEndDate);
                    }
                }

            });

        }

        // Initialize datepickers with validation for the first pair
        setupDatepickerWithValidation('#blog-overview-date-range-1', '#blog-overview-date-range-2');
        setupDatepickerWithValidation('#overview-date-bc-wise-1', '#overview-date-bc-wise-2');
        setupDatepickerWithValidation('#RevenueSummary-d1', '#RevenueSummary-d2');
        //End

        var transactionsString = $('#hiddenTransactions').val();
        var transactions = [];

        var SummaryDataString = $('#hiddenSummaryData').val();
        var SummaryDataTxn = [];

        var BcWiseDataString = $('#hiddenBCwiseData').val();
        var BCWiseDataTxn = [];

        var SwitchWiseDataString = $('#hiddenSwitchwiseData').val();
        var SwitchWiseDataTxn = [];

        var RevenueDataString = $('#hiddenRevenueData').val();
        var RevenueDataTxn = [];
        RevenueDataString = RevenueDataString.replace(/\\/g, '');
        RevenueDataTxn = JSON.parse(RevenueDataString);

        if (transactionsString) {
            try {

                transactionsString = transactionsString.replace(/\\/g, '');
                SummaryDataString = SummaryDataString.replace(/\\/g, '');
                BcWiseDataString = BcWiseDataString.replace(/\\/g, '');
                SwitchWiseDataString = SwitchWiseDataString.replace(/\\/g, '');


                transactions = JSON.parse(transactionsString);
                SummaryDataTxn = JSON.parse(SummaryDataString);
                BCWiseDataTxn = JSON.parse(BcWiseDataString);
                SwitchWiseDataTxn = JSON.parse(SwitchWiseDataString);


            } catch (e) {
                console.error("Error parsing JSON:", e);
            }
        } else {
            console.warn("No transactions data found.");
        }

        var SummaryTxnDays = transactions.map(t => t.Day);
        var currentMonthData = transactions.map(t => t.CurrentMonthCount);
        var previousMonthData = transactions.map(t => t.PreviousMonthCount);

        var SwitchTotalTxn = SummaryDataTxn.map(t => t.BC);
        var BCTotalTxn = SummaryDataTxn.map(t => t.Switch);
        var RuleTotalTxn = SummaryDataTxn.map(t => t.Rule);

        var SwitchName = SwitchWiseDataTxn.map(t => t.SwitchName);
        var SwitchCounts = SwitchWiseDataTxn.map(t => t.Counts);
        var SwitchPercent = SwitchWiseDataTxn.map(t => t.Percent);
        //

        // Datasets
        var boSmallStatsDatasets = [
            {
                backgroundColor: 'rgba(0, 184, 216, 0.1)',
                borderColor: 'rgb(0, 184, 216)',
                data: [1, 2, 1, 3, 5, 4, 7],
            },
            {
                backgroundColor: 'rgba(23,198,113,0.1)',
                borderColor: 'rgb(23,198,113)',
                data: [1, 2, 3, 3, 3, 4, 4]
            },
            {
                backgroundColor: 'rgba(255,180,0,0.1)',
                borderColor: 'rgb(255,180,0)',
                data: [2, 3, 3, 3, 4, 3, 3]
            },
            {
                backgroundColor: 'rgba(255,65,105,0.1)',
                borderColor: 'rgb(255,65,105)',
                data: [1, 7, 1, 3, 1, 4, 8]
            },
            {
                backgroundColor: 'rgb(0,123,255,0.1)',
                borderColor: 'rgb(0,123,255)',
                data: [3, 2, 3, 2, 4, 5, 4]
            }
        ];

        // Options
        function boSmallStatsOptions(max) {
            return {
                maintainAspectRatio: true,
                responsive: true,
                // animation: false,
                legend: {
                    display: false
                },
                tooltips: {
                    enabled: false,
                    custom: false
                },
                elements: {
                    point: {
                        radius: 0
                    },
                    line: {
                        tension: 0.3
                    }
                },
                scales: {
                    xAxes: [{
                        gridLines: false,
                        scaleLabel: false,
                        ticks: {
                            display: false
                        }
                    }],
                    yAxes: [{
                        gridLines: false,
                        scaleLabel: false,
                        ticks: {
                            display: false,
                            // Avoid getting the graph line cut of at the top of the canvas.
                            // Chart.js bug link: https://github.com/chartjs/Chart.js/issues/4790
                            suggestedMax: max
                        }
                    }],
                },
            };
        }

        // Generate the small charts
        boSmallStatsDatasets.map(function (el, index) {
            var chartOptions = boSmallStatsOptions(Math.max.apply(Math, el.data) + 1);
            var ctx = document.getElementsByClassName('blog-overview-stats-small-' + (index + 1));
            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ["Label 1", "Label 2", "Label 3", "Label 4", "Label 5", "Label 6", "Label 7"],
                    datasets: [{
                        label: 'Today',
                        fill: 'start',
                        data: el.data,
                        backgroundColor: el.backgroundColor,
                        borderColor: el.borderColor,
                        borderWidth: 1.5,
                    }]
                },
                options: chartOptions
            });
        });

        // Transaction Summary 
        var bouCtx = document.getElementsByClassName('blog-overview-users')[0];

        var bouData = {
            // Generate the days labels on the X axis. 
            //labels: Array.from(new Array(30), function (_, i) {
            //    return i === 0 ? 1 : i;
            //}),
            labels: SummaryTxnDays,
            datasets: [{
                label: 'Current Month',
                fill: 'start',
                data: currentMonthData,
                backgroundColor: 'rgba(0,123,255,0.1)',
                borderColor: 'rgba(0,123,255,1)',
                pointBackgroundColor: '#ffffff',
                pointHoverBackgroundColor: 'rgb(0,123,255)',
                borderWidth: 1.5,
                pointRadius: 0,
                pointHoverRadius: 3
            }, {
                label: 'Past Month',
                fill: 'start',
                data: previousMonthData,
                backgroundColor: 'rgba(255,65,105,0.1)',
                borderColor: 'rgba(255,65,105,1)',
                pointBackgroundColor: '#ffffff',
                pointHoverBackgroundColor: 'rgba(255,65,105,1)',
                borderDash: [3, 3],
                borderWidth: 1,
                pointRadius: 0,
                pointHoverRadius: 2,
                pointBorderColor: 'rgba(255,65,105,1)'
            }]
        };

        // Options
        var bouOptions = {
            responsive: true,
            legend: {
                position: 'top'
            },
            elements: {
                line: {
                    // A higher value makes the line look skewed at this ratio.
                    tension: 0.3
                },
                point: {
                    radius: 0
                }
            },
            scales: {
                xAxes: [{
                    gridLines: false,
                    ticks: {
                        callback: function (tick, index) {
                            // Jump every 7 values on the X axis labels to avoid clutter.
                            return index % 7 !== 0 ? '' : tick;
                        }
                    }
                }],
                yAxes: [{
                    ticks: {
                        suggestedMax: 45,
                        callback: function (tick, index, ticks) {
                            if (tick === 0) {
                                return tick;
                            }
                            // Format the amounts using Ks for thousands.
                            return tick > 999 ? (tick / 1000).toFixed(1) + 'K' : tick;
                        }
                    }
                }]
            },
            // Uncomment the next lines in order to disable the animations.
            // animation: {
            //   duration: 0
            // },
            hover: {
                mode: 'nearest',
                intersect: false
            },
            tooltips: {
                custom: false,
                mode: 'nearest',
                intersect: false
            }
        };

        // Generate the Analytics Overview chart.
        window.BlogOverviewUsers = new Chart(bouCtx, {
            type: 'LineWithLine',
            data: bouData,
            options: bouOptions
        });

        // Hide initially the first and last analytics overview chart points.
        // They can still be triggered on hover.
        var aocMeta = BlogOverviewUsers.getDatasetMeta(0);
        aocMeta.data[0]._model.radius = 0;
        aocMeta.data[bouData.datasets[0].data.length - 1]._model.radius = 0;

        // Render the chart.
        window.BlogOverviewUsers.render();

        // Switch Transactions Overview
        const hasData = SwitchCounts.some(count => count > 0);
        var ubdData = {
            datasets: [{
                hoverBorderColor: '#ffffff',
                data: hasData ? SwitchPercent : [1],
                backgroundColor: hasData ? [
                    'rgba(0,123,255,0.9)',
                    'rgba(0,123,255,0.5)',
                    'rgba(0,123,255,0.3)',
                    'rgba(0,123,255,0.7)'
                ] : ['rgba(144,238,144,0.5)']
            }],
            labels: hasData ? SwitchName : ['No Data']
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
                                const percent = SwitchPercent[index];
                                return {
                                    text: `${label}: ${percent}%`,
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
                            const percent = SwitchPercent[tooltipItem.index];
                            return `${percent}%`;
                        }
                    }
                }
            },
            cutoutPercentage: 75,
            tooltips: {
                enabled: true,
                mode: 'index',
                position: 'nearest',
                callbacks: {
                    label: function (tooltipItem, data) {
                        const percent = SwitchPercent[tooltipItem.index];
                        return `${percent}%`;
                    }
                }
            }
        };

        var ubdCtx = document.getElementsByClassName('blog-users-by-device')[0];
        window.ubdChart = new Chart(ubdCtx, {
            type: 'doughnut',
            data: ubdData,
            options: ubdOptions
        });
        window.ubdChart.update();
        //End


        //Bar graph for BC-wise Summary(ApexChart)
        window.BCWiseSummaryChart = (function () {
            var successCurrent = BCWiseDataTxn.map(t => t.Success);
            var failureCurrent = BCWiseDataTxn.map(t => t.Failure);
            var successRate = BCWiseDataTxn.map(t => t.SuccessRate);
            var failureRate = BCWiseDataTxn.map(t => t.FailureRate);
            var userNames = BCWiseDataTxn.map(t => t.bcname);

            var options = {
                chart: {
                    type: 'bar',
                    height: 410,
                    stacked: true,
                    toolbar: {
                        show: true
                    }
                },
                plotOptions: {
                    bar: {
                        horizontal: true,
                        barHeight: '70%'
                    }
                },
                dataLabels: {
                    enabled: true,
                    formatter: function (val) {
                        return val;
                    },
                    style: {
                        fontSize: '12px',
                        fontWeight: 'bold',
                        colors: ['#fff']
                    }
                },
                colors: ['RGBA(56, 142, 60, 0.8)', 'RGBA(211, 47, 47, 0.8)'],
                series: [
                    {
                        name: 'Success',
                        data: successCurrent
                    },
                    {
                        name: 'Failure',
                        data: failureCurrent
                    }
                ],
                xaxis: {
                    categories: userNames,
                    labels: {
                        formatter: function (val) {
                            return val > 999 ? (val / 1000).toFixed(1) + 'K' : val;
                        },
                        style: {
                            fontSize: '12px',
                            colors: '#555'
                        }
                    },
                    title: {
                        text: 'Transactions',
                        style: {
                            fontSize: '14px',
                            fontWeight: 'bold'
                        }
                    }
                },
                yaxis: {
                    title: {
                        text: 'Business Correspondence',
                        style: {
                            fontSize: '14px',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: '14px',
                            colors: ['#555']
                        }
                    }
                },
                tooltip: {
                    y: {
                        formatter: function (val, opts) {
                            var index = opts.seriesIndex;
                            var tooltipText = index === 0
                                ? `Success: ${val} counts`
                                : `Failure: ${val} counts`;
                            var rateLabel = index === 0
                                ? `Rate: ${successRate[opts.dataPointIndex]}%`
                                : `Rate: ${failureRate[opts.dataPointIndex]}%`;
                            return tooltipText + ' (' + rateLabel + ')';
                        }
                    }
                },
                legend: {
                    position: 'top',
                    horizontalAlign: 'center',
                    labels: {
                        useSeriesColors: false,
                        fontSize: '12px'
                    }
                }
            };

            var chart = new ApexCharts(document.querySelector("#blog-overview-BCWise"), options);
            chart.render();
            return chart;
        })();
        //ENd Bar graph for BC-wise Summary(ApexChart)

        /*Revenue Chart*/
        window.RevenueComboChart = (function () {
            var revenueData = RevenueDataTxn.map(t => t.TotalRevenue);
            var conversionRateData = RevenueDataTxn.map(t => t.ConversionRate);
            var categories = RevenueDataTxn.map(t => t.PeriodName);

            var options = {
                series: [
                    {
                        name: 'Revenue',
                        type: 'bar',
                        data: revenueData
                    },
                    {
                        name: 'Conversion Rate',
                        type: 'line',
                        data: conversionRateData
                    }
                ],
                chart: {
                    height: 400,
                    type: 'line',
                    zoom: {
                        enabled: false
                    }
                },
                dataLabels: {
                    enabled: true,
                    style: {
                        fontSize: '12px',
                        fontWeight: 'bold',
                        colors: ['#000'] // Color of the labels
                    },
                    dropShadow: {
                        enabled: true,
                        top: 2,
                        left: 2,
                        blur: 4,
                        opacity: 0.3
                    },
                    offsetY: -10
                },
                stroke: {
                    width: [0, 3]
                },
                title: {
                    text: 'Revenue and Conversion Rate Summary'
                },
                xaxis: {
                    categories: categories,
                },
                yaxis: [
                    {
                        title: {
                            text: 'Revenue (₹)'
                        }
                    },
                    {
                        opposite: true,
                        title: {
                            text: 'Conversion Rate (%)'
                        }
                    }
                ],
                tooltip: {
                    shared: true,
                    intersect: false
                },
                // Optional annotations section entirely
                plotOptions: {
                    bar: {
                        dataLabels: {
                            position: 'top'
                        }
                    }
                }
            };

            var chart = new ApexCharts(document.querySelector("#overview-RevenueSummary"), options);
            chart.render();
            return chart;
        })();
        /*  End Revenue Chart */

        //google chart new Channel wisess
        google.charts.load('current', { 'packages': ['corechart', 'charteditor'] });
        google.charts.setOnLoadCallback(drawChart);
        var chart;
        function drawChart() {
            var ChannelWiseDataString = $('#hiddenChannelwiseData').val();
            ChannelWiseDataString = ChannelWiseDataString.replace(/\\/g, '');
            var ChannelWiseDataTxn = JSON.parse(ChannelWiseDataString);
            var ChannelName = ChannelWiseDataTxn.map(t => t.channel);
            var ChannelCounts = ChannelWiseDataTxn.map(t => t.totalcount);
            var data = [['Channel', 'Count']];

            for (var i = 0; i < ChannelName.length; i++) {
                data.push([ChannelName[i], ChannelCounts[i]]);
            }

            var dataTable = google.visualization.arrayToDataTable(data);

            var options = {
                title: 'Channel Summary',
                titleTextStyle: {
                    color: '#333',
                    fontSize: 20,
                    bold: true
                },
                is3D: true,
                pieSliceText: 'percentage',
                pieSliceTextStyle: {
                    color: '#fff',
                    fontSize: 14
                },
                slices: {
                    0: { offset: 0.02, color: '#2196F3' },
                    1: { offset: 0.02, color: '#FFEB3B' },
                    2: { offset: 0.02, color: '#4CAF50' },
                },
                legend: {
                    position: 'bottom',
                    alignment: 'center',
                    textStyle: { color: '#555', fontSize: 14 }
                },
                tooltip: {
                    isHtml: true,
                    trigger: 'focus',
                    textStyle: { fontSize: 12, color: '#000' }
                },
                animation: {
                    duration: 1500,
                    easing: 'inAndOut'
                },
                chartArea: {
                    left: '10%',
                    top: '0%',
                    width: '80%',
                    height: '75%'
                },
                pieSliceBorderColor: '#ffffff',
            };

            var chart = new google.visualization.PieChart(document.getElementById('piechart_3d'));
            chart.draw(dataTable, options);

            google.visualization.events.addListener(chart, 'select', function () {
                var selection = chart.getSelection();
                if (selection.length > 0) {
                    var selectedSlice = selection[0];
                    var channel = dataTable.getValue(selectedSlice.row, 0);
                    var count = dataTable.getValue(selectedSlice.row, 1);
                    //alert('You selected: ' + channel + ' with ' + count + ' transactions');
                }
            });
        }

        // Export chart as image (PNG)
        function exportChartAsImage() {
            var imageUri = chart.getImageURI(); // Get the image URI (PNG)
            var link = document.createElement('a');
            link.href = imageUri;
            link.target = '_blank'; // Open in new tab for download
            link.download = 'channel-summary.png'; // Set default filename
            link.click();
        }

        // Export data as CSV
        function exportDataAsCSV() {
            var data = chart.getDataTable();
            var csv = 'Channel,Count\n'; // CSV header
            for (var row = 0; row < data.getNumberOfRows(); row++) {
                csv += data.getValue(row, 0) + ',' + data.getValue(row, 1) + '\n';
            }

            // Create a hidden download link for CSV
            var hiddenElement = document.createElement('a');
            hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(csv); // Encode CSV to URI
            hiddenElement.target = '_blank';
            hiddenElement.download = 'channel-summary.csv'; // Set default CSV filename
            hiddenElement.click();
        }
        //End

        //Tab controls Day/Week/Month/Year
        const buttons = document.querySelectorAll('.btn-group .btn');
        buttons.forEach(button => {
            button.addEventListener('click', () => {
                buttons.forEach(btn => btn.classList.remove('active'));
                button.classList.add('active');
            });
        });
        //end

    });
})(jQuery);