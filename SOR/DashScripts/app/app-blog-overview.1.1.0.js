
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

        /*var SummaryTxnDays = transactions.map(t => t.Day);
        var currentMonthData = transactions.map(t => t.CurrentMonthCount);
        var PreviousTxnDays = transactions.map(t => t.PreviousDay);
        var previousMonthData = transactions.map(t => t.PreviousMonthCount);*/

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
        var CurrentTxnDays = transactions.map(t => t.Day);
        var currentMonthData = transactions.map(t => t.CurrentMonthCount);
        var PreviousTxnDays = transactions.map(t => t.PreviousDay);
        var previousMonthData = transactions.map(t => t.PreviousMonthCount);

        var bouData = {
            labels: CurrentTxnDays,
            datasets: [{
                label: 'Current Data',
                fill: 'start',
                data: currentMonthData,
                backgroundColor: 'rgba(0,123,255,0.1)',
                borderColor: 'rgba(0,123,255,1)',
                pointBackgroundColor: '#ffffff',
                pointHoverBackgroundColor: 'rgb(0,123,255)',
                borderWidth: 1.5,
                pointRadius: 3,
                pointHoverRadius: 5,
                borderJoinStyle: 'round',
                lineTension: 0.3,
            }, {
                label: 'Previous Data',
                fill: 'start',
                data: previousMonthData,
                backgroundColor: 'rgba(255,65,105,0.1)',
                borderColor: 'rgba(255,65,105,1)',
                pointBackgroundColor: '#ffffff',
                pointHoverBackgroundColor: 'rgba(255,65,105,1)',
                borderDash: [3, 3],
                borderWidth: 1,
                pointRadius: 3,
                pointHoverRadius: 5,
                pointBorderColor: 'rgba(255,65,105,1)',
            }]
        };

        var bouOptions = {
            responsive: true,
            legend: {
                position: 'top'
            },
            elements: {
                line: {
                    tension: 0.3
                },
                point: {
                    radius: 3
                }
            },
            scales: {
                xAxes: [{
                    gridLines: false,
                    ticks: {
                        autoSkip: false,
                        maxRotation: 45,
                        minRotation: 0,
                        callback: function (tick, index) {
                            var totalLabels = CurrentTxnDays.length;
                            if (totalLabels <= 7) {
                                return tick;
                            }
                            return index % 7 === 0 ? tick : '';
                        },
                        padding: 15
                    }
                }],
                yAxes: [{
                    ticks: {
                        suggestedMax: 45,
                        callback: function (tick) {
                            if (tick >= 1e9) {
                                return (tick / 1e9).toFixed(1) + 'B'; // Billions
                            } else if (tick >= 1e6) {
                                return (tick / 1e6).toFixed(1) + 'M'; // Millions
                            } else if (tick >= 1e3) {
                                return (tick / 1e3).toFixed(1) + 'K'; // Thousands
                            } else {
                                return tick;
                            }
                        }
                    }
                }]
            },
            hover: {
                mode: 'nearest',
                intersect: false
            },
            tooltips: {
                enabled: true,
                mode: 'nearest',
                intersect: true,
                callbacks: {
                    title: function (tooltipItem, data) {
                        var index = tooltipItem[0].index;
                        var datasetIndex = tooltipItem[0].datasetIndex;

                        if (datasetIndex === 0) {
                            return `Date: ${transactions[index].Day}`;
                        } else if (datasetIndex === 1) {
                            return `Date: ${transactions[index].PreviousDay}`;
                        }
                    },
                    label: function (tooltipItem, data) {
                        var index = tooltipItem.index;
                        var datasetIndex = tooltipItem.datasetIndex;

                        var currentValue = (transactions[index] && transactions[index].CurrentMonthCount !== undefined)
                            ? transactions[index].CurrentMonthCount
                            : 'No data available';
                        var previousValue = (transactions[index] && transactions[index].PreviousMonthCount !== undefined)
                            ? transactions[index].PreviousMonthCount
                            : 'No data available';

                        if (datasetIndex === 0) {
                            return `Current Month: ${currentValue}`;
                        } else if (datasetIndex === 1) {
                            return `Previous Month: ${previousValue}`;
                        }
                    }

                }
            }

        };

        window.BlogOverviewUsers = new Chart(bouCtx, {
            type: 'LineWithLine',
            data: bouData,
            options: bouOptions
        });

        //Hide initially the first and last analytics overview chart points.
        // They can still be triggered on hover.
        /*var aocMeta = BlogOverviewUsers.getDatasetMeta(0);
        aocMeta.data[0]._model.radius = 0;
        aocMeta.data[bouData.datasets[0].data.length - 1]._model.radius = 0;*/
        window.BlogOverviewUsers.render();
        //End

        // Switch Transactions Overview(ChartJs)
        const hasData = SwitchCounts.some(count => count > 0);
        var ubdData = {
            datasets: [{
                borderWidth: 2,
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
                    bottom: 50
                }
            },
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            const label = SwitchName[tooltipItem.index];
                            const percent = SwitchPercent[tooltipItem.index];
                            return `${label}: ${percent}%`;
                        }
                    }
                },
                outlabels: {
                    display: true,
                    text: '%l: %p%',
                    color: '#000',
                    font: {
                        size: 12,
                        weight: 'bold'
                    },
                    lineWidth: 2,
                    lineColor: '#d3d3d3',
                    padding: 4,
                    stretch: 20
                }
            },
            cutout: '75%'
        };

        var ubdCtx = document.getElementsByClassName('blog-users-by-device')[0];
        window.ubdChart = new Chart(ubdCtx, {
            type: 'doughnut',
            data: ubdData,
            options: ubdOptions
        });

        //SwitchCounts = [10, 20, 30, 40]; // Replace with your counts
        //SwitchPercent = [25, 50, 15, 10]; // Replace with your percentages
        //SwitchName = ["Switch A", "Switch B", "Switch C", "Switch D"]; // Replace with labels

        //var hasData = SwitchCounts.some(count => count > 0);

        //// Dynamic data
        //const chartData = hasData ? SwitchPercent.map((percent, index) => ({
        //    name: SwitchName[index],
        //    y: percent,
        //    color: `rgba(0,123,255,${0.3 + (index * 0.2)})` // Adjusting opacity dynamically
        //})) : [{ name: "No Data", y: 1, color: 'rgba(144,238,144,0.5)' }];

        //// Highcharts Donut Chart
        //const chartContainer = document.getElementsByClassName('blog-users-by-device')[0];

        //Highcharts.chart(chartContainer, {
        //    chart: {
        //        type: 'pie'
        //    },
        //    title: {
        //        text: hasData ? 'Switch Data Distribution' : 'No Data Available'
        //    },
        //    credits: {
        //        enabled: false // Correct placement to remove the Highcharts watermark
        //    },
        //    plotOptions: {
        //        pie: {
        //            innerSize: '75%', // Creates the donut shape
        //            dataLabels: {
        //                enabled: true,
        //                format: '<b>{point.name}</b>: {point.y}%' // Label format
        //            },
        //            borderWidth: 2,
        //            borderColor: '#ffffff'
        //        }
        //    },
        //    tooltip: {
        //        formatter: function () {
        //            return `<b>${this.point.name}</b>: ${this.point.y}%`;
        //        }
        //    },
        //    series: [{
        //        name: 'Percentage',
        //        data: chartData
        //    }],
        //    legend: {
        //        enabled: false // Hide legend
        //    }
        //});



        //End


        //Bar graph for BC-wise Summary (ApexChart)
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
                    stacked: true, // Stacked bars enabled
                    toolbar: {
                        show: true
                    },
                    margin: {
                        left: 80,
                        right: 20
                    }
                },
                plotOptions: {
                    bar: {
                        horizontal: true,
                        barHeight: '70%',
                        distributed: false, // Set this to false to avoid overriding colors
                        padding: {
                            left: 10,
                            right: 10
                        }
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
                colors: ['#2196f3', '#ff4560'],
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
                            fontWeight: 'bold',
                            offsetX: 40 // Increased space from the y-axis
                        }
                    },
                    labels: {
                        style: {
                            fontSize: '14px',
                            colors: ['#555']
                        },
                        offsetX: 5
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
                    floating: true,
                    labels: {
                        useSeriesColors: false,
                        fontSize: '12px'
                    }
                },
                loading: {
                    show: true,
                    text: 'Loading...',
                    fontSize: '14px',
                    style: {
                        background: '#f0f0f0',
                        color: '#000',
                        opacity: 0.9
                    }
                }
            };

            var chart = new ApexCharts(document.querySelector("#blog-overview-BCWise"), options);
            chart.render();
            window.addEventListener('resize', function () {
                chart.updateOptions({ chart: { height: 410 } });
            });

            return chart;
        })();
        // End Bar graph for BC-wise Summary (ApexChart)


        /*Revenue Chart (ApexChart)*/
        //window.RevenueComboChart = (function () {
        //    var revenueData = RevenueDataTxn.map(t => t.TotalRevenue);
        //    var conversionRateData = RevenueDataTxn.map(t => t.ConversionRate);
        //    var categories = RevenueDataTxn.map(t => t.PeriodName);

        //    var options = {
        //        series: [
        //            {
        //                name: 'Revenue',
        //                type: 'bar',
        //                data: revenueData
        //            },
        //            {
        //                name: 'Conversion Rate',
        //                type: 'line',
        //                data: conversionRateData
        //            }
        //        ],
        //        chart: {
        //            height: 400,
        //            type: 'line',
        //            zoom: {
        //                enabled: false
        //            }
        //        },
        //        dataLabels: {
        //            enabled: true,
        //            style: {
        //                fontSize: '12px',
        //                fontWeight: 'bold',
        //                colors: ['#000'] // Color of the labels
        //            },
        //            dropShadow: {
        //                enabled: true,
        //                top: 2,
        //                left: 2,
        //                blur: 4,
        //                opacity: 0.3
        //            },
        //            offsetY: -10
        //        },
        //        stroke: {
        //            width: [0, 3]
        //        },
        //        title: {
        //            text: 'Revenue and Conversion Rate Summary'
        //        },
        //        xaxis: {
        //            categories: categories,
        //        },
        //        yaxis: [
        //            {
        //                title: {
        //                    text: 'Revenue (₹)'
        //                }
        //            },
        //            {
        //                opposite: true,
        //                title: {
        //                    text: 'Conversion Rate (%)'
        //                }
        //            }
        //        ],
        //        tooltip: {
        //            shared: true,
        //            intersect: false
        //        },
        //        // Optional annotations section entirely
        //        plotOptions: {
        //            bar: {
        //                dataLabels: {
        //                    position: 'top'
        //                }
        //            }
        //        }
        //    };

        //    var chart = new ApexCharts(document.querySelector("#overview-RevenueSummary"), options);
        //    chart.render();
        //    return chart;
        //})();

        /*Revenue Chart (ApexChart)*/
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
                        },
                        labels: {
                            formatter: function (value) {
                                if (value >= 1000000000) {
                                    return (value / 1000000000).toFixed(1) + "B"; // Billions
                                } else if (value >= 1000000) {
                                    return (value / 1000000).toFixed(1) + "M"; // Millions
                                } else if (value >= 1000) {
                                    return (value / 1000).toFixed(1) + "K"; // Thousands
                                } else {
                                    return value; // No suffix for smaller values
                                }
                            }
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