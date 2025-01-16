using AppLogger;
using BussinessAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SOR.Pages.Rule
{
    public partial class SwitchConfig : System.Web.UI.Page
    {
        #region Object Declaration
        private DataTable bindtxntype;
        public static RuleEntity _RuleEntityy = new RuleEntity(); // Initialize it here
        public static CommonEntity _CommonEntity = new CommonEntity(); // Initialize it here
        RuleEntity _RuleEntity = new RuleEntity();
        DataTable dt = new DataTable();
        private DataTable dtSwitchValues;
        DataTable updatesTable = new DataTable();
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("SwitchConfig | Page_Load() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "SwitchConfig.aspx", "8");
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
                            Session["Username"] = "Maximus";
                            txtSwitchName.Enabled = true;
                            BindSwitch();
                            BindDropdown();
                            InitializeDataTable();
                            BindManualDisableSwitch();
                            UserPermissions.RegisterStartupScriptForNavigationListActive("3", "8");
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
                ErrorLog.DashboardTrace("SwitchConfig : Page_Load(): Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        private void BindSwitch()
        {
            try
            {
                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                DataSet dt = _RuleEntity.GetSwitch();

                if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                {
                    rptrSwitch.DataSource = dt;
                    rptrSwitch.DataBind();

                    rptSwitchDetails.DataSource = dt;
                    rptSwitchDetails.DataBind();

                    rptSwitchDetailsDelete.DataSource = dt;
                    rptSwitchDetailsDelete.DataBind();

                    rptrSwitch.Visible = true;
                    rptSwitchDetails.Visible = true;
                    rptSwitchDetailsDelete.Visible = true;
                }
                else
                {
                    rptrSwitch.Visible = false;
                    rptSwitchDetails.Visible = false;
                    rptSwitchDetailsDelete.Visible = false;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("SwitchConfig : BindSwitch(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        private void BindManualDisableSwitch()
        {
            try
            {
                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
                DataSet dt = _RuleEntity.GetManualDisableSwitch();

                rptManual.DataSource = dt;
                rptManual.DataBind();
            }
            catch (Exception Ex)
            {
                ErrorLog.DashboardTrace("SwitchConfig : BindSwitch(): Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        protected void btnAddSwitch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("SwitchConfig | btnAddSwitch_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "SwitchConfig";
                _auditParams[2] = "btnAddSwitch_Click";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                Session["SwitchPercentage"] = null;
                

                txtSwitchName.Enabled = true;
                txtSwitchName.Text = string.Empty;
                txtSwitchDescription.Text = string.Empty;
                txtSwitchPercentage.Text = string.Empty;
                txtCount.Text = string.Empty;
                ddlSwitch1.ClearSelection();
                ddlSwitch2.ClearSelection();
                ddlSwitch3.ClearSelection();
                ddlSwitch4.ClearSelection();
                ddlSwitch5.ClearSelection();
                ddlSwitch6.ClearSelection();

                BindSwitch();
                BindDropdown();

                hdnShowModal.Value = "true";
                hdnshowmanual.Value = "false";
                HdnShowDelete.Value = "false";

                ErrorLog.RuleTrace("SwitchConfig | btnAddSwitch_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: btnAddGroup_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
            }
        }
        private void AddSwitchValue(DropDownList ddl, TextBox txt)
        {
            // Check if dropdown is selected and textbox is not empty
            if (ddl.SelectedIndex > 0 && !string.IsNullOrEmpty(txt.Text))
            {
                // Create a new DataRow
                DataRow row = dtSwitchValues.NewRow();
                row["SwitchName"] = ddl.SelectedItem.Text; // Use the text of the dropdown item
                row["SwitchValue"] = txt.Text; // Use the text box value

                // Add the DataRow to the DataTable
                dtSwitchValues.Rows.Add(row);
            }
        }

        protected void btnCreSwitch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("SwitchConfig | btnCreSwitch_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                
                if (string.IsNullOrEmpty(txtSwitchName.Text))
                {
                    ShowWarning("Please Enter Switch Name. Try again", "Warning");
                    return;
                }
                if (string.IsNullOrEmpty(txtSwitchDescription.Text))
                {
                    ShowWarning("Please Enter Switch Description. Try again", "Warning");
                    return;
                }

                if (string.IsNullOrEmpty(txtSwitchPercentage.Text) && string.IsNullOrEmpty(txtCount.Text))
                {
                    ShowWarning("Please Enter Switch Percentage Or Switch Count. Try again", "Warning");
                    return;
                }
                
                if (!CheckForDuplicateValues())
                {
                    return;
                }
                //if (!ValidateSwitchValues())
                //{
                //    return;
                //}
                if (!ValidateSwitchInputs())
                {
                    return;
                }

                if (string.IsNullOrEmpty(ddlSwitch1.SelectedValue) &&
                    string.IsNullOrEmpty(ddlSwitch2.SelectedValue) &&
                    string.IsNullOrEmpty(ddlSwitch3.SelectedValue) &&
                    string.IsNullOrEmpty(ddlSwitch4.SelectedValue) &&
                    string.IsNullOrEmpty(ddlSwitch5.SelectedValue) &&
                    string.IsNullOrEmpty(ddlSwitch6.SelectedValue))
                {
                    if (dtSwitchValues == null)
                    {
                        InitializeDataTable();
                    }
                    //ShowWarning("Please Enter Failover Switch Percentage. Try again", "Warning");
                    //return;
                }
                else
                {
                    if (!PopulateAndAddSwitchValues())
                    {
                        return;
                    }
                }


                #region DT
                DataTable resultTable = new DataTable();
                resultTable.Columns.Add("ID", typeof(string));
                resultTable.Columns.Add("Value", typeof(string));
                resultTable.Columns.Add("TabValue", typeof(string));
                #endregion

                if (string.IsNullOrEmpty(txtCount.Text))
                {
                    #region % Validation
                    decimal totalPercentage = 0;

                    // Iterate through the Repeater items to sum up percentages
                    foreach (RepeaterItem item in rptSwitchDetails.Items)
                    {
                        TextBox txtPercentage = (TextBox)item.FindControl("txtPercentage");

                        if (decimal.TryParse(txtPercentage.Text, out decimal percentage))
                        {
                            totalPercentage += percentage;
                        }
                    }

                    // Add the percentage from txtSwitchPercentage (outside the Repeater)
                    if (decimal.TryParse(txtSwitchPercentage.Text, out decimal switchPercentage))
                    {
                        totalPercentage += switchPercentage;
                    }

                    // Check if the total percentage exceeds 100
                    if (totalPercentage > 100 || totalPercentage < 100)
                    {
                        // Show an error message or handle the validation failure
                        lblErrorMessage.Text = "Total percentage cannot exceed or less 100%.";
                        lblErrorMessage.Visible = true;
                        return;
                    }

                    // Proceed with rule creation if total percentage is valid
                    lblErrorMessage.Visible = false;

                    updatesTable = CreateUpdatesDataTable();
                    // Iterate through the Repeater items
                    foreach (RepeaterItem item in rptSwitchDetails.Items)
                    {
                        // Find controls
                        var lblID = (Label)item.FindControl("lblID");
                        var txtPercentage = (TextBox)item.FindControl("txtPercentage");

                        if (lblID != null && txtPercentage != null)
                        {
                            int id = Convert.ToInt32(lblID.Text);
                            decimal percentage = Convert.ToDecimal(txtPercentage.Text);

                            // Add a new row to the DataTable
                            updatesTable.Rows.Add(id, percentage);
                        }
                    }
                    #endregion
                }
                else
                {
                    updatesTable = CreateUpdatesDataTable();

                    decimal totalPercentagee = 0;

                    // Iterate through the Repeater items
                    foreach (RepeaterItem item in rptSwitchDetails.Items)
                    {
                        var lblID = (Label)item.FindControl("lblID");
                        var txtPercentage = (TextBox)item.FindControl("txtPercentage");

                        if (lblID != null && txtPercentage != null)
                        {
                            decimal percentage = Convert.ToDecimal(txtPercentage.Text);
                            totalPercentagee += percentage;
                            updatesTable.Rows.Add(Convert.ToInt32(lblID.Text), percentage);
                        }
                    }

                    // Adjust last row to make the total percentage 100
                    if (totalPercentagee != 100 && updatesTable.Rows.Count > 0)
                    {
                        updatesTable.Rows[updatesTable.Rows.Count - 1]["Percentage"] = 100 - totalPercentagee + Convert.ToDecimal(updatesTable.Rows[updatesTable.Rows.Count - 1]["Percentage"]);
                    }
                }

                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "SwitchConfig";
                _auditParams[2] = "btnCreSwitch";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                if (Session["SwitchPercentage"] != null)
                {

                    foreach (string key in Request.Form.Keys)
                    {
                        if (key.StartsWith("input_"))
                        {
                            // Extract the tab and ID from the name
                            string[] parts = key.Split('_');
                            if (parts.Length == 3) // Expecting input_{Tab}_{ID}
                            {
                                string tab = parts[1]; // Tab name (AEPS, DMT, MATM)
                                string id = parts[2]; // Extract the ID
                                string value = Request.Form[key]; // Get the value from the input

                                // Add a new row to the DataTable
                                DataRow newRow = resultTable.NewRow();
                                newRow["ID"] = $"{id}"; // Combine tab and ID if necessary
                                newRow["Value"] = value;
                                newRow["TabValue"] = tab;
                                if (!string.IsNullOrEmpty(value))
                                {
                                    resultTable.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                    if (resultTable.Rows.Count == 0)
                    {
                        ShowWarning("Please Enter Txn Type Url. Try again", "Warning");
                        return;
                    }
                    _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                    _RuleEntity.SwitchName = !string.IsNullOrEmpty(txtSwitchName.Text) ? txtSwitchName.Text.Trim() : null;
                    _RuleEntity.SwitchDescription = !string.IsNullOrEmpty(txtSwitchDescription.Text) ? txtSwitchDescription.Text.Trim() : null;
                    if (!string.IsNullOrEmpty(hdnSwitchId.Value))
                    {
                        _RuleEntity.SwitchId = Convert.ToInt32(hdnSwitchId.Value) != 0 ? Convert.ToInt32(hdnSwitchId.Value) : 0;
                    }
                    else
                    {

                    }
                    _RuleEntity.percentage = int.TryParse(txtSwitchPercentage.Text.Trim(), out int percentage) ? percentage : 0;
                    _RuleEntity.Count = int.TryParse(txtCount.Text.Trim(), out int count) ? count : 0;
                    //_RuleEntity.Failoverpercentage = int.TryParse(txtPercentageFailover1.Value.Trim(), out int Failoverpercentage) ? Failoverpercentage : 0;
                    _RuleEntity.dt = updatesTable;
                    _RuleEntity.dt2 = resultTable;
                    _RuleEntity.dt3 = dtSwitchValues;
                    _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Edit;

                    //string validateCode = _RuleEntity.ValidateSwitch();
                    //if(validateCode=="00")
                    //{
                    string statusCode = _RuleEntity.InsertOrUpdateSwitch();
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
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Switch Name Already Exists. Try again', 'Warning');", true);
                    //    return;
                    //}
                }
                else
                {
                    txtSwitchName.Enabled = true;

                    // Loop through all input fields to get the values
                    foreach (string key in Request.Form.Keys)
                    {
                        if (key.StartsWith("input_"))
                        {
                            // Extract the tab and ID from the name
                            string[] parts = key.Split('_');
                            if (parts.Length == 3) // Expecting input_{Tab}_{ID}
                            {
                                string tab = parts[1]; // Tab name (AEPS, DMT, MATM)
                                string id = parts[2]; // Extract the ID
                                string value = Request.Form[key]; // Get the value from the input

                                // Add a new row to the DataTable
                                DataRow newRow = resultTable.NewRow();
                                newRow["ID"] = $"{id}"; // Combine tab and ID if necessary
                                newRow["Value"] = value;
                                newRow["TabValue"] = tab;

                                if (!string.IsNullOrEmpty(value))
                                {
                                    resultTable.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                    if (resultTable.Rows.Count == 0)
                    {
                        ShowWarning("Please Enter Txn Type Url. Try again", "Warning");
                        return;
                    }
                    _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                    _RuleEntity.SwitchName = !string.IsNullOrEmpty(txtSwitchName.Text) ? txtSwitchName.Text.Trim() : null;
                    _RuleEntity.SwitchDescription = !string.IsNullOrEmpty(txtSwitchDescription.Text) ? txtSwitchDescription.Text.Trim() : null;
                    _RuleEntity.percentage = int.TryParse(txtSwitchPercentage.Text.Trim(), out int percentage) ? percentage : 0;
                    _RuleEntity.Count = int.TryParse(txtCount.Text.Trim(), out int count) ? count : 0;
                    //_RuleEntity.FailoverSwitchId = !string.IsNullOrEmpty(hfSelectedValues.Value) ? hfSelectedValues.Value.Trim() : null;
                    //_RuleEntity.Failoverpercentage = int.TryParse(txtPercentageFailover1.Value.Trim(), out int Failoverpercentage) ? Failoverpercentage : 0;
                    _RuleEntity.dt = updatesTable;
                    _RuleEntity.dt2 = resultTable;//Txn Url
                    _RuleEntity.dt3 = dtSwitchValues;//Failover Switch
                    //_RuleEntity.Switchurl = !string.IsNullOrEmpty(txturl.Text) ? txturl.Text.Trim() : null;
                    //_RuleEntity.TxnType = !string.IsNullOrEmpty(hfSelectedSecondValues.Value) ? hfSelectedSecondValues.Value.Trim() : null;
                    _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Insert;
                    string validateCode = _RuleEntity.ValidateSwitch();
                    if (validateCode == "00")
                    {
                        string statusCode = _RuleEntity.InsertOrUpdateSwitch();
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
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Switch Name Already Exists. Try again', 'Warning');", true);
                        return;
                    }
                }

                hdnShowModal.Value = "false";
                BindSwitch();
                ErrorLog.RuleTrace("SwitchConfig | btnCreSwitch_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                hdnShowModal.Value = "false";
                ErrorLog.RuleTrace("TrRule: btnCreGroup_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
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
        protected void btnCloseSwitch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("SwitchConfig | btnCloseSwitch_Click() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                hdnShowModal.Value = "false";
                ErrorLog.RuleTrace("SwitchConfig | btnCloseSwitch_Click() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: btnAddGroup_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
            }
        }
        [WebMethod]
        public static string ToggleSlider(bool IsChecked, string Id)
        {
            LoginEntity _LoginEntity = new LoginEntity();
            string[] _auditParams = new string[4];
            ErrorLog.RuleTrace("SwitchConfig | ToggleSlider() | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            bool isActive = IsChecked;
            _RuleEntityy.IsActive = isActive ? 0 : 1;
            _RuleEntityy.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
            _RuleEntityy.SwitchId = Convert.ToInt32(Id);
            _RuleEntityy.Flag = (int)EnumCollection.EnumRuleType.Update;
            #region Audit
            _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
            _auditParams[1] = "SwitchConfig";
            _auditParams[2] = "ToggleSlider-UpdateSwitchStatus";
            _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
            _LoginEntity.StoreLoginActivities(_auditParams);
            #endregion
            //string validateCode = _RuleEntityy.ValidateFailoverSwitch();
            //if (validateCode == "00")
            //{
                string statusCode = _RuleEntityy.UpdateSwitchStatus();

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
                ErrorLog.RuleTrace("SwitchConfig: ToggleSlider(): DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                return new JavaScriptSerializer().Serialize(response);
            //}
            //else
            //{
            //    var response = new
            //    {
            //        StatusMessage = "This Switch Already Use In The Failover Switch"
            //    };
            //    ErrorLog.RuleTrace("SwitchConfig: ToggleSlider(): This Switch Already Use In The Failover Switch | DB_ValidateCode : " + validateCode);
            //    return new JavaScriptSerializer().Serialize(response);
            //}
            //ErrorLog.RuleTrace("SwitchConfig | ToggleSlider() | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
        }

        protected void rptrSwitch_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    var dataItem = (DataRowView)e.Item.DataItem;
                    int groupId = Convert.ToInt32(dataItem["id"]);
                    var parentDataItem = e.Item.DataItem;
                    string isActive = dataItem["switchstatus"].ToString(); // Assuming the value is in this field
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
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: rptrSwitch_ItemDataBound() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
            }
        }

        protected void rptrSwitch_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ddlSwitch1.ClearSelection();
            ddlSwitch2.ClearSelection();
            ddlSwitch3.ClearSelection();
            ddlSwitch4.ClearSelection();
            ddlSwitch5.ClearSelection();
            ddlSwitch6.ClearSelection();
            txtswitch1.Text = string.Empty;
            txtswitch2.Text = string.Empty;
            txtswitch3.Text = string.Empty;
            txtswitch4.Text = string.Empty;
            txtswitch5.Text = string.Empty;
            txtswitch6.Text = string.Empty;
            if (e.CommandName == "Edit")
            {
                ErrorLog.RuleTrace("SwitchConfig | rptrSwitch_ItemCommand-Edit | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "SwitchConfig";
                _auditParams[2] = "rptrSwitch_ItemCommand-Edit";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                txtSwitchName.Enabled = false;
                int itemId = Convert.ToInt32(e.CommandArgument);
                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                _RuleEntity.SwitchId = Convert.ToInt32(itemId);
                hdnSwitchId.Value = Convert.ToString(_RuleEntity.SwitchId);
                RemoveRowById(hdnSwitchId.Value);
                _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Export;
                DataSet ds = _RuleEntity.GetSwitch();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row1 = ds.Tables[0].Rows[0];
                    txtSwitchName.Text = row1["switchname"].ToString();
                    Session["SwitchName"] = txtSwitchName.Text;
                    txtSwitchDescription.Text = row1["description"].ToString();
                    BindEditDropdown();
                    if (row1["percentage"] != DBNull.Value && Convert.ToDecimal(row1["percentage"]) != 0 && !string.IsNullOrEmpty(row1["percentage"].ToString()))
                    {
                        txtCount.Text = string.Empty;
                        txtSwitchPercentage.Text = row1["percentage"].ToString();
                        Session["SwitchPercentage"] = txtSwitchPercentage.Text;
                    }
                    else
                    {
                        txtCount.Visible = true;
                        txtSwitchPercentage.Text = string.Empty;
                        txtCount.Text = row1["maxcount"].ToString();
                        Session["SwitchPercentage"] = txtCount.Text;
                        //Session["SwitchPercentage"] = txtSwitchPercentage.Text;
                        //string script = "setTimeout(function() { $('#Switch').prop('checked', true); callToggleSwitch(true); }, 0);";
                        //ClientScript.RegisterStartupScript(this.GetType(), "ToggleSwitch", script, true);
                        string script = @"
$(document).ready(function () {
    // Set checkbox to be checked automatically on page load
    var isChecked = true; // You can also set this dynamically based on your logic
    
    // Set the checkbox to checked
    $('#Switch').prop('checked', isChecked);

    // Trigger the checkbox change event (simulate a click)
    $('#Switch').change(); // This triggers the change event on the checkbox

    // Disable the checkbox while processing
    $('#Switch').prop('disabled', true);

    // AJAX call to the server to determine which panel to show and which textbox to clear
    $.ajax({
        type: 'POST',
        url: 'SwitchConfig.aspx/ToggleSwitch',
        data: JSON.stringify({ IsChecked: isChecked }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            var result = response.d;

            // Get the client IDs of the panels and textboxes
            var pnlPercentageID = '<%= pnlPercentage.ClientID %>';
            var pnlCountID = '<%= pnlCount.ClientID %>';
            var txtSwitchPercentageID = '<%= txtSwitchPercentage.ClientID %>';
            var txtCountID = '<%= txtCount.ClientID %>';

            // Check the result and show/hide panels accordingly
            if (result === 'showCount-clearPercentage') {
                $('#' + pnlPercentageID).hide();
                $('#' + pnlCountID).show();
                $('#' + txtSwitchPercentageID).val(''); // Clear Percentage textbox
            } else if (result === 'showPercentage-clearCount') {
                $('#' + pnlCountID).hide();
                $('#' + pnlPercentageID).show();
                $('#' + txtCountID).val(''); // Clear Count textbox
            } else {
                console.log('Unexpected result:', result);
            }

            // Enable the checkbox after processing
            $('#Switch').prop('disabled', false);
        },
        error: function (error) {
            console.error('An error occurred:', error);
            alert('An error occurred: ' + error.responseText);
            // Enable the checkbox after an error
            $('#Switch').prop('disabled', false);
        }
    });
});
";

                        ClientScript.RegisterStartupScript(this.GetType(), "ToggleSwitch", script, true);
                    }
                }
                BindFailoverData(ds.Tables[1]);

                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                DataTable dt2 = new DataTable();
                var tables = _RuleEntity.GetDropDownValues();

                #region TxnType
                DataTable txnTable = tables["temp_bindtxntype"];
                var tableBodyAEPS = new System.Text.StringBuilder();
                var tableBodyDMT = new System.Text.StringBuilder();
                var tableBodyMATM = new System.Text.StringBuilder();

                foreach (DataRow rows in txnTable.Rows)
                {
                    string type = rows["txntype_id"].ToString();
                    string id = rows["id"].ToString(); // Get the ID from the data row
                    string tabType = ds.Tables[2].AsEnumerable()
                                       .FirstOrDefault(r => r["transaction_type"].ToString() == id)?["channel"].ToString(); // Get the channel/tab type

                    // Find the corresponding edit value for the current ID
                    string editValue = string.Empty;
                    DataRow editRow = ds.Tables[2].AsEnumerable()
                                        .FirstOrDefault(r => r["transaction_type"].ToString() == id);

                    if (editRow != null)
                    {
                        editValue = editRow["switch_address"].ToString(); // Retrieve the value to fill
                    }

                    // Build the row for the current tab
                    string rowHtml = $"<tr><td>{type}</td>" +
                                     $"<td><input type='text' name='input_{tabType}_{id}' placeholder='Enter value for {type}' class='form-control' value='{editValue}' /></td></tr>";

                    // Append to the corresponding tab's StringBuilder
                    if (tabType == "AEPS")
                    {
                        tableBodyAEPS.Append(rowHtml);
                    }
                    else if (tabType == "DMT")
                    {
                        tableBodyDMT.Append(rowHtml);
                    }
                    else if (tabType == "MATM")
                    {
                        tableBodyMATM.Append(rowHtml);
                    }
                }

                // Set the inner HTML for each tab
                divAEPS.InnerHtml = tableBodyAEPS.ToString();
                divDMT.InnerHtml = tableBodyDMT.ToString();
                divMATM.InnerHtml = tableBodyMATM.ToString();
                #endregion


                hdnShowModal.Value = "true";
                hdnshowmanual.Value = "false";
                HdnShowDelete.Value = "false";
                ErrorLog.RuleTrace("SwitchConfig | rptrSwitch_ItemCommand-Edit | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
            else if (e.CommandName == "Delete")
            {
                ErrorLog.RuleTrace("SwitchConfig | rptrSwitch_ItemCommand-Delete | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "SwitchConfig";
                _auditParams[2] = "rptrSwitch_ItemCommand-Delete";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                int itemId = Convert.ToInt32(e.CommandArgument);
                _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                _RuleEntity.SwitchId = Convert.ToInt32(itemId);

                _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Update;
                string validateCode = _RuleEntity.ValidateFailoverSwitch();
                if (validateCode == "00")
                {
                    hdnSwitchId.Value = Convert.ToString(_RuleEntity.SwitchId);
                    RemoveRowById(hdnSwitchId.Value);

                    _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Export;
                    DataSet ds = _RuleEntity.GetSwitch();

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row1 = ds.Tables[0].Rows[0];
                        TextBox1.Text = row1["switchname"].ToString();
                        Session["SwitchName"] = TextBox1.Text;
                        TextBox2.Text = row1["description"].ToString();
                        //BindEditDropdown();
                        if (row1["percentage"] != DBNull.Value && Convert.ToDecimal(row1["percentage"]) != 0 && !string.IsNullOrEmpty(row1["percentage"].ToString()))
                        {
                            TextBox4.Text = string.Empty;
                            TextBox3.Text = row1["percentage"].ToString();
                            Session["SwitchPercentage"] = TextBox3.Text;
                        }
                        else
                        {
                            //txtCount.Visible = true;
                            TextBox3.Text = string.Empty;
                            TextBox4.Text = row1["maxcount"].ToString();
                            Session["SwitchPercentage"] = TextBox4.Text;

                            //string script = "setTimeout(function() { $('#Switch').prop('checked', true); callToggleSwitch(true); }, 0);";
                            //ClientScript.RegisterStartupScript(this.GetType(), "ToggleSwitch", script, true);
                            string script = @"
$(document).ready(function () {
    // Set checkbox to be checked automatically on page load
    var isChecked = true; // You can also set this dynamically based on your logic
    
    // Set the checkbox to checked
    $('#Switchh').prop('checked', isChecked);

    // Trigger the checkbox change event (simulate a click)
    $('#Switchh').change(); // This triggers the change event on the checkbox

    // Disable the checkbox while processing
    $('#Switchh').prop('disabled', true);

    // AJAX call to the server to determine which panel to show and which textbox to clear
    $.ajax({
        type: 'POST',
        url: 'SwitchConfig.aspx/ToggleSwitchh',
        data: JSON.stringify({ IsChecked: isChecked }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            var result = response.d;

            // Get the client IDs of the panels and textboxes
            var pnlPercentageID = '<%= pnlPercentage.ClientID %>';
            var pnlCountID = '<%= pnlCount.ClientID %>';
            var txtSwitchPercentageID = '<%= txtSwitchPercentage.ClientID %>';
            var txtCountID = '<%= txtCount.ClientID %>';

            // Check the result and show/hide panels accordingly
            if (result === 'showCount-clearPercentage') {
                $('#' + pnlPercentageID).hide();
                $('#' + pnlCountID).show();
                $('#' + txtSwitchPercentageID).val(''); // Clear Percentage textbox
            } else if (result === 'showPercentage-clearCount') {
                $('#' + pnlCountID).hide();
                $('#' + pnlPercentageID).show();
                $('#' + txtCountID).val(''); // Clear Count textbox
            } else {
                console.log('Unexpected result:', result);
            }

            // Enable the checkbox after processing
            $('#Switchh').prop('disabled', false);
        },
        error: function (error) {
            console.error('An error occurred:', error);
            alert('An error occurred: ' + error.responseText);
            // Enable the checkbox after an error
            $('#Switchh').prop('disabled', false);
        }
    });
});
";

                            ClientScript.RegisterStartupScript(this.GetType(), "ToggleSwitchh", script, true);
                        }
                    }
                    //BindFailoverData(ds.Tables[1]);

                    //_RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                    //DataTable dt2 = new DataTable();
                    //var tables = _RuleEntity.GetDropDownValues();
                    hdnShowModal.Value = "false";
                    hdnshowmanual.Value = "false";
                    HdnShowDelete.Value = "true";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "showDeleteModal", "checkDeleteModal();", true);

                    //int itemId = Convert.ToInt32(e.CommandArgument);
                    //_RuleEntityy.IsDelete = 1;
                    //_RuleEntityy.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                    //_RuleEntityy.SwitchId = Convert.ToInt32(itemId);
                    //_RuleEntityy.Flag = (int)EnumCollection.EnumRuleType.Delete;
                    //string validateCode = _RuleEntityy.ValidateSwitchConifg();
                    //if (validateCode == "00")
                    //{
                    //    string statusCode = _RuleEntityy.UpdateSwitchStatus();

                    //    if (statusCode == "DEL00")
                    //    {
                    //        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                    //        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                    //    }
                    //    else
                    //    {
                    //        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                    //        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                    //    }
                    //    var response = new
                    //    {
                    //        StatusMessage = _CommonEntity.ResponseMessage
                    //    };
                    //    ErrorLog.RuleTrace("SwitchConfig: Delete() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                    //    BindSwitch();
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Switch Name Already In Use Rule Configuration. Unable To Delete', 'Warning');", true);
                    //    return;
                    //}
                }
                if(validateCode=="98")
                {
                    HdnShowDelete.Value = "false";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('This Switch Already Use In The Rule', 'Warning');", true);
                    return;
                }
                if (validateCode == "96")
                {
                    HdnShowDelete.Value = "false";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('This Switch Already Use In The Failover Switch', 'Warning');", true);
                    return;
                }
                
                //ClientScript.RegisterStartupScript(this.GetType(), "showModalScript", "showDeleteModalIfTriggered();", true);
                ErrorLog.RuleTrace("SwitchConfig | rptrSwitch_ItemCommand-Delete | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
        }
        private void BindDropdown()
        {
            _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
            DataTable dt = new DataTable();
            var tables = _RuleEntity.GetDropDownValues();

            if (tables != null)
            {
                // Bind Group Name DropDown
                if (tables.ContainsKey("temp_bindswitch"))
                {
                    DataTable bindGroupTable = tables["temp_bindswitch"];

                    if (bindGroupTable.Rows.Count > 0)
                    {
                        ddlSwitch1.Items.Clear();
                        ddlSwitch1.DataSource = bindGroupTable;
                        ddlSwitch1.DataTextField = "switchname";
                        ddlSwitch1.DataValueField = "id";
                        ddlSwitch1.DataBind();
                        ddlSwitch1.Items.Insert(0, new ListItem("--Select--", ""));

                        ddlSwitch2.Items.Clear();
                        ddlSwitch2.DataSource = bindGroupTable;
                        ddlSwitch2.DataTextField = "switchname";
                        ddlSwitch2.DataValueField = "id";
                        ddlSwitch2.DataBind();
                        ddlSwitch2.Items.Insert(0, new ListItem("--Select--", ""));

                        ddlSwitch3.Items.Clear();
                        ddlSwitch3.DataSource = bindGroupTable;
                        ddlSwitch3.DataTextField = "switchname";
                        ddlSwitch3.DataValueField = "id";
                        ddlSwitch3.DataBind();
                        ddlSwitch3.Items.Insert(0, new ListItem("--Select--", ""));

                        ddlSwitch4.Items.Clear();
                        ddlSwitch4.DataSource = bindGroupTable;
                        ddlSwitch4.DataTextField = "switchname";
                        ddlSwitch4.DataValueField = "id";
                        ddlSwitch4.DataBind();
                        ddlSwitch4.Items.Insert(0, new ListItem("--Select--", ""));

                        ddlSwitch5.Items.Clear();
                        ddlSwitch5.DataSource = bindGroupTable;
                        ddlSwitch5.DataTextField = "switchname";
                        ddlSwitch5.DataValueField = "id";
                        ddlSwitch5.DataBind();
                        ddlSwitch5.Items.Insert(0, new ListItem("--Select--", ""));

                        ddlSwitch6.Items.Clear();
                        ddlSwitch6.DataSource = bindGroupTable;
                        ddlSwitch6.DataTextField = "switchname";
                        ddlSwitch6.DataValueField = "id";
                        ddlSwitch6.DataBind();
                        ddlSwitch6.Items.Insert(0, new ListItem("--Select--", ""));
                    }
                    else
                    {
                        ddlSwitch1.Items.Clear();
                        ddlSwitch1.DataSource = null;
                        ddlSwitch1.DataBind();

                        ddlSwitch2.Items.Clear();
                        ddlSwitch2.DataSource = null;
                        ddlSwitch2.DataBind();

                        ddlSwitch3.Items.Clear();
                        ddlSwitch3.DataSource = null;
                        ddlSwitch3.DataBind();

                        ddlSwitch4.Items.Clear();
                        ddlSwitch4.DataSource = null;
                        ddlSwitch4.DataBind();

                        ddlSwitch5.Items.Clear();
                        ddlSwitch5.DataSource = null;
                        ddlSwitch5.DataBind();

                        ddlSwitch6.Items.Clear();
                        ddlSwitch6.DataSource = null;
                        ddlSwitch6.DataBind();
                    }
                }
                // Bind Aggregator DropDown
                if (tables.ContainsKey("temp_bindtxntype"))
                {
                    bindtxntype = tables["temp_bindtxntype"];

                    if (bindtxntype.Rows.Count > 0)
                    {
                        var tableBody = new System.Text.StringBuilder();
                        var tableBodyDMT = new System.Text.StringBuilder();
                        var tableBodyMATM = new System.Text.StringBuilder();
                        foreach (DataRow row in bindtxntype.Rows)
                        {
                            string type = row["txntype_id"].ToString();
                            string id = row["id"].ToString(); // Get the ID from the data row

                            //tableBody.Append("<tr>");
                            //tableBody.Append($"<td>{type}</td>");
                            //tableBody.Append($"<td><input type='text' name='input_{id}' placeholder='Enter value for {type}' class='form-control' /></td>");
                            //tableBody.Append("</tr>");

                            string tabPrefix = "AEPS"; // Change accordingly based on the tab

                            tableBody.Append("<tr>");
                            tableBody.Append($"<td>{type}</td>");
                            tableBody.Append($"<td><input type='text' name='input_{tabPrefix}_{id}' placeholder='Enter value for {type}' class='form-control' /></td>");
                            tableBody.Append("</tr>");

                            string tabPrefixD = "DMT"; // Change accordingly based on the tab

                            tableBodyDMT.Append("<tr>");
                            tableBodyDMT.Append($"<td>{type}</td>");
                            tableBodyDMT.Append($"<td><input type='text' name='input_{tabPrefixD}_{id}' placeholder='Enter value for {type}' class='form-control' /></td>");
                            tableBodyDMT.Append("</tr>");

                            string tabPrefixM = "MATM"; // Change accordingly based on the tab

                            tableBodyMATM.Append("<tr>");
                            tableBodyMATM.Append($"<td>{type}</td>");
                            tableBodyMATM.Append($"<td><input type='text' name='input_{tabPrefixM}_{id}' placeholder='Enter value for {type}' class='form-control' /></td>");
                            tableBodyMATM.Append("</tr>");
                        }

                        divAEPS.InnerHtml = tableBody.ToString();
                        divDMT.InnerHtml = tableBodyDMT.ToString();
                        divMATM.InnerHtml = tableBodyMATM.ToString();
                    }
                    else
                    {
                        //ddlTxnType.Items.Clear();
                        //ddlTxnType.DataSource = null;
                        //ddlTxnType.DataBind();
                        //ddlTxnType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
                    }
                }
            }
        }
        private void BindEditDropdown()
        {
            _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
            DataTable dt = new DataTable();
            var tables = _RuleEntity.GetDropDownValues();

            if (tables != null)
            {
                // Bind Group Name DropDown
                if (tables.ContainsKey("temp_bindswitch"))
                {
                    DataTable bindGroupTable = tables["temp_bindswitch"];

                    if (bindGroupTable.Rows.Count > 0)
                    {
                        string someValueToRemove = Session["SwitchName"].ToString(); // Change this to your target value
                        foreach (DataRow row in bindGroupTable.Rows.Cast<DataRow>().ToList())
                        {
                            if (row["switchname"].ToString() == someValueToRemove)
                            {
                                bindGroupTable.Rows.Remove(row);
                                break; // Exit the loop once the row is removed
                            }
                        }

                        ddlSwitch1.Items.Clear();
                        ddlSwitch1.DataSource = bindGroupTable;
                        ddlSwitch1.DataTextField = "switchname";
                        ddlSwitch1.DataValueField = "id";
                        ddlSwitch1.DataBind();
                        ddlSwitch1.Items.Insert(0, new ListItem("--Select--", ""));

                        ddlSwitch2.Items.Clear();
                        ddlSwitch2.DataSource = bindGroupTable;
                        ddlSwitch2.DataTextField = "switchname";
                        ddlSwitch2.DataValueField = "id";
                        ddlSwitch2.DataBind();
                        ddlSwitch2.Items.Insert(0, new ListItem("--Select--", ""));

                        ddlSwitch3.Items.Clear();
                        ddlSwitch3.DataSource = bindGroupTable;
                        ddlSwitch3.DataTextField = "switchname";
                        ddlSwitch3.DataValueField = "id";
                        ddlSwitch3.DataBind();
                        ddlSwitch3.Items.Insert(0, new ListItem("--Select--", ""));

                        ddlSwitch4.Items.Clear();
                        ddlSwitch4.DataSource = bindGroupTable;
                        ddlSwitch4.DataTextField = "switchname";
                        ddlSwitch4.DataValueField = "id";
                        ddlSwitch4.DataBind();
                        ddlSwitch4.Items.Insert(0, new ListItem("--Select--", ""));

                        ddlSwitch5.Items.Clear();
                        ddlSwitch5.DataSource = bindGroupTable;
                        ddlSwitch5.DataTextField = "switchname";
                        ddlSwitch5.DataValueField = "id";
                        ddlSwitch5.DataBind();
                        ddlSwitch5.Items.Insert(0, new ListItem("--Select--", ""));

                        ddlSwitch6.Items.Clear();
                        ddlSwitch6.DataSource = bindGroupTable;
                        ddlSwitch6.DataTextField = "switchname";
                        ddlSwitch6.DataValueField = "id";
                        ddlSwitch6.DataBind();
                        ddlSwitch6.Items.Insert(0, new ListItem("--Select--", ""));
                    }
                    else
                    {
                        ddlSwitch1.Items.Clear();
                        ddlSwitch1.DataSource = null;
                        ddlSwitch1.DataBind();

                        ddlSwitch2.Items.Clear();
                        ddlSwitch2.DataSource = null;
                        ddlSwitch2.DataBind();

                        ddlSwitch3.Items.Clear();
                        ddlSwitch3.DataSource = null;
                        ddlSwitch3.DataBind();

                        ddlSwitch4.Items.Clear();
                        ddlSwitch4.DataSource = null;
                        ddlSwitch4.DataBind();

                        ddlSwitch5.Items.Clear();
                        ddlSwitch5.DataSource = null;
                        ddlSwitch5.DataBind();

                        ddlSwitch6.Items.Clear();
                        ddlSwitch6.DataSource = null;
                        ddlSwitch6.DataBind();
                    }
                }
                // Bind Aggregator DropDown
                if (tables.ContainsKey("temp_bindtxntype"))
                {
                    bindtxntype = tables["temp_bindtxntype"];

                    if (bindtxntype.Rows.Count > 0)
                    {
                        var tableBody = new System.Text.StringBuilder();
                        var tableBodyDMT = new System.Text.StringBuilder();
                        var tableBodyMATM = new System.Text.StringBuilder();
                        foreach (DataRow row in bindtxntype.Rows)
                        {
                            string type = row["txntype_id"].ToString();
                            string id = row["id"].ToString(); // Get the ID from the data row

                            //tableBody.Append("<tr>");
                            //tableBody.Append($"<td>{type}</td>");
                            //tableBody.Append($"<td><input type='text' name='input_{id}' placeholder='Enter value for {type}' class='form-control' /></td>");
                            //tableBody.Append("</tr>");

                            string tabPrefix = "AEPS"; // Change accordingly based on the tab

                            tableBody.Append("<tr>");
                            tableBody.Append($"<td>{type}</td>");
                            tableBody.Append($"<td><input type='text' name='input_{tabPrefix}_{id}' placeholder='Enter value for {type}' class='form-control' /></td>");
                            tableBody.Append("</tr>");

                            string tabPrefixD = "DMT"; // Change accordingly based on the tab

                            tableBodyDMT.Append("<tr>");
                            tableBodyDMT.Append($"<td>{type}</td>");
                            tableBodyDMT.Append($"<td><input type='text' name='input_{tabPrefixD}_{id}' placeholder='Enter value for {type}' class='form-control' /></td>");
                            tableBodyDMT.Append("</tr>");

                            string tabPrefixM = "MATM"; // Change accordingly based on the tab

                            tableBodyMATM.Append("<tr>");
                            tableBodyMATM.Append($"<td>{type}</td>");
                            tableBodyMATM.Append($"<td><input type='text' name='input_{tabPrefixM}_{id}' placeholder='Enter value for {type}' class='form-control' /></td>");
                            tableBodyMATM.Append("</tr>");
                        }

                        divAEPS.InnerHtml = tableBody.ToString();
                        divDMT.InnerHtml = tableBodyDMT.ToString();
                        divMATM.InnerHtml = tableBodyMATM.ToString();
                    }
                    else
                    {
                        //ddlTxnType.Items.Clear();
                        //ddlTxnType.DataSource = null;
                        //ddlTxnType.DataBind();
                        //ddlTxnType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
                    }
                }
            }
        }
        protected void rptSwitchDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                //if (e.CommandName == "Edit")
                //{
                //    ErrorLog.RuleTrace("SwitchConfig | rptSwitchDetails_ItemCommand-Edit | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                //    #region Audit
                //    _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                //    _auditParams[1] = "SwitchConfig";
                //    _auditParams[2] = "rptSwitchDetails_ItemCommand-Edit";
                //    _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                //    _LoginEntity.StoreLoginActivities(_auditParams);
                //    #endregion
                //    // Find the TextBox in the current row (RepeaterItem)
                //    //TextBox txtPercentage = (TextBox)e.Item.FindControl("txtPercentage");
                //    //txtPercentage.Enabled = true;
                //    TextBox txtPercentage = (TextBox)e.Item.FindControl("txtPercentage");
                //    txtPercentage.Enabled = true; // Enable the textbox for editing
                //    ErrorLog.RuleTrace("SwitchConfig | rptSwitchDetails_ItemCommand-Edit | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                //}
                //else if (e.CommandName == "Update")
                //{
                //    ErrorLog.RuleTrace("SwitchConfig | rptSwitchDetails_ItemCommand-Update | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                //    #region Audit
                //    _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                //    _auditParams[1] = "SwitchConfig";
                //    _auditParams[2] = "rptSwitchDetails_ItemCommand-Update";
                //    _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                //    _LoginEntity.StoreLoginActivities(_auditParams);
                //    #endregion
                //    // Find the TextBox in the current row and disable it after updating
                //    //TextBox txtPercentage = (TextBox)e.Item.FindControl("txtPercentage");
                //    //txtPercentage.Enabled = false;
                //    TextBox txtPercentage = (TextBox)e.Item.FindControl("txtPercentage");
                //    string updatedPercentage = txtPercentage.Text;
                //    ErrorLog.RuleTrace("SwitchConfig | rptSwitchDetails_ItemCommand-Update | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                //}
                if (e.CommandName == "Edit")
                {
                    // Log the Edit action with User and LoginKey information
                    ErrorLog.RuleTrace("SwitchConfig | rptSwitchDetails_ItemCommand-Edit | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());

                    // Audit logging
                    _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                    _auditParams[1] = "SwitchConfig";
                    _auditParams[2] = "rptSwitchDetails_ItemCommand-Edit";
                    _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);

                    // Find the TextBox and enable it for editing
                    TextBox txtPercentage = (TextBox)e.Item.FindControl("txtPercentage");
                    txtPercentage.Enabled = true;  // Enable the TextBox for editing

                    // Find and hide the Edit button
                    ImageButton btnEdit = (ImageButton)e.Item.FindControl("btnEdit");
                    btnEdit.Visible = false;

                    // Find and show the Update button
                    ImageButton btnUpdate = (ImageButton)e.Item.FindControl("btnUpdate");
                    btnUpdate.Visible = true;

                    // Log the Edit action end
                    ErrorLog.RuleTrace("SwitchConfig | rptSwitchDetails_ItemCommand-Edit | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                }
                else if (e.CommandName == "Update")
                {
                    // Log the Update action with User and LoginKey information
                    ErrorLog.RuleTrace("SwitchConfig | rptSwitchDetails_ItemCommand-Update | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());

                    // Audit logging
                    _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                    _auditParams[1] = "SwitchConfig";
                    _auditParams[2] = "rptSwitchDetails_ItemCommand-Update";
                    _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);

                    // Find the TextBox, retrieve the updated value, and disable the TextBox
                    TextBox txtPercentage = (TextBox)e.Item.FindControl("txtPercentage");
                    string updatedPercentage = txtPercentage.Text;

                    // Perform your update logic here, e.g., saving the updatedPercentage to the database

                    // Disable the TextBox after update
                    txtPercentage.Enabled = false;

                    // Find and show the Edit button
                    ImageButton btnEdit = (ImageButton)e.Item.FindControl("btnEdit");
                    btnEdit.Visible = true;

                    // Find and hide the Update button
                    ImageButton btnUpdate = (ImageButton)e.Item.FindControl("btnUpdate");
                    btnUpdate.Visible = false;

                    // Log the Update action end
                    ErrorLog.RuleTrace("SwitchConfig | rptSwitchDetails_ItemCommand-Update | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                }
                string script = @"
$(document).ready(function () {
    // Set checkbox to be checked automatically on page load
    var isChecked = true; // You can also set this dynamically based on your logic
    
    // Set the checkbox to checked
    $('#Switch').prop('checked', isChecked);

    // Trigger the checkbox change event (simulate a click)
    $('#Switch').change(); // This triggers the change event on the checkbox

    // Disable the checkbox while processing
    $('#Switch').prop('disabled', true);

    // AJAX call to the server to determine which panel to show and which textbox to clear
    $.ajax({
        type: 'POST',
        url: 'SwitchConfig.aspx/ToggleSwitch',
        data: JSON.stringify({ IsChecked: isChecked }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            var result = response.d;

            // Get the client IDs of the panels and textboxes
            var pnlPercentageID = '<%= pnlPercentage.ClientID %>';
            var pnlCountID = '<%= pnlCount.ClientID %>';
            var txtSwitchPercentageID = '<%= txtSwitchPercentage.ClientID %>';
            var txtCountID = '<%= txtCount.ClientID %>';

            // Check the result and show/hide panels accordingly
            if (result === 'showCount-clearPercentage') {
                $('#' + pnlPercentageID).hide();
                $('#' + pnlCountID).show();
                $('#' + txtSwitchPercentageID).val(''); // Clear Percentage textbox
            } else if (result === 'showPercentage-clearCount') {
                $('#' + pnlCountID).hide();
                $('#' + pnlPercentageID).show();
                $('#' + txtCountID).val(''); // Clear Count textbox
            } else {
                console.log('Unexpected result:', result);
            }

            // Enable the checkbox after processing
            $('#Switch').prop('disabled', false);
        },
        error: function (error) {
            console.error('An error occurred:', error);
            alert('An error occurred: ' + error.responseText);
            // Enable the checkbox after an error
            $('#Switch').prop('disabled', false);
        }
    });
});
";

                ClientScript.RegisterStartupScript(this.GetType(), "ToggleSwitch", script, true);
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: rptSwitchDetails_ItemCommand() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
        }


        [WebMethod]
        public static string ToggleSwitch(bool IsChecked)
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
        public static string ToggleSwitchh(bool IsChecked)
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

        private DataTable CreateUpdatesDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("percentage", typeof(int));
            return dt;
        }

        //private bool ValidateSwitchValues()
        //{
        //    // Get values from the text boxes
        //    int switch1 = GetSwitchValue(txtswitch1);
        //    int switch2 = GetSwitchValue(txtswitch2);
        //    int switch3 = GetSwitchValue(txtswitch3);
        //    int switch4 = GetSwitchValue(txtswitch4);
        //    int switch5 = GetSwitchValue(txtswitch5);
        //    int switch6 = GetSwitchValue(txtswitch6);

        //    // Calculate the sum
        //    int total = switch1 + switch2 + switch3 + switch4 + switch5 + switch6;

        //    // Check if total is exactly 100
        //    if (total < 100)
        //    {
        //        ShowWarning("The total must not be less than 100.");
        //        return false; // Indicate validation failure
        //    }

        //    if (total > 100)
        //    {
        //        ShowWarning("The total must not be greater than 100.");
        //        return false; // Indicate validation failure
        //    }

        //    return true; // Indicate successful validation
        //}
        private int GetSwitchValue(TextBox textBox)
        {
            // Use .Text to get the value of the text box
            int value;
            if (int.TryParse(textBox.Text, out value)) // Access .Text
            {
                return value;
            }
            return 0; // Return 0 for invalid input
        }
        private void ShowWarning(string message)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning",
                $"showWarning('{message}', 'Warning');", true);
        }
        private bool CheckForDuplicateValues()
        {
            HashSet<string> selectedValues = new HashSet<string>();

            List<DropDownList> dropdowns = new List<DropDownList>
            {
                ddlSwitch1,
                ddlSwitch2,
                ddlSwitch3,
                ddlSwitch4,
                ddlSwitch5,
                ddlSwitch6
            };

            //foreach (var ddl in dropdowns)
            //{
            //    if(!string.IsNullOrEmpty(ddl.SelectedItem.Text))
            //    {
            //        string selectedValue = ddl.SelectedItem.Text;

            //        // Check if the selected value is already in the HashSet
            //        if (!string.IsNullOrEmpty(selectedValue) && selectedValue != "--Select--" && !selectedValues.Add(selectedValue))
            //        {
            //            // Duplicate found, trigger warning
            //            ShowWarning($"Duplicate value found in the failover switch: {selectedValue}. Please try again.", "Warning");
            //            return false; // Return true indicating a duplicate was found
            //        }
            //    }
            //}
            foreach (var ddl in dropdowns)
            {
                // Skip if ddl is null
                if (ddl == null)
                {
                    continue; // Skip this iteration
                }

                // Skip if SelectedItem is null or its Text is null or empty
                if (ddl.SelectedItem == null || string.IsNullOrEmpty(ddl.SelectedItem.Text))
                {
                    continue; // Skip this iteration
                }

                // Now it's safe to access SelectedItem.Text
                string selectedValue = ddl.SelectedItem.Text;

                // Check if the selected value is valid and add to the HashSet
                if (selectedValue != "--Select--" && !selectedValues.Add(selectedValue))
                {
                    // Duplicate found, trigger warning
                    ShowWarning($"Duplicate value found in the failover switch: {selectedValue}. Please try again.", "Warning");
                    return false; // Return false indicating a duplicate was found
                }
            }



            // If no duplicates are found
            return true; // Return false indicating no duplicates
        }

        private bool ValidateSwitchInputs()
        {
            // Create a list of pairs for textboxes and dropdowns
            var switchPairs = new List<(TextBox textBox, DropDownList dropDown)>
            {
                (txtswitch1, ddlSwitch1),
                (txtswitch2, ddlSwitch2),
                (txtswitch3, ddlSwitch3),
                (txtswitch4, ddlSwitch4),
                (txtswitch5, ddlSwitch5),
                (txtswitch6, ddlSwitch6)
            };

            foreach (var (textBox, dropDown) in switchPairs)
            {
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    if (dropDown.SelectedIndex != 0)
                    {
                        if (string.IsNullOrEmpty(textBox.Text))
                        {
                            ShowWarning($"Please Enter Switch {textBox.ID.Last()} %. Try again", "Warning");
                            return false; // Validation failed
                        }
                    }
                }

                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    // Check if the corresponding dropdown is not selected
                    if (dropDown.SelectedIndex == 0)
                    {
                        ShowWarning($"Please Enter Switch {textBox.ID.Last()}. Try again", "Warning");
                        return false; // Validation failed
                    }
                }

            }
            return true; // Validation passed
        }
        private void InitializeDataTable()
        {
            dtSwitchValues = new DataTable();
            dtSwitchValues.Columns.Add("SwitchId", typeof(string));
            dtSwitchValues.Columns.Add("SwitchValue", typeof(string));
        }
        //    private void PopulateAndAddSwitchValues()
        //    {
        //        // Ensure dtSwitchValues is initialized
        //        if (dtSwitchValues == null)
        //        {
        //            InitializeDataTable();
        //        }

        //        // Clear previous values in the DataTable
        //        dtSwitchValues.Clear();

        //        // Create an array of dropdowns and corresponding textboxes
        //        var switchPairs = new[]
        //        {
        //    new { Dropdown = ddlSwitch1, Textbox = txtswitch1 },
        //    new { Dropdown = ddlSwitch2, Textbox = txtswitch2 },
        //    new { Dropdown = ddlSwitch3, Textbox = txtswitch3 },
        //    new { Dropdown = ddlSwitch4, Textbox = txtswitch4 },
        //    new { Dropdown = ddlSwitch5, Textbox = txtswitch5 },
        //    new { Dropdown = ddlSwitch6, Textbox = txtswitch6 }
        //};

        //        // Loop through each pair and add values to the DataTable
        //        foreach (var pair in switchPairs)
        //        {
        //            if (pair.Dropdown.SelectedIndex > 0 && !string.IsNullOrEmpty(pair.Textbox.Text))
        //            {
        //                // Create a new DataRow
        //                DataRow row = dtSwitchValues.NewRow();
        //                row["SwitchId"] = pair.Dropdown.SelectedValue; // Use the selected value of the dropdown
        //                row["SwitchValue"] = pair.Textbox.Text; // Use the text box value

        //                // Add the DataRow to the DataTable
        //                dtSwitchValues.Rows.Add(row);
        //            }
        //        }
        //    }
        //    private bool PopulateAndAddSwitchValues()
        //    {
        //        // Ensure dtSwitchValues is initialized
        //        if (dtSwitchValues == null)
        //        {
        //            InitializeDataTable();
        //        }

        //        // Clear previous values in the DataTable
        //        dtSwitchValues.Clear();

        //        // Create an array of dropdowns and corresponding textboxes
        //        var switchPairs = new[]
        //        {
        //    new { Dropdown = ddlSwitch1, Textbox = txtswitch1 },
        //    new { Dropdown = ddlSwitch2, Textbox = txtswitch2 },
        //    new { Dropdown = ddlSwitch3, Textbox = txtswitch3 },
        //    new { Dropdown = ddlSwitch4, Textbox = txtswitch4 },
        //    new { Dropdown = ddlSwitch5, Textbox = txtswitch5 },
        //    new { Dropdown = ddlSwitch6, Textbox = txtswitch6 }
        //};

        //        // Variable to keep track of the sum of all textbox values
        //        double totalSwitchValue = 0;

        //        // First, add the values to the DataTable and calculate the total
        //        foreach (var pair in switchPairs)
        //        {
        //            if (pair.Dropdown.SelectedIndex > 0 && !string.IsNullOrEmpty(pair.Textbox.Text))
        //            {
        //                // Parse the value of the Textbox to a double (assuming they are numeric)
        //                if (double.TryParse(pair.Textbox.Text, out double switchValue))
        //                {
        //                    // Create a new DataRow
        //                    DataRow row = dtSwitchValues.NewRow();
        //                    row["SwitchId"] = pair.Dropdown.SelectedValue; // Use the selected value of the dropdown
        //                    row["SwitchValue"] = switchValue.ToString(); // Store the value as a string in the DataTable

        //                    // Add the DataRow to the DataTable
        //                    dtSwitchValues.Rows.Add(row);

        //                    // Add the switch value to the total
        //                    totalSwitchValue += switchValue;
        //                }
        //                else
        //                {
        //                    ShowWarning("One of the switch values is not a valid number.", "Warning");
        //                    return false; // Stop if the input is invalid
        //                }
        //            }
        //        }

        //        // Now compare the totalSwitchValue with the txtPercentage value
        //        if(string.IsNullOrEmpty(txtCount.Text))
        //        {
        //            if (double.TryParse(txtSwitchPercentage.Text, out double percentageValue))
        //            {
        //                // Check if the total of switch values equals the percentage value
        //                if (totalSwitchValue == percentageValue)
        //                {
        //                    // If the sum matches the percentage value, proceed
        //                    //MessageBox.Show("The total of switch values is correct.", "Success");
        //                }
        //                else
        //                {
        //                    // If the sum does not match, show a warning
        //                    ShowWarning($"The total of switch values does not match the specified percentage value ({percentageValue}).", "Warning");
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                ShowWarning("The percentage value is not a valid number.", "Warning");
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            if (totalSwitchValue == 100)
        //            {
        //                // If the sum matches the percentage value, proceed
        //                //MessageBox.Show("The total of switch values is correct.", "Success");
        //            }
        //            else
        //            {
        //                // If the sum does not match, show a warning
        //                ShowWarning($"The total of failover switch values = 100).", "Warning");
        //                return false;
        //            }
        //        }

        //    }
        private bool PopulateAndAddSwitchValues()
        {
            // Ensure dtSwitchValues is initialized
            if (dtSwitchValues == null)
            {
                InitializeDataTable();
            }

            // Clear previous values in the DataTable
            dtSwitchValues.Clear();

            // Create an array of dropdowns and corresponding textboxes
            var switchPairs = new[]
            {
        new { Dropdown = ddlSwitch1, Textbox = txtswitch1 },
        new { Dropdown = ddlSwitch2, Textbox = txtswitch2 },
        new { Dropdown = ddlSwitch3, Textbox = txtswitch3 },
        new { Dropdown = ddlSwitch4, Textbox = txtswitch4 },
        new { Dropdown = ddlSwitch5, Textbox = txtswitch5 },
        new { Dropdown = ddlSwitch6, Textbox = txtswitch6 }
    };

            // Variable to keep track of the sum of all textbox values
            double totalSwitchValue = 0;

            // First, add the values to the DataTable and calculate the total
            foreach (var pair in switchPairs)
            {
                // Check if the dropdown has a selected value and the textbox is not empty
                if (pair.Dropdown.SelectedIndex > 0 && !string.IsNullOrEmpty(pair.Textbox.Text))
                {
                    // Try parsing the value of the Textbox to a double
                    if (double.TryParse(pair.Textbox.Text, out double switchValue))
                    {
                        // Create a new DataRow and set the values
                        DataRow row = dtSwitchValues.NewRow();
                        row["SwitchId"] = pair.Dropdown.SelectedValue; // Use the selected value of the dropdown
                        row["SwitchValue"] = switchValue.ToString(); // Store the value as a string in the DataTable

                        // Add the DataRow to the DataTable
                        dtSwitchValues.Rows.Add(row);

                        // Add the switch value to the total
                        totalSwitchValue += switchValue;
                    }
                    else
                    {
                        // Show a warning if one of the switch values is not a valid number and return false
                        ShowWarning("One of the switch values is not a valid number.", "Warning");
                        return false;
                    }
                }
            }

            // Now compare the totalSwitchValue with the txtPercentage value
            if (string.IsNullOrEmpty(txtCount.Text))
            {
                // If txtCount is empty, validate the percentage value
                if (double.TryParse(txtSwitchPercentage.Text, out double percentageValue))
                {
                    // Check if the total of switch values equals the percentage value
                    if (totalSwitchValue == percentageValue)
                    {
                        // If the sum matches the percentage value, proceed
                        return true; // Success
                    }
                    else
                    {
                        // If the sum does not match, show a warning and return false
                        ShowWarning($"The total of switch values does not match the specified percentage value ({percentageValue}).", "Warning");
                        return false;
                    }
                }
                else
                {
                    // If the percentage value is not valid, show a warning and return false
                    ShowWarning("The percentage value is not a valid number.", "Warning");
                    return false;
                }
            }
            else
            {
                // If txtCount is not empty, check if the total switch value equals 100
                if (totalSwitchValue == 100)
                {
                    // If the sum matches 100, proceed
                    return true; // Success
                }
                else
                {
                    // If the sum does not match, show a warning and return false
                    ShowWarning($"The total of failover switch values should be 100.", "Warning");
                    return false;
                }
            }
        }

        protected void ddlSwitch1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSwitch2.ClearSelection();
                ddlSwitch3.ClearSelection();
                ddlSwitch4.ClearSelection();
                ddlSwitch5.ClearSelection();
                ddlSwitch6.ClearSelection();

                txtswitch2.Text = string.Empty;
                txtswitch3.Text = string.Empty;
                txtswitch4.Text = string.Empty;
                txtswitch5.Text = string.Empty;
                txtswitch6.Text = string.Empty;

                ddlSwitch2.Enabled = true;
                txtswitch2.Enabled = true;
                ddlSwitch3.Enabled = false;
                txtswitch3.Enabled = false;
                ddlSwitch4.Enabled = false;
                txtswitch4.Enabled = false;
                ddlSwitch5.Enabled = false;
                txtswitch5.Enabled = false;
                ddlSwitch6.Enabled = false;
                txtswitch6.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ddlSwitch2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSwitch3.ClearSelection();
                ddlSwitch4.ClearSelection();
                ddlSwitch5.ClearSelection();
                ddlSwitch6.ClearSelection();

                txtswitch3.Text = string.Empty;
                txtswitch4.Text = string.Empty;
                txtswitch5.Text = string.Empty;
                txtswitch6.Text = string.Empty;

                ddlSwitch3.Enabled = true;
                txtswitch3.Enabled = true;

                ddlSwitch4.Enabled = false;
                txtswitch4.Enabled = false;
                ddlSwitch5.Enabled = false;
                txtswitch5.Enabled = false;
                ddlSwitch6.Enabled = false;
                txtswitch6.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ddlSwitch3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSwitch4.ClearSelection();
                ddlSwitch5.ClearSelection();
                ddlSwitch6.ClearSelection();

                txtswitch4.Text = string.Empty;
                txtswitch5.Text = string.Empty;
                txtswitch6.Text = string.Empty;

                ddlSwitch4.Enabled = true;
                txtswitch4.Enabled = true;

                ddlSwitch5.Enabled = false;
                txtswitch5.Enabled = false;
                ddlSwitch6.Enabled = false;
                txtswitch6.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ddlSwitch4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSwitch5.ClearSelection();
                ddlSwitch6.ClearSelection();

                txtswitch5.Text = string.Empty;
                txtswitch6.Text = string.Empty;

                ddlSwitch5.Enabled = true;
                txtswitch5.Enabled = true;

                ddlSwitch6.Enabled = false;
                txtswitch6.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ddlSwitch5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSwitch5.ClearSelection();
                txtswitch6.Text = string.Empty;
                txtswitch6.Enabled = true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void BindFailoverData(DataTable dataTable)
        {
            // Assuming you want to bind the first 6 switches
            for (int i = 0; i < dataTable.Rows.Count && i < 6; i++)
            {
                DataRow row = dataTable.Rows[i];

                // Switch 1
                if (i == 0)
                {
                    ddlSwitch1.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch1.Text = row["failover_percentage"].ToString();
                    ddlSwitch1.Enabled = true;  // Enable Switch 1
                    txtswitch1.Enabled = true;  // Enable TextBox 1
                }
                // Switch 2
                else if (i == 1)
                {
                    ddlSwitch2.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch2.Text = row["failover_percentage"].ToString();
                    ddlSwitch2.Enabled = true;  // Enable Switch 2
                    txtswitch2.Enabled = true;  // Enable TextBox 2
                }
                // Switch 3
                else if (i == 2)
                {
                    ddlSwitch3.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch3.Text = row["failover_percentage"].ToString();
                    ddlSwitch3.Enabled = true;  // Enable Switch 3
                    txtswitch3.Enabled = true;  // Enable TextBox 3
                }
                // Switch 4
                else if (i == 3)
                {
                    ddlSwitch4.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch4.Text = row["failover_percentage"].ToString();
                    ddlSwitch4.Enabled = true;  // Enable Switch 4
                    txtswitch4.Enabled = true;  // Enable TextBox 4
                }
                // Switch 5
                else if (i == 4)
                {
                    ddlSwitch5.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch5.Text = row["failover_percentage"].ToString();
                    ddlSwitch5.Enabled = true;  // Enable Switch 5
                    txtswitch5.Enabled = true;  // Enable TextBox 5
                }
                // Switch 6
                else if (i == 5)
                {
                    ddlSwitch6.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch6.Text = row["failover_percentage"].ToString();
                    ddlSwitch6.Enabled = true;  // Enable Switch 6
                    txtswitch6.Enabled = true;  // Enable TextBox 6
                }
            }
        }

        public void RemoveRowById(string id)
        {
            bool rowRemoved = false;

            // Retrieve the username and flag (this part remains unchanged)
            _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
            _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;

            // Get the DataSet
            DataSet dt = _RuleEntity.GetSwitch();

            // Remove rows where maxcount is not 0 (same as before)
            for (int i = dt.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(dt.Tables[0].Rows[i]["maxcount"]) != 0)
                {
                    dt.Tables[0].Rows.RemoveAt(i);
                }
            }

            // Remove row by matching ID
            for (int i = dt.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dt.Tables[0].Rows[i];
                if (row["id"].ToString() == id) // Compare the row ID with the provided ID
                {
                    dt.Tables[0].Rows.RemoveAt(i); // Remove row by index
                    rowRemoved = true; // Mark as removed
                    break; // Exit after the first match
                }
            }

            // Bind the updated DataSet to the Repeater controls
            rptSwitchDetails.DataSource = dt;
            rptSwitchDetails.DataBind();

            rptSwitchDetailsDelete.DataSource = dt;
            rptSwitchDetailsDelete.DataBind();

            // Update ViewState with the modified DataSet
            ViewState["Delete_dt"] = dt;

            // Optionally handle when no row is removed
            if (!rowRemoved)
            {
                // Handle the case where no matching row was found (could log, notify, etc.)
                Console.WriteLine($"No row found with ID: {id}");
            }
        }


        protected void btnManual_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("SwitchConfig | btnManual_Click() | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "SwitchConfig";
                _auditParams[2] = "btnManual";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                hdnshowmanual.Value = "true";
                hdnShowModal.Value = "false";
                BindManualDisableSwitch();
                ClientScript.RegisterStartupScript(this.GetType(), "showModalScript", "showModalIfTriggered();", true);
                ErrorLog.RuleTrace("SwitchConfig | btnManual_Click() | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: btnManual_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
        }

        //protected void btnUpdManual_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
        //        _RuleEntity.SwitchId = Convert.ToInt32(Session["id"].ToString()) != 0 ? Convert.ToInt32(Session["id"].ToString()) : 0;
        //        //_RuleEntity.BatchhName = !string.IsNullOrEmpty(txtBatchName.Text) ? txtBatchName.Text.Trim() : null;
        //        //_RuleEntity.percentage = Convert.ToInt32(txtBatchPercentage.Text) != 0 ? Convert.ToInt32(txtBatchPercentage.Text) : 0;
        //        //_RuleEntity.Count = Convert.ToInt32(txtBatchCount.Text) != 0 ? Convert.ToInt32(txtBatchCount.Text) : 0;
        //        _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;

        //        string statusCode = _RuleEntity.InsertOrUpdateBatch();
        //        if (statusCode == "INS00")
        //        {
        //            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
        //            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
        //            hdnshowmanual.Value = "false";
        //            //txtBatchName.Text = string.Empty;
        //            //txtBatchName.Text = string.Empty;
        //            //txtBatchName.Text = string.Empty;
        //            Session["id"] = null;
        //        }
        //        else
        //        {
        //            _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
        //            _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
        //        }
        //        var response = new
        //        {
        //            StatusMessage = _CommonEntity.ResponseMessage
        //        };
        //        ErrorLog.RuleTrace("TrRule: btnUpdManual_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.RuleTrace("SwitchConfig: btnUpdManual_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
        //    }
        //}

        protected void btnClsManual_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("SwitchConfig | btnClsManual_Click() | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "SwitchConfig";
                _auditParams[2] = "btnManual";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                hdnshowmanual.Value = "false";
                ErrorLog.RuleTrace("SwitchConfig | btnClsManual_Click() | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: btnClsManual_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
        }

        protected void rptManual_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Update")
                {
                    ErrorLog.RuleTrace("SwitchConfig | rptManual_ItemCommand-Update | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                    string id = e.CommandArgument.ToString();
                    _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                    _RuleEntity.SwitchId = Convert.ToInt32(id);
                    _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;

                    string statusCode = _RuleEntity.getmenualswitch();
                    if (statusCode == "INS00")
                    {
                        _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                        _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                        hdnshowmanual.Value = "false";
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
                    ErrorLog.RuleTrace("TrRule: btnUpdManual_Click() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                    ErrorLog.RuleTrace("SwitchConfig | rptManual_ItemCommand-Update | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: rptManual_ItemCommand() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("SwitchConfig | btnDelete_Click() | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "SwitchConfig";
                _auditParams[2] = "btnDelete";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                updatesTable = CreateUpdatesDataTable();

                if (ViewState["Delete_dt"] == null)
                {
                    #region % Validation

                    // Validate that TextBox3 (percentage) is 0
                    decimal switchPercentage;
                    if (decimal.TryParse(TextBox3.Text, out switchPercentage) && switchPercentage != 0)
                    {
                        lblDeleteMessage.Text = "Percentage in TextBox should be 0.";
                        lblDeleteMessage.Visible = true;
                        return;
                    }

                    // Initialize the total percentage accumulator
                    decimal totalPercentage = 0;

                    // Validate the percentages in the Repeater
                    foreach (RepeaterItem item in rptSwitchDetailsDelete.Items)
                    {
                        TextBox txtPercentage = (TextBox)item.FindControl("txtPercentage");

                        if (txtPercentage != null && decimal.TryParse(txtPercentage.Text, out decimal percentage))
                        {
                            // Check for negative percentages
                            if (percentage < 0)
                            {
                                lblDeleteMessage.Text = "Percentage cannot be negative.";
                                lblDeleteMessage.Visible = true;
                                return;
                            }

                            totalPercentage += percentage; // Accumulate the total percentage
                        }
                        else
                        {
                            // Handle invalid percentage input
                            lblDeleteMessage.Text = "Invalid percentage value.";
                            lblDeleteMessage.Visible = true;
                            return;
                        }
                    }

                    // Validate the total percentage must be exactly 100
                    if (totalPercentage != 100)
                    {
                        lblDeleteMessage.Text = "The total percentage of all items must be exactly 100%.";
                        lblDeleteMessage.Visible = true;
                        return;
                    }

                    // If validation passes, proceed with processing (e.g., database update)
                    lblDeleteMessage.Visible = false;  // Hide the error message if validation passes

                    #endregion

                    // Create a DataTable for updates
                    

                    // Iterate through the Repeater items and add data to the DataTable
                    foreach (RepeaterItem item in rptSwitchDetailsDelete.Items)
                    {
                        // Find controls within the Repeater item
                        var lblID = (Label)item.FindControl("lblID");
                        var txtPercentage = (TextBox)item.FindControl("txtPercentage");

                        // Proceed only if the controls are found
                        if (lblID != null && txtPercentage != null)
                        {
                            int id = Convert.ToInt32(lblID.Text);  // Get the ID from the Label
                            decimal percentage = Convert.ToDecimal(txtPercentage.Text);  // Get the percentage from the TextBox

                            // Add the values as a new row to the DataTable
                            updatesTable.Rows.Add(id, percentage);
                        }
                    }
                }


                // Assign the DataTable to the _RuleEntity entity (or whatever processing you need to do)
                _RuleEntity.dt = updatesTable;
                _RuleEntity.SwitchId = Convert.ToInt32(hdnSwitchId.Value) != 0 ? Convert.ToInt32(hdnSwitchId.Value) : 0;
                _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Delete;

                string statusCode = _RuleEntity.InsertOrUpdateSwitch();
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
                ErrorLog.RuleTrace("TrRule: EditGroup() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                HdnShowDelete.Value = "false";
                BindSwitch();
                ErrorLog.RuleTrace("SwitchConfig | btnDelete_Click() | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                lblDeleteMessage.Text = $"An error occurred: {Ex.Message}";
                lblDeleteMessage.Visible = true;
                HdnShowDelete.Value = "false";
                ErrorLog.RuleTrace("SwitchConfig: btnDelete_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
        }

        protected void btnCloseDel_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("SwitchConfig | btnCloseDel_Click() | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "SwitchConfig";
                _auditParams[2] = "btnCloseDel";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                HdnShowDelete.Value = "false";
                ErrorLog.RuleTrace("SwitchConfig | btnCloseDel_Click() | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: btnClsManual_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
        }

        protected void btnClearFailover_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.RuleTrace("SwitchConfig | btnClearFailover_Click() | Started. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = HttpContext.Current.Session["Username"].ToString();
                _auditParams[1] = "SwitchConfig";
                _auditParams[2] = "btnClearFailover";
                _auditParams[3] = HttpContext.Current.Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                if (HiddenField1.Value.ToString() == "Yes")
                {
                    ddlSwitch1.ClearSelection();
                    ddlSwitch2.ClearSelection();
                    ddlSwitch3.ClearSelection();
                    ddlSwitch4.ClearSelection();
                    ddlSwitch5.ClearSelection();
                    ddlSwitch6.ClearSelection();
                    txtswitch1.Text = string.Empty;
                    txtswitch2.Text = string.Empty;
                    txtswitch3.Text = string.Empty;
                    txtswitch4.Text = string.Empty;
                    txtswitch5.Text = string.Empty;
                    txtswitch6.Text = string.Empty;
                }
                ErrorLog.RuleTrace("SwitchConfig | btnClearFailover_Click() | Ended. | UserName : " + HttpContext.Current.Session["Username"].ToString() + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: btnClearFailover_Click() | Username : " + Session["Username"].ToString() + "Exception : " + Ex.Message + " | LoginKey : " + HttpContext.Current.Session["LoginKey"].ToString());
            }
        }
    }
}