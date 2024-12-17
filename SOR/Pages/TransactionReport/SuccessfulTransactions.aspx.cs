using AppLogger;
using BussinessAccessLayer;
//using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DecryptCardNumber;

namespace PayRakamSBM.Pages.TransactionReport
{
    public partial class SuccessfulTransactions : System.Web.UI.Page
    {
        #region Object Declration
        TransactionReportDAL _TransactionReportDAL = new TransactionReportDAL();
        string _cardNumber = null;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "SuccessfulTransactions.aspx", "27");
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
                        UserPermissions.RegisterStartupScriptForNavigationListActive("5", "27");
                        //Session["Username"] = "Maximus";
                        txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                        //BindDropdownsChannelType();
                        BindBc();
                        //FillGrid(EnumCollection.EnumBindingType.BindGrid);

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
                ErrorLog.TransactionReportTrace("SuccessfulTransactions: Page_Load: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
#endregion

        #region Grid Methods
        public DataSet FillGrid(EnumCollection.EnumBindingType _EnumBindingType)
        {
            DataSet _dsSuccessTransaction = null;
            try
            {
                SetPropertise();
                _TransactionReportDAL.PageIndex = gvSuccessfulTranaction.PageIndex;
                _TransactionReportDAL.flag = Convert.ToString((int)_EnumBindingType);
                _dsSuccessTransaction = _TransactionReportDAL.GetTransactions_Success();
               
                if (_dsSuccessTransaction != null && _dsSuccessTransaction.Tables.Count > 0 && _dsSuccessTransaction.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < _dsSuccessTransaction.Tables[0].Rows.Count; i++)
                    {
                        if (_dsSuccessTransaction.Tables[0].Rows[i]["Channel"].ToString() == "MATM")
                        {
                            try
                            {
                                _cardNumber = EncryptDecryptCard.DecryptCardNo(_dsSuccessTransaction.Tables[0].Rows[i][10].ToString());
                            }
                            catch (Exception Ex)
                            {
                                ErrorLog.CommonTrace("Class : pgAllTransaction.cs \nFunction : fillGrid() => Reading DataSet => Error While Decrypt Card Number : " + _cardNumber + " \nException Occured\n" + Ex.Message);
                            }

                            if (_cardNumber.Length == 19)
                            {
                                _dsSuccessTransaction.Tables[0].Rows[i][10] = _cardNumber.Substring(0, 6) + "XXXXXXXXX" + _cardNumber.Substring(_cardNumber.Length - 4, 4);
                            }
                            else if (_cardNumber.Length == 16)
                            {
                                _dsSuccessTransaction.Tables[0].Rows[i][10] = _cardNumber.Substring(0, 6) + "XXXXXX" + _cardNumber.Substring(_cardNumber.Length - 4, 4);
                            }
                        }
                    }
                }
                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                {
                    if (_dsSuccessTransaction != null && _dsSuccessTransaction.Tables.Count > 0 && _dsSuccessTransaction.Tables[0].Rows.Count > 0)
                    {
                        gvSuccessfulTranaction.VirtualItemCount = Convert.ToInt32(_dsSuccessTransaction.Tables[1].Rows[0][0]);
                        gvSuccessfulTranaction.DataSource = _dsSuccessTransaction.Tables[0];
                        gvSuccessfulTranaction.DataBind();
                        gvSuccessfulTranaction.Visible = true;
                        BtnCsv.Visible = true;
                        //BtnXls.Visible = true;
                        lblRecordCount.Text = "Total " + Convert.ToString(_dsSuccessTransaction.Tables[1].Rows[0][0]) + " Record(s) Found.";
                    }
                    else
                    {
                        gvSuccessfulTranaction.Visible = false;
                        BtnCsv.Visible = true;
                        //BtnXls.Visible = true;
                        lblRecordCount.Text = "Total 0 Record(s) Found.";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found In Search Criteria. Try again', 'Warning');", true);
                    }
                }
                        
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("SuccessfulTransactions: FillGrid: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return _dsSuccessTransaction;
        }
        public void SetPropertise()
        {
            try
            {

                _TransactionReportDAL.UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                _TransactionReportDAL.Fromdate = !string.IsNullOrEmpty(txtFromDate.Value) ? txtFromDate.Value.Trim() : null;
                _TransactionReportDAL.Todate = !string.IsNullOrEmpty(txtToDate.Value) ? txtToDate.Value.Trim() : null;
                _TransactionReportDAL.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _TransactionReportDAL.BCID = ddlBCCode.SelectedValue != "0" ? (ddlBCCode.SelectedValue) : null;
                _TransactionReportDAL.AgentCode = !string.IsNullOrEmpty(txtAgentCode.Value) ? txtAgentCode.Value.Trim() : null;
                _TransactionReportDAL.RRN = !string.IsNullOrEmpty(txtRRNo.Value) ? txtRRNo.Value.Trim() : null;
                _TransactionReportDAL.TransStatus = Convert.ToInt32(ddlTransactionStatus.SelectedValue) != 0 ? Convert.ToInt32(ddlTransactionStatus.SelectedValue) : 0;
                _TransactionReportDAL.CType = Convert.ToInt32(ddlChannelType.SelectedValue) != 0 ? Convert.ToInt32(ddlChannelType.SelectedValue) : 0;
                _TransactionReportDAL.TransType = ddlTranType.SelectedValue != "0" ? (ddlTranType.SelectedValue) : null;
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("SuccessfulTransactions: SetPropertise: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Page Index
        protected void gvSuccessfulTranaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSuccessfulTranaction.PageIndex = e.NewPageIndex;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("SuccessfulTransactions: gvSuccessfulTranaction_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Search
        protected void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(txtFromDate.Value) > Convert.ToDateTime(txtToDate.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('From date should be less than To date. Try again', 'Warning');", true);
                    return;
                }
                //else if (ddlChannelType.SelectedValue == "0")
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select Channel Type. Try again', 'Warning');", true);
                //    return;
                //}
                else
                {
                    FillGrid(EnumCollection.EnumBindingType.BindGrid);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("SuccessfulTransactions: buttonSearch_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Clear Button
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                ddlChannelType.ClearSelection();
                ddlBCCode.ClearSelection();
                txtAgentCode.Value = string.Empty;
                ddlTranType.ClearSelection();
                ddlTransactionStatus.ClearSelection();
                txtRRNo.Value = string.Empty;
                gvSuccessfulTranaction.DataSource = null;
                gvSuccessfulTranaction.DataBind();
                gvSuccessfulTranaction.PageIndex = 0;
                lblRecordCount.Text = string.Empty;
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("SuccessfulTransactions: btnClear_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Export
        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                //string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "Successful Transactions", dt);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("SuccessfulTransactions: BtnXls_Click: Exception: " + Ex.Message);
            }
        }

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                //string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "Successful Transactions", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("SuccessfulTransactions: BtnCsv_Click: Exception: " + Ex.Message);
            }
        }

        #endregion

        #region DropdownBindBc
        private void BindBc()
        {
            try
            {
                ddlBCCode.Items.Clear();
                ddlBCCode.DataSource = null;
                ddlBCCode.DataBind();

                _TransactionReportDAL.UserName = Session["Username"].ToString();

                _TransactionReportDAL.CreatedBy = Session["Username"].ToString();
                _TransactionReportDAL.IsRemoved = "0";
                _TransactionReportDAL.IsActive = "1";
                _TransactionReportDAL.IsdocUploaded = "1";
                _TransactionReportDAL.VerificationStatus = "1";
                //_ReportEntity.Clientcode = ddlClientCode.SelectedValue != "0" ? ddlClientCode.SelectedValue : null;
                _TransactionReportDAL.Flag = (int)EnumCollection.EnumBindingType.BindGrid;

                // _ReportEntity.BCID = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;

                DataSet ds = _TransactionReportDAL.BindBCddl();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        ddlBCCode.Items.Clear();
                        ddlBCCode.DataSource = ds.Tables[0].Copy();
                        ddlBCCode.DataTextField = "BCNameDetails";
                        ddlBCCode.DataValueField = "BCCode";
                        ddlBCCode.DataBind();
                        ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                    }
                    else
                    {
                        ddlBCCode.Items.Clear();
                        ddlBCCode.DataSource = ds.Tables[0].Copy();
                        ddlBCCode.DataTextField = "BCNameDetails";
                        ddlBCCode.DataValueField = "BCCode";
                        ddlBCCode.DataBind();
                        ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                    }
                }
                else
                {
                    ddlBCCode.DataSource = null;
                    ddlBCCode.DataBind();
                    ddlBCCode.Items.Insert(0, new ListItem("All", "0"));
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("SuccessfulTransactions: BindBc: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

    }
}