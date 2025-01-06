﻿/*
 |--------------------------------------------------------------------------
 | Shards Dashboards: Advance Dashbord|Milind
 |--------------------------------------------------------------------------
 */

'use strict';

(function ($) {
    $(document).ready(function () {

        // Blog overview date range init.
        $('#blog-overview-date-range').datepicker({});

        //
        // Small Stats
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
                // Uncomment the following line in order to disable the animations.
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

        //***Channel-wise Summary Bar Chart
        var DataString = $('#hiddenChannelData').val();
        var Records = [];
        DataString = DataString.replace(/\\/g, '');
        Records = JSON.parse(DataString);

        var timeperiod = Records.map(t => t.timeperiod);
        var channel = Records.map(t => t.channel);
        var totalcount = Records.map(t => t.totalcount);
        var successcount = Records.map(t => t.successcount);
        var failurecount = Records.map(t => t.failurecount);
        var successrate = Records.map(t => t.successrate);
        var failurerate = Records.map(t => t.failurerate);

        var bouCtx = document.getElementsByClassName('blog-overview-users')[0];
        var bouData = {
            labels: timeperiod,

            datasets: [{
                label: 'Success',
                data: successcount,
                backgroundColor: 'rgb(33, 150, 243)',
                borderColor: 'rgb(33, 150, 243)',
                borderWidth: 1.5,
                stack: 'stack1'
            }, {
                label: 'Failure',
                data: failurecount,
                backgroundColor: 'rgb(255, 69, 96)',
                borderColor: 'rgb(255, 69, 96)',
                borderWidth: 1.5,
                stack: 'stack1'
            }]
        };

        var bouOptions = {
            responsive: true,
            legend: {
                position: 'top'
            },
            scales: {
                x: {
                    grid: {
                        display: false
                    },
                    ticks: {
                        callback: function (tick, index) {
                            return index % 7 === 0 ? tick : '';
                        }
                    }
                },
                y: {
                    stacked: true,
                    beginAtZero: true, // Ensure the y-axis starts at zero
                    ticks: {
                        // Dynamically adjust the max value based on your data
                        min: 0,
                        max: Math.max(...successcount, ...failurecount) * 1.2, // Add 20% padding
                        callback: function (tick) {
                            // If the tick value is greater than 1 million, format as "M"
                            if (tick >= 1e6) {
                                return (tick / 1e6).toFixed(1) + 'M'; // Millions
                            }
                            // If the tick value is greater than 1 thousand, format as "K"
                            else if (tick >= 1e3) {
                                return (tick / 1e3).toFixed(1) + 'k'; // Thousands
                            }
                            // Return tick value as is if it's less than 1000
                            else {
                                return tick;
                            }
                        }
                    }
                }
            },

            hover: {
                mode: 'nearest',
                intersect: false
            },
            tooltips: {
                custom: false,
                mode: 'nearest',
                intersect: false
            },
            plugins: {
                datalabels: {
                    anchor: 'end',
                    align: 'top',
                    font: {
                        weight: 'bold',
                        size: 12
                    },
                    color: 'black',
                    formatter: function (value, context) {
                        const channelName = channel[context.dataIndex];
                        return channelName;
                    },
                    backgroundColor: 'rgba(255,255,255,0.8)',
                    borderRadius: 4
                }
            }
        };

        window.BlogOverviewUsers = new Chart(bouCtx, {
            type: 'bar',
            data: bouData,
            options: bouOptions
        });

        //***End




        //Agent OnBoard Line Chart
        var bouCtx = document.getElementsByClassName('AgentOnboardTrend')[0];
        var DataString = $('#hiddenAgentOnboradingData').val();
        var Records = [];
        DataString = DataString.replace(/\\/g, '');
        Records = JSON.parse(DataString);

        var timeperiod = Records.map(t => t.timeperiod);
        var Current = Records.map(t => t.currentcount);
        var Previous = Records.map(t => t.previouscount);

        var bouData = {
            labels: timeperiod,
            datasets: [{
                label: 'Current',
                fill: 'start',
                data: Current,
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
                label: 'Previous',
                fill: 'start',
                data: Previous,
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
                            var totalLabels = timeperiod.length;
                            if (totalLabels <= 7) {
                                return tick;
                            }
                            return index % 7 === 0 ? tick : '';
                        },
                        padding: 10
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
                custom: false,
                mode: 'nearest',
                intersect: false
            }
        };

        window.BlogOverviewUsers = new Chart(bouCtx, {
            type: 'LineWithLine',
            data: bouData,
            options: bouOptions
        });
        window.BlogOverviewUsers.render();
        //End Agent OnBoard Trend Chart



        //Agent Txn Bar Chart
        var ctx = document.getElementsByClassName('AggregatorView')[0];

        var AgentsName = [
            "DataHive", "ContentFusion", "InfoMerge", "WebSift", "StreamSync", "SyncMaster",
            "AgriFusion", "Collecto", "GatherPro", "DataBridge", "SmartFusion", "MegaCollect",
            "ContentCore", "MediaHub", "OmniAggregator"
        ];
        
        var agentData = [
            [20, 13, 8, 7, 22, 12, 3],  // DataHive
            [40, 10, 10, 30, 20, 30, 50], // ContentFusion
            [12, 23, 12, 14, 16, 18, 22], // InfoMerge
            [25, 18, 15, 22, 30, 35, 40], // WebSift
            [10, 12, 15, 20, 25, 30, 35], // StreamSync
            [30, 25, 10, 5, 40, 45, 50],  // SyncMaster
            [50, 45, 35, 40, 42, 47, 52], // AgriFusion
            [18, 16, 14, 13, 19, 17, 22], // Collecto
            [22, 30, 27, 28, 26, 32, 37], // GatherPro
            [15, 18, 22, 24, 27, 31, 33], // DataBridge
            [28, 19, 12, 25, 18, 15, 40], // SmartFusion
            [35, 40, 30, 27, 22, 18, 30], // MegaCollect
            [10, 14, 15, 18, 25, 35, 40], // ContentCore
            [5, 10, 12, 18, 20, 22, 25],  // MediaHub
            [40, 35, 32, 30, 27, 20, 22]  // OmniAggregator
        ];
       
        var chartData = {
            labels: ["January", "February", "March", "April", "May", "June", "July"],  // Example months
            datasets: []
        };
        
        for (var i = 0; i < AgentsName.length; i++) {
            chartData.datasets.push({
                label: AgentsName[i], 
                backgroundColor: `rgba(${(i * 50) % 255}, ${(i * 100) % 255}, ${(i * 150) % 255}, 0.5)`, 
                borderColor: `rgb(${(i * 50) % 255}, ${(i * 100) % 255}, ${(i * 150) % 255})`, 
                borderWidth: 1,
                data: agentData[i],  
                hoverBackgroundColor: `rgba(${(i * 50) % 255}, ${(i * 100) % 255}, ${(i * 150) % 255}, 0.7)`, 
            });
        }
        
        var myChart = new Chart(ctx, {
            type: 'bar', 
            data: chartData,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    x: {
                        grid: {
                            display: false
                        },
                        ticks: {
                            callback: function (tick, index) {
                                return index % 1 === 0 ? tick : '';
                            }
                        }
                    },
                    y: {
                        beginAtZero: true,
                        ticks: {
                            min: 0,
                            max: Math.max(...agentData.flat()) * 1.2,  
                            callback: function (tick) {
                                if (tick >= 1e6) {
                                    return (tick / 1e6).toFixed(1) + 'M'; // Millions
                                } else if (tick >= 1e3) {
                                    return (tick / 1e3).toFixed(1) + 'k'; // Thousands
                                } else {
                                    return tick;
                                }
                            }
                        }
                    }
                },
                plugins: {
                    tooltip: {
                        backgroundColor: 'rgba(0,0,0,0.7)',
                        titleColor: '#fff',
                        bodyColor: '#fff',
                    },
                    datalabels: {
                        anchor: 'end',
                        align: 'top',
                        font: {
                            weight: 'bold',
                            size: 12
                        },
                        color: 'black',
                        backgroundColor: 'rgba(255,255,255,0.8)',
                        borderRadius: 4
                    }
                }
            }
        });
        //End


        // Users by device pie chart
        // Data
        var ubdData = {
            datasets: [{
                hoverBorderColor: '#ffffff',
                data: [68.3, 24.2, 7.5],
                backgroundColor: [
                    'rgba(0,123,255,0.9)',
                    'rgba(0,123,255,0.5)',
                    'rgba(0,123,255,0.3)'
                ]
            }],
            labels: ["Desktop", "Tablet", "Mobile"]
        };

        // Options
        var ubdOptions = {
            legend: {
                position: 'bottom',
                labels: {
                    padding: 25,
                    boxWidth: 20
                }
            },
            cutoutPercentage: 0,
            // Uncomment the following line in order to disable the animations.
            // animation: false,
            tooltips: {
                custom: false,
                mode: 'index',
                position: 'nearest'
            }
        };

        var ubdCtx = document.getElementsByClassName('blog-users-by-device')[0];

        // Generate the users by device chart.
        window.ubdChart = new Chart(ubdCtx, {
            type: 'pie',
            data: ubdData,
            options: ubdOptions
        });

    });
})(jQuery);
