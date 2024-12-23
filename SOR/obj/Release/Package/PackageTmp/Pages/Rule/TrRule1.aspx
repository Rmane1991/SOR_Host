<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="TrRule1.aspx.cs" Inherits="SOR.Pages.Rule.TrRule1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<head>--%>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Standard List View | PatternFly</title>
    <link rel="canonical" href="https://pf3.patternfly.org" />
    <meta name="description" content="PatternFly is an open source web user interface framework promoting consistency and usability across IT applications through UX patterns and widgets">
    <link rel="shortcut icon" href="../../v3/components/patternfly/dist/img/favicon.ico">
    <!-- iPad retina icon -->
    <link rel="apple-touch-icon-precomposed" sizes="152x152" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-152.png">
    <!-- iPad retina icon (iOS < 7) -->
    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-144.png">
    <!-- iPad non-retina icon -->
    <%--<link rel="apple-touch-icon-precomposed" sizes="76x76" href=../../"v3/components/patternfly/dist/img/apple-touch-icon-precomposed-76.png">--%>
    <!-- iPad non-retina icon (iOS < 7) -->
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-72.png">
    <!-- iPhone 6 Plus icon -->
    <link rel="apple-touch-icon-precomposed" sizes="120x120" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-180.png">
    <!-- iPhone retina icon (iOS < 7) -->
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-114.png">
    <!-- iPhone non-retina icon (iOS < 7) -->
    <link rel="apple-touch-icon-precomposed" sizes="57x57" href="../../v3/components/patternfly/dist/img/apple-touch-icon-precomposed-57.png">
    <link rel="stylesheet" href="../../v3/assets/css/patternfly-adjusted.min.css">
    <link rel="stylesheet" href="../../v3/components/patternfly/dist/css/patternfly-additions.min.css">
    <link rel="stylesheet" href="../../v3/assets/css/patternfly-site.min.css">
    <script type="text/javascript">
        window.IMAGE_PATH = "/v3/assets/img"
  </script>
    <script src="../../ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="../../v3/components/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="../../v3/components/patternfly-bootstrap-combobox/js/bootstrap-combobox.js"></script>
    <script src="../../ajax/libs/moment_js/2.21.0/moment.min.js"></script>
    <script src="../../v3/components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"></script>
    <script src="../../ajax/libs/bootstrap-slider/9.9.0/bootstrap-slider.min.js"></script>
    <script src="../../v3/components/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="../../ajax/libs/bootstrap-datetimepicker/4.17.47/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../../ajax/libs/d3/3.5.0/d3.min.js"></script>
    <script src="../../ajax/libs/c3/0.4.11/c3.min.js"></script>
    <script src="../../v3/components/datatables/media/js/jquery.dataTables.js"></script>
    <script src="../../v3/components/google-code-prettify/bin/prettify.min.js"></script>
    <script src="../../v3/components/clipboard/dist/clipboard.min.js"></script>
    <script src="../../v3/components/patternfly/dist/js/patternfly.min.js"></script>
    <script src="../../v3/components/patternfly/dist/js/patternfly.dataTables.pfFilter.min.js"></script>
    <script src="../../v3/assets/js/patternfly-site.min.js"></script>
    <%--</head>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMasterMain" runat="server">
    <div id="pf-list-simple-expansion" class="list-group list-view-pf list-view-pf-view">
        <div class="list-group-item">
            <div class="list-group-item-header">
                <div class="list-view-pf-expand">
                    <span class="fa fa-angle-right"></span>
                </div>
                <div class="list-view-pf-checkbox">
                    <input type="checkbox">
                </div>
                <div class="list-view-pf-actions">
                    <button class="btn btn-default">Action</button>
                    <div class="dropdown pull-right dropdown-kebab-pf">
                        <button class="btn btn-link dropdown-toggle" type="button" id="dropdownKebabRight9" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <span class="fa fa-ellipsis-v"></span>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownKebabRight9">
                            <li><a href="#">Action</a></li>
                            <li><a href="#">Another Action</a></li>
                            <li><a href="#">Something Else Here</a></li>
                            <li role="separator" class="divider"></li>
                            <li><a href="#">Separated Link</a></li>
                        </ul>
                    </div>

                </div>
                <div class="list-view-pf-main-info">
                    <div class="list-view-pf-left">
                        <span class="fa fa-plane list-view-pf-icon-sm"></span>
                    </div>
                    <div class="list-view-pf-body">
                        <div class="list-view-pf-description">
                            <div class="list-group-item-heading">
                                Event One
           
                            </div>
                            <div class="list-group-item-text">
                                The following snippet of text is <a href="#">rendered as link text</a>.
           
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div class="list-group-item-container container-fluid hidden">
                <div class="close">
                    <span class="pficon pficon-close"></span>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <div id="utilizationDonutChart11" class="example-donut-chart-utilization"></div>

                        <div class="pct-donut-chart-pf example-donut-chart-utilization">
                            <div class="pct-donut-chart-pf-chart">
                                <div id="utilizationDonutChart12"></div>
                            </div>
                            <%--<span class="pct-donut-chart-pf-label">
    60 MHz of 100 MHz used
  </span>--%>
                        </div>


                        <script>
                            var c3ChartDefaults = $().c3ChartDefaults();

                            // Donut configuration 1
                            var donutConfig1 = c3ChartDefaults.getDefaultDonutConfig('A');

                            donutConfig1.bindto = '#utilizationDonutChart11';
                            donutConfig1.data = {
                                type: "donut",
                                columns: [
                                    ["Used", 60],
                                    ["Available", 40],
                                ],
                                groups: [
                                    ["used", "available"]
                                ],
                                order: null
                            };
                            donutConfig1.size = {
                                width: 180,
                                height: 180
                            };

                            donutConfig1.tooltip = {
                                contents: $().pfGetUtilizationDonutTooltipContentsFn('MHz')
                            };

                            c3.generate(donutConfig1);
                            $().pfSetDonutChartTitle("#utilizationDonutChart11", "60", "MHz Used");

                            // Donut configuration 2
                            var donutConfig2 = c3ChartDefaults.getDefaultDonutConfig('A');

                            donutConfig2.bindto = '#utilizationDonutChart12';
                            donutConfig2.data = {
                                type: "donut",
                                columns: [
                                    ["Used", 60],
                                    ["Available", 40]
                                ],
                                groups: [
                                    ["used", "available"]
                                ],
                                order: null
                            };
                            donutConfig2.size = {
                                width: 180,
                                height: 180
                            };

                            donutConfig2.tooltip = {
                                contents: $().pfGetUtilizationDonutTooltipContentsFn('MHz')
                            };

                            c3.generate(donutConfig2);
                            $().pfSetDonutChartTitle("#utilizationDonutChart12", "60", "MHz Used");

                            // Donut configuration 3
                            var donutConfig3 = c3ChartDefaults.getDefaultDonutConfig('A');

                            donutConfig3.bindto = '#utilizationDonutChart13';
                            donutConfig3.data = {
                                type: "donut",
                                columns: [
                                    ["Used", 60],
                                    ["Available", 40]
                                ],
                                groups: [
                                    ["used", "available"]
                                ],
                                order: null
                            };
                            donutConfig3.size = {
                                width: 140,
                                height: 140
                            };

                            donutConfig3.tooltip = {
                                contents: $().pfGetUtilizationDonutTooltipContentsFn('MHz')
                            };

                            c3.generate(donutConfig3);
                            $().pfSetDonutChartTitle("#utilizationDonutChart13", "60", "MHz Used");

                            // Donut configuration 4
                            var donutConfig4 = c3ChartDefaults.getDefaultDonutConfig('A');

                            donutConfig4.bindto = '#utilizationDonutChart14';
                            donutConfig4.data = {
                                type: "donut",
                                columns: [
                                    ["Used", 60],
                                    ["Available", 40]
                                ],
                                groups: [
                                    ["used", "available"]
                                ],
                                order: null
                            };
                            donutConfig4.size = {
                                width: 140,
                                height: 140
                            };

                            donutConfig4.tooltip = {
                                contents: $().pfGetUtilizationDonutTooltipContentsFn('MHz')
                            };

                            c3.generate(donutConfig4);
                            $().pfSetDonutChartTitle("#utilizationDonutChart14", "60", "MHz Used");
</script>

                    </div>
                    <div class="col-md-9">
                        <dl class="dl-horizontal">
                            <dt>Host Name</dt>
                            <dd>Hostceph1</dd>
                            <dt>Device Path</dt>
                            <dd>/dev/disk/pci-0000.05:00-sas-0.2-part1</dd>
                            <dt>Time</dt>
                            <dd>January 15, 2016 10:45:11 AM</dd>
                            <dt>Severity</dt>
                            <dd>Warning</dd>
                            <dt>Cluster</dt>
                            <dd>Cluster 1</dd>
                        </dl>
                        <p>
                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod
            tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
            quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo
            consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse
            cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non
            proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
         
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            // Row Checkbox Selection
            $("#pf-list-simple-expansion input[type='checkbox']").change(function (e) {
                if ($(this).is(":checked")) {
                    $(this).closest('.list-group-item').addClass("active");
                } else {
                    $(this).closest('.list-group-item').removeClass("active");
                }
            });
            // toggle dropdown menu
            $("#pf-list-simple-expansion .list-view-pf-actions").on('show.bs.dropdown', function () {
                var $this = $(this);
                var $dropdown = $this.find('.dropdown');
                var space = $(window).height() - $dropdown[0].getBoundingClientRect().top - $this.find('.dropdown-menu').outerHeight(true);
                $dropdown.toggleClass('dropup', space < 10);
            });

            // click the list-view heading then expand a row
            $("#pf-list-simple-expansion .list-group-item-header").click(function (event) {
                if (!$(event.target).is("button, a, input, .fa-ellipsis-v")) {
                    $(this).find(".fa-angle-right").toggleClass("fa-angle-down")
                        .end().parent().toggleClass("list-view-pf-expand-active")
                        .find(".list-group-item-container").toggleClass("hidden");
                } else {
                }
            })

            // click the close button, hide the expand row and remove the active status
            $("#pf-list-simple-expansion .list-group-item-container .close").on("click", function () {
                $(this).parent().addClass("hidden")
                    .parent().removeClass("list-view-pf-expand-active")
                    .find(".fa-angle-right").removeClass("fa-angle-down");
            })

        });
</script>
</asp:Content>
