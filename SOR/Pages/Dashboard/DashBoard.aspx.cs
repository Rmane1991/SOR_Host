using AppLogger;
using BussinessAccessLayer;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls; 
using System.Web.UI.WebControls;

namespace SOR.Pages.Dashboard
{
    public partial class DashBoard : System.Web.UI.Page
    {
        #region Object Declaration
        DashBoardDAL _DashDAL = new DashBoardDAL();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "DashBoard.aspx", "1");
                    if (!HasPagePermission)
                    {
                        try
                        {
                            Response.Redirect(ConfigurationManager.AppSettings["RedirectTo404"].ToString(), false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        catch (ThreadAbortException)
                        {
                        }
                    }
                    else
                    {
                        if (!IsPostBack && HasPagePermission)
                        {
                            LoadOnBoardCounts();
                            LoadTransactionCounts();
                        }

                    }
                }
                else
                {
                    try
                    {
                        Response.Redirect(ConfigurationManager.AppSettings["RedirectToLogin"].ToString(), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    catch (ThreadAbortException)
                    {
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("DashBoard: Page_Load(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        #region Method
        private void LoadTransactionCounts()
        {

            try
            {
                /* var transactions = _DashDAL.GetMonthlyTxnDataCount();

                 var formattedTransactions = transactions.Select(t => new
                 {
                     Day = t.Day,
                     CurrentMonthCount = t.CurrentMonthCount,
                     PreviousMonthCount = t.PreviousMonthCount
                 }).ToList();

                 var serializedTransactions = JsonConvert.SerializeObject(formattedTransactions);
                 ViewState["Transactions"] = serializedTransactions;*/

                var summary = _DashDAL.GetMonthlySummaryCount();
                var summaryData = summary.Select(t => new
                {
                    BC = t.BC,
                    Switch = t.Switch,
                    Rule = t.Rule
                }).ToList();

                var SerializeSummaryData = JsonConvert.SerializeObject(summaryData);
                ViewState["summary"] = SerializeSummaryData;

                var txnDetails = _DashDAL.Get_AllData();

                try
                {
                    if (txnDetails.Tables.Contains("Txnsummarychart"))
                    {
                        var txn = txnDetails.Tables["Txnsummarychart"].AsEnumerable().Select(t => new
                        {
                            Day = t.Field<string>("time_period"),
                            CurrentMonthCount = t.Field<int>("current_count"),
                            PreviousDay = t.Field<string>("previous_time_period"),
                            PreviousMonthCount = t.Field<int>("previous_count")
                        }).ToList();

                        var TxnSummary = JsonConvert.SerializeObject(txn);
                        ViewState["Transactions"] = TxnSummary;
                    }
                }

                catch(Exception ex)
                {
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Exception: {ex.Message}");
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Stack Trace: {ex.StackTrace}");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }

                try
                {
                    if (txnDetails.Tables.Contains("Aggregators"))
                    {
                        StringBuilder sb = new StringBuilder();
                        bool isFirstCard = true;
                        foreach (DataRow row in txnDetails.Tables["Aggregators"].Rows)
                        {
                            var aggregatorName = row["AggreName"].ToString();
                            var transactionCount = row["Count"].ToString();
                            var sucessCount = row["successCount"].ToString();
                            var failCount = row["failureCount"].ToString();
                            decimal successRate = Convert.ToDecimal(row["successRate"]);
                            decimal failureRate = Convert.ToDecimal(row["failureRate"]);
                            var changePercentage = Convert.ToDecimal(row["ChangePercentage"]);

                            decimal avgAmount = Convert.ToDecimal(row["AvgAmount"]);
                            decimal totalRevenue = Convert.ToDecimal(row["TotalRevenue"]);

                            int progressWidth = (int)Math.Min(Math.Abs(changePercentage), 100);
                            string progressClass = "bg-success";

                            if (changePercentage < 0)
                            {
                                progressClass = "bg-danger";
                            }
                            else if (changePercentage == 0)
                            {
                                progressClass = "bg-warning";
                            }
                            sb.Append($@"<div class='carousel-item {(isFirstCard ? "active" : "")}'>
                        <div class='aggregator-card'>
                        <div class='card-header'><h6>{aggregatorName}</h6>
                        <span class='badge {progressClass}'>High Performer</span>
                        </div>
                        <div class='card-body'>
                        <div class='row'>
                        <div class='col-md-7'>
                        <div class='d-flex justify-content-between mb-3'>
                        <span><strong>Transactions:</strong> {transactionCount}</span>
                        </div>
                        <div class='d-flex justify-content-between mb-3'>
                        <span><strong>Success:</strong> {sucessCount}</span>
                        </div>
                        <div class='d-flex justify-content-between mb-3'>
                        <span><strong>Failure:</strong> {failCount}</span>
                        </div>
                        <div class='d-flex justify-content-between mb-3'>
                        <span><strong>Avg. Amount:</strong> ₹{avgAmount:N2}</span>
                        </div>
                        <div class='d-flex justify-content-between mb-3'>
                        <span><strong>Success Rate:</strong> {successRate:N2}%</span>
                        </div>
                        <div class='d-flex justify-content-between mb-3'>
                       <span><strong>Failure Rate:</strong> {failureRate:N2}%</span>
                        </div>
                        </div>
                        
                        <div class='col-md-5'>
                        <canvas id='chart_{aggregatorName}_{transactionCount}' class='chart-container' style='display: block; box-sizing: border-box; height: 101px; width: 124px;'></canvas>
                        </div>
                        </div>
                        <div class='row mb-6'>
                        <div class='col-12'>
                        <div class='d-flex justify-content-between mb-3'>
                        </div>
                        <div class='d-flex justify-content-between mb-3'>
                        <span><strong></strong></span>
                        <span><strong></strong></span>
                        </div>
                        <div class='d-flex justify-content-between mb-3'>
                        <span><strong>Change:</strong> {changePercentage:0.##} % (vs. last week)</span>
                        </div>
                        <div progress mt-3' class='progress' style='height: 8px;'>
                        <div class='progress-bar {progressClass}' role='progressbar' style='width: {progressWidth}%' aria-valuenow='{progressWidth}' aria-valuemin='0' aria-valuemax='100'></div>
                        </div>
                        </div>
                        </div>
                        </div>
                        </div>
                        </div>
                      <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
                      <script src='https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels'></script>
                     <script>
                     var ctx = document.getElementById('chart_{aggregatorName}_{transactionCount}').getContext('2d');
                     var chart = new Chart(ctx, {{
                         type: 'doughnut',
                         data: {{
                             labels: ['Sucess %', 'Failure %'], 
                             datasets: [{{
                                 label: 'Transactions',
                                 data: [{successRate:N2}, {failureRate:N2}], 
                                 backgroundColor: ['#2196f3', '#ff4560'], 
                                 borderWidth: 1
                             }}]
                         }},
                         options: {{
                             responsive: false,
                             maintainAspectRatio: false,  
                             cutoutPercentage: 70,  
                             plugins: {{
                                 legend: {{
                                     display: false, 
                                 }},
                                 tooltip: {{
                                     enabled: true, 
                                     callbacks: {{
                                         label: function(tooltipItem) {{
                                             var label = tooltipItem.label;
                                             var value = tooltipItem.raw;
                                             if (label === 'Sucess %') {{
                                                 return 'Success Rate: ' + value + '%';
                                             }} else if (label === 'Failure %') {{
                                                 return 'Failure Rate: ' + value + '%';
                                             }}
                                         }}
                                     }},
                                     position: 'nearest',  
                                     caretSize: 12,  
                                     xAlign: 'left',  
                                     yAlign: 'left',  
                                     padding: 10, 
                                     backgroundColor: 'rgba(0, 0, 0, 0.7)', 
                                     titleFont: {{
                                         size: 14,
                                         weight: 'bold',
                                     }},
                                     bodyFont: {{
                                         size: 12
                                     }},
                                     caretPadding: 50
                                 }},
                                 datalabels: {{
                                     display: true, 
                                     formatter: function(value, ctx) {{
                                         return value + ' transactions';  
                                     }},
                                     color: '#fff',  
                                     font: {{
                                         weight: 'bold',
                                         size: 14,
                                     }},
                                     padding: 10,  
                                 }}
                             }}
                         }}
                     }});
                      </script>");
                            isFirstCard = false;
                        }
                        Aggregator.Text = sb.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Exception: {ex.Message}");
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Stack Trace: {ex.StackTrace}");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
                try
                {
                    if (txnDetails.Tables.Contains("SwitchData"))
                    {
                        var txn = txnDetails.Tables["SwitchData"].AsEnumerable().Select(t => new
                        {
                            SwitchName = t.Field<string>("switchname"),
                            Counts = t.Field<int>("Count"),
                            Percent = t.Field<decimal>("percent")
                        }).ToList();

                        var serializedTxn = JsonConvert.SerializeObject(txn);
                        ViewState["SwitchMonthlyData"] = serializedTxn;

                        StringBuilder sb = new StringBuilder();
                        foreach (DataRow row in txnDetails.Tables["SwitchData"].Rows)
                        {
                            var switchId = row["switchid"];
                            var SwitchName = row["switchname"];
                            var count = row["Count"];

                            sb.Append("<tr>");
                            sb.Append($"<td class='text-right'>{SwitchName}</td>");
                            sb.Append($"<td class='text-right' style='text-align: right !important;'>{count}</td>");
                            sb.Append("</tr>");
                        }
                        SwitchLteralControl.Text = sb.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Exception: {ex.Message}");
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Stack Trace: {ex.StackTrace}");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
                try
                {
                    if (txnDetails.Tables.Contains("BankRevenueData"))
                    {
                        var txn = txnDetails.Tables["BankRevenueData"].AsEnumerable().Select(t => new
                        {
                            TotalRevenue = t.Field<decimal>("TotalRevenue"),
                            ConversionRate = t.Field<decimal>("ConversionRt"),
                            PeriodName = t.Field<string>("PeriodName")
                        }).ToList();

                        var serializedTxn = JsonConvert.SerializeObject(txn);
                        ViewState["BankRevenueData"] = serializedTxn;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Exception: {ex.Message}");
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Stack Trace: {ex.StackTrace}");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
                try
                {
                    if (txnDetails.Tables.Contains("RuleData"))
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (DataRow row in txnDetails.Tables["RuleData"].Rows)
                        {
                            var groupID = HttpUtility.JavaScriptStringEncode(row["GroupID"].ToString());
                            var groupName = HttpUtility.JavaScriptStringEncode(row["GroupName"].ToString());
                            var ruleID = HttpUtility.JavaScriptStringEncode(row["ruleid"].ToString());
                            var ruleName = HttpUtility.JavaScriptStringEncode(row["RuleName"].ToString());
                            int totalTransactions = row["TotalCount"] != null && int.TryParse(row["TotalCount"].ToString(), out int r) ? r : 0;
                            var successCount = row["SuccessCount"] != null && int.TryParse(row["SuccessCount"].ToString(), out int s) ? s : 0;
                            var failureCount = row["FailureCount"] != null && int.TryParse(row["FailureCount"].ToString(), out int f) ? f : 0;
                            var RoutingPercentage = row["OverallPerc"];
                            var uniqueId = $"{groupName}{ruleName}_Chart";

                            sb.Append("<tr>");
                            sb.Append($"<td class='text-right'>{groupName} | {ruleName}</td>");
                            sb.Append($"<td class='text-right'>{totalTransactions}</td>");
                            sb.Append($"<td class='text-right'>Routing% {RoutingPercentage} </td>");
                            sb.Append($"<td class='text-right'>");
                            sb.Append($"<canvas class='my-auto' id='{uniqueId}' width='40' height='40' style='width: 40px; height: 40px;'></canvas>");
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append($@"<script>
                        document.addEventListener('DOMContentLoaded', function() {{
                        const ctx = document.getElementById('{uniqueId}').getContext('2d');
                        const data = {{
                            datasets: [{{
                                label: '{ruleName}', 
                                data: [{successCount}, {failureCount}], 
                                backgroundColor:{totalTransactions} > 0 ? [
                                    'rgba(54, 162, 235, 0.8)', 
                                    'rgba(255, 99, 132, 0.8)'  
                                ]:['rgba(144,238,144,0.5)'],
                                borderWidth: 1 
                            }}]
                        }};
                         
                        const chart = new Chart(ctx, {{
                            type: 'doughnut',
                            data: data,
                            options: {{
                                responsive: true,
                                maintainAspectRatio: false,
                                cutoutPercentage: 50,
                                plugins: {{
                                    tooltip: {{
                                        enabled: true
                                    }},
                                    legend: {{
                                        display: true
                                    }}
                                }}
                            }}
                        }});
                    }});
                    </script>
                    ");
                        }
                        ruleLiteralControl.Text = sb.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Exception: {ex.Message}");
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Stack Trace: {ex.StackTrace}");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
                try
                {
                    if (txnDetails.Tables.Contains("MonthlyBCData"))
                    {
                        var txn = txnDetails.Tables["MonthlyBCData"].AsEnumerable().Select(t => new
                        {
                            Month = t.Field<int>("month"),
                            Year = t.Field<int>("year"),
                            bcname = t.Field<string>("bcname"),
                            Success = t.Field<int>("currentsuccess"),
                            Failure = t.Field<int>("currentfailure"),
                            SuccessRate = t.Field<double>("successrate").ToString("F2"),
                            FailureRate = t.Field<double>("failrate").ToString("F2")
                        }).ToList();

                        var serializedTxn = JsonConvert.SerializeObject(txn);
                        ViewState["BcMonthlyData"] = serializedTxn;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Exception: {ex.Message}");
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Stack Trace: {ex.StackTrace}");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
                try
                {
                    if (txnDetails.Tables.Contains("ChannelwiseData"))
                    {
                        var txn = txnDetails.Tables["ChannelwiseData"].AsEnumerable().Select(t => new
                        {
                            channel = t.Field<string>("channel"),
                            totalcount = t.Field<long>("totalcount"),
                            successcount = t.Field<long>("successcount")
                        }).ToList();

                        var serializedTxn = JsonConvert.SerializeObject(txn);
                        ViewState["ChannelwiseData"] = serializedTxn;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Exception: {ex.Message}");
                    ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Stack Trace: {ex.StackTrace}");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("DashBoard: LoadTransactionCounts(): Exception: " + ex.Message);
                ErrorLog.DashboardTrace($"DashBoard: LoadTransactionCounts(): Stack Trace: {ex.StackTrace}");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        private void LoadOnBoardCounts()
        {
            try
            {
                var OnBoardedData = _DashDAL.GetOnBoardingData();

                var controls = new Dictionary<string, HtmlGenericControl>{
            { "TRANSACTION", TxnCount },
            { "BC", bcCount },
            { "AGGREGATOR", aggregatorCount },
            { "AGENT", agentCount },
            { "CUSTOMER", customerCount }};

                foreach (var data in OnBoardedData)
                {
                    if (controls.ContainsKey(data.Name))
                    {
                        controls[data.Name].InnerText = FormatCount(data.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("DashBoard: LoadOnBoardCounts(): Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        private string FormatCount(long count)
        {
            try
            {
                if (count >= 1_000_000_000) // Billion
                    return (count / 1_000_000_000.0).ToString("0.0") + "B";
                else if (count >= 1_000_000) // Million
                    return (count / 1_000_000.0).ToString("0.0") + "M";
                else if (count >= 1_000) // Thousand
                    return (count / 1_000.0).ToString("0.0") + "K";
                else
                    return count.ToString();
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("DashBoard: FormatCount(): Exception: " + ex.Message);
            }
            return count.ToString();
        }
        #endregion
        
        [WebMethod]
        public static string ChartdataFilter(string fromDate, string toDate, string FilterType, string Filter)
        {
            DashBoardDAL _DashDAL = new DashBoardDAL();
            DataSet _ds = null;
            var result = new Dictionary<string, object>();

            try
            {
                _ds = _DashDAL.FilterChartDate(fromDate, toDate, FilterType, Filter);

                if (_ds.Tables.Contains("Txnsummarychart"))
                {
                    var txnSummary = _ds.Tables["Txnsummarychart"].AsEnumerable().Select(t => new
                    {
                        Day = t.Field<string>("time_period"),
                        TxnsummaryCurrentMonthCount = t.Field<int>("current_count"),
                        PreviousDay = t.Field<string>("previous_time_period"),
                        TxnsummaryPreviousMonthCount = t.Field<int>("previous_count")
                    }).ToList();

                    result["TxnSummaryChart"] = txnSummary;
                }

                if (_ds.Tables.Contains("bcsummarychart"))
                {
                    var bcSummary = _ds.Tables["bcsummarychart"].AsEnumerable().Select(t => new
                    {
                        Month = t.Field<int>("month"),
                        Year = t.Field<int>("year"),
                        bcname = t.Field<string>("bcname"),
                        Success = t.Field<int>("currentsuccess"),
                        Failure = t.Field<int>("currentfailure"),
                        SuccessRate = t.Field<double>("successrate").ToString("F2"),
                        FailureRate = t.Field<double>("failrate").ToString("F2")
                    }).ToList();

                    result["BCSummaryChart"] = bcSummary;
                }

                if (_ds.Tables.Contains("switchchart"))
                {
                    var switchsummary = _ds.Tables["switchchart"].AsEnumerable().Select(t => new
                    {
                        SwitchName = t.Field<string>("switchname"),
                        Counts = t.Field<int>("Count"),
                    }).ToList();

                    result["SwitchChart"] = switchsummary;
                }

                if (_ds.Tables.Contains("Revenuechart"))
                {
                    var RevenueSummary = _ds.Tables["Revenuechart"].AsEnumerable().Select(t => new
                    {
                        TotalRevenue = t.Field<decimal>("TotalRevenue") == 0 ? (decimal?)null : t.Field<decimal>("TotalRevenue"),
                        ConversionRate = t.Field<decimal>("ConversionRt") == 0 ? (decimal?)null : t.Field<decimal>("ConversionRt"),
                        PeriodName = t.Field<string>("PeriodName")
                    }).ToList();
                    result["RevenueChart"] = RevenueSummary;
                }


                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("DashBoard: ChartdataFilter(): Exception: " + ex.Message);
            }

            return JsonConvert.SerializeObject(new Dictionary<string, object>());
        }


        [WebMethod]
        public static string GetDataByDatePeriod(string Type, string filterType)
        {

            DashBoardDAL _DashDAL = new DashBoardDAL();
            DataSet txnDetails = null;
            StringBuilder sb = new StringBuilder();
            var data = "";
            try
            {
                switch (Type)
                {
                    case "ddlSwitchFilter":
                        txnDetails = _DashDAL.FilterData(Type, filterType);

                        var txn = txnDetails.Tables["SwitchData"].AsEnumerable().Select(t => new
                        {
                            SwitchName = t.Field<string>("switchname"),
                            Counts = t.Field<int>("Count"),
                            Percent = t.Field<decimal>("percent")
                        }).ToList();
                        data = JsonConvert.SerializeObject(txn);

                        break;
                    case "ddlAggreFilter":
                        txnDetails = _DashDAL.FilterData(Type, filterType);

                        foreach (DataRow row in txnDetails.Tables[0].Rows)
                        {
                            var aggregators = row["AggreName"];
                            var count = row["Count"];
                            sb.Append("<tr>");
                            sb.Append($"<td class='text-left'>{aggregators}</td>");
                            sb.Append($"<td class='text-right' style='text-align: right !important;'>{count}</td>");
                            sb.Append("</tr>");

                        }
                        data = sb.ToString();

                        break;
                    case "ddlRuleFilter":
                        txnDetails = _DashDAL.FilterData(Type, filterType);

                        foreach (DataRow row in txnDetails.Tables["RuleData"].Rows)
                        {
                            var groupID = HttpUtility.JavaScriptStringEncode(row["GroupID"].ToString());
                            var groupName = HttpUtility.JavaScriptStringEncode(row["GroupName"].ToString());
                            var ruleID = HttpUtility.JavaScriptStringEncode(row["ruleid"].ToString());
                            var ruleName = HttpUtility.JavaScriptStringEncode(row["RuleName"].ToString());
                            int totalTransactions = row["TotalCount"] != null && int.TryParse(row["TotalCount"].ToString(), out int r) ? r : 0;
                            var successCount = row["SuccessCount"] != null && int.TryParse(row["SuccessCount"].ToString(), out int s) ? s : 0;
                            var failureCount = row["FailureCount"] != null && int.TryParse(row["FailureCount"].ToString(), out int f) ? f : 0;
                            var RoutingPercentage = row["OverallPerc"];
                            var uniqueId = $"{groupName}{ruleName}_Chart";
                            sb.Append("<tr>");
                            sb.Append($"<td class='text-right'>{groupName} | {ruleName}</td>");
                            sb.Append($"<input type='hidden' name='Success' value='{successCount}' id='hiddnSuccess'>");
                            sb.Append($"<input type='hidden' name='Fail' value='{failureCount}' id='hiddnFail'>");
                            sb.Append($"<td class='text-right'>{totalTransactions}</td>");
                            sb.Append($"<td class='text-right'>Routing% {RoutingPercentage} </td>");
                            sb.Append("<td class='text-right'>");
                            sb.Append($"<canvas class='my-auto' id='{uniqueId}' width='40' height='40' style='width: 40px; height: 40px;'></canvas>");
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append($@"
                    <script>
                    document.addEventListener('DOMContentLoaded', function() {{
                        const ctx = document.getElementById('{uniqueId}').getContext('2d');
                        const data = {{
                            datasets: [{{
                                label: '{ruleName}', 
                                data: [{successCount}, {failureCount}], 
                                backgroundColor: [
                                    'rgba(54, 162, 235, 0.8)', 
                                    'rgba(255, 99, 132, 0.8)'  
                                ],
                                borderWidth: 1 
                            }}]
                        }};
                    
                        const chart = new Chart(ctx, {{
                            type: 'doughnut',
                            data: data,
                            options: {{
                                responsive: true,
                                maintainAspectRatio: false,
                                cutoutPercentage: 50,
                                plugins: {{
                                    tooltip: {{
                                        enabled: true
                                    }},
                                    legend: {{
                                        display: true
                                    }}
                                }}
                            }}
                        }});
                    }});
                    </script>
                    ");
                        }
                        data = sb.ToString();

                        break;
                    case "ddlTopSwitchsFilter":
                        txnDetails = _DashDAL.FilterData(Type, filterType);

                        foreach (DataRow row in txnDetails.Tables["SwitchData"].Rows)
                        {
                            var switchId = row["switchid"];
                            var SwitchName = row["switchname"];
                            var count = row["Count"];
                            sb.Append("<tr>");
                            sb.Append($"<td class='text-right'>{SwitchName}</td>");
                            sb.Append($"<td class='text-right' style='text-align: right !important;'>{count}</td>");
                            sb.Append("</tr>");
                        }
                        data = sb.ToString();
                        break;

                    case "ddlChannelFilter":
                        txnDetails = _DashDAL.FilterData(Type, filterType);

                        foreach (DataRow row in txnDetails.Tables["SwitchData"].Rows)
                        {
                            var switchId = row["switchid"];
                            var SwitchName = row["switchname"];
                            var count = row["Count"];
                            sb.Append("<tr>");
                            sb.Append($"<td class='text-right'>{SwitchName}</td>");
                            sb.Append($"<td class='text-right' style='text-align: right !important;'>{count}</td>");
                            sb.Append("</tr>");
                        }
                        data = sb.ToString();
                        break;

                    default:

                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return data;
        }
    }
}