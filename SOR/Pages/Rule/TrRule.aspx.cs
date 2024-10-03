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
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Text.Json;
using Npgsql;
using MaxiSwitch.EncryptionDecryption;



namespace SOR.Pages.Rule
{
    public partial class TrRule : System.Web.UI.Page
    {
        public static RuleEntity _RuleEntityy = new RuleEntity(); // Initialize it here
        public static CommonEntity _CommonEntity = new CommonEntity(); // Initialize it here
        RuleEntity _RuleEntity = new RuleEntity();
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "TrRule.aspx", "7");
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
                            Session["priority"] = null;
                            Session["priority2"] = null;

                            Session["Username"] = "Maximus";
                            BindGroup();
                            UserPermissions.RegisterStartupScriptForNavigationListActive("3", "7");
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
        private void BindGroup()
        {
            try
            {
                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                DataTable dt = new DataTable();
                dt = _RuleEntity.GetGroup();

                rptrGroup.DataSource = dt;
                rptrGroup.DataBind();
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("TrRule: BindRules(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void rptrGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    var dataItem = (DataRowView)e.Item.DataItem;
                    int groupId = Convert.ToInt32(dataItem["id"]);
                    var parentDataItem = e.Item.DataItem;
                    Repeater rptChild = (Repeater)e.Item.FindControl("rptRule");
                    // Check if the child repeater is found
                    if (rptChild != null)
                    {
                        // Retrieve child data (replace this with your data retrieval logic)
                        _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _RuleEntity.GroupId = groupId;
                        DataTable dt = new DataTable();
                        dt = _RuleEntity.GetRule();
                        // Get the 'is_active' field to determine the color
                        string isActive = dataItem["isactive"].ToString(); // Assuming the value is in this field

                        // Find the element you want to change
                        var sliderDiv = e.Item.FindControl("chkSlider") as HtmlInputCheckBox;
                        

                        if (sliderDiv != null)
                        {
                            // Set the checkbox state
                            sliderDiv.Checked = (isActive == "Active");

                            // Apply styles based on the isActive status
                            if (sliderDiv.Checked)
                            {
                                sliderDiv.Attributes["class"] = "clsslider active"; // Apply additional classes if needed
                                sliderDiv.Style["background-color"] = "#0d6efd"; // Example style change
                                
                            }
                            else
                            {
                                sliderDiv.Attributes["class"] = "clsslider inactive";
                                sliderDiv.Style["background-color"] = "red"; // Example style change
                                
                            }


                            rptChild.DataSource = dt;
                            rptChild.DataBind();
                            groupId = 0;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        [WebMethod]
        public static string ToggleSlider(bool IsChecked, string Id)
        {
            bool isActive = IsChecked;
            _RuleEntityy.IsActive = isActive ? 1 : 0;
            _RuleEntityy.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
            //_RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
            _RuleEntityy.GroupId = Convert.ToInt32(Id);
            _RuleEntityy.Flag = (int)EnumCollection.EnumRuleType.Update;

            string statusCode = _RuleEntityy.UpdateGroupStatus();

            if (statusCode == "UPD00")
            {
                _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(ConstResponseCodes.Update, (int)EnumCollection.TransactionSource.Others);
                _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
            }
            else if (statusCode == "UPD96")
            {
                _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(ConstResponseCodes.UpdateFail, (int)EnumCollection.TransactionSource.Others);
                _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
            }
            else if (statusCode == "FLG96")
            {
                _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(ConstResponseCodes.FlagNotExist, (int)EnumCollection.TransactionSource.Others);
                _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
            }
            else
            {
                _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(ConstResponseCodes.Fail, (int)EnumCollection.TransactionSource.Others);
                _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
            }
            var response = new
            {
                StatusMessage = _CommonEntity.ResponseMessage
            };
            ErrorLog.RuleTrace("TrRule: ToggleSlider(): DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
            return new JavaScriptSerializer().Serialize(response);
        }

        [WebMethod]
        public static string ToggleRuleSlider(bool IsChecked, string Id)
        {
            bool isActive = IsChecked;
            _RuleEntityy.IsActive = isActive ? 1 : 0;
            _RuleEntityy.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
            //_RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
            _RuleEntityy.RuleId = Convert.ToInt32(Id);
            _RuleEntityy.Flag = (int)EnumCollection.EnumRuleType.Update;

            string statusCode = _RuleEntityy.UpdateRuleStatus();

            if (statusCode == "UPD00")
            {
                _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(ConstResponseCodes.Update, (int)EnumCollection.TransactionSource.Others);
                _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
            }
            else if (statusCode == "UPD96")
            {
                _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(ConstResponseCodes.UpdateFail, (int)EnumCollection.TransactionSource.Others);
                _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
            }
            else if (statusCode == "FLG96")
            {
                _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(ConstResponseCodes.FlagNotExist, (int)EnumCollection.TransactionSource.Others);
                _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
            }
            else
            {
                _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(ConstResponseCodes.Fail, (int)EnumCollection.TransactionSource.Others);
                _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
            }
            var response = new
            {
                StatusMessage = _CommonEntity.ResponseMessage
            };
            ErrorLog.RuleTrace("TrRule: ToggleRuleSlider(): DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
            return new JavaScriptSerializer().Serialize(response);
        }
        
        protected void rptrGroup_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int itemId = Convert.ToInt32(e.CommandArgument);
                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                _RuleEntity.GroupId = Convert.ToInt32(itemId);
                _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                dt = _RuleEntity.GetEditGroupDetails();
                if (dt.Rows.Count > 0)
                {
                    // Assuming first row data
                    DataRow row = dt.Rows[0];

                    // Binding data to TextBox controls
                    txtGroupName.Text = row["group_name"].ToString();
                    txtGroupDescription.Text = row["group_description"].ToString();
                    //string priority = row["priority"].ToString(); // If priority is integer, you can format as needed

                    //switch (priority)
                    //{
                    //    case "1":
                    //        LinkButton1.CssClass = "priority-label btn btn-outline-secondary active";
                    //        break;
                    //    case "2":
                    //        LinkButton2.CssClass = "priority-label btn btn-outline-secondary active";
                    //        break;
                    //    case "3":
                    //        LinkButton3.CssClass = "priority-label btn btn-outline-secondary active";
                    //        break;
                    //    default:
                    //        // Optionally handle invalid priority values
                    //        break;
                    //}
                    Session["priority"] = "IsEdit";
                    hdnShowModalG.Value = "true";
                }
            }
            else if (e.CommandName == "Delete")
            {
                int itemId = Convert.ToInt32(e.CommandArgument);
                _RuleEntityy.IsDelete = 1;
                _RuleEntityy.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                //_RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                _RuleEntityy.GroupId = Convert.ToInt32(itemId);
                _RuleEntityy.Flag = (int)EnumCollection.EnumRuleType.Delete;

                string statusCode = _RuleEntityy.UpdateGroupStatus();

                if (statusCode == "UPD00")
                {
                    _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                    _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                }
                else
                {
                    _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                    _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                }
                var response = new
                {
                    StatusMessage = _CommonEntity.ResponseMessage
                };
                ErrorLog.RuleTrace("TrRule: Delete() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
            }
            else if (e.CommandName == "AddRule")
            {
                int itemId = Convert.ToInt32(e.CommandArgument);
                BindDropdownValues();
                ddlGroupName.SelectedValue = Convert.ToString(itemId);
                hdnShowModalR.Value = "true";
            }
        }
        //protected void Priority1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Reset the CSS class for all LinkButtons to the default class (no active)
        //        LinkButton1.CssClass = "priority-label btn btn-outline-secondary"; // Low priority
        //        LinkButton2.CssClass = "priority-label btn btn-outline-secondary"; // Medium priority
        //        LinkButton2.CssClass = "priority-label btn btn-outline-secondary"; // High priority

        //        // Now, apply the "active" class only to the clicked button
        //        var clickedButton = (LinkButton)sender;
        //        hdnPriority1.Value = clickedButton.CommandArgument;
        //        clickedButton.CssClass = "priority-label btn btn-outline-secondary active"; // Mark this as active
        //        hdnShowModalG.Value = "true";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.RuleTrace("Priority1_Click() | Exception: " + ex.Message);
        //    }
        //}
        //protected void Priority2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Reset the CSS class for all LinkButtons to the default class (no active)
        //        LinkButton4.CssClass = "priority-label btn btn-outline-secondary"; // Low priority
        //        LinkButton5.CssClass = "priority-label btn btn-outline-secondary"; // Medium priority
        //        LinkButton6.CssClass = "priority-label btn btn-outline-secondary"; // High priority

        //        // Now, apply the "active" class only to the clicked button
        //        var clickedButton = (LinkButton)sender;
        //        hdnPriority2.Value = clickedButton.CommandArgument;
        //        clickedButton.CssClass = "priority-label btn btn-outline-secondary active"; // Mark this as active
        //        hdnShowModalR.Value = "true";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.RuleTrace("Priority2_Click() | Exception: " + ex.Message);
        //    }
        //}

        protected void btnCreGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtGroupName.Text))
                {
                    ShowWarning("Please Enter Group Name. Try again", "Warning");
                    return;
                }
                else if (string.IsNullOrEmpty(txtGroupDescription.Text))
                {
                    ShowWarning("Please Enter Group Description. Try again", "Warning");
                    return;
                }
                //else if (Session["priority"] == null || string.IsNullOrEmpty(Session["priority"].ToString()))
                //{
                //    ShowWarning("Please Select Priority. Try again", "Warning");
                //    return;
                //}
                else
                {
                    if (Session["priority"] != null)
                    {
                        _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _RuleEntity.groupName = !string.IsNullOrEmpty(txtGroupName.Text) ? txtGroupName.Text.Trim() : null;
                        _RuleEntity.groupDescription = !string.IsNullOrEmpty(txtGroupDescription.Text) ? txtGroupDescription.Text.Trim() : null;
                        //_RuleEntity.priority = Convert.ToInt32(Convert.ToString(Session["priority"])) != 0 ? Convert.ToInt32(Convert.ToString(Session["priority"])) : 0;
                        _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Edit;
                        string statusCode = _RuleEntity.InsertOrUpdateGroup();
                        if (statusCode == "VERI00")
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        else
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        var response = new
                        {
                            StatusMessage = _CommonEntity.ResponseMessage
                        };
                        ErrorLog.RuleTrace("TrRule: EditGroup() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                        
                    }
                    else
                    {
                        _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _RuleEntity.groupName = !string.IsNullOrEmpty(txtGroupName.Text) ? txtGroupName.Text.Trim() : null;
                        _RuleEntity.groupDescription = !string.IsNullOrEmpty(txtGroupDescription.Text) ? txtGroupDescription.Text.Trim() : null;
                        //_RuleEntity.priority = Convert.ToInt32(hdnPriority1.Value) != 0 ? Convert.ToInt32(hdnPriority1.Value) : 0;
                        _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Insert;
                        string statusCode = _RuleEntity.InsertOrUpdateGroup();
                        if (statusCode == "INS00")
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        else
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        var response = new
                        {
                            StatusMessage = _CommonEntity.ResponseMessage
                        };
                        ErrorLog.RuleTrace("TrRule: btnCreGroup_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                     
                    }
                }
                hdnShowModalG.Value = "false";
                BindGroup();
            }
            catch (Exception Ex)
            {
                hdnShowModalG.Value = "false";
                ErrorLog.RuleTrace("TrRule: btnCreGroup_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        protected void btnCloseGroup_Click(object sender, EventArgs e)
        {
            try
            {
                txtGroupName.Text = string.Empty;
                txtGroupDescription.Text = string.Empty;
                hdnShowModalG.Value = "false";
                hdnShowModalG.Value = string.Empty;
                Session["priority"] = null;
                Session["priority2"] = null;
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("TrRule: btnCloseGroup_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
            }
        }

        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            try
            {
                Session["priority"] = null;
                hdnShowModalR.Value = "false";
                hdnShowModalG.Value = "true";
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("TrRule: btnAddGroup_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
            }
        }

        protected void btnAddRule_Click(object sender, EventArgs e)
        {
            try
            {
                Session["priority2"] = null;
                hdnShowModalG.Value = "false";
                hdnShowModalR.Value = "true";
                BindDropdownValues();
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("TrRule: btnAddRule_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
            }
        }

        protected void btnCreateRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRuleName.Text))
                {
                    ShowWarning("Please Enter Rule Name. Try again", "Warning");
                    return;
                }
                else if (string.IsNullOrEmpty(txtRuleDescription.Text))
                {
                    ShowWarning("Please Enter Rule Description. Try again", "Warning");
                    return;
                }
                else if ((ddlTxnType.SelectedValue !="0") && !string.IsNullOrEmpty(ddlTxnTypeFIN.SelectedValue))
                {
                    ShowWarning("Please Any One Txn Type Or Fin/Non-Fin. Try again", "Warning");
                    return;
                }
                else if (string.IsNullOrEmpty(ddlSwitch.SelectedValue))
                {
                    ShowWarning("Please select Switch. Try again", "Warning");
                    return;
                }
                //else if (!string.IsNullOrEmpty(txtPercentage.Text) && !string.IsNullOrEmpty(txtCount.Text))
                //{
                //    ShowWarning("Please Any One From Percentage Or Count. Try again", "Warning");
                //    return;
                //}
                else
                {
                    if (Session["priority2"] != null)
                    {
                        //string[] selectedValues = Request.Form.GetValues("chkAggregator");
                        //if (selectedValues != null && selectedValues.Length > 0)
                        //{
                        //    _RuleEntity.Aggregator = string.Join(",", selectedValues);
                            
                        //}
                        //else
                        //{
                        //    _RuleEntity.Aggregator = null;
                        //}
                        //string[] selectedValuesiin = Request.Form.GetValues("chkiin");
                        //if (selectedValuesiin != null && selectedValuesiin.Length > 0)
                        //{
                        //    _RuleEntity.IIN = string.Join(",", selectedValuesiin);
                        //}
                        //else
                        //{
                        //    _RuleEntity.IIN = null;
                        //}
                        //string[] selectedValuestxntype = Request.Form.GetValues("chktxntype");
                        //if (selectedValuestxntype != null && selectedValuestxntype.Length > 0)
                        //{
                        //    _RuleEntity.TxnType = string.Join(",", selectedValuestxntype);
                        //}
                        //else
                        //{
                        //    _RuleEntity.TxnType = null;
                        //}

                        _RuleEntity.Channel = ddlChannel.SelectedValue != "0" ? (ddlChannel.SelectedValue) : null;
                        _RuleEntity.Switch = ddlSwitch.SelectedValue != "0" ? (ddlSwitch.SelectedValue) : null;
                        _RuleEntity.percentage = Convert.ToInt32(!string.IsNullOrEmpty(txtPercentage.Text)) != 0 ? Convert.ToInt32(txtPercentage.Text.Trim()) : 0;
                        _RuleEntity.Count = Convert.ToInt32(!string.IsNullOrEmpty(txtCount.Text)) != 0 ? Convert.ToInt32(txtCount.Text.Trim()) : 0;
                        _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _RuleEntity.groupId = Convert.ToInt32(ddlGroupName.SelectedValue) != 0 ? Convert.ToInt32(ddlGroupName.SelectedValue) : 0;
                        _RuleEntity.ruleName = !string.IsNullOrEmpty(txtRuleName.Text) ? txtRuleName.Text.Trim() : null;
                        _RuleEntity.ruleDescription = !string.IsNullOrEmpty(txtRuleDescription.Text) ? txtRuleDescription.Text.Trim() : null;
                        _RuleEntity.Aggregator = hfSelectedValues.Value != "0" ? hfSelectedValues.Value : null;
                        _RuleEntity.IIN = hfSelectedSecondValues.Value != "0" ? hfSelectedSecondValues.Value : null;
                        _RuleEntity.TxnType = hdnTxnType.Value != "0" ? hdnTxnType.Value : null;
                        _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Edit;
                        string statusCode = _RuleEntity.InsertOrUpdateRule();
                        if (statusCode == "VERI00")
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        else
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        var response = new
                        {
                            StatusMessage = _CommonEntity.ResponseMessage
                        };
                        ErrorLog.RuleTrace("TrRule: EditGroup() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                    }
                    else
                    {
                        

                        _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _RuleEntity.groupId = Convert.ToInt32(ddlGroupName.SelectedValue) != 0 ? Convert.ToInt32(ddlGroupName.SelectedValue) : 0;
                        _RuleEntity.ruleName = !string.IsNullOrEmpty(txtRuleName.Text) ? txtRuleName.Text.Trim() : null;
                        _RuleEntity.ruleDescription = !string.IsNullOrEmpty(txtRuleDescription.Text) ? txtRuleDescription.Text.Trim() : null;
                        _RuleEntity.Channel = ddlChannel.SelectedValue != "0" ? (ddlChannel.SelectedValue) : null;
                        _RuleEntity.Switch = ddlSwitch.SelectedValue != "0" ? (ddlSwitch.SelectedValue) : null;
                        _RuleEntity.percentage = Convert.ToInt32(!string.IsNullOrEmpty(txtPercentage.Text)) != 0 ? Convert.ToInt32(txtPercentage.Text.Trim()) : 0;
                        _RuleEntity.Count = Convert.ToInt32(!string.IsNullOrEmpty(txtCount.Text)) != 0 ? Convert.ToInt32(txtCount.Text.Trim()) : 0;
                        _RuleEntity.Aggregator = hfSelectedValues.Value != "0" ? hfSelectedValues.Value : null;
                        _RuleEntity.IIN = hfSelectedSecondValues.Value != "0" ? hfSelectedSecondValues.Value : null;
                        _RuleEntity.TxnType = hdnTxnType.Value != "0" ? hdnTxnType.Value : null;
                        _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Insert;
                        string statusCode = _RuleEntity.InsertOrUpdateRule();
                        if (statusCode == "INS00")
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        else
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        var response = new
                        {
                            StatusMessage = _CommonEntity.ResponseMessage
                        };
                        ErrorLog.RuleTrace("TrRule: btnCreGroup_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                    }
                }
                hdnShowModalR.Value = "false";
            }
            catch (Exception Ex)
            {
                hdnShowModalR.Value = "false";
                ErrorLog.RuleTrace("TrRule: btnCreateRule_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
            }
        }

        protected void btnCloseRule_Click(object sender, EventArgs e)
        {
            try
            {
                hdnShowModalR.Value = "false";
                hdnShowModalR.Value = string.Empty;
                Session["priority"] = null;
                Session["priority2"] = null;
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("TrRule: btnCloseRule_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
            }
        }
        #region Bind DropDown
        private void BindDropdownValues()
        {
            DataSet _dsClient = new DataSet();
            try
            {

                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                var tables = _RuleEntity.GetDropDownValues();

                if (tables != null)
                {
                    // Bind Group Name DropDown
                    if (tables.ContainsKey("temp_bindgroup"))
                    {
                        DataTable bindGroupTable = tables["temp_bindgroup"];

                        if (bindGroupTable.Rows.Count > 0)
                        {
                            ddlGroupName.Items.Clear();
                            ddlGroupName.DataSource = bindGroupTable;
                            ddlGroupName.DataTextField = "group_name";
                            ddlGroupName.DataValueField = "id";
                            ddlGroupName.DataBind();
                        }
                        else
                        {
                            ddlGroupName.Items.Clear();
                            ddlGroupName.DataSource = null;
                            ddlGroupName.DataBind();
                        }
                    }
                    // Bind Aggregator DropDown
                    if (tables.ContainsKey("temp_bcpartnerid"))
                    {
                        DataTable bcPartnerIdTable = tables["temp_bcpartnerid"];

                        if (bcPartnerIdTable.Rows.Count > 0)
                        {
                            ddlAggregator.Items.Clear();
                            ddlAggregator.DataSource = bcPartnerIdTable;
                            ddlAggregator.DataTextField = "bcpartner_id";
                            ddlAggregator.DataValueField = "bcpartner_id";
                            ddlAggregator.DataBind();
                            //sb.Append($"<div class='dropdown-item' onclick='selectItem(this)'>{item}</div>");
                            var sb = new StringBuilder();
                            foreach (DataRow row in bcPartnerIdTable.Rows)
                            {
                                string value = row["bcpartner_id"].ToString();
                                string text = row["bcpartner_id"].ToString();
                                sb.AppendFormat(
                                    "<label><input type='checkbox' name='chkAggregator' value='{0}' /> {1}</label><br/>",
                                    value,
                                    text
                                );
                            }
                            litAggregator.Text = sb.ToString();
                        }
                        else
                        {
                            ddlAggregator.Items.Clear();
                            ddlAggregator.DataSource = null;
                            ddlAggregator.DataBind();
                        }
                    }
                    // Bind Iin DropDown
                    if (tables.ContainsKey("temp_bindiin"))
                    {
                        DataTable bindiin = tables["temp_bindiin"];

                        if (bindiin.Rows.Count > 0)
                        {
                            ddlIIN.Items.Clear();
                            ddlIIN.DataSource = bindiin;
                            ddlIIN.DataTextField = "iin";
                            ddlIIN.DataValueField = "iin";
                            ddlIIN.DataBind();

                            var sb = new StringBuilder();
                            foreach (DataRow row in bindiin.Rows)
                            {
                                string value = row["iin"].ToString();
                                string text = row["iin"].ToString();
                                sb.AppendFormat(
                                    "<label><input type='checkbox' name='chkiin' value='{0}' /> {1}</label><br/>", 
                                    value,
                                    text
                                );
                            }
                            litIIN.Text = sb.ToString();
                        }
                        else
                        {
                            ddlIIN.Items.Clear();
                            ddlIIN.DataSource = null;
                            ddlIIN.DataBind();
                        }
                    }

                    if (tables.ContainsKey("temp_bindiin"))
                    {
                        DataTable bindtxntype = tables["temp_bindtxntype"];

                        if (bindtxntype.Rows.Count > 0)
                        {
                            ddlTxnType.Items.Clear();
                            ddlTxnType.DataSource = bindtxntype;
                            ddlTxnType.DataTextField = "txntype";
                            ddlTxnType.DataValueField = "id";
                            ddlTxnType.DataBind();
                            ddlTxnType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));


                            var sb = new StringBuilder();
                            foreach (DataRow row in bindtxntype.Rows)
                            {
                                string value = row["id"].ToString();
                                string text = row["txntype"].ToString();
                                sb.AppendFormat(
                                    "<label><input type='checkbox' name='chktxntype' value='{0}' /> {1}</label><br/>",
                                    value,
                                    text
                                );
                            }
                            ltrtxntype.Text = sb.ToString();
                        }
                        else
                        {
                            ddlTxnType.Items.Clear();
                            ddlTxnType.DataSource = null;
                            ddlTxnType.DataBind();
                            ddlTxnType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
                        }
                    }
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
        private void ShowWarning(string message, string title)
        {
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Page),
                "ShowWarning",
                $"showWarning('{message}', '{title}');",
                true
            );
        }

        protected void rptRule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditRule")
                {
                    int itemId = Convert.ToInt32(e.CommandArgument);
                    _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                    _RuleEntity.RuleId = Convert.ToInt32(itemId);
                    _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                    dt = _RuleEntity.GetEditRuleDetails();
                    if (dt.Rows.Count > 0)
                    {
                        // Assuming first row data
                        DataRow row = dt.Rows[0];
                        
                        // Binding data to TextBox controls
                        ddlGroupName.SelectedValue = row["groupid"].ToString();
                        //ddlTxnType.SelectedValue = row["txntype_id"].ToString();
                        txtRuleName.Text = row["rulename"].ToString();
                        txtRuleDescription.Text = row["ruledescription"].ToString();

                        txtPercentage.Text = row["percentage"].ToString();
                        txtCount.Text = row["maxcount"].ToString();
                        ddlChannel.SelectedValue = row["channel"].ToString();

                        //string priority = row["priority"].ToString(); // If priority is integer, you can format as needed

                        //switch (priority)
                        //{
                        //    case "1":
                        //        LinkButton4.CssClass = "priority-label btn btn-outline-secondary active";
                        //        break;
                        //    case "2":
                        //        LinkButton5.CssClass = "priority-label btn btn-outline-secondary active";
                        //        break;
                        //    case "3":
                        //        LinkButton6.CssClass = "priority-label btn btn-outline-secondary active";
                        //        break;
                        //    default:
                        //        // Optionally handle invalid priority values
                        //        break;
                        //}
                        Session["priority2"] = "IsEdit";
                        ddlSwitch.SelectedValue = row["switch_id"].ToString();
                        //ddlRatio.SelectedValue = row["distribution_ratio"].ToString();
                        //ddlSwithMode.SelectedValue = row["manual_reset"].ToString();

                        _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        DataTable dt2 = new DataTable();
                        var tables = _RuleEntity.GetDropDownValues();

                        if (tables != null)
                        {
                            if (tables.ContainsKey("temp_bindgroup"))
                            {
                                DataTable bindGroupTable = tables["temp_bindgroup"];

                                if (bindGroupTable.Rows.Count > 0)
                                {
                                    ddlGroupName.Items.Clear();
                                    ddlGroupName.DataSource = bindGroupTable;
                                    ddlGroupName.DataTextField = "group_name";
                                    ddlGroupName.DataValueField = "id";
                                    ddlGroupName.DataBind();
                                }
                                else
                                {
                                    ddlGroupName.Items.Clear();
                                    ddlGroupName.DataSource = null;
                                    ddlGroupName.DataBind();
                                }
                            }
                            //if (tables.ContainsKey("temp_bindtxntype"))
                            //{
                            //    DataTable bindtxntypeTable = tables["temp_bindtxntype"];

                            //    if (bindtxntypeTable.Rows.Count > 0)
                            //    {
                            //        ddlTxnType.Items.Clear();
                            //        ddlTxnType.DataSource = bindtxntypeTable;
                            //        ddlTxnType.DataTextField = "txntype";
                            //        ddlTxnType.DataValueField = "id";
                            //        ddlTxnType.DataBind();
                            //    }
                            //    else
                            //    {
                            //        ddlTxnType.Items.Clear();
                            //        ddlTxnType.DataSource = null;
                            //        ddlTxnType.DataBind();
                            //    }
                            //}
                            DataTable txntypeTable = tables["temp_bindtxntype"];
                            string[] tableNames = { "temp_bcpartnerid", "temp_bindiin", "temp_bindtxntype" };
                            string[] columnNames = { "bcpartner_id", "iin", "txntype" };
                            string[] selectedValues = { row["bcpartner_id"].ToString(), row["iin"].ToString(), txntypeTable.Rows[0]["txntype"].ToString() };
                            Literal[] literals = { litAggregator, litIIN, ltrtxntype };

                            BindMultipleDropdowns(tables, tableNames, columnNames, selectedValues, literals);
                        }
                        hdnShowModalR.Value = "true";
                    }
                }
                else if (e.CommandName == "DeleteRule")
                {
                    int itemId = Convert.ToInt32(e.CommandArgument);
                    _RuleEntityy.IsDelete = 1;
                    _RuleEntityy.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                    //_RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                    _RuleEntityy.RuleId = Convert.ToInt32(itemId);
                    _RuleEntityy.Flag = (int)EnumCollection.EnumRuleType.Delete;

                    string statusCode = _RuleEntityy.UpdateRuleStatus();

                    if (statusCode == "UPD00")
                    {
                        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                    }
                    else
                    {
                        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                    }
                    var response = new
                    {
                        StatusMessage = _CommonEntity.ResponseMessage
                    };
                    ErrorLog.RuleTrace("TrRule: Delete() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }
        
        protected void rptRule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    var dataItem = (DataRowView)e.Item.DataItem;

                    // Ensure 'isactive' exists
                    if (dataItem.Row.Table.Columns.Contains("is_active"))
                    {
                        string isActive = dataItem["is_active"].ToString();
                        var sliderRule = e.Item.FindControl("chkSliderRule") as HtmlInputCheckBox;

                        if (sliderRule != null)
                        {
                            sliderRule.Checked = (isActive == "Active");
                            sliderRule.Attributes["class"] = sliderRule.Checked ? "clssliderRule active" : "clssliderRule inactive";
                            sliderRule.Style["background-color"] = sliderRule.Checked ? "#0d6efd" : "red";
                        }
                    }
                    else
                    {
                        // Handle missing 'isactive' column scenario
                        Console.WriteLine("'isactive' column is missing.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                throw;
            }
        }
        // Method to handle binding multiple dropdowns with checkboxes
        private void BindMultipleDropdowns(Dictionary<string, DataTable> tables, string[] tableNames, string[] columnNames, string[] selectedValues, Literal[] literals)
        {
            for (int i = 0; i < tableNames.Length; i++)
            {
                if (tables.ContainsKey(tableNames[i]))
                {
                    BindCheckboxDropdown(tables[tableNames[i]], columnNames[i], selectedValues[i], literals[i]);
                }
            }
        }
        private void BindCheckboxDropdown(DataTable sourceTable, string columnName, string selectedValue, Literal literalControl)
        {
            if(columnName == "bcpartner_id")
            {
                literalControl.Text = string.Join("", sourceTable.AsEnumerable().Select(row =>
                {
                    string value = row[columnName].ToString();
                    string isChecked = value == selectedValue ? "checked" : "";
                    return $"<label><input type='checkbox' name='chkAggregator' value='{value}' {isChecked} /> {value}</label><br/>";
                }));
            }
            if(columnName == "iin")
            {
                literalControl.Text = string.Join("", sourceTable.AsEnumerable().Select(row =>
                {
                    string value = row[columnName].ToString();
                    string isChecked = value == selectedValue ? "checked" : "";
                    return $"<label><input type='checkbox' name='chkiin' value='{value}' {isChecked} /> {value}</label><br/>";
                }));
            }
            if (columnName == "txntype")
            {
                literalControl.Text = string.Join("", sourceTable.AsEnumerable().Select(row =>
                {
                    string value = row[columnName].ToString();
                    string isChecked = value == selectedValue ? "checked" : "";
                    return $"<label><input type='checkbox' name='chktxntype' value='{value}' {isChecked} /> {value}</label><br/>";
                }));
            }
        }
        [WebMethod]
        public static string ToggleTxnType(bool IsChecked)
        {
            if (IsChecked)
            {
                return "showCount-clearPercentage";
            }
            else
            {
                return "showPercentage-clearCount";
            }
        }
        //protected void imgAddRule_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Write("Image button clicked!");
        //}
        [WebMethod]
        public static string ShortingTxnRuleGroups(OrderData orderData)
        {
            string GroupOrder = string.Empty;
            string GroupID = string.Empty;
            string TxnRuleOrder = string.Empty;
            string TxnRuleId = string.Empty;
            try
            {
                foreach (var parent in orderData.ParentOrder)
                {
                    GroupOrder = parent.Order;
                    GroupID = parent.Id.ToString();

                    try
                    {
                        using (var conn = new NpgsqlConnection(Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
                        {
                            conn.Open();
                            using (var cmd = new NpgsqlCommand("CALL public.Proc_UpdateGroupandRulePriority(@p_groupid, @p_group_description, @p_group_priority, @p_ruleid, @p_ruledescription, @p_rule_priority, @p_modifiedby)", conn))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("p_groupid", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(GroupID));
                                cmd.Parameters.AddWithValue("p_group_description", NpgsqlTypes.NpgsqlDbType.Varchar, string.IsNullOrEmpty(string.Empty) ? (object)DBNull.Value : string.Empty);
                                cmd.Parameters.AddWithValue("p_group_priority", NpgsqlTypes.NpgsqlDbType.Varchar, string.IsNullOrEmpty(GroupOrder) ? (object)DBNull.Value : GroupOrder);
                                cmd.Parameters.AddWithValue("p_ruleid", NpgsqlTypes.NpgsqlDbType.Integer, string.IsNullOrEmpty(TxnRuleId) ? 0 : Convert.ToInt32(TxnRuleId));
                                cmd.Parameters.AddWithValue("p_ruledescription", NpgsqlTypes.NpgsqlDbType.Varchar, string.IsNullOrEmpty(string.Empty) ? (object)DBNull.Value : string.Empty);
                                cmd.Parameters.AddWithValue("p_rule_priority", NpgsqlTypes.NpgsqlDbType.Varchar, string.IsNullOrEmpty(TxnRuleOrder) ? (object)DBNull.Value : TxnRuleOrder);
                                var username = HttpContext.Current.Session["Username"];
                                cmd.Parameters.AddWithValue("p_modifiedby", NpgsqlTypes.NpgsqlDbType.Varchar, string.IsNullOrEmpty(username?.ToString()) ? (object)DBNull.Value : username.ToString());
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (PostgresException ex)
                    {
                        ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: ChangePriorityGroupTxnRules() \nException Occurred\n{ex.Message}");
                        ErrorLog.DBError(ex);
                        return "Somthing went to wrong!!!!...";
                        throw;
                    }

                    if (orderData.ChildOrders.TryGetValue(parent.Id.ToString(), out var childOrders))
                    {
                        foreach (var child in childOrders)
                        {
                            TxnRuleOrder = child.Order;
                            TxnRuleId = child.RuleId.ToString();

                            try
                            {
                                using (var conn = new NpgsqlConnection(Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
                                {
                                    conn.Open();
                                    using (var cmd = new NpgsqlCommand("CALL public.Proc_UpdateGroupandRulePriority(@p_groupid, @p_group_description, @p_group_priority, @p_ruleid, @p_ruledescription, @p_rule_priority, @p_modifiedby)", conn))
                                    {
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.AddWithValue("p_groupid", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(GroupID));
                                        cmd.Parameters.AddWithValue("p_group_description", NpgsqlTypes.NpgsqlDbType.Varchar, string.IsNullOrEmpty(string.Empty) ? (object)DBNull.Value : string.Empty);
                                        cmd.Parameters.AddWithValue("p_group_priority", NpgsqlTypes.NpgsqlDbType.Varchar, string.IsNullOrEmpty(GroupOrder) ? (object)DBNull.Value : GroupOrder);
                                        cmd.Parameters.AddWithValue("p_ruleid", NpgsqlTypes.NpgsqlDbType.Integer, string.IsNullOrEmpty(TxnRuleId) ? 0 : Convert.ToInt32(TxnRuleId));
                                        cmd.Parameters.AddWithValue("p_ruledescription", NpgsqlTypes.NpgsqlDbType.Varchar, string.IsNullOrEmpty(string.Empty) ? (object)DBNull.Value : string.Empty);
                                        cmd.Parameters.AddWithValue("p_rule_priority", NpgsqlTypes.NpgsqlDbType.Varchar, string.IsNullOrEmpty(TxnRuleOrder) ? (object)DBNull.Value : TxnRuleOrder);
                                        var username = HttpContext.Current.Session["Username"];
                                        cmd.Parameters.AddWithValue("p_modifiedby", NpgsqlTypes.NpgsqlDbType.Varchar, string.IsNullOrEmpty(username?.ToString()) ? (object)DBNull.Value : username.ToString());
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (PostgresException ex)
                            {
                                ErrorLog.CommonTrace($"Class: RuleEntity.cs \nFunction: ChangePriorityGroupTxnRules() \nException Occurred\n{ex.Message}");
                                ErrorLog.DBError(ex);
                                return "Somthing went to wrong!!!!...";
                                throw;
                            }
                        }
                    }
                }
                //TrRule pageInstance = new TrRule();
                //DataTable dt = pageInstance.RefreshGroupDataOnPriority();
                return "Priorites updated successful.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

        }
        //private DataTable RefreshGroupDataOnPriority()
        //{
        //    BindGroup();
        //    return dt;
        //}
    }
}