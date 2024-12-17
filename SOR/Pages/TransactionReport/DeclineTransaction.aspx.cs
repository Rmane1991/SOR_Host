using AppLogger;
using BussinessAccessLayer;
using System;
using System.Data;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using DecryptCardNumber;
using System.Configuration;
using System.Web;

namespace PayRakamSBM.Pages.TransactionReport
{
    public partial class DeclineTransaction : System.Web.UI.Page
    {

        #region Object Declration
        TransactionReportDAL _TransactionReportDAL = new TransactionReportDAL();
        string _cardNumber = null;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                //bool HasPagePermission = true;
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "DeclineTransaction.aspx", "28");
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
                     UserPermissions.RegisterStartupScriptForNavigationListActive("5", "28");
                    //Session["Username"] = "Maximus";
                    txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
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
                ErrorLog.TransactionReportTrace("DeclineTransaction: Page_Load: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        #region Grid Methods
        public DataSet FillGrid(EnumCollection.EnumBindingType _EnumBindingType)
        {
            DataSet _dsDeclineTransaction = null;
            try
            {
                SetPropertise();
                _TransactionReportDAL.flag = Convert.ToString((int)_EnumBindingType);
                _dsDeclineTransaction = _TransactionReportDAL.GetTransactions_Decline();
                
                if (_dsDeclineTransaction != null && _dsDeclineTransaction.Tables.Count > 0 && _dsDeclineTransaction.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < _dsDeclineTransaction.Tables[0].Rows.Count; i++)
                    {
                        if (_dsDeclineTransaction.Tables[0].Rows[i]["Channel"].ToString() == "MATM")
                        {
                            try
                            {
                                _cardNumber = EncryptDecryptCard.DecryptCardNo(_dsDeclineTransaction.Tables[0].Rows[i][10].ToString());
                            }
                            catch (Exception Ex)
                            {
                                ErrorLog.CommonTrace("Class : pgAllTransaction.cs \nFunction : fillGrid() => Reading DataSet => Error While Decrypt Card Number : " + _cardNumber + " \nException Occured\n" + Ex.Message);
                            }

                            if (_cardNumber.Length == 19)
                            {
                                _dsDeclineTransaction.Tables[0].Rows[i][10] = _cardNumber.Substring(0, 6) + "XXXXXXXXX" + _cardNumber.Substring(_cardNumber.Length - 4, 4);
                            }
                            else if (_cardNumber.Length == 16)
                            {
                                _dsDeclineTransaction.Tables[0].Rows[i][10] = _cardNumber.Substring(0, 6) + "XXXXXX" + _cardNumber.Substring(_cardNumber.Length - 4, 4);
                            }
                        }
                    }
                }
                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                    if (_dsDeclineTransaction != null && _dsDeclineTransaction.Tables.Count > 0 && _dsDeclineTransaction.Tables[0].Rows.Count > 0)
                    {
                        gvDeclineTranaction.VirtualItemCount = Convert.ToInt32(_dsDeclineTransaction.Tables[1].Rows[0][0]);
                        gvDeclineTranaction.DataSource = _dsDeclineTransaction.Tables[0];
                        gvDeclineTranaction.DataBind();
                        gvDeclineTranaction.Visible = true;
                        BtnCsv.Visible = true;
                        //BtnXls.Visible = true;
                        lblRecordCount.Text = "Total " + Convert.ToString(_dsDeclineTransaction.Tables[1].Rows[0][0]) + " Record(s) Found.";
                    }
                    else
                    {
                        gvDeclineTranaction.Visible = false;
                        BtnCsv.Visible = false;
                        //BtnXls.Visible = false;
                        lblRecordCount.Text = "Total 0 Record(s) Found.";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found In Search Criteria. Try again', 'Warning');", true);
                    }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("DeclineTransaction: FillGrid: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return _dsDeclineTransaction;
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
                ErrorLog.TransactionReportTrace("DeclineTransaction: SetPropertise: Exception: " + Ex.Message);
            }
        }
        #endregion

        #region Page Index
        protected void gvDeclineTranaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDeclineTranaction.PageIndex = e.NewPageIndex;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("DeclineTransaction: gvDeclineTranaction_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "Decline Transaction", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("DeclineTransaction: BtnCsv_Click: Exception: " + Ex.Message);
            }
        }

        protected void BtnXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExportFormat _ExportFormat = new ExportFormat();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "Decline Transaction", dt);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("DeclineTransaction: BtnXls_Click: Exception: " + Ex.Message);
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
                else
                {
                    FillGrid(EnumCollection.EnumBindingType.BindGrid);
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("DeclineTransaction: buttonSearch_Click: Exception: " + Ex.Message);
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
                txtRRNo.Value = string.Empty;
                ddlBCCode.ClearSelection();
                ddlTransactionStatus.ClearSelection();
                ddlTranType.ClearSelection();
                ddlChannelType.ClearSelection();
                gvDeclineTranaction.DataSource = null;
                gvDeclineTranaction.DataBind();
                gvDeclineTranaction.PageIndex = 0;
                lblRecordCount.Text = string.Empty;
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("DeclineTransaction: btnClear_Click: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region SetPageFiltersExport
        private string SetPageFiltersExport()
        {
            string pageFilters = string.Empty;
            try
            {
                pageFilters = "Generated By" + Convert.ToString(Session["Username"]);
                pageFilters += "From Date" + (Convert.ToDateTime(_TransactionReportDAL.Fromdate)).ToString("dd/MM/yyyy");
                pageFilters += "To Date" + (Convert.ToDateTime(_TransactionReportDAL.Todate)).ToString("dd/MM/yyyy");
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("DeclineTransaction: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }
        #endregion

        #region Dropdown
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
                ErrorLog.TransactionReportTrace("DeclineTransaction: BindBc: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion
    }
}