using AppLogger;
using BussinessAccessLayer;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DecryptCardNumber;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ionic.Zip;

namespace SOR.Pages.TransactionReport
{
    public partial class AllTransactions : System.Web.UI.Page
    {
        #region Object Declration
        TransactionReportDAL _TransactionReportDAL = new TransactionReportDAL();
        AggregatorEntity _AggregatorEntity = new AggregatorEntity();
        AgentRegistrationDAL _AgentRegistrationDAL = new AgentRegistrationDAL();
        string UserName = string.Empty;
        string _SummaryType = null, _TransactionStatus = null;
        string _cardNumber = null;
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[4];
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.TransactionReportTrace("AllTransactions | Page_Load | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Session["Username"] != null && Session["UserRoleID"] != null)
                {
                    bool HasPagePermission = UserPermissions.IsPageAccessibleToUser(Session["Username"].ToString(), Session["UserRoleID"].ToString(), "AllTransactions.aspx", "24");
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
                            UserPermissions.RegisterStartupScriptForNavigationListActive("7", "24");
                            txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                            FillBc();
                            BindAction();

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
                ErrorLog.TransactionReportTrace("AllTransactions: Page_Load: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
                return;
            }
        }
        #endregion
        #region Dropdown
        public void FillBc()
        {
            try
            {
                ddlbcCode.Items.Clear();
                ddlbcCode.DataSource = null;
                ddlbcCode.DataBind();
                string UserName = Session["Username"].ToString();
                int IsRemoved = 0;
                int IsActive = 1;
                int IsdocUploaded = 1;
                int VerificationStatus = 1;
                DataTable dsbc = _AgentRegistrationDAL.GetBC(UserName, VerificationStatus, IsActive, IsRemoved, null, IsdocUploaded);
                if (dsbc != null && dsbc.Rows.Count > 0 && dsbc.Rows.Count > 0)
                {
                    if (dsbc.Rows.Count == 1)
                    {
                        ddlbcCode.DataSource = dsbc;
                        ddlbcCode.DataValueField = "BCCode";
                        ddlbcCode.DataTextField = "BCName";
                        ddlbcCode.DataBind();
                    }
                    else
                    {
                        ddlbcCode.DataSource = dsbc;
                        ddlbcCode.DataValueField = "BCCode";
                        ddlbcCode.DataTextField = "BCName";
                        ddlbcCode.DataBind();
                        ddlbcCode.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlbcCode.DataSource = null;
                    ddlbcCode.DataBind();
                    ddlbcCode.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.TransactionReportTrace("Page : AllTransactions.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'All Transactions');", true);
                return;
            }
        }
        protected void ddlbcCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillAggregator();
            }
            catch (Exception ex)
            {
                ErrorLog.TransactionReportTrace("Page : AllTransactions.cs \nFunction : ddlbcCode_SelectedIndexChanged()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'All Transactions');", true);
                return;
            }
        }
        public void FillAggregator()
        {
            try
            {
                ddlAggregator.Items.Clear();
                ddlAggregator.DataSource = null;
                ddlAggregator.DataBind();
                string UserName = Session["Username"].ToString();
                int IsRemoved = 0;
                int IsActive = 1;
                int IsdocUploaded = 1;
                int VerificationStatus = 1;
                string ClientID = ddlbcCode.SelectedValue.ToString();
                DataTable dsbc = _AgentRegistrationDAL.GetAggregator(UserName, VerificationStatus, IsActive, IsRemoved, ClientID, IsdocUploaded);
                if (dsbc != null && dsbc.Rows.Count > 0 && dsbc.Rows.Count > 0)
                {
                    if (dsbc.Rows.Count == 1)
                    {
                        ddlAggregator.DataSource = dsbc;
                        ddlAggregator.DataValueField = "aggcode";
                        ddlAggregator.DataTextField = "agname";
                        ddlAggregator.DataBind();
                    }
                    else
                    {
                        ddlAggregator.DataSource = dsbc;
                        ddlAggregator.DataValueField = "aggcode";
                        ddlAggregator.DataTextField = "agname";
                        ddlAggregator.DataBind();
                        ddlAggregator.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlAggregator.DataSource = null;
                    ddlAggregator.DataBind();
                    ddlAggregator.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.TransactionReportTrace("Page : AllTransactions.cs \nFunction : FillAggregator()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'All Transactions');", true);
                return;
            }
        }
        #endregion
        #region FillGrid
        public DataSet fillGrid(EnumCollection.EnumBindingType _EnumBindingType, string sortExpression = null)
        {
            DataSet _dsAllTransaction = null;
            try
            {
                ErrorLog.TransactionReportTrace("AllTransactions | FillGrid() | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                gvAllTransactions.DataSource = null;
                gvAllTransactions.DataBind();
                SetProperties();
                _TransactionReportDAL.PageIndex = gvAllTransactions.PageIndex;
                _TransactionReportDAL.flag = Convert.ToString((int)_EnumBindingType);

                _dsAllTransaction = _TransactionReportDAL.GetTransactions_All();

                if (_dsAllTransaction != null && _dsAllTransaction.Tables.Count > 0 && _dsAllTransaction.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < _dsAllTransaction.Tables[0].Rows.Count; i++)
                    {
                        if (_dsAllTransaction.Tables[0].Rows[i]["Channel"].ToString() == "MATM")
                        {
                            try
                            {
                                _cardNumber = EncryptDecryptCard.DecryptCardNo(_dsAllTransaction.Tables[0].Rows[i][10].ToString());
                            }
                            catch (Exception Ex)
                            {
                                ErrorLog.CommonTrace("Class : pgAllTransaction.cs \nFunction : fillGrid() => Reading DataSet => Error While Decrypt Card Number : " + _cardNumber + " \nException Occured\n" + Ex.Message);
                            }

                            if (_cardNumber.Length == 19)
                            {
                                _dsAllTransaction.Tables[0].Rows[i][10] = _cardNumber.Substring(0, 6) + "XXXXXXXXX" + _cardNumber.Substring(_cardNumber.Length - 4, 4);
                            }
                            else if (_cardNumber.Length == 16)
                            {
                                _dsAllTransaction.Tables[0].Rows[i][10] = _cardNumber.Substring(0, 6) + "XXXXXX" + _cardNumber.Substring(_cardNumber.Length - 4, 4);
                            }
                        }
                    }
                }
                if (EnumCollection.EnumBindingType.BindGrid.Equals(_EnumBindingType))
                {
                    if (_dsAllTransaction != null && _dsAllTransaction.Tables.Count > 0 && _dsAllTransaction.Tables[0].Rows.Count > 0)
                    {
                        if (sortExpression != null)
                        {
                            gvAllTransactions.VirtualItemCount = Convert.ToInt32(_dsAllTransaction.Tables[4].Rows[0][0]);
                            DataView dv = _dsAllTransaction.Tables[0].AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";
                            dv.Sort = sortExpression + " " + this.SortDirection;

                            gvAllTransactions.DataSource = dv;
                            gvAllTransactions.DataBind();
                            gvAllTransactions.Visible = true;
                            LoadTransactionSummary(_dsAllTransaction);
                        }
                        else
                        {
                            gvAllTransactions.VirtualItemCount = Convert.ToInt32(_dsAllTransaction.Tables[1].Rows[0][0]);
                            gvAllTransactions.DataSource = _dsAllTransaction.Tables[0];
                            gvAllTransactions.DataBind();
                            gvAllTransactions.Visible = true;
                            // lblRecordCount.Text = "Total " + Convert.ToString(_dsAllTransaction.Tables[1].Rows[0][0]) + " Record(s) Found.";
                            //lblRecordCount.Text = "Total " + Convert.ToString(_dsAllTransaction.Tables[0].Rows.Count) + " Record(s) Found.";
                            LoadTransactionSummary(_dsAllTransaction);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideAccordion", "hideAccordion();", true);
                            //ErrorLog.CommonTrans("All Transaction Report Fetched From DB And Grid Binding Successful. Username : " + Session["Username"].ToString() + Environment.NewLine);
                        }
                    }
                    else
                    {
                        gvAllTransactions.Visible = false;
                        LoadTransactionSummary(_dsAllTransaction);
                        //lblRecordCount.Text = "Total 0 Record(s) Found.";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found In Search Criteria. Try again', 'Warning');", true);
                        //return;
                    }
                }
                ErrorLog.TransactionReportTrace("AllTransactions | FillGrid() | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AllTransactions: fillGrid: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return _dsAllTransaction;
        }

        private void SetProperties()
        {
            try
            {
                _TransactionReportDAL.UserName = Session["Username"] != null ? Session["Username"].ToString() : null;
                _TransactionReportDAL.Fromdate = !string.IsNullOrEmpty(txtFromDate.Value) ? txtFromDate.Value.Trim() : hdFromDate.Value;
                _TransactionReportDAL.Todate = !string.IsNullOrEmpty(txtToDate.Value) ? txtToDate.Value.Trim() : hdToDate.Value;
                _TransactionReportDAL.UserName = !string.IsNullOrEmpty(Convert.ToString(Session["Username"])) ? Convert.ToString(Session["Username"]) : null;
                _TransactionReportDAL.AggregatorCode = ddlAggregator.SelectedValue != "0" ? ddlAggregator.SelectedValue : null;
                _TransactionReportDAL.AgentCode = !string.IsNullOrEmpty(txtAgentCode.Value) ? txtAgentCode.Value.Trim() : null;
                _TransactionReportDAL.RRN = !string.IsNullOrEmpty(txtRRNo.Value) ? txtRRNo.Value.Trim() : null;
                _TransactionReportDAL.TransStatus = Convert.ToInt32(ddlTransactionStatus.SelectedValue) != 0 ? Convert.ToInt32(ddlTransactionStatus.SelectedValue) : 0;
                //_TransactionReportDAL.CType = Convert.ToInt32(ddlChannelType.SelectedValue) != 0 ? Convert.ToInt32(ddlChannelType.SelectedValue) : 0;
                _TransactionReportDAL.CType = Convert.ToInt32("1");
                _TransactionReportDAL.TransType = ddlTranType.SelectedValue != "0" ? (ddlTranType.SelectedValue) : null;

            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AllTransactions: SetProperties: Exception: " + Ex.Message);
            }
        }
        #endregion

        #region UnMaskString
        private static string UnMaskString(string objP)
        {
            string convertedObjP = string.Empty;
            try
            {
                string[] resP = objP.Split(';');
                for (int iP = 0; iP <= resP.Length - 1; iP++)
                {
                    if (resP[iP].ToString() != "")
                    {
                        int unicode = int.Parse(resP[iP].ToString());
                        char character = (char)unicode;
                        string text = character.ToString();

                        convertedObjP += text;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AllTransactions: UnMaskString: Exception: " + Ex.Message);
            }
            return convertedObjP;
        }
        #endregion

        #region SplitStringByComma
        private string SplitStringByComma(string _InputString, char[] Delimeter)
        {
            string tempInputString = null,
                    OutputString = null;
            bool IsFirstElement = true;
            try
            {
                if (!String.IsNullOrEmpty(_InputString))
                {
                    if (Delimeter.Count() > 0)
                    {
                        tempInputString = _InputString;
                        if (tempInputString.Contains(Delimeter[0]))
                        {
                            string[] stringSplitArray = tempInputString.Split(Delimeter, StringSplitOptions.RemoveEmptyEntries);
                            StringBuilder _stringBuilder = new StringBuilder();
                            foreach (string stringElement in stringSplitArray)
                            {
                                if (IsFirstElement)
                                    _stringBuilder.Append("'" + stringElement + "'");
                                else
                                    _stringBuilder.Append(",'" + stringElement + "'");
                                IsFirstElement = false;
                            }
                            OutputString = _stringBuilder.ToString();
                        }
                        else
                            OutputString = tempInputString;
                    }
                }
                else
                    OutputString = null;

            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AllTransactions: SplitStringByComma: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
            return OutputString;
        }
        #endregion

        #region LoadTransactionSummary
        private void LoadTransactionSummary(DataSet TansactionSummary)
        {
            try
            {
                if (TansactionSummary != null && TansactionSummary.Tables.Count > 1 && TansactionSummary.Tables[1].Rows.Count > 0)
                {
                    TxnCountTotal.Text = TansactionSummary.Tables[1].Rows[0]["Total"].ToString();
                    TxnCountApproved.Text = TansactionSummary.Tables[1].Rows[0]["Success"].ToString();
                    TxnCountFailed.Text = TansactionSummary.Tables[1].Rows[0]["Failed"].ToString();
                    TxnCountReversal.Text = TansactionSummary.Tables[1].Rows[0]["Reversal"].ToString();
                }
                else
                {
                    TxnCountTotal.Text = "0";
                    TxnCountApproved.Text = "0";
                    TxnCountFailed.Text = "0";
                    TxnCountReversal.Text = "0";
                }
                if (TansactionSummary != null && TansactionSummary.Tables.Count > 2 && TansactionSummary.Tables[2].Rows.Count > 0)
                {
                    TxnAmountTotal.Text = TansactionSummary.Tables[2].Rows[0]["Total"].ToString();
                    TxnAmountApproved.Text = TansactionSummary.Tables[2].Rows[0]["Success"].ToString();
                    TxnAmountFailed.Text = TansactionSummary.Tables[2].Rows[0]["Failed"].ToString();
                    TxnAmountReversal.Text = TansactionSummary.Tables[2].Rows[0]["Reversal"].ToString();
                }
                else
                {
                    TxnAmountTotal.Text = "0";
                    TxnAmountApproved.Text = "0";
                    TxnAmountFailed.Text = "0";
                    TxnAmountReversal.Text = "0";
                }
                if (TansactionSummary != null && TansactionSummary.Tables.Count > 3 && TansactionSummary.Tables[3].Rows.Count > 0)
                {
                    NFinTxnTotal.Text = TansactionSummary.Tables[3].Rows[0]["Total"].ToString();
                    NFinTxnApproved.Text = TansactionSummary.Tables[3].Rows[0]["Success"].ToString();
                    NFinTxnFailed.Text = TansactionSummary.Tables[3].Rows[0]["Failed"].ToString();
                    NFinTxnReversal.Text = TansactionSummary.Tables[3].Rows[0]["Reversal"].ToString();
                }
                else
                {
                    NFinTxnTotal.Text = "0";
                    NFinTxnApproved.Text = "0";
                    NFinTxnFailed.Text = "0";
                    NFinTxnReversal.Text = "0";
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AllTransactions: LoadTransactionSummary: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Button Events
        protected void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions | buttonSearch_Click | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                if (Convert.ToDateTime(txtFromDate.Value) > Convert.ToDateTime(txtToDate.Value))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('From date should be less than To date. Try again', 'Warning');", true);
                    return;
                }
                if (ddlbcCode.SelectedValue != "0")
                {
                    if (ddlAggregator.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please Select Aggregator. Try again', 'Warning');", true);
                        return;
                    }
                }
                else
                {
                    #region Audit
                    _auditParams[0] = Session["Username"].ToString();
                    _auditParams[1] = "Report-All";
                    _auditParams[2] = "btnSearch";
                    _auditParams[3] = Session["LoginKey"].ToString();
                    _LoginEntity.StoreLoginActivities(_auditParams);
                    #endregion
                    gvAllTransactions.PageIndex = 0;
                    fillGrid(EnumCollection.EnumBindingType.BindGrid);
                }
                ErrorLog.TransactionReportTrace("AEPSTransactions | buttonSearch_Click | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AllTransactions: buttonSearch_Click: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void btnReset_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLog.TransactionReportTrace("AEPSTransactions | btnReset_ServerClick | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                #region Audit
                _auditParams[0] = Session["Username"].ToString();
                _auditParams[1] = "Report-All";
                _auditParams[2] = "btnReset";
                _auditParams[3] = Session["LoginKey"].ToString();
                _LoginEntity.StoreLoginActivities(_auditParams);
                #endregion
                txtFromDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                ddlChannelType.ClearSelection();
                ddlbcCode.ClearSelection();
                ddlAggregator.ClearSelection();
                txtAgentCode.Value = string.Empty;
                ddlTranType.ClearSelection();
                ddlTransactionStatus.ClearSelection();
                txtRRNo.Value = string.Empty;
                gvAllTransactions.DataSource = null;
                gvAllTransactions.DataBind();
                gvAllTransactions.PageIndex = 0;

                TxnCountTotal.Text = "0";
                TxnCountApproved.Text = "0";
                TxnCountFailed.Text = "0";
                TxnCountReversal.Text = "0";

                TxnAmountTotal.Text = "0";
                TxnAmountApproved.Text = "0";
                TxnAmountFailed.Text = "0";
                TxnAmountReversal.Text = "0";

                NFinTxnTotal.Text = "0";
                NFinTxnApproved.Text = "0";
                NFinTxnFailed.Text = "0";
                NFinTxnReversal.Text = "0";
                ErrorLog.TransactionReportTrace("AEPSTransactions | btnReset_ServerClick | Ended. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AllTransactions: btnReset_ServerClick: Exception: " + Ex.Message + " | LoginKey : " + Session["LoginKey"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }
        #endregion

        #region Grid Events
        protected void gvAllTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvAllTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAllTransactions.PageIndex = e.NewPageIndex;
                fillGrid(EnumCollection.EnumBindingType.BindGrid);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AllTransactions: gvAllTransactions_PageIndexChanging: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        protected void gvAllTransactions_DataBound(object sender, EventArgs e)
        {

        }

        protected void gvAllTransactions_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                this.fillGrid(EnumCollection.EnumBindingType.BindGrid, e.SortExpression);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AllTransactions: gvAllTransactions_Sorting: Exception: " + Ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
            }
        }

        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        #endregion

        //#region Link Button Events
        //protected void LinkButtonTransactionSummary_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LinkButton _linkButtonClicked = sender as LinkButton;
        //        string _TxnSummaryType = _linkButtonClicked.CommandName.ToString();
        //        string _TxnStatus = _linkButtonClicked.CommandArgument.ToString();

        //        if (!String.IsNullOrEmpty(_TxnSummaryType) && !String.IsNullOrEmpty(_TxnStatus))
        //        {
        //            if (TransactionStatus.Total.ToString() == _TxnStatus)
        //                this._TransactionStatus = null;
        //            else if (TransactionStatus.Success.ToString() == _TxnStatus)
        //                this._TransactionStatus = TransactionStatus.Success.ToString();
        //            else if (TransactionStatus.Failed.ToString() == _TxnStatus)
        //                this._TransactionStatus = TransactionStatus.Failed.ToString();
        //            else if (TransactionStatus.Timeout.ToString() == _TxnStatus)
        //                this._TransactionStatus = TransactionStatus.Timeout.ToString();

        //            if ((int)TransactionType.TotalTxnCount == Convert.ToInt32(_TxnSummaryType))
        //                this._SummaryType = null;
        //            //else if ((int)TransactionType.TotalTxnAmount == Convert.ToInt32(_TxnSummaryType))
        //            //    this._SummaryType = null;

        //            else if ((int)TransactionType.AEPS == Convert.ToInt32(_TxnSummaryType))
        //                this._SummaryType = "AEPS";
        //            else if ((int)TransactionType.BBPS == Convert.ToInt32(_TxnSummaryType))
        //                this._SummaryType = "BBPS";

        //            else if ((int)TransactionType.DMT == Convert.ToInt32(_TxnSummaryType))
        //                this._SummaryType = "DMT";
        //            else if ((int)TransactionType.MATM == Convert.ToInt32(_TxnSummaryType))
        //                this._SummaryType = "MATM";
        //        }
        //        SetProperties();
        //        _TransactionReportDAL.Status = this._TransactionStatus == "Approved" ? "Success" : this._TransactionStatus;
        //        _TransactionReportDAL.CType = Convert.ToInt32(this._SummaryType);
        //        DataSet _dsAllTransaction = _TransactionReportDAL.GetTransactions_All();

        //        if (_dsAllTransaction != null && _dsAllTransaction.Tables.Count > 0 && _dsAllTransaction.Tables[0].Rows.Count > 0)
        //        {
        //            gvAllTransactions.DataSource = _dsAllTransaction.Tables[0];
        //            gvAllTransactions.DataBind();
        //        }
        //        else
        //        {
        //            gvAllTransactions.DataSource = null;
        //            gvAllTransactions.DataBind();
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrorLog.TransactionReportTrace("AllTransactions: LinkButtonTransactionSummary_Click: Exception: " + Ex.Message);
        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Something went wrong. Try again', 'Warning');", true);
        //    }
        //}
        //#endregion


        #region Export

        private string SetPageFiltersExport()
        {
            string pageFilters = string.Empty;
            try
            {
                pageFilters = "Generated By " + Convert.ToString(Session["Username"]);
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportTrace("AllTransactions: SetPageFiltersExport: Exception: " + Ex.Message);
            }
            return pageFilters;
        }
        #endregion

        //#region DropdownBindBc
        //private void BindBc()
        //{
        //    try
        //    {
        //        ddlBCCode.Items.Clear();
        //        ddlBCCode.DataSource = null;
        //        ddlBCCode.DataBind();
        //        string UserName = Session["Username"].ToString();
        //        int IsRemoved = 0;
        //        int IsActive = 1;
        //        int IsdocUploaded = 1;
        //        int VerificationStatus = 1;
        //        DataTable dsbc = _AggregatorEntity.GetBC(UserName, VerificationStatus, IsActive, IsRemoved, null, IsdocUploaded);
        //        if (dsbc != null && dsbc.Rows.Count > 0 && dsbc.Rows.Count > 0)
        //        {
        //            if (dsbc.Rows.Count == 1)
        //            {
        //                ddlBCCode.DataSource = dsbc;
        //                ddlBCCode.DataValueField = "bccode";
        //                ddlBCCode.DataTextField = "bcname";
        //                ddlBCCode.DataBind();
        //            }
        //            else
        //            {
        //                ddlBCCode.DataSource = dsbc;
        //                ddlBCCode.DataValueField = "bccode";
        //                ddlBCCode.DataTextField = "bcname";
        //                ddlBCCode.DataBind();
        //                ddlBCCode.Items.Insert(0, new ListItem("-- Select --", "0"));
        //            }
        //        }
        //        else
        //        {
        //            ddlBCCode.DataSource = null;
        //            ddlBCCode.DataBind();
        //            ddlBCCode.Items.Insert(0, new ListItem("No Data Found", "0"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.AgentManagementTrace("Page : AgentRegistration.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);

        //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'Agent Registration');", true);
        //        return;
        //    }
        //}
        //#endregion

        #region Export
        public void BindAction()
        {
            try
            {
                ddlAction.Items.Clear();
                ddlAction.DataSource = null;
                ddlAction.DataBind();

                DataTable dt = _TransactionReportDAL.GetAction();
                if (dt != null && dt.Rows.Count > 0 && dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1)
                    {
                        ddlAction.DataSource = dt;
                        ddlAction.DataValueField = "id";
                        ddlAction.DataTextField = "name";
                        ddlAction.DataBind();
                        ddlAction.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                    else
                    {
                        ddlAction.DataSource = dt;
                        ddlAction.DataValueField = "id";
                        ddlAction.DataTextField = "name";
                        ddlAction.DataBind();
                        ddlAction.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }
                }
                else
                {
                    ddlAction.DataSource = null;
                    ddlAction.DataBind();
                    ddlAction.Items.Insert(0, new ListItem("No Data Found", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AllTransactions.cs \nFunction : FillBc()\nException Occured\n" + ex.Message);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }
        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlAction.SelectedValue == "1")
                {
                    btnSearch.Visible = true;
                    btnClear.Visible = true;
                    btnexport.Visible = false;
                }
                else
                {
                    btnSearch.Visible = false;
                    btnClear.Visible = false;
                    btnexport.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AllTransactions.cs \nFunction : ddlAction_SelectedIndexChanged()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }

        protected void btnexport_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlAction.SelectedValue == "2")
                {
                    try
                    {
                        ErrorLog.TransactionReportTrace("AEPSTransactions | btnexport_ServerClick | Export-To-Excel | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        ExportFormat _ExportFormat = new ExportFormat();
                        string pageFilters = SetPageFiltersExport();
                        //SetProperties();
                        //_TransactionReportDAL.Flag = (int)EnumCollection.EnumBindingType.Export;
                        //DataSet dt = _TransactionReportDAL.GetTransactions_All();
                        DataSet dt = fillGrid(EnumCollection.EnumBindingType.Export);
                        DataSet dsExport = dt.Copy();

                        if (dt != null && dt.Tables[0].Rows.Count > 0)
                        {
                            #region Audit
                            _auditParams[0] = Session["Username"].ToString();
                            _auditParams[1] = "Report-All";
                            _auditParams[2] = "Export-To-Excel";
                            _auditParams[3] = Session["LoginKey"].ToString();
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            #endregion
                            _ExportFormat.ExporttoExcel(Convert.ToString(Session["Username"]), "Proxima", "All Transactions", dt);
                        }
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("AllTransactions: btnexport_ServerClick | Excel : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                if (ddlAction.SelectedValue == "3")
                {
                    try
                    {
                        ErrorLog.TransactionReportTrace("AEPSTransactions | btnexport_ServerClick | Export-To-CSV | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        //SetProperties();
                        //_TransactionReportDAL.Flag = 1;
                        //DataSet dt = _TransactionReportDAL.GetTransactions_All();
                        DataSet dt = fillGrid(EnumCollection.EnumBindingType.Export);
                        ExportFormat _ExportFormat = new ExportFormat();
                        string pageFilters = SetPageFiltersExport();

                        if (dt != null && dt.Tables[0].Rows.Count > 0)
                        {
                            #region Audit
                            _auditParams[0] = Session["Username"].ToString();
                            _auditParams[1] = "Report-All";
                            _auditParams[2] = "Export-To-CSV";
                            _auditParams[3] = Session["LoginKey"].ToString();
                            _LoginEntity.StoreLoginActivities(_auditParams);
                            #endregion
                            _ExportFormat.ExportInCSV(Convert.ToString(Session["Username"]), "Proxima", "All Transactions", dt);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No data found.', 'Alert');", true);
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("AllTransactions: btnexport_ServerClick | CSV : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                if (ddlAction.SelectedValue == "4")
                {
                    try
                    {
                        ErrorLog.TransactionReportTrace("AEPSTransactions | btnexport_ServerClick | Export-To-ZIP | Started. | UserName : " + Session["Username"].ToString() + " | LoginKey : " + Session["LoginKey"].ToString());
                        #region Audit
                        _auditParams[0] = Session["Username"].ToString();
                        _auditParams[1] = "Report-All";
                        _auditParams[2] = "Export-To-ZIP";
                        _auditParams[3] = Session["LoginKey"].ToString();
                        _LoginEntity.StoreLoginActivities(_auditParams);
                        #endregion
                        int ReportThreads = Convert.ToInt32(ConfigurationManager.AppSettings["ReportThreads"].ToString());
                        int ReportRecordsPerSheet = Convert.ToInt32(ConfigurationManager.AppSettings["ReportRecordsPerSheet"].ToString());
                        string ReportDownPath = ConfigurationManager.AppSettings["ReportDownPath"].ToString();
                        string StartTime = DateTime.Now.ToLongTimeString();
                        ErrorLog.TransactionReportTrace("Page : All Transaction Report - Request Received For Export Report. Start-Time : " + StartTime);
                        SetProperties();
                        List<DateRangeList> dateRangeList = DateRangeHandlerDynamic(_TransactionReportDAL.Fromdate, _TransactionReportDAL.Todate, ReportThreads);  //string , string , string , string 
                        DataTable dtTotalTxns = RunParallel(dateRangeList, _TransactionReportDAL.UserName, _TransactionReportDAL.AggregatorCode, _TransactionReportDAL.AgentCode, _TransactionReportDAL.RRN, _TransactionReportDAL.TransStatus, _TransactionReportDAL.CType, _TransactionReportDAL.TransType);
                        DataSet dsTxnTables = SplitDataTable(dtTotalTxns, ReportRecordsPerSheet);
                        if (dsTxnTables != null && dsTxnTables.Tables.Count > 0)
                        {
                            DeleteFiles(ReportDownPath);
                            string DownloadPath = GenerateCSVFiles(dsTxnTables, "Txn_Report_");
                            using (ZipFile zip = new ZipFile())
                            {
                                String[] files = Directory.GetFiles(DownloadPath);
                                foreach (string file in files)
                                {
                                    zip.AddFile(file, "Files");
                                }
                                Response.Clear();
                                Response.BufferOutput = false;
                                string zipName = String.Format("All_Transactions_{0}.zip", DateTime.Now.ToString("yyyyMMMdd_HHmmss"));
                                Response.ContentType = "application/zip";
                                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                                zip.Save(Response.OutputStream);
                                Response.End();
                            }
                            ErrorLog.TransactionReportTrace("Page : TxnExport - Request Received For Export Report. Start-Time : " + StartTime + " End-Time : " + DateTime.Now.ToLongTimeString() + Environment.NewLine);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('No Data Found', 'Warning');", true);
                            return;
                        }
                    }
                    catch (Exception Ex)
                    {
                        ErrorLog.TransactionReportTrace("AllTransactions: btnexport_ServerClick | ZIP : Exception: " + Ex.Message);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                        return;
                    }
                }
                else
                {
                    ErrorLog.AgentManagementTrace("Page : AllTransactions.cs \nFunction : ddlAction_SelectedIndexChanged() | Please select action type for export.");
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Please select action type for export', 'AEPS Transactions');", true);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AgentManagementTrace("Page : AllTransactions.cs \nFunction : ddlAction_SelectedIndexChanged()\nException Occured\n" + ex.Message);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning", "showWarning('Contact System Administrator', 'AEPS Transactions');", true);
                return;
            }
        }

        #region ZIP Export Methods
        public DataSet SplitDataTable(DataTable tableData, int max)
        {
            DataSet newDs = new DataSet();
            string StartTime = DateTime.Now.ToLongTimeString();
            try
            {
                if (tableData != null && tableData.Rows.Count > 0)
                {
                    int i = 0;
                    int j = 1;
                    int countOfRows = tableData.Rows.Count;

                    DataTable newDt = tableData.Clone();
                    newDt.TableName = "Txn_Sheet_" + j;
                    newDt.Clear();
                    foreach (DataRow row in tableData.Rows)
                    {
                        DataRow newRow = newDt.NewRow();
                        newRow.ItemArray = row.ItemArray;

                        newDt.Rows.Add(newRow);
                        i++;

                        countOfRows--;

                        if (i == max)
                        {
                            newDs.Tables.Add(newDt);
                            j++;
                            newDt = tableData.Clone();
                            newDt.TableName = tableData.TableName + "_" + j;
                            newDt.Clear();
                            i = 0;
                        }

                        if (countOfRows == 0 && i < max)
                        {
                            newDs.Tables.Add(newDt);
                            j++;
                            newDt = tableData.Clone();
                            newDt.TableName = tableData.TableName + "_" + j;
                            newDt.Clear();
                            i = 0;
                        }
                    }
                }
                ErrorLog.TransactionReportTrace("Page : AllTxnExport - Request Received For Split Data Table. Per Sheet Record Range : " + max + " Total Records : " + tableData.Rows.Count + " Start-Time : " + StartTime + " End-Time : " + DateTime.Now.ToLongTimeString());
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportError(Ex);
            }
            return newDs;
        }

        private List<DateRangeList> DateRangeHandlerDynamic(string FromDate, string ToDate, int TaskCount)
        {
            ErrorLog.TransactionReportTrace("Page : AllTxnExport - Request Received For Date Range Handler. FromDate : " + FromDate + " ToDate : " + ToDate);
            List<DateRangeList> _DateRangeList = null;
            if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
            {
                DateTime _FromDate = Convert.ToDateTime(FromDate);
                DateTime _ToDate = Convert.ToDateTime(ToDate);
                var NoOfDays = (_ToDate.AddDays(1) - _FromDate).TotalDays;
                _DateRangeList = new List<DateRangeList>();
                if (NoOfDays <= 3)
                {
                    DateRangeList _DateRange = new DateRangeList();
                    _DateRange.StartDate = Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd");
                    _DateRange.EndDate = Convert.ToDateTime(_ToDate).ToString("yyyy-MM-dd");
                    _DateRangeList.Add(_DateRange);
                }
                else
                {
                    int MainThreads = Convert.ToInt32(NoOfDays) / TaskCount;
                    int RemDays = Convert.ToInt32(NoOfDays) % TaskCount;
                    if (MainThreads == 0)
                    {
                        DateRangeList _DateRange = new DateRangeList();
                        _DateRange.StartDate = Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd");
                        _DateRange.EndDate = Convert.ToDateTime(_ToDate).ToString("yyyy-MM-dd");
                        _DateRangeList.Add(_DateRange);
                    }
                    else if (MainThreads > 0 && RemDays == 0)
                    {
                        DateTime tempDate = _FromDate;
                        for (int ImainThreads = 1; ImainThreads <= MainThreads; ImainThreads++)
                        {
                            DateRangeList _DateRange = new DateRangeList();
                            _DateRange.StartDate = ImainThreads == 1 ? Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd") : tempDate.AddDays(1).ToString("yyyy-MM-dd");
                            _DateRange.EndDate = Convert.ToDateTime(_DateRange.StartDate).AddDays(TaskCount - 1).ToString("yyyy-MM-dd");
                            _DateRangeList.Add(_DateRange);
                            tempDate = Convert.ToDateTime(_DateRange.StartDate).AddDays(TaskCount - 1);
                        }
                    }
                    else if (MainThreads > 0 && RemDays > 0)
                    {
                        DateTime tempDate = _FromDate;
                        for (int ImainThreads = 1; ImainThreads <= MainThreads; ImainThreads++)
                        {
                            DateRangeList _DateRange = new DateRangeList();
                            _DateRange.StartDate = ImainThreads == 1 ? Convert.ToDateTime(_FromDate).ToString("yyyy-MM-dd") : tempDate.AddDays(1).ToString("yyyy-MM-dd");
                            _DateRange.EndDate = Convert.ToDateTime(_DateRange.StartDate).AddDays(TaskCount - 1).ToString("yyyy-MM-dd");
                            _DateRangeList.Add(_DateRange);
                            tempDate = Convert.ToDateTime(_DateRange.EndDate);
                        }
                        DateRangeList _DateRangeRemDays = new DateRangeList();
                        _DateRangeRemDays.StartDate = tempDate.AddDays(1).ToString("yyyy-MM-dd");
                        _DateRangeRemDays.EndDate = Convert.ToDateTime(_ToDate).ToString("yyyy-MM-dd");
                        _DateRangeList.Add(_DateRangeRemDays);
                    }
                }
            }
            return _DateRangeList;
        }

        private void DeleteFiles(string FileDirectory)
        {
            try
            {
                string[] files = Directory.GetFiles(FileDirectory);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (IOException Ex)
            {
                ErrorLog.TransactionReportError(Ex);
            }
        }

        private string GenerateCSVFiles(DataSet _DataSet, string FileName)
        {
            string FilesPath = ConfigurationManager.AppSettings["ReportDownPath"].ToString();
            string StartTime = DateTime.Now.ToLongTimeString();
            try
            {
                int fileRange = 1;
                foreach (DataTable _DataTable in _DataSet.Tables)
                {
                    StreamWriter CsvfileWriter = new StreamWriter(FilesPath + "\\" + FileName + fileRange.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".csv");
                    using (CsvfileWriter)
                    {
                        for (int k = 0; k < _DataTable.Columns.Count; k++)
                        {
                            CsvfileWriter.Write(_DataTable.Columns[k].ToString() + ',');
                        }
                        CsvfileWriter.WriteLine();
                        foreach (DataRow row in _DataTable.Rows)
                        {
                            CsvfileWriter.WriteLine(string.Join(",", row.ItemArray));
                        }
                    }
                    fileRange += 1;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportError(Ex);
            }
            return FilesPath;
        }

        private string GenerateCSVFiles1(DataSet _DataSet, string FileName)
        {
            string FilesPath = ConfigurationManager.AppSettings["ReportDownPath"].ToString();
            string StartTime = DateTime.Now.ToLongTimeString();
            try
            {
                int fileRange = 1;
                foreach (DataTable _DataTable in _DataSet.Tables)
                {
                    StreamWriter CsvfileWriter = new StreamWriter(FilesPath + "\\" + FileName + fileRange.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmmss") + ".csv");
                    using (CsvfileWriter)
                    {
                        for (int k = 1; k < _DataTable.Columns.Count; k++)
                        {
                            CsvfileWriter.Write(_DataTable.Columns[k].ToString() + ',');
                        }
                        CsvfileWriter.WriteLine();
                        foreach (DataRow row in _DataTable.Rows)
                        {
                            CsvfileWriter.WriteLine(string.Join(",", row.ItemArray));
                        }
                    }
                    fileRange += 1;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog.TransactionReportError(Ex);
            }
            return FilesPath;
        }

        public static DataTable FetchTxnsDB(string startDate, string EndDate, string UserName, string Aggregator, string AgentCode, string RRN, int TransactionStatus, int CType, string TransType)
        {
            DataTable _dtTxns = new DataTable();
            try
            {
                TransactionReportDAL _TransactionReportDAL = new TransactionReportDAL();
                _TransactionReportDAL.UserName = UserName;
                _TransactionReportDAL.Fromdate = startDate;
                _TransactionReportDAL.Todate = EndDate;
                _TransactionReportDAL.AggregatorCode = Aggregator;
                _TransactionReportDAL.AgentCode = AgentCode;
                _TransactionReportDAL.RRN = RRN;
                _TransactionReportDAL.TransStatus = TransactionStatus;
                _TransactionReportDAL.CType = CType;
                _TransactionReportDAL.TransType = TransType;
                //_TransactionReportDAL.PageIndex = null;
                _TransactionReportDAL.flag = Convert.ToString((int)EnumCollection.EnumBindingType.Export);
                DataSet dsTransactionLogs = _TransactionReportDAL.GetTransactions_All();
                _dtTxns = dsTransactionLogs != null && dsTransactionLogs.Tables.Count > 0 ? dsTransactionLogs.Tables[0] : null;
            }
            catch (Exception)
            {
                throw;
            }
            return _dtTxns;
        }

        private static object _lockObj = new object();
        public static DataTable RunParallel(List<DateRangeList> _DateRangeList, string UserName, string Aggregator, string AgentCode, string RRN, int TransactionStatus, int CType, string TransType)
        {
            DataTable _dataTable = new DataTable();
            Parallel.ForEach(_DateRangeList, (dateRange) =>
            {
                DataTable dtResultSET = FetchTxnsDB(dateRange.StartDate, dateRange.EndDate, UserName, Aggregator, AgentCode, RRN, TransactionStatus, CType, TransType);
                lock (_lockObj)
                {
                    if (dtResultSET != null && dtResultSET.Rows.Count > 0)
                    {
                        _dataTable.Merge(dtResultSET.Copy());
                        _dataTable.AcceptChanges();
                    }
                }
            });
            return _dataTable;
        }
        #endregion

        #endregion
    }
}