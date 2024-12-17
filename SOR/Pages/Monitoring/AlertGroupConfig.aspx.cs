using AppLogger;
using BussinessAccessLayer;
using Ganss.Xss;
using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SOR.Pages.Monitoring
{
    public partial class AlertGroupConfig : System.Web.UI.Page
    {
        public static RuleEntity _RuleEntityy = new RuleEntity();
        public static CommonEntity _CommonEntity = new CommonEntity();
        RuleEntity _RuleEntity = new RuleEntity();
        AlertEntity _alertEntity = new AlertEntity();
        DataTable dt = new DataTable();
        private List<string> conditions;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "AlertGroupConfig.aspx", "29");
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
                            BindSwitch();
                            UserPermissions.RegisterStartupScriptForNavigationListActive("8", "29");
                        }


                        // Initialize the conditions list
                        if (!IsPostBack)
                        {
                            conditions = new List<string>();
                            ViewState["Conditions"] = conditions;
                        }
                        else
                        {
                            conditions = (List<string>)ViewState["Conditions"];
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
                ErrorLog.DashboardTrace("AlertGroupConfig: Page_Load(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }


        private void BindGroup()
        {
            try
            {
                _alertEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                DataTable dt = new DataTable();
                dt = _alertEntity.GetGroup();
                rptrGroup.DataSource = dt;
                rptrGroup.DataBind();
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("AlertGroupConfig: BindRules(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void BindSwitch()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = _alertEntity.SwitchBind(null);
                if (dt.Rows.Count > 0)
                {
                    ddlSwitch.Items.Clear();
                    ddlSwitch.DataSource = dt;
                    ddlSwitch.DataTextField = "switchname";
                    ddlSwitch.DataValueField = "id";
                    ddlSwitch.DataBind();
                    ddlSwitch.Items.Insert(0, new ListItem("--Select--", "0"));
                    ddlSwitch.SelectedIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("AlertGroupConfig: BindSwitch(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void QueryBindGrid()
        {
            gvConditions.DataSource = new[] { new { } };
            gvConditions.DataBind();
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

                    if (rptChild != null)
                    {
                        _alertEntity.CreatedBy = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _alertEntity.groupId = groupId;
                        DataTable dt = new DataTable();
                        dt = _alertEntity.getAlertMembers();
                        // Get the 'is_active' field to determine the color
                        string isActive = dataItem["isactive"].ToString();

                        // Find the element you want to change
                        var sliderDiv = e.Item.FindControl("chkSlider") as HtmlInputCheckBox;

                        if (sliderDiv != null)
                        {
                            // Set the checkbox state
                            sliderDiv.Checked = (isActive == "Active");

                            // Apply styles based on the isActive status
                            if (sliderDiv.Checked)
                            {
                                sliderDiv.Attributes["class"] = "clsslider active";
                                sliderDiv.Style["background-color"] = "#0d6efd";
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
                _alertEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                _alertEntity.GroupId = Convert.ToInt32(itemId);
                Session["GroupId"] = _alertEntity.GroupId;
                _alertEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                dt = _alertEntity.GetEditGroupDetails();
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtGroupName.Text = row["groupname"].ToString();
                    txtGroupDescription.Text = row["description"].ToString();

                    Session["priority"] = "IsEdit";
                    hdnShowModalG.Value = "true";
                }
            }
            else if (e.CommandName == "Delete")
            {
                int itemId = Convert.ToInt32(e.CommandArgument);
                _alertEntity.IsDelete = 1;
                _alertEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                _alertEntity.GroupId = Convert.ToInt32(itemId);
                _alertEntity.Flag = (int)EnumCollection.EnumRuleType.Delete;

                string statusCode = _alertEntity.UpdateGroupStatus();

                if (statusCode == "DEL00")
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
                BindGroup();
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
                else
                {
                    if (Session["priority"] != null)
                    {
                        _alertEntity.CreatedBy = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _alertEntity.AlertGroupName = !string.IsNullOrEmpty(txtGroupName.Text) ? txtGroupName.Text.Trim() : null;
                        _alertEntity.AlertDescription = !string.IsNullOrEmpty(txtGroupDescription.Text) ? txtGroupDescription.Text.Trim() : null;
                        _alertEntity.AlertGroupId = Convert.ToString(Session["GroupId"].ToString());
                        _alertEntity.InsType = "group";
                        _alertEntity.flag = (int)EnumCollection.EnumRuleType.Edit;
                        string statusCode = _alertEntity.InsUpdateAlertDetails();
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
                        ErrorLog.RuleTrace("TrRule: EditGroup() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                        Session["GroupId"] = null;
                    }
                    else
                    {
                        _alertEntity.CreatedBy = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _alertEntity.AlertGroupName = !string.IsNullOrEmpty(txtGroupName.Text) ? txtGroupName.Text.Trim() : null;
                        _alertEntity.AlertDescription = !string.IsNullOrEmpty(txtGroupDescription.Text) ? txtGroupDescription.Text.Trim() : null;
                        _alertEntity.InsType = "group";
                        _alertEntity.flag = (int)EnumCollection.EnumRuleType.Insert;
                        string statusCode = _alertEntity.InsUpdateAlertDetails();
                        if (statusCode == "00")
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

        protected void btnTemplateMaster_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AlertTemplateMaster.aspx");
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("AlertGroupMaster: btnTemplateMaster_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
            }
        }


        protected void btnCreateRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRuleName.Text))
                {
                    ShowWarning("Please Enter Contact Name. Try again", "Warning");
                    return;
                }
                //else if (string.IsNullOrEmpty(txtemailBody.Text))
                //{
                //    ShowWarning("Please Enter mailbody. Try again", "Warning");
                //    return;
                //}
                //else if (string.IsNullOrEmpty(txtsmsBody.Text))
                //{
                //    ShowWarning("Please Enter smsbody. Try again", "Warning");
                //    return;
                //}
                else if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    ShowWarning("Please Enter EmailID. Try again", "Warning");
                    return;
                }
                else if (string.IsNullOrEmpty(txtMobile.Text))
                {
                    ShowWarning("Please Enter Mobile. Try again", "Warning");
                    return;
                }
                else if (string.IsNullOrEmpty(txtEmailCC.Text))
                {
                    ShowWarning("Please Enter EmailCC. Try again", "Warning");
                    return;
                }
                else if ((ddlAlertsentOn.SelectedValue == "0") && !string.IsNullOrEmpty(ddlAlertsentOn.SelectedValue))
                {
                    ShowWarning("Please select SendOn. Try again", "Warning");
                    return;
                }

                else
                {
                    if (Session["priority2"] != null)
                    {
                        _alertEntity.CreatedBy = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _alertEntity.AlertGroupId = Convert.ToString(ddlGroupName.SelectedValue) != "0" ? Convert.ToString(ddlGroupName.SelectedValue) : "0";
                        _alertEntity.AlertMemberId = Convert.ToString(Session["RuleId"].ToString());
                        _alertEntity.AlertSwitch = Convert.ToString(ddlSwitch.SelectedValue) != "0" ? Convert.ToString(ddlSwitch.SelectedValue) : "0";
                        _alertEntity.AlertGroupName = !string.IsNullOrEmpty(txtGroupName.Text) ? txtGroupName.Text.Trim() : null;
                        _alertEntity.GroupType = "";
                        _alertEntity.MemeberName = !string.IsNullOrEmpty(txtRuleName.Text) ? txtRuleName.Text.Trim() : null;
                        _alertEntity.Email = !string.IsNullOrEmpty(txtEmail.Text) ? txtEmail.Text.Trim() : null;
                        _alertEntity.Phone = !string.IsNullOrEmpty(txtMobile.Text) ? txtMobile.Text.Trim() : null;
                        _alertEntity.EmailCC = !string.IsNullOrEmpty(txtEmailCC.Text) ? txtEmailCC.Text.Trim() : null;
                        _alertEntity.EmailBcc = !string.IsNullOrEmpty(txtEmailCC.Text) ? txtEmailCC.Text.Trim() : null;
                        _alertEntity.Subject = !string.IsNullOrEmpty(txtsubject.Text) ? txtsubject.Text.Trim() : null;
                        //_alertEntity.EmailBody = !string.IsNullOrEmpty(txtemailBody.Text) ? txtemailBody.Text.Trim() : null;
                        //_alertEntity.SMSBody = !string.IsNullOrEmpty(txtsmsBody.Text) ? txtsmsBody.Text.Trim() : null;
                        string sanitizedEmailBody = SanitizeHtml(txtemailBody.Text.Trim());
                        string sanitizedSMSBody = SanitizeHtml(txtsmsBody.Text.Trim());
                        _alertEntity.EmailBody = !string.IsNullOrEmpty(sanitizedEmailBody) ? sanitizedEmailBody : null;
                        _alertEntity.SMSBody = !string.IsNullOrEmpty(sanitizedSMSBody) ? sanitizedSMSBody : null;
                        _alertEntity.AlertType = Convert.ToString(ddlAlertType.SelectedItem.Text) != "0" ? Convert.ToString(ddlAlertType.SelectedItem.Text) : "0";
                        _alertEntity.AlertSentOn = Convert.ToString(ddlAlertsentOn.SelectedValue) != "0" ? Convert.ToString(ddlAlertsentOn.SelectedValue) : "0";
                        _alertEntity.TimerInterval = !string.IsNullOrEmpty(txtTimerInterval.Text) ? txtTimerInterval.Text.Trim() : null;
                        _alertEntity.MaxRetryCount = !string.IsNullOrEmpty(txtmaxRetry.Text) ? txtmaxRetry.Text.Trim() : null;
                        _alertEntity.NextInterval = !string.IsNullOrEmpty(txtNxtInterval.Text) ? txtNxtInterval.Text.Trim() : null;
                        _alertEntity.StartTime = !string.IsNullOrEmpty(txtStartTime.Text) ? txtStartTime.Text.Trim() : null;
                        _alertEntity.EndTime = !string.IsNullOrEmpty(txtEndTime.Text) ? txtEndTime.Text.Trim() : null;
                        _alertEntity.Bc = Convert.ToString(ddlBClist.SelectedItem.Text) != "0" ? Convert.ToString(ddlBClist.SelectedItem.Text) : "0";
                        _alertEntity.Channels = Convert.ToString(ddlChannels.SelectedItem.Text) != "0" ? Convert.ToString(ddlChannels.SelectedItem.Text) : "0";
                        _alertEntity.ColumSelector = !string.IsNullOrEmpty(ddlColumnSelected.Text) ? ddlColumnSelected.Text.Trim() : null;
                        _alertEntity.TransactionType = Convert.ToString(ddlTxnType.SelectedItem.Text) != "0" ? Convert.ToString(ddlTxnType.SelectedItem.Text) : "0";
                        _alertEntity.DeclineCount = !string.IsNullOrEmpty(txtConsicativeDeclineCount.Text) ? txtConsicativeDeclineCount.Text.Trim() : null;

                        StringBuilder sbquery = new StringBuilder();
                        bool isFirstCondition = true;
                        foreach (GridViewRow row in gvConditions.Rows)
                        {

                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                DropDownList ddlAndOr = (DropDownList)row.FindControl("ddlAndOr");
                                DropDownList ddlFieldName = (DropDownList)row.FindControl("ddlFieldName");
                                DropDownList ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                                DropDownList ddlValue = (DropDownList)row.FindControl("ddlValue");
                                string andOr = ddlAndOr.SelectedValue;
                                string fieldName = ddlFieldName.SelectedValue;
                                string operatorValue = ddlOperator.SelectedValue;
                                string value = ddlValue.SelectedValue;

                                if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(operatorValue) || string.IsNullOrEmpty(value))
                                {
                                    continue;
                                }

                                if (!isFirstCondition)
                                {
                                    sbquery.Append($" {andOr} ");
                                }
                                sbquery.Append($"{fieldName} {operatorValue} {value}");
                                isFirstCondition = false;
                            }
                        }
                        _alertEntity.QuerySelector = sbquery.ToString();
                        _alertEntity.InsType = "members";
                        _alertEntity.flag = (int)EnumCollection.EnumRuleType.Edit;
                        string statusCode = _alertEntity.InsUpdateAlertDetails();
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
                        ErrorLog.RuleTrace("AlertConfig: EditGroup() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                    }
                    else
                    {
                        _alertEntity.CreatedBy = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _alertEntity.AlertGroupId = Convert.ToString(ddlGroupName.SelectedValue) != "0" ? Convert.ToString(ddlGroupName.SelectedValue) : "0";
                        _alertEntity.AlertSwitch = Convert.ToString(ddlSwitch.SelectedValue) != "0" ? Convert.ToString(ddlSwitch.SelectedValue) : "0";
                        _alertEntity.AlertGroupName = !string.IsNullOrEmpty(txtGroupName.Text) ? txtGroupName.Text.Trim() : null;
                        _alertEntity.GroupType = "";
                        _alertEntity.MemeberName = !string.IsNullOrEmpty(txtRuleName.Text) ? txtRuleName.Text.Trim() : null;
                        _alertEntity.Email = !string.IsNullOrEmpty(txtEmail.Text) ? txtEmail.Text.Trim() : null;
                        _alertEntity.Phone = !string.IsNullOrEmpty(txtMobile.Text) ? txtMobile.Text.Trim() : null;
                        _alertEntity.EmailCC = !string.IsNullOrEmpty(txtEmailCC.Text) ? txtEmailCC.Text.Trim() : null;
                        _alertEntity.EmailBcc = !string.IsNullOrEmpty(txtEmailCC.Text) ? txtEmailCC.Text.Trim() : null;
                        _alertEntity.Subject = !string.IsNullOrEmpty(txtsubject.Text) ? txtsubject.Text.Trim() : null;
                        //_alertEntity.EmailBody = !string.IsNullOrEmpty(txtemailBody.Text) ? txtemailBody.Text.Trim() : null;
                        //_alertEntity.SMSBody = !string.IsNullOrEmpty(txtsmsBody.Text) ? txtsmsBody.Text.Trim() : null;

                        string sanitizedEmailBody = SanitizeHtml(txtemailBody.Text.Trim());
                        string sanitizedSMSBody = SanitizeHtml(txtsmsBody.Text.Trim());
                        _alertEntity.EmailBody = !string.IsNullOrEmpty(sanitizedEmailBody) ? sanitizedEmailBody : null;
                        _alertEntity.SMSBody = !string.IsNullOrEmpty(sanitizedSMSBody) ? sanitizedSMSBody : null;
                        _alertEntity.AlertType = Convert.ToString(ddlAlertType.SelectedItem.Text) != "0" ? Convert.ToString(ddlAlertType.SelectedItem.Text) : "0";
                        _alertEntity.AlertSentOn = Convert.ToString(ddlAlertsentOn.SelectedValue) != "0" ? Convert.ToString(ddlAlertsentOn.SelectedValue) : "0";
                        _alertEntity.TimerInterval = !string.IsNullOrEmpty(txtTimerInterval.Text) ? txtTimerInterval.Text.Trim() : null;
                        _alertEntity.MaxRetryCount = !string.IsNullOrEmpty(txtmaxRetry.Text) ? txtmaxRetry.Text.Trim() : null;
                        _alertEntity.NextInterval = !string.IsNullOrEmpty(txtNxtInterval.Text) ? txtNxtInterval.Text.Trim() : null;
                        _alertEntity.StartTime = !string.IsNullOrEmpty(txtStartTime.Text) ? txtStartTime.Text.Trim() : null;
                        _alertEntity.EndTime = !string.IsNullOrEmpty(txtEndTime.Text) ? txtEndTime.Text.Trim() : null;
                        _alertEntity.Bc = Convert.ToString(ddlBClist.SelectedItem.Text) != "0" ? Convert.ToString(ddlBClist.SelectedItem.Text) : "0";
                        _alertEntity.Channels = Convert.ToString(ddlChannels.SelectedItem.Text) != "0" ? Convert.ToString(ddlChannels.SelectedItem.Text) : "0";
                        _alertEntity.ColumSelector = !string.IsNullOrEmpty(ddlColumnSelected.Text) ? ddlColumnSelected.Text.Trim() : null;
                        _alertEntity.TransactionType = Convert.ToString(ddlTxnType.SelectedItem.Text) != "0" ? Convert.ToString(ddlTxnType.SelectedItem.Text) : "0";
                        _alertEntity.DeclineCount = !string.IsNullOrEmpty(txtConsicativeDeclineCount.Text) ? txtConsicativeDeclineCount.Text.Trim() : null;
                        StringBuilder sbquery = new StringBuilder();
                        bool isFirstCondition = true;
                        foreach (GridViewRow row in gvConditions.Rows)
                        {

                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                DropDownList ddlAndOr = (DropDownList)row.FindControl("ddlAndOr");
                                DropDownList ddlFieldName = (DropDownList)row.FindControl("ddlFieldName");
                                DropDownList ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                                DropDownList ddlValue = (DropDownList)row.FindControl("ddlValue");
                                string andOr = ddlAndOr.SelectedValue;
                                string fieldName = ddlFieldName.SelectedValue;
                                string operatorValue = ddlOperator.SelectedValue;
                                string value = ddlValue.SelectedValue;

                                if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(operatorValue) || string.IsNullOrEmpty(value))
                                {
                                    continue;
                                }

                                if (!isFirstCondition)
                                {
                                    sbquery.Append($" {andOr} ");
                                }
                                sbquery.Append($"{fieldName} {operatorValue} {value}");
                                isFirstCondition = false;
                            }
                        }
                        _alertEntity.QuerySelector = sbquery.ToString();
                        _alertEntity.QuerySelector = txtQueryPreview.Text;
                        _alertEntity.InsType = "members";
                        _alertEntity.flag = (int)EnumCollection.EnumRuleType.Insert;
                        string statusCode = _alertEntity.InsUpdateAlertDetails();

                        if (statusCode == "INS00")
                        {
                            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        }
                        else if (statusCode == "EXIT00")
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
                        ErrorLog.RuleTrace("AlertConfig: btnCreGroup_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                    }
                }
                BindGroup();
                hdnShowModalR.Value = "false";
                ClearLocalStorage();
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

                _alertEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                var tables = _alertEntity.GetDropDownValues();
                dt = _alertEntity.getAlertTypeList(null);

                if (tables != null)
                {
                    if (tables.ContainsKey("temp_bindAlertGroup"))
                    {
                        DataTable bindGroupTable = tables["temp_bindAlertGroup"];

                        if (bindGroupTable.Rows.Count > 0)
                        {
                            ddlGroupName.Items.Clear();
                            ddlGroupName.DataSource = bindGroupTable;
                            ddlGroupName.DataTextField = "groupname";
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


                    if (tables.ContainsKey("temp_bindAlertBCList"))
                    {
                        DataTable bindGroupTable = tables["temp_bindAlertBCList"];

                        if (bindGroupTable.Rows.Count > 0)
                        {
                            ddlBClist.Items.Clear();
                            ddlBClist.DataSource = bindGroupTable;
                            ddlBClist.DataTextField = "bcname";
                            ddlBClist.DataValueField = "id";
                            ddlBClist.DataBind();
                            ddlBClist.Items.Insert(0, new ListItem("--Select--", "0"));
                            ddlBClist.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlBClist.Items.Clear();
                            ddlBClist.DataSource = null;
                            ddlBClist.DataBind();
                        }
                    }

                }

                ddlAlertType.Items.Clear();
                ddlAlertType.DataSource = dt;
                ddlAlertType.DataTextField = "alerttypename";
                ddlAlertType.DataValueField = "id";
                ddlAlertType.DataBind();
                ddlAlertType.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlAlertType.SelectedIndex = 0;

            }
            catch (Exception Ex)
            {
                ErrorLog.AdminManagementTrace("pgUserAccessManagement: BindDropdownClientDetails: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }


        protected void ddlBClist_SelectedIndexChanged(object sender, EventArgs e)
        {


            string bcid = ddlBClist.SelectedValue;
            DataTable channel = _alertEntity.getChannelListofBCs(bcid);

            if (channel != null)
            {
                if (channel.Rows.Count > 0)
                {
                    ddlChannels.Items.Clear();
                    ddlChannels.DataSource = channel;
                    ddlChannels.DataTextField = "channel";
                    ddlChannels.DataValueField = "bcid";
                    ddlChannels.DataBind();
                    ddlChannels.Items.Insert(0, new ListItem("--Select--", "0"));
                    ddlChannels.SelectedIndex = 0;
                }
                else
                {
                    ddlChannels.Items.Clear();
                    ddlChannels.DataSource = null;
                    ddlChannels.DataBind();
                }
            }
        }

        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {

            string channels = ddlBClist.SelectedValue;
            DataSet channel = _alertEntity.GetAllDllData(channels);

            //QueryBindGrid(); //uncooment when use

            if (channel != null)
            {
                if (channel.Tables.Contains("ColumnList"))
                {

                    ddlColumnSelected.Items.Clear();
                    ddlColumnSelected.DataSource = channel.Tables["ColumnList"];
                    ddlColumnSelected.DataTextField = "columnsname";
                    ddlColumnSelected.DataValueField = "actualcolumnname";
                    ddlColumnSelected.DataBind();
                    ddlColumnSelected.Items.Insert(0, new ListItem("--Select--", "0"));
                    ddlColumnSelected.SelectedIndex = 0;
                    var responseCodes = (from row in channel.Tables["ResponseCode"].AsEnumerable()
                                         select new ListItem(
                                             row["code"].ToString() + " - " + row["descriptions"].ToString(),
                                             row["code"].ToString()
                                         )).ToList();
                    ddlResponseCode.Items.Clear();
                    ddlResponseCode.DataSource = responseCodes;
                    ddlResponseCode.DataTextField = "Text";
                    ddlResponseCode.DataValueField = "Value";
                    ddlResponseCode.DataBind();
                    ddlResponseCode.Items.Insert(0, new ListItem("--Select--", "0"));
                    ddlResponseCode.SelectedIndex = 0;

                }
                else
                {
                    ddlColumnSelected.Items.Clear();
                    ddlColumnSelected.DataSource = null;
                    ddlColumnSelected.DataBind();

                    ddlResponseCode.Items.Clear();
                    ddlResponseCode.DataSource = null;
                    ddlResponseCode.DataBind();
                }
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

                    string[] itemId = e.CommandArgument.ToString().Split(',');
                    _alertEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                    _alertEntity.AlertMemberId = Convert.ToString(itemId[0]);
                    _alertEntity.AlertGroupId = Convert.ToString(itemId[1]);
                    Session["RuleId"] = _alertEntity.AlertMemberId;
                    _alertEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                    dt = _alertEntity.getAletConfigEdit();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];

                        Session["RuleId"] = row["id"] != DBNull.Value ? row["id"].ToString() : string.Empty;
                        BindDropdownValues();

                        string channels = ddlBClist.SelectedValue;
                        DataSet channel = _alertEntity.GetAllDllData(channels);

                        if (channel != null)
                        {
                            if (channel.Tables.Contains("ColumnList"))
                            {

                                ddlColumnSelected.Items.Clear();
                                ddlColumnSelected.DataSource = channel.Tables["ColumnList"];
                                ddlColumnSelected.DataTextField = "columnsname";
                                ddlColumnSelected.DataValueField = "actualcolumnname";
                                ddlColumnSelected.DataBind();
                                ddlColumnSelected.Items.Insert(0, new ListItem("--Select--", "0"));
                                ddlColumnSelected.SelectedIndex = 0;

                                var responseCodes = (from r in channel.Tables["ResponseCode"].AsEnumerable()
                                                     select new ListItem(
                                                         r["code"].ToString() + " - " + r["descriptions"].ToString(),
                                                         r["code"].ToString()
                                                     )).ToList();


                                ddlResponseCode.Items.Clear();
                                ddlResponseCode.Items.Insert(0, new ListItem("--Select--", "0"));
                                ddlResponseCode.DataSource = responseCodes;
                                ddlResponseCode.DataBind();
                                ddlResponseCode.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlColumnSelected.Items.Clear();
                                ddlColumnSelected.DataSource = null;
                                ddlColumnSelected.DataBind();

                                ddlResponseCode.Items.Clear();
                                ddlResponseCode.DataSource = null;
                                ddlResponseCode.DataBind();
                            }
                        }

                        ddlGroupName.SelectedValue = row["gid"] != DBNull.Value ? row["gid"].ToString() : string.Empty;
                        ddlBClist.SelectedItem.Text = row["bcname"] != DBNull.Value ? row["bcname"].ToString() : string.Empty;
                        ddlChannels.SelectedItem.Text = row["channel"] != DBNull.Value ? row["channel"].ToString() : string.Empty;
                        ddlTxnType.SelectedItem.Text = row["transactiontype"] != DBNull.Value ? row["transactiontype"].ToString() : string.Empty;
                        ddlSwitch.SelectedItem.Text = row["switchs"] != DBNull.Value ? row["switchs"].ToString() : string.Empty;
                        ddlAlertType.SelectedItem.Text = row["alerttype"] != DBNull.Value ? row["alerttype"].ToString() : string.Empty;

                        txtRuleName.Text = row["contactname"] != DBNull.Value ? row["contactname"].ToString() : string.Empty;
                        txtEmail.Text = row["mailid"] != DBNull.Value ? row["mailid"].ToString() : string.Empty;
                        txtMobile.Text = row["mobile"] != DBNull.Value ? row["mobile"].ToString() : string.Empty;
                        txtEmailCC.Text = row["emailcc"] != DBNull.Value ? row["emailcc"].ToString() : string.Empty;
                        txtsubject.Text = row["subject"] != DBNull.Value ? row["subject"].ToString() : string.Empty;

                        txtemailBody.Text = row["mailbody"] != DBNull.Value ? row["mailbody"].ToString() : string.Empty;
                        txtsmsBody.Text = row["smsbody"] != DBNull.Value ? row["smsbody"].ToString() : string.Empty;

                        txtalertType.Text = row["alerttype"] != DBNull.Value ? row["alerttype"].ToString() : string.Empty;
                        ddlAlertsentOn.SelectedValue = row["alertmode"] != DBNull.Value ? row["alertmode"].ToString() : "3";
                        txtTimerInterval.Text = row["timerinterval"] != DBNull.Value ? row["timerinterval"].ToString() : string.Empty;
                        txtmaxRetry.Text = row["maxretrycount"] != DBNull.Value ? row["maxretrycount"].ToString() : string.Empty;
                        txtNxtInterval.Text = row["nextinterval"] != DBNull.Value ? row["nextinterval"].ToString() : string.Empty;
                        txtStartTime.Text = row["starttime"] != DBNull.Value ? row["starttime"].ToString() : string.Empty;
                        txtEndTime.Text = row["endtime"] != DBNull.Value ? row["endtime"].ToString() : string.Empty;

                        Session["priority2"] = "IsEdit";
                        _alertEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        DataTable dt2 = new DataTable();

                        hdnShowModalR.Value = "true";
                    }
                }
                else if (e.CommandName == "DeleteRule")
                {
                    string[] itemId = e.CommandArgument.ToString().Split(',');
                    _alertEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                    _alertEntity.AlertMemberId = Convert.ToString(itemId[0]);
                    _alertEntity.AlertGroupId = Convert.ToString(itemId[1]);
                    _alertEntity.InsType = "members";
                    _alertEntity.Flag = (int)EnumCollection.EnumRuleType.Delete;
                    string statusCode = _alertEntity.InsUpdateAlertDetails();

                    if (statusCode == "DEL00")
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
                    ErrorLog.RuleTrace("AlertGroupConfig: Delete() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                    _alertEntity.getAlertMembers();
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

                    if (dataItem.Row.Table.Columns.Contains("isactive"))
                    {
                        string isActive = dataItem["isactive"].ToString();
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
                        Console.WriteLine("'isactive' column is missing.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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
            if (columnName == "bcpartner_id")
            {
                literalControl.Text = string.Join("", sourceTable.AsEnumerable().Select(row =>
                {
                    string value = row[columnName].ToString();
                    string isChecked = value == selectedValue ? "checked" : "";
                    return $"<label><input type='checkbox' name='chkAggregator' value='{value}' {isChecked} /> {value}</label><br/>";
                }));
            }
            if (columnName == "iin")
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
                return "Priorites updated successful.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

        }


        public string SanitizeHtml(string htmlContent)
        {
            var sanitizer = new HtmlSanitizer();

            // Allow common text formatting tags
            sanitizer.AllowedTags.Add("b");
            sanitizer.AllowedTags.Add("i");
            sanitizer.AllowedTags.Add("u");
            sanitizer.AllowedTags.Add("p");
            sanitizer.AllowedTags.Add("ul");
            sanitizer.AllowedTags.Add("ol");
            sanitizer.AllowedTags.Add("li");
            // Allow table-related tags
            sanitizer.AllowedTags.Add("table");
            sanitizer.AllowedTags.Add("tr");
            sanitizer.AllowedTags.Add("td");
            sanitizer.AllowedTags.Add("th");
            sanitizer.AllowedTags.Add("thead");
            sanitizer.AllowedTags.Add("tbody");
            sanitizer.AllowedTags.Add("tfoot");
            // Allow the 'style' attribute to support inline CSS
            sanitizer.AllowedAttributes.Add("style");
            // Allow these common style attributes:
            // - color, background-color, font-size, width, height, etc.
            sanitizer.AllowedCssProperties.Add("color");
            sanitizer.AllowedCssProperties.Add("background-color");
            sanitizer.AllowedCssProperties.Add("font-size");
            sanitizer.AllowedCssProperties.Add("font-family");
            sanitizer.AllowedCssProperties.Add("text-align");
            sanitizer.AllowedCssProperties.Add("border");
            sanitizer.AllowedCssProperties.Add("border-radius");
            // Optionally, you can allow other attributes (e.g., 'class', 'id', 'width', etc.)
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedAttributes.Add("id");
            sanitizer.AllowedAttributes.Add("width");
            sanitizer.AllowedAttributes.Add("height");

            // Return the sanitized HTML content
            return sanitizer.Sanitize(htmlContent);
        }


        protected void gvConditions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlFieldName = (DropDownList)e.Row.FindControl("ddlFieldName");
                DropDownList ddlOperator = (DropDownList)e.Row.FindControl("ddlOperator");
                DropDownList ddlValue = (DropDownList)e.Row.FindControl("ddlValue");

                string bcid = ddlChannels.SelectedValue;

                if (!string.IsNullOrEmpty(bcid))
                {
                    var ds = _alertEntity.GetAllDllData();

                    if (ds.Tables.Contains("ColumnList"))
                    {
                        ddlFieldName.Items.Clear();
                        ddlFieldName.Items.Add(new ListItem("Select Field", ""));
                        foreach (DataRow row in ds.Tables["ColumnList"].Rows)
                        {
                            string actualColumnName = row["actualcolumnname"].ToString();
                            string columnName = row["columnsname"].ToString();
                            ddlFieldName.Items.Add(new ListItem(columnName, actualColumnName));
                            ddlColumnSelected.Items.Add(new ListItem(columnName, actualColumnName));
                        }
                    }

                    if (ds.Tables.Contains("Operators"))
                    {
                        ddlOperator.Items.Clear();
                        ddlOperator.Items.Add(new ListItem("Select Field", ""));
                        foreach (DataRow row in ds.Tables["Operators"].Rows)
                        {
                            string id = row["id"].ToString();
                            string operatorname = row["operatorname"].ToString();
                            ddlOperator.Items.Add(new ListItem(operatorname, operatorname));
                        }
                    }

                    if (ds.Tables.Contains("ResponseCode"))
                    {
                        ddlValue.Items.Clear();
                        ddlValue.Items.Add(new ListItem("Select Field", ""));
                        foreach (DataRow row in ds.Tables["ResponseCode"].Rows)
                        {
                            string RC = row["code"].ToString() + "-" + row["descriptions"].ToString(); ;
                            string ResponseCode = row["code"].ToString();
                            ddlValue.Items.Add(new ListItem(RC, ResponseCode));
                        }

                    }

                }

            }
        }


        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            var newRow = new GridViewRow(gvConditions.Rows.Count, -1, DataControlRowType.DataRow, DataControlRowState.Normal);

            TableCell cellAndOr = new TableCell();
            DropDownList ddlAndOr = new DropDownList();
            ddlAndOr.CssClass = "form-control"; // Ensures consistent styling
            ddlAndOr.Items.Add(new ListItem("AND", "AND"));
            ddlAndOr.Items.Add(new ListItem("OR", "OR"));
            cellAndOr.Controls.Add(ddlAndOr);
            newRow.Cells.Add(cellAndOr);


            TableCell cellFieldName = new TableCell();
            DropDownList ddlFieldName = new DropDownList();
            ddlFieldName.CssClass = "form-control";
            string bcid = ddlChannels.SelectedValue;

            var ds = _alertEntity.GetAllDllData();
            if (!string.IsNullOrEmpty(bcid))
            {
                if (ds.Tables.Contains("ColumnList"))
                {
                    ddlFieldName.Items.Clear();
                    ddlFieldName.Items.Add(new ListItem("Select Field", ""));
                    foreach (DataRow row in ds.Tables["ColumnList"].Rows)
                    {
                        string actualColumnName = row["actualcolumnname"].ToString();
                        string columnName = row["columnsname"].ToString();
                        ddlFieldName.Items.Add(new ListItem(columnName, actualColumnName));
                    }
                }
            }
            cellFieldName.Controls.Add(ddlFieldName);
            newRow.Cells.Add(cellFieldName);

            TableCell cellOperator = new TableCell();
            DropDownList ddlOperator = new DropDownList();
            ddlOperator.CssClass = "form-control";

            if (ds.Tables.Contains("Operators"))
            {
                ddlOperator.Items.Clear();
                ddlOperator.Items.Add(new ListItem("Select Operator", ""));
                foreach (DataRow row in ds.Tables["Operators"].Rows)
                {
                    string operatorname = row["operatorname"].ToString();
                    ddlOperator.Items.Add(new ListItem(operatorname, operatorname));
                }
            }
            cellOperator.Controls.Add(ddlOperator);
            newRow.Cells.Add(cellOperator);


            TableCell cellValue = new TableCell();
            DropDownList ddlValue = new DropDownList();
            ddlValue.CssClass = "form-control";

            if (ds.Tables.Contains("ResponseCode"))
            {
                ddlValue.Items.Clear();
                ddlValue.Items.Add(new ListItem("Select Value", ""));
                foreach (DataRow row in ds.Tables["ResponseCode"].Rows)
                {
                    string RC = row["code"].ToString() + " - " + row["descriptions"].ToString();
                    string ResponseCode = row["code"].ToString();
                    ddlValue.Items.Add(new ListItem(RC, ResponseCode));
                }
            }
            cellValue.Controls.Add(ddlValue);
            newRow.Cells.Add(cellValue);

            TableCell cellActions = new TableCell();


            var btnContainer = new HtmlGenericControl("div");
            btnContainer.Attributes.Add("style", "text-align: center; padding: 5px;");

            Button btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.CssClass = "btn btn-primary btn-sm";
            btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this row?');";

            Button btnAddRow = new Button();
            btnAddRow.Text = "Add";
            btnAddRow.CssClass = "btn btn-secondary btn-sm";
            btnAddRow.Click += btnAddRow_Click;

            btnContainer.Controls.Add(btnDelete);
            btnContainer.Controls.Add(btnAddRow);
            cellActions.Controls.Add(btnContainer);
            newRow.Cells.Add(cellActions);
            gvConditions.Controls[0].Controls.AddAt(gvConditions.Rows.Count, newRow);
        }


        protected void btnAddCondition_Click(object sender, EventArgs e)
        {
            string declineCount = txtConsicativeDeclineCount.Text.Trim();
            string txnType = ddlTxnType.SelectedValue;
            string columnField = ddlColumnSelected.SelectedValue;
            string ValueSelected = hdnValueSelc.Value;
            string retryInterval = txtTimerInterval.Text.Trim();
            string maxRetryCount = txtmaxRetry.Text.Trim();
            string nextInterval = txtNxtInterval.Text.Trim();

            StringBuilder newCondition = new StringBuilder();
            bool anyConditionAdded = false;

            //if (!string.IsNullOrEmpty(declineCount))
            //{
            //    string declineOperator = ddlConditions.SelectedValue;
            //    newCondition.Append($"DeclineCount {declineOperator} {declineCount}");
            //    anyConditionAdded = true;
            //}


            //if (txnType != "0")
            //{
            //    if (anyConditionAdded) newCondition.Append($" {ddlOperator0.SelectedValue} ");
            //    string transactionOperator = ddlConditions.SelectedValue;
            //    newCondition.Append($"TransactionType {transactionOperator} {txnType}");
            //    anyConditionAdded = true;
            //}

            if (!string.IsNullOrEmpty(columnField))
            {
                if (!string.IsNullOrWhiteSpace(txtQueryPreview.Text))
                {
                    newCondition.Append($" {ddlOperator0.SelectedValue} ");
                }
                string columnOperator = ddlConditions.SelectedValue;
                newCondition.Append($"{columnField} {columnOperator} '{ValueSelected}'");
                anyConditionAdded = true;
            }

            //if (!string.IsNullOrEmpty(retryInterval))
            //{
            //    if (anyConditionAdded) newCondition.Append($" {ddlOperator0.SelectedValue} ");
            //    string retryOperator = ddlConditions.SelectedValue;
            //    newCondition.Append($"RetryCount {retryOperator} {retryInterval}");
            //    anyConditionAdded = true;
            //}

            //if (!string.IsNullOrEmpty(maxRetryCount))
            //{
            //    if (anyConditionAdded) newCondition.Append($" {ddlOperator0.SelectedValue} ");
            //    string maxRetryOperator = ddlConditions.SelectedValue;
            //    newCondition.Append($"MaxRetryCount {maxRetryOperator} {maxRetryCount}");
            //    anyConditionAdded = true;
            //}


            //if (!string.IsNullOrEmpty(nextInterval))
            //{
            //    if (anyConditionAdded) newCondition.Append($" {ddlOperator0.SelectedValue} ");
            //    string nextOperator = ddlConditions.SelectedValue;
            //    newCondition.Append($"NextInterval {nextOperator} {nextInterval}");
            //    anyConditionAdded = true;
            //}


            if (newCondition.Length > 0)
            {
                conditions.Add(newCondition.ToString());
                ViewState["Conditions"] = conditions;
            }


            txtQueryPreview.Text = string.Join(" ", conditions);
        }

        protected void btnClearCondition_Click(object sender, EventArgs e)
        {
            txtQueryPreview.Text = string.Empty;
            ViewState["Conditions"] = new List<string>();
        }


        public void ClearLocalStorage()
        {
            try
            {
                txtQueryPreview.Text = string.Empty;
                string script = "localStorage.setItem('Conditions', JSON.stringify([]));";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ClearConditions", script, true);
            }
            catch (Exception ex)
            {
                ErrorLog.RuleTrace("AlertGroupConfig: ClearLocalStorage(): Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
    }
}