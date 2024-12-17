using AppLogger;
using BussinessAccessLayer;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOR.Pages.Monitoring
{
    public partial class AlertTemplateMaster : System.Web.UI.Page
    {
        #region objects
        public static CommonEntity _CommonEntity = new CommonEntity();
        AlertEntity _alertEntity = new AlertEntity();
        DataTable dt = new DataTable();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillTemplates();
                BindGridView();
            }
        }

        protected void ddlTemplateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillTemplates();

                //string type = ddlTemplateType.SelectedValue;

                //dt = _alertEntity.GetTemplateMaster(type);
                //ddlTemplateList.Items.Clear();
                //ddlTemplateList.DataSource = dt;
                //ddlTemplateList.DataTextField = "templatename";
                //ddlTemplateList.DataValueField = "id";
                //ddlTemplateList.DataBind();
                //ddlTemplateList.Items.Insert(0, new ListItem("--Select--", "0"));
                //ddlTemplateList.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("AlertTemplateMaster: ddlTemplateType_SelectedIndexChanged(): Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        private void FillTemplates()
        {
            try
            {
                string type = ddlTemplateType.SelectedValue;

                dt = _alertEntity.GetTemplateMaster(type);
                ddlTemplateList.Items.Clear();
                ddlTemplateList.DataSource = dt;
                ddlTemplateList.DataTextField = "templatename";
                ddlTemplateList.DataValueField = "id";
                ddlTemplateList.DataBind();
                ddlTemplateList.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlTemplateList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("AlertTemplateMaster: FillTemplates(): Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        private void BindGridView()
        {
            try
            {
                dt = _alertEntity.gridBind();
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvTemplateMaster.DataSource = dt;
                    gvTemplateMaster.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("AlertTemplateMaster: BindGridView(): Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void gvTemplateMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EDT")
                {
                    string arg = e.CommandArgument.ToString();
                    var args = arg.Split('=');

                    if (args.Length == 2)
                    {
                        string id = args[0];
                        string tempid = args[1];
                        _alertEntity.AlertGroupId = id;
                        _alertEntity.AlertType = tempid;
                        DataTable dnew = _alertEntity.getEditData();
                        FillTemplates();
                        ddlTemplateType.SelectedValue = dnew.Rows[0]["templatetype"].ToString();
                        ddlTemplateList.SelectedValue = dnew.Rows[0]["templateid"].ToString();
                        txtsubject.Text = dnew.Rows[0]["subject"].ToString();
                        txtBodyContent.Text = dnew.Rows[0]["mailbody"].ToString();
                        btnSubmit.Text = "Update";
                        hidAction.Value = "4";
                        hidUpdateID.Value = id;
                    }
                    else
                    {

                    }
                }
                else if (e.CommandName == "DLT")
                {
                    string arg = e.CommandArgument.ToString();
                    _alertEntity.Id = arg[0].ToString();
                    _alertEntity.flag = (int)EnumCollection.EnumRuleType.Delete;
                    string statusCode = _alertEntity.InsUpdTmpltContentMaster();
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
                }
                var response = new
                {
                    StatusMessage = _CommonEntity.ResponseMessage
                };
                BindGridView();
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("AlertTemplateMaster: gvTemplateMaster_PageIndexChanging: Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        protected void gvTemplateMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTemplateMaster.PageIndex = e.NewPageIndex;
                BindGridView();
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("AlertTemplateMaster: gvTemplateMaster_PageIndexChanging: Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        protected void gvTemplateMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                _alertEntity.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _alertEntity.AlertType = ddlTemplateType.SelectedValue;
                _alertEntity.TemplateName = ddlTemplateList.SelectedItem.Text;
                _alertEntity.TemplateID = ddlTemplateList.SelectedValue;
                _alertEntity.Subject = txtsubject.Text.Trim();
                _alertEntity.EmailBody = txtBodyContent.Text;
                if (hidAction.Value == "4") { _alertEntity.flag = (int)EnumCollection.EnumRuleType.Update; _alertEntity.Id = hidUpdateID.Value; } else { _alertEntity.flag = (int)EnumCollection.EnumRuleType.Insert; }
                string statusCode = _alertEntity.InsUpdTmpltContentMaster();

                if (statusCode == "INS00")
                {
                    _CommonEntity.ResponseCode = CommonEntity.GetResponseCode(statusCode, (int)EnumCollection.TransactionSource.Others);
                    _CommonEntity.ResponseMessage = CommonEntity.GetResponseCodeDescription(_CommonEntity.ResponseCode, (int)EnumCollection.TransactionSource.Others);
                }
                else if (statusCode == "UPD00")
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
                ClearFileds();
                BindGridView();
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("AlertTemplateMaster: gvTemplateMaster_PageIndexChanging: Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFileds();
        }


        public void ClearFileds()
        {
            try
            {
                ddlTemplateList.SelectedIndex = 0;
                ddlTemplateType.SelectedIndex = 0;
                ddlTemplateList.SelectedItem.Text = string.Empty;
                txtsubject.Text = string.Empty;
                txtBodyContent.Text = string.Empty;
                btnSubmit.Text = "Submit";
                hidAction.Value = string.Empty;
                hidUpdateID.Value = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorLog.DashboardTrace("AlertTemplateMaster: ClearFileds(): Exception: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }

        }

    }
}