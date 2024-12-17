using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessAccessLayer;
using AppLogger;
using System.Threading;
using MaxiSwitch.EncryptionDecryption;
using DecryptCardNumber;
using System.Configuration;
using System.Web;

namespace SOR.Pages.TransactionReport
{
    public partial class MATMTransactions : System.Web.UI.Page
    {
        #region Object Declration
        TransactionReportDAL _TransactionReportDAL = new TransactionReportDAL();
        AggregatorEntity _AggregatorEntity = new AggregatorEntity();
        string _cardNumber = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "MATMTransactions.aspx", "23");
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
                             UserPermissions.RegisterStartupScriptForNavigationListActive("7", "23");
                             txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                             txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                             //Session["Username"] = "Maximus";
                             BindBc();
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
                ErrorLog.TransactionReportTrace("MATMTransactions: Page_Load: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }

        #region Grid Methods
        public DataSet FillGrid(EnumCollection.EnumBindingType _EnumBindingType, string sortExpression = null)
        {
            DataSet _dsTransactionLogs = null;
            try
            {
                Setpropertise();
                _TransactionReportDAL.Flag = Convert.ToInt32(_EnumBindingType);
                _dsTransactionLogs = _TransactionReportDAL.GetMATMTransactionReport();
                

                if (_dsTransactionLogs != null && _dsTransactionLogs.Tables.Count > 0 && _dsTransactionLogs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < _dsTransactionLogs.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            _cardNumber = EncryptDecryptCard.DecryptCardNo(_dsTransactionLogs.Tables[0].Rows[i][10].ToString());
                        }
                        catch (Exception Ex)
                        {
                            ErrorLog.CommonTrace("Class : MATMTransactions.cs \nFunction : FillGrid() => Reading DataSet => Error While Decrypt Card Number : " + _cardNumber + " \nException Occured\n" + Ex.Message);
                        }

                        if (_cardNumber.Length == 19)
                        {
                            _dsTransactionLogs.Tables[0].Rows[i][10] = _cardNumber.Substring(0, 6) + "XXXXXXXXX" + _cardNumber.Substring(_cardNumber.Length - 4, 4);
                        }
                        else if (_cardNumber.Length == 16)
                        {
                            _dsTransactionLogs.Tables[0].Rows[i][10] = _cardNumber.Substring(0, 6) + "XXXXXX" + _cardNumber.Substring(_cardNumber.Length - 4, 4);
                        }
                    }
                }

                if (_EnumBindingType == EnumCollection.EnumBindingType.BindGrid)
                {
                    if (_dsTransactionLogs != null && _dsTransactionLogs.Tables.Count > 0 && _dsTransactionLogs.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            DataView dv = _dsTransactionLogs.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";

                            dv.Sort = sortExpression + " " + this.SortDirection;
                            gvMATMTransaction.DataSource = dv;
                            gvMATMTransaction.DataBind();
                            gvMATMTransaction.Visible = true;
                            //BtnXls.Visible = true;
                            BtnCsv.Visible = true;
                            lblRecordCount.Text = "Total " + Convert.ToString(_dsTransactionLogs.Tables[0].Rows.Count) + " Record(s) Found.";
                        }
                        else
                        {
                            gvMATMTransaction.VirtualItemCount = Convert.ToInt32(_dsTransactionLogs.Tables[1].Rows[0][0]);
                            gvMATMTransaction.DataSource = _dsTransactionLogs.Tables[0];
                            gvMATMTransaction.DataBind();
                            gvMATMTransaction.Visible = true;
                            BtnCsv.Visible = true;
                            //BtnXls.Visible = true;
                            lblRecordCount.Text = "Total " + Convert.ToString(_dsTransactionLogs.Tables[1].Rows[0][0]) + " Record(s) Found.";
                        }
                    }

                    else
                    {
                        gvMATMTransaction.Visible = false;
                        BtnCsv.Visible = false;
                        //BtnXls.Visible = false;
                        lblRecordCount.Text = "Total 0 Record(s) Found.";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found In Search Criteria. Try again', 'Warning');", true);
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("MATMTransactions: FillGrid: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return _dsTransactionLogs;
        }
        #endregion

        #region Setpropertise
        public void Setpropertise()
        {
            try
            {
                _TransactionReportDAL.UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                _TransactionReportDAL.Fromdate = !string.IsNullOrEmpty(txtFromDate.Value) ? Convert.ToDateTime(txtFromDate.Value).ToString("yyyy-MM-dd") : null;
                _TransactionReportDAL.Todate = !string.IsNullOrEmpty(txtToDate.Value) ? Convert.ToDateTime(txtToDate.Value).ToString("yyyy-MM-dd") : null;
                _TransactionReportDAL.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _TransactionReportDAL.BCID = ddlBCCode.SelectedValue != "0" ? (ddlBCCode.SelectedValue) : null;
                _TransactionReportDAL.AgentCode = !string.IsNullOrEmpty(txtAgentCode.Value) ? txtAgentCode.Value.Trim() : null;
                _TransactionReportDAL.RRN = !string.IsNullOrEmpty(txtRRNo.Value) ? txtRRNo.Value.Trim() : null;
                _TransactionReportDAL.TransStatus = Convert.ToInt32(ddlTransactionStatus.SelectedValue) != 0 ? Convert.ToInt32(ddlTransactionStatus.SelectedValue) : 0;
                _TransactionReportDAL.CType = Convert.ToInt32(ddlChannelType.SelectedValue) != 0 ? Convert.ToInt32(ddlChannelType.SelectedValue) : 0;
                _TransactionReportDAL.TransType = ddlTranType.SelectedValue != "0" ? (ddlTranType.SelectedValue) : null;
                _TransactionReportDAL.PageIndex = gvMATMTransaction.PageIndex;
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("MATMTransactions: Setpropertise: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region CardNumber
        public string CardNumber(string _cipherCardNumber)
        {
            try
            {
                _cipherCardNumber = ConnectionStringEncryptDecrypt.DecryptString(_cipherCardNumber);
                if (_cardNumber.Length == 19)
                {
                    _cipherCardNumber = _cardNumber.Substring(0, 6) + "XXXXXXXXX" + _cardNumber.Substring(_cardNumber.Length - 4, 4);
                }
                else if (_cardNumber.Length == 16)
                {
                    _cipherCardNumber = _cardNumber.Substring(0, 6) + "XXXXXX" + _cardNumber.Substring(_cardNumber.Length - 4, 4);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("MATMTransactions: CardNumber: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return _cipherCardNumber;
        }
        #endregion

        #region Search Button
        protected void btnSearch_ServerClick(object sender, EventArgs e)
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
                ErrorLog.TransactionReportTrace("MATMTransactions: btnSearch_ServerClick: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Clear
        protected void btnClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
               
                txtAgentCode. Value= string.Empty;
                ddlBCCode.ClearSelection();
                txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtRRNo.Value = string.Empty;
                ddlTransactionStatus.ClearSelection();
                ddlTranType.ClearSelection();
                ddlChannelType.ClearSelection();
                gvMATMTransaction.DataSource = null;
                gvMATMTransaction.DataBind();
                gvMATMTransaction.PageIndex = 0;
                lblRecordCount.Text = string.Empty;
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("MATMTransactions: btnClear_ServerClick: Exception: " + Ex.Message);
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
                string pageFilters = SetPageFiltersExport();
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);
                DataSet dsExport = dt.Copy();

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "PayRakam", "MATMT ransactions", dt);
                }
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("MATMTransactions: BtnXls_Click: Exception: " + Ex.Message);
            }
        }

        protected void BtnCsv_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataSet dt = FillGrid(EnumCollection.EnumBindingType.Export);
                ExportFormat _ExportFormat = new ExportFormat();
                string pageFilters = SetPageFiltersExport();

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "PayRakam", "MATM Transactions", dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                }

            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("MATMTransactions: BtnCsv_Click: Exception: " + Ex.Message);
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
                ErrorLog.TransactionReportTrace("MATMTransactions: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }
        #endregion

        #region Grid Events
        protected void gvMATMTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvMATMTransaction.PageIndex = e.NewPageIndex;
                FillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("MATMTransactions: gvMATMTransaction_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Sorting
        protected void gvMATMTransaction_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                this.FillGrid(EnumCollection.EnumBindingType.BindGrid, e.SortExpression);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("MATMTransactions: gvMATMTransaction_Sorting: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        #endregion

        #region DropdownBindBc
        private void BindBc()
        {
            //try
            //{
            //    ddlBCCode.Items.Clear();
            //    ddlBCCode.DataSource = null;
            //    ddlBCCode.DataBind();

            //    _TransactionReportDAL.UserName = Session["Username"].ToString();

            //    _TransactionReportDAL.CreatedBy = Session["Username"].ToString();
            //    _TransactionReportDAL.IsRemoved = "0";
            //    _TransactionReportDAL.IsActive = "1";
            //    _TransactionReportDAL.IsdocUploaded = "1";
            //    _TransactionReportDAL.VerificationStatus = "1";
            //    //_ReportEntity.Clientcode = ddlClientCode.SelectedValue != "0" ? ddlClientCode.SelectedValue : null;
            //    _TransactionReportDAL.Flag = (int)EnumCollection.EnumBindingType.BindGrid;

            //    // _ReportEntity.BCID = ddlBCCode.SelectedValue != "0" ? ddlBCCode.SelectedValue : null;

            //    DataSet ds = _TransactionReportDAL.BindBCddl();

            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        if (ds.Tables[0].Rows.Count == 1)
            //        {
            //            ddlBCCode.Items.Clear();
            //            ddlBCCode.DataSource = ds.Tables[0].Copy();
            //            ddlBCCode.DataTextField = "BCNameDetails";
            //            ddlBCCode.DataValueField = "BCCode";
            //            ddlBCCode.DataBind();
            //            ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
            //        }
            //        else
            //        {
            //            ddlBCCode.Items.Clear();
            //            ddlBCCode.DataSource = ds.Tables[0].Copy();
            //            ddlBCCode.DataTextField = "BCNameDetails";
            //            ddlBCCode.DataValueField = "BCCode";
            //            ddlBCCode.DataBind();
            //            ddlBCCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
            //        }
            //    }
            //    else
            //    {
            //        ddlBCCode.DataSource = null;
            //        ddlBCCode.DataBind();
            //        ddlBCCode.Items.Insert(0, new ListItem("All", "0"));
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    ErrorLog.TransactionReportTrace("MATMTransactions: BindBc: Exception: " + Ex.Message);
            //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            //}
            try
            {
                ddlBCCode.Items.Clear();
                ddlBCCode.DataSource = null;
                ddlBCCode.DataBind();
                string UserName = Session["Username"].ToString();
                int IsRemoved = 0;
                int IsActive = 1;
                int IsdocUploaded = 1;
                int VerificationStatus = 1;
                DataTable dsbc = _AggregatorEntity.GetBC(UserName, VerificationStatus, IsActive, IsRemoved, null, IsdocUploaded);
                if (dsbc != null && dsbc.Rows.Count > 0 && dsbc.Rows.Count > 0)
                {
                    if (dsbc.Rows.Count == 1)
                    {
                        ddlBCCode.DataSource = dsbc;
                        ddlBCCode.DataValueField = "bccode";
                        ddlBCCode.DataTextField = "bcname";
                        ddlBCCode.DataBind();
                    }
                    else
                    {
                        ddlBCCode.DataSource = dsbc;
                        ddlBCCode.DataValueField = "bccode";
                        ddlBCCode.DataTextField = "bcname";
                        ddlBCCode.DataBind();
                        ddlBCCode.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlBCCode.DataSource = null;
                    ddlBCCode.DataBind();
                    ddlBCCode.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Agent Registration');", true);
                return;
            }
        }
        #endregion
    }
}