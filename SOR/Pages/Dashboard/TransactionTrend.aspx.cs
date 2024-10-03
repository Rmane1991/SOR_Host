using System;
using System.Web.UI;
using System.Data;
using AppLogger;
using BussinessAccessLayer;
using System.Threading;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Text.Json;


namespace SOR.Pages.Dashboard
{
    public partial class TransactionTrend : System.Web.UI.Page
    {
        #region Object Declaration
        DashBoardDAL _DashDAL = new DashBoardDAL();

        
        StringBuilder sbJScriptForBarChart = new StringBuilder();

        StringBuilder sbJScriptForBarChart1 = new StringBuilder();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "TransactionTrend.aspx", "1");
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
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Hi. Try again', 'Warning');", true);
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('Hi');", true);
                            txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            lblAEPS.Font.Bold = true;
                            lblMATM.Font.Bold = true;
                            lblTotal.Font.Bold = true;
                            lblAgent.Font.Bold = true;
                            PieChartData();
                            //BindChart();
                            //BindChart1();
                            //GetHomePageData();
                            //UserPermissions.RegisterStartupScriptForNavigationListActive("1", "1");
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
                ErrorLog.DashboardTrace("TransactionTrend: Page_Load(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion
        
        #region Page LoadGraphData
        public void BindChart()
        {
            //string stringJsDynamicHTML = null;
            try
            {
                _DashDAL.Fromdate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                _DashDAL.Todate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                //_DashDAL.Date = !string.IsNullOrEmpty(Date.Value) ? Convert.ToDateTime(Date.Value).ToString("yyyy-MM-dd") : null;
                DataSet ds = _DashDAL.GetMaximusHomePageData();

                ltScriptsNew.Text = GetGraphData(ds);

            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("TransactionTrend: BindChart(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            //return stringJsDynamicHTML;
        }
        public string GetGraphData(DataSet ds_Graph)
        {
            string stringJsDynamicHTML = null;
            try
            {
                sbJScriptForBarChart.Append(@"
                                <script>

                          Highcharts.chart('container', {
                                chart: {
                                type: 'spline'
                                    },
                            title: {
                                text: 'Channel wise Day Transaction Trend'
                            },

                                  xAxis: {
                            title: {
                                        text: 'Hrs.'
                                    },
                                categories: [");
                bool IsFirstElement = true;
                foreach (DataRow dRow in ds_Graph.Tables[0].Rows)
                {
                    if (IsFirstElement)
                    {
                        IsFirstElement = false;
                        sbJScriptForBarChart.Append("'" + dRow[0].ToString() + "'");
                    }
                    else
                    {
                        sbJScriptForBarChart.Append(",'" + dRow[0].ToString() + "'");
                    }
                }
                sbJScriptForBarChart.Append(@" ]
                            },

                            yAxis: {
                                title: {
                                    text: 'Transactions'
                                }
                            },

                             tooltip: {
                            crosshairs: true,
                            shared: true
                                    },

                            legend: {
                                layout: 'vertical',
                                align: 'right',
                                verticalAlign: 'middle'
                            },
                              plotOptions: {
                                     spline: {
                                      marker: {
                                   Symbol: 'Circle',
                                    radius: 4,
                                    lineColor: '#EE4E34',
                                    lineWidth: 1
                                         }
                                        }
                                       },
                              credits: {
                             enabled: false
                                  },

                            series: [{
                                name: 'AEPS',
                            marker: {
                                        symbol: 'square'
                                    },
                                data: [");
                IsFirstElement = true;
                foreach (DataRow dRow in ds_Graph.Tables[0].Rows)
                {
                    if (dRow[1].ToString() == "AEPS")
                    {
                        if (IsFirstElement)
                        {
                            IsFirstElement = false;
                            sbJScriptForBarChart.Append(dRow[3].ToString());
                        }
                        else
                        {
                            sbJScriptForBarChart.Append("," + dRow[3].ToString());
                        }
                    }
                }
                sbJScriptForBarChart.Append(@"]
                            }, {
                                name: 'MATM',
                            marker: {
                                        symbol: 'diamond'
                                    },
                                data: [");
                IsFirstElement = true;
                foreach (DataRow dRow in ds_Graph.Tables[0].Rows)
                {
                    if (dRow[2].ToString() == "MATM")
                    {
                        if (IsFirstElement)
                        {
                            IsFirstElement = false;
                            sbJScriptForBarChart.Append(dRow[4].ToString());
                        }
                        else
                        {
                            sbJScriptForBarChart.Append("," + dRow[4].ToString());
                        }
                    }
                }
                sbJScriptForBarChart.Append(@"]
                            }]
                });
           
                </script>");

                stringJsDynamicHTML = sbJScriptForBarChart.ToString();

            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("TransactionTrend: GetGraphData(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return stringJsDynamicHTML;
        }
        #endregion

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                BindChart();
                BindChart1();
                GetHomePageData();
            }

            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("TransactionTrend: btnSearch_ServerClick(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }




        public void BindChart1()
        {
            //string stringJsDynamicHTML = null;
            try
            {
                _DashDAL.Fromdate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                _DashDAL.Todate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                //_DashDAL.Date = !string.IsNullOrEmpty(Date.Value) ? Convert.ToDateTime(Date.Value).ToString("yyyy-MM-dd") : null;
                DataSet ds = _DashDAL.GetMaximusHomePageData1();

                Literal1.Text = GetGraphData1(ds);

            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("TransactionTrend: BindChart(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            //return stringJsDynamicHTML;
        }
        public string GetGraphData1(DataSet ds_Graph)
        {
            string stringJsDynamicHTML = null;
            try
            {
                sbJScriptForBarChart1.Append(@"
                                <script>

                          Highcharts.chart('container1', {
                                chart: {
                                type: 'spline'
                                    },
                            title: {
                                text: 'Channel wise Monthly Transaction Trend'
                            },

                                  xAxis: {
                            title: {
                                        text: 'Days.'
                                    },
                                categories: [");
                bool IsFirstElement = true;
                foreach (DataRow dRow in ds_Graph.Tables[0].Rows)
                {
                    if (IsFirstElement)
                    {
                        IsFirstElement = false;
                        sbJScriptForBarChart1.Append("'" + dRow[0].ToString() + "'");
                    }
                    else
                    {
                        sbJScriptForBarChart1.Append(",'" + dRow[0].ToString() + "'");
                    }
                }
                sbJScriptForBarChart1.Append(@" ]
                            },

                            yAxis: {
                                title: {
                                    text: 'Transactions'
                                }
                            },

                             tooltip: {
                            crosshairs: true,
                            shared: true
                                    },

                            legend: {
                                layout: 'vertical',
                                align: 'right',
                                verticalAlign: 'middle'
                            },
                              plotOptions: {
                                     spline: {
                                      marker: {
                                   Symbol: 'Circle',
                                    radius: 4,
                                    lineColor: '#EE4E34',
                                    lineWidth: 1
                                         }
                                        }
                                       },
                              credits: {
                             enabled: false
                                  },

                            series: [{
                                name: 'AEPS',
                            marker: {
                                        symbol: 'square'
                                    },
                                data: [");
                IsFirstElement = true;
                foreach (DataRow dRow in ds_Graph.Tables[0].Rows)
                {
                    if (dRow[1].ToString() == "AEPS")
                    {
                        if (IsFirstElement)
                        {
                            IsFirstElement = false;
                            sbJScriptForBarChart1.Append(dRow[3].ToString());
                        }
                        else
                        {
                            sbJScriptForBarChart1.Append("," + dRow[3].ToString());
                        }
                    }
                }
                sbJScriptForBarChart1.Append(@"]
                            }, {
                                name: 'MATM',
                            marker: {
                                        symbol: 'diamond'
                                    },
                                data: [");
                IsFirstElement = true;
                foreach (DataRow dRow in ds_Graph.Tables[0].Rows)
                {
                    if (dRow[2].ToString() == "MATM")
                    {
                        if (IsFirstElement)
                        {
                            IsFirstElement = false;
                            sbJScriptForBarChart1.Append(dRow[4].ToString());
                        }
                        else
                        {
                            sbJScriptForBarChart1.Append("," + dRow[4].ToString());
                        }
                    }
                }
                sbJScriptForBarChart1.Append(@"]
                            }]
                });
           
                </script>");

                stringJsDynamicHTML = sbJScriptForBarChart1.ToString();

            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("TransactionTrend: GetGraphData(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return stringJsDynamicHTML;
        }

        #region Method
        public void GetHomePageData()

        {
            try
            {
                DataSet dt = new DataSet();
                _DashDAL.Fromdate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                _DashDAL.Todate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                //_DashDAL.Date = !string.IsNullOrEmpty(Date.Value) ? Convert.ToDateTime(Date.Value).ToString("yyyy-MM-dd") : null;
                _DashDAL.UserName = Session["Username"].ToString();
                _DashDAL.UserRoleId = Session["UserRoleID"].ToString();
                dt = _DashDAL.GetMaximusHomePageCount();
                if (dt.Tables[0].Rows.Count > 0)
                {
                    lblAEPS.Text = Convert.ToDecimal(dt.Tables[2].Rows[0]["Total"]).ToString();
                    lblMATM.Text = Convert.ToDecimal(dt.Tables[3].Rows[0]["Total"]).ToString();
                    lblTotal.Text = Convert.ToDecimal(dt.Tables[1].Rows[0]["Total"]).ToString();
                    lblAgent.Text = Convert.ToDecimal(dt.Tables[5].Rows[0]["Agent"]).ToString();
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("TransactionTrend: GetHomePageData(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        //protected void btnSearch1_ServerClick(object sender, EventArgs e)
        //{
        //    {
        //        try
        //        {
        //            BindChart1();
        //            ///// GetHomePageData1();
        //        }

        //        catch (Exception Ex)
        //        {
        //            ErrorLog.DashboardTrace("TransactionTrend: btnSearch_ServerClick(): Exception: " + Ex.Message);
        //            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //        }
        //    }
        //}
        #endregion


        //[WebMethod]
        //public static string GetChartData(string channel)
        //{
        //    var data = new List<object>();

        //    // Example data - replace this with your actual data fetching logic
        //    switch (channel)
        //    {
        //        case "AEPS":
        //            data.Add(new { label = "Equity", value = "300000" });
        //            data.Add(new { label = "Debt", value = "230000" });
        //            break;

        //        case "OtherChannel":
        //            data.Add(new { label = "Real-estate", value = "270000" });
        //            data.Add(new { label = "Insurance", value = "20000" });
        //            break;
        //    }

        //    // Serialize the data to JSON format
        //    var jsonSerializer = new JavaScriptSerializer();
        //    return jsonSerializer.Serialize(data);
        //}
        private void PieChartData()
        {
            var chartData = new
            {
                chart = new
                {
                    caption = "Recommended Switch Split",
                    subcaption = "For a net-worth of $1M",
                    showvalues = "1",
                    showpercentintooltip = "0",
                    numberprefix = "$",
                    enablemultislicing = "1",
                    theme = "candy"
                },
                data = new[]
                {
                new { label = "Maximus", value = "300000" },
                new { label = "Sarvatra", value = "230000" },
                new { label = "Proxima", value = "180000" },
                new { label = "PayRakam", value = "270000" },
                new { label = "Other", value = "20000" }
            }
            };

            // Serialize to JSON
            string jsonData = JsonSerializer.Serialize(chartData);//Need to uncomment

            // Bind to the HiddenField control
            hidChartData.Value = jsonData;

            // Register the script to call renderChart1 on page load
            ClientScript.RegisterStartupScript(this.GetType(), "CallRenderChart", "callRenderChart();", true);
        }
    }
}