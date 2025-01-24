/*
 |--------------------------------------------------------------------------
 | Shards Dashboards: Advance Dashbord|Milind
 |--------------------------------------------------------------------------
 */

'use strict';

(function ($) {
    $(document).ready(function () {


        /*  ***Date Picker Start**  */
        $('#blog-overview-date-range,.DateRangeCustom').datepicker({
            format: 'dd-mm-yyyy',
            autoclose: true
        });






        /*  **Date Picker End** */

        //***Channel-wise Summary ApexBar Chart
        var DataString = $('#hiddenChannelData').val();
        var Records = [];
        DataString = DataString.replace(/\\/g, '');
        Records = JSON.parse(DataString);

        var timeperiod = Records.map(t => t.timeperiod);
        var channel = Records.map(t => t.channel);
        var successcount = Records.map(t => t.successcount);
        var failurecount = Records.map(t => t.failurecount);
        var successrate = Records.map(t => t.successrate);
        var failurerate = Records.map(t => t.failurerate);

        var options = {
            chart: {
                type: 'bar',
                stacked: true,
                height: 350
            },
            series: [{
                name: 'Success',
                data: successcount
            }, {
                name: 'Failure',
                data: failurecount
            }],
            xaxis: {
                categories: timeperiod,
                labels: {
                    style: {
                        fontSize: '12px',
                        rotation: -45 //avoid overlap
                    },
                    formatter: function (value, index) {
                        if (timeperiod.length === 7) {
                            return value;
                        } else if (timeperiod.length === 31) {
                            return value;
                        }
                        return '';
                    }
                }
            },
            yaxis: {
                labels: {
                    formatter: function (value) {
                        if (value >= 1e9) {
                            return (value / 1e9).toFixed(1) + 'B'; // Billions
                        } else if (value >= 1e6) {
                            return (value / 1e6).toFixed(1) + 'M'; // Millions
                        } else if (value >= 1e3) {
                            return (value / 1e3).toFixed(1) + 'K'; // Thousands
                        } else {
                            return value;
                        }
                    }
                }
            },
            legend: {
                position: 'top'
            },
            tooltip: {
                y: {
                    formatter: function (value) {
                        return value;
                    }
                }
            },
            dataLabels: {
                enabled: true,
                formatter: function (value, { seriesIndex, dataPointIndex, w }) {
                    if (seriesIndex === 0) {
                        return `${successrate[dataPointIndex]}%`; // Success rate inside the success segment
                    } else if (seriesIndex === 1) {
                        return `${failurerate[dataPointIndex]}%`; // Failure rate inside the failure segment
                    }
                },
                style: {
                    fontSize: '10px',
                    fontWeight: 'bold',
                    colors: ['#fff']
                }
            },

            annotations: {
                points: timeperiod.map((period, index) => ({
                    x: period,
                    y: successcount[index] + failurecount[index] + 10,
                    marker: {
                        size: 0
                    },
                    label: {
                        text: channel[index],
                        style: {
                            fontSize: '12px',
                            fontWeight: 'bold',
                            color: '#000',
                            padding: 5
                        }
                    }
                }))
            },
            colors: ['#2196F3', '#dc3545'],
            fill: {
                type: 'gradient',
                gradient: {
                    shade: 'light',
                    type: 'vertical',
                    shadeIntensity: 0.5,
                    gradientToColors: ['#1e88e5'],
                    inverseColors: false,
                    opacityFrom: 1,
                    opacityTo: 1,
                    stops: [0, 100]
                }
            }
        };
        var chart = new ApexCharts(document.querySelectorAll(".ChannelTxnSummary")[0], options);
        chart.render();

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


        //**Agent Txn ApexBarChart
        var ctx = document.getElementsByClassName('AgentTxnSummary')[0];
        var DataString = $('#hiddenAgentTxnData').val();
        DataString = DataString.replace(/\\/g, '');
        var Records = JSON.parse(DataString);
        var timeperiod = Records.map(t => t.timeperiod);
        var agentname = Records.map(t => t.agentname);
        var TotalCount = Records.map(t => t.totalcount);
        var SuccessCount = Records.map(t => t.successcount);
        var Failurecount = Records.map(t => t.failurecount);
        var Successrate = Records.map(t => t.successrate);
        var Failurerate = Records.map(t => t.failurerate);

        var AgentsName = [...new Set(agentname)];

        var chartData = {
            labels: timeperiod,
            datasets: []
        };


        const colorPalette = [
            "#FF5733", "#33FF57", "#3357FF", "#57FF33", "#FF33FF", "#33FFFF", "#FFFF33", "#FF3333", "#33FF33", "#3333FF",
            "#FF9933", "#33FF99", "#9933FF", "#99FF33", "#FF6699", "#99FF66", "#6699FF", "#FF3366", "#66FF33", "#3366FF",
            "#FF6600", "#FF0066", "#6600FF", "#0066FF", "#FF3366", "#6600FF", "#0066FF", "#FF6600", "#FF0066", "#66FF33",
            "#3366FF", "#66FF33", "#3366FF", "#FF0033", "#33FF33", "#3333FF", "#FF0033", "#3399FF", "#99FF33", "#66FFFF",
            "#FF6666", "#66FF66", "#3366CC", "#FFCC00", "#00FFCC", "#CCCC00", "#33CCFF", "#FFCC33", "#3399CC", "#66CCCC"
        ];


        function getColorForAgent(index) {
            return colorPalette[index % colorPalette.length];
        }


        for (var i = 0; i < AgentsName.length; i++) {
            const agentData = Records.filter(t => t.agentname === AgentsName[i]).map(t => t.totalcount);


            chartData.datasets.push({
                label: AgentsName[i],
                backgroundColor: getColorForAgent(i),
                borderColor: getColorForAgent(i),
                borderWidth: 1,
                data: agentData,
                hoverBackgroundColor: getColorForAgent(i),
            });
        }

        var AgentTxnChart = new Chart(ctx, {
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
                            max: Math.max(...TotalCount) * 1.2,
                            callback: function (tick) {
                                if (tick >= 1e6) {
                                    return (tick / 1e6).toFixed(1) + 'M';
                                } else if (tick >= 1e3) {
                                    return (tick / 1e3).toFixed(1) + 'K';
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

        //**End

        //**Unique Customer Bar Chart
        window.UniqueCustomerChart = (function () {
            var DataString = $('#hiddenUnqCustomerData').val();
            DataString = DataString.replace(/\\/g, '');
            Records = JSON.parse(DataString);
            var CurrentTime = Records.map(t => t.CurrentPeriod);
            var currentData = Records.map(t => t.CurrentCount);
            var PreviousTime = Records.map(t => t.PreviousTimePeriod);
            var previousData = Records.map(t => t.PreviousCount);

            const allData = [...currentData, ...previousData];
            const minY = Math.min(...allData) - 0;
            const maxY = Math.max(...allData) + 5;

            var options = {
                series: [
                    {
                        name: 'Current Data',
                        type: 'line',
                        data: currentData
                    },
                    {
                        name: 'Previous Data',
                        type: 'line',
                        data: previousData
                    }
                ],
                chart: {
                    height: 350,
                    type: 'line',
                    dropShadow: {
                        enabled: true,
                        color: '#000',
                        top: 18,
                        left: 7,
                        blur: 10,
                        opacity: 0.5
                    },
                    zoom: {
                        enabled: false
                    },
                    toolbar: {
                        show: false
                    }
                },
                colors: ['#00BFFF', '#ADD8E6'],
                dataLabels: {
                    enabled: true
                },
                stroke: {
                    curve: 'smooth'
                },
                title: {
                    text: '',
                    align: 'left'
                },
                grid: {
                    borderColor: '#e7e7e7',
                    row: {
                        colors: ['#f3f3f3', 'transparent'],
                        opacity: 0.5
                    }
                },
                markers: {
                    size: 1
                },
                xaxis: {
                    categories: CurrentTime,
                    title: {
                        text: ''
                    }
                },
                yaxis: {
                    title: {
                        text: ''
                    },
                    min: 0,
                    max: 10,
                    labels: {
                        formatter: function (value) {
                            if (value >= 1e9) { // Billions
                                return Math.round(value / 1e9) + 'B';
                            } else if (value >= 1e6) { // Millions
                                return Math.round(value / 1e6) + 'M';
                            } else if (value >= 1e3) { // Thousands
                                return Math.round(value / 1e3) + 'K';
                            } else {
                                return value;
                            }
                        }
                    }
                },
                legend: {
                    position: 'top',
                    horizontalAlign: 'right',
                    floating: true,
                    offsetY: -25,
                    offsetX: -5
                }
            };
            var chart = new ApexCharts(document.querySelector(".UniqueCustomer"), options);
            chart.render();
            //window.UniqueCustomerChart = chart;
            return chart;
        })();
        //**END

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
