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
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
                            BindSwitch();
                            BindDropdown();
                            InitializeDataTable();
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
                ErrorLog.DashboardTrace("SwitchConfig : Page_Load(): Exception: " + Ex.Message);
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

                rptrSwitch.DataSource = dt;
                rptrSwitch.DataBind();

                rptSwitchDetails.DataSource = dt;
                rptSwitchDetails.DataBind();
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
                hdnShowModal.Value = "true";
                BindSwitch();
                BindDropdown();
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: btnAddGroup_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
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
                if (string.IsNullOrEmpty(ddlSwitch1.SelectedValue) &&
                    string.IsNullOrEmpty(ddlSwitch2.SelectedValue) &&
                    string.IsNullOrEmpty(ddlSwitch3.SelectedValue) &&
                    string.IsNullOrEmpty(ddlSwitch4.SelectedValue) &&
                    string.IsNullOrEmpty(ddlSwitch5.SelectedValue) &&
                    string.IsNullOrEmpty(ddlSwitch6.SelectedValue))
                {
                    
                }
                else

                {
                    if (!CheckForDuplicateValues())
                    {
                        return;
                    }
                    if (!ValidateSwitchValues())
                    {
                        return;
                    }
                    if (!ValidateSwitchInputs())
                    {
                        return;
                    }

                    PopulateAndAddSwitchValues();
                }

                #region DT
                DataTable resultTable = new DataTable();
                resultTable.Columns.Add("ID", typeof(string));
                resultTable.Columns.Add("Value", typeof(string));
                resultTable.Columns.Add("TabValue", typeof(string));
                #endregion

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

                DataTable updatesTable = CreateUpdatesDataTable();

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

                if (string.IsNullOrEmpty(txtSwitchName.Text))
                {
                    ShowWarning("Please Enter Switch Name. Try again", "Warning");
                    return;
                }
                else if (string.IsNullOrEmpty(txtSwitchDescription.Text))
                {
                    ShowWarning("Please Enter Switch Description. Try again", "Warning");
                    return;
                }
                else if (string.IsNullOrEmpty(txtSwitchPercentage.Text))
                {
                    ShowWarning("Please Enter Switch Percentage. Try again", "Warning");
                    return;
                }
                else
                {
                    if (Session["SwitchPercentage"] != null)
                    {
                        //string[] columnNames = { "switchname", "txntype" }; // Adjust as necessary
                        //List<string> selectedIdList = new List<string>();

                        //foreach (var column in columnNames)
                        //{
                        //    string[] selectedIds = Request.Form.GetValues("chk" + column);

                        //    if (selectedIds != null && selectedIds.Length > 0)
                        //    {
                        //        // Process the selected IDs
                        //        foreach (var id in selectedIds)
                        //        {
                        //            selectedIdList.Add(id); // Store or process the ID as needed
                        //        }
                        //    }
                        //}
                        //if (selectedIdList.Count > 0)
                        //{
                        //    _RuleEntity.FailoverSwitchId = string.Join(",", selectedIdList.Where(id => columnNames[0] == "switchname"));
                        //    _RuleEntity.TxnType = string.Join(",", selectedIdList.Where(id => columnNames[1] == "txntype"));
                        //}
                        // Initialize the entity

                        //string[] columnNames = { "switchname", "txntype" }; // Adjust as necessary
                        //List<string> failoverSwitchIds = new List<string>();
                        //List<string> txnTypes = new List<string>();

                        //foreach (var column in columnNames)
                        //{
                        //    string[] selectedIds = Request.Form.GetValues("chk" + column);
                        //    if (selectedIds != null && selectedIds.Length > 0)
                        //    {
                        //        foreach (var id in selectedIds)
                        //        {
                        //            if (column == "switchname")
                        //            {
                        //                failoverSwitchIds.Add(id); // Store switchname IDs
                        //            }
                        //            else if (column == "txntype")
                        //            {
                        //                txnTypes.Add(id); // Store txntype IDs
                        //            }
                        //        }
                        //    }
                        //}
                        //if (failoverSwitchIds.Count > 0)
                        //{
                        //    _RuleEntity.FailoverSwitchId = string.Join(",", failoverSwitchIds); // Join IDs for FailoverSwitchId
                        //}
                        //if (txnTypes.Count > 0)
                        //{
                        //    _RuleEntity.TxnType = string.Join(",", txnTypes); // Join IDs for TxnType
                        //}

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

                        _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _RuleEntity.SwitchName = !string.IsNullOrEmpty(txtSwitchName.Text) ? txtSwitchName.Text.Trim() : null;
                        _RuleEntity.SwitchDescription = !string.IsNullOrEmpty(txtSwitchDescription.Text) ? txtSwitchDescription.Text.Trim() : null;
                        _RuleEntity.SwitchId = Convert.ToInt32(hdnSwitchId.Value) != 0 ? Convert.ToInt32(hdnSwitchId.Value) : 0;
                        _RuleEntity.percentage = int.TryParse(txtSwitchPercentage.Text.Trim(), out int percentage) ? percentage : 0;
                        _RuleEntity.Count = int.TryParse(txtCount.Text.Trim(), out int count) ? count : 0;
                        //_RuleEntity.Failoverpercentage = int.TryParse(txtPercentageFailover1.Value.Trim(), out int Failoverpercentage) ? Failoverpercentage : 0;
                        _RuleEntity.dt = updatesTable;
                        _RuleEntity.dt2 = resultTable;
                        _RuleEntity.dt3 = dtSwitchValues;
                        _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Edit;

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

                    }
                    else
                    {
                        

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

                                    if(!string.IsNullOrEmpty(value))
                                    {
                                        resultTable.Rows.Add(newRow);
                                    }
                                }
                            }
                        }
                        
                        _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                        _RuleEntity.SwitchName = !string.IsNullOrEmpty(txtSwitchName.Text) ? txtSwitchName.Text.Trim() : null;
                        _RuleEntity.SwitchDescription = !string.IsNullOrEmpty(txtSwitchDescription.Text) ? txtSwitchDescription.Text.Trim() : null;
                        _RuleEntity.percentage = int.TryParse(txtSwitchPercentage.Text.Trim(), out int percentage) ? percentage : 0;
                        _RuleEntity.Count = int.TryParse(txtCount.Text.Trim(), out int count) ? count : 0;
                        //_RuleEntity.FailoverSwitchId = !string.IsNullOrEmpty(hfSelectedValues.Value) ? hfSelectedValues.Value.Trim() : null;
                        //_RuleEntity.Failoverpercentage = int.TryParse(txtPercentageFailover1.Value.Trim(), out int Failoverpercentage) ? Failoverpercentage : 0;
                        _RuleEntity.dt = updatesTable;
                        _RuleEntity.dt2 = resultTable;
                        _RuleEntity.dt3 = dtSwitchValues;
                        //_RuleEntity.Switchurl = !string.IsNullOrEmpty(txturl.Text) ? txturl.Text.Trim() : null;
                        //_RuleEntity.TxnType = !string.IsNullOrEmpty(hfSelectedSecondValues.Value) ? hfSelectedSecondValues.Value.Trim() : null;
                        _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.Insert;

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
                }
                hdnShowModal.Value = "false";
                BindSwitch();
            }
            catch (Exception Ex)
            {
                hdnShowModal.Value = "false";
                ErrorLog.RuleTrace("TrRule: btnCreGroup_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
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
                hdnShowModal.Value = "false";
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: btnAddGroup_Click() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
            }
        }
        [WebMethod]
        public static string ToggleSlider(bool IsChecked, string Id)
        {
            bool isActive = IsChecked;
            _RuleEntityy.IsActive = isActive ? 0 : 1;
            _RuleEntityy.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
            _RuleEntityy.SwitchId = Convert.ToInt32(Id);
            _RuleEntityy.Flag = (int)EnumCollection.EnumRuleType.Update;

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
            if (e.CommandName == "Edit")
            {
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
                    txtSwitchDescription.Text = row1["description"].ToString();
                    txtSwitchPercentage.Text = row1["percentage"].ToString();
                    Session["SwitchPercentage"] = txtSwitchPercentage.Text;
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
                        string type = rows["txntype"].ToString();
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
            }
            else if (e.CommandName == "Delete")
            {
                int itemId = Convert.ToInt32(e.CommandArgument);
                _RuleEntityy.IsDelete = 1;
                _RuleEntityy.UserName = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Username"])) ? Convert.ToString(HttpContext.Current.Session["Username"]) : null;
                _RuleEntityy.SwitchId = Convert.ToInt32(itemId);
                _RuleEntityy.Flag = (int)EnumCollection.EnumRuleType.Delete;

                string statusCode = _RuleEntityy.UpdateSwitchStatus();

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
                ErrorLog.RuleTrace("SwitchConfig: Delete() | DB_StatusCode : " + statusCode + " | ResponseCode : " + _CommonEntity.ResponseCode + " | ResponseMessage : " + _CommonEntity.ResponseMessage);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showSuccess('" + _CommonEntity.ResponseMessage + "');", true);
                BindSwitch();
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
                            string type = row["txntype"].ToString();
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
                if (e.CommandName == "Edit")
                {
                    // Find the TextBox in the current row (RepeaterItem)
                    TextBox txtPercentage = (TextBox)e.Item.FindControl("txtPercentage");
                    txtPercentage.Enabled = true;
                }
                else if (e.CommandName == "Update")
                {
                    // Find the TextBox in the current row and disable it after updating
                    TextBox txtPercentage = (TextBox)e.Item.FindControl("txtPercentage");
                    txtPercentage.Enabled = false;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.RuleTrace("SwitchConfig: rptSwitchDetails_ItemCommand() | Username :" + Session["Username"].ToString() + "Exception : " + Ex.Message);
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
        private DataTable CreateUpdatesDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("percentage", typeof(int));
            return dt;
        }
        
        private bool ValidateSwitchValues()
        {
            // Get values from the text boxes
            int switch1 = GetSwitchValue(txtswitch1);
            int switch2 = GetSwitchValue(txtswitch2);
            int switch3 = GetSwitchValue(txtswitch3);
            int switch4 = GetSwitchValue(txtswitch4);
            int switch5 = GetSwitchValue(txtswitch5);
            int switch6 = GetSwitchValue(txtswitch6);

            // Calculate the sum
            int total = switch1 + switch2 + switch3 + switch4 + switch5 + switch6;

            // Check if total is exactly 100
            if (total < 100)
            {
                ShowWarning("The total must not be less than 100.");
                return false; // Indicate validation failure
            }

            if (total > 100)
            {
                ShowWarning("The total must not be greater than 100.");
                return false; // Indicate validation failure
            }

            return true; // Indicate successful validation
        }
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
        private void PopulateAndAddSwitchValues()
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

            // Loop through each pair and add values to the DataTable
            foreach (var pair in switchPairs)
            {
                if (pair.Dropdown.SelectedIndex > 0 && !string.IsNullOrEmpty(pair.Textbox.Text))
                {
                    // Create a new DataRow
                    DataRow row = dtSwitchValues.NewRow();
                    row["SwitchId"] = pair.Dropdown.SelectedValue; // Use the selected value of the dropdown
                    row["SwitchValue"] = pair.Textbox.Text; // Use the text box value

                    // Add the DataRow to the DataTable
                    dtSwitchValues.Rows.Add(row);
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
                }
                // Switch 2
                else if (i == 1)
                {
                    ddlSwitch2.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch2.Text = row["failover_percentage"].ToString();
                }
                // Switch 3
                else if (i == 2)
                {
                    ddlSwitch3.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch3.Text = row["failover_percentage"].ToString();
                }
                // Switch 4
                else if (i == 3)
                {
                    ddlSwitch4.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch4.Text = row["failover_percentage"].ToString();
                }
                // Switch 5
                else if (i == 4)
                {
                    ddlSwitch5.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch5.Text = row["failover_percentage"].ToString();
                }
                // Switch 6
                else if (i == 5)
                {
                    ddlSwitch6.SelectedValue = row["failoverswitchid"].ToString();
                    txtswitch6.Text = row["failover_percentage"].ToString();
                }
            }
        }
        public void RemoveRowById(string id)
        {
            // Use a flag to track if a row was removed
            bool rowRemoved = false;

            _RuleEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
            _RuleEntity.Flag = (int)EnumCollection.EnumRuleType.BindGrid;
            DataSet dt = _RuleEntity.GetSwitch();

            // Loop through the rows in reverse order to avoid issues with changing collection
            for (int i = dt.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dt.Tables[0].Rows[i];
                if (row["id"].ToString() == id) // Compare with the ID value
                {
                    dt.Tables[0].Rows.Remove(row);
                    rowRemoved = true; // Mark that a row was removed
                    break; // Exit after removing the first matching row
                }
            }
            rptSwitchDetails.DataSource = dt;
            rptSwitchDetails.DataBind();
            if (!rowRemoved)
                // Handle the case where no matching row was found, if needed
                Console.WriteLine($"No row found with ID: {id}");
        }
    }
}