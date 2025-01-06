using AppLogger;
using BussinessAccessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOR.Pages.Dashboard
{
    public partial class AdvanceDashboad : System.Web.UI.Page
    {
        #region Object Declaration
        AdvanceDashbordDAL _DAL = new AdvanceDashbordDAL();
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
                            LoadDashBordData();
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
                ErrorLog.DashboardTrace("AdvanceDashboard: Page_Load(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
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

        private void LoadDashBordData()
        {
            try
            {
                var channeldata = _DAL.GetChannelWiseData();

                if (channeldata.Tables.Contains("ChannelData"))
                {
                    var txn = channeldata.Tables["ChannelData"].AsEnumerable().Select(t => new
                    {
                        timeperiod = t.Field<string>("time_period"),
                        channel = t.Field<string>("channel"),
                        totalcount = t.Field<long>("totalcount"),
                        successcount = t.Field<long>("successcount"),
                        failurecount = t.Field<long>("failurecount"),
                        successrate = t.Field<double>("success_rate"),
                        failurerate = t.Field<double>("failure_rate")
                    }).ToList();

                    var d = JsonConvert.SerializeObject(txn);
                    ViewState["ChannelData"] = d;
                }

                var AgentOnboarddata = _DAL.GetAgentOnboardingData();

                if (AgentOnboarddata.Tables.Contains("AgentOnbordingData"))
                {
                    var txn = AgentOnboarddata.Tables["AgentOnbordingData"].AsEnumerable().Select(t => new
                    {
                        timeperiod = t.Field<string>("time_period"),
                        currentcount = t.Field<int>("current_count"),
                        previouscount = t.Field<int>("previous_count")

                    }).ToList();

                    var d = JsonConvert.SerializeObject(txn);
                    ViewState["AgentOnboardingData"] = d;
                }

                var Data = _DAL.GetRuleWiseData();

                /* DataTable ruleDataTable = new DataTable("RuleData");
                 ruleDataTable.Columns.Add("groupid", typeof(int));
                 ruleDataTable.Columns.Add("ruleid", typeof(int));
                 ruleDataTable.Columns.Add("rulename", typeof(string));
                 ruleDataTable.Columns.Add("groupname", typeof(string));
                 ruleDataTable.Columns.Add("totalcount", typeof(int));
                 ruleDataTable.Columns.Add("successcount", typeof(int));
                 ruleDataTable.Columns.Add("failurecount", typeof(int));
                 ruleDataTable.Columns.Add("overallperc", typeof(double));
                 ruleDataTable.Columns.Add("successrate", typeof(double));
                 ruleDataTable.Columns.Add("failurerate", typeof(double));
                 ruleDataTable.Rows.Add(10, 21, "BC_003", "BC_Wise", 616090, 305292, 301983, 30.57, 49.55, 49.02);
                 ruleDataTable.Rows.Add(10, 23, "BC_005", "BC_Wise", 131096, 64638, 64482, 6.5, 49.31, 49.19);
                 ruleDataTable.Rows.Add(10, 22, "BC_004", "BC_Wise", 116510, 57368, 57238, 5.78, 49.24, 49.13);
                 ruleDataTable.Rows.Add(2, 4, "Bal_Enquiry_55%", "Percentage_Wise", 46223, 170, 46023, 2.29, 0.43, 99.49);
                 ruleDataTable.Rows.Add(2, 5, "Mini_Statement_45%", "Percentage_Wise", 42505, 162, 42325, 2.11, 0.46, 99.49);
                 DataSet dataSet = new DataSet();
                 dataSet.Tables.Add(ruleDataTable);    */


                if (Data.Tables.Contains("RuleData"))
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (DataRow row in Data.Tables["RuleData"].Rows)
                    {
                        var groupID = HttpUtility.JavaScriptStringEncode(row["GroupID"].ToString());
                        var groupName = HttpUtility.JavaScriptStringEncode(row["GroupName"].ToString());
                        var ruleID = HttpUtility.JavaScriptStringEncode(row["ruleid"].ToString());
                        var ruleName = HttpUtility.JavaScriptStringEncode(row["RuleName"].ToString());
                        int totalTransactions = row["TotalCount"] != null && int.TryParse(row["TotalCount"].ToString(), out int r) ? r : 0;
                        var successCount = row["SuccessCount"] != null && int.TryParse(row["SuccessCount"].ToString(), out int s) ? s : 0;
                        var failureCount = row["FailureCount"] != null && int.TryParse(row["FailureCount"].ToString(), out int f) ? f : 0;
                        var successRate = row["successrate"];
                        var failureRate = row["failurerate"];
                        var RoutingPercentage = row["OverallPerc"];

                        var uniqueId = $"{groupName}{ruleName}_Chart";
                        sb.Append("<div class='blog-comments__item d-flex p-2'>");
                        sb.Append("<div class='d-flex row w-100'>");
                        // Left Section
                        sb.Append("<div class='col-12 col-sm-6 col-md-6 col-lg-6'>");
                        sb.Append($"<h6 class='go-stats__label mb-1 text-muted' style='font-size: 0.875rem;'>Rule : {ruleName}</h6>");
                        sb.Append($"<div class='go-stats__meta mb-1' style='font-size: 0.75rem; font-weight: 400;'>");
                        sb.Append($"<span class='mr-1'>Total Transactions: <strong>{FormatCount(totalTransactions)}</strong></span>");
                        sb.Append("</div>");
                        sb.Append($"<div class='go-stats__meta mb-1' style='font-size: 0.75rem; font-weight: 400;'>");
                        sb.Append($"<span class='d-block d-sm-inline'>Success: <strong class='text-success'>{successRate}%</strong></span>");
                        sb.Append("</div>");
                        sb.Append($"<div class='go-stats__meta' style='font-size: 0.75rem; font-weight: 400;'>");
                        sb.Append($"<span class='d-block d-sm-inline'>Failure: <strong class='text-danger'>{failureRate}%</strong></span>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        // Middle Section
                        sb.Append("<div class='col-12 col-sm-6 col-md-6 col-lg-2'>");
                        sb.Append($"<div class='go-stats__meta mb-1' style='font-size: 0.75rem; font-weight: 400;'>");
                        sb.Append($"<span class='mr-1'>Routing: <strong>{RoutingPercentage}%</strong></span>");
                        sb.Append("</div>");
                        sb.Append($"<div class='go-stats__meta' style='font-size: 0.75rem; font-weight: 400;'>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        // Right Section
                        sb.Append("<div class='col-12 col-sm-6 col-md-6 col-lg-4'>");
                        sb.Append("<div class='d-flex justify-content-end'>");
                        sb.Append("<div class='go-stats__chart d-flex ml-auto'>");
                        sb.Append($"<canvas id='{uniqueId}' class='my-auto' width='80' height='80' style='width: 80px; height: 80px; display: block;'></canvas>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append($@"
                         <script>
                             document.addEventListener('DOMContentLoaded', function() {{
                                 generateDonutChart('{uniqueId}', {successRate},{failureRate});
                             }});
                         </script>");
                        Rule.Text = sb.ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("AdvanceDashboard: LoadDashBordData(): Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

    }
}