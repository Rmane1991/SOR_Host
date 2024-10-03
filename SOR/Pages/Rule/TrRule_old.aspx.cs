using AppLogger;
using BussinessAccessLayer;
using System;
using System.Collections;
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

namespace SOR.Pages.Rule
{
    public partial class TrRule_old : System.Web.UI.Page
    {
        RuleEntity _RuleEntity = new RuleEntity();
        //CheckBox _chkEditSubMenuOnOff = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = true;// UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "TrRule_old.aspx", "7");
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
                            BindRules();
                            //BindDropdownValues();
                            BindCheckboxes();
                            //UserPermissions.RegisterStartupScriptForNavigationListActive("3", "7");
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
                ErrorLog.DashboardTrace("TrRule: Page_Load(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void BindRules()
        {
            try
            {
                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                DataTable dt = new DataTable();
                dt = _RuleEntity.GetRule();

                gvRule.DataSource = dt;
                gvRule.DataBind();
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("TrRule: BindRules(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        protected void gvRule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Get the is_active value
                object isActiveValue = DataBinder.Eval(e.Item.DataItem, "is_active");

                // Convert to boolean
                bool isActive = ConvertToBoolean(isActiveValue);

                // Find the slider div and checkbox control
                HtmlGenericControl sliderDiv = (HtmlGenericControl)e.Item.FindControl("sliderDiv");
                CheckBox sliderButton = (CheckBox)e.Item.FindControl("SliderButton");

                // Apply styles based on the isActive value
                if (sliderDiv != null)
                {
                    sliderDiv.Style.Add("background-color", isActive ? "green" : "red");
                    sliderDiv.Style.Add("transform", isActive ? "translateX(26px)" : "translateX(0px)");
                }
            }
        }

        // Method to convert object to boolean
        private bool ConvertToBoolean(object value)
        {
            if (value is BitArray bitArray)
            {
                return bitArray.Count > 0 && bitArray[0];
            }
            return Convert.ToBoolean(value);
        }
        [System.Web.Services.WebMethod]
        public static void UpdateRowStatus(int id, bool isActive)
        {
            // Implement the update logic, e.g., update the database
            // Ensure you have proper data access and error handling
        }

        //protected void chkSlider_CheckedChanged(object sender, EventArgs e)
        //{ 
        //    CheckBox chkBox = (CheckBox)sender;
        //    RepeaterItem item = (RepeaterItem)chkBox.NamingContainer;
        //    HiddenField hfItemId = (HiddenField)item.FindControl("hfItemId");
        //    int id = int.Parse(hfItemId.Value);
        //    bool isChecked = chkBox.Checked;

        //    // Handle the slider state on the server side
        //    UpdateSliderState(id, isChecked);
        //}

        protected void btnAction_Click(object sender, EventArgs e)
        {
            string selectedValues = hfSelectedValues.Value;

            // Split the values by comma and process them
            if (!string.IsNullOrEmpty(selectedValues))
            {
                string[] values = selectedValues.Split(',');

                foreach (string value in values)
                {
                    // Process each selected value
                    // Example: Display in a label or store in a database
                    Response.Write($"Selected Value: {value}<br>");
                }
            }
            else
            {
                Response.Write("No items selected.");
            }
        }

        protected void EditRule_Click(object sender, EventArgs e)
        {
            // Edit rule logic here
        }

        protected void DeleteRule_Click(object sender, EventArgs e)
        {
            // Delete rule logic here
        }

        // This would normally be replaced by a data model from your data access layer
        public class Rule
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public string Severity { get; set; }
            public string Status { get; set; }
            public bool IsActive { get; set; }
        }

        protected void btnAddRule_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtender_AddRule.Show();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnSaveAction_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnCancelAction_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        #region BindDropDown
        private void BindDropdownValues()
        {
            DataSet _dsClient = new DataSet();
            try
            {

                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                DataTable dt = new DataTable();
                //dt = _RuleEntity.GetDropDownValues();

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows.Count == 1)
                        {
                            ddlAggregator.Items.Clear();
                            ddlAggregator.DataSource = dt.Copy();
                            ddlAggregator.DataTextField = "bcpartner_id";
                            ddlAggregator.DataValueField = "bcpartner_id";
                            ddlAggregator.DataBind();
                            //ddlAggregator.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                            var sb = new StringBuilder();
                            foreach (DataRow row in dt.Rows)
                            {
                                string value = row["bcpartner_id"].ToString(); // or "iin"
                                string text = row["bcpartner_id"].ToString();  // or "iin"
                                sb.AppendFormat(
                                    "<label><input type='checkbox' value='{0}' /> {1}</label><br/>",
                                    value,
                                    text
                                );
                            }
                            litAggregator.Text = sb.ToString();


                            //var sb = new StringBuilder();
                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    string value = row["bcpartner_id"].ToString(); // or "iin"
                            //    string text = row["bcpartner_id"].ToString();  // or "iin"
                            //    sb.AppendFormat(
                            //        "<label><input type='checkbox' value='{0}' /> {1}</label><br/>",
                            //        value,
                            //        text
                            //    );
                            //}
                            //dropdownItems.Controls.Add(new Literal { Text = sb.ToString() });

                            ddlIIN.Items.Clear();
                            ddlIIN.DataSource = dt.Copy();
                            ddlIIN.DataTextField = "iin";
                            ddlIIN.DataValueField = "iin";
                            ddlIIN.DataBind();
                            ddlIIN.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        }
                        else
                        {
                            ddlAggregator.Items.Clear();
                            ddlAggregator.DataSource = dt.Copy();
                            ddlAggregator.DataTextField = "bcpartner_id";
                            ddlAggregator.DataValueField = "bcpartner_id";
                            ddlAggregator.DataBind();

                            var sb = new StringBuilder();
                            foreach (DataRow row in dt.Rows)
                            {
                                string value = row["bcpartner_id"].ToString(); // or "iin"
                                string text = row["bcpartner_id"].ToString();  // or "iin"
                                sb.AppendFormat(
                                    "<label><input type='checkbox' value='{0}' /> {1}</label><br/>",
                                    value,
                                    text
                                );
                            }
                            litAggregator.Text = sb.ToString();

                            //var sb = new StringBuilder();
                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    string value = row["bcpartner_id"].ToString(); // or "iin"
                            //    string text = row["bcpartner_id"].ToString();  // or "iin"
                            //    sb.AppendFormat(
                            //        "<label><input type='checkbox' value='{0}' /> {1}</label><br/>",
                            //        value,
                            //        text
                            //    );
                            //}
                            //dropdownItems.Controls.Add(new Literal { Text = sb.ToString() });
                            //ddlAggregator.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                            ddlIIN.Items.Clear();
                            ddlIIN.DataSource = dt.Copy();
                            ddlIIN.DataTextField = "iin";
                            ddlIIN.DataValueField = "iin";
                            ddlIIN.DataBind();
                            ddlIIN.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                        }
                    }
                    else
                    {
                        ddlAggregator.Items.Clear();
                        ddlAggregator.DataSource = null;
                        ddlAggregator.DataBind();
                        //ddlAggregator.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));

                        ddlIIN.Items.Clear();
                        ddlIIN.DataSource = null;
                        ddlIIN.DataBind();
                        ddlIIN.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));
                    }
                }
                else
                {
                    ddlAggregator.Items.Clear();
                    ddlAggregator.DataSource = null;
                    ddlAggregator.DataBind();
                    //ddlAggregator.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));

                    ddlIIN.Items.Clear();
                    ddlIIN.DataSource = null;
                    ddlIIN.DataBind();
                    ddlIIN.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Data Found", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: BindDropdownClientDetails: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion
        private void BindCheckboxes()
        {

            //var sb = new StringBuilder();
            //foreach (ListItem item in ddlAggregator.Items)
            //{
            //    sb.AppendFormat(
            //        "<label><input type='checkbox' value='{0}' /> {1}</label><br/>",
            //        item.Value,
            //        item.Text
            //    );
            //}


            //litAggregator.Text = sb.ToString();
        }

    }
}