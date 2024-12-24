/*
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

        // Transaction Summary Bar Chart
        var bouCtx = document.getElementsByClassName('blog-overview-users')[0];

        var bouData = {
            // Generate the days labels on the X axis.
            labels: Array.from(new Array(30), function (_, i) {
                return i + 1; // 30 days
            }),

            datasets: [{
                label: 'Current Month',
                data: [500, 800, 320, 180, 240, 320, 230, 650, 590, 1200, 750, 940, 1420, 1200, 960, 1450, 1820, 2800, 2102, 1920, 3920, 3202, 3140, 2800, 3200, 3200, 3400, 2910, 3100, 4250],
                backgroundColor: 'rgba(0,123,255,0.5)',
                borderColor: 'rgba(0,123,255,1)',
                borderWidth: 1.5,
            }, {
                label: 'Past Month',
                data: [380, 430, 120, 230, 410, 740, 472, 219, 391, 229, 400, 203, 301, 1380, 291, 620, 700, 300, 630, 402, 320, 380, 289, 410, 300, 530, 630, 720, 780, 1200],
                backgroundColor: 'rgba(255,65,105,0.5)',
                borderColor: 'rgba(255,65,105,1)',
                borderWidth: 1.5,
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
                            // Display labels every 7th day
                            return index % 7 === 0 ? tick : '';
                        }
                    }
                },
                yAxes: [{
                    ticks: {
                        suggestedMax: 1500,
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
            type: 'bar',
            data: bouData,
            options: bouOptions
        });
        //End

        //Rule-Wise Chart 
        function generateDonutChart(canvasId, successPercentage) {
            var ctx = document.getElementById(canvasId).getContext('2d');
            var success = successPercentage;
            var failure = 100 - success;

            var chart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    datasets: [{
                        data: [success, failure],
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
        generateDonutChart('rule1-chart', 85);
        generateDonutChart('rule2-chart', 80);
        generateDonutChart('rule3-chart', 83.33);
        generateDonutChart('rule4-chart', 90);
        generateDonutChart('rule5-chart', 90);
        //End Rule-Wise Chart 


        //Agent OnBoard Trend Chart
        var bouCtx = document.getElementsByClassName('AgentOnboardTrend')[0];

        // Data
        var bouData = {
            labels: Array.from(new Array(30), function (_, i) {
                return i === 0 ? 1 : i;
            }),
            datasets: [{
                label: 'Current Years',
                fill: 'start',
                data: [500, 800, 320, 180, 240, 320, 230, 650, 590, 1200, 750, 940, 1420, 1200, 960, 1450, 1820, 2800, 2102, 1920, 3920, 3202, 3140, 2800, 3200, 3200, 3400, 2910, 3100, 4250],
                backgroundColor: 'rgba(0,123,255,0.1)',
                borderColor: 'rgba(0,123,255,1)',
                pointBackgroundColor: '#ffffff',
                pointHoverBackgroundColor: 'rgb(0,123,255)',
                borderWidth: 1.5,
                pointRadius: 0,
                pointHoverRadius: 3
            }, {
                label: 'Past Year',
                fill: 'start',
                data: [380, 430, 120, 230, 410, 740, 472, 219, 391, 229, 400, 203, 301, 380, 291, 620, 700, 300, 630, 402, 320, 380, 289, 410, 300, 530, 630, 720, 780, 1200],
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
                    radius: 0
                }
            },
            scales: {
                xAxes: [{
                    gridLines: false,
                    ticks: {
                        callback: function (tick, index) {
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
                            return tick > 999 ? (tick / 1000).toFixed(1) + 'K' : tick;
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



        //Aggregator Bar Chart
        var bouCtx = document.getElementsByClassName('AggregatorView')[0];
        var aggregators = [
            "DataHive", "ContentFusion", "InfoMerge", "WebSift", "StreamSync", "SyncMaster",
            "AgriFusion", "Collecto", "GatherPro", "DataBridge", "SmartFusion", "MegaCollect",
            "ContentCore", "MediaHub", "OmniAggregator"
        ];

        var uniqueColors = [
            'rgba(255,99,132,0.6)', 'rgba(54,162,235,0.6)', 'rgba(255,206,86,0.6)', 'rgba(75,192,192,0.6)',
            'rgba(153,102,255,0.6)', 'rgba(255,159,64,0.6)', 'rgba(199,99,132,0.6)', 'rgba(255,123,200,0.6)',
            'rgba(122,180,255,0.6)', 'rgba(12,20,255,0.6)', 'rgba(200,245,99,0.6)', 'rgba(99,122,255,0.6)',
            'rgba(90,180,90,0.6)', 'rgba(100,150,200,0.6)', 'rgba(255,100,200,0.6)'
        ];

        var dataValues = [
            500, 800, 320, 180, 240, 320, 230, 650, 590, 1200, 750, 940, 1420, 1200, 960
        ];

        var bouData = {
            labels: aggregators,

            datasets: [{
                label: 'Current Month',
                data: dataValues,
                backgroundColor: uniqueColors,
                borderColor: uniqueColors,
                borderWidth: 1.5,
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
                        autoSkip: false,
                        maxRotation: 45,
                        minRotation: 45,
                    }
                },
                yAxes: [{
                    ticks: {
                        suggestedMax: 45,
                        callback: function (tick) {
                            // Check the size of the tick and apply the appropriate suffix
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
            type: 'bar',
            data: bouData,
            options: bouOptions
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
